using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class CompanyGrid : INotifyPropertyChanged, ICloneable
    {
        #region Fields
        private TransactionOperations transactionOperation;
        int idCompany;
        string customerName;
        string name;
        string countryName;
        string countryZoneName;
        string registeredName;
        string cif;
        string city;
        int? numberOfEmployees;
        double? size;
        string businessFieldValue;
        string businessProductString;
        string businessCenterValue;
        int? line;
        string region;
        string zipCode;
        string fax;
        string createdin;
        string modifiedIn;
        double age;
        string email;
        string website;
        string telephone;
        string peopleFullName;
        string peopleSalesBUFullName;
        int? cuttingmachines;
        string groupplantname;
        string peopleCreatedBy;
        #endregion

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; OnPropertyChanged("IdCompany"); }
        }
        [DataMember]
        public string CustomerName //Customer.CustomerName
        {
            get { return customerName; }
            set { customerName = value; OnPropertyChanged("CustomerName"); }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }
        [DataMember]
        public string CountryName //Country.Name
        {
            get { return countryName; }
            set { countryName = value; OnPropertyChanged("CountryName"); }
        }
        [DataMember]
        public string CountryZoneName //Country.Zone.Name
        {
            get { return countryZoneName; }
            set { countryZoneName = value; OnPropertyChanged("CountryZoneName"); }
        }
        [DataMember]
        public string RegisteredName
        {
            get { return registeredName; }
            set { registeredName = value; OnPropertyChanged("RegisteredName"); }
        }
        [DataMember]
        public string CIF
        {
            get { return cif; }
            set { cif = value; OnPropertyChanged("CIF"); }
        }
        [DataMember]
        public string City
        {
            get { return city; }
            set { city = value; OnPropertyChanged("City"); }
        }
        [DataMember]
        public int? NumberOfEmployees
        {
            get { return numberOfEmployees; }
            set { numberOfEmployees = value; OnPropertyChanged("NumberOfEmployees"); }
        }
        [DataMember]
        public double? Size
        {
            get { return size; }
            set { size = value; OnPropertyChanged("Size"); }
        }
        [DataMember]
        public string BusinessFieldValue //BusinessField.Value
        {
            get { return businessFieldValue; }
            set { businessFieldValue = value; OnPropertyChanged("BusinessFieldValue"); }
        }
        [DataMember]
        public string BusinessProductString
        {
            get { return businessProductString; }
            set { businessProductString = value; OnPropertyChanged("BusinessProductString"); }
        }
        [DataMember]
        public string BusinessCenterValue //BusinessCenter.Value
        {
            get { return businessCenterValue; }
            set { businessCenterValue = value; OnPropertyChanged("BusinessCenterValue"); }
        }
        [DataMember]
        public int? Line
        {
            get { return line; }
            set { line = value; OnPropertyChanged("Line"); }
        }
        [DataMember]
        public string Region
        {
            get { return region; }
            set { region = value; OnPropertyChanged("Region"); }
        }
        [DataMember]
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; OnPropertyChanged("ZipCode"); }
        }
        [DataMember]
        public string Fax
        {
            get { return fax; }
            set { fax = value; OnPropertyChanged("Fax"); }
        }
        [DataMember]
        public string CreatedIn
        {
            get { return createdin; }
            set { createdin = value; OnPropertyChanged("CreatedIn"); }
        }
        [DataMember]
        public string ModifiedIn
        {
            get { return modifiedIn; }
            set { modifiedIn = value; OnPropertyChanged("ModifiedIn"); }
        }
        [DataMember]
        public double Age
        {
            get { return age; }
            set { age = value; OnPropertyChanged("Age"); }
        }
        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("Email"); }
        }
        [DataMember]
        public string Website
        {
            get { return website; }
            set { website = value; OnPropertyChanged("Website"); }
        }
        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; OnPropertyChanged("Telephone"); }
        }
        [DataMember]
        public string PeopleFullName  //People.FullName
        {
            get { return peopleFullName; }
            set { peopleFullName = value; OnPropertyChanged("PeopleFullName"); }
        }
        [DataMember]
        public string PeopleSalesBUFullName  //PeopleSalesResponsibleAssemblyBU.FullName
        {
            get { return peopleSalesBUFullName; }
            set { peopleSalesBUFullName = value; OnPropertyChanged("PeopleSalesBUFullName"); }
        }
        [DataMember]
        public int? CuttingMachines  //PeopleSalesResponsibleAssemblyBU.FullName
        {
            get { return cuttingmachines; }
            set { cuttingmachines = value; OnPropertyChanged("PeopleSalesBUFullName"); }
        }
        [DataMember]
        public string GroupPlantName  //PeopleSalesResponsibleAssemblyBU.FullName
        {
            get { return groupplantname; }
            set { groupplantname = value; OnPropertyChanged("GroupPlantName"); }
        }
        [DataMember]
        public string PeopleCreatedBy  //PeopleCreatedBy.FullName
        {
            get { return peopleCreatedBy; }
            set { peopleCreatedBy = value; OnPropertyChanged("PeopleCreatedBy"); }
        }
        #endregion



        #region Methods

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        public enum TransactionOperations
        {
            Add,
            Modify,
            Update,
            Delete
        }


        [DataMember]
        public TransactionOperations TransactionOperation
        {
            get
            {
                return this.transactionOperation;
            }
            set
            {
                this.transactionOperation = value;
                this.OnPropertyChanged("TransactionOperation");
            }
        }
        #endregion
    }
}
