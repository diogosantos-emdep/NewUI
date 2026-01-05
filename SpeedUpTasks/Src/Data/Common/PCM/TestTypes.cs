using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.PCM
{
    [DataContract]
    public class TestTypes : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idTestType;
        string name;

        #endregion

        #region Constructor

        public TestTypes()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdTestType
        {
            get { return idTestType; }
            set
            {
                idTestType = value;
                OnPropertyChanged("IdTestType");
            }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
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
