using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.SRM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHrmService" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(EmployeeEducationQualification))]
    [ServiceKnownType(typeof(EmployeeProfessionalEducation))]
    [ServiceKnownType(typeof(EmployeeContractSituation))]
    [ServiceKnownType(typeof(EmployeeDocument))]
    [ServiceKnownType(typeof(JobDescription))]
    [ServiceKnownType(typeof(ProfessionalTrainingResults))]
    public interface IHrmService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployees();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployee(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Country> GetAllCountries();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EducationQualification> GetAllEducationQualifications();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ContractSituation> GetAllContractSituations();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalCategory> GetAllProfessionalCategories();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee AddEmployee(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployee(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllDepartments();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetAllJobDescriptions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestEmployeeCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CountryRegion> GetAllCountryRegions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeEducationQualification GetEmployeeEducationDocumentFile(String employeeCode, EmployeeEducationQualification employeeEducationQualification);

        [OperationContract]
        //[KnownType(typeof(EmployeeEducationQualification))]
        byte[] GetEmployeeDocumentFile(String employeeCode, object employeeSubObject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesForOrganization();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyHoliday> GetCompanyHolidaysByIdCompany(Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesByIdCompany(Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesByDepartment();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyLeave> GetAllCompanyLeaves();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployeeAndIdCompany(Int32 idEmployee, string idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetAllEmployeesByDepartmentByIdCompany_V2032 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompany(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesForOrganizationByIdCompany(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetCompanyHolidaysBySelectedIdCompany_V2530 instead.")]
        List<CompanyHoliday> GetCompanyHolidaysBySelectedIdCompany(string idCompany, Int64 selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyLeave> GetSelectedIdCompanyLeaves(string idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeLeave AddEmployeeLeave(EmployeeLeave employeeLeave, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use AddEmployeeLeavesFromList_V2045 instead.")]
        List<EmployeeLeave> AddEmployeeLeavesFromList(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeLeave UpdateEmployeeLeave(EmployeeLeave employeeLeave, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetEmployeeLeaveAttachment(EmployeeLeave employeeLeave);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveEmployeeLeaveAttachment(string employeeCode, UInt64 idEmployeeLeave, string fileName, byte[] fileBytes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteEmployeeLeaveAttachment(string employeeCode, UInt64 idEmployeeLeave, string fileName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Department AddDepartment(Department department);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Department UpdateDepartment(Department department);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllDepartmentDetails();

        [OperationContract(Name = "AddEmployeeAttendanceList")]
        [FaultContract(typeof(ServiceException))]
        bool AddEmployeeAttendance(List<EmployeeAttendance> employeeAttendanceList);

        [OperationContract(Name = "AddEmployeeAttendance")]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use AddEmployeeAttendance_V2036 instead.")]
        EmployeeAttendance AddEmployeeAttendance(EmployeeAttendance employeeAttendance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use UpdateEmployeeAttendance_V2036 instead.")]
        bool UpdateEmployeeAttendance(EmployeeAttendance employeeAttendance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteEmployeeAttendance(Int64 idEmployeeAttendance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetUpcomingBirthdaysOfEmployees(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetUpcomingLeavesOfEmployees(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyHoliday AddCompanyHoliday(CompanyHoliday companyHoliday);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyHoliday UpdateCompanyHoliday(CompanyHoliday companyHoliday);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteEmployeeLeave(string employeeCode, UInt64 idEmployeeLeave, EmployeeLeave employeeLeave = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetEmployeesHeadCountByDepartmentByIdCompany(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeesCountByGenderByIdCompany(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ContractSituation> GetEmployeesCountByContractByIdCompany(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetEmployeesCountByIdCompany(string idCompanies, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetLengthOfServiceByDepartment(string idCompanies, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetAllEmployeesForLeaveByIdCompany_V2032 instead.")]
        List<Employee> GetAllEmployeesForLeaveByIdCompany(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        JobDescription AddJobDescription(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        JobDescription UpdateJobDescription(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyWork> GetAllCompanyWorks();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyWork> GetAllCompanyWorksByIdCompany(string idCompanies);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesForDashboard(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContractSituation> GetLatestContractExpirationForDashboard(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesWithAnniversaryDate(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesWithExitDateForDashboard(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetOrganizationHierarchy(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<City> GetAllCitiesByIdCountry(byte idCountry);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyShift> GetAllCompanyShifts(string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanySchedule> GetCompanyScheduleAndCompanyShifts(string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetOrganizationalChartDepartmentArea(string idCompanies, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetITEmployeeDetails(string idCompanies, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetEmployeeExitEmailTemplate();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetTodayBirthdayOfEmployees_V2045 instead.")]
        List<Employee> GetTodayBirthdayOfEmployees(long selectedPeriod = 0, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetUpcomingCompanyHolidays_V2045 instead.")]
        List<CompanyHoliday> GetUpcomingCompanyHolidays(ref List<string> emails, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetUpcomingEmployeeLeaves_V2044 instead.")]
        List<EmployeeLeave> GetUpcomingEmployeeLeaves(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //bool AddEmployeeChangelogs(Int32 idEmployee, List<EmployeeChangelog> employeeChangelogs);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmployeeAttendanceFromExcel(List<EmployeeAttendance> employeeAttendanceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string ReadMailTemplate(string templateName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetBirthdaysOfEmployeesByYear(string idCompanies, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetJobDescriptionById_V2180 instead.")]
        JobDescription GetJobDescriptionById(UInt32 idJobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        bool IsEmployeeEnjoyedAllAnnualLeaves(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift GetEmployeeShiftandDailyHours(Int32 idEmployee, Int32 idCompanyShift, Int32 idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetTodayEmployeeCompanyAnniversaries(long selectedPeriod, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetEmployeeEnjoyedLeaveHours_V2038 instead.")]
        EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetAllCompanyShiftsByIdCompany_V2032 instead.")]
        List<CompanyShift> GetAllCompanyShiftsByIdCompany(string idCompanies = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanySchedule> GetAllCompanySchedulesByIdCompany(string idCompanies = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use AddCompanyShift_V2032 instead.")]
        CompanyShift AddCompanyShift(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use UpdateCompanyShift_V2032 instead.")]
        CompanyShift UpdateCompanyShift(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetEmployeeRelatedCompanyShifts_V2035 instead.")]
        List<CompanyShift> GetEmployeeRelatedCompanyShifts(Int32 idEmployee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift GetCompanyShiftDetailByIdCompanyShift(Int32 idCompanyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetCompanyDetailsByCompanyId_V2600 instead.")]
        Company GetCompanyDetailsByCompanyId(Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EnterpriseGroup> GetAllEnterpriseGroup();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use AddEmpAttendanceWithClockIdFromExcel_V2036 instead.")]
        bool AddEmpAttendanceWithClockIdFromExcel(List<EmployeeAttendance> employeeAttendanceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use GetCompanyDetailsByCompanyIdSelectedPeriod_V2560 instead.")]
        Company GetCompanyDetailsByCompanyIdSelectedPeriod(Int32 idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetJobDescriptionCount(Int32 idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyDepartment> GetCompanyDepartment(Int32 idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> IsEmployeeExists(string firstName, string lastName, Int32 idEmployee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmpDtlByEmpDocumentNumber(string clockIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAuthorizedPlantsByIdUser(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetCompanyScheduleByIdCompany_V2330 instead.")]
        List<CompanySchedule> GetCompanyScheduleByIdCompany(string idCompany);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use IsEmployeeEnjoyedAllAnnualLeaves_V2038 instead.")]
        bool IsEmployeeEnjoyedAllAnnualLeavesSprint60(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use UpdateEmployeeLeave_V2045 instead.")]
        EmployeeLeave UpdateEmployeeLeaveSprint60(EmployeeLeave employeeLeave, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetTodayEmployeeCompanyAnniversaries_V2045 instead.")]
        List<Employee> GetTodayEmployeeCompanyAnniversariesSprint60(long selectedPeriod, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetEmployeeDetailsForLeaveSummary_V2034 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanySetting> GetCompanySettingByIdCompany(Int16 idAppSetting, string idCompany, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetEmployeeByIdEmployee_V2033 instead.")]
        Employee GetEmployeeByIdEmployee_V2031(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use AddEmployee_V2034 instead.")]
        Employee AddEmployee_V2031(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use UpdateEmployee_V2034 instead.")]
        bool UpdateEmployee_V2031(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetLengthOfServiceByDepartment_V2041 instead.")]
        List<Department> GetLengthOfServiceByDepartment_V2031(string idCompanies, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetEmployeesCountByIdCompany_V2033 instead.")]
        List<Company> GetEmployeesCountByIdCompany_V2031(string idCompanies, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesCountByJobPositionByIdCompany_V2041 instead.")]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2031(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetBirthdaysOfEmployeesByYear_V2041 instead.")]
        List<Employee> GetBirthdaysOfEmployeesByYear_V2031(string idCompanies, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesCountByDepartmentAreaByIdCompany_V2041 instead.")]
        List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2031(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesCountByGenderByIdCompany_V2041 instead.")]
        List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2031(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesCountByContractByIdCompany_V2041 instead.")]
        List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2031(string idCompanies, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeeLeavesForDashboard_V2041 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2031(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeeDocumentsExpirationForDashboard_V2041 instead.")]
        List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2031(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesWithExitDateForDashboard_V2041 instead.")]
        List<Employee> GetEmployeesWithExitDateForDashboard_V2031(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetLatestContractExpirationForDashboard_V2041 instead.")]
        List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2031(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetEmployeesWithAnniversaryDate_V2037 instead.")]
        List<Employee> GetEmployeesWithAnniversaryDate_V2031(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetAllEmployeesForOrganizationByIdCompany_V2032 instead.")]

        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2031(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetOrganizationHierarchy_V2033 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2031(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetOrganizationalChartDepartmentArea_V2033 instead.")]
        List<LookupValue> GetOrganizationalChartDepartmentArea_V2031(string idCompanies, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2031(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetIsCompanyAuthorizedPlantsByIdUser(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetIsOrganizationAuthorizedPlantsByIdUser(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetIsLocationAuthorizedPlantsByIdUser(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetAllEmployeesByIdCompany_V2032 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2031(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetAllEmployeesForAttendanceByIdCompany_V2032 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2031(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetSelectedIdCompanyEmployeeAttendance_V2032 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2031(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetEmployeeLeavesBySelectedIdCompany_V2032 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2031(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use AddJobDescription_V2040 instead.")]
        JobDescription AddJobDescription_V2031(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use UpdateJobDescription_V2040 instead.")]
        JobDescription UpdateJobDescription_V2031(JobDescription jobDescription);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetAllJobDescriptions_V2040 instead.")]
        List<JobDescription> GetAllJobDescriptions_V2031();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2032. Use GetEmpDtlByEmpDocNoAndPeriod_V2032 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2031(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAuthorizedPlantsByIdUser_V2031(Int32 idUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetEmployeeLeavesBySelectedIdCompany_V2039 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2032(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetSelectedIdCompanyEmployeeAttendance_V2035 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2032(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetAllEmployeesForOrganizationByIdCompany_V2033 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2032(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use AddCompanyShift_V2035 instead.")]
        CompanyShift AddCompanyShift_V2032(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateCompanyShift_V2035 instead.")]
        CompanyShift UpdateCompanyShift_V2032(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetAllCompanyShiftsByIdCompany_V2035 instead.")]
        List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2032(string idCompanies = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetEmployeeLatestJobDescriptionsByIdEmployee_V2033 instead.")]
        List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee(Int32 idEmployee, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetAllEmployeesForAttendanceByIdCompany_V2034 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2032(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetAllEmployeesForLeaveByIdCompany_V2039 instead.")]
        List<Employee> GetAllEmployeesForLeaveByIdCompany_V2032(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetEmpDtlByEmpDocNoAndPeriod_V2034 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2032(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2033. Use GetAllEmployeesByIdCompany_V2033 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2032(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetAllEmployeesByDepartmentByIdCompany_V2039 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompany_V2032(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetAllEmployeesForOrganizationByIdCompany_V2041 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2033(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2040. Use GetOrganizationHierarchy_V2040 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2033(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesCountByIdCompany_V2041 instead.")]
        List<Company> GetEmployeesCountByIdCompany_V2033(string idCompanies, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetOrganizationalChartDepartmentArea_V2041 instead.")]
        List<LookupValue> GetOrganizationalChartDepartmentArea_V2033(string idCompanies, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetAllEmployeesByIdCompany_V2036 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2033(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeeLatestJobDescriptionsByIdEmployee_V2041 instead.")]
        List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2033(Int32 idEmployee, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2034. Use GetEmployeeByIdEmployee_V2034 instead.")]
        Employee GetEmployeeByIdEmployee_V2033(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use GetEmployeeByIdEmployee_V2035 instead.")]
        Employee GetEmployeeByIdEmployee_V2034(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use AddEmployee_V2036 instead.")]
        Employee AddEmployee_V2034(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2035. Use UpdateEmployee_V2035 instead.")]
        bool UpdateEmployee_V2034(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetEmpDtlByEmpDocNoAndPeriod_V2036 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2034(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetAllEmployeesForAttendanceByIdCompany_V2110 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2034(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetEmployeeDetailsForLeaveSummary_V2039 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2034(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetEmployeeByIdEmployee_V2036 instead.")]
        Employee GetEmployeeByIdEmployee_V2035(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use UpdateEmployee_V2038 instead.")]
        bool UpdateEmployee_V2035(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use AddCompanyShift_V2330 instead.")]
        CompanyShift AddCompanyShift_V2035(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift UpdateCompanyShift_V2035(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetAllCompanyShiftsByIdCompany_V2330 instead.")]
        List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2035(string idCompanies = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyShift> GetEmployeeRelatedCompanyShifts_V2035(Int32 idEmployee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2036. Use GetSelectedIdCompanyEmployeeAttendance_V2036 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2035(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetAllEmployeesForAttendanceByIdCompany_V2037 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2035(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2037. Use GetSelectedIdCompanyEmployeeAttendance_V2037 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2036(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use AddEmployeeAttendance_V2060 instead.")]
        EmployeeAttendance AddEmployeeAttendance_V2036(EmployeeAttendance employeeAttendance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use UpdateEmployeeAttendance_V2170 instead.")]
        bool UpdateEmployeeAttendance_V2036(EmployeeAttendance employeeAttendance);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use AddEmployeeImportAttendance_V2038 instead.")]
        List<EmployeeAttendance> AddEmpAttendanceWithClockIdFromExcel_V2036(List<EmployeeAttendance> employeeAttendanceList);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetEmpDtlByEmpDocNoAndPeriod_V2039 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2036(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetAllEmployeesByIdCompany_V2041 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2036(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateExitEmployeeStatusInActive(DateTime currentDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeShift> GetEmployeeShiftsByIdEmployee(Int32 idEmployee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use GetEmployeeByIdEmployee_V2038 instead.")]
        Employee GetEmployeeByIdEmployee_V2036(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2042. Use AddEmployee_V2042 instead.")]
        Employee AddEmployee_V2036(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use AddEmployee_V2046 instead.")]
        Employee AddEmployee_V2042(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift GetAnnualScheduleByIdCompanyShift(Int32 idCompanyShift, Int32 year);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2300. Use GetExitEmployeeToUpdateStatusInActive_V2300 instead.")]
        List<Employee> GetExitEmployeeToUpdateStatusInActive(DateTime currentDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use IsExistDocumentNumberInAnotherEmployee_V2038 instead.")]
        bool IsExistDocumentNumberInAnotherEmployee(string employeeDocumentNumber, Int32 idEmployee, Int32 employeeDocumentIdType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsExistDocumentNumberInAnotherEmployee_V2038(string employeeDocumentNumber, Int32 idEmployee, Int32 employeeDocumentIdType, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetAllEmployeesForAttendanceByIdCompany_V2039 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2037(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2039. Use GetSelectedIdCompanyEmployeeAttendance_V2039 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2037(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesWithAnniversaryDate_V2041 instead.")]
        List<Employee> GetEmployeesWithAnniversaryDate_V2037(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompaniesDetailByIds(string companyIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeeShifts(string employeeIds);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2042. Use GetEmployeeByIdEmployee_V2042 instead.")]
        Employee GetEmployeeByIdEmployee_V2038(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use GetEmployeeByIdEmployee_V2046 instead.")]
        Employee GetEmployeeByIdEmployee_V2042(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use GetEmployeeEnjoyedLeaveHours_V2050 instead.")]
        EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2038(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use IsEmployeeEnjoyedAllAnnualLeaves_V2050 instead.")]
        bool IsEmployeeEnjoyedAllAnnualLeaves_V2038(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2042. Use UpdateEmployee_V2042 instead.")]
        bool UpdateEmployee_V2038(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use UpdateEmployee_V2046 instead.")]
        bool UpdateEmployee_V2042(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeHasWelcomeMessageBeenReceived(Int32 idEmployee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetEmployeeForWelcomeBoard_V2045 instead.")]
        Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeeDetailsForLeaveSummary_V2041 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2039(string idCompany, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeesForOrganizationChart_V2041 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart(Int32 idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmployeeLeavesBySelectedIdCompany_V2041 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2039(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetAllEmployeesForAttendanceByIdCompany_V2041 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2039(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetSelectedIdCompanyEmployeeAttendance_V2041 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2039(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetAllEmployeesByDepartmentByIdCompany_V2041 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompany_V2039(string idCompany, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetAllEmployeesForLeaveByIdCompany_V2041 instead.")]
        List<Employee> GetAllEmployeesForLeaveByIdCompany_V2039(string idCompany, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContractSituation> GetEmployeeContracts(string idEmployees);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyDepartment> GetNumberOfWorkStationByIdDepartment(string idsDepartment, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyDepartment> GetSizeByIdDepartmentArea(string departmentAreaIds, Int32 idCompany);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetEmpDtlByEmpDocNoAndPeriod_V2041 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2039(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, decimal> GetPlantWorkHours(Int32 idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Double GetCompanySize(Int32 idCompany);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use GetAllJobDescriptions_V2046 instead.")]
        List<JobDescription> GetAllJobDescriptions_V2040();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use UpdateJobDescription_V2046 instead.")]
        JobDescription UpdateJobDescription_V2040(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use AddJobDescription_V2046 instead.")]
        JobDescription AddJobDescription_V2040(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetIsMandatoryNotExistInJobDescriptionParent(Int32 IdJobDescription);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateIsMandatoryInJobDescriptionParent(Int32 IdJobDescription);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2041. Use GetOrganizationHierarchy_V2041 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2040(string idCompany, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanySchedule> GetEmployeeCompanySchedule(Int32 idEmployee, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetEmployeeLeavesBySelectedIdCompany_V2045 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use GetEmployeeDetailsForLeaveSummary_V2050 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetAllEmployeesByDepartmentByIdCompany_V2120 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeCurrentDetail(Int32 idUser, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use GetAllEmployeesByIdCompany_V2046 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetLengthOfServiceByDepartment_V2420 instead.")]
        List<Department> GetLengthOfServiceByDepartment_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeesCountByIdCompany_V2250 instead.")]
        List<Company> GetEmployeesCountByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeesCountByJobPositionByIdCompany_V2250 instead.")]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetBirthdaysOfEmployeesByYear_V2420 instead.")]
        List<Employee> GetBirthdaysOfEmployeesByYear_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeesCountByDepartmentAreaByIdCompany_V2250 instead.")]
        List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeesCountByGenderByIdCompany_V2250 instead.")]
        List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeesCountByContractByIdCompany_V2250 instead.")]
        List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeeLeavesForDashboard_V2420 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeeDocumentsExpirationForDashboard_V2420 instead.")]
        List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesWithExitDateForDashboard_V2420 instead.")]
        List<Employee> GetEmployeesWithExitDateForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetLatestContractExpirationForDashboard_V2420 instead.")]
        List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesWithAnniversaryDate_V2420 instead.")]
        List<Employee> GetEmployeesWithAnniversaryDate_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use GetAllEmployeesForOrganizationByIdCompany_V2050 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use GetOrganizationHierarchy_V2050 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetOrganizationalChartDepartmentArea_V2250 instead.")]
        List<LookupValue> GetOrganizationalChartDepartmentArea_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2046. Use GetEmployeesForOrganizationChart_V2046 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2041(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetAllEmployeesForAttendanceByIdCompany_V2044 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2044. Use GetSelectedIdCompanyEmployeeAttendance_V2044 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetEmployeeLatestJobDescriptionsByIdEmployee_V2045 instead.")]
        List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2041(Int32 idEmployee, Int64 selectedPeriod);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2090. Use GetEmpDtlByEmpDocNoAndPeriod_V2090 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2041(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetAllEmployeesForLeaveByIdCompany_V2120 instead.")]
        List<Employee> GetAllEmployeesForLeaveByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetUpcomingEmployeeLeaves_V2045 instead.")]
        List<EmployeeLeave> GetUpcomingEmployeeLeaves_V2044(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2045. Use GetSelectedIdCompanyEmployeeAttendance_V2045 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2044(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetAllEmployeesForAttendanceByIdCompany_V2060 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2044(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use GetEmployeeLatestJobDescriptionsByIdEmployee_V2260 instead.")]
        List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2045(Int32 idEmployee, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetSelectedIdCompanyEmployeeAttendance_V2060 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2045(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetSelectedIdCompanyEmployeeAttendance_V2460 instead.")]
        List<Employee> GetTodayBirthdayOfEmployees_V2045(long selectedPeriod = 0, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetSelectedIdCompanyEmployeeAttendance_V2330 instead.")]
        List<CompanyHoliday> GetUpcomingCompanyHolidays_V2045(ref List<string> emails, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetUpcomingEmployeeLeaves_V2045(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use GetTodayEmployeeCompanyAnniversariesDetailsNew_V2560 instead.")]
        List<Employee> GetTodayEmployeeCompanyAnniversaries_V2045(long selectedPeriod, DateTime? currentDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetEmployeeForWelcomeBoard_V2480 instead.")]
        Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard_V2045();



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use UpdateEmployeeLeave_V2170 instead.")]
        EmployeeLeave UpdateEmployeeLeave_V2045(EmployeeLeave employeeLeave, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetEmployeeLeavesBySelectedIdCompany_V2110 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2045(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2680. Use GetEmployeeContractExpirations_V2680 instead.")]
        List<EmployeeContractSituation> GetEmployeeContractExpirations(Int32 idCompany, ref List<string> emails, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeContractWarningDate(List<EmployeeContractSituation> employeeContractExpirationList, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use GetAllEmployeesByIdCompany_V2100 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2046(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetAllJobDescriptions_V2046();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        JobDescription UpdateJobDescription_V2046(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        JobDescription AddJobDescription_V2046(JobDescription jobDescription);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use GetEmployeesForOrganizationChart_V2050 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2046(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2120. Use GetEmployeeDetailsForLeaveSummary_V2120 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeAttendanceForNewLeave(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2100. Use GetEmployeesForOrganizationChart_V2100 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2050(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetAllEmployeesForOrganizationByIdCompany_V2250 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetOrganizationHierarchy_V2250 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetEmployeesForJobDescription(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetAllEmployeesShortDetail_V2360 instead.")]
        List<Employee> GetAllEmployeesShortDetail(string idCompany, string SelectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, string firstName, string lastName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2070. Use GetEmployeeAttendanceShortDetail_V2070 instead.")]
        List<EmployeeAttendance> GetEmployeeAttendanceShortDetail(string idCompany, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetAllEmployeesForAttendanceByIdCompany_V2110 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2060(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetSelectedIdCompanyEmployeeAttendance_V2110 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2060(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeAttendanceShortDetail_V2070(string idCompany, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use GetEmpDtlByEmpDocNoAndPeriod_V2370 instead.")]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2090(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetEmployeesForOrganizationChart_V2110 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2100(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetAllEmployeesByIdCompany_V2170 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2100(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalSkill> GetAllProfessionalSkill();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestProfessionalSkillCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 AddProfessionalSkill(ProfessionalSkill professionalSkill);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProfessionalSkill(ProfessionalSkill professionalSkill);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2110(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetAllEmployeesForAttendanceByIdCompany_V2410 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2110(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2110. Use GetEmployeeLeavesBySelectedIdCompany_V2330 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2110(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeesForOrganizationChart_V2250 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2110(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetAllEmployeesByDepartmentByIdCompany_V2410 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompany_V2120(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetAllEmployeesForLeaveByIdCompany_V2410 instead.")]
        List<Employee> GetAllEmployeesForLeaveByIdCompany_V2120(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestProfessionalObjectiveCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalObjective> GetProfessionalObjectives();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProfessionalObjective AddProfessionalObjective(ProfessionalObjective professionalObjective);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProfessionalObjective(UInt64 IdProfessionalObjective, ProfessionalObjective professionalObjective);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2120(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2120(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalTask> GetAllProfessionalTask();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestProfessionalTaskCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProfessionalTask GetProfessionalTaskDetailsById(UInt64 IdProfessionalTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalObjective> GetProfessionalObjectives_ForDDL();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalSkill> GetProfessionalSkillsForSelection();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProfessionalTask(ProfessionalTask professionalTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        UInt64 AddProfessionalTask(ProfessionalTask professionalTask);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalTraining> GetAllProfessionalTraining();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeleteProfessionalTraining(UInt64 IdProfessionalTraining);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetEmployeeHolidays_V2170 instead.")]
        List<EmployeeHoliday> GetEmployeeHolidays(Int32 idCompany, DateTime? currentDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use IsEmployeeEnjoyedAllAnnualLeaves_V2140 instead.")]
        bool IsEmployeeEnjoyedAllAnnualLeaves_V2050(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2140. Use GetEmployeeEnjoyedLeaveHours_V2140 instead.")]
        EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2050(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2140(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsEmployeeEnjoyedAllAnnualLeaves_V2140(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany);


        //Add
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use AddProfessionalTraining_V2180 instead.")]
        ProfessionalTraining AddProfessionalTraining(ProfessionalTraining professionalTraining);



        //Get new training code
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestProfessionalTrainingCode();



        //delete
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Is_Delete_Professional_Training(UInt64 IdProfessionalTraining);



        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use UpdateProfessionalTraining_V2180 instead.")]
        bool UpdateProfessionalTraining(ProfessionalTraining professionalTraining);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeShortDetailForMail GetJobDecriptionInformation(Int32 idJobDescription);

        //for task GEOS2-2844
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use GetEmployeeByIdEmployee_V2150 instead.")]
        Employee GetEmployeeByIdEmployee_V2046(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use UpdateEmployee_V2150 instead.")]
        bool UpdateEmployee_V2046(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2150. Use AddEmployee_V2150 instead.")]
        Employee AddEmployee_V2046(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use AddEmployee_V2160 instead.")]
        Employee AddEmployee_V2150(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use GetEmployeeByIdEmployee_V2160 instead.")]
        Employee GetEmployeeByIdEmployee_V2150(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2160. Use UpdateEmployee_V2160 instead.")]
        bool UpdateEmployee_V2150(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetNoLongerTrainer(Int32 IdEmployee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployee_V2160(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use UpdateEmployee_V2170 instead.")]
        bool UpdateEmployee_V2160(Employee employee);


        #region GEOS2-2607 Update Leave Summary Grid View Screen
        [ObsoleteAttribute("This method will be removed after version V2170. Use GetEmployeeDetailsForLeaveSummary_V2170 instead.")]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2120(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeeDetailsForLeaveSummary_V2420 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2170(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        #endregion


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetAllEmployeesByIdCompany_V2180 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2170(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use AddEmployeeLeavesFromList_V2170 instead.")]
        List<EmployeeLeave> AddEmployeeLeavesFromList_V2045(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0);


        //[GEOS2-3095]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use AddEmployeeLeavesFromList_V2340 instead.")]
        List<EmployeeLeave> AddEmployeeLeavesFromList_V2170(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use AddEmployeeAttendance_V2170 instead.")]
        EmployeeAttendance AddEmployeeAttendance_V2060(EmployeeAttendance employeeAttendance);


        //[GEOS2-3095]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use AddEmployeeAttendance_V2410 instead.")]
        EmployeeAttendance AddEmployeeAttendance_V2170(EmployeeAttendance employeeAttendance);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use UpdateEmployeeLeave_V2340 instead.")]
        EmployeeLeave UpdateEmployeeLeave_V2170(EmployeeLeave employeeLeave, long selectedPeriod);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeAttendance_V2170(EmployeeAttendance employeeAttendance);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2170. Use AddEmployeeImportAttendance_V2170 instead.")]
        List<EmployeeAttendance> AddEmployeeImportAttendance_V2038(List<EmployeeAttendance> employeeAttendanceList);

        //[GEOS2-3095]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2350. Use AddEmployeeImportAttendance_V2350 instead.")]
        List<EmployeeAttendance> AddEmployeeImportAttendance_V2170(List<EmployeeAttendance> employeeAttendanceList);

        //[GEOS2-2994]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Int64 UpdateExitEmployeeStatusActive(DateTime currentDate);


        #region GEOS2-3093 Update table2. Show new leave columns and all employees in the Automatic report number 11 email

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetEmployeeHolidays_V2250 instead.")]
        List<EmployeeHoliday> GetEmployeeHolidays_V2180(Int32 idCompany, DateTime? currentDate = null);

        #endregion

        //[GEOS2-3086]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalSkill> GetAllProfessionalSkillsForTraining();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetEmployeeJoinOrLeaveDetail_V2180 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail(DateTime curDate);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetEmployeeJoinOrLeaveDetail_V2200 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2180(DateTime curDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2180. Use GetProfessionalTrainingDetailsById_V2180 instead.")]
        ProfessionalTraining GetProfessionalTrainingDetailsById(UInt64 IdProfessionalTask);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetJobDescriptionById_V2180(string idJobDescription);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetITEmployeeDetails_V2180(string idCompanies, Int64 selectedPeriod, string IdJobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetAllEmployeesByIdCompany_V2250 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2180(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[GEOS2-3087]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2190. Use GetProfessionalTrainingDetailsById_V2190 instead.")]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2180(UInt64 IdProfessionalTask);




        //[GEOS2-2846]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use GetProfessionalTrainingDetailsById_V2230 instead.")]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2190(UInt64 IdProfessionalTask);

        //[GEOS2-3087]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2190. Use UpdateProfessionalTraining_V2190 instead.")]
        bool UpdateProfessionalTraining_V2180(ProfessionalTraining professionalTraining);



        //[GEOS2-3086]
        //Add
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use AddProfessionalTraining_V2200 instead.")]
        ProfessionalTraining AddProfessionalTraining_V2180(ProfessionalTraining professionalTraining);



        //[GEOS2-3317]
        //Add
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2210. Use AddProfessionalTraining_V2210 instead.")]
        ProfessionalTraining AddProfessionalTraining_V2200(ProfessionalTraining professionalTraining);


        //[GEOS2-2846]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use UpdateProfessionalTraining_V2200 instead.")]
        bool UpdateProfessionalTraining_V2190(ProfessionalTraining professionalTraining);



        //[GEOS2-3317]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2210. Use UpdateProfessionalTraining_V2210 instead.")]
        bool UpdateProfessionalTraining_V2200(ProfessionalTraining professionalTraining);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetAllTrainers_V2200 instead.")]
        List<Employee> GetAllTrainers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetAllResponsibles_V2200 instead.")]
        List<Employee> GetAllResponsibles();


        //[GEOS2-3317]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllTrainers_V2200();

        //[GEOS2-3317]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetAllResponsibles_V2200 instead.")]
        List<Employee> GetAllResponsibles_V2200();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetEmployeeJoinOrLeaveDetail_V2320 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2200(DateTime curDate);


        //[GEOS2-3317]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2200. Use GetAllEmployeesWithoutInactiveStatus_V2200 instead.")]
        List<Employee> GetAllEmployeesWithoutInactiveStatus();



        //[GEOS2-3317]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetAllEmployeesWithoutInactiveStatus_V2250 instead.")]
        List<Employee> GetAllEmployeesWithoutInactiveStatus_V2200(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[GEOS2-3453]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2210(UInt64 IdProfessionalTask);



        //[GEOS2-3454]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesForProfessionalTrainingResults(UInt64 IdProfessionalTraining);



        //[GEOS2-3454]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalSkill> GetProfessionalTrainingSkillListForResult(UInt64 IdProfessionalTraining);


        //GEOS2-3453
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProfessionalTrainingResult(List<ProfessionalTrainingResults> trainingResultList);


        [OperationContract]
        //[KnownType(typeof(ProfessionalTrainingResults))]
        byte[] GetEmployeeDocumentFile_V2210(String employeeCode, object employeeSubObject);


        //GEOS2-3454
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProfessionalTraining AddProfessionalTraining_V2210(ProfessionalTraining professionalTraining);

        //[GEOS2-3353]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProfessionalTraining_V2210(ProfessionalTraining professionalTraining);

        //[GEOS2-3456]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use UpdateProfessionalTraining_V2230 instead.")]
        bool UpdateProfessionalTraining_V2220(ProfessionalTraining professionalTraining);


        //[GEOS2-3420]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetEmployeeByIdEmployee_V2320 instead.")]
        Employee GetEmployeeByIdEmployee_V2220(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        //GEOS2-3456
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use AddProfessionalTraining_V2230 instead.")]
        ProfessionalTraining AddProfessionalTraining_V2220(ProfessionalTraining professionalTraining);



        //GEOS2-3502
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use AddProfessionalTraining_V2240 instead.")]
        ProfessionalTraining AddProfessionalTraining_V2230(ProfessionalTraining professionalTraining);

        //[GEOS2-3501]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use UpdateProfessionalTraining_V2240 instead.")]
        bool UpdateProfessionalTraining_V2230(ProfessionalTraining professionalTraining);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use AddEmployee_V2230 instead.")]
        Employee AddEmployee_V2160(Employee employee);

        // [GEOS2-3554]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use AddEmployee_V2260 instead.")]
        Employee AddEmployee_V2230(Employee employee);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2230. Use UpdateEmployee_V2230 instead.")]
        bool UpdateEmployee_V2170(Employee employee);

        // [GEOS2-3554]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2260. Use UpdateEmployee_V2260 instead.")]
        bool UpdateEmployee_V2230(Employee employee);

        //[GEOS2-2848]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2240. Use GetProfessionalTrainingDetailsById_V2240 instead.")]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2230(UInt64 IdProfessionalTask);



        //[GEOS2-3504]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2250. Use GetProfessionalTrainingDetailsById_V2250 instead.")]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2240(UInt64 IdProfessionalTask);


        //GEOS2-2948
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use AddProfessionalTraining_V2500 instead.")]
        ProfessionalTraining AddProfessionalTraining_V2240(ProfessionalTraining professionalTraining);

        //[GEOS2-2849]
        //Update
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use AddProfessionalTraining_V2500 instead.")]
        bool UpdateProfessionalTraining_V2240(ProfessionalTraining professionalTraining);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetAllEmployeesForOrganizationByIdCompany_V2330 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetOrganizationHierarchy_V2330 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetEmployeesCountByIdCompany_V2330 instead.")]
        List<Company> GetEmployeesCountByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetOrganizationalChartDepartmentArea_V2330 instead.")]
        List<LookupValue> GetOrganizationalChartDepartmentArea_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetEmployeesForOrganizationChart_V2330 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2250(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesCountByDepartmentAreaByIdCompany_V2420 instead.")]
        List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesCountByGenderByIdCompany_V2420 instead.")]
        List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesCountByContractByIdCompany_V2250 instead.")]
        List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesWithoutInactiveStatus_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[GEOS2-3637]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use UpdateEmployee_V2390 instead.")]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2250(UInt64 IdProfessionalTask, string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-3638]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeeHolidays_V2420 instead.")]
        List<EmployeeHoliday> GetEmployeeHolidays_V2250(Int32 idCompany, DateTime? currentDate = null);


        // [GEOS2-3562]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use UpdateEmployee_V2320 instead.")]
        bool UpdateEmployee_V2260(Employee employee);

        // [GEOS2-3562]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use AddEmployee_V2480 instead.")]
        Employee AddEmployee_V2260(Employee employee);

        //[GEOS2-3624]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2260(Int32 idEmployee, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use GetEmpDtlByEmailNoAndPeriod_V2370 instead.")]
        List<Employee> GetEmpDtlByEmailNoAndPeriod_V2260(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);

        //[GEOS2-3697]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> AddEmployeeImportLeave(List<EmployeeLeave> employeeLeavesList);

        //[GEOS2-3827]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use GetTravelExpenses_V2301 instead.")]
        List<TravelExpenses> GetTravelExpenses(string Plant);

        //[GEOS2-3757][rdixit][12.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetExitEmployeeToUpdateStatusInActive_V2300(DateTime currentDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2300(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

        //[rdixit][GEOS2-3828][29.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenseStatus> GetAllTravelExpenseStatus();

        //[rdixit][GEOS2-3828][29.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use GetAllWorkflowTransitions_V2370 instead.")]
        List<TravelExpenseWorkflowTransitions> GetAllWorkflowTransitions();

        //[rdixit][GEOS2-3828][29.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2301. Use UpdateTravelExpensesStatus_V2320 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2301(string Plant);

        //[rdixit][GEOS2-3828][29.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetTravelExpenses_V2320 instead.")]
        bool UpdateTravelExpensesStatus_V2301(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate);

        //[rdixit][GEOS2-3829][29.08.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2320. Use GetTravelExpenseChangelogs_V2470 instead.")]
        List<LogEntriesByTravelExpense> GetTravelExpenseChangelogs_V2320(Int64 IdEmployeeExpenseReport);

        //[rdixit][GEOS2-3829][21.09.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use UpdateTravelExpensesStatus_V2320 instead.")]
        bool UpdateTravelExpensesStatus_V2320(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate);
        //shubham[skadam] GEOS2-3842 Employee joining email is not sent since 1 y aprox 26 Sep 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetEmployeeJoinOrLeaveDetail_V2330 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2320(DateTime curDate);

        //[sdeshpande][19.10.2022][GEOS2-3362]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2330(DateTime curDate);

        //[rdixit][GEOS2-3829][26.09.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetTravelExpenses_V2340 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2320(string Plant);
        //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEnumeratedList(Int32 key);


        // [GEOS2-3497]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use UpdateEmployee_V2480 instead.")]
        bool UpdateEmployee_V2320(Employee employee);


        //[GEOS2-3420]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2330. Use GetEmployeeByIdEmployee_V2330 instead.")]
        Employee GetEmployeeByIdEmployee_V2320(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[GEOS2-3420]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetBackupEmployeeDetails(string idsEmployeeJobDescription, string idsDepartment);

        //[GEOS2-3763] [rdixit] [06.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<UserLongTermAbsent> GetLongTermAbsentUsers();

        //[GEOS2-3763] [rdixit] [06.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetOrganizationHRByCompany_V2480 instead.")]
        List<OrganizationHR> GetOrganizationHRBySite(Int32 IdSite);

        //shubham[skadam] GEOS2-3919 HRM - Register different leaves at the same time 11 OCT 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds);

        //[GEOS2-3957][rdixit][07.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2370. Use EmployeeExpenseByExpenseReport_V2370 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport(int IdEmployeeExpenseReport);

        //[GEOS2-3957][rdixit][07.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetEmployeeExpenseImageInBytes_V2340 instead.")]
        List<EmployeeExpensePhotoInfo> GetEmployeeExpenseImageInBytes(int idEmployeeExpense);

        //[GEOS2-3945][rdixit][18.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeWithInactiveBackupEmployees> GetEmployeesWithInactiveBackupEmployeeDetails();
        //[GEOS2-2716][sudhir.jangra][19/10/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift UpdateCompanyShift_V2330(CompanyShift companyShift);

        //[GEOS2-3944][rdixit][19.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeWithBackupEmployeeDetails GetEmployeeWithBackupEmployeeDetails(int IdEmployee, DateTime LeaveStartDate, DateTime LeaveEndDate);

        //[Geos-2618][pjadhav][10/20/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][21.10.2022][GEOS2-3263]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetEmployeeLeavesBySelectedIdCompany_V2410 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-3945][rdixit][31.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OrganizationHR> GetOrganizationHRByCompany(Int32 IdSite);

        //[GEOS2-3846][rdixt][31.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ProfessionalCategory> GetAllProfessionalCategoriesWithParents();

        //[GEOS2-2716][rdixit][31.10.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use GetEmployeeByIdEmployee_V2400 instead.")]
        Employee GetEmployeeByIdEmployee_V2330(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[GEOS2-2716][rdixit][01.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetAllCompanyShiftsByIdCompany_V2340 instead.")]
        List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2330(string idCompanies = null);

        //[GEOS2-2795][rdixit][01.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesForOrganizationByIdCompany_V2420 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-2795][rdixit][04.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOrganizationHierarchy_V2420 instead.")]
        List<JobDescription> GetOrganizationHierarchy_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-2795][rdixit][04.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesCountByIdCompany_V2420 instead.")]
        List<Company> GetEmployeesCountByIdCompany_V2330(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-2795][rdixit][04.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetOrganizationalChartDepartmentArea_V2420 instead.")]
        List<LookupValue> GetOrganizationalChartDepartmentArea_V2330(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-2795][rdixit][04.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2330(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-3958][rdixit][02.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2390. Use GetCompanies_V2390 instead.")]
        List<Company> GetCompanies();

        //[sudhir.jangra][GEOS2-2716][ADD New Service][04/11/2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        CompanyShift AddCompanyShift_V2330(CompanyShift companyShift);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GetIsCompanyShiftAssignedToEmployee(Int32 idCompanyShift);


        //[GEOS2-2716][cpatil][08.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyShift> GetAllCompanyShiftsInUseByIdCompany(string idCompanies = null);

        //[GEOS2-3114][cpatil][10.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanySchedule> GetCompanyScheduleByIdCompany_V2330(string idCompany);

        //Created by shubham[skadam] for GEOS2-3754 HRM - Weekly email plant holidays (#HRM103) - Requested by Top management  https://helpdesk.emdep.com/browse/GEOS2-3754  date 14 11 2022
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetUpcomingCompanyHolidays_V2440 instead.")]
        List<CompanyHoliday> GetUpcomingCompanyHolidays_V2330(ref List<string> emails, DateTime? currentDate = null);

        //[rdixit][GEOS2-3853][14.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetEmployeeDocumentExpiration_V2340 instead.")]
        List<EmployeeDocument> GetEmployeeDocumentExpiration(int idCompany, DateTime CurrentDate);

        //[rdixit][GEOS2-3853][17.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeDocumentWarningDate(List<EmployeeDocument> employeeDocumentExpirationList, DateTime? currentDate = null);


        //[pjadhav][GEOS2-235][17.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeleteEmployeeHolidays(UInt64 IdCompanyHoliday);

        //[GEOS2-3981][sshegaonkar][17.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2340(string idCompanies = null);

        //[rdixit][GEOS2-4022][24.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetTravelExpenses_V2360 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2340(string Plant);

        //[rdixit][GEOS2-4022][24.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTravelExpenses_V2340(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate);

        //[rdixit][GEOS2-4025][28.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetTravelExpenseReportLatestCode();

        //[rdixit][GEOS2-4025][28.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetEmployeesByIdSite_V2460 instead.")]
        List<Employee> GetEmployeesByIdSite(int idCompany);

        //[rdixit][GEOS2-4025][28.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use AddTravelExpenseReport_V2420 instead.")]
        TravelExpenses AddTravelExpenseReport(TravelExpenses travelExpense, int idCreator);

        //[rdixit][GEOS2-3263][28.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use AddEmployeeLeavesFromList_V2410 instead.")]
        List<EmployeeLeave> AddEmployeeLeavesFromList_V2340(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0);

        //[rdixit][GEOS2-3263][28.11.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeLeave UpdateEmployeeLeave_V2340(EmployeeLeave employeeLeave, long selectedPeriod);

        //[GEOS2-3957][rdixit][15.12.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2340. Use GetEmployeeExpenseImageInBytes_V2350 instead.")]
        List<EmployeeExpensePhotoInfo> GetEmployeeExpenseImageInBytes_V2340(int idEmployeeExpense);

        //[rdixit][GEOS2-4086][16.12.2022]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeDocument> GetEmployeeDocumentExpiration_V2340(int idCompany, DateTime CurrentDate);

        //[rdixit][GEOS2-4066][03.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetTravelExpenses_V2410 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2360(string Plant);

        //[rdixit][GEOS2-3943][04.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllTravelExpensewithPermission_V2420 instead.")]
        List<TravelExpenses> GetAllTravelExpensewithPermission_V2360(string Plant, string idDepartment, string idOrganization, int permission);

        //[cpatil][GEOS2-3934][05.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesShortDetail_V2350(string idCompany, string SelectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, string firstName, string lastName);

        //[rdixit][05-01-2023][GEOS2-4055]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2410 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2400(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[GEOS2-3906]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> AddEmployeeImportAttendance_V2350(List<EmployeeAttendance> employeeAttendanceList);

        //[GEOS2-4043]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateIsMandatoryInJobDescriptionParentByIdParent(Int32 IdJobDescription);

        //[Geos-4003][sshegaonkar][11/01/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany_V2360(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-4010][rdixit][20.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeExpensePhotoInfo> GetEmployeeExpenseImageInBytes_V2350(int idEmployeeExpense);
        //[Gulab lakade][GEOS2-4026][20.01.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2410. Use UpdateTravelExpenses_V2410 instead.")]
        bool UpdateTravelExpenses_V2350(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate);
        //Shubham[skadam] GEOS2-4175 Expense report not show proper for  status draft  03 02 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Currency> GetCurrencies_V2360();

        //[rdixit][09.03.2023][GEOS2-4239]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use EmployeeExpenseByExpenseReport_V2400 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2370(int IdEmployeeExpenseReport);

        //[rdixit][15.03.2023][GEOS2-4178]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeMealBudget GetMealExpenseByEmployyeAndCompany(int idCompany, int idEmployee);

        //[cpatil][15.03.2023][GEOS2-3981]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeShift> GetEmployeeShiftsByIdEmployee_V2370(Int32 idEmployee);

        //[cpatil][15.03.2023][GEOS2-3981]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2370(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);

        //[cpatil][15.03.2023][GEOS2-3981]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmpDtlByEmailNoAndPeriod_V2370(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);

        //[rdixit][15.03.2023][GEOS2-4180]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetMealAllowances_V2470 instead.")]
        List<MealAllowance> GetMealAllowances();

        //[rdixit][20.03.2023][GEOS2-4181]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateMealAllowance(List<EmployeeMealBudget> employeeMealBudget);

        //[rdixit][24.03.2023][GEOS2-4017]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GetEmployeeExpenseReportTemplate(string code, Employee selectedEmployee, TravelExpenseStatus status, string comments, string userCompanyEmail);

        //[rdixit][24.03.2023][GEOS2-4017]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenseWorkflowTransitions> GetAllWorkflowTransitions_V2370();

        //[Sudhir.Jangra][GEOS2-4023][19/04/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        CompanyShift AddCompanyShift_V2380(CompanyShift companyShift);

        //[Sudhir.jangra][GEOS2-4036][19/04/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2380(string idCompanies = null);

        //[Sudhir.Jangra][GEOS2-4023][20/04/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift UpdateCompanyShift_V2380(CompanyShift companyShift);

        //[GEOS2-4297][rdixit][08.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetCompanies_V2420 instead.")]
        List<Company> GetCompanies_V2390();

        //[rdixit][GEOS2-4476][22.05.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProfessionalTraining GetProfessionalTrainingDetailsById_V2390(UInt64 IdProfessionalTask, string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[Sudhir.Jangra][GEOS2-4536][01/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany_V2400(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][GEOS2-4285][08.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] HRMGetEmployeesImage(string employeeCode);

        //[rdixit][GEOS2-4515][09.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesCountByJobPositionByIdCompany_V2420 instead.")]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2400(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-2456][rdixit][12.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployee_V2400(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[GEOS2-4438][rdixit][15.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2400. Use EmployeeExpenseByExpenseReport_V2410 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2400(int IdEmployeeExpenseReport);


        //[Sudhir.Jangra][GEOS2-4037][29/06/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetEmployeeByIdEmployee_V2480 instead.")]
        Employee GetEmployeeByIdEmployee_V2410(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[GEOS2-4439][rdixit][03.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use EmployeeExpenseByExpenseReport_V2420 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2410(int IdEmployeeExpenseReport);

        //[GEOS2-4471][rdixit][04.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use UpdateTravelExpenses_V2420 instead.")]
        bool UpdateTravelExpenses_V2410(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList);

        //[rdixit][GEOS2-4472][05.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetEmployeeExpenseReport_V2630 instead.")]
        Dictionary<string, byte[]> GetEmployeeExpenseReport(string plantName, string EmployeeCode, string ExpenseCode);

        //[rdixit][GEOS2-4472][05.07.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetTravelExpenses_V2420 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2410(string Plant);

        //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
        //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
        //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesForAttendanceByIdCompany_V2420 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
        //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeeLeavesBySelectedIdCompany_V2420 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
        //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesForLeaveByIdCompany_V2420 instead.")]
        List<Employee> GetAllEmployeesForLeaveByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
        //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesByDepartmentByIdCompany_V2420 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> AddEmployeeLeavesFromList_V2410(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0);
        //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use AddEmployeeAttendance_V2480 instead.")]
        EmployeeAttendance AddEmployeeAttendance_V2410(EmployeeAttendance employeeAttendance);
        //[Sudhir.Jangra][GEOS2-4038][11/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesForOrganizationChart_V2420 instead.")]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2410(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[Sudhir.Jangra][GEOS2-4686][19/07/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesByIdCompany_V2420 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-4516][rdixit][01.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetLengthOfServiceByDepartment_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[GEOS2-4301][rdixit][07.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenses> GetTravelExpenses_V2420(string Plant);

        //[GEOS2-4301][rdixit][07.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetAllTravelExpensewithPermission_V2470 instead.")]
        List<TravelExpenses> GetAllTravelExpensewithPermission_V2420(string Plant, string idDepartment, string idOrganization, int permission);

        //[GEOS2-4301][rdixit][07.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTravelExpenses_V2420(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList);

        //[GEOS2-4301][rdixit][07.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetCompanies_V2420();

        //[GEOS2-4301][rdixit][07.08.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TravelExpenses AddTravelExpenseReport_V2420(TravelExpenses travelExpense, int idCreator);

        //[pramod.misal][07-08-2023][GEOS2-3362]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2460. Use GetEmployeeJoinOrLeaveDetail_V2460 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2420(DateTime curDate);

        //[rdixit][GEOS2-2466][08.06.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetAllEmployeesByIdCompany_V2470 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use AddEmployeeAttendance_V2420 instead.")]
        EmployeeAttendance AddEmployeeAttendance_V2420(EmployeeAttendance employeeAttendance, byte[] fileInBytes);

        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetSelectedIdCompanyEmployeeAttendance_V2520 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetEmployeeAttendanceAttachment(EmployeeAttendance employeeAttendance);

        //---

        //[pramod.misal][GEOS2-2466][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetEmployeesCountByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeesCountByJobPositionByIdCompany_V2620 instead.")]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetBirthdaysOfEmployeesByYear_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesWithExitDateForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]//
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetEmployeesWithAnniversaryDate_V2620 instead.")]
        List<Employee> GetEmployeesWithAnniversaryDate_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2440. Use GetAllEmployeesForOrganizationByIdCompany_V2440 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetOrganizationHierarchy_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2420(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesByDepartmentByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesForLeaveByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetEmployeeLeavesBySelectedIdCompany_V2520 instead.")]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420 instead.")]
        List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[pramod.misal][GEOS2-2466][08-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetAllEmployeesForAttendanceByIdCompany_V2480 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteEmployeeAttendanceAttachment(string employeeCode, Int64 idEmployeeAttendance, string fileName);

        //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SaveEmployeeAttendanceAttachment(string employeeCode, Int64 idEmployeeAttendance, string fileName, byte[] fileBytes);

        //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateEmployeeAttendance_V2520 instead.")]
        bool UpdateEmployeeAttendance_V2420(EmployeeAttendance employeeAttendance);

        //[rdixit][GEOS2-2466][10-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetOrganizationalChartDepartmentArea_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][GEOS2-2466][10-08-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use GetEmployeeDetailsForLeaveSummary_V2590 instead.")]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use EmployeeExpenseByExpenseReport_V2430 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2420(int IdEmployeeExpenseReport, string sourceCurrency, int sourceCurrencyId,
            string targetCurrency, int targetCurrencyId);

        //[rdixit][22.08.2023][GEOS2-4768]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2420. Use GetEmployeeHolidays_V2420 instead.")]
        List<EmployeeHoliday> GetEmployeeHolidays_V2420(Int32 idCompany, DateTime? currentDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2590. Use UpdateCompany_V2600 instead.")]
        Company UpdateCompany_V2430(Company company);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2430. Use GetAuthorizedPlantsByIdUser_V2480 instead.")]
        List<Company> GetAuthorizedPlantsByIdUser_V2430(Int32 idUser);

        //[rdixit][26.08.2023][GEOS2-4607]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeHoliday> GetEmployeeHolidays_V2430(Int32 idCompany, DateTime? currentDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Expenses> EmployeeExpenseByExpenseReport_V2430(int IdEmployeeExpenseReport, string sourceCurrency, int sourceCurrencyId,
    string targetCurrency, int targetCurrencyId);


        //[pramod.misal][GEOS2-4815][26-09-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetEmployeeTripsBySelectedIdCompany_V2480 instead.")]
        List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2440(string idCompany, Int64 selectedPeriod = 0);

        //[cpatil][GEOS2-4855][02-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetUpcomingCompanyHolidays_V2470 instead.")]
        List<CompanyHoliday> GetUpcomingCompanyHolidays_V2440(ref List<string> emails, DateTime? currentDate = null);

        //[rdixit][GEOS2-4621][10.10.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetAllEmployeesForOrganizationByIdCompany_V2470 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2440(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][GEOS2-4721][11.10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeMealBudget GetMealExpenseByEmployyeAndCompany_V2440(int idCompany, int idCurrencyTo, DateTime conversionDate, int idEmployee);

        //[rdixit][GEOS2-4721][11.10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Expenses> EmployeeExpenseByExpenseReport_V2440(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency);

        //[rdixit][GEOS2-4721][11.10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Currency> GetCurrencies_V2440();

        //rajashri[GEOS2-3693][12-10-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogNewJobDescription> GetLogEntriesForJob_V2440(UInt32 jobid);

        //[Sudhir.jangra][GEOS2-3693]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Notification AddCommonNotification(Notification notification);

        //[rdixit][GEOS2-4721][10.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use EmployeeExpenseByExpenseReport_V2480 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2450(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency);

        //[rajashrit][09-11-2023][GEOS2-4613]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetEmployeeJoinOrLeaveDetail_V2470 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2460(DateTime curDate);
        //[GEOS2-4612]  rajashri
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetTodayBirthdayOfEmployees_V2670 instead.")]
        List<Employee> GetTodayBirthdayOfEmployees_V2460(long selectedPeriod = 0, DateTime? currentDate = null);

        //[pramod.misal][GEOS2-4848][23.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2470. Use GetTravelExpenses_V2470 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2460(string Plant);

        //[pramod.misal][GEOS2-4848][23.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TravelExpenses AddTravelExpenseReport_V2460(TravelExpenses travelExpense, int idCreator);


        //[pramod.misal][GEOS2-4848][23.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTravelExpenses_V2460(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList);


        ////[rajashri][GEOS2-4997][30-11-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesByIdSite_V2460(int idCompany);

        //[pramod.misal][GEOS2-4848][05-12-2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTrips> GetEmployeeTripsBySelectedIdEmpolyee_V2460(int empid);

        //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetAllEmployeesForOrganizationByIdCompany_V2630 instead.")]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2470(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //Shubham[skadam] GEOS2-5139 Add country column with flag in expense reports 18 12 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetTravelExpenses_V2520 instead.")]
        List<TravelExpenses> GetTravelExpenses_V2470(string Plant);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetAllTravelExpensewithPermission_V2520 instead.")]
        List<TravelExpenses> GetAllTravelExpensewithPermission_V2470(string Plant, string idDepartment, string idOrganization, int permission);

        //Shubham[skadam] GEOS2-5138 Add country column with flag in meal allowance 18 12 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<MealAllowance> GetMealAllowances_V2470();
        //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 19 12 2023
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2480. Use GetMealAllowances_V2480 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2470(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use UpdateTravelExpenses_V2520 instead.")]
        bool UpdateTravelExpenses_V2470(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList);

        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByTravelExpense> GetTravelExpenseChangelogs_V2470(Int64 IdEmployeeExpenseReport);

        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByTravelExpense> GetTravelExpenseComments_V2470(Int64 IdEmployeeExpenseReport);

        //[chitra.girigosavi][GEOS2-4824][03.11.2023]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TravelExpenses AddTravelExpenseReport_V2470(TravelExpenses travelExpense, int idCreator, List<LogEntriesByTravelExpense> expenseReportLogList);
        //rajashri [GEOS2-3692]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use UpdateJobDescription_V2530 instead.")]
        JobDescription UpdateJobDescription_V2470(JobDescription jobDescription);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        JobDescription AddJobDescription_V2470(JobDescription jobDescription);

        //[rdixit][27.12.2023][GEOS2-4875][GEOS2-48756]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContact> GetEmployeeContactsByIdEmployee(string idEmployeeList);
        //rajashri GEOS2-4942
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyHoliday> GetUpcomingCompanyHolidays_V2470(ref List<string> emails, DateTime? currentDate = null);

        //[rajashrit][09-11-2023][GEOS2-4613]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetEmployeeJoinOrLeaveDetail_V2530 instead.")]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2470(DateTime curDate);

        //rajashi 09-01-2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetBestRegardsEmployeeData();

        //[rdixit][09.01.2024][GEOS2-5112]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetEmployeeTripsBySelectedIdCompany_V2480 instead.")]
        List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2480(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, UInt32 idUser);
        //rajashri [11-01-2024][GEOS2-5221]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard_V2480();
        #region GEOS2-4816
        //[Sudhir.Jangra][[GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetLatestCodeForTrip_V2480();

        //[sudhir.jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Traveller> GetActiveEmployeesForTraveller_V2480(string IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission);

        //[Sudhir.jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues(byte key);

        //[Sudhir.Jangra][GEOS2-4816]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Destination> GetPlantListForDestination_V2480(UInt32 idUser);

        //[sudhir.jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Destination> GetCustomersForTripDestination_V2480();

        //[sudhir.jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<Currency> GetCurrencyListForTrips_V2480();

        //[Sudhir.Jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips AddEmployeeTripDetails_V2480(EmployeeTrips employeeTrip);

        //[Sudhir.Jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<LogEntriesByEmployeeTrip> GetLogEntriesByEmployeeTrip_V2480(UInt32 idEmployeeTrip);

        //[Sudhir.Jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips EditEmployeeTripDetails_V2480(EmployeeTrips employeeTrip);

        //[Sudhir.jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAuthorizedPlantsByIdUser_V2480(Int32 idUser);

        //[Sudhir.Jangra][GEOS2-4816]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips GetEditEmployeeTripDetails_V2480(UInt32 idEmployeeTrip);
        #endregion

        //rajashi GEOS2-4911

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTraineeDetails> GetTraineedataforExcel(ulong Professionaltraining);


        //[GEOS2-5071][rdixit][17.01.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetAllEmployeesForAttendanceByIdCompany_V2520 instead.")]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2480(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //Shubham[skadam] GEOS2-5225 Automatic date not updated properly 17 01 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<OrganizationHR> GetOrganizationHRByCompany_V2480(Int32 IdSite);

        //Shubham[skadam] GEOS2-5145 Expenses Shared in different currency. 18 01 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use EmployeeExpenseByExpenseReport_V2510 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2480(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency);

        //[pramod.misal][GEOS2-5159][18.01.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<MealAllowance> GetMealAllowances_V2480();

        #region GEOS2-4846
        //[Sudhir.jangra][GEOS2-4846]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetAllJobDescriptionForApprovalResponsible_V2480(Int32 idJobDescription);

        //[Sudhir.jangra][GEOS2-4846]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee AddEmployee_V2480(Employee employee);

        //[Sudhir.jangra][GEOS2-4846]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2580. Use GetEmployeeByIdEmployee_V2580 instead.")]
        Employee GetEmployeeByIdEmployee_V2480(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[Sudhir.Jangra][GEOS2-4846]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployee_V2480(Employee employee);

        #endregion

        //rajashi GEOS2-4911
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetTrainingTemplate(string sitename);

        //pramod.misal GEOS2-5077 24.01.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Geos_app_settingsReceiptsPDF_PageSize();


        //pramod.misal GEOS2-5077 24.01.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, byte[]> Geos_app_settingsHeader_Image();


        //pramod.misal GEOS2-5077 24.01.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Geos_app_settingsHeader_Footer_DateTime();

        //pramod.misal GEOS2-5077 24.01.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Geos_app_settings_Footer_Email();

        //pramod.misal GEOS2-5077 24.01.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string Geos_app_settingsHeader_Footer_pageNumber();

        #region GEOS2-5275
        //[Sudhir.Jangra][GEOS2-5275]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetEmployeeSplitAttendance_V2500 instead.")]
        List<EmployeeAttendance> GetEmployeeSplitAttendance_V2480(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime startDate, DateTime endDate);




        #endregion

        //pramod.misal GEOS2-5077 06.02.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use HRM_GetAllExpenseAttachmentsByExpenseReportId_V2500 instead.")]
        List<EmployeeExpensePhotoInfo> HRM_GetAllExpenseAttachmentsByExpenseReportId(int idEmployeeExpense);
        //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2560. Use APILayerCurrencyConversions_V2560 instead.")]
        bool APILayerCurrencyConversions(DateTime StartDate, Currency sourceCurrency, Currency targetCurrency);

        //[pramod.misal][GEOS2-5286][09-02-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenses> GetTravelExpenses_V2490(string Plant);

        //[Sudhir.Jangra][GEOS2-3418]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2500. Use GetAllEmployeeListForProfessionalContact_V2500 instead.")]
        List<Employee> GetAllEmployeeListForProfessionalContact_V2490();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TripExpensesReport GetTripExpensesReport_V2490(UInt32 idEmployeeTrip);

        //[pramod.misal][GEOS2-5365][08.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2510. Use GetMealAllowances_V2510 instead.")]
        List<MealAllowance> GetMealAllowances_V2500();

        //rajashri GEOS2-4817
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTrips> GetTripDetails(string idCompany, UInt32 idUser, long selectedPeriod);

        //[rdixit][12.03.2024][GEOS2-5335]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use GetMealAllowances_V2650 instead.")]
        List<EmployeeHoliday> GetEmployeeHolidays_V2500(Int32 idCompany, DateTime? currentDate = null);

        //[pramod.misal][GEOS2-5365][12.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateMealAllowance_V2500(List<EmployeeMealBudget> employeeMealBudget);

        //[pramod.misal][GEOS2 - 5400][14.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ProfessionalTraining AddProfessionalTraining_V2500(ProfessionalTraining professionalTraining);

        //[pramod.misal][GEOS2 - 5400][14.03.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2540. Use UpdateProfessionalTraining_V2540 instead.")]
        bool UpdateProfessionalTraining_V2500(ProfessionalTraining professionalTraining);


        //[GEOS2-5545][27.03.2024][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeeListForProfessionalContact_V2500();

        //[Sudhir.Jangra][GEOS2-5336]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAnnualAdditionalLeave GetEmployeeAnnualCompensationAttendance_V2500(Int32 idEmployee, long selectionPeriod);

        //[rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAttendance SplittedEmployeeAttendanceAdd(EmployeeAttendance employeeAttendance);

        //[rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool SplittedEmployeeAttendanceUpdate(EmployeeAttendance employeeAttendance);

        //[rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2530. Use GetEmployeeSplitAttendance_V2530 instead.")]
        List<EmployeeAttendance> GetEmployeeSplitAttendance_V2500(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime startDate, DateTime endDate);

        //[Sudhir.Jangra][GEOS2-5336]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmployeeAttendanceLeave_V2500(EmployeeAnnualLeave employee, double hours);

        //[Sudhir.Jangra][GEOS2-5336]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAnnualLeave> GetEmployeeLeaveByIdEmployee_V2500(Int32 idEmployee, long selectedPeriod, string idCompany, List<EmployeeAnnualAdditionalLeave> employeeAnnualLeavesAdditional);

        //[Sudhir.Jangra][GEOS2-5336]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmployeeChangelogs_V2500(EmployeeChangelog changeLog);

        //[rajashri][01-04-2024][GEOS2-5360]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ExpenseReportCurrency> GetAllExpenseReportCurrency_V2500();

        //[sudhir.Jangra][GEOS2-5336]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteEmployeeLeaveForAttendance_V2500(long idEmployeeAnnualLeave);

        //[Sudhir.jangra][GEOS2-5614]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<MealAllowance> GetMealAllowances_V2510();

        //[rdixit][[GEOS2-5336]][19.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool DeleteEmployeeCompensatoryHours_V2501(Int32 idEmployee, long selectedPeriod, double hours);

        //rajashri GEOS2-5534
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2510(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, UInt32 idUser);

        //[rajashri] [GEOS2-5514][26.04.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use EmployeeExpenseByExpenseReport_V2520 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2510(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency);

        //[cpatil][GEOS2-5538][20.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenses> GetTravelExpenses_V2520(string Plant, Int64 selectedPeriod);

        //[cpatil][GEOS2-5538][20.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenses> GetAllTravelExpensewithPermission_V2520(string Plant, string idDepartment, string idOrganization, int permission, Int64 selectedPeriod);

        //[cpatil][GEOS2-5539][21.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2520. Use GetWorkFlowStatus_V2520 instead.")]
        List<WorkflowStatus> GetWorkFlowStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

        //[GEOS2-5640][cpatil][24.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[cpatil][GEOS2-5640][24-05-2024]//
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        //[rushikesh.gaikwad] [GEOS2-5555][23.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2540. Use EmployeeExpenseByExpenseReport_V2540 instead.")]
        List<Expenses> EmployeeExpenseByExpenseReport_V2520(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsProfileUpdate(string EmployeeCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use UpdateTravelExpenses_V2630 instead.")]
        bool UpdateTravelExpenses_V2520(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList);

        //[rajashri] [GEOS2-5209][29.05.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeAttendance_V2520(EmployeeAttendance employeeAttendance);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAttendance AddEmployeeAttendance_V2520(EmployeeAttendance employeeAttendance, byte[] fileInBytes);

        //[Sudhir.jangra][GEOS2-5540]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<WorkflowStatus> GetWorkFlowStatus_V2520();


        //[rdixit][06.06.2024][GEOS2-5786]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeSplitAttendance_V2530(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime startDate, DateTime endDate);

        //[pramod.misal][GEOS2-5549][13.06.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HealthAndSafetyAttachedDoc> GetAllHealthAndSafetyFilesByIdJobDescription(UInt32 jobid);

        //Shubham[skadam] GEOS2-5811 HRM - Wrong date in recursive Holidays  17 06 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyHoliday> GetCompanyHolidaysBySelectedIdCompany_V2530(string idCompany, Int64 selectedPeriod = 0);

        //[rajashrit][09-11-2023][GEOS2-4613]
        //Shubham[skadam] GEOS2-5781 HRM - Employee Exit Announcement  17 06 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2530(DateTime curDate);

        //[rushikesh.gaikwad][GEOS2-5549][17.06.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        JobDescription UpdateJobDescription_V2530(JobDescription jobDescription);

        //[Sudhir.Jangra][GEOS2-5549]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EquipmentAndToolsAttachedDoc> GetEquipmentAndToolsForJobDescription_V2530(UInt32 idJobDescription);

        //[Sudhir.jangra][GEOS2-5549]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetUserAllowSites_V2530(int idUser);

        //pramod.misal GEOS2-5793 24.06.2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2540. Use GetTripExpensesReport_V2540 instead.")]
        TripExpensesReport GetTripExpensesReport_V2530(UInt32 idEmployeeTrip);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2540. Use GetTripExpensesReport_V2570 instead.")]
        //Shubham[skadam] GEOS2-5792 HRM - Travel Reports currency exchange based on ticket IESD-86619 17 07 2024
        TripExpensesReport GetTripExpensesReport_V2540(UInt32 idEmployeeTrip, Int32 IdCurrencyTo);

        //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeExpenseStatus> GetRejectSendMail_V2540(int IdEmployeeExpenseReport);
        //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GetEmployeeExpenseReportTemplate_V2540(string code, Employee selectedEmployee, TravelExpenseStatus status, string comments, string userCompanyEmail);

        //chitra.girigosavi [17/07/2024] GEOS2-5955 Registro de Capacitación 2
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateProfessionalTraining_V2540(ProfessionalTraining professionalTraining);

        //[rdixit][17.07.2024][GEOS2-5767]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Expenses> EmployeeExpenseByExpenseReport_V2540(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency);

        //[rdixit][17.07.2024][GEOS2-5767]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TravelExpenses> GetExpensesReportByTrip(UInt32 idEmployeeTrip);


        //[rdixit][17.07.2024][GEOS2-5680]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyHolidaySetting> GetAllCompanyHolidaySetting();

        //[rdixit][17.07.2024][GEOS2-5680]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCompanyHolidaySetting(List<CompanyHolidaySetting> companyHolidaySetting);

        //[GEOS2-5681][22-07-2024][nsatpute]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyServiceLength> GetCompanyWiseLengthOfService();

        //[GEOS2-5681][23-07-2024][nsatpute]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCompanyWiseLengthOfService(string modifiedBy, List<CompanyServiceLength> lstCompanyService);

        //pramod.misal GEOS2-5077 06.02.2024
        //Shubham[skadam] GEOS2-6037 HRM Expense Report - Time to Generate Expense tickets 05 08 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use HRM_GetAllExpenseAttachmentsByExpenseReportId_V2600 instead.")]
        List<EmployeeExpensePhotoInfo> HRM_GetAllExpenseAttachmentsByExpenseReportId_V2500(int idEmployeeExpense);

        //[rdixit][23.08.2024][GEOS2-5945]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetWorkbenchMainServiceProvider();

        //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
        //Shubham[skadam] GEOS2-6430 One to one currency conversion need to do 10 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool APILayerCurrencyConversions_V2560(DateTime StartDate, Currency sourceCurrency, Currency targetCurrency);

        // [nsatpute][11-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> HRM_GetResponsibleForAddEditTrip();

        // [nsatpute][12-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Destination> HRM_GetSuppliersForDestination();

        //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        IList<LookupValue> GetLookupValues_V2560(byte key);

        //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2560(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, UInt32 idUser);

        // [nsatpute][13-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TripAssets> HRM_GetEmployeeTripAssets();

        //Shubham[skadam] GEOS2-5682 HRM - Holidays (3 of 4) 12 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LogEntriesByCompanyLeaves> GetCompanyLeavesChangeLog_V2560();

        //[GEOS2-5681][23-07-2024][nsatpute]
        //Shubham[skadam] GEOS2-5682 HRM - Holidays (3 of 4) 12 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCompanyWiseLengthOfService_V2560(string modifiedBy, List<CompanyServiceLength> lstCompanyService);

        //[rdixit][17.07.2024][GEOS2-5680]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateCompanyHolidaySetting_V2560(List<CompanyHolidaySetting> companyHolidaySetting);

        //[rushikesh.gaikwad][GEOS2-5927][16.09.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeleteTrips(Int32 idEmployeeTrip);

        //[GEOS2-6387][rdixit][18.09.2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetAuthorizedPlantsByIdUser_V2670 instead.")]
        List<Employee> GetTodayEmployeeCompanyAnniversariesDetailsNew_V2560(DateTime currentDate);

        // [nsatpute][16-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Destination> GetCustomersForTripDestination_V2560();

        // [nsatpute][16-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Destination> GetPlantListForDestination_V2560(UInt32 idUser);

        // [nsatpute][16-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetAuthorizedPlantsByIdUser_V2600 instead.")]
        List<Company> GetAuthorizedPlantsByIdUser_V2560(Int32 idUser);

        //[rdixit][20.09.2024][GEOS2-5930]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips AddEmployeeTripDetails_V2560(EmployeeTrips employeeTrip);

        //Shubham[skadam] GEOS2-5683 HRM - Holidays (4 of 4) 19 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ActiveEmployee> GetActiveEmployees_V2560();

        //Shubham[skadam] GEOS2-5683 HRM - Holidays (4 of 4) 19 09 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddUpdateEmployeeAnnualLeaves_V2560(ActiveEmployee ActiveEmployee);

        // [nsatpute][16-09-2024][GEOS2-5931]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetEditEmployeeTripDetails_V2600 instead.")]
        EmployeeTrips GetEditEmployeeTripDetails_V2560(UInt32 idEmployeeTrip);

        //[rdixit][20.09.2024][GEOS2-5930]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips EditEmployeeTripDetails_V2560(EmployeeTrips employeeTrip);

        // [nsatpute][21-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<TripStatus> GetAllTripStatusWorkflow();

        // [nsatpute][21-09-2024][GEOS2-5929]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowTransition> GetAllTripStatusWorkflowTransitions();

        // [nsatpute][16-09-2024][GEOS2-5931]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void StartSavingTripAttachmentFile(string saveDirectorPath, string fileName);

        // [nsatpute][16-09-2024][GEOS2-5931]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveTripAttachmentPartData(string saveDirectorPath, string fileName, byte[] partData);

        // [nsatpute][16-09-2024][GEOS2-5931]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteTripAttachmentFile(string directoryPath, string fileName);

        // [nsatpute][16-09-2024][GEOS2-5931]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetTripAttachmentFile(string directoryPath, string fileName);

        // [nsatpute][24-09-2024][GEOS2-6473]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<WorkflowStatus> HRM_GetAllTripWorkflowStatus();

        // [nsatpute][26-09-2024][GEOS2-6486]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips EditEmployeeTripDetails_V2570(EmployeeTrips employeeTrip);

        // [nsatpute][26-09-2024][GEOS2-6486]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips AddEmployeeTripDetails_V2570(EmployeeTrips employeeTrip);

        //[rgadhave][GEOS2-6385][27-09-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Destination GetIdCurrrencyFromIdCountry_V2570(byte? IdCountry);
        //[Rahul.Gadhave][GEOS2-6085] [Date:03-10-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2570(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        // [nsatpute][03-10-2024][GEOS2-6451]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2570(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);
        //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version 2680. Use GetTripExpensesReport_V2680 instead.")]
        TripExpensesReport GetTripExpensesReport_V2570(UInt32 idEmployeeTrip, Int32 IdCurrencyTo);

        //[nsatpute][15-10-2024][GEOS2-5933]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTripStatus> GetEmployeeTripStatusDetails();

        // [nsatpute][21-10-2024][GEOS2-5933]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        List<LogEntriesByEmployeeTrip> GetLogEntriesByEmployeeTrip_V2570(UInt32 idEmployeeTrip);

        // [nsatpute][22-10-2024][GEOS2-6656]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetActiveEmployeesForTraveller_V2600 instead.")]
        List<Traveller> GetActiveEmployeesForTraveller_V2570(string IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission);

        // [nsatpute][22-10-2024][GEOS2-6543]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use AddEmployeeTripDetails_V2600 instead.")]
        EmployeeTrips AddEmployeeTripDetails_V2580(EmployeeTrips employeeTrip);

        //[nsatpute][22-10-2024][GEOS2-6543]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use EditEmployeeTripDetails_V2600 instead.")]
        EmployeeTrips EditEmployeeTripDetails_V2580(EmployeeTrips employeeTrip);

        // [nsatpute][08-11-2024] HRM - Improve ERF . GEOS2-6475
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeDepartmentSituation> GetEmployeeDepartmentSituation(string employeeDepartmentName);

        // [nsatpute][03-10-2024][GEOS2-6451]
        //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2580(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);


        //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAttendance GetEmployeeAttendanceByIdEmployeeAttendance_V2580(Int64 IdEmployeeAttendance);

        // [nsatpute][14-11-2024][GEOS2-5747]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmdepSiteDetails> GetEmdepsitesCountryRegion();

        // [nsatpute][14-11-2024][GEOS2-5747]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesByDepartmentAndCompany();


        //[rdixit][15.11.2024][GEOS2-6013]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsCompanyInIdCardExtraInfoCountries(int idCompany);



        //[Sudhir.Jangra][GEOS2-5579]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployee_V2580(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);


        //[Sudhir.Jangra][GEOS2-5579]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2610. Use UpdateEmployee_V2610 instead.")]
        bool UpdateEmployee_V2580(Employee employee);

		// [nsatpute][19-11-2024][GEOS2-5748]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveEmployeeBacklogHours(List<Employee> employees);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2620. Use GetAllEmployeesByIdCompany_V2620 instead.")]
        List<Employee> GetAllEmployeesByIdCompany_V2590(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        // [nsatpute][10-12-2024][GEOS2-6367]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version 2680. Use GetTripExpensesReport_V2680 instead.")]
        TripExpensesReport GetTripExpensesReport_V2590(UInt32 idEmployeeTrip, Int32 IdCurrencyTo);

        // [nsatpute][17-12-2024][GEOS2-5747]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesByDepartmentAndCompany_V2590();

        // [nsatpute][17-12-2024][GEOS2-5747]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2600. Use GetEmployeeBacklogHours_V2600 instead.")]
        List<Employee> GetEmployeeBacklogHours(List<Employee> lstEmployees);


        //[rdixit][18.12.2024][GEOS2-6571]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<PlantLeave> GetLeaveTypesWithLocations_V2590();

        //[rdixit][18.12.2024][GEOS2-6571]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetLeavesByLocations_V2590(List<int> employeeIdList);

        //[rdixit][18.12.2024][GEOS2-6571]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2360. Use GetEmployeeByIdEmployee_V2600 instead.")]
        Employee GetEmployeeByIdEmployee_V2590(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[nsatpute][19-12-2024][GEOS2-6635]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAttendance AddEmployeeAttendance_V2590(EmployeeAttendance employeeAttendance, byte[] fileInBytes);


        //[nsatpute][19-12-2024][GEOS2-6635]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsPartialAttendanceExists(int idEmployee);

        //[nsatpute][19-12-2024][GEOS2-6636]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2590(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

        //[nsatpute][19-12-2024][GEOS2-6636]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2190(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds);

        // [nsatpute][24-12-2024][GEOS2-6774]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmdepSiteDetails> GetEmdepsitesCountryRegion_V2590();

        //[rdixit][24.12.2024][GEOS2-6571]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2590(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][24.12.2024][GEOS2-6571]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeeLeavesByLocations_V2590(List<int> employeeIdList);

        // [nsatpute][06-01-2025] [GEOS2-6775]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2600(Company company);

        // [nsatpute][06-01-2025] [GEOS2-6775]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyDetailsByCompanyIdSelectedPeriod_V2600(Int32 idCompany, Int64 selectedPeriod);

        // [nsatpute][09-01-2025][GEOS2-6776]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2630. Use GetEmployeeByIdEmployee_V2630 instead.")]
        Employee GetEmployeeByIdEmployee_V2600(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[GEOS2-6760][rdixit][09.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Traveller> GetActiveEmployeesForTraveller_V2600(string IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission);

        //[GEOS2-6760][rdixit][09.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips GetEditEmployeeTripDetails_V2600(UInt32 idEmployeeTrip);

        //[GEOS2-6760][rdixit][09.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips AddEmployeeTripDetails_V2600(EmployeeTrips employeeTrip);

        //[GEOS2-6760][rdixit][09.01.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips EditEmployeeTripDetails_V2600(EmployeeTrips employeeTrip);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> HRM_GetResponsibleByPlantAndIdEmployee_V2600(string idCompanies, uint idEmployee, uint idDept);

        // [nsatpute][13-01-2025][GEOS2-6776]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAuthorizedPlantsByIdUser_V2600(Int32 idUser);

        // [nsatpute][16-12-2024][GEOS2-6862]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeeBacklogHours_V2600(List<Employee> lstEmployees);

        //pramod.misal GEOS2-5077 06.02.2024
        //Shubham[skadam] GEOS2-6037 HRM Expense Report - Time to Generate Expense tickets 05 08 2024
        //Shubham[skadam] GEOS2-6500 HRM Expenses report not working properly.  17 01 2024
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeExpensePhotoInfo> HRM_GetAllExpenseAttachmentsByExpenseReportId_V2600(int idEmployeeExpense);

        //[rdixit][29.01.2025][GEOS2-6826]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee IsExistDocumentNumberInAnotherEmployee_V2610(string employeeDocumentNumber, Int32 idEmployee, Int32 employeeDocumentIdType, Int32 idCompany);

        //[rdixit][10.02.2025][GEOS2-6850]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAnnualLeave> GetAnnualLeavesByIdEmployees(string idEmployeeList, long selectedPeriod);

        //[rdixit][GEOS2-6872][10.02.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployee_V2610(Employee employee);

        //[rdixit][13.02.2025][GEOS2-3424]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> HRM_GetDraftToActiveEmployees_V2610();

        //[rdixit][13.02.2025][GEOS2-3424]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool InsertUpdateEmployeeProfessionalEmail_V2610(Employee employee);

         //[pallavi.kale][GEOS2-2497][07-03-25]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company GetCompanyDetailsByCompanyIdSelectedPeriod_V2620(Int32 idCompany, Int64 selectedPeriod);


          //[pallavi.kale][GEOS2-2497][07-03-25]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Company UpdateCompany_V2620(Company company);

        //[Sudhir.Jangra][GEOS2-5656]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesByIdCompany_V2620(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][GEOS2-5659][17.03.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]        
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2620(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[Shweta.Thube][GEOS2-5660]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetLengthOfServiceByDepartment_V2620(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);
        //[shweta.thube][GEOS2-5511]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeAttendance_V2620(EmployeeAttendance employeeAttendance);

        //[rdixit][21.03.2025][GEOS2-5661]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesWithAnniversaryDate_V2620(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[nsatpute][26-03-2025][GEOS2-7011]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<int, string> GetSitesPetronalNumbers();

        //[rdixit][GEOS2-5659][27.03.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2630(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

		//[nsatpute][31-03-2025][GEOS2-7611]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2670. Use GetEmployeeByIdEmployee_V2670 instead.")]
        Employee GetEmployeeByIdEmployee_V2630(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[rdixit][GEOS2-6979][02.04.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateTravelExpenses_V2630(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList);

        //[rdixit][GEOS2-7799][10.04.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2630(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        //[rdixit][GEOS2-4472][05.07.2023]
        //Shubham[skadam] GEOS2-7731 Need to improvement in Issue with Report Display in GEOS  18 04 2025
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, byte[]> GetEmployeeExpenseReport_V2630(string plantName, string EmployeeCode, string ExpenseCode);

        // [nsatpute][12-05-2025][GEOS2-5707]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Recruitment> GetEmployeeRecruitmentByIdSite(string idCompany, long selectedPeriod);

        // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedEmployeeAttendance_V2640(int idEmployee, string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

		//[nsatpute][16-05-2025][GEOS2-6617]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeHoliday GetEmployeeHolidaysByEmail(string email, DateTime currentDate);

        //[GEOS2-7987][23.05.2025][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContact> GetCCMailsFor25YearAnniversaryEmployees(int idCompany);

        // [pallavi.kale][28-05-2025][GEOS2-7941]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use AddEmployeeTripDetails_V2670 instead.")]
        EmployeeTrips AddEmployeeTripDetails_V2650(EmployeeTrips employeeTrip);

        // [pallavi.kale][28-05-2025][GEOS2-7941]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2650. Use EditEmployeeTripDetails_V2670 instead.")]
        EmployeeTrips EditEmployeeTripDetails_V2650(EmployeeTrips employeeTrip);

        // [pallavi.kale][28-05-2025][GEOS2-7941]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips GetEditEmployeeTripDetails_V2650(UInt32 idEmployeeTrip);

        //[GEOS2-7987][23.05.2025][rdixit]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContact> GetBCCMailsFor25YearAnniversaryEmployees(int idCompany);

        //chitra.girigosavi GES2-6813 26/06/2025
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeHoliday> GetEmployeeHolidays_V2650(Int32 idCompany, DateTime? currentDate = null);

        //[GEOS2-8833][rdixit][21.08.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContact> GetEmployeeContactsByJD_V2660(int idCompany, int idJobDescription);

        //[rdixit][GEOS2-9423][08.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetTodayEmployeeCompanyAnniversariesDetailsNew_V2670(DateTime currentDate, int idCompany);

        //[rdixit][GEOS2-9435][09.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetTodayBirthdayOfEmployees_V2670(int idcompany, DateTime? currentDate = null);

        //[cpatil][12-09-2025][GEOS2-6971]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployee_V2670(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        //[Rahul.Gadhave][GEOS2-7989][Date:18-09-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee_trips_transfers> HRM_GetArrivalTransferDetails_V2670(UInt32 idEmployeeTrip);

        //[Rahul.Gadhave][GEOS2-7989][Date:22-09-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips EditEmployeeTripDetails_V2670(EmployeeTrips employeeTrip, List<Employee_trips_transfers> transfersList, List<EmployeeTripsAccommodations> accommodationsList);
        //[Rahul.Gadhave][GEOS2-7989][Date:22-09-2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeTrips AddEmployeeTripDetails_V2670(EmployeeTrips employeeTrip, List<Employee_trips_transfers> transfersList,List<EmployeeTripsAccommodations> accommodationsList);
        //[shweta.thube][GEOS2-7989][26.09.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeTripsAccommodations> GetAccommodationsDetails_V2670(UInt32 idEmployeeTrip);

        //[nsatpute][07.10.2025][GEOS2-6367]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        TripExpensesReport GetTripExpensesReport_V2680(UInt32 idEmployeeTrip, Int32 IdCurrencyTo);


        //[GEOS2-9059][rdixit][08.10.2025]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContractSituation> GetEmployeeContractExpirations_V2680(Int32 idCompany, ref List<string> emails, DateTime? currentDate = null);

		//[nsatpute][07.10.2025][GEOS2-6367]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetServerCultureInfo();
    }
}
