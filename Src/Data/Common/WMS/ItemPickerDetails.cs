using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.WMS
{
    [DataContract]
    public class ItemPickerDetails : ModelBase, IDisposable
    {
        // created by [rani dhamankar][16-04-2025][GEOS2-7023]

        #region Declaration
        string modifiedByName;
        string modifiedBySurName;
        Int64 modifiedBy;
        #endregion

        #region Properties
        [DataMember]
        public string ModifiedByName

        {
            get { return modifiedByName; }
            set
            {
                modifiedByName = value;
                OnPropertyChanged("ModifiedByName");

            }
        }
        [DataMember]
        public string ModifiedBySurName

        {
            get { return modifiedBySurName; }
            set
            {
                modifiedBySurName = value;
                OnPropertyChanged("ModifiedBySurName");

            }
        }

      

        [DataMember]
        public Int64 ModifiedBy
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

        
        #endregion

        #region Constructor

        public ItemPickerDetails()
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

