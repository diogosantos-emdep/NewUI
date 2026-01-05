using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class DesignSystemTypeStatus : ModelBase, IDisposable
    {
        #region Field
        private string designSystem;
        private string designType;
        private string dSAStatus;
        private Int32 dSA_Status_sequence;  // [GEOS2-6874][gulab lakade][31 01 2025]
        #endregion
        #region Property

        [DataMember]
        public string DesignSystem
        {
            get
            {
                return designSystem;
            }

            set
            {
                designSystem = value;
                OnPropertyChanged("DesignSystem");
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
        public string DSAStatus
        {
            get
            {
                return dSAStatus;
            }

            set
            {
                dSAStatus = value;
                OnPropertyChanged("DSAStatus");
            }
        }
        // [GEOS2-6874][gulab lakade][31 01 2025]
        [DataMember]
        public Int32 DSA_Status_sequence
        {
            get
            {
                return dSA_Status_sequence;
            }

            set
            {
                dSA_Status_sequence = value;
                OnPropertyChanged("DSA_Status_sequence");
            }
        }
        // [GEOS2-6874][gulab lakade][31 01 2025]ssssss
        #endregion

        #region Constructor
        public DesignSystemTypeStatus()
        {

        }
        #endregion
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
