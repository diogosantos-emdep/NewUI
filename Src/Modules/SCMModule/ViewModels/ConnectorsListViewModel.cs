using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Interface;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Window = System.Windows.Window;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[GEOS2-9552][rdixit][19.09.2025]
    public class ConnectorsListViewModel : ViewModelBase, INotifyPropertyChanged, ITabViewModel, IConnectorViewModel, IDisposable
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());    
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        //ICrmService CRMService = new CrmServiceController("localhost:6699");
        #endregion

        #region public Events              
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public event EventHandler RequestClose;
        #endregion // Events

        #region Methods


        public void Dispose()
        {
        }

        #endregion // Methods

        #region Declarations  
        ObservableCollection<ITabViewModel> _Tabs = new ObservableCollection<ITabViewModel>();
        object parentViewModel;
        string tabName;
        private string _selectedValue;
        ObservableCollection<Connectors> connectorList;
        Connectors selectedConnector;
        ObservableCollection<SearchConnector> connectorSearchList= new ObservableCollection<SearchConnector>();
        //[nsatpute][21-05-2025][GEOS2-7996]
        bool allowPaging;
        int resultPages;
        double cardImageSize;
        #endregion

        #region Properties  
        public double CardImageSize
        {
            get { return cardImageSize; }
            set
            {
                cardImageSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CardImageSize"));
            }
        }
        public ObservableCollection<Connectors> ConnectorList
        {
            get
            {
                return connectorList;
            }

            set
            {
                connectorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorList"));
            }
        }
        public ObservableCollection<SearchConnector> ConnectorSearchList
        {
            get
            {
                return connectorSearchList;
            }

            set
            {
                connectorSearchList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectorSearchList"));
            }
        }    
        public ObservableCollection<ITabViewModel> Tabs
        {
            get
            {
                return _Tabs;
            }
            set
            {
                _Tabs = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tabs"));
            }
        }
        public Connectors SelectedConnector
        {
            get
            {
                return selectedConnector;
            }

            set
            {
                selectedConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConnector"));
            }
        }
        public bool AllowPaging
        {
            get
            {
                return allowPaging;
            }

            set
            {
                allowPaging = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllowPaging"));
            }
        }
        public int ResultPages
        {
            get
            {
                return resultPages;
            }

            set
            {
                resultPages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultPages"));
            }
        }  
        public virtual object ParentViewModel
        {
            get
            {
                return parentViewModel;
            }
            set
            {
                parentViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentViewModel"));
            }
        }       
        public virtual string TabName
        {
            get
            {
                return tabName;
            }
            set
            {
                tabName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TabName"));
            }
        }
        public virtual object TabContent { get; protected set; }
        #endregion

        #region Public Icommand
        public ICommand EditConnectorReferenceCommand { get; set; }
        public ICommand ImageClickCommand { get; set; }       
        #endregion

        #region Constructor
        public ConnectorsListViewModel()
        {
            try
            {
                EditConnectorReferenceCommand = new DelegateCommand<object>(EditConnectorReferenceCommandAction);
                ImageClickCommand = new DelegateCommand<object>(ImageClickAction);
                //[nsatpute][21-05-2025][GEOS2-7996]
                if (GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                    AllowPaging = Convert.ToBoolean(GeosApplication.Instance.UserSettings["AllowPaging"]);
                //[nsatpute][21-05-2025][GEOS2-7996]
                if (GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                    ResultPages = Convert.ToInt32(GeosApplication.Instance.UserSettings["ResultPages"]);
                else
                    ResultPages = 10;
                //[rdixit][14.04.2025][GEOS2-6631] //[GEOS2-8036][13.05.2025][rdixit]
                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ImageSize"))
                {
                    CardImageSize = Convert.ToDouble(GeosApplication.Instance.UserSettings["ImageSize"]);
                }
                else
                    CardImageSize = 200;
                LoadImageAsync();
                //[rdixit][14.10.2025][GEOS2-8895]
                if (GeosApplication.ImageUrlBytePair == null)
                    GeosApplication.ImageUrlBytePair = new Dictionary<string, byte[]>();
                GeosApplication.Instance.Logger.Log(string.Format("Method ConnectorViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ConnectorViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods     
        //[rdixit][14.10.2025][GEOS2-8895]
        public async void LoadImageAsync()
        {
            try
            { 
                if (ConnectorSearchList != null)
                {
                    foreach (var Connector in ConnectorSearchList)
                    {
                        if (Connector != null)
                        {
                            foreach (var con in Connector.ConnectorList)
                            {
                                byte[] bytes = await Task.Run(() =>
                                {
                                    if (!GeosApplication.IsImageURLException)
                                        return Utility.ImageUtil.GetImageByWebClient(con.ConnectorsImagePath);
                                    else
                                    {
                                        var service = new GeosRepositoryServiceController(
                                            GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                                        return service.GetImagesByUrl(con.ConnectorsImagePath);
                                    }
                                });

                                if (bytes != null)
                                {
                                    GeosApplication.ImageUrlBytePair[con.ConnectorsImagePath] = bytes;
                                    con.ConnectorsImageInBytes = bytes; // ✅ UI updates automatically
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                GeosApplication.isServiceDown = true;
            }
        }
        private void ImageClickAction(object sender)
        {
            try
            {
                Connectors connectors = new Connectors();
                if (sender != null)
                {
                    GridControl GridControl = (GridControl)sender;
                    if (GridControl.Columns?.Count == 1)
                    {
                        if (GridControl.CurrentItem != null)
                            connectors = (Connectors)GridControl.CurrentItem;
                    }
                    else
                    {
                        if (GridControl.CurrentColumn != null)
                        {
                            if (GridControl.CurrentColumn.FieldName.ToLower().Equals("ConnectorsImageInBytes".ToLower()))
                                connectors = (Connectors)GridControl.CurrentItem;
                        }
                    }
                    if (connectors != null)
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        Connectors SelectedConnectors = new Connectors();
                        SelectedConnectors.Ref = connectors.Ref;
                        SelectedConnectors.Description = connectors.Description;
                        SelectedConnectors.ConnectorsImageInBytes = connectors.ConnectorsImageInBytes;
                        ConnectorGridImageView connectorGridImageView = new ConnectorGridImageView();
                        ConnectorGridImageViewModel connectorGridImageViewModel = new ConnectorGridImageViewModel();
                        EventHandler handle = delegate { connectorGridImageView.Close(); };
                        connectorGridImageViewModel.RequestClose += handle;
                        connectorGridImageViewModel.Init(SelectedConnectors);
                        connectorGridImageView.DataContext = connectorGridImageViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        connectorGridImageView.ShowDialogWindow();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ImageClickAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        private void EditConnectorReferenceCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditConnectorReferenceCommandAction....", category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
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
                    if (obj != null)
                    {
                        if (obj is TableView)
                        {
                            TableView detailView = (TableView)obj;
                            SelectedConnector = (Connectors)detailView.DataControl.CurrentItem;
                        }
                        else if (obj is CardView)
                        {
                            CardView detailView = (CardView)obj;
                            SelectedConnector = (Connectors)detailView.DataControl.CurrentItem;
                        }
                        if (SCMCommon.Instance.Tabs.Any(i => i.TabName == SelectedConnector.Ref && i is ConnectorDetailViewMode))
                        {
                            SCMCommon.Instance.TabIndex = SCMCommon.Instance.Tabs.IndexOf(SCMCommon.Instance.Tabs.First(i => i.TabName == SelectedConnector.Ref && i is ConnectorDetailViewMode));
                        }
                        else
                        {
                            var vm = ViewModelSource.Create<ConnectorDetailViewMode>();
                            vm.Init(SelectedConnector);
                            SCMCommon.Instance.Tabs.Add(vm);
                            SCMCommon.Instance.IsPinned = true;
                            SCMCommon.Instance.Tabs = new ObservableCollection<ITabViewModel>(SCMCommon.Instance.Tabs);
                            SCMCommon.Instance.TabIndex = SCMCommon.Instance.Tabs.Count > 0 ? SCMCommon.Instance.Tabs.Count - 1 : 0;
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method EditConnectorReferenceCommandAction....Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method EditConnectorReferenceCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
