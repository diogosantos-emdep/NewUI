using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Emdep.Geos.Data.Common.ERM
{
   public class ERM_Warehouses : ModelBase, IDisposable
    {

            #region Fields

            Int64 idWarehouse;
            string name;
            Int64 idSite;
        string siteName; // [GEOS2-9404][gulab lakade][18 11 2025]

        #endregion
        #region Properties

        [Key]
            [Column("IdWarehouse")]
            [DataMember]
            public long IdWarehouse
            {
                get { return idWarehouse; }
                set
                {
                    idWarehouse = value;
                    OnPropertyChanged("IdWarehouse");
                }
            }

            [Column("Name")]
            [DataMember]
            public string Name
            {
                get { return name; }
                set
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }

            [Column("IdSite")]
            [DataMember]

        public long IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("Name");
            }
        }
        // start[GEOS2-9404][gulab lakade][18 11 2025]
        [Column("SiteName")]
        [DataMember]
        public string SiteName
        {
            get { return siteName; }
            set
            {
                siteName = value;
                OnPropertyChanged("SiteName");
            }
        }
        // End[GEOS2-9404][gulab lakade][18 11 2025]
        #endregion


        #region Constructor
        public ERM_Warehouses()
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

