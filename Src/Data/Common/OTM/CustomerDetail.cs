using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class CustomerDetail : ModelBase, IDisposable
    {
        #region declare
        private string pONumber = null;
        private string currency = null;
        private double totalNetValue=0;
        private DateTime dateIssued;
        private string email = null;
        private string offer = null;
        private string customer = null;
        private string pdfName = null;
        string incoterm = string.Empty;
        string shipTo;
        string invoiceTo;
        string invoiceAddress;
        double percentage;
        int idPORequest;

        int idincoterm;
        string incotermname = string.Empty;

        int idpaymenttype;
        string paymenttypename = string.Empty;
        private string fileText;
        private long idAttachment;
        string paymentterms = string.Empty;

        #endregion

        #region Properties
        [DataMember]
        public double Percentage
        {
            get
            {
                return percentage;
            }
            set
            {

                percentage = value;
                OnPropertyChanged("Percentage");
            }
        }
        [DataMember]
        public string Incoterm
        {
            get
            {
                return incoterm;
            }
            set
            {

                incoterm = value;
                OnPropertyChanged("Incoterm");
            }
        }
        [DataMember]
        public string PONumber
        {
            get
            {
                return pONumber;
            }
            set
            {
                
                pONumber = value;
                OnPropertyChanged("PONumber");
            }
        }
        [DataMember]
        public string Currency
        {
            get
            {
                return currency;
            }
            set
            {
               
                currency = value;
                OnPropertyChanged("Currency");
            }
        }
        [DataMember]
        public double TotalNetValue
        {
            get
            {
                return totalNetValue;
            }
            set
            {
                totalNetValue = value;
                OnPropertyChanged("TotalNetValue");
            }
        }
        [DataMember]
        public DateTime DateIssued
        {
            get
            {
                return dateIssued;
            }
            set
            {
                dateIssued = value;
                OnPropertyChanged("DateIssued");
            }
        }
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
        public string Offer
        {
            get
            {
                return offer;
            }
            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }
        [DataMember]
        public string Customer
        {
            get
            {
                return customer;
            }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [DataMember]
        public string ShipTo
        {
            get
            {
                return shipTo;
            }
            set
            {
                shipTo = value;
                OnPropertyChanged("ShipTo");
            }
        }
        [DataMember]
        public string InvoiceTo
        {
            get
            {
                return invoiceTo;
            }
            set
            {
                invoiceTo = value;
                OnPropertyChanged("InvoiceTo");
            }
        }
        [DataMember]
        public string InvoiceAddress
        {
            get
            {
                return invoiceAddress;
            }
            set
            {
                invoiceAddress = value;
                OnPropertyChanged("InvoiceAddress");
            }
        }

        [DataMember]
        public int IdPORequest
        {
            get
            {
                return idPORequest;
            }
            set
            {

                idPORequest = value;
                OnPropertyChanged("IdPORequest");
            }
        }
        //[RGadhave][12.11.2024][GEOS2-6462]
        [DataMember]
        public string IncotermName
        {
            get
            {
                return incotermname;
            }
            set
            {

                incotermname = value;
                OnPropertyChanged("IncotermName");
            }
        }
        [DataMember]
        public string PaymentTypeName
        {
            get
            {
                return paymenttypename;
            }
            set
            {

                paymenttypename = value;
                OnPropertyChanged("PaymentTypeName");
            }
        }




        [DataMember]
        public int IdIncoterm
        {
            get
            {
                return idincoterm;
            }
            set
            {

                idincoterm = value;
                OnPropertyChanged("IdIncoterm");
            }
        }


        [DataMember]
        public int IdPaymentType
        {
            get
            {
                return idpaymenttype;
            }
            set
            {

                idpaymenttype = value;
                OnPropertyChanged("IdPaymentType");
            }
        }
        //[rahul.gadhave][GEOS2-6799][Date:10/02/2025]
        [DataMember]
        public string FileText
        {
            get { return fileText; }
            set
            {
                fileText = value;
                OnPropertyChanged("FileText");
            }
        }
        [DataMember]
        public long IdAttachment
        {
            get { return idAttachment; }
            set
            {
                idAttachment = value;
                OnPropertyChanged("IdAttachment");
            }
        }
        //END
        [DataMember]
        public string PaymentTerms
        {
            get { return paymentterms; }
            set { paymentterms = value; OnPropertyChanged("PaymentTerms"); }
        }
        #endregion

        #region Constructor
        public CustomerDetail()
        {

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
