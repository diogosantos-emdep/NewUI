using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{

    public class WarehouseData : ModelBase, IDisposable
    {

        Int32 idWarehouse;
        string warehouseName;

        [DataMember]
        public Int32 IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged("IdWarehouse");
            }
        }




        [DataMember]
        public string WarehouseName
        {
            get
            {
                return warehouseName;
            }

            set
            {
                warehouseName = value;
                OnPropertyChanged("WarehouseName");
            }
        }


        #region Constructor
        public WarehouseData()
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
