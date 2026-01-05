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
    [Table("glpi_itilcategories")]
    [DataContract]
    public class GlpiItilCategory
    {
        #region Fields
        Int32 id;
        Int32? entitiesId;
        SByte? isRecursive;
        Int32? itilCategoriesId;
        string name;
        string completeName;
        string comment;
        Int32? level;
        Int32? knowbaseItemCategoriesId;
        Int32? usersId;
        Int32? groupsId;
        string ancestorsCache;
        string sonsCache;
        SByte? isHelpDeskVisible;
        Int32? ticketTemplatesIdIncident;
        Int32? ticketTemplatesIdDemand;
        Int32? isIncident;
        Int32? isRequest;
        Int32? isProblem;
        SByte? isChange;
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


        [Column("entities_id")]
        [DataMember]
        public Int32? EntitiesId
        {
            get { return entitiesId; }
            set { entitiesId = value; }
        }

        [Column("is_recursive")]
        [DataMember]
        public SByte? IsRecursive
        {
            get { return isRecursive; }
            set { isRecursive = value; }
        }

        [Column("itilcategories_id")]
        [DataMember]
        public Int32? ItilCategoriesId
        {
            get { return itilCategoriesId; }
            set { itilCategoriesId = value; }
        }

        [Column("level")]
        [DataMember]
        public Int32? Level
        {
            get { return level; }
            set { level = value; }
        }

        [Column("name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("completename")]
        [DataMember]
        public string CompleteName
        {
            get { return completeName; }
            set { completeName = value; }
        }

        [Column("comment")]
        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [Column("ancestors_cache")]
        [DataMember]
        public string AncestorsCache
        {
            get { return ancestorsCache; }
            set { ancestorsCache = value; }
        }

        [Column("sons_cache")]
        [DataMember]
        public string SonsCache
        {
            get { return sonsCache; }
            set { sonsCache = value; }
        }

        [Column("knowbaseitemcategories_id")]
        [DataMember]
        public Int32? KnowBaseItemCategoriesId
        {
            get { return knowbaseItemCategoriesId; }
            set { knowbaseItemCategoriesId = value; }
        }

        [Column("users_id")]
        [DataMember]
        public Int32? UsersId
        {
            get { return usersId; }
            set { usersId = value; }
        }

        [Column("groups_id")]
        [DataMember]
        public Int32? GroupsId
        {
            get { return groupsId; }
            set { groupsId = value; }
        }

        [Column("is_helpdeskvisible")]
        [DataMember]
        public SByte? IsHelpDeskVisible
        {
            get { return isHelpDeskVisible; }
            set { isHelpDeskVisible = value; }
        }

        [Column("tickettemplates_id_incident")]
        [DataMember]
        public Int32? TicketTemplatesIdIncident
        {
            get { return ticketTemplatesIdIncident; }
            set { ticketTemplatesIdIncident = value; }
        }

        [Column("tickettemplates_id_demand")]
        [DataMember]
        public Int32? TicketTemplatesIdDemand
        {
            get { return ticketTemplatesIdDemand; }
            set { ticketTemplatesIdDemand = value; }
        }

        [Column("is_incident")]
        [DataMember]
        public Int32? IsIncident
        {
            get { return isIncident; }
            set { isIncident = value; }
        }

        [Column("is_request")]
        [DataMember]
        public Int32? IsRequest
        {
            get { return isRequest; }
            set { isRequest = value; }
        }

        [Column("is_problem")]
        [DataMember]
        public Int32? IsProblem
        {
            get { return isProblem; }
            set { isProblem = value; }
        }

        [Column("is_change")]
        [DataMember]
        public SByte? IsChange
        {
            get { return isChange; }
            set { isChange = value; }
        }
        #endregion
    }
}
