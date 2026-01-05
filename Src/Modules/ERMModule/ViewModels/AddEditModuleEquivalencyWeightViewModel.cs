using System.Linq;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.ERM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;
using System.Data;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Data;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using Emdep.Geos.Utility;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using Emdep.Geos.UI.Common;
using System;
using DevExpress.Xpf.Core;
using System.Windows;
using System.Collections.Generic;
using Emdep.Geos.Modules.ERM.Views;
using System.Globalization;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Modules.ERM.CommonClasses;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class AddEditModuleEquivalencyWeightViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

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

        #region Declarations
        private string windowHeader;
        MaximizedElementPosition maximizedElementPosition;
        ObservableCollection<EquivalencyWeight> modulesEquivalentWeightList;
        List<EquivalencyWeight> modulesEquivalentWeightDeleteList;
        ObservableCollection<EquivalencyWeight> structuersEquivalentWeightList;
        public string ModulesEquivalencyWeightGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "ModulesEquivalencyWeightGridSetting.Xml";

        private EquivalencyWeight selectedEquivalentWeight;
        private EquivalencyWeight selectedStructuresEquivalentWeight;


        private List<EquivalencyWeight> addEquivalentWeightList;  //[GEOS2-4331][Rupali Sarode][18-04-2023]
        private List<EquivalencyWeight> addStructuresEquivalentWeightList;  //[GEOS2-4331][Rupali Sarode][18-04-2023]
        private int selectedIdCpType;  //[GEOS2-4331][Rupali Sarode][18-04-2023]

        private List<EquivalencyWeight> updateEquivalentWeightList;
        private List<EquivalencyWeight> updateStructuresEquivalentWeightList;
        private List<EquivalencyWeight> structuresEquivalentWeightList;
        private List<EquivalencyWeight> structuresEquivalentWeightDeleteList;
        #region [gulab lakade][GEOS2-4333][12 04 2023]
        private float? equivalentWeight;
        private string lblEquivalentWeight; 
        private DateTime? startDate;
        private DateTime? endDate;
        private bool isSave;
        private bool isUpdate;
        ObservableCollection<LogEntryByModuleEquivalenceWeight> moduleEquivalenceWeightChangeLogList;
        ObservableCollection<LogEntryByModuleEquivalenceWeight> moduleEquivalenceWeightAllChangeLogList;
        EquivalencyWeight latestEquivalentWeight; //[Rupali Sarode][26-04-2023]
        #endregion
        #endregion Declarations

        #region Properties
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
       
        
        public ObservableCollection<EquivalencyWeight> ModulesEquivalentWeightList
        {
            get
            {
                return modulesEquivalentWeightList;
            }
            set
            {
                modulesEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModulesEquivalentWeightList"));
            }
        }

        public List<EquivalencyWeight> ModulesEquivalentWeightDeleteList
        {
            get
            {
                return modulesEquivalentWeightDeleteList;
            }
            set
            {
                modulesEquivalentWeightDeleteList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModulesEquivalentWeightDeleteList"));
            }
        }
        public List<EquivalencyWeight> StructuresEquivalentWeightDeleteList
        {
            get
            {
                return modulesEquivalentWeightDeleteList;
            }
            set
            {
                modulesEquivalentWeightDeleteList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StructuresEquivalentWeightDeleteList"));
            }
        }

        //[Rupali Sarode][26-04-2023]
        public EquivalencyWeight LatestEquivalentWeight
        {
            get
            {
                return latestEquivalentWeight;
            }

            set
            {
                latestEquivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LatestEquivalentWeight"));
            }
        }
        public EquivalencyWeight SelectedEquivalentWeight
        {
            get
            {
                return selectedEquivalentWeight;
            }

            set
            {
                selectedEquivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEquivalentWeight"));
            }
        }
        public EquivalencyWeight SelectedStructuresEquivalentWeight
        {
            get
            {
                return selectedStructuresEquivalentWeight;
            }

            set
            {
                selectedStructuresEquivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedStructuresEquivalentWeight"));
            }
        }
        public List<EquivalencyWeight> UpdateEquivalentWeightList
        {
            get
            {
                return updateEquivalentWeightList;
            }

            set
            {
                updateEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateEquivalentWeightList"));
            }
        }

        public List<EquivalencyWeight> UpdateStructuresEquivalentWeightList
        {
            get
            {
                return updateStructuresEquivalentWeightList;
            }

            set
            {
                updateStructuresEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateStructuresEquivalentWeightList"));
            }
        }

        #region  [GEOS2-4335][Rupali Sarode][21-04-2023]
        public ObservableCollection<LogEntryByModuleEquivalenceWeight> ModuleEquivalenceWeightAllChangeLogList
        {
            get
            {
                return moduleEquivalenceWeightAllChangeLogList;
            }

            set
            {
                moduleEquivalenceWeightAllChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleEquivalenceWeightAllChangeLogList"));
            }
        }
        #endregion

        #region [GEOS2-4331][Rupali Sarode][18-04-2023]
        public List<EquivalencyWeight> AddEquivalentWeightList
        {
            get
            {
                return addEquivalentWeightList;
            }

            set
            {
                addEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddEquivalentWeightList"));
            }
        }

        public List<EquivalencyWeight> AddStructuresEquivalentWeightList
        {
            get
            {
                return addStructuresEquivalentWeightList;
            }

            set
            {
                addStructuresEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddStructuresEquivalentWeightList"));
            }
        }

        public int SelectedIdCpType
        {
            get { return selectedIdCpType; }
            set
            {
                selectedIdCpType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIdCpType"));
            }
        }

        #endregion [GEOS2-4331][Rupali Sarode][18-04-2023]

        public List<EquivalencyWeight> StructuresEquivalentWeightList
        {
            get
            {
                return structuresEquivalentWeightList;
            }
            set
            {
                structuresEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StructuresEquivalentWeightList"));
            }
        }

        public float? EquivalentWeight
        {
            get { return equivalentWeight; }
            set
            {
                equivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquivalentWeight"));
            }
        }
        public string LblEquivalentWeight
        {
            get { return lblEquivalentWeight; }
            set
            {
                lblEquivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LblEquivalentWeight"));
            }
        }
        public ModulesEquivalencyWeight ModuleEquivalencyWeight { get; set; }
        public ModulesEquivalencyWeight StructureEquivalencyWeight { get; set; }

        #region [gulab lakade][GEOS2-4333][12 04 2023]
        private ModulesEquivalencyWeight clonedMEW;
        public ModulesEquivalencyWeight ClonedMEW
        {
            get { return clonedMEW; }
            set
            {
                clonedMEW = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedArticle"));
            }
        }
       
       
        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));

            }
        }
        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));

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
        public bool IsUpdate
        {
            get
            {
                return isUpdate;
            }

            set
            {
                isUpdate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdate"));

            }
        }
        public ObservableCollection<LogEntryByModuleEquivalenceWeight> ModuleEquivalenceWeightChangeLogList
        {
            get { return moduleEquivalenceWeightChangeLogList; }
            set
            {
                moduleEquivalenceWeightChangeLogList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleEquivalenceWeightChangeLogList"));
            }
        }

        #endregion


        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }
        #endregion Properties


        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand ModulesEquivalencyWeightTableViewLoadedCommand { get; set; }

        public ICommand DeleteModuleEquivalencyWeightCommand { get; set; }
        public ICommand EditModulesEquivalencyWeightCommand { get; set; }

        public ICommand AddButtonCommand { get; set; }

        #endregion ICommands

        #region Constructor

        public AddEditModuleEquivalencyWeightViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditModuleEquivalencyWeightViewModel ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                ModulesEquivalencyWeightTableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(ModulesEquivalencyWeightTableViewLoadedCommandAction);
                DeleteModuleEquivalencyWeightCommand = new RelayCommand(new Action<object>(DeleteModuleEquivalencyWeightCommandAction));
                ModulesEquivalentWeightDeleteList = new List<EquivalencyWeight>();
                StructuresEquivalentWeightDeleteList = new List<EquivalencyWeight>();
                EditModulesEquivalencyWeightCommand = new DelegateCommand<object>(EditModulesEquivalencyWeightAction);
                AddButtonCommand = new DelegateCommand<object>(AddButtonCommandAction);
                ModuleEquivalenceWeightChangeLogList = new ObservableCollection<LogEntryByModuleEquivalenceWeight>();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddEditModuleEquivalencyWeightViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor AddEditModuleEquivalencyWeightViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
        }


        #endregion Constructor


        #region Methods

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()..."), category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(ModulesEquivalencyWeight tSelectedModuleEqivalentWeight)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][18/12/2023]
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                ModulesEquivalentWeightList = new ObservableCollection<EquivalencyWeight>();
                AddEquivalentWeightList = new List<EquivalencyWeight>();
                ModuleEquivalenceWeightChangeLogList = new ObservableCollection<LogEntryByModuleEquivalenceWeight>();
                // ModuleEquivalencyWeight = new ModulesEquivalencyWeight();

                SelectedIdCpType = tSelectedModuleEqivalentWeight.IdCPType; //[GEOS2-4331][Rupali Sarode][18-04-2023]

              // ERMService = new ERMServiceController("localhost:6699");
                if (tSelectedModuleEqivalentWeight != null)
                {
                    //ModuleEquivalencyWeight = ERMService.GetProductTypesEquivalencyWeightByCPType_V2380(tSelectedModuleEqivalentWeight.IdCPType);
                    ModulesEquivalencyWeight temp = ERMService.GetProductTypesEquivalencyWeightByCPType_V2380(tSelectedModuleEqivalentWeight.IdCPType);
                    //ModulesEquivalentWeightList = temp.LstEquivalencyWeight;
                    if (temp.LstEquivalencyWeight != null)
                    {
                        ModulesEquivalentWeightList = new ObservableCollection<EquivalencyWeight>(temp.LstEquivalencyWeight);
                    }
                    if (temp.LstLogEntryByModuleEquivalenceWeight != null)
                    {
                        ModuleEquivalenceWeightAllChangeLogList = new ObservableCollection<LogEntryByModuleEquivalenceWeight>(temp.LstLogEntryByModuleEquivalenceWeight.OrderByDescending(x => x.IdLogEntryByMEW).ToList());
                    }

                    #region [gulab lakade][GEOS2-4333][12 04 2023]
                    ClonedMEW = (ModulesEquivalencyWeight)temp.Clone();
                    //ModulesEquivalentWeightList = ClonedMEW.LstEquivalencyWeight;
                    var lastRecord = ClonedMEW.LstEquivalencyWeight.OrderByDescending(a => a.StartDate).FirstOrDefault();
                    
                    if (lastRecord != null)
                    {
                        EquivalentWeight = (float)(lastRecord.EquivalentWeight);
                       
                        if (lastRecord.StartDate != null)
                        {
                            StartDate = Convert.ToDateTime(lastRecord.StartDate);
                           
                        }
                        if (lastRecord.EndDate != null)
                        {
                            EndDate = Convert.ToDateTime(lastRecord.EndDate);
                        }

                    }
                    #endregion
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]

                ModuleEquivalencyWeight = new ModulesEquivalencyWeight();
                
                List<EquivalencyWeight> FinalModulesEquivalentWeightList = new List<EquivalencyWeight>();
                //[Pallavi Jadhav][Geos-4332][14 04 2023]
                if (LblEquivalentWeight == "Module")
                {
                    if (AddEquivalentWeightList != null)
                    {
                        if (AddEquivalentWeightList.Count() > 0)
                        {
                            foreach (EquivalencyWeight eModulesEquivalentWeightAdd in AddEquivalentWeightList)
                            {

                                EquivalencyWeight ModulesEquivalentWeightAdd = new EquivalencyWeight();
                                ModulesEquivalentWeightAdd.TransactionOperation = ModelBase.TransactionOperations.Add;
                                ModulesEquivalentWeightAdd.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                ModulesEquivalentWeightAdd.IdCPType = eModulesEquivalentWeightAdd.IdCPType;
                                ModulesEquivalentWeightAdd.EquivalentWeight = (float)eModulesEquivalentWeightAdd.EquivalentWeight;
                                ModulesEquivalentWeightAdd.StartDate = eModulesEquivalentWeightAdd.StartDate;
                                ModulesEquivalentWeightAdd.EndDate = eModulesEquivalentWeightAdd.EndDate;
                                FinalModulesEquivalentWeightList.Add(ModulesEquivalentWeightAdd);
                            }
                        }
                    }

                    if (UpdateEquivalentWeightList != null)
                    {
                        if (UpdateEquivalentWeightList.Count() > 0)
                        {
                            foreach (EquivalencyWeight eModulesEquivalentWeightUpdate in UpdateEquivalentWeightList)
                            {

                                EquivalencyWeight ModulesEquivalentWeightUpdate = new EquivalencyWeight();
                                ModulesEquivalentWeightUpdate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                ModulesEquivalentWeightUpdate.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                ModulesEquivalentWeightUpdate.ModifiedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                ModulesEquivalentWeightUpdate.IDCPTypeEquivalent = eModulesEquivalentWeightUpdate.IDCPTypeEquivalent;
                                ModulesEquivalentWeightUpdate.IdCPType = eModulesEquivalentWeightUpdate.IdCPType;
                                ModulesEquivalentWeightUpdate.EquivalentWeight = (float)eModulesEquivalentWeightUpdate.EquivalentWeight;
                                ModulesEquivalentWeightUpdate.StartDate = eModulesEquivalentWeightUpdate.StartDate;
                                ModulesEquivalentWeightUpdate.EndDate = eModulesEquivalentWeightUpdate.EndDate;
                                FinalModulesEquivalentWeightList.Add(ModulesEquivalentWeightUpdate);
                            }
                        }
                    }

                    if (ModulesEquivalentWeightDeleteList.Count() > 0)
                    {
                        foreach (var eModulesEquivalentWeightDelete in ModulesEquivalentWeightDeleteList)
                        {
                            EquivalencyWeight ModulesEquivalentWeightDelete = new EquivalencyWeight();

                            ModulesEquivalentWeightDelete.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            ModulesEquivalentWeightDelete.IDCPTypeEquivalent = eModulesEquivalentWeightDelete.IDCPTypeEquivalent;
                            ModulesEquivalentWeightDelete.IdCPType = eModulesEquivalentWeightDelete.IdCPType;
                            ModulesEquivalentWeightDelete.EquivalentWeight = (float)eModulesEquivalentWeightDelete.EquivalentWeight;
                            ModulesEquivalentWeightDelete.StartDate = eModulesEquivalentWeightDelete.StartDate;
                            ModulesEquivalentWeightDelete.EndDate = eModulesEquivalentWeightDelete.EndDate;
                            FinalModulesEquivalentWeightList.Add(ModulesEquivalentWeightDelete);
                        }
                    }
                    
                }
                if (LblEquivalentWeight == "Structure")
                {

                    if (AddStructuresEquivalentWeightList != null)
                    {
                        if (AddStructuresEquivalentWeightList.Count() > 0)
                        {
                            foreach (EquivalencyWeight eStructuresEquivalentWeightAdd in AddStructuresEquivalentWeightList)
                            {

                                EquivalencyWeight StructuresEquivalentWeightAdd = new EquivalencyWeight();
                                StructuresEquivalentWeightAdd.TransactionOperation = ModelBase.TransactionOperations.Add;
                                StructuresEquivalentWeightAdd.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                StructuresEquivalentWeightAdd.IdCPType = eStructuresEquivalentWeightAdd.IdCPType;
                                StructuresEquivalentWeightAdd.EquivalentWeight = (float)eStructuresEquivalentWeightAdd.EquivalentWeight;
                                StructuresEquivalentWeightAdd.StartDate = eStructuresEquivalentWeightAdd.StartDate;
                                StructuresEquivalentWeightAdd.EndDate = eStructuresEquivalentWeightAdd.EndDate;
                                FinalModulesEquivalentWeightList.Add(StructuresEquivalentWeightAdd);
                            }
                        }
                    }

                    if (UpdateStructuresEquivalentWeightList != null)
                    {
                        if (UpdateStructuresEquivalentWeightList.Count() > 0)
                        {
                            foreach (EquivalencyWeight eStructuresEquivalentWeightUpdate in UpdateStructuresEquivalentWeightList)
                            {

                                EquivalencyWeight StructuresEquivalentWeightUpdate = new EquivalencyWeight();
                                StructuresEquivalentWeightUpdate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                StructuresEquivalentWeightUpdate.CreatedBy = (uint)GeosApplication.Instance.ActiveUser.IdUser;
                                StructuresEquivalentWeightUpdate.ModifiedBy = (UInt32)GeosApplication.Instance.ActiveUser.IdUser;
                                StructuresEquivalentWeightUpdate.IDCPTypeEquivalent = eStructuresEquivalentWeightUpdate.IDCPTypeEquivalent;
                                StructuresEquivalentWeightUpdate.IdCPType = eStructuresEquivalentWeightUpdate.IdCPType;
                                StructuresEquivalentWeightUpdate.EquivalentWeight = (float)eStructuresEquivalentWeightUpdate.EquivalentWeight;
                                StructuresEquivalentWeightUpdate.StartDate = eStructuresEquivalentWeightUpdate.StartDate;
                                StructuresEquivalentWeightUpdate.EndDate = eStructuresEquivalentWeightUpdate.EndDate;
                                FinalModulesEquivalentWeightList.Add(StructuresEquivalentWeightUpdate);
                            }
                        }
                    }
                    if (StructuresEquivalentWeightDeleteList.Count() > 0)
                    {
                        foreach (EquivalencyWeight eStructuresEquivalentWeightDeleteList in StructuresEquivalentWeightDeleteList)
                        {
                            EquivalencyWeight StructuresEquivalentWeightDelete = new EquivalencyWeight();

                            StructuresEquivalentWeightDelete.TransactionOperation = ModelBase.TransactionOperations.Delete;
                            StructuresEquivalentWeightDelete.IDCPTypeEquivalent = eStructuresEquivalentWeightDeleteList.IDCPTypeEquivalent;
                            StructuresEquivalentWeightDelete.IdCPType = eStructuresEquivalentWeightDeleteList.IdCPType;
                            StructuresEquivalentWeightDelete.EquivalentWeight = (float)eStructuresEquivalentWeightDeleteList.EquivalentWeight;
                            StructuresEquivalentWeightDelete.StartDate = eStructuresEquivalentWeightDeleteList.StartDate;
                            StructuresEquivalentWeightDelete.EndDate = eStructuresEquivalentWeightDeleteList.EndDate;
                            FinalModulesEquivalentWeightList.Add(StructuresEquivalentWeightDelete);
                        }
                    }
                }
                 
                ModuleEquivalencyWeight.LstEquivalencyWeight = FinalModulesEquivalentWeightList;
                #region [gulab lakade][GEOS2-4335][13 04 2023]
                AddChangedModulesEquivalentWeightLogDetails(LblEquivalentWeight, FinalModulesEquivalentWeightList);
                #endregion

                ModuleEquivalencyWeight.LstLogEntryByModuleEquivalenceWeight = ModuleEquivalenceWeightChangeLogList.ToList();

              //  ERMService = new ERMServiceController("localhost:6699");
                // IsSave = ERMService.AddUpdateDeleteModulesEquivalencyWeight_V2380(FinalModulesEquivalentWeightList);
                //IsSave = ERMService.AddUpdateDeleteModulesEquivalencyWeight_V2380(ModuleEquivalencyWeight);
                IsSave = ERMService.SaveEquivalencyWeight_V2380(ModuleEquivalencyWeight); // [Rupali Sarode][14/04/2023]

                //ModulesEquivalentWeightList = new ObservableCollection<EquivalencyWeight>();
                //StructuresEquivalentWeightList = new List<EquivalencyWeight>();
                if (IsSave == true)
                {
                    LatestEquivalentWeight = ModulesEquivalentWeightList.Where(i => Convert.ToDateTime(DateTime.Now.ToShortDateString()) >= i.StartDate &&
                   Convert.ToDateTime(DateTime.Now.ToShortDateString()) <= (i.EndDate == null ? Convert.ToDateTime(DateTime.Now.ToShortDateString()) : i.EndDate) && i.TransactionOperation != ModelBase.TransactionOperations.Delete).FirstOrDefault();


                }

                var tempModulesEquivalentWeightList = ModulesEquivalentWeightList.OrderBy(i => i.StartDate);
                bool hasDuplicates = tempModulesEquivalentWeightList.Zip(tempModulesEquivalentWeightList.Skip(1), (current, next) => (Convert.ToDateTime(next.StartDate).Subtract(Convert.ToDateTime(current.EndDate)).TotalHours != 24)).Any(result => result);

                if (hasDuplicates)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ErrorMessageForDategap").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EquivalencyWeightUpdateSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);

                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }



        #region Column Chooser

        private void ModulesEquivalencyWeightTableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ModulesEquivalencyWeightTableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(ModulesEquivalencyWeightGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(ModulesEquivalencyWeightGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(ModulesEquivalencyWeightGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChanged);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }





                GeosApplication.Instance.Logger.Log("Method ModulesEquivalencyWeightTableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ModulesEquivalencyWeightTableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnAllowProperty()...", category: Category.Info, priority: Priority.Low);

                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
                GeosApplication.Instance.Logger.Log("Method OnAllowProperty()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ModulesEquivalencyWeightGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        void VisibleIndexChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(ModulesEquivalencyWeightGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion Column Chooser


        private void DeleteModuleEquivalencyWeightCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteModuleEquivalencyWeightCommandAction()...", category: Category.Info, priority: Priority.Low);

                //[Pallavi Jadhav][Geos-4332][14 04 2023]
                if (LblEquivalentWeight == "Module")
                {

                    EquivalencyWeight MainRow = ((EquivalencyWeight)obj);
                    

                    //MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["ModulesEquivalencyWeightDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    //if (MessageBoxResult == MessageBoxResult.Yes)
                    //{
                        if (!String.IsNullOrEmpty(Convert.ToString(MainRow.EndDate)))
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmptyEndDateErrorMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }


                        ModulesEquivalentWeightDeleteList.Add(MainRow);

                        ModulesEquivalentWeightList.Remove(MainRow);


                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ModulesEquivalencyWeightDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                   // }
                }
                if (LblEquivalentWeight == "Structure")
                {
                    EquivalencyWeight MainRow = ((EquivalencyWeight)obj);
                    
                    //MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["ModulesEquivalencyWeightDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                    //if (MessageBoxResult == MessageBoxResult.Yes)
                    //{
                   if (!String.IsNullOrEmpty(Convert.ToString(MainRow.EndDate)))
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EmptyEndDateErrorMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }


                        StructuresEquivalentWeightDeleteList.Add(MainRow);

                        ModulesEquivalentWeightList.Remove(MainRow);


                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("StructureEquivalencyWeightDeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                  //  }

                }

                    GeosApplication.Instance.Logger.Log("Method DeleteModuleEquivalencyWeightCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteModuleEquivalencyWeightCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void EditModulesEquivalencyWeightAction(object obj)
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); } //[GEOS2-4636][rupali sarode][04-07-2023]

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                EquivalencyWeight SelectedRow = (EquivalencyWeight)detailView.DataControl.CurrentItem;
                var lastDateRecord = ClonedMEW.LstEquivalencyWeight.OrderByDescending(a => a.EndDate).FirstOrDefault();
                var lastDateRecord1 = ClonedMEW.LstEquivalencyWeight.Where(a => a.EndDate != null).OrderByDescending(b => b.EndDate).FirstOrDefault();
                var StartDateRecord = SelectedRow.StartDate;
                var EndDateRecord = SelectedRow.EndDate;
                DateTime dTCurrent = DateTime.Now;
                //  TimeSpan teststartdate = dTCurrent.Subtract((DateTime)EndDateRecord);
                DateTime currentDateValues = Convert.ToDateTime(dTCurrent.ToLongDateString());
                if (currentDateValues <= StartDateRecord || currentDateValues <= EndDateRecord || EndDateRecord == null)
                {

                    GeosApplication.Instance.Logger.Log("Method EditModulesEquivalencyWeightAction()...", category: Category.Info, priority: Priority.Low);
                    //   TableView detailView = (TableView)obj;
                    if (LblEquivalentWeight == "Module")         //[Pallavi Jadhav][Geos-4332][14 04 2023]
                    {

                        EquivalencyWeight modulesEquivalencyWeight = (EquivalencyWeight)detailView.DataControl.CurrentItem;
                        AddEditEquivalentWeightView editModulesEquivalentWeightView = new AddEditEquivalentWeightView();
                        AddEditEquivalentWeightViewModel editModulesEquivalentWeightViewModel = new AddEditEquivalentWeightViewModel();
                        EventHandler handle = delegate { editModulesEquivalentWeightView.Close(); };
                        editModulesEquivalentWeightViewModel.RequestClose += handle;
                        editModulesEquivalentWeightViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditEquivalencyWeight").ToString();
                        editModulesEquivalentWeightViewModel.IsNew = false;
                        //editModulesEquivalentWeightViewModel.ClearAllProperties();
                        editModulesEquivalentWeightViewModel.EditInit(modulesEquivalencyWeight, ClonedMEW, ModulesEquivalentWeightList.Count, ModulesEquivalentWeightList);
                        editModulesEquivalentWeightView.DataContext = editModulesEquivalentWeightViewModel;
                        var ownerInfo = (detailView as FrameworkElement);
                        editModulesEquivalentWeightView.Owner = Window.GetWindow(ownerInfo);
                        editModulesEquivalentWeightView.ShowDialog();

                        if (editModulesEquivalentWeightViewModel.IsSave == true)
                        {

                            SelectedEquivalentWeight = new EquivalencyWeight();
                            SelectedEquivalentWeight = ModulesEquivalentWeightList.FirstOrDefault(i => i.IdCPType == modulesEquivalencyWeight.IdCPType && i.IDCPTypeEquivalent == modulesEquivalencyWeight.IDCPTypeEquivalent);
                            SelectedEquivalentWeight.EquivalentWeight = editModulesEquivalentWeightViewModel.EquivalentWeight;
                            SelectedEquivalentWeight.StartDate = editModulesEquivalentWeightViewModel.StartDate;
                            SelectedEquivalentWeight.EndDate = editModulesEquivalentWeightViewModel.EndDate;



                            foreach (EquivalencyWeight itemEquivalencyWeight in ModulesEquivalentWeightList)
                            {
                                if (itemEquivalencyWeight.IdCPType == modulesEquivalencyWeight.IdCPType && itemEquivalencyWeight.IDCPTypeEquivalent == modulesEquivalencyWeight.IDCPTypeEquivalent)
                                {
                                    itemEquivalencyWeight.EquivalentWeight = editModulesEquivalentWeightViewModel.EquivalentWeight;
                                    itemEquivalencyWeight.StartDate = editModulesEquivalentWeightViewModel.StartDate;
                                    itemEquivalencyWeight.EndDate = editModulesEquivalentWeightViewModel.EndDate;
                                }

                            }

                            // ModulesEquivalentWeightList.Add(SelectedEquivalentWeight);
                            if (UpdateEquivalentWeightList == null)
                            {
                                UpdateEquivalentWeightList = new List<EquivalencyWeight>();
                            }

                            if (UpdateEquivalentWeightList.Any(i => i.IdCPType == modulesEquivalencyWeight.IdCPType && i.IDCPTypeEquivalent == modulesEquivalencyWeight.IDCPTypeEquivalent))
                            {
                                //update list
                                foreach (EquivalencyWeight itemEquivalencyWeight in UpdateEquivalentWeightList)
                                {
                                    if (itemEquivalencyWeight.IdCPType == modulesEquivalencyWeight.IdCPType && itemEquivalencyWeight.IDCPTypeEquivalent == modulesEquivalencyWeight.IDCPTypeEquivalent)
                                    {
                                        itemEquivalencyWeight.EquivalentWeight = editModulesEquivalentWeightViewModel.EquivalentWeight;
                                        itemEquivalencyWeight.StartDate = editModulesEquivalentWeightViewModel.StartDate;
                                        itemEquivalencyWeight.EndDate = editModulesEquivalentWeightViewModel.EndDate;
                                    }

                                }
                            }
                            else
                            {
                                UpdateEquivalentWeightList.Add(SelectedEquivalentWeight);
                            }
                            var lastRecord = ModulesEquivalentWeightList.OrderByDescending(a => a.StartDate).FirstOrDefault();
                            if (lastRecord != null)
                            {
                                EquivalentWeight = (float)(lastRecord.EquivalentWeight);
                                if (lastRecord.StartDate != null)
                                {
                                    StartDate = Convert.ToDateTime(lastRecord.StartDate);
                                }
                                if (lastRecord.EndDate != null)
                                {
                                    EndDate = Convert.ToDateTime(lastRecord.EndDate);
                                }

                            }
                            #region [GEOS2-4331][Rupali Sarode][18-04-2023]


                            #endregion [GEOS2-4331][Rupali Sarode][18-04-2023]
                        }

                    }
                    //[Pallavi Jadhav][Geos-4332][14 04 2023]
                    if (LblEquivalentWeight == "Structure")
                    {

                        EquivalencyWeight structureEquivalencyWeight = (EquivalencyWeight)detailView.DataControl.CurrentItem;
                        AddEditEquivalentWeightView editStructuresEquivalentWeightView = new AddEditEquivalentWeightView();
                        AddEditEquivalentWeightViewModel editStructuresEquivalentWeightViewModel = new AddEditEquivalentWeightViewModel();
                        EventHandler handle = delegate { editStructuresEquivalentWeightView.Close(); };
                        editStructuresEquivalentWeightViewModel.RequestClose += handle;
                        editStructuresEquivalentWeightViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditStructuresEquivalencyWeight").ToString();
                        editStructuresEquivalentWeightViewModel.IsNew = false;
                        //editModulesEquivalentWeightViewModel.ClearAllProperties();
                        editStructuresEquivalentWeightViewModel.EditInit(structureEquivalencyWeight, ClonedMEW, ModulesEquivalentWeightList.Count, ModulesEquivalentWeightList);
                        editStructuresEquivalentWeightView.DataContext = editStructuresEquivalentWeightViewModel;
                        var ownerInfo = (detailView as FrameworkElement);
                        editStructuresEquivalentWeightView.Owner = Window.GetWindow(ownerInfo);
                        editStructuresEquivalentWeightView.ShowDialog();

                        if (editStructuresEquivalentWeightViewModel.IsSave == true)
                        {

                            SelectedStructuresEquivalentWeight = new EquivalencyWeight();
                            SelectedStructuresEquivalentWeight = ModulesEquivalentWeightList.FirstOrDefault(i => i.IdCPType == structureEquivalencyWeight.IdCPType && i.IDCPTypeEquivalent == structureEquivalencyWeight.IDCPTypeEquivalent);
                            SelectedStructuresEquivalentWeight.EquivalentWeight = editStructuresEquivalentWeightViewModel.EquivalentWeight;
                            SelectedStructuresEquivalentWeight.StartDate = editStructuresEquivalentWeightViewModel.StartDate;
                            SelectedStructuresEquivalentWeight.EndDate = editStructuresEquivalentWeightViewModel.EndDate;
                            // ModulesEquivalentWeightList.Add(SelectedEquivalentWeight);

                            foreach (EquivalencyWeight itemEquivalencyWeight in ModulesEquivalentWeightList)
                            {
                                if (itemEquivalencyWeight.IdCPType == structureEquivalencyWeight.IdCPType && itemEquivalencyWeight.IDCPTypeEquivalent == structureEquivalencyWeight.IDCPTypeEquivalent)
                                {
                                    itemEquivalencyWeight.EquivalentWeight = editStructuresEquivalentWeightViewModel.EquivalentWeight;
                                    itemEquivalencyWeight.StartDate = editStructuresEquivalentWeightViewModel.StartDate;
                                    itemEquivalencyWeight.EndDate = editStructuresEquivalentWeightViewModel.EndDate;
                                }

                            }

                            if (UpdateStructuresEquivalentWeightList == null)
                            {
                                UpdateStructuresEquivalentWeightList = new List<EquivalencyWeight>();
                            }
                            if (UpdateEquivalentWeightList == null)
                            {
                                UpdateEquivalentWeightList = new List<EquivalencyWeight>();
                            }
                            if (UpdateEquivalentWeightList == null)
                            {
                                UpdateEquivalentWeightList = new List<EquivalencyWeight>();
                            }

                            if (UpdateEquivalentWeightList.Any(i => i.IdCPType == structureEquivalencyWeight.IdCPType && i.IDCPTypeEquivalent == structureEquivalencyWeight.IDCPTypeEquivalent))
                            {
                                //update list
                                foreach (EquivalencyWeight itemEquivalencyWeight in UpdateEquivalentWeightList)
                                {
                                    if (itemEquivalencyWeight.IdCPType == structureEquivalencyWeight.IdCPType && itemEquivalencyWeight.IDCPTypeEquivalent == structureEquivalencyWeight.IDCPTypeEquivalent)
                                    {
                                        itemEquivalencyWeight.EquivalentWeight = editStructuresEquivalentWeightViewModel.EquivalentWeight;
                                        itemEquivalencyWeight.StartDate = editStructuresEquivalentWeightViewModel.StartDate;
                                        itemEquivalencyWeight.EndDate = editStructuresEquivalentWeightViewModel.EndDate;
                                    }
                                }
                            }
                            else
                            {
                                UpdateStructuresEquivalentWeightList.Add(SelectedStructuresEquivalentWeight);
                            }
                            var lastRecord = ModulesEquivalentWeightList.OrderByDescending(a => a.StartDate).FirstOrDefault();
                            if (lastRecord != null)
                            {
                                EquivalentWeight = (float)(lastRecord.EquivalentWeight);
                                if (lastRecord.StartDate != null)
                                {
                                    StartDate = Convert.ToDateTime(lastRecord.StartDate);
                                }
                                if (lastRecord.EndDate != null)
                                {
                                    EndDate = Convert.ToDateTime(lastRecord.EndDate);
                                }

                            }
                        }

                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditEmptyEndDateErrorMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Method EditModulesEquivalencyWeightAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditModulesEquivalencyWeightAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        public void EditInitStructures(ModulesEquivalencyWeight tSelectedStructuersEqivalentWeight)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInitStructures()...", category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = ERMCommon.Instance.SetMaximizedElementPosition();//[Aishwarya Ingale][4920][18/12/2023]
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                ModulesEquivalentWeightList = new ObservableCollection<EquivalencyWeight>();

                SelectedIdCpType = tSelectedStructuersEqivalentWeight.IdCPType; //[GEOS2-4331][Rupali Sarode][18-04-2023]

                // ERMService = new ERMServiceController("localhost:6699");
                if (tSelectedStructuersEqivalentWeight != null)
                {
                    //ModuleEquivalencyWeight = ERMService.GetProductTypesEquivalencyWeightByCPType_V2380(tSelectedModuleEqivalentWeight.IdCPType);
                    //ModulesEquivalencyWeight temp = ERMService.GetProductTypesEquivalencyWeightByCPType_V2380(tSelectedStructuersEqivalentWeight.IdCPType);
                    //[Rupali Sarode][GEOS2-4330][14/04/2023]
                    ModulesEquivalencyWeight temp = ERMService.GetStructuresEquivalencyWeightByCPType_V2380(tSelectedStructuersEqivalentWeight.IdCPType);

                    //ModulesEquivalentWeightList = temp.LstEquivalencyWeight;
                    if (temp.LstEquivalencyWeight != null)
                    {
                        ModulesEquivalentWeightList = new ObservableCollection<EquivalencyWeight>(temp.LstEquivalencyWeight);
                    }
                    #region [GEOS2-4335][Rupali Sarode][21-04-2023]
                    if (temp.LstLogEntryByModuleEquivalenceWeight != null)
                    {
                        ModuleEquivalenceWeightAllChangeLogList = new ObservableCollection<LogEntryByModuleEquivalenceWeight>(temp.LstLogEntryByModuleEquivalenceWeight.OrderByDescending(x => x.IdLogEntryByMEW).ToList());
                    }
                    #endregion

                    #region [gulab lakade][GEOS2-4333][12 04 2023]
                    ClonedMEW = (ModulesEquivalencyWeight)temp.Clone();
                    //ModulesEquivalentWeightList = ClonedMEW.LstEquivalencyWeight;
                    var lastRecord = ClonedMEW.LstEquivalencyWeight.OrderByDescending(a => a.StartDate).FirstOrDefault();
                    if (lastRecord != null)
                    {
                        EquivalentWeight = (float)(lastRecord.EquivalentWeight);
                        if (lastRecord.StartDate != null)
                        {
                            StartDate = Convert.ToDateTime(lastRecord.StartDate);
                        }
                        if (lastRecord.EndDate != null)
                        {
                            EndDate = Convert.ToDateTime(lastRecord.EndDate);
                        }

                    }
                    #endregion
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]

                GeosApplication.Instance.Logger.Log("Method EditInitStructures()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Error in Method EditInitStructures()...." + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        #region [gulab lakade][GEOS2-4335][13 04 2023]
        public void AddChangedModulesEquivalentWeightLogDetails(string LblEquivalentWeight, List<EquivalencyWeight> FinalModulesEquivalentWeightList)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddChangedModulesEquivalentWeightLogDetails()...", category: Category.Info, priority: Priority.Low);

                if (LblEquivalentWeight == "Module")
                {

                    if (FinalModulesEquivalentWeightList.Count > 0)
                    {
                        foreach (EquivalencyWeight item in FinalModulesEquivalentWeightList)
                        {

                            if (Convert.ToString(item.TransactionOperation) == "Add")
                            {
                                if (string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                {
                                    string log = "Equivalent weight " + Convert.ToString(item.EquivalentWeight) + " has been added from " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + ".";
                                    ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                                else
                                {
                                    string log = "Equivalent weight " + Convert.ToString(item.EquivalentWeight) + " has been added in between " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(item.EndDate).ToShortDateString()) + ".";
                                    ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }

                                
                            }

                            if (Convert.ToString(item.TransactionOperation) == "Delete")
                            {
                                string log = "Equivalent weight " + Convert.ToString(item.EquivalentWeight) + " has been deleted in between " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(DateTime.Now).ToShortDateString()) + ".";
                                ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                            else if (Convert.ToString(item.TransactionOperation) == "Update")
                            {
                                var tempOldRecord = ClonedMEW.LstEquivalencyWeight.Where(x => x.IDCPTypeEquivalent == item.IDCPTypeEquivalent && x.IdCPType == item.IdCPType).FirstOrDefault();
                                if(tempOldRecord!=null)
                                {
                                    if(Convert.ToString(tempOldRecord.EquivalentWeight)!=Convert.ToString(item.EquivalentWeight))
                                    {
                                        string log = string.Empty;
                                        if (string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EquivalentWeight)) && !string.IsNullOrEmpty(Convert.ToString(item.EquivalentWeight)))
                                        {
                                             log = "Equivalent weight has been changed from None to " + Convert.ToString(item.EquivalentWeight) + ".";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EquivalentWeight)) && string.IsNullOrEmpty(Convert.ToString(item.EquivalentWeight)))
                                        {
                                            log = "Equivalent weight has been changed from " + Convert.ToString(tempOldRecord.EquivalentWeight) + " to None.";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EquivalentWeight)) && !string.IsNullOrEmpty(Convert.ToString(item.EquivalentWeight)))
                                        {
                                            log = "Equivalent weight " + Convert.ToString(tempOldRecord.EquivalentWeight) + " has been changed to equivalent weight " + Convert.ToString(item.EquivalentWeight) + ".";
                                        }
                                        if(!string.IsNullOrEmpty(log))
                                        {
                                            ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }
                                       
                                    }
                                    if (Convert.ToString(tempOldRecord.StartDate) != Convert.ToString(item.StartDate))
                                    {
                                        string log = string.Empty;
                                        if (string.IsNullOrEmpty(Convert.ToString(tempOldRecord.StartDate)) && !string.IsNullOrEmpty(Convert.ToString(item.StartDate)))
                                        {
                                            log = "Equivalent start date has been changed from None to " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + ".";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.StartDate)) && string.IsNullOrEmpty(Convert.ToString(item.StartDate)))
                                        {
                                            log = "Equivalent start date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.StartDate).ToShortDateString()) + " to None.";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.StartDate)) && !string.IsNullOrEmpty(Convert.ToString(item.StartDate)))
                                        {
                                            log = "Equivalent start date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.StartDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + ".";
                                        }
                                        if (!string.IsNullOrEmpty(log))
                                        {
                                            ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }

                                    }
                                    if (Convert.ToString(tempOldRecord.EndDate) != Convert.ToString(item.EndDate))
                                    {
                                        string log = string.Empty;
                                        if (string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EndDate)) && !string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                        {
                                            log = "Equivalent end date has been changed from None to " + Convert.ToString(Convert.ToDateTime(item.EndDate).ToShortDateString()) + ".";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EndDate)) && string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                        {
                                            log = "Equivalent end date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.EndDate).ToShortDateString()) + " to None.";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EndDate)) && !string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                        {
                                            log = "Equivalent end date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.EndDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(item.EndDate).ToShortDateString()) + ".";
                                        }
                                        if (!string.IsNullOrEmpty(log))
                                        {
                                            ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }

                                    }
                                }

                            }
                        }
                    }

                }
                else
                    if (LblEquivalentWeight == "Structure")
                {
                    if (FinalModulesEquivalentWeightList.Count > 0)
                    {
                        foreach (EquivalencyWeight item in FinalModulesEquivalentWeightList)
                        {
                            if (Convert.ToString(item.TransactionOperation) == "Add")
                            {
                                if (string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                {
                                    string log = "Equivalent weight " + Convert.ToString(item.EquivalentWeight) + " has been added from " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + ".";
                                    ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                                else
                                {
                                    string log = "Equivalent weight " + Convert.ToString(item.EquivalentWeight) + " has been added in between " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(item.EndDate).ToShortDateString()) + ".";
                                    ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                }
                            }

                            if (Convert.ToString(item.TransactionOperation) == "Delete")
                            {
                                string log = "Equivalent weight " + Convert.ToString(item.EquivalentWeight) + " has been deleted in between " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(DateTime.Now).ToShortDateString()) + ".";
                                ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                            }
                            else if (Convert.ToString(item.TransactionOperation) == "Update")
                            {
                                var tempOldRecord = ClonedMEW.LstEquivalencyWeight.Where(x => x.IDCPTypeEquivalent == item.IDCPTypeEquivalent && x.IdCPType == item.IdCPType).FirstOrDefault();
                                if (tempOldRecord != null)
                                {
                                    if (Convert.ToString(tempOldRecord.EquivalentWeight) != Convert.ToString(item.EquivalentWeight))
                                    {
                                        string log = string.Empty;
                                        if (string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EquivalentWeight)) && !string.IsNullOrEmpty(Convert.ToString(item.EquivalentWeight)))
                                        {
                                            log = "Equivalent weight has been changed from None to " + Convert.ToString(item.EquivalentWeight) + ".";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EquivalentWeight)) && string.IsNullOrEmpty(Convert.ToString(item.EquivalentWeight)))
                                        {
                                            log = "Equivalent weight has been changed from " + Convert.ToString(tempOldRecord.EquivalentWeight) + " to None.";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EquivalentWeight)) && !string.IsNullOrEmpty(Convert.ToString(item.EquivalentWeight)))
                                        {
                                            log = "Equivalent weight " + Convert.ToString(tempOldRecord.EquivalentWeight) + " has been changed to equivalent weight " + Convert.ToString(item.EquivalentWeight) + ".";
                                        }
                                        if (!string.IsNullOrEmpty(log))
                                        {
                                            ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }

                                    }
                                    if (Convert.ToString(tempOldRecord.StartDate) != Convert.ToString(item.StartDate))
                                    {
                                        string log = string.Empty;
                                        if (string.IsNullOrEmpty(Convert.ToString(tempOldRecord.StartDate)) && !string.IsNullOrEmpty(Convert.ToString(item.StartDate)))
                                        {
                                            log = "Equivalent start date has been changed from None to " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + ".";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.StartDate)) && string.IsNullOrEmpty(Convert.ToString(item.StartDate)))
                                        {
                                            log = "Equivalent start date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.StartDate).ToShortDateString()) + " to None.";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.StartDate)) && !string.IsNullOrEmpty(Convert.ToString(item.StartDate)))
                                        {
                                            log = "Equivalent start date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.StartDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(item.StartDate).ToShortDateString()) + ".";
                                        }
                                        if (!string.IsNullOrEmpty(log))
                                        {
                                            ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }

                                    }
                                    if (Convert.ToString(tempOldRecord.EndDate) != Convert.ToString(item.EndDate))
                                    {
                                        string log = string.Empty;
                                        if (string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EndDate)) && !string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                        {
                                            log = "Equivalent end date has been changed from None to " + Convert.ToString(Convert.ToDateTime(item.EndDate).ToShortDateString()) + ".";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EndDate)) && string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                        {
                                            log = "Equivalent end date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.EndDate).ToShortDateString()) + " to None.";
                                        }
                                        else if (!string.IsNullOrEmpty(Convert.ToString(tempOldRecord.EndDate)) && !string.IsNullOrEmpty(Convert.ToString(item.EndDate)))
                                        {
                                            log = "Equivalent end date has been changed from " + Convert.ToString(Convert.ToDateTime(tempOldRecord.EndDate).ToShortDateString()) + " to " + Convert.ToString(Convert.ToDateTime(item.EndDate).ToShortDateString()) + ".";
                                        }
                                        if (!string.IsNullOrEmpty(log))
                                        {
                                            ModuleEquivalenceWeightChangeLogList.Add(new LogEntryByModuleEquivalenceWeight() { IdCPType = Convert.ToInt32(item.IdCPType), Datetime = GeosApplication.Instance.ServerDateTime, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Comments = log });
                                        }

                                    }
                                }

                            }
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method AddChangedModulesEquivalentWeightLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AddChangedModulesEquivalentWeightLogDetails()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region [GEOS2-4331][Rupali Sarode][18-04-2023]
        void AddButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]

                if (ClonedMEW.LstEquivalencyWeight== null)
                {
                    ClonedMEW.LstEquivalencyWeight = new List<EquivalencyWeight>();
                }
                // var lastDateRecord = ClonedMEW.LstEquivalencyWeight.OrderByDescending(a => a.StartDate).FirstOrDefault(); 
                var lastDateRecord = ModulesEquivalentWeightList.OrderByDescending(a => a.StartDate).FirstOrDefault();
                if (lastDateRecord != null )
                {
                    if (lastDateRecord.EndDate == null)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddEmptyEndDateErrorMessage").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    //lastDateRecord = new EquivalencyWeight() ;
                    //lastDateRecord.EndDate = DateTime.Now;
                }
                
                
                
                //  EquivalencyWeight structureEquivalencyWeight = (EquivalencyWeight)detailView.DataControl.CurrentItem;
                AddEditEquivalentWeightView editModulesEquivalentWeightView = new AddEditEquivalentWeightView();
                AddEditEquivalentWeightViewModel editModulesEquivalentWeightViewModel = new AddEditEquivalentWeightViewModel();
                EventHandler handle = delegate { editModulesEquivalentWeightView.Close(); };
                editModulesEquivalentWeightViewModel.RequestClose += handle;
                editModulesEquivalentWeightViewModel.WindowHeader = System.Windows.Application.Current.FindResource("ADDEquivalencyWeight").ToString();
                editModulesEquivalentWeightViewModel.IsNew = true;
                //editModulesEquivalentWeightViewModel.ClearAllProperties();
                // editModulesEquivalentWeightViewModel.AddInit(modulesEquivalencyWeight, ClonedMEW);
                editModulesEquivalentWeightViewModel.AddInit(ClonedMEW, ModulesEquivalentWeightList);
                editModulesEquivalentWeightView.DataContext = editModulesEquivalentWeightViewModel;
                //   var ownerInfo = (detailView as FrameworkElement);
                //   editModulesEquivalentWeightView.Owner = Window.GetWindow(ownerInfo);
                editModulesEquivalentWeightView.ShowDialog();

                if (editModulesEquivalentWeightViewModel.IsSave == true)
                {

                    SelectedEquivalentWeight = new EquivalencyWeight();
                    
                    SelectedEquivalentWeight.EquivalentWeight = editModulesEquivalentWeightViewModel.EquivalentWeight;
                    SelectedEquivalentWeight.StartDate = editModulesEquivalentWeightViewModel.StartDate;
                    SelectedEquivalentWeight.EndDate = editModulesEquivalentWeightViewModel.EndDate;
                    SelectedEquivalentWeight.IdCPType = SelectedIdCpType;

                    if (LblEquivalentWeight == "Module")
                    {
                        ModulesEquivalentWeightList.Add(SelectedEquivalentWeight);

                        if (AddEquivalentWeightList == null)
                        {
                            AddEquivalentWeightList = new List<EquivalencyWeight>();
                        }

                        AddEquivalentWeightList.Add(SelectedEquivalentWeight);
                        var lastRecord = AddEquivalentWeightList.OrderByDescending(a => a.StartDate).FirstOrDefault();
                        if (lastRecord != null)
                        {
                            EquivalentWeight = (float)(lastRecord.EquivalentWeight);
                            if (lastRecord.StartDate != null)
                            {
                                StartDate = Convert.ToDateTime(lastRecord.StartDate);
                            }
                            if (lastRecord.EndDate != null)
                            {
                                EndDate = Convert.ToDateTime(lastRecord.EndDate);
                            }

                        }
                    }
                    else if(LblEquivalentWeight == "Structure")
                    {
                        ModulesEquivalentWeightList.Add(SelectedEquivalentWeight);
                        if (AddStructuresEquivalentWeightList == null)
                        {
                            AddStructuresEquivalentWeightList = new List<EquivalencyWeight>();
                        }
                        AddStructuresEquivalentWeightList.Add(SelectedEquivalentWeight);
                        var lastRecord = AddStructuresEquivalentWeightList.OrderByDescending(a => a.StartDate).FirstOrDefault();
                        if (lastRecord != null)
                        {
                            EquivalentWeight = (float)(lastRecord.EquivalentWeight);
                            if (lastRecord.StartDate != null)
                            {
                                StartDate = Convert.ToDateTime(lastRecord.StartDate);
                            }
                            if (lastRecord.EndDate != null)
                            {
                                EndDate = Convert.ToDateTime(lastRecord.EndDate);
                            }

                        }
                    }
                   
                }
                //ObservableCollection<EquivalencyWeight> TempModulesEquivalentWeightList = new ObservableCollection<EquivalencyWeight>();
                //TempModulesEquivalentWeightList.AddRange(ModulesEquivalentWeightList.OrderBy(sort => sort.StartDate).ToList());
                //ModulesEquivalentWeightList = TempModulesEquivalentWeightList;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Method AddButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion [GEOS2-4331][Rupali Sarode][18-04-2023]



        #endregion Methods



    }
}
