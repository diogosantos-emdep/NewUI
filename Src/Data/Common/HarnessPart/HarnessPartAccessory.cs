using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.HarnessPart
{
    [Table("harness_part_accessories")]
    [DataContract]
    public class HarnessPartAccessory
    {
        #region Fields
        Int32 idHarnessPartAccessory;
        Int32? idHarnessPart;
        Byte? idAccessoryType;
        Byte? idColor;
        string reference;
        Int32? idCreator;
        DateTime? createdIn;
        Int32? idModifier;
        DateTime? modifiedIn;
        Int32? idSite;
        Byte? isApproved;
        Int32? idApprover;
        #endregion

        #region Properties
        [Key]
        [Column("idHarnessPartAccessory")]
        [DataMember]
        public Int32 IdHarnessPartAccessory
        {
            get { return idHarnessPartAccessory; }
            set { idHarnessPartAccessory = value; }
        }

        [Column("idHarnessPart")]
        [ForeignKey("Harness_Part")]
        [DataMember]
        public int? IdHarnessPart
        {
            get { return idHarnessPart; }
            set { idHarnessPart = value; }
        }

        [Column("idAccessoryType")]
        [ForeignKey("HarnessPartAccessoryType")]
        [DataMember]
        public Byte? IdAccessoryType
        {
            get { return idAccessoryType; }
            set { idAccessoryType = value; }
        }

        [Column("idColor")]
        [ForeignKey("Color")]
        [DataMember]
        public Byte? IdColor
        {
            get { return idColor; }
            set { idColor = value; }
        }

        [Column("reference")]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        [Column("idCreator")]
        [DataMember]
        public Int32? IdCreator
        {
            get { return idCreator; }
            set { idCreator = value; }
        }

        [Column("createdIn")]
        [DataMember]
        public DateTime? CreatedIn
        {
            get { return createdIn; }
            set { createdIn = value; }
        }

        [Column("idModifier")]
        [DataMember]
        public Int32? IdModifier
        {
            get { return idModifier; }
            set { idModifier = value; }
        }

        [Column("modifiedIn")]
        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set { modifiedIn = value; }
        }

        [Column("idSite")]
        [DataMember]
        public Int32? IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        [Column("isApproved")]
        [DataMember]
        public Byte? IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        [Column("idApprover")]
        [DataMember]
        public Int32? IdApprover
        {
            get { return idApprover; }
            set { idApprover = value; }
        }

        [DataMember]
        public virtual Color Color { get; set; }

        [DataMember]
        public virtual HarnessPartAccessoryType HarnessPartAccessoryType { get; set; }

        [DataMember]
        public virtual HarnessPart Harness_Part { get; set; }
        #endregion
    }
}
