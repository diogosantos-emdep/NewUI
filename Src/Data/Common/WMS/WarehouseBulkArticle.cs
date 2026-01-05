using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    [DataContract]
    public class WarehouseBulkArticle : ModelBase, IDisposable
    {

        #region Declaration
        Int64 idWarehouseBulkArticle;
        Int64 idWarehouse;
        Int32 idArticle;
        Int32? idParent;
        Int32 idCreator;
        DateTime creationDate;
        Int32? idModifier;
        DateTime? modificationDate;
        Article article;
        Article parentArticle;
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdWarehouseBulkArticle
        {
            get
            {
                return idWarehouseBulkArticle;
            }

            set
            {
                idWarehouseBulkArticle = value;
            }
        }

        [DataMember]
        public Int64 IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }

        [DataMember]
        public Int32 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        [DataMember]
        public Int32? IdParent
        {
            get
            {
                return idParent;
            }

            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }

        [DataMember]
        public Int32 IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }
        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [DataMember]
        public Int32? IdModifier
        {
            get
            {
                return idModifier;
            }

            set
            {
                idModifier = value;
                OnPropertyChanged("IdModifier");
            }
        }
        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }

            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [DataMember]
        public Article Article
        {
            get
            {
                return article;
            }

            set
            {
                article = value;
                OnPropertyChanged("Article");
            }
        }

        [DataMember]
        public Article ParentArticle
        {
            get
            {
                return parentArticle;
            }

            set
            {
               parentArticle = value;
                OnPropertyChanged("ParentArticle");
            }
        }
        #endregion

        #region Constructor

        public WarehouseBulkArticle()
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
