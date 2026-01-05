using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class StandardOperationsDictionaryWays : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardOperationsDictionaryWay;
        UInt64 idStandardOperationsDictionary;
        UInt64 idDetection;
        UInt64 idWorkoperation;
        float? observedTime;
        float? activity;
        float? normalTime;
        string optionName;
        string workOperationName;
        bool isDeletedIdWorkOperation;
        Int32 createdBy;
        Int32? modifiedBy;
        DateTime createdIn;
        DateTime? modifiedIn;
        string remarks; //[GEOS2-3933][Rupali Sarode][22/09/2022]

        #region [GEOS2-3954][Rupali Sarode]
        TimeSpan uITempWaysNormalTime; 
        TimeSpan uITempWaysObservedTime; 
        private bool isWaysObservedTimeHoursExist;
        private bool isWaysNormalTimeHoursExist;
        #endregion
        private UInt32? position;
        private uint wayPosition;////[Geos2-5629][gulab lakade][26 06 2024]
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdStandardOperationsDictionaryWay
        {
            get
            {
                return idStandardOperationsDictionaryWay;
            }

            set
            {
                idStandardOperationsDictionaryWay = value;
                OnPropertyChanged("IdStandardOperationsDictionaryWay");
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
        public UInt64 IdDetection
        {
            get
            {
                return idDetection;
            }

            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
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
                
                //#region [GEOS2-3954] [Rupali Sarode]
                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //string tempd = Convert.ToString(observedTime);
                //string[] parts = new string[2];
                //int i1 = 0;
                //int i2 = 0;
                //if (tempd.Contains(culterseparator))
                //{
                //    parts = tempd.Split(Convert.ToChar(culterseparator));
                //    i1 = int.Parse(parts[0]);
                //    i2 = int.Parse(parts[1]);

                //    if (Convert.ToString(parts[1]).Length == 1)
                //    {
                //        i1 = (i1 * 60) + i2 * 10;
                //    }
                //    else
                //    {
                //        i1 = (i1 * 60) + i2;
                //    }

                //}
                //else
                //{
                //    parts = tempd.Split(Convert.ToChar(culterseparator));
                //    i1 = int.Parse(parts[0]);
                //    i1 = (i1 * 60);
                //}



                //UITempWaysObservedTime = TimeSpan.FromSeconds(i1);
                //int ts1 = UITempWaysObservedTime.Hours;
                //int ts2 = UITempWaysObservedTime.Minutes;
                //int ts3 = UITempWaysObservedTime.Seconds;
                //#endregion

                OnPropertyChanged("ObservedTime");
            }
        }

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
        public string OptionName
        {
            get
            {
                return optionName;
            }

            set
            {
                optionName = value;
                OnPropertyChanged("OptionName");
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

        #region [GEOS2-3954][Rupali Sarode][15/10/2022]
        [DataMember]
        public TimeSpan UITempWaysObservedTime
        {
            get
            {
                return uITempWaysObservedTime;
            }

            set
            {
                uITempWaysObservedTime = value;
                if (UITempWaysObservedTime.Hours > 0)
                    IsWaysObservedTimeHoursExist = true;
                else
                    IsWaysObservedTimeHoursExist = false;
                OnPropertyChanged("UITempWaysObservedTime");
            }
        }

        [DataMember]
        public TimeSpan UITempWaysNormalTime
        {
            get
            {
                return uITempWaysNormalTime;
            }

            set
            {
                uITempWaysNormalTime = value;
                if (UITempWaysNormalTime.Hours > 0)
                    IsWaysNormalTimeHoursExist = true;
                else
                    IsWaysNormalTimeHoursExist = false;
                OnPropertyChanged("UITempWaysNormalTime");
            }
        }

        [DataMember]
        public bool IsWaysObservedTimeHoursExist
        {
            get { return isWaysObservedTimeHoursExist; }
            set
            {
                isWaysObservedTimeHoursExist = value;
                OnPropertyChanged("IsWaysObservedTimeHoursExist");
            }
        }

        [DataMember]
        public bool IsWaysNormalTimeHoursExist
        {
            get { return isWaysNormalTimeHoursExist; }
            set
            {
                isWaysNormalTimeHoursExist = value;
                OnPropertyChanged("IsWaysNormalTimeHoursExist");
            }
        }
        #endregion

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
        //[Geos2-5629][gulab lakade][26 06 2024]
        [DataMember]
        public uint WayPosition
        {
            get { return wayPosition; }
            set
            {
                wayPosition = value;
                OnPropertyChanged("WayPosition");
            }
        }
        #endregion

        #region Constructor

        public StandardOperationsDictionaryWays()
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
            var newSODClone = (StandardOperationsDictionaryWays)this.MemberwiseClone();



            return newSODClone;
        }

        #endregion
    }
}
