using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.FileReplicator;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Services.Contracts;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
        /// This method is to get all company shift related to InUse
        /// </summary>
        /// <param name="companyShift">Get InUse</param>
        /// <returns>List of company shifts details</returns>
        /// [GEOS2-2716][sudhir.jangra][19/10/2022]
        public CompanyShift UpdateCompanyShift_V2330(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyShift_V2330(workbenchConnectionString, companyShift);
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

        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2090(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2090(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
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

        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2100(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2100(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2100(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2100(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProfessionalSkill> GetAllProfessionalSkill()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProfessionalSkill(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetLatestProfessionalSkillCode()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestProfessionalSkillCode(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Int64 AddProfessionalSkill(ProfessionalSkill professionalSkill)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddProfessionalSkill(workbenchConnectionString, professionalSkill);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateProfessionalSkill(ProfessionalSkill professionalSkill)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalSkill(workbenchConnectionString, professionalSkill);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2110(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2110(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission,  fromDate,  toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2110(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2110(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2110(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2110(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2110(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2110(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Department> GetAllEmployeesByDepartmentByIdCompany_V2120(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany_V2120(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForLeaveByIdCompany_V2120(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany_V2120(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2120(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2120(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetLatestProfessionalObjectiveCode()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestProfessionalObjectiveCode(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProfessionalObjective> GetProfessionalObjectives()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalObjectives(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ProfessionalObjective AddProfessionalObjective(ProfessionalObjective professionalObjective)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddProfessionalObjective(workbenchConnectionString, professionalObjective);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateProfessionalObjective(UInt64 IdProfessionalObjective, ProfessionalObjective professionalObjective)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalObjective(workbenchConnectionString, IdProfessionalObjective, professionalObjective);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<EmployeeAttendance> GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2120(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2120(workbenchConnectionString, commaSeparatedCompanyIds, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate, commaSeparatedEmployeeIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2120(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2120(workbenchConnectionString, commaSeparatedCompanyIds, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate, commaSeparatedEmployeeIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ProfessionalTask> GetAllProfessionalTask()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProfessionalTask(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public string GetLatestProfessionalTaskCode()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestProfessionalTaskCode(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public ProfessionalTask GetProfessionalTaskDetailsById(UInt64 IdProfessionalTask)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTaskDetailsById(workbenchConnectionString, IdProfessionalTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ProfessionalObjective> GetProfessionalObjectives_ForDDL()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalObjectives_ForDDL(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ProfessionalSkill> GetProfessionalSkillsForSelection()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalSkillsForSelection(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateProfessionalTask(ProfessionalTask professionalTask)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTask(workbenchConnectionString, professionalTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public UInt64 AddProfessionalTask(ProfessionalTask professionalTask)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddProfessionalTask(workbenchConnectionString, professionalTask);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<ProfessionalTraining> GetAllProfessionalTraining()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProfessionalTraining(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsDeleteProfessionalTraining(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.IsDeleteProfessionalTraining(workbenchConnectionString, IdProfessionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeHoliday> GetEmployeeHolidays(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS23009
        public bool IsEmployeeEnjoyedAllAnnualLeaves_V2140(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsEmployeeEnjoyedAllAnnualLeaves_V2140(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public EmployeeAnnualLeave GetEmployeeEnjoyedLeaveHours_V2140(Int32 idEmployee, Int32 idEmployeeLeave, long selectedPeriod, Int32 idCompany = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeEnjoyedLeaveHours_V2140(workbenchConnectionString, idEmployee, idEmployeeLeave, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        // Add
        public ProfessionalTraining AddProfessionalTraining(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }




        // Get code
        public string GetLatestProfessionalTrainingCode()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestProfessionalTrainingCode(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }




        //Delete
        public bool Is_Delete_Professional_Training(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.Is_Delete_Professional_Training(workbenchConnectionString, IdProfessionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }



        // Update
        public bool UpdateProfessionalTraining(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining(workbenchConnectionString, professionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public EmployeeShortDetailForMail GetJobDecriptionInformation(Int32 idJobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetJobDecriptionInformation( idJobDescription, workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //For task GEOS2-2844
        public Employee GetEmployeeByIdEmployee_V2150(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2150(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateEmployee_V2150(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2150(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        public Employee AddEmployee_V2150(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2150(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        public List<Employee> GetAllTrainers()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTrainers(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllResponsibles()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllResponsibles(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // created to get training details by id 
        public ProfessionalTraining GetProfessionalTrainingDetailsById(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById(workbenchConnectionString, IdProfessionalTraining);

                
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        // get details of No longer trainer
        public Employee GetNoLongerTrainer(Int32 idEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetNoLongerTrainer(workbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //For task GEOS2-2844
        public Employee GetEmployeeByIdEmployee_V2160(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2160(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateEmployee_V2160(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2160(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        public Employee AddEmployee_V2160(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2160(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        #region GEOS2-2607 Update Leave Summary Grid View Screen
        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2170(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2170(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion


        public List<Employee> GetAllEmployeesByIdCompany_V2170(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2170(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool UpdateEmployee_V2170(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2170(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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


        //[GEOS2-3095]
        public List<EmployeeLeave> AddEmployeeLeavesFromList_V2170(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeLeavesFromList_V2170(MainWorkbenchConnectionString, workbenchConnectionString, Properties.Settings.Default.EmpLeaveAttachment, employeeLeaves, fileInBytes, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }





        //[GEOS2-3095]
        public EmployeeAttendance AddEmployeeAttendance_V2170(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2170(workbenchConnectionString, employeeAttendance);
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
        public EmployeeLeave UpdateEmployeeLeave_V2170(EmployeeLeave employeeLeave, long selectedPeriod)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeLeave_V2170(MainWorkbenchConnectionString, workbenchConnectionString, employeeLeave, selectedPeriod);
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
        /// This method is used to update employee attendance.
        /// </summary>
        /// <param name="employeeAttendance">The employee attendance.</param>
        /// <returns>True if updated else false.</returns>
        public bool UpdateEmployeeAttendance_V2170(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeAttendance_V2170(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3095]
        /// <summary>
        /// This method is to add employee attendance with clock id from excel
        /// </summary>
        /// <param name="employeeAttendanceList">Get employee attendance list details</param>
        /// <returns>If true added employee attendance with clock from excel otherwise false</returns>
        public List<EmployeeAttendance> AddEmployeeImportAttendance_V2170(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeImportAttendance_V2170(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public Int64 UpdateExitEmployeeStatusActive(DateTime currentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateExitEmployeeStatusActive(workbenchConnectionString, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-3093 Update table2. Show new leave columns and all employees in the Automatic report number 11 email
        public List<EmployeeHoliday> GetEmployeeHolidays_V2180(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays_V2180(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #endregion
        //[GEOS2-3086]
        public List<ProfessionalSkill> GetAllProfessionalSkillsForTraining()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllProfessionalSkillsForTraining(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        //[GEOS2-3086]
        // Add
        public ProfessionalTraining AddProfessionalTraining_V2180(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2180(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }

        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2180(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2180(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3087]
        // created to get training details by id 
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2180(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2180(workbenchConnectionString, IdProfessionalTraining);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3087]
        // Update
        public bool UpdateProfessionalTraining_V2180(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2180(workbenchConnectionString, professionalTraining);
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
        public List<JobDescription> GetJobDescriptionById_V2180(string idJobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetJobDescriptionById_V2180(workbenchConnectionString, idJobDescription);
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
        public List<Employee> GetITEmployeeDetails_V2180(string idCompanies, long selectedPeriod, string IdJobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetITEmployeeDetails_V2180(workbenchConnectionString, idCompanies, selectedPeriod, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2180(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2180(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2846]
        // created to get training details by id 
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2190(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2190(workbenchConnectionString, IdProfessionalTraining);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2846]
        // Update
        public bool UpdateProfessionalTraining_V2190(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2190(workbenchConnectionString, professionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3317]
        public List<Employee> GetAllEmployeesWithoutInactiveStatus()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesWithoutInactiveStatus(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-3317]
        // Add
        public ProfessionalTraining AddProfessionalTraining_V2200(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2200(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }

        //[GEOS2-3317]
        // Update
        public bool UpdateProfessionalTraining_V2200(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2200(workbenchConnectionString, professionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3317]
        public List<Employee> GetAllTrainers_V2200()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTrainers_V2200(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllResponsibles_V2200()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllResponsibles_V2200(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2200(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2200(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-3317]
        public List<Employee> GetAllEmployeesWithoutInactiveStatus_V2200(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesWithoutInactiveStatus_V2200(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3453]
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2210(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2210(workbenchConnectionString, IdProfessionalTraining);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3454
        public List<Employee> GetAllEmployeesForProfessionalTrainingResults(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForProfessionalTrainingResults(workbenchConnectionString, IdProfessionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
       


        //GEOS2-3454
        public List<ProfessionalSkill> GetProfessionalTrainingSkillListForResult(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingSkillListForResult(workbenchConnectionString, IdProfessionalTraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3453
        public bool UpdateProfessionalTrainingResult(List<ProfessionalTrainingResults> trainingResultList)
        {
            try
            {
                string WorkbenchContext = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTrainingResult(WorkbenchContext, trainingResultList, Properties.Settings.Default.TraineeResultFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3454
        public byte[] GetEmployeeDocumentFile_V2210(String employeeCode, object employeeSubObject)
        {
            try
            {
                return mgr.GetEmployeeDocumentFile_V2210(employeeCode, employeeSubObject, Properties.Settings.Default.EmpEducationQualificationFilesPath,
                                                                                    Properties.Settings.Default.EmpProfessionalEducationFilesPath,
                                                                                    Properties.Settings.Default.EmpContractSituationFilesPath,
                                                                                    Properties.Settings.Default.EmpIdentificationDocumentFilesPath,
                                                                                    Properties.Settings.Default.EmpJobDescriptionsFilesPath, Properties.Settings.Default.TraineeResultFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3454
        public ProfessionalTraining AddProfessionalTraining_V2210(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2210(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }
        //GEOS2-3453
        public bool UpdateProfessionalTraining_V2210(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2210(workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public bool UpdateProfessionalTraining_V2220(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2220(workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //GEOS2-3456
        public Employee GetEmployeeByIdEmployee_V2220(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2220(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3456
        public ProfessionalTraining AddProfessionalTraining_V2220(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2220(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }

        //[GEOS2-2848]
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2230(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2230(workbenchConnectionString, IdProfessionalTraining, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3502
        public ProfessionalTraining AddProfessionalTraining_V2230(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2230(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }

        //GEOS2-3501
        public bool UpdateProfessionalTraining_V2230(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2230(workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        // [GEOS2-3554]
        public Employee AddEmployee_V2230(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2230(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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


        // [GEOS2-3554]
        public bool UpdateEmployee_V2230(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2230(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        //[GEOS2-3504]
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2240(UInt64 IdProfessionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2240(workbenchConnectionString, IdProfessionalTraining, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //GEOS2-2849
        public ProfessionalTraining AddProfessionalTraining_V2240(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2240(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }

        //GEOS2-2849
        public bool UpdateProfessionalTraining_V2240(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2240(workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2250(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2250(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<JobDescription> GetOrganizationHierarchy_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2250(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Company> GetEmployeesCountByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany_V2250(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LookupValue> GetOrganizationalChartDepartmentArea_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea_V2250(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2250(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2250(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByDepartmentAreaByIdCompany_V2250(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByGenderByIdCompany_V2250(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByContractByIdCompany_V2250(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2250(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany_V2250(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        public List<Employee> GetAllEmployeesWithoutInactiveStatus_V2250(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesWithoutInactiveStatus_V2250(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3504]
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2250(UInt64 IdProfessionalTraining, string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2250(workbenchConnectionString, IdProfessionalTraining, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);


            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeHoliday> GetEmployeeHolidays_V2250(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays_V2250(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [GEOS2-3562]
        public bool UpdateEmployee_V2260(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2260(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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


        // [GEOS2-3562]
        public Employee AddEmployee_V2260(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2260(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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


        public List<JobDescription> GetEmployeeLatestJobDescriptionsByIdEmployee_V2260(Int32 idEmployee, Int64 selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLatestJobDescriptionsByIdEmployee_V2260(workbenchConnectionString, idEmployee, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetEmpDtlByEmailNoAndPeriod_V2260(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmailNoAndPeriod_V2260(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
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

        //[GEOS2-3697]
        public List<EmployeeLeave> AddEmployeeImportLeave(List<EmployeeLeave> employeeLeaveList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeImportLeave(mainServerWorkbenchConnectionString, employeeLeaveList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TravelExpenses> GetTravelExpenses(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //[GEOS2-3757][rdixit][12.08.2022]
        public List<Employee> GetExitEmployeeToUpdateStatusInActive_V2300(DateTime currentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                return mgr.GetExitEmployeeToUpdateStatusInActive_V2300(workbenchConnectionString, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2300(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2300(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3828][29.08.2022]
        public List<TravelExpenseStatus> GetAllTravelExpenseStatus()
        {
            List<TravelExpenseStatus> travelExpenseStatus = new List<TravelExpenseStatus>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenseStatus = mgr.GetAllTravelExpenseStatus(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenseStatus;
        }

        //[rdixit][GEOS2-3828][29.08.2022]
        public List<TravelExpenseWorkflowTransitions> GetAllWorkflowTransitions()
        {
            List<TravelExpenseWorkflowTransitions> travelExpenseWorkflowTransitions = new List<TravelExpenseWorkflowTransitions>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenseWorkflowTransitions = mgr.GetAllWorkflowTransitions(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenseWorkflowTransitions;
        }

        //[rdixit][GEOS2-3828][29.08.2022]
        public List<TravelExpenses> GetTravelExpenses_V2301(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2301(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //[rdixit][GEOS2-3828][29.08.2022]
        public bool UpdateTravelExpensesStatus_V2301(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpensesStatus_V2301(travelExpense, ModifiedBy, ModificationDate, WorkbenchConnectionString);
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

        //[rdixit][GEOS2-3829][21.09.2022]
        public List<LogEntriesByTravelExpense> GetTravelExpenseChangelogs_V2320(Int64 IdEmployeeExpenseReport)
        {
            List<LogEntriesByTravelExpense> logEntriesByTravelExpense = new List<LogEntriesByTravelExpense>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                logEntriesByTravelExpense = mgr.GetTravelExpenseChangelogs_V2320(WorkbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntriesByTravelExpense;
        }

        //[rdixit][GEOS2-3829][21.09.2022]
        public bool UpdateTravelExpensesStatus_V2320(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpensesStatus_V2320(travelExpense, ModifiedBy, ModificationDate, WorkbenchConnectionString);
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
        //shubham[skadam] GEOS2-3842 Employee joining email is not sent since 1 y aprox 26 Sep 2022
        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2320(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2320(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3829][26.09.2022]
        public List<TravelExpenses> GetTravelExpenses_V2320(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2320(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }
        //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
        public List<LookupValue> GetEnumeratedList(Int32 key)
        {
            List<LookupValue> list = null;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                list = mgr.GetEnumeratedList(WorkbenchConnectionString,key, Properties.Settings.Default.EnumeratedImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }


        /// <summary>
        /// This method is used to update employee.
        /// </summary>
        /// <param name="employee">The employee</param>
        /// <returns>True, if employee is updated else false.</returns>
        public bool UpdateEmployee_V2320(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2320(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        //GEOS2-3420
        public Employee GetEmployeeByIdEmployee_V2320(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2320(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //GEOS2-3420
        public List<Employee> GetBackupEmployeeDetails(string idsEmployeeJobDescription, string idsDepartment)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBackupEmployeeDetails(workbenchConnectionString, idsEmployeeJobDescription, idsDepartment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3763] [rdixit] [06.10.2022]
        public List<UserLongTermAbsent> GetLongTermAbsentUsers()
        {
            List<UserLongTermAbsent> UserLongTermAbsent = new List<UserLongTermAbsent>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                UserLongTermAbsent = mgr.GetLongTermAbsentUsers(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return UserLongTermAbsent;
        }

        //[GEOS2-3763] [rdixit] [06.10.2022]
        public List<OrganizationHR> GetOrganizationHRBySite(Int32 IdSite)
        {
            List<OrganizationHR> HR = new List<OrganizationHR>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                HR = mgr.GetOrganizationHRBySite(workbenchConnectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return HR;
        }

        public List<EmployeeLeave> GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesByCompanyIdsAndEmployeeIds_V2320(workbenchConnectionString, commaSeparatedCompanyIds, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate, commaSeparatedEmployeeIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3957][rdixit][07.10.2022]
        public List<Expenses> EmployeeExpenseByExpenseReport(int IdEmployeeExpenseReport)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport(workbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }

        //[GEOS2-3957][rdixit][07.10.2022]
        public List<EmployeeExpensePhotoInfo> GetEmployeeExpenseImageInBytes(int idEmployeeExpense)
        {
            List<EmployeeExpensePhotoInfo> EmployeeExpensePhotoInfoList = new List<EmployeeExpensePhotoInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeExpensePhotoInfoList = mgr.GetEmployeeExpenseImageInBytes(workbenchConnectionString, idEmployeeExpense, Properties.Settings.Default.ExpensesAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeExpensePhotoInfoList;
        }

        //[GEOS2-3957][rdixit][07.10.2022]
        public List<EmployeeWithInactiveBackupEmployees> GetEmployeesWithInactiveBackupEmployeeDetails()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithInactiveBackupEmployeeDetails(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());

            }
        }



        //[GEOS2-3944][rdixit][19.10.2022]
        public EmployeeWithBackupEmployeeDetails GetEmployeeWithBackupEmployeeDetails(int IdEmployee, DateTime LeaveStartDate, DateTime LeaveEndDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeWithBackupEmployeeDetails(workbenchConnectionString, IdEmployee, LeaveStartDate, LeaveEndDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());

            }
        }

        //[sdeshpande][19.10.2022][GEOS2-3362]
        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2330(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2330(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Geos-2618][pjadhav][10/20/2022]
        public List<Employee> GetAllEmployeesByIdCompany_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2330(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][21.10.2022][GEOS2-3263]
        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2330(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3945][rdixit][31.10.2022]
        public List<OrganizationHR> GetOrganizationHRByCompany(Int32 IdCompany)
        {
            List<OrganizationHR> HR = new List<OrganizationHR>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                HR = mgr.GetOrganizationHRByCompany(workbenchConnectionString, IdCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return HR;
        }

        //[GEOS2-3846][rdixt][31.10.2022]
        public List<ProfessionalCategory> GetAllProfessionalCategoriesWithParents()
        {
            List<ProfessionalCategory> professionalCategories = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                professionalCategories = mgr.GetAllProfessionalCategoriesWithParents(workbenchConnectionString);
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

        //[GEOS2-2716][rdixit][31.10.2022]
        public Employee GetEmployeeByIdEmployee_V2330(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2330(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2716][rdixit][01.11.2022]
        public List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2330(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsByIdCompany_V2330(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2795][rdixit][01.11.2022]
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2330(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2795][rdixit][04.11.2022]
        public List<JobDescription> GetOrganizationHierarchy_V2330(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2330(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2795][rdixit][04.11.2022]
        public List<Company> GetEmployeesCountByIdCompany_V2330(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany_V2330(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2795][rdixit][04.11.2022]
        public List<LookupValue> GetOrganizationalChartDepartmentArea_V2330(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea_V2330(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2795][rdixit][04.11.2022]
        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2330(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2330(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3958][rdixit][02.11.2022]
        public List<Company> GetCompanies()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanies(workbenchConnectionString, Properties.Settings.Default.CountryFilePath);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-2716][04/11/2022][Added New Stored Procedure With In Use]
        /// <summary>
        /// This method is to add company shift
        /// </summary>
        /// <param name="companyShift">Get company shift details to add</param>
        /// <returns>Get added company shift details </returns>
        public CompanyShift AddCompanyShift_V2330(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddCompanyShift_V2330(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-2716][07/11/2022]
        /// <summary>
        /// This method is to add company shift
        /// </summary>
        /// <param name="companyShift">Get company shift assigned to employee or not</param>
        /// <returns>If it exist it return true </returns>
        public bool GetIsCompanyShiftAssignedToEmployee(Int32 idCompanyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetIsCompanyShiftAssignedToEmployee(workbenchConnectionString, idCompanyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2716][cpatil][08.11.2022]
        public List<CompanyShift> GetAllCompanyShiftsInUseByIdCompany(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsInUseByIdCompany(workbenchConnectionString, idCompanies);
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
        /// This method is to get company schedule related to id company
        /// </summary>
        /// <param name="idCompany">Get id company</param>
        /// <returns>List of company schedule details</returns>
        public List<CompanySchedule> GetCompanyScheduleByIdCompany_V2330(string idCompany)
        {
            List<CompanySchedule> companySchedules = new List<CompanySchedule>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companySchedules = mgr.GetCompanyScheduleByIdCompany_V2330(WorkbenchConnectionString, idCompany);
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
        /// [001][2020-07-25][cpatil] GEOS2-2452 The email notifications from HRM are sent also to the users with Inactive status.
        /// [002][2022-11-14][skadam] GEOS2-3754 HRM - Weekly email plant holidays (#HRM103) - Requested by Top management
        /// [001] The upcoming company holidays.
        /// </summary>
        /// <param name="emails">The emails list.</param>
        /// <param name="currentDate">Get current date.</param>
        /// <returns>The list of company holidays.</returns>
        public List<CompanyHoliday> GetUpcomingCompanyHolidays_V2330(ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingCompanyHolidays_V2330(workbenchConnectionString, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3853][14.11.2022]
        public List<EmployeeDocument> GetEmployeeDocumentExpiration(int idCompany, DateTime CurrentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDocumentExpiration(workbenchConnectionString, idCompany, CurrentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3853][17.11.2022]
        public bool UpdateEmployeeDocumentWarningDate(List<EmployeeDocument> employeeDocumentExpirationList, DateTime? currentDate = null)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeDocumentWarningDate(mainServerWorkbenchConnectionString, employeeDocumentExpirationList, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pjadhav][GEOS2-235][17.11.2022]
        public bool IsDeleteEmployeeHolidays(UInt64 IdCompanyHoliday)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.IsDeleteEmployeeHolidays(workbenchConnectionString, IdCompanyHoliday);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3981][sshegaonkar][17.11.2022]
        public List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2340(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsByIdCompany_V2340(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4022][24.11.2022]
        public List<TravelExpenses> GetTravelExpenses_V2340(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2340(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //[rdixit][GEOS2-4022][24.11.2022]
        public bool UpdateTravelExpenses_V2340(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2340(travelExpense, ModifiedBy, ModificationDate, WorkbenchConnectionString);
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

        //[rdixit][GEOS2-4025][28.11.2022]
        public string GetTravelExpenseReportLatestCode()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTravelExpenseReportLatestCode(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4025][28.11.2022]
        public List<Employee> GetEmployeesByIdSite(int idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesByIdSite(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4025][28.11.2022]
        public TravelExpenses AddTravelExpenseReport(TravelExpenses travelExpense, int idCreator)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTravelExpenseReport(travelExpense, idCreator, WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3263][28.11.2022]
        public List<EmployeeLeave> AddEmployeeLeavesFromList_V2340(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeLeavesFromList_V2340(MainWorkbenchConnectionString, workbenchConnectionString, Properties.Settings.Default.EmpLeaveAttachment, employeeLeaves, fileInBytes, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-3263][28.11.2022]
        public EmployeeLeave UpdateEmployeeLeave_V2340(EmployeeLeave employeeLeave, long selectedPeriod)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeLeave_V2340(MainWorkbenchConnectionString, workbenchConnectionString, employeeLeave, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3957][rdixit][15.12.2022]
        public List<EmployeeExpensePhotoInfo> GetEmployeeExpenseImageInBytes_V2340(int idEmployeeExpense)
        {
            List<EmployeeExpensePhotoInfo> EmployeeExpensePhotoInfoList = new List<EmployeeExpensePhotoInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeExpensePhotoInfoList = mgr.GetEmployeeExpenseImageInBytes_V2340(workbenchConnectionString, idEmployeeExpense, Properties.Settings.Default.ExpensesAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeExpensePhotoInfoList;
        }

        //[rdixit][GEOS2-4086][16.12.2022]
        public List<EmployeeDocument> GetEmployeeDocumentExpiration_V2340(int idCompany, DateTime CurrentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDocumentExpiration_V2340(workbenchConnectionString, idCompany, CurrentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4066][03.01.2023]
        public List<TravelExpenses> GetTravelExpenses_V2360(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2360(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //[rdixit][GEOS2-3943][04.01.2023]
        public List<TravelExpenses> GetAllTravelExpensewithPermission_V2360(string Plant, string idDepartment, string idOrganization, int permission)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetAllTravelExpensewithPermission_V2360(WorkbenchConnectionString, Plant, idDepartment, idOrganization, permission, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }


        public List<Employee> GetAllEmployeesShortDetail_V2350(string idCompany, string selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, string firstName, string lastName)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesShortDetail_V2350(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, firstName, lastName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][05-01-2023][GEOS2-4055]
        public List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2400(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2400(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-3906]
        /// <summary>
        /// This method is to add employee attendance with clock id from excel
        /// </summary>
        /// <param name="employeeAttendanceList">Get employee attendance list details</param>
        /// <returns>If true added employee attendance with clock from excel otherwise false</returns>
        public List<EmployeeAttendance> AddEmployeeImportAttendance_V2350(List<EmployeeAttendance> employeeAttendanceList)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeImportAttendance_V2350(mainServerWorkbenchConnectionString, employeeAttendanceList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4043]
        public bool UpdateIsMandatoryInJobDescriptionParentByIdParent(Int32 IdJobDescription)
        {

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateIsMandatoryInJobDescriptionParentByIdParent(workbenchConnectionString, IdJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[sshegaonkar][11-01-2023][GEOS2-4003]
        public List<Employee> GetAllEmployeesByIdCompany_V2360(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2360(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4010][rdixit][20.01.2023]
        public List<EmployeeExpensePhotoInfo> GetEmployeeExpenseImageInBytes_V2350(int idEmployeeExpense)
        {
            List<EmployeeExpensePhotoInfo> EmployeeExpensePhotoInfoList = new List<EmployeeExpensePhotoInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeExpensePhotoInfoList = mgr.GetEmployeeExpenseImageInBytes_V2350(workbenchConnectionString, idEmployeeExpense, Properties.Settings.Default.ExpensesAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeExpensePhotoInfoList;
        }
        //[Gulab lakade][GEOS2-4026][20.01.2023]
        public bool UpdateTravelExpenses_V2350(TravelExpenses travelExpense, int ModifiedBy, DateTime ModificationDate)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2350(travelExpense, ModifiedBy, ModificationDate, WorkbenchConnectionString);
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
        //Shubham[skadam] GEOS2-4175 Expense report not show proper for  status draft  03 02 2023
        public List<Currency> GetCurrencies_V2360()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetCurrencies_V2360(WorkbenchConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][09.03.2023][GEOS2-4239]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2370(int IdEmployeeExpenseReport)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2370(workbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }

        //[rdixit][15.03.2023][GEOS2-4178]
        public EmployeeMealBudget GetMealExpenseByEmployyeAndCompany(int idCompany, int idEmployee)
        {
            EmployeeMealBudget EmployeeMealBudget = new EmployeeMealBudget();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeMealBudget = mgr.GetMealExpenseByEmployyeAndCompany(idCompany, idEmployee, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeMealBudget;
        }

        //[cpatil][15.03.2023][GEOS2-3981]
        public List<EmployeeShift> GetEmployeeShiftsByIdEmployee_V2370(Int32 idEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeShiftsByIdEmployee_V2370(workbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[cpatil][15.03.2023][GEOS2-3981]
        public List<Employee> GetEmpDtlByEmpDocNoAndPeriod_V2370(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmpDocNoAndPeriod_V2370(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
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

        public List<Employee> GetEmpDtlByEmailNoAndPeriod_V2370(string EmployeeDocumentNumbers, string idCompany, Int64 selectedPeriod)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employees = mgr.GetEmpDtlByEmailNoAndPeriod_V2370(WorkbenchConnectionString, EmployeeDocumentNumbers, idCompany, selectedPeriod);
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

        //[rdixit][15.03.2023][GEOS2-4180]
        public List<MealAllowance> GetMealAllowances()
        {
            List<MealAllowance> MealAllowanceList = new List<MealAllowance>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MealAllowanceList = mgr.GetMealAllowances(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return MealAllowanceList;
        }

        //[rdixit][20.03.2023][GEOS2-4181]
        public bool UpdateMealAllowance(List<EmployeeMealBudget> employeeMealBudget)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateMealAllowance(employeeMealBudget, WorkbenchConnectionString);
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

        //[rdixit][24.03.2023][GEOS2-4017]
        public bool GetEmployeeExpenseReportTemplate(string code, Employee selectedEmployee, TravelExpenseStatus status, string comments, string userCompanyEmail)
        {
            try
            {
                HRMMail mailServer = new HRMMail();
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeExpenseReportTemplate(mailServer, workbenchConnectionString, code, selectedEmployee, status, comments, userCompanyEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][24.03.2023][GEOS2-4017]
        public List<TravelExpenseWorkflowTransitions> GetAllWorkflowTransitions_V2370()
        {
            List<TravelExpenseWorkflowTransitions> travelExpenseWorkflowTransitions = new List<TravelExpenseWorkflowTransitions>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenseWorkflowTransitions = mgr.GetAllWorkflowTransitions_V2370(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenseWorkflowTransitions;
        }
        //[Sudhir.Jangra][GEOS2-4023][19/04/2023]
        /// <summary>
        /// This method is to add company shift
        /// </summary>
        /// <param name="companyShift">Get company shift details to add</param>
        /// <returns>Get added company shift details </returns>
        public CompanyShift AddCompanyShift_V2380(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddCompanyShift_V2380(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4036][19/04/2023]
        public List<CompanyShift> GetAllCompanyShiftsByIdCompany_V2380(string idCompanies = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllCompanyShiftsByIdCompany_V2380(workbenchConnectionString, idCompanies);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4023][20/04/2023]
        /// <summary>
        /// This method is to get all company shift related to InUse
        /// </summary>
        /// <param name="companyShift">Get InUse</param>
        /// <returns>List of company shifts details</returns>
        /// [GEOS2-2716][sudhir.jangra][19/10/2022]
        public CompanyShift UpdateCompanyShift_V2380(CompanyShift companyShift)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyShift_V2380(workbenchConnectionString, companyShift);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4297][rdixit][08.05.2023]
        public List<Company> GetCompanies_V2390()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanies_V2390(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4476][22.05.2023]
        public ProfessionalTraining GetProfessionalTrainingDetailsById_V2390(UInt64 IdProfessionalTraining, string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetProfessionalTrainingDetailsById_V2390(workbenchConnectionString, IdProfessionalTraining, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4536][01/06/2023]
        public List<Employee> GetAllEmployeesByIdCompany_V2400(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2400(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4285][08.06.2023]
        public byte[] HRMGetEmployeesImage(string employeeCode)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;

            if (File.Exists(Properties.Settings.Default.EmployeesProfileImage + employeeCode + ".png"))
            {
                fileUploadPath = Properties.Settings.Default.EmployeesProfileImage + employeeCode + ".png";
            }
            else
            {
                fileUploadPath = Properties.Settings.Default.EmployeesProfileImage + employeeCode + ".jpg";
            }

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4515][09.06.2023]
        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2400(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany_V2400(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-2456][rdixit][12.06.2023]
        public Employee GetEmployeeByIdEmployee_V2400(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2400(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4438][rdixit][15.06.2023]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2400(int IdEmployeeExpenseReport)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2400(workbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }


        //[Sudhir.Jangra][GEOS2-4037][29/06/2023]
        public Employee GetEmployeeByIdEmployee_V2410(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2410(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4439][rdixit][03.07.2023]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2410(int IdEmployeeExpenseReport)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2410(workbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }
        //[GEOS2-4471][rdixit][04.07.2023]
        public bool UpdateTravelExpenses_V2410(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2410(travelExpense, modifiedBy, modificationDate, expenseLogList, expenseList, WorkbenchConnectionString);
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

        //[GEOS2-4472][rdixit][05.07.2023]
        public Dictionary<string, byte[]> GetEmployeeExpenseReport(string plantName, string EmployeeCode, string ExpenseCode)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;
            string ExpenseReportFilePath = string.Empty;
            fileUploadPath = Properties.Settings.Default.ExpenseReportPath.Replace("{0}", plantName);
            Dictionary<string, byte[]> ResultFile = new Dictionary<string, byte[]>();
            if (!fileUploadPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                fileUploadPath += Path.DirectorySeparatorChar;
            }
            fileUploadPath = fileUploadPath + EmployeeCode + @"\" + ExpenseCode + @"\";
            if (!System.IO.Directory.Exists(fileUploadPath))
            {
                System.IO.Directory.CreateDirectory(fileUploadPath);
            }
            ExpenseReportFilePath = fileUploadPath;
            string search = ExpenseCode + "_" + '*' + ".pdf";
            FileInfo[] files = new DirectoryInfo(fileUploadPath).GetFiles(search).ToArray();


            List<string> finalpdflist = new List<string>();
            var allfiles = files.ToList();

            if (allfiles.Count() > 0)
            {
                allfiles.RemoveAll(r => r.Name.Contains("Receipts"));

                var test = allfiles.Max(f => f.LastWriteTimeUtc);
                fileUploadPath = files.FirstOrDefault(k => k.LastWriteTimeUtc == test).FullName;
                #region
                /*
                List<string> revPDFList = new List<string>();
                foreach (var itemPDF in allfiles)
                {
                    string substringitem = itemPDF.ToString().Split('\\').Last();
                    revPDFList.Add(substringitem);
                }

                List<string> maxNumberList = new List<string>();
                foreach (var maxNumberitem in revPDFList)
                {
                    int indexofrev = maxNumberitem.IndexOf("rev");
                    string substring = "00";
                    if (indexofrev == -1)
                    {
                        substring = "00";
                    }
                    else
                    {
                        substring = maxNumberitem.Substring(indexofrev + 3, 2);
                    }
                    maxNumberList.Add(substring);
                }

                string maxNumber = maxNumberList.Max();


                if (maxNumber != "00")
                {
                    string MaxRevFile = revPDFList.Where(k => k.Contains("rev" + maxNumber + "_EN.pdf")).FirstOrDefault();
                    fileUploadPath = fileUploadPath + MaxRevFile;
                }
                else
                {
                    var test = files.Min(f => f.CreationTime);
                    fileUploadPath = files.FirstOrDefault(k => k.CreationTime == test).FullName;
                }
                */
                #endregion
            }
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                if (bytes != null)
                    ResultFile.Add(Path.GetFileName(fileUploadPath), bytes);
                else
                    ResultFile.Add(ExpenseReportFilePath, bytes);
                return ResultFile;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4472][rdixit][05.07.2023]
        public List<TravelExpenses> GetTravelExpenses_V2410(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2410(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        #region GEOS2-3917&GEOS2-3916
        //Shubham[skadam]  GEOS2-3917 Include the “Draft” employees in Leaves 07 07 2023 
        //Shubham[skadam] GEOS2-3916 Include the “Draft” employees in Attendance 07 07 2023 
        public List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2410(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2410(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2410(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesForLeaveByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany_V2410(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Department> GetAllEmployeesByDepartmentByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany_V2410(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion
        //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023 
        public List<EmployeeLeave> AddEmployeeLeavesFromList_V2410(List<EmployeeLeave> employeeLeaves, byte[] fileInBytes, long selectedPeriod = 0)
        {
            try
            {
                string MainWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeLeavesFromList_V2410(MainWorkbenchConnectionString, workbenchConnectionString, Properties.Settings.Default.EmpLeaveAttachment, employeeLeaves, fileInBytes, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-4473 Improvements in Attendance and Leave registration using mobile APP (16/20)  11 07 2023
        public EmployeeAttendance AddEmployeeAttendance_V2410(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2410(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4038][11/07/2023]
        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2410(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2410(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4686][19/07/2023]
        public List<Employee> GetAllEmployeesByIdCompany_V2410(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2410(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4516][rdixit][01.08.2023]
        public List<Department> GetLengthOfServiceByDepartment_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLengthOfServiceByDepartment_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][GEOS2-4301][04.08.2023]
        public List<TravelExpenses> GetTravelExpenses_V2420(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2420(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }
        //[rdixit][GEOS2-4301][04.08.2023]
        public List<TravelExpenses> GetAllTravelExpensewithPermission_V2420(string Plant, string idDepartment, string idOrganization, int permission)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetAllTravelExpensewithPermission_V2420(WorkbenchConnectionString, Plant, idDepartment, idOrganization, permission, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }
        //[rdixit][GEOS2-4301][04.08.2023]
        public bool UpdateTravelExpenses_V2420(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2420(travelExpense, modifiedBy, modificationDate, expenseLogList, expenseList, WorkbenchConnectionString);
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
        //[rdixit][GEOS2-4301][04.08.2023]
        public List<Company> GetCompanies_V2420()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanies_V2420(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][GEOS2-4301][04.08.2023]
        public TravelExpenses AddTravelExpenseReport_V2420(TravelExpenses travelExpense, int idCreator)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTravelExpenseReport_V2420(travelExpense, idCreator, WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][07-08-2023][GEOS2-3400]
        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2420(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2420(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-2466][08.06.2023]
        public List<Employee> GetAllEmployeesByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-2466][08.06.2023]
        public List<LookupValue> GetOrganizationalChartDepartmentArea_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationalChartDepartmentArea_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-2466][08.06.2023]
        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Expenses> EmployeeExpenseByExpenseReport_V2420(int IdEmployeeExpenseReport, string sourceCurrency,  int sourceCurrencyId,
            string targetCurrency, int targetCurrencyId)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2420(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, sourceCurrencyId,
                    targetCurrency, targetCurrencyId, Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }


        public List<EmployeeHoliday> GetEmployeeHolidays_V2420(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays_V2420(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #region GEOS2-4018
        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        public EmployeeAttendance AddEmployeeAttendance_V2420(EmployeeAttendance employeeAttendance, byte[] fileInBytes)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2420(workbenchConnectionString, employeeAttendance, fileInBytes, Properties.Settings.Default.EmpAttendanceAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]

        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-4018][08/08/2023]
        /// <summary>
        /// [001][24-09-2018][skhade][HRM-M048-06] Add new column File in Leaves grid
        /// </summary>
        /// <param name="employeeLeave">The employee leave.</param>
        /// <returns>Bytes array if found file else null.</returns>
        public byte[] GetEmployeeAttendanceAttachment(EmployeeAttendance employeeAttendance)
        {
            try
            {
                return mgr.GetEmployeeAttendanceAttachment(Properties.Settings.Default.EmpAttendanceAttachment, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Company> GetEmployeesCountByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByIdCompany_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Employee> GetBirthdaysOfEmployeesByYear_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBirthdaysOfEmployeesByYear_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<LookupValue> GetEmployeesCountByDepartmentAreaByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByDepartmentAreaByIdCompany_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<LookupValue> GetEmployeesCountByGenderByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByGenderByIdCompany_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<ContractSituation> GetEmployeesCountByContractByIdCompany_V2420(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByContractByIdCompany_V2420(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<EmployeeLeave> GetEmployeeLeavesForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesForDashboard_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<EmployeeDocument> GetEmployeeDocumentsExpirationForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDocumentsExpirationForDashboard_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Employee> GetEmployeesWithExitDateForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithExitDateForDashboard_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<EmployeeContractSituation> GetLatestContractExpirationForDashboard_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestContractExpirationForDashboard_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Employee> GetEmployeesWithAnniversaryDate_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithAnniversaryDate_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<JobDescription> GetOrganizationHierarchy_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetOrganizationHierarchy_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<EmployeeOrganizationChart> GetEmployeesForOrganizationChart_V2420(Int32 idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesForOrganizationChart_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Department> GetAllEmployeesByDepartmentByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompany_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Employee> GetAllEmployeesForLeaveByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForLeaveByIdCompany_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2420(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-2466][08-08-2023]
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2420(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2420(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
        /// <summary>
        /// This method is used to delete employee leave attachment.
        /// </summary>
        /// <param name="employeeCode">The employee code.</param>
        /// <param name="idEmployeeAttendance">The idEmployeeAttendance.</param>
        /// <param name="fileName">The filename.</param>
        /// <returns>True if deleted leave, else false.</returns>
        public bool DeleteEmployeeAttendanceAttachment(string employeeCode, Int64 idEmployeeAttendance, string fileName)
        {
            try
            {
                return mgr.DeleteEmployeeAttendanceAttachment(Properties.Settings.Default.EmpAttendanceAttachment, employeeCode, idEmployeeAttendance, fileName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.Jangra][GEOS2-4019][08/08/2023]

        /// <summary>
        /// This method is used to update employee attendance.
        /// </summary>
        /// <param name="employeeAttendance">The employee attendance.</param>
        /// <returns>True if updated else false.</returns>
        public bool UpdateEmployeeAttendance_V2420(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeAttendance_V2420(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4019][08/08/2023]
        /// <summary>
        /// This method is used to add employee leave attachment.
        /// </summary>
        /// <param name="employeeCode">The employee code.</param>
        /// <param name="idEmployeeAttendance">The idEmployeeLeave.</param>
        /// <param name="fileName">The filename.</param>
        /// <param name="fileBytes">The file in bytes.</param>
        /// <returns>True if added leave, else false.</returns>
        public bool SaveEmployeeAttendanceAttachment(string employeeCode, Int64 idEmployeeAttendance, string fileName, byte[] fileBytes)
        {
            try
            {
                return mgr.SaveEmployeeAttendanceAttachment(employeeCode, idEmployeeAttendance, fileName, Properties.Settings.Default.EmpAttendanceAttachment, fileBytes);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        //[rdixit][26.08.2023][GEOS2-3483]
        public Company UpdateCompany_V2430(Company company)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                company = mgr.UpdateCompany_V2430(WorkbenchConnectionString, company, Properties.Settings.Default.SiteImage);
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

        //[rdixit][26.08.2023][GEOS2-3483]
        public List<Company> GetAuthorizedPlantsByIdUser_V2430(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlantsByIdUser_V2430(WorkbenchConnectionString, idUser);
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
        //[rdixit][26.08.2023][GEOS2-4607]
        public List<EmployeeHoliday> GetEmployeeHolidays_V2430(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays_V2430(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Expenses> EmployeeExpenseByExpenseReport_V2430(int IdEmployeeExpenseReport, string sourceCurrency, int sourceCurrencyId,
    string targetCurrency, int targetCurrencyId)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2430(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, sourceCurrencyId,
                    targetCurrency, targetCurrencyId, Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }


        //[pramod.misal][GEOS2-4815][26-09-2023]
        public List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2440(string idCompany, Int64 selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeTripsBySelectedIdCompany_V2440(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][GEOS2-4855][02-10-2023]
        public List<CompanyHoliday> GetUpcomingCompanyHolidays_V2440(ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingCompanyHolidays_V2440(workbenchConnectionString, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        
        //[rdixit][GEOS2-4621][10.10.2023]
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2440(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2440(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-4721][11.10-2023]
        public EmployeeMealBudget GetMealExpenseByEmployyeAndCompany_V2440(int idCompany, int idCurrencyTo, DateTime conversionDate, int idEmployee)
        {
            EmployeeMealBudget EmployeeMealBudget = new EmployeeMealBudget();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeMealBudget = mgr.GetMealExpenseByEmployyeAndCompany_V2440(idCompany, idEmployee, idCurrencyTo, conversionDate, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeMealBudget;
        }

        //[rdixit][GEOS2-4721][11.10-2023]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2440(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2440(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, targetCurrency,
                    Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }

        //[rdixit][GEOS2-4721][11.10-2023]
        public List<Currency> GetCurrencies_V2440()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;//[rdixit][19.01.2024][GEOS2-5265]
                return mgr.GetCurrencies_V2440(WorkbenchConnectionString, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri[GEOS2-3693][12-10-2023]
        public List<LogNewJobDescription> GetLogEntriesForJob_V2440(UInt32 jobid)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLogEntriesForJob_V2440(workbenchConnectionString, jobid);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-3693]
        /// <summary>
        /// This method is to add notification to main Server. (as notifications is replicated table)
        /// and Send email.
        /// </summary>
        /// <param name="notification">The notification</param>
        /// <returns>notification with idNotification</returns>
        public Notification AddCommonNotification(Notification notification)
        {
            try
            {
                HrmManager mgr = new HrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string connectionWorkbenchString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                notification = mgr.AddCommonNotification(notification, connectionString, connectionWorkbenchString, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, Properties.Settings.Default.MailFrom, Properties.Settings.Default.EmailTemplate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return notification;
        }
        //[rdixit][GEOS2-4721][10.11.2023]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2450(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2450(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, targetCurrency,
                    Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }
        //[rajashrit][09-11-2023][GEOS2-4613]
        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2460(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2460(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //rajashri GEOS2-4612
        public List<Employee> GetTodayBirthdayOfEmployees_V2460(long selectedPeriod = 0, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayBirthdayOfEmployees_V2460(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-4848][23.11.2023]
        public List<TravelExpenses> GetTravelExpenses_V2460(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2460(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //[pramod.misal][GEOS2-4848][23.11.2023]
        public TravelExpenses AddTravelExpenseReport_V2460(TravelExpenses travelExpense, int idCreator)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTravelExpenseReport_V2460(travelExpense, idCreator, WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-4848][23.11.2023]
        public bool UpdateTravelExpenses_V2460(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2460(travelExpense, modifiedBy, modificationDate, expenseLogList, expenseList, WorkbenchConnectionString);
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
        ////[rajashri][GEOS2-4997][30-11-2023]
        public List<Employee> GetEmployeesByIdSite_V2460(int idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesByIdSite_V2460(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-4848][05-12-2023]
        public List<EmployeeTrips> GetEmployeeTripsBySelectedIdEmpolyee_V2460(int empid)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeTripsBySelectedIdEmpolyee_V2460(workbenchConnectionString,empid);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-5140 Use url service to download the employee pictures 18 12 2023 
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2470(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2470(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<TravelExpenses> GetTravelExpenses_V2470(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2470(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        public List<TravelExpenses> GetAllTravelExpensewithPermission_V2470(string Plant, string idDepartment, string idOrganization, int permission)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetAllTravelExpensewithPermission_V2470(WorkbenchConnectionString, Plant, idDepartment, idOrganization, permission, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //Shubham[skadam] GEOS2-5138 Add country column with flag in meal allowance 18 12 2023
        public List<MealAllowance> GetMealAllowances_V2470()
        {
            List<MealAllowance> MealAllowanceList = new List<MealAllowance>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MealAllowanceList = mgr.GetMealAllowances_V2470(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return MealAllowanceList;
        }

        //Shubham[skadam] GEOS2-5137 Add flag in country column loaded through url service 19 12 2023
        public List<Employee> GetAllEmployeesByIdCompany_V2470(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2470(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public bool UpdateTravelExpenses_V2470(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2470(travelExpense, modifiedBy, modificationDate, expenseLogList, expenseList, WorkbenchConnectionString);
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

        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public List<LogEntriesByTravelExpense> GetTravelExpenseChangelogs_V2470(Int64 IdEmployeeExpenseReport)
        {
            List<LogEntriesByTravelExpense> logEntriesByTravelExpense = new List<LogEntriesByTravelExpense>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                logEntriesByTravelExpense = mgr.GetTravelExpenseChangelogs_V2470(WorkbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntriesByTravelExpense;
        }

        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        public List<LogEntriesByTravelExpense> GetTravelExpenseComments_V2470(Int64 IdEmployeeExpenseReport)
        {
            List<LogEntriesByTravelExpense> logEntriesByTravelExpense = new List<LogEntriesByTravelExpense>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                logEntriesByTravelExpense = mgr.GetTravelExpenseComments_V2470(WorkbenchConnectionString, IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return logEntriesByTravelExpense;
        }

        //[chitra.girigosavi][GEOS2-4824][03.11.2023]
        public TravelExpenses AddTravelExpenseReport_V2470(TravelExpenses travelExpense, int idCreator, List<LogEntriesByTravelExpense> expenseReportLogList)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddTravelExpenseReport_V2470(travelExpense, idCreator, expenseReportLogList, WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //rajashri
        public JobDescription AddJobDescription_V2470(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddJobDescription_V2470(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public JobDescription UpdateJobDescription_V2470(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateJobDescription_V2470(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, jobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][27.12.2023][GEOS2-4875][GEOS2-48756]
        public List<EmployeeContact> GetEmployeeContactsByIdEmployee(string idEmployeeList)
        {
            List<EmployeeContact> employeeContacts = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                employeeContacts = mgr.GetEmployeeContactsByIdEmployee(workbenchConnectionString, idEmployeeList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return employeeContacts;
        }
        //rajashri GEOS2-4942
        public List<CompanyHoliday> GetUpcomingCompanyHolidays_V2470(ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUpcomingCompanyHolidays_V2470(workbenchConnectionString, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
       
        //[rajashrit][09-11-2023][GEOS2-4613]
        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2470(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2470(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rajashrit][09-01-2024]
        public List<Employee> GetBestRegardsEmployeeData()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBestRegardsEmployeeData(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][09.01.2024][GEOS2-5112]
        public List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2480(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission,UInt32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeTripsBySelectedIdCompany_V2480(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public Tuple<List<Employee>, Employee> GetEmployeeForWelcomeBoard_V2480()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeForWelcomeBoard_V2480(WorkbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-4816
        //[Sudhir.jangra][GEOS2-4816]
        public string GetLatestCodeForTrip_V2480()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLatestCodeForTrip_V2480(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[SUDHIR.JANGRA][GEOS2-4816]
        public List<Traveller> GetActiveEmployeesForTraveller_V2480(string IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActiveEmployeesForTraveller_V2480(workbenchConnectionString, IdCompany, selectedPeriod, idsOrganization, idsDepartments, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4816]
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                list = mgr.GetLookupValues(key);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                if (mgr.IsConnectionStringNameExist("WorkbenchContext") == false)
                {
                    exp.ErrorMessage = "WorkbenchContext - connection string name not found.";
                    exp.ErrorCode = "000090";
                }
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }

        //[SUDHIR.JANGRA][GEOS2-4816]
        public List<Destination> GetPlantListForDestination_V2480(UInt32 idUser)
        {
            List<Destination> companies = new List<Destination>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetPlantListForDestination_V2480(WorkbenchConnectionString, idUser);
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

        //[Sudhir.jangra][GEOS2-4816]
        public List<Destination> GetCustomersForTripDestination_V2480()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCustomersForTripDestination_V2480(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-4816]
        public List<Currency> GetCurrencyListForTrips_V2480()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCurrencyListForTrips_V2480(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.jangra][GEOS2-4816]
        public EmployeeTrips AddEmployeeTripDetails_V2480(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2480(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[SUDHIR.JANGRA][GEOS2-4816]
        public List<LogEntriesByEmployeeTrip> GetLogEntriesByEmployeeTrip_V2480(UInt32 idEmployeeTrip)
        {
            List<LogEntriesByEmployeeTrip> changeLog = new List<LogEntriesByEmployeeTrip>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                changeLog = mgr.GetLogEntriesByEmployeeTrip_V2480(WorkbenchConnectionString, idEmployeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return changeLog;
        }

        //[Sudhir.jangra][GEOS2-4816]
        public EmployeeTrips EditEmployeeTripDetails_V2480(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2480(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4816]
        public List<Company> GetAuthorizedPlantsByIdUser_V2480(Int32 idUser)
        {
            List<Company> companies = new List<Company>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetAuthorizedPlantsByIdUser_V2480(WorkbenchConnectionString, idUser);
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
        //[Sudhir.Jangra][GEOS2-4816]
        public EmployeeTrips GetEditEmployeeTripDetails_V2480(UInt32 idEmployeeTrip)
        {
            EmployeeTrips companies = new EmployeeTrips();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetEditEmployeeTripDetails_V2480(WorkbenchConnectionString, idEmployeeTrip);
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
        #endregion

        //rajashri GEOS-4911
        public List<EmployeeTraineeDetails> GetTraineedataforExcel(ulong Professionaltraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTraineedataforExcel(workbenchConnectionString, Professionaltraining);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5071][rdixit][17.01.2024]
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2480(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2480(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-5225 Automatic date not updated properly 17 01 2024
        public List<OrganizationHR> GetOrganizationHRByCompany_V2480(Int32 IdSite)
        {
            List<OrganizationHR> HR = new List<OrganizationHR>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                HR = mgr.GetOrganizationHRByCompany_V2480(workbenchConnectionString, IdSite);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return HR;
        }
        //Shubham[skadam] GEOS2-5145 Expenses Shared in different currency. 18 01 2024
        public List<Expenses> EmployeeExpenseByExpenseReport_V2480(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2480(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, targetCurrency,
                Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }

        //[pramod.misal][GEOS2-5159][18.01.2024]
        public List<MealAllowance> GetMealAllowances_V2480()
        {
            List<MealAllowance> MealAllowanceList = new List<MealAllowance>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MealAllowanceList = mgr.GetMealAllowances_V2480(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return MealAllowanceList;
        }

        #region GEOS2-4846
        //[Sudhir.Jangra][GEOS2-4846]
        public List<JobDescription> GetAllJobDescriptionForApprovalResponsible_V2480(Int32 IdJobDescription)
        {
            List<JobDescription> jobDescriptions = null;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                jobDescriptions = mgr.GetAllJobDescriptionForApprovalResponsible_V2480(workbenchConnectionString, IdJobDescription);
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


        //[Sudhir.Jangra][GEOS2-4846]
        public Employee AddEmployee_V2480(Employee employee)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;

                employee = mgr.AddEmployee_V2480(mainServerWorkbenchConnectionString, workbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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

        //[Sudhir.jangra][GEOS2-4846]
        public Employee GetEmployeeByIdEmployee_V2480(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2480(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-4846]
        public bool UpdateEmployee_V2480(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2480(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument);
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
        #endregion

        //rajashri GEOS2-4911
        public byte[] GetTrainingTemplate(string siteName)
        {
            byte[] bytes = null;
            string TrainingtemplateFolderPath = string.Empty;
            string trainingtemplateFilePath = string.Empty;
            try
            {
                string fileName = @"\TrainingRecord.xlsx";
                TrainingtemplateFolderPath = Properties.Settings.Default.TraningTemplate;
                TrainingtemplateFolderPath = TrainingtemplateFolderPath.Replace("{0}", siteName);

                if (!System.IO.Directory.Exists(TrainingtemplateFolderPath))
                {
                    System.IO.Directory.CreateDirectory(TrainingtemplateFolderPath);
                }
                if (!Properties.Settings.Default.EnumeratedImage.EndsWith("\\", StringComparison.InvariantCultureIgnoreCase))
                {
                    TrainingtemplateFolderPath = string.Concat(TrainingtemplateFolderPath, "\\");
                }
                trainingtemplateFilePath = string.Format("{0}{1}", TrainingtemplateFolderPath, fileName);
                if (File.Exists(trainingtemplateFilePath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(trainingtemplateFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);
                            if (n == 0)
                                break;
                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                return bytes;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //pramod,misal GEOS2-5077 24.01.2024
        public string Geos_app_settingsReceiptsPDF_PageSize()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.Geos_app_settingsReceiptsPDF_PageSize(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //pramod,misal GEOS2-5077 24.01.2024
        public Dictionary<string, byte[]> Geos_app_settingsHeader_Image()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.Geos_app_settingsHeader_Image(workbenchConnectionString, Properties.Settings.Default.ExpenseReportHeaderImage);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //pramod,misal GEOS2-5077 24.01.2024
        public string Geos_app_settingsHeader_Footer_DateTime()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.Geos_app_settingsHeader_Footer_DateTime(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //pramod,misal GEOS2-5077 24.01.2024
        public string Geos_app_settings_Footer_Email()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.Geos_app_settings_Footer_Email(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //pramod,misal GEOS2-5077 24.01.2024
        public string Geos_app_settingsHeader_Footer_pageNumber()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.Geos_app_settingsHeader_Footer_pageNumber(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region GEOS2-5275
        //[Sudhir.jangra][GEOS2-5275]
        public List<EmployeeAttendance> GetEmployeeSplitAttendance_V2480(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime startDate, DateTime endDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeSplitAttendance_V2480(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, startDate, endDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


       

        #endregion

        //pramod.misal GEOS2-5077 06.02.2024
        public List<EmployeeExpensePhotoInfo> HRM_GetAllExpenseAttachmentsByExpenseReportId(int idEmployeeExpense)
        {
            List<EmployeeExpensePhotoInfo> EmployeeExpensePhotoInfoList = new List<EmployeeExpensePhotoInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeExpensePhotoInfoList = mgr.HRM_GetAllExpenseAttachmentsByExpenseReportId(workbenchConnectionString, idEmployeeExpense, Properties.Settings.Default.ExpensesAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeExpensePhotoInfoList;
        }

        //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
        public bool APILayerCurrencyConversions(DateTime StartDate, Currency sourceCurrency, Currency targetCurrency)
        {
            bool isInsert = false;
           
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isInsert = mgr.APILayerCurrencyConversions(StartDate,sourceCurrency,targetCurrency, mainServerWorkbenchConnectionString, Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isInsert;
        }

        //[pramod.misal][GEOS2-5286][05-02-2024]
        public List<TravelExpenses> GetTravelExpenses_V2490(string Plant)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2490(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        //[Sudhir.Jangra][GEOS2-3418]
        public List<Employee> GetAllEmployeeListForProfessionalContact_V2490()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeeListForProfessionalContact_V2490(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TripExpensesReport GetTripExpensesReport_V2490(UInt32 idEmployeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripExpensesReport_V2490(idEmployeeTrip , workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[pramod.misal][GEOS2-5365][08.03.2024]
        public List<MealAllowance> GetMealAllowances_V2500()
        {
            List<MealAllowance> MealAllowanceList = new List<MealAllowance>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MealAllowanceList = mgr.GetMealAllowances_V2500(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return MealAllowanceList;
        }
        //rajashri GEOS2-4817
        public List<EmployeeTrips> GetTripDetails(string idCompany, uint idUser, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripDetails(workbenchConnectionString, idCompany, idUser, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][12.03.2024][GEOS2-5335]
        public List<EmployeeHoliday> GetEmployeeHolidays_V2500(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays_V2500(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[pramod.misal][GEOS2-5365][12.03.2024]
        public bool UpdateMealAllowance_V2500(List<EmployeeMealBudget> employeeMealBudget)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateMealAllowance_V2500(employeeMealBudget, WorkbenchConnectionString);
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

        //[pramod.misal][GEOS2 - 5400][14.03.2024]
        public ProfessionalTraining AddProfessionalTraining_V2500(ProfessionalTraining professionalTraining)
        {
            try
            {
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;

                professionalTraining = mgr.AddProfessionalTraining_V2500(mainServerWorkbenchConnectionString, workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return professionalTraining;
        }

        //[pramod.misal][GEOS2 - 5400][14.03.2024]
        public bool UpdateProfessionalTraining_V2500(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2500(workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-5545][27.03.2024][rdixit]
        public List<Employee> GetAllEmployeeListForProfessionalContact_V2500()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeeListForProfessionalContact_V2500(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Sudhir.Jangra][GEOS2-5336]
        public EmployeeAnnualAdditionalLeave GetEmployeeAnnualCompensationAttendance_V2500(Int32 idEmployee, long selectionPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAnnualCompensationAttendance_V2500(workbenchConnectionString, idEmployee,selectionPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        public EmployeeAttendance SplittedEmployeeAttendanceAdd(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.SplittedEmployeeAttendanceAdd(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        public bool SplittedEmployeeAttendanceUpdate(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.SplittedEmployeeAttendanceUpdate(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][28.03.2024][GEOS2-5276][GEOS2-5277][GEOS2-5278]
        public List<EmployeeAttendance> GetEmployeeSplitAttendance_V2500(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime startDate, DateTime endDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeSplitAttendance_V2500(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, startDate, endDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.jangra][GEOS2-5336]
        public bool AddEmployeeAttendanceLeave_V2500(EmployeeAnnualLeave employee,double hours)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.AddEmployeeAttendanceLeave_V2500(WorkbenchConnectionString, employee,hours);
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


        //[Sudhir.Jangra][GEOS2-5336]
        public List<EmployeeAnnualLeave> GetEmployeeLeaveByIdEmployee_V2500(Int32 idEmployee, long selectedPeriod, string idCompany, List<EmployeeAnnualAdditionalLeave> employeeAnnualLeavesAdditional)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeaveByIdEmployee_V2500(workbenchConnectionString, idEmployee, selectedPeriod, idCompany, employeeAnnualLeavesAdditional);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.jangra][GEOS2-5336]
        public bool AddEmployeeChangelogs_V2500(EmployeeChangelog changeLog)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.AddEmployeeChangelogs_V2500(WorkbenchConnectionString, changeLog);
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
        //[rajashri][01-04-2024][GEOS2-5360]
        public List<ExpenseReportCurrency> GetAllExpenseReportCurrency_V2500()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllExpenseReportCurrency_V2500(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[Sudhir.jangra][GEOS2-5336]
        public bool DeleteEmployeeLeaveForAttendance_V2500(long idEmployeeAnnualLeave)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.DeleteEmployeeLeaveForAttendance_V2500(WorkbenchConnectionString, idEmployeeAnnualLeave);
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


        //[Sudhir.Jangra][GEOS2-5614]
        public List<MealAllowance> GetMealAllowances_V2510()
        {
            List<MealAllowance> MealAllowanceList = new List<MealAllowance>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                MealAllowanceList = mgr.GetMealAllowances_V2510(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return MealAllowanceList;
        }

        //[rdixit][[GEOS2-5336]][19.04.2024]
        public bool DeleteEmployeeCompensatoryHours_V2501(Int32 idEmployee, long selectedPeriod, double hours)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.DeleteEmployeeCompensatoryHours_V2501(WorkbenchConnectionString, idEmployee, selectedPeriod, hours);
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
        //rajashri GEOS2-5534
        public List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2510(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, UInt32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeTripsBySelectedIdCompany_V2510(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rajashri] [GEOS2-5514][26.04.2024]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2510(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2510(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, targetCurrency,
                Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }


        public List<TravelExpenses> GetTravelExpenses_V2520(string Plant, Int64 selectedPeriod)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetTravelExpenses_V2520(WorkbenchConnectionString, Plant, Properties.Settings.Default.CurrenciesImages, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }

        public List<TravelExpenses> GetAllTravelExpensewithPermission_V2520(string Plant, string idDepartment, string idOrganization, int permission, Int64 selectedPeriod)
        {
            List<TravelExpenses> travelExpenses = new List<TravelExpenses>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                travelExpenses = mgr.GetAllTravelExpensewithPermission_V2520(WorkbenchConnectionString, Plant, idDepartment, idOrganization, permission, Properties.Settings.Default.CurrenciesImages, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return travelExpenses;
        }


        public List<WorkflowStatus> GetWorkFlowStatus()
        {
            List<WorkflowStatus> workFlowStatusLst = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                workFlowStatusLst = mgr.GetWorkFlowStatus(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return workFlowStatusLst;
        }
        //rajashri GEOS2-5502 [21-05-2024
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2520(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-5640][cpatil][24.05.2024]
        public List<Employee> GetAllEmployeesForAttendanceByIdCompany_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForAttendanceByIdCompany_V2520(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[GEOS2-5640][cpatil][24.05.2024]
        public List<EmployeeLeave> GetEmployeeLeavesBySelectedIdCompany_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesBySelectedIdCompany_V2520(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad] [GEOS2-5555][23.05.2024]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2520(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2520(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, targetCurrency,
                Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2520(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2520(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public bool IsProfileUpdate(string EmployeeCode)
        {
            bool isUpdated = false;
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                isUpdated = mgr.IsProfileUpdate(EmployeeCode, workbenchConnectionString);
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

        public bool UpdateEmployeeAttendance_V2520(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeAttendance_V2520(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        public EmployeeAttendance AddEmployeeAttendance_V2520(EmployeeAttendance employeeAttendance, byte[] fileInBytes)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2520(workbenchConnectionString, employeeAttendance, fileInBytes, Properties.Settings.Default.EmpAttendanceAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[chitra.girigosavi][GEOS2-4824][02.11.2023]
        //Shubham[skadam] GEOS2-5501 HRM Travel - Change expenses type 30 05 2024
        public bool UpdateTravelExpenses_V2520(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2520(travelExpense, modifiedBy, modificationDate, expenseLogList, expenseList, WorkbenchConnectionString);
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


        //[Sudhir.jangra][GEOS2-5540]
        public List<WorkflowStatus> GetWorkFlowStatus_V2520()
        {
            List<WorkflowStatus> workFlowStatusLst = null;

            try
            {
                // SRMManager mgr = new SRMManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                workFlowStatusLst = mgr.GetWorkFlowStatus_V2520(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return workFlowStatusLst;
        }

        //[rdixit][06.06.2024][GEOS2-5786]
        public List<EmployeeAttendance> GetEmployeeSplitAttendance_V2530(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime startDate, DateTime endDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeSplitAttendance_V2530(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, startDate, endDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5549][13.06.2024]
        public List<HealthAndSafetyAttachedDoc> GetAllHealthAndSafetyFilesByIdJobDescription(UInt32 jobid)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllHealthAndSafetyFilesByIdJobDescription(workbenchConnectionString, jobid, Properties.Settings.Default.HealthAndSafetyTypeAttachedDoc);
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
        public List<CompanyHoliday> GetCompanyHolidaysBySelectedIdCompany_V2530(string idCompany, Int64 selectedPeriod = 0)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCompanyHolidaysBySelectedIdCompany_V2530(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[rajashrit][09-11-2023][GEOS2-4613]
        //Shubham[skadam] GEOS2-5781 HRM - Employee Exit Announcement  17 06 2024
        public List<EmployeeShortDetailForMail> GetEmployeeJoinOrLeaveDetail_V2530(DateTime curDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeJoinOrLeaveDetail_V2530(workbenchConnectionString, curDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad][GEOS2-5549][17.06.2024]
        public JobDescription UpdateJobDescription_V2530(JobDescription jobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateJobDescription_V2530(workbenchConnectionString, Properties.Settings.Default.EmpJobDescriptionsFilesPath, Properties.Settings.Default.HealthAndSafetyTypeAttachedDoc, jobDescription, Properties.Settings.Default.EquipmentTypeFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        public List<EquipmentAndToolsAttachedDoc> GetEquipmentAndToolsForJobDescription_V2530(UInt32 jobid)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEquipmentAndToolsForJobDescription_V2530(workbenchConnectionString, jobid, Properties.Settings.Default.EquipmentTypeFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5549]
        public string GetUserAllowSites_V2530(Int32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetUserAllowSites_V2530(workbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[pramod.misal][GEOS2-5793][24.06.2024]
        public TripExpensesReport GetTripExpensesReport_V2530(UInt32 idEmployeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripExpensesReport_V2530(idEmployeeTrip, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
        public List<EmployeeExpenseStatus> GetRejectSendMail_V2540(int IdEmployeeExpenseReport)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetRejectSendMail_V2540(workbenchConnectionString,IdEmployeeExpenseReport);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul Gadhave][GEOS2-5757][Date-12/07/2024]
        public bool GetEmployeeExpenseReportTemplate_V2540(string code, Employee selectedEmployee, TravelExpenseStatus status, string comments, string userCompanyEmail)
        {
            try
            {
                HRMMail mailServer = new HRMMail();
                mailServer.MailTemplatePath = Properties.Settings.Default.EmailTemplate;
                mailServer.MailServerName = Properties.Settings.Default.MailServerName;
                mailServer.MailServerPort = Properties.Settings.Default.MailServerPort;
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeExpenseReportTemplate_V2540(mailServer, workbenchConnectionString, code, selectedEmployee, status, comments, userCompanyEmail);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public TripExpensesReport GetTripExpensesReport_V2540(UInt32 idEmployeeTrip, Int32 IdCurrencyTo)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripExpensesReport_V2540(idEmployeeTrip, IdCurrencyTo, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        #region chitra.girigosavi [17/07/2024] GEOS2-5955 Registro de Capacitación 2
        public bool UpdateProfessionalTraining_V2540(ProfessionalTraining professionalTraining)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateProfessionalTraining_V2540(workbenchConnectionString, professionalTraining, Properties.Settings.Default.TraineeResultFilePath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.ProfessionalTrainingAttachmentFiles);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        #endregion

        //[rdixit][17.07.2024][GEOS2-5767]
        public List<Expenses> EmployeeExpenseByExpenseReport_V2540(int IdEmployeeExpenseReport, Currency sourceCurrency, Currency targetCurrency)
        {
            List<Expenses> Expenses = new List<Expenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                Expenses = mgr.EmployeeExpenseByExpenseReport_V2540(workbenchConnectionString, IdEmployeeExpenseReport, sourceCurrency, targetCurrency,
                Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return Expenses;
        }

        //[rdixit][17.07.2024][GEOS2-5767]
        public List<TravelExpenses> GetExpensesReportByTrip(UInt32 idEmployeeTrip)
        {
            List<TravelExpenses> ExpenseReports = new List<TravelExpenses>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                ExpenseReports = mgr.GetExpensesReportByTrip(workbenchConnectionString,idEmployeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ExpenseReports;
        }


        //[rdixit][22.07.2024][GEOS2-5680]
        public List<CompanyHolidaySetting> GetAllCompanyHolidaySetting()
        {
            List<CompanyHolidaySetting> companySettings = new List<CompanyHolidaySetting>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companySettings = mgr.GetAllCompanyHolidaySetting(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companySettings;
        }

        //[rdixit][22.07.2024][GEOS2-5680]
        public bool UpdateCompanyHolidaySetting(List<CompanyHolidaySetting> companyHolidaySetting)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyHolidaySetting(workbenchConnectionString, companyHolidaySetting);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

		//[GEOS2-5681][22-07-2024][nsatpute]
        public List<CompanyServiceLength> GetCompanyWiseLengthOfService()
        {
            List<CompanyServiceLength> lstCompanyServiceLength = new List<CompanyServiceLength>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                lstCompanyServiceLength = mgr.GetCompanyWiseLengthOfService(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return lstCompanyServiceLength;
        }
        //[GEOS2-5681][23-07-2024][nsatpute]
        public bool UpdateCompanyWiseLengthOfService(string modifiedBy, List<CompanyServiceLength> lstCompanyService)
        {
            List<CompanyServiceLength> lstCompanyServiceLength = new List<CompanyServiceLength>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyWiseLengthOfService(modifiedBy, lstCompanyService, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //pramod.misal GEOS2-5077 06.02.2024
        //Shubham[skadam] GEOS2-6037 HRM Expense Report - Time to Generate Expense tickets 05 08 2024
        public List<EmployeeExpensePhotoInfo> HRM_GetAllExpenseAttachmentsByExpenseReportId_V2500(int idEmployeeExpense)
        {
            List<EmployeeExpensePhotoInfo> EmployeeExpensePhotoInfoList = new List<EmployeeExpensePhotoInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeExpensePhotoInfoList = mgr.HRM_GetAllExpenseAttachmentsByExpenseReportId_V2500(workbenchConnectionString, idEmployeeExpense, Properties.Settings.Default.ExpensesAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeExpensePhotoInfoList;
        }


        //[rdixit][23.08.2024][GEOS2-5945]
        public string GetWorkbenchMainServiceProvider()
        {          
            try
            {
                return Properties.Settings.Default.WorkbenchMainServiceProvider;
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }           
        }

        //Shubham[skadam] GEOS2-5329 Insert not plant currency when is needed in DB to be used in travel reports 08 02 2024
        //Shubham[skadam] GEOS2-6430 One to one currency conversion need to do 10 09 2024
        public bool APILayerCurrencyConversions_V2560(DateTime StartDate, Currency sourceCurrency, Currency targetCurrency)
        {
            bool isInsert = false;

            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isInsert = mgr.APILayerCurrencyConversions_V2560(StartDate, sourceCurrency, targetCurrency, mainServerWorkbenchConnectionString, Properties.Settings.Default.CurrencyLayer, Properties.Settings.Default.CurrencyLayerKey);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isInsert;
        }
		// [nsatpute][11-09-2024][GEOS2-5929]
        public List<Employee> HRM_GetResponsibleForAddEditTrip()
        {
            List<Employee> lstResponsible = new List<Employee>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.HRM_GetResponsibleForAddEditTrip(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][12-09-2024][GEOS2-5929]
        public List<Destination> HRM_GetSuppliersForDestination()
        {
            List<Employee> lstResponsible = new List<Employee>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.HRM_GetSuppliersForDestination(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
        public IList<LookupValue> GetLookupValues_V2560(byte key)
        {
            IList<LookupValue> list = null;
            try
            {
                CrmManager mgr = new CrmManager();
                list = mgr.GetLookupValues(key);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();

                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return list;
        }
        //[rushikesh.gaikwad][GEOS2-5927][29.08.2024]
        public List<EmployeeTrips> GetEmployeeTripsBySelectedIdCompany_V2560(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, UInt32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeTripsBySelectedIdCompany_V2560(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][12-09-2024][GEOS2-5929]
        public List<TripAssets> HRM_GetEmployeeTripAssets()
        {
            List<Employee> lstResponsible = new List<Employee>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.HRM_GetEmployeeTripAssets(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<LogEntriesByCompanyLeaves> GetCompanyLeavesChangeLog_V2560()
        {
            List<LogEntriesByCompanyLeaves> CompanyLeavesChangeLog = new List<LogEntriesByCompanyLeaves>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                string mainServerWorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                CompanyLeavesChangeLog = mgr.GetCompanyLeavesChangeLog_V2560(mainServerWorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return CompanyLeavesChangeLog;
        }

        //[GEOS2-5681][23-07-2024][nsatpute]
        public bool UpdateCompanyWiseLengthOfService_V2560(string modifiedBy, List<CompanyServiceLength> lstCompanyService)
        {
            List<CompanyServiceLength> lstCompanyServiceLength = new List<CompanyServiceLength>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyWiseLengthOfService_V2560(modifiedBy, lstCompanyService, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][22.07.2024][GEOS2-5680]
        public bool UpdateCompanyHolidaySetting_V2560(List<CompanyHolidaySetting> companyHolidaySetting)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateCompanyHolidaySetting_V2560(workbenchConnectionString, companyHolidaySetting);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rushikesh.gaikwad][GEOS2-5927][16.09.2024]
        public bool IsDeleteTrips(Int32 idEmployeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.IsDeleteTrips(workbenchConnectionString, idEmployeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6387][rdixit][18.09.2024]
        public List<Employee> GetTodayEmployeeCompanyAnniversariesDetailsNew_V2560(DateTime currentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayEmployeeCompanyAnniversariesDetailsNew_V2560(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5929]
        public List<Destination> GetCustomersForTripDestination_V2560()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCustomersForTripDestination_V2560(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5929]
        public List<Destination> GetPlantListForDestination_V2560(UInt32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetPlantListForDestination_V2560(workbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][16-09-2024][GEOS2-5929]
        public List<Company> GetAuthorizedPlantsByIdUser_V2560(Int32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAuthorizedPlantsByIdUser_V2560(workbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][20.09.2024][GEOS2-5930]
        public EmployeeTrips AddEmployeeTripDetails_V2560(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2560(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-5683 HRM - Holidays (4 of 4) 19 09 2024
        public List<ActiveEmployee> GetActiveEmployees_V2560()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetActiveEmployees_V2560(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-5683 HRM - Holidays (4 of 4) 19 09 2024
        public bool AddUpdateEmployeeAnnualLeaves_V2560(ActiveEmployee ActiveEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddUpdateEmployeeAnnualLeaves_V2560(workbenchConnectionString, ActiveEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		// [nsatpute][16-09-2024][GEOS2-5931]
        public EmployeeTrips GetEditEmployeeTripDetails_V2560(UInt32 idEmployeeTrip)
        {
            EmployeeTrips companies = new EmployeeTrips();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetEditEmployeeTripDetails_V2560(WorkbenchConnectionString, idEmployeeTrip);
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

        //[rdixit][20.09.2024][GEOS2-5930]
        public EmployeeTrips EditEmployeeTripDetails_V2560(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2560(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][21-09-2024][GEOS2-5929]
        public List<TripStatus> GetAllTripStatusWorkflow()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTripStatusWorkflow(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        // [nsatpute][21-09-2024][GEOS2-5929]
        public List<WorkflowTransition> GetAllTripStatusWorkflowTransitions()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllTripStatusWorkflowTransitions(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        public void StartSavingTripAttachmentFile(string saveDirectorPath, string fileName)
        {
            try
            {
                string tripRequestAttachmentPath = null;
                mgr.StartSavingTripAttachmentFile(Path.Combine(tripRequestAttachmentPath, saveDirectorPath), fileName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][16-09-2024][GEOS2-5931]
        public void SaveTripAttachmentPartData(string saveDirectorPath, string fileName, byte[] partData)
        {
            try
            {
                string tripRequestAttachmentPath = null;
                mgr.SaveTripAttachmentPartData(Path.Combine(tripRequestAttachmentPath, saveDirectorPath), fileName, partData);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][16-09-2024][GEOS2-5931]
        public void DeleteTripAttachmentFile(string directoryPath, string fileName)
        {
            try
            {
                string tripRequestAttachmentPath = null;
                mgr.DeleteTripAttachmentFile(Path.Combine(tripRequestAttachmentPath, directoryPath), fileName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][16-09-2024][GEOS2-5931]
        public byte[] GetTripAttachmentFile(string directoryPath, string fileName)
        {
            try
            {
                string tripRequestAttachmentPath = null;
                return mgr.GetTripAttachmentFile(Path.Combine(tripRequestAttachmentPath, directoryPath), fileName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		 // [nsatpute][24-09-2024][GEOS2-6473]
        public List<WorkflowStatus> HRM_GetAllTripWorkflowStatus()
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.HRM_GetAllTripWorkflowStatus(WorkbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        // [nsatpute][26-09-2024][GEOS2-6486]
        public EmployeeTrips EditEmployeeTripDetails_V2570(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2570(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][26-09-2024][GEOS2-6486]
        public EmployeeTrips AddEmployeeTripDetails_V2570(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2570(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rgadhave][GEOS2-6385][27-09-2024]
        public Destination GetIdCurrrencyFromIdCountry_V2570(byte? IdCountry)
        {
            try
            {

                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetIdCurrrencyFromIdCountry_V2570(WorkbenchConnectionString, IdCountry);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-6085] [Date:03-10-2024]
        public List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2570(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2570(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][04-10-2024][GEOS2-6451]
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2570(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2570(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-6085] [Date:07-10-2024]
        public TripExpensesReport GetTripExpensesReport_V2570(UInt32 idEmployeeTrip, Int32 IdCurrencyTo)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripExpensesReport_V2570(idEmployeeTrip, IdCurrencyTo, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][15-10-2024][GEOS2-5933]
        public List<EmployeeTripStatus> GetEmployeeTripStatusDetails()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeTripStatusDetails(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][21-10-2024][GEOS2-5933]
        public List<LogEntriesByEmployeeTrip> GetLogEntriesByEmployeeTrip_V2570(UInt32 idEmployeeTrip)
        {
            List<LogEntriesByEmployeeTrip> changeLog = new List<LogEntriesByEmployeeTrip>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                changeLog = mgr.GetLogEntriesByEmployeeTrip_V2570(WorkbenchConnectionString, idEmployeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return changeLog;
        }
        // [nsatpute][22-10-2024][GEOS2-6656]
        public List<Traveller> GetActiveEmployeesForTraveller_V2570(string IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActiveEmployeesForTraveller_V2570(workbenchConnectionString, IdCompany, selectedPeriod, idsOrganization, idsDepartments, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		// [nsatpute][22-10-2024][GEOS2-6543]
        public EmployeeTrips AddEmployeeTripDetails_V2580(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2580(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][22-10-2024][GEOS2-6543]
        public EmployeeTrips EditEmployeeTripDetails_V2580(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2580(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		// [nsatpute][08-11-2024] HRM - Improve ERF . GEOS2-6475
        public List<EmployeeDepartmentSituation> GetEmployeeDepartmentSituation(string employeeDepartmentName)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDepartmentSituation(workbenchConnectionString, employeeDepartmentName);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][04-10-2024][GEOS2-6451]
        //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2580(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2580(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //Shubham[skadam] GEOS2-6447 Improve the display of the GPS Location in the attendance. (1/3) 05 11 2024
        public EmployeeAttendance GetEmployeeAttendanceByIdEmployeeAttendance_V2580(Int64 IdEmployeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAttendanceByIdEmployeeAttendance_V2580(workbenchConnectionString, IdEmployeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        /// [nsatpute][14-11-2024][GEOS2-5747]
        public List<EmdepSiteDetails> GetEmdepsitesCountryRegion()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmdepsitesCountryRegion(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// [nsatpute][14-11-2024][GEOS2-5747]
        public List<Employee> GetEmployeesByDepartmentAndCompany()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesByDepartmentAndCompany(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][15.11.2024][GEOS2-6013]
        public bool IsCompanyInIdCardExtraInfoCountries(int idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsCompanyInIdCardExtraInfoCountries(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][GEOS2-5579]
        public Employee GetEmployeeByIdEmployee_V2580(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2580(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[Sudhir.Jangra][Geos2-5579]
        public bool UpdateEmployee_V2580(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2580(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument);
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

		/// [nsatpute][14-11-2024][GEOS2-5747]
        public void SaveEmployeeBacklogHours(List<Employee> employees)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                mgr.SaveEmployeeBacklogHours(workbenchConnectionString, employees);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee> GetAllEmployeesByIdCompany_V2590(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2590(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][10-12-2024][GEOS2-6367]
        public TripExpensesReport GetTripExpensesReport_V2590(UInt32 idEmployeeTrip, Int32 IdCurrencyTo)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripExpensesReport_V2590(idEmployeeTrip, IdCurrencyTo, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		// [nsatpute][17-12-2024][GEOS2-5747]
        public List<Employee> GetEmployeesByDepartmentAndCompany_V2590()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesByDepartmentAndCompany_V2590(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		// [nsatpute][17-12-2024][GEOS2-5747]
        public List<Employee> GetEmployeeBacklogHours(List<Employee> lstEmployees)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeBacklogHours(workbenchConnectionString, lstEmployees);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }
        //[rdixit][18.12.2024][GEOS2-6571]
        public List<PlantLeave> GetLeaveTypesWithLocations_V2590()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLeaveTypesWithLocations_V2590(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][18.12.2024][GEOS2-6571]
        public List<LookupValue> GetLeavesByLocations_V2590(List<int> employeeIdList)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLeavesByLocations_V2590(workbenchConnectionString, employeeIdList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[rdixit][18.12.2024][GEOS2-6571]
        public Employee GetEmployeeByIdEmployee_V2590(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2590(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][19-12-2024][GEOS2-6635]
        public EmployeeAttendance AddEmployeeAttendance_V2590(EmployeeAttendance employeeAttendance, byte[] fileInBytes)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeAttendance_V2590(workbenchConnectionString, employeeAttendance, fileInBytes, Properties.Settings.Default.EmpAttendanceAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[nsatpute][19-12-2024][GEOS2-6635]
        public bool IsPartialAttendanceExists(int idEmployee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsPartialAttendanceExists(workbenchConnectionString, idEmployee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][19-12-2024][GEOS2-6636]
        public List<EmployeeAttendance> GetSelectedIdCompanyEmployeeAttendance_V2590(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedIdCompanyEmployeeAttendance_V2590(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][19-12-2024][GEOS2-6636]
        public List<EmployeeAttendance> GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2190(string commaSeparatedCompanyIds, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate, string commaSeparatedEmployeeIds)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeAttendanceByCompanyIdsAndEmployeeIds_V2190(workbenchConnectionString, commaSeparatedCompanyIds, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate, commaSeparatedEmployeeIds);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][24-12-2024][GEOS2-6774]
        public List<EmdepSiteDetails> GetEmdepsitesCountryRegion_V2590()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmdepsitesCountryRegion_V2590(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][24.12.2024][GEOS2-6571]
        public List<Employee> GetEmployeeDetailsForLeaveSummary_V2590(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeDetailsForLeaveSummary_V2590(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[rdixit][18.12.2024][GEOS2-6571]
        public List<LookupValue> GetEmployeeLeavesByLocations_V2590(List<int> employeeIdList)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeLeavesByLocations_V2590(workbenchConnectionString, employeeIdList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [nsatpute][06-01-2025] [GEOS2-6775]
        public Company UpdateCompany_V2600(Company company)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                company = mgr.UpdateCompany_V2600(WorkbenchConnectionString, company, Properties.Settings.Default.SiteImage);
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

        // [nsatpute][06-01-2025] [GEOS2-6775]
        public Company GetCompanyDetailsByCompanyIdSelectedPeriod_V2600(Int32 idCompany, Int64 selectedPeriod)
        {
            Company company = new Company();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                company = mgr.GetCompanyDetailsByCompanyIdSelectedPeriod_V2600(WorkbenchConnectionString, idCompany, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod);
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

        // [nsatpute][09-01-2025][GEOS2-6776]
        public Employee GetEmployeeByIdEmployee_V2600(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2600(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-6760][rdixit][09.01.2025]
        public List<Traveller> GetActiveEmployeesForTraveller_V2600(string IdCompany, long selectedPeriod, string idsOrganization, string idsDepartments, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetActiveEmployeesForTraveller_V2600(workbenchConnectionString, IdCompany, selectedPeriod, idsOrganization, idsDepartments, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-6760][rdixit][09.01.2025]
        public EmployeeTrips GetEditEmployeeTripDetails_V2600(UInt32 idEmployeeTrip)
        {
            EmployeeTrips companies = new EmployeeTrips();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetEditEmployeeTripDetails_V2600(WorkbenchConnectionString, idEmployeeTrip);
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
        //[GEOS2-6760][rdixit][09.01.2025]
        public EmployeeTrips AddEmployeeTripDetails_V2600(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2600(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-6760][rdixit][09.01.2025]
        public EmployeeTrips EditEmployeeTripDetails_V2600(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2600(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[GEOS2-6760][rdixit][14.01.2025]
        public List<Employee> HRM_GetResponsibleByPlantAndIdEmployee_V2600(string idCompanies, uint idEmployee, uint idDept)
        {
            List<Employee> responsibleList = new List<Employee>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                responsibleList = mgr.HRM_GetResponsibleByPlantAndIdEmployee_V2600(WorkbenchConnectionString, idCompanies, idEmployee, idDept);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return responsibleList;
        }
        // [nsatpute][13-01-2025][GEOS2-6776]
        public List<Company> GetAuthorizedPlantsByIdUser_V2600(Int32 idUser)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAuthorizedPlantsByIdUser_V2600(workbenchConnectionString, idUser);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][16-01-2025][GEOS2-6862]
        public List<Employee> GetEmployeeBacklogHours_V2600(List<Employee> lstEmployees)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeBacklogHours_V2600(workbenchConnectionString, lstEmployees);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }


        //pramod.misal GEOS2-5077 06.02.2024
        //Shubham[skadam] GEOS2-6037 HRM Expense Report - Time to Generate Expense tickets 05 08 2024
        //Shubham[skadam] GEOS2-6500 HRM Expenses report not working properly.  17 01 2024
        public List<EmployeeExpensePhotoInfo> HRM_GetAllExpenseAttachmentsByExpenseReportId_V2600(int idEmployeeExpense)
        {
            List<EmployeeExpensePhotoInfo> EmployeeExpensePhotoInfoList = new List<EmployeeExpensePhotoInfo>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeExpensePhotoInfoList = mgr.HRM_GetAllExpenseAttachmentsByExpenseReportId_V2600(workbenchConnectionString, idEmployeeExpense, Properties.Settings.Default.ExpensesAttachment);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeExpensePhotoInfoList;
        }

        //[rdixit][29.01.2025][GEOS2-6826]
        public Employee IsExistDocumentNumberInAnotherEmployee_V2610(string employeeDocumentNumber, Int32 idEmployee, Int32 employeeDocumentIdType, Int32 idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.IsExistDocumentNumberInAnotherEmployee_V2610(workbenchConnectionString, employeeDocumentNumber, idEmployee, employeeDocumentIdType, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][10.02.2025][GEOS2-6850]
        public List<EmployeeAnnualLeave> GetAnnualLeavesByIdEmployees(string idEmployeeList, long selectedPeriod)
        {
            List<EmployeeAnnualLeave> EmployeeAnnualLeaveList = new List<EmployeeAnnualLeave>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeAnnualLeaveList = mgr.GetAnnualLeavesByIdEmployees(workbenchConnectionString, idEmployeeList, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeAnnualLeaveList;
        }

        //[rdixit][GEOS2-6872][10.02.2025]
        public bool UpdateEmployee_V2610(Employee employee)
        {
            bool isUpdated = false;

            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateEmployee_V2610(WorkbenchConnectionString, employee, Properties.Settings.Default.EmployeesProfileImage, Properties.Settings.Default.EmpEducationQualificationFilesPath, Properties.Settings.Default.EmpProfessionalEducationFilesPath, Properties.Settings.Default.EmpContractSituationFilesPath, Properties.Settings.Default.EmpIdentificationDocumentFilesPath, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument);
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

        //[rdixit][13.02.2025][GEOS2-3424]
        public List<Employee> HRM_GetDraftToActiveEmployees_V2610()
        {
            List<Employee> EmployeeList = new List<Employee>();
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                EmployeeList = mgr.HRM_GetDraftToActiveEmployees_V2610(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return EmployeeList;
        }

        //[rdixit][13.02.2025][GEOS2-3424]
        public bool InsertUpdateEmployeeProfessionalEmail_V2610(Employee employee)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.InsertUpdateEmployeeProfessionalEmail_V2610(workbenchConnectionString, employee);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

         //[pallavi.kale][GEOS2-2497][07-03-25]
        public Company GetCompanyDetailsByCompanyIdSelectedPeriod_V2620(Int32 idCompany, Int64 selectedPeriod)
        {
            Company company = new Company();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                company = mgr.GetCompanyDetailsByCompanyIdSelectedPeriod_V2620(WorkbenchConnectionString, idCompany, Properties.Settings.Default.EmployeesProfileImage, selectedPeriod);
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

        //[pallavi.kale][GEOS2-2497][07-03-25]
        public Company UpdateCompany_V2620(Company company)
        {
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                company = mgr.UpdateCompany_V2620(WorkbenchConnectionString, company, Properties.Settings.Default.SiteImage);
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

        //[Sudhir.Jangra][GEOS2-5656]
        public List<Employee> GetAllEmployeesByIdCompany_V2620(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByIdCompany_V2620(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5659][17.03.2025]
        public List<JobDescription> GetEmployeesCountByJobPositionByIdCompany_V2620(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesCountByJobPositionByIdCompany_V2620(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Shweta.Thube][GEOS2-5660]
        public List<Department> GetLengthOfServiceByDepartment_V2620(string idCompanies, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetLengthOfServiceByDepartment_V2620(workbenchConnectionString, idCompanies, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-5511]
        public bool UpdateEmployeeAttendance_V2620(EmployeeAttendance employeeAttendance)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.UpdateEmployeeAttendance_V2620(workbenchConnectionString, employeeAttendance);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][21.03.2025][GEOS2-5661]
        public List<Employee> GetEmployeesWithAnniversaryDate_V2620(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeesWithAnniversaryDate_V2620(workbenchConnectionString, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][26-03-2025][GEOS2-7011]
        public Dictionary<int, string> GetSitesPetronalNumbers()
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSitesPetronalNumbers(workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-5659][27.03.2025]
        public List<Department> GetAllEmployeesForOrganizationByIdCompany_V2630(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesForOrganizationByIdCompany_V2630(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

		//[nsatpute][31-03-2025][GEOS2-7611]
        public Employee GetEmployeeByIdEmployee_V2630(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2630(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-6979][02.04.2025]
        public bool UpdateTravelExpenses_V2630(TravelExpenses travelExpense, int modifiedBy, DateTime modificationDate, List<LogEntriesByTravelExpense> expenseLogList, List<Expenses> expenseList)
        {
            bool isUpdated = false;
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                isUpdated = mgr.UpdateTravelExpenses_V2630(travelExpense, modifiedBy, modificationDate, expenseLogList, expenseList, WorkbenchConnectionString);
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

        //[rdixit][GEOS2-7799][10.04.2025]
        public List<Department> GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2630(string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAllEmployeesByDepartmentByIdCompanyForAttendance_V2630(workbenchConnectionString, Properties.Settings.Default.EmployeesProfileImage, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-4472][rdixit][05.07.2023]
        //Shubham[skadam] GEOS2-7731 Need to improvement in Issue with Report Display in GEOS  18 04 2025
        public Dictionary<string, byte[]> GetEmployeeExpenseReport_V2630(string plantName, string EmployeeCode, string ExpenseCode)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;
            string ExpenseReportFilePath = string.Empty;
            fileUploadPath = Properties.Settings.Default.ExpenseReportPath.Replace("{0}", plantName);
            Dictionary<string, byte[]> ResultFile = new Dictionary<string, byte[]>();
            if (!fileUploadPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                fileUploadPath += Path.DirectorySeparatorChar;
            }
            fileUploadPath = fileUploadPath + EmployeeCode + @"\" + ExpenseCode + @"\";
            if (!System.IO.Directory.Exists(fileUploadPath))
            {
                System.IO.Directory.CreateDirectory(fileUploadPath);
            }
            ExpenseReportFilePath = fileUploadPath;
            string search = ExpenseCode + "_" + '*' + ".pdf";
            FileInfo[] files = new DirectoryInfo(fileUploadPath).GetFiles(search).ToArray();

            List<string> maxNumberList = new List<string>();
            string containstr = "";
            string MaxRevFileName = "";
            foreach (var maxNumberitem in files)
            {
                int indexofrev = maxNumberitem.Name.IndexOf("rev");
                string substring = "00";
                if (indexofrev == -1)
                {
                    substring = "00";
                }
                else
                {
                    substring = maxNumberitem.Name.Substring(indexofrev + 3, 2);
                }
                maxNumberList.Add(substring);
            }

            List<string> finalpdflist = new List<string>();
            var allfiles = files.ToList();

            if (allfiles.Count() > 0)
            {
                allfiles.RemoveAll(r => r.Name.Contains("Receipts"));
                try
                {
                    string maxNumber = maxNumberList.Max();
                    if (maxNumber != "00")
                    {
                        foreach (FileInfo latest in allfiles)
                        {
                            if (latest.Name.Contains("rev" + maxNumber))
                            {
                                containstr = latest.Name;
                                var fileName = Directory.GetFiles(ExpenseReportFilePath, "*" + containstr + "*.*", SearchOption.AllDirectories).Where(fi => fi.Contains(".pdf")).FirstOrDefault();
                                if (!containstr.Contains("Receipts"))
                                {
                                    MaxRevFileName = fileName;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                }
                if (File.Exists(MaxRevFileName))
                {
                    fileUploadPath = MaxRevFileName;
                }
                else
                {
                    var test = allfiles.Max(f => f.LastWriteTimeUtc);
                    fileUploadPath = files.FirstOrDefault(k => k.LastWriteTimeUtc == test).FullName;
                }
            }
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }
                if (bytes != null)
                    ResultFile.Add(Path.GetFileName(fileUploadPath), bytes);
                else
                    ResultFile.Add(ExpenseReportFilePath, bytes);
                return ResultFile;
            }
            catch (FileNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000050";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (DirectoryNotFoundException ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                exp.ErrorCode = "000051";
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [nsatpute][12-05-2025][GEOS2-5707]
        public List<Recruitment> GetEmployeeRecruitmentByIdSite(string idCompany, long selectedPeriod)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeRecruitmentByIdSite(workbenchConnectionString, idCompany, selectedPeriod);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [rdixit][12.05.2025][GEOS2-7018][GEOS2-7761][GEOS2-7793][GEOS2-7796]
        public List<EmployeeAttendance> GetSelectedEmployeeAttendance_V2640(int idEmployee, string idCompany, long selectedPeriod, string idsOrganization, string idsDepartment, Int32 idPermission, DateTime fromDate, DateTime toDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetSelectedEmployeeAttendance_V2640(workbenchConnectionString, idEmployee, idCompany, selectedPeriod, idsOrganization, idsDepartment, idPermission, fromDate, toDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
		//[nsatpute][16-05-2025][GEOS2-6617]
        public EmployeeHoliday GetEmployeeHolidaysByEmail(string email, DateTime currentDate)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidaysByEmail(workbenchConnectionString, email, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-7987][23.05.2025][rdixit]
        public List<EmployeeContact> GetCCMailsFor25YearAnniversaryEmployees(int idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetCCMailsFor25YearAnniversaryEmployees(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [pallavi.kale][28-05-2025][GEOS2-7941]
        public EmployeeTrips EditEmployeeTripDetails_V2650(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2650(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        // [pallavi.kale][28-05-2025][GEOS2-7941]
        public EmployeeTrips AddEmployeeTripDetails_V2650(EmployeeTrips employeeTrip)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2650(workbenchConnectionString, employeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        // [pallavi.kale][28-05-2025][GEOS2-7941]
        public EmployeeTrips GetEditEmployeeTripDetails_V2650(UInt32 idEmployeeTrip)
        {
            EmployeeTrips companies = new EmployeeTrips();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.GetEditEmployeeTripDetails_V2650(WorkbenchConnectionString, idEmployeeTrip);
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

        //[GEOS2-7987][23.05.2025][rdixit]
        public List<EmployeeContact> GetBCCMailsFor25YearAnniversaryEmployees(int idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetBCCMailsFor25YearAnniversaryEmployees(workbenchConnectionString, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //chitra.girigosavi GES2-6813 26/06/2025
        public List<EmployeeHoliday> GetEmployeeHolidays_V2650(Int32 idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeHolidays_V2650(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-8833][rdixit][21.08.2025]
        public List<EmployeeContact> GetEmployeeContactsByJD_V2660(int idCompany, int idJobDescription)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeContactsByJD_V2660(workbenchConnectionString, idCompany, idJobDescription);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-9423][08.09.2025]
        public List<Employee> GetTodayEmployeeCompanyAnniversariesDetailsNew_V2670(DateTime currentDate, int idCompany)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayEmployeeCompanyAnniversariesDetailsNew_V2670(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[rdixit][GEOS2-9435][09.09.2025]
        public List<Employee> GetTodayBirthdayOfEmployees_V2670(int idCompany, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTodayBirthdayOfEmployees_V2670(workbenchConnectionString, idCompany, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }


        //[cpatil][12-09-2025][GEOS2-6971]
        public Employee GetEmployeeByIdEmployee_V2670(Int32 idEmployee, Int64 selectedPeriod = 0, string idCompany = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeByIdEmployee_V2670(workbenchConnectionString, idEmployee, Properties.Settings.Default.EmployeeExitDocument, Properties.Settings.Default.EmployeeEquipmentDocument, selectedPeriod, idCompany);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        public List<Employee_trips_transfers> HRM_GetArrivalTransferDetails_V2670(UInt32 idEmployeeTrip)
        {
            List<Employee_trips_transfers> companies = new List<Employee_trips_transfers>();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                companies = mgr.HRM_GetArrivalTransferDetails_V2670(WorkbenchConnectionString, idEmployeeTrip);
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
        //[Rahul.Gadhave][GEOS2-7989][Date:22-09-2025]
        public EmployeeTrips EditEmployeeTripDetails_V2670(EmployeeTrips employeeTrip, List<Employee_trips_transfers> transfersList, List<EmployeeTripsAccommodations> accommodationsList)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.EditEmployeeTripDetails_V2670(workbenchConnectionString, employeeTrip, transfersList, accommodationsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[Rahul.Gadhave][GEOS2-7989][Date:22-09-2025]
        public EmployeeTrips AddEmployeeTripDetails_V2670(EmployeeTrips employeeTrip,List<Employee_trips_transfers> transfersList, List<EmployeeTripsAccommodations> accommodationsList)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["MainServerWorkbenchContext"].ConnectionString;
                return mgr.AddEmployeeTripDetails_V2670(workbenchConnectionString, employeeTrip,transfersList, accommodationsList);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[shweta.thube][GEOS2-7989][26.09.2025]
        public List<EmployeeTripsAccommodations> GetAccommodationsDetails_V2670(UInt32 idEmployeeTrip)
        {
            EmployeeTripsAccommodations companies = new EmployeeTripsAccommodations();
            try
            {
                string WorkbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetAccommodationsDetails_V2670(WorkbenchConnectionString, idEmployeeTrip);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][07.10.2025][GEOS2-6367]
        public TripExpensesReport GetTripExpensesReport_V2680(UInt32 idEmployeeTrip, Int32 IdCurrencyTo)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetTripExpensesReport_V2680(idEmployeeTrip, IdCurrencyTo, workbenchConnectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        //[GEOS2-9059][rdixit][08.10.2025]
        public List<EmployeeContractSituation> GetEmployeeContractExpirations_V2680(Int32 idCompany, ref List<string> emails, DateTime? currentDate = null)
        {
            try
            {
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetEmployeeContractExpirations_V2680(workbenchConnectionString, idCompany, ref emails, currentDate);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }
        //[nsatpute][08.10.2025][GEOS2-6367]
        public string GetServerCultureInfo()
        {
            try
            {
                return CultureInfo.CurrentCulture.Name; // e.g., "en-US", "fr-FR"
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
