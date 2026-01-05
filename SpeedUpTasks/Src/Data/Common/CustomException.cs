using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class CustomException : Exception, ISerializable
    {
        public CustomException()
        {
            //put your custom code here
        }

        public CustomException(string message)
            : base(message)
        {
            //put your custom code here
        }

        public CustomException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
       
    }
}
