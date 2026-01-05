using Emdep.Geos.Data.Common.OTMDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OTM
{
    [DataContract]
    public class AttachemtType : ModelBase, IDisposable
    {

        #region Fields
        private Int32 idType;
        private string type;
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdType
        {
            get { return idType; }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

      
        [DataMember]
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        #endregion

        #region Constructor
        public AttachemtType()
        {

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
