using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class ConfigurationFamily : ModelBase, IDisposable
    {
        #region Declaration
        int idColor;
        private uint idFamily;
        private int idSearchConfiguration;
        private string nameFamily;
        private List<SimilarCharactersByConfiguration> similarCharactersList;         
        private List<SimilarColorsByConfiguration> colorSimilarityList;
        private List<ComponentsByConfiguration> componentsList;
        private int wayMargin=0;
        private int _Internal=0;
        private int _External=0;
        private int height=0;
        private int length=0;
        private int width=0;
        private string refPages = "0";
        private string waysPages = "0";
        private string colorPages = "0";
        private string sizePages = "0";
        private string compPages = "0";
        private int noOfPages;
        private Int32 createdBy;
        private DateTime createdIn;
        private Int32 modifiedBy;
        private DateTime modifiedIn;
        private int idWayMargin;
        #endregion

        #region Properties
        [DataMember]
        public uint IdFamily
        {
            get { return idFamily; }
            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }
        [DataMember]
        public int IdSearchConfiguration
        {
            get { return idSearchConfiguration; }
            set
            {
                idSearchConfiguration = value;
                OnPropertyChanged("IdSearchConfiguration");
            }
        }

        [DataMember]
        public string NameFamily
        {
            get { return nameFamily; }
            set
            {
                nameFamily = value;
                OnPropertyChanged("NameFamily");
            }
        }

        [DataMember]
        public List<ComponentsByConfiguration> ComponentsList
        {
            get { return componentsList; }
            set
            {
                componentsList = value;
                OnPropertyChanged("ComponentsList");
            }
        }

        [DataMember]
        public List<SimilarCharactersByConfiguration> SimilarCharactersList
        {
            get { return similarCharactersList; }
            set
            {
                similarCharactersList = value;
                OnPropertyChanged("SimilarCharactersList");
            }
        }
        [DataMember]
        public List<SimilarColorsByConfiguration> ColorSimilarityList
        {
            get { return colorSimilarityList; }
            set
            {
                colorSimilarityList = value;
                OnPropertyChanged("ColorSimilarityList");
            }
        }
        [DataMember]
        public string RefPages
        {
            get { return refPages; }
            set
            {
                refPages = value;
                OnPropertyChanged("RefPages");
            }
        }
        [DataMember]
        public string WaysPages
        {
            get { return waysPages; }
            set
            {
                waysPages = value;
                OnPropertyChanged("WaysPages");
            }
        }
        [DataMember]
        public int NoOfPages
        {
            get { return noOfPages; }
            set
            {
                noOfPages = value;
                OnPropertyChanged("NoOfPages");
            }
        }
        [DataMember]
        public string ColorPages
        {
            get { return colorPages; }
            set
            {
                colorPages = value;
                OnPropertyChanged("ColorPages");
            }
        }        
        [DataMember]
        public string SizePages
        {
            get { return sizePages; }
            set
            {
                sizePages = value;
                OnPropertyChanged("SizePages");
            }
        }
        [DataMember]
        public string CompPages
        {
            get { return compPages; }
            set
            {
                compPages = value;
                OnPropertyChanged("CompPages");
            }
        }

        [DataMember]
        public int WayMargin
        {
            get { return wayMargin; }
            set
            {
                wayMargin = value;
                OnPropertyChanged("WayMargin");
            }
        }

        [DataMember]
        public int IdWayMargin
        {
            get { return idWayMargin; }
            set
            {
                idWayMargin = value;
                OnPropertyChanged("IdWayMargin");
            }
        }

        [DataMember]
        public int Internal
        {
            get { return _Internal; }
            set
            {
                _Internal = value;
                OnPropertyChanged("Internal");
            }
        }
        [DataMember]
        public int External
        {
            get { return _External; }
            set
            {
                _External = value;
                OnPropertyChanged("External");
            }
        }
        [DataMember]
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }
        [DataMember]
        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged("Length");
            }
        }
        [DataMember]
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }
        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }

        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public int IdColor
        {
            get { return idColor; }
            set
            {
                idColor = value;
                OnPropertyChanged("IdColor");
            }
        }
        #endregion

        #region Constructor
        public ConfigurationFamily()
        {

        }
        #endregion

        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
