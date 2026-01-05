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
    [Table("glpi_locations")]
    [DataContract]
    public class GlpiLocation
    {
        #region Fields
        Int32 id;
        Int32? entitiesId;
        SByte? isRecursive;
        Int32? locationsId;
        Int32? level;
        string name;
        string completeName;
        string comment;
        string ancestorsCache;
        string sonsCache;
        string building;
        string room;
        string latitude;
        string longitude;
        string altitude;
      
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

        [Column("locations_id")]
        [DataMember]
        public Int32? LocationsId
        {
            get { return locationsId; }
            set { locationsId = value; }
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

        [Column("building")]
        [DataMember]
        public string Building
        {
            get { return building; }
            set { building = value; }
        }

        [Column("room")]
        [DataMember]
        public string Room
        {
            get { return room; }
            set { room = value; }
        }

        [Column("latitude")]
        [DataMember]
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [Column("longitude")]
        [DataMember]
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        [Column("altitude")]
        [DataMember]
        public string Altitude
        {
            get { return altitude; }
            set { altitude = value; }
        }

        #endregion
    }
}
