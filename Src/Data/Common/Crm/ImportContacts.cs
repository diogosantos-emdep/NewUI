using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Crm
{
	// [nsatpute] [GEOS2-5702][28-06-2024] Add new import accounts/contacts option (2/2)
    public class ImportContacts : ModelBase, IDisposable
    {
        #region Fields
        List<String> columnName;
        string column;
        Dictionary<Int32, string> fieldValue;
        List<String> targetFieldList;

        List<CustomerContactImportField> customerContactImportFieldList;
        CustomerContactImportField customerContactImportField;
        List<CustomerContactImportField> customerContactTargetFieldList;

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
        public List<CustomerContactImportField> CustomerContactImportFieldList
        {
            get { return customerContactImportFieldList; }
            set
            {
                customerContactImportFieldList = value;
                OnPropertyChanged("CustomerContactImportField");
            }
        }

        [NotMapped]
        [DataMember]
        public CustomerContactImportField CustomerContactImportField
        {
            get { return customerContactImportField; }
            set
            {
                customerContactImportField = value;
                OnPropertyChanged("CustomerContactImportField");
            }
        }


        [NotMapped]
        [DataMember]
        public List<CustomerContactImportField> CustomerContactTargetFieldList
        {
            get { return customerContactTargetFieldList; }
            set
            {
                customerContactTargetFieldList = value;
                OnPropertyChanged("CustomerContactTargetFieldList");
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
