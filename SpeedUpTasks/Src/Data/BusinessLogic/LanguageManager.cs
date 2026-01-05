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
    public class LanguageManager
    {
        /// <summary>
        /// This method is to get list of all languages
        /// </summary>
        /// <returns>List of all languages</returns>
        public List<Language> GetAllLanguage()
        {
            List<Language> language = null;
            using (var db = new WorkbenchContext())
            {
                language = (from records in db.Languages select records).ToList();
            }
            return language;
        }
    }
}
