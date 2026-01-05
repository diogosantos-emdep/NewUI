DELIMITER $$

CREATE DEFINER=`root`@`%` PROCEDURE `APM_GetActionPlanDetails_WithCounts`(
    IN _SelectedPeriod VARCHAR(200),
    IN _Iduser INT
)
BEGIN

    DECLARE _CompanyList1        TEXT;
    DECLARE _DepartmentList1     TEXT;
    DECLARE _EmployeeList1       TEXT;
    DECLARE _IdActionPlanList1   TEXT;
    DECLARE _IdActionPlanList2   TEXT;
    DECLARE __CompanyList2       TEXT;
    DECLARE _DepartmentList2     TEXT;
    DECLARE _EmployeeList2       TEXT;
    DECLARE _UserEmployeeId      INT;
    DECLARE _IdActionPlanTaskList1   TEXT;
    DECLARE _IdActionPlanTaskList2   TEXT;
    
    SET SESSION group_concat_max_len = 1000000; 
    
    SELECT IdEmployee INTO _UserEmployeeId FROM employees WHERE IdUser = _Iduser;
  
    -- =================================================================================
    -- 1. STATISTICS PRE-CALCULATION (ENHANCED with Theme/Status Aggregates)
    -- =================================================================================
    DROP TEMPORARY TABLE IF EXISTS ActionPlanCounts;
    
    CREATE TEMPORARY TABLE ActionPlanCounts (PRIMARY KEY (IdActionPlan))
    AS
    SELECT 
        Main.IdActionPlan,
        SUM(CASE WHEN Main.Type = 'Task' THEN 1 ELSE 0 END) AS TotalActionItems,
        SUM(CASE WHEN Main.Type = 'Task' AND Main.CloseDate IS NULL THEN 1 ELSE 0 END) AS TotalOpenItems,
        SUM(CASE WHEN Main.Type = 'Task' AND Main.CloseDate IS NOT NULL THEN 1 ELSE 0 END) AS TotalClosedItems,
        SUM(CASE WHEN Main.CloseDate IS NULL AND Main.DueDate < DATE_ADD(NOW(), INTERVAL -15 DAY) AND Main.IdStatus NOT IN (10002, 10003) THEN 1 ELSE 0 END) AS Stat_Overdue15,
        SUM(CASE WHEN Main.CloseDate IS NULL AND (Main.PriorityValue = 'High' OR Main.PriorityValue = 'Critical') AND Main.DueDate < NOW() AND Main.IdStatus NOT IN (10002, 10003) THEN 1 ELSE 0 END) AS Stat_HighPriorityOverdue,
        COALESCE(MAX(CASE WHEN Main.CloseDate IS NULL AND Main.DueDate < NOW() THEN DATEDIFF(NOW(), Main.DueDate) ELSE 0 END), 0) AS Stat_MaxDueDays,
        GROUP_CONCAT(DISTINCT Main.Theme SEPARATOR ',') AS Stat_ThemesList
    FROM 
    (
        SELECT T.IdActionPlan, 'Task' AS Type, T.CloseDate, T.DueDate, T.IdStatus, lvP.Value AS PriorityValue, lvT.Value AS Theme
        FROM emdep_geos.action_plan_task T
        LEFT JOIN emdep_geos.lookup_values lvP ON lvP.IdLookupValue = T.IdPriority
        LEFT JOIN emdep_geos.lookup_values lvT ON lvT.IdLookupValue = T.IdTheme
        WHERE T.InUse = 1 AND T.IdParent IS NULL
        UNION ALL
        SELECT Parent.IdActionPlan, 'SubTask' AS Type, ST.CloseDate, ST.DueDate, ST.IdStatus, lvP.Value AS PriorityValue, lvT.Value AS Theme
        FROM emdep_geos.action_plan_task ST
        INNER JOIN emdep_geos.action_plan_task Parent ON ST.IdParent = Parent.IdTask
        LEFT JOIN emdep_geos.lookup_values lvP ON lvP.IdLookupValue = ST.IdPriority
        LEFT JOIN emdep_geos.lookup_values lvT ON lvT.IdLookupValue = ST.IdTheme
        WHERE ST.InUse = 1 AND ST.IdParent IS NOT NULL
    ) AS Main
    GROUP BY Main.IdActionPlan;

    -- =================================================================================
    -- 1.1 PER-THEME OPEN ITEM COUNTS (NEW - for Side Filter precision)
    -- =================================================================================
    DROP TEMPORARY TABLE IF EXISTS ActionPlanThemeCounts;
    CREATE TEMPORARY TABLE ActionPlanThemeCounts (
        IdActionPlan INT,
        Theme VARCHAR(200),
        OpenCount INT,
        PRIMARY KEY (IdActionPlan, Theme)
    ) ENGINE=MEMORY;

    INSERT INTO ActionPlanThemeCounts (IdActionPlan, Theme, OpenCount)
    SELECT Main.IdActionPlan, Main.Theme, COUNT(*) AS OpenCount
    FROM 
    (
        SELECT T.IdActionPlan, lvT.Value AS Theme, T.IdStatus, T.CloseDate
        FROM emdep_geos.action_plan_task T
        LEFT JOIN emdep_geos.lookup_values lvT ON lvT.IdLookupValue = T.IdTheme
        WHERE T.InUse = 1 AND T.IdParent IS NULL
        UNION ALL
        SELECT Parent.IdActionPlan, lvT.Value AS Theme, ST.IdStatus, ST.CloseDate
        FROM emdep_geos.action_plan_task ST
        INNER JOIN emdep_geos.action_plan_task Parent ON ST.IdParent = Parent.IdTask
        LEFT JOIN emdep_geos.lookup_values lvT ON lvT.IdLookupValue = ST.IdTheme
        WHERE ST.InUse = 1 AND ST.IdParent IS NOT NULL
    ) AS Main
    WHERE Main.Theme IS NOT NULL 
      AND Main.Theme <> '' 
      AND Main.CloseDate IS NULL
      AND Main.IdStatus NOT IN (10002, 10003)
    GROUP BY Main.IdActionPlan, Main.Theme;

    -- =================================================================================
    -- 1.2 PER-STATUS COUNTS (NEW - for Alert Button status filters)
    -- =================================================================================
    DROP TEMPORARY TABLE IF EXISTS ActionPlanStatusCounts;
    CREATE TEMPORARY TABLE ActionPlanStatusCounts (
        IdActionPlan INT,
        StatusLabel VARCHAR(64),
        CountItems INT,
        PRIMARY KEY (IdActionPlan, StatusLabel)
    ) ENGINE=MEMORY;

    INSERT INTO ActionPlanStatusCounts (IdActionPlan, StatusLabel, CountItems)
    SELECT Main.IdActionPlan,
        CASE
            WHEN Main.StatusValue IS NULL THEN 'To do'
            WHEN LOWER(Main.StatusValue) LIKE '%done%' OR LOWER(Main.StatusValue) LIKE '%closed%' OR LOWER(Main.StatusValue) LIKE '%completed%' THEN 'Closed'
            WHEN LOWER(Main.StatusValue) LIKE '%blocked%' OR LOWER(Main.StatusValue) LIKE '%hold%' THEN 'Blocked'
            WHEN LOWER(Main.StatusValue) LIKE '%in progress%' OR LOWER(Main.StatusValue) LIKE '%working%' THEN 'In progress'
            WHEN LOWER(Main.StatusValue) LIKE '%to do%' OR LOWER(Main.StatusValue) LIKE '%open%' OR LOWER(Main.StatusValue) LIKE '%pending%' THEN 'To do'
            ELSE 'To do'
        END AS StatusLabel,
        COUNT(*)
    FROM 
    (
        SELECT T.IdActionPlan, lvS.Value AS StatusValue
        FROM emdep_geos.action_plan_task T
        LEFT JOIN emdep_geos.lookup_values lvS ON lvS.IdLookupValue = T.IdStatus
        WHERE T.InUse = 1 AND T.IdParent IS NULL
        UNION ALL
        SELECT Parent.IdActionPlan, lvS.Value AS StatusValue
        FROM emdep_geos.action_plan_task ST
        INNER JOIN emdep_geos.action_plan_task Parent ON ST.IdParent = Parent.IdTask
        LEFT JOIN emdep_geos.lookup_values lvS ON lvS.IdLookupValue = ST.IdStatus
        WHERE ST.InUse = 1 AND ST.IdParent IS NOT NULL
    ) AS Main
    GROUP BY Main.IdActionPlan, StatusLabel;

    -- =================================================================================
    -- 2. ACTION PLANS (ENHANCED with Theme and Status Aggregates)
    -- =================================================================================
    DROP TEMPORARY TABLE IF EXISTS ActionPlantemp;
  
    CREATE TEMPORARY TABLE ActionPlantemp
    AS
       SELECT DISTINCT
              ap.IdActionPlan, ap.Code, ap.Description, com.IdCompany, com.Alias AS Location,
              co.Name AS Country, co.Iso AS CountryIso, emp.EmployeeCode, emp.IdUser, emp.IdEmployee,
              emp.FirstName, emp.LastName, emp.IdGender, lv.IdLookupValue, lv.Value AS Origin,
              lvap.IdLookupValue AS IdBusinessUnit, lvap.Value AS BusinessUnit, lvap.HtmlColor AS BusinessUnitHtmlColor,
              
              COALESCE(cts.TotalActionItems, 0) AS TotalActionItems,
              COALESCE(cts.TotalOpenItems, 0) AS TotalOpenItems,
              COALESCE(cts.TotalClosedItems, 0) AS TotalClosedItems,
              COALESCE(cts.Stat_Overdue15, 0) AS Stat_Overdue15,
              COALESCE(cts.Stat_HighPriorityOverdue, 0) AS Stat_HighPriorityOverdue,
              COALESCE(cts.Stat_MaxDueDays, 0) AS Stat_MaxDueDays,
              COALESCE(cts.Stat_ThemesList, '') AS Stat_ThemesList,
              
              -- NEW: Precise aggregates for Side Filters and Alert Buttons
              (
                  SELECT GROUP_CONCAT(DISTINCT CONCAT('T|', tc.Theme, ':', tc.OpenCount) SEPARATOR ';')
                  FROM ActionPlanThemeCounts tc
                  WHERE tc.IdActionPlan = ap.IdActionPlan
              ) AS ThemeAggregates,
              
              (
                  SELECT GROUP_CONCAT(DISTINCT CONCAT('S|', sc.StatusLabel, ':', sc.CountItems) SEPARATOR ';')
                  FROM ActionPlanStatusCounts sc
                  WHERE sc.IdActionPlan = ap.IdActionPlan
              ) AS StatusAggregates,
              
              ap.CreatedBy, CONCAT(usr.FirstName, ' ', usr.LastName) AS CreatedByName, ap.CreatedIn,
              ap.IdDepartment, lvDep.DepartmentName AS Department, ap.OriginDescription,
              
              CONCAT(emp.FirstName, ' ', emp.LastName) AS Responsible,
              
              COALESCE(emp.DisplayName, CONCAT(emp.FirstName, ' ', emp.LastName)) AS ActionPlanResponsibleDisplayName,
              si.IdSite, si.Name AS SiteName, cus.IdCustomer, cus.Name AS CustomerName,
              lvz.IdLookupValue AS IdZone, lvz.Value AS Region
         FROM emdep_geos.action_plans ap
              INNER JOIN emdep_geos.companies com ON com.IdCompany = ap.IdLocation
              INNER JOIN countries co ON co.idCountry = com.IdCountry
              INNER JOIN emdep_geos.employees emp ON emp.IdEmployee = ap.IdResponsibleEmployee
              INNER JOIN emdep_geos.lookup_values lv ON lv.IdLookupValue = ap.IdOrigin
              LEFT JOIN emdep_geos.lookup_values lvap ON lvap.IdLookupValue = ap.IdBusinessUnit
*** End Patch