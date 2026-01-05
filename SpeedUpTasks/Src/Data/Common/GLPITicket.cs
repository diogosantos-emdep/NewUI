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
    [Table("glpi_tickets")]
    [DataContract]
    public class GLPITicket
    {
         #region Fields
        int id;
        Int32 entitiesId;
        string name;
        DateTime? date;
        DateTime? closeDate;
        DateTime? solveDate;
        DateTime? dateMod;
        Int32 usersIdLastupdater;
        byte status;
        Int32 usersIdRecipient;
        Int32 requestTypesId;
        string content;
        Int32 urgency;
        Int32 impact;
        Int32 priority;
        Int32 itilcategoriesId;
        Int32 type;
        Int32 solutionTypesId;
        string solution;
        Int32 globalValidation;
        Int32 slasId;
        Int32 slalevelsId;
        DateTime? dueDate;
        DateTime? beginWaitingDate;
        Int32 locationsId;
        Int32 others;
        int? validationPercent;
        byte? isDeleted;
        int? actionTime;
        int? takeintoaccountDelayStat;
        int? solveDelayStat;
        int? closeDelayStat;
        int? waitingDuration;
        int? slaWaitingDuration;
        #endregion

        #region Constructor
        public GLPITicket()
        {
            this.GlpiDocuments = new List<GLPIDocument>();
        }
        #endregion

        #region Properties

        [Key]
        [Column("id")]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id
        {
            get { return id; }
            set { id = value; }
        }

        
        [Column("entities_id")]
        [DataMember]
        public Int32 EntitiesId
        {
            get { return entitiesId; }
            set { entitiesId = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("date")]
        [DataMember]
        public DateTime? Date
        {
            get { return date; }
            set { date = value; }
        }

        
        [Column("closedate")]
        [DataMember]
        public DateTime? CloseDate
        {
            get { return closeDate; }
            set { closeDate = value; }
        }

        [Column("solvedate")]
        [DataMember]
        public DateTime? SolveDate
        {
            get { return solveDate; }
            set { solveDate = value; }
        }

        [Column("date_mod")]
        [DataMember]
        public DateTime? DateMod
        {
            get { return dateMod; }
            set { dateMod = value; }
        }

        [Column("users_id_lastupdater")]
        [DataMember]
        public Int32 UsersIdLastupdater
        {
            get { return usersIdLastupdater; }
            set { usersIdLastupdater = value; }
        }

        [Column("status")]
        [DataMember]
        public byte Status
        {
            get { return status; }
            set { status = value; }
        }

        [Column("users_id_recipient")]
        [DataMember]
        public Int32 UsersIdRecipient
        {
            get { return usersIdRecipient; }
            set { usersIdRecipient = value; }
        }


        [Column("requesttypes_id")]
        [DataMember]
        public Int32 RequestTypesId
        {
            get { return requestTypesId; }
            set { requestTypesId = value; }
        }

        [Column("content")]
        [DataMember]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        [Column("urgency")]
        [DataMember]
        public Int32 Urgency
        {
            get { return urgency; }
            set { urgency = value; }
        }

        [Column("impact")]
        [DataMember]
        public Int32 Impact
        {
            get { return impact; }
            set { impact = value; }
        }

        [Column("priority")]
        [DataMember]
        public Int32 Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        [Column("itilcategories_id")]
        [DataMember]
        public Int32 ItilcategoriesId
        {
            get { return itilcategoriesId; }
            set { itilcategoriesId = value; }
        }

        [Column("type")]
        [DataMember]
        public Int32 Type
        {
            get { return type; }
            set { type = value; }
        }

        [Column("solutiontypes_id")]
        [DataMember]
        public Int32 SolutiontypesId
        {
            get { return solutionTypesId; }
            set { solutionTypesId = value; }
        }

        [Column("solution")]
        [DataMember]
        public string Solution
        {
            get { return solution; }
            set { solution = value; }
        }

        [Column("global_validation")]
        [DataMember]
        public Int32 GlobalValidation
        {
            get { return globalValidation; }
            set { globalValidation = value; }
        }

        [Column("slas_id")]
        [DataMember]
        public Int32 SlasId
        {
            get { return slasId; }
            set { slasId = value; }
        }

        [Column("slalevels_id")]
        [DataMember]
        public Int32 SlalevelsId
        {
            get { return slalevelsId; }
            set { slalevelsId = value; }
        }

        [Column("due_date")]
        [DataMember]
        public DateTime? DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        [Column("begin_waiting_date")]
        [DataMember]
        public DateTime? BeginWaitingDate
        {
            get { return beginWaitingDate; }
            set { beginWaitingDate = value; }
        }

        [Column("locations_id")]
        [DataMember]
        public Int32 LocationsId
        {
            get { return locationsId; }
            set { locationsId = value; }
        }

        [DataMember]
        public virtual List<GLPIDocument> GlpiDocuments { get; set; }
        #endregion
    }
}
