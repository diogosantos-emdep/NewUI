using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
	// [nsatpute][08-11-2024] HRM - Improve ERF . GEOS2-6475
    public class EmployeeDepartmentSituation : ModelBase, IDisposable
    {
        #region Fields
        private string company;
        private float activeCadEmployees;
        private float totalActiveCadEmployees;
        private float activeEmployees;
        private float cadAverageYearsService;
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }

        [NotMapped]
        [DataMember]
        public float ActiveCadEmployees
        {
            get { return activeCadEmployees; }
            set
            {
                activeCadEmployees = value;
                OnPropertyChanged("ActiveCadEmployees");
            }
        }

        [NotMapped]
        [DataMember]
        public float TotalActiveCadEmployees
        {
            get { return totalActiveCadEmployees; }
            set
            {
                totalActiveCadEmployees = value;
                OnPropertyChanged("TotalActiveCadEmployees");
            }
        }

        [NotMapped]
        [DataMember]
        public float ActiveEmployees
        {
            get { return activeEmployees; }
            set
            {
                activeEmployees = value;
                OnPropertyChanged("ActiveEmployees");
            }
        }

        [NotMapped]
        [DataMember]
        public float CadAverageYearsService
        {
            get { return cadAverageYearsService; }
            set
            {
                cadAverageYearsService = value;
                OnPropertyChanged("CadAverageYearsService");
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
