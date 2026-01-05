using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
  
    [DataContract]
    public class WorkOrderType : ModelBase, IDisposable
    {
        #region Fields

        Int64 typeCount;
        string htmlColor;
        #endregion

        #region Constructor
        public WorkOrderType()
        {

        }
        #endregion

        #region Properties

     
        [DataMember]
        public Int64 TypeCount
        {
            get { return typeCount; }
            set
            {
                typeCount = value;
                OnPropertyChanged("TypeCount");
            }
        }

      
        [DataMember]
        public string HtmlColor
        {
            get { return htmlColor; }
            set
            {
                htmlColor = value;
                OnPropertyChanged("HtmlColor");
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
