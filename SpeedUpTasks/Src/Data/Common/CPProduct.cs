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
   
    [DataContract]
    public class CPProduct : ModelBase, IDisposable
    {
        #region Fields

        string reference;
        string otherReference;
        
        #endregion

        #region Constructor
        public CPProduct()
        {
        }

        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }

        [NotMapped]
        [DataMember]
        public string OtherReference
        {
            get
            {
                return otherReference;
            }

            set
            {
                otherReference = value;
                OnPropertyChanged("OtherReference");
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
