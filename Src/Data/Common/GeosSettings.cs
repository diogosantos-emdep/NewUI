using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class GeosSettings : ModelBase, IDisposable
    {
        #region Declaration
        string settingName;
        string settingValue;
        string description;


        

        [Column("SettingName")]
        [DataMember]
        public string SettingName
        {
            get { return settingName; }
            set
            {
                settingName = value;
                OnPropertyChanged("SettingName");
            }
        }

        [Column("SettingValue")]
        [DataMember]
        public string SettingValue
        {
            get { return settingValue; }
            set
            {
                settingValue = value;
                OnPropertyChanged("SettingValue");
            }
        }
        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        #endregion

        #region Constructor

        public GeosSettings()
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
