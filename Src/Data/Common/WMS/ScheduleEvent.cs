using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Media;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;

namespace Emdep.Geos.Data.Common.WMS
{
    //[nsatpute][12.09.2025][GEOS2-8791]
    [DataContract]
    public class ScheduleEvent : ModelBase, IDisposable
    {
        #region Declaration

        private uint idWarehouseSchedule;
        private long idWarehouse;
        private int idSite;
        private DateTime creationDate;
        private int createdBy;
        private string createdByPerson;
        private DateTime modificationDate;
        private int modifiedBy;
        private string modifiedByPerson;
        private int idTypeEvent;
        private int idLogistic;
        private DateTime startDate;
        private DateTime endDate;
        private bool isDone;
        private string observations;
        private System.Windows.Media.SolidColorBrush buttonColor;
        private Visibility circleVisibility;
        private string logisticName;
        private int idCurrency;
        #endregion

        #region Constructor

        public ScheduleEvent()
        {
        }

        #endregion

        #region Properties
        [DataMember]
        public uint IdWarehouseSchedule
        {
            get { return idWarehouseSchedule; }
            set
            {
                idWarehouseSchedule = value;
                    OnPropertyChanged("IdWarehouseSchedule");             
            }
        }

        [DataMember]
        public long IdWarehouse
        {
            get { return idWarehouse; }
            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [DataMember]
        public Int32 IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }
        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public int CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [DataMember]
        public string CreatedByPerson
        {
            get { return createdByPerson; }
            set
            {
                createdByPerson = value;
                OnPropertyChanged("CreatedByPerson");
            }
        }
        [DataMember]
        public DateTime ModificationDate
        {
            get { return modificationDate; }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public int ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public string ModifiedByPerson
        {
            get { return modifiedByPerson; }
            set
            {
                modifiedByPerson = value;
                OnPropertyChanged("ModifiedByPerson");
            }
        }
        [DataMember]
        public int IdTypeEvent
        {
            get { return idTypeEvent; }
            set
            {
                idTypeEvent = value;
                OnPropertyChanged("IdTypeEvent");
            }
        }

        [DataMember]
        public int IdLogistic
        {
            get { return idLogistic; }
            set
            {
                idLogistic = value;
                OnPropertyChanged("IdLogistic");
            }
        }

        [DataMember]
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [DataMember]
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [DataMember]
        public bool IsDone
        {
            get { return isDone; }
            set
            {
                isDone = value;
                OnPropertyChanged("IsDone");
            }
        }

        [DataMember]
        public string Observations
        {
            get { return observations; }
            set
            {
                observations = value;
                OnPropertyChanged("Observations");
            }
        }

         [DataMember]
        public System.Windows.Media.SolidColorBrush ButtonColor
        {
            get { return buttonColor; }
            set
            {
                buttonColor = value;
                OnPropertyChanged("ButtonColor");
            }
        }
        [DataMember]
        public Visibility CircleVisibility
        {
            get { return circleVisibility; }
            set
            {
                circleVisibility = value;
                OnPropertyChanged("CircleVisibility");
            }
        }

        public string LogisticName
        {
            get { return logisticName; }
            set
            {
                logisticName = value;
                OnPropertyChanged("LogisticName");
            }
        }

        [DataMember]
        public int IdCurrency
        {
            get
            {
                return idCurrency;
            }

            set
            {
                idCurrency = value;
                OnPropertyChanged("IdCurrency");
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
