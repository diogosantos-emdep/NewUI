using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4971]
    [DataContract]
    public class SimilarCharactersByConfiguration : ModelBase, IDisposable
    {
        #region Declaration
        private int idSearchConfiguration;
        private int idCharactersByConfiguration;
        private string characterA;
        private string characterB;
        private string description;
        private Int32 createdBy;
        private DateTime createdIn;
        private Int32 modifiedBy;
        private DateTime modifiedIn;
        #endregion

        #region Properties
        [DataMember]
        public int IdCharactersByConfiguration
        {
            get { return idCharactersByConfiguration; }
            set
            {
                idCharactersByConfiguration = value;
                OnPropertyChanged("IdCharactersByConfiguration");
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
        public string CharacterA
        {
            get { return characterA; }
            set
            {
                characterA = value;
                OnPropertyChanged("CharacterA");
            }
        }
        [DataMember]
        public string CharacterB
        {
            get { return characterB; }
            set
            {
                characterB = value;
                OnPropertyChanged("CharacterB");
            }
        }

        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
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
        #endregion

        #region Constructor
        public SimilarCharactersByConfiguration()
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
