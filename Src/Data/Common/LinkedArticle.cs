using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SCM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
//[Rahul.Gadhave][GEOS2-8316][Date:27-06-2025]
namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class LinkedArticle : ModelBase, IDisposable
    {
        #region Declaration

        Int64 idArticle;
        Int64 idPCMArticleImage;
        string reference;
        string status;
        string category;
        string name;
        Articles articles;
        private int idLinkType;
        private int idPCMArticleCategory;
        string linkedtypename;
        private Int32 idLookupValue;
        string hTMLColor;

        #endregion

        #region Properties
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
        public Int64 IdPCMArticleImage
        {
            get { return idPCMArticleImage; }
            set
            {
                idPCMArticleImage = value;
                OnPropertyChanged("IdPCMArticleImage");
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
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        [DataMember]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
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
        public Articles PCMArticle
        {
            get { return articles; }
            set
            {
                articles = value;
                OnPropertyChanged("PCMArticle");
            }
        }
        [DataMember]
        public int IdLinkType
        {
            get { return idLinkType; }
            set { idLinkType = value;
                OnPropertyChanged("IdLinkType"); }
        }
        [DataMember]
        public string LinkedTypeName
        {
            get { return linkedtypename; }
            set { linkedtypename = value; OnPropertyChanged("LinkedTypeName"); }
        }
        [DataMember]
        public int IdPCMArticleCategory
        {
            get { return idPCMArticleCategory; }
            set { idPCMArticleCategory = value; OnPropertyChanged("IdPCMArticleCategory"); }
        }
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }
        [DataMember]
        public string StatusHTMLColor
        {
            get
            {
                return hTMLColor;
            }

            set
            {
                hTMLColor = value;
                OnPropertyChanged("StatusHTMLColor");
            }
        }
        #endregion

        #region Constructor
        public LinkedArticle()
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
