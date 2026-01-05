using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMEmployeeJobDescription : ModelBase, IDisposable
    {
        #region Field
        private Int32 idEmployee;
        private Int32 idCompany;
        //private Int32 idSite;
        private Int32 idJobDescription;
        private decimal jobDescriptionUsage;
        private DateTime? jobDescriptionStartDate;
        private DateTime? endDate;

       
        #endregion
        #region Property
        [DataMember]
        public Int32 IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }
        // [NotMapped]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }
        [DataMember]
        public Int32 IdJobDescription
        {
            get { return idJobDescription; }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
            }
        }

        [DataMember]
        public decimal JobDescriptionUsage
        {
            get
            {
                return jobDescriptionUsage;
            }

            set
            {
                jobDescriptionUsage = value;
                OnPropertyChanged("JobDescriptionUsage");
            }
        }
        [DataMember]
        public DateTime? JobDescriptionStartDate
        {
            get
            {
                return jobDescriptionStartDate;
            }

            set
            {
                jobDescriptionStartDate = value;
                OnPropertyChanged("JobDescriptionStartDate");
            }
        }
        [DataMember]
        public DateTime? EndDate
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
        #region Constructor
        public ERMEmployeeJobDescription()
        {

        }
        #endregion
    }
}
