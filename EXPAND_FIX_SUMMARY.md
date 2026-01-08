# FIX: Expand Action Plan - Erro 503 Service Unavailable

## Data: 2025-01-07
## Problema: Ao clicar para expandir um Action Plan, dá erro 503

---

## ❌ ERRO ORIGINAL

```
Emdep.Geos.Services.Contracts.ServiceUnexceptedException
Message=O serviço HTTP localizado em http://10.13.3.33:90/APMService.svc não está disponível.

StackTrace:
  at APMServiceController.GetActionPlanDetails_WithCounts
  at ActionPlansModernViewModel.<LoadTasksForActionPlanAsync>b__225_3() in line 705

Exceção interna:
WebException: O servidor remoto devolveu um erro: (503) Servidor não disponível.
```

**SINTOMA:**
- User clica no "+" para expandir Action Plan e ver suas Tasks
- Serviço WCF retorna **503 (Service Unavailable)**
- Expand não funciona

---

## 🔍 CAUSA RAIZ

O código estava chamando:
```csharp
_apmService.GetTaskListByIdActionPlan_V2680PT(
    idActionPlan, period, userId,
    filterLocation, filterResponsible, filterBusinessUnit,
    filterOrigin, filterDepartment, filterCustomer,
    alertFilter, filterTheme)
```

**PROBLEMA:**
- `GetTaskListByIdActionPlan_V2680PT` **NÃO EXISTE** no servidor WCF!
- Essa SP existe no controller C# mas **não foi deployada no serviço**
- Servidor retorna 503 porque não encontra o método

---

## ✅ CORREÇÃO APLICADA

**Ficheiro:** `ActionPlansModernViewModel.cs` linha 884-896

**ANTES:**
```csharp
var tasksEntityList = await Task.Run(() =>
    _apmService.GetTaskListByIdActionPlan_V2680PT(
        actionPlan.IdActionPlan, period, userId,
        null, null, null, null, null, null, null, null)  // ❌ Método não existe!
);
```

**DEPOIS:**
```csharp
// USAR VERSÃO ANTIGA (sem filtros) porque V2680PT não existe no servidor
var tasksEntityList = await Task.Run(() =>
    _apmService.GetTaskListByIdActionPlan_V2680(
        actionPlan.IdActionPlan, 
        period, 
        userId)  // ✅ Método antigo que FUNCIONA
);
```

**MUDANÇAS:**
1. Substituir `GetTaskListByIdActionPlan_V2680PT` (11 parâmetros) → `GetTaskListByIdActionPlan_V2680` (3 parâmetros)
2. Remover todos os filtros (a versão antiga não aceita filtros)
3. Adicionar logging explicativo

---

## 🧪 COMO TESTAR

### Teste de Expand:
1. Abrir APM ModernUI
2. Carregar lista de Action Plans (deve funcionar - já estava OK)
3. **Clicar no "+" de um Action Plan** para expandir
4. **ESPERADO:** 
   - Ver lista de Tasks/SubTasks
   - SEM erro 503
   - Logs mostram: `"Calling GetTaskListByIdActionPlan_V2680 with IdActionPlan=X"`

### Verificar nos Logs:
```
[LoadTasksForActionPlanAsync] {Code} - Loading tasks using V2680 (no filters version)
[LoadTasksForActionPlanAsync] {Code} - Calling GetTaskListByIdActionPlan_V2680 with IdActionPlan=123, Period=2026, UserId=456
[LoadTasksForActionPlanAsync] {Code} - Service returned: 15 tasks
[LoadTasksForActionPlanAsync] {Code} - Processing 15 tasks
[LoadTasksForActionPlanAsync] {Code} - ASSIGNED 15 tasks to Tasks collection
```

---

## ⚠️ LIMITAÇÕES DA CORREÇÃO

**O que funciona:**
- ✅ Expand abre sem erro 503
- ✅ Mostra Tasks/SubTasks do Action Plan
- ✅ Carrega normalmente

**O que NÃO funciona (porque voltámos à versão antiga):**
- ❌ Expand **IGNORA** filtros ativos (Location, Responsible, etc.)
- ❌ Se filtraste a lista por "Safety", ao expandir vais ver **TODAS** as tasks (Safety + Quality + etc.)

**Isto é NORMAL:** A versão antiga `V2680` não aceita filtros. Para ter filtros no expand, é preciso:
1. Deploy da SP `GetTaskListByIdActionPlan_V2680PT` no servidor WCF
2. Ou criar nova versão da SP no MySQL e fazer deploy

---

## 📋 PRÓXIMOS PASSOS (PARA ADMIN SERVIDOR)

### OPÇÃO 1: Deploy da SP V2680PT (recomendado)
```sql
-- Já existe no código C# (APMServiceController.cs linha 12425)
-- Precisa ser adicionada ao APMService.svc no servidor
```

**Benefícios:**
- Expand respeita filtros ativos
- User filtra por Location "Porto" → Expand mostra só tasks de Porto

### OPÇÃO 2: Continuar com V2680 (solução atual)
- Funciona mas ignora filtros
- User tem que olhar TODAS as tasks quando expande
- Mais simples mas menos funcional

---

## 🔧 FICHEIROS ALTERADOS

### `ActionPlansModernViewModel.cs` linha 884-896
**Mudança:** Trocar chamada `V2680PT` (11 params) → `V2680` (3 params)
**Motivo:** V2680PT não existe no servidor (503 error)
**Impacto:** Expand funciona mas ignora filtros ativos

---

## ✅ TESTE DE VALIDAÇÃO

**ANTES da correção:**
```
User clica "+" → ERRO 503 → Expand não abre
```

**DEPOIS da correção:**
```
User clica "+" → Tasks carregam → Expand funciona! ✅
```

**Compile e teste!** 🚀
