using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Emdep.Geos.Data.Common.OTM
{
    //[nsatpute][19-02-2025][GEOS2-6722]
    public class POData : ModelBase, IDisposable
    {
        #region Fields
        private string customer;
        private string invoiceTO;
        private string poNumber;
        private string offers;
        private string contact;
        private string transferAmount;
        private string currency;
        private string incoterm;
        private string shipTo;
        private string dateIssued;
        private string poPaymentTerms;
        #endregion
        #region Constructor
        public POData()
        {
        }
        #endregion
        #region Properties
        [DataMember]
        public string Customer
        {
            get => customer;
            set
            {
                customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }
        [DataMember]
        public string InvoiceTO
        {
            get => invoiceTO;
            set
            {
                invoiceTO = value;
                OnPropertyChanged(nameof(InvoiceTO));
            }
        }
        [DataMember]
        public string DateIssued
        {
            get => dateIssued;
            set
            {
                dateIssued = value;
                OnPropertyChanged(nameof(DateIssued));
            }
        }
        [DataMember]
        public string PONumber
        {
            get => poNumber;
            set
            {
                poNumber = value;
                OnPropertyChanged(nameof(PONumber));
            }
        }
        [DataMember]
        public string Offers
        {
            get => offers;
            set
            {
                offers = value;
                OnPropertyChanged(nameof(Offers));
            }
        }
        [DataMember]
        public string Contact
        {
            get => contact;
            set
            {
                contact = value;
                OnPropertyChanged(nameof(Contact));
            }
        }
        [DataMember]
        public string TransferAmount
        {
            get => transferAmount;
            set
            {
                transferAmount = value;
                OnPropertyChanged(nameof(TransferAmount));
            }
        }
        [DataMember]
        public string Currency
        {
            get => currency;
            set
            {
                currency = value;
                OnPropertyChanged(nameof(Currency));
            }
        }
        [DataMember]
        public string Incoterm
        {
            get => incoterm;
            set
            {
                incoterm = value;
                OnPropertyChanged(nameof(Incoterm));
            }
        }
        [DataMember]
        public string ShipTo
        {
            get => shipTo;
            set
            {
                shipTo = value;
                OnPropertyChanged(nameof(ShipTo));
            }
        }
        [DataMember]
        public string POPaymentTerm
        {
            get { return poPaymentTerms; }
            set { poPaymentTerms = value; OnPropertyChanged(nameof(POPaymentTerm)); }
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