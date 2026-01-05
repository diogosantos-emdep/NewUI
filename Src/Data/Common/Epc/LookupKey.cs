using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("lookup_keys")]
    [DataContract]
    public class LookupKey : ModelBase, IDisposable
    {
        #region Fields

        Byte idLookupKey;
        string lookupKeyName;
        SByte isEditable;
        IList<LookupValue> lookupValues;

        #endregion

        #region Constructor

        public LookupKey()
        {
            //this.LookupValues = new List<LookupValue>();
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdLookupKey")]
        [DataMember]
        public Byte IdLookupKey
        {
            get { return idLookupKey; }
            set
            {
                idLookupKey = value;
                this.OnPropertyChanged("IdLookupKey");
            }
        }

        [Column("LookupKeyName")]
        [DataMember]
        public string LookupKeyName
        {
            get { return lookupKeyName; }
            set
            {
                lookupKeyName = value;
                this.OnPropertyChanged("LookupKeyName");
            }
        }

        [Column("IsEditable")]
        [DataMember]
        public SByte IsEditable
        {
            get { return isEditable; }
            set
            {
                isEditable = value;
                this.OnPropertyChanged("IsEditable");
            }
        }

        [DataMember]
        public virtual IList<LookupValue> LookupValues
        {
            get { return lookupValues; }
            set
            {
                lookupValues = value;
                OnPropertyChanged("LookupValues");
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
