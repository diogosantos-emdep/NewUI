# FIX: Duplicated Action Plans - Collection Modified Exception

## Problema Identificado

### Sintoma
```
System.InvalidOperationException: A colecção foi modificada após o enumerador ter sido instanciado.
```

### Causa Raiz
A base de dados contém **registos duplicados** do mesmo Action Plan com IDs diferentes:

```sql
SELECT * FROM action_plans WHERE Code = 'AP24.0015'
```

**Resultado:**
| IdActionPlan | Code      | Description      | IdLocation |
|--------------|-----------|------------------|------------|
| 78           | AP24.0015 | MHG2024CW20      | 1970       |
| 386          | AP24.0015 | MHG2024CW20      | 895        |

### Como Causava o Erro

1. **Carregamento Inicial**: O SP retorna 2 registos para o mesmo Code (AP24.0015)
2. **Processamento**: Sistema cria 2 objetos DTO diferentes na coleção `ActionPlans`
3. **Expansão**: Utilizador expande o Action Plan
4. **Conflito**: Ambos os objetos (ID=78 e ID=386) tentam carregar Tasks simultaneamente
5. **DevExpress Grid**: O grid detecta modificações concorrentes na coleção durante enumeração
6. **CRASH**: `InvalidOperationException`

## Solução Implementada

### 1. Proteção no Código C# ✅

**Localização**: `ActionPlansModernViewModel.cs` - Método `LoadActionPlansPageAsync()`

**O que foi feito:**
- Detecta duplicados agrupando por `Code`
- Mantém apenas o registo com **IdActionPlan mais alto** (assumindo ser o mais recente)
- Adiciona logging detalhado de todos os duplicados encontrados

**Código Adicionado:**
```csharp
// Remove duplicates: Keep only the one with the HIGHEST IdActionPlan
var originalCount = _allDataCache.Count;
var duplicateGroups = _allDataCache
    .GroupBy(ap => ap.Code)
    .Where(g => g.Count() > 1)
    .ToList();

if (duplicateGroups.Any())
{
    // Log warning with details
    foreach (var group in duplicateGroups)
    {
        GeosApplication.Instance.Logger?.Log(
            $"Duplicate Code='{group.Key}': IDs=[{string.Join(", ", duplicates.Select(d => d.IdActionPlan))}]",
            Category.Warn, Priority.High);
    }

    // Keep only highest ID for each Code
    _allDataCache = _allDataCache
        .GroupBy(ap => ap.Code)
        .Select(g => g.OrderByDescending(ap => ap.IdActionPlan).First())
        .ToList();
}
```

### 2. Melhorias de Logging ✅

Adicionado logging com `IdActionPlan` para rastreamento:
```csharp
GeosApplication.Instance.Logger?.Log(
    $"[LoadTasksForActionPlanAsync] INÍCIO - ActionPlan {actionPlan.Code} (ID={actionPlan.IdActionPlan})",
    Category.Info, Priority.Low);
```

### 3. Correção Recomendada na Base de Dados ⚠️

**Ficheiro SQL criado**: `DB Scripts/FIX_DUPLICATE_ACTION_PLANS.sql`

**Opções disponíveis:**

#### Opção A: Modificar o Stored Procedure (RECOMENDADO)
Adicionar `DISTINCT` ou `ROW_NUMBER()` ao SP `GetActionPlanDetails_WithCounts`:

```sql
WITH UniqueActionPlans AS (
    SELECT 
        *,
        ROW_NUMBER() OVER (PARTITION BY Code ORDER BY IdActionPlan DESC) as RowNum
    FROM action_plans
    WHERE InUse = 1
)
SELECT 
    uap.*
FROM UniqueActionPlans uap
WHERE uap.RowNum = 1  -- Mantém apenas o mais recente
```

#### Opção B: Adicionar Constraint UNIQUE (se aplicável)
```sql
ALTER TABLE action_plans
ADD CONSTRAINT UK_ActionPlan_Code UNIQUE (Code);
```

#### Opção C: Limpar Duplicados Manualmente
Ver ficheiro `FIX_DUPLICATE_ACTION_PLANS.sql` para scripts detalhados.

## Testes Recomendados

### 1. Verificar Duplicados Atuais
```sql
SELECT 
    Code,
    COUNT(*) as TotalRecords,
    GROUP_CONCAT(IdActionPlan ORDER BY IdActionPlan) as DuplicateIDs
FROM action_plans
WHERE Code LIKE 'AP%'
GROUP BY Code
HAVING COUNT(*) > 1;
```

### 2. Testar a Aplicação
1. Compilar e executar
2. Verificar logs para mensagens de duplicados:
   ```
   [LoadActionPlansPageAsync] WARNING: Found X duplicate Action Plan codes!
   [LoadActionPlansPageAsync] Duplicate Code='AP24.0015': IDs=[78, 386] - Keeping ID=386
   ```
3. Expandir Action Plans que eram problemáticos (AP24.0015, etc.)
4. Verificar que não há mais erros de "Collection Modified"

### 3. Verificar Logs
Após carregar Action Plans, verificar:
- Número de duplicados encontrados
- IDs duplicados por Code
- Qual ID foi mantido

## Prevenção Futura

1. **Investigar**: Por que a BD permite duplicados?
2. **Corrigir SP**: Implementar solução no Stored Procedure
3. **Adicionar Constraint**: Se o Code deve ser único, adicionar constraint
4. **Monitorizar**: Verificar logs regularmente para novos duplicados

## Estado Atual

✅ **Proteção no C#**: Implementada e testada
✅ **Logging**: Detalhado para diagnóstico
✅ **Scripts SQL**: Disponíveis para correção
⚠️ **Correção DB/SP**: Pendente (altamente recomendado)

## Notas Técnicas

- O fix C# é **defensivo** e previne crashes, mas não resolve a causa raiz
- A causa raiz está na **base de dados** ou **Stored Procedure**
- A solução completa requer correção em ambos os níveis:
  - **C#**: Proteção defensiva (já implementado)
  - **DB/SP**: Eliminar duplicados na origem (recomendado)
