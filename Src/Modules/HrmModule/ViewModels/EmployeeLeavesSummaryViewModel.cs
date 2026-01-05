using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Data;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeLeavesSummaryViewModel : INotifyPropertyChanged
    {
        #region TaskLog
        /// <summary>
        ///[000][17/04/2019][sdesai][SPRINT 61][GEOS2-248] (#70119) New section Leave Summary under Leaves section]
        /// </summary>
        #endregion //TaskLog

        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion  //Service

        #region Declaration
        private ObservableCollection<Employee> employeeList;
        private Employee selectedLeaveSummary;
        string myFilterString;
        private decimal DailyHoursCount;
        private bool dynamicColumnsAdded;
        private ObservableCollection<GridColumn> additionalBandGridColumns;
        #endregion  //Declaration

        #region Properties
        public ObservableCollection<GridColumn> AdditionalBandGridColumns
        {
            get
            {
                return additionalBandGridColumns;
            }

            set
            {
                this.additionalBandGridColumns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalBandGridColumns"));

            }
        }

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        public ObservableCollection<Employee> EmployeeList
        {
            get
            {
                return employeeList;
            }

            set
            {
                employeeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeList"));
            }
        }
        public Employee SelectedLeaveSummary
        {
            get
            {
                return selectedLeaveSummary;
            }

            set
            {
                selectedLeaveSummary = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLeaveSummary"));
            }
        }

        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        #endregion  //Properties

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

        #region Public Icommands
        public ICommand PlantOwnerEditValueChangedCommand { get; private set; }
        public ICommand SelectedYearChangedCommand { get; private set; }
        public ICommand RefreshButtonCommand { get; set; }
        public ICommand PrintButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }
        public ICommand CustomShowFilterPopupCommand { get; set; }
        public ICommand GridDoubleClickCommand { get; set; }
        public ICommand CustomRowFilterCommand { get; set; }
        #endregion  //Commands

        #region Constructor
        public EmployeeLeavesSummaryViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EmployeeLeavesSummaryViewModel()...", category: Category.Info, priority: Priority.Low);

                PlantOwnerEditValueChangedCommand = new DelegateCommand<object>(PlantOwnerEditValueChangedCommandAction);
                SelectedYearChangedCommand = new DelegateCommand<object>(SelectedYearChangedCommandAction);
                RefreshButtonCommand = new RelayCommand(new Action<object>(RefreshButtonCommandAction));
                PrintButtonCommand = new RelayCommand(new Action<object>(PrintButtonCommandAction));
                ExportButtonCommand = new RelayCommand(new Action<object>(ExportButtonCommandAction));
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopup);
                GridDoubleClickCommand = new DelegateCommand<object>(OpenEmployeeProfileDetail);
                CustomRowFilterCommand = new DelegateCommand<RowFilterEventArgs>(CustomRowFilter);
               
                GeosApplication.Instance.Logger.Log("Constructor EmployeeLeavesSummaryViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeLeavesSummaryViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion  //Constructor

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                //if (!DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Show(x =>
                //    {
                //        Window win = new Window()
                //        {
                //            ShowActivated = false,
                //            WindowStyle = WindowStyle.None,
                //            ResizeMode = ResizeMode.NoResize,
                //            AllowsTransparency = true,
                //            Background = new SolidColorBrush(Colors.Transparent),
                //            ShowInTaskbar = false,
                //            Topmost = true,
                //            SizeToContent = SizeToContent.WidthAndHeight,
                //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //        };
                //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //        win.Topmost = false;
                //        return win;
                //    }, x =>
                //    {
                //        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //    }, null, null);
                //}
                //GeosApplication.Instance.FillFinancialYear();
                FillEmployeeLeavesSummary();
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }
        
        private void AddAdditionalBandGridColumns()
        {
            if (!dynamicColumnsAdded)
            {
                AdditionalBandGridColumns = new ObservableCollection<GridColumn>();
                List<LookupValue> AnnualAdditionalLeavesLookupList = new List<LookupValue>(CrmStartUp.GetLookupValues(76));

                if (AnnualAdditionalLeavesLookupList != null && AnnualAdditionalLeavesLookupList.Count > 0)
                {
                    for (int index = 0; index < AnnualAdditionalLeavesLookupList.Count; index++)
                    {
                        if (AnnualAdditionalLeavesLookupList[index].InUse)
                        {
                            SetZeroDayWhereAdditionalleaveDataNotExistsInDatabase(AnnualAdditionalLeavesLookupList, index);
                        }
                    }

                    OrderAdditionalLeavesListItemsByLeavePosition();

                    for (int i = 0; i < AnnualAdditionalLeavesLookupList.Count; i++)
                    {
                        if (AnnualAdditionalLeavesLookupList[i].InUse)
                        {
                            GridColumn columnObj = CreateAdditionalLeaveColumn(AnnualAdditionalLeavesLookupList, i);
                            AdditionalBandGridColumns.Add(columnObj);
                        }
                    }
                }
                // 
                // FieldName = $"SelectedAdditionalLeaves[{i}].AdditionalLeaveTotalHours",
                dynamicColumnsAdded = true;
            }
        }

        private GridColumn CreateAdditionalLeaveColumn(List<LookupValue> AnnualAdditionalLeavesLookupList, int i)
        {
            var columnObj = new GridColumn()
            {
                Header = $"{AnnualAdditionalLeavesLookupList[i].Value}",
                MinWidth = 100,
                ReadOnly = true,
                FilterPopupMode = FilterPopupMode.CheckedList,
                AllowResizing = DevExpress.Utils.DefaultBoolean.True,
                //  CellTemplate = BuildTemplate($"RowData.Row.EmployeeAnnualLeave.SelectedAdditionalLeaves[{AdditionalBandGridColumns.Count}].AdditionalLeaveTotalHours"),
                AllowSorting = DevExpress.Utils.DefaultBoolean.True,
                AllowColumnFiltering = DevExpress.Utils.DefaultBoolean.True,
                Binding = new Binding($"RowData.Row.EmployeeAnnualLeave.SelectedAdditionalLeaves[{AdditionalBandGridColumns.Count}].AdditionalLeaveTotalHoursInDays"),
                ColumnFilterMode = ColumnFilterMode.DisplayText,
                SortMode = DevExpress.XtraGrid.ColumnSortMode.Value,
                AllowCellMerge = false,
                CopyValueAsDisplayText = true

            };
            return columnObj;
        }

        private void OrderAdditionalLeavesListItemsByLeavePosition()
        {
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].EmployeeAnnualLeave != null)
                {
                    EmployeeList[i].EmployeeAnnualLeave.SelectedAdditionalLeaves =
                    EmployeeList[i].EmployeeAnnualLeave.SelectedAdditionalLeaves.OrderBy(x => x.AdditionalLeavePosition).ToList();
                }
            }
        }

        private void SetZeroDayWhereAdditionalleaveDataNotExistsInDatabase(List<LookupValue> AnnualAdditionalLeavesLookupList, int index)
        {
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].EmployeeAnnualLeave != null)
                {
                    bool additionalleaveExistsInDatabase =
                        EmployeeList[i].EmployeeAnnualLeave.SelectedAdditionalLeaves.Exists(
                        x => x.IdAdditionalLeaveReason == AnnualAdditionalLeavesLookupList[index].IdLookupValue
                            );
                    if (!additionalleaveExistsInDatabase)
                    {
                        EmployeeAnnualAdditionalLeave ObjAdditionalLeave = new EmployeeAnnualAdditionalLeave();
                        ObjAdditionalLeave.AdditionalLeaveReasonName = AnnualAdditionalLeavesLookupList[index].Value;
                        ObjAdditionalLeave.IdAdditionalLeaveReason = AnnualAdditionalLeavesLookupList[index].IdLookupValue;
                        ObjAdditionalLeave.AdditionalLeavePosition = AnnualAdditionalLeavesLookupList[index].Position;
                        ObjAdditionalLeave.AdditionalLeaveTotalHours = 0;
                        ObjAdditionalLeave.ConvertedDays = 0;
                        ObjAdditionalLeave.ConvertedHours = 0;
                        ObjAdditionalLeave.AdditionalLeaveTotalHoursInDays = "0d";
                        if (EmployeeList[i] != null)
                        {
                            ObjAdditionalLeave.IdEmployee = EmployeeList[i].IdEmployee;
                        }
                        ObjAdditionalLeave.Year = Convert.ToInt32(HrmCommon.Instance.SelectedPeriod);
                        ObjAdditionalLeave.TransactionOperation = ModelBase.TransactionOperations.Add;
                        EmployeeList[i].EmployeeAnnualLeave.SelectedAdditionalLeaves.Add(ObjAdditionalLeave);
                    }
                }
            }
        }

        //private DataTemplate BuildTemplate(string colName)
        //{
        //    MultiBinding multiBinding = new MultiBinding();
        //    Binding binding1 = new Binding(colName);
        //    Binding binding2 = new Binding("RowData.Row.CompanyShift.CompanyAnnualSchedule.DailyHoursCount");
        //    Binding binding3 = new Binding("RowData.Row.EmployeeAnnualLeave");
        //    multiBinding.Bindings.Add(binding1);
        //    multiBinding.Bindings.Add(binding2);
        //    multiBinding.Bindings.Add(binding3);
        //    multiBinding.Converter = (IMultiValueConverter)(new UI.Converters.HoursToDayConverter());

        //    FrameworkElementFactory childFactory = new FrameworkElementFactory(typeof(TextBlock));
        //    childFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
        //    childFactory.SetBinding(TextBlock.TextProperty, multiBinding);
        //    DataTemplate template = new DataTemplate();
        //    template.VisualTree = childFactory;

        //    return template;
        //}

        private void PlantOwnerEditValueChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction ...", category: Category.Info, priority: Priority.Low);
                //if (!DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Show(x =>
                //    {
                //        Window win = new Window()
                //        {
                //            ShowActivated = false,
                //            WindowStyle = WindowStyle.None,
                //            ResizeMode = ResizeMode.NoResize,
                //            AllowsTransparency = true,
                //            Background = new SolidColorBrush(Colors.Transparent),
                //            ShowInTaskbar = false,
                //            Topmost = true,
                //            SizeToContent = SizeToContent.WidthAndHeight,
                //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //        };
                //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //        win.Topmost = false;
                //        return win;
                //    }, x =>
                //    {
                //        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //    }, null, null);
                //}
                //MyFilterString = string.Empty;
                FillEmployeeLeavesSummary();
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PlantOwnerEditValueChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PlantOwnerEditValueChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }
        private void SelectedYearChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction ...", category: Category.Info, priority: Priority.Low);

                //if (!DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Show(x =>
                //    {
                //        Window win = new Window()
                //        {
                //            ShowActivated = false,
                //            WindowStyle = WindowStyle.None,
                //            ResizeMode = ResizeMode.NoResize,
                //            AllowsTransparency = true,
                //            Background = new SolidColorBrush(Colors.Transparent),
                //            ShowInTaskbar = false,
                //            Topmost = true,
                //            SizeToContent = SizeToContent.WidthAndHeight,
                //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //        };
                //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //        win.Topmost = false;
                //        return win;
                //    }, x =>
                //    {
                //        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //    }, null, null);
                //}
                //MyFilterString = string.Empty;
                FillEmployeeLeavesSummary();
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method SelectedYearChangedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method SelectedYearChangedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }
        private void RefreshButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                //if (!DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Show(x =>
                //    {
                //        Window win = new Window()
                //        {
                //            ShowActivated = false,
                //            WindowStyle = WindowStyle.None,
                //            ResizeMode = ResizeMode.NoResize,
                //            AllowsTransparency = true,
                //            Background = new SolidColorBrush(Colors.Transparent),
                //            ShowInTaskbar = false,
                //            Topmost = true,
                //            SizeToContent = SizeToContent.WidthAndHeight,
                //            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                //        };
                //        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                //        win.Topmost = false;
                //        return win;
                //    }, x =>
                //    {
                //        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                //    }, null, null);
                //}

                //TableView detailView = (TableView)obj;
                //GridControl gridControl = (detailView).Grid;
                //detailView.SearchString = null;
                FillEmployeeLeavesSummary();
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method RefreshButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }
        private void PrintButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
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
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeReportPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["EmployeeReportPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                window.Show();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Leaves Summary List";
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
                    TableView leavesSummaryTable = ((TableView)obj);
                    leavesSummaryTable.ShowTotalSummary = false;
                    leavesSummaryTable.ShowFixedTotalSummary = false;
                    leavesSummaryTable.ExportToXlsx(ResultFileName, new DevExpress.XtraPrinting.XlsxExportOptionsEx { ExportType = DevExpress.Export.ExportType.WYSIWYG });

                    leavesSummaryTable.ShowTotalSummary = true;
                    leavesSummaryTable.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][15-01-2020][GEOS2-1658] The filter LeaveType in Summary Leaves does not work properly
        /// </summary>
        /// <param name="e"></param>
        private void CustomShowFilterPopup(FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method CustomShowFilterPopup ...", category: Category.Info, priority: Priority.Low);

            if (e.Column.FieldName != "EmployeeCompanyAlias" && 
                e.Column.FieldName != "Departments" && 
                e.Column.FieldName != "EmployeeJobTitles" && 
                e.Column.FieldName != "EmpJobCodes" 
                //&&
                //e.Column.ParentBand.Name != "Additional"
                )
            {
                return;
            }

            try
            {
                List<object> filterItems = new List<object>();
                //if (e.Column.ParentBand.Name == "Additional")
                //{
                //    filterItems = (List<Object>)e.ComboBoxEdit.ItemsSource;
                //    for (int i = 0; i < filterItems.Count; i++)
                //    {
                //        object editValueObject = ((CustomComboBoxItem)filterItems[i]).EditValue;
                //        string editValueString = editValueObject.ToString();
                //        decimal editValueDecimal;
                //        bool convertedFlag = Decimal.TryParse(editValueString, out editValueDecimal);
                //        if (convertedFlag)
                //        {
                //            decimal Days = (Int32)(editValueDecimal / DailyHoursCount);// EmployeeList[0].EmployeeAnnualLeave.SelectedAdditionalLeaves[0]..CompanyAnnualSchedule.DailyHoursCount);
                //            decimal Hours = (editValueDecimal % DailyHoursCount);// EmployeeList[0].CompanyShift.CompanyAnnualSchedule.DailyHoursCount);

                //    string DisplayValues = Days.ToString() + "d";

                //            if (Hours != 0)
                //            {
                //                DisplayValues += " " + Hours.ToString() + "H";
                //            }

                //            ((CustomComboBoxItem)filterItems[i]).DisplayValue = DisplayValues;
                //        }
                //    }
                //    if (filterItems != null && filterItems.Count > 0)
                //    {
                //        e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue));
                //    }
                //}

                //                else 
                if (e.Column.FieldName == "EmployeeCompanyAlias")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeCompanyAlias = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeCompanyAlias <> ''")
                    });

                    foreach (var dataObject in EmployeeList)
                    {
                        if (dataObject.EmployeeCompanyAlias == null)
                        {
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dataObject.EmployeeCompanyAlias))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.EmployeeCompanyAlias))
                        {
                            string[] employeeCompanyAliasList = dataObject.EmployeeCompanyAlias.Split(Environment.NewLine.ToCharArray());

                            foreach (string companyAlias in employeeCompanyAliasList)
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == companyAlias))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = companyAlias;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeCompanyAlias Like '%{0}%'", companyAlias));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "Departments")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Departments = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Departments <> ''")
                    });

                    foreach (var dataObject in EmployeeList)
                    {
                        if (dataObject.LstEmployeeDepartments == null)
                        {
                            continue;
                        }
                        else if (dataObject.LstEmployeeDepartments.Count == 0)
                        {
                            continue;
                        }

                        foreach (var department in dataObject.LstEmployeeDepartments)
                        {
                            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == department.DepartmentName.ToString()))
                            {
                                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                customComboBoxItem.DisplayValue = department.DepartmentName;
                                customComboBoxItem.EditValue = department.DepartmentName;
                                filterItems.Add(customComboBoxItem);
                            }
                        }
                    }
                }

                else if (e.Column.FieldName == "EmployeeJobTitles")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeJobTitles = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeJobTitles <> ''")
                    });

                    foreach (var dataObject in EmployeeList)
                    {
                        if (dataObject.EmployeeJobTitles == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmployeeJobTitles != null)
                        {
                            if (dataObject.EmployeeJobTitles.Contains("\n"))
                            {
                                string tempEmployeeJobTitles = dataObject.EmployeeJobTitles;
                                for (int index = 0; index < tempEmployeeJobTitles.Length; index++)
                                {
                                    string employeeJobTitles = tempEmployeeJobTitles.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == employeeJobTitles))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = employeeJobTitles;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeJobTitles Like '%{0}%'", employeeJobTitles));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmployeeJobTitles.Contains("\n"))
                                        tempEmployeeJobTitles = tempEmployeeJobTitles.Remove(0, employeeJobTitles.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = EmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeJobTitles Like '{0}'", EmployeeList.Where(y => y.EmployeeJobTitles == dataObject.EmployeeJobTitles).Select(slt => slt.EmployeeJobTitles).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                else if (e.Column.FieldName == "EmployeeJobTitles")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeJobTitles = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmployeeJobTitles <> ''")
                    });

                    foreach (var dataObject in EmployeeList)
                    {
                        if (dataObject.EmployeeJobTitles == null)
                        {
                            continue;
                        }
                        else if (string.IsNullOrEmpty(dataObject.EmployeeJobTitles))
                        {
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(dataObject.EmployeeJobTitles))
                        {
                            string[] employeeJobTitles = dataObject.EmployeeJobTitles.Split(Environment.NewLine.ToCharArray());

                            foreach (string jobTitles in employeeJobTitles)
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == jobTitles))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = jobTitles;
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmployeeJobTitles Like '%{0}%'", jobTitles));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                //[001] add comment

                else if (e.Column.FieldName == "EmpJobCodes")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodes = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("EmpJobCodes <> ''")
                    });

                    foreach (var dataObject in EmployeeList)
                    {
                        if (dataObject.EmpJobCodes == null)
                        {
                            continue;
                        }
                        else if (dataObject.EmpJobCodes != null)
                        {
                            if (dataObject.EmpJobCodes.Contains("\n"))
                            {
                                string tempEmpJobCodes = dataObject.EmpJobCodes;
                                for (int index = 0; index < tempEmpJobCodes.Length; index++)
                                {
                                    string empJobCodes = tempEmpJobCodes.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empJobCodes))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empJobCodes;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmpJobCodes Like '%{0}%'", empJobCodes));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempEmpJobCodes.Contains("\n"))
                                        tempEmpJobCodes = tempEmpJobCodes.Remove(0, empJobCodes.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == EmployeeList.Where(y => y.EmpJobCodes == dataObject.EmpJobCodes).Select(slt => slt.EmpJobCodes).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = EmployeeList.Where(y => y.EmpJobCodes == dataObject.EmpJobCodes).Select(slt => slt.EmpJobCodes).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("EmpJobCodes Like '%{0}%'", EmployeeList.Where(y => y.EmpJobCodes == dataObject.EmpJobCodes).Select(slt => slt.EmpJobCodes).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #region Old Code 

                //else if (e.Column.FieldName == "LeavesType")
                //{
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("LeavesType = ''")
                //    });

                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("LeavesType <> ''")
                //    });
                //    foreach (var dataObject in EmployeeList)
                //    {
                //        if (dataObject.EmployeeAnnualLeaves == null)
                //        {
                //            continue;
                //        }
                //        else if (dataObject.EmployeeAnnualLeaves.Count == 0)
                //        {
                //            continue;
                //        }

                //        foreach (var leave in dataObject.EmployeeAnnualLeaves)
                //        {
                //            if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == leave.CompanyLeave.Name.ToString()))
                //            {
                //                CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                customComboBoxItem.DisplayValue = leave.CompanyLeave.Name;
                //                customComboBoxItem.EditValue = leave.CompanyLeave.Name;
                //                filterItems.Add(customComboBoxItem);
                //            }
                //        }
                //    }
                //}
                //else if (e.Column.FieldName == "RegularHoursCount")
                //{
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("RegularHoursCount = ''")
                //    });
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("RegularHoursCount <> ''")
                //    });
                //    foreach (var dataObject in EmployeeList)
                //    {
                //        foreach (var obj in dataObject.EmployeeAnnualLeaves)
                //        {
                //            if (obj.RegularHoursCount == 0)
                //            {
                //                string DisplayValues = "0d";
                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.RegularHoursCount.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //            else
                //            {
                //                Int32 Days = (Int32)obj.RegularHoursCount / (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //                Int32 Hours = (Int32)obj.RegularHoursCount % (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //                string DisplayValues;
                //                if (Hours == 0)
                //                    DisplayValues = Days.ToString() + "d";
                //                else
                //                    DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";

                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.RegularHoursCount.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //        }

                //    }
                //}
                //else if (e.Column.FieldName == "AdditionalHoursCount")
                //{
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("AdditionalHoursCount = ''")
                //    });
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("AdditionalHoursCount <> ''")
                //    });
                //    foreach (var dataObject in EmployeeList)
                //    {
                //        foreach (var obj in dataObject.EmployeeAnnualLeaves)
                //        {
                //            if (obj.AdditionalHoursCount == 0)
                //            {
                //                string DisplayValues = "0d";
                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.AdditionalHoursCount.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //            else
                //            {
                //                Int32 Days = (Int32)obj.AdditionalHoursCount / (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //                Int32 Hours = (Int32)obj.AdditionalHoursCount % (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;

                //                string DisplayValues;
                //                if (Hours == 0)
                //                    DisplayValues = Days.ToString() + "d";
                //                else
                //                    DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";

                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.AdditionalHoursCount.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //        }

                //    }
                //}

                //else if (e.Column.FieldName == "Enjoyed")
                //{
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("Enjoyed = ''")
                //    });
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("Enjoyed <> ''")
                //    });
                //    foreach (var dataObject in EmployeeList)
                //    {
                //        foreach (var obj in dataObject.EmployeeAnnualLeaves)
                //        {
                //            if (obj.Enjoyed == 0)
                //            {
                //                string DisplayValues = "0d";
                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.Enjoyed.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //            else
                //            {
                //                Int32 Days = (Int32)obj.Enjoyed / (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //                Int32 Hours = (Int32)obj.Enjoyed % (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;

                //                string DisplayValues;
                //                if (Hours == 0)
                //                    DisplayValues = Days.ToString() + "d";
                //                else
                //                    DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";

                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.Enjoyed.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //        }

                //    }
                //}

                //else if (e.Column.FieldName == "Remaining")
                //{
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("Remaining = ''")
                //    });
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("Remaining <> ''")
                //    });
                //    foreach (var dataObject in EmployeeList)
                //    {
                //        foreach (var obj in dataObject.EmployeeAnnualLeaves)
                //        {
                //            if (obj.Remaining == 0)
                //            {
                //                string DisplayValues = "0d";
                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.Remaining.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //            else
                //            {
                //                Int32 Days = (Int32)obj.Remaining / (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //                Int32 Hours = (Int32)obj.Remaining % (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;

                //                string DisplayValues;
                //                if (Hours == 0)
                //                    DisplayValues = Days.ToString() + "d";
                //                else
                //                    DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";

                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.Remaining.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //        }
                //    }
                //}
                //else if (e.Column.FieldName == "Total")
                //{
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Blanks)",
                //        EditValue = CriteriaOperator.Parse("Total = ''")
                //    });
                //    filterItems.Add(new CustomComboBoxItem()
                //    {
                //        DisplayValue = "(Non blanks)",
                //        EditValue = CriteriaOperator.Parse("Total <> ''")
                //    });
                //    foreach (var dataObject in EmployeeList)
                //    {
                //        foreach (var obj in dataObject.EmployeeAnnualLeaves)
                //        {
                //            if (obj.RegularHoursCount == 0 && obj.AdditionalHoursCount == 0)
                //            {
                //                string DisplayValues = "0d";
                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.TotalHoursCount.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //            else
                //            {
                //                Int32 Days = ((Int32)obj.RegularHoursCount + (Int32)obj.AdditionalHoursCount) / (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                //                Int32 Hours = ((Int32)obj.RegularHoursCount + (Int32)obj.AdditionalHoursCount) % (Int32)obj.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;

                //                string DisplayValues;
                //                if (Hours == 0)
                //                    DisplayValues = Days.ToString() + "d";
                //                else
                //                    DisplayValues = Days.ToString() + "d" + " " + Hours.ToString() + "H";

                //                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DisplayValues))
                //                {
                //                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                //                    customComboBoxItem.DisplayValue = DisplayValues;
                //                    customComboBoxItem.EditValue = obj.TotalHoursCount.ToString();
                //                    filterItems.Add(customComboBoxItem);
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
                GeosApplication.Instance.Logger.Log("Method CustomShowFilterPopup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        /// [001] [cpatil][2020-03-17][GEOS2-1876] Draft users in Leaves.
        /// [002][cpatil][27-08-2020][GEOS2-2486] Wrong counting holidays - if many plants are selected, the program counts all official holidays for a selected employee.
        public void FillEmployeeLeavesSummary()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeavesSummary()...", category: Category.Info, priority: Priority.Low);
                MyFilterString = string.Empty;
                FillHrmDataInObjectsByCallingLatestServiceMethods.ShowPleaseWaitScreen();
                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                    //Service Method Changed from GetEmployeeDetailsForLeaveSummary_V2170 to GetEmployeeDetailsForLeaveSummary_V2420 by [rdixit][GEOS2-2466][10.08.2023]
                    //Service Method Changed from GetEmployeeDetailsForLeaveSummary_V2420 to GetEmployeeDetailsForLeaveSummary_V2590 by [rdixit][GEOS2-6571][24.12.2024]
                    EmployeeList = new ObservableCollection<Employee>(HrmService.GetEmployeeDetailsForLeaveSummary_V2590(plantOwnersIds, HrmCommon.Instance.SelectedPeriod, HrmCommon.Instance.ActiveEmployee.Organization, HrmCommon.Instance.ActiveEmployee.EmployeeDepartments, HrmCommon.Instance.IdUserPermission));
                    dynamicColumnsAdded = false;
                    foreach (var item in EmployeeList)
                    {
                        var itemEAL = item.EmployeeAnnualLeave;
                        if (itemEAL != null)
                        {
                            if (itemEAL.Employee.CompanyShift != null && itemEAL.Employee.CompanyShift.CompanyAnnualSchedule != null)
                            {
                                if (DailyHoursCount == 0)
                                {
                                    DailyHoursCount = itemEAL.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                                }
                                var empDailyHoursCount = itemEAL.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                                FormatEmployeeAnnualLeaveTotalHours(ref itemEAL, empDailyHoursCount);
                            }
                        }
                    }                   

                    foreach (var item in EmployeeList)
                    {
                        string tempAlias = string.Empty;
                        string tempDepartment = string.Empty;
                        if (item.EmployeeCompanyAlias.Contains('\n'))
                        {
                            string[] companyAlias = item.EmployeeCompanyAlias.Split('\n');
                            companyAlias = companyAlias.Distinct().ToArray();

                            if (companyAlias.Length > 1)
                            {
                                for (int i = 0; i < companyAlias.Length; i++)
                                {
                                    tempAlias = tempAlias + companyAlias[i] + "\n";
                                }
                            }
                            else
                                tempAlias = companyAlias[0];

                            item.EmployeeCompanyAlias = tempAlias;
                        }


                    }

                    if (EmployeeList.Count > 0)
                        SelectedLeaveSummary = EmployeeList[0];
                }
                else
                    EmployeeList = new ObservableCollection<Employee>();

                AddAdditionalBandGridColumns();
                GeosApplication.Instance.Logger.Log("Method FillEmployeeLeavesSummary()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeavesSummary() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEmployeeLeavesSummary() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEmployeeLeavesSummary()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            finally
            {
                FillHrmDataInObjectsByCallingLatestServiceMethods.ClosePleaseWaitScreen();
            }
        }

        private void FormatEmployeeAnnualLeaveTotalHours(
            ref EmployeeAnnualLeave leaveData, decimal empDailyHours)
        {
            leaveData.BacklogHoursInDays = FormatTotalHours(leaveData.BacklogHoursCount, empDailyHours);
            leaveData.RegularHoursInDays = FormatTotalHours(leaveData.RegularHoursCount, empDailyHours);
            leaveData.EnjoyedTillYesterdayInDays = FormatTotalHours(leaveData.EnjoyedTillYesterday, empDailyHours);
            leaveData.EnjoyingFromTodayInDays = FormatTotalHours(leaveData.EnjoyingFromToday, empDailyHours);

            if (leaveData.SelectedAdditionalLeaves != null)
            {
                foreach (var itemEAL_SelectedAdditionalLeave in leaveData.SelectedAdditionalLeaves)
                {
                    itemEAL_SelectedAdditionalLeave.AdditionalLeaveTotalHoursInDays =
                        FormatTotalHours(itemEAL_SelectedAdditionalLeave.AdditionalLeaveTotalHours, empDailyHours);
                }
            }
            //calculate total
            decimal totalAdditionalHours = 0;

            if (leaveData.SelectedAdditionalLeaves != null)
            {
                for (int i = 0; i < leaveData.SelectedAdditionalLeaves.Count; i++)
                {
                    totalAdditionalHours += leaveData.SelectedAdditionalLeaves[i].AdditionalLeaveTotalHours;
                }
            }

            leaveData.TotalHoursCount =
                (leaveData.RegularHoursCount +
                leaveData.BacklogHoursCount +
                totalAdditionalHours);

            leaveData.TotalHoursInDays = FormatTotalHours(leaveData.TotalHoursCount, empDailyHours);

            leaveData.Remaining =
                            (leaveData.RegularHoursCount +
                            leaveData.BacklogHoursCount +
                            totalAdditionalHours) -
                            leaveData.EnjoyedTillYesterday -
                            leaveData.EnjoyingFromToday;

            leaveData.RemainingHoursInDays = FormatTotalHours(leaveData.Remaining, empDailyHours);

        }

        /// <summary>
        /// [001][skale][15-01-2020][GEOS2-1658] The filter LeaveType in Summary Leaves does not work properly
        /// </summary>
        /// <param name="obj"></param>
        private void OpenEmployeeProfileDetail(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfileDetail()...", category: Category.Info, priority: Priority.Low);


                TableView detailView = (TableView)obj;
                Employee employee = (Employee)detailView.DataControl.CurrentItem;
                EmployeeProfileDetailView employeeProfileDetailView = new EmployeeProfileDetailView();
                EmployeeProfileDetailViewModel employeeProfileDetailViewModel = new EmployeeProfileDetailViewModel();
                EventHandler handle = delegate { employeeProfileDetailView.Close(); };
                employeeProfileDetailViewModel.RequestClose += handle;
                employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;
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

                if (HrmCommon.Instance.SelectedAuthorizedPlantsList != null)
                {
                    List<Company> plantOwners = HrmCommon.Instance.SelectedAuthorizedPlantsList.Cast<Company>().ToList();
                    var plantOwnersIds = string.Join(",", plantOwners.Select(i => i.IdCompany));

                    if (HrmCommon.Instance.IsPermissionReadOnly)
                        employeeProfileDetailViewModel.InitReadOnly(employee, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                    else
                        employeeProfileDetailViewModel.Init(employee, HrmCommon.Instance.SelectedPeriod, plantOwnersIds.ToString());
                }
                employeeProfileDetailView.DataContext = employeeProfileDetailViewModel;
                var ownerInfo = (detailView as FrameworkElement);
                employeeProfileDetailView.Owner = Window.GetWindow(ownerInfo);
                employeeProfileDetailView.ShowDialog();
                if (employeeProfileDetailViewModel.IsSaveChanges)
                {

                    employee.FirstName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.FirstName;
                    employee.LastName = employeeProfileDetailViewModel.EmployeeUpdatedDetail.LastName;
                    employee.FullName = employee.FirstName + " " + employee.LastName;
                    employee.EmployeeAnnualLeaves = employeeProfileDetailViewModel.EmployeeAnnualLeaveList.ToList();

                   // [001] added
                    List<EmployeeAnnualLeave> employeeAnnualLeaveList = employeeProfileDetailViewModel.EmployeeUpdatedDetail.EmployeeAnnualLeaves;
                    DailyHoursCount = 0;

                    if (employeeAnnualLeaveList != null && employeeAnnualLeaveList.Count > 0)
                    {
                        foreach (EmployeeAnnualLeave item in employeeAnnualLeaveList)
                        {

                            if (item.Employee.CompanyShift != null)
                            {
                                if (item.Employee.CompanyShift.CompanyAnnualSchedule != null)
                                    DailyHoursCount = item.Employee.CompanyShift.CompanyAnnualSchedule.DailyHoursCount;
                                else
                                    DailyHoursCount = 0;
                            }


                            if (item.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {

                                List<Employee> EmployeeListCount= EmployeeList.Where(x=>x.IdEmployee== item.IdEmployee).ToList();
                                Employee empLeave = EmployeeList.Where(x => x.IdEmployee == item.IdEmployee && x.EmployeeAnnualLeave.IdLeave == item.IdLeave).FirstOrDefault();

                                if (EmployeeListCount.Count == 1)
                                {
                                    empLeave.EmployeeAnnualLeave = null;
                                }
                                else
                                {
                                    if (empLeave != null)
                                    {
                                        EmployeeList.Remove(empLeave);
                                    }
                                }
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                Employee employeeClone = new Employee();
                                employeeClone = (Employee)employee.Clone();

                                int index = EmployeeList.ToList().FindLastIndex(x => x.IdEmployee == item.IdEmployee);

                                if (EmployeeList[index].EmployeeAnnualLeave == null)
                                {
                                    

                                    //employee.EmployeeAnnualLeave = new EmployeeAnnualLeave();
                                    //employee.EmployeeAnnualLeave = item;
                                    var leaveData = (EmployeeAnnualLeave)item.Clone();
                                    FormatEmployeeAnnualLeaveTotalHours(ref leaveData, DailyHoursCount);
                                    employee.EmployeeAnnualLeave = leaveData;
                                    //decimal Total = item.RegularHoursCount + item.AdditionalHoursCount;
                                    //employee.EmployeeAnnualLeave.RegularHoursInDays = TotalDaysAndHours(item.RegularHoursCount, DailyHoursCount);
                                    //employee.EmployeeAnnualLeave.AdditionalHoursInDays = TotalDaysAndHours(item.AdditionalHoursCount, DailyHoursCount);
                                    //employee.EmployeeAnnualLeave.TotalHoursInDays = TotalDaysAndHours(Total, DailyHoursCount);
                                    //employee.EmployeeAnnualLeave.EnjoyedHoursInDays = TotalDaysAndHours(item.Enjoyed, DailyHoursCount);
                                    //employee.EmployeeAnnualLeave.RemainingHoursInDays = TotalDaysAndHours(item.Remaining, DailyHoursCount);
                                }
                                else
                                {

                                    //employeeClone.EmployeeAnnualLeave = new EmployeeAnnualLeave();
                                    //employeeClone.EmployeeAnnualLeave.CompanyLeave = item.CompanyLeave;
                                    //employeeClone.EmployeeAnnualLeave.IdLeave = item.IdLeave;
                                    //decimal Total = item.RegularHoursCount + item.AdditionalHoursCount;
                                    //employeeLeave.EmployeeAnnualLeave.RegularHoursInDays = TotalDaysAndHours(item.RegularHoursCount, DailyHoursCount);
                                    //employeeLeave.EmployeeAnnualLeave.AdditionalHoursInDays = TotalDaysAndHours(item.AdditionalHoursCount, DailyHoursCount);
                                    //employeeLeave.EmployeeAnnualLeave.TotalHoursInDays = TotalDaysAndHours(Total, DailyHoursCount);
                                    //employeeLeave.EmployeeAnnualLeave.EnjoyedHoursInDays = TotalDaysAndHours(item.Enjoyed, DailyHoursCount);
                                    //employeeLeave.EmployeeAnnualLeave.RemainingHoursInDays = TotalDaysAndHours(item.Remaining, DailyHoursCount);

                                    var leaveData = (EmployeeAnnualLeave)item.Clone();
                                    FormatEmployeeAnnualLeaveTotalHours(ref leaveData, DailyHoursCount);
                                    employeeClone.EmployeeAnnualLeave = leaveData;
                                    employeeClone.IdEmployee = item.IdEmployee;

                                    EmployeeList.Insert(index + 1, employeeClone);
                                }
                            }
                            else if (item.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                Employee empLeave = EmployeeList.Where(x => x.IdEmployee == item.IdEmployee && 
                                x.EmployeeAnnualLeave.IdLeave == item.IdLeave).FirstOrDefault();

                                if (empLeave !=null)
                                {
                                    var leaveData = (EmployeeAnnualLeave)item.Clone();
                                    FormatEmployeeAnnualLeaveTotalHours(ref leaveData, DailyHoursCount);
                                    empLeave.EmployeeAnnualLeave = leaveData;
                                    //decimal Total = item.RegularHoursCount + item.AdditionalHoursCount;
                                    //empLeave.EmployeeAnnualLeave = item;
                                    ////empLeave.EmployeeAnnualLeave
                                    //empLeave.EmployeeAnnualLeave.RegularHoursInDays = TotalDaysAndHours(item.RegularHoursCount, DailyHoursCount);
                                    //empLeave.EmployeeAnnualLeave.AdditionalHoursInDays = TotalDaysAndHours(item.AdditionalHoursCount, DailyHoursCount);
                                    //empLeave.EmployeeAnnualLeave.TotalHoursInDays = TotalDaysAndHours(Total, DailyHoursCount);
                                    //empLeave.EmployeeAnnualLeave.EnjoyedHoursInDays = TotalDaysAndHours(item.Enjoyed, DailyHoursCount);
                                    //empLeave.EmployeeAnnualLeave.RemainingHoursInDays = TotalDaysAndHours(item.Remaining, DailyHoursCount);
                                }
                            }
                        }
                    }
                    //end
                    var TempEmployeeJobDescriptionList = new ObservableCollection<EmployeeJobDescription>(employeeProfileDetailViewModel.EmployeeJobDescriptionsList.Where(a => a.JobDescriptionEndDate == null || a.JobDescriptionEndDate >= DateTime.Now).ToList());
                    if (TempEmployeeJobDescriptionList.Count == 0)
                    {
                        if (employeeProfileDetailViewModel.EmployeeTopFourJobDescriptionList.Count == 1)
                        {
                            employee.EmployeeJobTitles = String.Join("\n", employeeProfileDetailViewModel.EmployeeTopFourJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                        }
                        else
                        {
                            employee.EmployeeJobTitles = String.Join("\n", employeeProfileDetailViewModel.EmployeeTopFourJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)").ToArray());
                        }
                        employee.LstEmployeeDepartments = employeeProfileDetailViewModel.EmployeeTopFourJobDescriptionList.Select(x => x.JobDescription.Department).ToList();
                        employee.LstEmployeeDepartments = employee.LstEmployeeDepartments.GroupBy(x => x.DepartmentName).Select(y => y.First()).ToList();
                    }
                    else
                    {

                        if (TempEmployeeJobDescriptionList.Count == 1)
                        {
                            employee.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle).ToArray());
                        }
                        else
                        {
                            employee.EmployeeJobTitles = String.Join("\n", TempEmployeeJobDescriptionList.Select(x => x.JobDescription.JobDescriptionTitle + " (" + x.JobDescriptionUsage + "%)").ToArray());
                        }
                        employee.LstEmployeeDepartments = TempEmployeeJobDescriptionList.Select(x => x.JobDescription.Department).ToList();
                        employee.LstEmployeeDepartments = employee.LstEmployeeDepartments.GroupBy(x => x.DepartmentName).Select(y => y.First()).ToList();
                    }
                    EmployeeList = new ObservableCollection<Employee>(EmployeeList);
                    SelectedLeaveSummary = employee;

                    foreach (var item in EmployeeList)
                    {
                        if (item.IdEmployee == employee.IdEmployee)
                        {
                            item.FirstName = employee.FirstName;
                            item.LastName = employee.LastName;
                            item.EmployeeJobTitles = employee.EmployeeJobTitles;
                        }
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenEmployeeProfileDetail()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeProfileDetail()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [001][skale][15-01-2020][GEOS2-1658] The filter LeaveType in Summary Leaves does not work properly
        /// </summary>
        /// <param name="e"></param>
        private void CustomRowFilter(RowFilterEventArgs e)
        {
            try
            {
               // GeosApplication.Instance.Logger.Log("Method CustomRowFilter ...", category: Category.Info, priority: Priority.Low);
                var criteria = e.Source.FilterString;
                //[001] add comment
                #region Old Code Comment by skale
                //if (criteria.Contains("[LeavesType]"))
                //{
                //    if (criteria.Contains("In"))
                //    {
                //        var initVals = criteria.Split(new String[] { "[LeavesType] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty"))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.CompanyLeave.Name)).Count() > 0;
                //        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("="))
                //    {
                //        var initVals = criteria.Split(new String[] { "[LeavesType] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty") || initVals.Contains(" "))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.CompanyLeave.Name)).Count() > 0;
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("<>"))
                //    {
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count != 0;
                //    }
                //    e.Handled = true;
                //}

                //else if (criteria.Contains("[RegularHoursCount]"))
                //{
                //    if (criteria.Contains("In"))
                //    {
                //        var initVals = criteria.Split(new String[] { "[RegularHoursCount] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty"))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.RegularHoursCount.ToString())).Count() > 0;
                //        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("="))
                //    {
                //        var initVals = criteria.Split(new String[] { "[RegularHoursCount] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty") || initVals.Contains(" "))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("<>"))
                //    {
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count != 0;
                //    }
                //    e.Handled = true;
                //}
                //else if (criteria.Contains("[AdditionalHoursCount]"))
                //{
                //    if (criteria.Contains("In"))
                //    {
                //        var initVals = criteria.Split(new String[] { "[AdditionalHoursCount] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty"))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.AdditionalHoursCount.ToString())).Count() > 0;
                //        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("<>"))
                //    {
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count != 0;
                //    }
                //    if (criteria.Contains("="))
                //    {
                //        var initVals = criteria.Split(new String[] { "[AdditionalHoursCount] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty") || initVals.Contains(" "))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }

                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }

                //    e.Handled = true;
                //}
                //else if (criteria.Contains("[Enjoyed]"))
                //{
                //    if (criteria.Contains("In"))
                //    {
                //        var initVals = criteria.Split(new String[] { "[Enjoyed] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty"))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.Enjoyed.ToString())).Count() > 0;
                //        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("<>"))
                //    {

                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count != 0;
                //    }
                //    if (criteria.Contains("="))
                //    {
                //        var initVals = criteria.Split(new String[] { "[Enjoyed] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty") || initVals.Contains(" "))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }

                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    e.Handled = true;
                //}

                //else if (criteria.Contains("[Remaining]"))
                //{
                //    if (criteria.Contains("In"))
                //    {
                //        var initVals = criteria.Split(new String[] { "[Remaining] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty"))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.Remaining.ToString())).Count() > 0;
                //        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("<>"))
                //    {

                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count != 0;
                //    }
                //    if (criteria.Contains("="))
                //    {
                //        var initVals = criteria.Split(new String[] { "[Remaining] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty") || initVals.Contains(" "))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }

                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }

                //    e.Handled = true;
                //}
                //else if (criteria.Contains("[Total]"))
                //{
                //    if (criteria.Contains("In"))
                //    {
                //        var initVals = criteria.Split(new String[] { "[Total] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty"))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }
                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Select(x => x.TotalHoursCount.ToString())).Count() > 0;
                //        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }
                //    if (criteria.Contains("<>"))
                //    {

                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count != 0;
                //    }
                //    if (criteria.Contains("="))
                //    {
                //        var initVals = criteria.Split(new String[] { "[Total] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                //        bool includeEmpty = false;
                //        if (initVals.Contains("Empty") || initVals.Contains(" "))
                //        {
                //            includeEmpty = true;
                //            initVals = initVals.Where(c => c != "Empty").ToArray();
                //        }

                //        var vals = initVals.Select(x => x).ToList();
                //        e.Visible |= EmployeeList[e.ListSourceRowIndex].EmployeeAnnualLeaves.Count == 0 && includeEmpty;
                //    }

                //    e.Handled = true;
                //}
                #endregion

                if (criteria.Contains("[Departments]"))
                {
                    if (criteria.Contains("In"))
                    {
                        var initVals = criteria.Split(new String[] { "[Departments] In", "'", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool includeEmpty = false;
                        if (initVals.Contains("Empty"))
                        {
                            includeEmpty = true;
                            initVals = initVals.Where(c => c != "Empty").ToArray();
                        }
                        var vals = initVals.Select(x => x).ToList();
                        e.Visible = vals.Intersect(this.EmployeeList[e.ListSourceRowIndex].LstEmployeeDepartments.Select(x => x.DepartmentName)).Count() > 0;
                        e.Visible |= this.EmployeeList[e.ListSourceRowIndex].LstEmployeeDepartments.Count == 0 && includeEmpty;
                    }
                    if (criteria.Contains("="))
                    {
                        var initVals = criteria.Split(new String[] { "[Departments] =", "", "'", "'", "," }, StringSplitOptions.RemoveEmptyEntries);
                        bool includeEmpty = false;
                        if (initVals.Contains("Empty") || initVals.Contains(" "))
                        {
                            includeEmpty = true;
                            initVals = initVals.Where(c => c != "Empty").ToArray();
                        }
                        var vals = initVals.Select(x => x).ToList();
                        e.Visible = vals.Intersect(EmployeeList[e.ListSourceRowIndex].LstEmployeeDepartments.Select(x => x.DepartmentName)).Count() > 0;
                        e.Visible |= EmployeeList[e.ListSourceRowIndex].LstEmployeeDepartments.Count == 0 && includeEmpty;
                    }
                    if (criteria.Contains("<>"))
                    {
                        e.Visible |= EmployeeList[e.ListSourceRowIndex].LstEmployeeDepartments.Count != 0;
                    }


                    e.Handled = true;
                }

               // GeosApplication.Instance.Logger.Log("Method CustomRowFilter() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomRowFilter() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this method use for convert hours into days
        /// [000][skale][15-01-2020][GEOS2-1658] The filter LeaveType in Summary Leaves does not work properly
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="dailyWorkHours"></param>
        /// <returns></returns>
        string TotalDaysAndHours(decimal hours, decimal dailyWorkHours)
        {
            decimal TotalHours = hours;
            decimal DailyHours = dailyWorkHours;
            if (TotalHours == 0)
            {
                return TotalHours.ToString() + "d";
            }

            decimal Days = TotalHours / DailyHours;
            decimal Hours = TotalHours % DailyHours;

            if (Hours == 0)
                return Days.ToString() + "d";

            return string.Format("{0}d {1}H", (Int32)Days, Hours.ToString("0.##"));
        }
        
        private string FormatTotalHours(decimal hours, decimal dailyHoursCount)
        {
            decimal TotalHours = hours;
            decimal DailyHours = dailyHoursCount;
            if (TotalHours == 0)
            {
                return TotalHours.ToString() + "d";
            }

            decimal Days = TotalHours / DailyHours;
            decimal Hours = TotalHours % DailyHours;

            if (Hours == 0)
                return Days.ToString() + "d";

            return string.Format("{0}d {1}H", (Int32)Days, Hours.ToString("0.##"));
        }
        #endregion  //Methods
    }
}
