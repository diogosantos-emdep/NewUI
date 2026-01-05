using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConnectorProperties : ModelBase, IDisposable
    {
        #region Fields
        private string canSaveRecord;
        private int idConnectorProperty;
        private string propertyName;
        private int idConnectorCategory;
        private string categoryName;
        //[nsatpute][GEOS2-4501][20/07/2023]
        private bool isEnabled;
        private bool isReadOnly;
        private string defaultValue = string.Empty;
        private uint minValue;
        private uint maxValue;
        private string minValueNew = string.Empty;
        private string maxValueNew = string.Empty;
        private uint idFamily;
        private int idConnectorType;//rajashri[GEOS2-5227]
        private bool isChecked;//[Sudhir.Jangra][GEOS2-4501]
        private Visibility isParentCheckBoxVisible;//[Sudhir.Jangra][GEOS2-4501]
        string maxValueValidation;
        string minValueValidation;
        string customFamily;
        bool isCustomProperty;
        private string status;
        private ObservableCollection<LookUpValues> customFieldComboboxList;//[sudhir.jangra][GEos2-5374]
        private LookUpValues selectedcustomField;//[GEOS2-5374][Sudhir.jangra]
        #endregion

        #region Properties
        [DataMember]
        public int IdConnectorProperty
        {
            get { return idConnectorProperty; }
            set { idConnectorProperty = value; OnPropertyChanged("IdConnectorProperty"); }
        }
        [DataMember]
        public int IdConnectorType
        {
            get { return idConnectorType; }
            set { idConnectorType = value; OnPropertyChanged("IdConnectorType"); }
        }
        [DataMember]
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; OnPropertyChanged("PropertyName"); }
        }
        [DataMember]
        public int IdConnectorCategory
        {
            get { return idConnectorCategory; }
            set { idConnectorCategory = value; OnPropertyChanged("IdConnectorCategory"); }
        }
        [DataMember]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; OnPropertyChanged("CategoryName"); }
        }
        //[nsatpute][GEOS2-4501][20/07/2023]
        [DataMember]
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        [DataMember]
        public string DefaultValue
        {
            get
            {
                return defaultValue;
            }
            set
            {
                defaultValue = value;
                OnPropertyChanged("DefaultValue");
            }
        }
        [DataMember]
        public uint MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
            }
        }
        [DataMember]
        public uint MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }
        [DataMember]
        public uint IdFamily
        {
            get { return idFamily; }
            set { idFamily = value; OnPropertyChanged("IdFamily"); }
        }
        [DataMember]
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        [DataMember]
        public Visibility IsParentCheckBoxVisible
        {
            get { return isParentCheckBoxVisible; }
            set
            {
                isParentCheckBoxVisible = value;
                OnPropertyChanged("IsParentCheckBoxVisible");
            }
        }

        [DataMember]
        public string MinValueNew
        {
            get
            {
                return minValueNew;
            }
            set
            {
                minValueNew = value;
                OnPropertyChanged("MinValueNew");
            }
        }
        [DataMember]
        public string MaxValueNew
        {
            get
            {
                return maxValueNew;
            }
            set
            {
                maxValueNew = value;
                OnPropertyChanged("MaxValueNew");
            }
        }
        [DataMember]
        public string MinValueValidation
        {
            get
            {
                return minValueValidation;
            }
            set
            {
                minValueValidation = value;
                OnPropertyChanged("MinValueValidation");
            }
        }
        [DataMember]
        public string MaxValueValidation
        {
            get
            {
                return maxValueValidation;
            }
            set
            {
                maxValueValidation = value;
                OnPropertyChanged("MaxValueValidation");
            }
        }

        [DataMember]
        public string CustomFamily
        {
            get
            {
                return customFamily;
            }
            set
            {
                customFamily = value;
                OnPropertyChanged("CustomFamily");
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }


        public string CanSaveRecord
        {
            get { return canSaveRecord; }
            set { canSaveRecord = value; }
        }
        [DataMember]
        public bool IsCustomProperty
        {
            get { return isCustomProperty; }
            set { isCustomProperty = value; OnPropertyChanged("IsCustomProperty"); }
        }

        //[Sudhir.Jangra][GEOS2-5374]
        [DataMember]
        public ObservableCollection<LookUpValues> CustomFieldComboboxList
        {
            get { return customFieldComboboxList; }
            set
            {
                customFieldComboboxList = value;
                OnPropertyChanged("CustomFieldComboboxList");
            }
        }

        //[Sudhir.Jangra][GEOS2-5374]
        [DataMember]
        public LookUpValues SelectedcustomField
        {
            get { return selectedcustomField; }
            set
            {
                selectedcustomField = value;
                OnPropertyChanged("SelectedcustomField");
            }
        }

        [DataMember]
        public bool IsReadOnly
        {
            get { return !IsEnabled; }
            set { }          
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
