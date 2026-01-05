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
    public class ERMSOPDetection : ModelBase, IDisposable
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

        int idStandardOperationsDictionaryDetections;
        int idStandardOperationsDictionary;
        int idCpType;
        int idWorkoperation;

        List<object> listOfSupplements;
        UInt64 idDetection;
        string detectionName;
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

        //[GEOS2-3933][Rupali Sarode][22/09/2022]
        string woremarks;
        string remarks;
        #region GEOS2-3954 Time format HH:MM:SS
        private TimeSpan uITempDetectionobservedTime;
        private TimeSpan uITempDetectionNormalTime;
        private bool isDetectionObservedTimeHoursExist;
        private bool isDetectionNormalTimeHoursExist;
        private string uIstringTempobservedTime;
        private TimeSpan uITempDetectionStandardSupplements;
        #endregion

        #region Aishwarya
        UInt32? idGroup;
        Int32? orderNumber;
        bool isCurrentDetection = false;
        #endregion

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
        public float? DetectionStandardSupplements
        {
            get
            {
                return standardSupplements;
            }

            set
            {
                standardSupplements = value;
                OnPropertyChanged("DetectionStandardSupplements");
            }
        }
        float? standardOperationTime;
        [DataMember]
        public float? DetectionStandardOperationTime
        {
            get
            {
                return standardOperationTime;
            }

            set
            {
                standardOperationTime = value;
                OnPropertyChanged("DetectionStandardOperationTime");
            }
        }

        float? supplementPlant1Value;
        [DataMember]
        public float? DetectionSupplementPlant1Value
        {
            get
            {
                return supplementPlant1Value;
            }

            set
            {
                supplementPlant1Value = value;
                OnPropertyChanged("DetectionSupplementPlant1Value");
            }
        }

        string supplementPlant1Name;
        [DataMember]
        public string DetectionSupplementPlant1Name
        {
            get
            {
                return supplementPlant1Name;
            }

            set
            {
                supplementPlant1Name = value;
                OnPropertyChanged("DetectionSupplementPlant1Name");
            }
        }

        float? supplementPlant2Value;
        [DataMember]
        public float? DetectionSupplementPlant2Value
        {
            get
            {
                return supplementPlant2Value;
            }

            set
            {
                supplementPlant2Value = value;
                OnPropertyChanged("DetectionSupplementPlant2Value");
            }
        }

        string supplementPlant2Name;
        [DataMember]
        public string DetectionSupplementPlant2Name
        {
            get
            {
                return supplementPlant2Name;
            }

            set
            {
                supplementPlant2Name = value;
                OnPropertyChanged("DetectionSupplementPlant2Name");
            }
        }
        float? supplementPlant3Value;
        [DataMember]
        public float? DetectionSupplementPlant3Value
        {
            get
            {
                return supplementPlant3Value;
            }

            set
            {
                supplementPlant3Value = value;
                OnPropertyChanged("DetectionSupplementPlant3Value");
            }
        }
        string supplementPlant3Name;
        [DataMember]
        public string DetectionSupplementPlant3Name
        {
            get
            {
                return supplementPlant3Name;
            }

            set
            {
                supplementPlant3Name = value;
                OnPropertyChanged("DetectionSupplementPlant3Name");
            }
        }
        float? supplementPlant4Value;
        [DataMember]
        public float? DetectionSupplementPlant4Value
        {
            get
            {
                return supplementPlant4Value;
            }

            set
            {
                supplementPlant4Value = value;
                OnPropertyChanged("DetectionSupplementPlant4Value");
            }
        }
        string supplementPlant4Name;
        [DataMember]
        public string DetectionSupplementPlant4Name
        {
            get
            {
                return supplementPlant4Name;
            }

            set
            {
                supplementPlant4Name = value;
                OnPropertyChanged("DetectionSupplementPlant4Name");
            }
        }
        float? supplementPlant5Value;
        [DataMember]
        public float? DetectionSupplementPlant5Value
        {
            get
            {
                return supplementPlant5Value;
            }

            set
            {
                supplementPlant5Value = value;
                OnPropertyChanged("DetectionSupplementPlant5Value");
            }
        }
        string supplementPlant5Name;
        [DataMember]
        public string DetectionSupplementPlant5Name
        {
            get
            {
                return supplementPlant5Name;
            }

            set
            {
                supplementPlant5Name = value;
                OnPropertyChanged("DetectionSupplementPlant5Name");
            }
        }

        string standardSupplementsName;
        [DataMember]
        public string DetectionStandardSupplementsName
        {
            get
            {
                return standardSupplementsName;
            }

            set
            {
                standardSupplementsName = value;
                OnPropertyChanged("DetectionStandardSupplementsName");
            }
        }
        string standardOperationTimeName;
        [DataMember]
        public string DetectionStandardOperationTimeName
        {
            get
            {
                return standardOperationTimeName;
            }

            set
            {
                standardOperationTimeName = value;
                OnPropertyChanged("DetectionStandardOperationTimeName");
            }
        }


        float? operationTimePlant1Value;
        [DataMember]
        public float? DetectionOperationTimePlant1Value
        {
            get
            {
                return operationTimePlant1Value;
            }

            set
            {
                operationTimePlant1Value = value;
                OnPropertyChanged("DetectionOperationTimePlant1Value");
            }
        }
        string operationTimePlant1Name;
        [DataMember]
        public string DetectionOperationTimePlant1Name
        {
            get
            {
                return operationTimePlant1Name;
            }

            set
            {
                operationTimePlant1Name = value;
                OnPropertyChanged("DetectionOperationTimePlant1Name");
            }
        }
        float? operationTimePlant2Value;
        [DataMember]
        public float? DetectionOperationTimePlant2Value
        {
            get
            {
                return operationTimePlant2Value;
            }

            set
            {
                operationTimePlant2Value = value;
                OnPropertyChanged("DetectionOperationTimePlant2Value");
            }
        }

        string operationTimePlant2Name;
        [DataMember]
        public string DetectionOperationTimePlant2Name
        {
            get
            {
                return operationTimePlant2Name;
            }

            set
            {
                operationTimePlant2Name = value;
                OnPropertyChanged("DetectionOperationTimePlant2Name");
            }
        }

        float? operationTimePlant3Value;
        [DataMember]
        public float? DetectionOperationTimePlant3Value
        {
            get
            {
                return operationTimePlant3Value;
            }

            set
            {
                operationTimePlant3Value = value;
                OnPropertyChanged("DetectionOperationTimePlant3Value");
            }
        }
        string operationTimePlant3Name;
        [DataMember]
        public string DetectionOperationTimePlant3Name
        {
            get
            {
                return operationTimePlant3Name;
            }

            set
            {
                operationTimePlant3Name = value;
                OnPropertyChanged("DetectionOperationTimePlant3Name");
            }
        }
        float? operationTimePlant4Value;
        [DataMember]
        public float? DetectionOperationTimePlant4Value
        {
            get
            {
                return operationTimePlant4Value;
            }

            set
            {
                operationTimePlant4Value = value;
                OnPropertyChanged("DetectionOperationTimePlant4Value");
            }
        }
        string operationTimePlant4Name;
        [DataMember]
        public string DetectionOperationTimePlant4Name
        {
            get
            {
                return operationTimePlant4Name;
            }

            set
            {
                operationTimePlant4Name = value;
                OnPropertyChanged("DetectionOperationTimePlant4Name");
            }
        }

        float? operationTimePlant5Value;
        [DataMember]
        public float? DetectionOperationTimePlant5Value
        {
            get
            {
                return operationTimePlant5Value;
            }

            set
            {

                operationTimePlant5Value = value;
                OnPropertyChanged("DetectionOperationTimePlant5Value");
            }
        }
        string operationTimePlant5Name;
        [DataMember]
        public string DetectionOperationTimePlant5Name
        {
            get
            {
                return operationTimePlant5Name;
            }

            set
            {
                operationTimePlant5Name = value;
                OnPropertyChanged("DetectionOperationTimePlant5Name");
            }
        }

        [DataMember]
        public int IdStandardOperationsDictionaryDetections
        {
            get
            {
                return idStandardOperationsDictionaryDetections;
            }

            set
            {
                idStandardOperationsDictionaryDetections = value;
                OnPropertyChanged("IdStandardOperationsDictionaryDetections");
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
        public double? DetectionObservedTime
        {
            get
            {
                return observedTime;
            }

            set
            {
                observedTime = value;
                //NormalTime = (double ?) (ObservedTime * Activity / 100);
                DetectionNormalTime = Math.Round(Convert.ToDouble(DetectionObservedTime * DetectionActivity / 100), 2);
                #region GEOS2-3954 Time format HH:MM:SS
                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //string temnormaltime = Convert.ToString(DetectionNormalTime);
                //string[] NormaltimeArr = new string[2];
                //int nt1 = 0;
                //int nt2 = 0;
                //if (temnormaltime.Contains(culterseparator))
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt2 = int.Parse(NormaltimeArr[1]);
                //    if (Convert.ToString(NormaltimeArr[1]).Length == 1)
                //    {
                //        nt1 = (nt1 * 60) + nt2 * 10;
                //    }
                //    else
                //    {
                //        nt1 = (nt1 * 60) + nt2;
                //    }
                //}
                //else
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt1 = (nt1 * 60);
                //}
                //UITempDetectionNormalTime = TimeSpan.FromMinutes(nt1);
                #endregion
                #region [GEOS2-5008][gulab lakade][1 11 2023]
                //UITempDetectionNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(DetectionNormalTime));
                UITempDetectionNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(DetectionNormalTime)));
                #endregion

                OnPropertyChanged("DetectionObservedTime");
            }
        }
        [DataMember]
        public double? DetectionActivity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
                DetectionNormalTime = Math.Round(Convert.ToDouble(DetectionObservedTime * DetectionActivity / 100), 2);
                //#region GEOS2-3954 gulab lakade time format HH:mm:ss
                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //string temnormaltime = Convert.ToString(DetectionNormalTime);
                //string[] NormaltimeArr = new string[2];
                //int nt1 = 0;
                //int nt2 = 0;
                //if (temnormaltime.Contains(culterseparator))
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt2 = int.Parse(NormaltimeArr[1]);
                //    if (Convert.ToString(NormaltimeArr[1]).Length == 1)
                //    {
                //        nt1 = (nt1 * 60) + nt2 * 10;
                //    }
                //    else
                //    {
                //        nt1 = (nt1 * 60) + nt2;
                //    }
                //}
                //else
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(culterseparator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt1 = (nt1 * 60);
                //}



                //UITempDetectionNormalTime = TimeSpan.FromSeconds(nt1);
                //#endregion
                #region [GEOS2-5008][gulab lakade][1 11 2023]
               // UITempDetectionNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(DetectionNormalTime));
                UITempDetectionNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(DetectionNormalTime)));
                #endregion

                OnPropertyChanged("DetectionActivity");
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
                OnPropertyChanged("UITempDetectionobservedTime");
               

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
                OnPropertyChanged("UIDetectionTempNormalTime");
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

        public TimeSpan UITempDetectionStandardSupplements
        {
            get { return uITempDetectionStandardSupplements; }
            set
            {

                uITempDetectionStandardSupplements = value;
                //if (UITempDetectionobservedTime.Hours > 0)
                //{
                //    IsDetectionObservedTimeHoursExist = true;
                //}
                //else
                //{
                //    IsDetectionObservedTimeHoursExist = false;
                //}
                OnPropertyChanged("UITempDetectionStandardSupplements");
            }
        }
        #endregion
        [DataMember]
        public double? DetectionNormalTime
        {
            get
            {
                return normalTime;
            }

            set
            {
                normalTime = value;
                //Operation Time (s) = NormalTime  *(1 + Supplements(%))
                DetectionStandardOperationTime = (float?)Math.Round(Convert.ToDouble(   DetectionNormalTime * (1 + Convert.ToDouble(DetectionStandardSupplements/100))), 2);
                DetectionOperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(DetectionNormalTime * (1 + Convert.ToDouble(DetectionSupplementPlant1Value / 100))), 2);
                DetectionOperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(DetectionNormalTime * (1 + Convert.ToDouble(DetectionSupplementPlant2Value / 100))), 2);
                DetectionOperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(DetectionNormalTime * (1 + Convert.ToDouble(DetectionSupplementPlant3Value / 100))), 2);
                DetectionOperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(DetectionNormalTime * (1 + Convert.ToDouble(DetectionSupplementPlant4Value / 100))), 2);
                DetectionOperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(DetectionNormalTime * (1 + Convert.ToDouble(DetectionSupplementPlant5Value / 100))), 2);
                //startGEOS2 - 4046 Gulab Lakade
                UiTempDetectionStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(DetectionStandardOperationTime));
                UiTempDetectionOperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(DetectionOperationTimePlant1Value));
                UiTempDetectionOperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(DetectionOperationTimePlant2Value));
                UiTempDetectionOperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(DetectionOperationTimePlant3Value));
                UiTempDetectionOperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(DetectionOperationTimePlant4Value));
                UiTempDetectionOperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(DetectionOperationTimePlant5Value));
                //end
                OnPropertyChanged("DetectionNormalTime");
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

        //[GEOS2-3933][Rupali Sarode][21/09/2022]
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

        private uint detectionPosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]
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


        #region GEOS2-3954 Show the WorkOperations and SOD Time in the format: MM:SS or HH:MM:SS
        private TimeSpan uITempobservedTime;
        private TimeSpan uITempNormalTime;
        private bool isObservedTimeHoursExist=false;
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
        public TimeSpan UiTempDetectionStandardOperationTimeName
        {
            get
            {
                return uiTempStandardOperationTimeName;
            }

            set
            {
                uiTempStandardOperationTimeName = value;
                if (UiTempDetectionStandardOperationTimeName.Hours > 0)
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
                OnPropertyChanged("UiTempDetectionStandardOperationTimeName");
            }
        }
        TimeSpan uiTempoperationTimePlant1Value;
        [DataMember]
        public TimeSpan UiTempDetectionOperationTimePlant1Value
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
                OnPropertyChanged("UiTempDetectionOperationTimePlant1Value");
            }
        }
        TimeSpan uiTempoperationTimePlant2Value;
        [DataMember]
        public TimeSpan UiTempDetectionOperationTimePlant2Value
        {
            get
            {
                return uiTempoperationTimePlant2Value;
            }

            set
            {
                uiTempoperationTimePlant2Value = value;
                OnPropertyChanged("UiTempDetectionOperationTimePlant2Value");
            }
        }
        TimeSpan uiTempoperationTimePlant3Value;
        [DataMember]
        public TimeSpan UiTempDetectionOperationTimePlant3Value
        {
            get
            {
                return uiTempoperationTimePlant3Value;
            }

            set
            {
                uiTempoperationTimePlant3Value = value;
                OnPropertyChanged("UiTempDetectionOperationTimePlant3Value");
            }
        }
        TimeSpan uiTempoperationTimePlant4Value;
        [DataMember]
        public TimeSpan UiTempDetectionOperationTimePlant4Value
        {
            get
            {
                return uiTempoperationTimePlant4Value;
            }

            set
            {
                uiTempoperationTimePlant4Value = value;
                OnPropertyChanged("UiTempDetectionOperationTimePlant4Value");
            }
        }
        TimeSpan uiTempoperationTimePlant5Value;
        [DataMember]
        public TimeSpan UiTempDetectionOperationTimePlant5Value
        {
            get
            {
                return uiTempoperationTimePlant5Value;
            }

            set
            {
                uiTempoperationTimePlant5Value = value;
                OnPropertyChanged("UiTempDetectionOperationTimePlant5Value");
            }
        }
        #endregion


        #region Aishwarya
        [DataMember]
        public uint? IdGroup
        {
            get
            {
                return idGroup;
            }

            set
            {
                idGroup = value;
                OnPropertyChanged("IdGroup");
            }
        }

        [DataMember]
        public int? OrderNumber
        {
            get
            {
                return orderNumber;
            }

            set
            {
                orderNumber = value;
                OnPropertyChanged("OrderNumber");
            }
        }

        [DataMember]
        public bool IsCurrentDetection
        {
            get
            {
                return isCurrentDetection;
            }

            set
            {
                isCurrentDetection = value;
                OnPropertyChanged("IsCurrentDetection");
            }
        }
        #endregion

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

        #endregion
    }
}
