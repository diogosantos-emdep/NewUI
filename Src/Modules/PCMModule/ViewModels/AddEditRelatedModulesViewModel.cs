using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
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
    //[Sudhir.Jangra][GEOS2-4460][26/06/2023]
    public class AddEditRelatedModulesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services
          IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // IPCMService PCMService = new PCMServiceController("localhost:6699");

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
        ObservableCollection<ProductTypes> relatedModulesListForMainGrid;
        ObservableCollection<ProductTypes> allRelatedModulesList;
        bool isNew;
        string windowHeader;
        bool isSave;
        string informationError;
        private string error = string.Empty;
        #endregion

        #region Properties
        public ObservableCollection<ProductTypes> RelatedModulesListForMainGrid
        {
            get
            {
                return relatedModulesListForMainGrid;
            }
            set
            {
                relatedModulesListForMainGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RelatedModulesListForMainGrid"));
            }
        }

        public ObservableCollection<ProductTypes> AllRelatedModulesList
        {
            get { return allRelatedModulesList; }
            set
            {
                allRelatedModulesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllRelatedModulesList"));
            }
        }

        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
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

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        #endregion

        #region ICommands
        public ICommand AddEditRelatedModulesCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditRelatedModulesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditRelatedModulesViewModel ...", category: Category.Info, priority: Priority.Low);
                AddEditRelatedModulesCommand = new DelegateCommand<object>(AddEditRelatedModulesCommandAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                GeosApplication.Instance.Logger.Log("Constructor AddEditRelatedModulesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditRelatedModulesViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
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

                AllRelatedModulesList = new ObservableCollection<ProductTypes>(PCMService.GetProductTypesByDetection_V2410());

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void AddEditRelatedModulesCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditRelatedModulesAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                if (AllRelatedModulesList.Any(x => x.IsChecked && x.Status.Value == "Inactive"))
                {
                    InformationError = null;
                    allowValidation = true;
                    error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("Status.Value"));
                    PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
                    //  PropertyChanged(this, new PropertyChangedEventArgs(""));

                    if (error!=null)
                    {
                        return;
                    }
                }
                else
                {
                    InformationError = " ";
                }

                RelatedModulesListForMainGrid = new ObservableCollection<ProductTypes>();
                foreach (ProductTypes temp in AllRelatedModulesList.Where(x => x.IsChecked).ToList())
                {
                    RelatedModulesListForMainGrid.Add(temp);
                }
                IsSave = true;
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddEditRelatedModulesAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddEditRelatedModulesAcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
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


       

        #endregion
    }
}
