using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System;

namespace Emdep.Geos.Data.Common
{
    [Table("harness_parts")]
    [DataContract]
    public class HarnessPart
    {
        //#region Fields
        //Byte IdHarnessPart;
        //string Reference;
        //Int16? IdHarnessPartType;
        //Byte? IdColor;
        //Int32? Cavities;
        //SByte? gender;
        //SByte? isSealed;
        //decimal? internalDiameter;
        //decimal? externalDiameter;
        //decimal? thickness;
        //string description;
        //#endregion

        //#region Properties
        //[Key]
        //[Column("IdHarnessPartAccessoryType")]
        //[DataMember]
        //public Byte IdHarnessPartAccessoryType
        //{
        //    get { return idHarnessPartAccessoryType; }
        //    set { idHarnessPartAccessoryType = value; }
        //}

        //[Column("Name")]
        //[DataMember]
        //public string Name
        //{
        //    get { return name; }
        //    set { name = value; }
        //}

        //[Column("Description")]
        //[DataMember]
        //public string Description
        //{
        //    get { return description; }
        //    set { description = value; }
        //}

        //#endregion
    }
}
