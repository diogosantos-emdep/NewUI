using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.Glpi
{
    [Table("glpi_documents_items")]
    [DataContract]
    public class GlpiDocumentItem
    {
        #region Fields
        Int32 id;
        Int32? documentsId;
        Int32? itemsId;
        string itemType;
        Int32? entitiesId;
        bool isRecursive;
        DateTime? dateMod;
        #endregion

        #region Properties

        [Key]
        [Column("id")]
        [DataMember]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        [Column("documents_id")]
        [ForeignKey("GlpiDocument")]
        [DataMember]
        public Int32? DocumentsId
        {
            get { return documentsId; }
            set { documentsId = value; }
        }

        [Column("items_id")]
        [ForeignKey("GlpiTicket")]
        [DataMember]
        public Int32? ItemsId
        {
            get { return itemsId; }
            set { itemsId = value; }
        }

        [Column("itemtype")]
        [DataMember]
        public string ItemType
        {
            get { return "Ticket"; }
            set { itemType = "Ticket"; }
        }

        [Column("entities_id")]
        [DataMember]
        public Int32? EntitiesId
        {
            get { return entitiesId; }
            set { entitiesId = value; }
        }

        [Column("is_recursive")]
        [DataMember]
        public bool IsRecursive
        {
            get { return isRecursive; }
            set { isRecursive = value; }
        }

        [Column("date_mod")]
        [DataMember]
        public DateTime? DateMod
        {
            get { return dateMod; }
            set { dateMod = value; }
        }

        [DataMember]
        public virtual GlpiDocument GlpiDocument { get; set; }

        [DataMember]
        public virtual GlpiTicket GlpiTicket { get; set; }
        #endregion
    }
}
