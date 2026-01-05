using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Warehouse.Common_Classes;
using Emdep.Geos.Modules.Warehouse.Views;
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
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class EditBoxViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private string boxNumber;
        private double length;
        private double width;
        private double height;
        private double netWeight;
        private double grossWeight;
        private List<PackingBoxType> packingBoxTypeList;
        private int selectedPackingBoxTypeIndex;
        private bool isResult;
        private bool isSave;
        private PackingBox existPackingBox;
        private string windowHeader;
        private Visibility isOpenCloseButtonVisibile;
        //private sbyte isClosed;
        private bool isCustomBox;
        private List<Company> customersList;
        private int selectedCustomerIndex;
        private ObservableCollection<OriginOfContent> originOfContentList;
        private int selectedoriginOfContentIndex;
        private Visibility isPrintButtonVisibile;
        private sbyte isStackable;
        private long? idWarehouse;
        private bool isBoxNumberExist;
        private Visibility isWeightMachineButtonVisibility;
        public bool isWeightAndSizeValueNull { get; set; }//[Sudhir.Jangra][GEOS2-4542][09/08/2023]

        private Visibility isGrossWeightVisible;//[Sudhir.Jangra][GEOS2-5646]
        #endregion

        #region Properties
        public long? IdWarehouse
        {
            get
            {
                return idWarehouse;
            }

            set
            {
                idWarehouse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdWarehouse"));
            }
        }
        public PackingBox UpdatePackingBox { get; set; }
        PrintLabel PrintLabel { get; set; }
        Dictionary<string, string> PrintValues { get; set; }
        public string BoxNumber
        {
            get
            {
                return boxNumber;
            }

            set
            {
                boxNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BoxNumber"));
            }
        }

        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Length"));
            }
        }

        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
        }

        public double Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }

        public double NetWeight
        {
            get
            {
                return netWeight;
            }

            set
            {
                netWeight = Math.Round(value, 3);
                OnPropertyChanged(new PropertyChangedEventArgs("NetWeight"));
            }
        }

        public double GrossWeight
        {
            get
            {
                return grossWeight;
            }

            set
            {
                grossWeight = Math.Round(value, 3);
                OnPropertyChanged(new PropertyChangedEventArgs("GrossWeight"));
            }
        }
        public List<PackingBoxType> PackingBoxTypeList
        {
            get
            {
                return packingBoxTypeList;
            }

            set
            {
                packingBoxTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PackingBoxTypeList"));
            }
        }
        public int SelectedPackingBoxTypeIndex
        {
            get
            {
                return selectedPackingBoxTypeIndex;
            }

            set
            {
                selectedPackingBoxTypeIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPackingBoxTypeIndex"));
            }
        }
        public bool IsResult
        {
            get
            {
                return isResult;
            }

            set
            {
                isResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsResult"));
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
        public PackingBox ExistPackingBox
        {
            get
            {
                return existPackingBox;
            }

            set
            {
                existPackingBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdSite"));
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
        public Visibility IsOpenCloseButtonVisibile
        {
            get
            {
                return isOpenCloseButtonVisibile;
            }

            set
            {
                isOpenCloseButtonVisibile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOpenCloseButtonVisibile"));
            }
        }

        public Visibility IsPrintButtonVisibile
        {
            get
            {
                return isPrintButtonVisibile;
            }

            set
            {
                isPrintButtonVisibile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPrintButtonVisibile"));
            }
        }

        //public sbyte IsClosed
        //{
        //    get
        //    {
        //        return isClosed;
        //    }

        //    set
        //    {
        //        isClosed = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("IsClosed"));
        //    }
        //}
        public bool IsCustomBox
        {
            get
            {
                return isCustomBox;
            }

            set
            {
                isCustomBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomBox"));
            }
        }
        public List<Company> CustomersList
        {
            get
            {
                return customersList;
            }

            set
            {
                customersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomersList"));
            }
        }
        public int SelectedCustomerIndex
        {
            get
            {
                return selectedCustomerIndex;
            }

            set
            {
                selectedCustomerIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerIndex"));
            }
        }

        public ObservableCollection<OriginOfContent> OriginOfContentList
        {
            get
            {
                return originOfContentList;
            }

            set
            {
                originOfContentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginOfContentList"));
            }
        }
        public int SelectedOriginOfContentIndex
        {
            get
            {
                return selectedoriginOfContentIndex;
            }

            set
            {
                if (value == -1)
                {
                    value = 0;
                }
                selectedoriginOfContentIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOriginOfContentIndex"));
            }
        }

        public sbyte IsStackable
        {
            get { return isStackable; }
            set { isStackable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsStackable")); }
        }

        public bool IsBoxNumberExist
        {
            get { return isBoxNumberExist; }
            set { isBoxNumberExist = value; OnPropertyChanged(new PropertyChangedEventArgs("IsBoxNumberExist")); }
        }

        public Visibility IsWeightMachineButtonVisibility
        {
            get { return isWeightMachineButtonVisibility; }
            set { isWeightMachineButtonVisibility = value; OnPropertyChanged(new PropertyChangedEventArgs("IsWeightMachineButtonVisibility")); }
        }

        //[Sudhir.Jangra][GEOS2-5646]
        public Visibility IsGrossWeightVisible
        {
            get { return isGrossWeightVisible; }
            set
            {
                isGrossWeightVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGrossWeightVisible"));
            }
        }

        #endregion

        #region Events

        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events

        #region ICommands
        public ICommand WeightMachineButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }
        //public ICommand OpenCloseBoxButtonCommand { get; set; }
        public ICommand PrintLabelCommand { get; set; }

        #endregion

        #region Constructor
        public EditBoxViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor NewBoxViewModel....", category: Category.Info, priority: Priority.Low);
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

                WeightMachineButtonCommand = new DelegateCommand<object>(ShowWeightMachine);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptButtonCommand = new RelayCommand(new Action<object>(EditBox));
                SelectedIndexChangedCommand = new DelegateCommand<object>(SelectedIndexChangedCommandAction);
                // OpenCloseBoxButtonCommand = new RelayCommand(new Action<object>(OpenCloseBoxButtonCommandAction));
                PrintLabelCommand = new RelayCommand(new Action<object>(PrintBoxLabel));
                FillPackingBoxTypeList();
                FillOriginOfContent();

                IsWeightMachineButtonVisibility = Visibility.Visible;

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor NewBoxViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in NewBoxViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods
        private void ShowWeightMachine(object obj)
        {
            if (GrossWeight > 0)
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["WeightMachineWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                    GrossWeight = WarehouseCommon.Instance.ScaleWeight;
            }
            else
                GrossWeight = WarehouseCommon.Instance.ScaleWeight;
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void EditBox(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditBox()...", category: Category.Info, priority: Priority.Low);
                bool isCountryGroupflag = false;
                if (ExistPackingBox.BoxNumber != BoxNumber)
                {
                    List<PackingCompany> PackingCompanyList = new List<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2240(WarehouseCommon.Instance.Selectedwarehouse, CustomersList[SelectedCustomerIndex].IdCompany.ToString()));
                    IsBoxNumberExist = PackingCompanyList.FirstOrDefault().PackingBoxes.Any(x => x.BoxNumber == BoxNumber);
                }
                //[Sudhir.Jangra][GEOS2-5646]
                if (!(GrossWeight-NetWeight>=0))
                {
                    string errorMessage1 = string.Format(System.Windows.Application.Current.FindResource("GrossWeightClosedButtonErrorMessage1").ToString());
                    string errorMessage2 = string.Format(System.Windows.Application.Current.FindResource("GrossWeightClosedButtonErrorMessage2").ToString());
                    string errorMessage = errorMessage1 + "\n" + errorMessage2;
                    string popUpWarningColor = Application.Current.Resources["PopUpWarningColor"].ToString();
                    CustomMessageBox.Show(
                        errorMessage,
                        popUpWarningColor,
                        CustomMessageBox.MessageImagePath.NotOk,
                        MessageBoxButton.OK
                    );
                    return;
                }
                
                // shubham[skadam] GEOS2-3918 EURO and NO EURO ots have been mixed in a box  15 Sep 2022
                #region shubham[skadam] GEOS2-3918 EURO and NO EURO ots have been mixed in a box  15 Sep 2022
                try
                {
                    CountryGroup countryGroup = (OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup > 0 ? new CountryGroup { IdCountryGroup = OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup, Name = OriginOfContentList[SelectedOriginOfContentIndex].Name, HtmlColor = OriginOfContentList[SelectedOriginOfContentIndex].HtmlColor } : null);
                    if (countryGroup != null)
                    {
                        //isCountryGroupflag = WarehouseService.WMS_ISExistsEuropeanAndNonEuropeanCountry(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)ExistPackingBox).IdPackingBox);
                        List<CountryEURO> listOfCountryGroup = WarehouseService.WMS_ISExistsEuropeanAndNonEuropeanCountry_V2340(WarehouseCommon.Instance.Selectedwarehouse, ((PackingBox)ExistPackingBox).IdPackingBox);
                        List<string> Countrylist = new List<string>();
                        foreach (CountryEURO CountryEURO in listOfCountryGroup)
                        {
                            if (!Countrylist.Any(a => a == CountryEURO.ProducerCountryGroup))
                            {
                                Countrylist.Add(CountryEURO.ProducerCountryGroup);
                            }
                        }
                        if (Countrylist.Count > 1)
                        {
                            isCountryGroupflag = true;
                        }
                        if (isCountryGroupflag != true)
                            if (countryGroup != null)
                            {
                                if (!Countrylist.Any(a => a == countryGroup.Name))
                                {
                                    isCountryGroupflag = true;
                                }
                                else
                                {
                                    isCountryGroupflag = false;
                                }
                            }
                        if (isCountryGroupflag)
                        {
                            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["WMSCountryGroupErrorMessage"].ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, null);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("BoxNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPackingBoxTypeIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCustomerIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedOriginOfContentIndex"));
                //[Sudhir.Jangra][GEOS2-4542][09/08/2023]
                PropertyChanged(this, new PropertyChangedEventArgs("GrossWeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("NetWeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("Height"));
                PropertyChanged(this, new PropertyChangedEventArgs("Width"));
                PropertyChanged(this, new PropertyChangedEventArgs("Length"));
                if (error != null)
                {
                    return;
                }

                UpdatePackingBox = new PackingBox()
                {
                    IdPackingBox = ExistPackingBox.IdPackingBox,
                    BoxNumber = BoxNumber,
                    IdPackingBoxType = PackingBoxTypeList[SelectedPackingBoxTypeIndex].IdPackingBoxType,
                    Length = Length,
                    Width = Width,
                    Height = Height,
                    NetWeight = NetWeight,
                    SizeMeasurementUnit = "CM",
                    WeightMeasurementUnit = "KG",
                    GrossWeight = GrossWeight,
                    Comments = " ",
                    IdSite = CustomersList[SelectedCustomerIndex].IdCompany,
                    // IsClosed = ExistPackingBox.IsClosed,
                    IsClosed = 1,//[Sudhir.Jangra][GEOS2-4542][11/08/2023]
                    IdCountryGroup = OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup,
                    CountryGroup = (OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup > 0 ? new CountryGroup { IdCountryGroup = OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup, Name = OriginOfContentList[SelectedOriginOfContentIndex].Name, HtmlColor = OriginOfContentList[SelectedOriginOfContentIndex].HtmlColor } : null),
                    IsVisibleCountryGroup = (OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup > 0 ? Visibility.Visible : Visibility.Hidden),
                    IsStackable = IsStackable,
                    IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse
                };
                IsResult = WarehouseService.UpdatePackingBox_V2240(WarehouseCommon.Instance.Selectedwarehouse, UpdatePackingBox);
                IsSave = true;

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateBoxInformationAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method EditBox()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditBox() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditBox() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditBox() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SelectedIndexChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction...", category: Category.Info, priority: Priority.Low);

                PackingBoxType tempPackingBoxType = PackingBoxTypeList[SelectedPackingBoxTypeIndex];
                if (tempPackingBoxType.IdPackingBoxType != 6)
                {
                    Length = tempPackingBoxType.Length;
                    Width = tempPackingBoxType.Width;
                    Height = tempPackingBoxType.Height;
                    NetWeight = tempPackingBoxType.NetWeight;
                    IsCustomBox = true;
                }
                else
                {
                    if (ExistPackingBox.IdPackingBoxType == 6)
                    {
                        Length = ExistPackingBox.Length;
                        Width = ExistPackingBox.Width;
                        Height = ExistPackingBox.Height;
                        NetWeight = ExistPackingBox.NetWeight;
                    }
                    else
                    {
                        Length = tempPackingBoxType.Length;
                        Width = tempPackingBoxType.Width;
                        Height = tempPackingBoxType.Height;
                        NetWeight = tempPackingBoxType.NetWeight;
                    }
                    IsCustomBox = false;
                }

                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillPackingBoxTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPackingBoxTypeList...", category: Category.Info, priority: Priority.Low);
                PackingBoxTypeList = new List<PackingBoxType>(WarehouseService.GetPackingBoxType(WarehouseCommon.Instance.Selectedwarehouse));
                PackingBoxTypeList.Insert(0, new PackingBoxType() { IdPackingBoxType = 0, Code = "---" });
                PackingBoxType tempPackingBoxType = PackingBoxTypeList.FirstOrDefault(x => x.IdPackingBoxType == 6);
                PackingBoxTypeList.Remove(tempPackingBoxType);
                PackingBoxTypeList.Add(tempPackingBoxType);
                GeosApplication.Instance.Logger.Log("Method FillPackingBoxTypeList executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPackingBoxTypeList() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPackingBoxTypeList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillPackingBoxTypeList() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(PackingBox selectedItem, List<Company> companyList, long idWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);

                ExistPackingBox = selectedItem;
                BoxNumber = ExistPackingBox.BoxNumber;
                SelectedPackingBoxTypeIndex = PackingBoxTypeList.FindIndex(x => x.IdPackingBoxType == ExistPackingBox.IdPackingBoxType);
                Length = ExistPackingBox.Length;
                Width = ExistPackingBox.Width;
                Height = ExistPackingBox.Height;
                NetWeight = ExistPackingBox.NetWeight;
                GrossWeight = ExistPackingBox.GrossWeight;
                //IsClosed = ExistPackingBox.IsClosed;
                CustomersList = companyList.Where(x => x.ShortName != null).ToList();
                CustomersList = CustomersList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                CustomersList.Insert(0, new Company() { IdCompany = 0, ShortName = "---" });
                SelectedCustomerIndex = CustomersList.FindIndex(x => x.IdCompany == ExistPackingBox.IdSite);
                SelectedOriginOfContentIndex = OriginOfContentList.IndexOf(OriginOfContentList.FirstOrDefault(x => x.IdCountryGroup == ExistPackingBox.IdCountryGroup));
                IsPrintButtonVisibile = Visibility.Visible;
                IsStackable = selectedItem.IsStackable;
                IdWarehouse = idWarehouse;
                if (isWeightAndSizeValueNull = true)
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("BoxNumber"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedPackingBoxTypeIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCustomerIndex"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedOriginOfContentIndex"));
                    //[Sudhir.Jangra][GEOS2-4542][09/08/2023]
                    PropertyChanged(this, new PropertyChangedEventArgs("GrossWeight"));
                    PropertyChanged(this, new PropertyChangedEventArgs("NetWeight"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Height"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Width"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Length"));
                    if (error != null)
                    {
                        return;
                    }
                }
                IsGrossWeightVisible = Visibility.Visible;//[Sudhir.Jangra][GEOS2-5646]
                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //private void OpenCloseBoxButtonCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method OpenCloseBoxButtonCommandAction...", category: Category.Info, priority: Priority.Low);
        //        if (IsClosed == 0)
        //            IsClosed = 1;
        //        else if (IsClosed == 1)
        //            IsClosed = 0;
        //        GeosApplication.Instance.Logger.Log("Method OpenCloseBoxButtonCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in OpenCloseBoxButtonCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        private void PrintBoxLabel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintBoxLabel...", category: Category.Info, priority: Priority.Low);
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

                List<BoxPrint> tempPackingBoxList = WarehouseService.GetWorkorderByIdPackingBox_V2210(WarehouseCommon.Instance.Selectedwarehouse, ExistPackingBox.IdPackingBox);
                PrintValues = new Dictionary<string, string>();
                byte[] printFile = GeosRepositoryService.GetBoxLabelFile(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
                PrintLabel = new PrintLabel(PrintValues, printFile);
                CreatePrintValues(tempPackingBoxList);
                PrintLabel.Print();
                if (printFile != null)
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BoxLabelPrintSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method PrintBoxLabel executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintBoxLabel() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CreatePrintValues(List<BoxPrint> tempPackingBoxList)
        {
            try
            {
                string UserCode = WarehouseService.GetEmployeeCodeByIdUser(GeosApplication.Instance.ActiveUser.IdUser); //[GEOS2-4014][rdixit][15.12.2022]
                GeosApplication.Instance.Logger.Log("Method CreatePrintValues...", category: Category.Info, priority: Priority.Low);
                PrintValues.Add("@USER", UserCode);

                #region SiteName(As per GPM)
                string customerName = String.Format("{0} - {1}", tempPackingBoxList.FirstOrDefault().CustomerName, tempPackingBoxList.FirstOrDefault().SiteNameWithCountry);

                if (customerName.Length > 26)
                {
                    int index = 0;
                    string firstLine = GetFirstLine(customerName, ref index);
                    PrintValues.Add("@CUSTOMER00", firstLine);
                    PrintValues.Add("@CUSTOMER01", customerName.Substring(index));
                }
                else
                {
                    PrintValues.Add("@CUSTOMER00", customerName);
                    PrintValues.Add("@CUSTOMER01", "");
                }
                #endregion

                PrintValues.Add("@OT00", "");
                PrintValues.Add("@OT01", "");
                PrintValues.Add("@OT02", "");
                PrintValues.Add("@OT03", "");
                PrintValues.Add("@OT04", "");
                PrintValues.Add("@OT05", "");
                PrintValues.Add("@OT06", "");
                PrintValues.Add("@OT07", "");
                PrintValues.Add("@OT08", "");
                PrintValues.Add("@OT09", "");
                int id = 0;
                foreach (BoxPrint item in tempPackingBoxList)
                {
                    PrintValues["@OT0" + id] = item.OtCode;
                    id++;
                }

                PrintValues.Add("@BOX_NUMBER", tempPackingBoxList.FirstOrDefault().BoxNumber);
                PrintValues.Add("@BOX_ID", GetPackingBoxBarCode(tempPackingBoxList.FirstOrDefault().IdPackingBox.ToString()));
                PrintValues.Add("@WEIGHT", tempPackingBoxList.FirstOrDefault().GrossWeight.ToString() + "Kg");
                PrintValues.Add("@CARRIAGE_METHOD_CODE", tempPackingBoxList.FirstOrDefault().CarriageMethodAbbreviation);
                PrintValues.Add("@CARRIAGE_METHOD_NAME", tempPackingBoxList.FirstOrDefault().CarriageMethodValue);
                PrintValues.Add("@WAREHOUSE", WarehouseCommon.Instance.Selectedwarehouse.Name);
                GeosApplication.Instance.Logger.Log("Method CreatePrintValues executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CreatePrintValues() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetFirstLine(string customer, ref int i)
        {
            string temp = "";

            for (int j = 0; j < customer.Length; j++)
            {
                if (customer[j] == ' ')
                {
                    if (j > 26)
                    {
                        break;
                    }
                    else
                    {
                        i = j + 1;
                        temp = customer.Substring(0, j);
                    }
                }
            }
            return temp;
        }

        private string GetPackingBoxBarCode(string idPackingBox)
        {
            string barcode = "";
            barcode = "B" + idPackingBox.PadLeft(9, '0');
            return barcode;
        }

        public void FillOriginOfContent()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOriginOfContent...", category: Category.Info, priority: Priority.Low);
                OriginOfContentList = new ObservableCollection<OriginOfContent>(WarehouseService.GetOriginOfContentList(WarehouseCommon.Instance.Selectedwarehouse));
                OriginOfContentList.Insert(0, new OriginOfContent() { IdCountryGroup = 0, Name = "---" });
                GeosApplication.Instance.Logger.Log("Method FillOriginOfContent executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOriginOfContent() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOriginOfContent() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillOriginOfContent() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation

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
                    me[BindableBase.GetPropertyName(() => BoxNumber)] +
                    me[BindableBase.GetPropertyName(() => SelectedPackingBoxTypeIndex)] +
                     me[BindableBase.GetPropertyName(() => SelectedCustomerIndex)] +
                     me[BindableBase.GetPropertyName(() => Width)] +
                      me[BindableBase.GetPropertyName(() => Height)] +
                       me[BindableBase.GetPropertyName(() => Length)] +
                       me[BindableBase.GetPropertyName(() => GrossWeight)] +
                        me[BindableBase.GetPropertyName(() => NetWeight)];

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
                string boxNumber = BindableBase.GetPropertyName(() => BoxNumber);
                string selectedType = BindableBase.GetPropertyName(() => SelectedPackingBoxTypeIndex);
                string selectedCustomer = BindableBase.GetPropertyName(() => SelectedCustomerIndex);
                //[Sudhir.Jangra][GEOS2-4542][09/08/2023]
                string height = BindableBase.GetPropertyName(() => Height);
                string width = BindableBase.GetPropertyName(() => Width);
                string length = BindableBase.GetPropertyName(() => Length);
                string netWeight = BindableBase.GetPropertyName(() => NetWeight);
                string grossWeight = BindableBase.GetPropertyName(() => GrossWeight);

                if (columnName == boxNumber)
                {
                    string errorMessage = PackingBoxValidation.GetErrorMessage(boxNumber, BoxNumber);
                    if (errorMessage == string.Empty)
                        errorMessage = PackingBoxValidation.GetErrorMessage(boxNumber, IsBoxNumberExist);

                    return errorMessage;
                }

                if (columnName == selectedType)
                {
                    return PackingBoxValidation.GetErrorMessage(selectedType, SelectedPackingBoxTypeIndex);
                }

                if (columnName == selectedCustomer)
                {
                    return PackingBoxValidation.GetErrorMessage(selectedCustomer, SelectedCustomerIndex);
                }
                if (columnName == height)
                {
                    return PackingBoxValidation.GetErrorMessage(height, Height);
                }
                if (columnName == width)
                {
                    return PackingBoxValidation.GetErrorMessage(width, Width);
                }
                if (columnName == length)
                {
                    return PackingBoxValidation.GetErrorMessage(length, Length);
                }
                if (columnName == netWeight)
                {
                    return PackingBoxValidation.GetErrorMessage(netWeight, NetWeight);
                }
                if (columnName == grossWeight)
                {
                    return PackingBoxValidation.GetErrorMessage(grossWeight, GrossWeight);
                }

                return null;
            }
        }

        #endregion
    }
}
