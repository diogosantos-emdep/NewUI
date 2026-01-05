using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Crm
{
    [DataContract]
    //[nsatpute][08.07.2025][GEOS2-7205]
    public class StatusMessage : ModelBase, IDisposable
    {
        #region  Fields
        string symbol;
        string message;
        int isSuccess;
        #endregion

        #region Constructor
        public StatusMessage()
        {

        }
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public string Symbol
        {
            get
            {
                return symbol;
            }

            set
            {
                symbol = value;
                OnPropertyChanged("Symbol");
            }
        }

        [NotMapped]
        [DataMember]
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        [NotMapped]
        [DataMember]
        public int IsSuccess
        {
            get
            {
                return isSuccess;
            }

            set
            {
                isSuccess = value;
                OnPropertyChanged("IsSuccess");
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
