using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{
	// [nsatpute][09-10-2024][GEOS2-5975]
    public class TaskStatuswise : ModelBase, IDisposable
    {
        #region Constructor
        public TaskStatuswise()
        {
            TaskList = new List<APMActionPlanTask>();
        }
        #endregion

        #region Declaration
        string htmlColor;
        Int32 idLookupValue;
        string value;
        List<APMActionPlanTask> taskList;
        #endregion

        #region Properties
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
        [DataMember]
        public Int32 IdLookupValue
        {
            get { return idLookupValue; }
            set
            {
                idLookupValue = value;
                OnPropertyChanged("IdLookupValue");
            }
        }


        [DataMember]
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }


        [DataMember]
        public List<APMActionPlanTask> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                OnPropertyChanged("TaskList");
            }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);  
        }
    }
}
