# Comparação: OLD ViewModel vs Modern UI - Filtros, Side Tiles e Alert Buttons

## 📊 RESUMO EXECUTIVO

### ✅ O que está CORRETO no Modern UI
- Métodos auxiliares de conversão (ToStringHashSet, ToResponsibleIdSet, etc.) ✓
- Estrutura básica de Side Tiles e Alert Buttons ✓  
- Lógica de ParseCriteria e PropertyMatches ✓

### ❌ O que está FALTANDO no Modern UI
1. **RecalculateAllCounts** - Lógica central de sincronização
2. **UpdateSideTileCounts** - Contagem dos side tiles
3. **UpdateAlertFiltersBasedOnFilteredData** - Contagem dos alert buttons
4. **BuildBaselineForAlertTiles** - Base de dados para alert buttons
5. **ApplyAlertToPlans** - Aplicação de filtros de alert
6. **Métodos de contagem de Tasks/SubTasks** (ExtractAllTasksAndSubTasksWithAllFilters)
7. **TaskOrSubTaskItem** - Classe helper para contagens

---

## 🔍 ANÁLISE DETALHADA

### 1. **RecalculateAllCounts() - O CORAÇÃO DO SISTEMA**

#### OLD ViewModel (CORRETO):
```csharp
private void RecalculateAllCounts()
{
    // 1. Constrói base de dados com TODOS os filtros de dropdown
    var baseData = BuildBaselineForAlertTiles();
    
    var activeSideFilter = _lastAppliedSideTileFilter;
    var activeAlertCaption = _lastAppliedAlertCaption;
    
    // 2. INDEPENDÊNCIA CRUZADA - Cada tile recebe dados SEM seu próprio filtro
    
    // Side Tiles: recebe dados COM alert + custom, MAS SEM side
    var dataForSideCounts = ApplyAlertFilterToData(
                               ApplyCustomFilterToData(baseData, activeCustomFilter),
                               activeAlertCaption);
    
    // Alert Buttons: recebe dados COM side + custom, MAS SEM alert
    var dataForAlertCounts = ApplyCustomFilterToData(
                                ApplySideFilterToData(baseData, activeSideFilter),
                                activeCustomFilter);
    
    // 3. Atualiza contagens
    UpdateSideTileCounts(dataForSideCounts);
    UpdateAlertFiltersBasedOnFilteredData(dataForAlertCounts);
    
    // 4. Grid final: todos os filtros aplicados
    var finalDataForGrid = ApplyAlertFilterToData(
                              ApplyCustomFilterToData(
                                ApplySideFilterToData(baseData, activeSideFilter),
                                activeCustomFilter),
                              activeAlertCaption);
    
    ActionPlanList = new ObservableCollection<APMActionPlan>(finalDataForGrid);
}
```

#### Modern UI (INCOMPLETO):
```csharp
private void RecalculateAllCounts()
{
    // ❌ FALTA: BuildBaselineForAlertTiles()
    // ❌ FALTA: ApplyAlertFilterToData()
    // ❌ FALTA: ApplyCustomFilterToData()
    // ❌ Só chama UpdateAlertButtonCounts() e UpdateSideTileCountsRespectingRules()
    // ❌ Não aplica a lógica de independência cruzada
}
```

---

### 2. **CONTAGEM DE ACTION PLANS E SUBTASKS**

#### OLD ViewModel (CORRETO):
```csharp
private List<TaskOrSubTaskItem> ExtractAllTasksAndSubTasksWithAllFilters(
    List<APMActionPlan> plans,
    HashSet<int> personIdSet,
    HashSet<string> personNameSet,
    HashSet<string> locSet,
    HashSet<string> buSet,
    HashSet<string> originSet,
    HashSet<string> customerSet,
    HashSet<int> customerIdSet,
    HashSet<string> codeSet,
    bool customerIncludesBlanks)
{
    // Extrai TODAS as tasks E subtasks em uma lista flat
    // Aplica TODOS os filtros de dropdown
    // Conta tasks + subtasks juntos
}

// Classe helper
private class TaskOrSubTaskItem
{
    public int DueDays { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Theme { get; set; }
    public bool IsTask { get; set; }
    
    public int GetActualDueDays()
    {
        if (DueDate >= DateTime.Now) return 0;
        return (int)(DateTime.Now - DueDate).TotalDays;
    }
}
```

**IMPORTANTE**: O OLD conta **TASKS + SUBTASKS JUNTAS** como um único conjunto!

#### Modern UI (FALTANDO):
```csharp
// ❌ NÃO EXISTE ExtractAllTasksAndSubTasksWithAllFilters
// ❌ NÃO EXISTE TaskOrSubTaskItem
// ❌ Contagem incorreta - não considera subtasks
```

---

### 3. **COMO ALERT BUTTONS ALTERAM SIDE FILTERS (mas não eles mesmos)**

#### OLD ViewModel - Lógica de Independência:
```csharp
private void UpdateSideTileCounts(List<APMActionPlan> source)
{
    // source JÁ VEM com alert aplicado (se houver)
    // source NÃO tem side filter aplicado
    
    foreach (var tile in ListOfFilterTile)
    {
        if (IsAllCaption(tile.Caption))
        {
            // ALL: conta tudo (com alert, sem side)
            var allItems = ExtractAllTasksAndSubTasksWithAllFilters(
                source, personIdSet, personNameSet,
                locSet, buSet, originSet, customerSet, customerIdSet, codeSet, 
                customerIncludesBlanks);
            
            // Se há alert ativo, filtra por ele
            if (!string.IsNullOrWhiteSpace(_lastAppliedAlertCaption))
            {
                allItems = FilterItemsByAlertCaption(allItems, _lastAppliedAlertCaption);
            }
            
            count = allItems.Count;
        }
        else
        {
            // Tile específico: aplica critério do tile
            var allItems = ExtractAllTasksAndSubTasksWithAllFilters(...);
            var exprs = ParseCriteria(tile.FilterCriteria);
            
            count = allItems.Count(item =>
            {
                // Verifica se item match os critérios do tile
                // (Theme, Status, Priority)
            });
        }
        
        tile.EntitiesCount = count;
    }
}
```

**REGRA CHAVE**: Side Tiles recebem dados que:
- ✅ TÊM alert filter aplicado (se houver)
- ❌ NÃO TÊM side filter aplicado
- Isso permite que cada tile veja sua própria contagem independente

---

### 4. **COMO SIDE FILTERS ALTERAM ALERT BUTTONS (mas não eles mesmos)**

#### OLD ViewModel - Lógica de Independência:
```csharp
private void UpdateAlertFiltersBasedOnFilteredData(List<APMActionPlan> source)
{
    // source JÁ VEM com side filter aplicado (se houver)
    // source NÃO tem alert filter aplicado
    
    var allItems = ExtractAllTasksAndSubTasksWithAllFilters(
        source, personIdSet, personNameSet,
        locSet, buSet, originSet, customerSet, customerIdSet, codeSet,
        customerIncludesBlanks);
    
    // Calcula métricas
    int longestOverdueDays = allItems
        .Where(t => IsOverdueItem(t))
        .Select(t => t.GetActualDueDays())
        .Max();
    
    int overdue15Count = allItems.Count(t => t.GetActualDueDays() >= 15);
    int highPriorityCount = allItems.Count(t => 
        t.GetActualDueDays() >= 5 && 
        t.Priority == "High");
    
    // Atualiza cada alert button
    foreach (var tile in AlertListOfFilterTile)
    {
        if (tile.Caption.Contains("longest overdue"))
        {
            tile.EntitiesCount = longestOverdueDays.ToString();
            tile.BackColor = ColorByDays(longestOverdueDays);
        }
        else if (tile.Caption.Contains("overdue >= 15"))
        {
            tile.EntitiesCount = overdue15Count.ToString();
            tile.BackColor = ColorByCount(overdue15Count);
        }
        // ... outros alert buttons
    }
    
    // IMPORTANTE: Restaura seleção do alert ativo
    if (!string.IsNullOrWhiteSpace(_lastAppliedAlertCaption))
    {
        var sel = AlertListOfFilterTile
            .FirstOrDefault(x => x.Caption == _lastAppliedAlertCaption);
        SelectedAlertTileBarItem = sel;
    }
}
```

**REGRA CHAVE**: Alert Buttons recebem dados que:
- ✅ TÊM side filter aplicado (se houver)
- ❌ NÃO TÊM alert filter aplicado
- Isso permite que cada alert veja sua própria contagem independente

---

### 5. **SIDE TILES - Programação Completa**

#### Como Side Tiles são Criados:
```csharp
// OLD: FillLeftTileList()
ListOfFilterTile.Add(new TileBarFilters()
{
    Caption = "All",
    Id = 0,
    BackColor = null,
    EntitiesCount = 0,  // Será atualizado por UpdateSideTileCounts
    EntitiesCountVisibility = Visibility.Visible,
    FilterCriteria = "",  // Vazio = sem filtro
    Height = 60,
    width = 230
});

// Tiles por Theme
foreach (var theme in themeList)
{
    ListOfFilterTile.Add(new TileBarFilters()
    {
        Caption = theme.Value,
        Id = theme.IdLookupValue,
        BackColor = theme.HtmlColor,
        ForeColor = null,
        FilterCriteria = $"[Theme] IN ('{theme.Value}')",  // Filtro SQL-like
        EntitiesCount = 0,
        EntitiesCountVisibility = Visibility.Visible,
        Height = 60,
        width = 230
    });
}
```

#### Como Side Tiles são Clicados:
```csharp
private void OnSideTileClicked(object arg)
{
    var filter = arg as TileBarFilters;
    var caption = filter.Caption;
    bool isClickingAll = IsAllCaption(caption);
    
    // LÓGICA ESPECIAL: Click em "All" quando há Alert ativo
    if (isClickingAll && IsAlertActive())
    {
        if (!_pendingAllWhileAlert)
        {
            // Primeiro click: mantém alert, reseta side
            _pendingAllWhileAlert = true;
            _lastAppliedSideCaption = "All";
            RecalculateAllCounts();
            return;
        }
        else
        {
            // Segundo click: reseta TUDO (alert + side)
            _pendingAllWhileAlert = false;
            _lastAppliedSideCaption = "All";
            _lastAppliedAlertCaption = null;
            SelectedAlertTileBarItem = null;
            ResetCustomFiltersState();
            RecalculateAllCounts();
            return;
        }
    }
    
    // Click normal: aplica filtro do tile
    _lastAppliedSideTileFilter = filter;
    _lastAppliedSideCaption = caption;
    RecalculateAllCounts();
}
```

---

### 6. **ALERT BUTTONS - Programação Completa**

#### Como Alert Buttons são Criados:
```csharp
// OLD: Predefinidos no XAML ou código
AlertListOfFilterTile = new ObservableCollection<APMAlertTileBarFilters>
{
    new APMAlertTileBarFilters 
    { 
        Caption = "Longest Overdue", 
        EntitiesCount = "0",
        BackColor = "Green"
    },
    new APMAlertTileBarFilters 
    { 
        Caption = "Overdue >= 15", 
        EntitiesCount = "0",
        BackColor = "Green"
    },
    new APMAlertTileBarFilters 
    { 
        Caption = "High Priority Overdue", 
        EntitiesCount = "0",
        BackColor = "Green"
    },
    // ... status buttons (To Do, In Progress, etc.)
};
```

#### Como Alert Buttons são Clicados:
```csharp
private void OnAlertTileClicked(object obj)
{
    var tile = obj as APMAlertTileBarFilters;
    var caption = tile.Caption;
    
    // Toggle: click no mesmo = desliga
    bool togglingOff = !string.IsNullOrWhiteSpace(_lastAppliedAlertCaption) &&
                      string.Equals(_lastAppliedAlertCaption, caption);
    
    if (togglingOff)
    {
        // Desliga alert
        _lastAppliedAlertCaption = null;
        SelectedAlertTileBarItem = null;
    }
    else
    {
        // Liga alert
        _lastAppliedAlertCaption = caption;
        SelectedAlertTileBarItem = tile;
    }
    
    // IMPORTANTE: RecalculateAllCounts() vai:
    // - Atualizar side tiles (com este alert aplicado)
    // - Atualizar outros alert buttons (sem este alert aplicado)
    // - Aplicar este alert na grid
    RecalculateAllCounts();
}
```

---

## 🎯 O QUE PRECISA SER IMPLEMENTADO NO MODERN UI

### 1. Classe TaskOrSubTaskItem
```csharp
private class TaskOrSubTaskItem
{
    public int DueDays { get; set; }
    public DateTime DueDate { get; set; }
    public int IdLookupStatus { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public string Theme { get; set; }
    public string Location { get; set; }
    public string Responsible { get; set; }
    public string TaskResponsibleDisplayName { get; set; }
    public string ActionPlanCode { get; set; }
    public bool IsTask { get; set; }
    
    public int GetActualDueDays() => DueDate >= DateTime.Now ? 0 : (int)(DateTime.Now - DueDate).TotalDays;
    
    public static TaskOrSubTaskItem FromTask(APMActionPlanTask t) { ... }
    public static TaskOrSubTaskItem FromSubTask(APMActionPlanSubTask st, string code) { ... }
}
```

### 2. ExtractAllTasksAndSubTasksWithAllFilters
```csharp
private List<TaskOrSubTaskItem> ExtractAllTasksAndSubTasksWithAllFilters(
    List<APMActionPlan> plans,
    HashSet<int> personIdSet,
    HashSet<string> personNameSet,
    HashSet<string> locSet,
    HashSet<string> buSet,
    HashSet<string> originSet,
    HashSet<string> customerSet,
    HashSet<int> customerIdSet,
    HashSet<string> codeSet,
    bool customerIncludesBlanks)
{
    // Extrair todas tasks + subtasks
    // Aplicar TODOS os filtros de dropdown
    // Retornar lista flat de TaskOrSubTaskItem
}
```

### 3. BuildBaselineForAlertTiles (COMPLETO)
```csharp
private List<APMActionPlan> BuildBaselineForAlertTiles()
{
    // 1. Pegar dados base (_currentFilteredBase ou cache)
    // 2. Aplicar TODOS os filtros de dropdown
    // 3. Aplicar side filter (se houver)
    // 4. NÃO aplicar alert filter
    // 5. Retornar lista filtrada
}
```

### 4. UpdateSideTileCounts (COMPLETO)
```csharp
private void UpdateSideTileCounts(List<APMActionPlan> source)
{
    // source já vem com alert aplicado
    // Para cada tile:
    //   - Se "All": conta tudo
    //   - Senão: aplica ParseCriteria e conta
    // Atualiza tile.EntitiesCount
}
```

### 5. UpdateAlertFiltersBasedOnFilteredData (COMPLETO)
```csharp
private void UpdateAlertFiltersBasedOnFilteredData(List<APMActionPlan> source)
{
    // source já vem com side filter aplicado
    // Calcula métricas: longestOverdue, overdue15, highPriority, etc.
    // Para cada alert button:
    //   - Atualiza EntitiesCount
    //   - Atualiza BackColor
    // Restaura seleção do alert ativo (se houver)
}
```

### 6. RecalculateAllCounts (REESCREVER)
```csharp
private void RecalculateAllCounts()
{
    // 1. BuildBaselineForAlertTiles()
    // 2. Aplicar filtros independentes para cada tipo de tile
    // 3. UpdateSideTileCounts(dataWithAlert)
    // 4. UpdateAlertFiltersBasedOnFilteredData(dataWithSide)
    // 5. Aplicar todos filtros na grid final
    // 6. Atualizar ActionPlanList/ObservableCollection
}
```

---

## 📋 CHECKLIST DE IMPLEMENTAÇÃO

- [ ] Criar classe `TaskOrSubTaskItem` com métodos FromTask/FromSubTask
- [ ] Implementar `ExtractAllTasksAndSubTasksWithAllFilters`
- [ ] Implementar `BuildBaselineForAlertTiles` (completo)
- [ ] Implementar `UpdateSideTileCounts` (completo)
- [ ] Implementar `UpdateAlertFiltersBasedOnFilteredData` (completo)
- [ ] Reescrever `RecalculateAllCounts` com lógica de independência
- [ ] Implementar `ApplyAlertFilterToData`
- [ ] Implementar `ApplyCustomFilterToData` (se houver custom filters)
- [ ] Implementar `FilterItemsByAlertCaption`
- [ ] Implementar `GetMostOverdueThemeNameFromItems`
- [ ] Implementar `GetMostOverdueResponsibleNameFromItems`
- [ ] Testar independência: click em alert atualiza side tiles
- [ ] Testar independência: click em side tile atualiza alert buttons
- [ ] Testar contagem: tasks + subtasks contadas juntas

---

## ⚠️ ERROS CRÍTICOS NO MODERN UI ATUAL

1. **RecalculateAllCounts não faz nada de útil** - só chama métodos vazios
2. **Contagens não consideram subtasks** - só conta tasks
3. **Não há lógica de independência** - side/alert se afetam mutuamente
4. **BuildBaselineForAlertTiles não existe** - dados base incorretos
5. **UpdateSideTileCounts incompleto** - não aplica alert filter nos dados
6. **UpdateAlertFiltersBasedOnFilteredData não existe** - alert buttons não atualizam
7. **ApplyAlertToPlans incompleto** - falta lógica de "longest overdue", "most theme", etc.

---

## 🎓 CONCEITOS CHAVE

### Independência Cruzada
- **Side Tiles** veem dados COM alert MAS SEM side
- **Alert Buttons** veem dados COM side MAS SEM alert
- Isso permite que cada filtro veja sua contagem independente

### Contagem Tasks + SubTasks
- OLD conta **Tasks E SubTasks juntas** como um único conjunto
- Use `TaskOrSubTaskItem` para unificar
- Método `ExtractAllTasksAndSubTasksWithAllFilters` faz isso

### RecalculateAllCounts é o Orquestrador
- Chamado após QUALQUER mudança (side click, alert click, dropdown change)
- Reconstrói TUDO do zero
- Garante consistência entre todas as contagens

---

**CONCLUSÃO**: O Modern UI está com 30% da lógica implementada. Precisa implementar os métodos de contagem, independência cruzada e RecalculateAllCounts completo para ter comportamento idêntico ao OLD.
