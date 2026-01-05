using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class GeosAppSetting : ModelBase, IDisposable
    {
        #region Declaration

        Int16 idAppSetting;
        string appSettingName;
        byte? isUserModify;
        string defaultValue;

        [Key]
        [Column("IdAppSetting")]
        [DataMember]
        public short IdAppSetting
        {
            get { return idAppSetting; }
            set
            {
                idAppSetting = value;
                OnPropertyChanged("IdAppSetting");
            }
        }

        [Column("AppSettingName")]
        [DataMember]
        public string AppSettingName
        {
            get { return appSettingName; }
            set
            {
                appSettingName = value;
                OnPropertyChanged("AppSettingName");
            }
        }

        [Column("IsUserModify")]
        [DataMember]
        public byte? IsUserModify
        {
            get { return isUserModify; }
            set
            {
                isUserModify = value;
                OnPropertyChanged("IsUserModify");
            }
        }

        [Column("DefaultValue")]
        [DataMember]
        public string DefaultValue
        {
            get { return defaultValue; }
            set
            {
                defaultValue = value;
                OnPropertyChanged("DefaultValue");
            }
        }

        #endregion

        #region Constructor

        public GeosAppSetting()
        {
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
