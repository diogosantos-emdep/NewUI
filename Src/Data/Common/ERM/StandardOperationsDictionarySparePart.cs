using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.ERM
{
    public class StandardOperationsDictionarySparePart : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardOperationsDictionarySpareParts;
        UInt64 idStandardOperationsDictionary;
        UInt64 idDetection;
        UInt64 idWorkoperation;
        float? observedTime;
        float? activity;
        float? normalTime;
        string sparePartName;
        string workOperationName;
        bool isDeletedIdWorkOperation;
        Int32 createdBy;
        Int32? modifiedBy;
        DateTime createdIn;
        DateTime? modifiedIn;

        string remarks; //[GEOS2-3933][Rupali Sarode][22/09/2022]

        TimeSpan uITempSparePartNormalTime;
        TimeSpan uITempSparePartObservedTime;
        private bool isSparePartObservedTimeHoursExist;
        private bool isSparePartNormalTimeHoursExist;
        private UInt32? position;
        private uint sparepartPosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdStandardOperationsDictionarySpareParts
        {
            get
            {
                return idStandardOperationsDictionarySpareParts;
            }

            set
            {
                idStandardOperationsDictionarySpareParts = value;
                OnPropertyChanged("IdStandardOperationsDictionarySpareParts");
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

                #region [GEOS2-3954] [Rupali Sarode][17/10/2022]
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



                //UITempSparePartObservedTime = TimeSpan.FromSeconds(i1);
                //int ts1 = UITempSparePartObservedTime.Hours;
                //int ts2 = UITempSparePartObservedTime.Minutes;
                //int ts3 = UITempSparePartObservedTime.Seconds;
                #endregion

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
        public string SparePartName
        {
            get
            {
                return sparePartName;
            }

            set
            {
                sparePartName = value;
                OnPropertyChanged("SparePartName");
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

        #region [GEOS2-3954][Rupali Sarode]
        [DataMember]
        public TimeSpan UITempSparePartObservedTime
        {
            get
            {
                return uITempSparePartObservedTime;
            }

            set
            {
                uITempSparePartObservedTime = value;
                if (UITempSparePartObservedTime.Hours > 0)
                    IsSparePartObservedTimeHoursExist = true;
                else
                    IsSparePartObservedTimeHoursExist = false;
                OnPropertyChanged("UITempSparePartObservedTime");

            }
        }

        [DataMember]
        public TimeSpan UITempSparePartNormalTime
        {
            get
            {
                return uITempSparePartNormalTime;
            }

            set
            {
                uITempSparePartNormalTime = value;
                if (UITempSparePartNormalTime.Hours > 0)
                    isSparePartNormalTimeHoursExist = true;
                else
                    isSparePartNormalTimeHoursExist = false;
                OnPropertyChanged("UITempSparePartNormalTime");
            }
        }

        [DataMember]
        public bool IsSparePartObservedTimeHoursExist
        {
            get { return isSparePartObservedTimeHoursExist; }
            set
            {
                isSparePartObservedTimeHoursExist = value;
                OnPropertyChanged("IsSparePartObservedTimeHoursExist");
            }
        }

        [DataMember]
        public bool IsSparePartNormalTimeHoursExist
        {
            get { return isSparePartNormalTimeHoursExist; }
            set
            {
                isSparePartNormalTimeHoursExist = value;
                OnPropertyChanged("IsSparePartNormalTimeHoursExist");
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
        //Aishwarya Ingale[Geos2-5629][27-06-2024]
        [DataMember]
        public uint SparepartPosition
        {
            get { return sparepartPosition; }
            set
            {
                sparepartPosition = value;
                OnPropertyChanged("SparepartPosition");
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

        public override object Clone()
        {
            var newSODClone = (StandardOperationsDictionarySparePart)this.MemberwiseClone();



            return newSODClone;
        }

        #endregion
    }
}
