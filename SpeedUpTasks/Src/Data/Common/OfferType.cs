using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("offertypes")]
    [DataContract]
    public class OfferType
    {
        #region Fields

        Byte idOfferType;
        string name;
        string name_es;
        string name_fr;
        string name_pt;
        string name_ro;
        string name_zh;

        #endregion

        #region Constructor

        public OfferType()
        {

        }

        #endregion

        #region Properties

        [Key]
        [Column("IdOfferType")]
        [DataMember]
        public Byte IdOfferType
        {
            get { return idOfferType; }
            set { idOfferType = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("Name_es")]
        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set { name_es = value; }
        }

        [Column("Name_fr")]
        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set { name_fr = value; }
        }

        [Column("Name_pt")]
        [DataMember]
        public string Name_pt
        {
            get { return name_pt; }
            set { name_pt = value; }
        }

        [Column("Name_ro")]
        [DataMember]
        public string Name_ro
        {
            get { return name_ro; }
            set { name_ro = value; }
        }

        [Column("Name_zh")]
        [DataMember]
        public string Name_zh
        {
            get { return name_zh; }
            set { name_zh = value; }
        }

        #endregion
    }
}
