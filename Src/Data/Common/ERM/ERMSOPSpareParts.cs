using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMSOPSpareParts : ModelBase, IDisposable
    {
        #region Fields
        private Int32 idStage;
        private Int32 idworkOperationByStage;
        private Int32 idSequence;
        private string name;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_zh;
        private string name_ru;
        private Int32 idStatus;
        private string status;
        private string code;
        private string parent;
        UInt64? idParent;
        string keyName;
        string parentName;
        uint position;
        int workOperation_count;
        int workOperation_count_original;
        string nameWithWorkOperationCount;

        int idStandardOperationsDictionarySpareParts;
        int idStandardOperationsDictionary;
        int idCpType;
        int idWorkoperation;

        List<object> listOfSupplements;
        UInt64 idDetection;
        string sparePartName;
        public string FieldName { get; set; }

        double? observedTime;
        double? activity;
        double? normalTime;
        bool isDeleteButton = true;
        Visibility isDeleteButtonVisibility = Visibility.Visible;
        bool observedTimeIsReadOnly = true;
        bool isReadOnly = false;
        bool isValidateOnTextInput = true;

        bool isInactiveWorkOperation = false;

      
        TimeSpan uITempSparePartObservedTime;
        TimeSpan uITempSparePartNormalTime;
        private bool isSparePartObservedTimeHoursExist;
        private bool isSparePartNormalTimeHoursExist;
        
        string woremarks;
        string remarks;

        [DataMember]
        public bool IsInactiveWorkOperation
        {
            get
            {
                return isInactiveWorkOperation;
            }

            set
            {
                isInactiveWorkOperation = value;
                OnPropertyChanged("IsInactiveWorkOperation");
            }
        }
        float? standardSupplements;
        [DataMember]
        public float? SparePartStandardSupplements
        {
            get
            {
                return standardSupplements;
            }

            set
            {
                standardSupplements = value;
                OnPropertyChanged("SparePartStandardSupplements");
            }
        }
        float? standardOperationTime;
        [DataMember]
        public float? SparePartStandardOperationTime
        {
            get
            {
                return standardOperationTime;
            }

            set
            {
                standardOperationTime = value;
                OnPropertyChanged("SparePartStandardOperationTime");
            }
        }

        float? supplementPlant1Value;
        [DataMember]
        public float? SparePartSupplementPlant1Value
        {
            get
            {
                return supplementPlant1Value;
            }

            set
            {
                supplementPlant1Value = value;
                OnPropertyChanged("SparePartSupplementPlant1Value");
            }
        }

        string supplementPlant1Name;
        [DataMember]
        public string SparePartSupplementPlant1Name
        {
            get
            {
                return supplementPlant1Name;
            }

            set
            {
                supplementPlant1Name = value;
                OnPropertyChanged("SparePartSupplementPlant1Name");
            }
        }

        float? supplementPlant2Value;
        [DataMember]
        public float? SparePartSupplementPlant2Value
        {
            get
            {
                return supplementPlant2Value;
            }

            set
            {
                supplementPlant2Value = value;
                OnPropertyChanged("SparePartSupplementPlant2Value");
            }
        }

        string supplementPlant2Name;
        [DataMember]
        public string SparePartSupplementPlant2Name
        {
            get
            {
                return supplementPlant2Name;
            }

            set
            {
                supplementPlant2Name = value;
                OnPropertyChanged("SparePartSupplementPlant2Name");
            }
        }
        float? supplementPlant3Value;
        [DataMember]
        public float? SparePartSupplementPlant3Value
        {
            get
            {
                return supplementPlant3Value;
            }

            set
            {
                supplementPlant3Value = value;
                OnPropertyChanged("SparePartSupplementPlant3Value");
            }
        }
        string supplementPlant3Name;
        [DataMember]
        public string SparePartSupplementPlant3Name
        {
            get
            {
                return supplementPlant3Name;
            }

            set
            {
                supplementPlant3Name = value;
                OnPropertyChanged("SparePartSupplementPlant3Name");
            }
        }
        float? supplementPlant4Value;
        [DataMember]
        public float? SparePartSupplementPlant4Value
        {
            get
            {
                return supplementPlant4Value;
            }

            set
            {
                supplementPlant4Value = value;
                OnPropertyChanged("SparePartSupplementPlant4Value");
            }
        }
        string supplementPlant4Name;
        [DataMember]
        public string SparePartSupplementPlant4Name
        {
            get
            {
                return supplementPlant4Name;
            }

            set
            {
                supplementPlant4Name = value;
                OnPropertyChanged("SparePartSupplementPlant4Name");
            }
        }
        float? supplementPlant5Value;
        [DataMember]
        public float? SparePartSupplementPlant5Value
        {
            get
            {
                return supplementPlant5Value;
            }

            set
            {
                supplementPlant5Value = value;
                OnPropertyChanged("SparePartSupplementPlant5Value");
            }
        }
        string supplementPlant5Name;
        [DataMember]
        public string SparePartSupplementPlant5Name
        {
            get
            {
                return supplementPlant5Name;
            }

            set
            {
                supplementPlant5Name = value;
                OnPropertyChanged("SparePartSupplementPlant5Name");
            }
        }

        string standardSupplementsName;
        [DataMember]
        public string SparePartStandardSupplementsName
        {
            get
            {
                return standardSupplementsName;
            }

            set
            {
                standardSupplementsName = value;
                OnPropertyChanged("SparePartStandardSupplementsName");
            }
        }
        string standardOperationTimeName;
        [DataMember]
        public string SparePartStandardOperationTimeName
        {
            get
            {
                return standardOperationTimeName;
            }

            set
            {
                standardOperationTimeName = value;
                OnPropertyChanged("SparePartStandardOperationTimeName");
            }
        }


        float? operationTimePlant1Value;
        [DataMember]
        public float? SparePartOperationTimePlant1Value
        {
            get
            {
                return operationTimePlant1Value;
            }

            set
            {
                operationTimePlant1Value = value;
                OnPropertyChanged("SparePartOperationTimePlant1Value");
            }
        }
        string operationTimePlant1Name;
        [DataMember]
        public string SparePartOperationTimePlant1Name
        {
            get
            {
                return operationTimePlant1Name;
            }

            set
            {
                operationTimePlant1Name = value;
                OnPropertyChanged("SparePartOperationTimePlant1Name");
            }
        }
        float? operationTimePlant2Value;
        [DataMember]
        public float? SparePartOperationTimePlant2Value
        {
            get
            {
                return operationTimePlant2Value;
            }

            set
            {
                operationTimePlant2Value = value;
                OnPropertyChanged("SparePartOperationTimePlant2Value");
            }
        }

        string operationTimePlant2Name;
        [DataMember]
        public string SparePartOperationTimePlant2Name
        {
            get
            {
                return operationTimePlant2Name;
            }

            set
            {
                operationTimePlant2Name = value;
                OnPropertyChanged("SparePartOperationTimePlant2Name");
            }
        }

        float? operationTimePlant3Value;
        [DataMember]
        public float? SparePartOperationTimePlant3Value
        {
            get
            {
                return operationTimePlant3Value;
            }

            set
            {
                operationTimePlant3Value = value;
                OnPropertyChanged("SparePartOperationTimePlant3Value");
            }
        }
        string operationTimePlant3Name;
        [DataMember]
        public string SparePartOperationTimePlant3Name
        {
            get
            {
                return operationTimePlant3Name;
            }

            set
            {
                operationTimePlant3Name = value;
                OnPropertyChanged("SparePartOperationTimePlant3Name");
            }
        }
        float? operationTimePlant4Value;
        [DataMember]
        public float? SparePartOperationTimePlant4Value
        {
            get
            {
                return operationTimePlant4Value;
            }

            set
            {
                operationTimePlant4Value = value;
                OnPropertyChanged("SparePartOperationTimePlant4Value");
            }
        }
        string operationTimePlant4Name;
        [DataMember]
        public string SparePartOperationTimePlant4Name
        {
            get
            {
                return operationTimePlant4Name;
            }

            set
            {
                operationTimePlant4Name = value;
                OnPropertyChanged("SparePartOperationTimePlant4Name");
            }
        }

        float? operationTimePlant5Value;
        [DataMember]
        public float? SparePartOperationTimePlant5Value
        {
            get
            {
                return operationTimePlant5Value;
            }

            set
            {

                operationTimePlant5Value = value;
                OnPropertyChanged("SparePartOperationTimePlant5Value");
            }
        }
        string operationTimePlant5Name;
        [DataMember]
        public string OptionOperationTimePlant5Name
        {
            get
            {
                return operationTimePlant5Name;
            }

            set
            {
                operationTimePlant5Name = value;
                OnPropertyChanged("SparePartOperationTimePlant5Name");
            }
        }

        [DataMember]
        public int IdStandardOperationsDictionarySpareParts
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
        public int IdStandardOperationsDictionary
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
        public int IdCpType
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
        public int IdWorkoperation
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
        public double? SparePartObservedTime
        {
            get
            {
                return observedTime;
            }

            set
            {
                observedTime = value;
                //NormalTime = (double ?) (ObservedTime * Activity / 100);
                SparePartNormalTime = Math.Round(Convert.ToDouble(SparePartObservedTime * SparePartActivity / 100), 2);

                //UITempSparePartNormalTime = ConvertfloattoTimespanForNormalTimeForProperty(Convert.ToString(SparePartNormalTime));

                #region [GEOS2-5008][gulab lakade][1 11 2023]
                //UITempSparePartNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(SparePartNormalTime));
                UITempSparePartNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartNormalTime)));
                #endregion
                OnPropertyChanged("SparePartObservedTime");
            }
        }
        [DataMember]
        public double? SparePartActivity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
                SparePartNormalTime = Math.Round(Convert.ToDouble(SparePartObservedTime * SparePartActivity / 100), 2);
                //UITempSparePartNormalTime = ConvertfloattoTimespanForNormalTimeForProperty(SparePartNormalTime.ToString());

                #region [GEOS2-5008][gulab lakade][1 11 2023]
               // UITempSparePartNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(SparePartNormalTime));
                UITempSparePartNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartNormalTime)));
                #endregion
                OnPropertyChanged("SparePartActivity");
            }
        }

        [DataMember]
        public double? SparePartNormalTime
        {
            get
            {
                return normalTime;
            }

            set
            {
                normalTime = value;
                //Operation Time (s) = NormalTime  *(1 + Supplements(%))
                SparePartStandardOperationTime = (float?)Math.Round(Convert.ToDouble(SparePartNormalTime * (1 + Convert.ToDouble(SparePartStandardSupplements / 100))), 2);
                SparePartOperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(SparePartNormalTime * (1 + Convert.ToDouble(SparePartSupplementPlant1Value / 100))), 2);
                SparePartOperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(SparePartNormalTime * (1 + Convert.ToDouble(SparePartSupplementPlant2Value / 100))), 2);
                SparePartOperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(SparePartNormalTime * (1 + Convert.ToDouble(SparePartSupplementPlant3Value / 100))), 2);
                SparePartOperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(SparePartNormalTime * (1 + Convert.ToDouble(SparePartSupplementPlant4Value / 100))), 2);
                SparePartOperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(SparePartNormalTime * (1 + Convert.ToDouble(SparePartSupplementPlant5Value / 100))), 2);


                #region [GEOS2-5008][gulab lakade][1 11 2023]
                //UiTempStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(SparePartStandardOperationTime));
                //UiTempoperationTimePlant1Value = TimeSpan.FromMinutes(Convert.ToDouble(SparePartOperationTimePlant1Value));
                //UiTempoperationTimePlant2Value = TimeSpan.FromMinutes(Convert.ToDouble(SparePartOperationTimePlant2Value));
                //UiTempoperationTimePlant3Value = TimeSpan.FromMinutes(Convert.ToDouble(SparePartOperationTimePlant3Value));
                //UiTempoperationTimePlant4Value = TimeSpan.FromMinutes(Convert.ToDouble(SparePartOperationTimePlant4Value));
                //UiTempoperationTimePlant5Value = TimeSpan.FromMinutes(Convert.ToDouble(SparePartOperationTimePlant5Value));

                UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartStandardOperationTime)));
                UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartOperationTimePlant1Value)));
                UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartOperationTimePlant2Value)));
                UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartOperationTimePlant3Value)));
                UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartOperationTimePlant4Value)));
                UiTempoperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(SparePartOperationTimePlant5Value)));

                #endregion
                OnPropertyChanged("SparePartNormalTime");
            }
        }


        [DataMember]
        public bool IsDeleteButton
        {
            get
            {
                return isDeleteButton;
            }

            set
            {
                isDeleteButton = value;
                OnPropertyChanged("IsDeleteButton");
            }
        }

        [DataMember]
        public Visibility IsDeleteButtonVisibility
        {
            get
            {
                return isDeleteButtonVisibility;
            }

            set
            {
                isDeleteButtonVisibility = value;
                OnPropertyChanged("IsDeleteButtonVisibility");
            }
        }

        [DataMember]
        public bool ObservedTimeIsReadOnly
        {
            get
            {
                return observedTimeIsReadOnly;
            }

            set
            {
                observedTimeIsReadOnly = value;
                OnPropertyChanged("ObservedTimeIsReadOnly");
            }
        }

        [DataMember]
        public List<object> ListOfSupplements
        {
            get
            {
                return listOfSupplements;
            }

            set
            {
                listOfSupplements = value;
                OnPropertyChanged("ListOfSupplements");
            }
        }

        [DataMember]
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }

            set
            {
                isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }
        [DataMember]
        public bool IsValidateOnTextInput
        {
            get
            {
                return isValidateOnTextInput;
            }

            set
            {
                isValidateOnTextInput = value;
                OnPropertyChanged("IsValidateOnTextInput");
            }
        }

        List<Tuple<string, float?>> allsupplementsBoxMenu;
        public List<Tuple<string, float?>> AllSupplementsBoxMenu
        {
            get { return allsupplementsBoxMenu; }
            set
            {
                allsupplementsBoxMenu = value;
                OnPropertyChanged("AllSupplementsBoxMenu");
            }
        }

        //[GEOS2-3933][Rupali Sarode][22/09/2022]
        public string WORemarks
        {

            get { return woremarks; }
            set
            {

                woremarks = value;
                OnPropertyChanged("WORemarks");
            }

        }
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

        #region[GEOS2-3954][Rupali Sarode][14/10/2022]
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
                //NormalTime = (double ?) (ObservedTime * Activity / 100);
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
                    IsSparePartNormalTimeHoursExist = true;
                else
                    IsSparePartNormalTimeHoursExist = false;

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

        #region GEOS2-3954 Show the WorkOperations and SOD Time in the format: MM:SS or HH:MM:SS
        private TimeSpan uITempobservedTime;
        private TimeSpan uITempNormalTime;
        private bool isObservedTimeHoursExist = false;
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
        private bool isNormalTimeHoursExist;
        private bool isOperationTimeHoursExist;
        private bool iSStandardOperationTime;
        public bool IsStandardOperationTime
        {
            get { return iSStandardOperationTime; }
            set
            {
                iSStandardOperationTime = value;
                OnPropertyChanged("IsStandardOperationTime");
            }
        }

        private bool isPlant1ValueOperationTime;
        public bool IsPlant1ValueOperationTime
        {
            get { return isPlant1ValueOperationTime; }
            set
            {
                isPlant1ValueOperationTime = value;
                OnPropertyChanged("IsPlant1ValueOperationTime");
            }
        }

        [DataMember]
        public bool IsOperationTimeHoursExist
        {
            get { return isOperationTimeHoursExist; }
            set
            {
                isOperationTimeHoursExist = value;
                OnPropertyChanged("IsOperationTimeHoursExist");
            }
        }
        TimeSpan uiTempStandardOperationTimeName;
        [DataMember]
        public TimeSpan UiTempStandardOperationTimeName
        {
            get
            {
                return uiTempStandardOperationTimeName;
            }

            set
            {
                uiTempStandardOperationTimeName = value;
                if (UiTempStandardOperationTimeName.Hours > 0)
                {
                    IsOperationTimeHoursExist = true;
                }
                else
                {
                    IsOperationTimeHoursExist = false;
                }
                try
                {
                    if (uiTempStandardOperationTimeName.Hours > 0)
                    {
                        IsStandardOperationTime = true;
                    }
                    else
                    {
                        IsStandardOperationTime = false;
                    }
                }
                catch (Exception ex) { }
                OnPropertyChanged("UiTempStandardOperationTimeName");
            }
        }
        TimeSpan uiTempoperationTimePlant1Value;
        [DataMember]
        public TimeSpan UiTempoperationTimePlant1Value
        {
            get
            {
                return uiTempoperationTimePlant1Value;
            }

            set
            {
                try
                {
                    TimeSpan time = TimeSpan.Parse(uiTempoperationTimePlant1Value.ToString());
                    if (time.Hours > 0)
                    {
                        IsPlant1ValueOperationTime = true;
                    }
                    else
                    {
                        IsPlant1ValueOperationTime = false;
                    }
                }
                catch (Exception ex) { }

                uiTempoperationTimePlant1Value = value;
                OnPropertyChanged("UiTempoperationTimePlant1Value");
            }
        }
        TimeSpan uiTempoperationTimePlant2Value;
        [DataMember]
        public TimeSpan UiTempoperationTimePlant2Value
        {
            get
            {
                return uiTempoperationTimePlant2Value;
            }

            set
            {
                uiTempoperationTimePlant2Value = value;
                OnPropertyChanged("UiTempoperationTimePlant2Value");
            }
        }
        TimeSpan uiTempoperationTimePlant3Value;
        [DataMember]
        public TimeSpan UiTempoperationTimePlant3Value
        {
            get
            {
                return uiTempoperationTimePlant3Value;
            }

            set
            {
                uiTempoperationTimePlant3Value = value;
                OnPropertyChanged("UiTempoperationTimePlant3Value");
            }
        }
        TimeSpan uiTempoperationTimePlant4Value;
        [DataMember]
        public TimeSpan UiTempoperationTimePlant4Value
        {
            get
            {
                return uiTempoperationTimePlant4Value;
            }

            set
            {
                uiTempoperationTimePlant4Value = value;
                OnPropertyChanged("UiTempoperationTimePlant4Value");
            }
        }
        TimeSpan uiTempoperationTimePlant5Value;
        [DataMember]
        public TimeSpan UiTempoperationTimePlant5Value
        {
            get
            {
                return uiTempoperationTimePlant5Value;
            }

            set
            {
                uiTempoperationTimePlant5Value = value;
                OnPropertyChanged("UiTempoperationTimePlant5Value");
            }
        }
        #endregion

        private uint sparepartPosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
        #endregion

        #region Properties
        [DataMember]
        public int WorkOperation_count_original
        {
            get
            {
                return workOperation_count_original;
            }

            set
            {
                workOperation_count_original = value;
                OnPropertyChanged("WorkOperation_count_original");
            }
        }
        [DataMember]
        public Int32 IdworkOperationByStage
        {
            get { return idworkOperationByStage; }
            set
            {
                idworkOperationByStage = value;
                OnPropertyChanged("IdworkOperationByStage");
            }
        }
        [DataMember]
        public string KeyName
        {
            get
            {
                return keyName;
            }

            set
            {
                keyName = value;
                OnPropertyChanged("KeyName");
            }
        }

        [DataMember]
        public string ParentName
        {
            get
            {
                return parentName;
            }

            set
            {
                parentName = value;
                OnPropertyChanged("ParentName");
            }
        }

        [DataMember]
        public int WorkOperation_count
        {
            get
            {
                return workOperation_count;
            }

            set
            {
                workOperation_count = value;
                OnPropertyChanged("WorkOperation_count");
            }
        }

        [DataMember]
        public string NameWithWorkOperationCount
        {
            get
            {
                return nameWithWorkOperationCount;
            }

            set
            {
                nameWithWorkOperationCount = value;
                OnPropertyChanged("NameWithWorkOperationCount");
            }
        }
        [DataMember]
        public uint Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }
        [DataMember]
        public UInt64? IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged("IdParent");
            }
        }
        [DataMember]
        public string Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }
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
        public Int32 IdSequence
        {
            get { return idSequence; }
            set
            {
                idSequence = value;
                OnPropertyChanged("IdSequence");
            }
        }
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        [DataMember]
        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged("Name_es");
            }
        }

        [DataMember]
        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged("Name_fr");
            }
        }

        [DataMember]
        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged("Name_pt");
            }
        }

        [DataMember]
        public string Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged("Name_ro");
            }
        }

        [DataMember]
        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged("Name_zh");
            }
        }

        [DataMember]
        public string Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged("Name_ru");
            }
        }
        [DataMember]
        public Int32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
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

        public override string ToString()
        {
            return Name;
        }
        #region [GEOS2-5008][gulab lakade][1 11 2023]
        public TimeSpan ConvertfloattoTimespan(string observedtime)
        {
            TimeSpan UITempobservedTime;
            try
            {
                //UITempobservedTime = TimeSpan.FromMinutes(Convert.ToDouble(observedtime));

                #region [GEOS2-5008][gulab lakade][1 11 2023]
                UITempobservedTime = TimeSpan.FromMinutes(Convert.ToDouble(observedtime));
                if (UITempobservedTime.Milliseconds >= 600)
                {

                    TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                    UITempobservedTime = UITempobservedTime.Add(Second);
                    UITempobservedTime = UITempobservedTime.Add(-TimeSpan.FromMilliseconds(UITempobservedTime.Milliseconds));

                }
                else
                {
                    UITempobservedTime = UITempobservedTime.Add(-TimeSpan.FromMilliseconds(UITempobservedTime.Milliseconds));
                }

                #endregion
                return UITempobservedTime;
            }
            catch (Exception ex)
            {
                UITempobservedTime = TimeSpan.FromMinutes(0);
                return UITempobservedTime;
            }

            //TimeSpan UITempobservedTime;
            //        try
            //        {
            //            #region GEOS2-3954 Time format HH:MM:SS
            //            var currentculter = CultureInfo.CurrentCulture;
            //            string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
            //            string tempd = Convert.ToString(observedtime);
            //            string[] parts = new string[2];
            //            int i1 = 0;
            //            int i2 = 0;
            //            if (tempd.Contains(culterseparator))
            //            {
            //                parts = tempd.Split(Convert.ToChar(culterseparator));
            //                i1 = int.Parse(parts[0]);
            //                i2 = int.Parse(parts[1]);

            //                if (Convert.ToString(parts[1]).Length == 1)
            //                {
            //                    i1 = (i1 * 60) + i2 * 10;
            //                }
            //                else
            //                {
            //                    i1 = (i1 * 60) + i2;
            //                }

            //            }
            //            else
            //            {
            //                parts = tempd.Split(Convert.ToChar(culterseparator));
            //                i1 = int.Parse(parts[0]);
            //                i1 = (i1 * 60);
            //            }



            //            UITempobservedTime = TimeSpan.FromSeconds(i1);
            //            int ts1 = UITempobservedTime.Hours;
            //            int ts2 = UITempobservedTime.Minutes;
            //            int ts3 = UITempobservedTime.Seconds;


            //            #endregion
            //            return UITempobservedTime;
            //        }
            //        catch (Exception ex)
            //        {
            //            UITempobservedTime = TimeSpan.FromSeconds(0);
            //            return UITempobservedTime;
            //        }

        }

        #endregion
        //private TimeSpan ConvertFloatToTimespanForNormalTimeForProperty(string Normaltime)
        //{
        //    TimeSpan UITempNormalTime;
        //    try
        //    {
        //        #region GEOS2-3954 Time format HH:MM:SS

        //        string temnormaltime = Convert.ToString(Normaltime);
        //        string[] NormaltimeArr = new string[2];
        //        int nt1 = 0;
        //        int nt2 = 0;
        //        if (temnormaltime.Contains("."))
        //        {
        //            NormaltimeArr = temnormaltime.Split('.');
        //            nt1 = int.Parse(NormaltimeArr[0]);
        //            nt2 = int.Parse(NormaltimeArr[1]);
        //            nt1 = (nt1 * 60) + nt2;
        //        }
        //        else
        //        {
        //            NormaltimeArr = temnormaltime.Split('.');
        //            nt1 = int.Parse(NormaltimeArr[0]);
        //            nt1 = (nt1 * 60);
        //        }

        //        UITempNormalTime = TimeSpan.FromSeconds(nt1);

        //        #endregion
        //        return UITempNormalTime;
        //    }
        //    catch (Exception ex)
        //    {
        //        UITempNormalTime = TimeSpan.FromSeconds(0);
        //        return UITempNormalTime;
        //    }

        //}

        public TimeSpan ConvertfloattoTimespanForNormalTimeForProperty(string Normaltime)
        {
            TimeSpan UITempNormalTime;
            try
            {
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                #region GEOS2-3954 Time format HH:MM:SS

                string temnormaltime = Convert.ToString(Normaltime);
                string[] NormaltimeArr = new string[2];
                int nt1 = 0;
                int nt2 = 0;
                if (temnormaltime.Contains(culterseparator))
                {
                    //char[] culterseparatorarr =Convert.ToChar(culterseparator);
                    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                    nt1 = int.Parse(NormaltimeArr[0]);
                    nt2 = int.Parse(NormaltimeArr[1]);
                    if (Convert.ToString(NormaltimeArr[1]).Length == 1)
                    {
                        nt1 = (nt1 * 60) + nt2 * 10;
                    }
                    else
                    {
                        nt1 = (nt1 * 60) + nt2;
                    }

                }
                else
                {
                    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                    nt1 = int.Parse(NormaltimeArr[0]);
                    nt1 = (nt1 * 60);
                }

                UITempNormalTime = TimeSpan.FromSeconds(nt1);

                #endregion
                return UITempNormalTime;
            }
            catch (Exception ex)
            {
                UITempNormalTime = TimeSpan.FromSeconds(0);
                return UITempNormalTime;
            }

        }
        #endregion
    }
}
