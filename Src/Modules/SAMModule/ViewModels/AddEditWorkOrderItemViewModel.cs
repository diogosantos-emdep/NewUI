using System;
using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.SAM;
using System.Windows.Input;
using System.ComponentModel;
using DevExpress.Xpf.Core;
using DevExpress.Mvvm.UI;
using Emdep.Geos.UI.Common;
using System.Windows;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using System.Windows.Media;
using DevExpress.Xpf.LayoutControl;
using System.Collections.ObjectModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common;
using System.ServiceModel;
using Emdep.Geos.UI.Helper;
using System.Linq;
using Emdep.Geos.Modules.SAM.Views;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class AddEditWorkOrderItemViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public event EventHandler RequestClose;
        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        public ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // End Services Region

        #region Declaration
        private double dialogHeight;
        private double dialogWidth;
        private OTs ot;
        private MaximizedElementPosition maximizedElementPosition;
        private ObservableCollection<OTAttachment> listAttachment;
        #endregion // End Of Declaration

        #region Properties
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }

        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
      
        public OTs OT
        {
            get { return ot; }
            set
            {
                ot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }

        public ObservableCollection<OTAttachment> ListAttachment
        {
            get { return listAttachment; }

            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
            }
        }
        #endregion //End Of Properties

        #region Icommands

        public ICommand CustomSummaryCommand { get; set; }
        public ICommand AddEditWorkOrderItemViewCancel { get; set; }

        #endregion //End Of Icommand

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Constructor

        public AddEditWorkOrderItemViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel....", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                MaximizedElementPosition = MaximizedElementPosition.Right;

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
                AddEditWorkOrderItemViewCancel = new DelegateCommand<object>(AddEditWorkOrderItemViewCancelAction);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WorkOrderViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in WorkOrderViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion //End Of Constructor
        public void Init(OTs foundRow)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                //OtSite = ot.Site;
                //Emdep.Geos.Data.Common.Company Company = SAMCommon.Instance.PlantOwnerList[0];
                //SAMService = new SAMServiceController("localhost:6699");
                //OT = SAMService.GetSAMOrderItemsInformationByIdOt_V2340(foundRow.IdOT, foundRow.Site);
                FillListAttachment(foundRow.Site, foundRow.IdOT);

                EditItemViewModel workOrderViewModel = new EditItemViewModel();
                EditItemView workOrderView = new EditItemView();

                EventHandler handle1 = delegate { workOrderView.Close(); };
                workOrderViewModel.RequestClose += handle1;
                workOrderViewModel.Init();
                workOrderView.DataContext = workOrderViewModel;
                workOrderView.ShowDialogWindow();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }


                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEditWorkOrderItemViewModel Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddEditWorkOrderItemViewCancelAction(object obj)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddEditWorkOrderItemViewCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillListAttachment(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListAttachment ...", category: Category.Info, priority: Priority.Low);

                ListAttachment = new ObservableCollection<OTAttachment>(SAMService.GetOTAttachment(company, idOT).ToList());

                foreach (OTAttachment items in ListAttachment)
                {
                    ImageSource imageObj = FileExtensionToFileIcon.FindIconForFilename(items.FileName, true);
                    items.AttachmentImage = imageObj;
                }

                GeosApplication.Instance.Logger.Log("Method FillListAttachment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
        }
    }
}