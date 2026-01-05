using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class EngineeringAnalysisArticleDetail : ModelBase, IDisposable
    {

        #region Fields
        byte idItemOtStatus;
        Int32 idArticle;
        string quotationCode;
        Int64 quantity;
        sbyte isUnLinkQuotation;
        bool isArticleEnabled;
        #endregion

        #region Constructor

        public EngineeringAnalysisArticleDetail()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public byte IdItemOtStatus
        {
            get { return idItemOtStatus; }
            set
            {
                idItemOtStatus = value;
                OnPropertyChanged("IdItemOtStatus");
            }
        }

        [DataMember]
        public Int32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public string QuotationCode
        {
            get { return quotationCode; }
            set
            {
                quotationCode = value;
                OnPropertyChanged("QuotationCode");
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
        public bool IsArticleEnabled
        {
            get { return isArticleEnabled; }
            set
            {
               isArticleEnabled = value;
                OnPropertyChanged("IsArticleEnabled");
            }
        }

        [DataMember]
        public sbyte IsUnLinkQuotation
        {
            get { return isUnLinkQuotation; }
            set
            {
                isUnLinkQuotation = value;
                OnPropertyChanged("IsUnLinkQuotation");
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
