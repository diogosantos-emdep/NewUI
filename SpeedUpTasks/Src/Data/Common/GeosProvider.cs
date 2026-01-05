using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("geos_providers")]
    [DataContract]
    public class GeosProvider
    {
        #region Fields
        Int32 idCompany;
        Int32 idGeosProvider;
        string serviceServerPrivateIP;
        Int32? serviceServerPrivatePort;
        string serviceServerPublicIP;
        Int32? serviceServerPublicPort;
        Int32? testingServerPort;
        Int32? testingServerIP;
        string serviceProviderUrl;
        #endregion

        #region Properties

        [Key]
        [Column("IdGeosProvider")]
        [DataMember]
        public Int32 IdGeosProvider
        {
            get { return idGeosProvider; }
            set { idGeosProvider = value; }
        }

        [ForeignKey("Company")]
        [Column("IdCompany")]
        [DataMember]
        public Int32 IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }

        [Column("ServiceServerPrivateIP")]
        [DataMember]
        public string ServiceServerPrivateIP
        {
            get { return serviceServerPrivateIP; }
            set { serviceServerPrivateIP = value; }
        }

        [Column("ServiceServerPrivatePort")]
        [DataMember]
        public Int32? ServiceServerPrivatePort
        {
            get { return serviceServerPrivatePort; }
            set { serviceServerPrivatePort = value; }
        }

        [Column("ServiceServerPublicIP")]
        [DataMember]
        public string ServiceServerPublicIP
        {
            get { return serviceServerPublicIP; }
            set { serviceServerPublicIP = value; }
        }

        [Column("ServiceServerPublicPort")]
        [DataMember]
        public Int32? ServiceServerPublicPort
        {
            get { return serviceServerPublicPort; }
            set { serviceServerPublicPort = value; }
        }

        [Column("TestingServerIP")]
        [DataMember]
        public Int32? TestingServerIP
        {
            get { return testingServerIP; }
            set { testingServerIP = value; }
        }

        [Column("TestingServerPort")]
        [DataMember]
        public Int32? TestingServerPort
        {
            get { return testingServerPort; }
            set { testingServerPort = value; }
        }

        [DataMember]
        public virtual Company Company { get; set; }

        [Column("ServiceProviderUrl")]
        [DataMember]
        public string ServiceProviderUrl
        {
            get { return serviceProviderUrl; }
            set { serviceProviderUrl = value; }
        }

        #endregion
    }
}