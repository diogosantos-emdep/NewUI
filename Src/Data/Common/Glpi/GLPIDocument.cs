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
    [Table("glpi_documents")]
    [DataContract]
    public class GlpiDocument
    {
        #region Fields
        Int32 id;
        Int32 entitiesId;
        bool isRecursive;
        Int32 documentCategoriesId;
        bool isDeleted;
        Int32 usersId;
        Int32 ticketsId;
        bool isBlacklisted;
        string name;
        string fileName;
        string filePath;
        string mime;
        DateTime? dateMod;
        string comment;
        string link;
        string sha1sum;
        string tag;
        #endregion

         #region Constructor
        public GlpiDocument()
        {
            this.GlpiDocumentItems = new List<GlpiDocumentItem>();
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

        [Column("is_recursive")]
        [DataMember]
        public bool IsRecursive
        {
            get { return isRecursive; }
            set { isRecursive = value; }
        }

        [Column("documentcategories_id")]
        [DataMember]
        public Int32 DocumentCategoriesId
        {
            get { return documentCategoriesId; }
            set { documentCategoriesId = value; }
        }

        [Column("is_deleted")]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        [Column("users_id")]
        [DataMember]
        public Int32 UserId
        {
            get { return usersId; }
            set { usersId = value; }
        }

        [Column("tickets_id")]
        [ForeignKey("GlpiTicket")]
        [DataMember]
        public Int32 TicketsId
        {
            get { return ticketsId; }
            set { ticketsId = value; }
        }

        [Column("is_blacklisted")]
        [DataMember]
        public bool IsBlacklisted
        {
            get { return isBlacklisted; }
            set { isBlacklisted = value; }
        }


        [Column("name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("filename")]
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        [Column("filepath")]
        [DataMember]
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        [Column("mime")]
        [DataMember]
        public string Mime
        {
            get { return mime; }
            set { mime = value; }
        }

        [Column("date_mod")]
        [DataMember]
        public DateTime? DateMod
        {
            get { return dateMod; }
            set { dateMod = value; }
        }

        [Column("comment")]
        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [Column("link")]
        [DataMember]
        public string Link
        {
            get { return link; }
            set { link = value; }
        }

        [Column("sha1sum")]
        [DataMember]
        public string Sha1sum
        {
            get { return sha1sum; }
            set { sha1sum = value; }
        }

        [Column("tag")]
        [DataMember]
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        

        [DataMember]
        public virtual GlpiTicket GlpiTicket { get; set; }

        [DataMember]
        public virtual List<GlpiDocumentItem> GlpiDocumentItems { get; set; }

        #endregion

    }
}
