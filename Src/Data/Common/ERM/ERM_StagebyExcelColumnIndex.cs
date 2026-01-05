using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
  public  class ERM_StagebyExcelColumnIndex : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idStage;
        private Int32 columnIndex;
        private string columName;
        #endregion
        #region Properties
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
        public Int32 ColumnIndex
        {
            get { return columnIndex; }
            set
            {
                columnIndex = value;
                OnPropertyChanged("ColumnIndex");
            }
        }
        [DataMember]
        public string ColumName
        {
            get { return columName; }
            set
            {
                columName = value;
                OnPropertyChanged("ColumName");
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
