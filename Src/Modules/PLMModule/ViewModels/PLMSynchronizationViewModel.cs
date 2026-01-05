using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.PLM.CommonClasses;
using Emdep.Geos.Modules.PLM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.SynchronizationClass;

namespace Emdep.Geos.Modules.PLM.ViewModels
{
    public class PLMSynchronizationViewModel
    {

        #region Service
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion


        #region Declaration
        private string name;
        private ITokenService tokenService;
        List<GeosAppSetting> geosAppSettingList;
        private string details;
        private bool isTextEditVisible;
        private string nullTextString;
        private Visibility isVisible;
        private List<BPLPlantCurrencyDetail> bplPlantCurrencyDetailList;
        private string pastekey;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));

                if (Name == "Product Prices")
                {
                    IsVisible = Visibility.Visible;
                    IsTextEditVisible = true;
                    NullTextString = @"The references must be separated by a comma (,). " + Environment.NewLine + "In case you want to synchronize ALL do not specify any reference.";
                }
                else
                {
                    IsVisible = Visibility.Collapsed;
                    IsTextEditVisible = false;
                    NullTextString = string.Empty;
                }
            }
        }

        public string Details
        {
            get { return details; }
            set
            {
                if(value!=null)
                {
                    value= value.Replace("Reference\r\n", "");
                    value=value.Replace("\r\n", ",");
                  
                }
                details = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Details"));
            }
        }


        public string NullTextString
        {
            get { return nullTextString; }
            set
            {
                nullTextString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("nullTextString"));
            }
        }

        public List<GeosAppSetting> GeosAppSettingList
        {
            get
            {
                return geosAppSettingList;
            }

            set
            {
                geosAppSettingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettingList"));
            }
        }

        public bool IsTextEditVisible
        {
            get { return isTextEditVisible; }
            set
            {
                isTextEditVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsTextEditVisible"));
            }
        }
        public Visibility IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
            }
        }

        public List<BPLPlantCurrencyDetail> BPLPlantCurrencyDetailList
        {
            get { return bplPlantCurrencyDetailList; }
            set
            {
                bplPlantCurrencyDetailList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BPLPlantCurrencyDetailList"));
            }
        }
        #endregion


        #region Command
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand EditCommand { get; set; }
        
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
        #endregion


        #region Contructor
        public PLMSynchronizationViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SynchronizationViewModel..."), category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                FilterCommand = new DelegateCommand<KeyEventArgs>(FilterCommandAction);
                EditCommand = new DelegateCommand<object>(EditCommandAction);
                GeosApplication.Instance.Logger.Log(string.Format("Method SynchronizationViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SynchronizationViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion



        #region Methods

        public void Init(string name)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init..."), category: Category.Info, priority: Priority.Low);
                Name = name;
                GeosAppSettingList = WorkbenchService.GetSelectedGeosAppSettings("59,60,61");
            
                Details = string.Empty;
          
                    GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private async void AcceptButtonCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction..."), category: Category.Info, priority: Priority.Low);
            var ownerInfo = (obj as FrameworkElement);
            try
            {
                MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["ECOSSynchronizationWarningMessage"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo, Window.GetWindow(ownerInfo));
                if (MessageBoxResult == MessageBoxResult.Yes)
                {


                    if (!DXSplashScreen.IsActive)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            Window win = new Window()
                            {
                                Focusable = true,
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = false,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = false,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, new object[] { new SplashScreenOwner(ownerInfo), WindowStartupLocation.CenterOwner }, null);
                    }
                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is running";

                    APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
                    APIErrorDetail values = new APIErrorDetail();
                    List<ErrorDetails> LstErrorDetail = new List<ErrorDetails>();
                    if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
                    {

                        string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
                        if (tokeninformations.Count() >= 2)
                        {                            
                                if (Name.ToUpper() == "PRODUCT PRICES")
                                {
                                    if (string.IsNullOrEmpty(Details.Trim()))
                                    {
                                        BPLPlantCurrencyDetailList = PLMService.GetBPLPlantCurrencyDetail("0", "Article");

                                    }
                                    else
                                    {

                                        BPLPlantCurrencyDetailList = PLMService.GetBPLPlantCurrencyDetail(Details.Trim(), "Article");
                                    }

                                    if (GeosAppSettingList.Any(i => i.IdAppSetting == 61) && BPLPlantCurrencyDetailList != null)
                                    {
                                        foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                        {
                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                            List<ErrorDetails> TempLstErrorDetail = await PLMService.IsPLMSynchronization(GeosAppSettingList, itemBPLPlantCurrency, Details, Name);
                                            foreach (ErrorDetails item in TempLstErrorDetail)
                                            {
                                                if (item != null && item.Error == string.Empty)
                                                {
                                                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is Done for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                }
                                                else
                                                {
                                                    if (item != null && item.Error != null)
                                                    {
                                                        GeosApplication.Instance.SplashScreenMessage = "Synchronization is failed for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                        ErrorDetails errorDetails = new ErrorDetails();
                                                        errorDetails.CompanyName = itemBPLPlantCurrency.CompanyName;
                                                        errorDetails.CurrencyName = itemBPLPlantCurrency.CurrencyName;
                                                        errorDetails.Error = item.Error;
                                                        LstErrorDetail.Add(errorDetails);
                                                    }
                                                }
                                            }
                                        }
                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                        if (LstErrorDetail != null && LstErrorDetail.Count > 0)
                                        {
                                            String FinalMessage = null;
                                            string msg = string.Empty;
                                            foreach (ErrorDetails item in LstErrorDetail)
                                            {

                                                if (string.IsNullOrEmpty(FinalMessage))
                                                    FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error), Window.GetWindow(ownerInfo));
                                                else
                                                    FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error);

                                            }
                                            if (!string.IsNullOrEmpty(FinalMessage))
                                            {
                                                CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                            }
                                        }
                                        else
                                        {
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                            RequestClose(null, null);
                                        }


                                    }
                                }
                                else if (Name.ToUpper() == "DETECTION PRICES")
                                {
                                    BPLPlantCurrencyDetailList = PLMService.GetBPLPlantCurrencyDetail("0", "Detection");

                                    if (GeosAppSettingList.Any(i => i.IdAppSetting == 60) && BPLPlantCurrencyDetailList != null)
                                    {
                                        foreach (BPLPlantCurrencyDetail itemBPLPlantCurrency in BPLPlantCurrencyDetailList)
                                        {
                                            GeosApplication.Instance.SplashScreenMessage = "Synchronization is running for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                            List<ErrorDetails> TempLstErrorDetail = await PLMService.IsPLMSynchronization(GeosAppSettingList, itemBPLPlantCurrency, Details, Name);
                                            foreach (ErrorDetails item in TempLstErrorDetail)
                                            {
                                                if (item != null && item.Error == string.Empty)
                                                {
                                                    GeosApplication.Instance.SplashScreenMessage = "Synchronization is Done for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                }
                                                else
                                                {
                                                    if (item != null && item.Error != null)
                                                    {
                                                        GeosApplication.Instance.SplashScreenMessage = "Synchronization is failed for plant " + itemBPLPlantCurrency.CompanyName + " and currency " + itemBPLPlantCurrency.CurrencyName + "";
                                                        ErrorDetails errorDetails = new ErrorDetails();
                                                        errorDetails.CompanyName = itemBPLPlantCurrency.CompanyName;
                                                        errorDetails.CurrencyName = itemBPLPlantCurrency.CurrencyName;
                                                        errorDetails.Error = item.Error;
                                                        LstErrorDetail.Add(errorDetails);
                                                    }
                                                }
                                            }
                                        }
                                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                                        if (LstErrorDetail != null && LstErrorDetail.Count > 0)
                                        {
                                            String FinalMessage = null;
                                            string msg = string.Empty;
                                            foreach (ErrorDetails item in LstErrorDetail)
                                            {

                                                if (string.IsNullOrEmpty(FinalMessage))
                                                    FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error), Window.GetWindow(ownerInfo));
                                                else
                                                    FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PLMSynchronizationFailedCompanyCurrency").ToString(), item.CompanyName, item.CurrencyName, item.Error);

                                            }
                                            if (!string.IsNullOrEmpty(FinalMessage))
                                            {
                                                CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                            }
                                        }
                                        else
                                        {
                                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                            RequestClose(null, null);
                                        }
                                    }
                                }
                                else
                                {

                                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                                    if (values != null && values.Message != null)
                                    {
                                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), values.Message._Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                        Details = string.Empty;
                                    }                                  
                                }                      
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.SplashScreenMessage = "Synchronization failed";
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationFailed").ToString(), ex.Message), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                Details = string.Empty;
                //  GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        private void EditCommandAction(object obj)
        {
            //try
            //{
            //    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            //    {

            //        var clipboardData = Clipboard.GetText();
            //        if (clipboardData != null)
            //        {
            //            string[] rows = clipboardData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //            if (rows.Length > 0)
            //            {

            //                if (rows[0].ToUpper() == "REFERENCE")
            //                {

            //                    string numToRemove = rows[0];
            //                    rows = rows.Where(val => val != numToRemove).ToArray();
            //                    string[] FinalArray = rows;
            //                    string search = string.Join(",", FinalArray.Select(a => a.ToString()));

            //                    if (!string.IsNullOrEmpty(search))
            //                        Details = search.Trim();
            //                }
            //                //else
            //                //{
            //                //    string[] FinalArray = rows;
            //                //    string search = string.Join(",", FinalArray.Select(a => a.ToString()));

            //                //    if (!string.IsNullOrEmpty(search))
            //                //        Details = search.Trim();
            //                //}

            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }
        private void FilterCommandAction(KeyEventArgs obj)
        {
            try
            {
                if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {

                    var clipboardData = Clipboard.GetText();
                    if (clipboardData != null)
                    {
                        string[] rows = clipboardData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (rows.Length > 0)
                        {

                            if (rows[0].ToUpper() == "REFERENCE")
                            {

                                string numToRemove = rows[0];
                                rows = rows.Where(val => val != numToRemove).ToArray();
                                string[] FinalArray = rows;
                                string search = string.Join(",", FinalArray.Select(a => a.ToString()));

                                if (!string.IsNullOrEmpty(search))
                                    Details = search.Trim();
                            }
                            //else
                            //{
                            //    string[] FinalArray = rows;
                            //    string search = string.Join(",", FinalArray.Select(a => a.ToString()));

                            //    if (!string.IsNullOrEmpty(search))
                            //        Details = search.Trim();
                            //}

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
