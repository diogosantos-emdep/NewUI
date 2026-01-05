using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;
using System.Globalization;

namespace Emdep.Geos.Data.Common.ERM
{
    /// <summary>
    /// StandardOperationsDictionary class is created by renaming StandardTime class
    /// </summary>
    [DataContract]
    public class StandardOperationsDictionaryModules : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardOperationsDictionaryModules;
        UInt64 idStandardOperationsDictionary;
        byte idCpType;
        Int64 idCpTypeNew;
        UInt64 idWorkoperation;
        float? observedTime;
        float? activity;
        float? normalTime;
        string cpTypeName;
        string workOperationName;
        bool isDeletedIdWorkOperation;
        Int32 createdBy;
        Int32? modifiedBy;
        DateTime createdIn;
        DateTime? modifiedIn;
        
        string remarks; //[GEOS2-3933][Rupali Sarode][22/09/2022]
        #region GEOS2-3954 Time format HH:MM:SS
        private TimeSpan uITempobservedTime;
        private TimeSpan uITempNormalTime;
        private bool isObservedTimeHoursExist;
        private bool isNormalTimeHoursExist;
        private string uIstringTempobservedTime;
        #endregion
        private UInt32? position;
        private uint modulePosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
        private int? idStage;
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdStandardOperationsDictionaryModules
        {
            get
            {
                return idStandardOperationsDictionaryModules;
            }

            set
            {
                idStandardOperationsDictionaryModules = value;
                OnPropertyChanged("IdStandardOperationsDictionaryModules");
            }
        }

        [DataMember]
        public UInt64 IdStandardOperationsDictionary
        {
            get
            {
                return idStandardOperationsDictionary;
            }

            set
            {
                idStandardOperationsDictionary = value;
                OnPropertyChanged("IdStandardOperationsDictionary");
            }
        }


        [DataMember]
        public byte IdCpType
        {
            get
            {
                return idCpType;
            }

            set
            {
                idCpType = value;
                OnPropertyChanged("IdCpType");
            }
        }
        //[rgadhave][GEOS2-5583][20-06-2024] 
        [DataMember]
        public Int64 IdCpTypeNew
        {
            get
            {
                return idCpTypeNew;
            }

            set
            {
                idCpTypeNew = value;
                OnPropertyChanged("IdCpTypeNew");
            }
        }
        [DataMember]
        public UInt64 IdWorkoperation
        {
            get
            {
                return idWorkoperation;
            }

            set
            {
                idWorkoperation = value;
                OnPropertyChanged("IdWorkoperation");
            }
        }
        [DataMember]
        public float? ObservedTime
        {
            get
            {
                return observedTime;
            }

            set
            {
                observedTime = value;
                //UITempobservedTime = TimeSpan.FromMinutes(Convert.ToDouble(observedTime));
                #region GEOS2-3954 Time format HH:MM:SS
                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //if (observedTime != null && observedTime > 0)
                //{

                //    string tempd = Convert.ToString(observedTime);
                //    string[] parts = new string[2];
                //    int i1 = 0;
                //    int i2 = 0;
                //    if (tempd.Contains(culterseparator))
                //    {
                //        parts = tempd.Split(Convert.ToChar(culterseparator));
                //        i1 = int.Parse(parts[0]);
                //        i2 = int.Parse(parts[1]);
                //        if (Convert.ToString(parts[1]).Length == 1)
                //        {
                //            i1 = (i1 * 60) + i2 * 10;
                //        }
                //        else
                //        {
                //            i1 = (i1 * 60) + i2;
                //        }
                //    }
                //    else
                //    {
                //        parts = tempd.Split(Convert.ToChar(culterseparator));
                //        i1 = int.Parse(parts[0]);
                //        i1 = (i1 * 60);
                //    }

                //    TimeSpan temptime = TimeSpan.FromSeconds(i1);
                //    if (UITempobservedTime != temptime)
                //    {
                //        UITempobservedTime = TimeSpan.FromSeconds(i1);
                //        int ts1 = UITempobservedTime.Hours;
                //        int ts2 = UITempobservedTime.Minutes;
                //        int ts3 = UITempobservedTime.Seconds;
                //    }


                //}
                #endregion
                OnPropertyChanged("ObservedTime");
            }
        }
        
        #region GEOS2-3954 Time format HH:MM:SS

        public TimeSpan UITempobservedTime
        {
            get { return uITempobservedTime; }
            set
            {
                uITempobservedTime = value;
                if (UITempobservedTime.Hours > 0)
                {
                    IsObservedTimeHoursExist = true;
                }
                else
                {
                    IsObservedTimeHoursExist = false;
                }

                //if (observedTime != null && observedTime > 0)
                //{


                //    if (observedTime != null && observedTime > 0 && Activity > 0)
                //        NormalTime = (float)Math.Round((float)observedTime * ((float)Activity / 100), 2);
                //    else
                //        NormalTime = 0;

                //    string temnormaltime = Convert.ToString(NormalTime);
                //    string[] NormaltimeArr = new string[2];
                //    int nt1 = 0;
                //    int nt2 = 0;
                //    if (temnormaltime.Contains("."))
                //    {
                //        NormaltimeArr = temnormaltime.Split('.');
                //        nt1 = int.Parse(NormaltimeArr[0]);
                //        nt2 = int.Parse(NormaltimeArr[1]);
                //        nt1 = (nt1 * 60) + nt2;
                //    }
                //    else
                //    {
                //        NormaltimeArr = temnormaltime.Split('.');
                //        nt1 = int.Parse(NormaltimeArr[0]);
                //        nt1 = (nt1 * 60);
                //    }



                //    UITempNormalTime = TimeSpan.FromSeconds(nt1);
                //}

                OnPropertyChanged(("UITempobservedTime"));
            }
        }
        public TimeSpan UITempNormalTime
        {
            get { return uITempNormalTime; }
            set
            {
                uITempNormalTime = value;
                if (UITempNormalTime.Hours > 0)
                {
                    IsNormalTimeHoursExist = true;
                }
                else
                {
                    IsNormalTimeHoursExist = false;
                }
                OnPropertyChanged(("UITempNormalTime"));

            }
        }
        [DataMember]
        public bool IsObservedTimeHoursExist
        {
            get { return isObservedTimeHoursExist; }
            set
            {
                isObservedTimeHoursExist = value;
                OnPropertyChanged("IsObservedTimeHoursExist");
            }
        }

        [DataMember]
        public bool IsNormalTimeHoursExist
        {
            get { return isNormalTimeHoursExist; }
            set
            {
                isNormalTimeHoursExist = value;
                OnPropertyChanged("IsNormalTimeHoursExist");
            }
        }
        #endregion

        [DataMember]
        public float? Activity
        {
            get
            {
                return activity;
            }
            set
            {
                activity = value;
                OnPropertyChanged("Activity");
            }

        }
    
        [DataMember]
        public float? NormalTime
        {
            get
            {
                return normalTime;
            }

            set
            {
                normalTime = value;
                OnPropertyChanged("NormalTime");
            }
        }
      

        [DataMember]
        public string CPTypeName
        {
            get
            {
                return cpTypeName;
            }

            set
            {
                cpTypeName = value;
                OnPropertyChanged("CPTypeName");
            }
        }


        [DataMember]
        public string WorkOperationName
        {
            get
            {
                return workOperationName;
            }

            set
            {
                workOperationName = value;
                OnPropertyChanged("WorkOperationName");
            }
        }

        [DataMember]
        public bool IsDeletedIdWorkOperation
        {
            get
            {
                return isDeletedIdWorkOperation;
            }

            set
            {
                isDeletedIdWorkOperation = value;
                OnPropertyChanged("IsDeletedIdWorkOperation");
            }
        }

        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [DataMember]
        public Int32? ModifiedBy
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

        [DataMember]
        public DateTime CreatedIn
        {
            get
            {
                return createdIn;
            }

            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [DataMember]
        public DateTime? ModifiedIn
        {
            get
            {
                return modifiedIn;
            }

            set
            {
                modifiedIn = value;
                OnPropertyChanged("ModifiedIn");
            }
        }

        //[GEOS2-3933][Rupali Sarode][22/09/2022]
        [DataMember]
        public string Remarks
        {
            get
            { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        //Aishwarya Ingale[18/06/2024][Geos2-5629]
        [DataMember]
        public UInt32? Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        //Aishwarya Ingale[Geos2-5629][27-06-2024]
        [DataMember]
        public uint ModulePosition
        {
            get { return modulePosition; }
            set
            {
                modulePosition = value;
                OnPropertyChanged("ModulePosition");
            }
        }

        public int? IdStage
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
        #endregion

        #region Constructor

        public StandardOperationsDictionaryModules()
        {
        }

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            var newSODClone = (StandardOperationsDictionaryModules)this.MemberwiseClone();

          

            return newSODClone;
        }

        #endregion
    }
}
