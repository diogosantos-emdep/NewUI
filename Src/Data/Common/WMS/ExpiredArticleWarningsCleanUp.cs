using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    public class ExpiredArticleWarningsCleanUp : ModelBase, IDisposable
    {


        #region Properties
        Int64 idArticleComment;
        [DataMember]
        public Int64 IdArticleComment
        {
            get
            {
                return idArticleComment;
            }

            set
            {
                idArticleComment = value;
            }
        }


        Int64 idoffer;
        [DataMember]
        public Int64 Idoffer
        {
            get
            {
                return idoffer;
            }

            set
            {
                idoffer = value;
            }
        }

        Int32 idwarehouse;
        [DataMember]
        public Int32 Idwarehouse
        {
            get
            {
                return idwarehouse;
            }

            set
            {
                idwarehouse = value;
            }
        }

        string databaseIP;
        [DataMember]
        public string DatabaseIP
        {
            get
            {
                return databaseIP;
            }

            set
            {
                databaseIP = value;
            }
        }

        string serviceProviderUrl;
        [DataMember]
        public string ServiceProviderUrl
        {
            get
            {
                return serviceProviderUrl;
            }

            set
            {
                serviceProviderUrl = value;
            }
        }
        #endregion
        #region Constructor

        public ExpiredArticleWarningsCleanUp()
        {
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
