using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System;

namespace Emdep.Geos.Data.Common.HarnessPart
{
    [Table("harness_parts")]
    [DataContract]
    public class HarnessPart
    {
        #region Fields
        Int32 idHarnessPart;
        string reference;
        Int16 idHarnessPartType;
        Byte? idColor;
        Int32? cavities;
        Genders? gender;
        Boolean? isSealed;
        Decimal? internalDiameter;
        Decimal? externalDiameter;
        Decimal? thickness;
        string description;
        Int32? idPartner;
        Int32? idCreator;
        DateTime? creationDate;
        Int32? idReplacement;
        Int32? idSite;
        Boolean? isDeleted; 
        #endregion

         #region Constructor
        public HarnessPart()
        {
            this.HarnessPartAccessories = new List<HarnessPartAccessory>();
        }
        #endregion

        #region Properties
       
        [Key]
        [Column("IdHarnessPart")]
        [DataMember]
        public Int32 IdHarnessPart
        {
            get { return idHarnessPart; }
            set { idHarnessPart = value; }
        }

        [Column("Reference")]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        [Column("IdHarnessPartType")]
        [DataMember]
        public Int16 IdHarnessPartType
        {
            get { return idHarnessPartType; }
            set { idHarnessPartType = value; }
        }

        [Column("IdColor")]
        [ForeignKey("Color")]
        [DataMember]
        public Byte? IdColor
        {
            get { return idColor; }
            set { idColor = value; }
        }

        [Column("Cavities")]
        [DataMember]
        public Int32? Cavities
        {
            get { return cavities; }
            set { cavities = value; }
        }

        [Column("Gender")]
        [DataMember]
        public Genders? Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        [Column("IsSealed")]
        [DataMember]
        public Boolean? IsSealed
        {
            get { return isSealed; }
            set { isSealed = value; }
        }

        [Column("InternalDiameter")]
        [DataMember]
        public Decimal? InternalDiameter
        {
            get { return internalDiameter; }
            set { internalDiameter = value; }
        }

        [Column("ExternalDiameter")]
        [DataMember]
        public Decimal? ExternalDiameter
        {
            get { return externalDiameter; }
            set { externalDiameter = value; }
        }

        [Column("Thickness")]
        [DataMember]
        public Decimal? Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [Column("IdPartner")]
        [DataMember]
        public Int32? IdPartner
        {
            get { return idPartner; }
            set { idPartner = value; }
        }

        [Column("IdCreator")]
        [DataMember]
        public Int32? IdCreator
        {
            get { return idCreator; }
            set { idCreator = value; }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [Column("IdReplacement")]
        [DataMember]
        public Int32? IdReplacement
        {
            get { return idReplacement; }
            set { idReplacement = value; }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32? IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        [Column("IsDeleted")]
        [DataMember]
        public Boolean? IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        [DataMember]
        public virtual Color Color { get; set; }

        [DataMember]
        public virtual List<HarnessPartAccessory> HarnessPartAccessories { get; set; }
        #endregion
    }
}
