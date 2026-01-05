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
    [Table("peopletypes")]
    [DataContract]
    public class PeopleType : ModelBase, IDisposable
    {
        #region Fields
        byte idPersonType;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        string name_zh;
        string name_ro;
        IList<People> peoples;
        #endregion

        #region Constructor
        public PeopleType()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdPersonType")]
        [DataMember]
        public byte IdPersonType
        {
            get
            {
                return idPersonType;
            }

            set
            {
                idPersonType = value;
                OnPropertyChanged("IdPersonType");
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

        [DataMember]
        public virtual IList<People> Peoples
        {
            get
            {
                return peoples;
            }

            set
            {
                peoples = value;
                OnPropertyChanged("Peoples");
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
