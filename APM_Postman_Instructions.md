APM Service - Postman & cURL Instructions

This small guide shows how to import and test the APM WCF service stored-procedure-backed methods using Postman or cURL.

1) Import collection into Postman
- Open Postman -> Import -> Choose File -> select `postman_APM_tests_collection.json` in the workspace.
- Edit each request and replace the placeholder URL `http://your-host:port/YourServiceFolder/APMService.svc` with your real service URL.
- Adjust SOAPAction header if your service namespace is different from `http://tempuri.org/`.

2) SOAP / cURL examples

- Example: GetActionPlanDetails_WithCounts

Save the SOAP body to `payload_GetActionPlanDetails_WithCounts.xml`, then run:

```bash
curl -X POST \
  "http://your-host:port/YourServiceFolder/APMService.svc" \
  -H "Content-Type: text/xml; charset=utf-8" \
  -H "SOAPAction: \"http://tempuri.org/IAPMService/GetActionPlanDetails_WithCounts\"" \
  -d @payload_GetActionPlanDetails_WithCounts.xml
```

- Example: GetTaskListByIdActionPlan_V2680PT

Save SOAP body to `payload_GetTaskListByIdActionPlan_V2680PT.xml` and run:

```bash
curl -X POST \
  "http://your-host:port/YourServiceFolder/APMService.svc" \
  -H "Content-Type: text/xml; charset=utf-8" \
  -H "SOAPAction: \"http://tempuri.org/IAPMService/GetTaskListByIdActionPlan_V2680PT\"" \
  -d @payload_GetTaskListByIdActionPlan_V2680PT.xml
```

3) Notes on filters and payload values
- Multi-select filters (Location, Responsible, BusinessUnit, Origin, Department, Customer) must be CSV strings like `1,2,3`.
- `alertFilter` and `filterTheme` are single string values, e.g. `Overdue15` or `Safety`.
- If a filter should be ignored, send empty element or omit the element depending on your SOAP client; safe option: include the element with empty body `<filterDepartment></filterDepartment>`.

4) How to find the correct SOAPAction / namespace
- If you can access the service WSDL, open `http://your-host:port/YourServiceFolder/APMService.svc?wsdl` and check the targetNamespace and soapAction for each operation.
- If running locally in IIS/VS, the WCF Test Client can help discover method signatures and namespaces.

5) Troubleshooting
- If you get 500 errors: inspect the response XML — service may be throwing a FaultException. Check server logs.
- If authentication is required adjust Postman to use Basic Auth or Windows Auth accordingly.

---

If you want, I can also:
- Generate sample payload files placed in the workspace (XML files) for each request.
- Attempt a live POST from this environment (needs the service host reachable from here).
