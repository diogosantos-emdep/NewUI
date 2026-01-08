-- ========================================
-- QUERIES DE TESTE - APM Stored Procedures
-- ========================================
-- Estas queries ajudam a preparar os testes no Postman
-- Executa no HeidiSQL para obter IDs válidos
-- ========================================

-- ========================================
-- 1. ENCONTRAR USER VÁLIDO COM PERMISSÕES
-- ========================================

-- Admin user (tem acesso a tudo)
SELECT 
    u.IdUser, 
    e.FirstName, 
    e.LastName,
    'Admin' as Role
FROM emdep_geos.user_permissions up
INNER JOIN emdep_geos.users u ON u.IdUser = up.IdUser
INNER JOIN emdep_geos.employees e ON e.IdUser = u.IdUser
WHERE up.IdPermission = 122  -- Admin permission
LIMIT 5;

-- Location Manager (tem acesso a locations específicas)
SELECT 
    u.IdUser, 
    e.FirstName, 
    e.LastName,
    'Location Manager' as Role
FROM emdep_geos.user_permissions up
INNER JOIN emdep_geos.users u ON u.IdUser = up.IdUser
INNER JOIN emdep_geos.employees e ON e.IdUser = u.IdUser
WHERE up.IdPermission = 134  -- Location Manager
LIMIT 5;

-- ========================================
-- 2. ENCONTRAR ACTION PLAN COM TASKS
-- ========================================

-- Action Plans com contagem de tasks e subtasks
SELECT 
    ap.IdActionPlan,
    ap.Code,
    ap.Description,
    c.Alias as Location,
    CONCAT(e.FirstName, ' ', e.LastName) as Responsible,
    COUNT(DISTINCT CASE WHEN apt.IdParent IS NULL THEN apt.IdTask END) as TotalTasks,
    COUNT(DISTINCT CASE WHEN apt.IdParent IS NOT NULL THEN apt.IdTask END) as TotalSubTasks
FROM emdep_geos.action_plans ap
LEFT JOIN emdep_geos.companies c ON c.IdCompany = ap.IdLocation
LEFT JOIN emdep_geos.employees e ON e.IdEmployee = ap.IdResponsibleEmployee
LEFT JOIN emdep_geos.action_plan_task apt ON apt.IdActionPlan = ap.IdActionPlan AND apt.InUse = 1
WHERE ap.InUse = 1
GROUP BY ap.IdActionPlan, ap.Code, ap.Description, c.Alias, e.FirstName, e.LastName
HAVING TotalTasks > 0
ORDER BY TotalTasks DESC
LIMIT 10;

-- ========================================
-- 3. VER TASKS DE UM ACTION PLAN ESPECÍFICO
-- ========================================

-- Substituir 123 pelo IdActionPlan real
SELECT 
    apt.IdTask,
    apt.TaskNumber,
    apt.Title,
    apt.Description,
    lvTheme.Value as Theme,
    lvStatus.Value as Status,
    apt.DueDate,
    apt.CloseDate,
    CASE 
        WHEN apt.CloseDate IS NOT NULL THEN DATEDIFF(apt.CloseDate, apt.DueDate)
        ELSE DATEDIFF(CURDATE(), apt.DueDate)
    END as DueDays,
    (SELECT COUNT(*) FROM emdep_geos.action_plan_task WHERE IdParent = apt.IdTask AND InUse = 1) as SubTaskCount
FROM emdep_geos.action_plan_task apt
LEFT JOIN emdep_geos.lookup_values lvTheme ON lvTheme.IdLookupValue = apt.IdTheme
LEFT JOIN emdep_geos.lookup_values lvStatus ON lvStatus.IdLookupValue = apt.IdStatus
WHERE apt.IdActionPlan = 123  -- SUBSTITUIR AQUI
  AND apt.IdParent IS NULL  -- Apenas tasks principais
  AND apt.InUse = 1
ORDER BY apt.TaskNumber;

-- ========================================
-- 4. VER SUBTASKS DE UMATask real (da task principal)
SELECT 
    apt.IdTask,
    apt.TaskNumber,
    apt.Title,
    apt.Description,
    lvTheme.Value as Theme,
    lvStatus.Value as Status,
    apt.DueDate,
    apt.CloseDate,
    parent.TaskNumber as ParentTaskNumber,
    parent.Title as ParentTaskTitle
FROM emdep_geos.action_plan_task apt
LEFT JOIN emdep_geos.lookup_values lvTheme ON lvTheme.IdLookupValue = apt.IdTheme
LEFT JOIN emdep_geos.lookup_values lvStatus ON lvStatus.IdLookupValue = apt.IdStatus
LEFT JOIN emdep_geos.action_plan_task parent ON parent.IdTask = apt.IdParent
WHERE apt.IdParent = 456  -- SUBSTITUIR AQUI (IdTask da task principal)
  AND apt.InUse = 1
ORDER BY apt.TaskNumber;

-- ========================================
-- 5. LISTAR THEMES DISPONÍVEIS
-- ========================================

SELECT 
    IdLookupValue,
    Value as ThemeName,
    HtmlColor
FROM emdep_geos.lookup_values
WHERE IdLookupType = 155  -- Theme lookup type
ORDER BY Value;

-- ========================================
-- 6. TESTAR SP DIRETAMENTE NO MySQL
-- ========================================

-- Teste 1: GetActionPlanDetails_WithCounts SEM filtros
CALL emdep_geos.APM_GetActionPlanDetails_WithCounts(
    '2025',     -- selectedPeriod
    123,        -- idUser (SUBSTITUIR)
    NULL,       -- filterLocation
    NULL,       -- filterResponsible
    NULL,       -- filterBusinessUnit
    NULL,       -- filterOrigin
    NULL,       -- filterDepartment
    NULL,       -- filterCustomer
    NULL,       -- alertFilter
    NULL        -- filterTheme
);

-- Teste 2: GetActionPlanDetails_WithCounts COM filtro Theme=Safety
CALL emdep_geos.APM_GetActionPlanDetails_WithCounts(
    '2025',
    123,        -- SUBSTITUIR
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    'Safety'    -- Theme filter
);

-- Teste 3: GetTaskListByIdActionPlan_V2680PT SEM filtros
CALL emdep_geos.APM_GetTaskListByIdActionPlan_V2680PT(
    123,        -- idActionPlan (SUBSTITUIR)
    '2025',     -- selectedPeriod
    123,        -- idUser (SUBSTITUIR)
    NULL,       -- filterLocation
    NULL,       -- filterResponsible
    NULL,       -- filterBusinessUnit
    NULL,       -- filterOrigin
    NULL,       -- filterDepartment
    NULL,       -- filterCustomer
    NULL,       -- alertFilter
    NULL        -- filterTheme
);

-- Teste 4: GetTaskListByIdActionPlan_V2680PT COM filtro Theme=Quality
CALL emdep_geos.APM_GetTaskListByIdActionPlan_V2680PT(
    123,        -- idActionPlan (SUBSTITUIR)
    '2025',
    123,        -- idUser (SUBSTITUIR)
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    'Quality'   -- Theme filter
);

-- ========================================
-- 7. VALIDAR RESULTADOS
-- ========================================

-- Contar Action Plans retornados pelo SP (sem filtro)
-- Executar query acima e contar linhas manualmente

-- Contar Action Plans que TÊM tasks com Theme=Safety
SELECT COUNT(DISTINCT ap.IdActionPlan) as ActionPlansWithSafety
FROM emdep_geos.action_plans ap
INNER JOIN emdep_geos.action_plan_task apt ON apt.IdActionPlan = ap.IdActionPlan
LEFT JOIN emdep_geos.lookup_values lv ON lv.IdLookupValue = apt.IdTheme
WHERE ap.InUse = 1
  AND lv.Value = 'Safety';

-- Contar Tasks de um AP específico
SELECT COUNT(*) as TotalTasks
FROM emdep_geos.action_plan_task
WHERE IdActionPlan = 123;  -- SUBSTITUIR

-- Contar Tasks de um AP com Theme=Quality
SELECT COUNT(*) as TasksWithQuality
FROM emdep_geos.action_plan_task apt
LEFT JOIN emdep_geos.lookup_values lv ON lv.IdLookupValue = apt.IdTheme
WHERE apt.IdActionPlan = 123  -- SUBSTITUIR
  AND lv.Value = 'Quality';

-- ========================================
-- 8. ENCONTRAR ACTION PLAN COM MÚLTIPLOS THEMES
-- ========================================

-- Action Plans que têm tasks com pelo menos 2 themes diferentes
SELECT 
    ap.IdActionPlan,
    ap.Code,
    GROUP_CONCAT(DISTINCT lv.Value SEPARATOR ', ') as Themes,
    COUNT(DISTINCT lv.IdLookupValue) as ThemeCount
FROM emdep_geos.action_plans ap
INNER JOIN emdep_geos.action_plan_task apt ON apt.IdActionPlan = ap.IdActionPlan
LEFT JOIN emdep_geos.lookup_values lv ON lv.IdLookupValue = apt.IdTheme
WHERE ap.InUse = 1
GROUP BY ap.IdActionPlan, ap.Code
HAVING ThemeCount >= 2
LIMIT 5;

-- ========================================
-- NOTAS DE UTILIZAÇÃO
-- ========================================

/*
1. Executar queries 1-2 para obter IdUser válido
2. Executar query 2 para obter IdActionPlan com tasks
3. Anotar os valores obtidos
4. Substituir nos testes SQL (secção 6) ou nos requests Postman
5. Comparar resultados SQL direto vs Postman (devem ser iguais)
6. Se SP retornar dados no MySQL mas Postman não, problema é no web service
7. Se SP não retornar dados no MySQL, problema é no SP ou permissões
*/
