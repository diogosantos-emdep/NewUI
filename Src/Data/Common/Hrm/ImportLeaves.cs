using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
   public class ImportLeaves : ModelBase, IDisposable
    {
        #region Fields

        List<String> columnName;
        string column;
        Dictionary<Int32, string> fieldValue;
        List<EmployeeLeavesImportField> employeeLeavesImportFieldList;
        EmployeeLeavesImportField employeeLeavesImportField;
        List<EmployeeLeavesImportField> employeeLeavesTargetFieldList;
        List<String> targetFieldList;
        #endregion

        #region Constructor
        public ImportLeaves()
        {
        }

        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public List<String> ColumnName
        {
            get { return columnName; }
            set
            {
                columnName = value;
                OnPropertyChanged("ColumnName");
            }
        }

        [NotMapped]
        [DataMember]
        public Dictionary<Int32, string> FieldValue
        {
            get { return fieldValue; }
            set
            {
                fieldValue = value;
                OnPropertyChanged("FieldValue");
            }
        }

        [NotMapped]
        [DataMember]
        public string Column
        {
            get { return column; }
            set
            {
                column = value;
                OnPropertyChanged("Column");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EmployeeLeavesImportField> EmployeeLeavesImportFieldList
        {
            get { return employeeLeavesImportFieldList; }
            set
            {
                employeeLeavesImportFieldList = value;
                OnPropertyChanged("EmployeeLeavesImportFieldList");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeLeavesImportField EmployeeLeavesImportField
        {
            get { return employeeLeavesImportField; }
            set
            {
                employeeLeavesImportField = value;
                OnPropertyChanged("EmployeeLeavesImportField");
            }
        }


        [NotMapped]
        [DataMember]
        public List<EmployeeLeavesImportField> EmployeeLeavesTargetFieldList
        {
            get { return employeeLeavesTargetFieldList; }
            set
            {
                employeeLeavesTargetFieldList = value;
                OnPropertyChanged("EmployeeLeavesTargetFieldList");
            }
        }


        [NotMapped]
        [DataMember]
        public List<String> TargetFieldList
        {
            get { return targetFieldList; }
            set
            {
                targetFieldList = value;
                OnPropertyChanged("TargetFieldList");
            }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }


        #endregion
    }
}
