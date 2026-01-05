using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.WMS;
using Emdep.Geos.Modules.Warehouse.Views;
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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class AddLocationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region TaskLog
        /// <summary>
        /// [WMS-M050-13] Add and edit warehouse locations in locations configuration[29/10/2018][adadibathina]
        /// WMS M054-14    New way to select easily the parent location u took this one [adadibathina]
        /// WMS	M056-11	Set automatic position to the warehouse locations [adadibathina]
        /// </summary>
        #endregion

        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private WarehouseLocation newLocation;
        private WarehouseLocation tempLocation;
        private ObservableCollection<WarehouseLocation> warehouseLocationList;
        private WarehouseLocation selectedParent;
        private int position = 1;
        private string locationName;
        private string regexp;
        string error;
        private string htmlColor;
        private bool inUse;
        private sbyte? isLeaf;
        ////private bool isUpdated;
        private string parentValidationMessage;
        private int selectedDirection;
        private int selectedManufacturer;
        private List<FileDetail> mapFiles;
        private FileDetail selectedMapFile;
        private bool isCoordinatesNull;

        private double? width;
        private double? height;
        private double? latitude;
        private double? longitude;
        private int maxPosition = 1;
        string fullName = string.Empty;
      

        #endregion

        #region Properties
        public List<LookupValue> DirectionList { get; set; }

        public List<Manufacturer> ManufacturerList { get; set; }

        public Visibility ManufacturerVisibility { get; set; }
        public string RegExp
        {
            get
            {
                return regexp;
            }

            set
            {
                regexp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegExp"));
            }
        }

        public string WindowHeader
        {
            get { return windowHeader; }
            set { windowHeader = value; OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader")); }
        }
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; OnPropertyChanged(new PropertyChangedEventArgs("isNew")); }
        }
        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; OnPropertyChanged(new PropertyChangedEventArgs("IsSave")); }
        }

        public WarehouseLocation NewLocation
        {
            get { return newLocation; }
            set { newLocation = value; OnPropertyChanged(new PropertyChangedEventArgs("NewLocation")); }
        }


        public WarehouseLocation TempLocation
        {
            get { return tempLocation; }
            set { tempLocation = value; }
        }

        public ObservableCollection<WarehouseLocation> WarehouseLocationList
        {
            get { return warehouseLocationList; }
            set { warehouseLocationList = value; OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationList")); }
        }

        public WarehouseLocation SelectedParent
        {
            get { return selectedParent; }
            set { selectedParent = value;
                if (selectedParent != null)
                {


                    if (selectedParent.FullName != "---")
                    {
                        if (IsRangeofLocation == true)
                        {
                            if (selectedParent.Parent == 0)
                            {
                                IsVisibleRangeofLocationLevel = Visibility.Collapsed;

                                IsVisibleRangeofLocation = Visibility.Visible;
                                InUse = true;
                                IsLeaf = 0;
                               // IsReadOnly = false;
                            }
                            else
                            {
                                IsVisibleRangeofLocationLevel = Visibility.Visible;
                                IsVisibleRangeofLocation = Visibility.Visible;
                                InUse = true;
                                IsLeaf = 1;
                              //  IsReadOnly = true;
                            }

                        }

                    }
                    else
                    {
                        if (IsRangeofLocation == true)
                        {
                            IsVisibleRangeofLocation = Visibility.Collapsed;
                        }
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedParent")); }

        }

        public int Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged(new PropertyChangedEventArgs("Position")); }

        }

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; OnPropertyChanged(new PropertyChangedEventArgs("LocationName")); }
        }
        public string HTMLColor
        {
            get { return htmlColor; }
            set { htmlColor = value; OnPropertyChanged(new PropertyChangedEventArgs("HTMLColor")); }
        }

        public bool InUse
        {
            get { return inUse; }
            set { inUse = value; OnPropertyChanged(new PropertyChangedEventArgs("InUse")); }
        }
        public sbyte? IsLeaf
        {
            get { return isLeaf; }
            set { isLeaf = value; OnPropertyChanged(new PropertyChangedEventArgs("IsLeaf")); }
        }

        public int SelectedDirection
        {
            get
            {
                return selectedDirection;
            }

            set
            {
                selectedDirection = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDirection"));
            }
        }

        public int SelectedManufacturer
        {
            get
            {
                return selectedManufacturer;
            }

            set
            {
                selectedManufacturer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedManufacturer"));
            }
        }

        public List<FileDetail> MapFiles
        {
            get { return mapFiles; }
            set { mapFiles = value; }
        }

        public FileDetail SelectedMapFile
        {
            get { return selectedMapFile; }
            set
            {
                selectedMapFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedMapFile"));
            }
        }

        public bool IsCoordinatesNull
        {
            get { return isCoordinatesNull; }
            set
            {
                isCoordinatesNull = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCoordinatesNull"));
            }
        }

        public int MaxPosition
        {
            get { return maxPosition; }
            set
            {
                if (value == 0)
                {
                    value = 1;
                }
                maxPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaxPosition"));
            }
        }

        #region [pallavi.jadhav][8 5 2025][GEOS2-6823]

        private Visibility isVisibleRangeofLocationLevel;
        public Visibility IsVisibleRangeofLocationLevel
        {
            get { return isVisibleRangeofLocationLevel; }
            set
            {

                isVisibleRangeofLocationLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleRangeofLocationLevel"));
            }
        }
        private Visibility isVisibleRangeofLocation;
        public Visibility IsVisibleRangeofLocation
        {
            get { return isVisibleRangeofLocation; }
            set
            {

                isVisibleRangeofLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleRangeofLocation"));
            }
        }

        private Visibility isVisibleSingleLocation;
        public Visibility IsVisibleSingleLocation
        {
            get { return isVisibleSingleLocation; }
            set
            {

                isVisibleSingleLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsVisibleSingleLocation"));
            }
        }

        private bool isReadOnly;
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value; OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnly")); }
        }
        private bool isSingleLocation;
        public bool IsSingleLocation
        {
            get { return isSingleLocation; }
            set
            {

                isSingleLocation = value;
                if (IsRangeofLocation==true)
                {
                    IsVisibleSingleLocation = Visibility.Collapsed;
                    IsVisibleRangeofLocation = Visibility.Collapsed;
                    IsVisibleRangeofLocationLevel = Visibility.Collapsed;
                    if (SelectedParent!=null)
                    {
                        selectedParent.FullName = "---";
                    }

                    //if (TempLocation != null)
                    //{
                    //    if (TempLocation.Parent == 0)
                    //    {
                    //        SelectedParent.FullName = TempLocation.Name;
                    //        IsVisibleRangeofLocation = Visibility.Visible;
                            
                    //    }
                    //    else
                    //    {
                    //        SelectedParent.FullName = TempLocation.FullName;
                    //        IsVisibleRangeofLocationLevel = Visibility.Visible;
                    //        IsVisibleRangeofLocation = Visibility.Visible;
                    //    }
                        
                    //}

                }
                else
                {
                    IsVisibleSingleLocation = Visibility.Visible;
                    IsVisibleRangeofLocation = Visibility.Collapsed;
                    IsVisibleRangeofLocationLevel = Visibility.Collapsed;
                    if (SelectedParent != null)
                    {
                       
                        selectedParent.FullName = "---";
                    }
                }


                OnPropertyChanged(new PropertyChangedEventArgs("IsSingleLocation"));
            }
        }

        private bool isRangeofLocation;
        public bool IsRangeofLocation
        {
            get { return isRangeofLocation; }
            set
            {
                isRangeofLocation = value;
                if (IsSingleLocation == true)
                {
                    IsVisibleSingleLocation = Visibility.Visible;
                    IsVisibleRangeofLocation = Visibility.Collapsed;
                    IsVisibleRangeofLocationLevel = Visibility.Collapsed;
                    //if (SelectedParent != null)
                    //{
                    //    selectedParent.FullName = "---";
                    //}
                    //if (TempLocation != null)
                    //{
                    //    if (TempLocation.Parent == 0)
                    //        SelectedParent.FullName = TempLocation.Name;
                    //    else
                    //        SelectedParent.FullName = WarehouseLocationMainList.FirstOrDefault(x => x.IdWarehouseLocation == TempLocation.Parent).FullName;

                    //}
                }
                else
                {
                    

                    IsVisibleRangeofLocationLevel = Visibility.Collapsed;
                    IsVisibleSingleLocation = Visibility.Collapsed;
                    IsVisibleRangeofLocation = Visibility.Collapsed;
                    if (SelectedParent != null)
                    {
                        selectedParent.FullName = "---";
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsRangeofLocation"));
            }
        }
        private ObservableCollection<AlphabetItems> alphabetList;
        public ObservableCollection<AlphabetItems> AlphabetList
        {
            get { return alphabetList; }
            set
            {
                alphabetList = value;
               
                OnPropertyChanged(new PropertyChangedEventArgs("AlphabetList"));
            }
        }

        private string selectedLevel;
        public string SelectedLevel
        {
            get { return selectedLevel; }
            set
            {
                selectedLevel = value;
               
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLevel"));
            }
        }
        private string from;
        public string From
        {
            get { return from; }
            set
            {
                from = value;

                OnPropertyChanged(new PropertyChangedEventArgs("From"));
            }
        }
        private string to;
        public string To
        {
            get { return to; }
            set
            {
                to = value;

                OnPropertyChanged(new PropertyChangedEventArgs("To"));
            }
        }

        private List<RangeItems> rangeItemsList;
        public List<RangeItems> RangeItemsList
        {
            get { return rangeItemsList; }
            set
            {
                rangeItemsList = value;

                OnPropertyChanged(new PropertyChangedEventArgs("RangeItemsList"));
            }
        }

        private ObservableCollection<WarehouseLocation> warehouseLocationMainList;
        public ObservableCollection<WarehouseLocation> WarehouseLocationMainList
        {
            get { return warehouseLocationMainList; }
            set
            {
                warehouseLocationMainList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WarehouseLocationMainList"));
            }
        }

        private WarehouseLocation selectedWarehouseLocation;
        public WarehouseLocation SelectedWarehouseLocation
        {
            get { return selectedWarehouseLocation; }
            set
            {
                selectedWarehouseLocation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWarehouseLocation"));
            }
        }
        private Visibility isEdit;
        public Visibility IsEdit
        {
            get { return isEdit; }
            set
            {
                isEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEdit"));
            }
        }
        #endregion
        #endregion

        #region public Events
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

        #region  ICommand
        public ICommand ColorValueChangingCommand { get; set; }
        public ICommand LocationViewCancelButtonCommand { get; set; }
        public ICommand AddLocationAcceptCommand { get; set; }
        public ICommand ParentSelectionChangedCommand { get; set; }
        //public ICommand IsLeafChangedCommand { get; set; }
        public ICommand AddMapLocatorCommand { get; set; }

        #endregion

        #region Constructor

        public AddLocationViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor AddLocationViewModel()...", category: Category.Info, priority: Priority.Low);

            LocationViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            AddLocationAcceptCommand = new RelayCommand(new Action<object>(AddLocationInformation));
            ParentSelectionChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(ParentAndNameValidation);
            //IsLeafChangedCommand = new DelegateCommand<EditValueChangedEventArgs>(ParentAndNameValidation);
            AddMapLocatorCommand = new DelegateCommand<object>(new Action<object>((obj) => { AddMapLocatorCommandAction(obj); }));

            FillParents();
            FillDirectionList();
            FillMapFiles();
            FillManufacturerList();
            #region [pallavi.jadhav][09 05 2025][GEOS2-6823]
            IsRangeofLocation = false;
            IsSingleLocation = true;
             AlphabetList = new ObservableCollection<AlphabetItems>( Enumerable.Range('A', 26)
         .Select(c => new AlphabetItems
         {
             AsciiValue = c,
             Character = ((char)c).ToString()
         }).ToList());
            AlphabetList.Insert(0, new AlphabetItems { AsciiValue = -1, Character = "---" });


            SelectedLevel = AlphabetList[0].Character;
            #endregion
            GeosApplication.Instance.Logger.Log("Constructor AddLocationViewModelMethod  executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region Validation
        bool allowValidation = false;
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
                IDataErrorInfo DataError = (IDataErrorInfo)this;
                string error = DataError[BindableBase.GetPropertyName(() => LocationName)]
                    //+ DataError[BindableBase.GetPropertyName(() => HTMLColor)]
                    + DataError[BindableBase.GetPropertyName(() => SelectedParent)]
                    + DataError[BindableBase.GetPropertyName(() => LocationName)]
                    + DataError[BindableBase.GetPropertyName(() => Position)]
                 + DataError[BindableBase.GetPropertyName(() => SelectedLevel)]
                    + DataError[BindableBase.GetPropertyName(() => From)]
                    + DataError[BindableBase.GetPropertyName(() => To)];
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
                string position = BindableBase.GetPropertyName(() => Position);
                if (columnName == position)
                {

                    if (Position > MaxPosition || Position == 0)
                    {
                        return LocationValidation.GetErrorMessage(position, this,null,null);
                    }
                }
                string locationName = string.Empty;
                if (IsRangeofLocation)
                {
                    if (selectedParent.Parent != 0)
                    {
                        locationName = BindableBase.GetPropertyName(() => SelectedLevel);
                        if (columnName == locationName)
                        {
                            return LocationValidation.GetErrorMessage(locationName, SelectedLevel, null, null);
                        }
                    }
                    string from = BindableBase.GetPropertyName(() => From);
                    string to = BindableBase.GetPropertyName(() => To);
                    if (columnName== from)
                    {
                        
                        return LocationValidation.GetErrorMessage(from, From, to, To);
                    }
                    if (columnName == to)
                    {
                        return LocationValidation.GetErrorMessage(from, From, to, To);
                    }
                   
                }
                else
                {
                    locationName = BindableBase.GetPropertyName(() => LocationName);
                }
                if (columnName == locationName)
                {
                    //return LocationValidation.GetErrorMessage(locationName, LocationName);
                    if (string.IsNullOrEmpty(LocationName))
                        return LocationValidation.GetErrorMessage(locationName, LocationName,null,null);
                    else
                    {
                        bool val = CheckRegExpForLocationName();
                        if (!val)
                            return "Not Valid Location name.";
                    }
                }


                string parent = BindableBase.GetPropertyName(() => SelectedParent);
                if (columnName == parent)
                {
                    return parentValidationMessage;
                }
                string locatiname = BindableBase.GetPropertyName(() => LocationName);
                if (columnName == htmlColor)
                {
                    // return LocationValidation.GetErrorMessage(htmlColor, HTMLColor);
                }

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to AddLocation Information
        /// [001][vsana][23-11-2020][GEOS2-2426]AutoSort for the new locations created.
        /// </summary>

        public void AddLocationInformation(object obj)
        {
            //if (CheckRegExpForLocationName())
            //{
                GeosApplication.Instance.Logger.Log("Method AddLocationViewModel Method AddLocationInformation ()...", category: Category.Info, priority: Priority.Low);
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

            if (IsRangeofLocation==true)
            {
                //ParentAndNameValidation(new EditValueChangedEventArgs("", ""));
                error = EnableValidationAndGetError();
                if (selectedParent.Parent != 0)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedLevel"));
                }
                // PropertyChanged(this, new PropertyChangedEventArgs("HTMLColor"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedParent"));
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                PropertyChanged(this, new PropertyChangedEventArgs("From"));
                PropertyChanged(this, new PropertyChangedEventArgs("To"));
                if (!string.IsNullOrEmpty(SelectedLevel))
                {
                    SelectedLevel = SelectedLevel.Trim();
                    if (SelectedLevel!=""&& SelectedLevel!="---")
                    {
                        SelectedLevel = AlphabetList[Convert.ToInt32(SelectedLevel)].Character;
                    }
                   // SelectedLevel = AlphabetList[Convert.ToInt32(SelectedLevel)].Character;
                    if (RangeItemsList==null)
                    {
                        RangeItemsList = new List<RangeItems>();
                    }
                    var lastPosition = WarehouseLocationMainList.Where(a => a.Parent == selectedParent.IdWarehouseLocation).OrderByDescending(b => b.FullName).FirstOrDefault();
                    if (selectedParent.Parent != 0 && selectedParent.Parent != -1)
                    {
                        if (SelectedLevel!=""&& SelectedLevel != "---")
                        {
                            int start = Convert.ToInt32(From);
                            int end = Convert.ToInt32(To);
                            int basePosition = 0;
                            if (lastPosition != null)
                            {
                                basePosition = Convert.ToInt32(lastPosition.Position);
                            }
                            RangeItemsList = Enumerable.Range(start, end - start + 1)
                                .Select((i, index) => new RangeItems
                                {
                                    Lavel = $"{SelectedLevel}{i.ToString("D2")}",
                                    Range = i,
                                    ConcatLevel = $"{selectedParent.FullName}-{SelectedLevel}{i.ToString("D2")}",
                                    Position = basePosition + index + 1 // incrementing with each item
                                })
                                .ToList();
                        }
                        else
                        {

                            int start = Convert.ToInt32(From);
                            int end = Convert.ToInt32(To);
                            int basePosition = 0;
                            if (lastPosition != null)
                            {
                                basePosition = Convert.ToInt32(lastPosition.Position);
                            }

                            RangeItemsList = Enumerable.Range(start, end - start + 1)
                                .Select((i, index) => new RangeItems
                                {
                                    Lavel = $"{i.ToString("D2")}",
                                    Range = i,
                                    ConcatLevel = $"{selectedParent.FullName}-{i.ToString("D2")}",
                                    Position = basePosition + index + 1 // incrementing with each item
                                })
                                .ToList();
                        }
                       
                        //int start = Convert.ToInt32(From);
                        //int end = Convert.ToInt32(To);
                        //int basePosition = 0;
                        //if (lastPosition != null)
                        //{
                        //    basePosition = Convert.ToInt32(lastPosition.Position);
                        //}

                        ////RangeItemsList = Enumerable.Range(start, end - start + 1)
                        ////    .Select((i, index) => new RangeItems
                        ////    {
                        ////        Lavel = $"{SelectedLevel}{i.ToString("D2")}",
                        ////        Range = i,
                        ////        ConcatLevel = $"{SelectedLevel}-{i.ToString("D2")}",
                        ////        Position = basePosition + index + 1 // incrementing with each item
                        ////    })
                        ////    .ToList();

                        //RangeItemsList = Enumerable.Range(start, end - start + 1)
                        //   .Select((i, index) => new RangeItems
                        //   {
                        //       Lavel = $"{SelectedLevel}{i.ToString("D2")}",
                        //       Range = i,
                        //       ConcatLevel = $"{selectedParent.FullName}-{i.ToString("D2")}",
                        //       Position = basePosition + index + 1 // incrementing with each item
                        //   })
                        //   .ToList();

                        //RangeItemsList = Enumerable.Range(Convert.ToInt32(From), Convert.ToInt32(To) - Convert.ToInt32(From) + 1)
                        //                         .Select(i => new RangeItems
                        //                         {
                        //                             Lavel = $"{SelectedLevel}{i.ToString("D2")}",
                        //                             Range = i,
                        //                             ConcatLevel = $"{SelectedLevel}-{i.ToString("D2")}",
                        //                             Position = lastPosition.Position +1
                        //                         })
                        //                         .ToList();
                    }
                    else
                    {

                        int start = Convert.ToInt32(From);
                        int end = Convert.ToInt32(To);
                        int basePosition = 0;
                        if (lastPosition != null)
                        {
                            basePosition = Convert.ToInt32(lastPosition.Position);
                        }

                        RangeItemsList = Enumerable.Range(start, end - start + 1)
                            .Select((i, index) => new RangeItems
                            {
                                Lavel = $"{i.ToString("D2")}",
                                Range = i,
                                ConcatLevel = $"{selectedParent.FullName}-{i.ToString("D2")}",
                                Position = basePosition + index + 1 // incrementing with each item
                            })
                            .ToList();
                        //RangeItemsList = Enumerable.Range(Convert.ToInt32(From), Convert.ToInt32(To) - Convert.ToInt32(From) + 1)
                        //                        .Select(i => new RangeItems
                        //                        {
                        //                            Lavel = $"{i.ToString("D2")}",
                        //                            Range = i,
                        //                            ConcatLevel = $"{selectedParent.Name}-{i.ToString("D2")}",
                        //                            Position= lastPosition.Position + 1

                        //                        })
                        //                        .ToList();
                    }

                    if (RangeItemsList.Count()>0)
                    {


                        
                            var ExistList = WarehouseLocationMainList.Where(a => a.Parent == selectedParent.IdWarehouseLocation).ToList();
                            if (ExistList.Count() > 0)
                            {
                                var ExistedString = string.Join(",", ExistList.Where(a => a.GetType().GetProperty("FullName") != null).Select(a => a.GetType().GetProperty("FullName")?.GetValue(a)?.ToString()));

                                if (ExistedString != string.Empty && ExistedString != "" && ExistedString != "---")
                                {
                                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(ExistedString + " " + string.Format(Application.Current.Resources["AddLocationWorningMSG"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                                }
                                else
                                {
                                    MessageBoxResult MessageBoxResult = MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["AddLocationParentWorningMSG"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);

                                }

                                var existedNames = ExistedString.Split(',').Select(s => s.Trim()).ToHashSet();

                                RangeItemsList.RemoveAll(item =>
                                {
                                    var prop = item.GetType().GetProperty("ConcatLevel");
                                    if (prop == null) return false;

                                    var Lavel = prop.GetValue(item)?.ToString();
                                    return Lavel != null && existedNames.Contains(Lavel);
                                });

                                RangeItemsList.RemoveAll(item => existedNames.Contains(item.ConcatLevel));
                            }
                       
                        
                    }

                    //if (selectedParent.Parent != -1 || selectedParent.Parent == 0)
                    //{
                    //    //TempLocation.Parent = WarehouseLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).IdWarehouseLocation;
                    //    //TempLocation.ParentName = WarehouseLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).FullName;
                    //    //RangeItemsList.Insert(0, new RangeItems { Lavel = TempLocation.ParentName, Range = 0 });
                    //    RangeItemsList.Insert(0, new RangeItems { Lavel = SelectedLevel, Range = 0 });
                    //}
                    //else
                    //{
                    //    RangeItemsList.Insert(0, new RangeItems { Lavel = SelectedLevel, Range = 0 });
                    //}


                }

                    

                if (!string.IsNullOrEmpty(error))
                    return;

                //var lastPosition = WarehouseLocationMainList.Where(a => a.Parent == selectedParent.IdWarehouseLocation).OrderByDescending(b => b.FullName).FirstOrDefault() ;

                TempLocation.Position = Position;
                if (selectedParent.Parent != -1)// for sub parents
                {
                    TempLocation.Parent = WarehouseLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).IdWarehouseLocation;
                    TempLocation.ParentName = WarehouseLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).FullName;
                }
                else//for parent location
                    TempLocation.Parent = 0;



                TempLocation.Name = SelectedLevel;
                if (HTMLColor != null)
                {
                    if (HTMLColor.StartsWith("#"))
                        HTMLColor = HTMLColor.Remove(1, 2);
                }
                TempLocation.HtmlColor = HTMLColor;
                TempLocation.FullName = SelectedLevel;

                // This condtion is used to check the InUse service
                /// [001] Added "IsInUSENoWarehouseLocation" service method
                TempLocation.InUse = WarehouseService.IsInUSENoWarehouseLocation(TempLocation.IdWarehouseLocation, TempLocation.IdWarehouse);
                if (InUse)
                {
                    if (TempLocation.InUse)
                    {
                        TempLocation.InUse = true;
                    }
                    else
                    {
                        TempLocation.InUse = true;
                    }
                }
                else
                {
                    if (TempLocation.InUse)
                    {
                        TempLocation.InUse = true;
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("InUsedFailed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else
                    {
                        TempLocation.InUse = false;
                    }
                }

                //TempLocation.InUse = InUse;
                TempLocation.IsLead = IsLeaf != null ? IsLeaf : 0;
                TempLocation.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                if (DirectionList[SelectedDirection].IdLookupValue == 0)
                {
                    TempLocation.IdDirection = null;
                    TempLocation.Direction = null;
                }
                else
                {
                    TempLocation.IdDirection = DirectionList[SelectedDirection].IdLookupValue;
                    TempLocation.Direction = DirectionList[SelectedDirection];
                }
                if (ManufacturerList[SelectedManufacturer].IdManufacturer == 0)
                {
                    TempLocation.IdManufacturer = null;
                    TempLocation.ManufacturerName = null;
                }
                else
                {
                    TempLocation.IdManufacturer = ManufacturerList[SelectedManufacturer].IdManufacturer;

                }
                if (TempLocation.Parent == 0)
                {
                    TempLocation.ParentName = TempLocation.ParentName;
                    TempLocation.FullName = TempLocation.Name;
                   // TempLocation.Name = TempLocation.Name;
                }

                if (SelectedMapFile != null)
                    TempLocation.FileName = SelectedMapFile.FileName;

            }
            else
            {
                if (!string.IsNullOrEmpty(LocationName))
                    LocationName = LocationName.Trim();

                ParentAndNameValidation(new EditValueChangedEventArgs("", ""));
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("LocationName"));
                // PropertyChanged(this, new PropertyChangedEventArgs("HTMLColor"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedParent"));
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));

                if (!string.IsNullOrEmpty(error))
                    return;

                TempLocation.Position = Position;
                if (selectedParent.Parent != -1)// for sub parents
                    TempLocation.Parent = WarehouseLocationList.FirstOrDefault(x => x.FullName == SelectedParent.FullName).IdWarehouseLocation;
                else//for parent location
                    TempLocation.Parent = 0;

                TempLocation.Name = LocationName;
                if (HTMLColor != null)
                {
                    if (HTMLColor.StartsWith("#"))
                        HTMLColor = HTMLColor.Remove(1, 2);
                }
                TempLocation.HtmlColor = HTMLColor;
                TempLocation.FullName = LocationName;

                // This condtion is used to check the InUse service
                /// [001] Added "IsInUSENoWarehouseLocation" service method
                TempLocation.InUse = WarehouseService.IsInUSENoWarehouseLocation(TempLocation.IdWarehouseLocation, TempLocation.IdWarehouse);
                if (InUse)
                {
                    if (TempLocation.InUse)
                    {
                        TempLocation.InUse = true;
                    }
                    else
                    {
                        TempLocation.InUse = true;
                    }
                }
                else
                {
                    if (TempLocation.InUse)
                    {
                        TempLocation.InUse = true;
                        CustomMessageBox.Show(string.Format(Application.Current.FindResource("InUsedFailed").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    else
                    {
                        TempLocation.InUse = false;
                    }
                }

                //TempLocation.InUse = InUse;
                TempLocation.IsLead = IsLeaf != null ? IsLeaf : 0;
                TempLocation.IdWarehouse = WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse;
                if (DirectionList[SelectedDirection].IdLookupValue == 0)
                {
                    TempLocation.IdDirection = null;
                    TempLocation.Direction = null;
                }
                else
                {
                    TempLocation.IdDirection = DirectionList[SelectedDirection].IdLookupValue;
                    TempLocation.Direction = DirectionList[SelectedDirection];
                }
                if (ManufacturerList[SelectedManufacturer].IdManufacturer == 0)
                {
                    TempLocation.IdManufacturer = null;
                    TempLocation.ManufacturerName = null;
                }
                else
                {
                    TempLocation.IdManufacturer = ManufacturerList[SelectedManufacturer].IdManufacturer;

                }
                if (TempLocation.Parent == 0)
                {
                    TempLocation.FullName = TempLocation.Name;
                }

                if (SelectedMapFile != null)
                    TempLocation.FileName = SelectedMapFile.FileName;

            }

            try
                {
                if (IsNew)
                {
                //  WarehouseService = new WarehouseServiceController("localhost:6699");
                    if (IsRangeofLocation)
                    {
                        NewLocation = WarehouseService.AddWarehouseLocation_V2640(TempLocation, RangeItemsList);
                    }
                    else
                    {
                        NewLocation = WarehouseService.AddWarehouseLocation_V2220(TempLocation);
                    }

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddLocationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                else
                {
                    if (IsRangeofLocation)
                    {
                      //  WarehouseService = new WarehouseServiceController("localhost:6699");
                      //  NewLocation = WarehouseService.UpdateWarehouseLocation_V2640(TempLocation, RangeItemsList);
                    }
                    else
                    {
                        NewLocation = WarehouseService.UpdateWarehouseLocation_V2220(TempLocation);
                    }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditLocationSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }
                    IsSave = true;
                }
                catch (FaultException<ServiceException> ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method AddLocationInformation()  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method AddLocationInformation() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method AddLocationInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod AddLocationInformation() executed successfully", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
            //}
            //else
            //{
            //    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddLocationViewModelMethodCheckLocationName").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            //    //AddLocationView.
            //    //ParentAndNameValidation(new EditValueChangedEventArgs("", ""));
            //    error = EnableValidationAndGetError();
            //}

        }

        /// <summary>
        /// Method to initialize AddLocationViewModel for edit
        /// [001] [vsana][24-11-2020][GEOS2-2426]AutoSort for the new locations created
        /// [002][cpatil][13-12-2021][GEOS2-3473]
        /// </summary>
        public void Init(WarehouseLocation SelectedWarehouseLocation, bool _manufacturerVisibility, ObservableCollection<WarehouseLocation> MainWarehouseLocationList)
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init(SelectedWarehouseLocation) ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (MainWarehouseLocationList!=null&& MainWarehouseLocationList.Count()>0)
                {
                    WarehouseLocationMainList = new ObservableCollection<WarehouseLocation>(MainWarehouseLocationList);
                }
                IsEdit = Visibility.Collapsed;
                TempLocation = SelectedWarehouseLocation;
                //[002]
                if(_manufacturerVisibility)
                ManufacturerVisibility = Visibility.Visible;
                else
                    ManufacturerVisibility = Visibility.Collapsed;
               
                    if (SelectedWarehouseLocation.Parent == 0)
                        SelectedParent = WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == -1);
                    else
                        SelectedParent = WarehouseLocationList.FirstOrDefault(x => x.IdWarehouseLocation == SelectedWarehouseLocation.Parent);
                    LocationName = SelectedWarehouseLocation.Name;
                
               

                
               
                fullName = GetFullName();
                ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                int mPosition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, fullName);
                MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                HTMLColor = SelectedWarehouseLocation.HtmlColor;
                Position = (int)SelectedWarehouseLocation.Position;
                InUse = SelectedWarehouseLocation.InUse;
                IsLeaf = SelectedWarehouseLocation.IsLead != null ? SelectedWarehouseLocation.IsLead : 0;
                if (!string.IsNullOrEmpty(HTMLColor))
                {
                    HTMLColor = HTMLColor.Insert(1, "F");
                    HTMLColor = HTMLColor.Insert(1, "F");
                }

                if (SelectedWarehouseLocation.IdDirection != null)
                {
                    SelectedDirection = DirectionList.FindIndex(x => x.IdLookupValue == SelectedWarehouseLocation.IdDirection);
                }

                if (SelectedWarehouseLocation.IdManufacturer != null)
                {
                    SelectedManufacturer = ManufacturerList.FindIndex(x => x.IdManufacturer == SelectedWarehouseLocation.IdManufacturer);
                }

                if (SelectedWarehouseLocation.FileName != null)
                {
                    SelectedMapFile = MapFiles.FirstOrDefault(x => x.FileName == SelectedWarehouseLocation.FileName);
                }
                else
                {
                    SelectedMapFile = MapFiles.FirstOrDefault(x => x.FileName == "---");
                }

                if (SelectedWarehouseLocation.Height != 0
                    && SelectedWarehouseLocation.Width != 0
                    && SelectedWarehouseLocation.Latitude != 0
                    && SelectedWarehouseLocation.Longitude != 0)
                {
                    IsCoordinatesNull = true;
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method Init(WarehouseLocation)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init(SelectedWarehouseLocation) executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method to initialize AddLocationViewModel for add
        /// [001][cpatil][13-12-2021][GEOS2-3473]
        /// </summary>
        /// <param name="WarehouseLocationList"></param>
        public void Init(bool _manufacturerVisibility, ObservableCollection<WarehouseLocation> MainWarehouseLocationList)
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init ...", category: Category.Info, priority: Priority.Low);
            try
            {
                WarehouseLocationMainList = new ObservableCollection<WarehouseLocation>(MainWarehouseLocationList);
                IsEdit = Visibility.Visible;
                    
                InUse = true;
                tempLocation = new WarehouseLocation();
                SelectedParent = WarehouseLocationList.FirstOrDefault(x => x.FullName == "---");
                SelectedMapFile = MapFiles.FirstOrDefault(x => x.FileName == "---");
               // SelectedLevel = AlphabetList.FirstOrDefault(x => x.Character == "---").Character;
                SelectedManufacturer = 0;
                //[001]
                if (_manufacturerVisibility)
                    ManufacturerVisibility = Visibility.Visible;
                else
                    ManufacturerVisibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod Init() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// To fill Warehouse locations 
        /// [001][cpatil][29-07-2019][GEOS2-1687]Add support for several warehouse TRANSIT locations
        /// </summary>
        private void FillParents()
        {
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod FillParents ...", category: Category.Info, priority: Priority.Low);
            try
            {

                // [001] Changed Service method
                WarehouseLocationList = new ObservableCollection<WarehouseLocation>(WarehouseService.GetIsLeafWarehouseLocations_V2034(WarehouseCommon.Instance.Selectedwarehouse) as List<WarehouseLocation>);
                WarehouseLocationList.Insert(0, new WarehouseLocation { Parent = -1, IdWarehouseLocation = -1, FullName = "---" });

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method FillParents()  " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddLocationViewModel Method FillParents() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method FillParents()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod FillParents() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Parent and name validation
        /// [001] [vsana][24-11-2020][GEOS2-2426]AutoSort for the new locations created
        /// </summary>
        /// <param name="obj"></param>
        public void ParentAndNameValidation(EditValueChangedEventArgs obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddLocationViewModel Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                parentValidationMessage = string.Empty;
                int mPosition = 0;

                if (IsNew)
                {
                    if (SelectedParent.Parent == -1)
                    {
                        if (WarehouseService.IsExistWarehouseLocationName(LocationName, 0, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                        {
                            parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                        }
                    }

                    else if (WarehouseService.IsExistWarehouseLocationName(LocationName, SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse))
                    {
                        parentValidationMessage = System.Windows.Application.Current.FindResource("AddLocationParentAndNameError").ToString();
                    }

                    if (IsRangeofLocation!=true)
                    {
                        fullName = GetFullName();
                    }
                    
                    if(SelectedParent.Parent == -1 || SelectedParent.Parent == 0)
                    {
                        ///[001] Added service method GetMaxPosition
                        mPosition = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(LocationName))
                        {
                            ///[001] Added service method GetMaxPosition
                            mPosition = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                        }
                        else
                        {
                            ///[001] Added service method GetMaxPosition_V2080
                            mPosition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, fullName);
                        }
                    }
                    ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                    //mPosition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, fullName);
                    //For add location MaxPostion incerease by one
                    //MaxPosition = ++mPosition;
                    if (mPosition == 0)
                    {
                        mPosition++;
                        MaxPosition = maxPosition;
                    }
                    MaxPosition = mPosition;
                    if (!(obj.NewValue == string.Empty))
                        Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationName();
                }
                else
                {
                    //if SelectedParent.IdWarehouseLocation==-1 send 0 means location full name is --- send 0 else send IdWarehouseLocation
                    mPosition = Position;
                    //mPosition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, GetFullName());
                    MaxPosition = mPosition == 0 ? ++mPosition : mPosition;
                    //  Position = MaxPosition;

                    //Regular Expression for Location Name
                    RegExpForLocationNameEdit();
                }

                

                EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Position"));
                GeosApplication.Instance.Logger.Log("AddLocationViewModelMethod ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  AddLocationViewModel Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001][vsana][25-11-2020][GEOS2-2426]AutoSort for the new locations created 
        /// This method  will return the string with fullName
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            string fullName = string.Empty;
            if (string.IsNullOrEmpty(LocationName))
            {
                if (SelectedParent.FullName.Equals("---"))
                {
                    fullName = string.Empty;
                }
                else
                {
                    fullName = SelectedParent.FullName;
                }
            }
            else
            {
                if (SelectedParent.FullName.Equals("---"))
                {
                    fullName = LocationName;
                }
                else
                {
                    fullName = SelectedParent.FullName + "-" + LocationName;
                }
            }
            return fullName;
        }
        //Method for Regular Expression for Location Name
        private void RegExpForLocationName()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                //RegExp = "([A-Z]{1}[0-9]{1})";          //Default value


                if (SelectedParent.IdWarehouseLocation == -1)
                {
                    IsLeaf = 0;
                    RegExp = "([A-Z]{1}[0-9]{1})";
                }
                else
                {
                    if (SelectedParent.Parent == 0)
                    {
                        IsLeaf = 0;
                        RegExp = "([0-9]{2})";                        //"";
                    }
                    else
                    {
                        IsLeaf = 1;
                        RegExp = "([A-Z]{1}[1-9]{0,1}[0-9]{1,2})";
                    }
                }
                GeosApplication.Instance.Logger.Log("RegExpForLocationName Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  RegExpForLocationName Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void RegExpForLocationNameEdit()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method RegExpForLocationNameEdit Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                //RegExp = "([A-Z]{1}[0-9]{1})";          //Default value


                if (SelectedParent.IdWarehouseLocation == -1)
                {
                    //IsLeaf = 0;
                    RegExp = "([A-Z]{1}[0-9]{1})";
                }
                else
                {
                    if (SelectedParent.Parent == 0)
                    {
                        //IsLeaf = 0;
                        RegExp = "([0-9]{2})";                        //"";
                    }
                    else
                    {
                        //IsLeaf = 1;
                        RegExp = "([A-Z]{1}[1-9]{0,1}[0-9]{1,2})";
                    }
                }
                GeosApplication.Instance.Logger.Log("RegExpForLocationNameEdit Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in  RegExpForLocationNameEdit Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// [001] [vsana][24-11-2020][GEOS2-2426]AutoSort for the new locations created 
        /// </summary>
        /// <returns></returns>
        private bool CheckRegExpForLocationName()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CheckRegExpForLocationName Method ParentAndNameValidation()...", category: Category.Info, priority: Priority.Low);
                bool retVal = true;
                int mposition = 0;
                var reg = new Regex("");

                if (IsNew)
                {
                    if (SelectedParent.IdWarehouseLocation == -1)
                    {
                        reg = new Regex("([A-Z]{1}[0-9]{1})");
                        if (reg.IsMatch(locationName.ToString()))
                        {
                            fullName = GetFullName();
                            ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                            mposition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, fullName);
                            //Position = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                            MaxPosition = mposition;
                            MaxPosition = MaxPosition == 0 ? ++MaxPosition : MaxPosition;
                            Position = MaxPosition;
                            retVal = true;
                        }
                    }
                    else
                    {
                        if (SelectedParent.Parent == 0)
                        {
                            reg = new Regex(@"^\d{2}$");                         //([0-9]{2})
                            if (reg.IsMatch(locationName.ToString()))
                            {
                                fullName = GetFullName();
                                ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                                mposition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, fullName);
                                //Position = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                MaxPosition = mposition;
                                Position = MaxPosition;
                                retVal = true;
                            }
                        }
                        else
                        {
                            reg = new Regex("([A-Z]{1}[1-9]{1}[-]{1}[0-9]{1,2})");
                            if (!reg.IsMatch(LocationName.ToString()))
                            {
                                reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                                if (reg.IsMatch(LocationName))
                                {
                                    fullName = GetFullName();
                                    ///[001] Changed service method GetMaxPosition to GetMaxPosition_V2080
                                    mposition = (int)WarehouseService.GetMaxPosition_V2080(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse, fullName);
                                    //Position = (int)WarehouseService.GetMaxPosition(SelectedParent.IdWarehouseLocation == -1 ? 0 : SelectedParent.IdWarehouseLocation, WarehouseCommon.Instance.Selectedwarehouse.IdWarehouse);
                                    MaxPosition = mposition;
                                    Position = MaxPosition;
                                    retVal = true;
                                }
                            }
                        }
                    }
                    if (!reg.IsMatch(LocationName.ToString()))
                        retVal = false;
                }
                else
                {
                    if (SelectedParent.IdWarehouseLocation == -1)
                    {
                        reg = new Regex("([A-Z]{1}[0-9]{1})");
                    }
                    else
                    {
                        if (SelectedParent.Parent == 0)
                        {
                            reg = new Regex(@"^\d{2}$");                         //([0-9]{2})
                        }
                        else
                        {
                            reg = new Regex("([A-Z]{1}[1-9]{1}[-]{1}[0-9]{1,2})");
                            if (!reg.IsMatch(LocationName.ToString()))
                                reg = new Regex("([A-Z]{1}[0-9]{1,2})");
                        }
                    }
                    if (!reg.IsMatch(LocationName.ToString()))
                        retVal = false;
                }
                
                GeosApplication.Instance.Logger.Log("CheckRegExpForLocationName Method ParentAndNameValidation() executed successfully", category: Category.Info, priority: Priority.Low);
                return retVal;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CheckRegExpForLocationName Method ParentAndNameValidation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                return false;
            }
        }


    /// <summary>
    /// Method to Close Window
    /// </summary>
    /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }


        /// <summary>
        /// WMS	M051-13	Add new column Direction in locations configuration
        /// Method to fill direction list
        /// </summary>
        private void FillDirectionList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDirectionList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> temptypeList = CrmStartUp.GetLookupValues(41);
                DirectionList = new List<LookupValue>();
                DirectionList = new List<LookupValue>(temptypeList);
                DirectionList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                GeosApplication.Instance.Logger.Log("Method FillDirectionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDirectionList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDirectionList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillHolidaysTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [cpatil][10-12-2021][GEOS2-3473]
        /// Method to fill Manufacturer list
        /// </summary>
        private void FillManufacturerList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDirectionList()...", category: Category.Info, priority: Priority.Low);
                List<Manufacturer> tempManufacturerListList = WarehouseService.GetAllActiveManufacturers();
                ManufacturerList = new List<Manufacturer>(tempManufacturerListList);
                ManufacturerList.Insert(0, new Manufacturer() { IdManufacturer=0, Name = "---" });
                GeosApplication.Instance.Logger.Log("Method FillDirectionList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDirectionList " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillDirectionList " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillHolidaysTypeList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillMapFiles()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillMapFiles()...", category: Category.Info, priority: Priority.Low);

                MapFiles = GeosRepositoryService.GetAllFileDetailsFromCompanyLayout();

                MapFiles.Insert(0, new FileDetail() { FileName = "---", FileByte = null });

                GeosApplication.Instance.Logger.Log("Method FillMapFiles()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMapFiles " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                UI.CustomControls.CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMapFiles " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillMapFiles " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void AddMapLocatorCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddMapLocatorCommandAction ...", category: Category.Info, priority: Priority.Low);

            MapLocatorView mapLocatorView = new MapLocatorView();
            MapLocatorViewModel mapLocatorViewModel = new MapLocatorViewModel();

            EventHandler handle = delegate { mapLocatorView.Close(); };
            mapLocatorViewModel.RequestClose += handle;
            mapLocatorView.DataContext = mapLocatorViewModel;
            mapLocatorViewModel.Init(SelectedMapFile, TempLocation);

            mapLocatorView.ShowDialog();

            if (mapLocatorViewModel.IsAcceptClick)
            {
                if (mapLocatorViewModel.height != null && mapLocatorViewModel.height != 0
                    && mapLocatorViewModel.width != null && mapLocatorViewModel.width != 0
                    && mapLocatorViewModel.latitude != null
                    && mapLocatorViewModel.longitude != null)
                {
                    IsCoordinatesNull = true;
                    TempLocation.Height = (double)mapLocatorViewModel.height;
                    TempLocation.Width = (double)mapLocatorViewModel.width;
                    TempLocation.Latitude = (double)mapLocatorViewModel.latitude;
                    TempLocation.Longitude = (double)mapLocatorViewModel.longitude;

                }
                else
                    IsCoordinatesNull = false;
            }

            GeosApplication.Instance.Logger.Log("Method AddMapLocatorCommandAction ...", category: Category.Info, priority: Priority.Low);
        }

        #endregion

    }
}
