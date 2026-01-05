using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class StageByOTItemAndIDDrawingAPI
    {
       


        [IgnoreDataMember]
        public List<StagesAPI> OTITemStagesList { get; set; }

        [IgnoreDataMember]
        public List<StagesAPI> DrawingIdStagesList { get; set; }
        

    }
}
