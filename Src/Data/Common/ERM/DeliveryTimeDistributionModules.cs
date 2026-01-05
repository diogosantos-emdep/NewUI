using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class DeliveryTimeDistributionModules : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idDeliveryTimeDistributionModule;
        UInt64 idDeliveryTimeDistribution;
        byte idCpType;
        Int64 idCpTypeNew;
        Int32 idStage;
        float dTDRate;
        string cpTypeName;
        string code;
        Int32 createdBy;
        Int32? modifiedBy;
        DateTime createdIn;
        DateTime? modifiedIn;
        Int32 idStatus;

        #endregion Declaration

        #region Properties

        [DataMember]
        public UInt64 IdDeliveryTimeDistributionModule
        {
            get { return idDeliveryTimeDistributionModule; }
            set
            {
                idDeliveryTimeDistributionModule = value;
                OnPropertyChanged("IdDeliveryTimeDistributionModule");
            }
        }

        [DataMember]
        public UInt64 IdDeliveryTimeDistribution
        {

            get { return idDeliveryTimeDistribution; }
            set
            {
                idDeliveryTimeDistribution = value;
                OnPropertyChanged("IdDeliveryTimeDistribution");
            }
        }

        [DataMember]
        public byte IdCpType
        {
            get { return idCpType; }
            set
            {
                idCpType = value;
                OnPropertyChanged("IdCpType");
            }

        }
        //[rgadhave][GEOS2-5583][20-06-2024] 
        [DataMember]
        public Int64 IdCpTypeNew
        {
            get { return idCpTypeNew; }
            set
            {
                idCpTypeNew = value;
                OnPropertyChanged("IdCpTypeNew");
            }

        }
        [DataMember]
        public Int32 IdStage
        {

            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [DataMember]
        public float DTDRate
        {

            get { return dTDRate; }
            set
            {
                dTDRate = value;
                OnPropertyChanged("DTDRate");
            }
        }

        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public Int32? ModifiedBy
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
        public DateTime CreatedIn
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
                OnPropertyChanged("ModifiedIn");
            }
        }

        [DataMember]
        public string CpTypeName
        {
            get { return cpTypeName; }
            set
            {
                cpTypeName = value;
                OnPropertyChanged("CpTypeName");
            }
        }

        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public Int32 IdStatus
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


        #endregion Properties

        #region Constructor

        public DeliveryTimeDistributionModules()
        {

        }


        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
