using DevExpress.Data;
using DevExpress.Mvvm;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
   public class CPsOperationsTimeInTimetrackingViewModel: ViewModelBase, INotifyPropertyChanged
    {
        #region Service


        //IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
         IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
         //IERMService ERMService = new ERMServiceController("localhost:6699");
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event EventHandler RequestClose;




        #endregion
        #region Delcaration 
        private ObservableCollection<BandItem> bandsCPOperation = new ObservableCollection<BandItem>();
        private DataTable dataTableForGridLayout;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columnsCPOperationTime;
        private string oTCode;
        private string drawingType;
        private string item;
        private string serialNumber;
        private string workStation;
        private string cPType;
        private string totalExpectedTime;
        private ObservableCollection<Stages> getStages;
        private ObservableCollection<WorkOperationByStages> workOperationMenulist;
        List<StandardOperationsDictionaryWays> lstStandardOperationsDictionaryWaysCP;
        List<StandardOperationsDictionaryDetection> lstStandardOperationsDictionaryDetectionCP;
         TreeListControl treeListControlInstance;
        private WorkOperationByStages selectedWorkOperationMenulist;
        private ObservableCollection<CPOperationsTime> cPOperationsTimeWaysList;
        private List<ERMSOPDetection> tempWorkOperationClonedDetection = new List<ERMSOPDetection>();
        private ObservableCollection<CPOperationsTime> cPOperationsTimeDetectionList;
        ObservableCollection<WorkOperationByStages> operationOptionMenulist = new ObservableCollection<WorkOperationByStages>();
        List<StandardOperationsDictionaryOption> lstStandardOperationsDictionaryOptionCP;
        private List<ERMSOPOptions> tempWorkOperationClonedOptions = new List<ERMSOPOptions>();
        ObservableCollection<CPOperationsTime> cPOperationsTimeOptionList;
        List<StandardOperationsDictionaryModules> lstStandardOperationsDictionaryModulesCP;
        private List<object> selectedPlant;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private DataTable dttable;
        private DataTable dttableCopy;
        private List<Tuple<string, float?>> supplementsBoxMenu;
        private bool isBusy; //Aishwarya Ingale[Geos2-6078]
        private TreeListView WaysGrid;//Aishwarya Ingale[Geos2-6078]
        private TreeListView DetectionGrid;//Aishwarya Ingale[Geos2-6078]
        private TreeListView OptionsGrid;//Aishwarya Ingale[Geos2-6078]
        #endregion

        #region Property
        public ObservableCollection<BandItem> BandsCPOperation
        {
            get { return bandsCPOperation; }
            set
            {
                bandsCPOperation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BandsCPOperation"));
            }
        }

        public DataTable DataTableForGridLayout
        {
            get
            {
                return dataTableForGridLayout;
            }
            set
            {
                dataTableForGridLayout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableForGridLayout"));
            }
        }
        public ObservableCollection<Summary> TotalSummary { get; private set; }


        public ObservableCollection<Emdep.Geos.UI.Helper.Column> ColumnsCPOperationTime
        {
            get { return columnsCPOperationTime; }
            set
            {
                columnsCPOperationTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsCPOperationTime"));
            }
        }


     
        public string OTCode
        {
            get
            {
                return oTCode;
            }

            set
            {
                oTCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTCode"));
            }
        }

        public string DrawingType
        {
            get
            {
                return drawingType;
            }

            set
            {
                drawingType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DrawingType"));
            }
        }

        public string Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Item"));
            }
        }

        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                serialNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SerialNumber"));
            }
        }
        public string WorkStation
        {
            get
            {
                return workStation;
            }

            set
            {
                workStation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkStation"));
            }
        }
        public string CPType
        {
            get
            {
                return cPType;
            }

            set
            {
                cPType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPType"));
            }
        }
        public string TotalExpectedTime
        {
            get
            {
                return totalExpectedTime;
            }

            set
            {
                totalExpectedTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TotalExpectedTime"));
            }
        }
        ObservableCollection<ERMSOPModule> ModuleMenulistupdated = new ObservableCollection<ERMSOPModule>();
       
        public List<StandardOperationsDictionaryWays> LstStandardOperationsDictionaryWaysCP
        {
            get { return lstStandardOperationsDictionaryWaysCP; }
            set
            {
                lstStandardOperationsDictionaryWaysCP = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryWaysCP"));
            }
        }

        public List<StandardOperationsDictionaryDetection> LstStandardOperationsDictionaryDetectionCP
        {
            get { return lstStandardOperationsDictionaryDetectionCP; }
            set
            {
                lstStandardOperationsDictionaryDetectionCP = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryDetectionCP"));
            }
        }
        public ObservableCollection<WorkOperationByStages> WorkOperationMenulist
        {
            get
            {
                return workOperationMenulist;
            }

            set
            {
                workOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkOperationMenulist"));
            }
        }
        ObservableCollection<WorkOperationByStages> operationWayMenulist = new ObservableCollection<WorkOperationByStages>();
        private List<ERMSOPWays> tempWorkOperationClonedWays = new List<ERMSOPWays>();
        public ObservableCollection<Stages> GetStages
        {
            get
            {
                return getStages;
            }
            set
            {
                getStages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GetStages"));
            }
        }
        List<Tuple<string, float?>> allsupplementsBoxMenu;
        public List<Tuple<string, float?>> AllSupplementsBoxMenu
        {
            get { return allsupplementsBoxMenu; }
            set
            {
                allsupplementsBoxMenu = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllSupplementsBoxMenu"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsWayForSupplements;
        public ObservableCollection<TreeListColumn> ColumnsWayForSupplements
        {
            get { return columnsWayForSupplements; }
            set
            {
                columnsWayForSupplements = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsWayForSupplements"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsWayOperationTime;
        public ObservableCollection<TreeListColumn> ColumnsWayOperationTime
        {
            get { return columnsWayOperationTime; }
            set
            {
                columnsWayOperationTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsWayOperationTime"));
            }
        }
        ObservableCollection<ERMSOPWays> waysMenulist;
        public ObservableCollection<ERMSOPWays> WaysMenulist
        {
            get { return waysMenulist; }
            set
            {
                waysMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WaysMenulist"));
            }
        }
        public WorkOperationByStages SelectedWorkOperationMenulist
        {
            get { return selectedWorkOperationMenulist; }
            set
            {
                selectedWorkOperationMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkOperationMenulist"));
            }

        }
        public ObservableCollection<CPOperationsTime> CPOperationsTimeWaysList
        {
            get { return cPOperationsTimeWaysList; }
            set
            {
                cPOperationsTimeWaysList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPOperationsTimeWaysList"));
            }
        }

        private ObservableCollection<TreeListColumn> columnsDetectionForSupplements;
        public ObservableCollection<TreeListColumn> ColumnsDetectionForSupplements
        {
            get { return columnsDetectionForSupplements; }
            set
            {
                columnsDetectionForSupplements = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsDetectionForSupplements"));
            }
        }

        private ObservableCollection<TreeListColumn> columnsDetectionOperationTime;
        public ObservableCollection<TreeListColumn> ColumnsDetectionOperationTime
        {
            get { return columnsDetectionOperationTime; }
            set
            {
                columnsDetectionOperationTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsDetectionOperationTime"));
            }
        }

        ObservableCollection<ERMSOPDetection> detectionMenulist;
        public ObservableCollection<ERMSOPDetection> DetectionMenulist
        {
            get { return detectionMenulist; }
            set
            {
                detectionMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetectionMenulist"));
            }
        }
        public ObservableCollection<CPOperationsTime> CPOperationsTimeDetectionList
        {
            get { return cPOperationsTimeDetectionList; }
            set
            {
                cPOperationsTimeDetectionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPOperationsTimeDetectionList"));
            }
        }


        ObservableCollection<ERMSOPOptions> optionsMenulist;
        public ObservableCollection<ERMSOPOptions> OptionsMenulist
        {
            get { return optionsMenulist; }
            set
            {
                optionsMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OptionsMenulist"));
            }
        }

        private ObservableCollection<TreeListColumn> columnsOptionOperationTime;
        public ObservableCollection<TreeListColumn> ColumnsOptionOperationTime
        {
            get { return columnsOptionOperationTime; }
            set
            {
                columnsOptionOperationTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsOptionOperationTime"));
            }
        }
        public List<StandardOperationsDictionaryOption> LstStandardOperationsDictionaryOptionCP
        {
            get { return lstStandardOperationsDictionaryOptionCP; }
            set
            {
                lstStandardOperationsDictionaryOptionCP = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryOptionCP"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsOptionForSupplements;
        public ObservableCollection<TreeListColumn> ColumnsOptionForSupplements
        {
            get { return columnsOptionForSupplements; }
            set
            {
                columnsOptionForSupplements = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsOptionForSupplements"));
            }
        }
        public ObservableCollection<CPOperationsTime> CPOperationsTimeOptionList
        {
            get { return cPOperationsTimeOptionList; }
            set
            {
                cPOperationsTimeOptionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CPOperationsTimeOptionList"));
            }
        }

        ObservableCollection<ERMSOPModule> moduleMenulist;
        public ObservableCollection<ERMSOPModule> ModuleMenulist
        {
            get { return moduleMenulist; }
            set
            {
                moduleMenulist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleMenulist"));
            }
        }
        ObservableCollection<WorkOperationByStages> operationMenulistCloned = new ObservableCollection<WorkOperationByStages>();
        public List<StandardOperationsDictionaryModules> LstStandardOperationsDictionaryModulesCP
        {
            get { return lstStandardOperationsDictionaryModulesCP; }
            set
            {
                lstStandardOperationsDictionaryModulesCP = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionaryModulesCP"));
            }
        }

        ObservableCollection<ERMSOPModule> moduleMenulistCloned;
        public ObservableCollection<ERMSOPModule> ModuleMenulistCloned
        {
            get { return moduleMenulistCloned; }
            set
            {
                moduleMenulistCloned = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleMenulistCloned"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsModuleForSupplements;
        public ObservableCollection<TreeListColumn> ColumnsModuleForSupplements
        {
            get { return columnsModuleForSupplements; }
            set
            {
                columnsModuleForSupplements = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsModuleForSupplements"));
            }
        }
        private ObservableCollection<TreeListColumn> columnsModuleOperationTime;
        public ObservableCollection<TreeListColumn> ColumnsModuleOperationTime
        {
            get { return columnsModuleOperationTime; }
            set
            {
                columnsModuleOperationTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColumnsModuleOperationTime"));
            }
        }

        public List<object> SelectedPlant
        {
            get
            {
                return selectedPlant;
            }

            set
            {
                selectedPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlant"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
      
        public DataTable DtTableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtTableCopy"));
            }
        }

        private List<LookupValue> supplements;
        private List<StandardOperationsDictionarySupplement> lstStandardOperationsDictionarySupplement;

        public List<StandardOperationsDictionarySupplement> LstStandardOperationsDictionarySupplement
        {
            get { return lstStandardOperationsDictionarySupplement; }
            set
            {
                lstStandardOperationsDictionarySupplement = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstStandardOperationsDictionarySupplement"));
            }
        }
        public DataTable Dttable
        {
            get
            {
              //  IsInformationVisible = true;
                return dttable;
            }
            set
            {
                //IsInformationVisible = true;
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        
        public List<Tuple<string, float?>> SupplementsBoxMenu
        {
            get { return supplementsBoxMenu; }
            set
            {
                supplementsBoxMenu = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupplementsBoxMenu"));
            }
        }

        private UInt64 idCPType;
            public UInt64 IdCPType
        {
            get
            {

                return idCPType;
            }
            set
            {

                idCPType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCPType"));
            }
        }

        public virtual bool DialogResult { get; protected set; }//Aishwarya Ingale[Geos2-6078]
        public virtual string ResultFileName { get; protected set; }//Aishwarya Ingale[Geos2-6078]

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        } //Aishwarya Ingale[Geos2-6078]

        #endregion
        #region ICommands

        public ICommand OptionCustomSummaryCommand { get; set; }

        public ICommand ExportCpsOperationCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }

        #endregion

        #region Constructor
        public CPsOperationsTimeInTimetrackingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditStandardOperationsDictionaryViewModel ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                ExportCpsOperationCommand = new DevExpress.Mvvm.DelegateCommand<object>(ExportCpsOperationCommandAction);//Aishwarya Ingale[Geos2-6078]
                CloseButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                // OptionCustomSummaryCommand = new DelegateCommand<object>(OptionCustomSummaryCommandAction);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor AddEditStandardOperationsDictionaryViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor AddEditStandardOperationsDictionaryViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method

        public void Init(List<StandardOperationsDictionaryModules> LstStandardOperationsDictionaryModules,List<StandardOperationsDictionaryWays> LstStandardOperationsDictionaryWays, List<StandardOperationsDictionaryDetection> LstStandardOperationsDictionaryDetection,List<StandardOperationsDictionaryOption> LstStandardOperationsDictionaryOption, string IdStage, double TempDesignValue,string RoleValue, string IdCP)
        {
            try
            {
               // IdCPType = 1;
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                LstStandardOperationsDictionaryModulesCP = LstStandardOperationsDictionaryModules;
                LstStandardOperationsDictionaryWaysCP = LstStandardOperationsDictionaryWays;
                LstStandardOperationsDictionaryDetectionCP = LstStandardOperationsDictionaryDetection;
                LstStandardOperationsDictionaryOptionCP = LstStandardOperationsDictionaryOption;
                FillWorkOperationsStages(IdStage, IdCP); //[GEOS2-6835][dhawal bhalerao][08 04 2025] : IdCP Parameter added to get the expected time from SP
                RetrieveWorkOperationsByCPTypes(TempDesignValue, RoleValue, IdStage);
                RetrieveWorkOperationsByWays(TempDesignValue, RoleValue);
                RetrieveWorkOperationsByDetection(TempDesignValue,RoleValue);
                RetrieveWorkOperationsByOption(TempDesignValue, RoleValue);
                 BandsCPOperation = new ObservableCollection<BandItem>(BandsCPOperation);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } 
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } 
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        List<ERMSOPModule> tempWorkOperationClonedModule = new List<ERMSOPModule>();
        private void RetrieveWorkOperationsByCPTypes(double TempDesignValue,string RoleValue,string IdStage)
        {
            GeosApplication.Instance.Logger.Log("Method RetrieveArticlesByCategory()...", category: Category.Info, priority: Priority.Low);
            
            
           

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
            ModuleMenulistupdated = new ObservableCollection<ERMSOPModule>();
            try
            {
                ObservableCollection<ERMSOPModule> ModuleMenulistTemp = new ObservableCollection<ERMSOPModule>();
                List<StandardOperationsDictionaryModules> tempStandardOperationsDictionaryModules = new List<StandardOperationsDictionaryModules>();
                if (IdStage == "Expected")
                {


                   tempStandardOperationsDictionaryModules = LstStandardOperationsDictionaryModulesCP;
                }
                else
                {

                    tempStandardOperationsDictionaryModules = LstStandardOperationsDictionaryModulesCP.Where(w => w.IdCpTypeNew == (long)IdCPType).ToList();
                }
                    operationMenulistCloned = operationMenulist;
                    ModuleMenulist = new ObservableCollection<ERMSOPModule>();

                    operationMenulist = new ObservableCollection<WorkOperationByStages>();
                    duplicateItemloop = 0;
                    if (tempStandardOperationsDictionaryModules.Count() > 0)
                    {
                        foreach (var eRMSOPModule in tempStandardOperationsDictionaryModules.OrderBy(x => x.ModulePosition).ToList())
                        {
                            
                            if (eRMSOPModule != null)
                            {
                                List<WorkOperationByStages> tempModuleList = WorkOperationMenulist.Where(w => w.IdworkOperationByStage == Convert.ToInt32(eRMSOPModule.IdWorkoperation) && w.Name == eRMSOPModule.WorkOperationName).ToList();//[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                                if (tempModuleList != null)
                                    foreach (WorkOperationByStages item in tempModuleList)
                                    {
                                        if (item.IdStatus == 1536)
                                        {
                                            item.IsInactiveWorkOperation = true;
                                            item.IsReadOnly = true;
                                        }
                                        WorkOperationByStages ParentName = WorkOperationMenulist.Where(w => w.ParentName != null && w.KeyName.Equals(item.ParentName)).FirstOrDefault();
                                        if (ParentName != null)
                                        {
                                            if (!operationMenulist.Any(a => a.KeyName.Equals(ParentName.KeyName)))
                                                operationMenulist.Add(ParentName);                                        
                                        }
                                        duplicateItemloop = 0;
                                        GetAllTreeList(item, WorkOperationMenulist);
                                    }

                            }

                        }
                    }
                    #region ModuleMenulist
                    
                        WorkOperationMenulist.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                        
                        #region RND
                        bool FlagAddItemModule = false; 
                        int tempmodulepos = 1;
                        foreach (WorkOperationByStages Module in WorkOperationMenulist)
                        {
                            FlagAddItemModule = true;  
                            if (operationMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(Module.IdworkOperationByStage))
                                && operationMenulist.Any(a => a.IdStage == Module.IdStage)
                                && operationMenulist.Any(a => a.Name.Trim().Equals(Module.Name.Trim()))
                                )
                            {
                                StandardOperationsDictionaryModules standardOperationsDictionaryModules = LstStandardOperationsDictionaryModulesCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(Module.IdworkOperationByStage) && Module.Parent != null
                                && w.IdCpTypeNew == Convert.ToInt32(IdCPType)).FirstOrDefault();
                                ERMSOPModule eRMSOPModule = new ERMSOPModule();

                                #region  [GEOS2-5135][Rupali Sarode][18-12-2023]
                              
                                List<WorkOperationByStages> childListInActiveDraft = operationMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(Module.KeyName)).ToList();

                                if (childListInActiveDraft.Count == 0)
                                {
                                    if (Module.IdStatus == 1535)
                                    {
                                        FlagAddItemModule = true;
                                    }
                                    else
                                        FlagAddItemModule = false;
                                }
                                else 
                                if (childListInActiveDraft.Count > 0)
                                {
                                    foreach (WorkOperationByStages objChild in childListInActiveDraft)
                                    {
                                        List<WorkOperationByStages> GrandChildListInActiveDraft = new List<WorkOperationByStages>();
                                        GrandChildListInActiveDraft = operationMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(objChild.KeyName)).ToList();
                                        if (GrandChildListInActiveDraft.Count == 0)
                                        {
                                            if (objChild.IdStatus == 1535)
                                            {
                                                FlagAddItemModule = true;
                                                break;
                                            }
                                            else
                                            {
                                                FlagAddItemModule = false;
                                            }

                                        }
                                        else
                                        {
                                            if (GrandChildListInActiveDraft.Count > 0)
                                            {
                                                if (GrandChildListInActiveDraft.Where(i => i.IdStatus == 1535).Count() > 0)
                                                {
                                                    FlagAddItemModule = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    FlagAddItemModule = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion  [GEOS2-5135][Rupali Sarode][18-12-2023]

                                if (FlagAddItemModule == true)  
                                { 

                                    if (standardOperationsDictionaryModules != null)
                                        eRMSOPModule.IdCpType = Convert.ToInt32(standardOperationsDictionaryModules.IdCpTypeNew);
                                    eRMSOPModule.Name = Module.Name;
                                    eRMSOPModule.IdworkOperationByStage = Module.IdworkOperationByStage;
                                    eRMSOPModule.IdSequence = Module.IdSequence;
                                    eRMSOPModule.IdStatus = Module.IdStatus;
                                    eRMSOPModule.Status = Module.Status;
                                    eRMSOPModule.Code = Module.Name;
                                    eRMSOPModule.Parent = Module.Parent;
                                    eRMSOPModule.IdParent = Module.IdParent;
                                    eRMSOPModule.KeyName = Module.KeyName;
                                    eRMSOPModule.ParentName = Module.ParentName;
                                    if (standardOperationsDictionaryModules != null)
                                    {
                                        eRMSOPModule.Position = Convert.ToUInt32(standardOperationsDictionaryModules.Position);
                                        eRMSOPModule.ModulePosition = Convert.ToUInt32(standardOperationsDictionaryModules.Position);//[GEOS2-5629][gulab lakade][10 07 2024]
                                        tempmodulepos++;
                                    }
                                    else
                                    {
                                        eRMSOPModule.Position = Module.Position;
                                        eRMSOPModule.ModulePosition = Module.Position; 
                                    }

                                    eRMSOPModule.WorkOperation_count = Module.WorkOperation_count;
                                    eRMSOPModule.WorkOperation_count_original = Module.WorkOperation_count_original;
                                    eRMSOPModule.NameWithWorkOperationCount = Module.NameWithWorkOperationCount;
                                   
                                    eRMSOPModule.IsReadOnly = Module.IsReadOnly;
                                    eRMSOPModule.IsInactiveWorkOperation = Module.IsInactiveWorkOperation;



                                    if (!GetStages.Any(a => a.Code.Equals(eRMSOPModule.Name)))
                                    {
                                        var tempActivity = LstStandardOperationsDictionaryModulesCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPModule.IdworkOperationByStage)
                                         && w.IdCpTypeNew == Convert.ToUInt32(eRMSOPModule.IdCpType)).Select(s => s.Activity).DefaultIfEmpty(null).FirstOrDefault();
                                        var tempObservedTime = LstStandardOperationsDictionaryModulesCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPModule.IdworkOperationByStage)
                                         && w.IdCpTypeNew == Convert.ToUInt32(eRMSOPModule.IdCpType)).Select(s => s.ObservedTime).DefaultIfEmpty(null).FirstOrDefault();


                                        #region [GEOS2-4071][Rupali Sarode][10-12-2022]

                                        var tempModule = LstStandardOperationsDictionaryModulesCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPModule.IdworkOperationByStage)
                                        && w.IdCpTypeNew == Convert.ToUInt32(eRMSOPModule.IdCpType)).FirstOrDefault();

                                        if (tempModule != null)
                                        {
                                            if (tempModule.IdWorkoperation == Convert.ToUInt64(eRMSOPModule.IdworkOperationByStage) && (tempActivity == null || tempActivity == 0))
                                                tempActivity = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.Activity).FirstOrDefault();   //[Cut  work operation not work properly.][gulab lakade][24 01 2023]

                                            if (tempModule.IdWorkoperation == Convert.ToUInt64(eRMSOPModule.IdworkOperationByStage) && (tempObservedTime == null || tempObservedTime == 0))
                                                tempObservedTime = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.ObservedTime).FirstOrDefault();  //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                                        }

                                        #endregion

                                        if (tempActivity != null)
                                            eRMSOPModule.Activity = Math.Round((double)tempActivity, 2);

                                        if (tempObservedTime != null)
                                        {
                                            eRMSOPModule.ObservedTime = Math.Round((double)tempObservedTime, 2);
                                            #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                            
                                            #region [GEOS2-5008][gulab lakade][1 11 2023]

                                            eRMSOPModule.UITempobservedTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(tempObservedTime)));
                                            #endregion
                                            #endregion
                                        }

                                        eRMSOPModule.NormalTime = Math.Round(Convert.ToDouble(eRMSOPModule.ObservedTime * eRMSOPModule.Activity / 100), 2);
                                        #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                        eRMSOPModule.UITempNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(eRMSOPModule.NormalTime));

                                        #endregion
                                        eRMSOPModule.WORemarks = Module.WORemarks == null ? "" : Module.WORemarks.Trim();

                                        var tempRemarks = LstStandardOperationsDictionaryModulesCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPModule.IdworkOperationByStage)
                                         && w.IdCpTypeNew == Convert.ToUInt32(eRMSOPModule.IdCpType)).Select(s => s.Remarks).DefaultIfEmpty(null).FirstOrDefault();

                                        eRMSOPModule.Remarks = tempRemarks == null ? "" : tempRemarks.Trim();

                                    }
                                    if (GetStages.Any(a => a != null && a.Code.Trim().Equals(Module.Name.Trim())))
                                    {
                                        eRMSOPModule.IsDeleteButton = false;
                                        eRMSOPModule.IsDeleteButtonVisibility = Visibility.Hidden;
                                        eRMSOPModule.IsReadOnly = true;
                                        eRMSOPModule.IsValidateOnTextInput = false;
                                        Module.IsCurrentWorkOperation = true;
                                    }
                                    else
                                    {
                                        Module.IsCurrentWorkOperation = true;
                                        eRMSOPModule.IsReadOnly = false;
                                        eRMSOPModule.IsValidateOnTextInput = true;
                                    }
                                    if (Module.IdStatus == 1536)
                                    {
                                        eRMSOPModule.IsInactiveWorkOperation = true;
                                        eRMSOPModule.IsReadOnly = true;
                                    }
                                   
                                    try
                                    {
                                        if (!ModuleMenulistTemp.Any(a => a.KeyName.Equals(Module.KeyName)))
                                        {

                                            ModuleMenulistTemp.Add(eRMSOPModule);

                                        }

                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                } 

                            }
                        }
                    var tempWorkOperationMenulist2 = ModuleMenulistTemp.OrderBy(x => x.ModulePosition).ToList();
                    tempWorkOperationClonedModule = new List<ERMSOPModule>();
                    tempWorkOperationClonedModule.AddRange(tempWorkOperationMenulist2);
                    ModuleMenulistTemp.Clear();
                    ModuleMenulistTemp.AddRange(tempWorkOperationMenulist2);
                    if (tempWorkOperationMenulist2 != null)
                        tempWorkOperationMenulist2.Clear();
                    if (ModuleMenulistTemp != null)
                    {
                        var tempModuleMenulistTemp = ModuleMenulistTemp.ToList();
                        for (int i = 0; i < tempModuleMenulistTemp.Count; i++)
                        {
                            if (GetStages.Any(a => a.Code.Equals(tempModuleMenulistTemp[i].Name)))
                            {
                                List<ERMSOPModule> childList = ModuleMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(tempModuleMenulistTemp[i].KeyName)).ToList();
                                if (childList.Count == 0)
                                {
                                    List<WorkOperationByStages> tempWOList = WorkOperationMenulist.Where(w => w.KeyName == tempModuleMenulistTemp[i].KeyName).ToList();
                                    if (tempWOList != null)
                                        tempWOList.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                                    ModuleMenulistTemp.Remove(tempModuleMenulistTemp[i]);
                                }
                            }
                        }
                    }
                    #endregion
                
               


                #endregion

                #region CustomSummarycalculation

                #region calculation
                #region [GEOS2-4830][gulab lakade][07 12 2023]


                if (ModuleMenulistTemp != null)
                    foreach (ERMSOPModule itemERMSOPModule in ModuleMenulistTemp)
                    {
                        WorkOperationMenulist.Where(w => w.KeyName == itemERMSOPModule.KeyName).ToList().ForEach(f => f.IsCurrentWorkOperation = true);
                        //[GEOS2-5135][Rupali Sarode][18-12-2023]
                        //var listCount = ModuleMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(itemERMSOPModule.KeyName) && w.IsInactiveWorkOperation == false).ToList();//[rdixit][03.08.2022][GEOS2-3764]
                        var listCount = ModuleMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(itemERMSOPModule.KeyName)).ToList();
                        if (listCount != null)
                            if (listCount.Count() > 0)
                            {
                                itemERMSOPModule.ObservedTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.ObservedTime)), 2);
                                itemERMSOPModule.NormalTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.NormalTime)), 2);
                                #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                itemERMSOPModule.UITempobservedTime = ConvertfloattoTimespan(Convert.ToString(listCount.Sum(s => s.ObservedTime)));
                                itemERMSOPModule.UITempNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(listCount.Sum(s => s.NormalTime)));
                                #endregion
                                itemERMSOPModule.IsReadOnly = true;
                            }
                    }
                #endregion
                if (ModuleMenulistTemp != null)
                    foreach (ERMSOPModule itemERMSOPModule in ModuleMenulistTemp)
                    {
                        if (RoleValue == "C")
                        {
                            itemERMSOPModule.UiTempStandardOperationTimeName = TimeSpan.Zero;
                        }

                        if (GetStages.Any(a => a.Code.Equals(itemERMSOPModule.Name)))
                        {
                            ObservableCollection<ERMSOPModule> ActivitySum = new ObservableCollection<ERMSOPModule>();
                            ObservableCollection<ERMSOPModule> TempActivitySum = new ObservableCollection<ERMSOPModule>();
                            int x = 1;
                            Loop:
                            x++;
                            if (x < 10)
                            {
                                if (ActivitySum.Count == 0)
                                {
                                    itemERMSOPModule.ObservedTime = 0;
                                    itemERMSOPModule.NormalTime = 0;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UITempobservedTime = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.ObservedTime));
                                    itemERMSOPModule.UITempNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(itemERMSOPModule.NormalTime));
                                    #endregion
                                    var ActivitySumList = ModuleMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(itemERMSOPModule.KeyName)).ToList();
                                    ActivitySum.AddRange(ActivitySumList);
                                }
                                else
                                {

                                }


                                goto Loop;
                            }

                            double? ObservedTime = 0;
                            double? NormalTime = 0;
                            float? StandardTime = 0;
                            float? OperationTimePlant1Value = 0;
                            float? OperationTimePlant2Value = 0;
                            float? OperationTimePlant3Value = 0;
                            float? OperationTimePlant4Value = 0;
                            float? OperationTimePlant5Value = 0;
                            var tempDistinctList = ActivitySum.Select(xa => xa.KeyName).Distinct().ToList();
                            foreach (var item in tempDistinctList)
                            {
                                if (itemERMSOPModule.ObservedTime == null)
                                    itemERMSOPModule.ObservedTime = 0;
                                double? tempObservedTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.ObservedTime).FirstOrDefault();
                                if (tempObservedTime != null)
                                {
                                    ObservedTime = Math.Round(Convert.ToDouble(ObservedTime + tempObservedTime), 2);
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UITempobservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                                    #endregion
                                }
                                if (itemERMSOPModule.NormalTime == null)
                                    itemERMSOPModule.NormalTime = 0;
                                double? tempNormalTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.NormalTime).FirstOrDefault();
                                if (tempNormalTime != null)
                                {
                                    NormalTime = Math.Round(Convert.ToDouble(NormalTime + tempNormalTime), 2);
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UITempNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(NormalTime));
                                    #endregion
                                }

                            }
                            itemERMSOPModule.ObservedTime = ObservedTime;
                            itemERMSOPModule.NormalTime = NormalTime;
                            #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                            itemERMSOPModule.UITempobservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                            itemERMSOPModule.UITempNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(itemERMSOPModule.NormalTime));
                            #endregion
                            //itemERMSOPModule.NormalTime = Math.Round(Convert.ToDouble(ObservedTime * itemERMSOPModule.Activity / 100), 2);
                            #region PlantValue


                            foreach (var item in tempDistinctList)
                            {
                                if (itemERMSOPModule.StandardOperationTime == null)
                                    itemERMSOPModule.StandardOperationTime = 0;
                                float? tempStandardTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.StandardOperationTime).FirstOrDefault();

                                if (tempStandardTime != null)
                                {
                                    StandardTime = StandardTime + tempStandardTime;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempStandardOperationTimeName = (ConvertfloattoTimespan(Convert.ToString(StandardTime)));
                                    #endregion
                                }


                                if (itemERMSOPModule.OperationTimePlant1Value == null)
                                {
                                    itemERMSOPModule.OperationTimePlant1Value = 0;

                                }

                                float? tempOperationTimePlant1Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OperationTimePlant1Value).FirstOrDefault();
                                if (tempOperationTimePlant1Value != null)
                                {
                                    OperationTimePlant1Value = OperationTimePlant1Value + tempOperationTimePlant1Value;

                                }


                                if (itemERMSOPModule.OperationTimePlant2Value == null)
                                {
                                    itemERMSOPModule.OperationTimePlant2Value = 0;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant2Value));
                                    #endregion
                                }

                                float? tempOperationTimePlant2Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OperationTimePlant2Value).FirstOrDefault();
                                if (tempOperationTimePlant2Value != null)
                                {
                                    OperationTimePlant2Value = OperationTimePlant2Value + tempOperationTimePlant2Value;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant2Value));
                                    #endregion
                                }


                                if (itemERMSOPModule.OperationTimePlant3Value == null)
                                {
                                    itemERMSOPModule.OperationTimePlant3Value = 0;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant3Value));
                                    #endregion
                                }

                                float? tempOperationTimePlant3Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OperationTimePlant3Value).FirstOrDefault();
                                if (tempOperationTimePlant3Value != null)
                                {
                                    OperationTimePlant3Value = OperationTimePlant3Value + tempOperationTimePlant3Value;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant3Value));
                                    #endregion
                                }


                                if (itemERMSOPModule.OperationTimePlant4Value == null)
                                {
                                    itemERMSOPModule.OperationTimePlant4Value = 0;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant4Value));
                                    #endregion
                                }

                                float? tempOperationTimePlant4Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OperationTimePlant4Value).FirstOrDefault();
                                if (tempOperationTimePlant4Value != null)
                                {
                                    OperationTimePlant4Value = OperationTimePlant4Value + tempOperationTimePlant4Value;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    itemERMSOPModule.UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant4Value));
                                    #endregion
                                }


                                if (itemERMSOPModule.OperationTimePlant5Value == null)
                                    itemERMSOPModule.OperationTimePlant5Value = 0;
                                float? tempOperationTimePlant5Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OperationTimePlant5Value).FirstOrDefault();
                                if (tempOperationTimePlant5Value != null)
                                    OperationTimePlant5Value = OperationTimePlant5Value + tempOperationTimePlant5Value;
                            }
                            #endregion
                            itemERMSOPModule.StandardOperationTime = (float?)Math.Round(Convert.ToDouble(StandardTime), 2);
                            #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                            
                            #endregion
                            itemERMSOPModule.OperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant1Value), 2);
                            itemERMSOPModule.OperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant2Value), 2);
                            itemERMSOPModule.OperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant3Value), 2);
                            itemERMSOPModule.OperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant4Value), 2);
                            itemERMSOPModule.OperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant5Value), 2);

                            if (!string.IsNullOrEmpty(RoleValue))
                            {
                                itemERMSOPModule.UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.StandardOperationTime));
                                if (RoleValue == "C")
                                {

                                    itemERMSOPModule.UiTempStandardOperationTimeName = TimeSpan.Zero;
                                    itemERMSOPModule.UiTempoperationTimePlant1Value = TimeSpan.Zero;
                                    itemERMSOPModule.UiTempoperationTimePlant2Value = TimeSpan.Zero;
                                    itemERMSOPModule.UiTempoperationTimePlant3Value = TimeSpan.Zero;
                                    itemERMSOPModule.UiTempoperationTimePlant4Value = TimeSpan.Zero;
                                    itemERMSOPModule.UiTempoperationTimePlant5Value = TimeSpan.Zero;
                                }
                                else
                                {
                                    itemERMSOPModule.UiTempStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(itemERMSOPModule.UiTempStandardOperationTimeName.TotalMinutes));
                                }
                            }
                            else
                            {
                                itemERMSOPModule.UiTempStandardOperationTimeName = (ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.StandardOperationTime)));
                                itemERMSOPModule.UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant1Value));
                                //itemERMSOPModule.UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant1Value)).ToString(@"mm\:ss");
                                itemERMSOPModule.UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant2Value));
                                itemERMSOPModule.UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant3Value));
                                itemERMSOPModule.UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant4Value));
                                itemERMSOPModule.UiTempoperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(itemERMSOPModule.OperationTimePlant5Value));

                            }

                           

                        }
                    }
                #endregion
                ModuleMenulist = ModuleMenulistTemp;
                #endregion


                // CustomSummarycalculation();
                if (ModuleMenulist != null)
                    ModuleMenulistCloned = ModuleMenulist;
              
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByCPTypes()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveWorkOperationsByCPTypes() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        int duplicateItemloopWay = 0;
     
        private void RetrieveWorkOperationsByWays(double TempDesignValue,string RoleValue)
        {

            GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByWays()...", category: Category.Info, priority: Priority.Low);

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
            ObservableCollection<ERMSOPWays> WaysMenulistTemp = new ObservableCollection<ERMSOPWays>();
            CPOperationsTime SelectedCPOperationsTime = new CPOperationsTime();
            if (CPOperationsTimeWaysList == null)
            {
                CPOperationsTimeWaysList = new ObservableCollection<CPOperationsTime>();
            }
            
            ModuleMenulistupdated = new ObservableCollection<ERMSOPModule>();
            try
            {

                List<StandardOperationsDictionaryWays> tempStandardOperationsDictionaryWay = new List<StandardOperationsDictionaryWays>( LstStandardOperationsDictionaryWaysCP.ToList());
                    if (tempStandardOperationsDictionaryWay.Count() > 0)
                    {
                        foreach (var eRMSOPWay in tempStandardOperationsDictionaryWay)
                        {
                            if (eRMSOPWay != null)
                            {
                                List<WorkOperationByStages> tempWayList = WorkOperationMenulist.Where(w => w.IdworkOperationByStage == Convert.ToInt32(eRMSOPWay.IdWorkoperation) && w.Name == eRMSOPWay.WorkOperationName).ToList();  //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                            if (tempWayList != null)
                                foreach (WorkOperationByStages item in tempWayList)
                                {
                                    if (item.IdStatus == 1536)
                                    {
                                        item.IsInactiveWorkOperation = true;
                                        item.IsReadOnly = true;
                                    }
                                    WorkOperationByStages ParentName = WorkOperationMenulist.Where(w => w.KeyName.Equals(item.ParentName)).FirstOrDefault();
                                    if (ParentName != null)
                                    {
                                        if (!operationWayMenulist.Any(a => a.KeyName.Equals(ParentName.KeyName)))
                                            operationWayMenulist.Add(ParentName);
                                        var newCPOperationsTimeWays = new CPOperationsTime();

                                        if (ParentName.Parent == null)
                                        {
                                            newCPOperationsTimeWays.Parent = ParentName.Name;
                                        }
                                        else
                                        {
                                            newCPOperationsTimeWays.Operation = ParentName.Name;
                                        }

                                       
                                        CPOperationsTimeWaysList.Add(newCPOperationsTimeWays);
                                    }
                                    duplicateItemloop = 0;
                                    duplicateItemloopWay = 0;
                                    GetAllTreeListWay(item, WorkOperationMenulist);
                                }

                            }

                        }
                    }
                    #region ModuleMenulist
                    WorkOperationMenulist.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                    #region [GEOS2-4830][gulab lakade][07 12 2023]
                   
                        #endregion
                        bool FlagAddItemModule = false;

                        foreach (WorkOperationByStages Module in operationWayMenulist)
                        {
                            FlagAddItemModule = true; 

                            if (operationWayMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(Module.IdworkOperationByStage))
                                && operationWayMenulist.Any(a => a.Name.Trim().Equals(Module.Name.Trim()))
                                )
                            {
                                StandardOperationsDictionaryWays standardOperationsDictionaryWay = LstStandardOperationsDictionaryWaysCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(Module.IdworkOperationByStage) && Module.Parent != null
                               ).FirstOrDefault();

                                #region  
                              
                                List<WorkOperationByStages> childListInActiveDraft = operationWayMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(Module.KeyName)).ToList();

                                if (childListInActiveDraft.Count == 0)
                                {
                                    if (Module.IdStatus == 1535)
                                    {
                                        FlagAddItemModule = true;
                                    }
                                    else
                                        FlagAddItemModule = false;
                                }
                                else 
                                if (childListInActiveDraft.Count > 0)
                                {
                                    foreach (WorkOperationByStages objChild in childListInActiveDraft)
                                    {
                                        List<WorkOperationByStages> GrandChildListInActiveDraft = new List<WorkOperationByStages>();
                                        GrandChildListInActiveDraft = operationWayMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(objChild.KeyName)).ToList();
                                        if (GrandChildListInActiveDraft.Count == 0)
                                        { 
                                            if (objChild.IdStatus == 1535)
                                            {
                                                FlagAddItemModule = true;
                                                break;
                                            }
                                            else
                                            {
                                                FlagAddItemModule = false;
                                            }

                                        }
                                        else
                                        {
                                            if (GrandChildListInActiveDraft.Count > 0)
                                            {
                                                if (GrandChildListInActiveDraft.Where(i => i.IdStatus == 1535).Count() > 0)
                                                {
                                                    FlagAddItemModule = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    FlagAddItemModule = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion  [GEOS2-5135][Rupali Sarode][18-12-2023]

                                if (FlagAddItemModule == true)  
                                {

                                    ERMSOPWays eRMSOPWay = new ERMSOPWays();
                                    if (standardOperationsDictionaryWay != null)
                                        eRMSOPWay.IdDetection = standardOperationsDictionaryWay.IdDetection;
                                    eRMSOPWay.Name = Module.Name;
                                    eRMSOPWay.IdworkOperationByStage = Module.IdworkOperationByStage;
                                    eRMSOPWay.IdSequence = Module.IdSequence;
                                    eRMSOPWay.IdStatus = Module.IdStatus;
                                    eRMSOPWay.Status = Module.Status;
                                    eRMSOPWay.Code = Module.Name;
                                    eRMSOPWay.Parent = Module.Parent;
                                    eRMSOPWay.IdParent = Module.IdParent;
                                    eRMSOPWay.KeyName = Module.KeyName;
                                    eRMSOPWay.ParentName = Module.ParentName;
                                    #region [GEOS2-5629][gulab lakade][15 07 2024]
                                    if (standardOperationsDictionaryWay != null)
                                    {
                                        eRMSOPWay.Position = Convert.ToUInt32(standardOperationsDictionaryWay.Position);
                                        eRMSOPWay.WayPosition = Convert.ToUInt32(standardOperationsDictionaryWay.Position);

                                    }
                                    else
                                    {
                                        eRMSOPWay.Position = Module.Position;
                                        eRMSOPWay.WayPosition = Module.Position; 
                                    }
                                    #endregion
                                    eRMSOPWay.WorkOperation_count = Module.WorkOperation_count;
                                    eRMSOPWay.WorkOperation_count_original = Module.WorkOperation_count_original;
                                    eRMSOPWay.NameWithWorkOperationCount = Module.NameWithWorkOperationCount;

                                    eRMSOPWay.IsInactiveWorkOperation = Module.IsInactiveWorkOperation;
                                    eRMSOPWay.IsReadOnly = Module.IsReadOnly;

                                    if (!GetStages.Any(a => a.Code.Equals(eRMSOPWay.Name)))
                                    {
                                        var tempActivity = LstStandardOperationsDictionaryWaysCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPWay.IdworkOperationByStage)
                                         && w.IdDetection == Convert.ToUInt64(eRMSOPWay.IdDetection)).Select(s => s.Activity).DefaultIfEmpty(null).FirstOrDefault();
                                        var tempObservedTime = LstStandardOperationsDictionaryWaysCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPWay.IdworkOperationByStage)
                                         && w.IdDetection == Convert.ToUInt64(eRMSOPWay.IdDetection)).Select(s => s.ObservedTime).DefaultIfEmpty(null).FirstOrDefault();
                                        #region 

                                        var tempWay = LstStandardOperationsDictionaryWaysCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPWay.IdworkOperationByStage)
                                        && w.IdDetection == Convert.ToUInt64(eRMSOPWay.IdDetection)).FirstOrDefault();

                                        if (tempWay != null)
                                        {
                                            if (tempWay.IdWorkoperation == Convert.ToUInt64(eRMSOPWay.IdworkOperationByStage) && (tempWay.Activity == null || tempWay.Activity == 0))
                                            {
                                                tempActivity = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.Activity).FirstOrDefault(); //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                                            }
                                            if (tempWay.IdWorkoperation == Convert.ToUInt64(eRMSOPWay.IdworkOperationByStage) && (tempWay.ObservedTime == null || tempWay.ObservedTime == 0))
                                            {
                                                tempObservedTime = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.ObservedTime).FirstOrDefault(); //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                                            }
                                        }
                                        #endregion
                                        if (tempActivity != null)
                                            eRMSOPWay.WayActivity = Math.Round((double)tempActivity, 2);
                                        if (tempObservedTime != null)
                                        {
                                            eRMSOPWay.WayObservedTime = Math.Round((double)tempObservedTime, 2);

                                            #region 
                                            #region 
                                            eRMSOPWay.UITempWaysObservedTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(tempObservedTime)));
                                            #endregion

                                            #endregion
                                        }
                                        eRMSOPWay.WayNormalTime = Math.Round(Convert.ToDouble(eRMSOPWay.WayObservedTime), 2);
                                         eRMSOPWay.WayNormalTime = eRMSOPWay.WayNormalTime * TempDesignValue;
                                        #region GEOS2-3954 [Rupali Sarode][15/10/2022]
                                          eRMSOPWay.UITempWaysNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(eRMSOPWay.WayNormalTime));
                                        #endregion

                                        eRMSOPWay.WORemarks = Module.WORemarks == null ? "" : Module.WORemarks.Trim();

                                        var tempRemarks = LstStandardOperationsDictionaryWaysCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPWay.IdworkOperationByStage)
                                        && w.IdDetection == Convert.ToUInt64(eRMSOPWay.IdDetection)).Select(s => s.Remarks).DefaultIfEmpty(null).FirstOrDefault();

                                        eRMSOPWay.Remarks = tempRemarks == null ? "" : tempRemarks.Trim();
                                    }


                                    if (GetStages.Any(a => a != null && a.Code.Trim().Equals(Module.Name.Trim())))
                                    {
                                        eRMSOPWay.IsDeleteButton = false;
                                        eRMSOPWay.IsDeleteButtonVisibility = Visibility.Hidden;
                                        eRMSOPWay.IsReadOnly = true;
                                        eRMSOPWay.IsValidateOnTextInput = false;
                                        Module.IsCurrentWorkOperation = true;
                                    }
                                    else
                                    {
                                        Module.IsCurrentWorkOperation = true;
                                        eRMSOPWay.IsReadOnly = false;
                                        eRMSOPWay.IsValidateOnTextInput = true;
                                    }
                                    if (Module.IdStatus == 1536)
                                    {
                                        eRMSOPWay.IsInactiveWorkOperation = true;
                                        eRMSOPWay.IsReadOnly = true;
                                    }
                               
                                    try
                                    {
                                        if (!WaysMenulistTemp.Any(a => a.KeyName.Equals(Module.KeyName)))
                                            WaysMenulistTemp.Add(eRMSOPWay);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                } 

                            }
                        }

                    #region 
                    var tempWorkOperationMenulist2 = WaysMenulistTemp.OrderBy(x => x.WayPosition).ToList();
                    tempWorkOperationClonedWays = new List<ERMSOPWays>();
                    tempWorkOperationClonedWays.AddRange(tempWorkOperationMenulist2);
                    #endregion

                    List<ERMSOPWays> tempWorkOperationWayMenulist = WaysMenulistTemp.OrderBy(x => x.WayPosition).ToList();
                    WaysMenulistTemp.Clear();
                    WaysMenulistTemp.AddRange(tempWorkOperationWayMenulist);
                    tempWorkOperationWayMenulist.Clear();
                    if (WaysMenulistTemp != null)
                    {
                        var tempWayMenulist = WaysMenulistTemp.ToList();
                        for (int i = 0; i < tempWayMenulist.Count; i++)
                        {
                            if (GetStages.Any(a => a.Code.Equals(tempWayMenulist[i].Name)))
                            {
                                List<ERMSOPWays> childList = WaysMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(tempWayMenulist[i].KeyName)).ToList();
                                if (childList.Count == 0)
                                {
                                    List<WorkOperationByStages> tempWOList = WorkOperationMenulist.Where(w => w.KeyName == tempWayMenulist[i].KeyName).ToList();
                                    if (tempWOList != null)
                                        tempWOList.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                                    WaysMenulistTemp.Remove(tempWayMenulist[i]);
                                }
                            }
                        }
                    }
                    #endregion
                
                
                #region RND
                #region calculation
                #region

                #endregion
                if (WaysMenulistTemp != null)
                    for (int d = 0; d < WaysMenulistTemp.Count(); d++)
                    {
                        WorkOperationMenulist.Where(w => w.KeyName == WaysMenulistTemp[d].KeyName).ToList().ForEach(f => f.IsCurrentWorkOperation = true);
                         var listCount = WaysMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(WaysMenulistTemp[d].KeyName)).ToList();
                        if (listCount != null)
                            if (listCount.Count() > 0)
                            {
                                WaysMenulistTemp[d].WayObservedTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.WayNormalTime)), 2);
                                WaysMenulistTemp[d].WayNormalTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.WayNormalTime)), 2);
                                #region [GEOS2-3954][Rupali Sarode] Time format HH:mm:ss
                                WaysMenulistTemp[d].UITempWaysObservedTime = ConvertfloattoTimespan(Convert.ToString(listCount.Sum(s => s.WayObservedTime)));
                                WaysMenulistTemp[d].UITempWaysNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(listCount.Sum(s => s.WayNormalTime)));
                                #endregion
                                WaysMenulistTemp[d].IsReadOnly = true;
                            }
                    }

                if (WaysMenulistTemp != null)
                    for (int d = 0; d < WaysMenulistTemp.Count(); d++)
                    {
                        if (RoleValue=="C")
                        {
                            WaysMenulistTemp[d].UiTempStandardOperationTimeName = TimeSpan.Zero;
                            
                        }
                        if (GetStages.Any(a => a.Code.Equals(WaysMenulistTemp[d].Name)))
                        {
                            ObservableCollection<ERMSOPWays> ActivitySum = new ObservableCollection<ERMSOPWays>();
                            ObservableCollection<ERMSOPWays> TempActivitySum = new ObservableCollection<ERMSOPWays>();
                            int x = 1;
                            Loop:
                            x++;
                            if (x < 10)
                            {
                                if (ActivitySum.Count == 0)
                                {
                                    WaysMenulistTemp[d].WayObservedTime = 0;
                                    WaysMenulistTemp[d].WayNormalTime = 0;
                                    #region [GEOS2-3954][Rupali Sarode] Time format HH:mm:ss
                                    WaysMenulistTemp[d].UITempWaysObservedTime = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayObservedTime));
                                    WaysMenulistTemp[d].UITempWaysNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(WaysMenulistTemp[d].WayNormalTime));
                                    #endregion

                                    var ActivitySumList = WaysMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(WaysMenulistTemp[d].KeyName)).ToList();
                                    ActivitySum.AddRange(ActivitySumList);
                                }
                                else
                                {

                                }


                                goto Loop;
                            }

                            double? ObservedTime = 0;
                            double? NormalTime = 0;
                            float? StandardTime = 0;
                            float? OperationTimePlant1Value = 0;
                            float? OperationTimePlant2Value = 0;
                            float? OperationTimePlant3Value = 0;
                            float? OperationTimePlant4Value = 0;
                            float? OperationTimePlant5Value = 0;
                            var tempDistinctList = ActivitySum.Select(xa => xa.KeyName).Distinct().ToList();
                            foreach (var item in tempDistinctList)
                            {
                                if (WaysMenulistTemp[d].WayObservedTime == null)
                                    WaysMenulistTemp[d].WayObservedTime = 0;
                                double? tempObservedTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayObservedTime).FirstOrDefault();
                                if (tempObservedTime != null)
                                {
                                    ObservedTime = Math.Round(Convert.ToDouble(ObservedTime + tempObservedTime), 2);
                                    #region [GEOS2-3954] [Rupali Sarode] Time format HH:mm:ss
                                    WaysMenulistTemp[d].UITempWaysObservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));

                                    #endregion
                                }
                                if (WaysMenulistTemp[d].WayNormalTime == null)
                                    WaysMenulistTemp[d].WayNormalTime = 0;
                                double? tempNormalTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayNormalTime).FirstOrDefault();
                                if (tempNormalTime != null)
                                {
                                    NormalTime = Math.Round(Convert.ToDouble(NormalTime + tempNormalTime), 2);
                                    #region [GEOS2-3954] [Rupali Sarode] Time format HH:mm:ss
                                    WaysMenulistTemp[d].UITempWaysNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(NormalTime));
                                    #endregion

                                }
                            }
                            WaysMenulistTemp[d].WayObservedTime = ObservedTime;
                            WaysMenulistTemp[d].WayNormalTime = NormalTime;
                            #region [GEOS2-3954] [Rupali Sarode] Time format HH:mm:ss
                            //WaysMenulistTemp[d].UITempWaysObservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                            TimeSpan timeSpanObservedTime = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(ObservedTime), 2)); // rounds to 2 decimal places

                            WaysMenulistTemp[d].UITempWaysObservedTime = timeSpanObservedTime;
                            //WaysMenulistTemp[d].UITempWaysNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(WaysMenulistTemp[d].WayNormalTime));
                            TimeSpan timeSpanNormalTime = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayNormalTime), 2)); // rounds to 2 decimal places

                            WaysMenulistTemp[d].UITempWaysNormalTime = timeSpanNormalTime;

                            #endregion


                            #region PlantValue


                            foreach (var item in tempDistinctList)
                            {
                                if (WaysMenulistTemp[d].WayStandardOperationTime == null)
                                    WaysMenulistTemp[d].WayStandardOperationTime = 0;
                                float? tempStandardTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayStandardOperationTime).FirstOrDefault();
                                if (tempStandardTime != null)
                                    StandardTime = StandardTime + tempStandardTime;

                                if (WaysMenulistTemp[d].WayOperationTimePlant1Value == null)
                                    WaysMenulistTemp[d].WayOperationTimePlant1Value = 0;
                                float? tempOperationTimePlant1Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayOperationTimePlant1Value).FirstOrDefault();
                                if (tempOperationTimePlant1Value != null)
                                    OperationTimePlant1Value = OperationTimePlant1Value + tempOperationTimePlant1Value;

                                if (WaysMenulistTemp[d].WayOperationTimePlant2Value == null)
                                    WaysMenulistTemp[d].WayOperationTimePlant2Value = 0;
                                float? tempOperationTimePlant2Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayOperationTimePlant2Value).FirstOrDefault();
                                if (tempOperationTimePlant2Value != null)
                                    OperationTimePlant2Value = OperationTimePlant2Value + tempOperationTimePlant2Value;

                                if (WaysMenulistTemp[d].WayOperationTimePlant3Value == null)
                                    WaysMenulistTemp[d].WayOperationTimePlant3Value = 0;
                                float? tempOperationTimePlant3Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayOperationTimePlant3Value).FirstOrDefault();
                                if (tempOperationTimePlant3Value != null)
                                    OperationTimePlant3Value = OperationTimePlant3Value + tempOperationTimePlant3Value;

                                if (WaysMenulistTemp[d].WayOperationTimePlant4Value == null)
                                    WaysMenulistTemp[d].WayOperationTimePlant4Value = 0;
                                float? tempOperationTimePlant4Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayOperationTimePlant4Value).FirstOrDefault();
                                if (tempOperationTimePlant4Value != null)
                                    OperationTimePlant4Value = OperationTimePlant4Value + tempOperationTimePlant4Value;

                                if (WaysMenulistTemp[d].WayOperationTimePlant5Value == null)
                                    WaysMenulistTemp[d].WayOperationTimePlant5Value = 0;
                                float? tempOperationTimePlant5Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.WayOperationTimePlant5Value).FirstOrDefault();
                                if (tempOperationTimePlant5Value != null)
                                    OperationTimePlant5Value = OperationTimePlant5Value + tempOperationTimePlant5Value;
                            }
                            #endregion
                            WaysMenulistTemp[d].WayStandardOperationTime = (float?)Math.Round(Convert.ToDouble(StandardTime), 2);
                            WaysMenulistTemp[d].WayOperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant1Value), 2);
                            WaysMenulistTemp[d].WayOperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant2Value), 2);
                            WaysMenulistTemp[d].WayOperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant3Value), 2);
                            WaysMenulistTemp[d].WayOperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant4Value), 2);
                            WaysMenulistTemp[d].WayOperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant5Value), 2);
                            #region 
                            try
                            {
                                if (!string.IsNullOrEmpty(RoleValue))
                                {
                                    //  WaysMenulistTemp[d].UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayStandardOperationTime));
                                   // TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(WaysMenulistTemp[d].WayStandardOperationTime));
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayStandardOperationTime), 2)); // rounds to 2 decimal places

                                    WaysMenulistTemp[d].UiTempStandardOperationTimeName = timeSpan;

                                    if (RoleValue == "C")
                                    {
                                       
                                        WaysMenulistTemp[d].UITempWaysNormalTime = TimeSpan.Zero;
                                        WaysMenulistTemp[d].UiTempStandardOperationTimeName = TimeSpan.Zero;
                                        WaysMenulistTemp[d].UiTempoperationTimePlant1Value = TimeSpan.Zero;
                                        WaysMenulistTemp[d].UiTempoperationTimePlant2Value = TimeSpan.Zero;
                                        WaysMenulistTemp[d].UiTempoperationTimePlant3Value = TimeSpan.Zero;
                                        WaysMenulistTemp[d].UiTempoperationTimePlant4Value = TimeSpan.Zero;
                                        WaysMenulistTemp[d].UiTempoperationTimePlant5Value = TimeSpan.Zero;
                                    }
                                    else
                                    {
                                        //WaysMenulistTemp[d].UiTempStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(WaysMenulistTemp[d].UiTempStandardOperationTimeName.TotalMinutes) );
                                        TimeSpan originalTimeSpan = WaysMenulistTemp[d].UiTempStandardOperationTimeName;

                                        // No need to convert back and forth if you're just retaining the value
                                        WaysMenulistTemp[d].UiTempStandardOperationTimeName = new TimeSpan(originalTimeSpan.Hours, originalTimeSpan.Minutes, originalTimeSpan.Seconds);

                                    }
                                }
                                else
                                {
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayStandardOperationTime), 2)); // rounds to 2 decimal places
                                    WaysMenulistTemp[d].UiTempStandardOperationTimeName = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    TimeSpan timeSpan1value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayOperationTimePlant1Value), 2)); // rounds to 2 decimal places
                                    WaysMenulistTemp[d].UiTempoperationTimePlant1Value = timeSpan1value.Add(-TimeSpan.FromMilliseconds(timeSpan1value.Milliseconds));
                                    TimeSpan timeSpan2value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayOperationTimePlant2Value), 2)); // rounds to 2 decimal places
                                    WaysMenulistTemp[d].UiTempoperationTimePlant2Value = timeSpan2value.Add(-TimeSpan.FromMilliseconds(timeSpan2value.Milliseconds));
                                    TimeSpan timeSpan3value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayOperationTimePlant3Value), 2)); // rounds to 2 decimal places
                                    WaysMenulistTemp[d].UiTempoperationTimePlant3Value = timeSpan3value.Add(-TimeSpan.FromMilliseconds(timeSpan3value.Milliseconds));
                                    TimeSpan timeSpan4value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayOperationTimePlant4Value), 2)); // rounds to 2 decimal places
                                    WaysMenulistTemp[d].UiTempoperationTimePlant4Value = timeSpan4value.Add(-TimeSpan.FromMilliseconds(timeSpan4value.Milliseconds));
                                    TimeSpan timeSpan5value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(WaysMenulistTemp[d].WayOperationTimePlant5Value), 2)); // rounds to 2 decimal places
                                    WaysMenulistTemp[d].UiTempoperationTimePlant5Value = timeSpan5value.Add(-TimeSpan.FromMilliseconds(timeSpan5value.Milliseconds));
                                    //  WaysMenulistTemp[d].UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayStandardOperationTime));
                                    //WaysMenulistTemp[d].UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayOperationTimePlant1Value));
                                   // WaysMenulistTemp[d].UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayOperationTimePlant2Value));
                                   // WaysMenulistTemp[d].UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayOperationTimePlant3Value));
                                   // WaysMenulistTemp[d].UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayOperationTimePlant4Value));
                                   // WaysMenulistTemp[d].UiTempoperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayOperationTimePlant5Value));

                                }
                                // WaysMenulistTemp[d].UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(WaysMenulistTemp[d].WayStandardOperationTime));
                               
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in Method WayCustomSummarycalculation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            #endregion
                        }
                    }
                #endregion
                WaysMenulist = new ObservableCollection<ERMSOPWays>();
                WaysMenulist = WaysMenulistTemp;
            //    var temp = LstStandardOperationsDictionaryWaysCP.Where(a => WaysMenulist.Contains(a.IdStandardOperationsDictionary)).ToList();
                #endregion
                //FillWayGridColumn();//[GEOS2-4830][gulab lakade][07 12 2023]
                //wayCustomSummarycalculation();//[GEOS2-4830][gulab lakade][07 12 2023]

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByWay()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveWorkOperationsByWay() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RetrieveWorkOperationsByDetection( double TempDesignValue, string RoleValue)
        {
          
            GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByDetection()...", category: Category.Info, priority: Priority.Low);

      

            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
        
            ModuleMenulistupdated = new ObservableCollection<ERMSOPModule>();
            ObservableCollection<ERMSOPDetection> DetectionMenulistTemp = new ObservableCollection<ERMSOPDetection>(); 
            try
            {
                if (CPOperationsTimeDetectionList==null)
                {
                    CPOperationsTimeDetectionList = new ObservableCollection<CPOperationsTime>();
                }
                

                    duplicateItemloop = 0;
                    if (LstStandardOperationsDictionaryDetectionCP.Count() > 0)
                    {
                        foreach (var eRMSOPDetection in LstStandardOperationsDictionaryDetectionCP)
                        {
                        if (eRMSOPDetection != null)
                        {
                            List<WorkOperationByStages> tempDetectionList = WorkOperationMenulist.Where(w => w.IdworkOperationByStage == Convert.ToInt32(eRMSOPDetection.IdWorkoperation) && w.Name == eRMSOPDetection.WorkOperationName).ToList(); //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                            if (tempDetectionList != null)
                                foreach (WorkOperationByStages item in tempDetectionList)
                                {
                                    var newCPOperationsDetectionTime = new CPOperationsTime();
                                    if (item.IdStatus == 1536)
                                    {
                                        item.IsInactiveWorkOperation = true;
                                        item.IsReadOnly = true;
                                    }
                                    WorkOperationByStages ParentName = WorkOperationMenulist.Where(w => w.KeyName.Equals(item.ParentName)).FirstOrDefault();
                                    if (ParentName != null)
                                    {
                                        if (!operationDetectionMenulist.Any(a => a.KeyName.Equals(ParentName.KeyName)))
                                            operationDetectionMenulist.Add(ParentName);
                                        if (ParentName.Parent == null)
                                        {
                                            newCPOperationsDetectionTime.Parent = ParentName.Name;
                                        }
                                        else
                                        {
                                            newCPOperationsDetectionTime.Operation = ParentName.Name;
                                        }
                                        CPOperationsTimeDetectionList.Add(newCPOperationsDetectionTime);
                                    }
                                    duplicateItemloopDetection = 0;
                                    duplicateItemloop = 0;
                                    GetAllTreeListDetection(item, WorkOperationMenulist);
                                }

                        }

                        }
                    }
                    #region ModuleMenulist
                    WorkOperationMenulist.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                    

                        bool FlagAddItemModule = false; 

                        foreach (WorkOperationByStages Module in operationDetectionMenulist)
                        {
                            FlagAddItemModule = true; 

                            if (operationDetectionMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(Module.IdworkOperationByStage))
                                && operationDetectionMenulist.Any(a => a.Name.Trim().Equals(Module.Name.Trim()))
                                )
                            {
                                StandardOperationsDictionaryDetection standardOperationsDictionaryDetection = LstStandardOperationsDictionaryDetectionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(Module.IdworkOperationByStage) && Module.Parent != null
                               ).FirstOrDefault();
                                ERMSOPDetection eRMSOPDetection = new ERMSOPDetection();

                                #region  [GEOS2-5135][Rupali Sarode][18-12-2023]
                               
                                List<WorkOperationByStages> childListInActiveDraft = operationDetectionMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(Module.KeyName)).ToList();

                                if (childListInActiveDraft.Count == 0)
                                {
                                    if (Module.IdStatus == 1535)
                                    {
                                        FlagAddItemModule = true;
                                    }
                                    else
                                        FlagAddItemModule = false;
                                }
                                else 
                                if (childListInActiveDraft.Count > 0)
                                {
                                   
                                    foreach (WorkOperationByStages objChild in childListInActiveDraft)
                                    {
                                        List<WorkOperationByStages> GrandChildListInActiveDraft = new List<WorkOperationByStages>();
                                        GrandChildListInActiveDraft = operationDetectionMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(objChild.KeyName)).ToList();
                                        if (GrandChildListInActiveDraft.Count == 0)
                                        { 
                                            if (objChild.IdStatus == 1535)
                                            {
                                                FlagAddItemModule = true;
                                                break;
                                            }
                                            else
                                            {
                                                FlagAddItemModule = false;
                                            }

                                        }
                                        else
                                        {
                                            if (GrandChildListInActiveDraft.Count > 0)
                                            {
                                                if (GrandChildListInActiveDraft.Where(i => i.IdStatus == 1535).Count() > 0)
                                                {
                                                    FlagAddItemModule = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    FlagAddItemModule = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion  [GEOS2-5135][Rupali Sarode][18-12-2023]

                                if (FlagAddItemModule == true)  
                                { 

                                    if (standardOperationsDictionaryDetection != null)
                                        eRMSOPDetection.IdDetection = standardOperationsDictionaryDetection.IdDetection;
                                    eRMSOPDetection.Name = Module.Name;
                                    eRMSOPDetection.IdworkOperationByStage = Module.IdworkOperationByStage;
                                    eRMSOPDetection.IdSequence = Module.IdSequence;
                                    eRMSOPDetection.IdStatus = Module.IdStatus;
                                    eRMSOPDetection.Status = Module.Status;
                                    eRMSOPDetection.Code = Module.Name;
                                    eRMSOPDetection.Parent = Module.Parent;
                                    eRMSOPDetection.IdParent = Module.IdParent;
                                    eRMSOPDetection.KeyName = Module.KeyName;
                                    eRMSOPDetection.ParentName = Module.ParentName;
                                    #region [GEOS2-5629][gulab lakade][15 07 2024]
                                   
                                    if (standardOperationsDictionaryDetection != null)
                                    {
                                        eRMSOPDetection.Position = Convert.ToUInt32(standardOperationsDictionaryDetection.Position);
                                        eRMSOPDetection.DetectionPosition = Convert.ToUInt32(standardOperationsDictionaryDetection.Position);//[GEOS2-5629][gulab lakade][10 07 2024]

                                    }
                                    else
                                    {
                                        eRMSOPDetection.Position = Module.Position;
                                        eRMSOPDetection.DetectionPosition = Module.Position; 
                                    }
                                    #endregion
                                    eRMSOPDetection.WorkOperation_count = Module.WorkOperation_count;
                                    eRMSOPDetection.WorkOperation_count_original = Module.WorkOperation_count_original;
                                    eRMSOPDetection.NameWithWorkOperationCount = Module.NameWithWorkOperationCount;
                                  
                                    eRMSOPDetection.IsInactiveWorkOperation = Module.IsInactiveWorkOperation;
                                    eRMSOPDetection.IsReadOnly = Module.IsReadOnly;
                                    
                                    if (!GetStages.Any(a => a.Code.Equals(eRMSOPDetection.Name)))
                                    {
                                        var tempActivity = LstStandardOperationsDictionaryDetectionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPDetection.IdworkOperationByStage)
                                         && w.IdDetection == Convert.ToUInt64(eRMSOPDetection.IdDetection)).Select(s => s.Activity).DefaultIfEmpty(null).FirstOrDefault();
                                        var tempObservedTime = LstStandardOperationsDictionaryDetectionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPDetection.IdworkOperationByStage)
                                         && w.IdDetection == Convert.ToUInt64(eRMSOPDetection.IdDetection)).Select(s => s.ObservedTime).DefaultIfEmpty(null).FirstOrDefault();

                                        #region 

                                        var tempDetection = LstStandardOperationsDictionaryDetectionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPDetection.IdworkOperationByStage)
                                        && w.IdDetection == Convert.ToUInt64(eRMSOPDetection.IdDetection)).FirstOrDefault();

                                        if (tempDetection != null)
                                        {
                                            if (tempDetection.IdWorkoperation == Convert.ToUInt64(eRMSOPDetection.IdworkOperationByStage) && (tempActivity == null || tempActivity == 0))
                                                tempActivity = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.Activity).FirstOrDefault(); //[Cut  work operation not work properly.][gulab lakade][24 01 2023]

                                            if (tempDetection.IdWorkoperation == Convert.ToUInt64(eRMSOPDetection.IdworkOperationByStage) && (tempObservedTime == null || tempObservedTime == 0))
                                                tempObservedTime = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.ObservedTime).FirstOrDefault(); //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                                        }

                                        #endregion
                                        if (tempActivity != null)
                                            eRMSOPDetection.DetectionActivity = Math.Round((double)tempActivity, 2);
                                        if (tempObservedTime != null)
                                        {
                                            eRMSOPDetection.DetectionObservedTime = Math.Round((double)tempObservedTime, 2);
                                            

                                            #region [GEOS2-5008][gulab lakade][1 11 2023]
                                            
                                            eRMSOPDetection.UITempDetectionobservedTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(tempObservedTime)));
                                            #endregion
                                        }

                                        eRMSOPDetection.DetectionNormalTime = Math.Round(Convert.ToDouble(eRMSOPDetection.DetectionObservedTime), 2);
                                        eRMSOPDetection.DetectionNormalTime = eRMSOPDetection.DetectionNormalTime * TempDesignValue;
                               
                                
                                #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                eRMSOPDetection.UITempDetectionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(eRMSOPDetection.DetectionNormalTime));
                                        #endregion
                                      
                                        eRMSOPDetection.WORemarks = Module.WORemarks == null ? "" : Module.WORemarks.Trim();

                                       
                                        var tempRemarks = LstStandardOperationsDictionaryDetectionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPDetection.IdworkOperationByStage)
                                        && w.IdDetection == Convert.ToUInt64(eRMSOPDetection.IdDetection)).Select(s => s.Remarks).DefaultIfEmpty(null).FirstOrDefault();

                                        eRMSOPDetection.Remarks = tempRemarks == null ? "" : tempRemarks.Trim();

                                    }


                                    if (GetStages.Any(a => a != null && a.Code.Trim().Equals(Module.Name.Trim())))
                                    {
                                        eRMSOPDetection.IsDeleteButton = false;
                                        eRMSOPDetection.IsDeleteButtonVisibility = Visibility.Hidden;
                                        eRMSOPDetection.IsReadOnly = true;
                                        eRMSOPDetection.IsValidateOnTextInput = false;
                                        Module.IsCurrentWorkOperation = true;
                                    }
                                    else
                                    {
                                        Module.IsCurrentWorkOperation = true;
                                        eRMSOPDetection.IsReadOnly = false;
                                        eRMSOPDetection.IsValidateOnTextInput = true;
                                    }
                                    if (Module.IdStatus == 1536)
                                    {
                                        eRMSOPDetection.IsInactiveWorkOperation = true;
                                        eRMSOPDetection.IsReadOnly = true;
                                    }
                                  
                                    try
                                    {
                                        if (!DetectionMenulistTemp.Any(a => a.KeyName.Equals(Module.KeyName)))
                                            DetectionMenulistTemp.Add(eRMSOPDetection);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                } 
                            }
                        }
                

                    #region GEOS2-3857 Rupali Sarode
               
                    var tempWorkOperationMenulist2 = DetectionMenulistTemp.OrderBy(x => x.DetectionPosition).ToList();
                    tempWorkOperationClonedDetection = new List<ERMSOPDetection>();
                    tempWorkOperationClonedDetection.AddRange(tempWorkOperationMenulist2);
                    #endregion

                    
                    List<ERMSOPDetection> tempWorkOperationDetectionMenulist = DetectionMenulistTemp.OrderBy(x => x.DetectionPosition).ToList();
                    DetectionMenulistTemp.Clear();
                    DetectionMenulistTemp.AddRange(tempWorkOperationDetectionMenulist);
                    tempWorkOperationDetectionMenulist.Clear();
                    if (DetectionMenulistTemp != null)
                    {
                        var tempDetectionMenulist = DetectionMenulistTemp.ToList();
                        for (int i = 0; i < tempDetectionMenulist.Count; i++)
                        {
                            if (GetStages.Any(a => a.Code.Equals(tempDetectionMenulist[i].Name)))
                            {
                                List<ERMSOPDetection> childList = DetectionMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(tempDetectionMenulist[i].KeyName)).ToList();
                                if (childList.Count == 0)
                                {
                                    List<WorkOperationByStages> tempWOList = WorkOperationMenulist.Where(w => w.KeyName == tempDetectionMenulist[i].KeyName).ToList();
                                    if (tempWOList != null)
                                        tempWOList.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                                    DetectionMenulistTemp.Remove(tempDetectionMenulist[i]);
                                }
                            }
                        }
                    }
                    #endregion
             
                #region RND 
                #region calculation

               
                if (DetectionMenulistTemp != null)
                    for (int d = 0; d < DetectionMenulistTemp.Count(); d++)
                    {
                        WorkOperationMenulist.Where(w => w.KeyName == DetectionMenulistTemp[d].KeyName).ToList().ForEach(f => f.IsCurrentWorkOperation = true);
                       
                        var listCount = DetectionMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(DetectionMenulistTemp[d].KeyName)).ToList();
                        if (listCount != null)
                            if (listCount.Count() > 0)
                            {
                                DetectionMenulistTemp[d].DetectionObservedTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.DetectionObservedTime)), 2);
                                DetectionMenulistTemp[d].DetectionNormalTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.DetectionNormalTime)), 2);
                                #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                DetectionMenulistTemp[d].UITempDetectionobservedTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(listCount.Sum(s => s.DetectionObservedTime))));
                                DetectionMenulistTemp[d].UITempDetectionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(Convert.ToDouble(listCount.Sum(s => s.DetectionNormalTime))));
                                #endregion
                                DetectionMenulistTemp[d].IsReadOnly = true;
                            }
                    }

                if (DetectionMenulistTemp != null)
                    //foreach (ERMSOPDetection itemERMSOPDetection in DetectionMenulistTemp)
                    for (int d = 0; d < DetectionMenulistTemp.Count(); d++)
                    {
                        if (RoleValue == "C")
                        {
                            DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = TimeSpan.Zero;

                        }
                        if (GetStages.Any(a => a.Code.Equals(DetectionMenulistTemp[d].Name)))
                        {
                            ObservableCollection<ERMSOPDetection> ActivitySum = new ObservableCollection<ERMSOPDetection>();
                            ObservableCollection<ERMSOPDetection> TempActivitySum = new ObservableCollection<ERMSOPDetection>();
                            int x = 1;
                            Loop:
                            x++;
                            if (x < 10)
                            {
                                if (ActivitySum.Count == 0)
                                {
                                    DetectionMenulistTemp[d].DetectionObservedTime = 0;
                                    DetectionMenulistTemp[d].DetectionNormalTime = 0;
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    DetectionMenulistTemp[d].UITempDetectionobservedTime = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionObservedTime));
                                    DetectionMenulistTemp[d].UITempDetectionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(DetectionMenulistTemp[d].DetectionNormalTime));
                                    #endregion
                                    
                                    var ActivitySumList = DetectionMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(DetectionMenulistTemp[d].KeyName)).ToList();
                                    ActivitySum.AddRange(ActivitySumList);
                                }
                                else
                                {
                                    
                                }


                                goto Loop;
                            }

                            double? ObservedTime = 0;
                            double? NormalTime = 0;
                            float? StandardTime = 0;
                            float? OperationTimePlant1Value = 0;
                            float? OperationTimePlant2Value = 0;
                            float? OperationTimePlant3Value = 0;
                            float? OperationTimePlant4Value = 0;
                            float? OperationTimePlant5Value = 0;
                            var tempDistinctList = ActivitySum.Select(xa => xa.KeyName).Distinct().ToList();
                            foreach (var item in tempDistinctList)
                            {
                                if (DetectionMenulistTemp[d].DetectionObservedTime == null)
                                    DetectionMenulistTemp[d].DetectionObservedTime = 0;
                                double? tempObservedTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionObservedTime).FirstOrDefault();
                                if (tempObservedTime != null)
                                {
                                    ObservedTime = Math.Round(Convert.ToDouble(ObservedTime + tempObservedTime), 2);
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    DetectionMenulistTemp[d].UITempDetectionobservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                                    #endregion
                                }

                                if (DetectionMenulistTemp[d].DetectionNormalTime == null)
                                    DetectionMenulistTemp[d].DetectionNormalTime = 0;
                                double? tempNormalTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionNormalTime).FirstOrDefault();
                                if (tempNormalTime != null)
                                {
                                    NormalTime = Math.Round(Convert.ToDouble(NormalTime + tempNormalTime), 2);
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    DetectionMenulistTemp[d].UITempDetectionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(NormalTime));
                                    #endregion
                                }

                            }
                            DetectionMenulistTemp[d].DetectionObservedTime = ObservedTime;
                            DetectionMenulistTemp[d].DetectionNormalTime = NormalTime;
                            #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                            DetectionMenulistTemp[d].UITempDetectionobservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                            DetectionMenulistTemp[d].UITempDetectionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(DetectionMenulistTemp[d].DetectionNormalTime));
                            #endregion
                            //DetectionMenulistTemp[d].DetectionNormalTime = Math.Round(Convert.ToDouble(ObservedTime * DetectionMenulistTemp[d].DetectionActivity / 100), 2);
                            #region PlantValue


                            foreach (var item in tempDistinctList)
                            {
                                if (DetectionMenulistTemp[d].DetectionStandardOperationTime == null)
                                    DetectionMenulistTemp[d].DetectionStandardOperationTime = 0;
                                float? tempStandardTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionStandardOperationTime).FirstOrDefault();
                                if (tempStandardTime != null)
                                    StandardTime = StandardTime + tempStandardTime;

                                if (DetectionMenulistTemp[d].DetectionOperationTimePlant1Value == null)
                                    DetectionMenulistTemp[d].DetectionOperationTimePlant1Value = 0;
                                float? tempOperationTimePlant1Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionOperationTimePlant1Value).FirstOrDefault();
                                if (tempOperationTimePlant1Value != null)
                                    OperationTimePlant1Value = OperationTimePlant1Value + tempOperationTimePlant1Value;

                                if (DetectionMenulistTemp[d].DetectionOperationTimePlant2Value == null)
                                    DetectionMenulistTemp[d].DetectionOperationTimePlant2Value = 0;
                                float? tempOperationTimePlant2Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionOperationTimePlant2Value).FirstOrDefault();
                                if (tempOperationTimePlant2Value != null)
                                    OperationTimePlant2Value = OperationTimePlant2Value + tempOperationTimePlant2Value;

                                if (DetectionMenulistTemp[d].DetectionOperationTimePlant3Value == null)
                                    DetectionMenulistTemp[d].DetectionOperationTimePlant3Value = 0;
                                float? tempOperationTimePlant3Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionOperationTimePlant3Value).FirstOrDefault();
                                if (tempOperationTimePlant3Value != null)
                                    OperationTimePlant3Value = OperationTimePlant3Value + tempOperationTimePlant3Value;

                                if (DetectionMenulistTemp[d].DetectionOperationTimePlant4Value == null)
                                    DetectionMenulistTemp[d].DetectionOperationTimePlant4Value = 0;
                                float? tempOperationTimePlant4Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionOperationTimePlant4Value).FirstOrDefault();
                                if (tempOperationTimePlant4Value != null)
                                    OperationTimePlant4Value = OperationTimePlant4Value + tempOperationTimePlant4Value;

                                if (DetectionMenulistTemp[d].DetectionOperationTimePlant5Value == null)
                                    DetectionMenulistTemp[d].DetectionOperationTimePlant5Value = 0;
                                float? tempOperationTimePlant5Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.DetectionOperationTimePlant5Value).FirstOrDefault();
                                if (tempOperationTimePlant5Value != null)
                                    OperationTimePlant5Value = OperationTimePlant5Value + tempOperationTimePlant5Value;
                            }
                            #endregion
                            DetectionMenulistTemp[d].DetectionStandardOperationTime = (float?)Math.Round(Convert.ToDouble(StandardTime), 2);
                            DetectionMenulistTemp[d].DetectionOperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant1Value), 2);
                            DetectionMenulistTemp[d].DetectionOperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant2Value), 2);
                            DetectionMenulistTemp[d].DetectionOperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant3Value), 2);
                            DetectionMenulistTemp[d].DetectionOperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant4Value), 2);
                            DetectionMenulistTemp[d].DetectionOperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant5Value), 2);

                            #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                            try
                            {

                                if (!string.IsNullOrEmpty(RoleValue))
                                {
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionStandardOperationTime), 2)); // rounds to 2 decimal places

                                    DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = timeSpan;

                              //      DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionStandardOperationTime));
                                    if (RoleValue == "C")
                                    {

                                        DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = TimeSpan.Zero;
                                        DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant1Value = TimeSpan.Zero;
                                        DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant2Value = TimeSpan.Zero;
                                        DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant3Value = TimeSpan.Zero;
                                        DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant4Value = TimeSpan.Zero;
                                        DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant5Value = TimeSpan.Zero;
                                    }
                                    else
                                    {
                                        //DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(DetectionMenulistTemp[d].DetectionStandardOperationTime.));
                                       // DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName.TotalMinutes));
                                        TimeSpan originalTimeSpan = DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName;

                                        // No need to convert back and forth if you're just retaining the value
                                        DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = new TimeSpan(originalTimeSpan.Hours, originalTimeSpan.Minutes, originalTimeSpan.Seconds);


                                    }
                                }
                                else
                                {
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionStandardOperationTime), 2)); // rounds to 2 decimal places
                                    DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    TimeSpan timeSpan1value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionOperationTimePlant1Value), 2)); // rounds to 2 decimal places
                                    DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant1Value = timeSpan1value.Add(-TimeSpan.FromMilliseconds(timeSpan1value.Milliseconds));
                                    TimeSpan timeSpan2value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionOperationTimePlant2Value), 2)); // rounds to 2 decimal places
                                    DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant2Value = timeSpan2value.Add(-TimeSpan.FromMilliseconds(timeSpan2value.Milliseconds));
                                    TimeSpan timeSpan3value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionOperationTimePlant3Value), 2)); // rounds to 2 decimal places
                                    DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant3Value = timeSpan3value.Add(-TimeSpan.FromMilliseconds(timeSpan3value.Milliseconds));
                                    TimeSpan timeSpan4value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionOperationTimePlant4Value), 2)); // rounds to 2 decimal places
                                    DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant4Value = timeSpan4value.Add(-TimeSpan.FromMilliseconds(timeSpan4value.Milliseconds));
                                    TimeSpan timeSpan5value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(DetectionMenulistTemp[d].DetectionOperationTimePlant5Value), 2)); // rounds to 2 decimal places
                                    DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant5Value = timeSpan5value.Add(-TimeSpan.FromMilliseconds(timeSpan5value.Milliseconds));

                                    //DetectionMenulistTemp[d].UiTempDetectionStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionStandardOperationTime));
                                    //DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionOperationTimePlant1Value));
                                    //DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionOperationTimePlant2Value));
                                    //DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionOperationTimePlant3Value));
                                    //DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionOperationTimePlant4Value));
                                    //DetectionMenulistTemp[d].UiTempDetectionOperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(DetectionMenulistTemp[d].DetectionOperationTimePlant5Value));

                                }
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillDetectionGridColumn() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            #endregion
                        }
                    }
                #endregion 
                DetectionMenulist = DetectionMenulistTemp;
                #endregion

                
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByDetection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveWorkOperationsByDetection() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        int duplicateItemloopOption = 0;
        private void RetrieveWorkOperationsByOption(double TempDesignValue, string RoleValue)
        {
            
            GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByOption()...", category: Category.Info, priority: Priority.Low);
           
           
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
            ObservableCollection<ERMSOPOptions> OptionsMenulistTemp = new ObservableCollection<ERMSOPOptions>();//[GEOS2-4830][gulab lakade][07 12 2023]
          
            ModuleMenulistupdated = new ObservableCollection<ERMSOPModule>();
            try
            {

                if (CPOperationsTimeOptionList == null)
                {
                    CPOperationsTimeOptionList = new ObservableCollection<CPOperationsTime>();
                }

                    
                    OptionsMenulist = new ObservableCollection<ERMSOPOptions>();
                    operationOptionMenulist = new ObservableCollection<WorkOperationByStages>();
                    duplicateItemloopOption = 0;
                    duplicateItemloop = 0;
                    if (LstStandardOperationsDictionaryOptionCP.Count() > 0)
                    {
                        foreach (var eRMSOPOption in LstStandardOperationsDictionaryOptionCP)
                        {
                            if (eRMSOPOption != null)
                            {
                                List<WorkOperationByStages> tempOptionList = WorkOperationMenulist.Where(w => w.IdworkOperationByStage == Convert.ToInt32(eRMSOPOption.IdWorkoperation) && w.Name == eRMSOPOption.WorkOperationName).ToList();
                            if (tempOptionList != null)
                                foreach (WorkOperationByStages item in tempOptionList)
                                {
                                    var newCPOperationsOptionTime = new CPOperationsTime();
                                    if (item.IdStatus == 1536)
                                    {
                                        item.IsInactiveWorkOperation = true;
                                        item.IsReadOnly = true;
                                    }
                                    WorkOperationByStages ParentName = WorkOperationMenulist.Where(w => w.KeyName.Equals(item.ParentName)).FirstOrDefault();
                                   


                                        if (ParentName != null)
                                        {
                                            if (!operationOptionMenulist.Any(a => a.KeyName.Equals(ParentName.KeyName)))
                                                operationOptionMenulist.Add(ParentName);
                                            if (ParentName.Parent == null)
                                            {
                                                newCPOperationsOptionTime.Parent = ParentName.Name;
                                            }
                                            else
                                            {
                                                newCPOperationsOptionTime.Operation = ParentName.Name;
                                            }
                                            CPOperationsTimeOptionList.Add(newCPOperationsOptionTime);
                                        }
                                   
                                    duplicateItemloopOption = 0;
                                    duplicateItemloop = 0;
                                    GetAllTreeListOption(item, WorkOperationMenulist);
                                }

                            }

                        }
                    }
                    #region ModuleMenulist
                    WorkOperationMenulist.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                    

                        bool FlagAddItemModule = false;  
                        foreach (WorkOperationByStages Module in operationOptionMenulist)
                        {
                            FlagAddItemModule = true; 

                            if (operationOptionMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(Module.IdworkOperationByStage))
                                && operationOptionMenulist.Any(a => a.Name.Trim().Equals(Module.Name.Trim()))
                                )
                            {
                                StandardOperationsDictionaryOption standardOperationsDictionaryOption = LstStandardOperationsDictionaryOptionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(Module.IdworkOperationByStage) && Module.Parent != null
                               ).FirstOrDefault();

                                #region  [GEOS2-5135][Rupali Sarode][18-12-2023]
                          
                                List<WorkOperationByStages> childListInActiveDraft = operationOptionMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(Module.KeyName)).ToList();

                                if (childListInActiveDraft.Count == 0)
                                {
                                    if (Module.IdStatus == 1535)
                                    {
                                        FlagAddItemModule = true;
                                    }
                                    else
                                        FlagAddItemModule = false;
                                }
                                else 
                                if (childListInActiveDraft.Count > 0)
                                {
                                    
                                    foreach (WorkOperationByStages objChild in childListInActiveDraft)
                                    {
                                        List<WorkOperationByStages> GrandChildListInActiveDraft = new List<WorkOperationByStages>();
                                        GrandChildListInActiveDraft = operationOptionMenulist.Where(w => w.ParentName != null && w.ParentName.Equals(objChild.KeyName)).ToList();
                                        if (GrandChildListInActiveDraft.Count == 0)
                                        { 
                                            if (objChild.IdStatus == 1535)
                                            {
                                                FlagAddItemModule = true;
                                                break;
                                            }
                                            else
                                            {
                                                FlagAddItemModule = false;
                                            }

                                        }
                                        else
                                        {
                                            if (GrandChildListInActiveDraft.Count > 0)
                                            {
                                                if (GrandChildListInActiveDraft.Where(i => i.IdStatus == 1535).Count() > 0)
                                                {
                                                    FlagAddItemModule = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    FlagAddItemModule = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion 


                                if (FlagAddItemModule == true)  
                                {

                                    ERMSOPOptions eRMSOPOption = new ERMSOPOptions();
                                    if (standardOperationsDictionaryOption != null)
                                        eRMSOPOption.IdDetection = standardOperationsDictionaryOption.IdDetection;

                                    eRMSOPOption.Name = Module.Name;
                                    eRMSOPOption.IdworkOperationByStage = Module.IdworkOperationByStage;
                                    eRMSOPOption.IdSequence = Module.IdSequence;
                                    eRMSOPOption.IdStatus = Module.IdStatus;
                                    eRMSOPOption.Status = Module.Status;
                                    eRMSOPOption.Code = Module.Name;
                                    eRMSOPOption.Parent = Module.Parent;
                                    eRMSOPOption.IdParent = Module.IdParent;
                                    eRMSOPOption.KeyName = Module.KeyName;
                                    eRMSOPOption.ParentName = Module.ParentName;
                                    #region [GEOS2-5629][gulab lakade][15 07 2024]

                                    if (standardOperationsDictionaryOption != null)
                                    {
                                        eRMSOPOption.Position = Convert.ToUInt32(standardOperationsDictionaryOption.Position);
                                        eRMSOPOption.OptionPosition = Convert.ToUInt32(standardOperationsDictionaryOption.Position);

                                    }
                                    else
                                    {
                                        eRMSOPOption.Position = Module.Position;
                                        eRMSOPOption.OptionPosition = Module.Position; 
                                    }
                                    #endregion
                                    eRMSOPOption.WorkOperation_count = Module.WorkOperation_count;
                                    eRMSOPOption.WorkOperation_count_original = Module.WorkOperation_count_original;
                                    eRMSOPOption.NameWithWorkOperationCount = Module.NameWithWorkOperationCount;

                                    eRMSOPOption.IsInactiveWorkOperation = Module.IsInactiveWorkOperation;
                                    eRMSOPOption.IsReadOnly = Module.IsReadOnly;

                                    if (!GetStages.Any(a => a.Code.Equals(eRMSOPOption.Name)))
                                    {
                                        var tempActivity = LstStandardOperationsDictionaryOptionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPOption.IdworkOperationByStage)
                                         && w.IdDetection == Convert.ToUInt64(eRMSOPOption.IdDetection)).Select(s => s.Activity).DefaultIfEmpty(null).FirstOrDefault();
                                        var tempObservedTime = LstStandardOperationsDictionaryOptionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPOption.IdworkOperationByStage)
                                         && w.IdDetection == Convert.ToUInt64(eRMSOPOption.IdDetection)).Select(s => s.ObservedTime).DefaultIfEmpty(null).FirstOrDefault();


                                        #region [GEOS2-4083][Rupali Sarode][10-12-2022]

                                        var tempOption = LstStandardOperationsDictionaryOptionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPOption.IdworkOperationByStage)
                                        && w.IdDetection == Convert.ToUInt64(eRMSOPOption.IdDetection)).FirstOrDefault();

                                        if (tempOption != null)
                                        {
                                            if (tempOption.IdWorkoperation == Convert.ToUInt64(eRMSOPOption.IdworkOperationByStage) && (tempActivity == null || tempActivity == 0))
                                                tempActivity = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.Activity).FirstOrDefault();  //[Cut  work operation not work properly.][gulab lakade][24 01 2023]

                                            if (tempOption.IdWorkoperation == Convert.ToUInt64(eRMSOPOption.IdworkOperationByStage) && (tempObservedTime == null || tempObservedTime == 0))
                                                tempObservedTime = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Module.IdworkOperationByStage && a.KeyName == Module.KeyName).Select(b => b.ObservedTime).FirstOrDefault();  //[Cut  work operation not work properly.][gulab lakade][24 01 2023]
                                        }

                                        #endregion

                                        if (tempActivity != null)
                                            eRMSOPOption.OptionActivity = Math.Round((double)tempActivity, 2);


                                        if (tempObservedTime != null)
                                        {
                                            eRMSOPOption.OptionObservedTime = Math.Round((double)tempObservedTime, 2);
                                            #region 
                                           
                                            #region [GEOS2-5008][gulab lakade][1 11 2023]
                                         
                                            eRMSOPOption.UITempOptionObservedTime = ConvertfloattoTimespan(Convert.ToString(Convert.ToDouble(tempObservedTime)));
                                            #endregion
                                            #endregion
                                        }


                                        eRMSOPOption.OptionNormalTime = Math.Round(Convert.ToDouble(eRMSOPOption.OptionObservedTime ), 2);
                                        eRMSOPOption.OptionNormalTime = eRMSOPOption.OptionNormalTime * TempDesignValue;
                                        #region [GEOS2-3954][Rupali Sarode]
                                        eRMSOPOption.UITempOptionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(eRMSOPOption.OptionNormalTime));
                                        #endregion


                                        
                                        eRMSOPOption.WORemarks = Module.WORemarks == null ? "" : Module.WORemarks.Trim();

                                        
                                        var tempRemarks = LstStandardOperationsDictionaryOptionCP.Where(w => w.IdWorkoperation == Convert.ToUInt64(eRMSOPOption.IdworkOperationByStage)
                                        && w.IdDetection == Convert.ToUInt64(eRMSOPOption.IdDetection)).Select(s => s.Remarks).DefaultIfEmpty(null).FirstOrDefault();

                                        eRMSOPOption.Remarks = tempRemarks == null ? "" : tempRemarks.Trim();
                                    }


                                    if (GetStages.Any(a => a != null && a.Code.Trim().Equals(Module.Name.Trim())))
                                    {
                                        eRMSOPOption.IsDeleteButton = false;
                                        eRMSOPOption.IsDeleteButtonVisibility = Visibility.Hidden;
                                        eRMSOPOption.IsReadOnly = true;
                                        eRMSOPOption.IsValidateOnTextInput = false;
                                        Module.IsCurrentWorkOperation = true;
                                    }
                                    else
                                    {
                                        Module.IsCurrentWorkOperation = true;
                                        eRMSOPOption.IsReadOnly = false;
                                        eRMSOPOption.IsValidateOnTextInput = true;
                                    }
                                    if (Module.IdStatus == 1536)
                                    {
                                        eRMSOPOption.IsInactiveWorkOperation = true;
                                        eRMSOPOption.IsReadOnly = true;
                                    }
                                   
                                    try
                                    {
                                        if (!OptionsMenulistTemp.Any(a => a.KeyName.Equals(Module.KeyName)))
                                            OptionsMenulistTemp.Add(eRMSOPOption);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                } 

                            }
                        }
                   

                    #region GEOS2-3860 Rupali Sarode
                    
                    var tempWorkOperationMenulist2 = OptionsMenulistTemp.OrderBy(x => x.OptionPosition).ToList();
                    tempWorkOperationClonedOptions = new List<ERMSOPOptions>();
                    tempWorkOperationClonedOptions.AddRange(tempWorkOperationMenulist2);
                    #endregion

                    
                    List<ERMSOPOptions> tempWorkOperationOptionMenulist = OptionsMenulistTemp.OrderBy(x => x.OptionPosition).ToList();//[GEOS2-5629][gulab lakade][15 07 2024]
                    OptionsMenulistTemp.Clear();
                    OptionsMenulistTemp.AddRange(tempWorkOperationOptionMenulist);
                    tempWorkOperationOptionMenulist.Clear();
                    if (OptionsMenulistTemp != null)
                    {
                        var tempOptionMenulist = OptionsMenulistTemp.ToList();
                        for (int i = 0; i < tempOptionMenulist.Count; i++)
                        {
                            if (GetStages.Any(a => a.Code.Equals(tempOptionMenulist[i].Name)))
                            {
                                List<ERMSOPOptions> childList = OptionsMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(tempOptionMenulist[i].KeyName)).ToList();
                                if (childList.Count == 0)
                                {
                                    List<WorkOperationByStages> tempWOList = WorkOperationMenulist.Where(w => w.KeyName == tempOptionMenulist[i].KeyName).ToList();
                                    if (tempWOList != null)
                                        tempWOList.ToList().ForEach(f => f.IsCurrentWorkOperation = false);
                                    OptionsMenulistTemp.Remove(tempOptionMenulist[i]);
                                }
                            }
                        }
                    }
                    #endregion
               
                #region RND
                #region calculation
              
                if (OptionsMenulistTemp != null)
                    for (int d = 0; d < OptionsMenulistTemp.Count(); d++)
                    {
                        WorkOperationMenulist.Where(w => w.KeyName == OptionsMenulistTemp[d].KeyName).ToList().ForEach(f => f.IsCurrentWorkOperation = true);
                       
                        var listCount = OptionsMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(OptionsMenulistTemp[d].KeyName)).ToList();
                        if (listCount != null)
                            if (listCount.Count() > 0)
                            {
                                OptionsMenulistTemp[d].OptionObservedTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.OptionObservedTime)), 2);
                                OptionsMenulistTemp[d].OptionNormalTime = Math.Round(Convert.ToDouble(listCount.Sum(s => s.OptionNormalTime)), 2);

                                
                                OptionsMenulistTemp[d].UITempOptionObservedTime = ConvertfloattoTimespan(Convert.ToString(listCount.Sum(s => s.OptionObservedTime)));
                                OptionsMenulistTemp[d].UITempOptionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(listCount.Sum(s => s.OptionNormalTime)));

                                OptionsMenulistTemp[d].IsReadOnly = true;
                            }
                    }

                if (OptionsMenulistTemp != null)
                    //foreach (ERMSOPDetection itemERMSOPDetection in DetectionMenulist)
                    for (int d = 0; d < OptionsMenulistTemp.Count(); d++)
                    {
                        if (RoleValue=="C")
                        {
                            OptionsMenulistTemp[d].UiTempStandardOperationTimeName = TimeSpan.Zero;
                        }
                        if (GetStages.Any(a => a.Code.Equals(OptionsMenulistTemp[d].Name)))
                        {
                            ObservableCollection<ERMSOPOptions> ActivitySum = new ObservableCollection<ERMSOPOptions>();
                            ObservableCollection<ERMSOPOptions> TempActivitySum = new ObservableCollection<ERMSOPOptions>();
                            int x = 1;
                            Loop:
                            x++;
                            if (x < 10)
                            {
                                if (ActivitySum.Count == 0)
                                {
                                    OptionsMenulistTemp[d].OptionObservedTime = 0;
                                    OptionsMenulistTemp[d].OptionNormalTime = 0;

                                   
                                    OptionsMenulistTemp[d].UITempOptionObservedTime = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionObservedTime));
                                    OptionsMenulistTemp[d].UITempOptionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(OptionsMenulistTemp[d].OptionNormalTime));
                                   
                                    var ActivitySumList = OptionsMenulistTemp.Where(w => w.ParentName != null && w.ParentName.Equals(OptionsMenulistTemp[d].KeyName)).ToList();
                                    ActivitySum.AddRange(ActivitySumList);
                                }
                                else
                                {
                                    
                                }


                                goto Loop;
                            }

                            double? ObservedTime = 0;
                            double? NormalTime = 0;
                            float? StandardTime = 0;
                            float? OperationTimePlant1Value = 0;
                            float? OperationTimePlant2Value = 0;
                            float? OperationTimePlant3Value = 0;
                            float? OperationTimePlant4Value = 0;
                            float? OperationTimePlant5Value = 0;
                            var tempDistinctList = ActivitySum.Select(xa => xa.KeyName).Distinct().ToList();
                            foreach (var item in tempDistinctList)
                            {
                                if (OptionsMenulistTemp[d].OptionObservedTime == null)
                                    OptionsMenulistTemp[d].OptionObservedTime = 0;
                                double? tempObservedTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionObservedTime).FirstOrDefault();
                                if (tempObservedTime != null)
                                {
                                    ObservedTime = Math.Round(Convert.ToDouble(ObservedTime + tempObservedTime), 2);
                                   
                                    OptionsMenulistTemp[d].UITempOptionObservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                                }
                                if (OptionsMenulistTemp[d].OptionNormalTime == null)
                                    OptionsMenulistTemp[d].OptionNormalTime = 0;
                                double? tempNormalTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionNormalTime).FirstOrDefault();
                                if (tempNormalTime != null)
                                {
                                    NormalTime = Math.Round(Convert.ToDouble(NormalTime + tempNormalTime), 2);
                                    OptionsMenulistTemp[d].UITempOptionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(NormalTime));
                                }

                            }
                            OptionsMenulistTemp[d].OptionObservedTime = ObservedTime;
                            OptionsMenulistTemp[d].OptionNormalTime = NormalTime;

                            
                            OptionsMenulistTemp[d].UITempOptionObservedTime = ConvertfloattoTimespan(Convert.ToString(ObservedTime));
                            OptionsMenulistTemp[d].UITempOptionNormalTime = ConvertfloattoTimespanForNormalTime(Convert.ToString(NormalTime));

                            //DetectionMenulist[d].DetectionNormalTime = Math.Round(Convert.ToDouble(ObservedTime * DetectionMenulist[d].DetectionActivity / 100), 2);
                            #region PlantValue


                            foreach (var item in tempDistinctList)
                            {
                                if (OptionsMenulistTemp[d].OptionStandardOperationTime == null)
                                    OptionsMenulistTemp[d].OptionStandardOperationTime = 0;
                                float? tempStandardTime = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionStandardOperationTime).FirstOrDefault();
                                if (tempStandardTime != null)
                                    StandardTime = StandardTime + tempStandardTime;

                                if (OptionsMenulistTemp[d].OptionOperationTimePlant1Value == null)
                                    OptionsMenulistTemp[d].OptionOperationTimePlant1Value = 0;
                                float? tempOperationTimePlant1Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionOperationTimePlant1Value).FirstOrDefault();
                                if (tempOperationTimePlant1Value != null)
                                    OperationTimePlant1Value = OperationTimePlant1Value + tempOperationTimePlant1Value;

                                if (OptionsMenulistTemp[d].OptionOperationTimePlant2Value == null)
                                    OptionsMenulistTemp[d].OptionOperationTimePlant2Value = 0;
                                float? tempOperationTimePlant2Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionOperationTimePlant2Value).FirstOrDefault();
                                if (tempOperationTimePlant2Value != null)
                                    OperationTimePlant2Value = OperationTimePlant2Value + tempOperationTimePlant2Value;

                                if (OptionsMenulistTemp[d].OptionOperationTimePlant3Value == null)
                                    OptionsMenulistTemp[d].OptionOperationTimePlant3Value = 0;
                                float? tempOperationTimePlant3Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionOperationTimePlant3Value).FirstOrDefault();
                                if (tempOperationTimePlant3Value != null)
                                    OperationTimePlant3Value = OperationTimePlant3Value + tempOperationTimePlant3Value;

                                if (OptionsMenulistTemp[d].OptionOperationTimePlant4Value == null)
                                    OptionsMenulistTemp[d].OptionOperationTimePlant4Value = 0;
                                float? tempOperationTimePlant4Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionOperationTimePlant4Value).FirstOrDefault();
                                if (tempOperationTimePlant4Value != null)
                                    OperationTimePlant4Value = OperationTimePlant4Value + tempOperationTimePlant4Value;

                                if (OptionsMenulistTemp[d].OptionOperationTimePlant5Value == null)
                                    OptionsMenulistTemp[d].OptionOperationTimePlant5Value = 0;
                                float? tempOperationTimePlant5Value = ActivitySum.Where(w => w.KeyName.Equals(item)).Select(s => s.OptionOperationTimePlant5Value).FirstOrDefault();
                                if (tempOperationTimePlant5Value != null)
                                    OperationTimePlant5Value = OperationTimePlant5Value + tempOperationTimePlant5Value;
                            }
                            #endregion
                            OptionsMenulistTemp[d].OptionStandardOperationTime = (float?)Math.Round(Convert.ToDouble(StandardTime), 2);
                            OptionsMenulistTemp[d].OptionOperationTimePlant1Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant1Value), 2);
                            OptionsMenulistTemp[d].OptionOperationTimePlant2Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant2Value), 2);
                            OptionsMenulistTemp[d].OptionOperationTimePlant3Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant3Value), 2);
                            OptionsMenulistTemp[d].OptionOperationTimePlant4Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant4Value), 2);
                            OptionsMenulistTemp[d].OptionOperationTimePlant5Value = (float?)Math.Round(Convert.ToDouble(OperationTimePlant5Value), 2);
                            #region GEOS2-3954 Show the WorkOperations and SOD Time in the format: MM:SS or HH:MM:SS
                            try
                            {

                                if (!string.IsNullOrEmpty(RoleValue))
                                {
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionStandardOperationTime), 2)); // rounds to 2 decimal places

                                    OptionsMenulistTemp[d].UiTempStandardOperationTimeName = timeSpan;

                                   // OptionsMenulistTemp[d].UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionStandardOperationTime));
                                    if (RoleValue == "C")
                                    {

                                        OptionsMenulistTemp[d].UiTempStandardOperationTimeName = TimeSpan.Zero;
                                        OptionsMenulistTemp[d].UiTempoperationTimePlant1Value = TimeSpan.Zero;
                                        OptionsMenulistTemp[d].UiTempoperationTimePlant2Value = TimeSpan.Zero;
                                        OptionsMenulistTemp[d].UiTempoperationTimePlant3Value = TimeSpan.Zero;
                                        OptionsMenulistTemp[d].UiTempoperationTimePlant4Value = TimeSpan.Zero;
                                        OptionsMenulistTemp[d].UiTempoperationTimePlant5Value = TimeSpan.Zero;
                                    }
                                    else
                                    {
                                        //OptionsMenulistTemp[d].UiTempStandardOperationTimeName = OptionsMenulistTemp[d].UiTempStandardOperationTimeName;//TimeSpan.FromMinutes(Convert.ToDouble(OptionsMenulistTemp[d].UiTempStandardOperationTimeName.TotalMinutes));
                                        TimeSpan originalTimeSpan = OptionsMenulistTemp[d].UiTempStandardOperationTimeName;

                                        // No need to convert back and forth if you're just retaining the value
                                        OptionsMenulistTemp[d].UiTempStandardOperationTimeName = new TimeSpan(originalTimeSpan.Hours, originalTimeSpan.Minutes, originalTimeSpan.Seconds);



                                        // OptionsMenulistTemp[d].UiTempStandardOperationTimeName = TimeSpan.FromMinutes(Convert.ToDouble(OptionsMenulistTemp[d].UiTempStandardOperationTimeName.TotalMinutes) );
                                    }
                                }
                                else
                                {


                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionStandardOperationTime), 2)); // rounds to 2 decimal places
                                    OptionsMenulistTemp[d].UiTempStandardOperationTimeName = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    TimeSpan timeSpan1value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionOperationTimePlant1Value), 2)); // rounds to 2 decimal places
                                    OptionsMenulistTemp[d].UiTempoperationTimePlant1Value = timeSpan1value.Add(-TimeSpan.FromMilliseconds(timeSpan1value.Milliseconds));
                                    TimeSpan timeSpan2value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionOperationTimePlant2Value), 2)); // rounds to 2 decimal places
                                    OptionsMenulistTemp[d].UiTempoperationTimePlant2Value = timeSpan2value.Add(-TimeSpan.FromMilliseconds(timeSpan2value.Milliseconds));
                                    TimeSpan timeSpan3value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionOperationTimePlant3Value), 2)); // rounds to 2 decimal places
                                    OptionsMenulistTemp[d].UiTempoperationTimePlant3Value = timeSpan3value.Add(-TimeSpan.FromMilliseconds(timeSpan3value.Milliseconds));
                                    TimeSpan timeSpan4value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionOperationTimePlant4Value), 2)); // rounds to 2 decimal places
                                    OptionsMenulistTemp[d].UiTempoperationTimePlant4Value = timeSpan4value.Add(-TimeSpan.FromMilliseconds(timeSpan4value.Milliseconds));
                                    TimeSpan timeSpan5value = TimeSpan.FromMinutes(Math.Round(Convert.ToDouble(OptionsMenulistTemp[d].OptionOperationTimePlant5Value), 2)); // rounds to 2 decimal places
                                    OptionsMenulistTemp[d].UiTempoperationTimePlant5Value = timeSpan5value.Add(-TimeSpan.FromMilliseconds(timeSpan5value.Milliseconds));

                                    //OptionsMenulistTemp[d].UiTempStandardOperationTimeName = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionStandardOperationTime));
                                    //OptionsMenulistTemp[d].UiTempoperationTimePlant1Value = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionOperationTimePlant1Value));
                                    //OptionsMenulistTemp[d].UiTempoperationTimePlant2Value = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionOperationTimePlant2Value));
                                    //OptionsMenulistTemp[d].UiTempoperationTimePlant3Value = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionOperationTimePlant3Value));
                                    //OptionsMenulistTemp[d].UiTempoperationTimePlant4Value = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionOperationTimePlant4Value));
                                    //OptionsMenulistTemp[d].UiTempoperationTimePlant5Value = ConvertfloattoTimespan(Convert.ToString(OptionsMenulistTemp[d].OptionOperationTimePlant5Value));

                                }
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log(string.Format("Error in Method OptionCustomSummarycalculation() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            #endregion
                        }
                    }
                #endregion
               // var tempOptionsMenulist = OptionsMenulistTemp.Where(a => a.Code == WorkStation).ToList();
                //foreach (var item in OptionsMenulistTemp.Where(a => a.Code == WorkStation))
                //{
                //    item.Code = null;
                //}
               // OptionsMenulistTemp = new ObservableCollection<ERMSOPOptions>(OptionsMenulistTemp.Where(a=>a.Code== WorkStation).ToList())
               OptionsMenulist = OptionsMenulistTemp;
                #endregion
                
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RetrieveWorkOperationsByOption()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method RetrieveWorkOperationsByOption() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillWorkOperationsStages(string IdStage, string IdCP)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillWorkOperationsStages()...", category: Category.Info, priority: Priority.Low);
                ObservableCollection<WorkOperationByStages> OperationList = new ObservableCollection<WorkOperationByStages>();
                ObservableCollection<WorkOperationByStages> tempWorkOperationMenulist = new ObservableCollection<WorkOperationByStages>();
                if (GetStages == null)
                {
                    GetStages = new ObservableCollection<Stages>(ERMService.GetStages());
                }
                else
                {
                    GetStages.Clear();
                    GetStages.AddRange(ERMService.GetStages());
                }
                if (IdStage == "Expected")
                {
                    GetStages = new ObservableCollection<Stages>(GetStages);
                }
                else
                {
                    GetStages = new ObservableCollection<Stages>(GetStages.Where(a => a.IdStage == Convert.ToInt32(IdStage)).ToList());
                }
              //  ERMCommon.Instance.GetStages = new ObservableCollection<Stages>();
               // ERMCommon.Instance.GetStages = GetStages;
                //OperationList.Clear();
                foreach (Stages item in GetStages)
                {
                    
                    //OperationList.AddRange(ERMService.GetAllWorkOperationStages_V2300(item.IdStage));
                    //OperationList.AddRange(ERMService.GetAllWorkOperationStages_V2320(2));
                                        
                    OperationList.AddRange(ERMService.GetAllWorkOperationStages_V2630(item.IdStage, IdCP)); 

                }

                UpdateWorkOperationCount(OperationList);
                if (WorkOperationMenulist == null)
                {
                    WorkOperationMenulist = new ObservableCollection<WorkOperationByStages>();
                }
                else
                {
                    WorkOperationMenulist.Clear();
                }

                WorkOperationByStages workOperationByStages = new WorkOperationByStages();
                workOperationByStages.Name = "All";
                workOperationByStages.KeyName = "Group_All";
                workOperationByStages.WorkOperation_count = OperationList.GroupBy(i => i.IdworkOperationByStage).Count();
                workOperationByStages.NameWithWorkOperationCount = "All [" + workOperationByStages.WorkOperation_count + "]";
                tempWorkOperationMenulist.Insert(0, workOperationByStages);
                foreach (Stages item in GetStages)
                {
                    WorkOperationByStages workOperation = new WorkOperationByStages();
                    workOperation.IdworkOperationByStage = item.IdStage;
                    workOperation.Name = item.Code;
                    workOperation.KeyName = "Stage_" + item.IdStage;
                    workOperation.Parent = null;
                    workOperation.IdParent = null;
                    workOperation.Position = Convert.ToUInt32(item.IdSequence);
                    workOperation.WorkOperation_count = OperationList.Where(x => x.IdStage == item.IdStage).GroupBy(i => i.IdworkOperationByStage).Count();
                    workOperation.NameWithWorkOperationCount = item.Code + " [" + workOperation.WorkOperation_count + "]";
                    tempWorkOperationMenulist.Add(workOperation);
                    tempWorkOperationMenulist.AddRange(OperationList.Where(x => x.IdStage == item.IdStage));
                }
                var tempWorkOperationMenulist2 = tempWorkOperationMenulist.OrderBy(x => x.Position).ToList();
                //WorkOperationMenulist.Clear();
                if (treeListControlInstance != null) treeListControlInstance.BeginDataUpdate();
                WorkOperationMenulist.AddRange(tempWorkOperationMenulist2);
                // var temp= WorkOperationMenulist.Where(x=>x.idsta)
                if (treeListControlInstance != null) treeListControlInstance.EndDataUpdate();

                //WorkOperationMenulist = new ObservableCollection<WorkOperationByStages>(WorkOperationMenulist.OrderBy(x => x.Position));
                SelectedWorkOperationMenulist = WorkOperationMenulist.FirstOrDefault();
                if (treeListControlInstance != null) treeListControlInstance.View.ExpandAllNodes();
                //WorkOperationsList = new ObservableCollection<WorkOperation>(WorkOperationsList_All);

                //SelectedWorkOperation = WorkOperationsList.FirstOrDefault();
                // ClonedWorkOperationByStages = (ObservableCollection<WorkOperationByStages>)WorkOperationMenulist;
                GeosApplication.Instance.Logger.Log("Method FillWorkOperationsStages()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOperationsStages() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillWorkOperationsStages() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillWorkOperationsStages()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void UpdateWorkOperationCount(ObservableCollection<WorkOperationByStages> OperationList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UpdateWorkOperationCount()...", category: Category.Info, priority: Priority.Low);

                foreach (WorkOperationByStages item in OperationList)
                {
                    int count = 0;
                    //[001]
                    //if (item.WorkOperation_count_original != null)
                    //{
                    //    count = item.WorkOperation_count_original;
                    //}
                    if (OperationList.Any(a => a.ParentName == item.KeyName))
                    {
                        List<WorkOperationByStages> getFirstList = OperationList.Where(a => a.ParentName == item.KeyName).ToList();
                        foreach (WorkOperationByStages item1 in getFirstList)
                        {
                            if (item1.WorkOperation_count_original != null)
                            {
                                count = count + item1.WorkOperation_count_original;
                            }
                            if (OperationList.Any(a => a.ParentName == item1.KeyName))
                            {
                                List<WorkOperationByStages> getSecondList = OperationList.Where(a => a.ParentName == item1.KeyName).ToList();
                                foreach (WorkOperationByStages item2 in getSecondList)
                                {
                                    if (item2.WorkOperation_count_original != null)
                                    {
                                        count = count + item2.WorkOperation_count_original;
                                    }
                                    if (OperationList.Any(a => a.ParentName == item2.KeyName))
                                    {
                                        List<WorkOperationByStages> getThirdList = OperationList.Where(a => a.ParentName == item2.KeyName).ToList();
                                        foreach (WorkOperationByStages item3 in getThirdList)
                                        {
                                            if (item3.WorkOperation_count_original != null)
                                            {
                                                count = count + item3.WorkOperation_count_original;
                                            }
                                            if (OperationList.Any(a => a.ParentName == item3.KeyName))
                                            {
                                                List<WorkOperationByStages> getForthList = OperationList.Where(a => a.ParentName == item3.KeyName).ToList();
                                                foreach (WorkOperationByStages item4 in getForthList)
                                                {
                                                    if (item4.WorkOperation_count_original != null)
                                                    {
                                                        count = count + item4.WorkOperation_count_original;
                                                    }
                                                    if (OperationList.Any(a => a.ParentName == item4.KeyName))
                                                    {
                                                        List<WorkOperationByStages> getFifthList = OperationList.Where(a => a.ParentName == item4.KeyName).ToList();
                                                        foreach (WorkOperationByStages item5 in getFifthList)
                                                        {
                                                            if (item5.WorkOperation_count_original != null)
                                                            {
                                                                count = count + item5.WorkOperation_count_original;
                                                            }
                                                            if (OperationList.Any(a => a.ParentName == item5.KeyName))
                                                            {
                                                                List<WorkOperationByStages> getSixthList = OperationList.Where(a => a.ParentName == item5.KeyName).ToList();
                                                                foreach (WorkOperationByStages item6 in getSixthList)
                                                                {
                                                                    if (item6.WorkOperation_count_original != null)
                                                                    {
                                                                        count = count + item6.WorkOperation_count_original;
                                                                    }
                                                                    if (OperationList.Any(a => a.ParentName == item6.KeyName))
                                                                    {
                                                                        List<WorkOperationByStages> getSeventhList = OperationList.Where(a => a.ParentName == item6.KeyName).ToList();
                                                                        foreach (WorkOperationByStages item7 in getSeventhList)
                                                                        {
                                                                            if (item7.WorkOperation_count_original != null)
                                                                            {
                                                                                count = count + item7.WorkOperation_count_original;
                                                                            }
                                                                            if (OperationList.Any(a => a.ParentName == item7.KeyName))
                                                                            {
                                                                                List<WorkOperationByStages> getEightthList = OperationList.Where(a => a.ParentName == item7.KeyName).ToList();
                                                                                foreach (WorkOperationByStages item8 in getEightthList)
                                                                                {
                                                                                    if (item8.WorkOperation_count_original != null)
                                                                                    {
                                                                                        count = count + item8.WorkOperation_count_original;
                                                                                    }
                                                                                    if (OperationList.Any(a => a.ParentName == item8.KeyName))
                                                                                    {
                                                                                        List<WorkOperationByStages> getNinethList = OperationList.Where(a => a.ParentName == item8.KeyName).ToList();
                                                                                        foreach (WorkOperationByStages item9 in getNinethList)
                                                                                        {
                                                                                            if (item9.WorkOperation_count_original != null)
                                                                                            {
                                                                                                count = count + item9.WorkOperation_count_original;
                                                                                            }
                                                                                            if (OperationList.Any(a => a.ParentName == item9.KeyName))
                                                                                            {
                                                                                                List<WorkOperationByStages> gettenthList = OperationList.Where(a => a.ParentName == item9.KeyName).ToList();
                                                                                                foreach (WorkOperationByStages item10 in gettenthList)
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
                    item.NameWithWorkOperationCount = Convert.ToString(item.Name + " [" + Convert.ToInt32(item.WorkOperation_count) + "]");
                }
                GeosApplication.Instance.Logger.Log("Method UpdateWorkOperationCount()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method UpdateWorkOperationCount() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public TimeSpan ConvertfloattoTimespanForNormalTime(string Normaltime)
        {
            TimeSpan UITempNormalTime;
            try
            {

                #region [GEOS2-5008][gulab lakade][1 11 2023]
                UITempNormalTime = TimeSpan.FromMinutes(Convert.ToDouble(Normaltime));
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
                return UITempNormalTime;
            }
            catch (Exception ex)
            {
                UITempNormalTime = TimeSpan.FromMinutes(0);
                return UITempNormalTime;
            }


        }
        public TimeSpan ConvertfloattoTimespan(string observedtime)
        {
            TimeSpan UITempobservedTime;
            try
            {
                #region GEOS2-3954 Time format HH:MM:SS
                var currentculter = CultureInfo.CurrentCulture;
                string culterseparator = currentculter.NumberFormat.NumberDecimalSeparator.ToString();
                string tempd = Convert.ToString((float)Convert.ToDouble(observedtime)); // [GEOS2-6835][dhawal bhalerao][08 04 2025] : modified to resolve error while directly converting observedtime db value to float
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
                    UITempobservedTime = TimeSpan.FromSeconds(i1);
                    int ts1 = UITempobservedTime.Hours;
                    int ts2 = UITempobservedTime.Minutes;
                    int ts3 = UITempobservedTime.Seconds;
                }
                else
                {
                    //parts = tempd.Split(Convert.ToChar(culterseparator));
                    //i1 = int.Parse(parts[0]);
                    //i1 = (i1 * 60);

                    UITempobservedTime = TimeSpan.FromSeconds(Convert.ToInt64(tempd) * 60);  //GEOS2-4045 Gulab Lakade time coversio issue
                    int ts1 = UITempobservedTime.Hours;
                    int ts2 = UITempobservedTime.Minutes;
                    int ts3 = UITempobservedTime.Seconds;
                }

                #endregion
                return UITempobservedTime;
            }
            catch (Exception ex)
            {
                UITempobservedTime = TimeSpan.FromSeconds(0);
                return UITempobservedTime;
            }

        }
        int duplicateItemloop = 0;
        private void GetAllTreeListWay(WorkOperationByStages StandardOperationsDictionaryModules, ObservableCollection<WorkOperationByStages> WorkOperationMenulist)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetAllTreeListWay()...", category: Category.Info, priority: Priority.Low);

                if (StandardOperationsDictionaryModules != null && StandardOperationsDictionaryModules.Name.ToLower() != "All".ToLower())
                {

                    if (Convert.ToInt32(StandardOperationsDictionaryModules.IdParent) == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage))
                    {
                        if (!operationWayMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                            operationWayMenulist.Add(StandardOperationsDictionaryModules);
                        if (StandardOperationsDictionaryModules.IdParent == null || StandardOperationsDictionaryModules.IdStage == 0)
                        {

                        }
                        else
                        {
                            WorkOperationByStages TempStagesList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage) &&
                            a.KeyName.Equals(("Stage_" + Convert.ToInt32(StandardOperationsDictionaryModules.IdStage)).ToString())).FirstOrDefault();
                            if (duplicateItemloopWay >= 20) { TempStagesList = null; }
                            GetAllTreeListWay(TempStagesList, WorkOperationMenulist);
                        }
                    }
                    else
                    {
                        if (operationWayMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(StandardOperationsDictionaryModules.IdworkOperationByStage)))
                        {
                            duplicateItemloopWay = duplicateItemloopWay + 1;
                            if (duplicateItemloopWay >= 20)
                            {
                                GetAllTreeListWay(null, WorkOperationMenulist);
                            }
                        }
                        else
                        {
                            if (!operationWayMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                                operationWayMenulist.Add(StandardOperationsDictionaryModules);
                        }
                        List<WorkOperationByStages> TempList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdParent)).ToList();
                        if (duplicateItemloopWay >= 20) { TempList = null; }
                        if (TempList != null)
                            foreach (WorkOperationByStages itemWorkOperationByStages in TempList)
                            {
                                GetAllTreeListWay(itemWorkOperationByStages, WorkOperationMenulist);
                            }
                    }

                }
                GeosApplication.Instance.Logger.Log("Method GetAllTreeListWay()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAllTreeListWay()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        ObservableCollection<WorkOperationByStages> operationDetectionMenulist = new ObservableCollection<WorkOperationByStages>();
        int duplicateItemloopDetection = 0;
        private void GetAllTreeListDetection(WorkOperationByStages StandardOperationsDictionaryModules, ObservableCollection<WorkOperationByStages> WorkOperationMenulist)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetAllTreeListDetection()...", category: Category.Info, priority: Priority.Low);

                if (StandardOperationsDictionaryModules != null && StandardOperationsDictionaryModules.Name.ToLower() != "All".ToLower())
                {

                    if (Convert.ToInt32(StandardOperationsDictionaryModules.IdParent) == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage))
                    {
                        if (!operationDetectionMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                            operationDetectionMenulist.Add(StandardOperationsDictionaryModules);
                        if (StandardOperationsDictionaryModules.IdParent == null || StandardOperationsDictionaryModules.IdStage == 0)
                        {

                        }
                        else
                        {
                            WorkOperationByStages TempStagesList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage) &&
                            a.KeyName.Equals(("Stage_" + Convert.ToInt32(StandardOperationsDictionaryModules.IdStage)).ToString())).FirstOrDefault();
                            if (duplicateItemloopDetection >= 20) { TempStagesList = null; }
                            GetAllTreeListDetection(TempStagesList, WorkOperationMenulist);
                        }
                    }
                    else
                    {
                        if (operationDetectionMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(StandardOperationsDictionaryModules.IdworkOperationByStage)))
                        {
                            duplicateItemloopDetection = duplicateItemloopDetection + 1;
                            if (duplicateItemloopDetection >= 20)
                            {
                                GetAllTreeListDetection(null, WorkOperationMenulist);
                            }
                        }
                        else
                        {
                            if (!operationDetectionMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                                operationDetectionMenulist.Add(StandardOperationsDictionaryModules);
                        }
                        List<WorkOperationByStages> TempList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdParent)).ToList();
                        if (duplicateItemloopDetection >= 20) { TempList = null; }
                        if (TempList != null)
                            foreach (WorkOperationByStages itemWorkOperationByStages in TempList)
                            {
                                GetAllTreeListDetection(itemWorkOperationByStages, WorkOperationMenulist);
                            }
                    }

                }
                GeosApplication.Instance.Logger.Log("Method GetAllTreeListDetection()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAllTreeListDetection()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void GetAllTreeListOption(WorkOperationByStages StandardOperationsDictionaryModules, ObservableCollection<WorkOperationByStages> WorkOperationMenulist)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetAllTreeListOption()...", category: Category.Info, priority: Priority.Low);

                if (StandardOperationsDictionaryModules != null && StandardOperationsDictionaryModules.Name.ToLower() != "All".ToLower())
                {

                    if (Convert.ToInt32(StandardOperationsDictionaryModules.IdParent) == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage))
                    {
                        if (!operationOptionMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                            operationOptionMenulist.Add(StandardOperationsDictionaryModules);
                        if (StandardOperationsDictionaryModules.IdParent == null || StandardOperationsDictionaryModules.IdStage == 0)
                        {

                        }
                        else
                        {
                            WorkOperationByStages TempStagesList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage) &&
                            a.KeyName.Equals(("Stage_" + Convert.ToInt32(StandardOperationsDictionaryModules.IdStage)).ToString())).FirstOrDefault();
                            if (duplicateItemloopOption >= 20) { TempStagesList = null; }
                            GetAllTreeListOption(TempStagesList, WorkOperationMenulist);
                        }
                    }
                    else
                    {
                        if (operationOptionMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(StandardOperationsDictionaryModules.IdworkOperationByStage)))
                        {
                            duplicateItemloopOption = duplicateItemloopOption + 1;
                            if (duplicateItemloopOption >= 20)
                            {
                                GetAllTreeListOption(null, WorkOperationMenulist);
                            }
                        }
                        else
                        {
                            if (!operationOptionMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                                operationOptionMenulist.Add(StandardOperationsDictionaryModules);
                        }
                        List<WorkOperationByStages> TempList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdParent)).ToList();
                        if (duplicateItemloopOption >= 20) { TempList = null; }
                        if (TempList != null)
                            foreach (WorkOperationByStages itemWorkOperationByStages in TempList)
                            {
                                GetAllTreeListOption(itemWorkOperationByStages, WorkOperationMenulist);
                            }
                    }

                }
                GeosApplication.Instance.Logger.Log("Method GetAllTreeListOption()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAllTreeListOption()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        ObservableCollection<ERMSOPModule> moduleList = new ObservableCollection<ERMSOPModule>();
        ObservableCollection<WorkOperationByStages> operationMenulist = new ObservableCollection<WorkOperationByStages>();
        //int duplicateItemloop = 0;
        private void GetAllTreeList(WorkOperationByStages StandardOperationsDictionaryModules, ObservableCollection<WorkOperationByStages> WorkOperationMenulist)
        {
            try
            {
                if (StandardOperationsDictionaryModules != null && StandardOperationsDictionaryModules.Name.ToLower() != "All".ToLower())
                {

                    if (Convert.ToInt32(StandardOperationsDictionaryModules.IdParent) == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage))
                    {
                        if (!operationMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                            operationMenulist.Add(StandardOperationsDictionaryModules);
                        if (StandardOperationsDictionaryModules.IdParent == null || StandardOperationsDictionaryModules.IdStage == 0)
                        {

                        }
                        else
                        {
                            WorkOperationByStages TempStagesList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdStage) &&
                            a.KeyName.Equals(("Stage_" + Convert.ToInt32(StandardOperationsDictionaryModules.IdStage)).ToString())).FirstOrDefault();
                            if (duplicateItemloop >= 20) { TempStagesList = null; }
                            GetAllTreeList(TempStagesList, WorkOperationMenulist);
                        }
                    }
                    else
                    {
                        if (operationMenulist.Any(a => a.IdworkOperationByStage == Convert.ToInt64(StandardOperationsDictionaryModules.IdworkOperationByStage)))
                        {
                            duplicateItemloop = duplicateItemloop + 1;
                            if (duplicateItemloop >= 20)
                            {
                                GetAllTreeList(null, WorkOperationMenulist);
                            }
                        }
                        else
                        {
                            if (!operationMenulist.Any(a => a.KeyName.Equals(StandardOperationsDictionaryModules.KeyName)))
                                operationMenulist.Add(StandardOperationsDictionaryModules);
                        }
                        List<WorkOperationByStages> TempList = WorkOperationMenulist.Where(a => a.IdworkOperationByStage == Convert.ToInt32(StandardOperationsDictionaryModules.IdParent)).ToList();
                        if (duplicateItemloop >= 20) { TempList = null; }
                        if (TempList != null)
                            foreach (WorkOperationByStages itemWorkOperationByStages in TempList)
                            {
                                GetAllTreeList(itemWorkOperationByStages, WorkOperationMenulist);
                            }
                    }

                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetAllTreeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region CustomSummary

        public void CustomSummaryCommandAction(TreeListCustomSummaryEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomSummaryCommandAction()...", category: Category.Info, priority: Priority.Low);

               // TreeListCustomSummaryEventArgs e = (TreeListCustomSummaryEventArgs)obj;
                
                #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                if (e.SummaryItem.FieldName == "UiTempStandardOperationTimeName")
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {

                        if (e.FieldValue != null && e.Node != null)
                        {
                            ERMSOPModule temp = (ERMSOPModule)(e.Node.Content);
                            if (GetStages.Any(a => a != null && a.Code.Trim().Equals(temp.Name.Trim())))
                            {
                             
                                #region Time changes 28 11 2022
                                TimeSpan temp3 = TimeSpan.Parse(e.FieldValue.ToString());
                                double TempFieldValue = 0;
                                if (string.IsNullOrEmpty(Convert.ToString(e.FieldValue)))
                                {
                                    TempFieldValue = 0;
                                }
                                else
                                {
                                    TempFieldValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.FieldValue)).TotalMinutes);
                                }

                                double TempTotalValue = 0;
                                if (string.IsNullOrEmpty(Convert.ToString(e.TotalValue)))
                                {
                                    TempTotalValue = 0;
                                }
                                else
                                {
                                    TempTotalValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.TotalValue)).TotalMinutes);
                                }

                                e.TotalValue = TempTotalValue + TempFieldValue;
                                if (e.TotalValue != null)
                                {
                                    //e.TotalValue = Math.Round(Convert.ToDouble(e.TotalValue), 2);
                                    #region GEOS2-3954 gulab lakade Time format HH:mm:ss
                                    //e.TotalValue = ConvertfloattoTimespan(Convert.ToString(e.TotalValue));
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(e.TotalValue));
                                    e.TotalValue = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    #endregion

                                }
                                #endregion
                            }

                        }

                    }
                }

                #endregion
                

                GeosApplication.Instance.Logger.Log("Method CustomSummaryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method CustomSummaryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void WaysCustomSummaryCommandAction(TreeListCustomSummaryEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method WaysCustomSummaryCommandAction()...", category: Category.Info, priority: Priority.Low);

                //    TreeListCustomSummaryEventArgs e = (TreeListCustomSummaryEventArgs)obj;

                #region 
                if (e.SummaryItem.FieldName == "UiTempStandardOperationTimeName")
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {

                        if (e.FieldValue != null && e.Node != null)
                        {
                             ERMSOPWays temp = (ERMSOPWays)(e.Node.Content);
                            
                                if (GetStages.Any(a => a != null && a.Code.Trim().Equals(temp.Name.Trim())))
                                {

                                    #region Time changes 28 11 2022
                                    TimeSpan temp3 = TimeSpan.Parse(e.FieldValue.ToString());
                                    double TempFieldValue = 0;
                                    if (string.IsNullOrEmpty(Convert.ToString(e.FieldValue)))
                                    {
                                        TempFieldValue = 0;
                                    }
                                    else
                                    {
                                        TempFieldValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.FieldValue)).TotalMinutes);
                                    }

                                    double TempTotalValue = 0;
                                    if (string.IsNullOrEmpty(Convert.ToString(e.TotalValue)))
                                    {
                                        TempTotalValue = 0;
                                    }
                                    else
                                    {
                                        TempTotalValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.TotalValue)).TotalMinutes);
                                    }

                                    e.TotalValue = TempTotalValue + TempFieldValue;
                                    if (e.TotalValue != null)
                                    {
                                    //   e.TotalValue = Math.Round(Convert.ToDouble(e.TotalValue), 2);
                                    #region 
                                    //e.TotalValue = ConvertfloattoTimespan(Convert.ToString(e.TotalValue));
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(e.TotalValue));
                                    e.TotalValue = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    #endregion

                                }
                                #endregion
                            }
                            
                        }

                    }

                    if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                    {
                        // Finalize the summary value, it should already be a TimeSpan
                        e.TotalValue = e.TotalValue; // You can modify this if needed
                    }
                }

                #endregion

                GeosApplication.Instance.Logger.Log("Method WaysCustomSummaryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method WaysCustomSummaryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        public void DetectionCustomSummaryCommandAction(TreeListCustomSummaryEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DetectionCustomSummaryCommandAction()...", category: Category.Info, priority: Priority.Low);

                //    TreeListCustomSummaryEventArgs e = (TreeListCustomSummaryEventArgs)obj;

                #region 
                if (e.SummaryItem.FieldName == "UiTempDetectionStandardOperationTimeName")
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {

                        if (e.FieldValue != null && e.Node != null)
                        {
                            ERMSOPDetection temp = (ERMSOPDetection)(e.Node.Content);
                            if (GetStages.Any(a => a != null && a.Code.Trim().Equals(temp.Name.Trim())))
                            {

                                #region Time changes 28 11 2022
                                TimeSpan temp3 = TimeSpan.Parse(e.FieldValue.ToString());
                                double TempFieldValue = 0;
                                if (string.IsNullOrEmpty(Convert.ToString(e.FieldValue)))
                                {
                                    TempFieldValue = 0;
                                }
                                else
                                {
                                    TempFieldValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.FieldValue)).TotalMinutes);
                                }

                                double TempTotalValue = 0;
                                if (string.IsNullOrEmpty(Convert.ToString(e.TotalValue)))
                                {
                                    TempTotalValue = 0;
                                }
                                else
                                {
                                    TempTotalValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.TotalValue)).TotalMinutes);
                                }

                                e.TotalValue = TempTotalValue + TempFieldValue;
                                if (e.TotalValue != null)
                                {
                                    // e.TotalValue = Math.Round(Convert.ToDouble(e.TotalValue), 2);
                                    #region 
                                    //e.TotalValue = ConvertfloattoTimespan(Convert.ToString(e.TotalValue));
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(e.TotalValue));
                                    e.TotalValue = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    #endregion

                                }
                                #endregion
                            }
                        }

                    }

                    if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                    {
                        // Finalize the summary value, it should already be a TimeSpan
                        e.TotalValue = e.TotalValue; // You can modify this if needed
                    }
                }

                #endregion

                GeosApplication.Instance.Logger.Log("Method DetectionCustomSummaryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method DetectionCustomSummaryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        public void OptionCustomSummaryCommandAction(TreeListCustomSummaryEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OptionCustomSummaryCommandAction()...", category: Category.Info, priority: Priority.Low);

            //    TreeListCustomSummaryEventArgs e = (TreeListCustomSummaryEventArgs)obj;

                #region 
                if (e.SummaryItem.FieldName == "UiTempStandardOperationTimeName")
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {

                        if (e.FieldValue != null && e.Node != null)
                        {
                            ERMSOPOptions temp = (ERMSOPOptions)(e.Node.Content);
                            if (GetStages.Any(a => a != null && a.Code.Trim().Equals(temp.Name.Trim())))
                            {

                                #region Time changes 28 11 2022
                                TimeSpan temp3 = TimeSpan.Parse(e.FieldValue.ToString());
                                double TempFieldValue = 0;
                                if (string.IsNullOrEmpty(Convert.ToString(e.FieldValue)))
                                {
                                    TempFieldValue = 0;
                                }
                                else
                                {
                                    TempFieldValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.FieldValue)).TotalMinutes);
                                }

                                double TempTotalValue = 0;
                                if (string.IsNullOrEmpty(Convert.ToString(e.TotalValue)))
                                {
                                    TempTotalValue = 0;
                                }
                                else
                                {
                                    TempTotalValue = Convert.ToDouble(TimeSpan.Parse(Convert.ToString(e.TotalValue)).TotalMinutes);
                                }

                                e.TotalValue = TempTotalValue + TempFieldValue;
                                if (e.TotalValue != null)
                                {
                                    // e.TotalValue = Math.Round(Convert.ToDouble(e.TotalValue), 2);
                                    #region 
                                    // e.TotalValue = ConvertfloattoTimespan(Convert.ToString(e.TotalValue));
                                  //  e.TotalValue = TimeSpan.FromMinutes(Convert.ToDouble(e.TotalValue));
                                    TimeSpan timeSpan = TimeSpan.FromMinutes(Convert.ToDouble(e.TotalValue));
                                    e.TotalValue = timeSpan.Add(-TimeSpan.FromMilliseconds(timeSpan.Milliseconds));
                                    #endregion

                                }
                                #endregion
                           }
                        }

                    }

                    if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                    {
                        // Finalize the summary value, it should already be a TimeSpan
                        e.TotalValue = e.TotalValue; // You can modify this if needed
                    }
                }
                
                #endregion

                GeosApplication.Instance.Logger.Log("Method OptionCustomSummaryCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log(string.Format("Error in method OptionCustomSummaryCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }


        #endregion

        #region  Export to Excel [Geos2-6078]
        private void ExportCpsOperationCommandAction(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method ExportCpsOperationCommandAction()...", category: Category.Info, priority: Priority.Low);


                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog
                {
                    DefaultExt = "xlsx",
                    FileName = "Expected Time Details",
                    Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FilterIndex = 1,
                    Title = "Save Excel Report"
                };

                bool? dialogResult = saveFile.ShowDialog();
                if (dialogResult != true)
                {
                    ResultFileName = string.Empty;
                    return;
                }

                ResultFileName = saveFile.FileName;


                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show<SplashScreenView>();
                }


                var workbook = new DevExpress.Spreadsheet.Workbook();


                var tuple = obj as Tuple<object, object, object, object, object>;
                if (tuple != null)
                {
                    var grid = tuple.Item1 as System.Windows.Controls.Grid;
                    var treeListControls = new List<DevExpress.Xpf.Grid.TreeListControl>
            {
                tuple.Item2 as DevExpress.Xpf.Grid.TreeListControl,
                tuple.Item3 as DevExpress.Xpf.Grid.TreeListControl,
                tuple.Item4 as DevExpress.Xpf.Grid.TreeListControl,
                tuple.Item5 as DevExpress.Xpf.Grid.TreeListControl
            };
                    int sheetIndex = 1;
                    System.Windows.Controls.UIElementCollection elements = (System.Windows.Controls.UIElementCollection)grid.Children;


                    var gridData = ExtractDataFromGrid(grid);

                    if (gridData.Count > 0)
                    {
                        var gridSheet = workbook.Worksheets.Add("Grid Data");
                        PopulateWorksheetWithGridData(gridSheet, gridData);
                    }
                    int textBlockRow = 0;
                  
                    int startRow = 0;
                    int startColumn = 0; 
                    var targetWorksheet = workbook.Worksheets[0];

                    int initialColumn = startColumn;
                    foreach (var element in elements)
                    {
                        if (element is TextBlock)
                        {
                            TextBlock textBlock = (TextBlock)element;

                            targetWorksheet.Cells[startRow, startColumn].Value = textBlock.Text;
                            targetWorksheet.Cells[startRow, startColumn].Font.Bold = true;
                            startColumn++;
                        }
                        else if (element is DevExpress.Xpf.Editors.TextEdit)
                        {
                            DevExpress.Xpf.Editors.TextEdit textEdit = (DevExpress.Xpf.Editors.TextEdit)element;
                            targetWorksheet.Cells[startRow, startColumn].Value = textEdit.DisplayText;
                            startColumn++;
                        }
                    }

                  
                    startRow++;
                    startColumn = initialColumn;  


                    startRow++;
                    //foreach (var element in elements)
                    //{
                    //    if (element is TextBlock)
                    //    {
                    //        TextBlock textBlock = (TextBlock)element;

                    //        Place the TextBlock value in the current row and column
                    //        targetWorksheet.Cells[startRow, startColumn].Value = textBlock.Text;
                    //        targetWorksheet.Cells[startRow, startColumn].Font.Bold = true;

                    //        Move to the next column for TextEdit
                    //       startColumn++;
                    //    }
                    //    else if (element is DevExpress.Xpf.Editors.TextEdit)
                    //    {
                    //        DevExpress.Xpf.Editors.TextEdit textEdit = (DevExpress.Xpf.Editors.TextEdit)element;

                    //        Place the TextEdit value in the column next to the TextBlock
                    //        targetWorksheet.Cells[startRow, startColumn].Value = textEdit.DisplayText;

                    //        Move to the next row and reset the column to the initial column
                    //       startRow++;
                    //        startColumn = initialColumn; // Reset to initial column for the next TextBlock and TextEdit
                    //    }
                    //}



                    foreach (var treeControl in treeListControls)
                    {
                        if (treeControl != null)
                        {
                            var treeListView = treeControl.View as DevExpress.Xpf.Grid.TreeListView;

                            if (treeListView != null)
                            {
                              
                                using (var stream = new MemoryStream())
                                {
                                    treeListView.ExportToXlsx(stream);
                                    stream.Position = 0;

                                   
                                    var tempWorkbook = new DevExpress.Spreadsheet.Workbook();
                                    tempWorkbook.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

                                  
                                    var tempWorksheet = tempWorkbook.Worksheets[0];
                                    var usedRange = tempWorksheet.GetUsedRange();

                                   
                                    if (usedRange == null || usedRange.RowCount == 0 || usedRange.ColumnCount == 0)
                                    {
                                        GeosApplication.Instance.Logger.Log($"Empty or invalid data in TreeListView at index {sheetIndex}.", category: Category.Info, priority: Priority.Low);
                                        continue;
                                    }


                                    for (int i = 0; i < 4; i++) 
                                    {
                                        int coll = startColumn + (i * 2); 
                                        workbook.Worksheets[0].Columns[coll].WidthInCharacters = 65; 
                                    }

                                    for (int i = 0; i < 4; i++)
                                    {
                                        int coll = startColumn + ((i * 2) + 1); // 1st, 3rd, 5th, 7th
                                        workbook.Worksheets[0].Columns[coll].WidthInCharacters = 20;
                                    }

                                    var targetRange = targetWorksheet.Range.FromLTRB(startColumn, startRow, startColumn + usedRange.ColumnCount - 1, startRow + usedRange.RowCount - 1);

                                   
                                    targetRange.CopyFrom(usedRange);

                               
                                    startColumn += usedRange.ColumnCount;
                                }
                            }
                            else
                            {
                                GeosApplication.Instance.Logger.Log($"TreeListControl at index {sheetIndex} does not contain a valid TreeListView.", category: Category.Info, priority: Priority.Low);
                            }
                        }
                    }


                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                }
                else
                {
                    
                    TreeListView moduleGrid = obj as TreeListView;
                    if (moduleGrid != null)
                    {
                       
                        using (var stream = new MemoryStream())
                        {
                            moduleGrid.ExportToXlsx(stream);
                            stream.Position = 0;

                            var tempWorkbook = new DevExpress.Spreadsheet.Workbook();
                            tempWorkbook.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

                            workbook.Worksheets.Add();
                            workbook.Worksheets[0].CopyFrom(tempWorkbook.Worksheets[0]);
                            workbook.Worksheets[0].Name = "Module Operations";
                        }

                       
                        AddSheetIfExists(workbook, WaysGrid, "Ways Operations");
                        AddSheetIfExists(workbook, DetectionGrid, "Detection Operations");
                        AddSheetIfExists(workbook, OptionsGrid, "Options Operations");
                    }
                    else
                    {
                        GeosApplication.Instance.Logger.Log("ExportCpsOperationCommandAction: The passed object is not a TreeListView.", category: Category.Info, priority: Priority.Low);
                    }
                }

             
                workbook.SaveDocument(ResultFileName, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

              
                System.Diagnostics.Process.Start(ResultFileName);

              
                GeosApplication.Instance.Logger.Log("Method ExportCpsOperationCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Error in ExportCpsOperationCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private List<object[]> ExtractDataFromGrid(System.Windows.Controls.Grid grid)
        {
            var data = new List<object[]>();

            return data;
        }
        private void PopulateWorksheetWithGridData(Worksheet worksheet, List<object[]> gridData)
        {

            for (int i = 0; i < gridData.Count; i++)
            {
                for (int j = 0; j < gridData[i].Length; j++)
                {
                    var cellValue = gridData[i][j];


                    if (cellValue is string)
                    {
                        worksheet.Cells[i, j].Value = (string)cellValue;
                    }
                    else if (cellValue is int)
                    {
                        worksheet.Cells[i, j].Value = (int)cellValue;
                    }
                    else if (cellValue is double)
                    {
                        worksheet.Cells[i, j].Value = (double)cellValue;
                    }
                    else if (cellValue is bool)
                    {
                        worksheet.Cells[i, j].Value = (bool)cellValue;
                    }
                    else if (cellValue is DateTime)
                    {
                        worksheet.Cells[i, j].Value = (DateTime)cellValue;
                    }
                    else
                    {
                        worksheet.Cells[i, j].Value = cellValue?.ToString() ?? string.Empty;  // Default to string representation
                    }
                }
            }
        }

        private void AddSheetIfExists(DevExpress.Spreadsheet.Workbook workbook, TreeListView grid, string sheetName)
        {
            if (grid != null)
            {
                using (var stream = new MemoryStream())
                {
                    grid.ExportToXlsx(stream);
                    stream.Position = 0;

                    var tempWorkbook = new DevExpress.Spreadsheet.Workbook();
                    tempWorkbook.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);

                    workbook.Worksheets.Add();
                    workbook.Worksheets[workbook.Worksheets.Count - 1].CopyFrom(tempWorkbook.Worksheets[0]);
                    workbook.Worksheets[workbook.Worksheets.Count - 1].Name = sheetName;
                }
            }
        }
        #endregion
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        #endregion
    }
}
