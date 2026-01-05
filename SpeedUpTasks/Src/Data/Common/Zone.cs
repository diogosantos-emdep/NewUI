using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("zones")]
    [DataContract]
    public class Zone : ModelBase, IDisposable
    {
        #region Fields
        Int32 idZone;
        string code;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        string name_ro;
        string name_ru;
        string name_zh;
        IList<Country> countries;
        #endregion

        #region Constructor
        public Zone()
        {
           this.Countries = new List<Country>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdZone")]
        [DataMember]
        public Int32 IdZone
        {
            get
            {
                return idZone;
            }

            set
            {
                idZone = value;
                OnPropertyChanged("IdZone");
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


        [Column("Name_es")]
        [DataMember]
        public string Name_es
        {
            get
            {
                return name_es;
            }

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
            get
            {
                return name_fr;
            }

            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [Column("Name_pt")]
        [DataMember]
        public string Name_pt
        {
            get
            {
                return name_pt;
            }

            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }


        [Column("Name_ro")]
        [DataMember]
        public string Name_ro
        {
            get
            {
                return name_ro;
            }

            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }


        [Column("Name_ru")]
        [DataMember]
        public string Name_ru
        {
            get
            {
                return name_ru;
            }

            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }


        [Column("Name_zh")]
        [DataMember]
        public string Name_zh
        {
            get
            {
                return name_zh;
            }

            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public virtual IList<Country> Countries
        {
            get
            {
                return countries;
            }

            set
            {
                countries = value;
                OnPropertyChanged("countries");
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
