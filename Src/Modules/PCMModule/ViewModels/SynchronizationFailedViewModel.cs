using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Newtonsoft.Json;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.Data.Common.SynchronizationClass;
namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class SynchronizationFailedViewModel : INotifyPropertyChanged
    {


        #region Service

        #endregion


        #region Declaration
        private string reference;
        private string details;
        private string errorMessage;
        private List<GeosAppSetting> geosAppSettingList;
        private string name;
        private ITokenService tokenService;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
            }
        }

        public string Details
        {
            get { return details; }
            set
            {
                details = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Details"));
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ErrorMessage"));
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
        #endregion


        #region Command
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CopyTextCommand { get; set; }
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
        public SynchronizationFailedViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method SynchronizationFailedViewModel..."), category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                //AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                CopyTextCommand = new DelegateCommand<object>(CopyTextCommandAction);
                GeosApplication.Instance.Logger.Log(string.Format("Method SynchronizationFailedViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method SynchronizationFailedViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion



        #region Methods

        public void Init(string errorMessage, string references, string name, List<GeosAppSetting> GeosAppSettingListPar)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init..."), category: Category.Info, priority: Priority.Low);
                ErrorMessage = errorMessage;
                GeosAppSettingList = GeosAppSettingListPar;
                // Reference = references;
                Name = name;
                Details = references;
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

        public void CopyTextCommandAction(object obj)
        {
            try
            {
                TextEdit detailView = (TextEdit)obj;
                detailView.SelectAll();
                detailView.Copy();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CopyTextCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //private async void AcceptButtonCommandAction(object obj)
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonCommandAction..."), category: Category.Info, priority: Priority.Low);
        //        GeosApplication.Instance.SplashScreenMessage = "The Synchronization is running";
        //        if (!DXSplashScreen.IsActive)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        APIErrorDetailForErrorFalse valuesErrorFalse = new APIErrorDetailForErrorFalse();
        //        APIErrorDetail values = new APIErrorDetail();
        //        tokenService = new AuthTokenService();
        //        if (GeosAppSettingList.Any(i => i.IdAppSetting == 59))
        //        {
        //            string[] tokeninformations = GeosAppSettingList.Where(i => i.IdAppSetting == 59).FirstOrDefault().DefaultValue.Split(';');
        //            if (tokeninformations.Count() >= 2)
        //            {

        //                var token = new AuthToken();
        //                var client = new HttpClient();
        //                var client_id = tokeninformations[1];
        //                var client_secret = tokeninformations[2];
        //                var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
        //                client.DefaultRequestHeaders.Authorization =
        //                    new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

        //                var postMessage = new Dictionary<string, string>();
        //                postMessage.Add("grant_type", "client_credentials");

        //                var request = new HttpRequestMessage(HttpMethod.Post, tokeninformations[0])
        //                {
        //                    Content = new FormUrlEncodedContent(postMessage)
        //                };

        //                var response = await client.SendAsync(request);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var json = await response.Content.ReadAsStringAsync();
        //                    token = JsonConvert.DeserializeObject<AuthToken>(json);
        //                    token.ExpiresAt = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
        //                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        //                    if (Name.ToUpper() == "CATEGORIES")
        //                    {
        //                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 56))
        //                        {
        //                            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //                            //  var res = await client.PostAsync(string.Format("{0}", GeosAppSettingList.Where(i => i.IdAppSetting == 56).FirstOrDefault().DefaultValue), httpContent);
        //                            using (HttpResponseMessage responseProduct = await client.PostAsync(string.Format("{0}", GeosAppSettingList.Where(i => i.IdAppSetting == 56).FirstOrDefault().DefaultValue, Details.Trim()), httpContent))
        //                            {
        //                                responseProduct.EnsureSuccessStatusCode();
        //                                string responseBody = await responseProduct.Content.ReadAsStringAsync();
        //                                if (responseBody.Contains("false"))
        //                                    valuesErrorFalse = JsonConvert.DeserializeObject<APIErrorDetailForErrorFalse>(responseBody);
        //                                else if (responseBody.Contains("true"))
        //                                    values = JsonConvert.DeserializeObject<APIErrorDetail>(responseBody);

        //                            }
        //                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //                            if (valuesErrorFalse.Error == "false")
        //                            {
        //                                var ownerInfo = (obj as FrameworkElement);
        //                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                                      
        //                            }
        //                            else
        //                            {
        //                                if (values.Message != null)
        //                                {
        //                                    ErrorMessage = values.Message._Message;
        //                                }
        //                                //values = new List<APIErrorDetail> { new APIErrorDetail { Code = res.StatusCode.ToString(), Error = res.ReasonPhrase } };
        //                            }
        //                        }
        //                    }
        //                    else if (Name.ToUpper() == "ARTICLES")
        //                    {
        //                        if (!string.IsNullOrEmpty(Details.Trim()))
        //                        {
        //                            if (GeosAppSettingList.Any(i => i.IdAppSetting == 57))
        //                            {
        //                                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        //                                // var res = await client.PostAsync(string.Format("{0}?references={1}", GeosAppSettingList.Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue, Details.Trim()), httpContent);
        //                                using (HttpResponseMessage responseProduct = await client.PostAsync(string.Format("{0}?references={1}", GeosAppSettingList.Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue, Details.Trim()), httpContent))
        //                                {
        //                                    responseProduct.EnsureSuccessStatusCode();
        //                                    string responseBody = await responseProduct.Content.ReadAsStringAsync();
        //                                    if (responseBody.Contains("false"))
        //                                        valuesErrorFalse = JsonConvert.DeserializeObject<APIErrorDetailForErrorFalse>(responseBody);
        //                                    else if (responseBody.Contains("true"))
        //                                        values = JsonConvert.DeserializeObject<APIErrorDetail>(responseBody);

        //                                }
        //                                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //                                if (valuesErrorFalse.Error == "false")
        //                                {
        //                                    var ownerInfo = (obj as FrameworkElement);
        //                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
        //                                    //var jsonreference = res.Content.ReadAsStringAsync().Result;
        //                                    //values = JsonConvert.DeserializeObject<APIErrorDetail>(jsonreference);
        //                                }
        //                                else
        //                                {
        //                                    if (values.Message != null)
        //                                    {
        //                                        ErrorMessage = values.Message._Message;
        //                                    }
        //                                    //values = new List<APIErrorDetail> { new APIErrorDetail { Code = res.StatusCode.ToString(), Error = res.ReasonPhrase } };
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (Name.ToUpper() == "DETECTIONS")
        //                    {
        //                        if (GeosAppSettingList.Any(i => i.IdAppSetting == 58))
        //                        {
        //                            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //                            var res = await client.PostAsync(string.Format("{0}", GeosAppSettingList.Where(i => i.IdAppSetting == 57).FirstOrDefault().DefaultValue), httpContent);
        //                            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //                            if (res.IsSuccessStatusCode)
        //                            {
        //                                var ownerInfo = (obj as FrameworkElement);
        //                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ECOSSynchronizationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
        //                                //var jsonreference = res.Content.ReadAsStringAsync().Result;
        //                                //values = JsonConvert.DeserializeObject<APIErrorDetail>(jsonreference);
        //                            }
        //                            else
        //                            {
        //                                if (values.Message != null)
        //                                {
        //                                    ErrorMessage = values.Message._Message;
        //                                }
        //                                //values = new List<APIErrorDetail> { new APIErrorDetail { Code = res.StatusCode.ToString(), Error = res.ReasonPhrase } };
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    ErrorMessage = response.ReasonPhrase;
        //                    //throw new ApplicationException("Unable to retrieve access token from ECOS");
        //                }
        //            }

        //        }


        //        RequestClose(null, null);

        //        GeosApplication.Instance.Logger.Log(string.Format("Method Init()....executed AcceptButtonCommandAction"), category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.SplashScreenMessage = "The Synchronization failed";
        //        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        //        ErrorMessage = ex.Message; 
        //        //GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
        //    }
        //    GeosApplication.Instance.SplashScreenMessage = string.Empty;
        //}
        #endregion
    }
}
