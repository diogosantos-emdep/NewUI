using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Modules.APM.Interface
{ 
    //[rdixit][GEOS2-9316][26.08.2025]
    public interface IGridExportService
     {
        void Export(string filePath);

     }
}
