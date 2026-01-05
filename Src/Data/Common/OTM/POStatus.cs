using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class POStatus : ModelBase, IDisposable
    {
        #region Fields
        int idLookupValue;
        string name;
        string htmlColor;
        #endregion

        #region Constructor

        public POStatus()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public int IdLookupValue
        {
            get { return idLookupValue; }
            set { idLookupValue =value; OnPropertyChanged("IdLookupValue"); }
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
        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
