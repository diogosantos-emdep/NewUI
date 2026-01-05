using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.FileReplicator;
using Emdep.Geos.Data.Common.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHrmService" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(EmployeeEducationQualification))]
    [ServiceKnownType(typeof(EmployeeProfessionalEducation))]
    [ServiceKnownType(typeof(EmployeeContractSituation))]
    [ServiceKnownType(typeof(EmployeeDocument))]
    [ServiceKnownType(typeof(JobDescription))]
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
        CompanyShift AddCompanyShift_V2035(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        CompanyShift UpdateCompanyShift_V2035(CompanyShift companyShift);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
        bool UpdateEmployeeAttendance_V2036(EmployeeAttendance employeeAttendance);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2038. Use AddEmployeeImportAttendance_V2038 instead.")]
        List<EmployeeAttendance> AddEmpAttendanceWithClockIdFromExcel_V2036(List<EmployeeAttendance> employeeAttendanceList);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> AddEmployeeImportAttendance_V2038(List<EmployeeAttendance> employeeAttendanceList);


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
        List<Department> GetLengthOfServiceByDepartment_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetEmployeesCountByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetBirthdaysOfEmployeesByYear_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetEmployeesWithExitDateForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
        List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2041(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2050. Use GetAllEmployeesForLeaveByIdCompany_V2050 instead.")]
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
        List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2045(Int32 idEmployee, Int64 selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2060. Use GetSelectedIdCompanyEmployeeAttendance_V2060 instead.")]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2045(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetTodayBirthdayOfEmployees_V2045(long selectedPeriod = 0, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<CompanyHoliday> GetUpcomingCompanyHolidays_V2045(ref List<string> emails, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetUpcomingEmployeeLeaves_V2045(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetTodayEmployeeCompanyAnniversaries_V2045(long selectedPeriod, DateTime? currentDate = null);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard_V2045();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> AddEmployeeLeavesFromList_V2045(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeLeave UpdateEmployeeLeave_V2045(EmployeeLeave employeeLeave, long selectedPeriod);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2045(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeContractSituation> GetEmployeeContractExpirations(Int32 idCompany, ref List<string> emails, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployeeContractWarningDate(List<EmployeeContractSituation> employeeContractExpirationList, DateTime? currentDate = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee AddEmployee_V2046(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UpdateEmployee_V2046(Employee employee);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Employee GetEmployeeByIdEmployee_V2046(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
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
        List<Employee> GetEmployeeDetailsForLeaveSummary_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2050(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsEmployeeEnjoyedAllAnnualLeaves_V2050(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeAttendanceForNewLeave(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2050(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Department> GetAllEmployeesForOrganizationByIdCompany_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetOrganizationHierarchy_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<JobDescription> GetEmployeesForJobDescription(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesShortDetail(string idCompany, string SelectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission,string firstName,string lastName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        [ObsoleteAttribute("This method will be removed after version V2070. Use GetEmployeeAttendanceShortDetail_V2070 instead.")]
        List<EmployeeAttendance> GetEmployeeAttendanceShortDetail(string idCompany, string idsOrganization, string idsDepartment, Int32 idPermission,DateTime fromDate,DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2060(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmployeeAttendance AddEmployeeAttendance_V2060(EmployeeAttendance employeeAttendance);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2060(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmployeeAttendance> GetEmployeeAttendanceShortDetail_V2070(string idCompany, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate);

    }
}
