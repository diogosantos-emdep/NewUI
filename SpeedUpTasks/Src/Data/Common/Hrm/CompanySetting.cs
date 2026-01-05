using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("company_settings")]
    [DataContract]
    public class CompanySetting : ModelBase, IDisposable
    {

        #region Fields

        Int64 idCompanySetting;
        Int32 idCompany;
        Int32 idAppSetting;
        DateTime? endDate;
        DateTime? startDate;
        string _value;
        Company company;
        GeosAppSetting geosAppSetting;
        #endregion

        #region Properties

        [Key]
        [Column("IdCompanySetting")]
        [DataMember]
        public Int64 IdCompanySetting
        {
            get { return idCompanySetting; }
            set
            {
                idCompanySetting = value;
                OnPropertyChanged("IdCompanySetting");
            }
        }

        [Column("IdCompany")]
        [DataMember]
        public int IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }

        [Column("IdAppSetting")]
        [DataMember]
        public Int32 IdAppSetting
        {
            get { return idAppSetting; }
            set
            {
                idAppSetting = value;
                OnPropertyChanged("IdAppSetting");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [Column("EndDate")]
        [DataMember]
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
               endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [Column("Value")]
        [DataMember]
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        [NotMapped]
        [DataMember]
        public GeosAppSetting GeosAppSetting
        {
            get { return geosAppSetting; }
            set
            {
                geosAppSetting = value;
                OnPropertyChanged("GeosAppSetting");
            }
        }


        #endregion

        #region Constructor

        public CompanySetting()
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
