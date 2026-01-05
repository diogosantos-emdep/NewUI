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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class NewBoxViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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
        private string packingBoxLengthErrorMessage=string.Empty;
        private string windowHeader;
        private Visibility isOpenCloseButtonVisibile;
        private bool isCustomBox;
        private List<Company> customersList;
        private int selectedCustomerIndex;
        #endregion

        #region Properties
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
                netWeight = value;
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
                grossWeight = value;
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
        #endregion

        #region ICommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand SelectedIndexChangedCommand { get; set; }

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

                CancelButtonCommand= new DelegateCommand<object>(CloseWindow);
                AcceptButtonCommand = new RelayCommand(new Action<object>(AddBox));
                SelectedIndexChangedCommand = new DelegateCommand<object>(SelectedIndexChangedCommandAction);
                FillPackingBoxTypeList();
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
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void AddBox(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddBox()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("BoxNumber"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPackingBoxTypeIndex"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedCustomerIndex"));

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
                    SizeMeasurementUnit ="CM",
                    WeightMeasurementUnit ="KG",
                    GrossWeight = GrossWeight,
                    Comments =" ",
                    IdSite = CustomersList[SelectedCustomerIndex].IdCompany,
                };
                NewPackingBox = WarehouseService.AddPackingBox(WarehouseCommon.Instance.Selectedwarehouse, NewPackingBox);
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

        private void FillPackingBoxTypeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillPackingBoxTypeList...", category: Category.Info, priority: Priority.Low);
                PackingBoxTypeList = new List<PackingBoxType>(WarehouseService.GetPackingBoxType(WarehouseCommon.Instance.Selectedwarehouse));
                PackingBoxTypeList.Insert(0,new PackingBoxType() { IdPackingBoxType =0,Code ="---"});
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

        public void Init(List<Company> companyList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init...", category: Category.Info, priority: Priority.Low);
                CustomersList= companyList.Where(x => x.ShortName != null).ToList();
                CustomersList = CustomersList.GroupBy(customer => customer.IdCompany).Select(group => group.First()).ToList();
                CustomersList.Insert(0, new Company() { IdCompany = 0, ShortName = "---" });
                GeosApplication.Instance.Logger.Log("Method Init executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                     me[BindableBase.GetPropertyName(() => SelectedCustomerIndex)];

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
                string selectedCustomer= BindableBase.GetPropertyName(() => SelectedCustomerIndex);

                if (columnName == boxNumber)
                {
                    return PackingBoxValidation.GetErrorMessage(boxNumber, BoxNumber);
                }

                if (columnName == selectedType)
                {
                    return PackingBoxValidation.GetErrorMessage(selectedType, SelectedPackingBoxTypeIndex);
                }

                if (columnName == selectedCustomer)
                {
                    return PackingBoxValidation.GetErrorMessage(selectedCustomer, SelectedCustomerIndex);
                }

                return null;
            }
        }

      
        #endregion
    }
}
