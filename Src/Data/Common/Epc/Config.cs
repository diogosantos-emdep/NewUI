using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("config")]
    [DataContract]
    public class Config :ModelBase,IDisposable
    {
     #region  Fields
        Int32 idConfig;
        string configValue;
        string name;
        #endregion

          #region Constructor
        public Config()
        {
        }
        #endregion

        #region Properties
       
        [Key]
        [Column("IdConfig")]
        [DataMember]
        public Int32 IdConfig
        {
            get
            {
                return idConfig;
            }
            set
            {
                idConfig = value;
                OnPropertyChanged("IdConfig");
            }
        }

        [Column("ConfigValue")]
        [DataMember]
        public string ConfigValue
        {
            get
            {
                return configValue;
            }
            set
            {
                this.configValue = value;
                OnPropertyChanged("ConfigValue");
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
