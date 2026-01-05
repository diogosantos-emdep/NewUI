using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [001][ashish.malkhede][03102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
    /// </summary>
    public class POEmployeeInfo : ModelBase, IDisposable
    {


        #region Fields
        Int64 idEmployee;
        Int64 idJobDescription;
        string employeeCode;
        string fullName;
        string jobDescriptionTitle;
        string email;







        #endregion
        #region Constructor
        public POEmployeeInfo()
        {

        }
        #endregion
        #region Properties


        [DataMember]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }


        [DataMember]
        public Int64 IdJobDescription
        {
            get
            {
                return idJobDescription;
            }
            set
            {
                idJobDescription = value;
                OnPropertyChanged("IdJobDescription");
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
        public Int64 IdEmployee
        {
            get
            {
                return idEmployee;
            }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }


        [DataMember]
        public string EmployeeCode
        {
            get
            {
                return employeeCode;
            }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }


        [DataMember]
        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
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
