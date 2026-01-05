using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Modules.ERM.Views;
using Emdep.Geos.UI.Common;
using DevExpress.Xpf.Core;
using System.Windows;
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
using Microsoft.Win32;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    public class ModuleEquivalencyWeightViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service

        IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RequestClose;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Declarations

        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }
        private DataTable dataTable;
        private DataTable dataTableForGridLayout;
       
        private ObservableCollection<ModulesEquivalencyWeight> modulesEquivalencyWeightList;
        private ModulesEquivalencyWeight modulesEquivalencyWeight;
        private ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> columns;

        private ObservableCollection<TileBarFilters> listofitem;
        private List<TileBarFilters> tempListofitem;
        private int selectedTileIndexEquivalencyWeight;
        public ObservableCollection<Summary> TotalSummary { get; private set; }
        private string userSettingsKey = "ERM_Module_Structuers_Equivalency_Weight ";
        public string EquivalencyWeightGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "EquivalencyWeightGridSetting.Xml";
        private string myFilterString;
        private TileBarFilters selectedFilter;
        private List<GridColumn> GridColumnList;
        private ObservableCollection<ModulesEquivalencyWeight> tempModulesEquivalencyWeightList;
        private bool isEdit;
        private int visibleRowCount;
        private bool isEquivalencyWeightColumnChooserVisible;
        private ModulesEquivalencyWeight selectedModulesEquivalentWeight;
        //private ModulesEquivalencyWeight selectedModuleEquivalentWeight; //[GEOS2-4330][Rupali Sarode][06-04-2023]
        #endregion Declarations

        #region Properties
        public ObservableCollection<ModulesEquivalencyWeight> ModulesEquivalencyWeightList
        {
            get
            {
                return modulesEquivalencyWeightList;
            }
            set
            {
                modulesEquivalencyWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModulesEquivalencyWeightList"));
            }
        }
        public ModulesEquivalencyWeight ModulesEquivalencyWeight
        {
            get
            {
                return modulesEquivalencyWeight;
            }
            set
            {
                modulesEquivalencyWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModulesEquivalencyWeight"));
            }
        }


        public DataTable DataTable
        {
            get { return dataTable; }
            set
            {
                dataTable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTable"));
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


        
        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnItem> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public ObservableCollection<TileBarFilters> Listofitem
        {
            get
            {
                return listofitem;
            }

            set
            {
                listofitem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Listofitem"));
            }
        }
        public List<TileBarFilters> TempListofitem
        {
            get
            {
                return tempListofitem;
            }

            set
            {
                tempListofitem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempListofitem"));
            }
        }
        public int SelectedTileIndexEquivalencyWeight
        {
            get
            {
                return selectedTileIndexEquivalencyWeight;
            }

            set
            {
                selectedTileIndexEquivalencyWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTileIndexEquivalencyWeight"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                if (myFilterString == "")
                {
                    SelectedFilter = Listofitem.FirstOrDefault();
                }
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public TileBarFilters SelectedFilter
        {
            get { return selectedFilter; }
            set
            {
                selectedFilter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFilter"));
            }
        }

         public string CustomFilterStringName { get; set; }

        public ObservableCollection<ModulesEquivalencyWeight> TempModulesEquivalencyWeightList
        {
            get
            {
                return tempModulesEquivalencyWeightList;
            }
            set
            {
                tempModulesEquivalencyWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempModulesEquivalencyWeightList"));
            }
        }
        public bool IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }

        public int VisibleRowCount
        {
            get { return visibleRowCount; }
            set
            {
                visibleRowCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("VisibleRowCount"));
            }
        }

        public bool IsEquivalencyWeightColumnChooserVisible
        {
            get
            {
                return isEquivalencyWeightColumnChooserVisible;
            }

            set
            {
                isEquivalencyWeightColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEquivalencyWeightColumnChooserVisible"));
            }
        }

        ////[GEOS2-4330][Rupali Sarode][06-04-2023]
        //public ModulesEquivalencyWeight SelectedModuleEquivalentWeight
        //{
        //    get
        //    {
        //        return selectedModuleEquivalentWeight;
        //    }
        //    set
        //    {
        //        selectedModuleEquivalentWeight = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedModuleEquivalentWeight"));
        //    }
        //}

        public ModulesEquivalencyWeight SelectedModulesEquivalentWeight
        {
            get
            {
                return selectedModulesEquivalentWeight;
            }

            set
            {
                selectedModulesEquivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedModulesEquivalentWeight"));
            }
        }
        
        #endregion

        #region ICommands
        public ICommand CommandEquivalencyWeightShowTileBarFilterPopupClick { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand PrintEquivalencyWeightCommand { get; set; }
        public ICommand ExportEquivalencyWeightCommand { get; set; }
        public ICommand EquivalencyWeightGridControlLoadedCommand { get; set; }
        public ICommand EquivalencyWeightItemListTableViewLoadedCommand { get; set; }
        public ICommand EquivalencyWeightGridControlUnloadedCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand EquivalencyWeightRefreshCommand { get; set; }

        //[GEOS2-4330][Rupali Sarode][06-04-2023]
        public ICommand ModuleEquivalencyWeightHyperlinkCommand { get; set; }
        #endregion

        #region Constructor
        public ModuleEquivalencyWeightViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ModuleEquivalencyWeightViewModel ...", category: Category.Info, priority: Priority.Low);

                CommandEquivalencyWeightShowTileBarFilterPopupClick = new DelegateCommand<object>(ShowSelectedFilterEquivalencyWeightGridAction);
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                PrintEquivalencyWeightCommand = new RelayCommand(new Action<object>(PrintEquivalencyWeightAction));
                ExportEquivalencyWeightCommand = new RelayCommand(new Action<object>(ExportEquivalencyWeightAction));
                //EquivalencyWeightGridControlLoadedCommand = new DelegateCommand<object>(EquivalencyWeightGridControlLoadedAction);
                //EquivalencyWeightItemListTableViewLoadedCommand = new DelegateCommand<object>(EquivalencyWeightItemListTableViewLoadedAction);
                EquivalencyWeightGridControlUnloadedCommand = new DelegateCommand<object>(EquivalencyWeightGridControlUnloadedCommandAction);
                EquivalencyWeightRefreshCommand = new RelayCommand(new Action<object>(EquivalencyWeightRefreshCommandAction));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
                // CommandShowFilterPopupClick = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                //[GEOS2-4330][Rupali Sarode][06-04-2023]
                ModuleEquivalencyWeightHyperlinkCommand = new DelegateCommand<object>(ModuleEquivalencyWeightHyperlinkCommandAction);

                
               
                GeosApplication.Instance.Logger.Log("Constructor ModuleEquivalencyWeightViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor ModuleEquivalencyWeightViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                ModulesEquivalencyWeightList = new ObservableCollection<ModulesEquivalencyWeight>();
              // ERMService = new ERMServiceController("localhost:6699");
                ModulesEquivalencyWeightList.AddRange(ERMService.GetAllProductTypesEquivalencyWeight_V2380());
                ModulesEquivalencyWeightList.Where(i => i.IdCPType == 1).ToList().ForEach(a=>a.FlagIdCPType=true);
                
                TileBarArrange(ModulesEquivalencyWeightList);
                AddCustomSetting();
              //  MyFilterString = string.Empty;
                TempModulesEquivalencyWeightList = ModulesEquivalencyWeightList;
                MyFilterString = string.Empty; // [GEOS2-4547][Rupali Sarode][06-06-2023]
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void TileBarArrange(ObservableCollection<ModulesEquivalencyWeight> templateMenulist)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TileBarArrange...", category: Category.Info, priority: Priority.Low);

                Listofitem = new ObservableCollection<TileBarFilters>();

                Listofitem.Add(new TileBarFilters()
                {
                    Caption = "All",
                    DisplayText = "All",
                    EntitiesCount = ModulesEquivalencyWeightList.Count(),
                    EntitiesCountVisibility = Visibility.Visible,
                    Height = 80,
                    width = 200
                });

                if (templateMenulist == null)
                {
                    templateMenulist = new ObservableCollection<ModulesEquivalencyWeight>();
                }
                List<ModulesEquivalencyWeight> TempTemplateList = templateMenulist.Where(a => a.IdTemplate == 0 || a.IdTemplate != 24).ToList();
                if (TempTemplateList != null)
                {
                    foreach (ModulesEquivalencyWeight template in TempTemplateList)
                    {
                        if (!Listofitem.Any(a => a.DisplayText == template.TemplateName))
                        {
                            Listofitem.Add(new TileBarFilters()
                            {
                                Caption = template.TemplateName,
                                DisplayText = template.TemplateName,
                                EntitiesCount = ModulesEquivalencyWeightList.Count(x => x.TemplateName == template.TemplateName && x.TemplateName != null),
                                EntitiesCountVisibility = Visibility.Visible,
                                BackColor = template.Color,
                                FilterCriteria = "[TemplateName] In ('" + template.TemplateName + "')",
                                Height = 80,
                                width = 200
                            });
                        }
                       
                    }
                    ObservableCollection<TileBarFilters> TempListofitem1 = new ObservableCollection<TileBarFilters>();
                    TempListofitem1.AddRange(Listofitem.OrderBy(sort => sort.DisplayText).ToList());
                    Listofitem = TempListofitem1;
                }

                Listofitem.Add(new TileBarFilters()
                {


                    Caption = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    FilterCriteria = (System.Windows.Application.Current.FindResource("CustomFilters").ToString()),
                    Height = 30,
                    width = 200
                });

                //Listofitem.OrderBy(x => x.DisplayText == template.TemplateName);
                // After change index it will automatically redirect to method ShowSelectedFilterGridAction(object obj) and arrange the tile section count.
                if (Listofitem.Count > 0)
                    SelectedTileIndexEquivalencyWeight = 0;
                //ObservableCollection<TileBarFilters>  TempListofitem1 = new ObservableCollection<TileBarFilters>();
                //TempListofitem1.AddRange(Listofitem.OrderBy(sort=> sort.DisplayText).ToList());
                //Listofitem = TempListofitem1;
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in TileBarArrange() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();

                if (tempUserSettings != null)
                {
                    foreach (var item in tempUserSettings)
                    {
                        try
                        {
                            string filter = item.Value.Replace("[Status]", "Status");
                            CriteriaOperator op = CriteriaOperator.Parse(filter);
                                                       
                            Listofitem.Add(
                                     new TileBarFilters()
                                     {
                                         Caption = item.Key.Replace(userSettingsKey, ""),
                                         Id = 0,
                                         BackColor = null,
                                         ForeColor = null,
                                         FilterCriteria = item.Value,
                                         EntitiesCountVisibility = Visibility.Visible,
                                         Height = 80,
                                         width = 200
                                     });
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void ShowSelectedFilterEquivalencyWeightGridAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterEquivalencyWeightGridAction....", category: Category.Info, priority: Priority.Low);

                if (Listofitem.Count > 0)
                {
                    var temp = (System.Windows.Controls.SelectionChangedEventArgs)obj;
                    if (temp.AddedItems.Count > 0)
                    {
                        string str = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).DisplayText;
                        string Template = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Type;
                        string _FilterString = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).FilterCriteria;
                        CustomFilterStringName = ((TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0]).Caption;


                        if (CustomFilterStringName.Equals(System.Windows.Application.Current.FindResource("CustomFilters").ToString()))
                            return;


                        if (str == null)
                        {
                            if (!string.IsNullOrEmpty(_FilterString))
                            {

                                if (!string.IsNullOrEmpty(_FilterString))
                                    MyFilterString = _FilterString;
                                else
                                    MyFilterString = string.Empty;
                            }
                            else
                                MyFilterString = string.Empty;
                        }
                        else
                        {
                            if (str.Equals("All"))
                            {
                                MyFilterString = string.Empty;
                                ModulesEquivalencyWeightList = TempModulesEquivalencyWeightList;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(_FilterString))
                                {

                                    if (!string.IsNullOrEmpty(_FilterString))
                                        MyFilterString = _FilterString;
                                    else
                                        MyFilterString = string.Empty;
                                }
                                else
                                    MyFilterString = string.Empty;
                            }
                        }

                    }
                }

                GeosApplication.Instance.Logger.Log("Method ShowSelectedFilterEquivalencyWeightGridAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowSelectedFilterEquivalencyWeightGridAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void AddCustomSettingCount(GridControl gridControl)
        {
            try
            {
                List<KeyValuePair<string, string>> tempUserSettings = new List<KeyValuePair<string, string>>();
                tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                foreach (var item in tempUserSettings)
                {
                    try
                    {
                        MyFilterString = Listofitem.FirstOrDefault(j => j.FilterCriteria == item.Value).FilterCriteria;
                        Listofitem.FirstOrDefault(j => j.FilterCriteria == item.Value).EntitiesCount = (int)gridControl.View.FixedSummariesLeft[0].Value;
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSettingCount() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }
                }
                MyFilterString = string.Empty;
                GeosApplication.Instance.Logger.Log("Method AddCustomSettingCount() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSettingCount() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                foreach (var item in ModulesEquivalencyWeightList)
                {
                    if (CustomFilterStringName != null)
                    {
                        if (CustomFilterStringName.Equals(item.Name))
                        {
                            return;
                        }
                    }
                }

                if (CustomFilterStringName == "CUSTOM FILTERS" || CustomFilterStringName == "All")
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.Header.ToString().Equals("Template"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FilterEditorCreatedCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                obj.Handled = true;
                TableView table = (TableView)obj.OriginalSource;
                GridControl gridControl = (table).Grid;
                ShowFilterEditor(obj);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Method FilterEditorCreatedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }  //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log("Get an error in Method FilterEditorCreatedCommandAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);

            }
        }
        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                CustomFilterEditorView customFilterEditorView = new CustomFilterEditorView();
                CustomFilterEditorViewModel customFilterEditorViewModel = new CustomFilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    customFilterEditorViewModel.FilterName = CustomFilterStringName;
                    customFilterEditorViewModel.IsSave = true;
                    customFilterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    customFilterEditorViewModel.IsNew = true;

                customFilterEditorViewModel.Init(e.FilterControl, Listofitem);
                customFilterEditorView.DataContext = customFilterEditorViewModel;
                EventHandler handle = delegate { customFilterEditorView.Close(); };
                customFilterEditorViewModel.RequestClose += handle;
                customFilterEditorView.Title = titleText;
                customFilterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                customFilterEditorView.Grid.Children.Add(e.FilterControl);
                customFilterEditorView.ShowDialog();

                if (customFilterEditorViewModel.IsDelete && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName))
                {
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));

                    if (tileBarItem != null)
                    {
                        Listofitem.Remove(tileBarItem);
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");



                            if (!key.Equals(tileBarItem.Caption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (!string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && !customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = Listofitem.FirstOrDefault(x => x.Caption.Equals(CustomFilterStringName));
                    if (tileBarItem != null)
                    {
                        CustomFilterStringName = customFilterEditorViewModel.FilterName;
                        TableView table = (TableView)e.OriginalSource;
                        GridControl gridControl = (table).Grid;
                        VisibleRowCount = gridControl.VisibleRowCount;
                        //List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.FixedSummariesLeft);
                        //VisibleRowCount = (Int32)summary.FirstOrDefault().Value;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = customFilterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = VisibleRowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = customFilterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>((userSettingsKey + tileBarItem.Caption), tileBarItem.FilterCriteria));


                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (customFilterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(customFilterEditorViewModel.FilterName) && customFilterEditorViewModel.IsSave && customFilterEditorViewModel.IsNew && customFilterEditorViewModel.IsCancel)
                {
                    
                    TableView table = (TableView)e.OriginalSource;
                    GridControl gridControl = (table).Grid;
                    //List<DevExpress.Xpf.Grid.GridTotalSummaryData> summary = new List<GridTotalSummaryData>(gridControl.View.tot);
                    //GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    //List<DevExpress.Xpf.Grid.GridSummaryItem> summuries = new List<DevExpress.Xpf.Grid.GridSummaryItem>(gridControl.TotalSummary);
                    
                    VisibleRowCount = gridControl.VisibleRowCount;

                    Listofitem.Add(new TileBarFilters()
                    {
                        Caption = customFilterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = customFilterEditorViewModel.FilterCriteria,
                        Height = 80,
                        width = 200,
                        EntitiesCount = VisibleRowCount
                    });

                    string filterName = "";

                    filterName = userSettingsKey + customFilterEditorViewModel.FilterName;

                    GeosApplication.Instance.UserSettings[filterName] = customFilterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    SelectedFilter = Listofitem.LastOrDefault();
                }

                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void PrintEquivalencyWeightAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintEquivalencyWeightAction()...", category: Category.Info, priority: Priority.Low);
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

                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["LeavesReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintEquivalencyWeightAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintEquivalencyWeightAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void ExportEquivalencyWeightAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportEquivalencyWeightAction()...", category: Category.Info, priority: Priority.Low);
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "ModulesEquivalencyWeight";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Modules Equivalency Weight Report";
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
                    TableView LeavesTableView = ((TableView)obj);
                    //LeavesTableView.ShowTotalSummary = false;
                    //LeavesTableView.ShowFixedTotalSummary = false;
                    LeavesTableView.ExportToXlsx(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    //LeavesTableView.ShowTotalSummary = false;
                    LeavesTableView.ShowFixedTotalSummary = true;
                }
                GeosApplication.Instance.Logger.Log("Method ExportEquivalencyWeightAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportEquivalencyWeightAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        //[Pallavi Jadhav][Geos-4332][14 04 2023]
        private void EquivalencyWeightGridControlUnloadedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EquivalencyWeightGridControlUnloadedCommandAction...", category: Category.Info, priority: Priority.Low);
                GridControl gridControl = obj as GridControl;
                TableView tableView = (TableView)gridControl.View;
                tableView.SearchString = string.Empty;
                if (gridControl.GroupCount > 0)
                    gridControl.ClearGrouping();
                gridControl.ClearSorting();
                gridControl.FilterString = null;
                gridControl.SaveLayoutToXml(EquivalencyWeightGridSettingFilePath);


                GeosApplication.Instance.Logger.Log("Method EquivalencyWeightGridControlUnloadedCommandAction()... executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error on EquivalencyWeightGridControlUnloadedCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Column Chooser [Pallavi Jadhav][Geos-4332][14 04 2023]

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(EquivalencyWeightGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(EquivalencyWeightGridSettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(EquivalencyWeightGridSettingFilePath);

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

                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                datailView.ShowTotalSummary = true;
                gridControl.TotalSummary.Clear();
                gridControl.TotalSummary.AddRange(new List<GridSummaryItem>() {
                new GridSummaryItem()
                {
                    SummaryType = SummaryItemType.Count,
                    Alignment = GridSummaryItemAlignment.Left,
                    DisplayFormat = "Total Count : {0}",
                }
                });



                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(EquivalencyWeightGridSettingFilePath);
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(EquivalencyWeightGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EquivalencyWeightRefreshCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EquivalencyWeightRefreshCommandAction()...", category: Category.Info, priority: Priority.Low);


                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
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
                Init();
                //detailView.SearchString = null;
                //MyFilterString = string.Empty;

                //TileBarArrange(ModulesEquivalencyWeightList);
                //AddCustomSetting();
                //MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method EquivalencyWeightRefreshCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EquivalencyWeightRefreshCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EquivalencyWeightRefreshCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EquivalencyWeightRefreshCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        //[GEOS2-4330][Rupali Sarode][06-04-2023]
        private void ModuleEquivalencyWeightHyperlinkCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ModuleEquivalencyWeightHyperlinkCommandAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;

                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                ModulesEquivalencyWeight SelectedRow = (ModulesEquivalencyWeight)detailView.DataControl.CurrentItem;
                DateTime? tempEndDate;

                ModulesEquivalencyWeight SelectedModuleEquivalentWeight = new ModulesEquivalencyWeight();

                SelectedModuleEquivalentWeight = SelectedRow;


                //SelectedModuleEquivalentWeight = (ModulesEquivalencyWeight)obj;

                AddEditModuleEquivalencyWeightView addEditModuleEquivalencyWeightView = new AddEditModuleEquivalencyWeightView();
                AddEditModuleEquivalencyWeightViewModel addEditModuleEquivalencyWeightViewModel = new AddEditModuleEquivalencyWeightViewModel();

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                addEditModuleEquivalencyWeightView.DataContext = addEditModuleEquivalencyWeightViewModel;
                EventHandler handle = delegate { addEditModuleEquivalencyWeightView.Close(); };
                addEditModuleEquivalencyWeightViewModel.RequestClose += handle;

                addEditModuleEquivalencyWeightViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditModulesEquivalencyWeight").ToString();
                addEditModuleEquivalencyWeightViewModel.LblEquivalentWeight = "Module";
                addEditModuleEquivalencyWeightViewModel.EditInit(SelectedModuleEquivalentWeight);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                var ownerInfo = (detailView as FrameworkElement);
                addEditModuleEquivalencyWeightView.Owner = Window.GetWindow(ownerInfo);
                addEditModuleEquivalencyWeightView.ShowDialog();

                if (addEditModuleEquivalencyWeightViewModel.IsSave == true)
                {

                    SelectedModulesEquivalentWeight = new ModulesEquivalencyWeight();
                    if (addEditModuleEquivalencyWeightViewModel.LatestEquivalentWeight != null)
                    {
                        //tempEndDate = addEditModuleEquivalencyWeightViewModel.LatestEquivalentWeight.EndDate;

                        //if (tempEndDate == null) tempEndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                        //if (Convert.ToDateTime(DateTime.Now.ToShortDateString()) >= addEditModuleEquivalencyWeightViewModel.LatestEquivalentWeight.StartDate &&
                        //    Convert.ToDateTime(DateTime.Now.ToShortDateString()) <= tempEndDate)
                        //{

                            SelectedModulesEquivalentWeight = ModulesEquivalencyWeightList.FirstOrDefault(i => i.IdCPType == SelectedModuleEquivalentWeight.IdCPType);

                            SelectedModulesEquivalentWeight.EquivalentWeight = addEditModuleEquivalencyWeightViewModel.LatestEquivalentWeight.EquivalentWeight;
                            SelectedModulesEquivalentWeight.StartDate = addEditModuleEquivalencyWeightViewModel.LatestEquivalentWeight.StartDate;
                            SelectedModulesEquivalentWeight.EndDate = addEditModuleEquivalencyWeightViewModel.LatestEquivalentWeight.EndDate;
                            SelectedModulesEquivalentWeight.LastUpdate = DateTime.Now;
                        //}
                        //else
                        //{
                        //    SelectedModulesEquivalentWeight = ModulesEquivalencyWeightList.FirstOrDefault(i => i.IdCPType == SelectedModuleEquivalentWeight.IdCPType);

                        //    SelectedModulesEquivalentWeight.EquivalentWeight = null;
                        //    SelectedModulesEquivalentWeight.StartDate = null;
                        //    SelectedModulesEquivalentWeight.EndDate = null;
                        //    SelectedModulesEquivalentWeight.LastUpdate = DateTime.Now;
                        //}
                    }
                    else
                    {
                        SelectedModulesEquivalentWeight = ModulesEquivalencyWeightList.FirstOrDefault(i => i.IdCPType == SelectedModuleEquivalentWeight.IdCPType);

                        SelectedModulesEquivalentWeight.EquivalentWeight = null;
                        SelectedModulesEquivalentWeight.StartDate = null;
                        SelectedModulesEquivalentWeight.EndDate = null;
                        SelectedModulesEquivalentWeight.LastUpdate = DateTime.Now;
                    }
                }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method ModuleEquivalencyWeightHyperlinkCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ModuleEquivalencyWeightHyperlinkCommandAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }

        #endregion

    }
}
