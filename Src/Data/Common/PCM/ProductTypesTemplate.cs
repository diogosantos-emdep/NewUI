using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ProductTypesTemplate : ModelBase, IDisposable
    {
        #region  Fields

        byte idTemplate;
        string name;
        UInt64 idCPType;
        ProductTypes productType;
        string key;
        string parent;

        #endregion

        #region Constructor
        public ProductTypesTemplate()
        {
        }

        #endregion

        #region Properties

        [DataMember]
        public byte IdTemplate
        {
            get { return idTemplate; }
            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public ulong IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public ProductTypes ProductType
        {
            get
            {
                return productType;
            }

            set
            {
                productType = value;
                OnPropertyChanged("ProductType");
            }
        }

        [DataMember]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [DataMember]
        public string Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        #endregion

        #region Methods
        public override object Clone()
        {
            ProductTypesTemplate productTypesTemplate = (ProductTypesTemplate)this.MemberwiseClone();

            if (ProductType != null)
                productTypesTemplate.ProductType = (ProductTypes)this.ProductType.Clone();

            return productTypesTemplate;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
