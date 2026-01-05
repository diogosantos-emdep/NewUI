
using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SAM
{
    [DataContract]
    public class ValidateItem : ModelBase, IDisposable
    {

        #region Fields
        //private Status itemStatus;
        Int32 idItemStatus;
        private LookupValue itemStatus;
        Int64 idOT;
        string siteName;
        string customer;
        string code;
        string numItem;
        string reference;
        string description;
        Int64 idArticle;
        string imagePath;
        Int64 quantity;
        Int64 idpartNumberTracking;
        byte[] imageInBytes;
        string partNumberCode;
        bool isEnabled;
        string barcodestring;
        #endregion

        #region Constructor

        public ValidateItem()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
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
        public string NumItem
        {
            get { return numItem; }
            set
            {
                numItem = value;
                OnPropertyChanged("NumItem");
            }
        }

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [DataMember]
        public Int64 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        [DataMember]
        public Int64 Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public Int64 IdPartNumberTracking
        {
            get { return idpartNumberTracking; }
            set
            {
                idpartNumberTracking = value;
                OnPropertyChanged("IdPartNumberTracking");
            }
        }

        [DataMember]
        public byte[] ImageInBytes
        {
            get { return imageInBytes; }
            set
            {
                imageInBytes = value;
                OnPropertyChanged("ImageInBytes");
            }
        }

        [DataMember]
        public string PartNumberCode
        {
            get { return partNumberCode; }
            set
            {
                partNumberCode = value;
                OnPropertyChanged("PartNumberCode");
            }
        }

        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public string Barcodestring
        {
            get { return barcodestring; }
            set
            {
                barcodestring = value;
                OnPropertyChanged("Barcodestring");
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



        [DataMember]
        public LookupValue ItemStatus
        {
            get
            {
                return this.itemStatus;
            }
            set
            {
                this.itemStatus = value;
                this.OnPropertyChanged("ItemStatus");
            }
        }

        [DataMember]
        public int IdItemStatus
        {
            get
            {
                return idItemStatus;
            }

            set
            {
                idItemStatus = value;
                this.OnPropertyChanged("IdItemStatus");
            }
        }
        #endregion
    }

    //public enum Status
    //{
    //    [Display(Name = "---", Description = "---")]
    //    EMPTY,
    //    [Display(Name = "", Description = "OK")]
    //    OK,
    //    [Display(Name = "", Description = "NOK")]
    //    NOK
    //}
}
