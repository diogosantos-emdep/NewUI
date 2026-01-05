using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class DiscountArticles : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idCustomerDiscountArticle;
        UInt64 idCustomerDiscount;
        UInt64? idArticle;
        double _value;
        string name;
        string reference;
        string category;
        int idPlant;
        UInt32 idCreator;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;
        Articles article;
       
        #endregion

        #region Properties
        [DataMember]
        public ulong IdCustomerDiscountArticle
        {
            get
            {
                return idCustomerDiscountArticle;
            }

            set
            {
                idCustomerDiscountArticle = value;
                OnPropertyChanged("IdCustomerDiscountArticle");
            }
        }

        [DataMember]
        public ulong IdCustomerDiscount
        {
            get
            {
                return idCustomerDiscount;
            }

            set
            {
                idCustomerDiscount = value;
                OnPropertyChanged("IdCustomerDiscount");
            }
        }

        [DataMember]
        public ulong? IdArticle
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
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [DataMember]
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        [DataMember]
        public uint IdCreator
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
        public uint? IdModifier
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
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

       
      

        #endregion

        #region Constuctor
        public DiscountArticles()
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
