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
    [DataContract]
    public class ImportAttendance : ModelBase, IDisposable
    {
        #region Fields

        List<String> columnName;
        string column;
        Dictionary<Int32, string> fieldValue;
        List<EmployeeAttendanceImportField> employeeAttendanceImportFieldList;
        EmployeeAttendanceImportField employeeAttendanceImportField;
        List<EmployeeAttendanceImportField> employeeAttendanceTargetFieldList;
  		List<String> targetFieldList;
        #endregion

        #region Constructor
        public ImportAttendance()
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
        public List<EmployeeAttendanceImportField> EmployeeAttendanceImportFieldList
        {
            get { return employeeAttendanceImportFieldList; }
            set
            {
                employeeAttendanceImportFieldList = value;
                OnPropertyChanged("EmployeeAttendanceImportFieldList");
            }
        }

        [NotMapped]
        [DataMember]
        public EmployeeAttendanceImportField EmployeeAttendanceImportField
        {
            get { return employeeAttendanceImportField; }
            set
            {
                employeeAttendanceImportField = value;
                OnPropertyChanged("EmployeeAttendanceImportField");
            }
        }


        [NotMapped]
        [DataMember]
        public List<EmployeeAttendanceImportField> EmployeeAttendanceTargetFieldList
        {
            get { return employeeAttendanceTargetFieldList; }
            set
            {
                employeeAttendanceTargetFieldList = value;
                OnPropertyChanged("EmployeeAttendanceTargetFieldList");
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
