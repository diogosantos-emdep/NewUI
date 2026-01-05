using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.TechnicalRestService;

namespace Emdep.Geos.Data.Common.APM
{
    [DataContract]
    public class OTs : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idOT;
        private Int32 idItemStatus;
        private LookupValue itemStatus;
        private string siteName;
        private string code;
        private string numItem;
        private string reference;
        private string description;
        private DateTime creationDate;
        private Int32 idSite;
        private string site;
        List<OTs> otItemsList;
        private Int64 idOTItem;
        private Int64 idItemOtStatus;
        private string statusName;
        private string codeNumber;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [NotMapped]
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
        [NotMapped]
        [DataMember]
        public string NumItem
        {
            get { return numItem; }
            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }

        [NotMapped]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        [NotMapped]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [NotMapped]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }
        [NotMapped]
        [DataMember]
        public string Site
        {
            get { return site; }
            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }
        [NotMapped]
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
        [NotMapped]
        [DataMember]
        public List<OTs> OtItemsList
        {
            get { return otItemsList; }
            set
            {
                otItemsList = value;
                OnPropertyChanged("OtItemsList");
            }
        }
        [NotMapped]
        [DataMember]
        public Int64 IdItemOtStatus
        {
            get { return idItemOtStatus; }
            set
            {
                idItemOtStatus = value;
                OnPropertyChanged("IdItemOtStatus");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdOTItem
        {
            get { return idOTItem; }
            set
            {
                idOTItem = value;
                OnPropertyChanged("IdOTItem");
            }
        }

        [NotMapped]
        [DataMember]
        public string StatusName
        {
            get { return statusName; }
            set
            {
                statusName = value;
                OnPropertyChanged("StatusName");
            }
        }

        [NotMapped]
        [DataMember]
        public string CodeNumber
        {
            get { return codeNumber; }
            set
            {
                codeNumber = value;
                OnPropertyChanged("CodeNumber");
            }
        }
        #endregion


        #region Constructor
        public OTs()
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
