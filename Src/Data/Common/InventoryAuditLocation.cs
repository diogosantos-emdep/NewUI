using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Drawing;
using Emdep.Geos.Data.Common.Epc;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common
{
    [DataContract]
    public class InventoryAuditLocation : ModelBase, IDisposable
    {
        #region Declaration
        long idWarehouseInventoryAudit;        
        long idWarehouseLocation;
        long idWarehouse;
        long parent;
        long position;
        long idArticle;
        string fullName;
        string name;

        int isAudited;
        string isAuditedYesNo;
        int idWarehouseInventoryAuditLocation;
        List<InventoryAuditArticle> inventoryAuditArticle;
        #endregion

        #region Properties

        [DataMember]
        public long IdWarehouseInventoryAudit
        {
            get
            {
                return idWarehouseInventoryAudit;
            }

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
            get
            {
                return idWarehouseLocation;
            }

            set
            {

                idWarehouseLocation = value;
                OnPropertyChanged("IdWarehouseLocation");
            }
        }

        [Column("IdWarehouse")]
        [DataMember]
        public long IdWarehouse
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

        [Column("Parent")]
        [DataMember]
        public long Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [Column("Position")]
        [DataMember]
        public long Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        [Column("FullName")]
        [DataMember]
        public string FullName
        {
            get
            {
                return fullName;
            }

            set
            {
                fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
     
        [DataMember]
        public int IsAudited
        {
            get
            {
                return isAudited;
            }

            set
            {
                isAudited = value;
                OnPropertyChanged("IsAudited");
            }
        }
      
        [DataMember]
        public string IsAuditedYesNo
        {
            get
            {
                return isAuditedYesNo;
            }

            set
            {
                isAuditedYesNo = value;
                OnPropertyChanged("IsAuditedYesNo");
            }
        }
     
        [DataMember]
        public int IdWarehouseInventoryAuditLocation
        {
            get
            {
                return idWarehouseInventoryAuditLocation;
            }
            set
            {
                idWarehouseInventoryAuditLocation = value;
                OnPropertyChanged("IdWarehouseInventoryAuditLocation");
            }
        }
        [DataMember]
        public long IdArticle
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

        [DataMember]
        public List<InventoryAuditArticle> InventoryAuditArticle
        {
            get
            {
                return inventoryAuditArticle;
            }

            set
            {

                inventoryAuditArticle = value;
                OnPropertyChanged("InventoryAuditArticle");
            }
        }
        #endregion

        #region Constructor

        public InventoryAuditLocation()
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
