using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERM_WorkOrder_Other_ProductionTimeline : ModelBase, IDisposable
    {
        #region Field
        List<ERM_Lookup_Value> eRM_WorkOrder_Other;
        List<ERM_Lookup_Value> eRM_WorkOrder;
        List<ERM_Lookup_Value> eRM_Other;
        List<ERM_Lookup_Value> eRM_Management; //[GEOS2-6759][Daivshala Vighne][15-01-2025]
        List<ERM_Lookup_Value> eRM_Breaks; //[GEOS2-6759][Daivshala Vighne][15-01-2025]
        List<ERM_Lookup_Value> eRM_NoWorkload; //[GEOS2-6759][Daivshala Vighne][15-01-2025]
        List<ERM_Lookup_Value> eRM_Workingwithbreak; //[GEOS2-6759][Daivshala Vighne][15-01-2025]
        List<ERM_Lookup_Value> eRM_Workingwithoutbreak; //[GEOS2-6759][Daivshala Vighne][15-01-2025]
        List<ERM_Lookup_Value> eRM_OtherTime; //[GEOS2-9443] [gulab lakade][2025 10 16]
        List<ERM_Lookup_Value> eRM_RegularTime; //[GEOS2-9443] [gulab lakade][2025 10 16]
        List<ERM_Lookup_Value> eRM_OverTime; //[GEOS2-9443] [gulab lakade][2025 10 16]
        List<ERM_Lookup_Value> eRM_MeetingType; //[GEOS2-9443] [gulab lakade][2025 10 16]
        List<ERM_Lookup_Value> eRM_MaintenanceType; //[pallavi jadhav][GEOS2-9419][2025 10 30]
        #endregion

        #region Property
        [DataMember]
        public List<ERM_Lookup_Value> ERM_WorkOrder_Other
        {
            get
            {
                return eRM_WorkOrder_Other;
            }

            set
            {
                eRM_WorkOrder_Other = value;
                OnPropertyChanged("ERM_WorkOrder_Other");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_WorkOrder
        {
            get
            {
                return eRM_WorkOrder;
            }

            set
            {
                eRM_WorkOrder = value;
                OnPropertyChanged("ERM_WorkOrder");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_Other
        {
            get
            {
                return eRM_Other;
            }

            set
            {
                eRM_Other = value;
                OnPropertyChanged("ERM_Other");
            }
        }

        #region [GEOS2-6759][Daivshala Vighne][14-01-2025]
        [DataMember]
        public List<ERM_Lookup_Value> ERM_Management
        {
            get
            {
                return eRM_Management;
            }

            set
            {
                eRM_Management = value;
                OnPropertyChanged("ERM_Management");
            }
        }

        [DataMember]
        public List<ERM_Lookup_Value> ERM_Breaks
        {
            get
            {
                return eRM_Breaks;
            }

            set
            {
                eRM_Breaks = value;
                OnPropertyChanged("ERM_Breaks");
            }
        }


        [DataMember]
        public List<ERM_Lookup_Value> ERM_NoWorkload
        {
            get
            {
                return eRM_NoWorkload;
            }

            set
            {
                eRM_NoWorkload = value;
                OnPropertyChanged("ERM_NoWorkload");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_Workingwithbreak
        {
            get
            {
                return eRM_Workingwithbreak;
            }

            set
            {
                eRM_Workingwithbreak = value;
                OnPropertyChanged("ERM_Workingwithbreak");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_Workingwithoutbreak
        {
            get
            {
                return eRM_Workingwithoutbreak;
            }

            set
            {
                eRM_Workingwithoutbreak = value;
                OnPropertyChanged("ERM_Workingwithoutbreak");
            }
        }
        #endregion
        #region [GEOS2-9443] [gulab lakade][2025 10 16]
        [DataMember]
        public List<ERM_Lookup_Value> ERM_OtherTime
        {
            get
            {
                return eRM_OtherTime;
            }

            set
            {
                eRM_OtherTime = value;
                OnPropertyChanged("ERM_OtherTime");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_RegularTime
        {
            get
            {
                return eRM_RegularTime;
            }

            set
            {
                eRM_RegularTime = value;
                OnPropertyChanged("ERM_RegularTime");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_OverTime
        {
            get
            {
                return eRM_OverTime;
            }

            set
            {
                eRM_OverTime = value;
                OnPropertyChanged("ERM_OverTime");
            }
        }
        [DataMember]
        public List<ERM_Lookup_Value> ERM_MeetingType
        {
            get
            {
                return eRM_MeetingType;
            }

            set
            {
                eRM_MeetingType = value;
                OnPropertyChanged("ERM_MeetingType");
            }
        }
        [DataMember]
        //[pallavi jadhav][GEOS2-9419][2025 10 30]
        
        public List<ERM_Lookup_Value> ERM_MaintenanceType
        {
            get
            {
                return eRM_MaintenanceType;
            }

            set
            {
                eRM_MaintenanceType = value;
                OnPropertyChanged("ERM_MaintenanceType");
            }
        }
        //[pallavi jadhav][GEOS2-9419][2025 10 30]
      
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
