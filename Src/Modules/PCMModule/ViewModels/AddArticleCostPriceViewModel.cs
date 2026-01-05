using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.PCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class AddArticleCostPriceViewModel : ViewModelBase, INotifyPropertyChanged
    {

        #region Service
          IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
      
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


        #region Declaration
        private string windowHeader;
        private ObservableCollection<Articles> allArticlesList;
        private bool isNew;
        private ObservableCollection<Company> allPlantsList;
        private List<object> allSelectedPlantsList;
        private ObservableCollection<Articles> articlesListForMainGrid;
        private ObservableCollection<Articles> existingArticlesList;
        private ObservableCollection<Articles> selectedAllArticlesListForMainGrid;
        
        private bool isSave;
        private Visibility isVisible;
        #endregion


        #region Properties
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

        public ObservableCollection<Articles> AllArticlesList
        {
            get { return allArticlesList; }
            set
            {
                allArticlesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllArticlesList"));
            }
        }

        public ObservableCollection<Articles> SelectedAllArticlesListForMainGrid
        {
            get { return selectedAllArticlesListForMainGrid; }
            set
            {
                selectedAllArticlesListForMainGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAllArticlesListForMainGrid"));
            }
        }

        public ObservableCollection<Company> AllPlantsList
        {
            get { return allPlantsList; }
            set
            {
                allPlantsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPlantsList"));
            }


        }


        public List<object> AllSelectedPlantsList
        {
            get { return allSelectedPlantsList; }
            set
            {
                allSelectedPlantsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllSelectedPlantsList"));
            }
        }

        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

        public ObservableCollection<Articles> ArticlesListForMainGrid
        {
            get
            {
                return articlesListForMainGrid;
            }

            set
            {
                articlesListForMainGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesListForMainGrid"));
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

        #endregion

        #region Icommands
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand AddArticleCostPriceAcceptButtonCommand { get; set; }

        public ICommand PlantOwnerPopupClosedCommand { get; private set; }
        #endregion

        #region Constructor
        public AddArticleCostPriceViewModel()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Constructor AddArticleCostPriceViewModel ...", category: Category.Info, priority: Priority.Low);

                AddArticleCostPriceAcceptButtonCommand = new DelegateCommand<object>(AddArticleCostPriceAcceptButtonCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                //PlantOwnerPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(PlantOwnerPopupClosedCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor AddArticleCostPriceViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddArticleCostPriceViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        public void Init(List<Articles> articleList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillAllPlantsList();
                ArticlesListForMainGrid = new ObservableCollection<Articles>(articleList);
                ArticlesListForMainGrid.ToList().ForEach(x => { x.IsChecked = true; });
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //private void PlantOwnerPopupClosedCommandAction(object obj)
        //{
        //    GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction ...", category: Category.Info, priority: Priority.Low);

        //    if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
        //    {
        //        return;
        //    }
        //    if (!DXSplashScreen.IsActive)
        //    {
        //        // DXSplashScreen.Show<SplashScreenView>(); 
        //        DXSplashScreen.Show(x =>
        //        {
        //            Window win = new Window()
        //            {
        //                ShowActivated = false,
        //                WindowStyle = WindowStyle.None,
        //                ResizeMode = ResizeMode.NoResize,
        //                AllowsTransparency = true,
        //                Background = new SolidColorBrush(Colors.Transparent),
        //                ShowInTaskbar = false,
        //                Topmost = true,
        //                SizeToContent = SizeToContent.WidthAndHeight,
        //                WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //            };
        //            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //            win.Topmost = false;
        //            return win;
        //        }, x =>
        //        {
        //            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //        }, null, null);
        //    }


        //    AllSelectedPlantsList.ToList();
        //    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

        //    GeosApplication.Instance.Logger.Log("Method PlantOwnerPopupClosedCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        //}
        /// [001][cpatil][GEOS2-5299][26-02-2024]
        private void FillAllPlantsList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillAllPlantsList()...", category: Category.Info, priority: Priority.Low);
                // [001] Changed Service method
                //Service GetEmdepSitesCompanies_V2490 updated with GetEmdepSitesCompanies_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                AllPlantsList = new ObservableCollection<Company>(PCMService.GetEmdepSitesCompanies_V2500());
                AllSelectedPlantsList = new List<object>();
                AllSelectedPlantsList.AddRange(AllPlantsList.Cast<Company>().ToList());
              
               
                GeosApplication.Instance.Logger.Log("Method FillAllPlantsList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllPlantsList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillAllPlantsList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                //CustomMessageBox.Show(string.Format("Could not find file '{0}'.", employeeEducationQualification.QualificationFileName), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillAllPlantsList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// CloseWindow Method is used for Cancel Button for Both Add and Edit Trainee
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                // IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddArticleCostPriceAcceptButtonCommandAction(object obj)
        {
            try
            {
                var ownerInfo = (obj as FrameworkElement);
                List<ErrorDetails> errorDetailsLst = new List<ErrorDetails>();
                List<ErrorDetails> errorDetailsForNotSavedLst = new List<ErrorDetails>();
                GeosApplication.Instance.Logger.Log("Method AddArticleCostPriceAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (ArticlesListForMainGrid.Where(i=>i.IsChecked==true).ToList().Count > 0)
                {
                    if(AllSelectedPlantsList.Count>0)
                    {
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
                        foreach (Company itemPlantOwnerUsers in AllSelectedPlantsList)
                        {
                            try
                            {
                                GeosApplication.Instance.SplashScreenMessage = "Connecting to " + itemPlantOwnerUsers.ShortName;
                                List<PODetail> EWHQArticlesByArticleComponentpoDetailLst = PCMService.GetAllArticlesByArticleComponentMaxPOFromEWHQ(itemPlantOwnerUsers.ConnectPlantConstr);
                                List<ArticlesByArticle> LstAllArticlesByArticle = PCMService.GetAllArticlesByArticle(itemPlantOwnerUsers.ConnectPlantConstr);
                                List<PODetail> ArticlesByArticleComponentpoDetailLst = PCMService.GetAllArticlesByArticleComponentMaxPOFromPlant(itemPlantOwnerUsers.ConnectPlantConstr);
                                foreach (Articles itemArticle in ArticlesListForMainGrid.Where(i => i.IsChecked == true).ToList())
                                {
                                    try
                                    {
                                        bool isupdateArticleCostPrice = PCMService.ImportArticleCostPriceCalculate(itemPlantOwnerUsers, Convert.ToUInt64(itemArticle.IdArticle), EWHQArticlesByArticleComponentpoDetailLst, LstAllArticlesByArticle, ArticlesByArticleComponentpoDetailLst);
                                        if(!isupdateArticleCostPrice)
                                        {
                                            ErrorDetails errorDetails = new ErrorDetails();
                                            errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                            errorDetails.ArticleReference = itemArticle.Reference;
                                            errorDetailsForNotSavedLst.Add(errorDetails);
                                        }
                                    }
                                    catch (FaultException<ServiceException> ex)
                                    {
                                        ErrorDetails errorDetails = new ErrorDetails();
                                        errorDetails.Error = ex.Message;
                                        errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                        errorDetails.ArticleReference = itemArticle.Reference;
                                        errorDetailsLst.Add(errorDetails);
                                       
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");

                                        System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (ServiceUnexceptedException ex)
                                    {
                                        ErrorDetails errorDetails = new ErrorDetails();
                                        errorDetails.Error = ex.Message;
                                        errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                        errorDetails.ArticleReference = itemArticle.Reference;
                                        errorDetailsLst.Add(errorDetails);
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");

                                        System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorDetails errorDetails = new ErrorDetails();
                                        errorDetails.Error = ex.Message;
                                        errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                        errorDetails.ArticleReference = itemArticle.Reference;
                                        errorDetailsLst.Add(errorDetails);
                                        GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");

                                        System.Threading.Thread.Sleep(1000);
                                        GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    }

                                }
                                
                            }
                            catch (FaultException<ServiceException> ex)
                            {
                                ErrorDetails errorDetails = new ErrorDetails();
                                errorDetails.Error = ex.Message;
                                errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                errorDetailsLst.Add(errorDetails);
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                              
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (ServiceUnexceptedException ex)
                            {
                                ErrorDetails errorDetails = new ErrorDetails();
                                errorDetails.Error = ex.Message;
                                errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                errorDetailsLst.Add(errorDetails);
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                               
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                            catch (Exception ex)
                            {
                                ErrorDetails errorDetails = new ErrorDetails();
                                errorDetails.Error = ex.Message;
                                errorDetails.CompanyName = itemPlantOwnerUsers.ShortName;
                                errorDetailsLst.Add(errorDetails);
                                GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed");
                              
                                System.Threading.Thread.Sleep(1000);
                                GeosApplication.Instance.Logger.Log(string.Concat("Connecting to ", itemPlantOwnerUsers.ShortName, " Failed ", ex.Message), category: Category.Exception, priority: Priority.Low);
                            }
                        }
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                        if (errorDetailsLst.Count > 0 || errorDetailsForNotSavedLst.Count>0)
                        {
                            String FinalMessage = null;
                            string msg = string.Empty;
                            foreach (ErrorDetails item in errorDetailsLst)
                            {
                                if (!string.IsNullOrEmpty(item.ArticleReference))
                                {
                                    if (string.IsNullOrEmpty(FinalMessage))
                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceHasNotBeenSavedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSaved").ToString(), item.ArticleReference, item.CompanyName, item.Error));
                                    else
                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSaved").ToString(), item.ArticleReference, item.CompanyName, item.Error);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(FinalMessage))
                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceHasNotBeenSavedForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSaved").ToString(), item.CompanyName, item.Error));
                                    else
                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSaved").ToString(), item.CompanyName, item.Error);
                                }
                            }
                            foreach (ErrorDetails item in errorDetailsForNotSavedLst)
                            {
                                if (!string.IsNullOrEmpty(item.ArticleReference))
                                {
                                    if (string.IsNullOrEmpty(FinalMessage))
                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PCMNoArticleCostPriceForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSavedNoError").ToString(), item.ArticleReference, item.CompanyName));
                                    else
                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSavedNoError").ToString(), item.ArticleReference, item.CompanyName);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(FinalMessage))
                                        FinalMessage = string.Format(System.Windows.Application.Current.FindResource("PCMNoArticleCostPriceForFollowingReasons").ToString() + System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSavedNoError").ToString(), item.CompanyName));
                                    else
                                        FinalMessage += System.Environment.NewLine + string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleCompanyNotSavedNoError").ToString(), item.CompanyName);
                                }
                            }
                            
                            if (!string.IsNullOrEmpty(FinalMessage))
                            {
                                CustomMessageBox.Show(FinalMessage, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                            }
                        }
                        
                        else
                        {
                            CustomMessageBox.Show(string.Format(Application.Current.FindResource("PCMArticleCostPriceSavedSuccessfully").ToString()),"Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                            IsSave = true;
                        }
                       
                        RequestClose(null, null);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPricePlantValidationMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                    }
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PCMArticleCostPriceArticleValidationMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK, Window.GetWindow(ownerInfo));
                }
                
                GeosApplication.Instance.Logger.Log("Method AddArticleCostPriceAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddArticleCostPriceAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
