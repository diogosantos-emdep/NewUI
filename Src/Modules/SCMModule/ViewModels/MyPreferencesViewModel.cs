using DevExpress.Mvvm;
using DevExpress.Office.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.XtraRichEdit.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;


namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[shweta.thube][GEOS2-6630][04.04.2025]
    public class MyPreferencesViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMStartUp = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        bool isThumbnails;
        bool isGrid;
        private int resultPages;
        private bool isCustomChecked;
        private bool isYearChecked = true;
        private DateTime maxDate;
        private long selectedPeriod;
        private List<int> pagesList;
        private int selectedIndexPage;
        private int selectedProfileIndex;
        private double imageSize;
        private string create;
        private string search;
        private string locations;
        private string properties;
        private string searchManager;
        private string families;
        private string newSamples;
        private string modifiedSamples;
        private string connectors3D;
        private List<object> selectedSCMSections;
        private bool isEnabledConfigurationSection;
        private bool isEnabledSampleRegistration;
        private bool isEnabledReportAndSampleSearch;
        private bool isEnabledLocationManager;
        private bool allowPaging;
        #endregion

        #region public Properties
        public string Create
        {
            get
            { return create; }
            set
            {
                create = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Create"));
            }
        }
        public string Search
        {
            get
            { return search; }
            set
            {
                search = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Search"));
            }
        }
        public string Locations
        {
            get
            { return locations; }
            set
            {
                locations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Locations"));
            }
        }
        public string Properties
        {
            get
            { return properties; }
            set
            {
                properties = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Properties"));
            }
        }
        public string Families
        {
            get
            { return families; }
            set
            {
                families = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Families"));
            }
        }
        public string SearchManager
        {
            get
            { return searchManager; }
            set
            {
                searchManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchManager"));
            }
        }
        public string NewSamples
        {
            get
            { return newSamples; }
            set
            {
                newSamples = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewSamples"));
            }
        }
        public string ModifiedSamples
        {
            get
            { return modifiedSamples; }
            set
            {
                modifiedSamples = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifiedSamples"));
            }
        }       
        public string Connectors3D
        {
            get
            { return connectors3D; }
            set
            {
                connectors3D = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Connectors3D"));
            }
        }
        public List<object> SelectedSCMSections
        {
            get
            {
                return selectedSCMSections;
            }

            set
            {
                selectedSCMSections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSCMSections"));
            }
        }
        public bool IsEnabledConfigurationSection
        {
            get
            { return isEnabledConfigurationSection; }
            set
            {
                isEnabledConfigurationSection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledConfigurationSection"));
            }
        }
        public bool IsEnabledSampleRegistration
        {
            get
            { return isEnabledSampleRegistration; }
            set
            {
                isEnabledSampleRegistration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledSampleRegistration"));
            }
        }
        public bool IsEnabledReportAndSampleSearch
        {
            get
            { return isEnabledReportAndSampleSearch; }
            set
            {
                isEnabledReportAndSampleSearch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledReportAndSampleSearch"));
            }
        }
        public bool IsEnabledLocationManager
        {
            get
            { return isEnabledLocationManager; }
            set
            {
                isEnabledLocationManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnabledLocationManager"));
            }
        }

        public bool IsGrid
        {
            get
            { return isGrid; }
            set
            {
                isGrid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGrid"));
            }
        }
        public bool IsThumbnails
        {
            get
            { return isThumbnails; }
            set
            {
                isThumbnails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsThumbnails"));
            }
        }
        public int ResultPages
        {
            get
            { return resultPages; }
            set
            {
                resultPages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ResultPages"));
            }
        }
        public double ImageSize
        {
            get
            { return imageSize; }
            set
            {
                imageSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageSize"));
            }
        }
        public List<int> PagesList
        {
            get
            { return pagesList; }
            set
            {
                pagesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PagesList"));
            }
        }
		//[nsatpute][21-05-2025][GEOS2-7996]
        public bool AllowPaging
        {
            get
            { return allowPaging; }
            set
            {
                allowPaging = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllowPaging"));
            }
        }
        #endregion

        #region public ICommand

        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand Create_KeyDownCommand { get; set; }
        public ICommand Create_PreviewKeyDownAllCommand { get; set; }
        public ICommand Create_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Create_PreviewKeyDownPasteCommand { get; set; }
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public ICommand Search_PreviewKeyDownAllCommand { get; set; }
        public ICommand Search_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Search_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Locations_PreviewKeyDownAllCommand { get; set; }
        public ICommand Locations_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Locations_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Properties_PreviewKeyDownAllCommand { get; set; }
        public ICommand Properties_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Properties_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Families_PreviewKeyDownAllCommand { get; set; }
        public ICommand Families_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Families_PreviewKeyDownPasteCommand { get; set; }
        public ICommand SearchManager_PreviewKeyDownAllCommand { get; set; }
        public ICommand SearchManager_PreviewKeyDownCopyCommand { get; set; }
        public ICommand SearchManager_PreviewKeyDownPasteCommand { get; set; }
        public ICommand NewSamples_PreviewKeyDownAllCommand { get; set; }
        public ICommand NewSamples_PreviewKeyDownCopyCommand { get; set; }
        public ICommand NewSamples_PreviewKeyDownPasteCommand { get; set; }
        public ICommand ModifiedSamples_PreviewKeyDownAllCommand { get; set; }
        public ICommand ModifiedSamples_PreviewKeyDownCopyCommand { get; set; }
        public ICommand ModifiedSamples_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Connectors3D_PreviewKeyDownAllCommand { get; set; }
        public ICommand Connectors3D_PreviewKeyDownCopyCommand { get; set; }
        public ICommand Connectors3D_PreviewKeyDownPasteCommand { get; set; }
        public ICommand Search_KeyDownCommand { get; set; }
        public ICommand Locations_KeyDownCommand { get; set; }
        public ICommand Properties_KeyDownCommand { get; set; }
        public ICommand Families_KeyDownCommand { get; set; }
        public ICommand SearchManager_KeyDownCommand { get; set; }
        public ICommand NewSamples_KeyDownCommand { get; set; }
        public ICommand ModifiedSamples_KeyDownCommand { get; set; }
        public ICommand Connectors3D_KeyDownCommand { get; set; }
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

        #region validation

       
           
        #endregion

        #region Constructor

        public MyPreferencesViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel ...", category: Category.Info, priority: Priority.Low);

                MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                Create_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Create_KeyDown);
                Create_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Create_PreviewKeyDown_All);
                Create_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Create_PreviewKeyDown_Copy);
                Create_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Create_PreviewKeyDown_Paste);
                MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));

                Search_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Search_PreviewKeyDown_All);
                Search_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Search_PreviewKeyDown_Copy);
                Search_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Search_PreviewKeyDown_Paste);

                Locations_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Locations_PreviewKeyDown_All);
                Locations_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Locations_PreviewKeyDown_Copy);
                Locations_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Locations_PreviewKeyDown_Paste);

                Properties_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Properties_PreviewKeyDown_All);
                Properties_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Properties_PreviewKeyDown_Copy);
                Properties_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Properties_PreviewKeyDown_Paste);

                Families_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Families_PreviewKeyDown_All);
                Families_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Families_PreviewKeyDown_Copy);
                Families_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Families_PreviewKeyDown_Paste);

                SearchManager_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(SearchManager_PreviewKeyDown_All);
                SearchManager_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(SearchManager_PreviewKeyDown_Copy);
                SearchManager_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(SearchManager_PreviewKeyDown_Paste);

                NewSamples_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(NewSamples_PreviewKeyDown_All);
                NewSamples_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(NewSamples_PreviewKeyDown_Copy);
                NewSamples_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(NewSamples_PreviewKeyDown_Paste);

                ModifiedSamples_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(ModifiedSamples_PreviewKeyDown_All);
                ModifiedSamples_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(ModifiedSamples_PreviewKeyDown_Copy);
                ModifiedSamples_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(ModifiedSamples_PreviewKeyDown_Paste);

                Connectors3D_PreviewKeyDownAllCommand = new DelegateCommand<KeyEventArgs>(Connectors3D_PreviewKeyDown_All);
                Connectors3D_PreviewKeyDownCopyCommand = new DelegateCommand<KeyEventArgs>(Connectors3D_PreviewKeyDown_Copy);
                Connectors3D_PreviewKeyDownPasteCommand = new DelegateCommand<KeyEventArgs>(Connectors3D_PreviewKeyDown_Paste);

                Search_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Search_KeyDown);
                Locations_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Locations_KeyDown);
                Properties_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Properties_KeyDown);
                Families_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Families_KeyDown);
                SearchManager_KeyDownCommand = new DelegateCommand<KeyEventArgs>(SearchManager_KeyDown);
                NewSamples_KeyDownCommand = new DelegateCommand<KeyEventArgs>(NewSamples_KeyDown);
                ModifiedSamples_KeyDownCommand = new DelegateCommand<KeyEventArgs>(ModifiedSamples_KeyDown);
                Connectors3D_KeyDownCommand = new DelegateCommand<KeyEventArgs>(Connectors3D_KeyDown);      

                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                FillPages();//[rdixit][GEOS2-6631][14.04.2025]
                if (GeosApplication.Instance.UserSettings != null)
                {
                    // Get shortcut for  Contact
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Create"))
                    {
                        Create = GeosApplication.Instance.UserSettings["Create"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Search"))
                    {
                        Search = GeosApplication.Instance.UserSettings["Search"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
                    {
                        Locations = GeosApplication.Instance.UserSettings["Locations"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
                    {
                        Properties = GeosApplication.Instance.UserSettings["Properties"].ToString();

                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Families"))
                    {
                        Families = GeosApplication.Instance.UserSettings["Families"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
                    {
                        SearchManager = GeosApplication.Instance.UserSettings["SearchManager"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
                    {
                        NewSamples = GeosApplication.Instance.UserSettings["NewSamples"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
                    {
                        ModifiedSamples = GeosApplication.Instance.UserSettings["ModifiedSamples"].ToString();
                    }
                    if (GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
                    {
                        Connectors3D = GeosApplication.Instance.UserSettings["Connectors3D"].ToString();
                    }

                    #region [rdixit][GEOS2-6631][14.04.2025]  - [GEOS2-8036][13.05.2025][rdixit]
                    if (GeosApplication.Instance.UserSettings.ContainsKey("DefaultView"))
                    {
                        if (GeosApplication.Instance.UserSettings["DefaultView"] == "Thumbnails")
                        {
                            IsThumbnails = true;
                            IsGrid = false;
                        }
                        else
                        {
                            IsThumbnails = false;
                            IsGrid = true;
                        }
                    }
                    else
                    {
                        IsThumbnails = false;
                        IsGrid = true;
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                    {
                        ResultPages = Convert.ToInt32(GeosApplication.Instance.UserSettings["ResultPages"]);
                    }

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImageSize"))
                    {
                        ImageSize = Convert.ToDouble(GeosApplication.Instance.UserSettings["ImageSize"]);
                    }
					//[nsatpute][21-05-2025][GEOS2-7996]
                    if (GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                    {
                        AllowPaging = Convert.ToBoolean(GeosApplication.Instance.UserSettings["AllowPaging"]);
                    }
                    #endregion

                }
                if (GeosApplication.Instance.SelectedScmSectionsList == null)
                    GeosApplication.Instance.SelectedScmSectionsList = new List<SCMSections>();

                if (GeosApplication.Instance.SelectedScmSectionsList.Count == 0 || GeosApplication.Instance.SelectedScmSectionsList[0] == null)
                {
                    SelectedSCMSections = new List<object>(GeosApplication.Instance.ScmSectionsList);
                }
                else
                {
                    SelectedSCMSections = new List<object>(GeosApplication.Instance.SelectedScmSectionsList);
                }

                #region Permissions
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                {
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                    {
                        IsEnabledConfigurationSection = true;
                    }
                    else
                    {
                        IsEnabledConfigurationSection = false;
                    }
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission ||
                               GeosApplication.Instance.IsSCMPermissionReadOnly ||
                               GeosApplication.Instance.IsSCMEditLocationsManager)
                    {
                        IsEnabledLocationManager = true;
                    }
                    else
                    {
                        IsEnabledLocationManager = false;
                    }

                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly)
                    {
                        IsEnabledReportAndSampleSearch = true;

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMSampleRegistrationPermission)
                        {
                            IsEnabledSampleRegistration = true;
                        }
                        else
                        {
                            IsEnabledSampleRegistration = false;
                        }


                    }
                    else
                    {
                        IsEnabledReportAndSampleSearch = false;
                    }
                }

                #endregion
                GeosApplication.Instance.Logger.Log("Constructor MyPreferencesViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in MyPreferencesViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }



        #endregion

        #region Methods
        //[rdixit][GEOS2-6631][14.04.2025]
        private void FillPages()
        {   
			//[nsatpute][21-05-2025][GEOS2-7996]         
            PagesList = new List<int>();
            IList<LookupValue> lstPages = CrmService.GetLookupValues(183);
            if (lstPages != null)
                PagesList.AddRange(lstPages.Select(x => int.Parse(x.Value)).ToList());            
        }
        public void CloseWindow(object obj)
        {
            //IsPreferenceChanged = false;
            RequestClose(null, null);

        }
        private void Create_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Create_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Create = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Create_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Create_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetShortcutKey(KeyEventArgs obj)
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
            return ShortcutKey;
        }
        public bool ShortKeyValidate(string shortcutKey)
        {
            bool status = true;

            if (shortcutKey.ToUpper().Contains(Key.Escape.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.LWin.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains("VOLUME") ||
                shortcutKey.ToUpper().Contains("MEDIA") ||
                shortcutKey.ToUpper().Contains("SYSTEM") ||
                shortcutKey.ToUpper().Contains("OEM") ||
                shortcutKey.ToUpper().Contains("NUM") ||
                shortcutKey.ToUpper().Contains(Key.Subtract.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Multiply.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Divide.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Tab.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Add.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Return.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.Decimal.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D1.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D2.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D3.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D4.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D5.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D6.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D7.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D8.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D9.ToString().ToUpper()) ||
                shortcutKey.ToUpper().Contains(Key.D0.ToString().ToUpper()))
            {
                status = false;
            }
            return status;
        }
        private void Create_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Create_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Create = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Create_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Create_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Create_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Create_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Create = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Create_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Create_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Create_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Create_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Create = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Create_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Create_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Search_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Search_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Search = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Search_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Search_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Search_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Search_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Search = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Search_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Search_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Search_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Search_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Search = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Search_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Search_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Locations_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Locations_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Locations = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Locations_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Locations_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Locations_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Locations_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Locations = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Locations_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Locations_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Locations_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Locations_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Locations = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Locations_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Locations_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Properties_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Properties_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Properties = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Properties_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Properties_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Properties_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Properties_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Properties = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Properties_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Properties_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Properties_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Properties_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Properties = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Properties_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Properties_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Families_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Families_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Families = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Families_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Families_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Families_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Families_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Families = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Families_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Families_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Families_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Families_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Families = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Families_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Families_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SearchManager_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchManager_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchManager = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method SearchManager_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchManager_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchManager_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchManager_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchManager = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method SearchManager_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchManager_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchManager_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchManager_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchManager = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method SearchManager_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchManager_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void NewSamples_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method NewSamples_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SearchManager = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method NewSamples_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NewSamples_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NewSamples_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method NewSamples_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                NewSamples = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method NewSamples_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NewSamples_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NewSamples_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method NewSamples_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                NewSamples = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method NewSamples_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NewSamples_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ModifiedSamples_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ModifiedSamples_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ModifiedSamples = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method ModifiedSamples_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ModifiedSamples_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ModifiedSamples_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ModifiedSamples_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ModifiedSamples = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method ModifiedSamples_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ModifiedSamples_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ModifiedSamples_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ModifiedSamples_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                ModifiedSamples = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method ModifiedSamples_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ModifiedSamples_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Connectors3D_PreviewKeyDown_All(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Connectors3D_PreviewKeyDown_All ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Connectors3D = "Ctrl + A";
                GeosApplication.Instance.Logger.Log("Method Connectors3D_PreviewKeyDown_All....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Connectors3D_PreviewKeyDown_All...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Connectors3D_PreviewKeyDown_Copy(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Connectors3D_PreviewKeyDown_Copy ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Connectors3D = "Ctrl + C";
                GeosApplication.Instance.Logger.Log("Method Connectors3D_PreviewKeyDown_Copy....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Connectors3D_PreviewKeyDown_Copy...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Connectors3D_PreviewKeyDown_Paste(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Connectors3D_PreviewKeyDown_Paste ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Connectors3D = "Ctrl + V";
                GeosApplication.Instance.Logger.Log("Method Connectors3D_PreviewKeyDown_Paste....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Connectors3D_PreviewKeyDown_Paste...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void Search_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Search_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Search = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Search_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Search_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Locations_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Locations_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Locations = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Locations_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Locations_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Properties_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Properties_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Properties = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Properties_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Properties_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Families_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Families_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Families = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Families_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Families_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void SearchManager_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method SearchManager_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                SearchManager = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method SearchManager_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method SearchManager_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void NewSamples_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method NewSamples_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                NewSamples = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method NewSamples_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method NewSamples_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ModifiedSamples_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method ModifiedSamples_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                ModifiedSamples = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method ModifiedSamples_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ModifiedSamples_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void Connectors3D_KeyDown(KeyEventArgs obj)
        {
            GeosApplication.Instance.Logger.Log("Method Connectors3D_KeyDown ...", category: Category.Info, priority: Priority.Low);
            try
            {
                string ShortcutKey = GetShortcutKey(obj);
                bool validate = ShortKeyValidate(ShortcutKey);
                if (validate == false)
                {
                    obj.Handled = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValidateShortcutKey").ToString(), ShortcutKey), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                obj.Handled = true;
                Connectors3D = GetFirstCharCapital(ShortcutKey);
                GeosApplication.Instance.Logger.Log("Method Connectors3D_KeyDown....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Connectors3D_KeyDown...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private string GetFirstCharCapital(string str)
        {
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
        }

        private void FillSectionsDeatils()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils ...", category: Category.Info, priority: Priority.Low);

                List<SCMSections> scmList = new List<SCMSections>();

                scmList.Add(new SCMSections() { IdSection = 1, SectionName = System.Windows.Application.Current.FindResource("SCMsampleregistraion").ToString() });
                scmList.Add(new SCMSections() { IdSection = 2, SectionName = System.Windows.Application.Current.FindResource("SCMConnectorsTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 3, SectionName = System.Windows.Application.Current.FindResource("SCMLocationsManagerTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 5, SectionName = System.Windows.Application.Current.FindResource("SCMPropertiesManager").ToString() });
                scmList.Add(new SCMSections() { IdSection = 6, SectionName = System.Windows.Application.Current.FindResource("SCMFamiliesManager").ToString() });
                scmList.Add(new SCMSections() { IdSection = 7, SectionName = System.Windows.Application.Current.FindResource("SCMSearchFilterManager").ToString() });
                scmList.Add(new SCMSections() { IdSection = 8, SectionName = System.Windows.Application.Current.FindResource("SCMNewSamplesTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 9, SectionName = System.Windows.Application.Current.FindResource("SCMModifiedSamplesTitle").ToString() });
                scmList.Add(new SCMSections() { IdSection = 10, SectionName = System.Windows.Application.Current.FindResource("SCM3DConnectorItemsTitle").ToString() });

                GeosApplication.Instance.ScmSectionsList = scmList;

                string[] ScmSelectedStr = GeosApplication.Instance.UserSettings["SelectedSCMSectionLoadData"].Split(',');
                GeosApplication.Instance.SelectedScmSectionsList = new List<SCMSections>();
                if (ScmSelectedStr != null && ScmSelectedStr[0] != string.Empty)
                {
                    foreach (var item in ScmSelectedStr)
                    {

                        GeosApplication.Instance.SelectedScmSectionsList.Add(GeosApplication.Instance.ScmSectionsList.FirstOrDefault(crm => crm.IdSection == Convert.ToInt16(item.ToString())));
                    }

                }
                else
                {
                    GeosApplication.Instance.SelectedScmSectionsList = GeosApplication.Instance.ScmSectionsList.ToList();
                }

                GeosApplication.Instance.Logger.Log("Method FillSectionsDeatils() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillSectionsDeatils() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void SaveMyPreference(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference ...", category: Category.Info, priority: Priority.Low);
               
               

                string selectedSCMSectionStr = string.Empty;

                List<string> Records = new List<string>();
                Records.Add(Create);
                Records.Add(Search);
                Records.Add(Locations);
                Records.Add(Properties);
                Records.Add(Families);
                Records.Add(SearchManager);
                Records.Add(NewSamples);
                Records.Add(ModifiedSamples);
                Records.Add(Connectors3D);


                var query = Records.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();

                if (query != null && query.Count > 0)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DuplicateShortcutKey").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (SelectedSCMSections != null && SelectedSCMSections.Count > 0)
                {
                    foreach (SCMSections SCMSection in SelectedSCMSections)
                    {
                        if (SCMSection != null)
                        {
                            if (string.IsNullOrEmpty(selectedSCMSectionStr))
                                selectedSCMSectionStr = SCMSection.IdSection.ToString();
                            else
                                selectedSCMSectionStr += "," + SCMSection.IdSection.ToString();
                        }
                    }

                    GeosApplication.Instance.SelectedScmSectionsList = SelectedSCMSections.Cast<SCMSections>().ToList();
                }

                else
                {
                    GeosApplication.Instance.SelectedScmSectionsList = null;
                }
                List<Tuple<string, string>> lstUserConfiguration = new List<Tuple<string, string>>();

                if (GeosApplication.Instance.UserSettings != null)
                {
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Create"))
                    {
                        GeosApplication.Instance.UserSettings["Create"] = Create.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Search"))
                    {
                        GeosApplication.Instance.UserSettings["Search"] = Search.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
                    {
                        GeosApplication.Instance.UserSettings["Locations"] = Locations.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
                    {
                        GeosApplication.Instance.UserSettings["Properties"] = Properties.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Families"))
                    {
                        GeosApplication.Instance.UserSettings["Families"] = Families.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
                    {
                        GeosApplication.Instance.UserSettings["SearchManager"] = SearchManager.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
                    {
                        GeosApplication.Instance.UserSettings["NewSamples"] = NewSamples.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
                    {
                        GeosApplication.Instance.UserSettings["ModifiedSamples"] = ModifiedSamples.TrimStart().TrimEnd();
                    }
                    if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
                    {
                        GeosApplication.Instance.UserSettings["Connectors3D"] = Connectors3D.TrimStart().TrimEnd();
                    }
                    //[rdixit][GEOS2-6631][14.04.2025] - [GEOS2-8036][13.05.2025][rdixit]
                    if (GeosApplication.Instance.UserSettings.ContainsKey("DefaultView"))
                        GeosApplication.Instance.UserSettings["DefaultView"] = IsThumbnails ? "Thumbnails" : "Grid";
                    else
                        GeosApplication.Instance.UserSettings.Add("DefaultView", IsThumbnails ? "Thumbnails" : "Grid");

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ResultPages"))
                        GeosApplication.Instance.UserSettings["ResultPages"] = ResultPages.ToString();
                    else
                        GeosApplication.Instance.UserSettings.Add("ResultPages", ResultPages.ToString());

                    if (GeosApplication.Instance.UserSettings.ContainsKey("ImageSize"))
                        GeosApplication.Instance.UserSettings["ImageSize"] = ImageSize.ToString();
                    else
                        GeosApplication.Instance.UserSettings.Add("ImageSize", ImageSize.ToString());

					//[nsatpute][21-05-2025][GEOS2-7996]
                    if (GeosApplication.Instance.UserSettings.ContainsKey("AllowPaging"))
                        GeosApplication.Instance.UserSettings["AllowPaging"] = AllowPaging.ToString();
                    else
                        GeosApplication.Instance.UserSettings.Add("AllowPaging", AllowPaging.ToString());

                    SCMShortcuts.Instance.GetShortcuts();
                }
                foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
                {
                    lstUserConfiguration.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
                }

                ApplicationOperation.CreateNewSetting(lstUserConfiguration, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method SaveMyPreference() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SaveMyPreference() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                SCMShortcuts.Instance.IsActive = false;
                OpenWindowClickOnShortcutKey(obj);
                //SCMShortcuts.Instance.IsActive = false;
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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
                    if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                    {
                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMPermissionReadOnly)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("NewSamples"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["NewSamples"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }
                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("ModifiedSamples"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["ModifiedSamples"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }
                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("Connectors3D"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Connectors3D"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }
                            }

                            if (GeosApplication.Instance.UserSettings.ContainsKey("Search"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Search"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }


                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMSampleRegistrationPermission)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Create"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Create"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }


                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("SearchManager"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["SearchManager"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    RequestClose(null, null);
                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                }
                            }
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Properties"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Properties"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    RequestClose(null, null);
                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);

                                }
                            }
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Families"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Families"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {

                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }
                                }
                            }
                        }

                        if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMViewConfigurationPermission ||
                               GeosApplication.Instance.IsSCMPermissionReadOnly ||
                               GeosApplication.Instance.IsSCMEditLocationsManager)
                        {
                            if (GeosApplication.Instance.UserSettings.ContainsKey("Locations"))
                            {
                                string[] StoredKeys = GeosApplication.Instance.UserSettings["Locations"].ToString().Split('+');

                                int count = getComparedShortcutKeyCount(Keys, StoredKeys);

                                if (count == StoredKeys.Count())
                                {
                                    SCMShortcuts.Instance.IsActive = false;
                                    SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                                    if (SCMShortcuts.Instance.IsActive)
                                    {
                                        RequestClose(null, null);
                                        SCMShortcuts.Instance.IsActive = false;
                                    }

                                }
                            }


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


        #endregion
    }

}
