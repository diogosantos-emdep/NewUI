using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Emdep.Geos.Data.Common
{
    [Serializable]
    [DataContract]
    public class ProductCategoryGrid : INotifyPropertyChanged, ICloneable
    {
        #region Fields

        Int64 idProductCategory;
        string name;
        Int64 idParent;
        Int32 level;
        ProductCategoryGrid category;
        Int64 position;

        #endregion

        #region Constructor
        public ProductCategoryGrid()
        {

        }

        #endregion


        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
       
        [DataMember]
        public Int64 IdProductCategory
        {
            get { return idProductCategory; }
            set
            {
                idProductCategory = value;
                OnPropertyChanged("IdProductCategory");
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
        public Int64 IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }


        
        [DataMember]
        public Int32 Level
        {
            get { return level; }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }


       
        [DataMember]
        public ProductCategoryGrid Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }


      
        [DataMember]
        public Int64 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
       
        #endregion
    }
}
