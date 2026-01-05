using Emdep.Geos.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class CPOperationsTime: ModelBase, IDisposable
    {


        #region Fields
        private string parent;
        private string operation;
        private float? time;
        #endregion


        #region Properties
        
        [DataMember]
        public string Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }
        [DataMember]
        public string Operation
        {
            get
            {
                return operation;
            }

            set
            {
                operation = value;
                OnPropertyChanged("Operation");
            }
        }

        [DataMember]
        public float? Time
        {
            get { return time; }
            set
            {
                time = value;
                OnPropertyChanged("Time");
            }
        }




        #endregion


        #region Constructor

        #endregion

        #region Methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
       
        #endregion
    }
}
