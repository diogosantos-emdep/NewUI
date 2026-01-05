using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class WarehouseCategory : ModelBase, IDisposable
    {
        Int64 idKey;
        Int64 idParent;
        string baseName;
        Int32 idArticle;
        Int64 idArticleCategory;
        bool isChecked;

        public long IdKey
        {
            get { return idKey; }
            set
            {
                idKey = value;
                OnPropertyChanged("IdKey");
            }
        }

        public long IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }

        public string BaseName
        {
            get { return baseName; }
            set
            {
                baseName = value;
                OnPropertyChanged("BaseName");
            }
        }

        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        public long IdArticleCategory
        {
            get { return idArticleCategory; }
            set
            {
                idArticleCategory = value;
                OnPropertyChanged("IdArticleCategory");
            }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
