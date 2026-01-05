using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class LocationFileTemplateValue : ModelBase, IDisposable
    {
        #region Fields
        long idOTRequestTemplateField;
        int idOTRequestTemplate;
        int idfield;
        string fieldValue;
        string coordinates;
        int idfieldType;
        string fieldTypeValue;
        #endregion

        #region Constructor
        public LocationFileTemplateValue()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public long IdOTRequestTemplateField
        {
            get
            {
                return idOTRequestTemplateField;
            }

            set
            {
                idOTRequestTemplateField = value;
                OnPropertyChanged("IdOTRequestTemplateField");
            }
        }

        [DataMember]
        public int IdOTRequestTemplate
        {
            get
            {
                return idOTRequestTemplate;
            }

            set
            {
                idOTRequestTemplate = value;
                OnPropertyChanged("IdOTRequestTemplate");
            }
        }

        [DataMember]
        public int Idfield
        {
            get
            {
                return idfield;
            }

            set
            {
                idfield = value;
                OnPropertyChanged("Idfield");
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
        public string Coordinates
        {
            get
            {
                return coordinates;
            }

            set
            {
                coordinates = value;
                OnPropertyChanged("Coordinates");
            }
        }
        [DataMember]
        public int IdfieldType
        {
            get
            {
                return idfieldType;
            }

            set
            {
                idfieldType = value;
                OnPropertyChanged("IdfieldType");
            }
        }
        [DataMember]
        public string FieldTypeValue
        {
            get { return fieldTypeValue; }
            set { fieldTypeValue = value; OnPropertyChanged("FieldTypeValue"); }
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
