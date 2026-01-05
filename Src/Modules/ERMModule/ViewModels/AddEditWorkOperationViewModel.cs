using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using DevExpress.Spreadsheet;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Modules.ERM.CommonClasses;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class AddEditWorkOperationViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service

        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IERMService ERMService = new ERMServiceController("localhost:6699");
        IGeosRepositoryService geosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion


        #region declaration
        private UInt32 idWorkOperation;
        private bool isSave;
        private string windowHeader;
        private bool isNew;
        private ObservableCollection<Language> languages;
        private Language languageSelected;
        private List<LookupValue> statusList;
        private List<LookupValue> typeList;
        private ObservableCollection<Stages> stagesList;
        //private List<Stages> stagesList;
        private ObservableCollection<WorkOperation> parentList;
        private ObservableCollection<WorkOperation> allWorkOperationList;
        private ObservableCollection<WorkOperation> orderList;
        private LookupValue selectedStatus;
        private LookupValue selectedType;
        //private ObservableCollection<Stages> selectedStages;
        private List<object> selectedStages;
        private WorkOperation clonedWorkOperation;
        private string description;
        private string description_en;
        private string description_es;
        private string description_fr;
        private string description_pt;
        private string description_ro;
        private string description_ru;
        private string description_zh;
        private string name;
        private string name_en;
        private string name_es;
        private string name_fr;
        private string name_pt;
        private string name_ro;
        private string name_ru;
        private string name_zh;
        private string informationError;
        private bool isCheckedCopyNameReadOnly;
        private bool IsCopyDescription;
        private bool isEnabledCopyNameReadOnly;
        private bool isReadOnlyName;
        private string code;
        private bool isCheckedCopyNameAndDescription;
        private string error = string.Empty;
        public bool IsFromInformation = false;
        public ObservableCollection<WorkOperation> tempparentList { get; set; }
        public ObservableCollection<WorkOperation> temporderList { get; set; }
        private WorkOperation selectedParent;
        private WorkOperation selectedOrder;
        private UInt32 idStage;
        private string order;
        private UInt64 idOrder;
        private ulong? idParent;
        private string parent;
        private WorkOperation newWorkOperation;
        private string selectedCurrentStage;
        private float distance; //[001][kshinde][GEOS2-3709][09/06/2022]
        private float? observedTime;
        #region GEOS2-3954 Time format HH:MM:SS
        private TimeSpan uITempobservedTime;
        private TimeSpan uITempNormalTime;
        private string uIstringTempobservedTime;
        string decimalSeperator;
        bool isHourseExist;
        bool isNormalTimeHourseExist;
        #endregion
        private Int32 activity;
        private float normalTime;
        private string detectedProblems;
        private string remarks; //[GEOS2-3933][Rupali Sarode][19/09/2022]
        private string improvementsProposals;
        private uint oldPosition;
        #region GEOS2-3880 Work Operation log
        private ObservableCollection<WorkOperationChangeLog> workOperationChangeLogList;
        private ObservableCollection<WorkOperationChangeLog> workOperationAllChangeLogList;
        #endregion
        #region GEOS2-3880 change log gulab lakade
        private string tempworkstagename;
        //private WorkOperation selectedParentoldTemp;
        #endregion
        private bool isFatherFlag;
        private List<WorkOperation> previousOrderList; //[GEOS-4933][Rupali Sarode][04-12-2023]
        MaximizedElementPosition maximizedElementPosition;
        #endregion


        #region Events handlers
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion


        #region Icommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand AcceptWorkOperationCommand { get; set; }
        public ICommand CommandEditValueChanged { get; set; }
        public ICommand CheckedCopyNameDescriptionCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand ChangeCodeCommand { get; set; }
        public ICommand CommandStagesChanged { get; set; }
        public ICommand changeDetectedProblems { get; set; }
        public ICommand changeImprovementsProposals { get; set; }
        public ICommand LogsHidePanelCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }

        #endregion
        #region Properties
        public bool IsSave
        {
            get
            {
                return isSave;
            }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public bool IsNew
        {
            get
            {
                return isNew;
            }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public WorkOperation ClonedWorkOperation
        {
            get { return clonedWorkOperation; }
            set
            {
                clonedWorkOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedWorkOperation"));
            }
        }
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Languages"));
            }
        }
        public Language LanguageSelected
        {
            get
            {
                return languageSelected;
            }

            set
            {
                languageSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LanguageSelected"));
            }
        }
        public List<LookupValue> StatusList
        {
            get { return statusList; }
            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        public List<LookupValue> TypeList
        {
            get { return typeList; }
            set
            {
                typeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeList"));
            }
        }
        public ObservableCollection<Stages> StagesList
        {
            get { return stagesList; }
            set
            {
                stagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StagesList"));
            }
        }
        public ObservableCollection<WorkOperation> ParentList
        {
            get { return parentList; }
            set
            {
                parentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentList"));
            }
        }

        public ObservableCollection<WorkOperation> AllWorkOperationList
        {
            get { return allWorkOperationList; }
            set
            {
                allWorkOperationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllWorkOperationList"));
            }
        }

        public ObservableCollection<WorkOperation> OrderList
        {
            get { return orderList; }
            set
            {
                orderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OrderList"));
            }
        }
        //public ObservableCollection<Stages> SelectedStages
        //{
        //    get
        //    {
        //        return selectedStages;
        //    }

        //    set
        //    {
        //        selectedStages = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedStages"));
        //    }
        //}
        public List<object> SelectedStages
        {
            get
            {
                return selectedStages;
            }

            set
            {
                if (value != null && value.Count >= 2)
                {
                    foreach (Stages item in value)
                    {
                        if (item.Code.Equals("---"))
                        {
                            value.Remove(item);
                            break;
                        }
                    }
                }

                selectedStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStages"));
            }
        }
        public LookupValue SelectedStatus
        {
            get
            {
                return selectedStatus;
            }

            set
            {
                selectedStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStatus"));
            }
        }
        public LookupValue SelectedType
        {
            get
            {
                return selectedType;
            }

            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }
        public bool IsReadOnlyName
        {
            get
            {
                return isReadOnlyName;
            }

            set
            {
                isReadOnlyName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyName"));
            }
        }
        public bool IsCheckedCopyNameReadOnly
        {
            get
            {
                return isCheckedCopyNameReadOnly;
            }

            set
            {
                isCheckedCopyNameReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyNameReadOnly"));
            }
        }
        public bool IsEnabledCopyNameReadOnly
        {
            get
            {
                return isEnabledCopyNameReadOnly;
            }

            set
            {
                isEnabledCopyNameReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledCopyNameReadOnly"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        public UInt32 IdWorkOperation
        {
            get { return idWorkOperation; }
            set
            {
                idWorkOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdWorkOperation"));
            }
        }
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }

        #region Description

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                if (GeosApplication.Instance.IsPermissionNameEditInPCMArticle == true)
                {
                    IsReadOnlyName = false;
                    IsCheckedCopyNameReadOnly = false;
                    IsEnabledCopyNameReadOnly = true;

                    if (!(string.IsNullOrEmpty(description)))
                        InformationError = " ";

                    else
                        InformationError = null;
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
                else
                {
                    description = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                    IsReadOnlyName = true;
                    IsCheckedCopyNameReadOnly = false;
                    IsEnabledCopyNameReadOnly = false;
                }
            }
        }

        public string Description_en
        {
            get
            {
                return description_en;
            }
            set
            {
                description_en = value;
                description_en = description_en.Trim(' ', '\r');
                if (!(string.IsNullOrEmpty(description_en)))
                    InformationError = " ";

                else
                    InformationError = null;
                OnPropertyChanged(new PropertyChangedEventArgs("Description_en"));
            }
        }

        public string Description_es
        {
            get
            {
                return description_es;
            }
            set
            {
                description_es = value;
                description_es = description_es.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_es"));
            }
        }

        public string Description_fr
        {
            get
            {
                return description_fr;
            }
            set
            {
                description_fr = value;
                description_fr = description_fr.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_fr"));
            }
        }

        public string Description_pt
        {
            get
            {
                return description_pt;
            }
            set
            {
                description_pt = value;
                description_pt = description_pt.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_pt"));
            }
        }

        public string Description_ro
        {
            get
            {
                return description_ro;
            }
            set
            {
                description_ro = value;
                description_ro = description_ro.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ro"));
            }
        }

        public string Description_ru
        {
            get
            {
                return description_ru;
            }
            set
            {
                description_ru = value;
                description_ru = description_ru.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_ru"));
            }
        }

        public string Description_zh
        {
            get
            {
                return description_zh;
            }
            set
            {
                description_zh = value;
                description_zh = description_zh.Trim(' ', '\r');
                OnPropertyChanged(new PropertyChangedEventArgs("Description_zh"));
            }
        }
        #endregion
        #region Name
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }
        public string Name_en
        {
            get { return name_en; }
            set
            {
                name_en = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_en"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }

        public string Name_es
        {
            get { return name_es; }
            set
            {
                name_es = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_es"));
            }
        }

        public string Name_fr
        {
            get { return name_fr; }
            set
            {
                name_fr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_fr"));
            }
        }

        public string Name_pt
        {
            get { return name_pt; }
            set
            {
                name_pt = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_pt"));
            }
        }

        public string Name_ro
        {
            get { return name_ro; }
            set
            {
                name_ro = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ro"));
            }
        }

        public string Name_zh
        {
            get { return name_zh; }
            set
            {
                name_zh = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_zh"));
            }
        }

        public string Name_ru
        {
            get { return name_ru; }
            set
            {
                name_ru = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name_ru"));
            }
        }
        #endregion
        public bool IsCheckedCopyNameAndDescription
        {
            get
            {
                return isCheckedCopyNameAndDescription;
            }

            set
            {
                isCheckedCopyNameAndDescription = value;
                if (isCheckedCopyNameAndDescription)
                {
                    EnableListBoxForLanguage = false;
                }
                else
                {
                    EnableListBoxForLanguage = true;

                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedCopyNameAndDescription"));
            }
        }
        private bool enableListBoxForLanguage;
        public bool EnableListBoxForLanguage
        {
            get
            {
                return enableListBoxForLanguage;
            }

            set
            {
                enableListBoxForLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnableListBoxForLanguage"));
            }
        }
        public WorkOperation UpdatedWorkOperation { get; set; }
        public WorkOperation SelectedParent
        {
            get
            {
                return selectedParent;
            }

            set
            {
                selectedParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParent"));
            }
        }
        public WorkOperation SelectedOrder
        {
            get
            {
                return selectedOrder;
            }

            set
            {
                selectedOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOrder"));
            }
        }
        public string KeyName { get; set; }
        public UInt32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdStage"));
            }
        }
        public UInt64 IdOrder
        {
            get { return idOrder; }
            set
            {
                idOrder = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOrder"));
            }
        }
        public UInt64? IdParent
        {
            get { return idParent; }
            set
            {
                idParent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdParent"));
            }
        }
        public string Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Parent"));
            }
        }
        private int selectedIndexForStages;
        public int SelectedIndexForStages
        {
            get
            {
                return selectedIndexForStages;
            }

            set
            {
                selectedIndexForStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexForStages"));
            }
        }

        public WorkOperation NewWorkOperation
        {
            get
            {
                return newWorkOperation;
            }

            set
            {
                newWorkOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewWorkOperation"));
            }
        }
        int workOperation_count;
        public int WorkOperation_count
        {
            get
            {
                return workOperation_count;
            }

            set
            {
                workOperation_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperation_count"));
            }
        }
        private uint position;
        public uint Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Position"));
            }
        }

        public string SelectedCurrentStage
        {
            get { return selectedCurrentStage; }
            set
            {
                selectedCurrentStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrentStage"));
            }
        }

        //[001][kshinde][GEOS2-3709][09/06/2022]
        public float Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Distance"));
            }
        }
        public float? ObservedTime
        {
            get
            {
                return observedTime;
            }
            set
            {
                observedTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObservedTime"));
                if (value != null)
                {
                    InformationError = null;
                }
                if (ObservedTime != null && ObservedTime > 0 && Activity > 0)
                    NormalTime = (float)Math.Round(ObservedTime.Value * ((float)Activity / 100), 2);
                else
                    NormalTime = 0;


            }
        }
        #region GEOS2-3954 Time format HH:MM:SS

        public TimeSpan UITempobservedTime
        {
            get { return uITempobservedTime; }
            set
            {
                uITempobservedTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UITempobservedTime"));
                if (value != null)
                {
                    InformationError = null;
                }
                if (UITempobservedTime != null)
                {
                    int TempOTDay = UITempobservedTime.Days;
                    int TempOTHours = UITempobservedTime.Hours;
                    if (TempOTHours > 0)
                        IsHourseExist = true;
                    else
                        IsHourseExist = false;
                    int TempOTminute = UITempobservedTime.Minutes;
                    int TempOTSecond = UITempobservedTime.Seconds;
                    //string tempstring = Convert.ToString(((TempOTDay * 24) + TempOTHours) * 60 + TempOTminute) + DecimalSeperator + TempOTSecond;

                    //float tempfloat = float.Parse(tempstring);
                    ObservedTime = (float?)Math.Round(Convert.ToDouble(UITempobservedTime.TotalMinutes), 2);
                    if (ObservedTime != null && ObservedTime > 0 && Activity > 0)
                        NormalTime = (float)Math.Round((float)ObservedTime * ((float)Activity / 100), 2);
                    else
                        NormalTime = 0;



                    //string temnormaltime = Convert.ToString(NormalTime);
                    //string[] NormaltimeArr = new string[2];
                    //int nt1 = 0;
                    //int nt2 = 0;
                    //if (temnormaltime.Contains(DecimalSeperator))
                    //{
                    //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                    //    nt1 = int.Parse(NormaltimeArr[0]);
                    //    nt2 = int.Parse(NormaltimeArr[1]);
                    //    //nt1 = (nt1 * 60) + nt2;
                    //    // shubham[skadam]GEOS2-4046 Some issue in WO & SOD 24 11 2022
                    //    nt1 = (int) uITempobservedTime.TotalSeconds;
                    //}
                    //else
                    //{
                    //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                    //    nt1 = int.Parse(NormaltimeArr[0]);
                    //    nt1 = (nt1 * 60);
                    //}
                    //UITempNormalTime = TimeSpan.FromSeconds(nt1);
                    #region [GEOS2-4982][gulab lakade][25 10 2023]
                    UITempNormalTime = TimeSpan.FromMinutes(NormalTime);
                    if(UITempNormalTime.Milliseconds>=600)
                    {
                       
                        TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                        UITempNormalTime = UITempNormalTime.Add(Second);
                        UITempNormalTime = UITempNormalTime.Add(-TimeSpan.FromMilliseconds(UITempNormalTime.Milliseconds));

                    }
                    else
                    {
                        UITempNormalTime = UITempNormalTime.Add(-TimeSpan.FromMilliseconds(UITempNormalTime.Milliseconds));
                    }
                    #endregion


                }
            }
        }
        public string UIstringTempobservedTime
        {
            get { return uIstringTempobservedTime; }
            set
            {
                uIstringTempobservedTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UIstringTempobservedTime"));
            }
        }
        public TimeSpan UITempNormalTime
        {
            get { return uITempNormalTime; }
            set
            {
                uITempNormalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UITempNormalTime"));
                if (uITempNormalTime.Hours <= 0)
                    IsNormalTimeHourseExist = false;
                else
                    IsNormalTimeHourseExist = true;

            }
        }
        public string DecimalSeperator
        {
            get
            {
                return decimalSeperator;
            }
            set
            {
                decimalSeperator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DecimalSeperator"));
            }
        }
        public bool IsHourseExist
        {
            get
            {
                return isHourseExist;
            }

            set
            {
                isHourseExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsHourseExist"));
            }
        }

        public bool IsNormalTimeHourseExist
        {
            get
            {
                return isNormalTimeHourseExist;
            }

            set
            {
                isNormalTimeHourseExist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormalTimeHourseExist"));
            }
        }
        #endregion
        public Int32 Activity
        {
            get
            {
                return activity;
            }
            set
            {
                activity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Activity"));
                if (value >= 0)
                {
                    InformationError = null;
                }
                if (ObservedTime != null && ObservedTime > 0 && Activity > 0)
                    NormalTime = (float)Math.Round(ObservedTime.Value * ((float)Activity / 100), 2);
                else
                    NormalTime = 0;
                #region [GEOS2-4982][gulab lakade][25 10 2023]
                UITempNormalTime = TimeSpan.FromMinutes(NormalTime);
                if (UITempNormalTime.Milliseconds >= 600)
                {
                    TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                    UITempNormalTime = UITempNormalTime.Add(Second);
                    UITempNormalTime = UITempNormalTime.Add(-TimeSpan.FromMilliseconds(UITempNormalTime.Milliseconds));
                }
                else
                {
                    UITempNormalTime = UITempNormalTime.Add(-TimeSpan.FromMilliseconds(UITempNormalTime.Milliseconds));
                }
                #endregion
                //#region GEOS2-3954 HH:mm:ss
                //string temnormaltime = Convert.ToString(NormalTime);
                //string[] NormaltimeArr = new string[2];
                //int nt1 = 0;
                //int nt2 = 0;
                //if (temnormaltime.Contains(DecimalSeperator))
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt2 = int.Parse(NormaltimeArr[1]);
                //    nt1 = (nt1 * 60) + nt2;
                //}
                //else
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt1 = (nt1 * 60);
                //}
                //UITempNormalTime = TimeSpan.FromSeconds(nt1);
                //#endregion
            }
        }
        public float NormalTime
        {
            get
            {
                return normalTime;
            }
            set
            {
                normalTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NormalTime"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));

            }
        }

        public string DetectedProblems
        {
            get
            {
                return detectedProblems;
            }
            set
            {
                detectedProblems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectedProblems"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }


        public string ImprovementsProposals
        {
            get { return improvementsProposals; }
            set
            {
                improvementsProposals = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImprovementsProposals"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }
        #region GEOS2-3880 change log gulab lakade
        //string Tempworkstagename = string.Empty;
        public string Tempworkstagename
        {
            get { return tempworkstagename; }
            set
            {
                tempworkstagename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tempworkstagename"));
                if (!string.IsNullOrEmpty(value))
                {
                    InformationError = null;
                }
            }
        }
        //public WorkOperation SelectedParentoldTemp
        //{
        //    get
        //    {
        //        return selectedParentoldTemp;
        //    }

        //    set
        //    {
        //        selectedParentoldTemp = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedParentoldTemp"));
        //    }
        //}
        #endregion


        #region GEOS2-3880 work operation gulab lakade

        public ObservableCollection<WorkOperationChangeLog> WorkOperationChangeLogList
        {
            get { return workOperationChangeLogList; }
            set
            {
                workOperationChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationChangeLogList"));
            }
        }
        public ObservableCollection<WorkOperationChangeLog> WorkOperationAllChangeLogList
        {
            get { return workOperationAllChangeLogList; }
            set
            {
                workOperationAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationAllChangeLogList"));
            }
        }
        #endregion
        public uint OldPosition
        {
            get
            {
                return oldPosition;
            }

            set
            {
                oldPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldPosition"));
            }
        }

        #region [GEOS2-4994][Rupali Sarode][23-11-2023]
        public virtual bool DialogResult { get; set; }
        #endregion [GEOS2-4994][Rupali Sarode][23-11-2023]
        public virtual string ResultFileName { get; set; }

        public bool IsFatherFlag
        {
            get { return isFatherFlag; }
            set
            {
                isFatherFlag = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsFatherFlag"));
            }
        }

        ////[GEOS-4933][Rupali Sarode][04-12-2023]
        public List<WorkOperation> PreviousOrderList
        {
            get { return previousOrderList; }
            set
            {
                previousOrderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PreviousOrderList"));
            }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }
        #endregion

        #region Constructor
        public AddEditWorkOperationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddEditWorkOperationViewModel()..."), category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                //ChangeLanguageCommand = new DelegateCommand<object>(SetNameToLanguage);
                AcceptWorkOperationCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                CommandEditValueChanged = new DelegateCommand<object>(CommandEditValueChangedAction);

                CheckedCopyNameDescriptionCommand = new DelegateCommand<object>(CheckedCopyNameDescription);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeNameCommand = new DelegateCommand<EditValueChangingEventArgs>(SetNameToLanguage);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameDescriptionByLanguge);
                //EditCommand = new DelegateCommand<object>(EditAction);
                CommandStagesChanged = new DelegateCommand<object>(CommandStageChangedAction);
                #region GEOS2-3880
                LogsHidePanelCommand = new RelayCommand(new Action<object>(HideLogPanel));
                WorkOperationChangeLogList = new ObservableCollection<WorkOperationChangeLog>();
                #endregion
                #region GEOS2-3954 gulab lakade time change HH:mm:ss
                var currentculter = CultureInfo.CurrentCulture;
                DecimalSeperator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                #endregion

                ExportToExcelCommand = new DelegateCommand<object>(ExportToExcel); //[GEOS2-4994][Rupali Sarode][23-11-2023]

                GeosApplication.Instance.Logger.Log(string.Format("Method AddEditWorkOperationViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditWorkOperationViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()..."), category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][05-04-2022][cpatil][GEOS2-3646]
        private void CommandStageChangedAction(object obj)
        {

            GeosApplication.Instance.Logger.Log("Method CommandStageChangedAction()...", category: Category.Info, priority: Priority.Low);

            #region GEOS2-3880 change log gulab lakade
            Tempworkstagename = string.Empty;
            #endregion
            try
            {
                if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Normal)
                {
                    //return;
                    if (SelectedStages != null)
                    {
                        if (SelectedStages.Count() == 1)
                        {
                            foreach (Stages item in SelectedStages)
                            {
                                #region GEOS2-3880 change log gulab lakade
                                if (string.IsNullOrEmpty(Tempworkstagename))
                                {
                                    Tempworkstagename = Convert.ToString(item.CodeWithName);
                                }
                                else
                                {
                                    Tempworkstagename = Tempworkstagename + ", " + Convert.ToString(item.CodeWithName);
                                }

                                #endregion
                                //SelectedStages.RemoveAll(r => r.Equals(item));
                                if (item.Code.Equals("---"))
                                {
                                    return;
                                }
                                else
                                {

                                }
                            }
                        }
                        else
                        {
                            foreach (Stages item in SelectedStages)
                            {
                                #region GEOS2-3880 change log gulab lakade
                                if (string.IsNullOrEmpty(Tempworkstagename))
                                {
                                    Tempworkstagename = Convert.ToString(item.CodeWithName);
                                }
                                else
                                {
                                    Tempworkstagename = Tempworkstagename + ", " + Convert.ToString(item.CodeWithName);
                                }
                                #endregion
                                //SelectedStages.RemoveAll(r => r.Equals(item));
                                if (item.Code.Equals("---"))
                                {
                                    SelectedStages.Remove(item);
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }


                    }
                    if (WindowHeader != Application.Current.FindResource("EditWorkOperationHeader").ToString())
                    {

                        if (SelectedStages != null)
                        {
                            List<Stages> TempSelectedStagesList = new List<Stages>();
                            TempSelectedStagesList = SelectedStages.Cast<Stages>().ToList();

                            if (TempSelectedStagesList.Any(tsl => tsl.IdStage == 0))
                            {
                                var tempRemovedObject = TempSelectedStagesList.Where(x => x.IdStage == 0);
                                TempSelectedStagesList.RemoveAll(x => x.IdStage == 0);
                                TempSelectedStagesList = new List<Stages>(TempSelectedStagesList);
                                SelectedStages = new List<object>();
                                //SelectedStages.Clear();
                                foreach (var temp in TempSelectedStagesList)
                                {
                                    SelectedStages.Add(temp);
                                }
                            }

                            if (TempSelectedStagesList.Count > 1 && TempSelectedStagesList.Count != 1)  // less than 2
                            {
                                //string tempCode = ERMService.GetLatestWorkOperationCode();
                                //[001] WOP changed to GOP
                                //Service chaged from GetLatestWorkOperationCodeByCode to GetLatestWorkOperationCodeByCode_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                                //string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2350("GOP");
                                //IERMService ERMService = new ERMServiceController("localhost:6699");
                                string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2620("GOP");//[GEOS2-7031][gulab lakade][25 02 2025]
                                Code = "GOP" + tempCode;
                            }

                            else if (TempSelectedStagesList.Count == 1)
                            {
                                //Service chaged from GetLatestWorkOperationCodeByCode to GetLatestWorkOperationCodeByCode_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                                //string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2350(Convert.ToString(TempSelectedStagesList[0].Code));
                                //IERMService ERMService = new ERMServiceController("localhost:6699");
                                string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2620(Convert.ToString(TempSelectedStagesList[0].Code));//[GEOS2-7031][gulab lakade][25 02 2025]
                                Code = TempSelectedStagesList[0].Code + tempCode;
                            }
                            else // count is greater than 1
                            {
                                Code = null;
                            }
                            string abc = Code.Substring(0, 3);
                            FillparentList(abc, 0);
                        }
                        else
                        {
                            Code = string.Empty;
                            if (ParentList != null)
                            {
                                SelectedParent = ParentList.FirstOrDefault();
                            }
                        }
                    }

                    else
                    {
                        // SelectedCurrentStage = ClonedWorkOperation.Code.Substring(0, 3);
                        //if (SelectedStages.Any(Code = SelectedCurrentStage))
                        if (SelectedStages != null)
                        {
                            List<Stages> TempSelectedStagesList = new List<Stages>();
                            TempSelectedStagesList = SelectedStages.Cast<Stages>().ToList();

                            if (TempSelectedStagesList.Any(tsl => tsl.IdStage == 0))
                            {
                                var tempRemovedObject = TempSelectedStagesList.Where(x => x.IdStage == 0);
                                TempSelectedStagesList.RemoveAll(x => x.IdStage == 0);
                                TempSelectedStagesList = new List<Stages>(TempSelectedStagesList);
                                SelectedStages = new List<object>();
                                foreach (var temp in TempSelectedStagesList)
                                {
                                    SelectedStages.Add(temp);
                                }
                            }

                            if (TempSelectedStagesList.Count > 1 && TempSelectedStagesList.Count != 1)  // less than 2
                            {
                                string CompareString = "GOP";
                                if (SelectedCurrentStage != CompareString)
                                {
                                    //Service chaged from GetLatestWorkOperationCodeByCode to GetLatestWorkOperationCodeByCode_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                                    //string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2350("GOP");
                                    //IERMService ERMService = new ERMServiceController("localhost:6699");
                                    string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2620("GOP");//[GEOS2-7031][gulab lakade][25 02 2025]
                                    Code = "GOP" + tempCode;
                                    //if (!string.IsNullOrEmpty(SelectedCurrentStage))
                                    //{
                                    //    SelectedCurrentStage = "WOP";
                                    //}
                                }
                                else
                                {
                                    Code = ClonedWorkOperation.Code;
                                }
                            }

                            else if (TempSelectedStagesList.Count == 1)
                            {
                                //if (!string.IsNullOrEmpty(SelectedCurrentStage))
                                //{
                                //    SelectedCurrentStage = TempSelectedStagesList[0].Code;
                                //}
                                if (TempSelectedStagesList[0].Code != SelectedCurrentStage)
                                {
                                    //Service chaged from GetLatestWorkOperationCodeByCode to GetLatestWorkOperationCodeByCode_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                                    //string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2350(Convert.ToString(TempSelectedStagesList[0].Code));
                                    //IERMService ERMService = new ERMServiceController("localhost:6699");
                                    string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2620(Convert.ToString(TempSelectedStagesList[0].Code));//[GEOS2-7031][gulab lakade][25 02 2025]
                                    Code = TempSelectedStagesList[0].Code + tempCode;
                                    //if (!string.IsNullOrEmpty(SelectedCurrentStage))
                                    //{
                                    //    SelectedCurrentStage = TempSelectedStagesList[0].Code;
                                    //}
                                    //SelectedCurrentStage = string.Empty;
                                    //SelectedCurrentStage = TempSelectedStagesList[0].Code;
                                }
                                else
                                {
                                    //string tempCode = ERMService.GetLatestWorkOperationCodeByCode(Convert.ToString(TempSelectedStagesList[0].Code));
                                    //Code = TempSelectedStagesList[0].Code + tempCode;
                                    Code = ClonedWorkOperation.Code;
                                }
                                //AllWorkOperationList
                            }
                            string abc = Code.Substring(0, 3);
                            FillparentList(abc, 0);
                            //if (TempSelectedStagesList.Count > 1 && TempSelectedStagesList.Count != 1)  // less than 2
                            //{
                            //    string tempCode = Code; 
                            //    string str = tempCode.Remove(0, 3); 
                            //    Code = "WOP" + str;
                            //}

                            //else if (TempSelectedStagesList.Count == 1)
                            //{
                            //    string tempCode = Code; 
                            //    string str = tempCode.Remove(0, 3);
                            //    Code = TempSelectedStagesList[0].Code + str;
                            //}
                        }
                        else
                        {
                            Code = string.Empty;
                            if (ParentList != null)
                            {
                                SelectedParent = ParentList.FirstOrDefault();
                            }
                        }
                    }
                }
                
                    GeosApplication.Instance.Logger.Log("Method CommandStageChangedAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CommandStageChangedAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        //public void AddInit()
        //{
        //    try
        //    {
        //        Init();
        //        GeosApplication.Instance.Logger.Log(string.Format("Method AddInit..."), category: Category.Info, priority: Priority.Low);

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Error in method AddInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }

        //}
        public void EditInit(WorkOperation SelectedRow)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][18/12/2023]
                Init();
                SelectedStages = null;
                SelectedOrder = null;
                // WorkOperation temp = (ERMService.GetWorkOperationByIdWorkOperation_V2240(Convert.ToInt32(SelectedRow.IdWorkOperation)));
                // WorkOperation temp = (ERMService.GetWorkOperationByIdWorkOperation_V2280(Convert.ToInt32(SelectedRow.IdWorkOperation)));//[001][kshinde][GEOS2-3709][09/06/2022]
                //[GEOS2-3933][Rupali Sarode][19/09/2022]

              //  IERMService ERMService = new ERMServiceController("localhost:6699");
                WorkOperation temp = (ERMService.GetWorkOperationByIdWorkOperation_V2320(Convert.ToInt32(SelectedRow.IdWorkOperation)));

                ClonedWorkOperation = (WorkOperation)temp.Clone();
                SelectedCurrentStage = ClonedWorkOperation.Code.Substring(0, 3);
                IdWorkOperation = temp.IdWorkOperation;
                string onlyCharacters = temp.Code.Substring(0, 3);
                FillparentList(onlyCharacters, Convert.ToInt32(temp.IdParent));
                var currentOparationInList = ParentList.FirstOrDefault(x => x.IdWorkOperation == temp.IdWorkOperation);
                if (currentOparationInList != null)
                {
                    ParentList.Remove(currentOparationInList);
                }
                // ParentList = AllWorkOperationList.Where
                WorkOperation_count = AllWorkOperationList.Where(p => p.IdWorkOperation == SelectedRow.IdWorkOperation).Select(s => s.WorkOperation_count).FirstOrDefault();
                KeyName = AllWorkOperationList.Where(p => p.IdWorkOperation == SelectedRow.IdWorkOperation).Select(s => s.KeyName).FirstOrDefault();
                Code = temp.Code;
                Name = temp.Name;
                IdOrder = temp.Order;
                Position = Convert.ToUInt32(temp.Order);
                OldPosition = Position;
                temp.IdOrder = temp.Order;
                IdStage = temp.IdStage;
                IdParent = temp.IdParent;
                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
                Distance = temp.Distance;//[001][kshinde][09/06/2022][GEOS2-3709]
                #region GEOS2-3954 Time format HH:MM:SS

                //string tempd = Convert.ToString(temp.ObservedTime.Value);
                //string[] parts = new string[2];
                //int i1 = 0;
                //int i2 = 0;
                //if (tempd.Contains(DecimalSeperator))
                //{
                //    parts = tempd.Split(Convert.ToChar(DecimalSeperator));
                //    i1 = int.Parse(parts[0]);
                //    i2 = int.Parse(parts[1]);
                //    i1 = (i1 * 60) + i2;
                //}
                //else
                //{
                //    parts = tempd.Split(Convert.ToChar(DecimalSeperator));
                //    i1 = int.Parse(parts[0]);
                //    i1 = (i1 * 60);
                //}

                TimeSpan tempObserved = TimeSpan.FromMinutes(Convert.ToDouble(temp.ObservedTime.Value));
                if (tempObserved.Milliseconds >= 600)
                {

                    tempObserved = tempObserved.Add(-TimeSpan.FromMilliseconds(tempObserved.Milliseconds));
                    TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                    tempObserved = tempObserved.Add(Second);
                    UITempobservedTime = tempObserved;// TimeSpan.FromMinutes(Convert.ToDouble(temp.ObservedTime.Value));

                }
                else
                {
                    tempObserved = tempObserved.Add(-TimeSpan.FromMilliseconds(tempObserved.Milliseconds));
                   
                    UITempobservedTime = tempObserved;
                }

                //UITempobservedTime = TimeSpan.FromMinutes(Convert.ToDouble(temp.ObservedTime.Value));
                //UITempobservedTime = TimeSpan.FromSeconds(i1);
                //int ts1 = UITempobservedTime.Hours;
                //int ts2 = UITempobservedTime.Minutes;
                //int ts3 = UITempobservedTime.Seconds;


                #endregion
                ObservedTime = temp.ObservedTime.Value;
                Activity = temp.Activity;
                if (observedTime > 0)
                {
                    NormalTime = (float)Math.Round(ObservedTime.Value * ((float)Activity / 100), 2);
                }
                else
                {
                    NormalTime = temp.NormalTime;
                }
                #region GEOS2-3954 Time format HH:MM:SS

                //string temnormaltime = Convert.ToString(NormalTime);
                //string[] NormaltimeArr = new string[2];
                //int nt1 = 0;
                //int nt2 = 0;
                //if (temnormaltime.Contains(DecimalSeperator))
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt2 = int.Parse(NormaltimeArr[1]);
                //    nt1 = (nt1 * 60) + nt2;
                //}
                //else
                //{
                //    NormaltimeArr = temnormaltime.Split(Convert.ToChar(DecimalSeperator));
                //    nt1 = int.Parse(NormaltimeArr[0]);
                //    nt1 = (nt1 * 60);
                //}
                //UITempNormalTime = TimeSpan.FromSeconds(nt1);
                TimeSpan tempnormalTime= TimeSpan.FromMinutes(Convert.ToDouble(NormalTime));
                if (tempnormalTime.Milliseconds >= 600)
                {

                    tempnormalTime = tempnormalTime.Add(-TimeSpan.FromMilliseconds(tempnormalTime.Milliseconds));
                    TimeSpan Second = TimeSpan.FromMilliseconds(1000);
                    tempnormalTime = tempnormalTime.Add(Second);
                    UITempNormalTime = tempnormalTime;// TimeSpan.FromMinutes(Convert.ToDouble(temp.ObservedTime.Value));

                }
                else
                {
                    tempnormalTime = tempnormalTime.Add(-TimeSpan.FromMilliseconds(tempnormalTime.Milliseconds));

                    UITempNormalTime = tempnormalTime;

                }
               // UITempNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(NormalTime));


                #endregion
                Remarks = temp.Remarks == null ? "" : temp.Remarks.Trim(); //[GEOS2-3933][Rupali Sarode][19/09/2022]
                DetectedProblems = temp.DetectedProblems;
                ImprovementsProposals = temp.ImprovementsProposals;

                Description = temp.Description == null ? "" : temp.Description;
                Description_en = temp.Description == null ? "" : temp.Description;
                Description_es = temp.Description_es == null ? "" : temp.Description_es;
                Description_fr = temp.Description_fr == null ? "" : temp.Description_fr;
                Description_pt = temp.Description_pt == null ? "" : temp.Description_pt;
                Description_ro = temp.Description_ro == null ? "" : temp.Description_ro;
                Description_ru = temp.Description_ru == null ? "" : temp.Description_ru;
                Description_zh = temp.Description_zh == null ? "" : temp.Description_zh;
                //SelectedStatus = temp.Status;
                // StatusList.Where(s=>s.IdLookupKey.Equals(SelectedStatus.IdLookupKey));
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                   Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyNameAndDescription = true;
                }
                else
                {
                    IsCheckedCopyNameAndDescription = false;
                }
                if (temp.Type != null)
                {
                    SelectedType = TypeList.FirstOrDefault(x => x.IdLookupValue == temp.Type.IdLookupValue);
                }
                else
                {
                    SelectedType = TypeList.FirstOrDefault(x => x.IdLookupValue == 0);
                    ClonedWorkOperation.IdType = 0;
                }
                if (temp.Status != null)
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.Status.IdLookupValue);
                }
                else
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 1537);
                    ClonedWorkOperation.IdStatus = 1537;
                }
                Tempworkstagename = string.Empty;  //GEOS2-3880 change log gulab lakade
                if (temp.WorkStage != null)
                {
                    //SelectedStages = StagesList.FirstOrDefault(x => x.IdStage == temp.IdStage);
                    List<string> stringList = temp.WorkStage.Split(',').ToList();
                    foreach (var item in stringList)
                    {
                        Stages stage = StagesList.Where(s => s.IdStage == Convert.ToInt32(item)).FirstOrDefault();
                        if (stage != null)
                        {
                            if (SelectedStages == null)
                            {
                                SelectedStages = new List<object>();
                            }
                            SelectedStages.Add(stage);
                        }
                        if (SelectedStages == null)
                        {
                            if (SelectedStages == null)
                            {
                                SelectedStages = new List<object>();
                            }
                            Stages stage1 = StagesList.Where(s => s.IdStage == Convert.ToInt32(0)).FirstOrDefault();
                            SelectedStages.Add(stage1);
                        }
                        #region GEOS2-3880 change log gulab lakade
                        if (string.IsNullOrEmpty(Tempworkstagename))
                        {
                            Tempworkstagename = Convert.ToString(stage.CodeWithName);
                        }
                        else
                        {
                            Tempworkstagename = Tempworkstagename + ", " + Convert.ToString(stage.CodeWithName);
                        }
                        #endregion
                    }
                }
                else
                {
                    if (SelectedStages == null)
                    {
                        SelectedStages = new List<object>();
                    }
                    Stages stage = StagesList.Where(s => s.IdStage == Convert.ToInt32(0)).FirstOrDefault();
                    SelectedStages.Add(stage);
                    #region GEOS2-3880 change log gulab lakade
                    if (string.IsNullOrEmpty(Tempworkstagename))
                    {
                        Tempworkstagename = Convert.ToString(stage.CodeWithName);
                    }
                    else
                    {
                        Tempworkstagename = Tempworkstagename + ", " + Convert.ToString(stage.CodeWithName);
                    }
                    #endregion
                }
                if (temp.Parent != null)
                {
                    SelectedParent = ParentList.FirstOrDefault(x => x.IdWorkOperation == temp.IdParent);

                    if (SelectedParent == null)
                    {
                        SelectedParent = ParentList.FirstOrDefault(x => x.IdWorkOperation == 0);
                    }

                }
                if (SelectedParent.IdWorkOperation != 0)
                {
                    OrderList = new ObservableCollection<WorkOperation>(AllWorkOperationList.Where(a => a.IdParent == SelectedParent.IdWorkOperation).ToList());
                    OrderList.Insert(0, new WorkOperation() { Name = "---", KeyName = "default", IdWorkOperation = 0 });
                    // Save old OrderList
                    if (PreviousOrderList == null)
                        PreviousOrderList = new List<WorkOperation>();
                    PreviousOrderList = OrderList.ToList();
                }
                else
                {
                    //if (OrderList == null)
                    //{
                    //    OrderList = new ObservableCollection<WorkOperation>();
                    //    OrderList.Insert(0, new WorkOperation() { Name = "---", KeyName = "default", IdWorkOperation = 0 });
                    //}

                    #region [GEOS2-4933][Rupali Sarode][21-11-2023]
                    if (OrderList == null)
                    {
                        OrderList = new ObservableCollection<WorkOperation>();
                        //[001]Added if condition
                        if (ParentList != null)
                        {
                            OrderList = new ObservableCollection<WorkOperation>(ParentList.Where(a => a.IdParent == 0 || a.IdParent == null).ToList()); // Pallavi jadhav adding IdParent == null condition
                            // Save old OrderList
                            if (PreviousOrderList == null)
                                PreviousOrderList = new List<WorkOperation>();

                            PreviousOrderList = ParentList.Where(a => a.IdParent == 0).OrderBy(a => a.Position).ToList();

                            // OrderList.Insert(0, new WorkOperation() { Name = "---", KeyName = "default", IdWorkOperation = 0 });
                        }

                    }
                    #endregion [GEOS2-4933][Rupali Sarode][21-11-2023]

                }

                OrderList = new ObservableCollection<WorkOperation>(OrderList);
                //SelectedOrder.Position && (workOperation.IdWorkOperation == SelectedOrder.IdWorkOperation || workOperation.KeyName == UpdatedWorkOperation.KeyName));
                #region [GEOS2-3976][Rupali Sarode][12-11-2022]
                // SelectedOrder = OrderList.FirstOrDefault(x => x.IdParent == SelectedParent.IdWorkOperation);
                uint tmpPosition = 0;
                if (IdWorkOperation == Position) tmpPosition = Position;
                else tmpPosition = Position - 1;

                //var tmpPosition = Position - 1;

                SelectedOrder = OrderList.Where(a => a.Position == tmpPosition).FirstOrDefault();

                //SelectedOrder = OrderList.FirstOrDefault(x => x.IdParent == SelectedParent.IdWorkOperation);

                #endregion [GEOS2-3976][Rupali Sarode][12-11-2022]

                //string ordertemp = Convert.ToString(SelectedOrder.Name);
                IsNew = false;
                Parent = selectedParent.Name;
                #region GEOS2-3880 Get log list
                WorkOperationAllChangeLogList = new ObservableCollection<WorkOperationChangeLog>(temp.LstWorkOperationChangeLogList.OrderByDescending(x => x.IdLogEntryByWO).ToList());
                #endregion
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message + GeosApplication.createExceptionDetailsMsg(ex)), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][rdixit][GEOS2-3646][12.04.2022] 
        public void Init(WorkOperationByStages SelectedWorkOperationByStages = null)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][18/12/2023]
                AddLanguages();
                GetAllStages();
                // FillparentList();
                //FillorderList();
                FillStatusList();
                FillTypeList();
                IsCheckedCopyNameAndDescription = true;
                AllWorkOperationList = new ObservableCollection<WorkOperation>(ERMService.GetparentAndOrder_V2240());
                //[001] Added
                if (StagesList != null && SelectedWorkOperationByStages != null && StagesList.Count > 0 && SelectedWorkOperationByStages.IdStage != null && SelectedWorkOperationByStages.IdStage > 0)
                {

                    FillparentList(StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdStage).FirstOrDefault().Code, 0);
                    if (ParentList != null && ParentList.Count > 0 && SelectedWorkOperationByStages != null && SelectedWorkOperationByStages.IdworkOperationByStage != null)
                    {
                        if (ParentList.Any(i => i.IdWorkOperation == Convert.ToInt32(SelectedWorkOperationByStages.IdworkOperationByStage)))
                        {
                            SelectedStages = new List<object>();
                            SelectedStages.Add(StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdStage).FirstOrDefault());
                            SelectedParent = ParentList.Where(i => i.IdWorkOperation == Convert.ToInt32(SelectedWorkOperationByStages.IdworkOperationByStage)).FirstOrDefault();
                            if (SelectedStages.Count == 1)
                            {
                                //Service chaged from GetLatestWorkOperationCodeByCode to GetLatestWorkOperationCodeByCode_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                                string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2350((StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdStage).FirstOrDefault().Code));
                                Code = StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdStage).FirstOrDefault().Code + tempCode;
                            }
                            else // count is greater than 1
                            {
                                Code = null;
                            }
                        }
                    }

                }
                else if (SelectedWorkOperationByStages != null)
                {
                    if (SelectedWorkOperationByStages.IdworkOperationByStage != null && SelectedWorkOperationByStages.IdworkOperationByStage > 0)
                    {
                        FillparentList(StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdworkOperationByStage).FirstOrDefault().Code, 0);
                        if (SelectedWorkOperationByStages != null && SelectedWorkOperationByStages.IdworkOperationByStage != null)
                        {

                            SelectedStages = new List<object>();
                            SelectedStages.Add(StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdworkOperationByStage).FirstOrDefault());
                            if (SelectedStages.Count == 1)
                            {
                                //Service chaged from GetLatestWorkOperationCodeByCode to GetLatestWorkOperationCodeByCode_V2350 by [rdixit][10.01.2022][GEOS2-4121]
                                string tempCode = ERMService.GetLatestWorkOperationCodeByCode_V2350((StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdworkOperationByStage).FirstOrDefault().Code));
                                Code = StagesList.Where(i => i.IdStage == SelectedWorkOperationByStages.IdworkOperationByStage).FirstOrDefault().Code + tempCode;
                            }
                            else // count is greater than 1
                            {
                                Code = null;
                            }

                        }
                    }
                }
                Activity = 100;
                Distance = 0;
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddLanguages..."), category: Category.Info, priority: Priority.Low);

                Languages = new ObservableCollection<Language>(PCMService.GetAllLanguages());
                LanguageSelected = Languages.FirstOrDefault();

                GeosApplication.Instance.Logger.Log(string.Format("Method AddLanguages()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddLanguages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillStatusList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStatusList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempStatusList = PCMService.GetLookupValues(83);
                StatusList = new List<LookupValue>(tempStatusList);
                SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 1537);

                GeosApplication.Instance.Logger.Log("Method FillStatusList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillStatusList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStatusList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTypeList()...", category: Category.Info, priority: Priority.Low);

                List<LookupValue> tempTypeList = new List<LookupValue>(PCMService.GetLookupValues(93));
                tempTypeList.Insert(0, new LookupValue() { IdLookupValue = 0, Value = "---", InUse = true });
                TypeList = new List<LookupValue>(tempTypeList);
                SelectedType = TypeList.FirstOrDefault(x => x.IdLookupValue == 0);

                foreach (var item in tempTypeList)
                {
                    item.ImageData = geosService.GetLookupImages(item.ImageName); //"1-1.png"); //item.LookupValueImages
                }



                GeosApplication.Instance.Logger.Log("Method FillTypeList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTypeList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillTypeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillparentList(string code, Int32 idParent)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillparentList()...", category: Category.Info, priority: Priority.Low);
                //IList<WorkOperation> tempStatusList = ERMService.GetparentAndOrder();
                //parentList = new List<WorkOperation>();
                //parentList = new List<WorkOperation>(tempStatusList);
                //parentList = new ObservableCollection<WorkOperation>(ERMService.GetparentAndOrder());

                ParentList = new ObservableCollection<WorkOperation>(ERMService.GetParentListByIdParentAndCode_V2240(idParent, code));
                UpdateCountForParent();
                ParentList.Insert(0, new WorkOperation() { Name = "---", KeyName = "default", IdWorkOperation = 0 });
                ParentList = new ObservableCollection<WorkOperation>(parentList.OrderBy(x => x.Position));
                tempparentList = new ObservableCollection<WorkOperation>(parentList);
                SelectedParent = tempparentList.FirstOrDefault();


                GeosApplication.Instance.Logger.Log("Method FillparentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillparentList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillparentList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillparentList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UpdateCountForParent()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateCountForParent()...", category: Category.Info, priority: Priority.Low);


                foreach (WorkOperation item in parentList)
                {
                    int count = 0;
                    if (item.WorkOperation_count_original != null)
                    {
                        count = item.WorkOperation_count_original;
                    }
                    if (parentList.Any(a => a.IdParent == item.IdWorkOperation))
                    {
                        List<WorkOperation> getFirstParentList = parentList.Where(a => a.IdParent == item.IdWorkOperation).ToList();
                        foreach (WorkOperation item1 in getFirstParentList)
                        {
                            if (item1.WorkOperation_count_original != null)
                            {
                                count = count + item1.WorkOperation_count_original;
                            }
                            if (parentList.Any(a => a.IdParent == item1.IdWorkOperation))
                            {
                                List<WorkOperation> getSecondParentList = parentList.Where(a => a.IdParent == item1.IdWorkOperation).ToList();
                                foreach (WorkOperation item2 in getSecondParentList)
                                {
                                    if (item2.WorkOperation_count_original != null)
                                    {
                                        count = count + item2.WorkOperation_count_original;
                                    }
                                    if (parentList.Any(a => a.IdParent == item2.IdWorkOperation))
                                    {
                                        List<WorkOperation> getThirdParentList = parentList.Where(a => a.IdParent == item2.IdWorkOperation).ToList();
                                        foreach (WorkOperation item3 in getThirdParentList)
                                        {
                                            if (item3.WorkOperation_count_original != null)
                                            {
                                                count = count + item3.WorkOperation_count_original;
                                            }
                                            if (parentList.Any(a => a.IdParent == item3.IdWorkOperation))
                                            {
                                                List<WorkOperation> getForthParentList = parentList.Where(a => a.IdParent == item3.IdWorkOperation).ToList();
                                                foreach (WorkOperation item4 in getForthParentList)
                                                {
                                                    if (item4.WorkOperation_count_original != null)
                                                    {
                                                        count = count + item4.WorkOperation_count_original;
                                                    }
                                                    if (parentList.Any(a => a.IdParent == item4.IdWorkOperation))
                                                    {
                                                        List<WorkOperation> getFifthParentList = parentList.Where(a => a.IdParent == item4.IdWorkOperation).ToList();
                                                        foreach (WorkOperation item5 in getFifthParentList)
                                                        {
                                                            if (item5.WorkOperation_count_original != null)
                                                            {
                                                                count = count + item5.WorkOperation_count_original;
                                                            }
                                                            if (parentList.Any(a => a.IdParent == item5.IdWorkOperation))
                                                            {
                                                                List<WorkOperation> getSixthParentList = parentList.Where(a => a.IdParent == item5.IdWorkOperation).ToList();
                                                                foreach (WorkOperation item6 in getSixthParentList)
                                                                {
                                                                    if (item6.WorkOperation_count_original != null)
                                                                    {
                                                                        count = count + item6.WorkOperation_count_original;
                                                                    }
                                                                    if (parentList.Any(a => a.IdParent == item6.IdWorkOperation))
                                                                    {
                                                                        List<WorkOperation> getSeventhParentList = parentList.Where(a => a.IdParent == item6.IdWorkOperation).ToList();
                                                                        foreach (WorkOperation item7 in getSeventhParentList)
                                                                        {
                                                                            if (item7.WorkOperation_count_original != null)
                                                                            {
                                                                                count = count + item7.WorkOperation_count_original;
                                                                            }
                                                                            if (parentList.Any(a => a.IdParent == item7.IdWorkOperation))
                                                                            {
                                                                                List<WorkOperation> getEightthParentList = parentList.Where(a => a.IdParent == item7.IdWorkOperation).ToList();
                                                                                foreach (WorkOperation item8 in getEightthParentList)
                                                                                {
                                                                                    if (item8.WorkOperation_count_original != null)
                                                                                    {
                                                                                        count = count + item8.WorkOperation_count_original;
                                                                                    }
                                                                                    if (parentList.Any(a => a.IdParent == item8.IdWorkOperation))
                                                                                    {
                                                                                        List<WorkOperation> getNinethParentList = parentList.Where(a => a.IdParent == item8.IdWorkOperation).ToList();
                                                                                        foreach (WorkOperation item9 in getNinethParentList)
                                                                                        {
                                                                                            if (item9.WorkOperation_count_original != null)
                                                                                            {
                                                                                                count = count + item9.WorkOperation_count_original;
                                                                                            }
                                                                                            if (parentList.Any(a => a.IdParent == item9.IdWorkOperation))
                                                                                            {
                                                                                                List<WorkOperation> gettenthParentList = parentList.Where(a => a.IdParent == item9.IdWorkOperation).ToList();
                                                                                                foreach (WorkOperation item10 in gettenthParentList)
                                                                                                {
                                                                                                    if (item10.WorkOperation_count_original != null)
                                                                                                    {
                                                                                                        count = count + item10.WorkOperation_count_original;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    item.WorkOperation_count = count;
                    // item.NameWithCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.WorkOperation_count) + "]");
                }
                GeosApplication.Instance.Logger.Log("Method UpdateCountForParent()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method UpdateCountForParent() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        private void FillorderList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillorderList()...", category: Category.Info, priority: Priority.Low);
                //orderList = new ObservableCollection<WorkOperation>(ERMService.GetparentAndOrder());
                //orderList.Insert(0, new WorkOperation() { Name = "---", KeyName = "defaultCategory", IdWorkOperation = 0 });
                //orderList = new ObservableCollection<WorkOperation>(orderList.OrderBy(x => x.Position));
                //temporderList = new ObservableCollection<WorkOperation>(orderList);
                //SelectedOrder = temporderList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillorderList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillorderList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillorderList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillorderList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetAllStages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetAllStages()...", category: Category.Info, priority: Priority.Low);
                IList<Stages> tempStagesList = ERMService.GetAllStages();
                StagesList = new ObservableCollection<Stages>(tempStagesList);
                StagesList.Insert(0, new Stages() { Code = "---", Name = "default", IdStage = 0 });
                SelectedStages = new List<object>();
                SelectedStages.Add(StagesList[0]);
                GeosApplication.Instance.Logger.Log("Method GetAllStages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllStages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllStages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetAllStages() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UncheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected.TwoLetterISOLanguage == "EN")
                {
                    Description = Description_en;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                {
                    Description = Description_es;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                {
                    Description = Description_fr;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                {
                    Description = Description_pt;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                {
                    Description = Description_ro;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                {
                    Description = Description_ru;
                }
                else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                {
                    Description = Description_zh;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method UncheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method UncheckedCopyNameDescription() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CheckedCopyNameDescription(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CheckedCopyNameDescription()..."), category: Category.Info, priority: Priority.Low);

                if (LanguageSelected != null)
                {
                    if (string.IsNullOrEmpty(Description))
                        Description = string.Empty;
                    if (string.IsNullOrEmpty(Name))
                        Name = string.Empty;

                    Description_en = Description;
                    Description_es = Description;
                    Description_fr = Description;
                    Description_pt = Description;
                    Description_ro = Description;
                    Description_ru = Description;
                    Description_zh = Description;

                    Name_en = Name;
                    Name_es = Name;
                    Name_fr = Name;
                    Name_pt = Name;
                    Name_ro = Name;
                    Name_ru = Name;
                    Name_zh = Name;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method CheckedCopyNameDescription()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CheckedCopyNameDescription() - {0}", ex.Message + GeosApplication.createExceptionDetailsMsg(ex)), category: Category.Exception, priority: Priority.Low);

            }
        }
        //[001][cpatil][GEOS2-3646][28-04-2022]
        private void SetDescriptionToLanguage(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()..."), category: Category.Info, priority: Priority.Low);
                //[001] Removed else part
                if (IsCheckedCopyNameAndDescription == false && LanguageSelected != null)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description_en = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description_es = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description_fr = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description_pt = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description_ro = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description_ru = Description == null ? "" : Description;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description_zh = Description == null ? "" : Description;
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method SetDescriptionToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[001][cpatil][GEOS2-3646][28-04-2022]
        private void SetNameToLanguage(EditValueChangingEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()..."), category: Category.Info, priority: Priority.Low);
                //[001] Removed else part
                if (IsCheckedCopyNameAndDescription == false && LanguageSelected != null)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Name_en = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Name_es = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Name_fr = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Name_pt = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Name_ro = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Name_ru = Name;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Name_zh = Name;
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method SetNameToLanguage()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetDescriptionToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RetrieveNameDescriptionByLanguge(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method RetrieveNameDescriptionByLanguge()..."), category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyNameAndDescription == false)
                {
                    if (LanguageSelected.TwoLetterISOLanguage == "EN")
                    {
                        Description = Description_en;
                        Name = Name_en;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ES")
                    {
                        Description = Description_es;
                        Name = Name_es;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "FR")
                    {
                        Description = Description_fr;
                        Name = Name_fr;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "PT")
                    {
                        Description = Description_pt;
                        Name = Name_pt;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RO")
                    {
                        Description = Description_ro;
                        Name = Name_ro;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "RU")
                    {
                        Description = Description_ru;
                        Name = Name_ru;
                    }
                    else if (LanguageSelected.TwoLetterISOLanguage == "ZH")
                    {
                        Description = Description_zh;
                        Name = Name_zh;
                    }
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method RetrieveNameDescriptionByLanguge()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveNameDescriptionByLanguge() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        //[001][cpatil][19/05/2022][GEOS2-3727]
        private void CommandEditValueChangedAction(object obj)
        {
            if (SelectedParent.IdWorkOperation != 0)
            {
                OrderList = new ObservableCollection<WorkOperation>(AllWorkOperationList.Where(a => a.IdParent == SelectedParent.IdWorkOperation).ToList());
                OrderList.Insert(0, new WorkOperation() { Name = "---", KeyName = "default", IdWorkOperation = 0 });
            }
            else
            {
                if (OrderList == null)
                {
                    OrderList = new ObservableCollection<WorkOperation>();
                    //[001]Added if condition
                    if (ParentList != null)
                        OrderList = new ObservableCollection<WorkOperation>(ParentList.Where(a => a.IdParent == 0 || a.IdParent==null).ToList()); // Pallavi jadhav adding IdParent == null condition

                 //   OrderList.Insert(0, new WorkOperation() { Name = "---", KeyName = "default", IdWorkOperation = 0 });
                }
            }

            if (IdWorkOperation > 0 && ClonedWorkOperation.IdParent == SelectedParent.IdWorkOperation)
            {
                #region [GEOS2-3976][Rupali Sarode][12-11-2022]
                //SelectedOrder = OrderList.FirstOrDefault(a => a.IdWorkOperation == IdWorkOperation);
                uint tmpPosition = 0;
                if (IdWorkOperation == Position) tmpPosition = Position;
                else tmpPosition = Position - 1;
                //var tPosition = Position - 1;

                SelectedOrder = OrderList.FirstOrDefault(a => a.Position == tmpPosition);

                #endregion [GEOS2-3976][Rupali Sarode][12-11-2022]

                if (SelectedOrder == null && OrderList != null)
                {
                    SelectedOrder = OrderList.FirstOrDefault();
                }
            }
            else
            {
                SelectedOrder = OrderList.FirstOrDefault();
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                #region [GEOS2-4933][Rupali Sarode][21-11-2023]
                if (SelectedParent != null)
                {
                    if (SelectedParent.IdWorkOperation == 0 && SelectedParent.Name == "---")
                        IsFatherFlag = true;
                    else
                        IsFatherFlag = false;
                }
                else
                {
                    IsFatherFlag = false;
                }

                #endregion [GEOS2-4933][Rupali Sarode][21-11-2023]

                    InformationError = null;
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedStages"));
                PropertyChanged(this, new PropertyChangedEventArgs("Code"));
                //PropertyChanged(this, new PropertyChangedEventArgs("ObservedTime"));
                #region GEOS2-3954 Time format HH:MM:SS
                PropertyChanged(this, new PropertyChangedEventArgs("UITempobservedTime"));
                #endregion
                PropertyChanged(this, new PropertyChangedEventArgs("Activity"));

                //PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = " ";
                if (error != null)
                {
                    return;
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()..."), category: Category.Info, priority: Priority.Low);
                UpdatedWorkOperation = new WorkOperation();

                #region GEOS2-3954 Time format HH:MM:SS
                //int TempOTDay = UITempobservedTime.Days;
                //int TempOTHours = UITempobservedTime.Hours;
                //int TempOTminute = UITempobservedTime.Minutes;
                //int TempOTSecond = UITempobservedTime.Seconds;
                //string tempstring = Convert.ToString(((TempOTDay * 24) + TempOTHours) * 60 + TempOTminute) + Convert.ToChar(DecimalSeperator) + TempOTSecond;
                //float tempfloat = float.Parse(tempstring);

                //ObservedTime = tempfloat;

                ObservedTime = (float?)Math.Round(Convert.ToDouble(UITempobservedTime.TotalMinutes), 2);

                //int TempNTDay = UITempNormalTime.Days;
                //int TempNTHours = UITempNormalTime.Hours;
                //int TempNTminute = UITempNormalTime.Minutes;
                //int TempNTSecond = UITempNormalTime.Seconds;
                //string tempNTstring = Convert.ToString(((TempNTDay * 24) + TempNTHours) * 60 + TempNTminute) + Convert.ToChar(DecimalSeperator) + TempNTSecond;
                //float tempNTfloat = float.Parse(tempNTstring);
                //NormalTime = tempNTfloat;
                #endregion

                if (IsNew)
                {
                    NewWorkOperation = new WorkOperation();

                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        NewWorkOperation.Name = Name;
                        NewWorkOperation.Name_es = Name;
                        NewWorkOperation.Name_fr = Name;
                        NewWorkOperation.Name_pt = Name;
                        NewWorkOperation.Name_ro = Name;
                        NewWorkOperation.Name_ru = Name;
                        NewWorkOperation.Name_zh = Name;

                        NewWorkOperation.Description = Description;
                        NewWorkOperation.Description_es = Description;
                        NewWorkOperation.Description_fr = Description;
                        NewWorkOperation.Description_pt = Description;
                        NewWorkOperation.Description_ro = Description;
                        NewWorkOperation.Description_ru = Description;
                        NewWorkOperation.Description_zh = Description;
                        //NewWorkOperation.NameWithCount = Name + " [0]";
                    }
                    else
                    {
                        NewWorkOperation.Name = Name_en;
                        NewWorkOperation.Name_es = Name_es;
                        NewWorkOperation.Name_fr = Name_fr;
                        NewWorkOperation.Name_pt = Name_pt;
                        NewWorkOperation.Name_ro = Name_ro;
                        NewWorkOperation.Name_ru = Name_ru;
                        NewWorkOperation.Name_zh = Name_zh;

                        NewWorkOperation.Description = Description_en;
                        NewWorkOperation.Description_es = Description_es;
                        NewWorkOperation.Description_fr = Description_fr;
                        NewWorkOperation.Description_pt = Description_pt;
                        NewWorkOperation.Description_ro = Description_ro;
                        NewWorkOperation.Description_ru = Description_ru;
                        NewWorkOperation.Description_zh = Description_zh;
                        // NewWorkOperation.NameWithCount = Name_en + " [0]";
                    }

                    if (SelectedOrder.IdWorkOperation > 0)
                    {
                        NewWorkOperation.Position = (SelectedOrder.Position == 0 ? 0 : SelectedOrder.Position);
                        NewWorkOperation.IdParent = SelectedOrder.IdParent;
                        //NewWorkOperation.IsLeaf = SelectedOrder.IsLeaf;
                        NewWorkOperation.Parent = SelectedParent.Name; //SelectedOrder.Name; // ParentName;
                    }
                    else
                    {
                        if (orderList.Count > 1)
                        {
                            if (SelectedOrder.Position == 0)
                            {
                                NewWorkOperation.Position = 0;
                            }
                            else
                            {
                                NewWorkOperation.Position = orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().Position == 0 ? 0 : orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().Position; ;
                            }

                            // [GEOS2-4933][Rupali Sarode][28-11-2023]
                            // NewWorkOperation.IdParent = orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().IdParent == 0 ? null : orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().IdParent;
                            NewWorkOperation.IdParent = orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().IdParent == 0 ? 0 : orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().IdParent;
                            NewWorkOperation.Parent = SelectedParent.Name; //orderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().Name;
                        }
                        else
                        {
                            if (orderList.Count == 1)
                            {
                                NewWorkOperation.Position = (SelectedOrder.Position == 0 ? 0 : SelectedOrder.Position);
                                NewWorkOperation.IdParent = selectedParent.IdWorkOperation;
                                NewWorkOperation.Parent = selectedParent.Name;
                            }
                        }
                    }

                    NewWorkOperation.KeyName = "Group_0";
                    //NewWorkOperation. Article_count = 0;
                    NewWorkOperation.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                    orderList.Add(NewWorkOperation);
                    List<WorkOperation> workOperation_ForSetOrder = orderList.Where(a => a.IdParent == NewWorkOperation.IdParent && a.Name != "---").OrderBy(a => a.Position).ToList();
                    uint pos = 1;
                    uint Old_Position_set = 0;

                    if (NewWorkOperation.IdParent == null)
                    { // IdParent is 0 but never null
                        foreach (WorkOperation workOperation in workOperation_ForSetOrder)
                        {
                            if (workOperation.Position == SelectedOrder.Position && workOperation.IdWorkOperation == SelectedOrder.IdWorkOperation && workOperation.KeyName != NewWorkOperation.KeyName)
                            {
                                pos++;
                                Old_Position_set = pos;
                                orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.KeyName == workOperation.KeyName).ToList().ForEach(a => { a.Position = pos; });
                            }
                            else
                            {
                                if (workOperation.KeyName == NewWorkOperation.KeyName)
                                {
                                    orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.KeyName == workOperation.KeyName).ToList().ForEach(a => { a.Position = Old_Position_set - 1; });
                                    pos++;
                                }
                                else
                                {
                                    orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.KeyName == workOperation.KeyName).ToList().ForEach(a => { a.Position = pos++; });
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (WorkOperation workOperation in workOperation_ForSetOrder)
                        {
                            if (workOperation.Position == SelectedOrder.Position && workOperation.IdWorkOperation == SelectedOrder.IdWorkOperation && workOperation.KeyName != NewWorkOperation.KeyName)
                            {
                                //pos++;
                                //orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.IdParent == workOperation.IdParent).ToList().ForEach(a => { a.Position = pos; });

                                orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.IdParent == workOperation.IdParent).ToList().ForEach(a => { a.Position = pos++; });
                            }
                            else
                            {
                                if (workOperation.KeyName == NewWorkOperation.KeyName)
                                {
                                    //pos--;
                                    //orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.IdParent == workOperation.IdParent).ToList().ForEach(a => { a.Position = pos++; });
                                    //pos++;
                                    orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.IdParent == workOperation.IdParent).ToList().ForEach(a => { a.Position = pos++; });
                                }
                                else
                                {
                                    orderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.IdParent == workOperation.IdParent).ToList().ForEach(a => { a.Position = pos++; });
                                }
                            }
                        }
                    }

                    List<WorkOperation> workOperation_ForSetOrder_new = orderList.Where(a => a.IdParent == NewWorkOperation.IdParent && a.IdWorkOperation > 0).OrderBy(a => a.Position).ToList();
                    NewWorkOperation.Code = Code;
                    NewWorkOperation.Type = SelectedType;
                    NewWorkOperation.Status = SelectedStatus;
                    NewWorkOperation.IdType = Convert.ToUInt32(SelectedType.IdLookupValue);
                    NewWorkOperation.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);
                    NewWorkOperation.IdOrder = Convert.ToUInt32(SelectedOrder.IdWorkOperation);
                    NewWorkOperation.Stages = new List<Stages>();

                    List<Stages> tempSelectedStageList = new List<Stages>();
                    tempSelectedStageList = SelectedStages.Cast<Stages>().ToList();

                    if (tempSelectedStageList.Any(x => x.IdStage == 0))
                    {
                        tempSelectedStageList.RemoveAll(y => y.IdStage == 0);
                        SelectedStages = new List<object>();
                        foreach (var temp in tempSelectedStageList)
                        {
                            SelectedStages.Add(temp);
                        }
                        Code = NewWorkOperation.Code;
                    }

                    foreach (Stages temp in SelectedStages)
                    {
                        NewWorkOperation.Stages.Add(temp);
                    }
                    //[001][kshinde][09/06/2022][GEOS2-3709]
                    NewWorkOperation.Distance = Distance;
                    NewWorkOperation.ObservedTime = ObservedTime;
                    NewWorkOperation.Activity = Activity;
                    NewWorkOperation.NormalTime = NormalTime;
                    NewWorkOperation.Remarks = Remarks == null ? "" : Remarks.Trim(); //[GEOS2-3933][Rupali Sarode][19/09/2022]
                    NewWorkOperation.DetectedProblems = DetectedProblems;
                    NewWorkOperation.ImprovementsProposals = ImprovementsProposals;
                    // NewWorkOperation = ERMService.AddWorkOperation_V2240(NewWorkOperation, workOperation_ForSetOrder_new);
                    //NewWorkOperation = ERMService.AddWorkOperation_V2280(NewWorkOperation, workOperation_ForSetOrder_new);//[001][kshinde][09/06/2022][GEOS2-3709]
                    //[GEOS2-3933][Rupali Sarode][19/09/2022]
                    #region GEOS2-3880 Add Log gulab lakade
                    AddChangedWorkOperationLogDetails("New");
                    NewWorkOperation.LstWorkOperationChangeLogList = WorkOperationChangeLogList.ToList();
                    #endregion
                    NewWorkOperation = ERMService.AddWorkOperation_V2320(NewWorkOperation, workOperation_ForSetOrder_new);

                    IsSave = true;
                    if (IsSave)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationsAddSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        //var ownerInfo = (obj as FrameworkElement);
                        //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationsAddSuccessMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(),
                        //CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                    }
                    RequestClose(null, null);


                }
                else
                {
                    if (Code == null)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("Code"));
                        if (error != null)
                        {
                            return;
                        }
                    }
                    if (Name == null)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                        if (error != null)
                        {
                            return;
                        }
                    }
                    if (SelectedStages == null)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("SelectedStages"));
                        if (error != null)
                        {
                            return;
                        }
                    }

                    if (ObservedTime == 0 || ObservedTime.ToString() == null)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("ObservedTime"));
                        if (error != null)
                        {
                            return;
                        }
                    }
                    if (Activity == 100 || Activity.ToString() == null)
                    {
                        error = EnableValidationAndGetError();
                        PropertyChanged(this, new PropertyChangedEventArgs("Activity"));
                        if (error != null)
                        {
                            return;
                        }
                    }

                    UpdatedWorkOperation.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    UpdatedWorkOperation.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    UpdatedWorkOperation.IdWorkOperation = IdWorkOperation;
                    UpdatedWorkOperation.Type = SelectedType;
                    UpdatedWorkOperation.IdType = Convert.ToUInt32(SelectedType.IdLookupValue);
                    UpdatedWorkOperation.Status = SelectedStatus;
                    UpdatedWorkOperation.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);
                    UpdatedWorkOperation.IdOrder = Convert.ToUInt32(SelectedOrder.IdWorkOperation);
                    UpdatedWorkOperation.Distance = Distance;//[001][kshinde][09/06/2022][GEOS2-3709]
                    UpdatedWorkOperation.ObservedTime = ObservedTime;
                    UpdatedWorkOperation.Activity = Activity;
                    UpdatedWorkOperation.NormalTime = NormalTime;


                    //[GEOS2-3933][Rupali Sarode][19/09/2022]

                    UpdatedWorkOperation.Remarks = Remarks == null ? "" : Remarks.Trim();


                    if (DetectedProblems != null)
                    {
                        UpdatedWorkOperation.DetectedProblems = DetectedProblems == null ? "" : DetectedProblems.Trim();
                    }
                    if (ImprovementsProposals != null)
                    {
                        UpdatedWorkOperation.ImprovementsProposals = ImprovementsProposals == null ? "" : ImprovementsProposals.Trim();
                    }
                    //UpdatedWorkOperation.IdStage = Convert.ToUInt32(SelectedStages.IdStage);
                    string CloneWorkStage = ClonedWorkOperation.WorkStage;
                    List<string> stringList = null;
                    if (!string.IsNullOrEmpty(CloneWorkStage))
                    {
                        stringList = CloneWorkStage.Split(',').ToList();
                    }
                    if (SelectedStages != null && SelectedStages.Count > 0)
                    {
                        if (UpdatedWorkOperation.Stages == null)
                        {
                            UpdatedWorkOperation.Stages = new List<Stages>();
                        }
                        foreach (Stages item in SelectedStages)
                        {
                            Stages stage = new Stages();
                            stage.Code = item.Code;
                            stage.IdStage = item.IdStage;
                            if (!stringList.Any(a => a.Equals(Convert.ToString(item.IdStage))))
                            {
                                stage.TransactionOperation = ModelBase.TransactionOperations.Add;
                                UpdatedWorkOperation.Stages.Add(stage);
                            }
                            if (stringList != null)
                            {
                                stringList.RemoveAll(d => d.Equals(item.IdStage.ToString()));

                            }
                        }

                    }
                    if (stringList != null)
                    {
                        foreach (var olditem in stringList)
                        {
                            Stages stage = new Stages();
                            if (UpdatedWorkOperation.Stages == null)
                            {
                                UpdatedWorkOperation.Stages = new List<Stages>();
                            }
                            stage.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            //stage.Code = item.Code;
                            stage.IdStage = Convert.ToInt32(olditem);// item.IdStage;
                            UpdatedWorkOperation.Stages.Add(stage);
                        }
                    }
                    UpdatedWorkOperation.Stages.RemoveAll(d => d.IdStage == 0);
                    UpdatedWorkOperation.IdParent = Convert.ToUInt32(selectedParent.IdWorkOperation);
                    if (Code != null)
                    {
                        UpdatedWorkOperation.Code = Code == null ? "" : Code.Trim();
                    }
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        IsFromInformation = true;
                        UpdatedWorkOperation.Description = Description == null ? "" : Description.Trim();
                        UpdatedWorkOperation.Description_es = Description == null ? "" : Description.Trim();
                        UpdatedWorkOperation.Description_fr = Description == null ? "" : Description.Trim();
                        UpdatedWorkOperation.Description_pt = Description == null ? "" : Description.Trim();
                        UpdatedWorkOperation.Description_ro = Description == null ? "" : Description.Trim();
                        UpdatedWorkOperation.Description_ru = Description == null ? "" : Description.Trim();
                        UpdatedWorkOperation.Description_zh = Description == null ? "" : Description.Trim();

                        UpdatedWorkOperation.Name = Name == null ? "" : Name.Trim();
                        UpdatedWorkOperation.Name_es = Name == null ? "" : Name.Trim();
                        UpdatedWorkOperation.Name_fr = Name == null ? "" : Name.Trim();
                        UpdatedWorkOperation.Name_pt = Name == null ? "" : Name.Trim();
                        UpdatedWorkOperation.Name_ro = Name == null ? "" : Name.Trim();
                        UpdatedWorkOperation.Name_ru = Name == null ? "" : Name.Trim();
                        UpdatedWorkOperation.Name_zh = Name == null ? "" : Name.Trim();
                    }
                    else
                    {
                        IsFromInformation = true;
                        UpdatedWorkOperation.Description = Description_en == null ? "" : Description_en.Trim();
                        UpdatedWorkOperation.Description_es = Description_es == null ? "" : Description_es.Trim();
                        UpdatedWorkOperation.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                        UpdatedWorkOperation.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                        UpdatedWorkOperation.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                        UpdatedWorkOperation.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                        UpdatedWorkOperation.Description_zh = Description_zh == null ? "" : Description_zh.Trim();

                        UpdatedWorkOperation.Name = Name_en == null ? "" : Name_en.Trim();
                        UpdatedWorkOperation.Name_es = Name_es == null ? "" : Name_es.Trim();
                        UpdatedWorkOperation.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                        UpdatedWorkOperation.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                        UpdatedWorkOperation.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                        UpdatedWorkOperation.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                        UpdatedWorkOperation.Name_zh = Name_zh == null ? "" : Name_zh.Trim();

                    }


                    if (SelectedOrder.IdWorkOperation > 0)
                    {
                        if (UpdatedWorkOperation.IdWorkOperation == SelectedOrder.IdWorkOperation)
                        {
                            //Same operation is selected. No need to change.

                            UpdatedWorkOperation.IdParent = SelectedOrder.IdParent == 0 ? null : SelectedOrder.IdParent; //[GEOS2-4933][Rupali Sarode][21-11-2023]  
                            UpdatedWorkOperation.Parent = SelectedOrder.Parent; //[GEOS2-4933][Rupali Sarode][21-11-2023]
                        }
                        else if ((ClonedWorkOperation.Parent == null && SelectedOrder.Parent == null) || (ClonedWorkOperation.Parent != null && SelectedOrder.Parent != null)
                            || SelectedOrder.Parent != null)
                        {
                            // Check existing position of the operation
                            var operation = OrderList.FirstOrDefault(x => x.IdWorkOperation == UpdatedWorkOperation.IdWorkOperation);
                            // bool existingPositionIsLessThanCurrentPosition = false;
                            UInt32 increasePositionByNumber = 1;
                            if (operation != null)
                            {
                                #region [GEOS2-3976][Rupali Sarode] [12-11-2022]
                                //if (operation.Position < SelectedOrder.Position)
                                //{
                                //    // existingPositionIsLessThanCurrentPosition
                                //    increasePositionByNumber = 2;
                                //}
                                #endregion [GEOS2-3976][Rupali Sarode] [12-11-2022]

                            }


                            #region [GEOS2-4933][Rupali Sarode] [08-11-2023] 
                            //UpdatedWorkOperation.Position = SelectedOrder.Position + increasePositionByNumber;
                            //UpdatedWorkOperation.IdOrder = SelectedOrder.Position + increasePositionByNumber;
                            if (ClonedWorkOperation.Parent == null && SelectedOrder.Parent != null)
                            {
                                UpdatedWorkOperation.Position = SelectedOrder.Position + 1;
                                UpdatedWorkOperation.IdOrder = SelectedOrder.Position + 1;
                            }
                            else
                            {
                                if (ClonedWorkOperation.IdParent == SelectedParent.IdWorkOperation)
                                {
                                    if (OldPosition < SelectedOrder.Position)
                                    {
                                        UpdatedWorkOperation.Position = SelectedOrder.Position;
                                        UpdatedWorkOperation.IdOrder = SelectedOrder.Position;
                                    }
                                    else if (OldPosition > SelectedOrder.Position)
                                    {
                                        UpdatedWorkOperation.Position = SelectedOrder.Position + 1;
                                        UpdatedWorkOperation.IdOrder = SelectedOrder.Position + 1;
                                    }
                                }
                                else if (ClonedWorkOperation.IdParent != SelectedParent.IdWorkOperation)
                                {
                                    UpdatedWorkOperation.Position = SelectedOrder.Position + 1;
                                    UpdatedWorkOperation.IdOrder = SelectedOrder.Position + 1;
                                }

                            }
                                #endregion [GEOS2-4933][Rupali Sarode] [08-11-2023] 
                                UpdatedWorkOperation.IdParent = SelectedOrder.IdParent == 0 ? null : SelectedOrder.IdParent;
                            UpdatedWorkOperation.Parent = SelectedOrder.Parent;
                        }
                        else
                        {
                            UpdatedWorkOperation.Position = Position;
                            UpdatedWorkOperation.IdOrder = Position;
                            UpdatedWorkOperation.IdParent = IdParent == 0 ? null : IdParent;
                            UpdatedWorkOperation.Parent = Parent;
                        }
                    }
                    else
                    {
                        if (OrderList.Count > 1 && SelectedOrder.Parent != null)
                        {
                            UpdatedWorkOperation.Position = OrderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().Position - 1;
                            UpdatedWorkOperation.IdParent = OrderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().IdParent == 0 ? null : OrderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().IdParent;
                            UpdatedWorkOperation.Parent = OrderList.Where(x => x.IdWorkOperation != 0).FirstOrDefault().Parent;
                        }
                        else
                        {
                            if (OrderList.Count >= 1)
                            {
                                UpdatedWorkOperation.Position = 1;
                                UpdatedWorkOperation.IdOrder = 1;
                                UpdatedWorkOperation.IdParent = SelectedParent.IdWorkOperation;
                                UpdatedWorkOperation.Parent = SelectedParent.Parent;
                            }
                        }
                    }

                   

                    UpdatedWorkOperation.KeyName = KeyName;
                    UpdatedWorkOperation.NameWithCount = Name_en + " [" + WorkOperation_count + "]";

                    UpdatedWorkOperation.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    // orderList.Add(UpdatedWorkOperation); // [GEOS2-3976][Rupali Sarode][15-11-2022]
                    List<WorkOperation> workOperation_ForSetOrder = orderList.Where(a => a.IdParent == UpdatedWorkOperation.IdParent && a.Name != "---").OrderBy(a => a.Position).ToList();
                    //uint pos = 1;
                    uint Old_Position_set = 0;
                    List<WorkOperation> workOperation_ForSetOrder_new = new List<WorkOperation>();
                    if (SelectedOrder.IdWorkOperation != ClonedWorkOperation.IdWorkOperation)
                    {
                        List<WorkOperation> work_Operation_ForSetOrder = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent).OrderBy(a => a.Position).ToList();
                        uint pos = 1;
                        uint status = 0;
                        if (ClonedWorkOperation.Parent == null && SelectedOrder.Parent == null)
                        {
                            foreach (WorkOperation pcmArticleCategory in work_Operation_ForSetOrder)
                            {
                                if (status == 0 && pcmArticleCategory.Position == SelectedOrder.Position && (pcmArticleCategory.IdWorkOperation == SelectedOrder.IdWorkOperation || pcmArticleCategory.KeyName == UpdatedWorkOperation.KeyName))
                                {
                                    status = 1;
                                    UpdatedWorkOperation.Position = pos;
                                    OrderList.Where(a => a.IdWorkOperation == UpdatedWorkOperation.IdWorkOperation).ToList().ForEach(a => { a.Position = pos++; });
                                    OrderList.Where(a => a.IdWorkOperation == pcmArticleCategory.IdWorkOperation && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = pos++; });
                                }
                                else
                                {
                                    if (pcmArticleCategory.KeyName != UpdatedWorkOperation.KeyName)
                                    {
                                        OrderList.Where(a => a.IdWorkOperation == pcmArticleCategory.IdWorkOperation && a.KeyName == pcmArticleCategory.KeyName).ToList().ForEach(a => { a.Position = pos++; });
                                    }
                                }
                            }
                            workOperation_ForSetOrder_new = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent && a.IdWorkOperation != UpdatedWorkOperation.IdWorkOperation).OrderBy(a => a.Position).ToList();
                        }
                        else if (ClonedWorkOperation.Parent != null && SelectedOrder.Parent != null) 
                        {
                            if (ClonedWorkOperation.IdParent == SelectedParent.IdWorkOperation) // If parent is same
                            {
                                if (UpdatedWorkOperation.Position != OldPosition)
                                {
                                    foreach (WorkOperation workOperation in work_Operation_ForSetOrder)
                                    {
                                        //if (status == 0 && workOperation.Position == SelectedOrder.Position && (workOperation.IdWorkOperation == SelectedOrder.IdWorkOperation || workOperation.KeyName == UpdatedWorkOperation.KeyName))
                                        //{
                                        //    status = 1;
                                        //    UpdatedWorkOperation.Position = pos;
                                        //    OrderList.Where(a => a.IdWorkOperation == UpdatedWorkOperation.IdWorkOperation).ToList().ForEach(a => { a.Position = pos++; });
                                        //    #region [GEOS2-3976][Rupali Sarode] [14-11-2022]
                                        //    //Commented to display correct order
                                        //    //    OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Parent == workOperation.Parent).ToList().ForEach(a => { a.Position = pos++; });
                                        //    #endregion
                                        //}
                                        //else
                                        //{
                                        if (workOperation.KeyName != UpdatedWorkOperation.KeyName) // [GEOS2-4933][Rupali Sarode][22-11-2023]
                                        {
                                            if (OldPosition < UpdatedWorkOperation.Position && workOperation.Position >= OldPosition && workOperation.Position <= UpdatedWorkOperation.Position)
                                            {
                                                OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= OldPosition && a.Position <= UpdatedWorkOperation.Position).ToList().ForEach(a => { a.Position = a.Position - 1; });
                                            }

                                            if (OldPosition > UpdatedWorkOperation.Position && workOperation.Position >= UpdatedWorkOperation.Position && workOperation.Position <= OldPosition)
                                            {
                                                OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= UpdatedWorkOperation.Position && a.Position <= OldPosition).ToList().ForEach(a => { a.Position = a.Position + 1; });
                                            }
                                        }
                                        //

                                        #region [GEOS2-3976][Rupali Sarode] [14-11-2022]
                                        //Commented to display correct order
                                        //if (workOperation.KeyName != UpdatedWorkOperation.KeyName)
                                        //{
                                        //    OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Parent == workOperation.Parent).ToList().ForEach(a => { a.Position = pos++; });
                                        //}
                                        #endregion
                                        //}
                                    }

                                    workOperation_ForSetOrder_new = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent && a.IdWorkOperation != UpdatedWorkOperation.IdWorkOperation).OrderBy(a => a.Position).ToList();
                                }
                            }
                            else if (ClonedWorkOperation.IdParent != SelectedParent.IdWorkOperation) // If parent is changed [GEOS2-4933][Rupali Sarode][22-11-2023]
                            {
                                uint tPosition = 0;
                                tPosition = UpdatedWorkOperation.Position + 1;

                                foreach (WorkOperation workOperation in work_Operation_ForSetOrder)
                                {
                                    if (workOperation.KeyName != UpdatedWorkOperation.KeyName) 
                                    {
                                        if (workOperation.Position >= UpdatedWorkOperation.Position)
                                        {
                                            OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation).ToList().ForEach(a => { a.Position = tPosition ++; });
                                        }
                                    }
                                }
                                workOperation_ForSetOrder_new = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent && a.IdWorkOperation != UpdatedWorkOperation.IdWorkOperation).OrderBy(a => a.Position).ToList();
                            }
                            
                        }
                        else if (ClonedWorkOperation.Parent == null && SelectedOrder.Parent == "Group_0")
                        {
                            if (SelectedOrder.Parent == "Group_0") // [GEOS2-4933][Rupali Sarode][21-11-2023]
                            {
                                UpdatedWorkOperation.IdParent = 0;
                            }

                            // [GEOS2-4933][Rupali Sarode][21-11-2023]
                            foreach (WorkOperation workOperation in work_Operation_ForSetOrder)
                            {
                                if (workOperation.KeyName != UpdatedWorkOperation.KeyName)
                                {
                                    if (OldPosition < UpdatedWorkOperation.Position && workOperation.Position >= OldPosition && workOperation.Position <= UpdatedWorkOperation.Position)
                                    {
                                        OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= OldPosition && a.Position <= UpdatedWorkOperation.Position).ToList().ForEach(a => { a.Position = a.Position - 1; });
                                    }

                                    if (OldPosition > UpdatedWorkOperation.Position && workOperation.Position >= UpdatedWorkOperation.Position && workOperation.Position <= OldPosition)
                                    {
                                        OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= UpdatedWorkOperation.Position && a.Position <= OldPosition).ToList().ForEach(a => { a.Position = a.Position + 1; });
                                    }
                                }

                               
                            }
                            workOperation_ForSetOrder_new = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent && a.IdWorkOperation != UpdatedWorkOperation.IdWorkOperation).OrderBy(a => a.Position).ToList();
                            //
                        }
                        else if (ClonedWorkOperation.Parent == null && SelectedOrder.Parent != null && SelectedOrder.Parent != "Group_0")
                        {
                            foreach (WorkOperation workOperation in work_Operation_ForSetOrder)
                            {
                                if (workOperation.KeyName != UpdatedWorkOperation.KeyName)
                                {
                                    // If initially Parent was not there and at the time of update, parent is added then increase position by 1
                                    if (workOperation.Position >= UpdatedWorkOperation.Position)
                                    {
                                        OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= UpdatedWorkOperation.Position).ToList().ForEach(a => { a.Position = a.Position + 1; });
                                    }
                                }


                            }
                            workOperation_ForSetOrder_new = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent && a.IdWorkOperation != UpdatedWorkOperation.IdWorkOperation).OrderBy(a => a.Position).ToList();

                        }
                    }
                    else
                    {
                        List<WorkOperation> work_Operation_ForSetOrder = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent).OrderBy(a => a.Position).ToList();

                        if (ClonedWorkOperation.Parent != null && SelectedOrder.Parent != null) // [GEOS2-4933][Rupali Sarode][21-11-2023]
                        {
                            foreach (WorkOperation workOperation in work_Operation_ForSetOrder)
                            {
                                if (workOperation.KeyName != UpdatedWorkOperation.KeyName)
                                {
                                    if (OldPosition < UpdatedWorkOperation.IdOrder && workOperation.Position >= OldPosition && workOperation.Position <= UpdatedWorkOperation.IdOrder)
                                    {
                                        OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= OldPosition && a.Position <= UpdatedWorkOperation.IdOrder).ToList().ForEach(a => { a.Position = a.Position - 1; });
                                    }

                                    //if (OldPosition < UpdatedWorkOperation.Position && workOperation.Position >= OldPosition && workOperation.Position <= UpdatedWorkOperation.Position)
                                    //{
                                    //    OrderList.Where(a => a.IdWorkOperation == workOperation.IdWorkOperation && a.Position >= UpdatedWorkOperation.Position && a.Position <= OldPosition).ToList().ForEach(a => { a.Position = a.Position + 1; });
                                    //}
                                }
                              
                            }
                            workOperation_ForSetOrder_new = OrderList.Where(a => a.Parent == UpdatedWorkOperation.Parent && a.IdWorkOperation != UpdatedWorkOperation.IdWorkOperation).OrderBy(a => a.Position).ToList();

                        }
                    }

                    uint tempPosition = 0;
                    tempPosition = OldPosition;
                    if ((ClonedWorkOperation.IdParent == 0 && UpdatedWorkOperation.IdParent > 0) || (ClonedWorkOperation.IdParent > 0 && ClonedWorkOperation.IdParent != UpdatedWorkOperation.IdParent))  /// If old WO is father (i.e. parent is null or 0) and new parent is assigned then
                    {
                        List<WorkOperation> workOperation_PreviousOrderList = PreviousOrderList.Where(i => i.Name != "---").OrderBy(i => i.Position).ToList();

                        foreach (WorkOperation OldWorkOperation in workOperation_PreviousOrderList)
                        {
                            if (OldWorkOperation.Position > OldPosition)
                            {

                                PreviousOrderList.Where(a => a.IdWorkOperation == OldWorkOperation.IdWorkOperation).ToList().ForEach(a => { a.Position = tempPosition ++; });
                                if (workOperation_ForSetOrder_new != null)
                                {
                                    workOperation_ForSetOrder_new.Add(PreviousOrderList.Where(a => a.IdWorkOperation == OldWorkOperation.IdWorkOperation).FirstOrDefault());

                                }
                            }
                        }
                    }

                    #region GEOS2-3880 Add Log gulab lakade
                    AddChangedWorkOperationLogDetails("Update");
                    UpdatedWorkOperation.LstWorkOperationChangeLogList = WorkOperationChangeLogList.ToList();
                    #endregion
                    //  bool result = ERMService.UpdateWorkOperation_V2240(UpdatedWorkOperation, workOperation_ForSetOrder_new);

                    //[GEOS2-3933][Rupali Sarode][19/09/2022]
                    //bool result = ERMService.UpdateWorkOperation_V2280(UpdatedWorkOperation, workOperation_ForSetOrder_new);//[001][kshinde][10/06/2022][GEOS2-3709]

                    bool result = ERMService.UpdateWorkOperation_V2320(UpdatedWorkOperation, workOperation_ForSetOrder_new);
                    //bool result = true;
                    if (result == true)
                    {
                        try
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationsUpdateMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        { }
                        //var ownerInfo1 = (obj as FrameworkElement);
                        //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOperationsUpdateMessage").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(),
                        //CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo1));
                    }
                    IsSave = true;
                    RequestClose(null, null);

                }
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #region GEOS2-3954 Gulab lakade Time format HH:mm:ss
        public TimeSpan ConvertfloattoTimespan(string observedtime)
        {
            TimeSpan UITempobservedTime;
            try
            {
                GeosApplication.Instance.Logger.Log("Method ConvertfloattoTimespan()...", category: Category.Info, priority: Priority.Low);

                #region GEOS2-3954 Time format HH:MM:SS
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                string tempd = Convert.ToString(observedtime);
                string[] parts = new string[2];
                int i1 = 0;
                int i2 = 0;
                if (tempd.Contains(culterseparator))
                {
                    parts = tempd.Split(Convert.ToChar(culterseparator));
                    i1 = int.Parse(parts[0]);
                    i2 = int.Parse(parts[1]);

                    if (Convert.ToString(parts[1]).Length == 1)
                    {
                        i1 = (i1 * 60) + i2 * 10;
                    }
                    else
                    {
                        i1 = (i1 * 60) + i2;
                    }

                }
                else
                {
                    parts = tempd.Split(Convert.ToChar(culterseparator));
                    i1 = int.Parse(parts[0]);
                    i1 = (i1 * 60);
                }



                UITempobservedTime = TimeSpan.FromSeconds(i1);
                int ts1 = UITempobservedTime.Hours;
                int ts2 = UITempobservedTime.Minutes;
                int ts3 = UITempobservedTime.Seconds;

                GeosApplication.Instance.Logger.Log("Method ConvertfloattoTimespan()....executed successfully", category: Category.Info, priority: Priority.Low);

                #endregion
                return UITempobservedTime;
            }
            catch (Exception ex)
            {
                UITempobservedTime = TimeSpan.FromSeconds(0);
                return UITempobservedTime;
            }

        }

        public TimeSpan ConvertfloattoTimespanForNormalTime(string Normaltime)
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
        public String ConvertTimespantoFloat(String observedtime)
        {

            var currentculter = CultureInfo.CurrentCulture;
            string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
            string[] NormaltimeArr = new string[2];
            int nt1 = 0;
            int nt2 = 0;
            int nt3 = 0;
            if (observedtime.Contains(":"))
            {
                NormaltimeArr = observedtime.Split(':');
                nt1 = int.Parse(NormaltimeArr[0]);
                nt2 = int.Parse(NormaltimeArr[1]);
                nt3 = int.Parse(NormaltimeArr[2]);
                nt1 = (nt1 * 60) + nt2;
            }
            string tempstring = string.Empty;
            if (Convert.ToString(nt3).Length == 1)
            {
                tempstring = Convert.ToString(nt1) + culterseparator + "0" + Convert.ToString(nt3);
            }
            else
            {
                tempstring = Convert.ToString(nt1) + culterseparator + Convert.ToString(nt3);
            }

            return tempstring;
        }
        public string CheckHour(TimeSpan hours)
        {
            string Temptime = string.Empty;
            try
            {
                if (hours.Hours > 0)
                {
                    Temptime = Convert.ToString(hours.ToString(@"hh\:mm\:ss"));
                }
                else
                {
                    Temptime = Convert.ToString(hours.ToString(@"mm\:ss"));
                }
                return Temptime;
            }
            catch (Exception ex)
            {
                return Temptime;
            }
        }
        #endregion
        #region GEOS2-3880 work operation gulab lakade
        public void AddChangedWorkOperationLogDetails(string LogValue)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedWorkOperationLogDetails()...", category: Category.Info, priority: Priority.Low);
                if (LogValue == "New")
                {

                    //string log = "Standard Operations Dictionary " + Convert.ToString(SelectedSODCode) + " has been added.";
                    string log = "The Work Operation " + Convert.ToString(Code) + " has been added.";
                    //WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                    WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });

                }
                else if (LogValue == "Update")
                {
                    #region Workstage
                    List<string> stringList = ClonedWorkOperation.WorkStage.Split(',').ToList();

                    foreach (var item in stringList)
                    {
                        Stages stage = StagesList.Where(s => s.IdStage == Convert.ToInt32(item)).FirstOrDefault();
                        if (stage != null)
                        {
                            foreach (Stages selecteditem in SelectedStages)
                            {
                                if (Convert.ToString(selecteditem.IdStage) != Convert.ToString(stage.IdStage))
                                {
                                    string log = "Work Operation Work stage " + Convert.ToString(stage.CodeWithName) + " has been removed.";
                                    WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                                }
                            }

                        }
                    }

                    if (SelectedStages.Count > 0)
                    {
                        foreach (Stages item in SelectedStages)
                        {
                            if (!stringList.Any(a => a.Equals(Convert.ToString(item.IdStage))))
                            {
                                //tempNewstagelist.Add(item);
                                //string Newstagename = Convert.ToString(item.CodeWithName);
                                string log = "Work Operation Work stage " + Convert.ToString(item.CodeWithName) + " has been added.";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                    }
                    //string newstage = Convert.ToString(SelectedStages);
                    #endregion
                    #region Name
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        if ((Name_en != ClonedWorkOperation.Name_es || Name_en != ClonedWorkOperation.Name_fr || Name_en != ClonedWorkOperation.Name_pt || Name_en != ClonedWorkOperation.Name_ro || Name_en != ClonedWorkOperation.Name_ru || Name_en != ClonedWorkOperation.Name_zh))
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_en) && !string.IsNullOrEmpty(ClonedWorkOperation.Name))
                            {
                                log = "The Name for all language has been changed from " + Convert.ToString(ClonedWorkOperation.Name) + " to " + Convert.ToString(Name) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_en) && !string.IsNullOrEmpty(ClonedWorkOperation.Name))
                            {
                                log = "The Name for all language has been changed from " + Convert.ToString(ClonedWorkOperation.Name) + " to None.";
                            }
                            else
                                if (!string.IsNullOrEmpty(Name_en) && string.IsNullOrEmpty(ClonedWorkOperation.Name))
                            {
                                log = "The Name for all language has been changed from None to " + Convert.ToString(Name) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }

                        }
                    }
                    else
                    {

                        if (Name_en != ClonedWorkOperation.Name)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrEmpty(ClonedWorkOperation.Name) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name))
                            {
                                log = "The Name EN has been changed from " + Convert.ToString((ClonedWorkOperation.Name).Trim()) + " to " + Convert.ToString(Name.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ClonedWorkOperation.Name) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name))
                            {
                                log = "The Name EN has been changed from " + Convert.ToString((ClonedWorkOperation.Name).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name) && !string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name))
                            {
                                log = "The Name EN has been changed from None to " + Convert.ToString(Name.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Name_es != ClonedWorkOperation.Name_es)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_es) && !string.IsNullOrWhiteSpace(Name_es) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_es) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_es))
                            {
                                log = "The Name ES has been changed from " + Convert.ToString((ClonedWorkOperation.Name_es).Trim()) + " to " + Convert.ToString(Name_es.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_es) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_es) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_es))
                            {
                                log = "The Name ES has been changed from " + Convert.ToString((ClonedWorkOperation.Name_es).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name_es) && !string.IsNullOrEmpty(Name_es) && !string.IsNullOrWhiteSpace(Name_es))
                            {
                                log = "The Name ES has been changed from None to " + Convert.ToString(Name_es.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Name_fr != ClonedWorkOperation.Name_fr)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_fr) && !string.IsNullOrWhiteSpace(Name_fr) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_fr) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_fr))
                            {
                                log = "The Name FR has been changed from " + Convert.ToString((ClonedWorkOperation.Name_fr).Trim()) + " to " + Convert.ToString(Name_fr.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_fr) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_fr) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_fr))
                            {
                                log = "The Name FR has been changed from " + Convert.ToString((ClonedWorkOperation.Name_fr).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name_fr) && !string.IsNullOrEmpty(Name_fr) && !string.IsNullOrWhiteSpace(Name_fr))
                            {
                                log = "The Name FR has been changed from None to " + Convert.ToString(Name_fr.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Name_pt != ClonedWorkOperation.Name_pt)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_pt) && !string.IsNullOrWhiteSpace(Name_pt) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_pt) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_pt))
                            {
                                log = "The Name PT has been changed from " + Convert.ToString((ClonedWorkOperation.Name_pt).Trim()) + " to " + Convert.ToString(Name_pt.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_pt) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_pt) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_pt))
                            {
                                log = "The Name PT has been changed from " + Convert.ToString((ClonedWorkOperation.Name_pt).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name_pt) && !string.IsNullOrEmpty(Name_pt) && !string.IsNullOrWhiteSpace(Name_pt))
                            {
                                log = "The Name PT has been changed from None to " + Convert.ToString(Name_pt.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Name_ro != ClonedWorkOperation.Name_ro)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_ro) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_ro) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_ro))
                            {
                                log = "The Name RO has been changed from " + Convert.ToString((ClonedWorkOperation.Name_ro).Trim()) + " to " + Convert.ToString(Name_ro.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_ro) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_ro) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_ro))
                            {
                                log = "The Name RO has been changed from " + Convert.ToString((ClonedWorkOperation.Name_ro).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name_ro) && !string.IsNullOrEmpty(Name_ro) && !string.IsNullOrWhiteSpace(Name_ro))
                            {
                                log = "The Name RO has been changed from None to " + Convert.ToString(Name_ro.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Name_ru != ClonedWorkOperation.Name_ru)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_ru) && !string.IsNullOrWhiteSpace(Name_ru) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_ru) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_ru))
                            {
                                log = "The Name RU has been changed from " + Convert.ToString((ClonedWorkOperation.Name_ru).Trim()) + " to " + Convert.ToString(Name_ru.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_ru) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_ru) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_ru))
                            {
                                log = "The Name RU has been changed from " + Convert.ToString((ClonedWorkOperation.Name_ru).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name_ru) && !string.IsNullOrEmpty(Name_ru) && !string.IsNullOrWhiteSpace(Name_ru))
                            {
                                log = "The Name RU has been changed from None to " + Convert.ToString(Name_ru.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Name_zh != ClonedWorkOperation.Name_zh)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_zh) && !string.IsNullOrWhiteSpace(Name_zh) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_zh) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_zh))
                            {
                                log = "The Name ZH has been changed from " + Convert.ToString((ClonedWorkOperation.Name_zh).Trim()) + " to " + Convert.ToString(Name_zh.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_zh) && !string.IsNullOrEmpty(ClonedWorkOperation.Name_zh) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Name_zh))
                            {
                                log = "The Name ZH has been changed from " + Convert.ToString((ClonedWorkOperation.Name_zh).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Name_zh) && !string.IsNullOrEmpty(Name_zh) && !string.IsNullOrWhiteSpace(Name_zh))
                            {
                                log = "The Name ZH has been changed from None to " + Convert.ToString(Name_zh.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                    }

                    #endregion

                    #region Description
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        if (Description_en != ClonedWorkOperation.Description_es || Description_en != ClonedWorkOperation.Description_fr || Description_en != ClonedWorkOperation.Description_pt || Description_en != ClonedWorkOperation.Description_ro || Description_en != ClonedWorkOperation.Description_ru || Description_en != ClonedWorkOperation.Description_zh)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Description)) && !string.IsNullOrEmpty(Convert.ToString(Description)))
                            {
                                log = "The Description for all language has been changed from " + Convert.ToString(ClonedWorkOperation.Description) + " to " + Convert.ToString(Description) + ".";
                            }
                            else
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Description)) && !string.IsNullOrEmpty(Convert.ToString(Description)))
                            {
                                log = "The Description for all language has been changed from None to " + Convert.ToString(Description) + ".";
                            }
                            else
                                if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Description)) && string.IsNullOrEmpty(Convert.ToString(Description)))
                            {
                                log = "The Description for all language has been changed from " + Convert.ToString(ClonedWorkOperation.Description) + " to None.";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                    }
                    else
                    {
                        if (Description_en != ClonedWorkOperation.Description)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description) && !string.IsNullOrWhiteSpace(Description) && !string.IsNullOrEmpty(ClonedWorkOperation.Description) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description))
                            {
                                log = "The Description EN has been changed from " + Convert.ToString((ClonedWorkOperation.Description).Trim()) + " to " + Convert.ToString(Description.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(ClonedWorkOperation.Description) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description))
                            {
                                log = "The Description EN has been changed from " + Convert.ToString((ClonedWorkOperation.Description).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description) && !string.IsNullOrEmpty(Description) && !string.IsNullOrWhiteSpace(Description))
                            {
                                log = "The Description EN has been changed from None to " + Convert.ToString(Description.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Description_es != ClonedWorkOperation.Description_es)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_es) && !string.IsNullOrWhiteSpace(Description_es) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_es) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_es))
                            {
                                log = "The Description ES has been changed from " + Convert.ToString((ClonedWorkOperation.Description_es).Trim()) + " to " + Convert.ToString(Description_es.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_es) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_es) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_es))
                            {
                                log = "The Description ES has been changed from " + Convert.ToString((ClonedWorkOperation.Description_es).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description_es) && !string.IsNullOrEmpty(Description_es) && !string.IsNullOrWhiteSpace(Description_es))
                            {
                                log = "The Description ES has been changed from None to " + Convert.ToString(Description_es.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Description_fr != ClonedWorkOperation.Description_fr)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_fr) && !string.IsNullOrWhiteSpace(Description_fr) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_fr) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_fr))
                            {
                                log = "The Description FR has been changed from " + Convert.ToString((ClonedWorkOperation.Description_fr).Trim()) + " to " + Convert.ToString(Description_fr.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_fr) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_fr) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_fr))
                            {
                                log = "The Description FR has been changed from " + Convert.ToString((ClonedWorkOperation.Description_fr).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description_fr) && !string.IsNullOrEmpty(Description_fr) && !string.IsNullOrWhiteSpace(Description_fr))
                            {
                                log = "The Description FR has been changed from None to " + Convert.ToString(Description_fr.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                        if (Description_pt != ClonedWorkOperation.Description_pt)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_pt) && !string.IsNullOrWhiteSpace(Description_pt) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_pt) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_pt))
                            {
                                log = "The Description PT has been changed from " + Convert.ToString((ClonedWorkOperation.Description_pt).Trim()) + " to " + Convert.ToString(Description_pt.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_pt) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_pt) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_pt))
                            {
                                log = "The Description PT has been changed from " + Convert.ToString((ClonedWorkOperation.Description_pt).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description_pt) && !string.IsNullOrEmpty(Description_pt) && !string.IsNullOrWhiteSpace(Description_pt))
                            {
                                log = "The Description PT has been changed from None to " + Convert.ToString(Description_pt.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Description_ro != ClonedWorkOperation.Description_ro)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_ro) && !string.IsNullOrWhiteSpace(Description_ro) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_ro) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_ro))
                            {
                                log = "The Description RO has been changed from " + Convert.ToString((ClonedWorkOperation.Description_ro).Trim()) + " to " + Convert.ToString(Description_ro.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_ro) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_ro) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_ro))
                            {
                                log = "The Description RO has been changed from " + Convert.ToString((ClonedWorkOperation.Description_ro).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description_ro) && !string.IsNullOrEmpty(Description_ro) && !string.IsNullOrWhiteSpace(Description_ro))
                            {
                                log = "The Description RO has been changed from None to " + Convert.ToString(Description_ro.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Description_ru != ClonedWorkOperation.Description_ru)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_ru) && !string.IsNullOrWhiteSpace(Description_ru) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_ru) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_ru))
                            {
                                log = "The Description RU has been changed from " + Convert.ToString((ClonedWorkOperation.Description_ru).Trim()) + " to " + Convert.ToString(Description_ru.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_ru) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_ru) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_ru))
                            {
                                log = "The Description RU has been changed from " + Convert.ToString((ClonedWorkOperation.Description_ru).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description_ru) && !string.IsNullOrEmpty(Description_ru) && !string.IsNullOrWhiteSpace(Description_ru))
                            {
                                log = "The Description RU has been changed from None to " + Convert.ToString(Description_ru.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }

                        if (Description_zh != ClonedWorkOperation.Description_zh)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_zh) && !string.IsNullOrWhiteSpace(Description_zh) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_zh) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_zh))
                            {
                                log = "The Description ZH has been changed from " + Convert.ToString((ClonedWorkOperation.Description_zh).Trim()) + " to " + Convert.ToString(Description_zh.Trim()) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_zh) && !string.IsNullOrEmpty(ClonedWorkOperation.Description_zh) && !string.IsNullOrWhiteSpace(ClonedWorkOperation.Description_zh))
                            {
                                log = "The Description ZH has been changed from " + Convert.ToString((ClonedWorkOperation.Description_zh).Trim()) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkOperation.Description_zh) && !string.IsNullOrEmpty(Description_zh) && !string.IsNullOrWhiteSpace(Description_zh))
                            {
                                log = "The Description ZH has been changed from None to " + Convert.ToString(Description_zh.Trim()) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                            }
                        }
                    }
                    #endregion
                    #region Parent
                    //if (Convert.ToString(ClonedWorkOperation.IdParent) !=null)
                    //{
                    string SelectedParentold = string.Empty;
                    SelectedParentold = Convert.ToString(ClonedWorkOperation.Parent);
                    // var SelectedParentold = ParentList.FirstOrDefault(x => x.IdWorkOperation == ClonedWorkOperation.IdParent);
                    //if(SelectedParentold!=null)
                    //{ 
                    if (Convert.ToString(SelectedParentold) != Convert.ToString(selectedParent.Name))
                    {

                        if (!string.IsNullOrEmpty(Convert.ToString(SelectedParentold)) && !string.IsNullOrEmpty(Convert.ToString(selectedParent.Name)) && (Convert.ToString(selectedParent.Name) != "---"))
                        {
                            string log = "Work Operation Parent has been changed from " + Convert.ToString(SelectedParentold) + " to " + Convert.ToString(selectedParent.Name) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                        if ((string.IsNullOrEmpty(Convert.ToString(selectedParent.Name)) || (Convert.ToString(selectedParent.Name) == "---")) && (!string.IsNullOrEmpty(Convert.ToString(SelectedParentold)) && Convert.ToString(SelectedParentold) != "---"))
                        {
                            string log = "Work Operation Parent has been changed from " + Convert.ToString(SelectedParentold) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(SelectedParentold)) && (Convert.ToString(selectedParent.Name) != "---") && !string.IsNullOrEmpty(Convert.ToString(selectedParent.Name)))
                        {
                            string log = "Work Operation Parent has been changed from None to " + Convert.ToString(selectedParent.Name) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }

                    }
                    // }
                    //}
                    //else
                    //if(string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.IdParent)))
                    //{

                    //}
                    #endregion
                    #region Order old Commented for [GEOS2-3976][Rupali Sarode][15-11-2022]
                    //if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.IdWorkOperation)))
                    //{
                    //    string OrderNamenew = Convert.ToString(SelectedOrder.Name);

                    //    //SelectedParentoldTemp = ParentList.FirstOrDefault(x => x.IdWorkOperation == ClonedWorkOperation.IdParent);
                    //    var OrderListold = new ObservableCollection<WorkOperation>(AllWorkOperationList.Where(a => a.IdWorkOperation == ClonedWorkOperation.IdWorkOperation).ToList());

                    //    if (OrderListold.Count > 0)
                    //    {
                    //        var SelectedOrderold = OrderListold.FirstOrDefault(x => x.IdWorkOperation == ClonedWorkOperation.IdWorkOperation);
                    //        if (SelectedOrderold != null)
                    //        {
                    //            string OrderNameOld = Convert.ToString(SelectedOrderold.Name);
                    //            if (OrderNameOld != OrderNamenew)
                    //            {
                    //                if (!string.IsNullOrEmpty(OrderNameOld) && !string.IsNullOrEmpty(OrderNamenew))
                    //                {
                    //                    if (OrderNamenew == "---")
                    //                    {
                    //                        OrderNamenew = "None";
                    //                    }
                    //                    if (OrderNameOld == "---")
                    //                    {
                    //                        OrderNameOld = "None";
                    //                    }
                    //                    string log = "Work Operation Order has been changed from " + Convert.ToString(OrderNameOld) + " to " + Convert.ToString(OrderNamenew) + ".";
                    //                    WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                    //                }
                    //                else
                    //                    if (!string.IsNullOrEmpty(OrderNameOld) && string.IsNullOrEmpty(OrderNamenew))
                    //                    {
                    //                        if (OrderNameOld == "---")
                    //                        {
                    //                            OrderNameOld = "None";
                    //                        }
                    //                        if (OrderNameOld != "None")
                    //                        {
                    //                            string log = "Work Operation Order has been changed from " + Convert.ToString(OrderNameOld) + " to None.";
                    //                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                    //                        }
                    //                    }

                    //            }

                    //        }
                    //        else
                    //        if (!string.IsNullOrEmpty(OrderNamenew))
                    //        {
                    //            if (OrderNamenew == "---")
                    //            {
                    //                OrderNamenew = "None";
                    //            }
                    //            if (OrderNamenew != "None")
                    //            {
                    //                string log = "Work Operation Order has been changed from None to " + Convert.ToString(OrderNamenew) + ".";
                    //                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                    //            }
                    //        }
                    //    }



                    //}
                    #endregion old  // 

                    #region Order New [GEOS2-3976][Rupali Sarode][15-11-2022]
                    if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.IdWorkOperation)))
                    {
                        string OrderNamenew = Convert.ToString(SelectedOrder.Name);

                        //SelectedParentoldTemp = ParentList.FirstOrDefault(x => x.IdWorkOperation == ClonedWorkOperation.IdParent);
                        #region [GEOS2-3976][Rupali Sarode][15-11-2022]
                        //var OrderListold = new ObservableCollection<WorkOperation>(AllWorkOperationList.Where(a => a.IdWorkOperation == ClonedWorkOperation.IdWorkOperation).ToList());
                        // var tOrder = ClonedWorkOperation.Order - 1;
                        string OrderNameOld = string.Empty;
                        ulong tOrder = 0;

                        if (idWorkOperation == position) tOrder = ClonedWorkOperation.Order;
                        else
                            //{  if (position == 1)
                            //        tOrder = ClonedWorkOperation.Order;
                            //    else
                            tOrder = ClonedWorkOperation.Order - 1;
                        //}

                        if (Convert.ToString(SelectedParentold) != Convert.ToString(selectedParent.Name))
                        {
                            if (tOrder == 0)
                                OrderNameOld = OrderList.Where(a => a.Position == tOrder).Select(b => b.Name).FirstOrDefault();
                            else  // If parent is changed then search in original List.
                                OrderNameOld = AllWorkOperationList.Where(a => a.IdParent == ClonedWorkOperation.IdParent && a.Position == tOrder).Select(b => b.Name).FirstOrDefault();
                        }
                        else
                            OrderNameOld = OrderList.Where(a => a.Position == tOrder).Select(b => b.Name).FirstOrDefault();




                        #endregion [GEOS2-3976][Rupali Sarode][15-11-2022]

                        if (!string.IsNullOrEmpty(OrderNameOld) && !string.IsNullOrEmpty(OrderNamenew))
                        {
                            if (OrderNameOld != OrderNamenew)
                            {
                                if (OrderNamenew == "---") OrderNamenew = "None";

                                if (OrderNameOld == "---") OrderNameOld = "None";

                                string log = "Work Operation Order has been changed from " + Convert.ToString(OrderNameOld) + " to " + Convert.ToString(OrderNamenew) + ".";
                                WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });

                            }

                        }

                    }
                    #endregion Order New [GEOS2-3976][Rupali Sarode][15-11-2022]



                    #region Type
                    if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Type)) && !string.IsNullOrEmpty(Convert.ToString(SelectedType)))
                    {
                        if (Convert.ToString(SelectedType.Value) != "---")
                        {
                            string log = "Work Operation Type has been changed from None to " + Convert.ToString(SelectedType.Value) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    else
                    if (Convert.ToString(ClonedWorkOperation.Type.Value) != Convert.ToString(SelectedType.Value))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Type.Value)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Type.Value)) && Convert.ToString(SelectedType.Value) != "---")
                        {
                            string log = "Work Operation Type has been changed from " + Convert.ToString(ClonedWorkOperation.Type.Value) + " to " + Convert.ToString(SelectedType.Value) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Type.Value)) && (Convert.ToString(SelectedType.Value) == "---" || String.IsNullOrEmpty(Convert.ToString(SelectedType.Value))))
                        {
                            string log = "Work Operation Type has been changed from " + Convert.ToString(ClonedWorkOperation.Type.Value) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }

                    }
                    #endregion


                    #region Status
                    if (Convert.ToString(ClonedWorkOperation.Status.Value) != Convert.ToString(SelectedStatus.Value))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Status.Value)) && !string.IsNullOrEmpty(Convert.ToString(SelectedStatus.Value)))
                        {
                            string log = "Work Operation Status has been changed from " + Convert.ToString(ClonedWorkOperation.Status.Value) + " to " + Convert.ToString(SelectedStatus.Value) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Status.Value)) && !string.IsNullOrEmpty(Convert.ToString(SelectedStatus.Value)))
                        {
                            string log = "Work Operation Status has been changed from None to " + Convert.ToString(SelectedStatus.Value) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(SelectedStatus.Value)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Status.Value)))
                        {
                            string log = "Work Operation Status has been changed from " + Convert.ToString(ClonedWorkOperation.Status.Value) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }

                    #endregion
                    #region Distance
                    if (Convert.ToString(ClonedWorkOperation.Distance) != Convert.ToString(Distance))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Distance)) && !string.IsNullOrEmpty(Convert.ToString(Distance)))
                        {
                            string log = "Work Operation Distance has been changed from " + Convert.ToString(ClonedWorkOperation.Distance) + " to " + Convert.ToString(Distance) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Distance)) && !string.IsNullOrEmpty(Convert.ToString(Distance)))
                        {
                            string log = "Work Operation Distance has been changed from None to " + Convert.ToString(Distance) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(Distance)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Distance)))
                        {
                            string log = "Work Operation Distance has been changed from " + Convert.ToString(ClonedWorkOperation.Distance) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    #endregion
                    #region ObserveTime
                    if (Convert.ToString(ClonedWorkOperation.ObservedTime) != Convert.ToString(ObservedTime))
                    {
                        string OldObservedTime = string.Empty;
                        string NewObservedTime = string.Empty;
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ObservedTime)) && !string.IsNullOrEmpty(Convert.ToString(ObservedTime)))
                        {
                            ClonedWorkOperation.UITempobservedTime = TimeSpan.FromMinutes(Convert.ToDouble(ClonedWorkOperation.ObservedTime)); //ConvertfloattoTimespan(Convert.ToString(ClonedWorkOperation.ObservedTime));
                            OldObservedTime = Convert.ToString(CheckHour(ClonedWorkOperation.UITempobservedTime));
                            NewObservedTime = Convert.ToString(CheckHour(UITempobservedTime));
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ObservedTime)) && !string.IsNullOrEmpty(Convert.ToString(ObservedTime)))
                        {
                            // string OldObservedTime =Convert.ToString(ConvertfloattoTimespan(Convert.ToString(ClonedWorkOperation.ObservedTime)));
                            string log = "Work Operation ObservedTime has been changed from " + Convert.ToString(OldObservedTime) + " to " + Convert.ToString(NewObservedTime) + ".";
                            //string log = "Work Operation ObservedTime has been changed from " + Convert.ToString(ClonedWorkOperation.ObservedTime) + " to " + Convert.ToString(ObservedTime) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ObservedTime)) && !string.IsNullOrEmpty(Convert.ToString(ObservedTime)))
                        {
                            string log = "Work Operation ObservedTime has been changed from None to " + Convert.ToString(NewObservedTime) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(ObservedTime)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ObservedTime)))
                        {
                            //string OldObservedTime = Convert.ToString(ConvertfloattoTimespan(Convert.ToString(ClonedWorkOperation.ObservedTime)));
                            string log = "Work Operation ObservedTime has been changed from " + Convert.ToString(OldObservedTime) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    #endregion
                    #region Activity
                    if (Convert.ToString(ClonedWorkOperation.Activity) != Convert.ToString(Activity))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Activity)) && !string.IsNullOrEmpty(Convert.ToString(Activity)))
                        {
                            string log = "Work Operation Activity has been changed from " + Convert.ToString(ClonedWorkOperation.Activity) + " to " + Convert.ToString(Activity) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Activity)) && !string.IsNullOrEmpty(Convert.ToString(Activity)))
                        {
                            string log = "Work Operation Activity has been changed from None to " + Convert.ToString(Activity) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if (string.IsNullOrEmpty(Convert.ToString(Activity)) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Activity)))
                        {
                            string log = "Work Operation Activity has been changed from " + Convert.ToString(ClonedWorkOperation.Activity) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    #endregion
                    //#region NormalTime
                    //float oldNormalTime = 0;
                    //if (ClonedWorkOperation.ObservedTime.Value > 0)
                    //{
                    //    oldNormalTime = (float)Math.Round(ClonedWorkOperation.ObservedTime.Value * ((float)ClonedWorkOperation.Activity / 100), 2);
                    //}
                    //else
                    //{
                    //    oldNormalTime = ClonedWorkOperation.NormalTime;
                    //}
                    //if ((Convert.ToString(oldNormalTime) != Convert.ToString(NormalTime)) && oldNormalTime != 0)
                    //{
                    //    if (!string.IsNullOrEmpty(Convert.ToString(oldNormalTime)) && !string.IsNullOrEmpty(Convert.ToString(NormalTime)))
                    //    {
                    //        string log = "Work Operation NormalTime has been changed from " + Convert.ToString(oldNormalTime) + " to " + Convert.ToString(NormalTime) + ".";
                    //        WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                    //    }
                    //    else
                    //        if (string.IsNullOrEmpty(Convert.ToString(oldNormalTime)) || oldNormalTime == 0)
                    //    {
                    //        string log = "Work Operation NormalTime has been changed from None to " + Convert.ToString(NormalTime) + ".";
                    //        WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                    //    }
                    //    else
                    //        if (string.IsNullOrEmpty(Convert.ToString(NormalTime)))
                    //    {
                    //        string log = "Work Operation NormalTime has been changed from " + Convert.ToString(oldNormalTime) + " to None.";
                    //        WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                    //    }
                    //}
                    //#endregion
                    #region Detected problem
                    if (Convert.ToString(ClonedWorkOperation.DetectedProblems) != Convert.ToString(DetectedProblems))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.DetectedProblems)) && !string.IsNullOrEmpty(Convert.ToString(DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(DetectedProblems)))
                        {
                            string log = "Work Operation Detected Problems has been changed from " + Convert.ToString((ClonedWorkOperation.DetectedProblems).Trim()) + " to " + Convert.ToString(DetectedProblems.Trim()) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if ((string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.DetectedProblems)) || string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.DetectedProblems))) && !string.IsNullOrEmpty(Convert.ToString(DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(DetectedProblems)))
                        {
                            string log = "Work Operation Detected Problems has been changed from None to " + Convert.ToString(DetectedProblems.Trim()) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if ((string.IsNullOrEmpty(Convert.ToString(DetectedProblems)) || string.IsNullOrWhiteSpace(Convert.ToString(DetectedProblems))) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.DetectedProblems)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.DetectedProblems)))
                        {
                            string log = "Work Operation Detected Problems has been changed from " + Convert.ToString((ClonedWorkOperation.DetectedProblems).Trim()) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    #endregion
                    #region Improvementproposal
                    if (Convert.ToString(ClonedWorkOperation.ImprovementsProposals) != Convert.ToString(ImprovementsProposals))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.ImprovementsProposals)) && !string.IsNullOrEmpty(Convert.ToString(ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(ImprovementsProposals)))
                        {
                            string log = "Work Operation Improvements Proposals has been changed from " + Convert.ToString((ClonedWorkOperation.ImprovementsProposals).Trim()) + " to " + Convert.ToString(ImprovementsProposals.Trim()) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if ((string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ImprovementsProposals)) || string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.ImprovementsProposals))) && !string.IsNullOrEmpty(Convert.ToString(ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(ImprovementsProposals)))
                        {
                            string log = "Work Operation Improvements Proposals has been changed from None to " + Convert.ToString(ImprovementsProposals.Trim()) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if ((string.IsNullOrEmpty(Convert.ToString(ImprovementsProposals)) || string.IsNullOrWhiteSpace(Convert.ToString(ImprovementsProposals))) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.ImprovementsProposals)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.ImprovementsProposals)))
                        {
                            string log = "Work Operation Improvements Proposals has been changed from " + Convert.ToString((ClonedWorkOperation.ImprovementsProposals).Trim()) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    #endregion
                    #region Remarks
                    if (Convert.ToString(ClonedWorkOperation.Remarks) != Convert.ToString(Remarks))
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(Remarks)) && !string.IsNullOrEmpty(Convert.ToString(Remarks)))
                        {
                            string log = "Work Operation Remarks has been changed from " + Convert.ToString(ClonedWorkOperation.Remarks) + " to " + Convert.ToString(Remarks) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if ((string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Remarks)) || string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.Remarks))) && !string.IsNullOrEmpty(Convert.ToString(Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(Remarks)))
                        {
                            string log = "Work Operation Remarks has been changed from None to " + Convert.ToString(Remarks.Trim()) + ".";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                        else
                            if ((string.IsNullOrEmpty(Convert.ToString(Remarks)) || string.IsNullOrWhiteSpace(Convert.ToString(Remarks))) && !string.IsNullOrEmpty(Convert.ToString(ClonedWorkOperation.Remarks)) && !string.IsNullOrWhiteSpace(Convert.ToString(ClonedWorkOperation.Remarks)))
                        {
                            string log = "Work Operation Remarks has been changed from " + Convert.ToString((ClonedWorkOperation.Remarks).Trim()) + " to None.";
                            WorkOperationChangeLogList.Add(new WorkOperationChangeLog() { IdWorkOperation = idWorkOperation, Datetime = Convert.ToDateTime(GeosApplication.Instance.ServerDateTime), IdUser = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser), Comments = Convert.ToString(log) });
                        }
                    }
                    #endregion

                }


                GeosApplication.Instance.Logger.Log("Method AddChangedWorkOperationLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedWorkOperationLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private Visibility isAccordionControlVisibleLogs;
        public Visibility IsAccordionControlVisibleLogs
        {
            get { return isAccordionControlVisibleLogs; }
            set
            {
                isAccordionControlVisibleLogs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisibleLogs"));
            }
        }
        private void HideLogPanel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method HideLogPanel ...", category: Category.Info, priority: Priority.Low);

                if (IsAccordionControlVisibleLogs == Visibility.Collapsed)
                    IsAccordionControlVisibleLogs = Visibility.Visible;
                else
                    IsAccordionControlVisibleLogs = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method HideLogPanel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion


        #region Validations
        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                //string error =
                //    me[BindableBase.GetPropertyName(() => SelectedStages)] +
                //    me[BindableBase.GetPropertyName(() => Name)] +
                //     me[BindableBase.GetPropertyName(() => Code)] +
                //     me[BindableBase.GetPropertyName(() => ObservedTime)] +
                //     me[BindableBase.GetPropertyName(() => Activity)];

                string error =
                   me[BindableBase.GetPropertyName(() => SelectedStages)] +
                   me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => Code)] +
                    me[BindableBase.GetPropertyName(() => UITempobservedTime)] +
                    me[BindableBase.GetPropertyName(() => Activity)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string name = BindableBase.GetPropertyName(() => Name);
                string code = BindableBase.GetPropertyName(() => Code);
                var selectedStages = BindableBase.GetPropertyName(() => SelectedStages);

                var observedTime = BindableBase.GetPropertyName(() => ObservedTime);
                var activity = BindableBase.GetPropertyName(() => Activity);
                //[Rupali Sarode][21-11-2022]
                var uITempobservedTime = BindableBase.GetPropertyName(() => UITempobservedTime);


                if (columnName == name)
                {
                    return AddEditWorkOperationValidation.GetErrorMessage(name, Name, false);
                }
                if (columnName == code)
                {
                    return AddEditWorkOperationValidation.GetErrorMessage(code, Code, false);
                }
                if (columnName == selectedStages)
                {
                    return AddEditWorkOperationValidation.GetErrorMessage(selectedStages, SelectedStages, false);
                }
                //[001][kshinde][GEOS2-3709][14/06/2022]

                if (columnName == observedTime)
                {
                    return AddEditWorkOperationValidation.GetErrorMessage(observedTime.ToString(), ObservedTime, false);
                }
                if (columnName == activity)
                {
                    return AddEditWorkOperationValidation.GetErrorMessage(activity.ToString(), Activity, false);
                }
                if (columnName == uITempobservedTime) //[Rupali Sarode][21-11-2022]
                {
                    return AddEditWorkOperationValidation.GetErrorMessage(uITempobservedTime.ToString(), UITempobservedTime, IsFatherFlag);
                }
                return null;
            }
        }
        #endregion

        #region [GEOS2-4994][Rupali Sarode][23-11-2023]
        private void ExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()...", category: Category.Info, priority: Priority.Low);

                string TimePart = string.Empty;
                string DatePart = string.Empty;
                DatePart = DateTime.Now.ToShortDateString().Replace("/", "");
                DatePart = DatePart.Replace("-", "");
                DatePart = DatePart.Replace(".", "");

                TimePart = DateTime.Now.ToShortTimeString().Replace(":", "");
                TimePart = TimePart.Replace(" AM", "");
                TimePart = TimePart.Replace(" PM", "");

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                //  saveFile.FileName = "ERMHistory_" + Code + "_" + DateTime.Now.ToString("MMddyyyy_hhmm");
                saveFile.FileName = "ERMHistory_" + Code + "_" + DatePart + "_" + TimePart;

                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {

                    if (!DXSplashScreen.IsActive)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }
                    ResultFileName = (saveFile.FileName);
                    TableView ChangeLogTableView = ((TableView)obj);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = false;
                    ChangeLogTableView.ExportToXlsx(ResultFileName);


                    //Wrap Text

                    var workbook = new Workbook();

                    workbook.LoadDocument(ResultFileName, DocumentFormat.Xlsx);
                    Worksheet worksheet = workbook.Worksheets[0];

                    workbook.BeginUpdate();

                    workbook.Worksheets[0].Columns[1].Width = 450;
                    workbook.Worksheets[0].Columns[2].Alignment.WrapText = true;
                    workbook.Worksheets[0].Columns[2].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                    workbook.EndUpdate();

                    workbook.SaveDocument(ResultFileName);

                    //


                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    ChangeLogTableView.ShowTotalSummary = false;
                    ChangeLogTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportToExcel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportToExcel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}

