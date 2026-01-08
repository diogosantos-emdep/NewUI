# Postman Test Requests - APM Stored Procedures

## 📌 Objetivo
Testar os 3 stored procedures para verificar que retornam dados corretos ANTES de mexer na aplicação.

---

## 🧪 Teste 1: GetActionPlanDetails_WithCounts (Lista de Action Plans)

### Request SOAP (Sem Filtros)

**URL:** `http://your-server:port/ServiceFolder/APMService.svc`

**Headers:**
- `Content-Type: text/xml; charset=utf-8`
- `SOAPAction: "http://tempuri.org/IAPMService/GetActionPlanDetails_WithCounts"`

**Body:**
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <GetActionPlanDetails_WithCounts xmlns="http://tempuri.org/">
         <selectedPeriod>2025</selectedPeriod>
         <idUser>123</idUser>
         <filterLocation></filterLocation>
         <filterResponsible></filterResponsible>
         <filterBusinessUnit></filterBusinessUnit>
         <filterOrigin></filterOrigin>
         <filterDepartment></filterDepartment>
         <filterCustomer></filterCustomer>
         <alertFilter></alertFilter>
         <filterTheme></filterTheme>
      </GetActionPlanDetails_WithCounts>
   </soap:Body>
</soap:Envelope>
```

**Resposta Esperada:**
- Lista de Action Plans (XML com elementos `<a:APMActionPlanModern>`)
- Cada Action Plan tem: `IdActionPlan`, `Code`, `Description`, `Location`, `Responsible`, etc.

**Notas:**
- Se retornar vazio, trocar `idUser` por um válido (ver query abaixo)
- Anotar um `IdActionPlan` da resposta para usar no Teste 2

---

### Request SOAP (Com Filtro Theme=Safety)

**Body:**
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <GetActionPlanDetails_WithCounts xmlns="http://tempuri.org/">
         <selectedPeriod>2025</selectedPeriod>
         <idUser>123</idUser>
         <filterLocation></filterLocation>
         <filterResponsible></filterResponsible>
         <filterBusinessUnit></filterBusinessUnit>
         <filterOrigin></filterOrigin>
         <filterDepartment></filterDepartment>
         <filterCustomer></filterCustomer>
         <alertFilter></alertFilter>
         <filterTheme>Safety</filterTheme>
      </GetActionPlanDetails_WithCounts>
   </soap:Body>
</soap:Envelope>
```

**Teste de Validação:**
- Comparar com resultado SEM filtro
- Deve retornar MENOS Action Plans (só os que têm tasks com Theme=Safety)

---

## 🧪 Teste 2: GetTaskListByIdActionPlan_V2680PT (Tasks de um Action Plan)

### Request SOAP (Sem Filtros)

**Body:**
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <GetTaskListByIdActionPlan_V2680PT xmlns="http://tempuri.org/">
         <idActionPlan>123</idActionPlan>
         <selectedPeriod>2025</selectedPeriod>
         <idUser>123</idUser>
         <filterLocation></filterLocation>
         <filterResponsible></filterResponsible>
         <filterBusinessUnit></filterBusinessUnit>
         <filterOrigin></filterOrigin>
         <filterDepartment></filterDepartment>
         <filterCustomer></filterCustomer>
         <alertFilter></alertFilter>
         <filterTheme></filterTheme>
      </GetTaskListByIdActionPlan_V2680PT>
   </soap:Body>
</soap:Envelope>
```

**Substituir:**
- `<idActionPlan>123</idActionPlan>` → usar ID obtido no Teste 1

**Resposta Esperada:**
- Lista de Tasks com SubTasks
- Cada Task tem: `TaskNumber`, `Description`, `Theme`, `Status`, `SubTaskList`

---

### Request SOAP (Com Filtro Theme=Quality)

**Body:**
```xml
<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
   <soap:Body>
      <GetTaskListByIdActionPlan_V2680PT xmlns="http://tempuri.org/">
         <idActionPlan>123</idActionPlan>
         <selectedPeriod>2025</selectedPeriod>
         <idUser>123</idUser>
         <filterLocation></filterLocation>
         <filterResponsible></filterResponsible>
         <filterBusinessUnit></filterBusinessUnit>
         <filterOrigin></filterOrigin>
         <filterDepartment></filterDepartment>
         <filterCustomer></filterCustomer>
         <alertFilter></alertFilter>
         <filterTheme>Quality</filterTheme>
      </GetTaskListByIdActionPlan_V2680PT>
   </soap:Body>
</soap:Envelope>
```

**Teste de Validação:**
- Comparar com resultado SEM filtro
- Deve retornar APENAS tasks/subtasks onde `Theme = "Quality"`

---

## 🔍 Queries SQL para Preparar Testes

### 1. Encontrar um User válido com permissões

```sql
-- Admin user
SELECT u.IdUser, e.FirstName, e.LastName
FROM emdep_geos.user_permissions up
INNER JOIN emdep_geos.users u ON u.IdUser = up.IdUser
INNER JOIN emdep_geos.employees e ON e.IdUser = u.IdUser
WHERE up.IdPermission = 122  -- Admin
LIMIT 1;
```

### 2. Encontrar um Action Plan com Tasks

```sql
-- Action Plan com tasks e subtasks
SELECT 
    ap.IdActionPlan,
    ap.Code,
    ap.Description,
    COUNT(DISTINCT apt.IdActionPlanTask) as TotalTasks,
    COUNT(DISTINCT aps.IdActionPlanSubTask) as TotalSubTasks
FROM emdep_geos.action_plans ap
LEFT JOIN emdep_geos.action_plan_task apt ON apt.IdActionPlan = ap.IdActionPlan
LEFT JOIN emdep_geos.action_plan_sub_task aps ON aps.IdActionPlanTask = apt.IdActionPlanTask
WHERE ap.InUse = 1
GROUP BY ap.IdActionPlan, ap.Code, ap.Description
HAVING TotalTasks > 0
LIMIT 5;
```

### 3. Ver Themes disponíveis numa Task

```sql
-- Ver tasks de um Action Plan específico com Themes
SELECT 
    apt.IdActionPlanTask,
    apt.TaskNumber,
    apt.Description,
    lv.Value as Theme,
    apt.Status
FROM emdep_geos.action_plan_task apt
LEFT JOIN emdep_geos.lookup_values lv ON lv.IdLookupValue = apt.IdTheme
WHERE apt.IdActionPlan = 123  -- substituir pelo ID real
LIMIT 10;
```

---

## ✅ Checklist de Testes

### Teste 1: Action Plans List

- [ ] **Sem filtros:** Retorna lista de Action Plans
- [ ] **Com Theme=Safety:** Retorna menos APs (só os que têm tasks Safety)
- [ ] **Com Theme=Quality:** Retorna menos APs (só os que têm tasks Quality)
- [ ] **Theme inexistente:** Retorna vazio

### Teste 2: Tasks de um Action Plan

- [ ] **Sem filtros:** Retorna TODAS as tasks e subtasks do AP
- [ ] **Com Theme=Quality:** Retorna só tasks/subtasks Quality
- [ ] **Verificar SubTaskList:** SubTasks vêm dentro de Task.SubTaskList

### Teste 3: Comparação com Classic UI

- [ ] Abrir mesmo Action Plan no Classic UI
- [ ] Comparar número de tasks retornadas
- [ ] Confirmar que filtros SQL funcionam igual ou melhor

---

## 📊 Exemplo de Resultado Esperado

### GetActionPlanDetails_WithCounts (COM filtro Theme=Safety)

```xml
<GetActionPlanDetails_WithCountsResponse>
    <GetActionPlanDetails_WithCountsResult>
        <a:APMActionPlanModern>
            <a:IdActionPlan>456</a:IdActionPlan>
            <a:Code>AP-2025-001</a:Code>
            <a:Description>Safety Improvement Plan</a:Description>
            <a:Location>Factory A</a:Location>
            <a:Responsible>John Doe</a:Responsible>
            <!-- ... outros campos -->
        </a:APMActionPlanModern>
        <!-- Mais Action Plans que têm tasks Safety -->
    </GetActionPlanDetails_WithCountsResult>
</GetActionPlanDetails_WithCountsResponse>
```

### GetTaskListByIdActionPlan_V2680PT (SEM filtros)

```xml
<GetTaskListByIdActionPlan_V2680PTResponse>
    <GetTaskListByIdActionPlan_V2680PTResult>
        <a:APMActionPlanTask>
            <a:TaskNumber>1</a:TaskNumber>
            <a:Description>Install safety equipment</a:Description>
            <a:Theme>Safety</a:Theme>
            <a:Status>In progress</a:Status>
            <a:SubTaskList>
                <a:APMActionPlanSubTask>
                    <a:SubTaskNumber>1.1</a:SubTaskNumber>
                    <a:Description>Order helmets</a:Description>
                    <a:Theme>Safety</a:Theme>
                    <a:Status>To do</a:Status>
                </a:APMActionPlanSubTask>
                <a:APMActionPlanSubTask>
                    <a:SubTaskNumber>1.2</a:SubTaskNumber>
                    <a:Description>Install fire extinguishers</a:Description>
                    <a:Theme>Safety</a:Theme>
                    <a:Status>In progress</a:Status>
                </a:APMActionPlanSubTask>
            </a:SubTaskList>
        </a:APMActionPlanTask>
        <a:APMActionPlanTask>
            <a:TaskNumber>2</a:TaskNumber>
            <a:Description>Quality audit</a:Description>
            <a:Theme>Quality</a:Theme>
            <a:Status>To do</a:Status>
            <a:SubTaskList />
        </a:APMActionPlanTask>
    </GetTaskListByIdActionPlan_V2680PTResult>
</GetTaskListByIdActionPlan_V2680PTResponse>
```

---

## 🚀 Como Usar Este Ficheiro

1. **Executa queries SQL** para obter IDs válidos
2. **Substitui `idUser` e `idActionPlan`** nos exemplos acima
3. **Copia Body XML** e cola no Postman
4. **Envia request** e compara com resultado esperado
5. **Anota problemas** e reporta aqui

Se algum teste falhar, copia a resposta completa (XML) e diz-me qual foi o request que enviaste.
