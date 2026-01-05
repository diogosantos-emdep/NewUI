using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [001][ashish.malkhede][07.11.2024][GEOS2-6460]
    /// </summary>
    public class POType: ModelBase,IDisposable
    {
        #region Fields
        int idPoType;
        string type;
        string htmlColor;
        string abbreviation;
        #endregion

        #region Constructor

        public POType()
        {

        }

        #endregion

        #region Properties
        [DataMember]
        public int IdPoType
        {
            get { return idPoType; }
            set { idPoType = value; OnPropertyChanged("IdPoType"); }
        }

        [DataMember]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
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
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
            }
        }
        [DataMember]
        public string Abbreviation
        {
            get
            {
                return abbreviation;
            }

            set
            {
                abbreviation = value;
                OnPropertyChanged("Abbreviation");
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
