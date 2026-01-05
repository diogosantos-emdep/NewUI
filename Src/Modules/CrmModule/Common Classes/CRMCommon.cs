using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm
{
    public sealed class CRMCommon : Prism.Mvvm.BindableBase
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declarations

        private string opportunity;
        private string contact;
        private string account;
        private string appointment;
        private string call;
        private string task;
        private string email;
        private string action;
        private string searchOpportunityOrOrder;
        private ActionPlan actionPlan;
        private List<UserSiteGeosServiceProvider> commonAllPlantList;
        private string matrixList;
        public bool IsTimelineOpen;
        #endregion

        #region Properties

        public string Opportunity
        {
            get
            {
                return opportunity;
            }

            set
            {
                opportunity = value;
                this.OnPropertyChanged("Opportunity");
            }
        }

        public string Contact
        {
            get
            {
                return contact;
            }

            set
            {
                contact = value;
                this.OnPropertyChanged("Contact");
            }
        }

        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
                this.OnPropertyChanged("Account");
            }
        }

        public string Appointment
        {
            get
            {
                return appointment;
            }

            set
            {
                appointment = value;
                this.OnPropertyChanged("Appointment");
            }
        }

        public string Call
        {
            get
            {
                return call;
            }

            set
            {
                call = value;
                this.OnPropertyChanged("Call");
            }
        }

        public string Task
        {
            get
            {
                return task;
            }

            set
            {
                task = value;
                this.OnPropertyChanged("Task");
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
                this.OnPropertyChanged("Email");
            }
        }
        public string Action
        {
            get
            {
                return action;
            }

            set
            {
                action = value;
                this.OnPropertyChanged("Action");
            }
        }
        public string SearchOpportunityOrOrder
        {
            get
            {
                return searchOpportunityOrOrder;
            }

            set
            {
                searchOpportunityOrOrder = value;
                this.OnPropertyChanged("SearchOpportunityOrOrder");
            }
        }

        public string MatrixList
        {
            get
            {
                return matrixList;

            }
            set
            {
                matrixList = value;
                this.OnPropertyChanged("MatrixList");
            }
        }
        public static CRMCommon Instance
        {
            get { return instance; }
        }

        public ActionPlan ActionPlan
        {
            get
            {
                return actionPlan;
            }

            set
            {
                actionPlan = value;
            }
        }

        public List<UserSiteGeosServiceProvider> CommonAllPlantList
        {
            get
            {
                return commonAllPlantList;
            }

            set
            {
                commonAllPlantList = value;
                OnPropertyChanged("CommonAllPlantList");
            }
        }



        #endregion //Properties

        #region Singleton object

        //Singleton object
        private static readonly CRMCommon instance = new CRMCommon();

        #endregion //Singleton object

        #region Constructor

        private CRMCommon()
        {
        }

        #endregion //Constructor

        #region Methods
        public void GetShortcuts()
        {
            if (GeosApplication.Instance.UserSettings != null)
            {
                // shortcuts
                // Get shortcut for Opportunity
                if (GeosApplication.Instance.UserSettings.ContainsKey("Opportunity"))
                {
                    Opportunity = GeosApplication.Instance.UserSettings["Opportunity"].ToString();
                }

                // Get shortcut for  Contact
                if (GeosApplication.Instance.UserSettings.ContainsKey("Contact"))
                {
                    Contact = GeosApplication.Instance.UserSettings["Contact"].ToString();
                }

                // Get shortcut for  Account
                if (GeosApplication.Instance.UserSettings.ContainsKey("Account"))
                {
                    Account = GeosApplication.Instance.UserSettings["Account"].ToString();
                }

                // Get shortcut for Appointment
                if (GeosApplication.Instance.UserSettings.ContainsKey("Appointment"))
                {
                    Appointment = GeosApplication.Instance.UserSettings["Appointment"].ToString();
                }

                // Get shortcut for Call
                if (GeosApplication.Instance.UserSettings.ContainsKey("Call"))
                {
                    Call = GeosApplication.Instance.UserSettings["Call"].ToString();
                }

                // Get shortcut for Task
                if (GeosApplication.Instance.UserSettings.ContainsKey("Task"))
                {
                    Task = GeosApplication.Instance.UserSettings["Task"].ToString();
                }

                // Get shortcut for Email
                if (GeosApplication.Instance.UserSettings.ContainsKey("Email"))
                {
                    Email = GeosApplication.Instance.UserSettings["Email"].ToString();
                }

                // Get shortcut for Action
                if (GeosApplication.Instance.UserSettings.ContainsKey("Action"))
                {
                    Action = GeosApplication.Instance.UserSettings["Action"].ToString();
                }

                // Get shortcut Search Opportunity Or Order 
                if (GeosApplication.Instance.UserSettings.ContainsKey("SearchOpportunityOrOrder"))
                {
                    SearchOpportunityOrOrder = GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"].ToString();
                }
                if (GeosApplication.Instance.UserSettings.ContainsKey("MatrixList"))
                {
                    MatrixList = GeosApplication.Instance.UserSettings["MatrixList"].ToString();
                }
            }
        }

        public ActionPlan GetActionPlan()
        {
            if (CRMCommon.Instance.ActionPlan == null)
            {
                CRMCommon.Instance.ActionPlan = CrmStartUp.GetActionPlan(1);
            }

            return CRMCommon.Instance.ActionPlan;
        }
        /// <summary>
        /// [001][cpatil][GEOS2-4778][02-10-2023]
        /// </summary>
        /// <param name="obj"></param>
        public void OpenWindowClickOnShortcutKey(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = "";
                if (obj.KeyboardDevice.Modifiers == ModifierKeys.None)
                {
                    ShortcutKey = obj.Key.ToString();
                }
                else
                {
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        ShortcutKey = "ctrl";
                    }
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + shift";
                        }
                        else
                        {
                            ShortcutKey = "shift";
                        }
                    }
                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + alt";
                        }
                        else
                        {
                            ShortcutKey = "alt";
                        }
                    }

                    if ((obj.KeyboardDevice.Modifiers & ModifierKeys.Windows) == ModifierKeys.Windows)
                    {
                        if (ShortcutKey != "")
                        {
                            ShortcutKey = ShortcutKey + " + windows";
                        }
                        else
                        {
                            ShortcutKey = "windows";
                        }
                    }
                    if (obj.Key == Key.System)
                    {
                        if (obj.SystemKey.ToString().Contains("Left") || obj.SystemKey.ToString().Contains("Right"))
                        {
                            //checking
                        }
                        else
                        {
                            ShortcutKey = ShortcutKey + " + " + obj.SystemKey.ToString();
                        }
                    }
                    else
                    {
                        if (obj.Key.ToString().Contains("Left") || obj.Key.ToString().Contains("Right"))
                        {
                            //checking
                        }
                        else
                        {
                            ShortcutKey = ShortcutKey + " + " + obj.Key.ToString();
                        }
                    }
                }

                string[] Keys = ShortcutKey.Split('+');

                if (GeosApplication.Instance.UserSettings != null)
                {
                    // shortcuts
                    // Get shortcut for Opportunity
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Opportunity"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Opportunity"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                    }

                    // Get shortcut for  Contact
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Contact"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Contact"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //Contact = GeosApplication.Instance.UserSettings["Contact"].ToString();
                    }

                    // Get shortcut for  Account
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Account"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Account"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //Account = GeosApplication.Instance.UserSettings["Account"].ToString();
                    }

                    // Get shortcut for Appointment
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Appointment"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Appointment"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //Appointment = GeosApplication.Instance.UserSettings["Appointment"].ToString();
                    }

                    // Get shortcut for Call
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Call"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Call"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //Call = GeosApplication.Instance.UserSettings["Call"].ToString();
                    }

                    // Get shortcut for Task
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Task"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Task"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
                        {
                            Processing();
                            AddActivityView addActivityView = new AddActivityView();
                            AddActivityViewModel addActivityViewModel = new AddActivityViewModel();
                            int index = addActivityViewModel.TypeList.IndexOf(addActivityViewModel.TypeList.Where(a => a.IdLookupValue == 40).FirstOrDefault());
                            addActivityViewModel.SelectedIndexType = index;
                            addActivityViewModel.IsInternalEnable = true;
                           // [001]
                            addActivityViewModel.IsAppointmentVisible = Visibility.Collapsed;
                            addActivityViewModel.IsContactVisible = Visibility.Visible;
                            addActivityViewModel.IsEmailorCallVisible = Visibility.Collapsed;
                            addActivityViewModel.IsTaskVisible = Visibility.Visible;
                            addActivityViewModel.IsWatcherVisible = Visibility.Visible;
                            addActivityViewModel.IsDueDateVisible = Visibility.Visible;
                            addActivityViewModel.IsInternalVisible = Visibility.Visible;
                            addActivityViewModel.IsOwnerVisible = Visibility.Visible;
                            addActivityViewModel.IsTagVisible = Visibility.Visible;
                            addActivityViewModel.IsCommentsVisible = Visibility.Visible;
                            addActivityViewModel.IsInformationVisible = Visibility.Collapsed;
                            addActivityViewModel.IsCompletedVisible = Visibility.Collapsed;
                            addActivityViewModel.IsWatcherUserVisible = Visibility.Collapsed;
                            addActivityViewModel.IsWatcherRegionVisible = Visibility.Collapsed;
                            addActivityViewModel.SelectedWatchersUserList.Clear();
                            EventHandler handle = delegate { addActivityView.Close(); };
                            addActivityViewModel.RequestClose += handle;
                            addActivityView.DataContext = addActivityViewModel;

                            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                            addActivityView.ShowDialog();
                        }
                        //Task = GeosApplication.Instance.UserSettings["Task"].ToString();
                    }

                    // Get shortcut for Email
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Email"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Email"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //Email = GeosApplication.Instance.UserSettings["Email"].ToString();
                    }

                    // Get shortcut for Action
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Action"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["Action"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //Email = GeosApplication.Instance.UserSettings["Email"].ToString();
                    }

                
                    // Get shortcut Search Opportunity Or Order 
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SearchOpportunityOrOrder"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
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
                        //SearchOpportunityOrOrder = GeosApplication.Instance.UserSettings["SearchOpportunityOrOrder"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("MatrixList"))
                    {
                        string[] StoredKeys = GeosApplication.Instance.UserSettings["MatrixList"].ToString().Split('+');

                        int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                        if (count == StoredKeys.Count())
                        {
                            OpenMatrixListView();
                        }
                    }
                }
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method OpenWindowClickOnShortcutKey....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenWindowClickOnShortcutKey...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SetMyFilterString(MatrixViewModel viewModel)
        {
            var objCrmMainViewModel = (CrmMainViewModel)GeosApplication.Instance.ObjectPool["CrmMainViewModel"];
            var listofFilters = new List<string>();
            string concatedGridFilter = null;
            if (IsTimelineOpen && objCrmMainViewModel != null &&
              objCrmMainViewModel.ObjLeadsViewModel != null &&
              objCrmMainViewModel.ObjLeadsViewModel.SelectedObject != null)
            {

                var regionName = (string)objCrmMainViewModel.ObjLeadsViewModel.SelectedObject.Row.ItemArray[5];
                var groupName = (string)objCrmMainViewModel.ObjLeadsViewModel.SelectedObject.Row.ItemArray[6];
                var category1Name = (string)objCrmMainViewModel.ObjLeadsViewModel.SelectedObject.Row.ItemArray[29];
                listofFilters.Add($"[InUseYesOrNo] In('Yes')");
                if (!string.IsNullOrEmpty(category1Name))
                {
                    listofFilters.Add($" [ProductCategory.Name] In('{category1Name.Trim()}') ");
                }
                if (!string.IsNullOrEmpty(groupName))
                {
                    listofFilters.Add($" [Customer.CustomerName] In('{groupName.Trim()}') ");
                }
                if (!string.IsNullOrEmpty(regionName))
                {
                    listofFilters.Add($" [RegionLookupValue.Region] In('{regionName.Trim()}') ");
                }
            }
            else
            {
                listofFilters.Add($"[InUseYesOrNo] In('Yes')");
            }
            concatedGridFilter = String.Join(" AND ", listofFilters);
            viewModel.MyFilterString = concatedGridFilter;
        }
        private int getComparedShortcutKeyCount(string[] Keys, string[] StoredKeys)
        {
            int count = 0;
            if (Keys.Count() == StoredKeys.Count())
            {
                foreach (string key in Keys)
                {
                    foreach (string storedKey in StoredKeys)
                    {
                        if (key.ToUpper().TrimStart().TrimEnd() == storedKey.ToUpper().TrimStart().TrimEnd())
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        public void OpenMatrixListView()
        {
            Processing();
            var view = new MatrixView();
            var viewModel = new MatrixViewModel();
            EventHandler handle = delegate { view.Close(); };
            viewModel.RequestClose += handle;
            SetMyFilterString(viewModel);
            viewModel.Init();
            view.DataContext = viewModel;
            view.ShowDialog();
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
        #endregion;

        public void Init()
        {
            GeosApplication.Instance.Logger.Log("Starts Init() Method", category: Prism.Logging.Category.Exception, priority: Priority.Low);
            if (Application.Current == null)
            {
                // create the Application object
                try
                {
                    new Application
                    {
                        ShutdownMode = ShutdownMode.OnExplicitShutdown
                    };

                    //Themes.BlackAndBlue.v19.2
                    Theme theme = new Theme("BlackAndBlue", "DevExpress.Xpf.Themes.BlackAndBlue.v19.2");
                    theme.AssemblyName = "DevExpress.Xpf.Themes.BlackAndBlue.v19.2";
                    Theme.RegisterTheme(theme);

                    //Themes.WhiteAndBlue.v19.2
                    Theme themeWhiteAndBlue = new Theme("WhiteAndBlue", "DevExpress.Xpf.Themes.WhiteAndBlue.v19.2");
                    themeWhiteAndBlue.AssemblyName = "DevExpress.Xpf.Themes.WhiteAndBlue.v19.2";
                    Theme.RegisterTheme(themeWhiteAndBlue);

                    //ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
                    //ResourceDictionary dict = new ResourceDictionary();
                    //GeosApplication.Instance.Logger.Log("Before Themes in Init() Method", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ThemeName") && !string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["ThemeName"].ToString()))
                    {
                        if (GeosApplication.Instance.UserSettings["ThemeName"].ToString() == "WhiteAndBlue")
                        {
                            ApplicationThemeHelper.ApplicationThemeName = GeosApplication.Instance.UserSettings["ThemeName"].ToString();
                            //GeosApplication.Instance.Logger.Log("Before WhiteAndBlueTheme to dict in Init() Method", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                            //dict.Source = new Uri("/Emdep.Geos.Modules.Crm;component/Themes/WhiteAndBlueTheme.xaml", UriKind.Relative);
                        }
                        else
                        {
                            ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
                            //GeosApplication.Instance.Logger.Log("Before BlackAndBlueTheme to dict in Init() Method", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                            //dict.Source = new Uri("Emdep.Geos.Modules.Crm;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
                        }
                    }
                    else
                    {
                        ApplicationThemeHelper.ApplicationThemeName = "BlackAndBlue";
                        //dict.Source = new Uri("/Emdep.Geos.Modules.Crm;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
                    }
                    //GeosApplication.Instance.Logger.Log("Before MergedDictionaries to dict in Init() Method", category: Prism.Logging.Category.Exception, priority: Priority.Low);
                    //Application.Current.Resources.MergedDictionaries.Add(dict);


                    // merge in your application resources
                    Application.Current.Resources.MergedDictionaries.Add(
                    Application.LoadComponent(
                    new Uri("/Emdep.Geos.Modules.Crm;component/Resources/Language.xaml",
                    UriKind.Relative)) as ResourceDictionary);

                    //ApplicationThemeHelper.UseLegacyDefaultTheme = true;
                    ////Themes.BlackAndBlue.v19.2
                    //Theme theme = new Theme("BlackAndBlue", "DevExpress.Xpf.Themes.BlackAndBlue.v19.2");
                    //theme.AssemblyName = "DevExpress.Xpf.Themes.BlackAndBlue.v19.2";
                    //Theme.RegisterTheme(theme);

                    //// merge in your application resources
                    //Application.Current.Resources.MergedDictionaries.Add(
                    //Application.LoadComponent(
                    //new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml",
                    //UriKind.Relative)) as ResourceDictionary);

                    //new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml",
                    //dict.Source = new Uri("/GeosWorkbench;component/Themes/BlackAndBlueTheme.xaml", UriKind.Relative);
                }
                catch (Exception ex)
                {
                    //Application.
                }
            }
        }

        //public void EndInit()
        //{
        //    if (Application.Current != null)
        //    {
        //        // create the Application object
        //        try
        //        {
        //            Application.Current.MainWindow.Close(); // = null;
        //            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        //            Application.Current.Shutdown();
        //            GC.SuppressFinalize(this);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Application.
        //        }
        //    }
        //}
    }
}
