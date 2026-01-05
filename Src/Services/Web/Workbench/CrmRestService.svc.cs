using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Services.Contracts;
using System.Configuration;
using System.Data;
using Emdep.Geos.Data.Common.File;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CrmRestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CrmRestService.svc or CrmRestService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CrmRestService : ICrmRestService
    {



        public List<ActivityGrid> GetActivity(ActivityParams objActivityParams)
        {
            List<ActivityGrid> activities = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                activities = mgr.GetSharedActivitiesByIdPermission(connectionString, objActivityParams);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return activities;
        }

        public List<CompanyGrid> GetCustomersBySalesOwnerId(CompanyParams companyParams)
        {
            //CompanyParams companyParams = new CompanyParams();
            //companyParams.idUser = 666;
            //companyParams.idZone = 3;
            //companyParams.idUserPermission = 20;
            List<CompanyGrid> companies = null;

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                if (companyParams.Roles == RoleType.SalesAssistant)
                    companies = mgr.GetCustomersBySalesOwnerId(connectionString, companyParams);
                else if (companyParams.Roles == RoleType.SalesPlantManager)
                    companies = mgr.GetSelectedUserCustomersBySalesOwnerId(connectionString, companyParams);
                else if (companyParams.Roles == RoleType.SalesGlobalManager)
                    companies = mgr.GetSelectedUserCustomersByPlant(connectionString, companyParams);

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return companies;
        }

        public List<TimelineGrid> GetTimelineGridDetails(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                if (timelineParams.Roles == RoleType.SalesAssistant)
                    timelineGridList = mgr.GetTimelineGridDetails(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesPlantManager)
                    timelineGridList = mgr.GetSelectedUsersTimelineGridDetails(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesGlobalManager)
                    timelineGridList = mgr.GetTimelineGridDetails(timelineParams).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        public List<PeopleDetails> GetPeoples()
        {
            List<PeopleDetails> peoples = null;

            try
            {
                CrmRestManager mgr = new CrmRestManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                
                peoples = mgr.GetPeoples(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return peoples;
        }


        public List<OrderGrid> GetOrderGridDetails(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails(orderParams).ToList();
             }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }


        public List<TimelineGrid> GetTimelineGridDetails_V2031(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                if (timelineParams.Roles == RoleType.SalesAssistant)
                    timelineGridList = mgr.GetTimelineGridDetails_V2031(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesPlantManager)
                    timelineGridList = mgr.GetSelectedUsersTimelineGridDetails_V2031(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesGlobalManager)
                    timelineGridList = mgr.GetTimelineGridDetails_V2031(timelineParams).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        public List<TimelineGrid> GetTimelineGridDetails_V2033(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                if (timelineParams.Roles == RoleType.SalesAssistant)
                    timelineGridList = mgr.GetTimelineGridDetails_V2033(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesPlantManager)
                    timelineGridList = mgr.GetSelectedUsersTimelineGridDetails_V2033(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesGlobalManager)
                    timelineGridList = mgr.GetTimelineGridDetails_V2033(timelineParams).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        public List<OrderGrid> GetOrderGridDetails_V2035(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2035(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }


        public List<TimelineGrid> GetTimelineGridDetails_V2036(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                if (timelineParams.Roles == RoleType.SalesAssistant)
                    timelineGridList = mgr.GetTimelineGridDetails_V2036(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesPlantManager)
                    timelineGridList = mgr.GetSelectedUsersTimelineGridDetails_V2036(timelineParams).ToList();
                else if (timelineParams.Roles == RoleType.SalesGlobalManager)
                    timelineGridList = mgr.GetTimelineGridDetails_V2036(timelineParams).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }


        public List<TimelineGrid> GetTimelineGridDetails_V2037(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                timelineGridList = mgr.GetTimelineGridDetails_V2037(timelineParams).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        public List<OrderGrid> GetOrderGridDetails_V2037(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2037(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }

        public Tuple<string, byte[]> GetOfferEngAnalysisAttachments(OfferParams offerParams)
        {
          
            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                return mgr.GetOfferEngAnalysisAttachments(Properties.Settings.Default.WorkingOrdersPath, offerParams);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
         
        }

        public List<GeosProviderDetail> GetGeosProviderDetail()
        {
           
           try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["WorkbenchContext"].ConnectionString;
                return mgr.GetGeosProviderDetail(connectionString);
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
           
        }


        public List<TimelineGrid> GetTimelineGridDetails_V2040(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                timelineGridList = mgr.GetTimelineGridDetails_V2040(timelineParams,connectionString).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }


        public List<OrderGrid> GetOrderGridDetails_V2110(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2110(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }

        public List<OrderGrid> GetOrderGridDetails_V2120(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2120(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }



        public List<TimelineGrid> GetTimelineGridDetails_V2120(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                timelineGridList = mgr.GetTimelineGridDetails_V2120(timelineParams, connectionString).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }


        public List<OrderGrid> GetOrderGridDetails_V2200(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2200(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }



        public List<TimelineGrid> GetTimelineGridDetails_V2200(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                timelineGridList = mgr.GetTimelineGridDetails_V2200(timelineParams, connectionString).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        //[pmisal][GEOS2-4323][10.04.2023]
        public List<TimelineGrid> GetTimelineGridDetails_V2380(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                timelineGridList = mgr.GetTimelineGridDetails_V2380(timelineParams, connectionString, Properties.Settings.Default.CountryFilePath).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        //[pmisal][10.04.2023]
        public List<OrderGrid> GetOrderGridDetails_V2380(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2380(orderParams, Properties.Settings.Default.CountryFilePath).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }

        //[GEOS2-4284][rdixit][09.05.2023]
        public List<TimelineGrid> GetTimelineGridDetails_V2390(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                timelineGridList = mgr.GetTimelineGridDetails_V2390(timelineParams, connectionString, Properties.Settings.Default.CountryFilePath).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }
        //[GEOS2-4284][rdixit][09.05.2023]
        public List<OrderGrid> GetOrderGridDetails_V2390(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2390(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }

        public List<TimelineGrid> GetTimelineGridDetails_V2420(TimelineParams timelineParams)
        {
            List<TimelineGrid> timelineGridList = new List<TimelineGrid>();

            try
            {
                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                timelineGridList = mgr.GetTimelineGridDetails_V2420(timelineParams, connectionString, Properties.Settings.Default.CountryFilePath).ToList();

            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return timelineGridList;
        }

        public List<OrderGrid> GetOrderGridDetails_V2420(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2420(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }


        public List<OrderGrid> GetOrderGridDetails_V2670(OrderParams orderParams)
        {
            List<OrderGrid> orderGridList = new List<OrderGrid>();

            try
            {
                OptimizedCrmManager mgr = new OptimizedCrmManager();
                orderGridList = mgr.GetOrderGridDetails_V2670(orderParams).ToList();
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }
            return orderGridList;
        }
    }
}
