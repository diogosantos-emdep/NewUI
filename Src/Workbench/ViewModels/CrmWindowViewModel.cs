using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
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

namespace Workbench.ViewModels
{
    public class CrmWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Declaration

        private string showHideMenuButtonToolTip;
        private string moduleName;

        private string shortcutKey;
        private string visible;
        private string moduleShortName;
        #endregion

        #region Properties
        public string ShowHideMenuButtonToolTip
        {
            get { return showHideMenuButtonToolTip; }
            set
            {
                showHideMenuButtonToolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        public string ModuleName
        {
            get
            {
                return moduleName;
            }

            set
            {
                moduleName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        public string ShortcutKey
        {
            get
            {
                return shortcutKey;
            }

            set
            {
                shortcutKey = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShortcutKey"));
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Visible"));
            }
        }

        public string ModuleShortName
        {
            get
            {
                return moduleShortName;
            }

            set
            {
                moduleShortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleShortName"));
            }
        }
        #endregion

        #region Public Commands

        public ICommand HideTileBarButtonClickCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public ICommand Opportunity_ClickCommand { get; set; }





        #endregion  // Public Commands

        #region Constructor

        public CrmWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            Opportunity_ClickCommand = new DelegateCommand<object>(Opportunity_Click);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu

            if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
            {
                ModuleShortName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 5).Select(s => s.Acronym).FirstOrDefault();
                ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 5).Select(s => s.Name).FirstOrDefault();


            }
            else
            {
                ModuleShortName = "CRM";
                ModuleName = "Customer Relationship Management";
            }
            //get shortcut values from user setting
            CRMCommon.Instance.GetShortcuts();

            //set hide/show shortcuts on permissions
            Visible = Visibility.Visible.ToString();
            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                Visible = Visibility.Hidden.ToString();
            }
            else
            {
                Visible = Visibility.Visible.ToString();
            }
        }

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

        #endregion // Events

        #region Methods

        private void HideTileBarButtonClickCommandAction(RoutedEventArgs obj)
        {
            if (GeosApplication.Instance.TileBarVisibility == Visibility.Collapsed)
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Visible;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString(); //Hide menu
            }
            else
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Collapsed;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("ShowMenuButtonToolTip").ToString(); // ShowMenu
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Opportunity_Click(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method Opportunity_Click ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                string Selected = ((System.Windows.UIElement)((DevExpress.Xpf.Accordion.AccordionControl)obj).SelectedItem).Uid;

                // shortcuts
                // Get shortcut for Opportunity
                if (Selected == "Opportunity")
                {
                    Processing();
                    LeadAddViewModel leadAddViewModel = new LeadAddViewModel();
                    LeadsAddView leadsAddView = new LeadsAddView();
                    EventHandler handle = delegate { leadsAddView.Close(); };
                    leadAddViewModel.RequestClose += handle;
                    leadsAddView.DataContext = leadAddViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //IsTimelineColumnChooserVisible = false;
                    leadsAddView.ShowDialogWindow();
                }

                // Get shortcut for  Contact

                if (Selected == "Contact")
                {
                    Processing();
                    AddContactViewModel addContactViewModel = new AddContactViewModel();
                    AddContactView addContactView = new AddContactView();
                    EventHandler handle = delegate { addContactView.Close(); };
                    addContactViewModel.RequestClose += handle;
                    addContactView.DataContext = addContactViewModel;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    //IsContactColumnChooserVisible = false;
                    addContactView.ShowDialogWindow();
                }
                // Get shortcut for  Account

                if (Selected == "Account")
                {
                    Processing();
                    AddCustomerViewModel addCustomerViewModel = new AddCustomerViewModel();
                    AddCustomerView addCustomerView = new AddCustomerView();
                    EventHandler handle = delegate { addCustomerView.Close(); };
                    addCustomerViewModel.RequestClose += handle;
                    addCustomerView.DataContext = addCustomerViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Close();
                    }
                    //IsAccountColumnChooserVisible = false;
                    addCustomerView.ShowDialog();
                }

                // Get shortcut for  Appointment

                if (Selected == "Appointment")
                {
                    Processing();
                    AddActivityView addActivityView = new AddActivityView();
                    AddActivityViewModel addActivityViewModel = new AddActivityViewModel();

                    int index = addActivityViewModel.TypeList.IndexOf(addActivityViewModel.TypeList.Where(a => a.IdLookupValue == 37).FirstOrDefault());

                    addActivityViewModel.SelectedIndexType = index;
                    addActivityViewModel.IsInternalEnable = true;
                    EventHandler handle = delegate { addActivityView.Close(); };
                    addActivityViewModel.RequestClose += handle;
                    addActivityView.DataContext = addActivityViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    addActivityView.ShowDialog();
                }
                // Get shortcut for  Call

                if (Selected == "Call")
                {
                    Processing();
                    AddActivityView addActivityView = new AddActivityView();
                    AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                    int index = addActivityViewModel.TypeList.IndexOf(addActivityViewModel.TypeList.Where(a => a.IdLookupValue == 38).FirstOrDefault());
                    addActivityViewModel.SelectedIndexType = index;
                    addActivityViewModel.IsInternalEnable = true;
                    EventHandler handle = delegate { addActivityView.Close(); };
                    addActivityViewModel.RequestClose += handle;
                    addActivityView.DataContext = addActivityViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    addActivityView.ShowDialog();
                }
                // Get shortcut for  Task

                if (Selected == "Task")
                {
                    Processing();
                    AddActivityView addActivityView = new AddActivityView();
                    AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                    int index = addActivityViewModel.TypeList.IndexOf(addActivityViewModel.TypeList.Where(a => a.IdLookupValue == 40).FirstOrDefault());
                    addActivityViewModel.SelectedIndexType = index;
                    addActivityViewModel.IsInternalEnable = true;
                    EventHandler handle = delegate { addActivityView.Close(); };
                    addActivityViewModel.RequestClose += handle;
                    addActivityView.DataContext = addActivityViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    addActivityView.ShowDialog();
                }
                // Get shortcut for  Email

                if (Selected == "Email")
                {
                    Processing();
                    AddActivityView addActivityView = new AddActivityView();
                    AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                    int index = addActivityViewModel.TypeList.IndexOf(addActivityViewModel.TypeList.Where(a => a.IdLookupValue == 39).FirstOrDefault());
                    addActivityViewModel.SelectedIndexType = index;
                    addActivityViewModel.IsInternalEnable = true;
                    EventHandler handle = delegate { addActivityView.Close(); };
                    addActivityViewModel.RequestClose += handle;
                    addActivityView.DataContext = addActivityViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    addActivityView.ShowDialog();
                }
                // Get shortcut for  Action

                if (Selected == "Action")
                {
                    Processing();
                    AddNewActionsViewModel addNewActionsViewModel = new AddNewActionsViewModel();
                    AddNewActionsView addNewActionsView = new AddNewActionsView();
                    EventHandler handle = delegate { addNewActionsView.Close(); };
                    addNewActionsViewModel.RequestClose += handle;
                    addNewActionsView.DataContext = addNewActionsViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    addNewActionsView.ShowDialog();
                }
                // Get shortcut for  SearchOpportunityOrOrder

                if (Selected == "SearchOpportunityOrOrder")
                {
                    Processing();
                    SearchOpportunityOrOfferViewModel searchOpportunityOrOfferViewModel = new SearchOpportunityOrOfferViewModel();
                    SearchOpportunityOrOfferView searchOpportunityOrOfferView = new SearchOpportunityOrOfferView();
                    EventHandler handle = delegate { searchOpportunityOrOfferView.Close(); };
                    searchOpportunityOrOfferViewModel.RequestClose += handle;
                    searchOpportunityOrOfferView.DataContext = searchOpportunityOrOfferViewModel;

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    searchOpportunityOrOfferView.ShowDialog();

                }
                if (Selected == "MatrixList")
                {
                    CRMCommon.Instance.OpenMatrixListView();
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method Opportunity_Click....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Opportunity_Click...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }        
        private void Processing()
        {
            if (!DXSplashScreen.IsActive)
            {
                //DXSplashScreen.Show<SplashScreenView>(); 
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
        }
        #endregion // Methods
    }
}
