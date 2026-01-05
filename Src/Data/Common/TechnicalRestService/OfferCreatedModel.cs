using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TechnicalRestService
{
    public class OfferCreatedModel

    {

        [Display(Order = 1)]
        public Int64 id { get; set; }

        [Display(Order = 2)]
        public string code { get; set; }


    }
}
