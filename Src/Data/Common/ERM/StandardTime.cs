using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [Obsolete("Use StandardOperationsDictionary class")]
    [DataContract]
    public class StandardTime: ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardTime;
        string code;
        string name;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        DateTime effectiveDate;
        DateTime expiryDate;
        UInt32 idStatus;
        string remark;
        string remark_es;
        string remark_fr;
        string remark_ro;
        string remark_zh;
        string remark_pt;
        string remark_ru;
        byte isDeleted;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        LookupValue status;
        string plants;
        DateTime lastUpdated;
        private string statusAbbreviation;
        private string statusHtmlColor;
        private string htmlColor;
        #endregion

        #region Properties


        [DataMember]
        public ulong IdStandardTime {
            get
            {
                return idStandardTime;
            }
            set
            {
                idStandardTime = value;
                OnPropertyChanged("IdStandardTime");
            }
                
                }
        [Key]
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
        [Key]
        [Column("Name")]
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
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public string Description_es
        {
            get
            {
                return description_es;
            }

            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [DataMember]
        public string Description_fr
        {
            get
            {
                return description_fr;
            }

            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [DataMember]
        public string Description_ro
        {
            get
            {
                return description_ro;
            }

            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [DataMember]
        public string Description_zh
        {
            get
            {
                return description_zh;
            }

            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [DataMember]
        public string Description_pt
        {
            get
            {
                return description_pt;
            }

            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [DataMember]
        public string Description_ru
        {
            get
            {
                return description_ru;
            }

            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }

        [DataMember]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDate;
            }

            set
            {
                effectiveDate = value;
                OnPropertyChanged("EffectiveDate");
            }
        }

        [DataMember]
        public DateTime ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                expiryDate = value;
                OnPropertyChanged("ExpiryDate");
            }
        }



        [DataMember]
        public UInt32 IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public string Remark
        {
            get
            {
                return remark;
            }

            set
            {
                remark = value;
                OnPropertyChanged("Remark");
            }
        }

        [DataMember]
        public string Remark_es
        {
            get
            {
                return remark_es;
            }

            set
            {
                remark_es = value;
                OnPropertyChanged("Remark_es");
            }
        }

        [DataMember]
        public string Remark_fr
        {
            get
            {
                return remark_fr;
            }

            set
            {
                remark_fr = value;
                OnPropertyChanged("Remark_fr");
            }
        }

        [DataMember]
        public string Remark_ro
        {
            get
            {
                return remark_ro;
            }

            set
            {
                remark_ro = value;
                OnPropertyChanged("Remark_ro");
            }
        }

        [DataMember]
        public string Remark_zh
        {
            get
            {
                return remark_zh;
            }

            set
            {
                remark_zh = value;
                OnPropertyChanged("Remark_zh");
            }
        }

        [DataMember]
        public string Remark_pt
        {
            get
            {
                return remark_pt;
            }

            set
            {
                remark_pt = value;
                OnPropertyChanged("Remark_pt");
            }
        }

        [DataMember]
        public string Remark_ru
        {
            get
            {
                return remark_ru;
            }

            set
            {
                remark_ru = value;
                OnPropertyChanged("Remark_ru");
            }
        }

        [DataMember]
        public byte IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [DataMember]
        public uint IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public uint? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }

        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public LookupValue Status
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

        [DataMember]
        public string Plants
        {
            get
            {
                return plants;
            }

            set
            {
                plants = value;
                OnPropertyChanged("Plants");
            }
        }


        [DataMember]
        public DateTime LastUpdated
        {
            get
            {
                return lastUpdated;
            }

            set
            {
                lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public string StatusHtmlColor
        {
            get
            {
                return statusHtmlColor;
            }

            set
            {
                statusHtmlColor = value;
                OnPropertyChanged("StatusHtmlColor");
            }
        }
        [DataMember]
        public string StatusAbbreviation
        {
            get
            {
                return statusAbbreviation;
            }

            set
            {
                statusAbbreviation = value;
                OnPropertyChanged("StatusAbbreviation");
            }
        }

        #endregion

        #region Constructor

        public StandardTime()
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
