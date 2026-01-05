using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
    public class EmployeeAttendanceImportField : ModelBase, IDisposable
    {
        #region Fields
        string name;
        DateTime startDate;
        DateTime endDate;
        Int32 selectedFieldIndex;
        string selectedFieldName;
        #endregion

        #region Constructor
        public EmployeeAttendanceImportField()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 SelectedFieldIndex
        {
            get { return selectedFieldIndex; }
            set
            {
                selectedFieldIndex = value;
                OnPropertyChanged("SelectedFieldIndex");
            }
        }

        [NotMapped]
        [DataMember]
        public string SelectedFieldName
        {
            get { return selectedFieldName; }
            set
            {
                selectedFieldName = value;
                OnPropertyChanged("SelectedFieldName");
            }
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
