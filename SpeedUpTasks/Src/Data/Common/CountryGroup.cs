using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("country_groups")]
    [DataContract]
    public class CountryGroup : ModelBase, IDisposable
    {
        #region Fields

        Int64 idCountryGroup;
        string name;
        byte isFreeTrade;
        string htmlColor;
        #endregion

        #region Constructor
        public CountryGroup()
        {
        }
        #endregion

        #region Constructor

        [Key]
        [Column("IdCountryGroup")]
        [DataMember]
        public long IdCountryGroup
        {
            get { return idCountryGroup; }
            set
            {
                idCountryGroup = value;
                OnPropertyChanged("IdCountryGroup");
            }
        }

        [Column("Name")]
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

        [Column("IsFreeTrade")]
        [DataMember]
        public byte IsFreeTrade
        {
            get { return isFreeTrade; }
            set
            {
                isFreeTrade = value;
                OnPropertyChanged("IsFreeTrade");
            }
        }

        [Column("HtmlColor")]
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
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
