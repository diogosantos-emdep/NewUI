using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class ArticleCategorieMapping : ModelBase, IDisposable
    {
        #region Fields

        long idWMSArticleCategory;
        uint idPCMArticleCategory;
        string wmsName;
        string wmsparent;
        string pcmName;
        string pCMArticleMapping;
        string wMSArticleMapping;
        string pcmparent;
        #endregion

        #region Constructor
        public ArticleCategorieMapping()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public long IdWMSArticleCategory
        {
            get { return idWMSArticleCategory; }
            set
            {
                idWMSArticleCategory = value;
                OnPropertyChanged("IdWMSArticleCategory");
            }
        }

        [DataMember]

        public uint IdPCMArticleCategory
        {
            get { return idPCMArticleCategory; }
            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged("IdPCMArticleCategory");
            }
        }

        [DataMember]
        public string PCMArticleMapping
        {
            get { return pCMArticleMapping; }
            set
            {
                pCMArticleMapping = value;
                OnPropertyChanged("PCMArticleMapping");
            }
        }
        [DataMember]
        public string WMSArticleMapping
        {
            get { return wMSArticleMapping; }
            set
            {
                wMSArticleMapping = value;
                OnPropertyChanged("WMSArticleMapping");
            }
        }
        [DataMember]
        public string WMSName
        {
            get { return wmsName; }
            set
            {
                wmsName = value;
                OnPropertyChanged("WMSName");
            }
        }
        [DataMember]
        public string WMSParent
        {
            get { return wmsparent; }
            set
            {
                wmsparent = value;
                OnPropertyChanged("WMSParent");
            }
        }
        [DataMember]
        public string PCMName
        {
            get { return pcmName; }
            set
            {
                pcmName = value;
                OnPropertyChanged("PCMName");
            }
        }
        [DataMember]
        public string PCMParent
        {
            get { return pcmparent; }
            set
            {
                pcmparent = value;
                OnPropertyChanged("PCMParent");
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
