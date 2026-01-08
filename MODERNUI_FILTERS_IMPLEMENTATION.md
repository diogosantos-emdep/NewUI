# Implementação Completa dos Filtros ModernUI - APM Module

## 📋 Resumo das Alterações

Foram atualizados **3 Stored Procedures** para suportar a arquitetura de filtros do ModernUI:

### ✅ 1. `APM_GetActionPlanDetails_WithCounts`
**Responsabilidade:** Devolver lista de Action Plans (colapsados) baseado em filtros aplicados às tasks/subtasks

**Novos Parâmetros:**
```sql
IN _FilterLocation VARCHAR(500),       -- IDs separados por vírgula: '1,2,3'
IN _FilterResponsible VARCHAR(500),    -- IDs separados por vírgula
IN _FilterBusinessUnit VARCHAR(500),   -- IDs separados por vírgula
IN _FilterOrigin VARCHAR(500),         -- IDs separados por vírgula
IN _FilterDepartment VARCHAR(500),     -- IDs separados por vírgula
IN _FilterCustomer VARCHAR(500),       -- IDs separados por vírgula
IN _AlertFilter VARCHAR(50),           -- 'ToDo', 'InProgress', 'Blocked', 'Closed', 'Overdue15', etc.
IN _FilterTheme VARCHAR(50)            -- 'Safety', 'Quality', etc.
```

**Lógica Implementada:**
- ✅ Filtra Action Plans que **TÊM ≥1 task ou subtask** que corresponde aos critérios
- ✅ Action Plans aparecem **colapsados** na UI
- ✅ Removidas contagens (`ThemeAggregates`, `StatusAggregates`) - não necessárias no ModernUI
- ✅ Suporta múltiplos valores nos dropdowns (CSV)

---

### ✅ 2. `APM_GetActionPlanDetailsPT`
**Responsabilidade:** Carregar tasks/subtasks quando utilizador expande um Action Plan

**Novos Parâmetros:**
```sql
IN _FilterLocation VARCHAR(500),
IN _FilterResponsible VARCHAR(500),
IN _FilterBusinessUnit VARCHAR(500),
IN _FilterOrigin VARCHAR(500),
IN _FilterDepartment VARCHAR(500),
IN _FilterCustomer VARCHAR(500),
IN _AlertFilter VARCHAR(50),
IN _FilterTheme VARCHAR(50)
```

**Lógica Hierárquica Implementada:**

#### **RESULT SET 1: Tasks (Parents)**
Mostra task se:
1. **A própria task corresponde aos filtros** OU
2. **Tem ≥1 subtask que corresponde aos filtros** (para manter hierarquia visível)

#### **RESULT SET 2: SubTasks (Children)**
Mostra **APENAS subtasks que correspondem aos filtros**

**Correções DueDays/DueColor:**
```sql
-- DueDays
CASE 
    WHEN lvStatus.Value LIKE '%Done%' THEN DATEDIFF(CloseDate, DueDate)
    ELSE DATEDIFF(CURDATE(), DueDate)
END

-- DueColor
CASE
    WHEN DueDate >= CURDATE() THEN ''              -- Não atrasado
    WHEN DATEDIFF(CURDATE(), DueDate) <= 2 THEN '#008000'  -- 🟢 Verde (0-2 dias)
    WHEN DATEDIFF(CURDATE(), DueDate) <= 7 THEN '#FFFF00'  -- 🟡 Amarelo (3-7 dias)
    WHEN DATEDIFF(CURDATE(), DueDate) > 7 THEN '#FF0000'   -- 🔴 Vermelho (>7 dias)
END
```

---

### ✅ 3. `APM_GetTaskListByIdActionPlan_V2680PT`
**Responsabilidade:** Carregar todas as tasks de um Action Plan (usado em views específicas)

**Alterações:**
- ✅ Mesmos parâmetros e lógica de filtros do `APM_GetActionPlanDetailsPT`
- ✅ Correções DueDays/DueColor aplicadas
- ✅ Filtros hierárquicos (tasks aparecem se elas ou suas subtasks correspondem)

---

## 🎯 Arquitetura de Filtros ModernUI

### **Fluxo de Utilização:**

```
1️⃣ User abre módulo
   └─> Chama: APM_GetActionPlanDetails_WithCounts(filtros)
   └─> SQL: Devolve Action Plans que TÊM tasks/subtasks com os filtros
   └─> UI: Mostra lista colapsada

2️⃣ User seleciona filtro "Theme = Safety"
   └─> Chama: APM_GetActionPlanDetails_WithCounts(_FilterTheme = 'Safety')
   └─> SQL: Devolve apenas APs que têm ≥1 task/subtask com Theme=Safety
   └─> UI: Atualiza lista (colapsada)

3️⃣ User expande Action Plan #123
   └─> Chama: APM_GetActionPlanDetailsPT(123, _FilterTheme = 'Safety')
   └─> SQL Result Set 1: Tasks que são Safety OU têm subtasks Safety
   └─> SQL Result Set 2: Apenas SubTasks com Safety
   └─> UI: Mostra tasks expandidas (filtradas)
```

---

## 🔍 Tipos de Filtros

### **Dropdown Filters (Múltipla Seleção):**
- Location
- Responsible
- Business Unit
- Origin
- Department
- Customer

**Formato:** CSV string - `'1,2,3'`  
**Lógica:** `FIND_IN_SET(valor, _FilterLocation) > 0`

---

### **Alert Filters (Apenas um ativo):**
- `'ToDo'` - Status contém "To do"
- `'InProgress'` - Status contém "In Progress"
- `'Blocked'` - Status contém "Blocked"
- `'Closed'` - Status contém "Done" ou "Closed"
- `'Overdue15'` - Atrasado ≥15 dias
- `'HighPriorityOverdue'` - High Priority + Atrasado
- `'LongestOverdue'` - Atrasado (qualquer tempo)
- `'MostOverdueTheme'` - Atrasado (agrupado por Theme)

---

### **Side Filters (Apenas um ativo):**
- Cada Theme: `'Safety'`, `'Quality'`, etc.

**Lógica:** `lvTheme.Value = _FilterTheme`

---

## 📊 Exemplo de Filtros Combinados

### **Cenário:**
Filtro: `Location=1,2` + `Theme=Safety` + `AlertFilter=Overdue15`

**Resultado:**
- **Action Plans:** Apenas os que têm tasks/subtasks que são:
  - De Location 1 ou 2 **E**
  - Com Theme Safety **E**
  - Atrasadas ≥15 dias

- **Tasks expandidas:** 
  - Parent task aparece se **ela própria** ou **suas subtasks** correspondem
  - Subtasks aparecem **apenas** se correspondem aos 3 filtros

---

## ⚠️ Notas Importantes

### **1. Filtros no C# (UI):**
Os parâmetros devem ser passados como:
- Dropdowns: `"1,2,3"` (CSV string) ou `NULL`/`""` se vazio
- Alert/Side: `"Safety"` ou `NULL`/`""` se não selecionado

### **2. Performance:**
- Usa `INNER JOIN` para filtrar (não LEFT JOIN)
- Usa tabela temporária `FilteredSubTasks` no `APM_GetActionPlanDetailsPT`
- Evita subqueries desnecessárias

### **3. Manutenção:**
- Se adicionares novos filtros, adiciona-os na secção `WHERE` de cada SP
- Se mudares IDs de Status/Theme, ajusta os `LIKE '%value%'`

---

## 🚀 Próximos Passos (C# UI)

1. **Atualizar chamadas aos SPs:**
   ```csharp
   APM_GetActionPlanDetails_WithCounts(
       period, 
       userId, 
       locationFilter,      // "1,2,3" ou null
       responsibleFilter,   // "10,20" ou null
       businessUnitFilter,  // "5" ou null
       originFilter,        // null
       departmentFilter,    // null
       customerFilter,      // null
       alertFilter,         // "Overdue15" ou null
       themeFilter          // "Safety" ou null
   );
   ```

2. **Ao expandir Action Plan:**
   ```csharp
   APM_GetActionPlanDetailsPT(
       actionPlanId,
       locationFilter,   // Passar os MESMOS filtros
       responsibleFilter,
       businessUnitFilter,
       originFilter,
       departmentFilter,
       customerFilter,
       alertFilter,
       themeFilter
   );
   ```

3. **Remover lógica de contagens:**
   - `ThemeAggregates` e `StatusAggregates` já não existem no output
   - Remover parsing desses campos no C#

---

## ✅ Validação

**Teste este cenário:**
1. Seleciona Theme = "Safety"
2. Verifica que apenas Action Plans com tasks Safety aparecem
3. Expande um Action Plan
4. Verifica que:
   - Tasks Safety aparecem
   - Tasks que TÊM subtasks Safety aparecem (mesmo que a task não seja Safety)
   - Subtasks Safety aparecem
   - Subtasks que NÃO são Safety **NÃO** aparecem

---

**Implementação Completa! 🎉**
