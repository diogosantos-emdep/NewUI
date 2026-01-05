using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.DataAccess;
using Emdep.Geos.Data.Common;
using System.Net;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure;
using System.Xml;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.HarnessPart;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class ColorManager
    {
        /// <summary>
        /// This method is to get all Color
        /// </summary>
        /// <returns>List of all color from class color</returns>
        public List<Color> GetAllColor()
        {
            List<Color> Colors = null;
            using (var db = new HarnessPartContext())
            {
                Colors = (from records in db.Colors select records).ToList();
            }
            return Colors;
        }
    }
}
