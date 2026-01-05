-- Stored Procedure: APM_GetActionPlanAggregates_ByPlan
-- Returns aggregated counts per IdActionPlan: total open items, overdue counts, per-theme open counts and per-status counts
-- NOTE: Review and deploy to the database by DB admin. Uses temporary tables similar to your provided SP.

DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `APM_GetActionPlanAggregates_ByPlan`(
    IN _SelectedPeriod VARCHAR(200),
    IN _Iduser INT
)
BEGIN
    SET SESSION group_concat_max_len = 1000000;

    DECLARE _UserEmployeeId INT;
    SELECT IdEmployee INTO _UserEmployeeId FROM employees WHERE IdUser = _Iduser;

    -- Temporary: flat tasks + subtasks with necessary fields
    DROP TEMPORARY TABLE IF EXISTS tmp_all_items;
    CREATE TEMPORARY TABLE tmp_all_items (
        IdActionPlan INT,
        IsSubTask TINYINT(1),
        IdTask INT,
        DueDate DATE,
        IdStatus INT,
        StatusText VARCHAR(200),
        IdPriority INT,
        PriorityText VARCHAR(50),
        ThemeText VARCHAR(200),
        IdEmployee INT
    ) ENGINE=MEMORY;

    INSERT INTO tmp_all_items
    SELECT ap.IdActionPlan, 0 AS IsSubTask, t.IdTask, t.DueDate, t.IdStatus, lvStatus.Value, t.IdPriority, lvP.Value, lvT.Value, t.IdResponsibleEmployee
    FROM emdep_geos.action_plan_task t
    INNER JOIN emdep_geos.action_plans ap ON ap.IdActionPlan = t.IdActionPlan
    LEFT JOIN emdep_geos.lookup_values lvStatus ON lvStatus.IdLookupValue = t.IdStatus
    LEFT JOIN emdep_geos.lookup_values lvP ON lvP.IdLookupValue = t.IdPriority
    LEFT JOIN emdep_geos.lookup_values lvT ON lvT.IdLookupValue = t.IdTheme
    WHERE t.InUse = 1 AND t.IdParent IS NULL AND FIND_IN_SET(YEAR(ap.CreatedIn), _SelectedPeriod) > 0
    UNION ALL
    SELECT ap.IdActionPlan, 1 AS IsSubTask, st.IdTask, st.DueDate, st.IdStatus, lvStatus.Value, st.IdPriority, lvP.Value, lvT.Value, st.IdEmployee
    FROM emdep_geos.action_plan_task st
    INNER JOIN emdep_geos.action_plan_task parent ON parent.IdTask = st.IdParent
    INNER JOIN emdep_geos.action_plans ap ON ap.IdActionPlan = parent.IdActionPlan
    LEFT JOIN emdep_geos.lookup_values lvStatus ON lvStatus.IdLookupValue = st.IdStatus
    LEFT JOIN emdep_geos.lookup_values lvP ON lvP.IdLookupValue = st.IdPriority
    LEFT JOIN emdep_geos.lookup_values lvT ON lvT.IdLookupValue = st.IdTheme
    WHERE st.InUse = 1 AND st.IdParent IS NOT NULL AND FIND_IN_SET(YEAR(ap.CreatedIn), _SelectedPeriod) > 0;

    -- Base aggregates per plan
    DROP TEMPORARY TABLE IF EXISTS ActionPlanAggregates;
    CREATE TEMPORARY TABLE ActionPlanAggregates (
        IdActionPlan INT PRIMARY KEY,
        TotalOpenItems INT,
        Overdue15 INT,
        HighPriorityOverdue INT,
        MaxDueDays INT
    ) ENGINE=MEMORY;

    INSERT INTO ActionPlanAggregates (IdActionPlan, TotalOpenItems, Overdue15, HighPriorityOverdue, MaxDueDays)
    SELECT
        IdActionPlan,
        SUM(CASE WHEN (DueDate < CURDATE() AND IdStatus NOT IN (10002,10003)) THEN 1 ELSE 0 END) AS TotalOpenItems,
        SUM(CASE WHEN (DueDate < DATE_SUB(CURDATE(), INTERVAL 15 DAY) AND IdStatus NOT IN (10002,10003)) THEN 1 ELSE 0 END) AS Overdue15,
        SUM(CASE WHEN (DueDate < CURDATE() AND (PriorityText = 'High' OR PriorityText = 'Critical') AND IdStatus NOT IN (10002,10003)) THEN 1 ELSE 0 END) AS HighPriorityOverdue,
        COALESCE(MAX(CASE WHEN (DueDate < CURDATE() AND IdStatus NOT IN (10002,10003)) THEN DATEDIFF(CURDATE(), DueDate) ELSE 0 END), 0) AS MaxDueDays
    FROM tmp_all_items
    GROUP BY IdActionPlan;

    -- Per-plan per-theme open counts
    DROP TEMPORARY TABLE IF EXISTS ActionPlanThemeCounts;
    CREATE TEMPORARY TABLE ActionPlanThemeCounts (
        IdActionPlan INT,
        Theme VARCHAR(200),
        OpenCount INT,
        PRIMARY KEY (IdActionPlan, Theme)
    ) ENGINE=MEMORY;

    INSERT INTO ActionPlanThemeCounts (IdActionPlan, Theme, OpenCount)
    SELECT IdActionPlan, ThemeText AS Theme, COUNT(*) AS OpenCount
    FROM tmp_all_items
    WHERE ThemeText IS NOT NULL AND ThemeText <> '' AND IdStatus NOT IN (10002,10003) AND DueDate < CURDATE()
    GROUP BY IdActionPlan, ThemeText;

    -- Per-plan per-status counts (To do, In progress, Blocked, Done)
    DROP TEMPORARY TABLE IF EXISTS ActionPlanStatusCounts;
    CREATE TEMPORARY TABLE ActionPlanStatusCounts (
        IdActionPlan INT,
        StatusLabel VARCHAR(64),
        CountItems INT,
        PRIMARY KEY (IdActionPlan, StatusLabel)
    ) ENGINE=MEMORY;

    INSERT INTO ActionPlanStatusCounts (IdActionPlan, StatusLabel, CountItems)
    SELECT IdActionPlan,
        CASE
            WHEN StatusText IS NULL THEN 'To do'
            WHEN LOWER(StatusText) LIKE '%done%' OR LOWER(StatusText) LIKE '%closed%' THEN 'Closed'
            WHEN LOWER(StatusText) LIKE '%blocked%' OR LOWER(StatusText) LIKE '%hold%' THEN 'Blocked'
            WHEN LOWER(StatusText) LIKE '%in progress%' OR LOWER(StatusText) LIKE '%working%' THEN 'In progress'
            WHEN LOWER(StatusText) LIKE '%to do%' OR LOWER(StatusText) LIKE '%open%' THEN 'To do'
            ELSE 'To do'
        END AS StatusLabel,
        COUNT(*)
    FROM tmp_all_items
    GROUP BY IdActionPlan, StatusLabel;

    -- Final result set: join plan details with aggregates
    SELECT ap.IdActionPlan, ap.Code, ap.Description, ag.TotalOpenItems, ag.Overdue15, ag.HighPriorityOverdue, ag.MaxDueDays,
           GROUP_CONCAT(DISTINCT CONCAT('T|', atc.Theme, ':', atc.OpenCount) SEPARATOR ';') AS ThemeAggregates,
           GROUP_CONCAT(DISTINCT CONCAT('S|', asc.StatusLabel, ':', asc.CountItems) SEPARATOR ';') AS StatusAggregates
    FROM emdep_geos.action_plans ap
    LEFT JOIN ActionPlanAggregates ag ON ag.IdActionPlan = ap.IdActionPlan
    LEFT JOIN ActionPlanThemeCounts atc ON atc.IdActionPlan = ap.IdActionPlan
    LEFT JOIN ActionPlanStatusCounts asc ON asc.IdActionPlan = ap.IdActionPlan
    WHERE FIND_IN_SET(YEAR(ap.CreatedIn), _SelectedPeriod) > 0 AND ap.InUse = 1
    GROUP BY ap.IdActionPlan;

END$$
DELIMITER ;

-- Notes:
-- - `ThemeAggregates` returns values like "T|Safety:3;T|Maintenance:2" meaning theme Safety->3 open items, Maintenance->2.
-- - `StatusAggregates` returns values like "S|To do:5;S|In progress:2;S|Closed:8".
-- - Client should parse the concatenated strings (or adjust the procedure to return normalized rows).
