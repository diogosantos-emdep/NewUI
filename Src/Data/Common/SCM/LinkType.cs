using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class LinkType : ModelBase, IDisposable
    {
        #region Fields
        private int idLinkType;
        private string name;
        private int idPartnerLinkType;
        private int idLinkTypeScope;
        #endregion

        #region Properties

        [DataMember]
        public int IdLinkType
        {
            get { return idLinkType; }
            set { idLinkType = value; OnPropertyChanged("IdLinkType"); }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
     
        [DataMember]
        public int IdPartnerLinkType
        {
            get { return idPartnerLinkType; }
            set { idPartnerLinkType = value; OnPropertyChanged("IdPartnerLinkType"); }
        }

        [DataMember]
        public int IdLinkTypeScope
        {
            get { return idLinkTypeScope; }
            set { idLinkTypeScope = value; OnPropertyChanged("IdLinkTypeScope"); }
        }
      
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
