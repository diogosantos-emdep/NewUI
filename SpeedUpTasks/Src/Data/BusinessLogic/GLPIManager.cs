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
using Emdep.Geos.Utility;
using System.Web;
using System.IO;
using System.IO.Compression;
using Emdep.Geos.Data.Common.Glpi;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class GlpiManager
    {
        /// <summary>
        /// This method is to add GLPI ticket
        /// </summary>
        /// <param name="glpiticket">Get GLPI ticket details</param>
        public void AddGLPITicket(GlpiTicket glpiTicket, string GLPIZipFilePath)
        {
            string[] files = null;
            if (glpiTicket.GlpiDocuments.Count > 0)
            {
                //Extract Zip folder in _tmp by its zip file name
                ZipFile.ExtractToDirectory(GLPIZipFilePath + @"_tmp\" + glpiTicket.GUIDString.ToString() + ".Zip", GLPIZipFilePath + @"_tmp\" + glpiTicket.GUIDString.ToString());
                //Delete Zip Folder
                File.Delete(GLPIZipFilePath + @"_tmp\" + glpiTicket.GUIDString.ToString() + ".Zip");
                //Get all files in Extract folder in _tmp
                files = Directory.GetFiles(GLPIZipFilePath + @"_tmp\" + glpiTicket.GUIDString.ToString());

                foreach (GlpiDocument glpidocumentfile in glpiTicket.GlpiDocuments)
                {
                    //Get File from Extract folder
                    foreach (string file in files)
                    {
                        //Check GLPIDocument file name with Extract folder file name
                        if (glpidocumentfile.FileName == Path.GetFileName(file))
                        {
                            //Get source file(extract file path in _tmp) path
                            string GetServerFilename = GLPIZipFilePath + @"_tmp\" + glpiTicket.GUIDString.ToString() + @"\" + Path.GetFileName(file);
                            //Get 40 char SHA1HashString
                            string SHA1HashString = SHA1Util.GetSha1Hash(GetServerFilename);

                            //Check if exist or not
                            if (!Directory.Exists(Path.Combine(GLPIZipFilePath, Path.GetExtension(glpidocumentfile.FilePath.ToString()).Remove(0, 1).ToUpper())))
                            {
                                //Create Directory for file extension
                                Directory.CreateDirectory(Path.Combine(GLPIZipFilePath, Path.GetExtension(glpidocumentfile.FilePath.ToString()).Remove(0, 1).ToUpper()));
                            }
                            //Create sub-directory from SHA1HashString - 2char
                            Directory.CreateDirectory(Path.Combine(GLPIZipFilePath, Path.GetExtension(glpidocumentfile.FilePath.ToString()).Remove(0, 1).ToUpper(), SHA1HashString.Substring(0, 2)));

                            //Copy the file from source to destination(filename from SHA1HashString - 38char)
                            System.IO.File.Copy(file, Path.Combine(GLPIZipFilePath, Path.GetExtension(glpidocumentfile.FilePath.ToString()).Remove(0, 1).ToUpper(), SHA1HashString.Substring(0, 2), SHA1HashString.Substring(2, 38) + Path.GetExtension(Path.GetFileName(file).ToString())), true);

                            //Get destination file path 
                            string ExtractFilePath = Path.Combine(GLPIZipFilePath, Path.GetExtension(glpidocumentfile.FilePath.ToString()).Remove(0, 1).ToUpper(), SHA1HashString.Substring(0, 2), SHA1HashString.Substring(2, 38) + Path.GetExtension(Path.GetFileName(file).ToString()));

                            //Replace "/" attachment file path to store in database documentItem
                            glpidocumentfile.FilePath = ExtractFilePath.Replace(@"C:\xampp\htdocs\glpi\files\", "").Replace(@"\", "/");
                            glpidocumentfile.Sha1sum = SHA1HashString;

                            //Delete attachment file from extract folder in_tmp
                            File.Delete(file);

                            break;
                        }
                    }
                }
                //Delete attachment Extract folder from _tmp
                Directory.Delete(GLPIZipFilePath + @"_tmp\" + glpiTicket.GUIDString.ToString());
            }
            using (var dbTicket = new GLPIContext())
            {
                //Add entry of ItemTypeLink = "0",LinkedAction = 20 in GLPILog
                GlpiLog glpiLog = new GlpiLog();
                glpiLog.ItemType = "Ticket";
                glpiLog.ItemsId = glpiTicket.Id;
                glpiLog.ItemTypeLink = "0";
                glpiLog.LinkedAction = 20;
                glpiLog.UserName = glpiTicket.GlpiUserName.ToString();
                glpiLog.DateMod = DateTime.Now;
                glpiLog.IdSearchOption = 0;
                glpiTicket.GlpiLogs.Add(glpiLog);

                //Add entry of ItemTypeLink = "User",LinkedAction = 15 in GLPILog
                glpiLog = new GlpiLog();
                glpiLog.ItemType = "Ticket";
                glpiLog.ItemsId = glpiTicket.Id;
                glpiLog.ItemTypeLink = "User";
                glpiLog.LinkedAction = 15;
                glpiLog.UserName = glpiTicket.GlpiUserName.ToString();
                glpiLog.DateMod = DateTime.Now;
                glpiLog.IdSearchOption = 0;
                glpiLog.NewValue = glpiTicket.GlpiUserName.ToString();
                glpiTicket.GlpiLogs.Add(glpiLog);

                foreach (GlpiDocument glpidocumentfile in glpiTicket.GlpiDocuments)
                {
                    //Add entry of GLPIDocumentItem in GLPIDocumentItem
                    GlpiDocumentItem glpiDocumentItem = new GlpiDocumentItem();
                    glpiDocumentItem.DateMod = DateTime.Now;
                    glpiDocumentItem.EntitiesId = 0;
                    glpiDocumentItem.ItemsId = glpiTicket.Id;
                    glpiDocumentItem.DocumentsId = glpidocumentfile.Id;
                    glpidocumentfile.GlpiDocumentItems.Add(glpiDocumentItem);

                    //Add entry of GLPIDocuments in GLPILog
                    glpiLog = new GlpiLog();
                    glpiLog.ItemType = "Ticket";
                    glpiLog.ItemsId = glpiTicket.Id;
                    glpiLog.ItemTypeLink = "Document";
                    glpiLog.LinkedAction = 15;
                    glpiLog.UserName = glpiTicket.GlpiUserName.ToString();
                    glpiLog.DateMod = DateTime.Now;
                    glpiLog.IdSearchOption = 0;
                    glpiLog.NewValue = "Document Ticket " + glpiTicket.Id + " (" + glpidocumentfile.Id + ")";
                    glpiTicket.GlpiLogs.Add(glpiLog);

                }

                //Add Ticket in GLPITicket
                glpiTicket = dbTicket.GLPITickets.Add(glpiTicket);
                dbTicket.SaveChanges();

                //Update GLPIDocument with TicketID
                foreach (GlpiDocument glpidocumentfile in glpiTicket.GlpiDocuments)
                {
                    glpidocumentfile.DateMod = DateTime.Now;
                    glpidocumentfile.Tag = GUIDCode.GUIDCodeString();
                    glpidocumentfile.FileName = glpidocumentfile.FileName;
                    glpidocumentfile.Name = "Document Ticket " + glpiTicket.Id;
                    glpidocumentfile.Mime = MimeMapping.GetMimeMapping(glpidocumentfile.FilePath.ToString());
                }

                //Update GLPILogs with TicketID and GLPIDocumentItem ID
                foreach (GlpiDocument glpidocumentfile in glpiTicket.GlpiDocuments)
                {
                    foreach (GlpiLog glpilogTicket in glpiTicket.GlpiLogs.Where(a => a.NewValue == "Document Ticket 0 (0)"))
                    {
                        glpilogTicket.NewValue = "Document Ticket " + glpiTicket.Id + " (" + glpidocumentfile.Id + ")";
                        break;
                    }
                }

                dbTicket.SaveChanges();
            }

        }

        /// <summary>
        /// This method is to get GLPIUser detail by name
        /// </summary>
        /// <param name="name">Get geos user name</param>
        /// <returns>Details of GLPIUser related to name from class GLPIUser</returns>
        public GlpiUser GetGLPIUserByName(string name)
        {
            GlpiUser glpiUser = new GlpiUser();

            //using (var db = new GLPIContext())
            //{
            //   glpiUser = (from records in db.GLPIUsers where records.Name == name select records).SingleOrDefault();
            //}

            glpiUser.Name = name;

            return glpiUser;
        }

        /// <summary>
        /// This method is to get list of GLPI Document types
        /// </summary>
        /// <returns>List of GLPI Document types</returns>
        public List<GlpiDocumentType> GetGLPIDocumentType()
        {
            List<GlpiDocumentType> glpiDocumentTypes = null;
            using (var db = new GLPIContext())
            {
                glpiDocumentTypes = (from records in db.GLPIDocumentTypes select records).ToList();
            }
            return glpiDocumentTypes;
        }

        /// <summary>
        /// This method is to get GLPILocation detail by geos siteid
        /// </summary>
        /// <param name="siteId">Get geos siteId</param>
        /// <returns>Details of GLPILocation related to Id from class GLPILocation</returns>
        public GlpiLocation GetGLPILocationBySiteId(Int32 siteId)
        {
            GlpiLocation glpiLocation = null;
            Int32? glpiSiteId;
            using (var dbgeos = new WorkbenchContext())
            {
                glpiSiteId = (from geosrecords in dbgeos.GeosGlpiSites where geosrecords.GeosSiteId == siteId select geosrecords.GlpiSiteId).SingleOrDefault();
            }

            using (var dbglpi = new GLPIContext())
            {
                glpiLocation = (from records in dbglpi.GLPILocations where records.Id == glpiSiteId select records).SingleOrDefault();
            }

            return glpiLocation;
        }

        /// <summary>
        /// This method is to get GLPIItilCategory detail by geos moduleid
        /// </summary>
        /// <param name="moduleId">Get geos module id</param>
        /// <returns>Details of GLPIItilCategory related to Id from class GLPIItilCategory</returns>
        public GlpiItilCategory GetGLPIItilCategoryByModuleId(Int32 moduleId)
        {
            GlpiItilCategory glpiItilCategory = null;
            Int32? glpiItliCategoryId;
            using (var dbgeos = new WorkbenchContext())
            {
                glpiItliCategoryId = (from geosrecords in dbgeos.GeosGlpiItilCategories where geosrecords.IdModule == moduleId select geosrecords.IdItilCategory).SingleOrDefault();
            }

            using (var dbglpi = new GLPIContext())
            {
                glpiItilCategory = (from records in dbglpi.GLPIItilCategories where records.Id == glpiItliCategoryId select records).SingleOrDefault();
            }

            return glpiItilCategory;
        }
    }
}

