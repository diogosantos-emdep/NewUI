using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [pramod.misal][GEOS2-6735][23.01.2025]
    /// </summary>
    public class ExcelFileTemplateValue : ModelBase, IDisposable
    {

        #region Fields
        long idOTRequestTemplateTextField;
        Int64 idOTRequestTemplateFieldOption;
        Int64 idFieldType;
        string fieldValue;
        string fieldTypevalue;
        string range;
        string keyword;
        string delimiter;
        #endregion

        #region Properties
        [DataMember]
        public long IdOTRequestTemplateTextField
        {
            get
            {
                return idOTRequestTemplateTextField;
            }

            set
            {
                idOTRequestTemplateTextField = value;
                OnPropertyChanged("IdOTRequestTemplateTextField");
            }
        }


        [DataMember]
        public Int64 IdOTRequestTemplateFieldOption
        {
            get
            {
                return idOTRequestTemplateFieldOption;
            }

            set
            {
                idOTRequestTemplateFieldOption = value;
                OnPropertyChanged("IdOTRequestTemplateFieldOption");
            }
        }

        [DataMember]
        public Int64 IdFieldType
        {
            get
            {
                return idFieldType;
            }

            set
            {
                idFieldType = value;
                OnPropertyChanged("IdFieldType");
            }
        }


        [DataMember]
        public string FieldValue
        {
            get
            {
                return fieldValue;
            }

            set
            {
                fieldValue = value;
                OnPropertyChanged("FieldValue");
            }
        }

        [DataMember]
        public string FieldTypevalue
        {
            get
            {
                return fieldTypevalue;
            }

            set
            {
                fieldTypevalue = value;
                OnPropertyChanged("FieldTypevalue");
            }
        }

        [DataMember]
        public string Range
        {
            get
            {
                return range;
            }

            set
            {
                range = value;
                OnPropertyChanged("Range");
            }
        }

        [DataMember]
        public string Keyword
        {
            get
            {
                return keyword;
            }

            set
            {
                keyword = value;
                OnPropertyChanged("Keyword");
            }
        }

        [DataMember]
        public string Delimiter
        {
            get
            {
                return delimiter;
            }

            set
            {
                delimiter = value;
                OnPropertyChanged("Delimiter");
            }
        }

        #endregion

        #region Constructor
        public ExcelFileTemplateValue()
        {

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
