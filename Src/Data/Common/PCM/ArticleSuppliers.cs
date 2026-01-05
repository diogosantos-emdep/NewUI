using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ArticleSuppliers : ModelBase, IDisposable
    {
        #region Fields

        string key;
        string groupName;
        string regionName;
        string parent;
        UInt32 idGroup;
        UInt32? idRegion;
        UInt32 idCreator;
        bool isChecked;
        string uniqueId;
        Country country;
        byte? idCountry;
        byte[] countryIconbytes;
        Site plant;
        UInt32? idPlant;
        UInt64 idArticleCustomerReferences;
        string iso;
        DateTime creationDate;
        UInt32? idModifier;
        DateTime? modificationDate;

        UInt64 idArticleList;
        Int32 isIncluded;
        string reference;
        #endregion

        #region Constructor

        public ArticleSuppliers()
        {

        }

        #endregion


        #region Properties

       
        [DataMember]
        public uint IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        
        [DataMember]
        public string GroupName
        {
            get
            {
                return groupName;
            }

            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
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
        public ulong IdArticleList
        {
            get
            {
                return idArticleList;
            }

            set
            {
                idArticleList = value;
                OnPropertyChanged("IdArticleList");
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
            ArticleSuppliers articleSupplier = (ArticleSuppliers)this.MemberwiseClone();
            //if (Country != null)
            //    articleCustomer.Country = (Country)this.Country.Clone();

            //if (plant != null)
            //    articleCustomer.plant = (Site)this.plant.Clone();

            return articleSupplier;
        }

        #endregion
    }
}
