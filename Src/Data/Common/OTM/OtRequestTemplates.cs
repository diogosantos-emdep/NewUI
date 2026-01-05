using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Emdep.Geos.Data.Common;

namespace Emdep.Geos.Data.Common.OTM
{
    public  class OtRequestTemplates : ModelBase, IDisposable
    {
        #region Fields
        string code;
        string templateName;
        string group;
        string region;
        string country;
        string plant;
        string createdby;
        private DateTime? createdat;
        string updatedby;
        private DateTime? updatedat;
        string file;
        string fileLocation;
        string fileextension;
        string action;
        private Int32 inuse;
        private Int32 idOTRequestTemplate;
        byte[] fileDocInBytes;

        int idOtRequestTemplate;
        int idGroup;
        int idRegion;
        int idCountry;
        int idPlant;
        int idcreatedby;
        int idupdatedby;
        public OtRequestTemplateFeildOptions otRequestTemplateFeildOption;
        public OtRequestTemplateTextFields otRequestTemplateTextField;
        public OtRequestTemplateLocationFields otRequestTemplateLocationField;
        public OtRequestTemplateCellFields otRequestTemplateCellField;

        public ObservableCollection <OtRequestTemplateFeildOptions> otRequestTemplateFeildOptions;
        public ObservableCollection <OtRequestTemplateTextFields> otRequestTemplateTextFields;
        public ObservableCollection <OtRequestTemplateLocationFields> otRequestTemplateLocationFields;
        public ObservableCollection <OtRequestTemplateCellFields> otRequestTemplateCellFields;
        Visibility isVisiblePOReq;
        string header;
        Int32 idCustomer;
        private int idCustomerPlant;
        public bool changingfield = false;
        #endregion

        #region Constructor
        public OtRequestTemplates()
        {

        }
        #endregion
        #region Properties
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
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [DataMember]
        public ObservableCollection <OtRequestTemplateFeildOptions> OtRequestTemplateFeildOptions
        {
            get { return otRequestTemplateFeildOptions; }
            set
            {
                otRequestTemplateFeildOptions = value;
                OnPropertyChanged("OtRequestTemplateFeildOptions");
            }
        }
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }
        public Visibility ISVisiblePOReq
        {
            get { return isVisiblePOReq; }
            set
            {
                isVisiblePOReq = value;
                OnPropertyChanged("ISVisiblePOReq");
            }
        }

        [DataMember]
        public ObservableCollection <OtRequestTemplateTextFields> OtRequestTemplateTextFields
        {
            get { return otRequestTemplateTextFields; }
            set
            {
                otRequestTemplateTextFields = value;
                OnPropertyChanged("OtRequestTemplateTextFields");
            }
        }

        [DataMember]
        public ObservableCollection <OtRequestTemplateLocationFields> OtRequestTemplateLocationFields
        {
            get { return otRequestTemplateLocationFields; }
            set
            {
                otRequestTemplateLocationFields = value;
                OnPropertyChanged("OtRequestTemplateLocationFields");
            }
        }

        [DataMember]
        public ObservableCollection<OtRequestTemplateCellFields> OtRequestTemplateCellFields
        {
            get { return otRequestTemplateCellFields; }
            set
            {
                otRequestTemplateCellFields = value;
                OnPropertyChanged("otRequestTemplateCellFields");
            }
        }


        [DataMember]
        public OtRequestTemplateFeildOptions OtRequestTemplateFeildOption
        {
            get { return otRequestTemplateFeildOption; }
            set
            {
                otRequestTemplateFeildOption = value;
                OnPropertyChanged("OtRequestTemplateFeildOption");
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
        public int IdOtRequestTemplate
        {
            get { return idOtRequestTemplate; }
            set
            {
                idOtRequestTemplate = value;
                OnPropertyChanged("IdOtRequestTemplate");
            }
        }

        [DataMember]
        public int IdGroup
        {
            get { return idGroup; }
            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public int IdRegion
        {
            get { return idRegion; }
            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [DataMember]
        public int IdCountry
        {
            get { return idCountry; }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }

        public int IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {

                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
            }
        }
        [DataMember]
        public int IdPlant
        {
            get { return idPlant; }
            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
            }
        }

        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        [DataMember]
        public string TemplateName
        {
            get { return templateName; }
            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }
        [DataMember]
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }
        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }
        [DataMember]
        public string CreatedBy
        {
            get { return createdby; }
            set
            {
                createdby = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public int IdCreatedBy
        {
            get { return idcreatedby; }
            set
            {
                idcreatedby = value;
                OnPropertyChanged("IdCreatedBy");
            }
        }
        [DataMember]
        public int IdUpdatedBy
        {
            get { return idupdatedby; }
            set
            {
                idupdatedby = value;
                OnPropertyChanged("IdUpdatedBy");
            }
        }
        [DataMember]
        public DateTime? CreatedAt
        {
            get { return createdat; }
            set
            {
                createdat = value;
                OnPropertyChanged("CreatedAt");
            }
        }
        [DataMember]
        public string UpdatedBy
        {
            get { return updatedby; }
            set
            {
                updatedby = value;
                OnPropertyChanged("UpdatedBy");
            }
        }
        [DataMember]
        public DateTime? UpdatedAt
        {
            get { return updatedat; }
            set
            {
                updatedat = value;
                OnPropertyChanged("UpdatedAt");
            }
        }
        [DataMember]
        public string File
        {
            get { return file; }
            set
            {
                file = value;
                OnPropertyChanged("File");
            }
        }

        [DataMember]
        public string FileLocation
        {
            get { return fileLocation; }
            set
            {
                fileLocation = value;
                OnPropertyChanged("FileLocation");
            }
        }
        [DataMember]
        public string Action
        {
            get { return action; }
            set
            {
                action = value;
                OnPropertyChanged("Action");
            }
        }
        [DataMember]
        public Int32 InUse
        {
            get { return inuse; }
            set
            {
                inuse = value;
                OnPropertyChanged("InUse");

            }
        }
        [DataMember]
        public Int32 IdOTRequestTemplate
        {
            get { return idOTRequestTemplate; }
            set
            {
                idOTRequestTemplate = value;
                OnPropertyChanged("IdOTRequestTemplate");

            }
        }
        public byte[] FileDocInBytes
        {
            get
            {
                return fileDocInBytes;
            }

            set
            {
                fileDocInBytes = value;
                OnPropertyChanged("FileDocInBytes");
            }
        }
        [DataMember]
        public string fileExtension
        {
            get { return fileextension; }
            set
            {
                fileextension = value;
                OnPropertyChanged("fileExtension");
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
