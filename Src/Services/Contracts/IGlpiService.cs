
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.Glpi;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Emdep.Geos.Services.Contracts
{
    [ServiceContract]
    public interface IGlpiService
    {
        /// <summary>
        /// This method is to add GLPI ticket
        /// </summary>
        /// <param name="glpiticket">Get GLPI ticket details</param>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddGLPITicket(GlpiTicket glpiTicket);

         /// <summary>
        /// This method is to get GLPIUser detail by name
        /// </summary>
        /// <param name="name">Get geos user name</param>
        /// <returns>Details of GLPIUser related to name from class GLPIUser</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GlpiUser GetGLPIUserByName(string name);

        /// <summary>
        /// This method is to get GLPILocation detail by geos companyid
        /// </summary>
        /// <param name="siteId">Get geos companyId</param>
        /// <returns>Details of GLPILocation related to Id from class GLPILocation</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GlpiLocation GetGLPILocationByCompanyId(Int32 companyId);

         /// <summary>
        /// This method is to get list of GLPI Document types
        /// </summary>
        /// <returns>List of GLPI Document types</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<GlpiDocumentType> GetGLPIDocumentType();

        /// <summary>
        /// This method is to get GLPIItilCategory detail by geos moduleid
        /// </summary>
        /// <param name="moduleId">Get geos module id</param>
        /// <returns>Details of GLPIItilCategory related to Id from class GLPIItilCategory</returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        GlpiItilCategory GetGLPIItilCategoryByModuleId(Int32 moduleId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SendGlpiTicketMail(GlpiTicketMail glpiTicketMail);
    }
}
