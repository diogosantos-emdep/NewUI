using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
namespace Emdep.Geos.Data.Common
{
    [Serializable]
    [DataContract]
    public class ActivityGrid : INotifyPropertyChanged, ICloneable
    {
        #region Fields
        private TransactionOperations transactionOperation;
        int idsite;
        long idactivity;
        int idowner;
        string subject;
        int idactivitytype;
        byte? iscompleted;
        string activitytype;
        long? idimageactivitytype;
        DateTime? fromdate;
        DateTime? todate;
        int idactivitystatus;
        string activitygridstatus;
        long idimageactivitygridstatus;
        DateTime? closedate;
        string fullname;
        string login;
        string customername;
        string companyname;
        string activityattendeesstring;
        string activitytagsstring;
        string description;
        string location;
        string sitenamewithoutcountry;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        [DataMember]
        public int IdSite
        {
            get { return idsite; }
            set { idsite = value; OnPropertyChanged("IdSite"); }
        }
        [DataMember]
        public long IdActivity
        {
            get { return idactivity; }
            set { idactivity = value; OnPropertyChanged("IdActivity"); }
        }
        [DataMember]
        public int IdOwner
        {
            get { return idowner; }
            set { idowner = value; OnPropertyChanged("IdOwner"); }
        }
        [DataMember]
        public string Subject
        {
            get { return subject; }
            set { subject = value; OnPropertyChanged("Subject"); }
        }
        [DataMember]
        public int IdActivityType
        {
            get { return idactivitytype; }
            set { idactivitytype = value; OnPropertyChanged("IdActivityType"); }
        }
        [DataMember]
        public byte? IsCompleted
        {
            get { return iscompleted; }
            set { iscompleted = value; OnPropertyChanged("IsCompleted"); }
        }
        [DataMember]
        public string ActivityType
        {
            get { return activitytype; }
            set { activitytype = value; OnPropertyChanged("ActivityType"); }
        }
        [DataMember]
        public long? IdImageActivityType
        {
            get { return idimageactivitytype; }
            set { idimageactivitytype = value; OnPropertyChanged("IdImageActivityType"); }
        }
        [DataMember]
        public DateTime? FromDate
        {
            get { return fromdate; }
            set { fromdate = value; OnPropertyChanged("FromDate"); }
        }
        [DataMember]
        public DateTime? ToDate
        {
            get { return todate; }
            set { todate = value; OnPropertyChanged("ToDate"); }
        }
        [DataMember]
        public int IdActivityStatus
        {
            get { return idactivitystatus; }
            set { idactivitystatus = value; OnPropertyChanged("IdActivityStatus"); }
        }
        [DataMember]
        public string ActivityGridStatus
        {
            get { return activitygridstatus; }
            set { activitygridstatus = value; OnPropertyChanged("ActivityGridStatus"); }
        }
        [DataMember]
        public long IdImageActivityGridStatus
        {
            get { return idimageactivitygridstatus; }
            set { idimageactivitygridstatus = value; OnPropertyChanged("IdImageActivityGridStatus"); }
        }
        [DataMember]
        public DateTime? CloseDate
        {
            get { return closedate; }
            set { closedate = value; OnPropertyChanged("CloseDate"); }
        }
        [DataMember]
        public string FullName
        {
            get { return fullname; }
            set { fullname = value; OnPropertyChanged("FullName"); }
        }
        [DataMember]
        public string Login
        {
            get { return login; }
            set { login = value; OnPropertyChanged("Login"); }
        }
        [DataMember]
        public string CustomerName
        {
            get { return customername; }
            set { customername = value; OnPropertyChanged("CustomerName"); }
        }
        [DataMember]
        public string CompanyName
        {
            get { return companyname; }
            set { companyname = value; OnPropertyChanged("CompanyName"); }
        }
        [DataMember]
        public string ActivityAttendeesString
        {
            get { return activityattendeesstring; }
            set { activityattendeesstring = value; OnPropertyChanged("ActivityAttendeesString"); }
        }
        [DataMember]
        public string ActivityTagsString
        {
            get { return activitytagsstring; }
            set { activitytagsstring = value; OnPropertyChanged("ActivityTagsString"); }
        }
        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }
        [DataMember]
        public string Location
        {
            get { return location; }
            set { location = value; OnPropertyChanged("Location"); }
        }
        [DataMember]
        public string SiteNameWithoutCountry
        {
            get { return sitenamewithoutcountry; }
            set { sitenamewithoutcountry = value; OnPropertyChanged("SiteNameWithoutCountry"); }
        }
        #endregion

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
    }
}
