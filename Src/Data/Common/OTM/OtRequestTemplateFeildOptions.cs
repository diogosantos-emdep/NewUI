using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class OtRequestTemplateFeildOptions : ModelBase, IDisposable
    {
        #region Fields

        int idOTRequestTemplateFieldOption;
        int idOTRequestTemplate;
        int idField;
        int idFieldType;
        int createdBy;
        DateTime createdIn;
        int? updatedBy;
        DateTime updatedIn;
        public OtRequestTemplateCellFields otRequestTemplateCellField;
        public OtRequestTemplateTextFields otRequestTemplateTextField;
        public OtRequestTemplateLocationFields otRequestTemplateLocationField;

        #endregion

        #region Properties
        public bool changingfield = false;
        public bool ChangingField
        {
            get { return changingfield; }
            set
            {
                changingfield = value;
                OnPropertyChanged("ChangingField");
            }
        }
        [DataMember]
        public OtRequestTemplateTextFields OtRequestTemplateTextField
        {
            get { return otRequestTemplateTextField; }
            set
            {
                otRequestTemplateTextField = value;
                OnPropertyChanged("OtRequestTemplateTextField");
            }
        }

        [DataMember]
        public OtRequestTemplateLocationFields OtRequestTemplateLocationField
        {
            get { return otRequestTemplateLocationField; }
            set
            {
                otRequestTemplateLocationField = value;
                OnPropertyChanged("OtRequestTemplateLocationField");
            }
        }

        [DataMember]

        public OtRequestTemplateCellFields OtRequestTemplateCellField
        {
            get { return otRequestTemplateCellField; }
            set
            {
                otRequestTemplateCellField = value;
                OnPropertyChanged("otRequestTemplateCellField");
            }
        }

        [DataMember]
        public int IdOTRequestTemplateFieldOption
        {
            get { return idOTRequestTemplateFieldOption; }
            set { idOTRequestTemplateFieldOption = value; OnPropertyChanged("IdOTRequestTemplateFieldOption"); }
        }

        [DataMember]
        public int IdOTRequestTemplate
        {
            get { return idOTRequestTemplate; }
            set { idOTRequestTemplate = value; OnPropertyChanged("IdOTRequestTemplate"); }
        }

        [DataMember]
        public int IdField
        {
            get { return idField; }
            set { idField = value; OnPropertyChanged("IdField"); }
        }

        [DataMember]
        public int IdFieldType
        {
            get { return idFieldType; }
            set { idFieldType = value; OnPropertyChanged("IdFieldType"); }
        }

        [DataMember]
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; OnPropertyChanged("CreatedBy"); }
        }

        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set { createdIn = value; OnPropertyChanged("CreatedIn"); }
        }

        [DataMember]
        public int? UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; OnPropertyChanged("UpdatedBy"); }
        }

        [DataMember]
        public DateTime UpdatedIn
        {
            get { return updatedIn; }
            set { updatedIn = value; OnPropertyChanged("UpdatedIn"); }
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
