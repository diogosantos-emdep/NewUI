using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Entities;
namespace BusinessLogic
{
    public class ActivityManager
    {
        string _ConnString;
        public ActivityManager(string ConnString)
        {
            this._ConnString = ConnString;
        }
        public List<Activity> GetActivities(DateTime FromDate, DateTime ToDate, string Plants)
        {
            List<Activity> Activities = new List<Activity>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_ConnString))
                {
                    conn.Open();
                    MySqlCommand command = new MySqlCommand("GeosApiGetActivities", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600;
                    command.Parameters.AddWithValue("_FromDate", FromDate);
                    command.Parameters.AddWithValue("_ToDate", ToDate);
                    command.Parameters.AddWithValue("_Plants", Plants);
                    using (MySqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Activity activity = new Activity();
                            if (dr["IdActivity"] != DBNull.Value)
                                activity.IdActivity = Convert.ToInt64(dr["IdActivity"].ToString());

                            if (dr["IsInternal"] != DBNull.Value)
                                activity.IsInternal = Convert.ToByte(dr["IsInternal"].ToString());

                            if (dr["ActivityType"] != DBNull.Value)
                                activity.ActivityType = dr["ActivityType"].ToString();

                            activity.Subject = dr["Subject"].ToString();
                            //activity.ActivityTagsString = dr["ActivityTagsString"].ToString();
                            activity.Description = dr["Description"].ToString();
                            // activity.ActivityAttendeesString = dr["ActivityAttendeesString"].ToString();
                            activity.Location = dr["Location"].ToString();

                            if (dr["ToDate"] != DBNull.Value)
                                activity.DueDate = Convert.ToDateTime(dr["ToDate"]).ToString("yyyy-MM-dd");


                            if (dr["ActivityStatus"] != DBNull.Value)
                                activity.Status = dr["ActivityStatus"].ToString();

                            if (dr["CloseDate"] != DBNull.Value)
                                activity.CloseDate = Convert.ToDateTime(dr["CloseDate"]).ToString("yyyy-MM-dd");

                            activity.SalesOwner = dr["Name"].ToString() + " " + dr["Surname"].ToString();
                            //activity.LinkedAccountGroup = dr["Group"].ToString();
                            // activity.ActivityAttendeesString = "Test";
                            // activity.ActivityLinkedItems = GetActivityLinkedItems(activity.IdActivity);
                            Activities.Add(activity);
                        }


                        if (dr.NextResult())
                        {

                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if (activity != null)
                                    {
                                        activity.LinkedAccountGroup = dr["Group"].ToString();
                                        activity.LinkedAccountPlant = dr["SiteNameWithoutCountry"].ToString();
                                        activity.LinkedAccountCountry = dr["Country"].ToString();
                                    }
                                }
                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if (activity != null)
                                        activity.ActivityAttendeesString = dr["ActivityAttendeesString"].ToString();
                                }
                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if (activity != null)
                                        activity.ActivityTagsString = dr["ActivityTagsString"].ToString();
                                }
                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if (activity != null)
                                        activity.CarprojectString = dr["CarprojectString"].ToString();
                                }
                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if (activity != null)
                                        activity.ContactString = dr["ContactString"].ToString();
                                }
                            }
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if (activity != null)
                                        activity.OfferCodeString = dr["OfferCodeString"].ToString();
                                }
                            }
                        }

                        if (dr.NextResult())
                        {

                            while (dr.Read())
                            {
                                if (dr["IdActivity"] != DBNull.Value)
                                {
                                    Activity activity = Activities.Find(i => i.IdActivity == Convert.ToInt64(dr["IdActivity"].ToString()));
                                    if(activity!=null)
                                    activity.CompetitorString = dr["CompetitorString"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Activities;
        }
     
    }
}
