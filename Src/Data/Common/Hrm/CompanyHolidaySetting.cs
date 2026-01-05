using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class CompanyHolidaySetting : ModelBase, IDisposable
    {
        #region Fields
        Int64 idHolidaySetting;
        Int32 idValue;
        Int32? modifiedBy;
        DateTime? modifiedIn;
        Company company;
        LookupValue holidaySetting;
        List<LookupValue> holidaySettingList;
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdHolidaySetting
        {
            get { return idHolidaySetting; }
            set
            {
                idHolidaySetting = value;
                OnPropertyChanged("IdHolidaySetting");
            }
        }

        [DataMember]
        public Int32 IdValue
        {
            get { return idValue; }
            set
            {
                idValue = value;
                OnPropertyChanged("IdValue");
            }
        }

        [DataMember]
        public Int32? ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public Company Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [DataMember]
        public LookupValue HolidaySetting
        {
            get { return holidaySetting; }
            set
            {
                holidaySetting = value;
                OnPropertyChanged("HolidaySetting");
            }
        }
  
        [DataMember]
        public List<LookupValue> HolidaySettingList
        {
            get { return holidaySettingList; }
            set
            {
                holidaySettingList = value;
                OnPropertyChanged("HolidaySettingList");
            }
        }

        List<LogEntriesByCompanyLeaves> changeLog;
        [DataMember]
        public List<LogEntriesByCompanyLeaves> ChangeLog
        {
            get { return changeLog; }
            set { changeLog = value; OnPropertyChanged("ChangeLog"); }
        }
        #endregion

        #region Constructor

        public CompanyHolidaySetting()
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
