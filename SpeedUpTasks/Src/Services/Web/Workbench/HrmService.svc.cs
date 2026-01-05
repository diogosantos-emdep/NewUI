using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.FileReplicator;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HrmService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select HrmService.svc or HrmService.svc.cs at the Solution Explorer and start debugging.
    /// <summary>
    /// This Service is contains all HRM service functions.
    /// </summary>
    public class HrmService : IHrmService
    {
        HrmManager mgr = new HrmManager();

        /// <summary>
        /// This method is used get all employees of HRM.
        /// </summary>
        /// <returns>The list of employees.</returns>
        public List<Employee> GetAllEmployees()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployees(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get employee details by idEmployee.
        /// </summary>
        /// <param name="idEmployee">The employee id.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The employee details.</returns>
        public Employee GetEmployeeByIdEmployee(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee(workbenchConnectionString, idEmployee, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// Get all countries from emdep_geos database.
        /// </summary>
        /// <returns>The List of countries</returns>
        public List<Country> GetAllCountries()
        {
            List<Country> countries = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                countries = mgr.GetAllCountries(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return countries;
        }

        /// <summary>
        /// This method is used to get all education qualifications.
        /// </summary>
        /// <returns>List of education qualifications.</returns>
        public List<EducationQualification> GetAllEducationQualifications()
        {
            List<EducationQualification> educationQualifications = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                educationQualifications = mgr.GetAllEducationQualifications(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return educationQualifications;
        }

        /// <summary>
        /// This method is used to get all contract situations.
        /// </summary>
        /// <returns>All contract situations.</returns>
        public List<ContractSituation> GetAllContractSituations()
        {
            List<ContractSituation> contractSituations = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                contractSituations = mgr.GetAllContractSituations(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return contractSituations;
        }

        /// <summary>
        /// This method is used to get all professional categories.
        /// </summary>
        /// <returns>All professional categories.</returns>
        public List<ProfessionalCategory> GetAllProfessionalCategories()
        {
            List<ProfessionalCategory> professionalCategories = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                professionalCategories = mgr.GetAllProfessionalCategories(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return professionalCategories;
        }

        /// <summary>
        /// This method is used to all departments.
        /// </summary>
        /// <returns>The list of departments.</returns>
        public List<Department> GetAllDepartments()
        {
            List<Department> departments = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                departments = mgr.GetAllDepartments(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return departments;
        }

        /// <summary>
        /// Get all job descriptions.
        /// </summary>
        /// <returns>The list of job descriptions.</returns>
        public List<JobDescription> GetAllJobDescriptions()
        {
            List<JobDescription> jobDescriptions = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                jobDescriptions = mgr.GetAllJobDescriptions(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return jobDescriptions;
        }

        /// <summary>
        /// This method is used to add employee.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>The employee with idemployee.</returns>
        public Employee AddEmployee(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employee;
        }

        /// <summary>
        /// This method is used to update employee.
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <returns>True, if employee is updated else false.</returns>
        public bool UpdateEmployee(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to get lastest employee code.
        /// </summary>
        /// <returns>The auto generated employee code.</returns>
        public string GetLatestEmployeeCode()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestEmployeeCode(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all country regions.
        /// </summary>
        /// <returns>The list of country regions.</returns>
        public List<CountryRegion> GetAllCountryRegions()
        {
            List<CountryRegion> countryRegions = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                countryRegions = mgr.GetAllCountryRegions(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return countryRegions;
        }

        /// <summary>
        /// This method is used to EmployeeEducationQualification document file in bytes array.
        /// </summary>
        /// <param name="employeeCode">The employee code.</param>
        /// <param name="employeeEducationQualification">The employee education qualification file.</param>
        /// <returns>The employee education file.</returns>
        public EmployeeEducationQualification GetEmployeeEducationDocumentFile(String employeeCode, EmployeeEducationQualification employeeEducationQualification)
        {
            try
            {
                return mgr.GetEmployeeEducationDocumentFile(employeeCode, employeeEducationQualification, Properties.Settings.Default.EmpEducationQualificationFilesPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get employee documents of respective sections.
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="employeeSubObject"></param>
        /// <returns>The file in byte array.</returns>
        public byte[] GetEmployeeDocumentFile(String employeeCode, object employeeSubObject)
        {
            try
            {
                return mgr.GetEmployeeDocumentFile(employeeCode, employeeSubObject, Properties.Settings.Default.EmpEducationQualificationFilesPath,
                                                                                    Properties.Settings.Default.EmpProfessionalEducationFilesPath,
                                                                                    Properties.Settings.Default.EmpContractSituationFilesPath,
                                                                                    Properties.Settings.Default.EmpIdentificationDocumentFilesPath,
                                                                                    Properties.Settings.Default.EmpJobDescriptionsFilesPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used get all employees of HRM for Organization.
        /// </summary>
        /// <returns>The list of employees.</returns>
        public List<Department> GetAllEmployeesForOrganization()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganization(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// The get all company leaves.
        /// </summary>
        /// <param name="idCompany">The id companies.</param>
        /// <returns>The list of company holidays.</returns>
        public List<CompanyHoliday> GetCompanyHolidaysByIdCompany(Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanyHolidaysByIdCompany(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all employee leaves by id company
        /// </summary>
        /// <param name="idCompany">The id companies</param>
        /// <returns>The list of employee leaves.</returns>
        public List<EmployeeLeave> GetEmployeeLeavesByIdCompany(Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesByIdCompany(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to all employees by iddepartment.
        /// </summary>
        /// <returns>The list of departments.</returns>
        public List<Department> GetAllEmployeesByDepartment()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartment(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all company leaves.
        /// </summary>
        /// <returns>The company leaves.</returns>
        public List<CompanyLeave> GetAllCompanyLeaves()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyLeaves(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all employees by idcompany in employee job description
        /// </summary>
        /// <param name="idCompany">The idcompany</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of employees.</returns>
        public List<Employee> GetAllEmployeesByIdCompany(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// Get employee by idemployee.
        /// </summary>
        /// <param name="idEmployee">The id employee</param>
        /// <param name="idCompany">The id company</param>
        /// <returns>The Employee.</returns>
        public Employee GetEmployeeByIdEmployeeAndIdCompany(Int32 idEmployee, string idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployeeAndIdCompany(workbenchConnectionString, idEmployee, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all departments with list of employees.
        /// </summary>
        /// <param name="idCompany">The companies</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of departments.</returns>
        public List<Department> GetAllEmployeesByDepartmentByIdCompany(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all employees for attendance
        /// </summary>
        /// <param name="idCompany"></param>
        /// <param name="selectedPeriod"></param>
        /// <returns></returns>
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all employees for organization by idCompany
        /// </summary>
        /// <param name="idCompany">The id companies</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of departments.</returns>
        public List<Department> GetAllEmployeesForOrganizationByIdCompany(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method used to get company holidays by idcompany
        /// </summary>
        /// <param name="idCompany">The Company Holiday.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of company holidays.</returns>
        public List<CompanyHoliday> GetCompanyHolidaysBySelectedIdCompany(string idCompany, Int64 selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanyHolidaysBySelectedIdCompany(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// The employee leaves by idcompany
        /// </summary>
        /// <param name="idCompany">The idcompanies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of employee leaves.</returns>
        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// The company leaves by idcompany
        /// </summary>
        /// <param name="idCompany">The idcompanies.</param>
        /// <returns>The list of company leave.</returns>
        public List<CompanyLeave> GetSelectedIdCompanyLeaves(string idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyLeaves(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// The employee attendance by idcompany.
        /// </summary>
        /// <param name="idCompany">The idcompanies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of employee attendance.</returns>
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update employee leave.
        /// </summary>
        /// <param name="employeeLeave">The employee leave.</param>
        /// <returns>The employee leave with idEmployeeLeave.</returns>
        public EmployeeLeave AddEmployeeLeave(EmployeeLeave employeeLeave, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeLeave(MainWorkbenchConnectionString, workbenchConnectionString, employeeLeave, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> AddEmployeeLeavesFromList(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeLeavesFromList(MainWorkbenchConnectionString, workbenchConnectionString, Properties.Settings.Default.EmpLeaveAttachment, employeeLeaves, fileInBytes, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to update employee leave.
        /// </summary>
        /// <param name="employeeLeave">The employee leave.</param>
        /// <returns>The updated employee leave.</returns>
        public EmployeeLeave UpdateEmployeeLeave(EmployeeLeave employeeLeave, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeLeave(MainWorkbenchConnectionString, workbenchConnectionString, employeeLeave, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][24-09-2018][skhade][HRM-M048-06] Add new column File in Leaves grid
        /// </summary>
        /// <param name="employeeLeave">The employee leave.</param>
        /// <returns>Bytes array if found file else null.</returns>
        public byte[] GetEmployeeLeaveAttachment(EmployeeLeave employeeLeave)
        {
            try
            {
                return mgr.GetEmployeeLeaveAttachment(Properties.Settings.Default.EmpLeaveAttachment, employeeLeave);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to add employee leave attachment.
        /// </summary>
        /// <param name="employeeCode">The employee code.</param>
        /// <param name="idEmployeeLeave">The idEmployeeLeave.</param>
        /// <param name="fileName">The filename.</param>
        /// <param name="fileBytes">The file in bytes.</param>
        /// <returns>True if added leave, else false.</returns>
        public bool SaveEmployeeLeaveAttachment(string employeeCode, UInt64 idEmployeeLeave, string fileName, byte[] fileBytes)
        {
            try
            {
                return mgr.SaveEmployeeLeaveAttachment(employeeCode, idEmployeeLeave, fileName, Properties.Settings.Default.EmpLeaveAttachment, fileBytes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to delete employee leave attachment.
        /// </summary>
        /// <param name="employeeCode">The employee code.</param>
        /// <param name="idEmployeeLeave">The idEmployeeLeave.</param>
        /// <param name="fileName">The filename.</param>
        /// <returns>True if deleted leave, else false.</returns>
        public bool DeleteEmployeeLeaveAttachment(string employeeCode, UInt64 idEmployeeLeave, string fileName)
        {
            try
            {
                return mgr.DeleteEmployeeLeaveAttachment(Properties.Settings.Default.EmpLeaveAttachment, employeeCode, idEmployeeLeave, fileName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to add department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns>The added department with department id.</returns>
        public Department AddDepartment(Department department)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddDepartment(workbenchConnectionString, department);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is used to update department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns>The updated department.</returns>
        public Department UpdateDepartment(Department department)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateDepartment(workbenchConnectionString, department);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all dept details.
        /// </summary>
        /// <returns>The list of departments.</returns>
        public List<Department> GetAllDepartmentDetails()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllDepartmentDetails(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [HRM-M039-03] Add import attendance time option
        /// This method is used to add employee attendance
        /// </summary>
        /// <param name="employeeAttendanceList">The employee attendance list</param>
        /// <returns>True if added, else false.</returns>
        public bool AddEmployeeAttendance(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance(workbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-03][skhade][HRM-M042-06] Add and Edit Attendance.
        /// This method is used to add employee attendance. 
        /// </summary>
        /// <param name="employeeAttendance">The employee attendance.</param>
        /// <returns>The employee attendance with idEmployeeAttendance.</returns>
        public EmployeeAttendance AddEmployeeAttendance(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-03][skhade][HRM-M042-06] Add and Edit Attendance.
        /// This method is used to update employee attendance.
        /// </summary>
        /// <param name="employeeAttendance">The employee attendance.</param>
        /// <returns>True if updated else false.</returns>
        public bool UpdateEmployeeAttendance(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeAttendance(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][24-09-2018][skhade][HRM-M048-10] Allow to remove entries in attendance.
        /// </summary>
        /// <param name="idEmployeeAttendance">The id employee attendance.</param>
        /// <returns>True if deleted, else false</returns>
        public bool DeleteEmployeeAttendance(Int64 idEmployeeAttendance)
        {
            try
            {
                string mainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.DeleteEmployeeAttendance(mainWorkbenchConnectionString, idEmployeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// Dashboard - [HRM-M041-09] New section Dashboard.
        /// Show the birthdays from today + 7 days
        /// </summary>
        /// <returns>List of employees.</returns>
        public List<Employee> GetUpcomingBirthdaysOfEmployees(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingBirthdaysOfEmployees(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// Dashboard - [HRM-M041-09] New section Dashboard.
        /// Show the leaves from today + 7 days
        /// </summary>
        /// <returns>List of employees.</returns>
        public List<EmployeeLeave> GetUpcomingLeavesOfEmployees(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingLeavesOfEmployees(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-21][skhade][HRM-M041-07] New configuration section Holidays
        /// This method is used to Add Company Holiday.
        /// </summary>
        /// <param name="companyHoliday">The Company Holiday.</param>
        /// <returns>The added company holiday.</returns>
        public CompanyHoliday AddCompanyHoliday(CompanyHoliday companyHoliday)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddCompanyHoliday(workbenchConnectionString, companyHoliday);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-21][skhade][HRM-M041-07] New configuration section Holidays
        /// This method is used to Update Company Holiday.
        /// </summary>
        /// <param name="companyHoliday">The Company Holiday.</param>
        /// <returns>The updated company holiday.</returns>
        public CompanyHoliday UpdateCompanyHoliday(CompanyHoliday companyHoliday)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyHoliday(workbenchConnectionString, companyHoliday);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-22][skhade][HRM-M041-18] Enable to delete leaves
        /// </summary>
        /// <param name="employeeCode">The employee code to delete file</param>
        /// <param name="idEmployeeLeave">The idEmployeeLeave</param>
        /// <returns>True, if deleted else false.</returns>
        public bool DeleteEmployeeLeave(string employeeCode, UInt64 idEmployeeLeave, EmployeeLeave employeeLeave = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.DeleteEmployeeLeave(workbenchConnectionString, employeeCode, idEmployeeLeave, Properties.Settings.Default.EmpLeaveAttachment, employeeLeave);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-25][skhade][HRM-M041-09] New section Dashboard.
        /// This method is used to get employees count by department.
        /// </summary>
        /// <param name="idCompanies">The id companies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of contract situations with count.</returns>
        public List<Department> GetEmployeesHeadCountByDepartmentByIdCompany(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesHeadCountByDepartmentByIdCompany(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-25][skhade][HRM-M041-09] New section Dashboard.
        /// This method is used to get employees count by gender.
        /// </summary>
        /// <param name="idCompanies">The id companies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of contract situations with count.</returns>
        public List<LookupValue> GetEmployeesCountByGenderByIdCompany(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByGenderByIdCompany(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-24][skhade][HRM-M041-09] New section Dashboard.
        /// This method is used to get employees count by contract situations.
        /// </summary>
        /// <param name="idCompanies">The id companies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of contract situations with count.</returns>
        public List<ContractSituation> GetEmployeesCountByContractByIdCompany(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByContractByIdCompany(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-24][skhade][HRM-M041-09] New section Dashboard.
        /// This method is used to get employees count by department area.
        /// </summary>
        /// <param name="idCompanies">The id companies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of dept areas with count.</returns>
        public List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByDepartmentAreaByIdCompany(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-24][skhade][HRM-M041-09] New section Dashboard.
        /// This method is used to get employees count by job description.
        /// </summary>
        /// <param name="idCompanies">The id companies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of job descriptions with count.</returns>
        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// </summary>
        /// <param name="idCompanies">The idCompany</param>
        /// <param name="selectedPeriod">The selected Period</param>
        /// <returns></returns>
        public List<Company> GetEmployeesCountByIdCompany(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// </summary>
        /// <param name="idCompanies">The idCompany</param>
        /// <param name="selectedPeriod">The selected Period</param>
        /// <returns></returns>
        public List<Department> GetLengthOfServiceByDepartment(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLengthOfServiceByDepartment(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-26][skhade]This method is used to show employees list in add/edit leaves section.
        /// </summary>
        /// <param name="idCompany">The comma seperated companies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The list of employees.</returns>
        public List<Employee> GetAllEmployeesForLeaveByIdCompany(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-26][skhade][HRM-M041-08] New configuration section Job Descriptions.
        /// This method is used to add job description on main server.
        /// </summary>
        /// <param name="jobDescription">The job description to add.</param>
        /// <returns>The job description with id job description.</returns>
        public JobDescription AddJobDescription(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddJobDescription(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-26][skhade][HRM-M041-08] New configuration section Job Descriptions.
        /// This method is used to update job description on main server.
        /// </summary>
        /// <param name="jobDescription">The job description to update.</param>
        /// <returns>The updated job description.</returns>
        public JobDescription UpdateJobDescription(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateJobDescription(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is used to get all company works.
        /// </summary>
        /// <returns>The list of company works.</returns>
        public List<CompanyWork> GetAllCompanyWorks()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyWorks(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-04][skhade][HRM-M042-06] Add and Edit Attendance.
        /// This method is used to get all company works by company.
        /// </summary>
        /// <param name="idCompanies">The list of companies</param>
        /// <returns>The list of company works</returns>
        public List<CompanyWork> GetAllCompanyWorksByIdCompany(string idCompanies)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyWorksByIdCompany(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// </summary>
        /// <param name="idCompany">The idCompany.</param>
        /// <param name="selectedPeriod">The selected Period.</param>
        /// <returns></returns>
        public List<EmployeeLeave> GetEmployeeLeavesForDashboard(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesForDashboard(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// </summary>
        /// <param name="idCompany">The idCompany</param>
        /// <param name="selectedPeriod">The selected Period</param>
        /// <returns></returns>
        public List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDocumentsExpirationForDashboard(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// </summary>
        /// <param name="idCompany">The idCompany</param>
        /// <param name="selectedPeriod">The selected Period</param>
        /// <returns></returns>
        public List<EmployeeContractSituation> GetLatestContractExpirationForDashboard(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestContractExpirationForDashboard(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// </summary>
        /// <param name="idCompany">The idCompany</param>
        /// <param name="selectedPeriod">The selected Period</param>
        /// <returns></returns>
        public List<Employee> GetEmployeesWithAnniversaryDate(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithAnniversaryDate(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-25][skhade][changes in dashboard]
        /// Get employees with exitdate for dashboard.
        /// </summary>
        /// <param name="idCompany">The idCompany.</param>
        /// <param name="selectedPeriod">The selected Period.</param>
        /// <returns></returns>
        public List<Employee> GetEmployeesWithExitDateForDashboard(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithExitDateForDashboard(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][skhade][sprint43]
        /// </summary>
        /// <param name="idCompany">The idcountry.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns></returns>
        public List<JobDescription> GetOrganizationHierarchy(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][cpatil][2018-06-03][HRM-M044-13] Autocomplete field for city field in employee profile
        /// Get all cities by idcountry.
        /// </summary>
        /// <param name="idCountry">The idcountry</param>
        /// <returns>The list of cities.</returns>
        public List<City> GetAllCitiesByIdCountry(byte idCountry)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCitiesByIdCountry(workbenchConnectionString, idCountry);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-08-06][skhade][HRM-M044-24] Add new field work schedule in employee.
        /// This method is used to get all data related to company shifts.
        /// </summary>
        /// <returns>The list of company shifts with schedule.</returns>
        public List<CompanyShift> GetAllCompanyShifts(string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShifts(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-08-07][skhade][HRM-M044-24] Add new field work schedule in employee.
        /// This method is used to get all data related to company schedule with company shifts as inner list.
        /// </summary>
        /// <returns>The list of company schedules with shifts.</returns>
        public List<CompanySchedule> GetCompanyScheduleAndCompanyShifts(string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanyScheduleAndCompanyShifts(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [HRM-M045-13] Changes in Organization Print option.
        /// Get records for department area percentage in plant compared with avg of all other plants.
        /// </summary>
        /// <param name="idCompanies">The idcompanies.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns></returns>
        public List<LookupValue> GetOrganizationalChartDepartmentArea(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// [001][skhade][2018-08-22] [HRM M045-14] Email notification to IT when some employee leave the company
        /// </summary>
        /// <param name="idCompanies">The id companies</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetITEmployeeDetails(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetITEmployeeDetails(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to use exit main template
        /// </summary>
        /// <returns>Employee mail template</returns>
        public string GetEmployeeExitEmailTemplate()
        {
            try
            {
                ApplicationManager appManager = new ApplicationManager();
                MailServer mailServer = new MailServer();
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return appManager.GetEmployeeExitEmailTemplate(mailServer);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][skhade][2018-08-22][HRM M045-15] New automatic report for birthdays
        /// [001] This method is used to get today employees birthdays and send mail to employee and cc department employees
        /// </summary>
        /// <param name="workbenchConnectionString">The workbench connection string</param>
        /// <param name="employeesProfileImagePath">The employees profile image path</param>
        /// <param name="selectedPeriod">The selected period</param>
        /// <returns>The list of employees.</returns>
        public List<Employee> GetTodayBirthdayOfEmployees(long selectedPeriod = 0, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayBirthdayOfEmployees(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-08-22][skhade] HRM M045-16 New automatic report for company holidays
        /// [001] The upcoming company holidays.
        /// </summary>
        /// <param name="emails">The emails list.</param>
        /// <param name="currentDate">Get current date.</param>
        /// <returns>The list of company holidays.</returns>
        public List<CompanyHoliday> GetUpcomingCompanyHolidays(ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingCompanyHolidays(workbenchConnectionString, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This is used to get upcoming employee leaves for weekly mail.
        /// </summary>
        /// <param name="idCompany">The Idcompany</param>
        /// <param name="ToEmailList">The To emails list</param>
        /// <param name="ccEmailList">The CC emails list.</param>
        /// <param name="currentDate">Get current date.</param>
        /// <returns>The list of employee list.</returns>
        public List<EmployeeLeave> GetUpcomingEmployeeLeaves(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingEmployeeLeaves(workbenchConnectionString, idCompany, ref ToEmailList, ref ccEmailList, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //public bool AddEmployeeChangelogs(Int32 idEmployee, List<EmployeeChangelog> employeeChangelogs)
        //{
        //    try
        //    {
        //        string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
        //        return mgr.AddAllEmployeeChangelogs(mainServerWorkbenchConnectionString, idEmployee, employeeChangelogs);
        //    }
        //    catch (Exception ex)
        //    {
        //        ServiceException exp = new ServiceException();
        //        exp.ErrorMessage = ex.Message;
        //        exp.ErrorDetails = ex.ToString();
        //        throw new FaultException<ServiceException>(exp, ex.ToString());
        //    }
        //}

        /// <summary>
        /// This method is to add employee attendance from excel
        /// </summary>
        /// <param name="employeeAttendanceList">Get employee attendance details</param>
        /// <returns>If true  Added Employee Attendance From Excel otherwise false</returns>
        public bool AddEmployeeAttendanceFromExcel(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendanceFromExcel(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to read mail template details
        /// </summary>
        /// <param name="templateName">Get template name</param>
        /// <returns>Mail template</returns>
        public string ReadMailTemplate(string templateName)
        {
            try
            {
                return System.IO.File.ReadAllText(string.Format(@"{0}\{1}", Properties.Settings.Default.EmailTemplate, templateName));
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get birthdays of employee related to year
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetBirthdaysOfEmployeesByYear(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBirthdaysOfEmployeesByYear(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get job description related to id job description
        /// </summary>
        /// <param name="idJobDescription">Get job description id</param>
        /// <returns>Get Job description details</returns>
        public JobDescription GetJobDescriptionById(UInt32 idJobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetJobDescriptionById(workbenchConnectionString, idJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to check employee enjoyed all annual leave or not
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="idEmployeeLeave">Get employee leave id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>If true employee enojyed all annual leaves otherwise false</returns>
        public bool IsEmployeeEnjoyedAllAnnualLeaves(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsEmployeeEnjoyedAllAnnualLeaves(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][27-08-2018][skhade][HRM-M048-09] Add, remove and Edit authorized leaves.
        /// This method is used to get company daily hours.
        /// </summary>
        /// <param name="idEmployee">The idEmployee</param>
        /// <param name="idCompanyShift">The idCompanyShift</param>
        /// <param name="idCompany">The idCompany</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The Company shift.</returns>
        public CompanyShift GetEmployeeShiftandDailyHours(Int32 idEmployee, Int32 idCompanyShift, Int32 idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeShiftandDailyHours(workbenchConnectionString, idEmployee, idCompanyShift, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][28-09-2018][skhade][HRM-M048-12] Automatic Report for the Company Anniversaries.
        /// This method is used to get Today anniversaries of employees who works in current year. 
        /// </summary>
        /// <param name="selectedPeriod">Selected period now is current year.</param>
        /// <param name="currentDate">Get current date</param>
        /// <returns>The list of employees.</returns>
        public List<Employee> GetTodayEmployeeCompanyAnniversaries(long selectedPeriod, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayEmployeeCompanyAnniversaries(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee enjoyed leave hours
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="idEmployeeLeave">Get employee leave id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Employee annual leave details</returns>
        public EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeEnjoyedLeaveHours(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all company shift related to company id
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <returns>List of company shift details</returns>
        public List<CompanyShift> GetAllCompanyShiftsByIdCompany(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsByIdCompany(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][skhade][09-10-2018][HRM-M049-02] Add field shift in new or edit attendance - Added method
        /// </summary>
        /// <param name="idCompanies">The companies</param>
        /// <returns>The companies</returns>
        public List<CompanySchedule> GetAllCompanySchedulesByIdCompany(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanySchedulesByIdCompany(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][skhade][09-10-2018][HRM-M049-02] Add field shift in new or edit attendance - Added method
        /// </summary>
        /// <param name="companyShift">The company shift</param>
        /// <returns>The company shift</returns>
        public CompanyShift AddCompanyShift(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddCompanyShift(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][skhade][09-10-2018][HRM-M049-02] Add field shift in new or edit attendance - Added method
        /// </summary>
        /// <param name="companyShift">The company shift</param>
        /// <returns>The company shift</returns>
        public CompanyShift UpdateCompanyShift(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyShift(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee related company shifts related to employee id
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <returns>List of company shift details</returns>
        public List<CompanyShift> GetEmployeeRelatedCompanyShifts(Int32 idEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeRelatedCompanyShifts(workbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get company shift details related to id company shift
        /// </summary>
        /// <param name="idCompanyShift">Get id company shift</param>
        /// <returns>Get company shift details</returns>
        public CompanyShift GetCompanyShiftDetailByIdCompanyShift(Int32 idCompanyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanyShiftDetailByIdCompanyShift(workbenchConnectionString, idCompanyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update company details
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <returns>Get company details</returns>
        public Company UpdateCompany(Company company)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                company = mgr.UpdateCompany(WorkbenchConnectionString, company, Properties.Settings.Default.SiteImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// This method is to get company details related company id
        /// </summary>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Get Company details</returns>
        public Company GetCompanyDetailsByCompanyId(Int32 idCompany)
        {
            Company company = new Company();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                company = mgr.GetCompanyDetailsByCompanyId(WorkbenchConnectionString, idCompany, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// This method is to get all enterprise group details
        /// </summary>
        /// <returns>List of enterprise group details</returns>
        public List<EnterpriseGroup> GetAllEnterpriseGroup()
        {
            List<EnterpriseGroup> enterpriseGroups = new List<EnterpriseGroup>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                enterpriseGroups = mgr.GetAllEnterpriseGroup(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return enterpriseGroups;
        }

        /// <summary>
        /// This method is to add employee attendance with clock id from excel
        /// </summary>
        /// <param name="employeeAttendanceList">Get employee attendance list details</param>
        /// <returns>If true added employee attendance with clock from excel otherwise false</returns>
        public bool AddEmpAttendanceWithClockIdFromExcel(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmpAttendanceWithClockIdFromExcel(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get company details related id company and selected period
        /// </summary>
        /// <param name="idCompany">Get company id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Get company details</returns>
        public Company GetCompanyDetailsByCompanyIdSelectedPeriod(Int32 idCompany, Int64 selectedPeriod)
        {
            Company company = new Company();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                company = mgr.GetCompanyDetailsByCompanyIdSelectedPeriod(WorkbenchConnectionString, idCompany, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// This method is to get job description count
        /// </summary>
        /// <param name="idCompany">Get id company</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of job description details</returns>
        public List<JobDescription> GetJobDescriptionCount(Int32 idCompany, Int64 selectedPeriod)
        {
            List<JobDescription> jobDescriptions = new List<JobDescription>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                jobDescriptions = mgr.GetJobDescriptionCount(WorkbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return jobDescriptions;
        }

        /// <summary>
        /// This method is to get company department details
        /// </summary>
        /// <param name="idCompany">Get company id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of company department details</returns>
        public List<CompanyDepartment> GetCompanyDepartment(Int32 idCompany, Int64 selectedPeriod)
        {
            List<CompanyDepartment> companyDepartment = new List<CompanyDepartment>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companyDepartment = mgr.GetCompanyDepartment(WorkbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companyDepartment;
        }

        /// <summary>
        /// This method is to check employee exist or not
        /// </summary>
        /// <param name="firstName">Get first name</param>
        /// <param name="lastName">Get last name</param>
        /// <param name="idEmployee">Get employee id</param>
        /// <returns>List of employee details</returns>
        public List<Employee> IsEmployeeExists(string firstName, string lastName, Int32 idEmployee)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.IsEmployeeExists(WorkbenchConnectionString, firstName, lastName, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        /// <summary>
        /// This method is to get employee details related to employee document number
        /// </summary>
        /// <param name="clockIds">Get clockIds</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmpDtlByEmpDocumentNumber(string clockIds)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocumentNumber(WorkbenchConnectionString, clockIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        /// <summary>
        /// This method is to get employee details related to employee document number and period
        /// </summary>
        /// <param name="EmployeeDocumentNumbers">Get employee document number</param>
        /// <param name="idCompany">Get id company</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        /// <summary>
        /// This method is to get authorized plants related to iduser
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of company details</returns>
        public List<Company> GetAuthorizedPlantsByIdUser(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlantsByIdUser(WorkbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get company schedule related to id company
        /// </summary>
        /// <param name="idCompany">Get id company</param>
        /// <returns>List of company schedule details</returns>
        public List<CompanySchedule> GetCompanyScheduleByIdCompany(string idCompany)
        {
            List<CompanySchedule> companySchedules = new List<CompanySchedule>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companySchedules = mgr.GetCompanyScheduleByIdCompany(WorkbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companySchedules;
        }

        /// <summary>
        /// This method is to check employee enjoyed all annual leaves --sprint60
        /// </summary>
        /// <param name="idEmployee">Get id employee</param>
        /// <param name="idEmployeeLeave">Get id employee leave</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get id company</param>
        /// <returns>If true employee enjoyed all annual leaves otherwise false</returns>
        public bool IsEmployeeEnjoyedAllAnnualLeavesSprint60(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsEmployeeEnjoyedAllAnnualLeavesSprint60(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update employee leave sprint 60
        /// </summary>
        /// <param name="employeeLeave">Get employee leave details</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Employee leave details</returns>
        public EmployeeLeave UpdateEmployeeLeaveSprint60(EmployeeLeave employeeLeave, long selectedPeriod)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeLeaveSprint60(MainWorkbenchConnectionString, workbenchConnectionString, employeeLeave, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get today employee company anniversaries
        /// </summary>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="currentDate">Get current date</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetTodayEmployeeCompanyAnniversariesSprint60(long selectedPeriod, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayEmployeeCompanyAnniversariesSprint60(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee details for leave summary
        /// </summary>
        /// <param name="idCompany">Get id company</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmployeeDetailsForLeaveSummary(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to get company setting related to id company
        /// </summary>
        /// <param name="idAppSetting">Get id appsetting</param>
        /// <param name="idCompany">Get id company</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of company setting details</returns>
        public List<CompanySetting> GetCompanySettingByIdCompany(Int16 idAppSetting, string idCompany, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanySettingByIdCompany(idAppSetting, idCompany, selectedPeriod, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to add employee details
        /// </summary>
        /// <param name="employee">Get employee details</param>
        /// <returns>Get employee details</returns>
        public Employee AddEmployee_V2031(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2031(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employee;
        }

        /// <summary>
        /// This method is used to update employee.
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <returns>True, if employee is updated else false.</returns>
        public bool UpdateEmployee_V2031(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2031(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        /// <summary>
        /// This method is used to get employee details by idEmployee.
        /// </summary>
        /// <param name="idEmployee">The employee id.</param>
        /// <param name="selectedPeriod">The selected period.</param>
        /// <returns>The employee details.</returns>
        public Employee GetEmployeeByIdEmployee_V2031(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2031(workbenchConnectionString, idEmployee, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get length of service related to department
        /// </summary>
        /// <param name="idCompanies">Get selected id companies</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of department details</returns>
        public List<Department> GetLengthOfServiceByDepartment_V2031(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLengthOfServiceByDepartment_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees count related to selected id company
        /// </summary>
        /// <param name="idCompanies">Get selected companies id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of company details</returns>
        public List<Company> GetEmployeesCountByIdCompany_V2031(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees count related to job position anf selected id company
        /// </summary>
        /// <param name="idCompanies">Get selected companies id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of job description details</returns>
        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2031(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get birthday of employee related to selected year
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetBirthdaysOfEmployeesByYear_V2031(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBirthdaysOfEmployeesByYear_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees count related to department area and selected to company
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Get employee count in each department area related selected companies</returns>
        public List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2031(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByDepartmentAreaByIdCompany_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees count related to gender and selected companies
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Get employee count related to gender and selected companies</returns>
        public List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2031(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByGenderByIdCompany_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees count related to contract and selected companies ids
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee count related to contract and selected companies ids</returns>
        public List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2031(string idCompanies, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByContractByIdCompany_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees leaves for dashboard
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period </param>
        /// <returns>List of employee leave details</returns>
        public List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2031(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesForDashboard_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee dcument expiration for dashboard
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List fo employee document details</returns>
        public List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2031(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDocumentsExpirationForDashboard_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method to get employees with exit date for dashboard
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmployeesWithExitDateForDashboard_V2031(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithExitDateForDashboard_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get latest contract expiration for dashboard
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee contract situation details</returns>
        public List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2031(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestContractExpirationForDashboard_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees with anniversary date
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employees details</returns>
        public List<Employee> GetEmployeesWithAnniversaryDate_V2031(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithAnniversaryDate_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all employees for organization related to selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of department details</returns>
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2031(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2031(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get organization hierarchies
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of job description details</returns>
        public List<JobDescription> GetOrganizationHierarchy_V2031(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get organizational chart department area details
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Get list of Department are details</returns>
        public List<LookupValue> GetOrganizationalChartDepartmentArea_V2031(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea_V2031(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update company details
        /// </summary>
        /// <param name="company">Get company details</param>
        /// <returns>Get company details</returns>
        public Company UpdateCompany_V2031(Company company)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                company = mgr.UpdateCompany_V2031(WorkbenchConnectionString, company, Properties.Settings.Default.SiteImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return company;
        }

        /// <summary>
        /// This method is to get is company authorized plants related to id user
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of company details</returns>
        public List<Company> GetIsCompanyAuthorizedPlantsByIdUser(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetIsCompanyAuthorizedPlantsByIdUser(WorkbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get is organization authorized plants related to id user.
        /// </summary>
        /// <param name="idUser">Get id user details</param>
        /// <returns>List of companies details</returns>
        public List<Company> GetIsOrganizationAuthorizedPlantsByIdUser(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetIsOrganizationAuthorizedPlantsByIdUser(WorkbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get is location authorized plants related to id user
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of company details</returns>
        public List<Company> GetIsLocationAuthorizedPlantsByIdUser(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetIsLocationAuthorizedPlantsByIdUser(WorkbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        /// <summary>
        /// This method is to get all employees related to id company
        /// </summary>
        /// <param name="idCompany">Get selected id companies</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetAllEmployeesByIdCompany_V2031(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2031(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all employees for attendance related to selected id companies
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2031(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2031(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get selected id company employee attendance
        /// </summary>
        /// <param name="idCompany">Get id company</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee attendance details</returns>
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2031(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2032(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee leaves related to selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee leave details</returns>
        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2031(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2031(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// [001][2018-06-26][skhade][HRM-M041-08] New configuration section Job Descriptions.
        /// This method is used to add job description on main server.
        /// </summary>
        /// <param name="jobDescription">The job description to add.</param>
        /// <returns>The job description with id job description.</returns>
        public JobDescription AddJobDescription_V2031(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddJobDescription_V2031(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-06-26][skhade][HRM-M041-08] New configuration section Job Descriptions.
        /// This method is used to update job description on main server.
        /// </summary>
        /// <param name="jobDescription">The job description to update.</param>
        /// <returns>The updated job description.</returns>
        public JobDescription UpdateJobDescription_V2031(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateJobDescription_V2031(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// Get all job descriptions.
        /// </summary>
        /// <returns>The list of job descriptions.</returns>
        public List<JobDescription> GetAllJobDescriptions_V2031()
        {
            List<JobDescription> jobDescriptions = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                jobDescriptions = mgr.GetAllJobDescriptions_V2031(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return jobDescriptions;
        }

        /// <summary>
        /// This method is to get employee details related employee documnet number and selected period
        /// </summary>
        /// <param name="EmployeeDocumentNumbers">Get employee document numbers</param>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2031(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2031(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        /// <summary>
        /// This method to get authorized plant related to id user
        /// </summary>
        /// <param name="idUser">Get id user</param>
        /// <returns>List of companies</returns>
        public List<Company> GetAuthorizedPlantsByIdUser_V2031(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlantsByIdUser_V2031(WorkbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }


        /// <summary>
        /// This method is to get employee leaves related to selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected cmpanies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee leave details</returns>
        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2032(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2032(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get selected companies ids employee attendance
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee attendance details</returns>
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2032(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2032(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all employees for organization related to selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of department details</returns>
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2032(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2032(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to add company shift
        /// </summary>
        /// <param name="companyShift">Get company shift details to add</param>
        /// <returns>Get added company shift details </returns>
        public CompanyShift AddCompanyShift_V2032(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddCompanyShift_V2032(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update company shift
        /// </summary>
        /// <param name="companyShift">Gewt company shift details</param>
        /// <returns>updated company shift details</returns>
        public CompanyShift UpdateCompanyShift_V2032(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyShift_V2032(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all company shift related to selected companies ids
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <returns>List of company shifts details</returns>
        public List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2032(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsByIdCompany_V2032(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee latest job descriptions related to selected id employee
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of job descriptions details</returns>
        public List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee(Int32 idEmployee, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLatestJobDescriptionsByIdEmployee(workbenchConnectionString, idEmployee, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all employees for attendance related selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2032(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2032(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to get all employees for leave details related to selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selcted period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetAllEmployeesForLeaveByIdCompany_V2032(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany_V2032(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee details related employee document number and selected period
        /// </summary>
        /// <param name="EmployeeDocumentNumbers">Get employee document number</param>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2032(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2032(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        /// <summary>
        /// This method is to get all employees selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employees details</returns>
        public List<Employee> GetAllEmployeesByIdCompany_V2032(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2032(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to get all employees related to department and selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of department details</returns>
        public List<Department> GetAllEmployeesByDepartmentByIdCompany_V2032(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany_V2032(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all employees for organization related to selected companies ids.
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of department details</returns>
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2033(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2033(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method to get organization hierarchy
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List fo job description details</returns>
        public List<JobDescription> GetOrganizationHierarchy_V2033(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2033(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        /// <summary>
        /// This method is to get employee count related to selected companies
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of company details</returns>
        public List<Company> GetEmployeesCountByIdCompany_V2033(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany_V2033(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to get organizational chart department area details
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Get list of Department are details</returns>
        public List<LookupValue> GetOrganizationalChartDepartmentArea_V2033(string idCompanies, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea_V2033(workbenchConnectionString, idCompanies, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all employees related to companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetAllEmployeesByIdCompany_V2033(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2033(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee latest job descriptions related to selected id employee
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of job descriptions details</returns>
        public List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2033(Int32 idEmployee, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLatestJobDescriptionsByIdEmployee_V2033(workbenchConnectionString, idEmployee, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee details
        /// </summary>
        /// <param name="idEmployee">Get id employee</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Employee details</returns>
        public Employee GetEmployeeByIdEmployee_V2033(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2033(workbenchConnectionString, idEmployee, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee related to id employee
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <returns>employee details</returns>
        public Employee GetEmployeeByIdEmployee_V2034(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2034(workbenchConnectionString, idEmployee, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to add employee details
        /// </summary>
        /// <param name="employee">Get employee details</param>
        /// <returns>Get employee details</returns>
        public Employee AddEmployee_V2034(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2034(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employee;
        }

        /// <summary>
        /// This method is to update employee details
        /// </summary>
        /// <param name="employee">Get employee details</param>
        /// <returns>If true updated employee otherwise false</returns>
        public bool UpdateEmployee_V2034(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2034(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to get employee details related employee document number and selected period
        /// </summary>
        /// <param name="EmployeeDocumentNumbers">Get employee document number</param>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2034(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2034(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }



        /// <summary>
        /// This method is to get all employees for attendance related selected companies ids
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2034(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2034(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2034(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2034(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }




        /// <summary>
        /// This method is to update employee details
        /// </summary>
        /// <param name="employee">Get employee details</param>
        /// <returns>If true updated employee otherwise false</returns>
        public bool UpdateEmployee_V2035(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2035(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        /// <summary>
        /// This method is to get employee related to id employee
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <returns>employee details</returns>
        public Employee GetEmployeeByIdEmployee_V2035(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2035(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to add company shift
        /// </summary>
        /// <param name="companyShift">Get company shift details to add</param>
        /// <returns>Get added company shift details </returns>
        public CompanyShift AddCompanyShift_V2035(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddCompanyShift_V2035(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update company shift
        /// </summary>
        /// <param name="companyShift">Gewt company shift details</param>
        /// <returns>updated company shift details</returns>
        public CompanyShift UpdateCompanyShift_V2035(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyShift_V2035(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get all company shift related to selected companies ids
        /// </summary>
        /// <param name="idCompanies">Get selected companies ids</param>
        /// <returns>List of company shifts details</returns>
        public List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2035(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsByIdCompany_V2035(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CompanyShift> GetEmployeeRelatedCompanyShifts_V2035(Int32 idEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeRelatedCompanyShifts_V2035(workbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2035(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2035(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2035(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2035(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2036(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2036(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// [001][2018-07-03][skhade][HRM-M042-06] Add and Edit Attendance.
        /// This method is used to add employee attendance. 
        /// </summary>
        /// <param name="employeeAttendance">The employee attendance.</param>
        /// <returns>The employee attendance with idEmployeeAttendance.</returns>
        public EmployeeAttendance AddEmployeeAttendance_V2036(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2036(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2018-07-03][skhade][HRM-M042-06] Add and Edit Attendance.
        /// This method is used to update employee attendance.
        /// </summary>
        /// <param name="employeeAttendance">The employee attendance.</param>
        /// <returns>True if updated else false.</returns>
        public bool UpdateEmployeeAttendance_V2036(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeAttendance_V2036(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to add employee attendance with clock id from excel
        /// </summary>
        /// <param name="employeeAttendanceList">Get employee attendance list details</param>
        /// <returns>If true added employee attendance with clock from excel otherwise false</returns>
        public List<EmployeeAttendance> AddEmpAttendanceWithClockIdFromExcel_V2036(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmpAttendanceWithClockIdFromExcel_V2036(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to add employee attendance with clock id from excel
        /// </summary>
        /// <param name="employeeAttendanceList">Get employee attendance list details</param>
        /// <returns>If true added employee attendance with clock from excel otherwise false</returns>
        public List<EmployeeAttendance> AddEmployeeImportAttendance_V2038(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeImportAttendance_V2038(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2036(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2036(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2036(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2036(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateExitEmployeeStatusInActive(DateTime currentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateExitEmployeeStatusInActive(workbenchConnectionString, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        public List<EmployeeShift> GetEmployeeShiftsByIdEmployee(Int32 idEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeShiftsByIdEmployee(workbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Employee GetEmployeeByIdEmployee_V2036(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2036(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Employee AddEmployee_V2036(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2036(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employee;
        }

        public Employee AddEmployee_V2042(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2042(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employee;
        }

        public Employee AddEmployee_V2046(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2046(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employee;
        }


        public CompanyShift GetAnnualScheduleByIdCompanyShift(Int32 idCompanyShift, Int32 year)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetAnnualScheduleByIdCompanyShift(workbenchConnectionString, idCompanyShift, year);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<Employee> GetExitEmployeeToUpdateStatusInActive(DateTime currentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetExitEmployeeToUpdateStatusInActive(workbenchConnectionString, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public bool IsExistDocumentNumberInAnotherEmployee(string employeeDocumentNumber, Int32 idEmployee, Int32 employeeDocumentIdType)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.IsExistDocumentNumberInAnotherEmployee(workbenchConnectionString, employeeDocumentNumber, idEmployee, employeeDocumentIdType);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [000][04-12-2019][skhade][GEOS2-1938] Check duplicated "ClockTimeId" documents only in the plants of the employee contract
        /// </summary>
        /// <param name="employeeDocumentNumber"></param>
        /// <param name="idEmployee"></param>
        /// <param name="employeeDocumentIdType"></param>
        /// <param name="idCompany"></param>
        /// <returns></returns>
        public bool IsExistDocumentNumberInAnotherEmployee_V2038(string employeeDocumentNumber, Int32 idEmployee, Int32 employeeDocumentIdType, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsExistDocumentNumberInAnotherEmployee_V2038(workbenchConnectionString, employeeDocumentNumber, idEmployee, employeeDocumentIdType, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2037(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2037(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2037(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2037(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employees with anniversary date
        /// </summary>
        /// <param name="idCompany">Get selected companies ids</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>List of employees details</returns>
        public List<Employee> GetEmployeesWithAnniversaryDate_V2037(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithAnniversaryDate_V2037(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Company> GetCompaniesDetailByIds(string companyIds)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompaniesDetailByIds(workbenchConnectionString, companyIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeeShifts(string employeeIds)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeeShifts(workbenchConnectionString, employeeIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Employee GetEmployeeByIdEmployee_V2038(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2038(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public Employee GetEmployeeByIdEmployee_V2042(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2042(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Employee GetEmployeeByIdEmployee_V2046(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2046(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get employee enjoyed leave hours
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="idEmployeeLeave">Get employee leave id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Employee annual leave details</returns>
        public EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2038(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeEnjoyedLeaveHours_V2038(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to check employee enjoyed all annual leave or not
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="idEmployeeLeave">Get employee leave id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>If true employee enojyed all annual leaves otherwise false</returns>
        public bool IsEmployeeEnjoyedAllAnnualLeaves_V2038(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsEmployeeEnjoyedAllAnnualLeaves_V2038(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to update employee details
        /// </summary>
        /// <param name="employee">Get employee details</param>
        /// <returns>If true updated employee otherwise false</returns>
        public bool UpdateEmployee_V2038(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2038(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is to update employee details
        /// </summary>
        /// <param name="employee">Get employee details</param>
        /// <returns>If true updated employee otherwise false</returns>
        public bool UpdateEmployee_V2042(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2042(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }

        public bool UpdateEmployee_V2046(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2046(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public bool UpdateEmployeeHasWelcomeMessageBeenReceived(Int32 idEmployee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployeeHasWelcomeMessageBeenReceived(WorkbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isUpdated;
        }


        public Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard()
        {

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeForWelcomeBoard(WorkbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2039(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2039(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart(Int32 idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2039(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2039(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2039(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2039(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2039(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2039(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Department> GetAllEmployeesByDepartmentByIdCompany_V2039(string idCompany, long selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany_V2039(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForLeaveByIdCompany_V2039(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany_V2039(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeContractSituation> GetEmployeeContracts(string idEmployees)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeContracts(workbenchConnectionString, idEmployees);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CompanyDepartment> GetNumberOfWorkStationByIdDepartment(string idsDepartment, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetNumberOfWorkStationByIdDepartment(workbenchConnectionString, idsDepartment, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<CompanyDepartment> GetSizeByIdDepartmentArea(string departmentAreaIds, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSizeByIdDepartmentArea(workbenchConnectionString, departmentAreaIds, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2039(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2039(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }


        public Dictionary<string, decimal> GetPlantWorkHours(Int32 idCompany, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantWorkHours(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Double GetCompanySize(Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanySize(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public JobDescription AddJobDescription_V2040(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddJobDescription_V2040(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public JobDescription UpdateJobDescription_V2040(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateJobDescription_V2040(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetAllJobDescriptions_V2040()
        {
            List<JobDescription> jobDescriptions = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                jobDescriptions = mgr.GetAllJobDescriptions_V2040(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return jobDescriptions;
        }

        public List<JobDescription> GetIsMandatoryNotExistInJobDescriptionParent(Int32 IdJobDescription)
        {

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetIsMandatoryNotExistInJobDescriptionParent(workbenchConnectionString, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }


        }

        public bool UpdateIsMandatoryInJobDescriptionParent(Int32 IdJobDescription)
        {

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateIsMandatoryInJobDescriptionParent(workbenchConnectionString, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetOrganizationHierarchy_V2040(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2040(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<CompanySchedule> GetEmployeeCompanySchedule(Int32 idEmployee, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeCompanySchedule(workbenchConnectionString, idEmployee, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2041(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2050(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Department> GetAllEmployeesByDepartmentByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany_V2041(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Employee GetEmployeeCurrentDetail(Int32 idUser, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeCurrentDetail(workbenchConnectionString, idUser, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetAllEmployeesByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2041(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2046(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2046(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesShortDetail(string idCompany, string selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission,string firstName,string lastName)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesShortDetail(workbenchConnectionString,  idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, firstName,lastName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Department> GetLengthOfServiceByDepartment_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLengthOfServiceByDepartment_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Company> GetEmployeesCountByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetBirthdaysOfEmployeesByYear_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBirthdaysOfEmployeesByYear_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByDepartmentAreaByIdCompany_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByGenderByIdCompany_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByContractByIdCompany_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesForDashboard_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDocumentsExpirationForDashboard_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetEmployeesWithExitDateForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithExitDateForDashboard_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestContractExpirationForDashboard_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetEmployeesWithAnniversaryDate_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithAnniversaryDate_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2041(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2050(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<JobDescription> GetEmployeesForJobDescription(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForJobDescription(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetOrganizationHierarchy_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<JobDescription> GetOrganizationHierarchy_V2050(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2050(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<LookupValue> GetOrganizationalChartDepartmentArea_V2041(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea_V2041(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2041(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2046(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2046(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2041(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2041(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2041(Int32 idEmployee, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLatestJobDescriptionsByIdEmployee_V2041(workbenchConnectionString, idEmployee, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2041(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2041(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return employees;
        }

        public List<Employee> GetAllEmployeesForLeaveByIdCompany_V2041(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany_V2041(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeLeave> GetUpcomingEmployeeLeaves_V2044(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingEmployeeLeaves_V2044(workbenchConnectionString, idCompany, ref ToEmailList, ref ccEmailList, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2044(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2044(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2044(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2044(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2045(Int32 idEmployee, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLatestJobDescriptionsByIdEmployee_V2045(workbenchConnectionString, idEmployee, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2045(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2045(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetEmployeeAttendanceShortDetail(string idCompany, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAttendanceShortDetail(workbenchConnectionString, idCompany, idsOrganization, idsDepartment, idPermission,fromDate,toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetTodayBirthdayOfEmployees_V2045(long selectedPeriod = 0, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayBirthdayOfEmployees_V2045(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// [001][2020-07-25][cpatil] GEOS2-2452 The email notifications from HRM are sent also to the users with Inactive status.
        /// [001] The upcoming company holidays.
        /// </summary>
        /// <param name="emails">The emails list.</param>
        /// <param name="currentDate">Get current date.</param>
        /// <returns>The list of company holidays.</returns>
        public List<CompanyHoliday> GetUpcomingCompanyHolidays_V2045(ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingCompanyHolidays_V2045(workbenchConnectionString, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetUpcomingEmployeeLeaves_V2045(Int32 idCompany, ref List<string> ToEmailList, ref List<string> ccEmailList, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingEmployeeLeaves_V2045(workbenchConnectionString, idCompany, ref ToEmailList, ref ccEmailList, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get today employee company anniversaries
        /// </summary>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="currentDate">Get current date</param>
        /// <returns>List of employee details</returns>
        public List<Employee> GetTodayEmployeeCompanyAnniversaries_V2045(long selectedPeriod, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayEmployeeCompanyAnniversaries_V2045(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard_V2045()
        {

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeForWelcomeBoard_V2045(WorkbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<EmployeeLeave> AddEmployeeLeavesFromList_V2045(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeLeavesFromList_V2045(MainWorkbenchConnectionString, workbenchConnectionString, Properties.Settings.Default.EmpLeaveAttachment, employeeLeaves, fileInBytes, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to update employee leave 
        /// </summary>
        /// <param name="employeeLeave">Get employee leave details</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>Employee leave details</returns>
        public EmployeeLeave UpdateEmployeeLeave_V2045(EmployeeLeave employeeLeave, long selectedPeriod)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeLeave_V2045(MainWorkbenchConnectionString, workbenchConnectionString, employeeLeave, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2045(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2045(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeContractSituation> GetEmployeeContractExpirations(Int32 idCompany, ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeContractExpirations(workbenchConnectionString, idCompany, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateEmployeeContractWarningDate(List<EmployeeContractSituation> employeeContractExpirationList, DateTime? currentDate = null)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeContractWarningDate(mainServerWorkbenchConnectionString, employeeContractExpirationList, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public JobDescription AddJobDescription_V2046(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddJobDescription_V2046(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public JobDescription UpdateJobDescription_V2046(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateJobDescription_V2046(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetAllJobDescriptions_V2046()
        {
            List<JobDescription> jobDescriptions = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                jobDescriptions = mgr.GetAllJobDescriptions_V2046(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return jobDescriptions;
        }

        /// <summary>
        /// This method is to get employee enjoyed leave hours
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="idEmployeeLeave">Get employee leave id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <param name="idCompany">Get company id</param>
        /// <returns>Employee annual leave details</returns>
        public EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2050(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeEnjoyedLeaveHours_V2050(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        /// <summary>
        /// This method is to check employee enjoyed all annual leave or not
        /// </summary>
        /// <param name="idEmployee">Get employee id</param>
        /// <param name="idEmployeeLeave">Get employee leave id</param>
        /// <param name="selectedPeriod">Get selected period</param>
        /// <returns>If true employee enojyed all annual leaves otherwise false</returns>
        public bool IsEmployeeEnjoyedAllAnnualLeaves_V2050(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsEmployeeEnjoyedAllAnnualLeaves_V2050(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeAttendance> GetEmployeeAttendanceForNewLeave(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAttendanceForNewLeave(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2050(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2050(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2060(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2060(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public EmployeeAttendance AddEmployeeAttendance_V2060(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2060(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2060(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2060(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeAttendance> GetEmployeeAttendanceShortDetail_V2070(string idCompany, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAttendanceShortDetail_V2070(workbenchConnectionString, idCompany, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

    }
}
