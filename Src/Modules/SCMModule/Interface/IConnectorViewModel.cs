using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.SCM.Interface
{
    //[GEOS2-9552][rdixit][19.09.2025]
    internal interface IConnectorViewModel : ITabViewModel
    {
        ObservableCollection<SearchConnector> ConnectorSearchList { get; }
        string TabName { get; set; }
    }
}
