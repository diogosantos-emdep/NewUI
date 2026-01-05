using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("offerstatustypes")]
    [DataContract]
    public class GeosStatus : ModelBase,IDisposable
    {
        #region  Fields
        Int64 idOfferStatusType;
        Int64 position;
        string name;
        string htmlColor;
        List<Offer> offers;
        Brush htmlBrushColor;
        Int64? idSalesStatusType;
        SalesStatusType salesStatusType;
        bool isEnabled;
        #endregion

        #region Constructor
        public GeosStatus()
        {
            this.Offers = new List<Offer>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdOfferStatusType")]
        [DataMember]
        public Int64 IdOfferStatusType
        {
            get
            {
                return idOfferStatusType;
            }

            set
            {
                idOfferStatusType = value;
                OnPropertyChanged("IdOfferStatusType");
            }
        }

        [Column("Position")]
        [DataMember]
        public Int64 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        [Column("HtmlColor")]
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

        [Column("IdSalesStatusType")]
        [DataMember]
        public Int64? IdSalesStatusType
        {
            get
            {
                return idSalesStatusType;
            }

            set
            {
                idSalesStatusType = value;
                OnPropertyChanged("IdSalesStatusType");
            }
        }

        [NotMapped]
        [DataMember]
        public Brush HtmlBrushColor
        {
            get
            {
                return htmlBrushColor;
            }
            set
            {
                htmlBrushColor = value;
                OnPropertyChanged("HtmlBrushColor");
            }
        }

        [DataMember]
        public virtual List<Offer> Offers
        {
            get
            {
                return offers;
            }

            set
            {
                offers = value;
                OnPropertyChanged("Offers");
            }
        }

        [NotMapped]
        [DataMember]
        public virtual SalesStatusType SalesStatusType
        {
            get
            {
                return salesStatusType;
            }

            set
            {
                salesStatusType = value;
                OnPropertyChanged("SalesStatusType");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }

            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        #endregion

        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
