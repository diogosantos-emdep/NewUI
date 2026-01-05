using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4971]
    [DataContract]
    public class NumbersMarginByConfiguration : ModelBase, IDisposable
    {
        #region Declarations
        private uint idSearchConfiguration;
        private Int32 idNumberMargin;
        private double margin;
        private Int32 createdBy;
        private DateTime createdIn;
        private Int32 modifiedBy;
        private DateTime modifiedIn;
        #endregion

        #region Properties
        [DataMember]
        public uint IdSearchConfiguration
        {
            get { return idSearchConfiguration; }
            set
            {
                idSearchConfiguration = value;
                OnPropertyChanged("IdSearchConfiguration");
            }
        }

        [DataMember]
        public Int32 IdNumberMargin
        {
            get { return idNumberMargin; }
            set
            {
                idNumberMargin = value;
                OnPropertyChanged("IdNumberMargin");
            }
        }
        [DataMember]
        public double Margin
        {
            get { return margin; }
            set
            {
                margin = value;
                OnPropertyChanged("Margin");
            }
        }
        [DataMember]
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public Int32 ModifiedBy
        {
            get { return modifiedBy; }
            set
            {
                modifiedBy = value;
                OnPropertyChanged("ModifiedBy");
            }
        }
        [DataMember]
        public DateTime ModifiedIn
        {
            get { return modifiedIn; }
            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }
        #endregion

        #region Constructor
        public NumbersMarginByConfiguration()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
