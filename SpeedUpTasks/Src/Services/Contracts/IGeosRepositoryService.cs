
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Emdep.Geos.Services.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGeosRepositoryService" in both code and config file together.
    [ServiceContract]
    public interface IGeosRepositoryService
    {
        /// <summary>
        /// Function to download current version and return it in bytes from table GetWorkbenchDownloadVersion
        /// </summary>
        /// <param name="geosworkbenchversion">To get current version number from table GeosWorkbenchVersion</param>
        /// <returns>FileTransferRequest:-File in bytes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileDownloadReturnMessage GetWorkbenchDownloadVersion(FileDownloadMessage mgr);

        /// <summary>
        /// This method is to download current site image and return it in bytes 
        /// </summary>
        /// <param name="idCompany">To get current id company from class site</param>
        /// <returns>File in bytes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCompanyImage(Int32 idCompany);

        /// <summary>
        /// This method is to upload user profile image from bytes
        /// </summary>
        /// <param name="fileBytes">Get file bytes to upload</param>
        /// <param name="userID">Get user id</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void Uploader(FileUploader userProfileFileUploader);

        /// <summary>
        ///  This method is to get user profile image in bytes
        /// </summary>
        /// <param name="userName">Get user name</param>
        /// <returns>File in bytes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetUserProfileImage(string userName, byte isValidate = 1);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetEmployeesImage(string employeeCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetUserProfileImageWithoutException(string userName, byte isValidate = 1);
        /// <summary>
        ///  This method is to upload zip file from bytes
        /// </summary>
        /// <param name="fileUploader">Get file in  bytes</param>
        /// <returns>Uploader Zip file or not</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileUploadReturnMessage UploaderGLPIZipFile(FileUploader fileUploader);

        /// <summary>
        /// This method is to upload user profile invalidate image from bytes
        /// </summary>
        /// <param name="userProfileFileUploader">Get File details to upload user profile invalidate image </param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UploadIsValidateFalseUserImage(FileUploader userProfileFileUploader);

        /// <summary>
        /// This method is to get module image
        /// </summary>
        /// <param name="idModule">Get Module id to get Module image</param>
        /// <returns>Module image file in bytes</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetModuleImage(Int32 idModule);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileUploadReturnMessage TaskAttachmentUploader(FileUploader userProfileFileUploader);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileUploadReturnMessage UploaderProjectScopeFile(FileUploader fileUploader);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileUploadReturnMessage UploaderActivityAttachmentZipFile(FileUploader fileUploader);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCaroemIconFileInBytes(string caroemName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCustomerIconFileInBytes(string customerName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        FileUploadReturnMessage UploaderEngineeringAnalysisZipFile(FileUploader fileUploader);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsEmployeeDetailFileDeleted(string fileName, string folderName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetArticleAttachmentFile(string atricleReference, string savedFileName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetEmployeesExitDocument(string employeeCode, string fileName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetPrintLabelFile(string printerName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetCompanyLayoutFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<FileDetail> GetAllFileDetailsFromCompanyLayout();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetArticleImageInBytes(string ImagePath);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetPrintDNItemLabelFile(string printerName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetBoxLabelFile(string printerName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetOrganizationChart(string companyAlias);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetActionPlanExcel();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        byte[] GetPrintSmallDNItemLabelFile(string printerName);

    }
}
