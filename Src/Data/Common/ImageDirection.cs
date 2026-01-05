using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Drawing;

namespace Emdep.Geos.Data.Common
{
    public class ImageDirection : ModelBase, IDisposable
    {
        #region Declaration

        UInt16 idImage;
        string name;
        Bitmap bitmapDirection;

        #endregion

        #region Properties

        public ushort IdImage
        {
            get{ return idImage;}
            set{ idImage = value; OnPropertyChanged("IdImage"); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        public Bitmap BitmapDirection
        {

            get { return bitmapDirection; }
            set { bitmapDirection = value; OnPropertyChanged("BitmapDirection"); }
        }


        #endregion

        #region Constructor

        public ImageDirection()
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
