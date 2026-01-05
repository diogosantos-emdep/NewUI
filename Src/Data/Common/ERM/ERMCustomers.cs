using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    
    public class ERMCustomers : ModelBase, IDisposable
    {
        #region Fields
        private string deliveryWeek;
        private Int32 idCustomer;
        private string customerName;
        private string workOrder;
        private Int32 idProject;
        private string projectName;
        #endregion

        #region Properties 

        [DataMember]
        public string DeliveryWeek
        {
            get { return deliveryWeek; }
            set
            {
                deliveryWeek = value;
                OnPropertyChanged("DeliveryWeek");
            }
        }

        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }
        [DataMember]
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }

        [DataMember]
        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged("WorkOrder");
            }
        }

        [DataMember]
        public Int32 IdProject
        {
            get { return idProject; }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }

        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        #endregion

        #region Constructor
        public ERMCustomers()
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


    
    public class ERMCustomerPlant : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idOfferSite;
        private string offerSiteName;
        private Int32 idCustomer;
        private string workOrder;
        private Int32 idProject;
        private string projectName;

        #endregion

        #region Properties 
        [DataMember]
        public Int32 IdOfferSite
        {
            get { return idOfferSite; }
            set
            {
                idOfferSite = value;
                OnPropertyChanged("IdOfferSite");
            }
        }
        [DataMember]
        public string OfferSiteName
        {
            get { return offerSiteName; }
            set
            {
                offerSiteName = value;
                OnPropertyChanged("OfferSiteName");
            }
        }

        [DataMember]        public Int32 IdCustomer        {            get { return idCustomer; }            set            {                idCustomer = value;                OnPropertyChanged("IdCustomer");            }        }

        [DataMember]
        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged("WorkOrder");
            }
        }

        [DataMember]
        public Int32 IdProject
        {
            get { return idProject; }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }

        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        #endregion

        #region Constructor
        public ERMCustomerPlant()
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


    public class ERMProject : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idProject;
        private string projectName;


        #endregion

        #region Properties 
        [DataMember]
        public Int32 IdProject
        {
            get { return idProject; }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }
        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        #endregion

        #region Constructor
        public ERMProject()
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

    public class ERMWorkOrder : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idProject;
        private string projectName;


        #endregion

        #region Properties 
        [DataMember]
        public Int32 IdProject
        {
            get { return idProject; }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }
        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        #endregion

        #region Constructor
        public ERMWorkOrder()
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

    public class ERMRTMFilters : ModelBase, IDisposable
    {
        #region Fields
        private string deliveryWeek;
        private Int32 idCustomer;
        private string customer;
        private Int32 idOfferSite;
        private string offerSiteName;
        private string workOrder;
        private Int32 idProject;
        private string project;
        

        #endregion

        #region Properties 

        [DataMember]
        public string DeliveryWeek
        {
            get { return deliveryWeek; }
            set
            {
                deliveryWeek = value;
                OnPropertyChanged("DeliveryWeek");
            }
        }

        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }
        [DataMember]
        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [DataMember]
        public Int32 IdOfferSite
        {
            get { return idOfferSite; }
            set
            {
                idOfferSite = value;
                OnPropertyChanged("IdOfferSite");
            }
        }
        [DataMember]
        public string OfferSiteName
        {
            get { return offerSiteName; }
            set
            {
                offerSiteName = value;
                OnPropertyChanged("OfferSiteName");
            }
        }

        [DataMember]
        public string WorkOrder
        {
            get { return workOrder; }
            set
            {
                workOrder = value;
                OnPropertyChanged("WorkOrder");
            }
        }

      

        [DataMember]
        public Int32 IdProject
        {
            get { return idProject; }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }

        [DataMember]
        public string Project
        {
            get { return project; }
            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }


        #endregion

        #region Constructor
        public ERMRTMFilters()
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
