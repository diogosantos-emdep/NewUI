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
    [Table("glpi_logs")]
    [DataContract]
    public class GlpiLog
    {
        #region Fields
        Int32 id;
        string itemType;
        Int32? itemsId;
        string itemTypeLink;
        Int32? linkedAction;
        Int32? idSearchOption;
        string userName;
        DateTime? dateMod;
        string oldValue;
        string newValue;
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

        [Column("itemtype")]
        [DataMember]
        public string ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        [Column("items_id")]
        [ForeignKey("GlpiTicket")]
        [DataMember]
        public Int32? ItemsId
        {
            get { return itemsId; }
            set { itemsId = value; }
        }

        [Column("itemtype_link")]
        [DataMember]
        public string ItemTypeLink
        {
            get { return itemTypeLink; }
            set { itemTypeLink = value; }
        }

        [Column("linked_action")]
        [DataMember]
        public Int32? LinkedAction
        {
            get { return linkedAction; }
            set { linkedAction = value; }
        }

        [Column("id_search_option")]
        [DataMember]
        public Int32? IdSearchOption
        {
            get { return idSearchOption; }
            set { idSearchOption = value; }
        }

        [Column("user_name")]
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [Column("date_mod")]
        [DataMember]
        public DateTime? DateMod
        {
            get { return dateMod; }
            set { dateMod = value; }
        }

        [Column("old_value")]
        [DataMember]
        public string OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        [Column("new_value")]
        [DataMember]
        public string NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        [DataMember]
        public virtual GlpiTicket GlpiTicket { get; set; }
        #endregion
    }
}
