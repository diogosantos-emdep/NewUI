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
    public class ContactManager
    {
        string _ConnString;
        public ContactManager(string ConnString)
        {
            this._ConnString = ConnString;
        }

        public List<Contact> GetContacts(string Plants)
        {
            List<Contact> Contacts = new List<Contact>();
            List<string> AllPlants = new List<string>();
            string IDOffers = string.Empty;
            try
            {
                if (CommonManager.DictonarySitesWithIds == null || CommonManager.DictonarySitesWithIds.Count == 0)
                {
                    CommonManager.ConnString = _ConnString;
                    CommonManager.GetAllSites();
                }
                if (Plants == "0")
                {

                    foreach (KeyValuePair<string, string> item in CommonManager.DictonarySitesWithIds)
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
                    MySqlCommand command = new MySqlCommand("people_GetSelectedUserContactsByPlant", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("_idSite", IDOffers);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Contact contact = new Contact();
                            contact.Id = Convert.ToInt32(dr["IdPerson"]);
                            contact.FirstName= dr["FirstName"].ToString();
                            contact.LastName = dr["LastName"].ToString();
                            contact.Department = dr["value"].ToString();
                            contact.JobTitle = dr["JobTitle"].ToString();
                            contact.Group = dr["CustomerGroup"].ToString();
                            contact.Plant = dr["CustomerPlant"].ToString();
                            contact.Country = dr["Country"].ToString();
                            contact.Region = dr["SiteRegion"].ToString();
                            contact.Email = dr["Email"].ToString();
                            contact.Phone = dr["Telephone"].ToString();
                            contact.EmdepAffinity = dr["EmdepAffinity"].ToString();
                            contact.InfluenceLevel = dr["InfluenceLevel"].ToString();
                            contact.CompetitorAffinity = dr["Competitor"].ToString();
                            contact.ProductInvolved = dr["ProductInvolved"].ToString();


                            Contacts.Add(contact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Contacts;
        }
    }
}
