# 🔧 DEBUG - NullReferenceException Modern UI

## 📋 Problema Original
```
System.NullReferenceException
Message=A referência de objecto não foi definida como uma instância de um objecto.
```

## ✅ Correções Aplicadas

### 1️⃣ **Construtor ActionPlansModernViewModel**
**Antes:**
```csharp
public ActionPlansModernViewModel()
{
    _apmService = new APMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
    // ❌ Crash se GeosApplication.Instance ou ApplicationSettings for null
}
```

**Depois:**
```csharp
public ActionPlansModernViewModel()
{
    try
    {
        // ✅ Validações completas
        if (GeosApplication.Instance == null)
            throw new InvalidOperationException("GeosApplication.Instance is null");
        
        if (GeosApplication.Instance.ApplicationSettings == null)
            throw new InvalidOperationException("ApplicationSettings is null");
        
        if (!GeosApplication.Instance.ApplicationSettings.ContainsKey("ServicePath"))
            throw new InvalidOperationException("ServicePath not found");
        
        string servicePath = GeosApplication.Instance.ApplicationSettings["ServicePath"]?.ToString();
        if (string.IsNullOrEmpty(servicePath))
            throw new InvalidOperationException("ServicePath is empty");

        _apmService = new APMServiceController(servicePath);
        // ... resto do código
    }
    catch (Exception ex)
    {
        GeosApplication.Instance?.Logger?.Log($"CRITICAL: Constructor failed: {ex.Message}", ...);
        throw; // Re-throw para o chamador saber
    }
}
```

### 2️⃣ **LoadActionPlansPageAsync - Período e UserId**
**Antes:**
```csharp
string period = DateTime.Now.Year.ToString();
int userId = GeosApplication.Instance.ActiveUser.IdUser;
// ❌ Crash se ActiveUser for null
// ❌ Não usa APMCommon.Instance.SelectedPeriod (inconsistente com código original)
```

**Depois:**
```csharp
// ✅ Validações
if (GeosApplication.Instance == null) return;
if (GeosApplication.Instance.ActiveUser == null) return;

// ✅ Usar APMCommon.Instance.SelectedPeriod (igual ao código original)
string period;
if (APMCommon.Instance?.SelectedPeriod != null && APMCommon.Instance.SelectedPeriod.Count > 0)
{
    var selectedYear = APMCommon.Instance.SelectedPeriod.Cast<long>().FirstOrDefault();
    period = selectedYear.ToString();
}
else
{
    period = DateTime.Now.Year.ToString(); // Fallback
}

int userId = GeosApplication.Instance.ActiveUser.IdUser;
```

### 3️⃣ **MapToDto - Proteções Null**
**Antes:**
```csharp
private ActionPlanModernDto MapToDto(APMActionPlan entity)
{
    return new ActionPlanModernDto
    {
        Code = entity.Code, // ❌ Crash se Code for null
        // ...
    };
}
```

**Depois:**
```csharp
private ActionPlanModernDto MapToDto(APMActionPlan entity)
{
    if (entity == null) return null;
    
    try
    {
        return new ActionPlanModernDto
        {
            Code = entity.Code ?? string.Empty, // ✅ Proteção null
            Title = entity.Description ?? string.Empty,
            Responsible = entity.FullName ?? string.Empty,
            // ... todos os campos com ?? operators
        };
    }
    catch (Exception ex)
    {
        GeosApplication.Instance.Logger?.Log($"MapToDto Exception: {ex.Message}", ...);
        return null;
    }
}
```

### 4️⃣ **NavigateActionPlansModernView - Validações Antecipadas**
**Antes:**
```csharp
private async void NavigateActionPlansModernView()
{
    try
    {
        var modernViewModel = new ActionPlansModernViewModel(); // ❌ Erro no construtor não é capturado
    }
    catch (Exception ex) { }
}
```

**Depois:**
```csharp
private async void NavigateActionPlansModernView()
{
    try
    {
        // ✅ Validações ANTES de criar ViewModel
        if (GeosApplication.Instance == null)
            throw new InvalidOperationException("GeosApplication.Instance is null");
        
        if (GeosApplication.Instance.ApplicationSettings == null)
            throw new InvalidOperationException("ApplicationSettings is null");
        
        GeosApplication.Instance.Logger.Log("Creating ActionPlansModernViewModel...", ...);
        var modernViewModel = new ActionPlansModernViewModel();
        
        // ... resto do código com logs em cada passo
    }
    catch (Exception ex)
    {
        // ✅ Mostra stacktrace completo
        GeosApplication.Instance?.Logger?.Log($"{ex.Message}\nStackTrace: {ex.StackTrace}", ...);
        CustomMessageBox.Show($"Erro:\n\n{ex.Message}\n\nVer logs para detalhes.", ...);
    }
}
```

### 5️⃣ **Using APMCommon**
**Adicionado:**
```csharp
using Emdep.Geos.Modules.APM.CommonClasses;
```

## 🧪 Como Testar

### 1. **Build Clean**
```powershell
# Rebuild solução
cd "c:\Users\diogo.santos\Desktop\tortoise\Src"
msbuild Emdep.Geos.sln /t:Rebuild /p:Configuration=Debug
```

### 2. **Executar com Logs**
1. Abrir GEOS Workbench
2. Ir ao módulo APM
3. Clicar "Action Plans (Modern UI)"
4. **VER LOGS** (ficheiro de log ou output window)

### 3. **Pontos de Verificação nos Logs**
Deves ver esta sequência:
```
[INFO] Method NavigateActionPlansModernView - START
[INFO] Creating ActionPlansModernView...
[INFO] Creating ActionPlansModernViewModel...
[INFO] ActionPlansModernViewModel created successfully
[INFO] Setting DataContext...
[INFO] Navigating to view...
[INFO] Calling InitAsync()...
[INFO] ActionPlansModernViewModel.InitAsync() - Starting AUTO-LOAD...
[INFO] Loading Action Plans: period=2025, userId=123
[INFO] Loaded page 1, 50 items
[INFO] Loaded page 2, 50 items
...
[INFO] Auto-load completed: 150 total Action Plans loaded
[INFO] Method NavigateActionPlansModernView - SUCCESS
```

### 4. **Se Erro Persistir, Verificar:**

#### ❌ **Erro: "GeosApplication.Instance is null"**
**Causa:** Aplicação não foi inicializada corretamente
**Solução:** Verificar ordem de startup do módulo APM

#### ❌ **Erro: "ServicePath not found"**
**Causa:** Configuração ApplicationSettings não tem ServicePath
**Solução:** Verificar ficheiro `ApplicationSettings.config`:
```xml
<add key="ServicePath" value="localhost:6699" />
```

#### ❌ **Erro: "ActiveUser is null"**
**Causa:** Utilizador não fez login
**Solução:** Garantir que login foi feito antes de abrir APM module

#### ❌ **Erro: "GetActionPlanDetails_V2680 returned null"**
**Causa:** Serviço não devolveu dados
**Solução:** 
- Verificar se serviço está a correr
- Verificar se utilizador tem dados para o período selecionado
- Verificar logs do serviço

## 📝 Logs Críticos Adicionados

### Construtor
- ✅ "ActionPlansModernViewModel created successfully"
- ❌ "CRITICAL: ActionPlansModernViewModel constructor failed: [erro]"

### Navegação
- ✅ "Method NavigateActionPlansModernView - START"
- ✅ "Creating ActionPlansModernViewModel..."
- ✅ "Method NavigateActionPlansModernView - SUCCESS"
- ❌ "Error in NavigateActionPlansModernView: [erro]"

### Load Data
- ✅ "Loading Action Plans: period=X, userId=Y"
- ✅ "Loaded page N, X items"
- ❌ "GeosApplication.Instance is null"
- ❌ "ActiveUser is null"
- ❌ "GetActionPlanDetails_V2680 returned null"

### Mapeamento
- ❌ "MapToDto: entity is null"
- ❌ "MapToDto Exception for IdActionPlan=X: [erro]"

## 🎯 Próximos Passos

1. **Executar aplicação**
2. **Capturar logs completos** (desde "START" até "SUCCESS" ou erro)
3. **Se erro persistir:**
   - Copiar **TODA a mensagem de erro** (incluindo stack trace)
   - Copiar **TODOS os logs** desde o início
   - Enviar para análise

## 💡 Dicas de Debug

### Ver Stack Trace Completo
O erro agora mostra stacktrace na messagebox:
```
Erro ao carregar Action Plans (Modern):

[mensagem do erro]

Ver logs para detalhes.
```

### Logs Detalhados
Os logs agora incluem:
- Cada passo da navegação
- Valores de período e userId
- Resultado de cada página carregada
- Erros de mapeamento individuais

### Breakpoints Sugeridos (se usar debugger)
1. `ActionPlansModernViewModel()` - linha do construtor
2. `NavigateActionPlansModernView()` - primeira linha
3. `LoadActionPlansPageAsync()` - linha das validações
4. `MapToDto()` - primeira linha

---

**Resultado Esperado:**
- ✅ Sem NullReferenceException
- ✅ Grid carrega dados automaticamente
- ✅ Logs claros mostram progresso
- ✅ Mensagens de erro detalhadas se algo falhar
