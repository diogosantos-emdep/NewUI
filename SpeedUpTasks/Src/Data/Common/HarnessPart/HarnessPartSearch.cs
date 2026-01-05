using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common.HarnessPart
{
     [DataContract]
    public class HarnessPartSearch
    {
        #region Fields
        int? idEnterpriseGroupReference;
        string reference;
        Int16? idHarnessPartType;
        int? idColor;
        Int32? cavitiesFrom;
        Int32? cavitiesTo;
        Genders? gender;
        SByte isSealed;
        decimal? internalDiameterFrom;
        decimal? internalDiameterTo;
        decimal? externalDiameterFrom;
        decimal? externalDiameterTo;
        decimal? thicknessFrom;
        decimal? thicknessTo;
        string accessoryName;
        SByte? isCondition;
        Byte? idDocumentType;
        int? idEnterpriseGroupDocument;
        string sortName;
        List<HarnessPartAccessory> harnessPartAccessory;
        #endregion

        #region Properties
       
        [NotMapped]
        [DataMember]
        public int? IdEnterpriseGroupReference
        {
            get { return idEnterpriseGroupReference; }
            set { idEnterpriseGroupReference = value; }
        }

        [NotMapped]
        [DataMember]
        public Int16? IdHarnessPartType
        {
            get { return idHarnessPartType; }
            set { idHarnessPartType = value; }
        }

        [NotMapped]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; }
        }

         [NotMapped]
         [DataMember]
         public int? IdColor
         {
             get { return idColor; }
             set { idColor = value; }
         }

         [NotMapped]
         [DataMember]
         public Int32? CavitiesFrom
         {
             get { return cavitiesFrom; }
             set { cavitiesFrom = value; }
         }

         [NotMapped]
         [DataMember]
         public Int32? CavitiesTo
         {
             get { return cavitiesTo; }
             set { cavitiesTo = value; }
         }

         [NotMapped]
         [DataMember]
         public Genders? Gender
         {
             get { return gender; }
             set { gender = value; }
         }

         [NotMapped]
        [DataMember]
        public sbyte IsSealed
        {
            get { return isSealed; }
            set { isSealed = value; }
        }

         [NotMapped]
        [DataMember]
        public decimal? InternalDiameterFrom
        {
            get { return internalDiameterFrom; }
            set { internalDiameterFrom = value; }
        }

         [NotMapped]
         [DataMember]
         public decimal? InternalDiameterTo
         {
             get { return internalDiameterTo; }
             set { internalDiameterTo = value; }
         }

         [NotMapped]
        [DataMember]
        public decimal? ExternalDiameterFrom
        {
            get { return externalDiameterFrom; }
            set { externalDiameterFrom = value; }
        }

         [NotMapped]
         [DataMember]
         public decimal? ExternalDiameterTo
         {
             get { return externalDiameterTo; }
             set { externalDiameterTo = value; }
         }

        [NotMapped]
        [DataMember]
        public decimal? ThicknessFrom
        {
            get { return thicknessFrom; }
            set { thicknessFrom = value; }
        }

        [NotMapped]
        [DataMember]
        public decimal? ThicknessTo
        {
            get { return thicknessTo; }
            set { thicknessTo = value; }
        }

        [NotMapped]
        [DataMember]
        public string AccessoryName
        {
            get { return accessoryName; }
            set { accessoryName = value; }
        }

        [NotMapped]
        [DataMember]
        public SByte? IsCondition
        {
            get { return isCondition; }
            set { isCondition = value; }
        }

        [NotMapped]
        [DataMember]
        public Byte? IdDocumentType
        {
            get { return idDocumentType; }
            set { idDocumentType = value; }
        }

        [NotMapped]
        [DataMember]
        public int? IdEnterpriseGroupDocument
        {
            get { return idEnterpriseGroupDocument; }
            set {idEnterpriseGroupDocument = value; }
        }

        [NotMapped]
        [DataMember]
        public string SortName
        {
            get { return sortName; }
            set { sortName = value; }
        }

        [NotMapped]
        [DataMember]
        public List<HarnessPartAccessory> HarnessPartAccessories
        {
            get { return harnessPartAccessory; }
            set { harnessPartAccessory = value; }
        }
        #endregion
     }
}
