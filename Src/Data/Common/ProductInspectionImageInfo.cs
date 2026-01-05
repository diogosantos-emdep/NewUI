using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    //[Sudhir.Jangra][Geos2-3531][20/01/2023]
    [DataContract]
    public class ProductInspectionImageInfo : ModelBase, IDisposable
    {
        #region Fields
        string imagePath;
        byte[] imageInByte;
        #endregion

        #region Properties
        [DataMember]
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        [DataMember]
        public byte[] ImageInByte
        {
            get { return imageInByte; }
            set
            {
                imageInByte = value;
                OnPropertyChanged("ImageInByte");
            }
        }
        #endregion

        #region Constructor
        public ProductInspectionImageInfo()
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
