using DevExpress.DataProcessing;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
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
    public class NewBoxViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // End Services Region

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
        private string packingBoxLengthErrorMessage = string.Empty;
        private string windowHeader;
        private Visibility isOpenCloseButtonVisibile;
        private bool isCustomBox;
        private List<Company> customersList;
        private int selectedCustomerIndex;

        private ObservableCollection<OriginOfContent> originOfContentList;
        private int selectedoriginOfContentIndex;
        private Visibility isPrintButtonVisibile;
        private long? idWarehouse;
        private sbyte isStackable;
        private bool isBoxNumberExist;
        private Visibility isWeightMachineButtonVisibility;
        public bool isNew { get; set; }//[Sudhir.Jangra][GEOS2-5646]
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
        public PackingBox NewPackingBox { get; set; }
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

                if (length == 0)
                    packingBoxLengthErrorMessage = System.Windows.Application.Current.FindResource("PackingBoxLengthErrorMessage").ToString();
                else
                    packingBoxLengthErrorMessage = string.Empty;

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

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }
        public RelayCommand MyProperty { get; set; }

        #endregion

        #region Constructor
        public NewBoxViewModel()
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

                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AddBox));
                SelectedIndexChangedCommand = new DelegateCommand<object>(SelectedIndexChangedCommandAction);
                MyProperty = new RelayCommand((object obj) => new RelayCommand(ExecuteSave));
                FillPackingBoxTypeList();
                FillOriginOfContent();

                IsWeightMachineButtonVisibility = Visibility.Collapsed;

               

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor NewBoxViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in NewBoxViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        void ExecuteSave(object obj)
        {

        }

        #endregion

        #region Methods    
        //[cpatil][001][16-03-2022][GEOS2-3652]
        //[cpatil][002][26-04-2022][GEOS2-3722]
        private void AddBox(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddBox()...", category: Category.Info, priority: Priority.Low);

                List<PackingCompany> PackingCompanyList = new List<PackingCompany>(WarehouseService.GetCompanyPackingWorkOrders_V2240(WarehouseCommon.Instance.Selectedwarehouse, CustomersList[SelectedCustomerIndex].IdCompany.ToString()));
                if (PackingCompanyList != null && PackingCompanyList.Count > 0)
                    IsBoxNumberExist = PackingCompanyList.FirstOrDefault().PackingBoxes.Any(x => x.BoxNumber == BoxNumber);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("BoxNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPackingBoxTypeIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCustomerIndex"));
                //[Sudhir.Jangra][GEOS2-4542][09/08/2023]
                if (!isNew)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GrossWeight"));
                }
              
                PropertyChanged(this, new PropertyChangedEventArgs("NetWeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("Height"));
                PropertyChanged(this, new PropertyChangedEventArgs("Width"));
                PropertyChanged(this, new PropertyChangedEventArgs("Length"));

                if (error != null)
                {
                    return;
                }

                NewPackingBox = new PackingBox()
                {
                    BoxNumber = BoxNumber,
                    IdPackingBoxType = PackingBoxTypeList[SelectedPackingBoxTypeIndex].IdPackingBoxType,
                    Length = Length,
                    Width = Width,
                    Height = Height,
                    NetWeight = NetWeight,
                    SizeMeasurementUnit = "CM",
                    WeightMeasurementUnit = "KG",
                    GrossWeight = GrossWeight,
                    IsStackable = IsStackable,
                    Comments = " ",
                    IdSite = CustomersList[SelectedCustomerIndex].IdCompany,
                    IsClosed = 1,//[Sudhir.Jangra][GEOS2-4542][11/08/2023]
                    IdCountryGroup = OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup,
                    CountryGroup = (OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup > 0 ? new CountryGroup { IdCountryGroup = OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup, Name = OriginOfContentList[SelectedOriginOfContentIndex].Name, HtmlColor = OriginOfContentList[SelectedOriginOfContentIndex].HtmlColor } : null),
                    IsVisibleCountryGroup = (OriginOfContentList[SelectedOriginOfContentIndex].IdCountryGroup > 0 ? Visibility.Visible : Visibility.Hidden),
                    IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse
                };
                //[002]
                NewPackingBox = WarehouseService.AddPackingBox_V2260(WarehouseCommon.Instance.Selectedwarehouse, NewPackingBox);
                IsSave = true;
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("NewBoxInformationAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddBox()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddBox() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddBox() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddBox() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
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
        private void SelectedIndexChangedCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction...", category: Category.Info, priority: Priority.Low);
                PackingBoxType tempPackingBoxType = PackingBoxTypeList[SelectedPackingBoxTypeIndex];
                Length = tempPackingBoxType.Length;
                Width = tempPackingBoxType.Width;
                Height = tempPackingBoxType.Height;
                NetWeight = tempPackingBoxType.NetWeight;

                if (tempPackingBoxType.IdPackingBoxType == 6)
                    IsCustomBox = false;
                else
                    IsCustomBox = true;
                GeosApplication.Instance.Logger.Log("Method SelectedIndexChangedCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexChangedCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(List<Company> companyList, long idWarehouse)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                CustomersList = companyList.Where(x => x.ShortName != null).ToList();
                CustomersList = CustomersList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                CustomersList.Insert(0, new Company() { IdCompany = 0, ShortName = "---" });
                IsPrintButtonVisibile = Visibility.Hidden;
                IdWarehouse = idWarehouse;

                if (isNew)//[Sudhir.Jangra][GEOS2-5646]
                {
                    IsGrossWeightVisible = Visibility.Hidden;
                }
                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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

        /// <summary>
        ///  method to Check Box Weight Tolerance
        /// </summary>

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
                    //   me[BindableBase.GetPropertyName(() => GrossWeight)] +
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
               // string grossWeight = BindableBase.GetPropertyName(() => GrossWeight);

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
                //if (columnName == grossWeight)
                //{
                //    return PackingBoxValidation.GetErrorMessage(grossWeight, GrossWeight);
                //}

                return null;
            }
        }
        #endregion
    }
}
