using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
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

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class AddEditWorkStagesViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IERMService ERMService = new ERMServiceController("localhost:6699");
        IGeosRepositoryService geosService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        #region Declaration
        private Int32 idStage;
        private ObservableCollection<Site> plantList;
        private string windowHeader;
        private List<LookupValue> statusList;
        private LookupValue selectedStatus;
        private ObservableCollection<Language> languages;
        private Language languageSelected;

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
        private bool activateRework;
        private bool isProductionStage;
        private bool isNew;
        private bool isSave;
        private Visibility isAccordionControlVisibleLogs;
        private WorkStages clonedWorkStage;
        private object sequence;
        private ObservableCollection<WorkStages> sequenceList;
        private ObservableCollection<WorkStages> tempSequenceList;
        private bool? oldIsCheckedAllLanguages = null;
        private bool isCheckedAllLanguages;
        private bool isEnabledSaveButton;
        private WorkStages newWorkStages;
        private WorkStages updateWorkStages;
        public bool IsFromInformation = false;
        private string error = string.Empty;
        private List<string> CodeList;
        private string prevCode;

        #region [GEOS2-3908][Rupali Sarode][16-1-2023]
        private ObservableCollection<LogentriesbyStages> stagesChangeLogList;
        private ObservableCollection<LogentriesbyStages> stagesAllChangeLogList;
        private string tempIsProductionStage;
        public LookupValue SelectedOriginalStatus { get; set; }

        #endregion [GEOS2-3908][Rupali Sarode][16-1-2023]

        MaximizedElementPosition maximizedElementPosition;
        #endregion

        #region Icommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand CheckedCopyNameDescriptionCommand { get; set; }
        public ICommand ChangeDescriptionCommand { get; set; }
        public ICommand ChangeNameCommand { get; set; }
        public ICommand ChangeCheckAllLanguagesCommand { get; set; }
        public ICommand UncheckedCopyNameDescriptionCommand { get; set; }
        public ICommand ChangePlantCommand { get; set; }
        #endregion

        #region Properties

        public Int32 IdStage
        {
            get { return idStage; }
            set
            {
                idStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdStage"));
            }
        }
        public Visibility IsAccordionControlVisibleLogs
        {
            get { return isAccordionControlVisibleLogs; }
            set
            {
                isAccordionControlVisibleLogs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccordionControlVisibleLogs"));
            }
        }

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

        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }
        private string tempSelectedPlant;
        public string TempSelectedPlant
        {
            get
            {
                return tempSelectedPlant;
            }

            set
            {
                tempSelectedPlant = value;
            }
        }
        private List<object> selectedPlant;
        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                if (value != null && value.Count >= 2)
                {
                    foreach (Site item in value)
                    {
                        if (item.Name.Equals("---"))
                        {
                            value.Remove(item);
                            break;
                        }
                    }
                }
                TempSelectedPlant = string.Empty;
                if (value != null)
                {
                    foreach (Site item in value)
                    {
                        if (string.IsNullOrEmpty(TempSelectedPlant))
                        {
                            if (Convert.ToString(item.Name) != "---")
                            {
                                TempSelectedPlant = Convert.ToString(item.Name);
                            }

                        }
                        else
                        {
                            if (Convert.ToString(item.Name) != "---")
                            {
                                TempSelectedPlant = TempSelectedPlant + "," + Convert.ToString(item.Name);
                            }
                        }

                    }
                }
                selectedPlant = value;
                if (selectedPlant == null)
                {
                    FillPlant();
                }

                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        private void FillPlant()
        {
            try
            {
                SelectedPlant = new List<object>();
                SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == 0));
            }
            catch (Exception ex) { }
        }
        private List<object> selectedPlantold;
        public List<object> SelectedPlantold
        {
            get
            {
                return selectedPlantold;
            }

            set
            {
                if (value != null && value.Count >= 2)
                {
                    foreach (Site item in value)
                    {
                        if (item.Name.Equals("---"))
                        {
                            value.Remove(item);
                            break;
                        }
                    }
                }
                selectedPlantold = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantold"));
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

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
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

        public string Code
        {
            get { return code; }
            set
            {
                code = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
                //if (!string.IsNullOrEmpty(value))
                //{
                //    InformationError = null;
                //}
            }
        }

        public string PrevCode
        {
            get
            {
                return prevCode;
            }
            set
            {
                prevCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrevCode"));
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

        public bool ActivateRework
        {
            get { return activateRework; }
            set
            {
                activateRework = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActivateRework"));
            }
        }

        public bool IsProductionStage
        {
            get { return isProductionStage; }
            set
            {
                isProductionStage = value;
                if (isProductionStage == false)
                {
                    TempIsProductionStage = "NO";
                    Code = null;
                }
                else if (isProductionStage == true)
                {
                    TempIsProductionStage = "YES";
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsProductionStage"));
            }
        }
        public string TempIsProductionStage
        {
            get { return tempIsProductionStage; }
            set
            {
                tempIsProductionStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempIsProductionStage"));
            }
        }
        public WorkStages ClonedWorkStage
        {
            get { return clonedWorkStage; }
            set
            {
                clonedWorkStage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedWorkStage"));
            }
        }

        public virtual object Sequence
        {
            get { return sequence; }
            set
            {
                var tempWorkStagesForSeq = (WorkStages)value;
                if (tempWorkStagesForSeq.IsSequenceExists == true)
                { return; }
                sequence = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sequence"));
            }
        }
        public bool IsCheckedAllLanguages
        {
            get
            {
                return isCheckedAllLanguages;
            }
            set
            {
                isCheckedAllLanguages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCheckedAllLanguages"));
            }
        }
        public bool IsEnabledSaveButton
        {
            get { return isEnabledSaveButton; }
            set
            {
                //if (GeosApplication.Instance.IsEditSODPermissionERM)
                //{
                if (isEnabledSaveButton != value)
                {
                    isEnabledSaveButton = value;
                }
                //}
                //else
                //{
                //    isEnabledSaveButton = false;
                //}

                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledSaveButton"));
            }
        }
        public ObservableCollection<WorkStages> SequenceList
        {
            get
            {
                return sequenceList;
            }

            set
            {
                sequenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SequenceList"));
            }
        }
        public ObservableCollection<WorkStages> TempSequenceList
        {
            get
            {
                return tempSequenceList;
            }

            set
            {
                tempSequenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempSequenceList"));
            }
        }
        public WorkStages NewWorkStages
        {
            get
            {
                return newWorkStages;
            }

            set
            {
                newWorkStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewWorkStages"));
            }
        }

        public WorkStages UpdateWorkStages
        {
            get
            {
                return updateWorkStages;
            }

            set
            {
                updateWorkStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateWorkStages"));
            }
        }


        public ObservableCollection<LogentriesbyStages> StagesChangeLogList
        {
            get { return stagesChangeLogList; }
            set
            {
                stagesChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StagesChangeLogList"));
            }
        }

        public ObservableCollection<LogentriesbyStages> StagesAllChangeLogList
        {
            get { return stagesAllChangeLogList; }
            set
            {
                stagesAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StagesAllChangeLogList"));
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
        public AddEditWorkStagesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AddEditWorkStagesViewModel()..."), category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                CheckedCopyNameDescriptionCommand = new DelegateCommand<object>(CheckedCopyNameDescription);
                ChangeDescriptionCommand = new DelegateCommand<object>(SetDescriptionToLanguage);
                ChangeNameCommand = new DelegateCommand<EditValueChangingEventArgs>(SetNameToLanguage);
                ChangeLanguageCommand = new DelegateCommand<object>(RetrieveNameDescriptionByLanguge);
                ChangePlantCommand = new DelegateCommand<object>(ChangePlantCommandAction);
                StagesChangeLogList = new ObservableCollection<LogentriesbyStages>();
                
                GeosApplication.Instance.Logger.Log(string.Format("Method AddEditWorkStagesViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditWorkStagesViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion



        #region methods
        public void EditInit(WorkStages SelectedRow)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][18/12/2023]
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                Init();
                WorkStages temp = (ERMService.GetWorkStagesByIdStages_V2350(Convert.ToUInt64(SelectedRow.IdStage)));

                ClonedWorkStage = (WorkStages)temp.Clone();
                IdStage = Convert.ToInt32(temp.IdStage);
                Code = temp.Code;
                Name = temp.Name;

                PrevCode = temp.Code;

                Name_en = temp.Name;
                Name_es = temp.Name_es;
                Name_fr = temp.Name_fr;
                Name_pt = temp.Name_pt;
                Name_ro = temp.Name_ro;
                Name_ru = temp.Name_ru;
                Name_zh = temp.Name_zh;
                #region GEOS2-3954 Time format HH:MM:SS
                Description = temp.Description == null ? "" : temp.Description.Trim();
                Description_en = temp.Description == null ? "" : temp.Description.Trim();
                Description_es = temp.Description_es == null ? "" : temp.Description_es.Trim();
                Description_fr = temp.Description_fr == null ? "" : temp.Description_fr.Trim();
                Description_pt = temp.Description_pt == null ? "" : temp.Description_pt.Trim();
                Description_ro = temp.Description_ro == null ? "" : temp.Description_ro.Trim();
                Description_ru = temp.Description_ru == null ? "" : temp.Description_ru.Trim();
                Description_zh = temp.Description_zh == null ? "" : temp.Description_zh.Trim();
               
                if (Description_en == Description_es && Description_en == Description_fr && Description_en == Description_pt && Description_en == Description_ro && Description_en == Description_ru && Description_en == Description_zh &&
                   Name_en == Name_es && Name_en == Name_fr && Name_en == Name_pt && Name_en == Name_ro && Name_en == Name_ru && Name_en == Name_zh)
                {
                    IsCheckedCopyNameAndDescription = true;
                }
                else
                {
                    IsCheckedCopyNameAndDescription = false;
                }
                //Sequence
                SequenceList.Where(x => x.IdStage == temp.IdStage).ToList().ForEach(a => a.IsSequenceExists = false);
                if (temp.Sequence == "0")
                {
                    Sequence = SequenceList.FirstOrDefault(x => x.Sequence == "---");
                }
                else
                {
                    Sequence = SequenceList.FirstOrDefault(x => x.Sequence == temp.Sequence);
                }


                //Plant
                SelectedPlant = new List<object>();
                if (!string.IsNullOrEmpty(temp.ActiveInPlants))
                {
                    List<string> PlantIDlist = temp.ActiveInPlants.Split(',').ToList();
                    foreach (var item in PlantIDlist)
                    {

                        UInt32 plantid = Convert.ToUInt32(item);
                        SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == plantid));
                    }

                    SelectedPlantold = new List<object>();
                    SelectedPlantold = SelectedPlant;
                }
                else
                {
                    SelectedPlant.AddRange(PlantList);
                    SelectedPlantold = new List<object>();
                    SelectedPlantold = SelectedPlant;
                }


                if (temp.Status != null)
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == temp.Status.IdLookupValue);
                    SelectedOriginalStatus = SelectedStatus;
                }
                else
                {
                    SelectedStatus = StatusList.FirstOrDefault(x => x.IdLookupValue == 1537);
                    ClonedWorkStage.IdStatus = 1537;
                }



                if (temp.ActivateRework == 1)
                {
                    ActivateRework = true;
                }
                else if (temp.ActivateRework == 0)
                {
                    ActivateRework = false;
                }
                if (temp.IsProductionStage == "YES")
                {
                    IsProductionStage = true;
                }
                else if (temp.IsProductionStage == "NO")
                {
                    IsProductionStage = false;
                }

                #endregion
                // [GEOS2-3908][Rupali Sarode][16-1-2023]
                StagesAllChangeLogList = new ObservableCollection<LogentriesbyStages>(temp.LstStagesChangeLogList.OrderByDescending(x => x.IdLogEntryByStages).ToList());
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Method EditInit()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message + GeosApplication.createExceptionDetailsMsg(ex)), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init()..."), category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][18/12/2023]
                AddLanguages();
                FillStatusList();
                GetPlants();
                FillSequenceList();
                IsCheckedCopyNameAndDescription = true;
                CodeList = new List<string>();
                foreach (WorkStages cCode in SequenceList)
                {
                    CodeList.Add(cCode.Code);
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
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

        private void GetPlants()
        {
            try
            {
                //if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);
                if (PlantList == null || PlantList.Count == 0)
                {
                    PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                   
                }
                // [Rupali Sarode][12/1/2023]
                PlantList.Insert(0, new Site { IdSite = 0, Name = "---" });

                List<Company> plantOwners = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.ConnectPlantId));

                List<Site> PlantList1 = new List<Site>();
                foreach (Company item in plantOwners)
                {

                    UInt32 plantid = Convert.ToUInt32(item.ConnectPlantId);
                    PlantList1 = PlantList.Where(x => x.IdSite == plantid).ToList();
                    if (SelectedPlant == null)
                        SelectedPlant = new List<object>();

                    SelectedPlant.Add(PlantList.FirstOrDefault(a => a.IdSite == 0));
                    if (SelectedPlantold == null)
                        SelectedPlantold = new List<object>();
                    SelectedPlantold = SelectedPlant;
                }

                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SetNameToLanguage() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
        private void ChangeCheckAllLanguagesCommandAction()
        {
            if (oldIsCheckedAllLanguages != null && oldIsCheckedAllLanguages != IsCheckedAllLanguages)
            { oldIsCheckedAllLanguages = IsCheckedAllLanguages; IsEnabledSaveButton = true; }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                InformationError = null;
                allowValidation = true;
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("Code"));
                PropertyChanged(this, new PropertyChangedEventArgs("Sequence"));
                PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPlant"));

                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = " ";
                if (error != null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(Code) && IsProductionStage == true)
                {

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CodeNull").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (!string.IsNullOrEmpty(Code) && IsProductionStage == true)
                {
                    int tempCodeAlreadyExists = TempSequenceList.Where(x => x.Code == Code && x.IdStage != IdStage).Count();
                    if (tempCodeAlreadyExists > 0)
                    {

                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("CodeAlreadyExists").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                }
                

                string TempPlantID = string.Empty;
                if (SelectedPlant.Count > 0)
                {
                    //if (SelectedPlant.Count != PlantList.Count)
                    if (SelectedPlant.Count != PlantList.Count - 1)
                    {
                        foreach (Site item in selectedPlant)
                        {
                            if (item.Name != "---")
                            {
                                if (string.IsNullOrEmpty(TempPlantID))
                                {
                                    TempPlantID = Convert.ToString(item.IdSite);
                                }
                                else
                                {
                                    TempPlantID = TempPlantID + "," + Convert.ToString(item.IdSite);
                                }
                            }

                        }

                    }

                }
                UpdateWorkStages = new WorkStages();
                if (IsNew)
                {
                    NewWorkStages = new WorkStages();
                    if (Code != null)
                    {
                        NewWorkStages.Code = Code == null ? "" : Code.Trim();
                    }
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        NewWorkStages.Name = Name == null ? "" : Name.Trim();
                        NewWorkStages.Name_es = Name == null ? "" : Name.Trim();
                        NewWorkStages.Name_fr = Name == null ? "" : Name.Trim();
                        NewWorkStages.Name_pt = Name == null ? "" : Name.Trim();
                        NewWorkStages.Name_ro = Name == null ? "" : Name.Trim();
                        NewWorkStages.Name_ru = Name == null ? "" : Name.Trim();
                        NewWorkStages.Name_zh = Name == null ? "" : Name.Trim();

                        NewWorkStages.Description = Description == null ? "" : Description.Trim();
                        NewWorkStages.Description_es = Description == null ? "" : Description.Trim();
                        NewWorkStages.Description_fr = Description == null ? "" : Description.Trim();
                        NewWorkStages.Description_pt = Description == null ? "" : Description.Trim();
                        NewWorkStages.Description_ro = Description == null ? "" : Description.Trim();
                        NewWorkStages.Description_ru = Description == null ? "" : Description.Trim();
                        NewWorkStages.Description_zh = Description == null ? "" : Description.Trim();

                    }
                    else
                    {
                        NewWorkStages.Description = Description_en == null ? "" : Description_en.Trim();
                        NewWorkStages.Description_es = Description_es == null ? "" : Description_es.Trim();
                        NewWorkStages.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                        NewWorkStages.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                        NewWorkStages.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                        NewWorkStages.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                        NewWorkStages.Description_zh = Description_zh == null ? "" : Description_zh.Trim();

                        NewWorkStages.Name = Name_en == null ? "" : Name_en.Trim();
                        NewWorkStages.Name_es = Name_es == null ? "" : Name_es.Trim();
                        NewWorkStages.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                        NewWorkStages.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                        NewWorkStages.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                        NewWorkStages.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                        NewWorkStages.Name_zh = Name_zh == null ? "" : Name_zh.Trim();
                    }
                    NewWorkStages.Status = SelectedStatus;
                    NewWorkStages.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);
                    if (Convert.ToString(((WorkStages)Sequence).Sequence) == "---")
                    {
                        NewWorkStages.Sequence = "0";
                    }
                    else
                    {
                        NewWorkStages.Sequence = Convert.ToString(((WorkStages)Sequence).Sequence);
                    }
                    NewWorkStages.ActiveInPlants = TempPlantID;
                    if (ActivateRework == true)
                    {
                        NewWorkStages.ActivateRework = 1;
                    }
                    else if (ActivateRework == false)
                    {
                        NewWorkStages.ActivateRework = 0;
                    }
                    if (IsProductionStage == true)
                    {
                        NewWorkStages.IsProductionStage = "1";
                    }
                    else if (IsProductionStage == false)
                    {
                        NewWorkStages.IsProductionStage = "0";
                    }

                    //[GEOS2-3908][Rupali Sarode][16-1-2023]
                    AddChangedStagesLogDetails("New");
                    NewWorkStages.LstStagesChangeLogList = StagesChangeLogList.ToList();
                    //

                    NewWorkStages.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    NewWorkStages = ERMService.AddWorkStage_V2350(NewWorkStages);

                    IsSave = true;
                    if (IsSave)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkStagesAddSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    }
                    RequestClose(null, null);
                }
                else
                {
                    UpdateWorkStages.IdStage = Convert.ToInt32(IdStage);
                    if (Code != null)
                    {
                        UpdateWorkStages.Code = Code == null ? "" : Code.Trim();
                    }
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        IsFromInformation = true;
                        UpdateWorkStages.Description = Description == null ? "" : Description.Trim();
                        UpdateWorkStages.Description_es = Description == null ? "" : Description.Trim();
                        UpdateWorkStages.Description_fr = Description == null ? "" : Description.Trim();
                        UpdateWorkStages.Description_pt = Description == null ? "" : Description.Trim();
                        UpdateWorkStages.Description_ro = Description == null ? "" : Description.Trim();
                        UpdateWorkStages.Description_ru = Description == null ? "" : Description.Trim();
                        UpdateWorkStages.Description_zh = Description == null ? "" : Description.Trim();

                        UpdateWorkStages.Name = Name == null ? "" : Name.Trim();
                        UpdateWorkStages.Name_es = Name == null ? "" : Name.Trim();
                        UpdateWorkStages.Name_fr = Name == null ? "" : Name.Trim();
                        UpdateWorkStages.Name_pt = Name == null ? "" : Name.Trim();
                        UpdateWorkStages.Name_ro = Name == null ? "" : Name.Trim();
                        UpdateWorkStages.Name_ru = Name == null ? "" : Name.Trim();
                        UpdateWorkStages.Name_zh = Name == null ? "" : Name.Trim();
                    }
                    else
                    {
                        IsFromInformation = true;
                        UpdateWorkStages.Description = Description_en == null ? "" : Description_en.Trim();
                        UpdateWorkStages.Description_es = Description_es == null ? "" : Description_es.Trim();
                        UpdateWorkStages.Description_fr = Description_fr == null ? "" : Description_fr.Trim();
                        UpdateWorkStages.Description_pt = Description_pt == null ? "" : Description_pt.Trim();
                        UpdateWorkStages.Description_ro = Description_ro == null ? "" : Description_ro.Trim();
                        UpdateWorkStages.Description_ru = Description_ru == null ? "" : Description_ru.Trim();
                        UpdateWorkStages.Description_zh = Description_zh == null ? "" : Description_zh.Trim();

                        UpdateWorkStages.Name = Name_en == null ? "" : Name_en.Trim();
                        UpdateWorkStages.Name_es = Name_es == null ? "" : Name_es.Trim();
                        UpdateWorkStages.Name_fr = Name_fr == null ? "" : Name_fr.Trim();
                        UpdateWorkStages.Name_pt = Name_pt == null ? "" : Name_pt.Trim();
                        UpdateWorkStages.Name_ro = Name_ro == null ? "" : Name_ro.Trim();
                        UpdateWorkStages.Name_ru = Name_ru == null ? "" : Name_ru.Trim();
                        UpdateWorkStages.Name_zh = Name_zh == null ? "" : Name_zh.Trim();

                    }

                    UpdateWorkStages.Status = SelectedStatus;
                    UpdateWorkStages.IdStatus = Convert.ToUInt32(SelectedStatus.IdLookupValue);
                    if (Convert.ToString(((WorkStages)Sequence).Sequence) == "---")
                    {
                        UpdateWorkStages.Sequence = "0";
                    }
                    else
                    {
                        UpdateWorkStages.Sequence = Convert.ToString(((WorkStages)Sequence).Sequence);
                    }
                    UpdateWorkStages.ActiveInPlants = TempPlantID;
                    if (ActivateRework == true)
                    {
                        UpdateWorkStages.ActivateRework = 1;
                    }
                    else if (ActivateRework == false)
                    {
                        UpdateWorkStages.ActivateRework = 0;
                    }
                    if (IsProductionStage == true)
                    {
                        UpdateWorkStages.IsProductionStage = "1";
                    }
                    else if (IsProductionStage == false)
                    {
                        UpdateWorkStages.IsProductionStage = "0";
                    }

                    //[GEOS2-3908][Rupali Sarode][16-1-2023]
                    AddChangedStagesLogDetails("Update");
                    UpdateWorkStages.LstStagesChangeLogList = StagesChangeLogList.ToList();
                    //

                    UpdateWorkStages.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;

                    bool result = ERMService.UpdateWorkStage_V2350(UpdateWorkStages);

                    if (result == true)
                    {
                        try
                        {
                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkStagesUpdateMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {

                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                        }

                    }
                    IsSave = true;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                    RequestClose(null, null);

                }
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void FillSequenceList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSequenceList()...", category: Category.Info, priority: Priority.Low);


                Int32 maxSequence = 0;
                Int32 minSequence = 0;

                TempSequenceList = new ObservableCollection<WorkStages>(ERMService.GetAllWorkStageSequence_V2350());
                maxSequence = TempSequenceList.Max(x => Convert.ToInt32(x.Sequence)) + 1;
                SequenceList = new ObservableCollection<WorkStages>();

                var ZeroSequence = TempSequenceList.Where(x => Convert.ToUInt32(x.Sequence) == 0).FirstOrDefault();
                if (ZeroSequence == null)
                    SequenceList.Insert(0, new WorkStages { Sequence = "---", IsSequenceExists = false });
                else
                {
                    SequenceList.Insert(0, new WorkStages { Sequence = "---", Name = ZeroSequence.Name, Code = ZeroSequence.Code, IdStage = ZeroSequence.IdStage, IsSequenceExists = true });
                }

                for (int i = 1; i <= maxSequence; i++)
                {

                    WorkStages tWorkStages = new WorkStages();

                    var tSequence = TempSequenceList.Where(x => Convert.ToInt32(x.Sequence) == i).FirstOrDefault();
                    if (tSequence != null)
                    {
                        //if (Convert.ToUInt32(tSequence.Sequence) == 0)
                        //    tWorkStages.Sequence = "---";
                        //else
                        tWorkStages.Sequence = Convert.ToString(tSequence.Sequence);

                        tWorkStages.IdStage = tSequence.IdStage;
                        tWorkStages.Name = tSequence.Name;
                        tWorkStages.Code = tSequence.Code;
                        tWorkStages.IsSequenceExists = true;
                    }
                    else
                    {

                        //if (!TempSequenceList.Any(a => a.KeyName.Equals(Module.KeyName)))
                        tWorkStages.Sequence = Convert.ToString(i);
                        tWorkStages.IdStage = 0;
                        tWorkStages.Name = "";
                        tWorkStages.Code = "";
                        tWorkStages.IsSequenceExists = false;
                    }


                    SequenceList.Add(tWorkStages);
                }

                Sequence = SequenceList.Where(x => x.Sequence == Convert.ToString(maxSequence)).FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillSequenceList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillSequenceList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
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

                string error =
                   me[BindableBase.GetPropertyName(() => Code)] +
                    me[BindableBase.GetPropertyName(() => Sequence)] +
                    me[BindableBase.GetPropertyName(() => Name)] +
                    me[BindableBase.GetPropertyName(() => SelectedPlant)];


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
                string code = BindableBase.GetPropertyName(() => Code);
                string sequence = BindableBase.GetPropertyName(() => Sequence);
                string isProductionStage = BindableBase.GetPropertyName(() => IsProductionStage);
                string name = BindableBase.GetPropertyName(() => Name);
                string selectedPlant = BindableBase.GetPropertyName(() => SelectedPlant);
                
                if (columnName == code)
                {
                    return AddEditWorkStageValidation.GetErrorMessage(code, Code, IsProductionStage, CodeList, PrevCode, SelectedPlant);
                }
                if (columnName == sequence)
                {
                    return AddEditWorkStageValidation.GetErrorMessage(sequence, Sequence, IsProductionStage, CodeList, PrevCode, SelectedPlant);
                }
                if (columnName == name)
                {
                    return AddEditWorkStageValidation.GetErrorMessage(name, Name, IsProductionStage, CodeList, PrevCode, SelectedPlant);
                }
                if (columnName == selectedPlant)
                {
                    return AddEditWorkStageValidation.GetErrorMessage(selectedPlant, SelectedPlant, IsProductionStage, CodeList, PrevCode, SelectedPlant);
                }

                return null;
            }
        }
        #endregion


        private void ChangePlantCommandAction(object obj)
        {
            try
            {

            }
            catch (Exception ex)
            { }
        }

        #region [GEOS2-3908][Rupali Sarode][16-1-2023]
        public void AddChangedStagesLogDetails(string LogValue)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedStagesLogDetails()...", category: Category.Info, priority: Priority.Low);
                if (LogValue == "New")
                {

                    string log = "Stage " + Convert.ToString(Code) + " has been added.";
                    StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });


                }
                else if (LogValue == "Update")
                {
                    #region Code
                    if (Convert.ToString(ClonedWorkStage.Code) != Convert.ToString(Code))
                    {
                        string log = string.Empty;


                        if (!string.IsNullOrEmpty(ClonedWorkStage.Code) && !string.IsNullOrEmpty(Code))
                        {
                            if (Convert.ToString(ClonedWorkStage.Code) != "---" && Convert.ToString(Code) != "---")
                            {
                                log = "The Code has been changed from " + Convert.ToString(ClonedWorkStage.Code) + " to " + Convert.ToString(Code) + ".";
                            }
                            else if (Convert.ToString(ClonedWorkStage.Code) == "---" && Convert.ToString(Code) != "---")
                            {
                                log = "The Code has been changed from None to " + Convert.ToString(Code) + ".";
                            }
                            else if (Convert.ToString(ClonedWorkStage.Code) != "---" && Convert.ToString(Code) == "---")
                            {
                                log = "The Code has been changed from " + Convert.ToString(ClonedWorkStage.Code) + " to None.";
                            }

                        }
                        else if (string.IsNullOrEmpty(ClonedWorkStage.Code) && !string.IsNullOrEmpty(Code))
                        {
                            if(Convert.ToString(Code)!="---")
                            {
                                log = "The Code has been changed from None to " + Convert.ToString(Code) + ".";
                            }  
                        }
                        else if (!string.IsNullOrEmpty(ClonedWorkStage.Code) && string.IsNullOrEmpty(Code))
                        {
                            if(Convert.ToString(ClonedWorkStage.Code) != "---")
                            {
                                log = "The Code has been changed from " + Convert.ToString(ClonedWorkStage.Code) + " to None.";
                            }
                            
                        }
                        if (!string.IsNullOrEmpty(log))
                        {
                            StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                        }
                    }
                    #endregion


                    #region Update name and Description gulab lakade
                    #region Name
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        if ((Name != ClonedWorkStage.Name_es || Name != ClonedWorkStage.Name_fr || Name != ClonedWorkStage.Name_pt || Name != ClonedWorkStage.Name_ro || Name != ClonedWorkStage.Name_ru || Name != ClonedWorkStage.Name_zh))
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ClonedWorkStage.Name))
                            {
                                log = "The Name for all language has been changed from " + Convert.ToString(ClonedWorkStage.Name) + " to " + Convert.ToString(Name) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ClonedWorkStage.Name))
                            {
                                log = "The Name for all language has been changed from " + Convert.ToString(ClonedWorkStage.Name) + " to None.";
                            }
                            else
                                if (!string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(ClonedWorkStage.Name))
                            {
                                log = "The Name for all language has been changed from None to " + Convert.ToString(Name) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }

                        }

                        //}

                    }
                    else
                    {
                        if (Name != ClonedWorkStage.Name)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(ClonedWorkStage.Name))
                            {
                                log = "The Name EN has been changed from " + Convert.ToString(ClonedWorkStage.Name) + " to " + Convert.ToString(Name) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name))
                            {
                                log = "The Name EN has been changed from " + Convert.ToString(ClonedWorkStage.Name) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name))
                            {
                                log = "The Name EN has been changed from None to " + Convert.ToString(Name) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Name_es != ClonedWorkStage.Name_es)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_es) && !string.IsNullOrEmpty(ClonedWorkStage.Name_es))
                            {
                                log = "The Name ES has been changed from " + Convert.ToString(ClonedWorkStage.Name_es) + " to " + Convert.ToString(Name_es) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_es))
                            {
                                log = "The Name ES has been changed from " + Convert.ToString(ClonedWorkStage.Name_es) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name_es))
                            {
                                log = "The Name ES has been changed from None to " + Convert.ToString(Name_es) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Name_fr != ClonedWorkStage.Name_fr)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_fr) && !string.IsNullOrEmpty(ClonedWorkStage.Name_fr))
                            {
                                log = "The Name FR has been changed from " + Convert.ToString(ClonedWorkStage.Name_fr) + " to " + Convert.ToString(Name_fr) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_fr))
                            {
                                log = "The Name FR has been changed from " + Convert.ToString(ClonedWorkStage.Name_fr) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name_fr))
                            {
                                log = "The Name FR has been changed from None to " + Convert.ToString(Name_fr) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Name_pt != ClonedWorkStage.Name_pt)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_pt) && !string.IsNullOrEmpty(ClonedWorkStage.Name_pt))
                            {
                                log = "The Name PT has been changed from " + Convert.ToString(ClonedWorkStage.Name_pt) + " to " + Convert.ToString(Name_pt) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_pt))
                            {
                                log = "The Name PT has been changed from " + Convert.ToString(ClonedWorkStage.Name_pt) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name_pt))
                            {
                                log = "The Name PT has been changed from None to " + Convert.ToString(Name_pt) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Name_ro != ClonedWorkStage.Name_ro)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_ro) && !string.IsNullOrEmpty(ClonedWorkStage.Name_ro))
                            {
                                log = "The Name RO has been changed from " + Convert.ToString(ClonedWorkStage.Name_ro) + " to " + Convert.ToString(Name_ro) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_ro))
                            {
                                log = "The Name RO has been changed from " + Convert.ToString(ClonedWorkStage.Name_ro) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name_ro))
                            {
                                log = "The Name RO has been changed from None to " + Convert.ToString(Name_ro) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });

                            }
                        }

                        if (Name_ru != ClonedWorkStage.Name_ru)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_ru) && !string.IsNullOrEmpty(ClonedWorkStage.Name_ru))
                            {
                                log = "The Name RU has been changed from " + Convert.ToString(ClonedWorkStage.Name_ru) + " to " + Convert.ToString(Name_ru) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_ru))
                            {
                                log = "The Name RU has been changed from " + Convert.ToString(ClonedWorkStage.Name_ru) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name_ru))
                            {
                                log = "The Name RU has been changed from None to " + Convert.ToString(Name_ru) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Name_zh != ClonedWorkStage.Name_zh)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Name_zh) && !string.IsNullOrEmpty(ClonedWorkStage.Name_zh))
                            {
                                log = "The Name ZH has been changed from " + Convert.ToString(ClonedWorkStage.Name_zh) + " to " + Convert.ToString(Name_zh) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Name_zh))
                            {
                                log = "The Name ZH has been changed from " + Convert.ToString(ClonedWorkStage.Name_zh) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Name_zh))
                            {
                                log = "The Name ZH has been changed from None to " + Convert.ToString(Name_zh) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }
                    }
                    #endregion

                    #region Description
                    if (IsCheckedCopyNameAndDescription == true)
                    {
                        if (Description != ClonedWorkStage.Description_es || Description != ClonedWorkStage.Description_fr || Description != ClonedWorkStage.Description_pt || Description != ClonedWorkStage.Description_ro || Description != ClonedWorkStage.Description_ru || Description != ClonedWorkStage.Description_zh)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkStage.Description)) && !string.IsNullOrEmpty(Convert.ToString(Description)))
                            {
                                log = "The Description for all language has been changed from " + Convert.ToString(ClonedWorkStage.Description) + " to " + Convert.ToString(Description) + ".";
                            }
                            else
                            if (string.IsNullOrEmpty(Convert.ToString(ClonedWorkStage.Description)) && !string.IsNullOrEmpty(Convert.ToString(Description)))
                            {
                                log = "The Description for all language has been changed from None to " + Convert.ToString(Description) + ".";
                            }
                            else
                                if (!string.IsNullOrEmpty(Convert.ToString(ClonedWorkStage.Description)) && string.IsNullOrEmpty(Convert.ToString(Description)))
                            {
                                log = "The Description for all language has been changed from " + Convert.ToString(ClonedWorkStage.Description) + " to None.";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                    }
                    else
                    {
                        if (Description_en != ClonedWorkStage.Description)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(ClonedWorkStage.Description))
                            {
                                log = "The Description EN has been changed from " + Convert.ToString(ClonedWorkStage.Description) + " to " + Convert.ToString(Description) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description))
                            {
                                log = "The Description EN has been changed from " + Convert.ToString(ClonedWorkStage.Description) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description))
                            {
                                log = "The Description EN has been changed from None to " + Convert.ToString(Description) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Description_es != ClonedWorkStage.Description_es)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_es) && !string.IsNullOrEmpty(ClonedWorkStage.Description_es))
                            {
                                log = "The Description ES has been changed from " + Convert.ToString(ClonedWorkStage.Description_es) + " to " + Convert.ToString(Description_es) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_es))
                            {
                                log = "The Description ES has been changed from " + Convert.ToString(ClonedWorkStage.Description_es) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description_es))
                            {
                                log = "The Description ES has been changed from None to " + Convert.ToString(Description_es) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Description_fr != ClonedWorkStage.Description_fr)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_fr) && !string.IsNullOrEmpty(ClonedWorkStage.Description_fr))
                            {
                                log = "The Description FR has been changed from " + Convert.ToString(ClonedWorkStage.Description_fr) + " to " + Convert.ToString(Description_fr) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_fr))
                            {
                                log = "The Description FR has been changed from " + Convert.ToString(ClonedWorkStage.Description_fr) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description_fr))
                            {
                                log = "The Description FR has been changed from None to " + Convert.ToString(Description_fr) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }
                        if (Description_pt != ClonedWorkStage.Description_pt)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_pt) && !string.IsNullOrEmpty(ClonedWorkStage.Description_pt))
                            {
                                log = "The Description PT has been changed from " + Convert.ToString(ClonedWorkStage.Description_pt) + " to " + Convert.ToString(Description_pt) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_pt))
                            {
                                log = "The Description PT has been changed from " + Convert.ToString(ClonedWorkStage.Description_pt) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description_pt))
                            {
                                log = "The Description PT has been changed from None to " + Convert.ToString(Description_pt) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Description_ro != ClonedWorkStage.Description_ro)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_ro) && !string.IsNullOrEmpty(ClonedWorkStage.Description_ro))
                            {
                                log = "The Description RO has been changed from " + Convert.ToString(ClonedWorkStage.Description_ro) + " to " + Convert.ToString(Description_ro) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_ro))
                            {
                                log = "The Description RO has been changed from " + Convert.ToString(ClonedWorkStage.Description_ro) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description_ro))
                            {
                                log = "The Description RO has been changed from None to " + Convert.ToString(Description_ro) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Description_ru != ClonedWorkStage.Description_ru)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_ru) && !string.IsNullOrEmpty(ClonedWorkStage.Description_ru))
                            {
                                log = "The Description RU has been changed from " + Convert.ToString(ClonedWorkStage.Description_ru) + " to " + Convert.ToString(Description_ru) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_ru))
                            {
                                log = "The Description RU has been changed from " + Convert.ToString(ClonedWorkStage.Description_ru) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description_ru))
                            {
                                log = "The Description RU has been changed from None to " + Convert.ToString(Description_ru) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }

                        if (Description_zh != ClonedWorkStage.Description_zh)
                        {
                            string log = string.Empty;
                            if (!string.IsNullOrEmpty(Description_zh) && !string.IsNullOrEmpty(ClonedWorkStage.Description_zh))
                            {
                                log = "The Description ZH has been changed from " + Convert.ToString(ClonedWorkStage.Description_zh) + " to " + Convert.ToString(Description_zh) + ".";
                            }
                            else
                                if (string.IsNullOrEmpty(Description_zh))
                            {
                                log = "The Description ZH has been changed from " + Convert.ToString(ClonedWorkStage.Description_zh) + " to None.";
                            }
                            else
                                if (string.IsNullOrEmpty(ClonedWorkStage.Description_zh))
                            {
                                log = "The Description ZH has been changed from None to " + Convert.ToString(Description_zh) + ".";
                            }
                            if (!string.IsNullOrEmpty(log))
                            {
                                StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                        }
                    }
                    #endregion

                    #endregion
                    #region Sequence
                    string NewSequence = Convert.ToString(((WorkStages)Sequence).Sequence);
                    if (Convert.ToString(ClonedWorkStage.Sequence) != Convert.ToString(NewSequence))
                    {
                        string log = string.Empty;

                        if (ClonedWorkStage.Sequence == "---")
                        {
                            log = "The Sequence has been changed from 0 to " + Convert.ToString(NewSequence) + ".";
                        }
                        else if (Convert.ToString(NewSequence) == "---")
                        {
                            log = "The Sequence has been changed from " + Convert.ToString(ClonedWorkStage.Sequence) + " to 0.";
                        }
                        else
                        {
                            log = "The Sequence has been changed from " + Convert.ToString(ClonedWorkStage.Sequence) + " to " + Convert.ToString(NewSequence) + ".";
                        }
                        if (!string.IsNullOrEmpty(log))
                        {
                            StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                        }
                    }
                    #endregion
                    #region  Plant
                    string AddPlantName = string.Empty;
                    string RemovePlantName = string.Empty;
                    
                    if (SelectedPlant.Count > 0)
                    {

                        if (SelectedPlantold.Count > 0)
                        {
                            var NewPlantlist = SelectedPlant.Where(x => !SelectedPlantold.Any(y => ((Site)y).IdSite == ((Site)x).IdSite)).ToList();
                            foreach (Site add in NewPlantlist)
                            {
                                if (string.IsNullOrEmpty(AddPlantName))
                                {
                                    if (Convert.ToString(add.Name) != "---")
                                    {
                                        AddPlantName = Convert.ToString(add.Name);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(add.Name) != "---")
                                    {
                                        AddPlantName = AddPlantName + "," + Convert.ToString(add.Name);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(AddPlantName))
                            {
                                string log = string.Empty;
                                log = "The Plant " + Convert.ToString(AddPlantName) + " has been added.";
                                if (!string.IsNullOrEmpty(log))
                                {
                                    StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }
                            var RemovePlantlist = SelectedPlantold.Where(x => !SelectedPlant.Any(y => ((Site)y).IdSite == ((Site)x).IdSite)).ToList();
                            foreach (Site remove in RemovePlantlist)
                            {
                                if (string.IsNullOrEmpty(RemovePlantName))
                                {
                                    if (Convert.ToString(remove.Name) != "---")
                                    {
                                        RemovePlantName = Convert.ToString(remove.Name);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(remove.Name) != "---")
                                    {
                                        RemovePlantName = RemovePlantName + "," + Convert.ToString(remove.Name);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(RemovePlantName))
                            {
                                string log = string.Empty;
                                log = "The Plant " + Convert.ToString(RemovePlantName) + " has been removed.";
                                if (!string.IsNullOrEmpty(log))
                                {
                                    StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }
                        }
                    }
                    

                    #endregion
                    #region Update Status gulab lakade

                    if (!string.IsNullOrEmpty(Convert.ToString(SelectedStatus.Value)) && (!string.IsNullOrEmpty(Convert.ToString(SelectedOriginalStatus.Value))) && (Convert.ToString(SelectedStatus.Value) != Convert.ToString(SelectedOriginalStatus.Value)))
                    {
                        string log = "The Status has been changed from " + Convert.ToString(Convert.ToString(SelectedOriginalStatus.Value)) + " to " + Convert.ToString(SelectedStatus.Value) + ".";
                        StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                    }
                    #endregion
                    #region ActivateRework
                    string OldActivateRework = string.Empty;
                    string NewActivateRework = string.Empty;
                    if (ClonedWorkStage.ActivateRework == 1)
                    {
                        OldActivateRework = "YES";
                    }
                    else
                    {
                        OldActivateRework = "NO";
                    }
                    if (ActivateRework == true)
                    {
                        NewActivateRework = "YES";
                    }
                    else
                    {
                        NewActivateRework = "NO";
                    }
                    if (Convert.ToString(OldActivateRework) != Convert.ToString(NewActivateRework))
                    {
                        string log = string.Empty;
                        log = "The ActivateRework has been changed from " + Convert.ToString(OldActivateRework) + " to " + Convert.ToString(NewActivateRework) + ".";

                        if (!string.IsNullOrEmpty(log))
                        {
                            StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                        }
                    }
                    #endregion
                    #region IsProductionStage
                    string NewIsProductionStage = string.Empty;
                    if (IsProductionStage == true)
                    {
                        NewIsProductionStage = "Yes";
                    }
                    else
                    {
                        NewIsProductionStage = "No";
                    }
                    if (Convert.ToString(ClonedWorkStage.IsProductionStage) != Convert.ToString(NewIsProductionStage))
                    {
                        string log = string.Empty;
                        log = "The Production Stage has been changed from " + Convert.ToString(ClonedWorkStage.IsProductionStage) + " to " + Convert.ToString(NewIsProductionStage) + ".";

                        if (!string.IsNullOrEmpty(log))
                        {
                            StagesChangeLogList.Add(new LogentriesbyStages() { IdStage = Convert.ToUInt32(IdStage), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                        }
                    }
                    #endregion


                }

                GeosApplication.Instance.Logger.Log("Method AddChangedStagesLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get error in method AddChangedStagesLogDetails()..." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
