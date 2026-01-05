using Emdep.Geos.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using Prism.Logging;
using System.Windows;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Hrm;
using System.ComponentModel;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using Emdep.Geos.UI.Commands;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using DevExpress.XtraPrinting;
using System.IO;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Data.Filtering;
using System.Globalization;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class MealAllowanceViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IHrmService HrmService = new HrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        private INavigationService Service { get { return GetService<INavigationService>(); } }
        #endregion // End Services

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Declaration
        ObservableCollection<MealAllowance> mealAllowanceList;
        MealAllowance selectedGridRow;
        bool isBusy;
        string myFilterString;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public string HRM_MealExpense_SettingFilePath = GeosApplication.Instance.UserSettingFolderName + "HRM_MealExpense_Setting.Xml";
        private bool isMealExpenseColumnChooserVisible;
        #endregion

        #region Properties

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public ObservableCollection<MealAllowance> MealAllowanceList
        {
            get { return mealAllowanceList; }
            set
            {
                mealAllowanceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MealAllowanceList"));
            }
        }
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public MealAllowance SelectedGridRow
        {
            get { return selectedGridRow; }
            set
            {
                selectedGridRow = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedGridRow"));

            }
        }

        public bool IsMealExpenseColumnChooserVisible
        {
            get { return isMealExpenseColumnChooserVisible; }
            set
            {
                isMealExpenseColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMealExpenseColumnChooserVisible"));
            }
        }
        double usablePageWidth;
        public double UsablePageWidth
        {
            get { return usablePageWidth; }
            set
            {
                usablePageWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UsablePageWidth"));
            }
        }
        #endregion

        #region Command
        public ICommand PrintButtonCommand { get; set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand MouseDoubleClickCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }

        #endregion

        #region Constructor
        public MealAllowanceViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor MealAllowanceViewModel()...", category: Category.Info, priority: Priority.Low);
            RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
            PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
            TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedCommandAction);
            MouseDoubleClickCommand = new RelayCommand(new Action<object>(EditSelectedMealAllowanceCommandAction));
            CustomShowFilterPopupCommand = new DelegateCommand<DevExpress.Xpf.Grid.FilterPopupEventArgs>(CustomShowFilterPopupAction);
            MyFilterString = string.Empty;
            GeosApplication.Instance.Logger.Log("Constructor MealAllowanceViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
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
                MyFilterString = string.Empty;
                FillMealAllowanceGrid();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillMealAllowanceGrid()
        {
            try
            {
                //MealAllowanceList = new ObservableCollection<MealAllowance>(HrmService.GetMealAllowances());   
                //Shubham[skadam] GEOS2-5138 Add country column with flag in meal allowance 18 12 2023
                //MealAllowanceList = new ObservableCollection<MealAllowance>(HrmService.GetMealAllowances_V2470());
                //[pramod.misal][GEOS2-5159][18-01-2024]
                //MealAllowanceList = new ObservableCollection<MealAllowance>(HrmService.GetMealAllowances_V2480());
                //[pramod.misal][GEOS2-GEOS2-5365][18-03-2024]
                //  MealAllowanceList = new ObservableCollection<MealAllowance>(HrmService.GetMealAllowances_V2500());
                //[Sudhir.jangra][GEOS2-5614]
                // IHrmService HrmService = new HrmServiceController("localhost:6699");

                MealAllowanceList = new ObservableCollection<MealAllowance>(HrmService.GetMealAllowances_V2510());


                //[pramod.misal][GEOS2-5159][18-01-2024]
                if (MealAllowanceList?.Count > 0)
                {
                    MealAllowanceList.ToList().ForEach(i =>
                    {

                        if (i.RegularEmp != null)
                        {
                            i.RegularEmp.DisplayAmount = Math.Round(i.RegularEmp.Amount, 2).ToString("n", CultureInfo.CurrentCulture) + " " + i.RegularEmp.CurrencySymbol;
                        }
                        if (i.GlobalEmp != null)
                        {
                            i.GlobalEmp.DisplayAmount = Math.Round(i.GlobalEmp.Amount, 2).ToString("n", CultureInfo.CurrentCulture) + " " + i.GlobalEmp.CurrencySymbol;
                        }
                        //i.RegularEmp.DisplayAmount = Math.Round(i.RegularEmp.Amount, 2).ToString("n", CultureInfo.CurrentCulture) + " " + i.RegularEmp.CurrencySymbol;
                        //i.GlobalEmp.DisplayAmount = Math.Round(i.GlobalEmp.Amount, 2).ToString("n", CultureInfo.CurrentCulture) + " " + i.GlobalEmp.CurrencySymbol;

                    });
                    SelectedGridRow = MealAllowanceList.FirstOrDefault();
                }
                IsBusy = false;
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMealAllowanceGrid() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillMealAllowanceGrid() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMealAllowanceGrid()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region CommandAction
        public void RefreshButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("MealAllowanceViewModel Method RefreshButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
            try
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
                MyFilterString = string.Empty;
                FillMealAllowanceGrid();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MealAllowanceViewModel RefreshButtonCommandAction method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("MealAllowanceViewModel RefreshButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                pcl.PaperKind = System.Drawing.Printing.PaperKind.A4;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["MealAllowanceReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["MealAllowanceReportPrintFooterTemplate"];            
                pcl.CreateDocument(false);
                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                UsablePageWidth = window.PreviewControl.Width;
                window.PreviewControl.DocumentSource = pcl;                
                IsBusy = false;
                window.Show();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("MealAllowanceViewModel Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Meal Allowance Report";
                saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                saveFile.FilterIndex = 1;
                saveFile.Title = "Save Excel Report";
                DialogResult = (Boolean)saveFile.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    IsBusy = true;
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
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;               
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    activityTableView.ExportToXlsx(ResultFileName, options);
                    IsBusy = false;                
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);
                    activityTableView.ShowTotalSummary = true;
                    GeosApplication.Instance.Logger.Log("MealAllowanceViewModel Method ExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in MealAllowanceViewModel Method ExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void TableViewLoadedCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(HRM_MealExpense_SettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(HRM_MealExpense_SettingFilePath);
                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(HRM_MealExpense_SettingFilePath);

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

                if (visibleFalseCoulumn > 0)
                {
                    IsMealExpenseColumnChooserVisible = true;
                }
                else
                {
                    IsMealExpenseColumnChooserVisible = false;
                }
                TableView datailView = ((TableView)((System.Windows.RoutedEventArgs)obj).OriginalSource);
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
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
                    ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_MealExpense_SettingFilePath);
                }

                if (column.Visible == false)
                {
                    IsMealExpenseColumnChooserVisible = true;
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
                ((GridControl)((System.Windows.FrameworkContentElement)sender).Parent).SaveLayoutToXml(HRM_MealExpense_SettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        //[rdixit][GEOS2-4181][20.03.2023]
        private void EditSelectedMealAllowanceCommandAction(object obj)
       {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditSelectedMealAllowanceCommandAction....", category: Category.Info, priority: Priority.Low);

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
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                TableView detailView = (TableView)obj;
                var mealAllowance = (MealAllowance)detailView.DataControl.CurrentItem;
                string Alias = mealAllowance.CompanyAlias;
                MealAllowance FoundRow = MealAllowanceList.Where(mol => mol.CompanyAlias.ToLower() == Alias.ToLower()).FirstOrDefault();
                EditMealAllowanceViewModel EditMealAllowanceViewModel = new EditMealAllowanceViewModel();
                EditMealAllowanceView EditMealAllowanceView = new EditMealAllowanceView();
                EventHandler handle1 = delegate { EditMealAllowanceView.Close(); };
                EditMealAllowanceViewModel.RequestClose += handle1;           
                EditMealAllowanceViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditMealAllowanceViewTitle").ToString();
                EditMealAllowanceViewModel.EditInit(FoundRow);
                EditMealAllowanceView.DataContext = EditMealAllowanceViewModel;
                EditMealAllowanceView.ShowDialogWindow();           
                FillMealAllowanceGrid();
                                
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EditSelectedMealAllowanceCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //Shubham[skadam] GEOS2-5138 Add country column with flag in meal allowance 21 12 2023
        private void CustomShowFilterPopupAction(DevExpress.Xpf.Grid.FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopupAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                MyFilterString = string.Empty;
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName != "Country")
                {
                    return;
                }
                #region Country
                if (e.Column.FieldName == "Country")
                {
                    foreach (var dataObject in MealAllowanceList)
                    {
                        if (dataObject.CountryName == null)
                        {
                            continue;
                        }
                        else if (dataObject.CountryName != null)
                        {

                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == dataObject.CountryName))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = dataObject.CountryName;
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CountryName Like '%{0}%'", dataObject.CountryName));
                                filterItems.Add(customComboBoxItem);
                            }
                            else
                                continue;
                        }
                        else
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == MealAllowanceList.Where(y => y.CountryName == dataObject.CountryName).Select(slt => slt.CountryName).FirstOrDefault().Trim()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = MealAllowanceList.Where(y => y.CountryName == dataObject.CountryName).Select(slt => slt.CountryName).FirstOrDefault().Trim();
                                customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("CountryName Like '%{0}%'", MealAllowanceList.Where(y => y.CountryName == dataObject.CountryName).Select(slt => slt.CountryName).FirstOrDefault().Trim()));
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }
                #endregion
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        #endregion
    }
}
