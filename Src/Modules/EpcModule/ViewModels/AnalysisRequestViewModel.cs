using DevExpress.Mvvm;
using Emdep.Geos.Modules.Epc.Common.EPC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
  public class AnalysisRequestViewModel:NavigationViewModelBase
    {

        ObservableCollection<DummyProjects> listAnalysisRequest = new ObservableCollection<DummyProjects>();

        public ObservableCollection<DummyProjects> ListAnalysisRequest
        {
            get { return listAnalysisRequest; }
            set { listAnalysisRequest = value; }
        }
      public AnalysisRequestViewModel()
      {
          ListAnalysisRequest.Add(new DummyProjects() { ProjectCode = "2016PR-2309", ProjectName = "GTI Module", OTCode = "20162309", Customer = "FujikuraIndia", ProjectStatusTypes = ProjectStatusType.UnderStudy, AnalysisOwner = "JAMZ", Requester = "Acruz", RequestDate = "10/30/2015", ProjectTypes = ProjectType.Short, StartDate = "01/30/2016", ExpectedEndDate = "06/30/2016" });
          ListAnalysisRequest.Add(new DummyProjects() { ProjectCode = "2016PR-2335", ProjectName = "KSK Navigation", OTCode = "20162335", Customer = "Delphi", ProjectStatusTypes = ProjectStatusType.NewRequest, AnalysisOwner = "fpinas", Requester = "Zordi", RequestDate = "10/30/2015", StartDate = "07/30/2016", ProjectTypes = ProjectType.Strategical, ExpectedEndDate = "06/30/2016" });
          ListAnalysisRequest.Add(new DummyProjects() { ProjectCode = "2015PR-4507", ProjectName = "GEOS2", OTCode = "20154507", Customer = "Emdep Spain", ProjectStatusTypes = ProjectStatusType.NewRequest, AnalysisOwner = "sbambrule", Requester = "Acruz", RequestDate = "10/30/2015", StartDate = "01/12/2015", ProjectTypes = ProjectType.Long, ExpectedEndDate = "12/30/2015" });
      }
    }
}
