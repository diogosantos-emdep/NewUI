using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    // [pramod.misal][23-05-2025][GEOS2-5731]
    [DataContract]
    public  class MaterialType : ModelBase, IDisposable
    {

        #region Declaration
        int idMaterialType;
        string typematerial;


   


        string article;
       
        DateTime latestPickingDate;       
        string warehouse;
        #endregion

        #region Properties

        [DataMember]
        public int IdMaterialType
        {
            get { return idMaterialType; }
            set
            {
                idMaterialType = value;
                OnPropertyChanged(nameof(IdMaterialType));
            }
        }

        [DataMember]
        public string TypeMaterial
        {
            get { return typematerial; }
            set
            {
                typematerial = value;
                OnPropertyChanged(nameof(TypeMaterial));
            }
        }









        [DataMember]
        public string Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged(nameof(Article));
            }
        }

        







        [DataMember]
        public DateTime LatestPickingDate
        {
            get { return latestPickingDate; }
            set
            {
                latestPickingDate = value;
                OnPropertyChanged(nameof(LatestPickingDate));
            }
        }

        [DataMember]
        public string Warehouse
        {
            get { return warehouse; }
            set
            {
                warehouse = value;
                OnPropertyChanged(nameof(Warehouse));
            }
        }

        #endregion

        #region Constructor

        public MaterialType()
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
