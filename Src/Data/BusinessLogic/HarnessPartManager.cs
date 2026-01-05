using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.DataAccess;
using Emdep.Geos.Data.Common;
using System.Net;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure;
using System.Xml;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;
using Emdep.Geos.Data.BusinessLogic.Logging;
using System.IO;
using Prism.Logging;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class HarnessPartManager
    {
        #region [GEOS2-5404][rdixit][13.08.2024]
        public HarnessPartManager()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.Web.Hosting.HostingEnvironment.MapPath("/") + "log4net.config";
                    CreateIfNotExists(ApplicationLogFilePath);
                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
                    Log4NetLogger.Logger.Log(string.Format("HarnessPartManager()..... Constructor Executed"), category: Category.Info, priority: Priority.Low);
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

        /// <summary>
        /// This method is to get all companies
        /// </summary>
        /// <returns>List of company</returns>
        public List<Company> GetAllCompanies()
        {
            List<Company> Companies = null;
            using (var db = new WorkbenchContext())
            {
                Companies = (from records in db.Companies select records).ToList();
            }
            return Companies;
        }


        /// <summary>
        /// This method is to get all Harness Part Accessory Type
        /// </summary>
        /// <returns>List of all Harness Part Accessory Type</returns>
        public List<HarnessPartAccessoryType> GetAllHarnessPartAccessoryType()
        {
            List<HarnessPartAccessoryType> HarnessPartAccessoryTypes = null;
            using (var db = new HarnessPartContext())
            {
                HarnessPartAccessoryTypes = (from records in db.HarnessPartAccessoryTypes select records).ToList();
            }
            return HarnessPartAccessoryTypes;
        }

        /// <summary>
        /// This method is to get all enterprise group
        /// </summary>
        /// <returns>List of all enterprise group</returns>
        public List<EnterpriseGroup> GetAllEnterpriseGroup()
        {
            List<EnterpriseGroup> EnterpriseGroups = null;
            using (var db = new WorkbenchContext())
            {
                EnterpriseGroups = (from records in db.EnterpriseGroups select records).ToList();
            }
            return EnterpriseGroups;
        }

        /// <summary>
        /// This method is to get all harness part type
        /// </summary>
        /// <returns>List of all harness part type</returns>
        public List<HarnessPartType> GetAllHarnessPartType()
        {
            List<HarnessPartType> HarnessPartTypes = null;
            using (var db = new HarnessPartContext())
            {
                HarnessPartTypes = (from records in db.HarnessPartTypes select records).ToList();
            }
            return HarnessPartTypes;
        }

        /// <summary>
        /// This method is to get all harness part search result
        /// </summary>
        /// <returns>List of all harness part search result</returns>
        public List<HarnessPart> GetAllHarnessPartSearchResult(HarnessPartSearch harnessPartSearch)
        {
            List<HarnessPart> HarnessParts = null;
            using (var db = new HarnessPartContext())
            {
              //  HarnessParts = db.HarnessParts.SqlQuery("select * from harness_parts").ToList();
               // db.ex
               HarnessParts = (from records in db.HarnessParts.SqlQuery(BulidDynamicQueryText(harnessPartSearch)) select records).ToList(); 
            }
            return HarnessParts;
        }

        public List<HarnessPart> GetEnumTest()
        {
            List<HarnessPart> harnessPart = null;
            using (var db = new HarnessPartContext())
            {
                harnessPart = (from records in db.HarnessParts select records).ToList();
            }
            return harnessPart;
        }
        /// <summary>
        /// This method is to get all harness part search result for possible search
        /// </summary>
        /// <returns>List of all harness part search result</returns>
        public List<HarnessPart> GetAllHarnessPartSearchResultPossibleSearch(HarnessPartSearch harnessPartSearch)
        {
            List<HarnessPart> HarnessParts = null;
            using (var db = new HarnessPartContext())
            {
                try
                {
                  // var querytest=(from records in db.HarnessParts.SqlQuery("select * from harness_parts") select new HarnessPart(records));
                  var QueryTest = db.HarnessParts.SqlQuery("select * from harness_parts").ToList();
                   HarnessParts =  (from records in db.HarnessParts.SqlQuery(BulidDynamicPossibleQueryText(harnessPartSearch)) select records).ToList<HarnessPart>();
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
              
            }
            return HarnessParts;
        }

        /// <summary>
        /// This method is to create dynamic possible query
        /// </summary>
        /// <param name="harnessPartSearch">Get harnessPartSearch class to create dynamic possible query</param>
        /// <returns>Dynamic created possible SQL query</returns>
        private string BulidDynamicPossibleQueryText(HarnessPartSearch harnessPartSearch)
        {
            string idAccessoryType = "";
            string iditemColor = "";
            string reference = "";
            List<string> queryCondition = new List<string>();

            queryCondition.Add("select harness_parts.IdHarnessPart as IdHarnessPart,harness_parts.Reference as Reference,harness_parts.IdHarnessPartType as IdHarnessPartType,harness_parts.IdColor as IdColor,harness_parts.Cavities as Cavities,harness_parts.Gender as Gender,harness_parts.IsSealed as IsSealed,harness_parts.InternalDiameter as InternalDiameter,harness_parts.ExternalDiameter as ExternalDiameter,harness_parts.Thickness as Thickness,harness_parts.Description as Description,harness_parts.IdPartner as IdPartner,harness_parts.IdCreator as IdCreator,harness_parts.CreationDate as CreationDate,harness_parts.IdReplacement as IdReplacement,harness_parts.IdSite as IdSite,harness_parts.IsDeleted  as IsDeleted from harness_parts inner join harness_part_accessories on harness_parts.IdHarnessPart = harness_part_accessories.idHarnessPart where IsDeleted = 0");
            foreach (HarnessPartAccessory item in harnessPartSearch.HarnessPartAccessories)
            {
                idAccessoryType = "'" + item.IdAccessoryType + "',";
                iditemColor = "'" + item.IdColor + "',";
                reference = "'" + item.Reference + "',";
            }
            if (idAccessoryType != "")
            {
                idAccessoryType = idAccessoryType.Remove(idAccessoryType.Count() - 1, 1);
            }
            if (iditemColor != "")
            {
                iditemColor = iditemColor.Remove(iditemColor.Count() - 1, 1);
            }
            if (reference != "")
            {
                reference = reference.Remove(reference.Count() - 1, 1);
            }

            if (harnessPartSearch.Reference != null)
            {
                queryCondition.Add("(harness_parts.Reference RLike '%" + BuildRegexp(harnessPartSearch.Reference) + "%')");
            }
            if (harnessPartSearch.CavitiesFrom.HasValue || harnessPartSearch.CavitiesTo.HasValue)
            {
                queryCondition.Add("(harness_parts.Cavities BETWEEN " + harnessPartSearch.CavitiesFrom + " AND " + harnessPartSearch.CavitiesTo + ")");
            }
            if (harnessPartSearch.InternalDiameterFrom.HasValue || harnessPartSearch.InternalDiameterTo.HasValue)
            {
                queryCondition.Add("(harness_parts.InternalDiameter BETWEEN " + harnessPartSearch.InternalDiameterFrom + " AND " + harnessPartSearch.InternalDiameterTo + ")");
            }
            if (harnessPartSearch.ExternalDiameterFrom.HasValue || harnessPartSearch.ExternalDiameterTo.HasValue)
            {
                queryCondition.Add("(harness_parts.ExternalDiameter BETWEEN " + harnessPartSearch.ExternalDiameterFrom + " AND " + harnessPartSearch.ExternalDiameterTo + ")");
            }
            if (harnessPartSearch.ThicknessFrom.HasValue || harnessPartSearch.ThicknessTo.HasValue)
            {
                queryCondition.Add("(harness_parts.Thickness BETWEEN " + harnessPartSearch.ExternalDiameterFrom + " AND " + harnessPartSearch.ExternalDiameterTo + ")");
            }
            if (harnessPartSearch.HarnessPartAccessories.Count > 0)
            {
                if (harnessPartSearch.IsCondition == 1)
                    queryCondition.Add("((harness_part_accessories.idAccessoryType IN (" + idAccessoryType + ")) AND  (harness_part_accessories.idColor IN (" + iditemColor + ")) AND (harness_part_accessories.reference IN (" + reference + ")))");
                else
                    queryCondition.Add("((harness_part_accessories.idAccessoryType IN (" + idAccessoryType + ")) OR  (harness_part_accessories.idColor IN (" + iditemColor + ")) OR (harness_part_accessories.reference IN (" + reference + ")))");
            }
            if (harnessPartSearch.IsSealed == 1)
                queryCondition.Add("(IsSealed = 1)");
            else
                queryCondition.Add("(IsSealed = 0)");

            if (harnessPartSearch.Gender.HasValue)
            {
                if (harnessPartSearch.Gender == Genders.Female)
                    queryCondition.Add("(Gender = '" + Genders.Female + "')");
                else if (harnessPartSearch.Gender == Genders.Male)
                    queryCondition.Add("(Gender = '" + Genders.Male + "')");
                else
                    queryCondition.Add("(Gender = 'null')");
            }

            string query = string.Join(" AND ", queryCondition);
            if (harnessPartSearch.SortName != "")
            {
                query += " order by harness_parts." + harnessPartSearch.SortName + "";
            }
            return query;
        }

        /// <summary>
        /// This method is to create dynamic query
        /// </summary>
        /// <param name="harnessPartSearch">Get harnessPartSearch class to create dynamic query</param>
        /// <returns>Dynamic created SQL query</returns>
        private string BulidDynamicQueryText(HarnessPartSearch harnessPartSearch)
        {
            string idAccessoryType = "";
            string iditemColor = "";
            string reference = "";
            List<string> queryCondition = new List<string>();
            queryCondition.Add("select harness_parts.IdHarnessPart as IdHarnessPart,harness_parts.Reference as Reference,harness_parts.IdHarnessPartType as IdHarnessPartType,harness_parts.IdColor as IdColor,harness_parts.Cavities as Cavities,harness_parts.IsSealed as IsSealed,harness_parts.InternalDiameter as InternalDiameter,harness_parts.ExternalDiameter as ExternalDiameter,harness_parts.Thickness as Thickness,harness_parts.Description as Description,harness_parts.IdPartner as IdPartner,harness_parts.IdCreator as IdCreator,harness_parts.CreationDate as CreationDate,harness_parts.IdReplacement as IdReplacement,harness_parts.IdSite as IdSite,harness_parts.IsDeleted  as IsDeleted from harness_parts inner join harness_part_accessories on harness_parts.IdHarnessPart = harness_part_accessories.idHarnessPart where IsDeleted = 0");
           // queryCondition.Add("select harness_parts.IdHarnessPart as IdHarnessPart,harness_parts.Reference as Reference,harness_parts.IdHarnessPartType as IdHarnessPartType,harness_parts.IdColor as IdColor,harness_parts.Cavities as Cavities,harness_parts.Gender as Gender,harness_parts.IsSealed as IsSealed,harness_parts.InternalDiameter as InternalDiameter,harness_parts.ExternalDiameter as ExternalDiameter,harness_parts.Thickness as Thickness,harness_parts.Description as Description,harness_parts.IdPartner as IdPartner,harness_parts.IdCreator as IdCreator,harness_parts.CreationDate as CreationDate,harness_parts.IdReplacement as IdReplacement,harness_parts.IdSite as IdSite,harness_parts.IsDeleted  as IsDeleted from harness_parts inner join harness_part_accessories on harness_parts.IdHarnessPart = harness_part_accessories.idHarnessPart where IsDeleted = 0");
            foreach (HarnessPartAccessory item in harnessPartSearch.HarnessPartAccessories)
            {
                idAccessoryType = "'" + item.IdAccessoryType + "',";
                iditemColor = "'" + item.IdColor + "',";
                reference = "'" + item.Reference + "',";
            }
            if (idAccessoryType != "")
            {
                idAccessoryType = idAccessoryType.Remove(idAccessoryType.Count() - 1, 1);
            }
            if (iditemColor != "")
            {
                iditemColor = iditemColor.Remove(iditemColor.Count() - 1, 1);
            }
            if (reference != "")
            {
                reference = reference.Remove(reference.Count() - 1, 1);
            }

            if (harnessPartSearch.Reference != null)
            {
                queryCondition.Add("(harness_parts.Reference like '" + harnessPartSearch.Reference + "')");
            }
            if (harnessPartSearch.CavitiesFrom.HasValue || harnessPartSearch.CavitiesTo.HasValue)
            {
                queryCondition.Add("(harness_parts.Cavities BETWEEN " + harnessPartSearch.CavitiesFrom + " AND " + harnessPartSearch.CavitiesTo + ")");
            }
            if (harnessPartSearch.InternalDiameterFrom.HasValue || harnessPartSearch.InternalDiameterTo.HasValue)
            {
                queryCondition.Add("(harness_parts.InternalDiameter BETWEEN " + harnessPartSearch.InternalDiameterFrom + " AND " + harnessPartSearch.InternalDiameterTo + ")");
            }
            if (harnessPartSearch.ExternalDiameterFrom.HasValue || harnessPartSearch.ExternalDiameterTo.HasValue)
            {
                queryCondition.Add("(harness_parts.ExternalDiameter BETWEEN " + harnessPartSearch.ExternalDiameterFrom + " AND " + harnessPartSearch.ExternalDiameterTo + ")");
            }
            if (harnessPartSearch.ThicknessFrom.HasValue || harnessPartSearch.ThicknessTo.HasValue)
            {
                queryCondition.Add("(harness_parts.Thickness BETWEEN " + harnessPartSearch.ExternalDiameterFrom + " AND " + harnessPartSearch.ExternalDiameterTo + ")");
            }
            if (harnessPartSearch.HarnessPartAccessories.Count > 0)
            {
                if (harnessPartSearch.IsCondition == 1)
                    queryCondition.Add("((harness_part_accessories.idAccessoryType IN (" + idAccessoryType + ")) AND (harness_part_accessories.idColor IN (" + iditemColor + ")) AND (harness_part_accessories.reference IN (" + reference + ")))");
                else
                    queryCondition.Add("((harness_part_accessories.idAccessoryType IN (" + idAccessoryType + ")) OR  (harness_part_accessories.idColor IN (" + iditemColor + ")) OR (harness_part_accessories.reference IN (" + reference + ")))");
            }
            if (harnessPartSearch.IsSealed == 1)
                queryCondition.Add("(IsSealed = 1)");
            else
                queryCondition.Add("(IsSealed = 0)");

            if (harnessPartSearch.Gender.HasValue)
            {
                if (harnessPartSearch.Gender == Genders.Female)
                    queryCondition.Add("(Gender = '" + Genders.Female + "')");
                else if (harnessPartSearch.Gender == Genders.Male)
                    queryCondition.Add("(Gender = '" + Genders.Male + "')");
                else
                    queryCondition.Add("(Gender = 'null')");
            }

            string query = string.Join(" AND ", queryCondition);
            if (harnessPartSearch.SortName != "")
            {
                query += " order by harness_parts." + harnessPartSearch.SortName + "";
            }
            return query;
        }


        private string BuildRegexp(string reference)
        {
            reference = reference.Replace(' ', '%');
            reference = reference.Replace('-', '%');
            reference = reference.Replace('_', '%');
            reference = reference.Replace('.', '%');
            reference = reference.Replace(',', '%');
            reference = reference.Replace('=', '%');
            reference = reference.Replace('/', '%');
            reference = reference.Replace('\'', '%');

            // Removing any non-alphanumerical characters from the input.
            //emdepCode = Regex.Replace(emdepCode, @"[^\w%]", "");

            char[] tmp = reference.ToCharArray();

            string regexpString = "";

            if (!reference.Equals(string.Empty))
            {
                // Building the regular expression.
                foreach (char c in tmp)
                {
                    if (c.ToString().Equals("%"))
                    {
                        regexpString += "[^[:alnum:]]*.*";
                    }
                    else if (c.ToString().Equals("_") || (c.ToString().Equals("+")))
                    {
                        regexpString += "[^[:alnum:]]*.";
                    }
                    else if (c.ToString().Equals("0") || c.ToString().Equals("O"))
                    {
                        regexpString += "[^[:alnum:]]*(O|0)";
                        regexpString += "[^[:alnum:]]*.*";
                    }
                    else if (c.ToString().Equals("I") || c.ToString().Equals("l") || c.ToString().Equals("1"))
                    {
                        regexpString += "[^[:alnum:]]*(I|l|1)";
                        regexpString += "[^[:alnum:]]*.*";
                    }
                    else if (c.ToString().Equals("B") || c.ToString().Equals("8"))
                    {
                        regexpString += "[^[:alnum:]]*(B|8)";
                        regexpString += "[^[:alnum:]]*.*";
                    }
                    else if (c.ToString().Equals("S") || c.ToString().Equals("5"))
                    {
                        regexpString += "[^[:alnum:]]*(S|5)";
                        regexpString += "[^[:alnum:]]*.*";
                    }
                    else if (c.ToString().Equals("Z") || c.ToString().Equals("2"))
                    {
                        regexpString += "[^[:alnum:]]*(Z|2)";
                        regexpString += "[^[:alnum:]]*.*";
                    }
                    else
                    {
                        regexpString += "[^[:alnum:]]*" + c.ToString();
                        regexpString += "[^[:alnum:]]*.*";
                    }
                }

                regexpString += "[^[:alnum:]]*";


                string prueba = ""; ;

                for (int i = 0; i < reference.Length; i++)
                {
                    prueba = "|";
                    prueba += reference.Remove(i, 1);
                    prueba = prueba.Insert(i + 1, ".");
                    regexpString += prueba;
                }

                return regexpString;
            }
            else
            {
                return ".";
            }
        }
    }
}
