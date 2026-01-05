using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public class ProductionTimeReportLegend: ModelBase, IDisposable
    {
        #region Field

        private Int32 idLookupValue;
        private Int32 idLookupKey;
        private string name;
        private string htmlColor;
        private bool? inUse;
        private Int32 position;
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
        public Int32 IdLookupKey
        {
            get
            {
                return idLookupKey;
            }

            set
            {
                idLookupKey = value;
                OnPropertyChanged("IdLookupKey");
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
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

        [DataMember]
        public bool? InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }
        [DataMember]
        public Int32 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
