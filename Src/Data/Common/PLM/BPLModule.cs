using Emdep.Geos.Data.Common.PCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PLM
{
    public class BPLModule : ModelBase, IDisposable
    {
        #region  Fields

        byte idTemplate;
        string name;
        string templateName;
        UInt64 idCPType;
        ProductTypes productType;
        string key;
        string parent;
        bool isBPLModule_Current;
        string statusAbbreviation;
        string statusHtmlColor;
        #endregion

        #region Constructor
        public BPLModule()
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
        public string TemplateName
        {
            get { return templateName; }
            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
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

        [DataMember]
        public bool IsBPLdModule_Current
        {
            get
            {
                return isBPLModule_Current;
            }

            set
            {
                isBPLModule_Current = value;
                OnPropertyChanged("IsBPLdModule_Current");
            }
        }

        [DataMember]
        public string StatusAbbreviation
        {
            get
            {
                return statusAbbreviation;
            }

            set
            {
                statusAbbreviation = value;
                OnPropertyChanged("StatusAbbreviation");
            }
        }

        [DataMember]
        public string StatusHtmlColor
        {
            get
            {
                return statusHtmlColor;
            }

            set
            {
                statusHtmlColor = value;
                OnPropertyChanged("StatusHtmlColor");
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
            BPLModule bPLModule = (BPLModule)this.MemberwiseClone();

            return bPLModule;
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
