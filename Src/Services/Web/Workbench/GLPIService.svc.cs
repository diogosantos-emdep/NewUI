using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Glpi;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Emdep.Geos.Services.Web.Workbench
{
    /// <summary>
    /// GLPIService class use for getting information of GLPI
    /// </summary>
    public class GlpiService : IGlpiService
    {
        /// <summary>
        /// This method is to add GLPI ticket
        /// </summary>
        /// <param name="glpiTicket">Get GLPI ticket details</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         FileUploader userProfileFileUploader1 = new FileUploader();
        ///    userProfileFileUploader1.FileUploadName = GUIDCode.GUIDCodeString();
        ///    if (AttachmentList != null)
        ///    {
        ///        foreach (FileInfo fs in AttachmentList)
        ///        {
        ///            FileDetail.Add(fs);
        ///        }
        ///
        ///        userProfileFileUploader1.FileByte = ConvertZipToByte(FileDetail, userProfileFileUploader1.FileUploadName);// read byte[]  zip file Byte 
        ///
        ///
        ///        fileUploadReturnMessage = FileControl.UploaderGLPIZipFile(userProfileFileUploader1);
        ///    }
        ///    if (fileUploadReturnMessage.IsFileUpload == true)
        ///    {
        ///        for (int i = 0; i &lt; FileDetail.Count; i++)
        ///        {
        ///            glpi = new GLPIDocument();
        ///            glpi.Comment = "XYZ";
        ///            glpi.DocumentCategoriesId = 1;
        ///            glpi.EntitiesId = 17;
        ///
        ///            glpi.FileName = FileDetail[i].Name;//((System.IO.FileInfo)((AttachmentList)._items[i])).Name.ToString();
        ///            glpi.FilePath = FileDetail[i].FullName;
        ///            glpi.IsBlacklisted = true;
        ///            glpi.IsDeleted = true;
        ///            glpi.IsRecursive = true;
        ///            glpi.Name = SupportWindowTitle;
        ///            glpiDocument.Add(glpi);
        ///        }
        ///    }
        ///    GLPITicket glpiticket = new GLPITicket();
        ///    glpiticket.EntitiesId = 17;
        ///
        ///    glpiticket.Name = SupportWindowTitle;
        ///    glpiticket.CloseDate = null;
        ///    glpiticket.SolveDate = null;
        ///    glpiticket.DateMod = null;
        ///    glpiticket.UsersIdLastupdater = 0;
        ///    glpiticket.Status = 1;
        ///
        ///    if (GLPIUser == null)
        ///    {
        ///        glpiticket.UsersIdRecipient = 2;
        ///    }
        ///    else
        ///    {
        ///        glpiticket.UsersIdRecipient = GLPIUser.Id;
        ///        glpiticket.GLPIUserName = GLPIUser.Name + " " + (GLPIUser.Id);
        ///    }
        ///
        ///    glpiticket.RequestTypesId = 1;
        ///    glpiticket.Content = SupportWindowDescription;
        ///    glpiticket.Urgency = 3;
        ///    glpiticket.Impact = 3;
        ///    glpiticket.Priority = 3;
        ///    glpiticket.ItilcategoriesId = 0;
        ///    glpiticket.Type = 1;
        ///    glpiticket.SolutiontypesId = 0;
        ///    glpiticket.Solution = null;
        ///    glpiticket.GlobalValidation = 1;
        ///    glpiticket.SlasId = 0;
        ///    glpiticket.SlalevelsId = 0;
        ///    glpiticket.DueDate = null;
        ///    glpiticket.BeginWaitingDate = null;
        ///    GLPILocation = GLPIControl.GetGLPILocationBySiteId(Convert.ToInt32(GeosApplication.ActiveUser.IdSite));
        ///    if (GLPILocation == null)
        ///    {
        ///        glpiticket.LocationsId = 0;
        ///    }
        ///    else
        ///    {
        ///        glpiticket.LocationsId = GLPILocation.Id;
        ///    }
        ///    if (AttachmentList != null)
        ///    {
        ///        glpiticket.GlpiDocuments = glpiDocument;
        ///        glpiticket.GUIDString = userProfileFileUploader1.FileUploadName;
        ///    }
        ///    GLPIControl.AddGLPITicket(glpiticket);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void AddGLPITicket(GlpiTicket glpiTicket)
        {



            try
            {
                GlpiManager glpimgr = new GlpiManager();
                glpiTicket.Date = DateTime.Now;
                glpimgr.AddGLPITicket(glpiTicket, Properties.Settings.Default.GLPIZipFile);
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
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
        }

        /// <summary>
        /// This method is to get GLPIUser detail by name
        /// </summary>
        /// <param name="name">Get geos user name</param>
        /// <returns>Details of GLPIUser related to name from class GLPIUser</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGLPIService GLPIControl = new GLPIController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GLPIUser glpiUser = GLPIControl.GetGLPIUserByName(user.Login);
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public GlpiUser GetGLPIUserByName(string name)
        {
            GlpiUser glpiUser = null;
            try
            {
                GlpiManager glpimgr = new GlpiManager();
                glpiUser = glpimgr.GetGLPIUserByName(name);
            }
            catch(EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message.ToString();
                exp.ErrorDetails = ex.ToString();
            
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return glpiUser;
        }

        /// <summary>
        /// This method is to get GLPILocation detail by geos companyid
        /// </summary>
        /// <param name="companyId">Get geos companyId</param>
        /// <returns>Details of GLPILocation related to Id from class GLPILocation</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGLPIService GLPIControl = new GLPIController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GLPILocation glpiLocation = GLPIControl.GetGLPILocationByCompanyId(Company.IdCompany);
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public GlpiLocation GetGLPILocationByCompanyId(Int32 companyId)
        {
            GlpiLocation glpiLocation = null;
            try
            {
                GlpiManager glpimgr = new GlpiManager();
                glpiLocation = glpimgr.GetGLPILocationBySiteId(companyId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return glpiLocation;
        }

        /// <summary>
        /// This method is to get list of GLPI Document types
        /// </summary>
        /// <returns>List of GLPI Document types</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGLPIService GLPIControl = new GLPIController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         List&lt;GLPIDocumentType&gt; glpiDocumentTypes = GLPIControl.GetGLPIDocumentType();
        ///         
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<GlpiDocumentType> GetGLPIDocumentType()
        {
            List<GlpiDocumentType> glpiDocumentTypes = null;
            try
            {
                GlpiManager glpimgr = new GlpiManager();
                glpiDocumentTypes = glpimgr.GetGLPIDocumentType();
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return glpiDocumentTypes;
        }

        /// <summary>
        /// This method is to get GLPIItilCategory detail by geos moduleid
        /// </summary>
        /// <param name="moduleId">Get geos module id</param>
        /// <returns>Details of GLPIItilCategory related to Id from class GLPIItilCategory</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGLPIService GLPIControl = new GLPIController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         GLPIItilCategory glpiItilCategory = GLPIControl.GetGLPIItilCategoryByModuleId(geosModule.IdGeosModule);
        ///     }
        /// }
        /// </code>
        /// </example>
        public GlpiItilCategory GetGLPIItilCategoryByModuleId(Int32 moduleId)
        {
            GlpiItilCategory glpiItilCategory = null;
            try
            {
                GlpiManager glpimgr = new GlpiManager();
                glpiItilCategory = glpimgr.GetGLPIItilCategoryByModuleId(moduleId);
            }
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return glpiItilCategory;
        }

        public void SendGlpiTicketMail(GlpiTicketMail glpiTicketMail)
        {
            try
            {
                ApplicationManager Appmgr = new ApplicationManager();
                CrmManager crmmgr = new CrmManager();
                string workbenchConnectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                GeosAppSetting geosAppSetting = crmmgr.GetGeosAppSettings(9, workbenchConnectionString);

                if (geosAppSetting != null)
                {
                    Appmgr.SendGlpiTicketMail(glpiTicketMail, Properties.Settings.Default.MailServerName, Properties.Settings.Default.MailServerPort, glpiTicketMail.ActiveUserMailId, Properties.Settings.Default.EmailTemplate, geosAppSetting.DefaultValue);
                }
                else
                {
                    ServiceException exp = new ServiceException();
                    exp.ErrorCode = "000010";
                    throw new FaultException<ServiceException>(exp);
                }
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
            catch (EntityException ee)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ee.InnerException.Message.ToString();
                exp.ErrorDetails = ee.ToString();

                throw new FaultException<ServiceException>(exp, ee.ToString());
            }
        }
    }
}
