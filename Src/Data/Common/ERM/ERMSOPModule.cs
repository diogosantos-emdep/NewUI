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
    public class ERMSOPModule : ModelBase, IDisposable
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

        int idStandardOperationsDictionaryModules;
        int idStandardOperationsDictionary;
        int idCpType;
        int idWorkoperation;

        List<object> listOfSupplements;

        public string FieldName { get; set; }

        double? observedTime;
        #region GEOS2-3954 Time format HH:MM:SS
        private TimeSpan uITempobservedTime;
        private TimeSpan uITempNormalTime;
        private bool isObservedTimeHoursExist;
        private bool isNormalTimeHoursExist;
        private bool isOperationTimeHoursExist;
        private string uIstringTempobservedTime;
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
        #endregion
        double? activity;
        double? normalTime;
        bool isDeleteButton = true;
        Visibility isDeleteButtonVisibility = Visibility.Visible;
        bool observedTimeIsReadOnly = true;
        bool isReadOnly = false;
        bool isValidateOnTextInput = true;

        bool isInactiveWorkOperation = false;
        //[GEOS2-3933][Rupali Sarode][21/09/2022]
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
        public float? StandardSupplements
        {
            get
            {
                return standardSupplements;
            }

            set
            {
                standardSupplements = value;
                OnPropertyChanged("StandardSupplements");
            }
        }
        float? standardOperationTime;
        [DataMember]
        public float? StandardOperationTime
        {
            get
            {
                return standardOperationTime;
            }

            set
            {
                standardOperationTime = value;
                OnPropertyChanged("StandardOperationTime");
            }
        }

        float? supplementPlant1Value;
        [DataMember]
        public float? SupplementPlant1Value
        {
            get
            {
                return supplementPlant1Value;
            }

            set
            {
                supplementPlant1Value = value;
                OnPropertyChanged("SupplementPlant1Value");
            }
        }

        string supplementPlant1Name;
        [DataMember]
        public string SupplementPlant1Name
        {
            get
            {
                return supplementPlant1Name;
            }

            set
            {
                supplementPlant1Name = value;
                OnPropertyChanged("SupplementPlant1Name");
            }
        }

        float? supplementPlant2Value;
        [DataMember]
        public float? SupplementPlant2Value
        {
            get
            {
                return supplementPlant2Value;
            }

            set
            {
                supplementPlant2Value = value;
                OnPropertyChanged("SupplementPlant2Value");
            }
        }

        string supplementPlant2Name;
        [DataMember]
        public string SupplementPlant2Name
        {
            get
            {
                return supplementPlant2Name;
            }

            set
            {
                supplementPlant2Name = value;
                OnPropertyChanged("SupplementPlant2Name");
            }
        }
        float? supplementPlant3Value;
        [DataMember]
        public float? SupplementPlant3Value
        {
            get
            {
                return supplementPlant3Value;
           } 

            set
            {
                supplementPlant3Value = value;
                OnPropertyChanged("SupplementPlant3Value");
            }
        }
        string supplementPlant3Name;
        [DataMember]
        public string SupplementPlant3Name
        {
            get
            {
                return supplementPlant3Name;
            }

            set
            {
                supplementPlant3Name = value;
                OnPropertyChanged("SupplementPlant3Name");
            }
        }
        float? supplementPlant4Value;
        [DataMember]
        public float? SupplementPlant4Value
        {
            get
            {
                return supplementPlant4Value;
            }

            set
            {
                supplementPlant4Value = value;
                OnPropertyChanged("SupplementPlant4Value");
            }
        }
        string supplementPlant4Name;
        [DataMember]
        public string SupplementPlant4Name
        {
            get
            {
                return supplementPlant4Name;
            }

            set
            {
                supplementPlant4Name = value;
                OnPropertyChanged("SupplementPlant4Name");
            }
        }
        float? supplementPlant5Value;
        [DataMember]
        public float? SupplementPlant5Value
        {
            get
            {
                return supplementPlant5Value;
            }

            set
            {
                supplementPlant5Value = value;
                OnPropertyChanged("SupplementPlant5Value");
            }
        }
        string supplementPlant5Name;
        [DataMember]
        public string SupplementPlant5Name
        {
            get
            {
                return supplementPlant5Name;
            }

            set
            {
                supplementPlant5Name = value;
                OnPropertyChanged("SupplementPlant5Name");
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
        public float? OperationTimePlant1Value
        {
            get
            {
                return operationTimePlant1Value;
            }

            set
            {
                operationTimePlant1Value = value;
                OnPropertyChanged("OperationTimePlant1Value");
            }
        }
        string operationTimePlant1Name;
        [DataMember]
        public string OperationTimePlant1Name
        {
            get
            {
                return operationTimePlant1Name;
            }

            set
            {
                operationTimePlant1Name = value;
                OnPropertyChanged("OperationTimePlant1Name");
            }
        }
        float? operationTimePlant2Value;
        [DataMember]
        public float? OperationTimePlant2Value
        {
            get
            {
                return operationTimePlant2Value;
            }

            set
            {
                operationTimePlant2Value = value;
                OnPropertyChanged("OperationTimePlant2Value");
            }
        }

        string operationTimePlant2Name;
        [DataMember]
        public string OperationTimePlant2Name
        {
            get
            {
                return operationTimePlant2Name;
            }

            set
            {
                operationTimePlant2Name = value;
                OnPropertyChanged("OperationTimePlant2Name");
            }
        }

        float? operationTimePlant3Value;
        [DataMember]
        public float? OperationTimePlant3Value
        {
            get
            {
                return operationTimePlant3Value;
            }

            set
            {
                operationTimePlant3Value = value;
                OnPropertyChanged("OperationTimePlant3Value");
            }
        }
        string operationTimePlant3Name;
        [DataMember]
        public string OperationTimePlant3Name
        {
            get
            {
                return operationTimePlant3Name;
            }

            set
            {
                operationTimePlant3Name = value;
                OnPropertyChanged("OperationTimePlant3Name");
            }
        }
        float? operationTimePlant4Value;
        [DataMember]
        public float? OperationTimePlant4Value
        {
            get
            {
                return operationTimePlant4Value;
            }

            set
            {
                operationTimePlant4Value = value;
                OnPropertyChanged("OperationTimePlant4Value");
            }
        }
        string operationTimePlant4Name;
        [DataMember]
        public string OperationTimePlant4Name
        {
            get
            {
                return operationTimePlant4Name;
            }

            set
            {
                operationTimePlant4Name = value;
                OnPropertyChanged("OperationTimePlant4Name");
            }
        }

        float? operationTimePlant5Value;
        [DataMember]
        public float? OperationTimePlant5Value
        {
            get
            {
                return operationTimePlant5Value;
            }

            set
            {

                operationTimePlant5Value = value;
                OnPropertyChanged("OperationTimePlant5Value");
            }
        }
        string operationTimePlant5Name;
        [DataMember]
        public string OperationTimePlant5Name
        {
            get
            {
                return operationTimePlant5Name;
            }

            set
            {
                operationTimePlant5Name = value;
                OnPropertyChanged("OperationTimePlant5Name");
            }
        }

        #region GEOS2-3954 Time format HH:MM:SS
        //private TimeSpan uITempobservedTime;
        //private TimeSpan uITempNormalTime;
        //private bool isObservedTimeHoursExist;
        //private bool isNormalTimeHoursExist;

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
                    //TimeSpan time = TimeSpan.Parse(uiTempoperationTimePlant1Value);
                    TimeSpan time = uiTempoperationTimePlant1Value;
                    if (time.Hours > 0)
                    {
                        IsPlant1ValueOperationTime = true;
                    }
                    else
                    {
                        IsPlant1ValueOperationTime = false;
                    }
                }catch (Exception ex) { }
                
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


        [DataMember]
        public int IdStandardOperationsDictionaryModules
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
        public double? ObservedTime
        {
            get
            {
                return observedTime;
            }

            set
            {
                observedTime = value;
                //NormalTime = (double ?) (ObservedTime * Activity / 100);
                 NormalTime = Math.Round(Convert.ToDouble(ObservedTime * Activity / 100),2);
                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //string temnormaltime = Convert.ToString(NormalTime);
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
                //UITempNormalTime = TimeSpan.FromSeconds(nt1);


                #region [GEOS2-5008][gulab lakade][1 11 2023]
                //UITempNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(NormalTime));
                UITempNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(NormalTime)));
                #endregion

                OnPropertyChanged("ObservedTime");
            }
        }
        [DataMember]
        public double? Activity
        {
            get
            {
                return activity;
            }

            set
            {
                activity = value;
                NormalTime = Math.Round(Convert.ToDouble(ObservedTime * Activity / 100),2);
                #region [GEOS2-5008][gulab lakade][1 11 2023]
                //UITempNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(NormalTime));
                UITempNormalTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(NormalTime)));
                #endregion

                //#region GEOS2-3954 gulab lakade time format HH:mm:ss
                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //string temnormaltime = Convert.ToString(NormalTime);
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



                //UITempNormalTime = TimeSpan.FromSeconds(nt1);
                //#endregion
                OnPropertyChanged("Activity");
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
                OnPropertyChanged("UITempobservedTime");
                //if (UITempobservedTime != null)
                //{
                //int TempOTDay = UITempobservedTime.Days;
                //int TempOTHours = UITempobservedTime.Hours;
                //int TempOTminute = UITempobservedTime.Minutes;
                //int TempOTSecond = UITempobservedTime.Seconds;
                //string tempstring = Convert.ToString(((TempOTDay * 24) + TempOTHours) * 60 + TempOTminute) + "." + TempOTSecond;

                //float tempfloat = float.Parse(tempstring);
                //ObservedTime = tempfloat;
                //    if (tempfloat != null && tempfloat > 0 && Activity > 0)
                //        NormalTime = (float)Math.Round(tempfloat * ((float)Activity / 100), 2);
                //    else
                //        NormalTime = 0;
                //    string temnormaltime = Convert.ToString((float)Math.Round(tempfloat * ((float)Activity / 100), 2));
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
                OnPropertyChanged("UITempNormalTime");
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
        #endregion

        [DataMember]
        public double? NormalTime
        {
            get
            {
                return normalTime;
            }

            set
            {
                normalTime = value;
                //Operation Time (s) = NormalTime  *(1 + Supplements(%))
                StandardOperationTime = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(StandardSupplements / 100))), 2);

                //var currentculter = CultureInfo.CurrentCulture;
                //string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                //string tempStandardOperationTime = Convert.ToString(StandardOperationTime);
                //string[] NormaltimeArr = new string[2];
                //int nt1 = 0;
                //int nt2 = 0;
                //if (tempStandardOperationTime.Contains(culterseparator))
                //{
                //    NormaltimeArr = tempStandardOperationTime.Split(Convert.ToChar(culterseparator));
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
                //    NormaltimeArr = tempStandardOperationTime.Split(Convert.ToChar(culterseparator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt1 = (nt1 * 60);
                //}
                //if (TimeSpan.FromSeconds(nt1).Hours == 0)
                //{
                //    timeconvert = Convert.ToString(TimeSpan.FromSeconds(nt1).Minutes + ":" + TimeSpan.FromSeconds(nt1).Seconds);

                //}
                //else
                //{
                //    timeconvert = Convert.ToString(TimeSpan.FromSeconds(nt1));
                //}
                //UiTempStandardOperationTimeName =ConverttoTimespan(Convert.ToString(StandardOperationTime));
                //UiTempStandardOperationTimeName = TimeSpan.FromSeconds(nt1); //Convert.ToString(ConverttoTimespan(Convert.ToDouble(StandardOperationTime)));


                //OperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant1Value / 100))), 2);
                ////string t1 = ConverttoTimespan(Convert.ToString(OperationTimePlant1Value)).ToString(@"mm\:ss");
                //// UiTempoperationTimePlant1Value = t1; 
                //UiTempoperationTimePlant1Value=Convert.ToString(ConverttoTimespan(Convert.ToString(OperationTimePlant1Value)));
                //OperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant2Value / 100))), 2);
                //UiTempoperationTimePlant2Value = Convert.ToString(ConverttoTimespan(Convert.ToString(OperationTimePlant2Value)));
                //OperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant3Value / 100))), 2);
                //UiTempoperationTimePlant3Value = Convert.ToString(ConverttoTimespan(Convert.ToString(OperationTimePlant3Value)));
                //OperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant4Value / 100))), 2);
                //UiTempoperationTimePlant4Value = Convert.ToString(ConverttoTimespan(Convert.ToString(OperationTimePlant4Value)));
                //OperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant5Value / 100))), 2);
                ////GEOS2-4046 Gulab Lakade
                //UiTempoperationTimePlant5Value = Convert.ToString(ConverttoTimespan(Convert.ToString(OperationTimePlant5Value)));


                // UiTempStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(StandardOperationTime)); //Convert.ToString(ConverttoTimespan(Convert.ToDouble(StandardOperationTime)));


                // OperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant1Value / 100))), 2);
                // //string t1 = ConverttoTimespan(Convert.ToString(OperationTimePlant1Value)).ToString(@"mm\:ss");
                // // UiTempoperationTimePlant1Value = t1; 
                // UiTempoperationTimePlant1Value = TimeSpan.FromMinutes(Convert.ToDouble(OperationTimePlant1Value));
                // OperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant2Value / 100))), 2);
                // UiTempoperationTimePlant2Value = TimeSpan.FromMinutes(Convert.ToDouble(OperationTimePlant2Value));
                // OperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant3Value / 100))), 2);
                // UiTempoperationTimePlant3Value = TimeSpan.FromMinutes(Convert.ToDouble(OperationTimePlant3Value));
                // OperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant4Value / 100))), 2);
                //UiTempoperationTimePlant4Value =TimeSpan.FromMinutes(Convert.ToDouble(OperationTimePlant4Value));
                // OperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant5Value / 100))), 2);
                // //GEOS2-4046 Gulab Lakade
                // UiTempoperationTimePlant5Value = TimeSpan.FromMinutes(Convert.ToDouble(OperationTimePlant5Value));
                #region [GEOS2-5008][gulab lakade][1 11 2023]
                UiTempStandardOperationTimeName =ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(StandardOperationTime)));
                

                OperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant1Value / 100))), 2);
                //string t1 = ConverttoTimespan(Convert.ToString(OperationTimePlant1Value)).ToString(@"mm\:ss");
                // UiTempoperationTimePlant1Value = t1; 
                UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(OperationTimePlant1Value)));
               
                OperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant2Value / 100))), 2);
                UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(OperationTimePlant2Value)));
               
                OperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant3Value / 100))), 2);
                UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(OperationTimePlant3Value)));
                
                OperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant4Value / 100))), 2);
                UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(OperationTimePlant4Value)));
                
                OperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(NormalTime * (1 + Convert.ToDouble(SupplementPlant5Value / 100))), 2);
                //GEOS2-4046 Gulab Lakade
                UiTempoperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(OperationTimePlant5Value)));
                
                #endregion

                OnPropertyChanged("NormalTime");
            }
        }

        //public TimeSpan ConverttoTimespan(string temp)
        //{
        //    //string timeconvert=string.Empty;
        //    TimeSpan timeconvert ;
        //    try
        //    {
                
        //        #region
        //        var currentculter = CultureInfo.CurrentCulture;
        //        string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
        //        string tempStandardOperationTime = Convert.ToString(temp);
        //        string[] NormaltimeArr = new string[2];
        //        int nt1 = 0;
        //        int nt2 = 0;
        //        if (tempStandardOperationTime.Contains(culterseparator))
        //        {
        //            NormaltimeArr = tempStandardOperationTime.Split(Convert.ToChar(culterseparator));
        //            nt1 = int.Parse(NormaltimeArr[0]);
        //            nt2 = int.Parse(NormaltimeArr[1]);
        //            if (Convert.ToString(NormaltimeArr[1]).Length == 1)
        //            {
        //                nt1 = (nt1 * 60) + nt2 * 10;
        //            }
        //            else
        //            {
        //                nt1 = (nt1 * 60) + nt2;
        //            }
        //        }
        //        else
        //        {
        //            NormaltimeArr = tempStandardOperationTime.Split(Convert.ToChar(culterseparator));
        //            nt1 = int.Parse(NormaltimeArr[0]);
        //            nt1 = (nt1 * 60);
        //        }

        //        //if (TimeSpan.FromSeconds(nt1).Hours == 0)
        //        //{
        //        //    timeconvert = Convert.ToString(TimeSpan.FromSeconds(nt1).Minutes + ":" + TimeSpan.FromSeconds(nt1).Seconds);

        //        //}
        //        //else
        //        //{
        //        //    timeconvert = Convert.ToString(TimeSpan.FromSeconds(nt1));
        //        //}
        //        // UiTempStandardOperationTimeName = TimeSpan.FromSeconds(nt1).ToString(@"mm\:ss");
        //        #endregion
        //        // timeconvert = Convert.ToString(TimeSpan.FromSeconds(nt1));
        //        timeconvert = TimeSpan.FromSeconds(nt1);
        //        return timeconvert;
        //    }
        //    catch (Exception ex)
        //    {
        //        timeconvert = TimeSpan.FromSeconds(0);
        //        return timeconvert;
        //    }
        //}

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
            set {

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

        private uint modulePosition;//[Geos2-5629][Aishwarya Ingale][26 06 2024]

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
        public uint ModulePosition
        {
            get { return modulePosition; }
            set
            {
                modulePosition = value;
                OnPropertyChanged("ModulePosition");
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

        #endregion
    }

    public class ERMColumn
    {
        // Specifies the name of a data source field to which the column is bound. 
        public string FieldName { get; set; }
    }
    // Corresponds to a band column. 
    public class Band
    {
        // Specifies the band header.
        public string Header { get; set; }
        public ObservableCollection<ERMColumn> ChildColumns { get; set; }
    }
}
