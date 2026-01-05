using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class StandardOperationsDictionaryDetection : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardOperationsDictionaryDetection;
        UInt64 idStandardOperationsDictionary;
        UInt64 idDetection;
        UInt64 idWorkoperation;
        float? observedTime;
        float? activity;
        float? normalTime;
        string detectionName;
        string workOperationName;
        bool isDeletedIdWorkOperation;
        Int32 createdBy;
        Int32? modifiedBy;
        DateTime createdIn;
        DateTime? modifiedIn;
        string remarks; //[GEOS2-3933][Rupali Sarode][22/09/2022]
        #region GEOS2-3954 Time format HH:MM:SS
        private TimeSpan uITempDetectionobservedTime;
        private TimeSpan uITempDetectionNormalTime;
        private bool isDetectionObservedTimeHoursExist;
        private bool isDetectionNormalTimeHoursExist;
        private string uIstringTempobservedTime;
        #endregion
        private UInt32? position;
        private uint detectionPosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdStandardOperationsDictionaryDetection
        {
            get
            {
                return idStandardOperationsDictionaryDetection;
            }

            set
            {
                idStandardOperationsDictionaryDetection = value;
                OnPropertyChanged("IdStandardOperationsDictionaryDetection");
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
                //    if (UITempDetectionobservedTime != temptime)
                //    {
                //        UITempDetectionobservedTime = TimeSpan.FromSeconds(i1);
                //        int ts1 = UITempDetectionobservedTime.Hours;
                //        int ts2 = UITempDetectionobservedTime.Minutes;
                //        int ts3 = UITempDetectionobservedTime.Seconds;
                //    }


                //}
                #endregion
                OnPropertyChanged("ObservedTime");
            }
        }

        #region GEOS2-3954 Time format HH:MM:SS

        public TimeSpan UITempDetectionobservedTime
        {
            get { return uITempDetectionobservedTime; }
            set
            {
                uITempDetectionobservedTime = value;
                if (UITempDetectionobservedTime.Hours > 0)
                {
                    IsDetectionObservedTimeHoursExist = true;
                }
                else
                {
                    IsDetectionObservedTimeHoursExist = false;
                }

                

                OnPropertyChanged(("UITempDetectionobservedTime"));
            }
        }
        public TimeSpan UITempDetectionNormalTime
        {
            get { return uITempDetectionNormalTime; }
            set
            {
                uITempDetectionNormalTime = value;
                if (UITempDetectionNormalTime.Hours > 0)
                {
                    IsDetectionNormalTimeHoursExist = true;
                }
                else
                {
                    IsDetectionNormalTimeHoursExist = false;
                }
                OnPropertyChanged(("UITempDetectionNormalTime"));

            }
        }
        [DataMember]
        public bool IsDetectionObservedTimeHoursExist
        {
            get { return isDetectionObservedTimeHoursExist; }
            set
            {
                isDetectionObservedTimeHoursExist = value;
                OnPropertyChanged("IsDetectionObservedTimeHoursExist");
            }
        }

        [DataMember]
        public bool IsDetectionNormalTimeHoursExist
        {
            get { return isDetectionNormalTimeHoursExist; }
            set
            {
                isDetectionNormalTimeHoursExist = value;
                OnPropertyChanged("IsDetectionNormalTimeHoursExist");
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
        public string DetectionName
        {
            get
            {
                return detectionName;
            }

            set
            {
                detectionName = value;
                OnPropertyChanged("DetectionName");
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
            {
                return remarks;
            }
            set
            {
                remarks = value;
                OnPropertyChanged("Remarks");
            }

        }

        //Aishwarya Ingale
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
        public uint DetectionPosition
        {
            get { return detectionPosition; }
            set
            {
                detectionPosition = value;
                OnPropertyChanged("DetectionPosition");
            }
        }

        #endregion

        #region Constructor

        public StandardOperationsDictionaryDetection()
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
            var newSODClone = (StandardOperationsDictionaryDetection)this.MemberwiseClone();



            return newSODClone;
        }

        #endregion


    }
}
