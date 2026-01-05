using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class ModulesEquivalencyWeight : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idCPType;
        private string code;
        private string name;
        private Int32 idTemplate;
        private string templateName;
        private Int16 idStatus;
        private string status;
        private float? equivalentWeight;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime? lastUpdate;
        private DateTime? createdIn;
        private DateTime? modifiedIn;
        private string statusHtmlColor;
        private uint? createdBy;
        private UInt32 modifiedBy;
        private string color;
        private string rowBackColor;
        private UInt64 iDCPTypeEquivalent;  //[GEOS2-4330][Rupali Sarode][11-04-2023]
        List<EquivalencyWeight> lstEquivalencyWeight;
        List<LogEntryByModuleEquivalenceWeight> lstLogEntryByModuleEquivalenceWeight;
        private bool flagIdCPType;
        #endregion

        #region Properites

        public bool FlagIdCPType
        {
            get
            {
                return flagIdCPType;
            }

            set
            {
                flagIdCPType = value;
                OnPropertyChanged("FlagIdCPType");
            }
        }
        [DataMember]
        public Int32 IdCPType
        {
            get
            {
                return idCPType;
            }

            set
            {
                idCPType = value;
                OnPropertyChanged("IdCPType");
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

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
        public Int32 IdTemplate
        {
            get
            {
                return idTemplate;
            }

            set
            {
                idTemplate = value;
                OnPropertyChanged("IdTemplate");
            }
        }

        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }

        [DataMember]
        public Int16 IdStatus
        {
            get
            {
                return idStatus;
            }

            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public float? EquivalentWeight
        {
            get
            {
                return equivalentWeight;
            }

            set
            {
                equivalentWeight = value;
                OnPropertyChanged("EquivalentWeight");
            }
        }

        [DataMember]
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        [DataMember]
        public DateTime? LastUpdate
        {
            get
            {
                return lastUpdate;
            }

            set
            {
                lastUpdate = value;
                OnPropertyChanged("LastUpdate");
            }
        }

        [DataMember]
        public string StatusHtmlColor
        {
            get
            {
                return statusHtmlColor;
            }

            set
            {
                statusHtmlColor = value;
                OnPropertyChanged("StatusHtmlColor");
            }
        }
        [DataMember]
        public string Color
        {
            get { return color; }
            set
            {
                this.color = value;
                OnPropertyChanged("Color");
            }
        }

        //[GEOS2-4330][Rupali Sarode][11-04-2023]
        [NotMapped]
        [DataMember]
        public UInt64 IDCPTypeEquivalent
        {
            get
            {
                return iDCPTypeEquivalent;
            }

            set
            {
                iDCPTypeEquivalent = value;
                OnPropertyChanged("IDCPTypeEquivalent");
            }
        }
        [DataMember]
        public DateTime? CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }
        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public UInt32 ModifiedBy
        {
            get
            {
                return modifiedBy;
            }

            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public uint? CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [DataMember]
        public List<EquivalencyWeight> LstEquivalencyWeight
        {
            get
            {
                return lstEquivalencyWeight;
            }

            set
            {
                lstEquivalencyWeight = value;
                OnPropertyChanged("LstEquivalencyWeight");
            }
        }

        [DataMember]
        public List<LogEntryByModuleEquivalenceWeight> LstLogEntryByModuleEquivalenceWeight
        {
            get
            {
                return lstLogEntryByModuleEquivalenceWeight;
            }

            set
            {
                lstLogEntryByModuleEquivalenceWeight = value;
                OnPropertyChanged("LstLogEntryByModuleEquivalenceWeight");
            }
        }
        public string RowBackColor
        {
            get
            {
                return rowBackColor;
            }

            set
            {
                rowBackColor = value;
                OnPropertyChanged("RowBackColor");
            }
        }
        #endregion
        #region Constructor

        public ModulesEquivalencyWeight()
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
            var newMEWone = (ModulesEquivalencyWeight)this.MemberwiseClone();

            if (LstEquivalencyWeight != null)
                newMEWone.LstEquivalencyWeight = LstEquivalencyWeight.Select(x => (EquivalencyWeight)x.Clone()).ToList();
            if (LstLogEntryByModuleEquivalenceWeight != null)
                newMEWone.LstLogEntryByModuleEquivalenceWeight = LstLogEntryByModuleEquivalenceWeight.Select(x => (LogEntryByModuleEquivalenceWeight)x.Clone()).ToList();

            return newMEWone;
        }
        #endregion

    }
}
