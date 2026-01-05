using Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
   public class QualityManager
    {
        string _ConnString;
        //static Dictionary<string, string> DictonarySites = new Dictionary<string, string>();
        //static Dictionary<string, int> Dictonarycurrency = new Dictionary<string, int>();
        public QualityManager(string ConnString)
        {
            this._ConnString = ConnString;
        }
        #region Methods
        public List<ServiceRequest> GetServiceRequestData()
        {
            List<ServiceRequest> serviceRequestList = new List<ServiceRequest>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("servicerequest_GetServiceRequestDetails", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600;

                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ServiceRequest serviceRequest = new ServiceRequest();
                            serviceRequest.Id = dr["Id"].ToString();
                            serviceRequest.Code = dr["Code"].ToString();
                            serviceRequest.Title = dr["Title"].ToString();
                            serviceRequest.Type = dr["Type"].ToString();
                            serviceRequest.Group = dr["Group"].ToString();
                            serviceRequest.Plant = dr["Plant"].ToString();
                            serviceRequest.Country = dr["Country"].ToString();
                            serviceRequest.Priority = dr["Priority"].ToString();
                            serviceRequest.CreatedBy = dr["CreatedBy"].ToString();
                            if (dr["CreationDate"] != DBNull.Value)
                                serviceRequest.CreationDate = Convert.ToDateTime(dr["CreationDate"]).ToString("yyyy-MM-dd");
                            if (dr["EndDate"] != DBNull.Value)
                                serviceRequest.EndDate = Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd");
                            if (dr["ExpectedEndDate"] != DBNull.Value)
                                serviceRequest.ExpectedEndDate = Convert.ToDateTime(dr["ExpectedEndDate"]).ToString("yyyy-MM-dd"); 
                            serviceRequest.Solution = dr["Solution"].ToString();
                            serviceRequest.Owner = dr["Owner"].ToString();
                            serviceRequest.Status = dr["Status"].ToString();
                            serviceRequest.ProductCategory = dr["ProductCategory"].ToString();
                            serviceRequest.ProductName = dr["ProductName"].ToString();
                            if (dr["LatestPostDate"] != DBNull.Value)
                                serviceRequest.LatestPostDate = Convert.ToDateTime(dr["LatestPostDate"]).ToString("yyyy-MM-dd"); 
                            serviceRequest.ProgressInPercentage = dr["ProgressInPercentage"].ToString();
                            serviceRequestList.Add(serviceRequest);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serviceRequestList;
      
    }
               
        #endregion
    }
}
