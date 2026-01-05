using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static DevExpress.DataAccess.Native.DataFederation.QueryBuilder.AvailableItemData;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class SearchFiltersManagerViewModel : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region Services
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        public void Dispose()
        {

        }
        #endregion

        #region Declarations
        List<SimilarColorsByConfiguration> colorSimilarityList1;
        Dictionary<int, HashSet<int>> colorConnections = new Dictionary<int, HashSet<int>>();
        List<List<int>> colorGroups = new List<List<int>>();
        List<SimilarColorsByConfiguration> allColorSimilarityList;
        bool isstart = true;
        private ObservableCollection<Data.Common.SCM.Color> listColor;
        private ObservableCollection<Data.Common.SCM.Color> originalColorList;
        private Data.Common.SCM.Color selectedColor;
        private string language;
        private ObservableCollection<SimilarColorsByConfiguration> colorSimilarityList;
        private SimilarColorsByConfiguration selectedColorSimilarity;
        private ObservableCollection<SimilarCharactersByConfiguration> similarCharactersList;
        private SimilarCharactersByConfiguration selectedSimilarCharacters;
        private ObservableCollection<ComponentsByConfiguration> componentsList;
        private ComponentsByConfiguration selectedComponent;
        private ObservableCollection<ComponentType> componentTypeList;
        private ObservableCollection<ConfigurationFamily> configurationList;
        private int _Internal = 0;
        private int _External = 0;
        private int height = 0;
        private int length = 0;
        private int width = 0;
        private int wayMargin = 0;
        private ConfigurationFamily selectedConfiguration;
        bool allowValidation = false;
        private string referencePagesToApply = "0";
        private string appearancePagesToApply = "0";
        private string diameterAndSizePagesToApply = "0";
        private string componentsPagesToApply = "0";
        private string waysMarginPagesApply = "0";
        private Int32 numberOfPages;
        bool isSave;
        private bool isSCMEditFiltersManager;//[pramod.misal][GEOS2-5481][23.05.2024]
        private bool isSCMEditFiltersManagerBtn;//[pramod.misal][GEOS2-5482][24.05.2024]

        private string windowHeader; //[GEOS2-5826][shweta.thube][22.07.2024]

        public Int32 IdSelectedColorList;
        #endregion

        #region Properties
        //[pramod.misal][GEOS2-5481][23.05.2024]
        public bool IsSCMEditFiltersManager
        {
            get { return isSCMEditFiltersManager; }
            set
            {

                isSCMEditFiltersManager = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFiltersManager"));

            }
        }

        //[pramod.misal][GEOS2-5482][24.05.2024]
        public bool IsSCMEditFiltersManagerBtn
        {
            get { return isSCMEditFiltersManagerBtn; }
            set
            {

                isSCMEditFiltersManagerBtn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSCMEditFiltersManagerBtn"));

            }
        }
       
        //public bool IsSCMEditFiltersManager
        //{
        //    get { return isSCMEditFiltersManager; }
        //    set
        //    {
        //        if (isSCMEditFiltersManager != value)
        //        {
        //            isSCMEditFiltersManager = GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditFiltersManager;
        //            OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsSCMEditFiltersManager)));
        //        }
        //    }
        //}
        #region [GEOS2-5437][rdixit][07.03.2024]
        public List<SimilarColorsByConfiguration> AllColorSimilarityList
        {
            get { return allColorSimilarityList; }
            set
            {
                allColorSimilarityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllColorSimilarityList"));
            }
        }

        public List<List<int>> ColorGroups
        {
            get { return colorGroups; }
            set
            {
                colorGroups = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColorGroups"));
            }
        }
        public Dictionary<int, HashSet<int>> ColorConnections
        {
            get { return colorConnections; }
            set
            {
                colorConnections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColorConnections"));
            }
        }
        #endregion
        public int WayMargin
        {
            get { return wayMargin; }
            set
            {
                wayMargin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WayMargin"));
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.WayMargin = WayMargin;
                }
            }
        }
        public int Internal
        {
            get { return _Internal; }
            set
            {
                _Internal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Internal"));
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.Internal = Internal;
                }
            }
        }
        public int External
        {
            get { return _External; }
            set
            {
                _External = value;
                OnPropertyChanged(new PropertyChangedEventArgs("External"));
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.External = External;
                }
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.Height = Height;
                }
            }
        }
        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Length"));
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.Length = Length;
                }
            }
        }
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.Width = Width;
                }
            }
        }
        public ObservableCollection<Data.Common.SCM.Color> ListColor
        {
            get { return listColor; }
            set
            {
                listColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListColor"));
            }
        }
        public ObservableCollection<Data.Common.SCM.Color> OriginalColorList
        {
            get { return originalColorList; }
            set
            {
                originalColorList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalColorList"));
            }
        }
        public Data.Common.SCM.Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedColor"));
                if (SelectedColor != null && SelectedConfiguration != null)
                {
                    //[GEOS2-5437][rdixit][07.03.2024]
                    SelectedConfiguration.IdColor = SelectedColor.Id;
                    ColorSimilarityList = new ObservableCollection<SimilarColorsByConfiguration>(AllColorSimilarityList.Where(i => i.IdColorA == SelectedColor.Id).ToList());
                    SelectedConfiguration.ColorSimilarityList = AllColorSimilarityList.Where(i => i.IdColorA == SelectedColor.Id).ToList();
                }
            }
        }
        public ObservableCollection<SimilarColorsByConfiguration> ColorSimilarityList
        {
            get { return colorSimilarityList; }
            set
            {
                colorSimilarityList = value;
                ColorSimilarityList1 = ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList();
                OnPropertyChanged(new PropertyChangedEventArgs("ColorSimilarityList"));
            }
        }
        public SimilarColorsByConfiguration SelectedColorSimilarity
        {
            get { return selectedColorSimilarity; }
            set
            {
                selectedColorSimilarity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedColorSimilarity"));
            }
        }

        public List<SimilarColorsByConfiguration> ColorSimilarityList1
        {
            get { return colorSimilarityList1; }
            set
            {
                colorSimilarityList1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ColorSimilarityList1"));
            }
        }
        public ObservableCollection<SimilarCharactersByConfiguration> SimilarCharactersList
        {
            get { return similarCharactersList; }
            set
            {
                similarCharactersList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SimilarCharactersList"));
            }
        }
        public SimilarCharactersByConfiguration SelectedSimilarCharacters
        {
            get { return selectedSimilarCharacters; }
            set
            {
                selectedSimilarCharacters = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSimilarCharacters"));
            }
        }
        public ObservableCollection<ComponentsByConfiguration> ComponentsList
        {
            get { return componentsList; }
            set
            {
                componentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentsList"));
            }
        }
        public ComponentsByConfiguration SelectedComponent
        {
            get { return selectedComponent; }
            set
            {
                selectedComponent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComponent"));
            }
        }
        public ObservableCollection<ComponentType> ComponentTypeList
        {
            get { return componentTypeList; }
            set
            {
                componentTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentTypeList"));
            }
        }
        public ObservableCollection<ConfigurationFamily> ConfigurationList
        {
            get { return configurationList; }
            set
            {
                configurationList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConfigurationList"));
            }
        }
        public ConfigurationFamily SelectedConfiguration
        {
            get { return selectedConfiguration; }
            set
            {
                selectedConfiguration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedConfiguration"));
                IdSelectedColorList = SelectedConfiguration.IdSearchConfiguration;
            }
        }
        public string ReferencePagesToApply
        {
            get { return referencePagesToApply; }
            set
            {
                referencePagesToApply = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferencePagesToApply"));
                #region To update in Current config
                try
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("ReferencePagesToApply"));
                    if (error != null)
                    {
                        return;
                    }
                    if ((!string.IsNullOrEmpty(ReferencePagesToApply)) && SelectedConfiguration != null)
                        SelectedConfiguration.RefPages = ReferencePagesToApply;
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ReferencePagesToApply Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }
        public string AppearancePagesToApply
        {
            get { return appearancePagesToApply; }
            set
            {
                appearancePagesToApply = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AppearancePagesToApply"));
                #region To update in Current config
                try
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("AppearancePagesToApply"));

                    if (error != null)
                    {
                        return;
                    }
                    if ((!string.IsNullOrEmpty(AppearancePagesToApply)) && SelectedConfiguration != null)
                    {
                        SelectedConfiguration.ColorPages = AppearancePagesToApply;
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AppearancePagesToApply Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }
        public string DiameterAndSizePagesToApply
        {
            get { return diameterAndSizePagesToApply; }
            set
            {
                diameterAndSizePagesToApply = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DiameterAndSizePagesToApply"));
                #region To update in Current config
                try
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("DiameterAndSizePagesToApply"));

                    if (error != null)
                    {
                        return;
                    }
                    if ((!string.IsNullOrEmpty(DiameterAndSizePagesToApply)) && SelectedConfiguration != null)
                    {
                        SelectedConfiguration.SizePages = DiameterAndSizePagesToApply;
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in DiameterAndSizePagesToApply Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }
        public string ComponentsPagesToApply
        {
            get { return componentsPagesToApply; }
            set
            {
                componentsPagesToApply = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ComponentsPagesToApply"));
                #region To update in Current config
                try
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("ComponentsPagesToApply"));

                    if (error != null)
                    {
                        return;
                    }
                    if ((!string.IsNullOrEmpty(ComponentsPagesToApply)) && SelectedConfiguration != null)
                    {
                        SelectedConfiguration.CompPages = ComponentsPagesToApply;
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in ComponentsPagesToApply Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }
        public string WaysMarginPagesApply
        {
            get { return waysMarginPagesApply; }
            set
            {
                waysMarginPagesApply = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WaysMarginPagesApply"));
                #region To update in Current config
                try
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("WaysMarginPagesApply"));

                    if (error != null)
                    {
                        return;
                    }
                    if ((!string.IsNullOrEmpty(WaysMarginPagesApply)) && SelectedConfiguration != null)
                    {
                        SelectedConfiguration.WaysPages = WaysMarginPagesApply;
                    }
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in WaysMarginPagesApply Property " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
        }
        public Int32 NumberOfPages
        {
            get { return numberOfPages; }
            set
            {
                numberOfPages = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NumberOfPages"));
                SelectedConfiguration.NoOfPages = NumberOfPages;
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

        //[GEOS2-5826][shweta.thube][22.07.2024]
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        #endregion

        #region Public ICommand        
        public ICommand ComponentsRowUpdatedCommnad { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AddSearchFamilyConfigurationButtonCommand { get; set; }
        public ICommand AddColorSimilarityButtonCommand { get; set; }
        public ICommand AppearanceCrossCommand { get; set; }
        public ICommand AddSimilarCharactersCommand { get; set; }
        public ICommand ReferenceCrossButtonCommand { get; set; }
        public ICommand AddComponentsButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand DeleteComponentsCommand { get; set; }
        public ICommand ConfigurationClosedCommand { get; set; }
        public ICommand ColorRowUpdatedCommnad { get; set; }
        public ICommand ConfigClosingCommand { get; set; }
        public ICommand CloseButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Constructor
        public SearchFiltersManagerViewModel()
        {
            try
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
                AllColorSimilarityList = new List<SimilarColorsByConfiguration>();
                ColorGroups = new List<List<int>>();
                ColorConnections = new Dictionary<int, HashSet<int>>();
                language = GeosApplication.Instance.CurrentCulture;
                ColorSimilarityList = new ObservableCollection<SimilarColorsByConfiguration>();
                SelectedColorSimilarity = new SimilarColorsByConfiguration();
                SimilarCharactersList = new ObservableCollection<SimilarCharactersByConfiguration>();
                SelectedSimilarCharacters = new SimilarCharactersByConfiguration();
                ComponentsList = new ObservableCollection<ComponentsByConfiguration>();
                SelectedComponent = new ComponentsByConfiguration();
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                AddSearchFamilyConfigurationButtonCommand = new RelayCommand(new Action<object>(AddSearchFamilyConfigurationButtonCommandAction));
                AddColorSimilarityButtonCommand = new RelayCommand(new Action<object>(ColorAddCommandAction));
                AppearanceCrossCommand = new RelayCommand(new Action<object>(DeleteColorCommandAction));
                AddSimilarCharactersCommand = new RelayCommand(new Action<object>(RefAddCommandAction));
                ReferenceCrossButtonCommand = new RelayCommand(new Action<object>(DeleteReferenceButtonCommandAction));
                AddComponentsButtonCommand = new RelayCommand(new Action<object>(ComponentAddCommandAction));
                DeleteComponentsCommand = new RelayCommand(new Action<object>(DeleteComponentsCommandAction));
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonCommandAction);
                ConfigurationClosedCommand = new DelegateCommand<object>(ConfigurationClosedCommandAction);
                ColorRowUpdatedCommnad = new DelegateCommand<object>(ColorRowUpdatedCommnadAction);
                ComponentsRowUpdatedCommnad = new DelegateCommand<object>(ComponentsRowUpdatedCommnadAction);
                ConfigClosingCommand = new DelegateCommand<object>(ConfigClosingCommandAction);
                //[GEOS2-5826][shweta.thube][22.07.2024]
                WindowHeader = System.Windows.Application.Current.FindResource("SCMSearchFiltersManagerTitle").ToString();
                CloseButtonCommand = new RelayCommand(new Action<object>(CloseButtonCommandAction));

                //[pramod.misal][GEOS2-5481][23.05.2024]
                if (GeosApplication.Instance.IsSCMPermissionAdmin || GeosApplication.Instance.IsSCMEditFiltersManager)
                {
                    IsSCMEditFiltersManager = false;
                    IsSCMEditFiltersManagerBtn = true;
                }
                else if (GeosApplication.Instance.IsSCMViewConfigurationPermission)
                {
                    IsSCMEditFiltersManager = true;
                    IsSCMEditFiltersManagerBtn = false;
                }
                else if (GeosApplication.Instance.IsSCMViewConfigurationPermission && GeosApplication.Instance.IsSCMEditFiltersManager)
                {
                    IsSCMEditFiltersManager = false;
                    IsSCMEditFiltersManagerBtn = true;
                }
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SearchFiltersManagerViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods       
        private void ConfigClosingCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ConfigClosingCommandAction()...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (SelectedConfiguration != null && isstart == false)
                {
                    if (SimilarCharactersList == null)
                        SelectedConfiguration.SimilarCharactersList = new List<SimilarCharactersByConfiguration>();
                    else
                        SelectedConfiguration.SimilarCharactersList = new List<SimilarCharactersByConfiguration>
                            (SimilarCharactersList.Select(i => (SimilarCharactersByConfiguration)i.Clone()).ToList());

                    ColorSimilarityList.ToList().RemoveAll(i => i.SelectedColor == null);

                    if (ColorSimilarityList == null)
                        SelectedConfiguration.ColorSimilarityList = new List<SimilarColorsByConfiguration>();
                    else
                        SelectedConfiguration.ColorSimilarityList = new List<SimilarColorsByConfiguration>
                            (ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()));

                    if (ComponentsList == null)
                        SelectedConfiguration.ComponentsList = new List<ComponentsByConfiguration>();
                    else
                        SelectedConfiguration.ComponentsList = new List<ComponentsByConfiguration>
                            (ComponentsList.Select(item => (ComponentsByConfiguration)item.Clone()).ToList());

                    //ListColor = new ObservableCollection<Data.Common.SCM.Color>(
                    //OriginalColorList.Select(i => (Data.Common.SCM.Color)i.Clone()).ToList());
                }
                isstart = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ConfigClosingCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        } //[rdixit][24.01.2024][GEOS2-5227]
        private void ConfigurationClosedCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ConfigurationClosedCommandAction()...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (SelectedConfiguration != null)
                {
                    //if (SelectedConfiguration.SimilarCharactersList == null)
                    //    SimilarCharactersList = new ObservableCollection<SimilarCharactersByConfiguration>();
                    //else
                    //    SimilarCharactersList = new ObservableCollection<SimilarCharactersByConfiguration>
                    //        (SelectedConfiguration.SimilarCharactersList.Select(i => (SimilarCharactersByConfiguration)i.Clone()).ToList());

                    if (SelectedConfiguration.ComponentsList == null)
                        ComponentsList = new ObservableCollection<ComponentsByConfiguration>();
                    else
                        ComponentsList = new ObservableCollection<ComponentsByConfiguration>
                            (SelectedConfiguration.ComponentsList.Select(item => (ComponentsByConfiguration)item.Clone()).ToList());

                    SelectedComponent = ComponentsList.FirstOrDefault();
                    if(SelectedColor == null)
                    SelectedColor = OriginalColorList.FirstOrDefault(i => i.Id == SelectedConfiguration.IdColor);
                    NumberOfPages = SelectedConfiguration.NoOfPages;
                    ReferencePagesToApply = SelectedConfiguration.RefPages;
                    AppearancePagesToApply = SelectedConfiguration.ColorPages;
                    DiameterAndSizePagesToApply = SelectedConfiguration.SizePages;
                    ComponentsPagesToApply = SelectedConfiguration.CompPages;
                    WaysMarginPagesApply = SelectedConfiguration.WaysPages;
                    Internal = SelectedConfiguration.Internal;
                    External = SelectedConfiguration.External;
                    Height = SelectedConfiguration.Height;
                    Length = SelectedConfiguration.Length;
                    Width = SelectedConfiguration.Width;
                    WayMargin = SelectedConfiguration.WayMargin;
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedColor"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ListColor"));
                    if (error != null)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method ConfigurationClosedCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ComponentAddCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ComponentAddCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (SelectedConfiguration != null)
                {
                    if (SelectedConfiguration.ComponentsList == null)
                        SelectedConfiguration.ComponentsList = new List<ComponentsByConfiguration>();

                    ComponentsByConfiguration comp = new ComponentsByConfiguration();
                    comp.ColorList = new ObservableCollection<Data.Common.SCM.Color>(ListColor.Select(i => (Data.Common.SCM.Color)i.Clone()).ToList());
                    comp.ComponentTypeList = new ObservableCollection<ComponentType>(ComponentTypeList.Select(i => (ComponentType)i.Clone()).ToList());
                    comp.ConditionList = new ObservableCollection<string>() { "IS", "NOT" };
                    comp.SelectedCondition = "IS";
                    comp.IdColor = null;
                    comp.IdType = null;
                    comp.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    comp.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    ComponentsList.Add(comp);
                    SelectedConfiguration.ComponentsList.Add(comp);
                }
                GeosApplication.Instance.Logger.Log("Method ComponentAddCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ComponentAddCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ColorAddCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ColorAddCommandAction()...", category: Category.Info, priority: Priority.Low);

                //[GEOS2-5437][rdixit][07.03.2024]
                SimilarColorsByConfiguration row = new SimilarColorsByConfiguration();
                row.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                row.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                row.IdColorA = SelectedColor.Id;
                if (OriginalColorList != null && ColorGroups != null)
                {
                    row.ColorList = new List<Data.Common.SCM.Color>(OriginalColorList.Where(i => !ColorGroups.Any(k => k?.Contains(i.Id) ?? false) && i.Id != 0).ToList());
                }
                else
                    row.ColorList = new List<Data.Common.SCM.Color>();
                if (SelectedColor != null)
                    row.ColorList.Remove(SelectedColor);
                ColorSimilarityList.Add(row);
                if (SelectedConfiguration != null)
                {
                    SelectedConfiguration.ColorSimilarityList = new List<SimilarColorsByConfiguration>(
                        ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList());
                }
                else
                {
                    SelectedConfiguration = new ConfigurationFamily();
                    SelectedConfiguration.ColorSimilarityList = new List<SimilarColorsByConfiguration>();
                }
                GeosApplication.Instance.Logger.Log("Method ColorAddCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ColorAddCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void RefAddCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RefAddCommandAction()...", category: Category.Info, priority: Priority.Low);

                SimilarCharactersByConfiguration row = new SimilarCharactersByConfiguration();
                row.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                row.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                if (SelectedConfiguration != null)
                {
                    if (SelectedConfiguration.SimilarCharactersList == null)
                        SelectedConfiguration.SimilarCharactersList = new List<SimilarCharactersByConfiguration>();
                    SelectedConfiguration.SimilarCharactersList.Add(row);
                    SimilarCharactersList.Add(row);
                }
                GeosApplication.Instance.Logger.Log("Method RefAddCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in RefAddCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (ConfigurationList != null)
                {
                    string error = EnableValidationAndGetError();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedColor"));

                    if (error != null)
                    {
                        return;
                    }
                    string colorDuplicate = string.Empty;
                    string refDuplicate = string.Empty;
                    string comDuplicate = string.Empty;
                    List<string> FinalMsg = new List<string>();
                    List<Tuple<int, int>> similarity = new List<Tuple<int, int>>();

                    foreach (var item in ConfigurationList)
                    {
                        item.ColorSimilarityList.ToList().RemoveAll(i => i.SelectedColor == null);
                        #region ColorSimilarity [GEOS2-5437][rdixit][07.03.2024]
                        if (item.ColorSimilarityList != null)
                        {
                            ColorSimilarityList = new ObservableCollection<SimilarColorsByConfiguration>(AllColorSimilarityList.Where(i => i.IdColorA == item.IdColor).ToList());
                            item.ColorSimilarityList = AllColorSimilarityList.Where(i => i.IdColorA == item.IdColor).ToList();
                            List<int> colors = new List<int>();
                            foreach (var it in item.ColorSimilarityList)
                            {
                                int? selectedColorId = it.SelectedColor?.Id;
                                if (selectedColorId != null)
                                {
                                    colors.Add((int)selectedColorId);
                                }
                            }
                            colors.Add(item.IdColor);
                            foreach (var it1 in colors)
                            {
                                foreach (var it2 in colors)
                                {
                                    if (it1 != it2 && !similarity.Any(i => i.Item1 == it1 && i.Item2 == it2))
                                    {
                                        similarity.Add(new Tuple<int, int>(it1, it2));
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Reference Appearance Components Duplicate
                        List<string> msg = new List<string>();
                        //if (item.SimilarCharactersList?.Count > 1)
                        //{
                        //    if (item.SimilarCharactersList.GroupBy(i => new { i.CharacterA, i.CharacterB, i.Description }).Any(g => g.Count() > 1))
                        //    {
                        //        refDuplicate += item.NameFamily + " ";
                        //        msg.Add("Reference");
                        //    }
                        //}

                        //if (item.ColorSimilarityList?.Count > 1)
                        //{
                        //    if (item.ColorSimilarityList.GroupBy(i => new { i.SelectedColor.Id }).Any(g => g.Count() > 1))
                        //    {
                        //        msg.Add("Appearance");
                        //        colorDuplicate += item.NameFamily + " ";
                        //    }
                        //}

                        if (item.ComponentsList?.Count > 1)
                        {
                            if (item.ComponentsList.GroupBy(i => new { i.SelectedColor?.Id, i.SelectedComponentType?.IdType, i.Reference, i.SelectedCondition }).Any(g => g.Count() > 1))
                            {
                                comDuplicate += item.NameFamily + " ";
                                msg.Add("Components");
                            }
                        }
                        if (msg?.Count > 0)
                        {
                            FinalMsg.Add(item.NameFamily + " " + System.Windows.Application.Current.FindResource("ConnSearchDuplicatMsg").ToString() + " " + string.Join(", ", msg));
                        }
                        #endregion
                    }
                    if (FinalMsg?.Count > 0)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(string.Join("\n", FinalMsg)), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    if (SimilarCharactersList?.Count > 1)
                    {
                        if (SimilarCharactersList.GroupBy(i => new { i.CharacterA, i.CharacterB }).Any(g => g.Count() > 1))
                        {
                            var msg = Application.Current.FindResource("ConnSearchDuplicatMsg").ToString() + " Reference";
                            CustomMessageBox.Show(msg, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }

                    if (ColorSimilarityList?.Count > 1)
                    {
                        if (ColorSimilarityList.GroupBy(i => new { i.SelectedColor.Id }).Any(g => g.Count() > 1))
                        {
                            var msg = Application.Current.FindResource("ConnSearchDuplicatMsg").ToString() + " Appearance";
                            CustomMessageBox.Show(msg, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }
                    ConfigurationList.ToList().ForEach(p => { p.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser; p.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser; });
                    #region Service Comments
                    //Service Methods SaveConfigurationsForSearchFilters_V2490 to SaveConfigurationsForSearchFilters_V2500 by [rdixit][GEOS2-5485][13.03.2024]
                    #endregion
                    bool isColorSimilarity = SCMService.AddColorsByConfiguration_V2490(similarity, GeosApplication.Instance.ActiveUser.IdUser);
                    IsSave = SCMService.SaveConfigurationsForSearchFilters_V2650(ConfigurationList.ToList());     //[rdixit][11.06.2025][GEOS2-6644]
                    if (IsSave && isColorSimilarity)
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("SearchConfigSaveSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    ColorSimilarityList = new ObservableCollection<SimilarColorsByConfiguration>(ConfigurationList.FirstOrDefault(x=>x.IdSearchConfiguration== IdSelectedColorList).ColorSimilarityList);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    
                    //RequestClose(null, null);GEOS2-5826 shweta.thube  After click on accept, the control of Search filters manager is automatically closed and this is not necessary. After click on Accept, the Search filters manager can remain opened and the user have to close it manually if he want to exit.
                }
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteComponentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteComponentsCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["SCMSearchFilterManagerComponentDetailsDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ComponentsList.Remove(SelectedComponent);
                    SelectedConfiguration.ComponentsList = new List<ComponentsByConfiguration>(ComponentsList.Select(i => (ComponentsByConfiguration)i.Clone()).ToList());
                }
                GeosApplication.Instance.Logger.Log("Method DeleteComponentsCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DeleteComponentsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteReferenceButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReferenceCrossButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["SCMSearchFilterManagerReferenceDetailsDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    SimilarCharactersList.Remove(SelectedSimilarCharacters);
                    SelectedConfiguration.SimilarCharactersList = new List<SimilarCharactersByConfiguration>(SimilarCharactersList.Select(i => (SimilarCharactersByConfiguration)i.Clone()).ToList());
                }
                GeosApplication.Instance.Logger.Log("Method ReferenceCrossButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ReferenceCrossButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteColorCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AppearanceCrossCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["SCMSearchFilterManagerAppearanceDetailsDeleteMessage"].ToString()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    //[GEOS2-5437][rdixit][07.03.2024]
                    var foundIndex = ColorGroups.FindIndex(k => k.Contains(SelectedColorSimilarity.IdColorB));
                    if (foundIndex != -1)
                    {
                        ColorGroups[foundIndex] = ColorGroups[foundIndex].ToList()?.Where(i => i != SelectedColorSimilarity.IdColorB).ToList();
                    }
                    AllColorSimilarityList.RemoveAll(i => i.IdColorA == SelectedColorSimilarity.IdColorB || i.IdColorB == SelectedColorSimilarity.IdColorB);
                    ColorSimilarityList.Remove(SelectedColorSimilarity);
                    ColorSimilarityList1 = ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList();
                    SelectedConfiguration.ColorSimilarityList = new List<SimilarColorsByConfiguration>(ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList());
                }
                GeosApplication.Instance.Logger.Log("Method AppearanceCrossCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method AppearanceCrossCommandAction()...", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
                FillColorSimilarityList();
                FillColor();
                FillComponentType();
                FillConfigurationList();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method Init()...", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
        }
        private void FillConfigurationList()
        {
            try
            {
                //[GEOS2-5437][rdixit][07.03.2024]
                GeosApplication.Instance.Logger.Log("Method FillConfigurationList()...Started Execution", category: Category.Info, priority: Priority.Low);
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
                ConfigurationList = new ObservableCollection<ConfigurationFamily>(SCMService.GetConfigurationsForSearchFilters_V2490());
                if (ConfigurationList != null)
                {
                    foreach (var item in ConfigurationList)
                    {
                        if (OriginalColorList != null && ColorGroups != null && AllColorSimilarityList != null)
                        {
                            AllColorSimilarityList.ForEach(i =>
                            {
                                i.ColorList = OriginalColorList.Where(p => !ColorGroups.Any(k => k.Contains((int)p.Id)) && p.Id != 0).ToList();
                            });
                        }
                        //AllColorSimilarityList.ForEach(i => i.ColorList = OriginalColorList.Where(p => !ColorGroups.Any(k => k.Contains(p.Id.ToString()))).ToList());
                        item.ColorSimilarityList = AllColorSimilarityList.Where(i => i.IdColorA == item.IdColor).ToList();
                        if (item.ComponentsList != null)
                            item.ComponentsList.ForEach(i =>
                            {
                                i.ColorList = new ObservableCollection<Data.Common.SCM.Color>(ListColor);
                                i.SelectedColor = i.ColorList.FirstOrDefault(j => j.Id == i.IdColor);
                                i.ComponentTypeList = new ObservableCollection<ComponentType>(ComponentTypeList);
                                i.SelectedComponentType = i.ComponentTypeList.FirstOrDefault(j => j.IdType == i.IdType);
                                i.ConditionList = new ObservableCollection<string>() { "IS", "NOT" };
                                i.SelectedCondition = i.ConditionList.FirstOrDefault(j => j == i.SelectedCondition);
                            });
                    }

                    SelectedConfiguration = ConfigurationList.FirstOrDefault();
                    if (SelectedConfiguration.ColorSimilarityList != null)
                        ColorSimilarityList = new ObservableCollection<SimilarColorsByConfiguration>(SelectedConfiguration.ColorSimilarityList);
                    if (SelectedConfiguration.ComponentsList != null)
                        ComponentsList = new ObservableCollection<ComponentsByConfiguration>(SelectedConfiguration.ComponentsList);
                    if (SelectedConfiguration.SimilarCharactersList != null)
                        SimilarCharactersList = new ObservableCollection<SimilarCharactersByConfiguration>(SelectedConfiguration.SimilarCharactersList);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method FillConfigurationList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillConfigurationList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillConfigurationList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillConfigurationList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillComponentType()
        {
            try
            {
                ComponentTypeList = new ObservableCollection<ComponentType>(SCMService.GetAllComponentTypes());
                GeosApplication.Instance.Logger.Log(string.Format("Method FillColor()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillComponentType() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillComponentType() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillComponentType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillColor()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillColor()....executed successfully", category: Category.Info, priority: Priority.Low);
                ListColor = new ObservableCollection<Data.Common.SCM.Color>(SCMService.GetAllColors(language));
                OriginalColorList = new ObservableCollection<Data.Common.SCM.Color>(SCMService.GetAllColors(language));
                Data.Common.SCM.Color colorToRemove = ListColor.FirstOrDefault(x => x.Name == "None");
                if (colorToRemove != null)//[Sudhir.Jangra][GEOS2-4963]
                {
                    ListColor.Remove(colorToRemove);
                }
                SelectedColor = null;

                GeosApplication.Instance.Logger.Log(string.Format("Method FillColor()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillColor() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #region[GEOS2-5437][rdixit][07.03.2024]
        HashSet<int> FindConnectedColors(int color, HashSet<int> visited = null)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FindConnectedColors()....Started Execution", category: Category.Info, priority: Priority.Low);
                if (visited == null)
                    visited = new HashSet<int>();

                if (!visited.Contains(color))
                {
                    visited.Add(color);
                    foreach (var connectedColor in ColorConnections[color])
                    {
                        FindConnectedColors(connectedColor, visited);
                    }
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method FindConnectedColors()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FindConnectedColors() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return visited;
        }
        private void FillColorSimilarityList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillColorSimilarityList()....Started Execution", category: Category.Info, priority: Priority.Low);
                AllColorSimilarityList = SCMService.GetSimilarColorsByConfiguration();
                if (AllColorSimilarityList?.Count > 0)
                {
                    foreach (var pair in allColorSimilarityList)
                    {
                        int colorA = pair.IdColorA;
                        int colorB = pair.IdColorB;

                        if (!ColorConnections.ContainsKey(colorA))
                            ColorConnections[colorA] = new HashSet<int>();
                        if (!ColorConnections.ContainsKey(colorB))
                            ColorConnections[colorB] = new HashSet<int>();

                        ColorConnections[colorA].Add(colorB);
                        ColorConnections[colorB].Add(colorA);
                    }


                    HashSet<int> visitedColors = new HashSet<int>();

                    // Find color groups
                    foreach (var color in ColorConnections.Keys)
                    {
                        if (!visitedColors.Contains(color))
                        {
                            HashSet<int> connectedColors = FindConnectedColors(color);
                            ColorGroups.Add(connectedColors.OrderBy(x => x).ToList());
                            visitedColors.UnionWith(connectedColors);
                        }
                    }
                    ColorGroups = ColorGroups?.Select(group => group.Distinct().OrderBy(item => item).ToList()).ToList();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method FillColorSimilarityList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillColorSimilarityList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        private void AddSearchFamilyConfigurationButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddSearchFamilyConfigurationButtonCommandAction()....Started Execution", category: Category.Info, priority: Priority.Low);
                AddSearchFamilyConfigurationView addSearchFamilyConfigurationView = new AddSearchFamilyConfigurationView();
                AddSearchFamilyConfigurationViewModel addSearchFamilyConfigurationViewModel = new AddSearchFamilyConfigurationViewModel();
                EventHandler handle = delegate { addSearchFamilyConfigurationView.Close(); };
                addSearchFamilyConfigurationViewModel.RequestClose += handle;
                addSearchFamilyConfigurationView.DataContext = addSearchFamilyConfigurationViewModel;
                addSearchFamilyConfigurationViewModel.Init(ConfigurationList);
                addSearchFamilyConfigurationView.ShowDialogWindow();

                if (addSearchFamilyConfigurationViewModel.IsSave)
                {
                    ConfigurationFamily newConfigurationFamily = new ConfigurationFamily();
                    ConfigurationFamily default1 = ConfigurationList.FirstOrDefault(i => i.IdFamily == 0);
                    newConfigurationFamily.IdFamily = addSearchFamilyConfigurationViewModel.SelectedFamily.Id;
                    newConfigurationFamily.NameFamily = addSearchFamilyConfigurationViewModel.SelectedFamily.Name;
                    newConfigurationFamily.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    newConfigurationFamily.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                    newConfigurationFamily.Height = default1.Height;
                    newConfigurationFamily.Length = default1.Length;
                    newConfigurationFamily.Width = default1.Width;
                    newConfigurationFamily.Internal = default1.Internal;
                    newConfigurationFamily.External = default1.External;
                    newConfigurationFamily.ColorPages = default1.ColorPages;
                    newConfigurationFamily.CompPages = default1.CompPages;
                    newConfigurationFamily.NoOfPages = default1.NoOfPages;
                    newConfigurationFamily.RefPages = default1.RefPages;
                    newConfigurationFamily.SizePages = default1.SizePages;
                    newConfigurationFamily.WaysPages = default1.WaysPages;
                    newConfigurationFamily.WayMargin = default1.WayMargin;
                    newConfigurationFamily.IdColor = default1.IdColor;

                    if (default1.ColorSimilarityList != null)
                    {
                        newConfigurationFamily.ColorSimilarityList = new List<SimilarColorsByConfiguration>(default1.ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList());
                        ColorSimilarityList = new ObservableCollection<SimilarColorsByConfiguration>(default1.ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList());
                    }
                    if (default1.ComponentsList != null)
                    {
                        newConfigurationFamily.ComponentsList = new List<ComponentsByConfiguration>(default1.ComponentsList.Select(i => (ComponentsByConfiguration)i.Clone()).ToList());
                        ComponentsList = new ObservableCollection<ComponentsByConfiguration>(default1.ComponentsList.Select(i => (ComponentsByConfiguration)i.Clone()).ToList());
                    }
                    if (default1.SimilarCharactersList != null)
                    {
                        newConfigurationFamily.SimilarCharactersList = new List<SimilarCharactersByConfiguration>(default1.SimilarCharactersList.Select(i => (SimilarCharactersByConfiguration)i.Clone()).ToList());
                        SimilarCharactersList = new ObservableCollection<SimilarCharactersByConfiguration>(default1.SimilarCharactersList.Select(i => (SimilarCharactersByConfiguration)i.Clone()).ToList());
                    }
                    int maxidChar = ConfigurationList.Count == 0 ? 0 : ConfigurationList.Select(i => i.IdSearchConfiguration).Max();

                    if (newConfigurationFamily.IdSearchConfiguration == 0)
                        newConfigurationFamily.IdSearchConfiguration = ++maxidChar;

                    ConfigurationList.Add(newConfigurationFamily);
                    SelectedConfiguration = ConfigurationList.FirstOrDefault(x => x.IdFamily == addSearchFamilyConfigurationViewModel.SelectedFamily.Id);
                }


                GeosApplication.Instance.Logger.Log("Method AddSearchFamilyConfigurationButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddSearchFamilyConfigurationButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-5826][shweta.thube][20.08.2024]
        private void CancelButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }      
        private void ColorRowUpdatedCommnadAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ColorRowUpdatedCommnadAction()...Started Execution", category: Category.Info, priority: Priority.Low);
                if (SelectedColorSimilarity != null)
                {
                    #region If color is edited in between the combo box (Not add or delete)
                    if (ColorSimilarityList1?.Count == ColorSimilarityList?.Count)
                    {
                        for (int c = 0; c < ColorSimilarityList1?.Count; c++)
                        {
                            if (ColorSimilarityList1[c].SelectedColor.Id != ColorSimilarityList[c].SelectedColor.Id)
                            {
                                var foundIndex = ColorGroups.FindIndex(k => k.Contains(ColorSimilarityList1[c].SelectedColor.Id));
                                if (foundIndex != -1)
                                {
                                    ColorGroups[foundIndex] = ColorGroups[foundIndex].ToList()?.Where(i => i != SelectedColorSimilarity.IdColorB).ToList();
                                }
                                AllColorSimilarityList.RemoveAll(i => i.IdColorA == SelectedColorSimilarity.IdColorB || i.IdColorB == SelectedColorSimilarity.IdColorB);
                            }
                        }
                    }
                    #endregion
                    ColorSimilarityList1 = ColorSimilarityList.Select(i => (SimilarColorsByConfiguration)i.Clone()).ToList();
                    Data.Common.SCM.Color col = (Data.Common.SCM.Color)SelectedColorSimilarity.SelectedColor;
                    SelectedColorSimilarity.IdColorB = col.Id;
                    if (col != null)
                    {
                        var ind = ColorGroups?.FindIndex(k => k.Contains(SelectedColor.Id)) ?? -1;
                        if (ind == -1 && SelectedColor != null)
                        {
                            List<int> ar = new List<int>();
                            ar.Add(SelectedColor.Id);
                            ar.Add(col.Id);
                            ColorGroups.Add(ar);
                        }
                        var foundIndex = ColorGroups?.FindIndex(k => k.Contains(SelectedColor.Id)) ?? -1;
                        List<int> arr = ColorGroups[foundIndex];
                        // Ensure ColorSimilarityList, ListColor, and SelectedColor are not null before using them
                        if (ColorSimilarityList != null && ListColor != null && SelectedColor != null)
                        {
                            ColorGroups[foundIndex] = ColorSimilarityList.Select(i => (int)i.SelectedColor.Id).ToList();
                            ColorGroups[foundIndex].Add(SelectedColor.Id);
                            if (AllColorSimilarityList == null)
                                AllColorSimilarityList = new List<SimilarColorsByConfiguration>();

                            #region all new combination
                            foreach (var item in arr)
                            {
                                if (!AllColorSimilarityList.Any(i => i.IdColorA == col.Id && i.IdColorB == item))
                                {
                                    AllColorSimilarityList.Add(new SimilarColorsByConfiguration
                                    {
                                        IdColorA = col.Id,
                                        IdColorB = item,
                                        SelectedColor = ListColor.FirstOrDefault(i => i.Id == item)
                                    });
                                }
                                if (!AllColorSimilarityList.Any(i => i.IdColorA == item && i.IdColorB == col.Id))
                                {
                                    AllColorSimilarityList.Add(new SimilarColorsByConfiguration
                                    {
                                        IdColorA = item,
                                        IdColorB = col.Id,
                                        SelectedColor = ListColor.FirstOrDefault(i => i.Id == col.Id)
                                    });
                                }

                            }
                            #endregion

                            // Ensure OriginalColorList and ColorGroups are not null before proceeding
                            if (OriginalColorList != null && ColorGroups != null)
                            {
                                AllColorSimilarityList.ForEach(i => i.ColorList = OriginalColorList.Where(p => !ColorGroups.Any(k => k.Contains(p.Id)) && p.Id != 0).ToList());
                            }
                        }
                    }
                }
                SelectedConfiguration.ColorSimilarityList = new List<SimilarColorsByConfiguration>(ColorSimilarityList?.Select(i =>
                (SimilarColorsByConfiguration)i.Clone()).ToList() ?? new List<SimilarColorsByConfiguration>());
                GeosApplication.Instance.Logger.Log("Method ColorRowUpdatedCommnadAction()...Execution Sucessfull", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ColorRowUpdatedCommnadAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ComponentsRowUpdatedCommnadAction(object obj)
        {
            try
            {
                if (obj != null)
                    SelectedConfiguration.ComponentsList = new List<ComponentsByConfiguration>(ComponentsList.Select(i => (ComponentsByConfiguration)i.Clone()).ToList());
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ComponentsRowUpdatedCommnadAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[GEOS2-5826][shweta.thube][22.07.2024]
        private void CloseButtonCommandAction(object obj)
        {
            RequestClose(null, null);
        }
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

        #region Validation
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;

            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => ComponentsPagesToApply)] +
                    me[BindableBase.GetPropertyName(() => NumberOfPages)] +
                    me[BindableBase.GetPropertyName(() => ReferencePagesToApply)] +
                    me[BindableBase.GetPropertyName(() => AppearancePagesToApply)] +
                    me[BindableBase.GetPropertyName(() => DiameterAndSizePagesToApply)] +
                    me[BindableBase.GetPropertyName(() => SelectedColor)];

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
                string componentPagesToApply = BindableBase.GetPropertyName(() => ComponentsPagesToApply);
                string referencePagesToApply = BindableBase.GetPropertyName(() => ReferencePagesToApply);
                string appearancePagesToApply = BindableBase.GetPropertyName(() => AppearancePagesToApply);
                string diameterAndSizePagesToApply = BindableBase.GetPropertyName(() => DiameterAndSizePagesToApply);
                string waysPagesToApply = BindableBase.GetPropertyName(() => WaysMarginPagesApply);
                string selectedColor = BindableBase.GetPropertyName(() => SelectedColor);

                if (columnName == componentPagesToApply)
                    return SCMSearchFilterManagerValidation.GetErrorMessage(componentPagesToApply, NumberOfPages, ComponentsPagesToApply);

                if (columnName == referencePagesToApply)
                    return SCMSearchFilterManagerValidation.GetErrorMessage(referencePagesToApply, NumberOfPages, ReferencePagesToApply);

                if (columnName == appearancePagesToApply)
                    return SCMSearchFilterManagerValidation.GetErrorMessage(appearancePagesToApply, NumberOfPages, AppearancePagesToApply);

                if (columnName == diameterAndSizePagesToApply)
                    return SCMSearchFilterManagerValidation.GetErrorMessage(diameterAndSizePagesToApply, NumberOfPages, DiameterAndSizePagesToApply);

                if (columnName == waysPagesToApply)
                    return SCMSearchFilterManagerValidation.GetErrorMessage(waysPagesToApply, NumberOfPages, WaysMarginPagesApply);

                if (columnName == selectedColor)
                    return SCMSearchFilterManagerValidation.GetErrorMessage(selectedColor, ColorSimilarityList.Count, SelectedColor);
                return null;
            }
        }
        #endregion
    }
}
