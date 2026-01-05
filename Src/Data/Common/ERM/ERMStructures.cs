using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMStructures : ModelBase, IDisposable
    {
        #region Constructor

        #endregion

        #region fields

        private string name;
        private UInt32 idCPType;
        private string description;
        private string reference;
        private UInt32 orderNumber;
        private string nameToShow;
        private string code;
        private Int32 isEnabled;
        private Int32 idStatus;
        private string status;
        private string woremarks;
        private string remarks;
        private DateTime? lastUpdate;
        private bool isLastUpdate;
        #endregion

        #region Properties
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

        public UInt32 IdCPType
        {
            get { return idCPType;  }
            set { idCPType = value;
                OnPropertyChanged("IdCPType");
            }

        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }

        }
        
        public UInt32 OrderNumber
        {

            get { return orderNumber; }
            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        public string NameToShow
        {
            get { return nameToShow; }
            set
            {
                nameToShow = value;
                OnPropertyChanged("NameToShow");
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }

        }
        
        public Int32 IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        
        public Int32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }
        
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

        public string WORemarks
        {

            get { return woremarks; }
            set
            {

                woremarks = value;
                OnPropertyChanged("WORemarks");
            }

        }
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
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

