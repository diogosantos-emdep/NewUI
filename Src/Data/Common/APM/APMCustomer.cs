using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{
    //[Shweta.Thube][GEOS2-6911]
    [DataContract]
    public class APMCustomer : ModelBase, IDisposable
    {
        #region Field
        private string group;
        private string site;
        private string region;
        private Int32 idSite;
        private Int32 idRegion;
        private Int32 idCustomer;
        private string groupName;
        private Int32 idCompany;

        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }
        [NotMapped]
        [DataMember]
        public string Site
        {
            get { return site; }
            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }

        [NotMapped]
        [DataMember]
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdRegion
        {
            get { return idRegion; }
            set
            {
                idRegion = value;
                OnPropertyChanged("IdRegion");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 IdCustomer
        {
            get { return idCustomer; }
            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }
        //[Shweta.Thube][GEOS2-6911]
        [NotMapped]
        [DataMember]
        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                OnPropertyChanged("GroupName");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set
            {
                idCompany = value;
                OnPropertyChanged("IdCompany");
            }
        }
        #endregion

        #region Constructor
        public APMCustomer()
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
