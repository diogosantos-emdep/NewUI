using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public class ERM_CounterPartFailurData : ModelBase, IDisposable
    {
        #region Field
       
        private Int32? idCounterpartTracking;
        private string failCode;//[GEOS2-5127][gulab lakade][20 12 2023]
        private string name;//[GEOS2-5127][gulab lakade][20 12 2023]
        private Int64 idCounterpart;
        private int? qTY;
        private string oTCode;
        private Int32? idStage;
        private Int32?  sequence;
        private string code;
        private Int32? idFailure; // [suggestion of yuvraj sir][gulab lakade][03 10 2025]
        #endregion
        #region Property


        [DataMember]
        public Int32? IdCounterpartTracking
        {
            get
            {
                return idCounterpartTracking;
            }

            set
            {
                idCounterpartTracking = value;
                OnPropertyChanged("IdCounterpartTracking");
            }
        }
       
        //[GEOS2-5127][gulab lakade][20 12 2023]
        [DataMember]
        public string FailCode
        {
            get
            {
                return failCode;
            }

            set
            {
                failCode = value;
                OnPropertyChanged("FailCode");
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
        public Int64 IdCounterpart
        {
            get
            {
                return idCounterpart;
            }

            set
            {
                idCounterpart = value;
                OnPropertyChanged("IdCounterpart");
            }
        }
        [DataMember]
        public Int32? QTY
        {
            get
            {
                return qTY;
            }

            set
            {
                qTY = value;
                OnPropertyChanged("QTY");
            }
        }
        [DataMember]
        public string OTCode
        {
            get
            {
                return oTCode;
            }

            set
            {
                oTCode = value;
                OnPropertyChanged("OTCode");
            }
        }

        [DataMember]
        public Int32? IdStage
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
        public Int32? Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                sequence = value;
                OnPropertyChanged("Sequence");
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
        // [suggestion of yuvraj sir][gulab lakade][03 10 2025]
        [DataMember]
        public Int32? IdFailure
        {
            get
            {
                return idFailure;
            }

            set
            {
                idFailure = value;
                OnPropertyChanged("IdFailure");
            }
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
