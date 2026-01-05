using System;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class AdditionalArticleCost : ModelBase, IDisposable
    {
        #region Declaration

     
        Int32 idSite;
        string siteName;
        float? transport;
        float? customs;
        #endregion

        #region Properties

     
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }

        [DataMember]
        public float? Transport
        {
            get { return transport; }
            set
            {
                transport = value;
                OnPropertyChanged("Transport");
            }
        }


      
        [DataMember]
        public float? Customs
        {
            get { return customs; }
            set
            {
                customs = value;
                OnPropertyChanged("Customs");
            }
        }

        
        #endregion

        #region Constructor

        public AdditionalArticleCost()
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
