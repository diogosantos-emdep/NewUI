using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.ERM
{
    public class ERMSOPStructures : ModelBase, IDisposable
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

        int idStandardOperationsDictionaryStructures;
        int idStandardOperationsDictionary;
        int idCpType;
        int idWorkoperation;

        List<object> listOfSupplements;

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
        string woremarks;
        string remarks;

        #region [GEOS2-3954][Rupali Sarode][17/10/2022]
        TimeSpan uITempStructureNormalTime;
        TimeSpan uITempStructureObservedTime;
        private bool isStructureObservedTimeHoursExist;
        private bool isStructureNormalTimeHoursExist;
        #endregion
        private uint structurePosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
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
        public float? StructureStandardSupplements
        {
            get
            {
                return standardSupplements;
            }

            set
            {
                standardSupplements = value;
                OnPropertyChanged("StructureStandardSupplements");
            }
        }
        float? standardOperationTime;
        [DataMember]
        public float? StructureStandardOperationTime
        {
            get
            {
                return standardOperationTime;
            }

            set
            {
                standardOperationTime = value;
                OnPropertyChanged("StructureStandardOperationTime");
            }
        }

        float? supplementPlant1Value;
        [DataMember]
        public float? StructureSupplementPlant1Value
        {
            get
            {
                return supplementPlant1Value;
            }

            set
            {
                supplementPlant1Value = value;
                OnPropertyChanged("StructureSupplementPlant1Value");
            }
        }

        string supplementPlant1Name;
        [DataMember]
        public string StructureSupplementPlant1Name
        {
            get
            {
                return supplementPlant1Name;
            }

            set
            {
                supplementPlant1Name = value;
                OnPropertyChanged("StructureSupplementPlant1Name");
            }
        }

        float? supplementPlant2Value;
        [DataMember]
        public float? StructureSupplementPlant2Value
        {
            get
            {
                return supplementPlant2Value;
            }

            set
            {
                supplementPlant2Value = value;
                OnPropertyChanged("StructureSupplementPlant2Value");
            }
        }

        string supplementPlant2Name;
        [DataMember]
        public string StructureSupplementPlant2Name
        {
            get
            {
                return supplementPlant2Name;
            }

            set
            {
                supplementPlant2Name = value;
                OnPropertyChanged("StructureSupplementPlant2Name");
            }
        }
        float? supplementPlant3Value;
        [DataMember]
        public float? StructureSupplementPlant3Value
        {
            get
            {
                return supplementPlant3Value;
            }

            set
            {
                supplementPlant3Value = value;
                OnPropertyChanged("StructureSupplementPlant3Value");
            }
        }
        string supplementPlant3Name;
        [DataMember]
        public string StructureSupplementPlant3Name
        {
            get
            {
                return supplementPlant3Name;
            }

            set
            {
                supplementPlant3Name = value;
                OnPropertyChanged("StructureSupplementPlant3Name");
            }
        }
        float? supplementPlant4Value;
        [DataMember]
        public float? StructureSupplementPlant4Value
        {
            get
            {
                return supplementPlant4Value;
            }

            set
            {
                supplementPlant4Value = value;
                OnPropertyChanged("StructureSupplementPlant4Value");
            }
        }
        string supplementPlant4Name;
        [DataMember]
        public string StructureSupplementPlant4Name
        {
            get
            {
                return supplementPlant4Name;
            }

            set
            {
                supplementPlant4Name = value;
                OnPropertyChanged("StructureSupplementPlant4Name");
            }
        }
        float? supplementPlant5Value;
        [DataMember]
        public float? StructureSupplementPlant5Value
        {
            get
            {
                return supplementPlant5Value;
            }

            set
            {
                supplementPlant5Value = value;
                OnPropertyChanged("StructureSupplementPlant5Value");
            }
        }
        string supplementPlant5Name;
        [DataMember]
        public string StructureSupplementPlant5Name
        {
            get
            {
                return supplementPlant5Name;
            }

            set
            {
                supplementPlant5Name = value;
                OnPropertyChanged("StructureSupplementPlant5Name");
            }
        }

        string standardSupplementsName;
        [DataMember]
        public string StandardSupplementsName
        {
            get
            {
                return standardSupplementsName;
            }

            set
            {
                standardSupplementsName = value;
                OnPropertyChanged("StandardSupplementsName");
            }
        }
        string standardOperationTimeName;
        [DataMember]
        public string StandardOperationTimeName
        {
            get
            {
                return standardOperationTimeName;
            }

            set
            {
                standardOperationTimeName = value;
                OnPropertyChanged("StandardOperationTimeName");
            }
        }


        float? operationTimePlant1Value;
        [DataMember]
        public float? StructureOperationTimePlant1Value
        {
            get
            {
                return operationTimePlant1Value;
            }

            set
            {
                operationTimePlant1Value = value;
                OnPropertyChanged("StructureOperationTimePlant1Value");
            }
        }
        string operationTimePlant1Name;
        [DataMember]
        public string StructureOperationTimePlant1Name
        {
            get
            {
                return operationTimePlant1Name;
            }

            set
            {
                operationTimePlant1Name = value;
                OnPropertyChanged("StructureOperationTimePlant1Name");
            }
        }
        float? operationTimePlant2Value;
        [DataMember]
        public float? StructureOperationTimePlant2Value
        {
            get
            {
                return operationTimePlant2Value;
            }

            set
            {
                operationTimePlant2Value = value;
                OnPropertyChanged("StructureOperationTimePlant2Value");
            }
        }

        string operationTimePlant2Name;
        [DataMember]
        public string StructureOperationTimePlant2Name
        {
            get
            {
                return operationTimePlant2Name;
            }

            set
            {
                operationTimePlant2Name = value;
                OnPropertyChanged("StructureOperationTimePlant2Name");
            }
        }

        float? operationTimePlant3Value;
        [DataMember]
        public float? StructureOperationTimePlant3Value
        {
            get
            {
                return operationTimePlant3Value;
            }

            set
            {
                operationTimePlant3Value = value;
                OnPropertyChanged("StructureOperationTimePlant3Value");
            }
        }
        string operationTimePlant3Name;
        [DataMember]
        public string StructureOperationTimePlant3Name
        {
            get
            {
                return operationTimePlant3Name;
            }

            set
            {
                operationTimePlant3Name = value;
                OnPropertyChanged("StructureOperationTimePlant3Name");
            }
        }
        float? operationTimePlant4Value;
        [DataMember]
        public float? StructureOperationTimePlant4Value
        {
            get
            {
                return operationTimePlant4Value;
            }

            set
            {
                operationTimePlant4Value = value;
                OnPropertyChanged("StructureOperationTimePlant4Value");
            }
        }
        string operationTimePlant4Name;
        [DataMember]
        public string StructureOperationTimePlant4Name
        {
            get
            {
                return operationTimePlant4Name;
            }

            set
            {
                operationTimePlant4Name = value;
                OnPropertyChanged("StructureOperationTimePlant4Name");
            }
        }

        float? operationTimePlant5Value;
        [DataMember]
        public float? StructureOperationTimePlant5Value
        {
            get
            {
                return operationTimePlant5Value;
            }

            set
            {

                operationTimePlant5Value = value;
                OnPropertyChanged("StructureOperationTimePlant5Value");
            }
        }
        string operationTimePlant5Name;
        [DataMember]
        public string StructureOperationTimePlant5Name
        {
            get
            {
                return operationTimePlant5Name;
            }

            set
            {
                operationTimePlant5Name = value;
                OnPropertyChanged("StructureOperationTimePlant5Name");
            }
        }

        [DataMember]
        public int IdStandardOperationsDictionaryStructures
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
        public double? StructureObservedTime
        {
            get
            {
                return observedTime;
            }

            set
            {
                observedTime = value;
                //NormalTime = (double ?) (ObservedTime * Activity / 100);
                StructureNormalTime = Math.Round(Convert.ToDouble(StructureObservedTime * StructureActivity / 100), 2);
                //UITempStructureNormalTime = ConvertfloattoTimespanForNormalTimeForProperty(Convert.ToString(StructureNormalTime));

                #region [GEOS2-5008][gulab lakade][1 11 2023]
               // UITempStructureNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(StructureNormalTime));
                UITempStructureNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(StructureNormalTime)));
                #endregion
                OnPropertyChanged("StructureObservedTime");
            }
        }
        [DataMember]
        public double? StructureActivity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
                StructureNormalTime = Math.Round(Convert.ToDouble(StructureObservedTime * StructureActivity / 100), 2);
                //UITempStructureNormalTime = ConvertfloattoTimespanForNormalTimeForProperty(Convert.ToString(StructureNormalTime));

                #region [GEOS2-5008][gulab lakade][1 11 2023]
                //UITempStructureNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(StructureNormalTime));
                UITempStructureNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(StructureNormalTime)));
                #endregion
                OnPropertyChanged("StructureActivity");
            }
        }

        [DataMember]
        public double? StructureNormalTime
        {
            get
            {
                return normalTime;
            }

            set
            {
                normalTime = value;
                //Operation Time (s) = NormalTime  *(1 + Supplements(%))
                StructureStandardOperationTime = (float?)Math.Round(Convert.ToDouble(StructureNormalTime * (1 + Convert.ToDouble(StructureStandardSupplements / 100))), 2);
                StructureOperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(StructureNormalTime * (1 + Convert.ToDouble(StructureSupplementPlant1Value / 100))), 2);
                StructureOperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(StructureNormalTime * (1 + Convert.ToDouble(StructureSupplementPlant2Value / 100))), 2);
                StructureOperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(StructureNormalTime * (1 + Convert.ToDouble(StructureSupplementPlant3Value / 100))), 2);
                StructureOperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(StructureNormalTime * (1 + Convert.ToDouble(StructureSupplementPlant4Value / 100))), 2);
                StructureOperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(StructureNormalTime * (1 + Convert.ToDouble(StructureSupplementPlant5Value / 100))), 2);

                #region [GEOS2-5008][gulab lakade][1 11 2023]
                
                //startGEOS2 - 4046 Gulab Lakade
                UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(StructureStandardOperationTime));
                UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(StructureOperationTimePlant1Value));
                UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(StructureOperationTimePlant2Value));
                UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(StructureOperationTimePlant3Value));
                UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(StructureOperationTimePlant4Value));
                UiTempoperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(StructureOperationTimePlant5Value));

                //UiTempStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(StructureStandardOperationTime));
                //UiTempoperationTimePlant1Value = TimeSpan.FromMinutes(Convert.ToDouble(StructureOperationTimePlant1Value));
                //UiTempoperationTimePlant2Value = TimeSpan.FromMinutes(Convert.ToDouble(StructureOperationTimePlant2Value));
                //UiTempoperationTimePlant3Value = TimeSpan.FromMinutes(Convert.ToDouble(StructureOperationTimePlant3Value));
                //UiTempoperationTimePlant4Value = TimeSpan.FromMinutes(Convert.ToDouble(StructureOperationTimePlant4Value));
                //UiTempoperationTimePlant5Value = TimeSpan.FromMinutes(Convert.ToDouble(StructureOperationTimePlant5Value));
                #endregion
                OnPropertyChanged("StructureNormalTime");
            }
        }

        //startGEOS2 - 4046 Gulab Lakade
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
            //try
            //{
            //    #region GEOS2-3954 Time format HH:MM:SS
            //    var currentculter = CultureInfo.CurrentCulture;
            //    string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
            //    string tempd = Convert.ToString(observedtime);
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



            //    UITempobservedTime = TimeSpan.FromSeconds(i1);
            //    int ts1 = UITempobservedTime.Hours;
            //    int ts2 = UITempobservedTime.Minutes;
            //    int ts3 = UITempobservedTime.Seconds;


            //    #endregion
            //    return UITempobservedTime;
            //}
            //catch (Exception ex)
            //{
            //    UITempobservedTime = TimeSpan.FromSeconds(0);
            //    return UITempobservedTime;
            //}

        }
        //End
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

        #region[GEOS2-3954][Rupali Sarode][14/10/2022]
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
                //NormalTime = (double ?) (ObservedTime * Activity / 100);
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
