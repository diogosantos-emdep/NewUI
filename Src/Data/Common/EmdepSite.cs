using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("emdepsites")]
    [DataContract]
    public class EmdepSite : ModelBase, IDisposable
    {
        #region Fields
        Int64 idSite;
        string ip;
        string code;
        string quotationPattern;
        string connectorsPath;
        sbyte status;
        string articlesPath;
        string publicIP;
        byte alias;
        byte idCurrency;
        byte syncStatus;
        DateTime latestSyncStatusVerification;
        string fileServerIP;
        string databaseIP;
        string shortName;
        bool isSitePermission;
        #endregion

        #region Constructor
        public EmdepSite()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("idSite")]
        [DataMember]
        public Int64 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [Column("IP")]
        [DataMember]
        public string IP
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
                OnPropertyChanged("IP");
            }
        }

        [NotMapped]
        [DataMember]
        public string ShortName
        {
            get
            {
                return shortName;
            }

            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }


        [Column("QuotationPattern")]
        [DataMember]
        public string QuotationPattern
        {
            get
            {
                return quotationPattern;
            }

            set
            {
                quotationPattern = value;
                OnPropertyChanged("QuotationPattern");
            }
        }


        [Column("ConnectorsPath")]
        [DataMember]
        public string ConnectorsPath
        {
            get
            {
                return connectorsPath;
            }

            set
            {
                connectorsPath = value;
                OnPropertyChanged("ConnectorsPath");
            }
        }


        [Column("Status")]
        [DataMember]
        public sbyte Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [Column("ArticlesPath")]
        [DataMember]
        public string ArticlesPath
        {
            get
            {
                return articlesPath;
            }

            set
            {
                articlesPath = value;
                OnPropertyChanged("ArticlesPath");
            }
        }

        [Column("PublicIP")]
        [DataMember]
        public string PublicIP
        {
            get
            {
                return publicIP;
            }

            set
            {
                publicIP = value;
                OnPropertyChanged("PublicIP");
            }
        }

        [Column("Alias")]
        [DataMember]
        public byte Alias
        {
            get
            {
                return alias;
            }

            set
            {
                alias = value;
                OnPropertyChanged("Alias");
            }
        }

        [Column("IdCurrency")]
        [DataMember]
        public byte IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }

        [Column("SyncStatus")]
        [DataMember]
        public byte SyncStatus
        {
            get
            {
                return syncStatus;
            }

            set
            {
                syncStatus = value;
                OnPropertyChanged("SyncStatus");
            }
        }


        [Column("LatestSyncStatusVerification")]
        [DataMember]
        public DateTime LatestSyncStatusVerification
        {
            get
            {
                return latestSyncStatusVerification;
            }

            set
            {
                latestSyncStatusVerification = value;
                OnPropertyChanged("LatestSyncStatusVerification");
            }
        }

        [Column("FileServerIP")]
        [DataMember]
        public string FileServerIP
        {
            get
            {
                return fileServerIP;
            }

            set
            {
                fileServerIP = value;
                OnPropertyChanged("FileServerIP");
            }
        }

        [Column("DatabaseIP")]
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
                OnPropertyChanged("DatabaseIP");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsSitePermission
        {
            get
            {
                return isSitePermission;
            }

            set
            {
                isSitePermission = value;
                OnPropertyChanged("IsSitePermission");
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
