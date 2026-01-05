using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("product_version_validations")]
    [DataContract(IsReference = true)]
    public class ProductVersionValidation : ModelBase, IDisposable
    {
        #region Fields
        Int64 idProductVersionValidation;
        Int64 idProductVersionItem;
        Int32 idValidator;
        DateTime validationDate;
        bool status;
        ProductVersionItem productVersionItem;
        User validator;

        #endregion


        #region Properties
        [Key]
        [Column("IdProductVersionValidation")]
        [DataMember]
        public Int64 IdProductVersionValidation
        {
            get
            {
                return idProductVersionValidation;
            }

            set
            {
                idProductVersionValidation = value;
                OnPropertyChanged("IdProductVersionValidation");
            }
        }

        [Column("IdProductVersionItem")]
        [ForeignKey("ProductVersionItem")]
        [DataMember]
        public Int64 IdProductVersionItem
        {
            get
            {
                return idProductVersionItem;
            }

            set
            {
                idProductVersionItem = value;
                OnPropertyChanged("IdProductVersionItem");
            }
        }

        [Column("IdValidator")]
        [ForeignKey("Validator")]
        [DataMember]
        public Int32 IdValidator
        {
            get
            {
                return idValidator;
            }

            set
            {
                idValidator = value;
                OnPropertyChanged("IdValidator");
            }
        }

        [Column("ValidationDate")]
        [DataMember]
        public DateTime ValidationDate
        {
            get
            {
                return validationDate;
            }

            set
            {
                validationDate = value;
                OnPropertyChanged("ValidationDate");
            }
        }

        [Column("Status")]
        [DataMember]
        public bool Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public virtual ProductVersionItem ProductVersionItem
        {
            get
            {
                return productVersionItem;
            }

            set
            {
                productVersionItem = value;
                OnPropertyChanged("ProductVersionItem");
            }
        }

        [DataMember]
        public virtual User Validator
        {
            get
            {
                return validator;
            }

            set
            {
                validator = value;
                OnPropertyChanged("Validator");
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
