using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Data.Common.Hrm;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;
namespace Emdep.Geos.Services.Contracts
{

    [ServiceContract]
    public interface IHarnessPartService
    {
        /// <summary>
        /// This method is to get list of all languages
        /// </summary>
        /// <returns>List of all languages</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Language> GetAllLanguage();

         /// <summary>
        /// This method is to get all Color
        /// </summary>
        /// <returns>List of all color from class color</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Color> GetAllColor();

        /// <summary>
        /// This method is to get all companies
        /// </summary>
        /// <returns>List of company</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<Company> GetAllCompanies();

        /// <summary>
        /// This method is to get all Harness Part Accessory Type
        /// </summary>
        /// <returns>List of all Harness Part Accessory Type</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HarnessPartAccessoryType> GetAllHarnessPartAccessoryType();


        /// <summary>
        /// This method is to get all enterprise group
        /// </summary>
        /// <returns>List of all enterprise group</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EnterpriseGroup> GetAllEnterpriseGroup();


        /// <summary>
        /// This method is to get all harness part type
        /// </summary>
        /// <returns>List of all harness part type</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HarnessPartType> GetAllHarnessPartType();

         /// <summary>
        /// This method is to get all harness part search result
        /// </summary>
        /// <returns>List of all harness part search result</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HarnessPart> GetAllHarnessPartSearchResult(HarnessPartSearch harnessPartSearch);

        /// <summary>
        /// This method is to get all harness part search result for possible Search
        /// </summary>
        /// <returns>List of all harness part search result for possible Search</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HarnessPart> GetAllHarnessPartSearchResultPossibleSearch(HarnessPartSearch harnessPartSearch);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<HarnessPart> GetEnumTest();
    }
}
