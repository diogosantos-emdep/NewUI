using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PLM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
   public class ConfigurationCategories : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idPCMArticleCategory;
        string name;
        string positionname;
        string parentname;
        UInt32 orderNumber;
        string description;
        string imagePath;
        string inUse;
        bool isAttachmentExist = false;
        private Visibility isAttachmentExistVisibility;

        #endregion

        #region Constructor

        public ConfigurationCategories()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public UInt64 IdPCMArticleCategory
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
        public UInt32 OrderNumber
        {
            get
            {
                return orderNumber;
            }

            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        [DataMember]
        public string PositionName
        {
            get
            {
                return positionname;
            }

            set
            {
                positionname = value;
                OnPropertyChanged("PositionName");
            }
        }

        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }


        [DataMember]
        public string ImagePath
        {
            get
            {
                return imagePath;
            }

            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        [DataMember]
        public string InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged("InUse");
            }
        }

        [DataMember]
        public string ParentName
        {
            get
            {
                return parentname;
            }

            set
            {
                parentname = value;
                OnPropertyChanged("ParentName");
            }
        }
        [DataMember]
        public Visibility IsAttachmentExistVisibility
        {
            get
            {
                return isAttachmentExistVisibility;
            }

            set
            {
                isAttachmentExistVisibility = value;
                OnPropertyChanged("IsAttachmentExistVisibility");
            }
        }

        [DataMember]
        public bool IsAttachmentExist
        {
            get
            {
                return isAttachmentExist;
            }

            set
            {
                isAttachmentExist = value;
                OnPropertyChanged("IsAttachmentExist");
            }
        }

        #endregion

        #region Methods

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
