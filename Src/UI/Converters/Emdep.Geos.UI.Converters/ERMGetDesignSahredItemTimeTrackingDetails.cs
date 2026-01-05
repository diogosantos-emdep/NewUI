using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using System.Windows;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.ERM;
using static System.Net.WebRequestMethods;


namespace Emdep.Geos.UI.Converters
{

    public class ERMGetDesignSahredItemTimeTrackingDetails 
    {
        //
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMService = new ERMServiceController("localhost:6699");
       IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      //  IPLMService PLMService = new PLMServiceController("localhost:6699");
        #region [GEOS2-7091][rani dhamankar][11 09 2025]
        public List<TimeTrackingCurrentStage> GetTrackingDetails(UInt32 PlantID, DateTime fromDate, DateTime toDate, string IdOperator)
        {
            List<TimeTrackingCurrentStage> Trackinglist = new List<TimeTrackingCurrentStage>();
            try
            {
                if (PlantID != 0)
                {
                    IList<Company> allPlantswithURL = PLMService.GetEmdepSitesCompaniesWithServiceURL_V2490();
                    var Plant = allPlantswithURL.Where(a => a.IdCompany == PlantID).FirstOrDefault();

                    if (Plant != null)
                    {
                        ERMService = new ERMServiceController(Plant.ServiceProviderUrl);
                        //ERMService = new ERMServiceController("10.13.3.33:86");
                        //ERMService = new ERMServiceController("localhost:6699");
                        //Trackinglist = ERMService.GetOperatorDesignSahredItemTimeTrackingDetails_V2670(PlantID, fromDate, toDate, IdOperator, 0);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return Trackinglist;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}
