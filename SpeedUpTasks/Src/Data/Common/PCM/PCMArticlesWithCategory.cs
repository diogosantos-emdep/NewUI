using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class PCMArticlesWithCategory : ModelBase, IDisposable
    {
        #region  Fields

        uint? idPCMArticleCategory;
        string name;
        UInt64? parent;
        string keyName;
        string parentName;
        //UInt32? idArticle;
        //string reference;
        //string description;
        #endregion

        #region Constructor
        public PCMArticlesWithCategory()
        {
        }

        #endregion

        #region Properties

        [DataMember]
        public uint? IdPCMArticleCategory
        {
            get
            {
                return idPCMArticleCategory;
            }

            set
            {
                idPCMArticleCategory = value;
                OnPropertyChanged("IdPCMArticleCategory");
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
        public ulong? Parent
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
        public string KeyName
        {
            get
            {
                return keyName;
            }

            set
            {
                keyName = value;
                OnPropertyChanged("KeyName");
            }
        }

        [DataMember]
        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
            }
        }

        //[DataMember]
        //public uint? IdArticle
        //{
        //    get
        //    {
        //        return idArticle;
        //    }

        //    set
        //    {
        //        idArticle = value;
        //        OnPropertyChanged("IdArticle");
        //    }
        //}

        //[DataMember]
        //public string Reference
        //{
        //    get
        //    {
        //        return reference;
        //    }

        //    set
        //    {
        //        reference = value;
        //        OnPropertyChanged("Reference");
        //    }
        //}

        //[DataMember]
        //public string Description
        //{
        //    get
        //    {
        //        return description;
        //    }

        //    set
        //    {
        //        description = value;
        //        OnPropertyChanged("Description");
        //    }
        //}

        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
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
