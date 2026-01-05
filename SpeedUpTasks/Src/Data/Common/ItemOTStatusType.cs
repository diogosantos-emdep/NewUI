using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("itemotstatustypes")]
    [DataContract]
    public class ItemOTStatusType : ModelBase, IDisposable
    {
        #region Fields
        Int32 idItemOtStatus;
        string name;
        string htmlColor;
        Int32 sequence;
        #endregion

        #region Constructor
        public ItemOTStatusType()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdItemOtStatus")]
        [DataMember]
        public Int32 IdItemOtStatus
        {
            get
            {
                return idItemOtStatus;
            }

            set
            {
                idItemOtStatus = value;
                OnPropertyChanged("IdItemOtStatus");
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

        [Column("Sequence")]
        [DataMember]
        public Int32 Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
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
