using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    //[Sudhir.Jangra][GEOS2-4441][21/09/2023]
    [DataContract]
    public class HardLockLicenses : ModelBase, IDisposable
    {
        #region Declaration
        UInt32 idArticle;
        string reference;
        string description;
        List<HardLockPlugins> hardLockPluginList;

        UInt32 idPCMArticle;
        #endregion

        #region Properties
        [DataMember]
        public UInt32 IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
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
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Reference");
            }
        }
        [DataMember]
        public List<HardLockPlugins> HardLockPluginList
        {
            get { return hardLockPluginList; }
            set
            {
                hardLockPluginList = value;
                OnPropertyChanged("HardLockPluginList");
            }
        }

        [DataMember]
        public UInt32 IdPCMArticle
        {
            get { return idPCMArticle; }
            set
            {
                idPCMArticle = value;
                OnPropertyChanged("IdPCMArticle");
            }
        }
        #endregion

        #region Constructor
        public HardLockLicenses()
        {

        }


        #endregion

        #region Methods
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
