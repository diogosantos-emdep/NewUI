using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
    public class ArticleSuppliersDoc : ModelBase, IDisposable
    {
        #region Declaration

        UInt64 idArticleSupplier;
        UInt64 idSupplierDoc;
        string originalFileName;
        string savedFileName;
        string description;
        UInt64 idDocType;
        DateTime? expirationDate;
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [DataMember]
        public ulong IdSupplierDoc
        {
            get
            {
                return idSupplierDoc;
            }

            set
            {
                idSupplierDoc = value;
                OnPropertyChanged("IdSupplierDoc");
            }
        }

        [DataMember]
        public string OriginalFileName
        {
            get
            {
                return originalFileName;
            }

            set
            {
                originalFileName = value;
                OnPropertyChanged("OriginalFileName");
            }
        }

        [DataMember]
        public string SavedFileName
        {
            get
            {
                return savedFileName;
            }

            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [DataMember]
        public ulong IdDocType
        {
            get
            {
                return idDocType;
            }

            set
            {
                idDocType = value;
                OnPropertyChanged("IdDocType");
            }
        }

        [DataMember]
        public DateTime? ExpirationDate
        {
            get
            {
                return expirationDate;
            }

            set
            {
                expirationDate = value;
                OnPropertyChanged("ExpirationDate");
            }
        }


        #endregion

        #region Constructor

        public ArticleSuppliersDoc()
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
