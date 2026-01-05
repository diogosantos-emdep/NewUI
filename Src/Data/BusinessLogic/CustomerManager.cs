using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.DataAccess;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.BusinessLogic
{
    public partial class CustomerManager
    {
        /// <summary>
        /// This method is to get all customers
        /// </summary>
        /// <returns>List of customer</returns>
        public IList<Customer> GetCustomers(string connectionString)
        {
            IList<Customer> customers = new List<Customer>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("GetGeosCustomers", myConnection);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCompany = Convert.ToInt32(reader["IdCompany"].ToString());
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["CustomerName"].ToString();
                        customer.Email = reader["Email"].ToString();
                        customers.Add(customer);
                    }
                }

            }

            return customers;
        }


        /// <summary>
        /// This method is to get company group details
        /// </summary>
        /// <param name="connectionString">Get connection string</param>
        /// <param name="idUser">Get user id</param>
        /// <param name="idZone">Get zone id</param>
        /// <returns>List of customer</returns>
        public List<Customer> GetCompanyGroup(string connectionString, Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = new List<Customer>();
            if (isIncludeDefault)
            {
                Customer customerDefault = new Customer();
                customerDefault.IdCustomer = 0;
                customerDefault.CustomerName = "---";
                customers.Add(customerDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("customers_GetCompanyGroup", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString();
                        customers.Add(customer);
                    }
                }

            }

            return customers;
        }

        public List<Customer> GetSelectedUserCompanyGroup(string connectionString, string idUser, bool isIncludeDefault = false)
        {
            List<Customer> customers = new List<Customer>();
            if (isIncludeDefault)
            {
                Customer customerDefault = new Customer();
                customerDefault.IdCustomer = 0;
                customerDefault.CustomerName = "---";
                customers.Add(customerDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("customers_GetSelectedUserCompanyGroup", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString();
                        customers.Add(customer);
                    }
                }

            }

            return customers;
        }

        public bool IsExistCustomer(string connectionString, string customerName)
        {
            bool isExist = false;

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("select * from customers where Name = '" + customerName.ToUpper() + "'", myConnection);
                myCommand.CommandType = System.Data.CommandType.Text;

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isExist = true;
                    }
                }

            }
            return isExist;
        }


        /// <summary>
        /// This method is to get company plant details by customer id
        /// </summary>
        /// <param name="connectionString">Get connection string</param>
        /// <param name="idCustomer">Get customer id</param>
        /// <returns>List of company</returns>
        public List<Company> GetCompanyPlantByCustomerId(string connectionString, Int32 idCustomer, Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = new List<Company>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetCompanyPlantByCustomerIdAndUserId", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idCustomer", idCustomer);
                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();

                        if (reader["IdSalesResponsible"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"].ToString());

                        if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());

                        if (reader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(reader["Latitude"].ToString());

                        if (reader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(reader["Longitude"].ToString());

                        companies.Add(company);
                    }
                }

            }
            return companies;
        }


        public List<Company> GetSelectedUserCompanyPlantByCustomerId(string connectionString, Int32 idCustomer, string idUser)
        {
            List<Company> companies = new List<Company>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetSelectedUserCompanyPlantByCustomerIdAndUserId", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idCustomer", idCustomer);
                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();

                        if (reader["IdSalesResponsible"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"].ToString());

                        if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());

                        if (reader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(reader["Latitude"].ToString());

                        if (reader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(reader["Longitude"].ToString());

                        companies.Add(company);
                    }
                }
            }
            return companies;
        }

        public List<Customer> GetAllCustomer(string connectionString)
        {
            List<Customer> customers = new List<Customer>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("select IdCustomer,Name from customers", myConnection);
                myCommand.CommandType = System.Data.CommandType.Text;

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString().ToUpper();
                        customers.Add(customer);
                    }
                }

            }
            return customers;
        }


        public List<Company> GetSelectedSalesOwnerSites(string connectionString, string idUser)
        {
            List<Company> companies = new List<Company>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetSelectedSalesOwnerSites", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();
                        company.GroupPlantName = reader["GroupPlantName"].ToString();
                        if (reader["IdSalesResponsible"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"].ToString());

                        if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());
                        companies.Add(company);
                    }
                }
            }
            return companies;
        }

        public List<Company> GetSelectedUserCustomerPlant(string connectionString, string idUser)
        {
            List<Company> companies = new List<Company>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetSelectedUserCustomerPlant", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader companyReader = myCommand.ExecuteReader())
                {
                    while (companyReader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(companyReader["IdSite"].ToString());
                        company.Name = companyReader["CustomerPlant"].ToString();

                        if (companyReader["IdCustomer"] != DBNull.Value)
                        {
                            company.IdCustomer = Convert.ToInt32(companyReader["IdCustomer"].ToString());
                            company.Customers = new List<Customer> { new Customer { IdCustomer = Convert.ToInt32(companyReader["IdCustomer"].ToString()), CustomerName = companyReader["CustomerGroup"].ToString() } };
                        }

                        company.GroupPlantName = companyReader["GroupPlantName"].ToString();

                        if (companyReader["Address"] != DBNull.Value)
                            company.Address = companyReader["Address"].ToString();

                        if (companyReader["City"] != DBNull.Value)
                            company.City = companyReader["City"].ToString();

                        if (companyReader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(companyReader["Latitude"].ToString());

                        if (companyReader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(companyReader["Longitude"].ToString());

                        companies.Add(company);
                    }
                }
            }
            return companies;
        }

        public List<Company> GetCustomerPlant(string connectionString, Int32 idUser, Int32 idZone, Int32 idUserPermission)
        {
            List<Company> companies = new List<Company>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetCustomerPlant", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);

                using (MySqlDataReader companyreader = myCommand.ExecuteReader())
                {
                    while (companyreader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(companyreader["IdSite"].ToString());
                        company.Name = companyreader["CustomerPlant"].ToString();
                        if (companyreader["IdCustomer"] != DBNull.Value)
                        {
                            company.IdCustomer = Convert.ToInt32(companyreader["IdCustomer"].ToString());
                            company.Customers = new List<Customer> { new Customer { IdCustomer = Convert.ToInt32(companyreader["IdCustomer"].ToString()), CustomerName = companyreader["CustomerGroup"].ToString() } };
                        }
                        company.GroupPlantName = companyreader["GroupPlantName"].ToString();

                        companies.Add(company);
                    }
                }
            }
            return companies;
        }


        /// <summary>
        /// Sprint 23 - [CRM-M023-04]
        /// Accounts not displayed properly when adding new activity for role22  (same like role 22 in account section).
        /// </summary>
        /// <param name="connectionString">The CRM context connection string.</param>
        /// <param name="idActiveUser">The active user.</param>
        /// <param name="idSite">The selected idplant from plant owner conbobox.</param>
        /// <returns>List of companies.</returns>
        public List<Company> GetAccountBySelectedPlant(string connectionString, Int32 idActiveUser, string idSite)
        {
            List<Company> companies = new List<Company>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetAccountBySelectedPlant", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idActiveUser", idActiveUser);
                myCommand.Parameters.AddWithValue("_idSite", idSite);

                using (MySqlDataReader companyReader = myCommand.ExecuteReader())
                {
                    while (companyReader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(companyReader["IdSite"].ToString());
                        company.Name = companyReader["CustomerPlant"].ToString();

                        if (companyReader["IdCustomer"] != DBNull.Value)
                        {
                            company.IdCustomer = Convert.ToInt32(companyReader["IdCustomer"].ToString());
                            company.Customers = new List<Customer> { new Customer { IdCustomer = Convert.ToInt32(companyReader["IdCustomer"].ToString()), CustomerName = companyReader["CustomerGroup"].ToString() } };
                        }

                        company.GroupPlantName = companyReader["GroupPlantName"].ToString();

                        if (companyReader["Address"] != DBNull.Value)
                            company.Address = companyReader["Address"].ToString();

                        if (companyReader["City"] != DBNull.Value)
                            company.City = companyReader["City"].ToString();

                        if (companyReader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(companyReader["Latitude"].ToString());

                        if (companyReader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(companyReader["Longitude"].ToString());

                        companies.Add(company);
                    }
                }
            }

            return companies;
        }

        public List<Company> GetSelectedUserCompanyPlantByIdUser(string connectionString, string idUser, bool isIncludeDefault = false)
        {
            List<Company> companies = new List<Company>();
            if (isIncludeDefault)
            {
                Company companyDefault = new Company();
                companyDefault.IdCompany = 0;
                companyDefault.Name = "---";
                companyDefault.SiteNameWithoutCountry = "---";
                companies.Add(companyDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetSelectedUserCompanyPlantByIdUser", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();

                        if (reader["IdSalesResponsible"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"].ToString());

                        if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());

                        if (reader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(reader["Latitude"].ToString());

                        if (reader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(reader["Longitude"].ToString());

                        if (reader["IdCustomer"] != DBNull.Value)
                            company.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());

                        companies.Add(company);
                    }
                }
            }
            return companies;
        }


        public List<Company> GetCompanyPlantByUserId(string connectionString, Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Company> companies = new List<Company>();
            if (isIncludeDefault)
            {
                Company companyDefault = new Company();
                companyDefault.IdCompany = 0;
                companyDefault.Name = "---";
                companyDefault.SiteNameWithoutCountry = "---";
                companies.Add(companyDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetCompanyPlantByUserId", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();

                        if (reader["IdCustomer"] != DBNull.Value)
                            company.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());

                        if (reader["IdSalesResponsible"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["IdSalesResponsible"].ToString());

                        if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                            company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());

                        if (reader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(reader["Latitude"].ToString());

                        if (reader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(reader["Longitude"].ToString());

                        companies.Add(company);
                    }
                }

            }
            return companies;
        }


        public bool IsExistCustomer_V2040(string connectionString, string customerName, byte idCustomerType)
        {
            bool isExist = false;

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("CRM_IsExistCustomer_V2040", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_CustomerName", customerName.ToUpper());
                myCommand.Parameters.AddWithValue("_IdCustomerType", idCustomerType);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            if (reader["IdCustomer"] != DBNull.Value)
                                isExist = true;
                        }
                        catch (Exception ex)
                        {
                            reader.Close();
                            myConnection.Close();
                            throw;
                        }
                      
                    }
                }

            }
            return isExist;
        }


        //[GEOS2-3144] [GEOS2-3145] [GEOS2-3185] [YJoshi]
        //[CCI EHQ2002 - Price Matrix - Automatic selection in CRM & GOM - Add MartixList And load the grid details in Configuration section]
        //[nsatpute][05-02-2025][GEOS2-6921]
        public List<Customer> GetAllCustomersDetails_V2160(string connectionString)
        {
            var customers = new List<Customer>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                using (var myCommand = new MySqlCommand("CRM_GetAllCustomersDetails_V2160", myConnection) { CommandType = System.Data.CommandType.StoredProcedure })
                {
                    using (MySqlDataReader reader = myCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customer = new Customer();

                            if (reader["IdCustomer"] != DBNull.Value)
                                customer.IdCustomer =
                                    Convert.ToInt32(reader["IdCustomer"].ToString());

                            if (reader["Name"] != DBNull.Value)
                                customer.CustomerName =
                                    Convert.ToString(reader["Name"].ToString());

                            if (reader["IdCustomerType"] != DBNull.Value)
                                customer.IdCustomerType =
                                    Convert.ToByte(reader["IdCustomerType"].ToString());

                            if (reader["Logo"] != DBNull.Value)
                                customer.Logo =
                                    Convert.ToString(reader["Logo"].ToString());

                            if (reader["PatternForConnectorReferences"] != DBNull.Value)
                                customer.PatternForConnectorReferences =
                                    Convert.ToString(reader["PatternForConnectorReferences"].ToString());

                            if (reader["Web"] != DBNull.Value)
                                customer.Web =
                                    Convert.ToString(reader["Web"].ToString());

                            if (reader["IsStillActive"] != DBNull.Value)
                                customer.IsStillActive =
                                    Convert.ToSByte(reader["IsStillActive"]);

                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers;
        }

        //[GEOS2-4120][rdixit][10.01.2022]
        public bool IsExistCustomer_V2350(string connectionString, string customerName, byte idCustomerType)
        {
            bool isExist = false;

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("CRM_IsExistCustomer_V2040", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_CustomerName", customerName.ToUpper());
                myCommand.Parameters.AddWithValue("_IdCustomerType", idCustomerType);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            if (reader["IdCustomer"] != DBNull.Value)
                                isExist = true;
                        }
                        catch (Exception ex)
                        {
                            reader.Close();
                            myConnection.Close();
                            throw;
                        }

                    }
                }

            }
            return isExist;
        }

        //[pramod.misal][GEOS2-4682][07/08/2023]
        public List<Customer> GetCompanyGroup_V2420(string connectionString, Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = new List<Customer>();
            if (isIncludeDefault)
            {
                Customer customerDefault = new Customer();
                customerDefault.IdCustomer = 0;
                customerDefault.CustomerName = "---";
                customers.Add(customerDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("customers_GetCompanyGroup_V2420", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString();
                        customers.Add(customer);
                    }
                }

            }

            return customers;
        }

        //[pramod.misal][GEOS2-4682][08/08/2023]
        public List<Company> GetSelectedUserCompanyPlantByIdUser_V2420(string connectionString, string idUser, bool isIncludeDefault = false)
        {
            List<Company> companies = new List<Company>();
            if (isIncludeDefault)
            {
                Company companyDefault = new Company();
                companyDefault.IdCompany = 0;
                companyDefault.Name = "---";
                companyDefault.SiteNameWithoutCountry = "---";
                companies.Add(companyDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetSelectedUserCompanyPlantByIdUser_V2420", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();

                        if (reader["SalesOwner"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["SalesOwner"].ToString());

                        //if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                        //    company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());

                        if (reader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(reader["Latitude"].ToString());

                        if (reader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(reader["Longitude"].ToString());

                        if (reader["IdCustomer"] != DBNull.Value)
                            company.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());

                        companies.Add(company);
                    }
                }
            }
            return companies;
        }

        //[pramod.misal][GEOS2-4682][08/08/2023]
        public List<Company> GetCompanyPlantByUserId_V2420(string connectionString, Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Company> companies = new List<Company>();
            if (isIncludeDefault)
            {
                Company companyDefault = new Company();
                companyDefault.IdCompany = 0;
                companyDefault.Name = "---";
                companyDefault.SiteNameWithoutCountry = "---";
                companies.Add(companyDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("sites_GetCompanyPlantByUserId_V2420", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company();
                        company.IdCompany = Convert.ToInt32(reader["IdSite"].ToString());
                        company.Name = reader["Name"].ToString();
                        company.Country = new Country { IdCountry = Convert.ToByte(reader["idCountry"].ToString()), Name = reader["Country"].ToString(), Zone = new Zone { IdZone = Convert.ToInt32(reader["IdZone"].ToString()), Name = reader["Zone"].ToString() } };
                        company.IdCountry = Convert.ToByte(reader["idCountry"].ToString());
                        company.Address = reader["Address"].ToString();
                        company.City = reader["City"].ToString();
                        company.Region = reader["Region"].ToString();
                        company.ZipCode = reader["PostCode"].ToString();
                        company.Telephone = reader["Telephone"].ToString();
                        company.SiteNameWithoutCountry = reader["SiteName"].ToString();

                        if (reader["IdCustomer"] != DBNull.Value)
                            company.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());

                        if (reader["SalesOwner"] != DBNull.Value)
                            company.IdSalesResponsible = Convert.ToInt32(reader["SalesOwner"].ToString());

                        //if (reader["IdSalesResponsibleAssemblyBU"] != DBNull.Value)
                        //    company.IdSalesResponsibleAssemblyBU = Convert.ToInt32(reader["IdSalesResponsibleAssemblyBU"].ToString());

                        if (reader["Latitude"] != DBNull.Value)
                            company.Latitude = Convert.ToDouble(reader["Latitude"].ToString());

                        if (reader["Longitude"] != DBNull.Value)
                            company.Longitude = Convert.ToDouble(reader["Longitude"].ToString());

                        companies.Add(company);
                    }
                }

            }
            return companies;
        }

        //[GEOS2-4682][08/08/2023][rdixit] 
        public List<Customer> GetSelectedUserCompanyGroup_V2420(string connectionString, string idUser, bool isIncludeDefault = false)
        {
            List<Customer> customers = new List<Customer>();
            if (isIncludeDefault)
            {
                Customer customerDefault = new Customer();
                customerDefault.IdCustomer = 0;
                customerDefault.CustomerName = "---";
                customers.Add(customerDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("customers_GetSelectedUserCompanyGroup_V2420", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString();
                        customers.Add(customer);
                    }
                }

            }

            return customers;
        }

        //[19.10.2023][GEOS2-4903][rdixit] 
        public List<Customer> GetCompanyGroup_V2450(string connectionString, Int32 idUser, Int32 idZone, Int32 idUserPermission, bool isIncludeDefault = false)
        {
            List<Customer> customers = new List<Customer>();
            if (isIncludeDefault)
            {
                Customer customerDefault = new Customer();
                customerDefault.IdCustomer = 0;
                customerDefault.CustomerName = "---";
                customers.Add(customerDefault);
            }

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("customers_GetCompanyGroup_V2450", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                myCommand.Parameters.AddWithValue("_idUser", idUser);
                myCommand.Parameters.AddWithValue("_idZone", idZone);
                myCommand.Parameters.AddWithValue("_idPermission", idUserPermission);
                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString();
                        customers.Add(customer);
                    }
                }

            }

            return customers;
        }

        //chitra.girigosavi[GEOS2-7207][25/03/2025]
        public List<Customer> GetAllCustomer_V2630(string connectionString)
        {
            List<Customer> customers = new List<Customer>();

            using (MySqlConnection myConnection = new MySqlConnection(connectionString))
            {
                myConnection.Open();
                MySqlCommand myCommand = new MySqlCommand("CRM_GetAllCustomer_V2630", myConnection);
                myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                using (MySqlDataReader reader = myCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.IdCustomer = Convert.ToInt32(reader["IdCustomer"].ToString());
                        customer.CustomerName = reader["Name"].ToString().ToUpper();
                        customers.Add(customer);
                    }
                }

            }
            return customers;
        }
    }
}
