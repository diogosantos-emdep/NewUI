-- ============================================================================
-- FIX: Duplicate Action Plans with Same Code but Different IDs
-- ============================================================================
-- PROBLEMA IDENTIFICADO:
-- Query: SELECT * FROM action_plans WHERE Code = 'AP24.0015'
-- Retorna 2 registos: IdActionPlan=78 e IdActionPlan=386
-- 
-- Isto causa erro "Collection was modified" no DevExpress Grid
-- porque o sistema tenta processar o mesmo Action Plan duas vezes
-- ============================================================================

-- 1. INVESTIGAR DUPLICADOS NA BASE DE DADOS
-- Execute esta query para ver todos os códigos duplicados:

SELECT 
    Code,
    COUNT(*) as TotalRecords,
    GROUP_CONCAT(IdActionPlan ORDER BY IdActionPlan) as DuplicateIDs
FROM action_plans
WHERE Code LIKE 'AP%'
GROUP BY Code
HAVING COUNT(*) > 1
ORDER BY Code;

-- ============================================================================
-- 2. VER DETALHES DOS DUPLICADOS (exemplo para AP24.0015)
-- ============================================================================

SELECT 
    IdActionPlan,
    Code,
    description,
    IdLocation,
    IdResponsibleEmployee,
    IdOrigin,
    InUse,
    CreatedBy,
    CreatedIn,
    ModifiedBy,
    ModifiedIn
FROM action_plans
WHERE Code = 'AP24.0015'
ORDER BY IdActionPlan;

-- ============================================================================
-- 3. SOLUÇÃO A: Adicionar DISTINCT ao Stored Procedure
-- ============================================================================
-- Modifique o SP GetActionPlanDetails_WithCounts para adicionar DISTINCT
-- ou GROUP BY no final para eliminar duplicados

-- Exemplo de modificação no SP:
/*
SELECT DISTINCT
    ap.IdActionPlan,
    ap.Code,
    ap.Title,
    -- ... outros campos
FROM action_plans ap
-- ... resto do JOIN e WHERE
GROUP BY ap.Code, ap.IdActionPlan  -- Garante unicidade
ORDER BY ap.Code;
*/

-- ============================================================================
-- 4. SOLUÇÃO B: Criar Constraint UNIQUE no Code (se aplicável)
-- ============================================================================
-- Se o Code deveria ser único na tabela, adicione um constraint:

-- AVISO: Antes de executar, certifique-se que não há duplicados válidos
-- ALTER TABLE action_plans
-- ADD CONSTRAINT UK_ActionPlan_Code UNIQUE (Code);

-- ============================================================================
-- 5. SOLUÇÃO C: Remover Duplicados Manualmente (CUIDADO!)
-- ============================================================================
-- Se os registos duplicados são de facto inválidos, pode removê-los:

-- PASSO 1: Criar backup
-- CREATE TABLE action_plans_backup AS SELECT * FROM action_plans;

-- PASSO 2: Identificar IDs a manter (normalmente o mais recente)
-- SELECT 
--     Code,
--     MAX(IdActionPlan) as IdToKeep
-- FROM action_plans
-- GROUP BY Code
-- HAVING COUNT(*) > 1;

-- PASSO 3: Remover duplicados (EXECUTE COM CUIDADO!)
-- DELETE FROM action_plans
-- WHERE IdActionPlan NOT IN (
--     SELECT MAX(IdActionPlan) as IdToKeep
--     FROM action_plans_backup
--     GROUP BY Code
-- )
-- AND Code IN (
--     SELECT Code 
--     FROM action_plans_backup
--     GROUP BY Code
--     HAVING COUNT(*) > 1
-- );

-- ============================================================================
-- 6. VERIFICAR STORED PROCEDURE ATUAL
-- ============================================================================
-- Veja o código do SP para entender de onde vêm os duplicados:

SHOW CREATE PROCEDURE GetActionPlanDetails_WithCounts;

-- ============================================================================
-- 7. SOLUÇÃO RECOMENDADA PARA O SP
-- ============================================================================
-- Adicione esta CTE no início do SP para eliminar duplicados:

/*
WITH UniqueActionPlans AS (
    SELECT 
        *,
        ROW_NUMBER() OVER (PARTITION BY Code ORDER BY IdActionPlan DESC) as RowNum
    FROM action_plans
    WHERE InUse = 1
)
SELECT 
    uap.IdActionPlan,
    uap.Code,
    uap.Title,
    -- ... resto dos campos
FROM UniqueActionPlans uap
WHERE uap.RowNum = 1  -- Mantém apenas o registro mais recente por Code
-- ... resto dos JOINs e filtros
*/

-- ============================================================================
-- NOTA IMPORTANTE:
-- O código C# já foi modificado para filtrar duplicados automaticamente,
-- mantendo o IdActionPlan mais alto (mais recente).
-- 
-- No entanto, é ALTAMENTE RECOMENDADO corrigir a fonte do problema (SP/DB)
-- para evitar inconsistências futuras.
-- ============================================================================
