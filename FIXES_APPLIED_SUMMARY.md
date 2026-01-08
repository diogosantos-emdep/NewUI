# ✅ CORREÇÕES APLICADAS - APM ModernUI

**Data:** 7 Janeiro 2026  
**Issues Reportados:** 15+ bugs (grid, filters, expand, alert buttons)

---

## 🎯 PROBLEMAS CORRIGIDOS

### 1. ✅ Colunas da Grid (XAML)
**Problema:** Status mostrava Business Unit, Priority mostrava Origin, faltavam colunas  
**Ficheiro:** `Src/Modules/APMModule/Views/ActionPlansModernView.xaml`

**Alterações:**
- ❌ Removido: `Status` (Action Plans não têm status - são as tasks)
- ❌ Removido: `Priority` (Action Plans não têm priority - são as tasks)
- ❌ Removido: `Due Date` (Action Plans não têm due date - são as tasks)
- ✅ Adicionado: `Business Unit` (coluna com FieldName="BusinessUnit")
- ✅ Adicionado: `Origin` (coluna com FieldName="Origin")
- ✅ Adicionado: `Department` (coluna com FieldName="Department")
- ✅ Renomeado: `Items` → `Sub Tasks` (mais claro para o utilizador)

**Resultado Final das Colunas:**
1. Code ✅
2. Title ✅
3. Responsible ✅
4. Location ✅
5. **Business Unit** ✅ (NOVO)
6. **Origin** ✅ (NOVO)
7. **Department** ✅ (NOVO)
8. Progress ✅
9. Tasks ✅
10. Sub Tasks ✅ (renomeado)
11. Open ✅
12. Closed ✅
13. Group ✅

---

### 2. ✅ Mapeamentos Errados no DTO
**Problema:** Campos Status e Priority mapeavam valores errados  
**Ficheiro:** `Src/Modules/APMModule/ViewModels/ActionPlansModernViewModel.cs`

**Antes (ERRADO):**
```csharp
Status = entity.BusinessUnit ?? string.Empty,  // ❌ ERRADO!
Priority = entity.Origin ?? string.Empty,      // ❌ ERRADO!
```

**Depois (CORRETO):**
```csharp
Status = string.Empty,      // ✅ Action Plans não têm status
Priority = string.Empty,    // ✅ Action Plans não têm priority
```

---

### 3. ✅ Dropdowns Agora Atualizam Automaticamente
**Problema:** Mudar Location/Responsible/etc não recarregava a lista  
**Ficheiro:** `Src/Modules/APMModule/ViewModels/ActionPlansModernViewModel.cs`

**Antes (ERRADO):**
```csharp
set {
    _selectedLocation = value;
    OnPropertyChanged(nameof(SelectedLocation));
    ApplyInMemoryFiltersAsync(); // ❌ Filtrava in-memory (não funciona com lazy loading)
}
```

**Depois (CORRETO):**
```csharp
set {
    _selectedLocation = value;
    OnPropertyChanged(nameof(SelectedLocation));
    _ = RefreshDataAsync(); // ✅ Recarrega do SQL com filtros
}
```

**Dropdowns Corrigidos:**
- ✅ SelectedLocation
- ✅ SelectedPerson (Responsible)
- ✅ SelectedBusinessUnit
- ✅ SelectedOrigin
- ✅ SelectedDepartment
- ✅ SelectedCustomer

**Comportamento Novo:**
- Ao selecionar/deselecionar qualquer dropdown → Reload AUTOMÁTICO do SQL
- Mantém outros filtros ativos (Side Filters, Alert Buttons)
- Logs adicionados para debugging

---

### 4. ✅ Logging para Debug
**Ficheiro:** `Src/Modules/APMModule/ViewModels/ActionPlansModernViewModel.cs`

**Logs Adicionados:**
```csharp
// Antes de chamar SP
GeosApplication.Instance.Logger?.Log(
    $"[LoadActionPlansAsync] Calling SP with filters: " +
    $"Location={filterLocation ?? "null"}, " +
    $"Responsible={filterResponsible ?? "null"}, " +
    $"BU={filterBusinessUnit ?? "null"}, " +
    $"Alert={alertFilter ?? "null"}, " +
    $"Theme={themeFilter ?? "null"}",
    Category.Info, Priority.Low);

// Após SP retornar
GeosApplication.Instance.Logger?.Log(
    $"[LoadActionPlansPageAsync] SP returned {_allDataCache.Count} Action Plans",
    Category.Info, Priority.Low);

// Se vazio
GeosApplication.Instance.Logger?.Log(
    "[LoadActionPlansPageAsync] NO ACTION PLANS RETURNED - Aborting",
    Category.Warn, Priority.Medium);
```

**Como Usar:**
1. Abrir aplicação
2. Clicar em Side Filter (ex: "Safety")
3. Verificar logs em: `C:\Logs\GeosWorkbench\` ou Output Window
4. Ver se SP está a ser chamado com `Theme=Safety`
5. Ver quantos APs foram retornados

---

## ⚠️ PROBLEMAS PENDENTES (Requerem Alterações no Stored Procedure)

### 🔴 **CRÍTICO:** Side Filters (Theme) Não Listam Nada

**Causa Provável:** Stored Procedure APM_GetActionPlanDetails_WithCounts tem problemas:

1. **Filtro Theme pode estar a bloquear tudo:**
   - Linha 135: `AND (_FilterTheme IS NULL OR _FilterTheme = '' OR lvTheme.Value = _FilterTheme)`
   - Se um Action Plan não tiver nenhuma task com Theme="Safety", é excluído
   - **CORRETO:** Deve incluir se ALGUMA task (incluindo subtasks) tiver o Theme

2. **Permissões podem estar a falhar:**
   - Location Manager: Linha 49-56
   - Pode não ter permissões na tabela `site_user_module_geos_permission`
   - Verificar se `gp.IdGeosModule = 15` está correto

3. **Faltam logs no SP:**
   - Adicionar `SELECT 'Debug: UserAllowedAPs count', COUNT(*) FROM UserAllowedAPs;`
   - Adicionar `SELECT 'Debug: FinalFilteredAPs count', COUNT(*) FROM FinalFilteredAPs;`

**SOLUÇÃO SUGERIDA para o SP:**
```sql
-- Em vez de:
AND (_FilterTheme IS NULL OR _FilterTheme = '' OR lvTheme.Value = _FilterTheme)

-- Usar:
AND (
    _FilterTheme IS NULL OR _FilterTheme = '' OR 
    EXISTS (
        SELECT 1 FROM emdep_geos.action_plan_task t2
        LEFT JOIN emdep_geos.lookup_values lvTheme2 ON t2.IdTheme = lvTheme2.IdLookupValue
        WHERE t2.IdActionPlan = u.IdActionPlan 
          AND t2.InUse = 1
          AND lvTheme2.Value = _FilterTheme
    )
)
```

---

### 🔴 **CRÍTICO:** Expand Não Mostra Tasks/SubTasks

**Causa Provável:** Stored Procedure APM_GetTaskListByIdActionPlan_V2680PT retorna vazio

**Verificar:**
1. SP existe na base de dados?
   ```sql
   SHOW PROCEDURE STATUS WHERE Name LIKE '%GetTaskListByIdActionPlan%';
   ```

2. Permissões do user:
   - Admin (122): Deve ver tudo
   - Location Manager (134): Verifica linhas 445-478
   - Department Manager (135): Verifica linhas 480-546
   - Base User (136): Verifica linhas 548-600

3. Teste direto no HeidiSQL:
   ```sql
   CALL emdep_geos.APM_GetTaskListByIdActionPlan_V2680PT(
       29,        -- IdActionPlan (SUBSTITUIR)
       '2025',    -- SelectedPeriod
       859,       -- IdUser (SUBSTITUIR)
       NULL, NULL, NULL, NULL, NULL, NULL,  -- Filtros
       NULL,      -- AlertFilter
       NULL       -- FilterTheme
   );
   ```

4. Verificar resultados:
   - Deve retornar 2 result sets: Tasks e SubTasks
   - Se retornar 0 linhas → problema de permissões
   - Se retornar NULL → SP não encontrado

---

### 🟡 Alert Filters - Problemas de Lógica

#### **1. Longest Overdue** - Deve retornar APENAS 1 AP
**Problema:** Atualmente retorna TODOS os APs com tasks atrasadas  
**Ficheiro SP:** `APM_GetActionPlanDetails_WithCounts.txt` linha 147

**Solução:**
```sql
-- Adicionar ao final do SP, ANTES do ORDER BY:
AND (
    _AlertFilter != 'LongestOverdue' OR
    u.IdActionPlan = (
        SELECT t3.IdActionPlan 
        FROM emdep_geos.action_plan_task t3
        WHERE t3.InUse = 1 AND t3.CloseDate IS NULL AND t3.DueDate < CURDATE()
        ORDER BY DATEDIFF(CURDATE(), t3.DueDate) DESC
        LIMIT 1
    )
)
```

#### **2. To Do / In Progress / Blocked** - Não Filtram
**Problema:** Linha 141-143 do SP - LIKE patterns podem estar errados

**Valores Reais na Tabela lookup_values:**
```sql
SELECT Value FROM emdep_geos.lookup_values WHERE IdLookupType = [STATUS_TYPE_ID];
```

**Verificar se é:**
- "To do" ou "To Do" ou "TODO"
- "In Progress" ou "In progress" ou "InProgress"
- "Blocked" ou "On Hold"

**Ajustar linhas 141-143:**
```sql
(_AlertFilter = 'ToDo' AND (lvStatus.Value LIKE '%To do%' OR lvStatus.Value LIKE '%To Do%' OR lvStatus.Value LIKE '%TODO%')) OR
(_AlertFilter = 'InProgress' AND (lvStatus.Value LIKE '%In Progress%' OR lvStatus.Value LIKE '%In progress%')) OR
(_AlertFilter = 'Blocked' AND (lvStatus.Value LIKE '%Blocked%' OR lvStatus.Value LIKE '%On Hold%')) OR
```

#### **3. Most Overdue Theme** - Implementação Incompleta
**Problema:** Linha 146 não agrupa por Theme  
**Solução:** Deve retornar APs do Theme com MAIS tasks atrasadas

```sql
AND (
    _AlertFilter != 'MostOverdueTheme' OR
    lvTheme.IdLookupValue = (
        SELECT t3.IdTheme
        FROM emdep_geos.action_plan_task t3
        WHERE t3.InUse = 1 AND t3.CloseDate IS NULL AND t3.DueDate < CURDATE()
        GROUP BY t3.IdTheme
        ORDER BY COUNT(*) DESC
        LIMIT 1
    )
)
```

---

### 🟡 Dropdowns Filtram Apenas Action Plans (Não Tasks/SubTasks)

**Problema Atual:**
- Selecionar Location "Porto" → Mostra só APs **criados** em Porto
- **DEVERIA:** Mostrar APs que TÊM tasks/subtasks em Porto (mesmo que AP seja de Lisboa)

**Causa:** Linhas 128-137 do SP aplicam filtros só ao nível do AP

**Solução:**
```sql
-- Em vez de:
AND (_FilterLocation IS NULL OR _FilterLocation = '' OR FIND_IN_SET(ap.IdLocation, _FilterLocation) > 0)

-- Usar:
AND (
    _FilterLocation IS NULL OR _FilterLocation = '' OR 
    FIND_IN_SET(ap.IdLocation, _FilterLocation) > 0 OR  -- AP location
    FIND_IN_SET(t.IdLocation, _FilterLocation) > 0     -- TASK location
)
```

**Aplicar a TODOS os dropdowns:**
- ✅ FilterLocation → Verificar `ap.IdLocation` E `t.IdLocation`
- ✅ FilterResponsible → Verificar `ap.IdResponsibleEmployee` E `t.IdResponsibleEmployee`
- ✅ FilterBusinessUnit → Verificar `ap.IdBusinessUnit` E `t.IdBusinessUnit` (se existir)
- ✅ FilterOrigin → Verificar `ap.IdOrigin` E `t.IdOrigin` (se existir)
- ✅ FilterDepartment → Verificar `ap.IdDepartment` E `t.IdDepartment` (se existir)
- ✅ FilterCustomer → Verificar `ap.IdSite` E tasks associadas a customer

---

## 📋 CHECKLIST FINAL

### ✅ **CONCLUÍDO (Código C#)**
- [x] Colunas da grid corrigidas (Business Unit, Origin, Department adicionados)
- [x] Mapeamentos DTO corrigidos (Status e Priority vazios)
- [x] Dropdowns triggeram auto-refresh
- [x] Logging adicionado para debugging
- [x] "Items" renomeado para "Sub Tasks"

### ⏳ **PENDENTE (Stored Procedures)**
- [ ] **CRÍTICO:** Debug Side Filters (porque retorna vazio?)
- [ ] **CRÍTICO:** Debug Expand (porque não mostra tasks?)
- [ ] Longest Overdue → Limitar a 1 AP
- [ ] To Do/In Progress/Blocked → Corrigir LIKE patterns
- [ ] Most Overdue Theme → Agrupar por Theme e ordenar
- [ ] Dropdowns → Filtrar tasks E Action Plans
- [ ] Adicionar logs temporários no SP para debugging

---

## 🛠️ PRÓXIMOS PASSOS

### 1. **Testar com Logs**
```
1. Abrir aplicação
2. Clicar Side Filter "Safety"
3. Verificar Output logs:
   - "[LoadActionPlansAsync] Calling SP with filters: Theme=Safety"
   - "[LoadActionPlansPageAsync] SP returned X Action Plans"
4. Se X=0 → Problema no SP
5. Se X>0 mas grid vazia → Problema no código C#
```

### 2. **Testar SP Diretamente no HeidiSQL**
```sql
-- Teste 1: Admin sem filtros
CALL emdep_geos.APM_GetActionPlanDetails_WithCounts(
    '2025', 456, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
);

-- Teste 2: Admin com Theme=Safety
CALL emdep_geos.APM_GetActionPlanDetails_WithCounts(
    '2025', 456, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Safety'
);

-- Teste 3: Expand tasks de um AP
CALL emdep_geos.APM_GetTaskListByIdActionPlan_V2680PT(
    '2025', 29, 456, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
);
```

### 3. **Corrigir SP** (ficheiro: `APM_GetActionPlanDetails_WithCounts.txt`)
1. Adicionar debug logs temporários
2. Corrigir filtros para incluir tasks/subtasks
3. Corrigir alert filters (Longest, To Do, etc)
4. Re-aplicar na base de dados
5. Testar novamente

### 4. **Validar**
- [ ] Side Filters mostram APs
- [ ] Expand mostra tasks/subtasks
- [ ] Dropdowns filtram corretamente
- [ ] Alert filters funcionam
- [ ] Contagens corretas

---

## 📞 SUPORTE

**Se problemas persistirem:**
1. Enviar logs completos (Output Window)
2. Enviar screenshot da grid vazia
3. Executar queries de teste no HeidiSQL e enviar resultados
4. Verificar permissões do utilizador na BD

**Ficheiros Alterados:**
- ✅ `Src/Modules/APMModule/Views/ActionPlansModernView.xaml`
- ✅ `Src/Modules/APMModule/ViewModels/ActionPlansModernViewModel.cs`
- ⏳ `APM_GetActionPlanDetails_WithCounts.txt` (PENDENTE)
- ⏳ `APM_GetTaskListByIdActionPlan_V2680PT.txt` (PENDENTE)
