using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Utility;
using Microsoft.Win32;
using Prism.Logging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class MatrixListViewModel : INotifyPropertyChanged
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration
        private ObservableCollection<QuotationMatrixTemplate> quotationMatrixTemplateList;
        private QuotationMatrixTemplate selectedQuotationMatrixTemplate;
        private ObservableCollection<TileBarFilters> filterStatusListOfTile;
        private TileBarFilters selectedTileBarItem;
        private List<GridColumn> GridColumnList;
        private bool isEdit;
        TableView view;
        private string userSettingsKey = "CRM_MatrixList_";
        string myFilterString;
        List<LookupValue> regionLookupValueList;
        private bool isAddMatrixEnable;
        private static GridControl gridControl;

        #endregion

        #region public Events
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Public ICommand

        public ICommand AddMatrixCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand MatrixListGridDoubleClickCommand { get; set; }

        public ICommand ExportButtonCommand { get; set; }
        public ICommand HyperlinkForWebsite { get; set; }

        public ICommand CommandFilterStatusTileClick { get; set; }
        public ICommand FilterEditorCreatedCommand { get; set; }
        public ICommand GridControlLoadedCommand { get; set; }
        public ICommand CommandTileBarClickDoubleClick { get; set; }
        public ICommand CommandShowFilterPopupClick { get; set; }
        #endregion

        #region Properties

        public bool IsAddMatrixEnable
        {
            get
            {
                return isAddMatrixEnable;
            }

            set
            {
                isAddMatrixEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsAddMatrixEnable)));
            }
        }
        public ObservableCollection<QuotationMatrixTemplate> QuotationMatrixTemplateList
        {
            get { return quotationMatrixTemplateList; }
            set
            {
                quotationMatrixTemplateList = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(QuotationMatrixTemplateList)));
            }
        }
        public QuotationMatrixTemplate SelectedQuotationMatrixTemplate
        {
            get
            {
                return selectedQuotationMatrixTemplate;
            }

            set
            {
                selectedQuotationMatrixTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedQuotationMatrixTemplate)));
            }
        }

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }


        public ObservableCollection<TileBarFilters> FilterStatusListOfTile
        {
            get { return filterStatusListOfTile; }
            set
            {
                filterStatusListOfTile = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FilterStatusListOfTile)));
            }
        }
        public TileBarFilters SelectedTileBarItem
        {
            get { return selectedTileBarItem; }
            set
            {
                selectedTileBarItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedTileBarItem)));
            }
        }
        public bool IsEdit
        {
            get
            {
                return isEdit;
            }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsEdit)));
            }
        }
        public string FilterStringCriteria { get; set; }
        public string FilterStringName { get; set; }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(MyFilterString)));
            }
        }

        #endregion

        public MatrixListViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MatrixListViewModel()...", category: Category.Info, priority: Priority.Low);
                AddMatrixCommand = new RelayCommand(new Action<object>(AddMatrixViewWindowShow));

                PrintButtonCommand = new RelayCommand(new Action<object>(PrintMatrixListList));
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshMatrixList));
                MatrixListGridDoubleClickCommand = new DelegateCommand<object>(EditQuotationMatrixTemplateViewWindowShow);

                ExportButtonCommand = new RelayCommand(new Action<object>(ExportMatrixListButtonCommandAction));
                HyperlinkForWebsite = new DelegateCommand<object>(new Action<object>((obj) => { OpenWebsite(obj); }));

                CommandFilterStatusTileClick = new DelegateCommand<object>(CommandFilterStatusTileClickAction);
                FilterEditorCreatedCommand = new DelegateCommand<FilterEditorEventArgs>(FilterEditorCreatedCommandAction);
                GridControlLoadedCommand = new DelegateCommand<Object>(GridControlLoadedCommandAction);
                CommandTileBarClickDoubleClick = new DelegateCommand<object>(CommandTileBarClickDoubleClickAction);

                GeosApplication.Instance.Logger.Log("Constructor MatrixListViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor MatrixListViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        var win = new Window
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

                FillData();
                FillFilterTileBar();
                IsAddMatrixEnable = GeosApplication.Instance.IsPermissionAdminOnly;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to get data as per User Permission
        /// </summary>
        public void FillData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillData ...", category: Category.Info, priority: Priority.Low);

                QuotationMatrixTemplateList = new ObservableCollection<QuotationMatrixTemplate>(
                    CrmStartUp.GetAllQuotationMatrixTemplates_V2160());

                if (QuotationMatrixTemplateList.Count > 0)
                    SelectedQuotationMatrixTemplate = QuotationMatrixTemplateList[0];

                GeosApplication.Instance.Logger.Log("Method FillData() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillData() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillData() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillData()", category: Category.Exception, priority: Priority.Low);
            }
        }

        private static void PrintMatrixListList(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintMatrixListList()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        var win = new Window
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
                var pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["MatrixListReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["MatrixListReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                var window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintMatrixListList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintMatrixListList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddMatrixViewWindowShow(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddMatrixViewWindowShow...", category: Category.Info, priority: Priority.Low);
                var tableView = (TableView)obj;
                DXSplashScreen.Show<SplashScreenView>();
                var addMatrixViewModel = new AddMatrixViewModel();
                addMatrixViewModel.Init();
                var addMatrixView = new AddMatrixView();

                EventHandler handle = delegate { addMatrixView.Close(); };
                addMatrixViewModel.RequestClose += handle;
                addMatrixView.DataContext = addMatrixViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                var ownerInfo = (tableView as FrameworkElement);
                addMatrixView.Owner = Window.GetWindow(ownerInfo);
                addMatrixView.ShowDialogWindow();

                if (addMatrixViewModel.IsSave)
                {
                    QuotationMatrixTemplateList.Add(addMatrixViewModel.SelectedMatrix);
                    SelectedQuotationMatrixTemplate = QuotationMatrixTemplateList.LastOrDefault();
                    FillFilterTileBar();
                    ((GridControl)tableView.Grid).RefreshData();
                    ((GridControl)tableView.Grid).Focus();

                    GeosApplication.Instance.Logger.Log("Method AddMatrixViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddMatrixViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RefreshMatrixList(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method RefreshMatrixList()...", category: Category.Info, priority: Priority.Low);

                var detailView = (TableView)obj;
                var gridControl = (detailView).Grid;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {

                    DXSplashScreen.Show(x =>
                    {
                        var win = new Window
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

                detailView.SearchString = string.Empty;
                FillData();
                FillFilterTileBar();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshMatrixList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshMatrixList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshMatrixList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefreshMatrixList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditQuotationMatrixTemplateViewWindowShow(object obj)
        {
            try
            {
                if (GeosApplication.Instance.IsPermissionAdminOnly)
                {
                    GeosApplication.Instance.Logger.Log("Method EditQuotationMatrixTemplateViewWindowShow()...",
                        category: Category.Info, priority: Priority.Low);
                    var detailView = (TableView)obj;

                    if (SelectedQuotationMatrixTemplate != null)
                    {
                        DXSplashScreen.Show<SplashScreenView>();
                        var addMatrixViewModel = new AddMatrixViewModel();
                        var matrixForEditing = (QuotationMatrixTemplate)detailView.DataControl.CurrentItem;

                        addMatrixViewModel.EditInit(SelectedQuotationMatrixTemplate);
                        var addMatrixView = new AddMatrixView();
                        EventHandler handle = delegate { addMatrixView.Close(); };
                        addMatrixViewModel.RequestClose += handle;
                        addMatrixView.DataContext = addMatrixViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                        var ownerInfo = (detailView as FrameworkElement);
                        addMatrixView.Owner = Window.GetWindow(ownerInfo);
                        addMatrixView.ShowDialogWindow();

                        if (addMatrixViewModel.IsSave)
                        {
                            var index = -1;
                            for (int i = 0; i < QuotationMatrixTemplateList.Count; i++)
                            {
                                if (QuotationMatrixTemplateList[i].IdQuotationMatrixTemplate ==
                                    addMatrixViewModel.SelectedMatrix.IdQuotationMatrixTemplate)
                                {
                                    index = i;
                                    break;
                                }
                            }

                            QuotationMatrixTemplateList[index].Name = addMatrixViewModel.SelectedMatrix.Name;
                            QuotationMatrixTemplateList[index].Description = addMatrixViewModel.SelectedMatrix.Description;
                            QuotationMatrixTemplateList[index].Customer = addMatrixViewModel.SelectedMatrix.Customer;
                            QuotationMatrixTemplateList[index].RegionLookupValue = addMatrixViewModel.SelectedMatrix.RegionLookupValue;
                            QuotationMatrixTemplateList[index].ProductCategory = addMatrixViewModel.SelectedMatrix.ProductCategory;
                            QuotationMatrixTemplateList[index].Url = addMatrixViewModel.SelectedMatrix.Url;
                            QuotationMatrixTemplateList[index].InUse = addMatrixViewModel.SelectedMatrix.InUse;
                            FillFilterTileBar();
                            ((GridControl)detailView.Grid).RefreshData();
                            ((GridControl)detailView.Grid).Focus();
                            GeosApplication.Instance.Logger.Log("Method AddMatrixViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditQuotationMatrixTemplateViewWindowShow()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditQuotationMatrixTemplateViewWindowShow()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ExportMatrixListButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportMatrixListButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                var saveFile = new SaveFileDialog
                {
                    DefaultExt = "xlsx",
                    FileName = "Matrix List",
                    Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*",
                    FilterIndex = 1,
                    Title = "Save Excel Report"
                };
                DialogResult = (bool)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            var win = new Window
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
                    var MatrixListTableView = ((TableView)obj);
                    MatrixListTableView.ShowTotalSummary = false;
                    MatrixListTableView.ShowFixedTotalSummary = false;
                    MatrixListTableView.ExportToXlsx(ResultFileName);

                    using (var control = new SpreadsheetControl())
                    {
                        control.LoadDocument(ResultFileName);
                        var worksheet = control.ActiveWorksheet;

                        var range = worksheet.Range["A1:F1"];
                        range.Font.Bold = true;
                        range.Fill.BackgroundColor = System.Drawing.Color.LightGray;
                        control.SaveDocument();

                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                        System.Diagnostics.Process.Start(ResultFileName);
                        MatrixListTableView.ShowTotalSummary = false;
                        MatrixListTableView.ShowFixedTotalSummary = true;
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ExportMatrixListButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportMatrixListButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void OpenWebsite(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenWebsite ...", category: Category.Info, priority: Priority.Low);

                var website = Convert.ToString(obj);
                if (!string.IsNullOrEmpty(website) && website != "-" && !website.Contains("@"))
                {
                    var websiteArray = website.Split(' ');
                    System.Diagnostics.Process.Start(websiteArray[0]);
                }

                GeosApplication.Instance.Logger.Log("Method OpenWebsite() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenWebsite() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void FillFilterTileBar()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFilterTileBar()...", category: Category.Info, priority: Priority.Low);

                if (FilterStatusListOfTile == null)
                {
                    FilterStatusListOfTile = new ObservableCollection<TileBarFilters>();
                }
                else
                {
                    FilterStatusListOfTile.Clear();
                }

                regionLookupValueList = new List<LookupValue>(CrmStartUp.GetLookupValues(8));

                if (regionLookupValueList != null && QuotationMatrixTemplateList != null)
                {
                    FilterStatusListOfTile.Add(new TileBarFilters()
                    {
                        Caption = (System.Windows.Application.Current.FindResource("MatrixTileBarCaptionAll").ToString()),
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCount = QuotationMatrixTemplateList.Count(),
                        EntitiesCountVisibility = Visibility.Visible,
                        Height = 80,
                        width = 200,
                        FilterCriteria = $"[InUseYesOrNo] In('Yes')"
                    });

                    foreach (var item in regionLookupValueList)
                    {
                        FilterStatusListOfTile.Add(new TileBarFilters()
                        {
                            Caption = item.Value,
                            Id = item.IdLookupValue,
                            BackColor = item.HtmlColor,
                            ForeColor = item.HtmlColor,
                            EntitiesCount = QuotationMatrixTemplateList.Count(x =>
                                    x.RegionLookupValue.IdRegion == item.IdLookupValue
                                    ),
                            EntitiesCountVisibility = Visibility.Visible,
                            Height = 80,
                            width = 200,
                            FilterCriteria = $"[InUseYesOrNo] In('Yes') AND [RegionLookupValue.Region] In('{item.Value}')"
                        });
                    }
                }

                FilterStatusListOfTile.Add(new TileBarFilters()
                {
                    Caption = (System.Windows.Application.Current.FindResource("MatrixCustomFilter").ToString()),
                    Id = 0,
                    BackColor = null,
                    ForeColor = null,
                    EntitiesCountVisibility = Visibility.Collapsed,
                    Height = 30,
                    width = 200,
                });

                if (FilterStatusListOfTile.Count > 0)
                    SelectedTileBarItem = FilterStatusListOfTile[0];

                AddCustomSetting();
                GeosApplication.Instance.Logger.Log("Method FillFilterTileBar() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillFilterTileBar() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandFilterStatusTileClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandFilterStatusTileClickAction()...", category: Category.Info, priority: Priority.Low);
                string FilterString = string.Empty;
                object[] addedItemsArray = ((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems);
                if (addedItemsArray != null && addedItemsArray.Length > 0)
                {
                    TileBarFilters itemTileBarFilters = (TileBarFilters)((object[])((System.Windows.Controls.SelectionChangedEventArgs)obj).AddedItems)[0];
                    FilterString = itemTileBarFilters.FilterCriteria;
                    FilterStringName = itemTileBarFilters.Caption;
                }
                if (FilterStringName.Equals(System.Windows.Application.Current.FindResource("MatrixCustomFilter").ToString()))
                {
                    return;
                }
                else if (FilterString != null && !string.IsNullOrEmpty(FilterString))
                {
                    MyFilterString = FilterString;
                    FilterStringCriteria = FilterString;
                }
                else
                {
                    MyFilterString = "[InUseYesOrNo] In('Yes')";
                }
                GeosApplication.Instance.Logger.Log("Method CommandFilterStatusTileClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandFilterStatusTileClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FilterEditorCreatedCommandAction(FilterEditorEventArgs obj)
        {
            obj.Handled = true;
            TableView table = (TableView)obj.OriginalSource;
            GridControl gridControl = (table).Grid;
            GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
            GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals("RegionLookupValue.Region"));
            ShowFilterEditor(obj);
        }

        public void GridControlLoadedCommandAction(object obj)
        {
            TableView table = (TableView)((GridControl)(((RoutedEventArgs)obj).OriginalSource)).View;
            table.BestFitColumns();
            gridControl = ((GridControl)(((RoutedEventArgs)obj).OriginalSource));

            FillFilterTileBar();
        }

        private void ShowFilterEditor(FilterEditorEventArgs e)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor()...", category: Category.Info, priority: Priority.Low);
                FilterEditorView filterEditorView = new FilterEditorView();
                FilterEditorViewModel filterEditorViewModel = new FilterEditorViewModel();
                string titleText = DevExpress.Xpf.Grid.GridControlLocalizer.Active.GetLocalizedString(GridControlStringId.FilterEditorTitle);
                if (IsEdit)
                {
                    filterEditorViewModel.FilterName = FilterStringName;
                    filterEditorViewModel.IsSave = true;
                    filterEditorViewModel.IsNew = false;
                    IsEdit = false;
                }
                else
                    filterEditorViewModel.IsNew = true;

                filterEditorViewModel.Init(e.FilterControl, FilterStatusListOfTile);
                filterEditorView.DataContext = filterEditorViewModel;
                EventHandler handle = delegate { filterEditorView.Close(); };
                filterEditorViewModel.RequestClose += handle;
                filterEditorView.Title = titleText;
                filterEditorView.Icon = DevExpress.Xpf.Core.Native.ImageHelper.CreateImageFromCoreEmbeddedResource("Editors.Images.FilterControl.filter.png");
                filterEditorView.Grid.Children.Add(e.FilterControl);
                filterEditorView.ShowDialog();

                if (filterEditorViewModel.IsDelete && !string.IsNullOrEmpty(filterEditorViewModel.FilterName) && filterEditorViewModel.IsSave)
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTile.FirstOrDefault(x => x.Caption.Equals(FilterStringName));
                    if (tileBarItem != null)
                    {
                        FilterStatusListOfTile.Remove(tileBarItem);
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
                else if (filterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(filterEditorViewModel.FilterName) && filterEditorViewModel.IsSave && !filterEditorViewModel.IsNew && filterEditorViewModel.IsCancel)
                {
                    TileBarFilters tileBarItem = FilterStatusListOfTile.FirstOrDefault(x => x.Caption.Equals(FilterStringName));
                    if (tileBarItem != null)
                    {
                        GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString() == "RegionLookupValue.Region");
                        GridControl gridControl = ((GridControl)((System.Windows.FrameworkContentElement)(GridColumn)column).Parent);
                        int rowCount = gridControl.VisibleRowCount;
                        FilterStringName = filterEditorViewModel.FilterName;
                        string filterCaption = tileBarItem.Caption;
                        tileBarItem.Caption = filterEditorViewModel.FilterName;
                        tileBarItem.EntitiesCount = rowCount;
                        tileBarItem.EntitiesCountVisibility = Visibility.Visible;
                        tileBarItem.FilterCriteria = filterEditorViewModel.FilterCriteria;
                        List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();
                        foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                        {
                            string key = setting.Key;

                            if (setting.Key.Contains(userSettingsKey))
                                key = setting.Key.Replace(userSettingsKey, "");

                            if (!key.Equals(filterCaption))
                                lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                            else
                                lstUserConfiguration.Add(new Tuple<string, string>(tileBarItem.Caption, tileBarItem.FilterCriteria));
                        }
                        ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                        GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                    }
                }
                else if (filterEditorViewModel.FilterName != null && !string.IsNullOrEmpty(filterEditorViewModel.FilterName) && filterEditorViewModel.IsSave && filterEditorViewModel.IsNew && filterEditorViewModel.IsCancel)
                {
                    GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString() == "RegionLookupValue.Region");
                    GridControl gridControl = ((GridControl)((System.Windows.FrameworkContentElement)(GridColumn)column).Parent);
                    int rowCount = gridControl.VisibleRowCount;
                    FilterStatusListOfTile.Add(new TileBarFilters()
                    {
                        Caption = filterEditorViewModel.FilterName,
                        Id = 0,
                        BackColor = null,
                        ForeColor = null,
                        EntitiesCountVisibility = Visibility.Visible,
                        FilterCriteria = filterEditorViewModel.FilterCriteria,
                        EntitiesCount = rowCount,
                        Height = 80,
                        width = 200
                    });
                    SelectedTileBarItem = FilterStatusListOfTile.LastOrDefault();
                    string filterName = userSettingsKey + filterEditorViewModel.FilterName;
                    GeosApplication.Instance.UserSettings[filterName] = filterEditorViewModel.FilterCriteria;
                    ApplicationOperation.CreateNewSetting(GeosApplication.Instance.UserSettings, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                    GeosApplication.Instance.UserSettings = ApplicationOperation.GetSetting(GeosApplication.Instance.UserSettingFilePath);
                }
                GeosApplication.Instance.Logger.Log("Method ShowFilterEditor() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ShowFilterEditor() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CommandTileBarClickDoubleClickAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction()...", category: Category.Info, priority: Priority.Low);
                foreach (var item in regionLookupValueList)
                {
                    if (FilterStringName != null)
                    {
                        if (FilterStringName.Equals(item.Value))
                        {
                            return;
                        }
                    }
                }

                if (FilterStringName.Equals(System.Windows.Application.Current.FindResource("LeadsViewCustomFilter").ToString()) || FilterStringName.Equals(System.Windows.Application.Current.FindResource("LeadsViewTileBarCaption").ToString()))
                {
                    return;
                }

                TableView table = (TableView)obj;
                GridControl gridControl = (table).Grid;
                GridColumnList = gridControl.Columns.Where(x => x.Header != null).ToList();
                GridColumn column = GridColumnList.FirstOrDefault(x => x.FieldName.ToString().Equals("RegionLookupValue.Region"));
                IsEdit = true;
                table.ShowFilterEditor(column);
                GeosApplication.Instance.Logger.Log("Method CommandTileBarClickDoubleClickAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandTileBarClickDoubleClickAction() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AddCustomSetting()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddCustomSetting()...", category: Category.Info, priority: Priority.Low);
                var tempUserSettings = GeosApplication.Instance.UserSettings.Where(x => x.Key.Contains(userSettingsKey)).ToList();
                if (tempUserSettings != null && gridControl!=null)
                {
                    var existingGridControlFilterCriteria = gridControl.FilterCriteria;

                    foreach (var item in tempUserSettings)
                    {
                        var count = 0;

                        try
                        {
                            //var filter = item.Value.Replace("[", "");
                            //filter = filter.Replace("]", "");
                            var customeFilterCriteriaOperator = CriteriaOperator.Parse(item.Value);
                            gridControl.FilterCriteria = customeFilterCriteriaOperator;
                            gridControl.RefreshData();
                            count = gridControl.VisibleRowCount;

                          //  count = QuotationMatrixTemplateList.Count(CriteriaToWhereClauseHelper.GetDataSetWhere(op));

                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddCustomSetting() {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                        FilterStatusListOfTile.Add(
                            new TileBarFilters()
                            {
                                Caption = item.Key.Replace(userSettingsKey, ""),
                                Id = 0,
                                BackColor = null,
                                ForeColor = null,
                                FilterCriteria = item.Value,
                                EntitiesCount = count,
                                EntitiesCountVisibility = Visibility.Visible,
                                Height = 80,
                                width = 200
                            });
                    }
                    gridControl.FilterCriteria = existingGridControlFilterCriteria;
                }

                GeosApplication.Instance.Logger.Log("Method AddCustomSetting() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddCustomSetting() " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
