using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public  class ERM_CADCAMTimePerDesignType : ModelBase, IDisposable
    {
        #region Field
        private Int32 idStage;
        private string designType;
        private Int32 designValue;
        private string roleValue;
        #endregion
        #region Property
        [DataMember]
        public Int32 IdStage
        {
            get
            {
                return idStage;
            }

            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [DataMember]
        public string DesignType
        {
            get
            {
                return designType;
            }

            set
            {
                designType = value;
                OnPropertyChanged("DesignType");
            }
        }

        [DataMember]
        public Int32 DesignValue
        {
            get
            {
                return designValue;
            }

            set
            {
                designValue = value;
                OnPropertyChanged("DesignValue");
            }
        }

        [DataMember]
        public string RoleValue
        {
            get
            {
                return roleValue;
            }

            set
            {
                roleValue = value;
                OnPropertyChanged("RoleValue");
            }
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
