using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("language_dependent_entries")]
    [DataContract]
    public class LanguageDependentEntry
    {
        #region Fields
        Int32 idLanguageDependentEntry;
        Int16? idTable;
        string columnName;
        Int32? idLanguage;
        string _value;
        Int32? idRecord;
        #endregion

        #region Properties
        [Key]
        [Column("IdLanguageDependentEntry")]
        [DataMember]
        public Int32 IdLanguageDependentEntry
        {
            get { return idLanguageDependentEntry; }
            set { idLanguageDependentEntry = value; }
        }

        [Column("idTable")]
        [DataMember]
        public Int16? IdTable
        {
            get { return idTable; }
            set { idTable = value; }
        }

        [Column("ColumnName")]
        [DataMember]
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        [Column("IdLanguage")]
        [DataMember]
        public Int32? IdLanguage
        {
            get { return idLanguage; }
            set { idLanguage = value; }
        }

        [Column("Value")]
        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [Column("IdRecord")]
        [DataMember]
        public Int32? IdRecord
        {
            get { return idRecord; }
            set { idRecord = value; }
        }
        #endregion
    }
}
