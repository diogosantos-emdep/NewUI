using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;

namespace Emdep.Geos.Modules.Hrm.CommonClass
{
    /// <summary>
    /// [001][skale][31-12-2019][GEOS2-1831]Import attendance (all data sources) with night shifts values. [IES15]
    /// </summary>
    public class EmployeeAttandance : IDataErrorInfo
    {
        public string Employee { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public bool In { get; set; }
        public bool Out { get; set; }
        public LookupValue Type { get; set; }
        public string EmployeeClockTimeID { get; set; }
        private EmployeeAttandance pairedInRecord;

        public bool IsRowError {get;set;}
        //[001] added
        public Int32 idEmployee { get; set; }

        private CompanyShift companyShift;

        public CompanyShift CompanyShift {
            get { return companyShift; }
            set { companyShift = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShift"));
            }
        }

        public DateTime? AccountingDate { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        private ObservableCollection<CompanyShift> companyShiftList;

        public ObservableCollection<CompanyShift> CompanyShiftList
        {
            get { return companyShiftList; }
            set
            {
                companyShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShiftList"));
            }
        }

        #region Validation

        public bool allowValidation = false;

        public string EnableValidationAndGetError(EmployeeAttandance PairedInRecord = null)
        {
            pairedInRecord = PairedInRecord;

            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            OnPropertyChanged(new PropertyChangedEventArgs("CompanyShift"));
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }

            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error = me[BindableBase.GetPropertyName(() => CompanyShift)];
                
                if (!string.IsNullOrEmpty(error))
                    return System.Windows.Application.Current.FindResource("ReadImportedAttendanceValidationErrorCheckTheDataEntered").ToString();

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;

                string companyShiftProperty = BindableBase.GetPropertyName(() => CompanyShift);

                if (columnName == companyShiftProperty)
                {
                    if ((In == true || Out == true) &&
                            (CompanyShift == null || CompanyShift.IdCompanyShift == 0))
                    {
                            return System.Windows.Application.Current.FindResource("ReadImportedAttendanceValidationErrorShiftValueIsRequired").ToString();
                    }

                    if (pairedInRecord != null &&
                        pairedInRecord.CompanyShift != null &&
                        CompanyShift.IdCompanyShift != pairedInRecord.CompanyShift.IdCompanyShift)
                    {
                        return System.Windows.Application.Current.FindResource("ReadImportedAttendanceValidationErrorShiftValueMustBeSameForInOut").ToString();
                    }

                }

                return null;
            }
        }

        #endregion
    }
}
