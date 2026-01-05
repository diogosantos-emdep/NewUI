using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
   public class ERM_MonthlyProductionTimeline : ModelBase, IDisposable
    {
        #region Field
        List<ERM_ProductionTimeline> eRM_ProductionTimeline;////[GEOS2-5627][gulab lakade][07 052 2024]
        List<ERM_Lookup_Value> overTime;////[GEOS2-5627][gulab lakade][07 052 2024]
        List<ERM_Lookup_Value> otherTime;////[GEOS2-5627][gulab lakade][07 052 2024]
        List<ERM_Lookup_Value> regularTime;////[GEOS2-6529][gulab lakade][22 10 2024]
        List<ERM_Lookup_Value> managementTime; //[GEOS2-6759][Daivshala Vighne][13-01-2025]
        List<ERM_Lookup_Value> breakTime; //[GEOS2-6759][Daivshala Vighne][13-01-2025]
        List<DesignSharedItemsEmployeeDetails> designSharedItemsEmployeeDetailsList; //[pallavi.jadhav][GEOS2-8550][23 09 2025]
        #endregion

        #region Property
        [DataMember]
        public List<ERM_ProductionTimeline> ERM_ProductionTimeline
        {
            get
            {
                return eRM_ProductionTimeline;
            }

            set
            {
                eRM_ProductionTimeline = value;
                OnPropertyChanged("ERM_ProductionTimeline");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> OverTime
        {
            get
            {
                return overTime;
            }

            set
            {
                overTime = value;
                OnPropertyChanged("OverTime");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> OtherTime
        {
            get
            {
                return otherTime;
            }

            set
            {
                otherTime = value;
                OnPropertyChanged("OtherTime");
            }
        }
        ////start[GEOS2-6529][gulab lakade][22 10 2024]
        [DataMember]
        public List<ERM_Lookup_Value> RegularTime
        {
            get
            {
                return regularTime;
            }

            set
            {
                regularTime = value;
                OnPropertyChanged("RegularTime");
            }
        }
        //// end[GEOS2-6529][gulab lakade][22 10 2024]




        ////start[GEOS2-6759][Daivshala Vighne][13-01-2025]
        [DataMember]
        public List<ERM_Lookup_Value> ManagementTime
        {
            get
            {
                return managementTime;
            }

            set
            {
                managementTime = value;
                OnPropertyChanged("ManagementTime");
            }
        }

        public List<ERM_Lookup_Value> BreakTime
        {
            get
            {
                return breakTime;
            }

            set
            {
                breakTime = value;
                OnPropertyChanged("BreakTime");
            }
        }


        //// end[GEOS2-6759][Daivshala Vighne][13-01-2025]

        #region [pallavi.jadhav][GEOS2-8550][23 09 2025]
        public List<DesignSharedItemsEmployeeDetails> DesignSharedItemsEmployeeDetailsList
        {
            get
            {
                return designSharedItemsEmployeeDetailsList;
            }

            set
            {
                designSharedItemsEmployeeDetailsList = value;
                OnPropertyChanged("DesignSharedItemsEmployeeDetailsList");
            }
        }
        #endregion

        #endregion


        #region method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
