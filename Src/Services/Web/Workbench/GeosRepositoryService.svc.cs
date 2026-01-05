using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Emdep.Geos.Data.BusinessLogic;
using System.IO;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common.File;
using System.ServiceModel.Activation;
using System.Net;

namespace Emdep.Geos.Services.Web.Workbench
{
    /// <summary>
    /// GeosRepositoryService class use for getting information of File
    /// </summary>
    public class GeosRepositoryService : IGeosRepositoryService
    {

        /// <summary>
        /// This method is to download current version and return it in bytes from class GetWorkbenchDownloadVersion
        /// </summary>
        /// <param name="mgr">To get current version number from class GeosWorkbenchVersion</param>
        /// <returns>FileTransferRequest:-File in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         FileDownloadMessage mgr  = new FileDownloadMessage();
        ///         FileDownloadReturnMessage result = FileControl.GetWorkbenchDownloadVersion(mgr);
        ///     }
        /// }
        /// </code>
        /// </example>
        public FileDownloadReturnMessage GetWorkbenchDownloadVersion(FileDownloadMessage mgr)
        {
            FileStream fs = null;
            FileDownloadReturnMessage result = null;
            try
            {

                string filepath = Properties.Settings.Default.WorkbenchVersionFolder + "V" + ((GeosWorkbenchVersion)mgr.Version).VersionNumber + ".Zip";

                fs = new FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                result = new FileDownloadReturnMessage(new FileMetaData(filepath, filepath, fs.Length), fs);

            }

            catch (IOException e)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = e.Message;
                exp.ErrorDetails = e.ToString();
                throw new FaultException<ServiceException>(exp, e.ToString());
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            finally
            {

            }
            return result;
        }

        /// <summary>
        /// This method is to download current company image and return it in bytes 
        /// </summary>
        /// <param name="idCompany">To get current id company from class company</param>
        /// <returns>File in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes = FileControl.GetCompanyImage(idCompany);
        ///     }
        /// }
        /// </code>
        /// </example>
        public byte[] GetCompanyImage(Int32 idCompany)
        {
            byte[] bytes = null;
            try
            {
                // var name = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory.ToString());

                string filepathjpg = Properties.Settings.Default.SiteImage + idCompany + ".jpg";
                string filepathpng = Properties.Settings.Default.SiteImage + idCompany + ".png";
                //name + @"\Asset\Images\EmdepSites\" + idCompany + ".jpg";
                if (File.Exists(filepathjpg))
                {
                    // open stream
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathjpg, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                else if (File.Exists(filepathpng))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathpng, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        /// <summary>
        /// This method is to upload user profile image from bytes
        /// </summary>
        /// <param name="userProfileFileUploader">Get file name and bytes to upload</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes;
        ///         FileUploader userProfileFileUploader = new FileUploader();
        ///         userProfileFileUploader.FileUploadName = "V0.0.0.3" ;
        ///         userProfileFileUploader.FileByte = bytes;
        ///         FileControl.Uploader(userProfileFileUploader);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void Uploader(FileUploader userProfileFileUploader)
        {

            FileStream fileStream = null;
            string fileUploadPath = Properties.Settings.Default.UserProfileImage + userProfileFileUploader.FileUploadName + ".jpg";
            try
            {
                if (userProfileFileUploader.FileByte.Length > 0 && userProfileFileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(userProfileFileUploader.FileByte, 0, userProfileFileUploader.FileByte.Length);
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to uploader task attachment
        /// </summary>
        /// <param name="userProfileFileUploader">Get file details to upload</param>
        /// <returns>IsFileUpload or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///          FileUploader fileUploader = new FileUploader();
        ///          FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
        ///          fileUploader.FileUploadName = @"2016\2016-PR1308\8068-EPCServiceMethods.txt";
        ///          fileUploader.FileByte = prog.GetCompanyImage(); 
        ///          fileUploadReturnMessage=geosControl.TaskAttachmentUploader(fileUploader);
        ///     }
        /// }
        /// </code>
        /// </example>
        public FileUploadReturnMessage TaskAttachmentUploader(FileUploader userProfileFileUploader)
        {

            FileStream fileStream = null;
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            string fileUploadPath = Properties.Settings.Default.EpcProjectFolder + userProfileFileUploader.FileUploadName;
            try
            {
                if (userProfileFileUploader.FileByte.Length > 0 && userProfileFileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(userProfileFileUploader.FileByte, 0, userProfileFileUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }

                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileUploadReturnMessage;
        }
        /// <summary>
        /// This method is to get Get user profile image as per isvalidate
        /// </summary>
        /// <param name="userName">Get user name </param>
        /// <param name="isValidate">Get isvalidate user profile image</param>
        /// <returns>File in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///       
        ///         byte[] bytes = FileControl.GetUserProfileImage("cpatil"); OR
        ///         byte[] bytes = FileControl.GetUserProfileImage("cpatil",0);
        ///     }
        /// }
        /// </code>
        /// </example>
        public byte[] GetUserProfileImage(string userName, byte isValidate = 1)
        {
            byte[] bytes = null;
            string fileUploadPath = "";

            if (isValidate == 1)
            {

                if (File.Exists(Properties.Settings.Default.UserProfileImage + userName + ".png"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + userName + ".png";
                }
                else
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + userName + ".jpg";
                }
            }
            else
                fileUploadPath = Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + "_temp.jpg";
            try
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

        public byte[] GetEmployeesImage(string employeeCode)
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

        public byte[] GetUserProfileImageWithoutException(string userName, byte isValidate = 1)
        {
            byte[] bytes = null;
            string fileUploadPath = null;

            if (isValidate == 1)
            {
                if (File.Exists(Properties.Settings.Default.UserProfileImage + userName + ".png"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + userName + ".png";
                }
                else if (File.Exists(Properties.Settings.Default.UserProfileImage + userName + ".jpg"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + userName + ".jpg";
                }
            }
            else
            {
                if (File.Exists(Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + "_temp.jpg"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + "_temp.jpg";
                }
            }

            if (!string.IsNullOrEmpty(fileUploadPath))
            {
                try
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
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  This method is to upload zip file from bytes
        /// </summary>
        /// <param name="fileUploader">Get file in  bytes</param>
        /// <returns>Is Upload Zip file or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes;
        ///         FileUploader fileUploader = new FileUploader();
        ///         fileUploader.FileByte = bytes;
        ///         fileUploader.FileUploadName = GUIDCode.GUIDCodeString();
        ///         FileUploadReturnMessage fileUploadReturnMessage = FileControl.UploaderGLPIZipFile(fileUploader); 
        ///     }
        /// }
        /// </code>
        /// </example>
        public FileUploadReturnMessage UploaderGLPIZipFile(FileUploader fileUploader)
        {
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            FileStream fileStream = null;
            //Get the file upload path store in web services web.config file.  
            string fileUploadPath = Properties.Settings.Default.GLPIZipFile + @"_tmp\" + fileUploader.FileUploadName + ".Zip";
            try
            {
                if (fileUploader.FileByte.Length > 0 && fileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(fileUploader.FileByte, 0, fileUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }

                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileUploadReturnMessage;

        }

        /// <summary>
        ///  This method is to upload zip file from bytes
        /// </summary>
        /// <param name="fileUploader">Get file in  bytes</param>
        /// <returns>Is Upload Zip file or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes;
        ///         FileUploader fileUploader = new FileUploader();
        ///         fileUploader.FileByte = bytes;
        ///         fileUploader.FileUploadName = GUIDCode.GUIDCodeString();
        ///         FileUploadReturnMessage fileUploadReturnMessage = FileControl.UploaderProjectScopeFile(fileUploader); 
        ///     }
        /// }
        /// </code>
        /// </example>
        public FileUploadReturnMessage UploaderProjectScopeFile(FileUploader fileUploader)
        {
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            FileStream fileStream = null;
            //Get the file upload path store in web services web.config file.  
            string fileUploadPath = fileUploader.FileUploadName;
            try
            {
                if (fileUploader.FileByte.Length > 0 && fileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(fileUploader.FileByte, 0, fileUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }

                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return fileUploadReturnMessage;

        }


        /// <summary>
        /// This method is to upload user profile invalidate image from bytes
        /// </summary>
        /// <param name="userProfileFileUploader">Get File details to upload user profile invalidate image </param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes;
        ///         FileUploader fileUploader = new FileUploader();
        ///         fileUploader.FileByte = bytes;
        ///         fileUploader.FileUploadName = user.Login;
        ///         FileControl.UploadIsValidateFalseUserImage(fileUploader); 
        ///     }
        /// }
        /// </code>
        /// </example>
        public void UploadIsValidateFalseUserImage(FileUploader userProfileFileUploader)
        {
            FileStream fileStream = null;
            //string fileUploadPath = Properties.Settings.Default.UserProfileImage + "/_tmp/" + userProfileFileUploader.FileUploadName + "_temp.jpg";
            string fileUploadPath = Properties.Settings.Default.UserProfileImage +  userProfileFileUploader.FileUploadName + ".jpg";
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    File.Delete(fileUploadPath);
                }
                if (userProfileFileUploader.FileByte.Length > 0 && userProfileFileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(userProfileFileUploader.FileByte, 0, userProfileFileUploader.FileByte.Length);
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        /// <summary>
        /// This method is to get module image
        /// </summary>
        /// <param name="idModule">Get Module id to get Module image</param>
        /// <returns>Module image file in bytes</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         IGeosRepositoryService  FileControl = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), GeosApplication.Instance.ApplicationSettings["ServiceProviderPort"].ToString());
        ///         byte[] bytes = FileControl.GetModuleImage(geosModule.IdGeosModule); 
        ///     }
        /// }
        /// </code>
        /// </example>

        public byte[] GetModuleImage(Int32 idModule)
        {
            byte[] bytes = null;
            try
            {
                string filepath = "";
                if (File.Exists(Properties.Settings.Default.ModuleImage + idModule + ".jpg"))
                {
                    filepath = Properties.Settings.Default.ModuleImage + idModule + ".jpg";
                }
                else if (File.Exists(Properties.Settings.Default.ModuleImage + idModule + ".png"))
                {
                    filepath = Properties.Settings.Default.ModuleImage + idModule + ".png";
                }

                // open stream
                using (System.IO.FileStream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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


        public FileUploadReturnMessage UploaderActivityAttachmentZipFile(FileUploader fileUploader)
        {
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            FileStream fileStream = null;
            //Get the file upload path store in web services web.config file.  
            string fileUploadPath = Properties.Settings.Default.ActivityAttachmentsPath + @"\_tmp\" + fileUploader.FileUploadName + ".Zip";
            try
            {
                if (!Directory.Exists(Properties.Settings.Default.ActivityAttachmentsPath + @"\_tmp"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.ActivityAttachmentsPath + @"\_tmp");
                }

                if (fileUploader.FileByte.Length > 0 && fileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(fileUploader.FileByte, 0, fileUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return fileUploadReturnMessage;

        }

        public byte[] GetCaroemIconFileInBytes(string caroemName)
        {
            byte[] bytes = null;
            try
            {

                string filepathjpg = Properties.Settings.Default.CaroemFilePath + caroemName + ".jpg";
                string filepathpng = Properties.Settings.Default.CaroemFilePath + caroemName + ".png";

                if (File.Exists(filepathjpg))
                {

                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathjpg, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                else if (File.Exists(filepathpng))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathpng, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        public byte[] GetCustomerIconFileInBytes(string customerName)
        {
            byte[] bytes = null;
            try
            {

                string filepathjpg = Properties.Settings.Default.CustomerFilePath + customerName + ".jpg";
                string filepathpng = Properties.Settings.Default.CustomerFilePath + customerName + ".png";

                if (File.Exists(filepathjpg))
                {

                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathjpg, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                else if (File.Exists(filepathpng))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(filepathpng, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        /// <summary>
        /// Upload EngineeringAnalysis ZipFile ON Server.
        /// </summary>
        /// <param name="fileUploader"></param>
        /// <returns></returns>
        public FileUploadReturnMessage UploaderEngineeringAnalysisZipFile(FileUploader fileUploader)
        {
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            FileStream fileStream = null;

            //Get the file upload path store in web services web.config file.  
            string fileUploadPath = Properties.Settings.Default.WorkingOrdersPath + @"\_tmp\" + fileUploader.FileUploadName + ".Zip";
            try
            {
                if (!Directory.Exists(Properties.Settings.Default.WorkingOrdersPath + @"\_tmp"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.WorkingOrdersPath + @"\_tmp");
                }

                if (fileUploader.FileByte.Length > 0 && fileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {
                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(fileUploader.FileByte, 0, fileUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return fileUploadReturnMessage;
        }

        public bool IsEmployeeDetailFileDeleted(string fileName, string folderName)
        {
            bool isDeleted = false;
            string fileUploadPath = "";

            if (folderName == "Education")
                fileUploadPath = Properties.Settings.Default.EmpEducationQualificationFilesPath + fileName;

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    File.Delete(fileUploadPath);
                    isDeleted = true;
                }
                else
                {
                    isDeleted = false;
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return isDeleted;

        }

        public byte[] GetArticleAttachmentFile(string atricleReference, string savedFileName)
        {
            byte[] bytes = null;
            try
            {
                string filepath = Properties.Settings.Default.ArticleAttachmentDocPath + @"\" + atricleReference + @"\" + savedFileName;



                // open stream
                using (System.IO.FileStream stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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


        public byte[] GetEmployeesExitDocument(string employeeCode,string fileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;

            if (File.Exists(Properties.Settings.Default.EmployeeExitDocument + "/" + employeeCode + "/"+ fileName + ".png"))
            {
                fileUploadPath = Properties.Settings.Default.EmployeeExitDocument + "/" + employeeCode + "/" + fileName + ".png";
            }
            else if (File.Exists(Properties.Settings.Default.EmployeeExitDocument + "/" + employeeCode + "/" + fileName))
            {
                fileUploadPath = Properties.Settings.Default.EmployeeExitDocument + "/" + employeeCode + "/" + fileName;
            }
            else
            {
                fileUploadPath = Properties.Settings.Default.EmployeeExitDocument + "/" + employeeCode + "/" + fileName + ".jpg";
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


        public byte[] GetPrintLabelFile(string printerName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;

            
            fileUploadPath = Properties.Settings.Default.MaterialWorkOrderItemLabel.Replace("{0}", printerName);

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

        public byte[] GetCompanyLayoutFile(string fileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;


            fileUploadPath = Properties.Settings.Default.CompanyLayouts+ fileName;

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


        public List<FileDetail> GetAllFileDetailsFromCompanyLayout()
        {
            
            string folderPath = string.Empty;
            List<FileDetail> fileDetails = new List<FileDetail>();

           folderPath = Properties.Settings.Default.CompanyLayouts;

            try
            {
                if (Directory.Exists(folderPath))
                {
                    DirectoryInfo d = new DirectoryInfo(folderPath);
                    FileInfo[] Files = d.GetFiles(); //Getting files
                  
                    foreach (FileInfo file in Files)
                    {
                        FileDetail fileDetail = new FileDetail();
                        fileDetail.FileName = file.Name;
                        using (System.IO.FileStream stream = new System.IO.FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            byte[] bytes = null;
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
                            fileDetail.FileByte = bytes;
                        }
                      
                        fileDetails.Add(fileDetail);


                    }

                    
                }

                return fileDetails;
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

        public byte[] GetArticleImageInBytes(string ImagePath)
        {

            byte[] bytes = null;

            if (!Directory.Exists(Properties.Settings.Default.ArticleVisualAidsPath))
            {
                return null;
            }

            string fileUploadPath = Properties.Settings.Default.ArticleVisualAidsPath + ImagePath;

            if (!File.Exists(fileUploadPath))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(ImagePath))
            {
                try
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

            return bytes;


        }

        public byte[] GetPrintDNItemLabelFile(string printerName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;


            fileUploadPath = Properties.Settings.Default.MaterialDNItemLabel.Replace("{0}", printerName);

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

        public byte[] GetPrintSmallDNItemLabelFile(string printerName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;


            fileUploadPath = Properties.Settings.Default.MaterialDNItemLabelSmall.Replace("{0}", printerName);

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

        public byte[] GetPrintDeliveryNoteItemLabelFile(string printerName,string labelSize)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;


            fileUploadPath = Properties.Settings.Default.MaterialDNItemLabelFile.Replace("{0}", printerName);
            fileUploadPath= fileUploadPath.Replace("{1}", labelSize);

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

        public byte[] GetBoxLabelFile(string printerName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;


            fileUploadPath = Properties.Settings.Default.BoxLabel.Replace("{0}", printerName);

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


        public byte[] GetOrganizationChart(string companyAlias)
        {
            byte[] bytes = null;
            string defaultFileName = @"\OrgChart.xlsm";

            string companyFileName = string.Format("{0}{1}{2}{3}",@"\", companyAlias, "_", "OrgChart.xlsm"); 

            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.CompanyOrgCharts, companyFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.CompanyOrgCharts, companyFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                else if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.CompanyOrgCharts, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.CompanyOrgCharts, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
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


        public byte[] GetActionPlanExcel()
        {
            byte[] bytes = null;
            string defaultFileName = @"\ActionPlans\ActionPlan.xlsm";

            //string companyFileName = string.Format("{0}{1}",  "OrgChart.xlsm");

            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.EmailTemplate, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        public byte[] GetBPLDetailsExcel(string fileName)
        {
            byte[] bytes = null;
            string defaultFileName = @"\PLM_BPL_Details.xlsm";
            // Properties.Settings.Default.BPL_CPL_Details is set to "C:\Temp\Templates\BPL_CPL_Details"
            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.BPLCPLExportTemplates, fileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.BPLCPLExportTemplates, fileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
                else if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.BPLCPLExportTemplates, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.BPLCPLExportTemplates, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
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



        public byte[] GetLookupImages(string fileName)
        {
            byte[] bytes = null;
            string lookupImagesFolderPath = string.Empty;
            string lookupImagesFilePath = string.Empty;
            try
            {
                lookupImagesFolderPath = Properties.Settings.Default.EnumeratedImage;

                if (!Properties.Settings.Default.EnumeratedImage.EndsWith("\\", StringComparison.InvariantCultureIgnoreCase))
                {
                    lookupImagesFolderPath = string.Concat(lookupImagesFolderPath, "\\");
                }
                lookupImagesFilePath = string.Format("{0}{1}", lookupImagesFolderPath, fileName);
                if (File.Exists(lookupImagesFilePath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(lookupImagesFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
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

        public FileUploadReturnMessage UploaderOTAttachmentZipFile(FileOTAttachmentUploader fileOTAttachmentUploader)
        {
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            FileStream fileStream = null;

            //Get the file upload path store in web services web.config file.  
            string fileUploadPath = Path.Combine(Properties.Settings.Default.WorkingOrdersPath, fileOTAttachmentUploader.Year, fileOTAttachmentUploader.QuotationCode, "Additional Information", @"_tmp", fileOTAttachmentUploader.FileUploadName + ".Zip");
            try
            {
                if (!Directory.Exists(Path.Combine(Properties.Settings.Default.WorkingOrdersPath, fileOTAttachmentUploader.Year, fileOTAttachmentUploader.QuotationCode, "Additional Information", @"_tmp")))
                {
                    Directory.CreateDirectory(Path.Combine(Properties.Settings.Default.WorkingOrdersPath, fileOTAttachmentUploader.Year, fileOTAttachmentUploader.QuotationCode, "Additional Information", @"_tmp"));
                }

                if (fileOTAttachmentUploader.FileByte.Length > 0 && fileOTAttachmentUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(fileOTAttachmentUploader.FileByte, 0, fileOTAttachmentUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return fileUploadReturnMessage;

        }

        public byte[] GetQCTemplate()
        {
            byte[] bytes = null;
            string defaultFileName = @"\TestBoardCheckList.xlsx";

            try
            {
                if (File.Exists(string.Format("{0}{1}", Properties.Settings.Default.QualityCertificationTemplatePath, defaultFileName)))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}{1}", Properties.Settings.Default.QualityCertificationTemplatePath, defaultFileName), System.IO.FileMode.Open, System.IO.FileAccess.Read))
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
            return bytes;
        }

        //[rdixit][24.05.2023]
        public byte[] GetImagesByUrl(string imageUrl)
        {
            byte[] ImageBytes = null;
            try
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    //using (WebClient webClient = new WebClient())
                    //{
                    //    ImageBytes = webClient.DownloadData(imageUrl);
                    //}
                    ImageBytes = Utility.ImageUtil.GetImageByWebClient(imageUrl);
                }
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return ImageBytes;
        }

        //[rdixit][GEOS2-3741][24.05.2023]
        public void UploadIsValidateFalseUserImage_V2400(FileUploader userProfileFileUploader)
        {
            FileStream fileStream = null;
            string fileUploadPath = Properties.Settings.Default.UserProfileImage + userProfileFileUploader.FileUploadName + ".png";
            try
            {
                if (File.Exists(fileUploadPath))
                {
                    File.Delete(fileUploadPath);
                }
                if (userProfileFileUploader.FileByte.Length > 0 && userProfileFileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(userProfileFileUploader.FileByte, 0, userProfileFileUploader.FileByte.Length);
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

        }

        //[rdixit][GEOS2-3741][24.05.2023]
        public byte[] GetUserProfileImage_V2400(string userName, byte isValidate = 1)
        {
            byte[] bytes = null;
            string fileUploadPath = "";

            if (isValidate == 1)
            {
                if (File.Exists(Properties.Settings.Default.UserProfileImage + userName + ".png"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + userName + ".png";
                }
                else if (File.Exists(Properties.Settings.Default.UserProfileImage + userName + ".jpg"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + userName + ".jpg";
                }
            }
            else
            {
                if (File.Exists(Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + "_temp.png"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + "_temp.png";
                }
                else if (File.Exists(Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + ".jpg"))
                {
                    fileUploadPath = Properties.Settings.Default.UserProfileImage + "/_tmp/" + userName + ".jpg";
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(fileUploadPath))
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
        //[nsatpute][04-09-2024][GEOS2-5415]
        public byte[] GetPrintQcPassLabelFile(string printerName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Empty;


            fileUploadPath = Properties.Settings.Default.BoxLabel.Replace("{0}", printerName);
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


        //[Sudhir.Jangra][GEOS2-6016]
        public FileUploadReturnMessage UploaderActionPlanTaskAttachmentZipFile_V2580(FileUploader fileUploader)
        {
            FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();

            fileUploadReturnMessage.IsFileUpload = false;
            FileStream fileStream = null;
            //Get the file upload path store in web services web.config file.  
            string fileUploadPath = Properties.Settings.Default.APMActionPlanTaskAttachments + @"\_tmp\" + fileUploader.FileUploadName + ".Zip";
            try
            {
                if (!Directory.Exists(Properties.Settings.Default.APMActionPlanTaskAttachments + @"\_tmp"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.APMActionPlanTaskAttachments + @"\_tmp");
                }

                if (fileUploader.FileByte.Length > 0 && fileUploader.FileByte != null)
                {
                    if (!string.IsNullOrEmpty(fileUploadPath))
                    {

                        fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        // write file stream into the specified file  
                        using (System.IO.FileStream fs = fileStream)
                        {
                            fs.Write(fileUploader.FileByte, 0, fileUploader.FileByte.Length);
                            fileUploadReturnMessage.IsFileUpload = true;
                        }
                    }
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
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                fileUploadReturnMessage.IsFileUpload = false;
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            return fileUploadReturnMessage;

        }
    }
}