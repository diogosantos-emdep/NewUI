using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMOptions : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idDetection;
        private string name;
        Int32 position;
        private string nameToShow;
        Int32 family;
        private Int32 weldOrder;
        string code;
        string orientation;
        private Int32 idDetectionType;
        string description;
        private Int32 idGroup;
        private Int32 isEnabled;
        private Int32 idStatus;
        private string status;
        private DateTime? lastUpdate;
        private bool isLastUpdate; //[Rupali Sarode][GEOS2-4355][23-05-2023]
        #endregion

        #region Properties 
        [DataMember]
        public Int32 IdDetection
        {
            get { return idDetection; }
            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
            }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public Int32 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [DataMember]
        public string NameToShow
        {
            get { return nameToShow; }
            set
            {
                nameToShow = value;
                OnPropertyChanged("NameToShow");
            }
        }

        [DataMember]
        public Int32 Family
        {
            get { return family; }
            set
            {
                family = value;
                OnPropertyChanged("Family");
            }
        }
        [DataMember]
        public Int32 WeldOrder
        {
            get { return weldOrder; }
            set
            {
                weldOrder = value;
                OnPropertyChanged("WeldOrder");
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
        public string Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                OnPropertyChanged("Orientation");
            }
        }
        [DataMember]
        public Int32 IdDetectionType
        {
            get { return idDetectionType; }
            set
            {
                idDetectionType = value;
                OnPropertyChanged("IdDetectionType");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        [DataMember]
        public Int32 IdGroup
        {
            get { return idGroup; }
            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }
        [DataMember]
        public Int32 IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        [DataMember]
        public Int32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
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

        [DataMember]
        public DateTime? LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        //[Rupali Sarode][GEOS2-4355][23-05-2023]
        [DataMember]
        public bool IsLastUpdate
        {
            get { return isLastUpdate; }
            set
            {
                isLastUpdate = value;
                OnPropertyChanged("IsLastUpdate");
            }
        }
        #endregion


        #region Constructor

        #endregion


        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
