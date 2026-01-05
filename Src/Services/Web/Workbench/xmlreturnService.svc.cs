using Emdep.Geos.Data.BusinessLogic;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace Emdep.Geos.Services.Web.Workbench
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "xmlreturnService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select xmlreturnService.svc or xmlreturnService.svc.cs at the Solution Explorer and start debugging.
    public class xmlreturnService : IxmlreturnService
    {
        public DataSet GetOffersWithoutPurchaseOrderReturnListDatatable1()
        {

            DataSet offerdt = new DataSet();
            StringWriter strwtr = new StringWriter();
            try
            {

                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Company companydetails = new Company();
                companydetails.ConnectPlantConstr = connectionString;
                offerdt = mgr.GetOffersWithoutPurchaseOrderReturnListDatatable1(1, 666, 2, 2016, companydetails, 22);
               // offerdt.WriteXml(offerdt);
               // offerdt = "Hello";
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            //WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml; charset=utf-8";
            return offerdt;
            //return offerdt;
        }

        public string GetOffersWithoutPurchaseOrder()
        {

            string offerdt = null;
            //StringWriter strwtr = new StringWriter();
            try
            {

                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Company companydetails = new Company();
                companydetails.ConnectPlantConstr = connectionString;
                //offerdt = mgr.GetOffersWithoutPurchaseOrderReturnListDatatable1(1, 666, 2, 2016, companydetails, 22);
              //  //offerdt.WriteXml(offerdt);
               // offerdt = "Hello";
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            //return XElement.Parse(strwtr.ToString());
            return offerdt;
        }
        public string GetOffersWithoutPurchaseOrderList()
        {

            string offerdt = null;
            //StringWriter strwtr = new StringWriter();
            try
            {

                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Company companydetails = new Company();
                companydetails.ConnectPlantConstr = connectionString;
                //offerdt = mgr.GetOffersWithoutPurchaseOrderReturnListstring(1, 666, 2, 2016, companydetails);
                //offerdt.WriteXml(strwtr);
                // offerdt = "Hello";
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            //return XElement.Parse(strwtr.ToString());
            return offerdt;
        }
        public string GetoptionsByOfferList()
        {

            string offerdt = null;
            //StringWriter strwtr = new StringWriter();
            try
            {

                CrmManager mgr = new CrmManager();
                string connectionString = ConfigurationManager.ConnectionStrings["CrmContext"].ConnectionString;
                Company companydetails = new Company();
                companydetails.ConnectPlantConstr = connectionString;
              //  offerdt = mgr.GetoptionsByOfferListstring(1, 666, 2, 2016, companydetails);
                //offerdt.WriteXml(strwtr);
                // offerdt = "Hello";
            }
            catch (Exception ex)
            {
                ServiceException exp = new ServiceException();
                exp.ErrorMessage = ex.Message;
                exp.ErrorDetails = ex.ToString();
                throw new FaultException<ServiceException>(exp, ex.ToString());
            }

            //return XElement.Parse(strwtr.ToString());
            return offerdt;
        }
    }
}
