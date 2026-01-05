using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{

 //[pramod.misal] [GEOS2-5379] [29.03.2024]
    [DataContract]
    public class ConnectorReference: ModelBase, IDisposable
    {
        #region Fields
        private string reference;
        private string oldReference;
        private Int32 idCustomer;
        private string creatorName;
        private Int32 creatorId;
        bool isDelVisible;
        private string creatorSurname;
        private string creatorSitename;
        private DateTime createdIn;
        private string modifierName;
        private string modifierSurname;
        private string modifierSitename;
        private DateTime modifiedIn;
        private UInt32 debugged;
        private string customerName;
        object selectedCompany;
        List<Company> companyList;
        private Company company;
        #endregion

        #region Properties
        [DataMember]
        public string OldReference
        {
            get { return oldReference; }
            set { oldReference = value; OnPropertyChanged("OldReference"); }
        }
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; OnPropertyChanged("Reference"); }
        }

        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; OnPropertyChanged("IdCustomer"); }
        }

        [DataMember]
        public string CreatorName
        {
            get { return creatorName; }
            set { creatorName = value; OnPropertyChanged("CreatorName"); }
        }

        [DataMember]
        public Int32 CreatorId
        {
            get { return creatorId; }
            set { creatorId = value; OnPropertyChanged("CreatorId"); }
        }

        [DataMember]
        public string CreatorSurname
        {
            get { return creatorSurname; }
            set { creatorSurname = value; OnPropertyChanged("CreatorSurname"); }
        }

        [DataMember]
        public string CreatorSitename
        {
            get { return creatorSitename; }
            set { creatorSitename = value; OnPropertyChanged("CreatorSitename"); }
        }


        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set { createdIn = value; OnPropertyChanged("CreatedIn"); }
        }

        [DataMember]
        public string ModifierName
        {
            get { return modifierName; }
            set { modifierName = value; OnPropertyChanged("ModifierName"); }
        }

        [DataMember]
        public string ModifierSurname
        {
            get { return modifierSurname; }
            set { modifierSurname = value; OnPropertyChanged("ModifierSurname"); }
        }

        [DataMember]
        public string ModifierSitename
        {
            get { return modifierSitename; }
            set { modifierSitename = value; OnPropertyChanged("ModifierSitename"); }
        }


        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set { modifiedIn = value; OnPropertyChanged("ModifiedIn"); }
        }

        [DataMember]
        public UInt32 Debugged
        {
            get { return debugged; }
            set { debugged = value; OnPropertyChanged("Debugged"); }
        }

        [DataMember]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; OnPropertyChanged("CustomerName"); }
        }

        [DataMember]
        public object SelectedCompany
        {
            get { return selectedCompany; }
            set { selectedCompany = value; OnPropertyChanged("SelectedCompany"); }
        }


        [DataMember]
        public List<Company> CompanyList
        {
            get { return companyList; }
            set { companyList = value; OnPropertyChanged("CompanyList"); }
        }

        [DataMember]
        public Company Company
        {
            get { return company; }
            set { company = value; OnPropertyChanged("Company"); }
        }

        [DataMember]
        public bool IsDelVisible
        {
            get { return isDelVisible; }
            set { isDelVisible = value; OnPropertyChanged("IsDelVisible"); }
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }



    }
}
