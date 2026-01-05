using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class ParallelPort : ModelBase, IDisposable
    {
        #region Declaration

        Int64 availability;
        string caption;
        Int64 configManagerErrorCode;
        bool configManagerUserConfig;
        string creationClassName;
        string description;
        string deviceID;
        bool dmaSupport;
        string name;
        bool osAutoDiscovered;
        string pnpDeviceID;
        bool powerManagementSupported;
        Int64 protocolSupported;
        string status;
        string systemCreationClassName;
        string systemName;

        #endregion

        #region Properties

        public long Availability
        {
            get { return availability; }
            set
            {
                availability = value;
                OnPropertyChanged("Availability");
            }
        }

        public string Caption
        {
            get { return caption; }
            set
            {
                caption = value;
                OnPropertyChanged("Caption");
            }
        }

        public long ConfigManagerErrorCode
        {
            get { return configManagerErrorCode; }
            set
            {
                configManagerErrorCode = value;
                OnPropertyChanged("ConfigManagerErrorCode");
            }
        }

        public bool ConfigManagerUserConfig
        {
            get { return configManagerUserConfig; }
            set
            {
                configManagerUserConfig = value;
                OnPropertyChanged("ConfigManagerUserConfig");
            }
        }

        public string CreationClassName
        {
            get { return creationClassName; }
            set
            {
                creationClassName = value;
                OnPropertyChanged("CreationClassName");
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

        public string DeviceID
        {
            get { return deviceID; }
            set
            {
                deviceID = value;
                OnPropertyChanged("DeviceID");
            }
        }

        public bool DMASupport
        {
            get { return dmaSupport; }
            set
            {
                dmaSupport = value;
                OnPropertyChanged("DMASupport");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool OSAutoDiscovered
        {
            get { return osAutoDiscovered; }
            set
            {
                osAutoDiscovered = value;
                OnPropertyChanged("OSAutoDiscovered");
            }
        }

        public string PNPDeviceID
        {
            get { return pnpDeviceID; }
            set
            {
                pnpDeviceID = value;
                OnPropertyChanged("PNPDeviceID");
            }
        }

        public bool PowerManagementSupported
        {
            get { return powerManagementSupported; }
            set
            {
                powerManagementSupported = value;
                OnPropertyChanged("PowerManagementSupported");
            }
        }

        public long ProtocolSupported
        {
            get { return protocolSupported; }
            set
            {
                protocolSupported = value;
                OnPropertyChanged("ProtocolSupported");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public string SystemCreationClassName
        {
            get { return systemCreationClassName; }
            set
            {
                systemCreationClassName = value;
                OnPropertyChanged("SystemCreationClassName");
            }
        }

        public string SystemName
        {
            get { return systemName; }
            set
            {
                systemName = value;
                OnPropertyChanged("SystemName");
            }
        }

        #endregion

        #region Constructor

        public ParallelPort()
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
