using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("offeroptiontypes")]
    [DataContract]
    public class OfferOptionType : ModelBase, IDisposable
    {

        #region  Fields
        Int32 idOfferOptionType;
        string name;
        List<OfferOption> offerOptions;
        #endregion

        #region Constructor
        public OfferOptionType()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdOfferOptionType")]
        [DataMember]
        public Int32 IdOfferOptionType
        {
            get
            {
                return idOfferOptionType;
            }
            set
            {
                idOfferOptionType = value;
                OnPropertyChanged("IdOfferOptionType");
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

        [DataMember]
        public virtual List<OfferOption> OfferOptions
        {
            get
            {
                return offerOptions;
            }
            set
            {
                offerOptions = value;
                OnPropertyChanged("OfferOptions");
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
