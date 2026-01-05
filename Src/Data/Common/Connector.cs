using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{


    public class Connector
    {
        #region Fields
       
        string connectorEmdepRef;
        string connectorImagePath;
        byte[] connImageBytes;
        #endregion


        #region Properties
      
        [DataMember]
        public string ConnectorEmdepRef
        {
            get
            {
                return connectorEmdepRef;
            }

            set
            {
                connectorEmdepRef = value;
            }
        }

        [DataMember]
        public string ConnectorImagePath
        {
            get
            {
                return connectorImagePath;
            }

            set
            {
                connectorImagePath = value;
            }
        }

        [DataMember]
        public byte[] ConnImageBytes
        {
            get
            {
                return connImageBytes;
            }

            set
            {
                connImageBytes = value;
            }
        }


        #endregion


    }
}
