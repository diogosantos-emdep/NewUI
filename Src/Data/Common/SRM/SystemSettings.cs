using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
     public class SystemSettings : ModelBase, IDisposable
    {
        #region Declaration
        string warehouse;//[Sudhir.jangra][GEOS2-4407][04/05/2023]
        string cc;//[Sudhir.Jangra][GEOS2-4407]
        #endregion

        #region Properties
        //[Sudhir.jangra][GEOS2-4407][04/05/2023]  
        [Column("Warehouse")]
        [DataMember]
        public string Warehouse
        {
            get { return warehouse; }
            set
            {
                warehouse = value;
                OnPropertyChanged("Warehouse");
            }
        }

        //[Sudhir.jangra][GEOS2-4407][04/05/2023]
        [Column("CC")]
        [DataMember]
        public string CC
        {
            get { return cc; }
            set
            {
                cc = value;
                OnPropertyChanged("CC");
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
