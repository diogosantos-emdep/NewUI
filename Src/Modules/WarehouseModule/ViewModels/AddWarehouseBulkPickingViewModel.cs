using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Modules.Warehouse.Views;
using Emdep.Geos.Services.Contracts;
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
    public class AddWarehouseBulkPickingViewModel : NavigationViewModelBase, IDisposable, IDataErrorInfo, INotifyPropertyChanged
    {


        #region Service
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IWarehouseService WarehouseService = new WarehouseServiceController("localhost:6699");
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

        #region Declaration
        private string windowHeader;
        private ObservableCollection<BulkPicking> articleList;
        private ObservableCollection<BulkPicking> parentArticleList;
        private BulkPicking selectedArticleList;
        private BulkPicking selectedParentArticleList;
        private bool isNew;
        private bool isSave;
        private WarehouseBulkArticle newBulkPickingList;
        private WarehouseBulkArticle updateBulkPickingList;
        public long IdWarehouseBulkPicking { get; set; }

        private string error = string.Empty;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public ObservableCollection<BulkPicking> ArticleList
        {
            get { return articleList; }
            set
            {
                articleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleList"));
            }
        }
        public ObservableCollection<BulkPicking> ParentArticleList
        {
            get { return parentArticleList; }
            set
            {
                parentArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentArticleList"));
            }
        }
        public BulkPicking SelectedArticleList
        {
            get { return selectedArticleList; }
            set
            {
                selectedArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedArticleList"));
            }
        }
        public BulkPicking SelectedParentArticleList
        {
            get { return selectedParentArticleList; }
            set
            {
                selectedParentArticleList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParentArticleList"));
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        public WarehouseBulkArticle NewBulkPickingList
        {
            get { return newBulkPickingList; }
            set
            {
                newBulkPickingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewBulkPickingList"));
            }
        }
        public WarehouseBulkArticle UpdateBulkPickingList
        {
            get { return updateBulkPickingList; }
            set
            {
                updateBulkPickingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateBulkPickingList"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddWarehouseBulkPickingViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddWarehouseBulkPickingViewModel ...", category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonCommandAction);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor AddWarehouseBulkPickingViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddWarehouseBulkPickingViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                //allowValidation = true;

                //if (SelectedArticleList.Reference.ToString().Equals("---"))
                //{
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedArticleList"));
                    if (error != null)
                    {
                        return;
                    }
               // }
                if (IsNew)
                {
                    
                    NewBulkPickingList = new WarehouseBulkArticle();
                    NewBulkPickingList.TransactionOperation = ModelBase.TransactionOperations.Add;
                    NewBulkPickingList.IdArticle = SelectedArticleList.IdArticle;
                    NewBulkPickingList.IdParent = SelectedParentArticleList.IdArticle;
                    NewBulkPickingList.ParentArticle = new Article();
                    NewBulkPickingList.ParentArticle.Reference = SelectedParentArticleList.Reference;
                    NewBulkPickingList.Article = new Article();
                    NewBulkPickingList.Article.Reference = SelectedArticleList.Reference;
                    NewBulkPickingList.IdWarehouse=WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    NewBulkPickingList.IdCreator= Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    try
                    {
                        NewBulkPickingList = WarehouseService.AddUpdateWarehouseBulkArticle(NewBulkPickingList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BulkPickingDetailsAddedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                        IsSave = true;
                    }
                    catch (Exception ex)
                    {
                        if (NewBulkPickingList.ParentArticle.Reference=="---")
                        {
                            CustomMessageBox.Show("Error Message:" + " " + NewBulkPickingList.Article.Reference + "  " + "this reference already exist.", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

                        }
                        else
                        {
                            CustomMessageBox.Show("Error Message:" + " " + NewBulkPickingList.Article.Reference + "  "+ "with parent reference"+ " " + NewBulkPickingList.ParentArticle.Reference+" "+ "already exist.", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }

                   
                }
                else
                {
                    UpdateBulkPickingList = new WarehouseBulkArticle();
                    UpdateBulkPickingList.TransactionOperation = ModelBase.TransactionOperations.Update;
                    UpdateBulkPickingList.IdWarehouseBulkArticle = IdWarehouseBulkPicking;
                    UpdateBulkPickingList.IdArticle = SelectedArticleList.IdArticle;
                    UpdateBulkPickingList.IdParent = SelectedParentArticleList.IdArticle;
                    UpdateBulkPickingList.ParentArticle = new Article();
                    UpdateBulkPickingList.ParentArticle.Reference = SelectedParentArticleList.Reference;
                    UpdateBulkPickingList.Article = new Article();
                    UpdateBulkPickingList.Article.Reference = SelectedArticleList.Reference;
                    UpdateBulkPickingList.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                    UpdateBulkPickingList.IdCreator = Convert.ToInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    try
                    {
                        UpdateBulkPickingList = WarehouseService.AddUpdateWarehouseBulkArticle(UpdateBulkPickingList);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("BulkPickingDetailsUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        IsSave = true;
                    }
                    catch (Exception ex)
                    {
                        if (UpdateBulkPickingList.ParentArticle.Reference == "---")
                        {
                            CustomMessageBox.Show("Error Message:" + " " + NewBulkPickingList.Article.Reference + "  " + "this reference already exist.", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                        else
                        {
                            CustomMessageBox.Show("Error Message:" + " " + NewBulkPickingList.Article.Reference + "  " + "with parent reference" + " " + NewBulkPickingList.ParentArticle.Reference + " " + "already exist.", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        }
                    }
                   
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void EditINIT(WarehouseBulkArticle editData)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ADDINIT()...", category: Category.Info, priority: Priority.Low);
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

                //[sudhir.Jangra][GEOS2-4414][08/09/2023]
                if (GeosApplication.Instance.IsEditBulkPickingPermissionWMS == true)
                {
                    //[Sudhir.Jangra][GEOS2-4414][08/09/2023]
                    if (GeosApplication.Instance.ArticleBulkPickingList == null || GeosApplication.Instance.ArticleBulkPickingList.Count() == 0)
                    {
                        List<BulkPicking> lstBulkPicking = WarehouseService.GetAllArticles();
                        lstBulkPicking.Insert(0, new BulkPicking() { Reference = "---" });

                        GeosApplication.Instance.ArticleBulkPickingList = lstBulkPicking;
                    }

                }
               
                var temp = new ObservableCollection<BulkPicking>(GeosApplication.Instance.ArticleBulkPickingList);
                 ArticleList = temp;
                ParentArticleList = temp;

               // WarehouseBulkArticle tempEditData = WarehouseService.GetAllArticlesByIdBulkPicking(IdWarehouseBulkPickkingArticle);

                IdWarehouseBulkPicking = editData.IdWarehouseBulkArticle ;
                SelectedArticleList = ArticleList.FirstOrDefault(x=>x.IdArticle== editData.IdArticle);
                SelectedParentArticleList = ParentArticleList.FirstOrDefault(x=>x.IdArticle== editData.IdParent);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ADDInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ADDINIT()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ADDINIT()...", category: Category.Info, priority: Priority.Low);
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

                //[sudhir.Jangra][GEOS2-4414][08/09/2023]
                if (GeosApplication.Instance.IsEditBulkPickingPermissionWMS == true )
                {
                    //[Sudhir.Jangra][GEOS2-4414][08/09/2023]
                    if(GeosApplication.Instance.ArticleBulkPickingList==null || GeosApplication.Instance.ArticleBulkPickingList.Count()==0)
                    {
                        List<BulkPicking> lstBulkPicking = WarehouseService.GetAllArticles();
                        lstBulkPicking.Insert(0, new BulkPicking() { Reference = "---" });

                        GeosApplication.Instance.ArticleBulkPickingList = lstBulkPicking;
                    }
                   
                }
                var temp =new ObservableCollection<BulkPicking>(GeosApplication.Instance.ArticleBulkPickingList);

                ArticleList = temp;
                ParentArticleList = temp;
              
                 //   ArticleList.Add(new Article() { IdArticle = 0, Reference = "---" });
                 //   ParentArticleList.Add(new Article() { IdArticle = 0, Reference = "---" });
                 SelectedArticleList = ArticleList.FirstOrDefault(x=>x.IdArticle==0);
                SelectedParentArticleList = ParentArticleList.FirstOrDefault(x => x.IdArticle == 0);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ADDInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {

        }
        #endregion

        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                              
                                me[BindableBase.GetPropertyName(() => SelectedArticleList)];

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


                string selectedTemplateError = BindableBase.GetPropertyName(() => SelectedArticleList);
              
                if (columnName == selectedTemplateError)
                {
                    return AddEditBulkPickingValidation.GetErrorMessage(selectedTemplateError, SelectedArticleList.Reference);
                }
              
                return null;
            }
        }
       

        #endregion

    }
}
