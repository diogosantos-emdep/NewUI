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
    [Table("templates")]
    [DataContract]
    public class Template : ModelBase, IDisposable
    {
        #region  Fields

        byte idTemplate;
        string name;
        string name_es;
        string name_fr;
        string suffix;
        string name_ro;
        Int64 type;
        string name_zh;
        string name_pt;
        string name_ru;
        string description;
        string description_es;
        string description_fr;
        string description_ro;
        string description_zh;
        string description_pt;
        string description_ru;
        byte isObsolete;
        DateTime createdIn;
        DateTime modifiedIn;
        Int32 createdBy;
        Int32 modifiedBy;
        IList<Quotation> quotations;
        string htmlColor;

        #endregion

        #region Constructor
        public Template()
        {
            this.Quotations = new List<Quotation>();
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdTemplate")]
        [DataMember]
        public byte IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [Column("Name_es")]
        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [Column("Name_fr")]
        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [Column("Suffix")]
        [DataMember]
        public string Suffix
        {
            get { return suffix; }
            set
            {
                suffix = value;
                OnPropertyChanged("Suffix");
            }
        }

        [Column("Name_ro")]
        [DataMember]
        public string Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [Column("Type")]
        [DataMember]
        public Int64 Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        [Column("Name_zh")]
        [DataMember]
        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [Column("Name_pt")]
        [DataMember]
        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [Column("Name_ru")]
        [DataMember]
        public string Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }

        [Column("Description")]
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

        [Column("Description_es")]
        [DataMember]
        public string Description_es
        {
            get { return description_es; }
            set
            {
                description_es = value;
                OnPropertyChanged("Description_es");
            }
        }

        [Column("Description_fr")]
        [DataMember]
        public string Description_fr
        {
            get { return description_fr; }
            set
            {
                description_fr = value;
                OnPropertyChanged("Description_fr");
            }
        }

        [Column("Description_ro")]
        [DataMember]
        public string Description_ro
        {
            get { return description_ro; }
            set
            {
                description_ro = value;
                OnPropertyChanged("Description_ro");
            }
        }

        [Column("Description_zh")]
        [DataMember]
        public string Description_zh
        {
            get { return description_zh; }
            set
            {
                description_zh = value;
                OnPropertyChanged("Description_zh");
            }
        }

        [Column("Description_pt")]
        [DataMember]
        public string Description_pt
        {
            get { return description_pt; }
            set
            {
                description_pt = value;
                OnPropertyChanged("Description_pt");
            }
        }

        [Column("Description_ru")]
        [DataMember]
        public string Description_ru
        {
            get { return description_ru; }
            set
            {
                description_ru = value;
                OnPropertyChanged("Description_ru");
            }
        }

        [Column("IsObsolete")]
        [DataMember]
        public byte IsObsolete
        {
            get { return isObsolete; }
            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }

        [Column("CreatedIn")]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [Column("ModifiedIn")]
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [Column("CreatedBy")]
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("ModifiedBy")]
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public virtual IList<Quotation> Quotations
        {
            get { return quotations; }
            set
            {
                quotations = value;
                OnPropertyChanged("Quotations");
            }
        }

        [Column("HtmlColor")]
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

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
