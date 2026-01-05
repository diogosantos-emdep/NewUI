using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_Lookup_Value : ModelBase, IDisposable
    {
        #region Field
        private Int32 idLookupValue;
        private string lookupValue;
        private string htmlColor;//[GEOS2-5750][gulab lakade][21052024]

        #endregion
        #region Property
        [DataMember]
        public Int32 IdLookupValue
        {
            get
            {
                return idLookupValue;
            }

            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }

        [DataMember]
        public string LookupValue
        {
            get
            {
                return lookupValue;
            }

            set
            {
                lookupValue = value;
                OnPropertyChanged("LookupValue");
            }
        }
        //[GEOS2-5750][gulab lakade][21052024]
        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }

            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        #endregion

        #region method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
