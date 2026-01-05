using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class PackingCompany : ModelBase, IDisposable
    {
        #region Fields

        Int32 idCompany;
        string name;
        string shortName;
        ObservableCollection<PackingBox> packingBoxes;
        #endregion

        #region Constructor
        public PackingCompany()
        {
        }
        #endregion

        #region Properties

       
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
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
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                OnPropertyChanged("ShortName");
            }
        }


        [DataMember]
        public ObservableCollection<PackingBox> PackingBoxes
        {
            get { return packingBoxes; }
            set
            {
                packingBoxes = value;
                OnPropertyChanged("PackingBoxes");
            }
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
