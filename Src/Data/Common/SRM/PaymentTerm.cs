using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.SRM
{
    [Table("paymenttypes")]
    [DataContract]
    public class PaymentTerm : ModelBase,IDisposable
    {
        #region Fields

        int idPaymentType;
        string paymentType;
        

        #endregion

        #region Constructor

        public PaymentTerm()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdPaymentType")]
        [DataMember]
        public int IdPaymentType
        {
            get { return idPaymentType; }
            set
            {
                idPaymentType = value;
                OnPropertyChanged("IdPaymentType");
            }
        }

        [Column("PaymentType")]
        [DataMember]
        public string PaymentType
        {
            get { return paymentType; }
            set
            {
                paymentType = value;
                OnPropertyChanged("PaymentType");
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

