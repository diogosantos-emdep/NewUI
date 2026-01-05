using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class ResourceStorage : ModelBase, IDisposable
    {
        #region Fields

        private int id;
        private string model;
        private byte[] picture;
        //private BitmapImage image;

        #endregion

        #region Constructor

        public ResourceStorage()
        {
        }

        #endregion

        #region Properties

        [DataMember]
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        [DataMember]
        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged("Model");
            }
        }

        [DataMember]
        public byte[] Picture
        {
            get { return picture; }
            set
            {
                picture = value;
                OnPropertyChanged("Picture");
            }
        }

        //[DataMember]
        //public BitmapImage Image
        //{
        //    get { return image; }
        //    set
        //    {
        //        image = value;
        //        OnPropertyChanged("Image");
        //    }
        //}

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
