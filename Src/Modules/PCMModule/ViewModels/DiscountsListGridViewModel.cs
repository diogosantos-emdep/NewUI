using System;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Data.Filtering;
using System.ComponentModel;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common.PCM;
using DevExpress.Xpf.Core;
using Prism.Logging;
using System.Windows.Input;
using System.Collections.Generic;
using Emdep.Geos.UI.Commands;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Xpf.Grid;
using System.ServiceModel;
using static System.Net.Mime.MediaTypeNames;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using DevExpress.Xpf.Printing;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Export.Xl;
using DevExpress.XtraPrinting.Drawing;
using System.Linq;

namespace Emdep.Geos.Modules.PCM.ViewModels
{   
    //[001][kshinde][27/07/2022][GEOS2-3099]
    //[rdixit][17.08.2022][GEOS2-3099] 
    public class DiscountsListGridViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
       IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IPCMService PCMService = new PCMServiceController("localhost:6699");
       // IPLMService PLMService = new PLMServiceController("localhost:6699");

        #endregion

        #region Public Events
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

        #region Declarations

        private ObservableCollection<Discounts> discountsList;
        private Discounts selectedDiscounts;

        private bool isDLColumnChooserVisible;
        private bool isBusy;
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        private ObservableCollection<Site> plantList;
        #endregion

        #region Properties

        public string DLGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "DLGridSetting.Xmal";

        public ObservableCollection<Discounts> DiscountsList
        {
            get { return discountsList; }
            set
            {
                discountsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiscountsList"));
            }
        }
        public Discounts SelectedDiscounts
        {
            get
            {
                return selectedDiscounts;
            }
            set
            {
                selectedDiscounts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDiscounts"));
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

        public bool IsDLColumnChooserVisible
        {
            get
            {
                return isDLColumnChooserVisible;
            }

            set
            {
                isDLColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDLColumnChooserVisible"));
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
        #endregion

        #region ICommands
        public ICommand AddDiscountsListCommand { get; set; }
        public ICommand RefreshDiscountsListCommand { get; set; }
        public ICommand PrintDiscountsListCommand { get; set; }
        public ICommand ExportDiscounts { get; set; }
        public ICommand EditDiscountCommand { get; set; }
        public ICommand DiscountShowFilterPopupCommand { get; set; }

        #endregion

        #region Constructor

        public DiscountsListGridViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DiscountListGridViewModel ...", category: Category.Info, priority: Priority.Low);
                AddDiscountsListCommand = new RelayCommand(new Action<object>(AddDiscountsListAction));
                RefreshDiscountsListCommand = new RelayCommand(new Action<object>(RefreshDiscountsListAction));
                PrintDiscountsListCommand = new RelayCommand(new Action<object>(PrintDiscountsListAction));
                ExportDiscounts = new RelayCommand(new Action<object>(ExportDiscountsAction));
                EditDiscountCommand = new RelayCommand(new Action<object>(EditDiscountAction));
                DiscountShowFilterPopupCommand = new DelegateCommand<DevExpress.Xpf.Grid.FilterPopupEventArgs>(DiscountShowFilterPopup);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor DiscountListGridViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            FillDiscounts();
        }
        public void FillDiscounts()
        {
            try
            {
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                //GetDiscounts() Service Method Updated to GetDiscounts_V2320() on [rdixit][GEOS2-3101][23.09.2022]
                DiscountsList = new ObservableCollection<Discounts>(PCMService.GetDiscounts_V2320());
                PlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                foreach (var item in DiscountsList)
                {
                    List<string> result = item.Plants.Split('\n').ToList();
                    if (result.Count == PlantList.Count())
                    {
                        item.Plants = "ALL";
                    }
                    /*
                    try
                    {
                        item.EndDateNew = Convert.ToDateTime(item.EndDate);
                        item.LastUpdateNew = Convert.ToDateTime(item.LastUpdate);
                        item.StartDateNew = Convert.ToDateTime(item.StartDate);
                    }
                    catch (Exception ex)
                    { }*/
                }
                if (DiscountsList.Count > 0)
                    SelectedDiscounts = DiscountsList.FirstOrDefault();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDiscounts() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillDiscounts() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDiscounts()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }              
        private void RefreshDiscountsListAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefreshDiscountsListAction()...", category: Category.Info, priority: Priority.Low);
                TableView detailView = (TableView)obj;
                GridControl gridControl = (detailView).Grid;
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                FillDiscounts();
                detailView.SearchString = null;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method RefreshDiscountsListAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDiscountsListAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), System.Windows.Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in RefreshDiscountsListAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method RefreshDiscountsListAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PrintDiscountsListAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintDiscountsListAction()...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;

                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                PrintableControlLink pcl = new PrintableControlLink((TableView)obj);
                pcl.Margins.Bottom = 5;
                pcl.Margins.Top = 5;
                pcl.Margins.Left = 5;
                pcl.Margins.Right = 5;
                pcl.PaperKind = System.Drawing.Printing.PaperKind.BPlus;
                pcl.PageHeaderTemplate = (DataTemplate)((TableView)obj).Resources["DiscountsListPrintHeaderTemplate"];
                pcl.PageFooterTemplate = (DataTemplate)((TableView)obj).Resources["DiscountsListPrintFooterTemplate"];
                pcl.Landscape = true;
                pcl.CreateDocument(false);

                DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };
                window.PreviewControl.DocumentSource = pcl;
                IsBusy = false;
                window.Show();

                DevExpress.XtraPrinting.PrintTool printTool = new DevExpress.XtraPrinting.PrintTool(pcl.PrintingSystem);
                printTool.PrinterSettings.PrinterName = GeosApplication.Instance.UserSettings["SelectedPrinter"];

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method PrintDiscountsListAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintDiscountsListAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ExportDiscountsAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportDiscountsAction()...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
                saveFile.DefaultExt = "xlsx";
                saveFile.FileName = "Discounts";
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
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    ResultFileName = (saveFile.FileName);
                    TableView activityTableView = ((TableView)obj);
                    activityTableView.ShowTotalSummary = false;
                    activityTableView.ShowFixedTotalSummary = false;
                    XlsxExportOptionsEx options = new XlsxExportOptionsEx();
                    options.CustomizeCell += Options_CustomizeCell;
                    activityTableView.ExportToXlsx(ResultFileName, options);

                    IsBusy = false;
                    activityTableView.ShowFixedTotalSummary = true;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    System.Diagnostics.Process.Start(ResultFileName);

                    GeosApplication.Instance.Logger.Log("Method ExportDiscountsAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in Method ExportDiscountsAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        private void DiscountShowFilterPopup(DevExpress.Xpf.Grid.FilterPopupEventArgs e)
        {
            GeosApplication.Instance.Logger.Log(" Method DiscountShowFilterPopup ...", category: Category.Info, priority: Priority.Low);
            try
            {
                List<object> filterItems = new List<object>();
                if (e.Column.FieldName != "Plants" && e.Column.FieldName != "Region" && e.Column.FieldName != "Group" && e.Column.FieldName != "Country" && e.Column.FieldName != "Plant")
                {
                    return;
                }
                #region Plants
                if (e.Column.FieldName == "Plants")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("Plants = ''")
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("Plants <> ''")
                    });

                    foreach (var dataObject in DiscountsList)
                    {
                        if (dataObject.Plants == null)
                        {
                            continue;
                        }
                        else if (dataObject.Plants != null)
                        {
                            if (dataObject.Plants.Contains("\n"))
                            {
                                string tempPlants = dataObject.Plants;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string plant = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == plant))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = plant;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", plant));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, plant.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DiscountsList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = DiscountsList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plants Like '%{0}%'", DiscountsList.Where(y => y.Plants == dataObject.Plants).Select(slt => slt.Plants).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Group
                else if (e.Column.FieldName == "Group")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Group])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Group])")
                    });

                    foreach (var dataObject in DiscountsList)
                    {
                        if (dataObject.Group == null)
                        {
                            continue;
                        }
                        else if (dataObject.Group != null)
                        {
                            if (dataObject.Group.Contains("\n"))
                            {
                                string tempGroup = dataObject.Group;
                                for (int index = 0; index < tempGroup.Length; index++)
                                {
                                    string empGroup = tempGroup.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empGroup))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empGroup;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Group Like '%{0}%'", empGroup));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempGroup.Contains("\n"))
                                        tempGroup = tempGroup.Remove(0, empGroup.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DiscountsList.Where(y => y.Group == dataObject.Group).Select(slt => slt.Group).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = DiscountsList.Where(y => y.Group == dataObject.Group).Select(slt => slt.Group).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Group Like '%{0}%'", DiscountsList.Where(y => y.Group == dataObject.Group).Select(slt => slt.Group).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Region
                else if (e.Column.FieldName == "Region")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Region])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Region])")
                    });

                    foreach (var dataObject in DiscountsList)
                    {
                        if (dataObject.Region == null)
                        {
                            continue;
                        }
                        else if (dataObject.Region != null)
                        {
                            if (dataObject.Region.Contains("\n"))
                            {
                                string tempRegion = dataObject.Region;
                                for (int index = 0; index < tempRegion.Length; index++)
                                {
                                    string empRegion = tempRegion.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empRegion))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empRegion;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Region Like '%{0}%'", empRegion));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempRegion.Contains("\n"))
                                        tempRegion = tempRegion.Remove(0, empRegion.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DiscountsList.Where(y => y.Region == dataObject.Region).Select(slt => slt.Region).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = DiscountsList.Where(y => y.Region == dataObject.Region).Select(slt => slt.Region).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Region Like '%{0}%'", DiscountsList.Where(y => y.Region == dataObject.Region).Select(slt => slt.Region).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Country
                else if (e.Column.FieldName == "Country")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Country])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Country])")
                    });

                    foreach (var dataObject in DiscountsList)
                    {
                        if (dataObject.Country == null)
                        {
                            continue;
                        }
                        else if (dataObject.Country != null)
                        {
                            if (dataObject.Country.Contains("\n"))
                            {
                                string tempCountry = dataObject.Country;
                                for (int index = 0; index < tempCountry.Length; index++)
                                {
                                    string empCountry = tempCountry.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empCountry))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empCountry;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", empCountry));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempCountry.Contains("\n"))
                                        tempCountry = tempCountry.Remove(0, empCountry.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DiscountsList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = DiscountsList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Country Like '%{0}%'", DiscountsList.Where(y => y.Country == dataObject.Country).Select(slt => slt.Country).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Plant
                else if (e.Column.FieldName == "Plant")
                {
                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Blanks)",
                        EditValue = CriteriaOperator.Parse("IsNull([Plant])")//[002] added
                    });

                    filterItems.Add(new CustomComboBoxItem()
                    {
                        DisplayValue = "(Non blanks)",
                        EditValue = CriteriaOperator.Parse("!IsNull([Plant])")
                    });

                    foreach (var dataObject in DiscountsList)
                    {
                        if (dataObject.Plant == null)
                        {
                            continue;
                        }
                        else if (dataObject.Plant != null)
                        {
                            if (dataObject.Plant.Contains("\n"))
                            {
                                string tempPlants = dataObject.Plant;
                                for (int index = 0; index < tempPlants.Length; index++)
                                {
                                    string empPlants = tempPlants.Split('\n').First();

                                    if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == empPlants))
                                    {
                                        CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                        customComboBoxItem.DisplayValue = empPlants;
                                        customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plant Like '%{0}%'", empPlants));
                                        filterItems.Add(customComboBoxItem);
                                    }
                                    if (tempPlants.Contains("\n"))
                                        tempPlants = tempPlants.Remove(0, empPlants.Length + 1);
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                if (!filterItems.Any(x => Convert.ToString(((CustomComboBoxItem)x).DisplayValue) == DiscountsList.Where(y => y.Plant == dataObject.Plant).Select(slt => slt.Plant).FirstOrDefault().Trim()))
                                {
                                    CustomComboBoxItem customComboBoxItem = new CustomComboBoxItem();
                                    customComboBoxItem.DisplayValue = DiscountsList.Where(y => y.Plant == dataObject.Plant).Select(slt => slt.Plant).FirstOrDefault().Trim();
                                    customComboBoxItem.EditValue = CriteriaOperator.Parse(string.Format("Plant Like '%{0}%'", DiscountsList.Where(y => y.Plant == dataObject.Plant).Select(slt => slt.Plant).FirstOrDefault().Trim()));
                                    filterItems.Add(customComboBoxItem);
                                }
                            }
                        }
                    }
                }
                #endregion
                e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(sort => Convert.ToString(((CustomComboBoxItem)sort).DisplayValue)).ToList();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DiscountShowFilterPopup() method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        private void Options_CustomizeCell(CustomizeCellEventArgs e)
        {
            e.Formatting.Alignment = new XlCellAlignment() { WrapText = true };
            e.Handled = true;
        }
        //[rdixit][13.09.2022][GEOS2-3099]
        private void AddDiscountsListAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDiscountsListAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;

                AddEditDiscountsListGridView AddEditDiscountsListGridView = new AddEditDiscountsListGridView();
                AddEditDiscountsListGridViewModel AddEditDiscountsListGridViewModel = new AddEditDiscountsListGridViewModel();
                if (obj is TableView)
                {
                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }

                    AddEditDiscountsListGridView.DataContext = AddEditDiscountsListGridViewModel;
                    EventHandler handle = delegate { AddEditDiscountsListGridView.Close(); };
                    AddEditDiscountsListGridViewModel.RequestClose += handle;
                    AddEditDiscountsListGridViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddDiscountsList").ToString();
                    AddEditDiscountsListGridViewModel.IsNew = true;
                    AddEditDiscountsListGridViewModel.IsFirstTimeLoad = true;
                    AddEditDiscountsListGridViewModel.Init(System.Windows.Application.Current.FindResource("AddDiscountsList").ToString());
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    TableView detailView = (TableView)obj;
                    var ownerInfo = (detailView as FrameworkElement);
                    AddEditDiscountsListGridView.Owner = Window.GetWindow(ownerInfo);
                    AddEditDiscountsListGridView.ShowDialog();
                    if (AddEditDiscountsListGridViewModel.IsNewDiscountSave == true)
                    {
                        FillDiscounts();
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AddDiscountsListAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EditDiscountAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditDiscountsAction()", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;
                AddEditDiscountsListGridView AddEditDiscountsListGridView = new AddEditDiscountsListGridView();
                AddEditDiscountsListGridViewModel AddEditDiscountsListGridViewModel = new AddEditDiscountsListGridViewModel();
                if (obj is TableView)
                {
                    //TableView detailView = (TableView)obj;
                    //var ownerInfo = (detailView as FrameworkElement);

                    if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                    TableView detailView = (TableView)obj;
                    var ownerInfo = (detailView as FrameworkElement);
                    AddEditDiscountsListGridView.DataContext = AddEditDiscountsListGridViewModel;
                    EventHandler handle = delegate { AddEditDiscountsListGridView.Close(); };
                    AddEditDiscountsListGridViewModel.RequestClose += handle;
                    AddEditDiscountsListGridViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditDiscountsList").ToString();
                    AddEditDiscountsListGridViewModel.IsNew = false;
                    AddEditDiscountsListGridViewModel.IsFirstTimeLoad = true;
                    AddEditDiscountsListGridViewModel.EditInit(SelectedDiscounts);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                    AddEditDiscountsListGridView.Owner = Window.GetWindow(ownerInfo);
                    AddEditDiscountsListGridViewModel.IsEnabledCancelButton = false;//[Sudhir.Jangra][GEOS2-3132][15/02/2023]
                    AddEditDiscountsListGridView.ShowDialog();
                    if (AddEditDiscountsListGridViewModel.IsDiscountUpdated == true)
                    {
                        FillDiscounts();
                    }
                    else
                    {
                        SelectedDiscounts.DiscountArticles = AddEditDiscountsListGridViewModel.ClonedDiscount.DiscountArticles;
                    }
                
                }
                //if (!DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditDiscountsAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
