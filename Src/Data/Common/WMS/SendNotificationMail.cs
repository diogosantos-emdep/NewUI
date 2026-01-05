using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    [DataContract]
    public class SendNotificationMail : ModelBase, IDisposable
    {

        #region Declaration
        Int32 idJobDescription;
        string employeeContactValue;
        string jobDescriptionTitle;
        string warehouse;

        #endregion

        #region Properties
        [DataMember]
        public Int32 IdJobDescription
        {
            get
            {
                return idJobDescription;
            }

            set
            {
                idJobDescription = value;
            }
        }
       
        [DataMember]
        public string EmployeeContactValue
        {
            get
            {
                return employeeContactValue;
            }

            set
            {
                employeeContactValue = value;
                OnPropertyChanged("EmployeeContactValue");
            }
        }

        [DataMember]
        public string JobDescriptionTitle
        {
            get
            {
                return jobDescriptionTitle;
            }

            set
            {
                jobDescriptionTitle = value;
                OnPropertyChanged("JobDescriptionTitle");
            }
        }
        [DataMember]
        public string Warehouse
        {
            get
            {
                return warehouse;
            }

            set
            {
                warehouse = value;
                OnPropertyChanged("Warehouse");
            }
        }

        #endregion

        #region Constructor

        public SendNotificationMail()
        {
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
