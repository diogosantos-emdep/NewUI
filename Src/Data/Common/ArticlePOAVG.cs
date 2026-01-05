using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class ArticlePOAVG
    {
        #region  Fields
        UInt64 idArticle;
        double poAVG;
       
        #endregion
        #region Properties

        [DataMember]
        public UInt64 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
            }
        }

        [DataMember]
        public double POAVG
        {
            get
            {
                return poAVG;
            }

            set
            {
                poAVG = value;
            }
        }

       

        #endregion
    }
}
