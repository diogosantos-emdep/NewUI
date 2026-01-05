using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4971]
    [DataContract]
    public class SimilarColorsByConfiguration : ModelBase, IDisposable
    {
        #region Declarations
        private int idColorsByConfiguration;
        private int idSearchConfiguration;
        private Int32 idColorA;
        private Int32 idColorB;
        private Int32 createdBy;
        private DateTime createdIn;
        private Int32 modifiedBy;
        private DateTime modifiedIn;
        private Color selectedColor;
        private List<Color> colorList;
        #endregion

        #region Properties
        [DataMember]
        public int IdColorsByConfiguration
        {
            get { return idColorsByConfiguration; }
            set
            {
                idColorsByConfiguration = value;
                OnPropertyChanged("IdColorsByConfiguration");
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
        public Int32 IdColorA
        {
            get { return idColorA; }
            set
            {
                idColorA = value;
                OnPropertyChanged("IdColorA");
            }
        }
        [DataMember]
        public Int32 IdColorB
        {
            get { return idColorB; }
            set
            {
                idColorB = value;
                OnPropertyChanged("IdColorB");
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
        public List<Data.Common.SCM.Color> ColorList
        {
            get { return colorList; }
            set
            {
                colorList = value;
                OnPropertyChanged("ColorList");
            }
        }
        [DataMember]
        public Data.Common.SCM.Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (value != null)
                {
                    selectedColor = value;
                    OnPropertyChanged("SelectedColor");
                }
            }
        }
        #endregion

        #region Constructor
        public SimilarColorsByConfiguration()
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
