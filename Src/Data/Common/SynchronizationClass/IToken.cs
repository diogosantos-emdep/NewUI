using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SynchronizationClass
{

        public interface ITokenService
        {
            Task<string> GetToken();
        }
   
}
