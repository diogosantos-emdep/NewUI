using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Crm
{
    [DataContract]
    public class ImportAccounts : ModelBase, IDisposable
    {

        #region Fields
        List<String> columnName;
        string column;
        Dictionary<Int32, string> fieldValue;
        List<String> targetFieldList;

        List<CustomerAccountImportField> customerAccountImportFieldList;
        CustomerAccountImportField customerAccountImportField;
        List<CustomerAccountImportField> customerAccountTargetFieldList;

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
        public List<CustomerAccountImportField> CustomerAccountImportFieldList
        {
            get { return customerAccountImportFieldList; }
            set
            {
                customerAccountImportFieldList = value;
                OnPropertyChanged("CustomerAccountImportFieldList");
            }
        }

        [NotMapped]
        [DataMember]
        public CustomerAccountImportField CustomerAccountImportField
        {
            get { return customerAccountImportField; }
            set
            {
                customerAccountImportField = value;
                OnPropertyChanged("CustomerAccountImportField");
            }
        }


        [NotMapped]
        [DataMember]
        public List<CustomerAccountImportField> CustomerAccountTargetFieldList
        {
            get { return customerAccountTargetFieldList; }
            set
            {
                customerAccountTargetFieldList = value;
                OnPropertyChanged("CustomerAccountTargetFieldList");
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
        #endregion
    }
}
