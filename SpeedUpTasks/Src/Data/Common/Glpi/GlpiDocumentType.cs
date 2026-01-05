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
    [Table("glpi_documenttypes")]
    [DataContract]
    public class GlpiDocumentType
    {
        #region Fields
        int id;
        SByte? isUploadable;
        string name;
        string ext;
        string icon;
        string mime;
        DateTime? dateMod;
        string comment;
        #endregion

        #region Properties

        [Key]
        [Column("id")]
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Column("is_uploadable")]
        [DataMember]
        public SByte? IsUploadable
        {
            get { return isUploadable; }
            set { isUploadable = value; }
        }

        [Column("name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("ext")]
        [DataMember]
        public string Ext
        {
            get { return ext; }
            set { ext = value; }
        }

        [Column("icon")]
        [DataMember]
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
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

        #endregion
    }
}
