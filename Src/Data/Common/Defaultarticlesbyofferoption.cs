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
    [Table("Defaultarticlesbyofferoption")]
    [DataContract]
    class Defaultarticlesbyofferoption : ModelBase, IDisposable
    {
        string quantity;
        byte idTemplate;
        Int32 idOfferOption;
        byte idDefaultItemOtStatus;
        Int16 idArticle;

        [Column("Quantity")]
        [DataMember]
        public string Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        [Column("IdTemplate")]
        [DataMember]
        public byte IdTemplate
        {
            get
            {
                return idTemplate;
            }

            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [Column("IdOfferOption")]
        [DataMember]
        public Int32 IdOfferOption
        {
            get
            {
                return idOfferOption;
            }

            set
            {
                idOfferOption = value;
                OnPropertyChanged("IdOfferOption");
            }
        }

        [Column("IdDefaultItemOtStatus")]
        [DataMember]
        public byte IdDefaultItemOtStatus
        {
            get
            {
                return idDefaultItemOtStatus;
            }

            set
            {
                idDefaultItemOtStatus = value;
                OnPropertyChanged("IdDefaultItemOtStatus");
            }
        }


        [Column("IdArticle")]
        [DataMember]
        public Int16 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }

        #region Constructor
        public Defaultarticlesbyofferoption()
        {

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
