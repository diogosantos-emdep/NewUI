using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Entities;
using System.Data;

namespace BusinessLogic
{
    public class CustomerManager
    {
        string _ConnString;
        public CustomerManager(string ConnString)
        {
            this._ConnString = ConnString;
        }

        public List<Customer> GetCustomers(string Plants)
        {
            List<Customer> Customers = new List<Customer>();
            List<string> AllPlants = new List<string>();
            string IDOffers = string.Empty;
            try
            {
                if (CommonManager.DictonarySitesWithIds == null || CommonManager.DictonarySitesWithIds.Count == 0)
                {
                    CommonManager.ConnString = _ConnString;
                    CommonManager.GetAllSites();
                }
                if (Plants=="0")
                {
                    
                    foreach (KeyValuePair<string,string> item in CommonManager.DictonarySitesWithIds)
                    {
                        AllPlants.Add(item.Value);
                    }
                }
                else
                {
                    string[] PlantsArray = Plants.Split(',');
                    foreach (string plant in PlantsArray)
                    {
                        if (CommonManager.DictonarySitesWithIds.ContainsKey(plant))
                            AllPlants.Add(CommonManager.DictonarySitesWithIds[plant]);
                    }
                }
                
                IDOffers = String.Join(",", ((List<string>)AllPlants));
                //if(CommonManager.DictonarySitesWithIds)
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("sites_GetSelectedUserCustomersByPlant", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idSite", IDOffers);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Customer customer = new Customer();
                            customer.Id = Convert.ToInt32(dr["IdCustomer"]);
                            customer.Group = dr["CustomerGroup"].ToString();
                            customer.Plant = dr["CustomerPlant"].ToString();
                            customer.RegisteredName = dr["RegisteredName"].ToString();
                            customer.RegistrationNumber = dr["RegistrationNumber"].ToString();
                            customer.Address = dr["Address"].ToString();
                            customer.City = dr["City"].ToString();
                            customer.Zipcode = dr["PostCode"].ToString();
                            customer.State = dr["Region"].ToString();
                            customer.Country = dr["Country"].ToString();
                            customer.Region = dr["SalesZone"].ToString();
                            customer.Email = dr["Email"].ToString();
                            customer.Fax = dr["Fax"].ToString();
                            customer.Website = dr["WebSite"].ToString();
                            customer.Phone = dr["Phone"].ToString();
                            customer.BusinessField = dr["BusinessField"].ToString();
                            customer.BusinessType = dr["BusinessType"].ToString();
                            customer.BusinessProduct = GetBusinessProduct(Convert.ToInt32(dr["IdSite"]));
                            customer.Size = dr["Size"].ToString();
                            customer.NumberOfEmployees = dr["NumberOfEmployees"].ToString();
                            customer.NumberOfCuttingMachines = dr["CuttingMachines"].ToString();
                            customer.NumberOfLines = dr["Line"].ToString();
                            if (dr["CreatedIn"] != DBNull.Value)
                            {
                                customer.Age = Math.Round((double)((DateTime.Now.Month - Convert.ToDateTime(dr["CreatedIn"]).Month) + 12 * (DateTime.Now.Year - Convert.ToDateTime(dr["CreatedIn"]).Year)) / 12, 1);
                                customer.CreationDate = Convert.ToDateTime(dr["CreatedIn"]).ToString("yyyy-MM-dd");
                            }
                                
                            
                            customer.SalesOwner = dr["SalesResponsibleName"].ToString() + " " + dr["SalesResponsibleSurname"].ToString();
                            Customers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Customers;
        }
        public string GetBusinessProduct(int IdCompany)
        {
            string BusinessProduct = string.Empty;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("sitesbybusinessproduct_GetBusinessProduct", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idCompany", IdCompany);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if(string.IsNullOrEmpty(BusinessProduct))
                                BusinessProduct = dr["Value"].ToString();
                            else
                                BusinessProduct = BusinessProduct + "," + dr["Value"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BusinessProduct;
        }
    }
}
