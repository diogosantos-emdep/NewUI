using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.BusinessLogic.Logging;
using System.IO;
using Prism.Logging;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class CrmRestManager
    {
        #region [GEOS2-5404][rdixit][13.08.2024]
        public CrmRestManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("CrmRestManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch
            {
                throw;
            }
        }
        void CreateIfNotExists(string config_path)
        {
            string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <configuration>
                                          <configSections>
                                            <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                          </configSections>
                                          <log4net debug=""true"">
                                            <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                              <file value=""C:\Temp\LogsService.txt""/>
                                              <appendToFile value=""true"" />
                                              <rollingStyle value=""Size"" />
                                              <maxSizeRollBackups value=""10"" />
                                              <maximumFileSize value=""10MB"" />
                                              <staticLogFileName value=""true"" />
                                              <layout type=""log4net.Layout.PatternLayout"">
                                                <conversionPattern value=""%-5p %d %5rms - %m%n"" />
                                              </layout>
                                            </appender>
                                            <root>
                                              <level value=""Info"" />
                                              <appender-ref ref=""RollingLogFileAppender"" />
                                            </root>
                                          </log4net>
                                        </configuration>";

            if (!File.Exists(config_path))
            {
                File.WriteAllText(config_path, log4netConfig);
            }
        }
        #endregion
        public List<PeopleDetails> GetPeoples(string connectionstring)
        {
            List<PeopleDetails> peoples = new List<PeopleDetails>();
            
            using (MySqlConnection conofferwithoutpurchaseorder = new MySqlConnection(connectionstring))
            {
                conofferwithoutpurchaseorder.Open();
                MySqlCommand conofferwithoutpurchaseordercommand = new MySqlCommand("people_GetAllPeoples", conofferwithoutpurchaseorder);
                conofferwithoutpurchaseordercommand.CommandType = CommandType.StoredProcedure;

                using (MySqlDataReader reader = conofferwithoutpurchaseordercommand.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        PeopleDetails people = new PeopleDetails();
                        people.IdPerson = Convert.ToInt32(reader["IdPerson"].ToString());
                        people.Name = reader["Name"].ToString();
                        people.Surname = reader["Surname"].ToString();
                        people.Email = reader["Email"].ToString();

                        peoples.Add(people);
                    }
                }
            }

            return peoples;
        }
    }
}
