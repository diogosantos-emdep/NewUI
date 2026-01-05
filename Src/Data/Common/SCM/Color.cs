using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.SCM
{
    [DataContract]
    public class Color : ModelBase, IDisposable
    {
        #region Fields
        private ushort id;
        private string name;
        string htmlColor;
        object selectedColor;
        #endregion

        #region Properties
        [DataMember]
        public ushort Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged("Id"); }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        [DataMember]
        public string HtmlColor
        {
            get
            {
                return htmlColor;
            }
            set
            {
                htmlColor = value; OnPropertyChanged("HtmlColor");
            }
        }
        Visibility colorVisibility;
        [DataMember]
        public Visibility ColorVisibility
        {
            get { return colorVisibility; }
            set { colorVisibility = value; OnPropertyChanged("ColorVisibility"); }
        }
        Visibility imageVisibility;
        [DataMember]
        public Visibility ImageVisibility
        {
            get { return imageVisibility; }
            set { imageVisibility = value; OnPropertyChanged("ImageVisibility"); }
        }
        [DataMember]
        public object SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged("SelectedColor");
            }
        }
        #endregion

        #region Moethod
        public override string ToString()
        {
            return Name;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
