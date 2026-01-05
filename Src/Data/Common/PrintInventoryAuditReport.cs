using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class PrintInventoryAuditReport : ModelBase, IDisposable
    {
        #region Fields

        Int64 idWarehouseInventoryAuditItem;
        Int64 idWarehouseInventoryAudit;
        string mainWarehouseLocation;
        long idWarehouseLocation;
        Int64 totalAudited;
        Int64 descrepancies;
        double percentage;
    
        [DataMember]
        public string MainWarehouseLocation
        {
            get { return mainWarehouseLocation; }
            set
            {
                mainWarehouseLocation = value;
                OnPropertyChanged("MainWarehouseLocation");
            }
        }
       
        [DataMember]
        public Int64 TotalAudited
        {
            get { return totalAudited; }
            set
            {
                totalAudited = value;
                OnPropertyChanged("TotalAudited");
            }
        }
    
        [DataMember]
        public Int64 Descrepancies
        {
            get { return descrepancies; }
            set
            {
                descrepancies = value;
                OnPropertyChanged("Descrepancies");
            }
        }

        [Column("IdWarehouseInventoryAuditItem")]
        [DataMember]
        public long IdWarehouseInventoryAuditItem
        {
            get { return idWarehouseInventoryAuditItem; }
            set
            {
                idWarehouseInventoryAuditItem = value;
                OnPropertyChanged("IdWarehouseInventoryAuditItem");
            }
        }

        [Column("IdWarehouseInventoryAudit")]
        [DataMember]
        public long IdWarehouseInventoryAudit
        {
            get { return idWarehouseInventoryAudit; }
            set
            {
                idWarehouseInventoryAudit = value;
                OnPropertyChanged("IdWarehouseInventoryAudit");
            }
        }

        [Column("IdWarehouseLocation")]
        [DataMember]
        public long IdWarehouseLocation
        {
            get { return idWarehouseLocation; }
            set
            {
                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        [DataMember]
        public double Percentage
        {
            get { return percentage; }
            set
            {
                percentage = value;
                OnPropertyChanged("Percentage");
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
