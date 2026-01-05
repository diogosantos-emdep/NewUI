using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    public class POEmailNotification : ModelBase, IDisposable
    {
        #region Declaration
      
        #endregion
        #region Properties
        private string _Code = string.Empty;
        [DataMember(Order = 1)]
        public string Code
        {
            get { return _Code; }
            set {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }
        private Int32 _IdWorkflowStatus;
        [DataMember(Order = 2)]
        public Int32 IdWorkflowStatus
        {
            get { return _IdWorkflowStatus; }
            set
            {
                _IdWorkflowStatus = value;
                OnPropertyChanged("IdWorkflowStatus");
            }
        }
        private Int32? _IdWarehouse ;
        [DataMember(Order = 3)]
        public Int32? IdWarehouse
        {
            get { return _IdWarehouse; }
            set
            {
                _IdWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }
        private Int32? _IdCurrency ;
        [DataMember(Order = 4)]
        public Int32? IdCurrency
        {
            get { return _IdCurrency; }
            set
            {
                _IdCurrency = value;
                OnPropertyChanged("IdCurrency");
            }
        }
        private Int64 _IdWarehousePurchaseOrder;
        [DataMember(Order = 5)]
        public Int64 IdWarehousePurchaseOrder
        {
            get { return _IdWarehousePurchaseOrder; }
            set
            {
                _IdWarehousePurchaseOrder = value;
                OnPropertyChanged("IdWarehousePurchaseOrder");
            }
        }
        private Int64 _IdAssignee ;
        [DataMember(Order = 6)]
        public Int64 IdAssignee
        {
            get { return _IdAssignee; }
            set
            {
                _IdAssignee = value;
                OnPropertyChanged("IdAssignee");

            }
        }
        private Int64 _IdApprover;
        [DataMember(Order = 7)]
        public Int64 IdApprover
        {
            get { return _IdApprover; }
            set { _IdApprover = value;
                OnPropertyChanged("IdApprover");
            }
        }
        private string _ReasonClosed = string.Empty;
        [DataMember(Order = 8)]
        public string ReasonClosed
        {
            get { return _ReasonClosed; }
            set { _ReasonClosed = value;
                OnPropertyChanged("ReasonClosed");
            }
        }
        private string _Supplier = string.Empty;
        [DataMember(Order = 9)]
        public string Supplier
        {
            get { return _Supplier; }
            set { _Supplier = value;
                OnPropertyChanged("Supplier");
            }
        }
        private Int64 _IdArticleSupplierType;
        [DataMember(Order = 10)]
        public Int64 IdArticleSupplierType
        {
            get { return _IdArticleSupplierType; }
            set { _IdArticleSupplierType = value;
                OnPropertyChanged("IdArticleSupplierType");
            }
        }
        private string _ArticleSupplierType = string.Empty;
        [DataMember(Order = 11)]
        public string ArticleSupplierType
        {
            get { return _ArticleSupplierType; }
            set { _ArticleSupplierType = value;
                OnPropertyChanged("ArticleSupplierType");
            }
        }
        private string _HtmlColor = string.Empty;
        [DataMember(Order = 12)]
        public string HtmlColor
        {
            get { return _HtmlColor; }
            set { _HtmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        private string _Comments = string.Empty;
        [DataMember(Order = 13)]
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value;
                OnPropertyChanged("Comments");
            }
        }
        private string _Status = string.Empty;
        [DataMember(Order = 14)]
        public string Status
        {
            get { return _Status; }
            set { _Status = value;
                OnPropertyChanged("Status");
            }
        }
        private string _StatusHtmlColor = string.Empty;
        [DataMember(Order = 15)]
        public string StatusHtmlColor
        {
            get { return _StatusHtmlColor; }
            set { _StatusHtmlColor = value;
                OnPropertyChanged("StatusHtmlColor");
            }
        }
        private string _SupplierContactPerson = string.Empty;
        [DataMember(Order = 16)]
        public string SupplierContactPerson
        {
            get { return _SupplierContactPerson; }
            set { _SupplierContactPerson = value;
                OnPropertyChanged("SupplierContactPerson");
            }
        }
        private string _SupplierContactEmail = string.Empty;
        [DataMember(Order = 17)]
        public string SupplierContactEmail
        {
            get { return _SupplierContactEmail; }
            set { _SupplierContactEmail = value;
                OnPropertyChanged("SupplierContactEmail");
            }
        }
        private Int64 _AssineeId;
        [DataMember(Order = 18)]
        public Int64 AssineeId
        {
            get { return _AssineeId; }
            set { _AssineeId = value;
                OnPropertyChanged("AssineeId");
            }
        }
        private string _AssineeFname = string.Empty;
        [DataMember(Order = 19)]
        public string AssineeFname
        {
            get { return _AssineeFname; }
            set { _AssineeFname = value;
                OnPropertyChanged("AssineeFname");
            }
        }
        private string _AssineeLname = string.Empty;
        [DataMember(Order = 20)]
        public string AssineeLname
        {
            get { return _AssineeLname; }
            set { _AssineeLname = value;
                OnPropertyChanged("AssineeLname");
            }
        }
        private string _AssineeCompEmail = string.Empty;
        [DataMember(Order = 21)]
        public string AssineeCompEmail
        {
            get { return _AssineeCompEmail; }
            set { _AssineeCompEmail = value;
                OnPropertyChanged("AssineeCompEmail");
            }
        }
        private Int64 _ApproverId;
        [DataMember(Order = 22)]
        public Int64 ApproverId
        {
            get { return _ApproverId; }
            set { _ApproverId = value;
                OnPropertyChanged("ApproverId");
            }
        }
        private string _ApproverFname = string.Empty;
        [DataMember(Order = 23)]
        public string ApproverFname
        {
            get { return _ApproverFname; }
            set { _ApproverFname = value;
                OnPropertyChanged("ApproverFname");
            }
        }
        private string _ApproverLname = string.Empty;
        [DataMember(Order = 24)]
        public string ApproverLname
        {
            get { return _ApproverLname; }
            set { _ApproverLname = value;
                OnPropertyChanged("ApproverLname");
            }
        }
        private string _ApproverCompEmail = string.Empty;
        [DataMember(Order = 25)]
        public string ApproverCompEmail
        {
            get { return _ApproverCompEmail; }
            set { _ApproverCompEmail = value;
                OnPropertyChanged("ApproverCompEmail");
            }
        }
        private string _CreatorEmail = string.Empty;
        [DataMember(Order = 26)]
        public string CreatorEmail
        {
            get { return _CreatorEmail; }
            set { _CreatorEmail = value;
                OnPropertyChanged("CreatorEmail");
            }
        }

        private string _CreatedByEmail = string.Empty;
        [IgnoreDataMember]
        public string CreatedByEmail
        {
            get { return _CreatedByEmail; }
            set { _CreatedByEmail = value;
                OnPropertyChanged("CreatedByEmail");
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
