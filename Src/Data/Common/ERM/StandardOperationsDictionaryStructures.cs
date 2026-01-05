using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.Data.Common.ERM
{
    [DataContract]
    public class StandardOperationsDictionaryStructures : ModelBase, IDisposable
    {
        #region Declaration

        UInt64 idStandardOperationsDictionaryStructures;
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
        string remarks;

        #region [GEOS2-3954][Rupali Sarode][17/10/2022]
        TimeSpan uITempStructureNormalTime; 
        TimeSpan uITempStructureObservedTime; 
        private bool isStructureObservedTimeHoursExist;
        private bool isStructureNormalTimeHoursExist;
        #endregion

        private UInt32? position;
        private uint structurePosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdStandardOperationsDictionaryStructures
        {
            get
            {
                return idStandardOperationsDictionaryStructures;
            }

            set
            {
                idStandardOperationsDictionaryStructures = value;
                OnPropertyChanged("IdStandardOperationsDictionaryStructures");
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



                //UITempStructureObservedTime = TimeSpan.FromSeconds(i1);
                //int ts1 = UITempStructureObservedTime.Hours;
                //int ts2 = UITempStructureObservedTime.Minutes;
                //int ts3 = UITempStructureObservedTime.Seconds;
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


        #region [GEOS2-3954][Rupali Sarode][17/10/2022]
        [DataMember]
        public TimeSpan UITempStructureObservedTime
        {
            get
            {
                return uITempStructureObservedTime;
            }

            set
            {
                uITempStructureObservedTime = value;
                if (UITempStructureObservedTime.Hours > 0)
                    IsStructureObservedTimeHoursExist = true;
                else
                    IsStructureObservedTimeHoursExist = false;
                OnPropertyChanged("UITempStructureObservedTime");
            }
        }

        [DataMember]
        public TimeSpan UITempStructureNormalTime
        {
            get
            {
                return uITempStructureNormalTime;
            }

            set
            {
                uITempStructureNormalTime = value;
                if (UITempStructureNormalTime.Hours > 0)
                    IsStructureNormalTimeHoursExist = true;
                else
                    IsStructureNormalTimeHoursExist = false;
                OnPropertyChanged("UITempStructureNormalTime");
            }
        }

        [DataMember]
        public bool IsStructureObservedTimeHoursExist
        {
            get { return isStructureObservedTimeHoursExist; }
            set
            {
                isStructureObservedTimeHoursExist = value;
                OnPropertyChanged("IsStructureObservedTimeHoursExist");
            }
        }

        [DataMember]
        public bool IsStructureNormalTimeHoursExist
        {
            get { return isStructureNormalTimeHoursExist; }
            set
            {
                isStructureNormalTimeHoursExist = value;
                OnPropertyChanged("IsStructureNormalTimeHoursExist");
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
        public uint StructurePosition
        {
            get { return structurePosition; }
            set
            {
                structurePosition = value;
                OnPropertyChanged("StructurePosition");
            }
        }

        #endregion

        #region Constructor

        public StandardOperationsDictionaryStructures()
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
            var newSODClone = (StandardOperationsDictionaryStructures)this.MemberwiseClone();



            return newSODClone;
        }

        #endregion
    }
}
