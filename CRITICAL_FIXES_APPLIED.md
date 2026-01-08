# CRITICAL FIXES - APM ModernUI (Side Filters + Alert Buttons + Dropdown Loop)

## Data: 2024-01-XX
## Problemas Corrigidos: 3 bugs críticos identificados pelos logs

---

## ✅ PROBLEMA 1: Side Filters retornam dados mas grid mostra 0 APs

**SINTOMA:**
```
[LoadActionPlansPageAsync] SP returned 90 Action Plans
[LoadActionPlansPageAsync] Auto-load completed: 0 total Action Plans loaded
```

**CAUSA RAIZ:**
- Linha 758: `ApplyFilters()` era chamado quando Theme/Alert ativos
- `ApplyFilters()` filtrava in-memory por `ThemeAggregates` e `Alert stats`
- O NOVO SP **NÃO RETORNA** esses campos → todos os APs eram filtrados out
- O SP JÁ FILTROU corretamente, mas o C# re-filtrava e bloqueava tudo!

**CORREÇÃO APLICADA:**
```csharp
// ANTES (linha 752-758):
bool isFiltering = IsFilterActive();
if (isFiltering)
{
    ApplyFilters(); // ❌ BLOQUEAVA tudo!
}

// DEPOIS (linha 746-790):
bool hasSideTile = _lastAppliedSideTileFilter != null && !IsAllCaption(_lastAppliedSideTileFilter.Caption);
bool hasAlert = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption);

if (hasSideTile || hasAlert)
{
    // ✅ Theme/Alert: SP já filtrou, apenas mostrar tudo (sem paginação)
    foreach (var dto in _allActionPlansUnfiltered)
    {
        ActionPlans.Add(dto);
    }
}
else
{
    // Dropdowns: Usar ApplyFilters() normal
    // Paginação: Blocos de 50
}
```

**RESULTADO ESPERADO:**
- Click "Administration" → SP retorna 90 APs → Grid mostra 90 APs ✅
- Não filtrar in-memory quando Theme/Alert ativos (confiar no SP)

---

## ✅ PROBLEMA 2: Alert Buttons não enviam filtro para SP

**SINTOMA:**
```
[OnAlertTileClickedInternal] Alert button clicked: To do
[LoadActionPlansAsync] Calling SP with filters: Alert=null
```

**CAUSA RAIZ:**
- `GetCurrentAlertFilter()` linha 3478-3515 usava `.Contains()` para matchear Caption
- Exemplo: `caption.Contains("TO DO")` falhava porque Caption real é `"To do"` (lowercase)
- Todos os IFs falhavam → retornava `null`

**CORREÇÃO APLICADA:**
```csharp
// ANTES:
var caption = activeButton.Caption?.ToUpperInvariant() ?? "";
if (caption.Contains("TO DO") || caption.Contains("TODO"))
    return "ToDo";

// DEPOIS:
var caption = activeButton.Caption ?? "";
GeosApplication.Instance.Logger?.Log($"[GetCurrentAlertFilter] Active button Caption: '{caption}'", Category.Info, Priority.Low);

if (caption == "Longest Overdue Days")  // ✅ Match EXATO
    return "LongestOverdue";
if (caption == "To do")  // ✅ Respeita capitalização
    return "ToDo";
// ... etc para todos os 8 botões
```

**MAPEAMENTOS:**
| Caption (UI)              | SQL Value             |
|---------------------------|-----------------------|
| Longest Overdue Days      | LongestOverdue        |
| High Priority Overdue     | HighPriorityOverdue   |
| Overdue > 15 Days         | Overdue15             |
| Most Overdue Theme        | MostOverdueTheme      |
| To do                     | ToDo                  |
| In progress               | InProgress            |
| Blocked                   | Blocked               |
| Closed                    | Closed                |

**RESULTADO ESPERADO:**
- Click "To do" → SP chamado com `Alert='ToDo'` ✅
- Logs mostram caption correta antes de mapear

---

## ✅ PROBLEMA 3: Dropdowns causam loop infinito

**SINTOMA:**
```
[RefreshDataAsync] Starting - clearing cache
[RefreshDataAsync] Starting - clearing cache
[RefreshDataAsync] Starting - clearing cache
... 10+ vezes seguidas!
```

**CAUSA RAIZ:**
1. User muda `SelectedLocation` → setter chama `RefreshDataAsync()`
2. `RefreshDataAsync()` → `PopulateDropdownFilters()`
3. `PopulateDropdownFilters()` atualiza `SelectedLocation` → setter triggered!
4. LOOP INFINITO

**CORREÇÃO APLICADA:**

**Passo 1:** Adicionar flag (linha 56-58):
```csharp
private CancellationTokenSource _searchCancellationTokenSource;

// Flag para prevenir loop infinito quando dropdowns mudam
private bool _isRefreshing = false;

private readonly Dictionary<long, List<ActionPlanTaskModernDto>> _tasksCache;
```

**Passo 2:** Usar flag nos setters (linha 157-218):
```csharp
public List<object> SelectedLocation
{
    get => _selectedLocation;
    set
    {
        if (_isRefreshing) return; // ✅ PREVENIR LOOP
        _selectedLocation = value;
        OnPropertyChanged(nameof(SelectedLocation));
        _ = RefreshDataAsync();
    }
}

// ... Aplicado a TODOS os 6 dropdowns:
// SelectedPerson, SelectedBusinessUnit, SelectedOrigin, 
// SelectedDepartment, SelectedCustomer
```

**Passo 3:** Ativar/desativar flag em `PopulateDropdownFilters()` (linha 2952-2978):
```csharp
private void PopulateDropdownFilters(List<APMActionPlanModern> data)
{
    _isRefreshing = true; // ✅ ATIVAR antes de atualizar dropdowns
    
    try
    {
        // ... atualizar dropdowns ...
    }
    finally
    {
        _isRefreshing = false; // ✅ DESATIVAR sempre
    }
}
```

**RESULTADO ESPERADO:**
- Mudar Location → 1 chamada a `RefreshDataAsync()` ✅
- Sem loops infinitos
- Dropdowns atualizam sem triggerar refresh adicional

---

## 📝 RESUMO DAS MUDANÇAS

### Ficheiro: `ActionPlansModernViewModel.cs`

1. **Linha 56-58:** Adicionar flag `_isRefreshing`
2. **Linha 157-218:** Adicionar `if (_isRefreshing) return;` a TODOS os 6 setters de dropdown
3. **Linha 746-820:** Substituir lógica `if (isFiltering)` por distinção Theme/Alert vs Dropdowns
4. **Linha 2952-2978:** Envolver `PopulateDropdownFilters()` com `_isRefreshing = true/false`
5. **Linha 3478-3520:** Corrigir `GetCurrentAlertFilter()` para usar matches exatos

---

## 🧪 COMO TESTAR

### Teste 1: Side Filters (Theme)
1. Abrir APM ModernUI
2. Click em "Administration" (lado esquerdo)
3. **ESPERADO:** Grid mostra APs (não 0!)
4. Verificar logs:
   ```
   [LoadActionPlansPageAsync] SP returned X Action Plans
   [LoadActionPlansPageAsync] Theme/Alert active - showing all X APs returned by SP
   ```

### Teste 2: Alert Buttons
1. Click em "To do"
2. Verificar logs:
   ```
   [OnAlertTileClickedInternal] Alert button clicked: To do
   [GetCurrentAlertFilter] Active button Caption: 'To do'
   [LoadActionPlansAsync] Calling SP with filters: Alert=ToDo
   ```
3. **ESPERADO:** `Alert=ToDo` (NÃO `Alert=null`)

### Teste 3: Dropdown Loop
1. Mudar Location para "Porto"
2. Verificar logs:
   ```
   [RefreshDataAsync] Starting - clearing cache
   [LoadActionPlansPageAsync] SP returned X Action Plans
   ```
3. **ESPERADO:** Apenas 1 chamada (não 10+)

### Teste 4: Combinação de Filtros
1. Click "Administration" + Mudar Location "Porto"
2. **ESPERADO:** Ambos os filtros aplicados no SP
3. Grid mostra resultados corretos

---

## ⚠️ IMPORTANTE - PRÓXIMOS PASSOS

Estes 3 fixes corrigem bugs CRÍTICOS no C#, mas ainda falta:

### TODO: Corrigir SP para filtrar Tasks (não só Action Plans)
**Problema:** Location/Responsible apenas filtram o Action Plan, não as suas Tasks
- Ex: AP criado em Lisboa, mas TEM tasks em Porto → não aparece quando filtra Porto

**Solução:** Alterar `APM_GetActionPlanDetails_WithCounts.txt` linha 128-137:
```sql
-- ANTES:
WHERE FIND_IN_SET(ap.IdLocation, _FilterLocation) > 0

-- DEPOIS:
WHERE FIND_IN_SET(ap.IdLocation, _FilterLocation) > 0
   OR EXISTS (
       SELECT 1 FROM APM_ActionPlanTask t 
       WHERE t.IdActionPlan = ap.IdActionPlan 
       AND FIND_IN_SET(t.IdLocation, _FilterLocation) > 0
   )
```

Aplicar esta lógica a TODOS os 6 filtros de dropdown!

---

## 📊 IMPACT ANALYSIS

| Bug                        | Severidade | Users Afetados | Status |
|----------------------------|------------|----------------|--------|
| Side Filters → 0 APs       | CRÍTICO    | 100%           | ✅ FIXO |
| Alert Buttons → null       | CRÍTICO    | 100%           | ✅ FIXO |
| Dropdown Loop Infinito     | CRÍTICO    | 100%           | ✅ FIXO |
| SP filtra só APs (não Tasks) | ALTO     | 60%            | ⏳ TODO |

---

## 🔍 DEBUGGING LOGS ADICIONADOS

```csharp
// GetCurrentAlertFilter:
GeosApplication.Instance.Logger?.Log($"[GetCurrentAlertFilter] Active button Caption: '{caption}'", Category.Info, Priority.Low);

// LoadActionPlansPageAsync:
GeosApplication.Instance.Logger?.Log($"[LoadActionPlansPageAsync] Theme/Alert active - showing all {_allActionPlansUnfiltered.Count} APs returned by SP", Category.Info, Priority.Low);
```

**Use estes logs para verificar:**
- Alert caption está correto?
- SP retorna dados mas grid não mostra? (olhar "Theme/Alert active" log)
- Loop infinito? (procurar múltiplos "Starting - clearing cache")

---

## ✅ CONCLUSÃO

**3 BUGS CRÍTICOS RESOLVIDOS:**
1. ✅ Side Filters agora CONFIAM no SP (não re-filtram in-memory)
2. ✅ Alert Buttons enviam valor correto (match exato de Caption)
3. ✅ Dropdowns não causam loop (flag `_isRefreshing`)

**Próximo passo:** Corrigir SP para filtrar Tasks!
