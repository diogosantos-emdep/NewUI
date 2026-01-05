using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using Prism.Logging;
using System.Windows;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Modules.OTM.CommonClass;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Charts;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    /// <summary>
    /// // [pramod.misal][23-04-2025][GEOS2-6463] https://helpdesk.emdep.com/browse/GEOS2-7250
    /// </summary>
    public class PORequestLinkedOffersViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        #endregion

        #region Properties
        private string selectedIdPlant;
        private ObservableCollection<LinkedOffers> linkedofferlist;
        private LinkedOffers selectedlinkedoffer;
        private LinkedOffers selectedIndexLinkedOffer;
        private bool isAccepted;
        private GeosAppSetting geosAppSetting;
        private bool isLinkedOffersViewColumnChooserVisible;
        public string OTM_PORequestLinkedOffersViewGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "OTM_PORequestLinkedOffersGridSetting.Xml";

        #endregion

        #region Declarations

        public bool IsLinkedOffersViewColumnChooserVisible
        {
            get { return isLinkedOffersViewColumnChooserVisible; }
            set
            {
                isLinkedOffersViewColumnChooserVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLinkedOffersViewColumnChooserVisible"));
            }
        }

        public string SelectedIdPlant
        {
            get
            {
                return selectedIdPlant;
            }

            set
            {
                selectedIdPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIdPlant"));
            }
        }
        private string selectedidgroup;
        public string SelectedIdGroup
        {
            get
            {
                return selectedidgroup;
            }

            set
            {
                selectedidgroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIdGroup"));
            }
        }
        public ObservableCollection<LinkedOffers> Linkedofferlist
        {
            get
            {
                return linkedofferlist;
            }

            set
            {
                linkedofferlist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Linkedofferlist"));
            }
        }
        public LinkedOffers SelectedLinkedOffersDetails
        {
            get
            {
                return selectedlinkedoffer;
            }

            set
            {
                selectedlinkedoffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLinkedOffersDetails"));
            }
        }
        public LinkedOffers SelectedIndexLinkedOffer
        {
            get
            {
                return selectedIndexLinkedOffer;
            }

            set
            {
                selectedIndexLinkedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexLinkedOffer"));
            }
        }

        private ObservableCollection<LinkedOffers> selectedIndexPOLinkedOffer;
        public ObservableCollection<LinkedOffers> SelectedIndexPOLinkedOffer
        {
            get
            {
                return selectedIndexPOLinkedOffer;
            }
            set
            {
                selectedIndexPOLinkedOffer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPOLinkedOffer"));
            }
        }



        //private LinkedOffers selectedIndexPOLinkedOffer;
        //public LinkedOffers SelectedIndexPOLinkedOffer
        //{
        //    get
        //    {
        //        return selectedIndexPOLinkedOffer;
        //    }
        //    set
        //    {
        //        selectedIndexPOLinkedOffer = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPOLinkedOffer"));
        //    }
        //}


        public bool IsAccepted
        {
            get { return isAccepted; }
            set
            {
                isAccepted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAccepted"));
            }
        }
        public GeosAppSetting GeosAppSettings
        {
            get { return geosAppSetting; }
            set
            {
                geosAppSetting = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GeosAppSettings"));
            }
        }
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

        #region Constructor
        public PORequestLinkedOffersViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PORequestLinkedOffersViewModel()...", category: Category.Info, priority: Priority.Low);

                LinkedOfferViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                LinkedOfferViewAcceptButtonCommand = new RelayCommand(new Action<object>(LinkedOfferViewAcceptButtonAction));
                LinkedOfferOpenCommand = new RelayCommand(new Action<object>(LinkedOfferOpenCommandAction));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedLinkedOfferCommandAction);
                CustomShowFilterPopupCommand = new DelegateCommand<FilterPopupEventArgs>(CustomShowFilterPopupAction);



                GeosApplication.Instance.Logger.Log("Method PORequestLinkedOffersViewModel()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method PORequestLinkedOffersViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region ICommand
        public ICommand LinkedOfferViewCancelButtonCommand { get; set; }
        public ICommand LinkedOfferViewAcceptButtonCommand { get; set; }

        public ICommand LinkedOfferOpenCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }

        public ICommand CustomShowFilterPopupCommand { get; set; }



        #endregion

        #region Method
        private void CustomShowFilterPopupAction(FilterPopupEventArgs e)
        {
            try
            {
                #region Offers
                if (e.Column.FieldName == "LinkedPO")
                {
                    List<object> filterItems = new List<object>();

                    //// Blank
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(Blanks)",
                    //    EditValue = CriteriaOperator.Parse("IsNullOrEmpty([LinkedPO])")
                    //});

                    //// Non-blank
                    //filterItems.Add(new CustomComboBoxItem()
                    //{
                    //    DisplayValue = "(Non blanks)",
                    //    EditValue = CriteriaOperator.Parse("!IsNullOrEmpty([LinkedPO])")
                    //});

                    // Unique individual names from comma-separated values
                    HashSet<string> uniqueNames = new HashSet<string>();
                    foreach (var poRequest in Linkedofferlist)
                    {
                        if (!string.IsNullOrWhiteSpace(poRequest.LinkedPO))
                        {
                            string[] names = poRequest.LinkedPO.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var name in names)
                            {
                                string trimmedName = name.Trim();
                                if (!string.IsNullOrWhiteSpace(trimmedName) && uniqueNames.Add(trimmedName))
                                {
                                    filterItems.Add(new CustomComboBoxItem
                                    {
                                        DisplayValue = trimmedName,
                                        EditValue = CriteriaOperator.Parse("LinkedPO Like ?", $"%{trimmedName}%")
                                    });
                                }
                            }
                        }
                    }
                    // Final assignment (REMOVE the null line!)
                    e.ComboBoxEdit.ItemsSource = filterItems.OrderBy(item => ((CustomComboBoxItem)item).DisplayValue.ToString()).ToList();
                }
                #endregion
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomShowFilterPopupAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        //[pramod.misal][23-04-2024][GEOS2-7250]  https://helpdesk.emdep.com/browse/GEOS2-7250
        //public void InIt(PORegisteredDetails poregistereddetails)
        //[Rahul.Gadhave][GEOS2-9080][Date:30-07-2025]
        public void InIt(ObservableCollection<LinkedOffers> LinkedofferListfromGrid, Int64 IdCustomerGroup, Int64 IdCustomerPlant)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt()...", category: Category.Info, priority: Priority.Low);
                if (IdCustomerGroup != 0 || IdCustomerPlant != 0)
                {


                    //[rahul.gadhave][GEOS2-9020][23.07.2025] 
                    if (OTMCommon.Instance.SelectedSinglePlantForPO == null)
                    {
                        string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                        Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                        OTMCommon.Instance.SelectedSinglePlantForPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    }
                    else
                    {
                        Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany);
                        OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                            selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    }
                    SelectedIdPlant = IdCustomerPlant.ToString();
                    SelectedIdGroup = IdCustomerGroup.ToString();
                    if (SelectedIdPlant == "0")
                    {
                        SelectedIdPlant = string.Empty;
                    }
                    if (SelectedIdGroup == "0")
                    {
                        SelectedIdGroup = string.Empty;
                    }
                    //OTMService = new OTMServiceController("localhost:6699");
                    GeosAppSettings = OTMService.GetGeosAppSettings(153);
                    //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2640(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                    //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2660(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                    Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2670(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                    var offersToRemove = Linkedofferlist.Where(offer => LinkedofferListfromGrid.Any(gridOffer => gridOffer.IdOffer == offer.IdOffer)).ToList();

                    // Remove each item from Linkedofferlist outside of the loop
                    foreach (var offer in offersToRemove)
                    {
                        Linkedofferlist.Remove(offer);
                    }
                }
                else
                {
                    if (LinkedofferListfromGrid != null && LinkedofferListfromGrid.Count > 0)
                    {
                        if (OTMCommon.Instance.SelectedSinglePlantForPO == null)
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                            Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                            OTMCommon.Instance.SelectedSinglePlantForPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                        }
                        else
                        {
                            Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany);
                            OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                                selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        }

                        if (LinkedofferListfromGrid.All(x => x.IdSite == 0))
                        {
                            // Use IdPlant if all IdSite are null
                            SelectedIdPlant = string.Join(",", LinkedofferListfromGrid
                                .Where(x => x.IdPlant != 0)
                                .Select(x => x.IdPlant));
                        }
                        else
                        {
                            // Use IdSite if at least one is not null
                            SelectedIdPlant = string.Join(",", LinkedofferListfromGrid
                                .Where(x => x.IdSite != 0)
                                .Select(x => x.IdSite));
                        }

                        // SelectedIdGroup will always use IdCustomer
                        SelectedIdGroup = string.Join(",", LinkedofferListfromGrid
                            .Where(x => x.IdCustomer != 0)
                            .Select(x => x.IdCustomer));
                        SelectedIdGroup = string.Join(",", LinkedofferListfromGrid.Select(x => x.IdCustomer));
                        //OTMService = new OTMServiceController("localhost:6699");
                        GeosAppSettings = OTMService.GetGeosAppSettings(153);
                        //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2640(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                        //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2660(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                        Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2670(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                        // Use a list to store the items that should be removed
                        // var offersToRemove = Linkedofferlist.Where(offer => LinkedofferListfromGrid.Any(gridOffer => gridOffer.IdOffer == offer.IdOffer)).ToList();
                        var offersToRemove = Linkedofferlist.Where(offer => LinkedofferListfromGrid.Any(gridOffer => gridOffer.IdOffer == offer.IdOffer)).ToList();

                        // Remove each item from Linkedofferlist outside of the loop
                        foreach (var offer in offersToRemove)
                        {
                            Linkedofferlist.Remove(offer);
                        }
                    }
                    if ((IdCustomerGroup == 0 || IdCustomerPlant == 0 )&& (LinkedofferListfromGrid == null || LinkedofferListfromGrid.Count == 0))
                    {

                        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
                        if (OTMCommon.Instance.SelectedSinglePlantForPO == null)
                        {
                            string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                            Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                            OTMCommon.Instance.SelectedSinglePlantForPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                        }
                        else
                        {
                            Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany);
                            OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                                selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        }
                        SelectedIdPlant = IdCustomerPlant.ToString();
                        SelectedIdGroup = IdCustomerGroup.ToString();
                        if (SelectedIdPlant == "0")
                        {
                            SelectedIdPlant = string.Empty;
                        }
                        if (SelectedIdGroup == "0")
                        {
                            SelectedIdGroup = string.Empty;
                        }
                        //OTMService = new OTMServiceController("localhost:6699");
                        GeosAppSettings = OTMService.GetGeosAppSettings(153);
                        //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2640(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                        //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2660(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                        Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2670(SelectedIdPlant, SelectedIdGroup, GeosAppSettings));
                        var offersToRemove = Linkedofferlist.Where(offer => LinkedofferListfromGrid.Any(gridOffer => gridOffer.IdOffer == offer.IdOffer)).ToList();

                        // Remove each item from Linkedofferlist outside of the loop
                        foreach (var offer in offersToRemove)
                        {
                            Linkedofferlist.Remove(offer);
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method InIt()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InIt()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }


        public void TableViewLoadedLinkedOfferCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedLinkedOfferCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(OTM_PORequestLinkedOffersViewGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(OTM_PORequestLinkedOffersViewGridSettingFilePath);
                    try
                    {
                        // Get the TableView and GridControl from the event source
                        TableView gridTableView = obj.OriginalSource as TableView;
                        GridControl tempgridControl = gridTableView.DataControl as GridControl;
                        // Manually parse the XML to enforce visibility settings
                        ApplyLinkedOfferVisibilityFromXml(tempgridControl);
                    }
                    catch (Exception ex)
                    {

                    }

                    GridControl GridControlSTDetails = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;

                    TableView tableView = (TableView)GridControlSTDetails.View;
                    if (tableView.SearchString != null)
                    {
                        tableView.SearchString = null;
                    }
                }

                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.AddHandler(DXSerializer.AllowPropertyEvent, new AllowPropertyEventHandler(OnAllowProperty));

                //This code for save grid layout.
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(OTM_PORequestLinkedOffersViewGridSettingFilePath);

                GridControl gridControl = ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid;
                foreach (GridColumn column in gridControl.Columns)
                {
                    DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleProperty, typeof(GridColumn));
                    if (descriptor != null)
                    {
                        descriptor.AddValueChanged(column, VisibleChangedLinkedOffer);
                    }

                    DependencyPropertyDescriptor descriptorColumnPosition = DependencyPropertyDescriptor.FromProperty(GridColumn.VisibleIndexProperty, typeof(GridColumn));
                    if (descriptorColumnPosition != null)
                    {
                        descriptorColumnPosition.AddValueChanged(column, VisibleIndexLinkedOfferChanged);
                    }

                    if (column.Visible == false)
                    {
                        visibleFalseCoulumn++;
                    }
                }

                if (visibleFalseCoulumn > 0)
                {
                    IsLinkedOffersViewColumnChooserVisible = true;
                }
                else
                {
                    IsLinkedOffersViewColumnChooserVisible = false;
                }



                //AddPORegisteredCustomSettingCount(gridControl);
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedLinkedOfferCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TableViewLoadedLinkedOfferCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        // [pramod.misal][19-12-2024][GEOS2-6463]
        private void ApplyLinkedOfferVisibilityFromXml(GridControl gridControl)
        {
            try
            {
                XDocument layoutXml = XDocument.Load(OTM_PORequestLinkedOffersViewGridSettingFilePath);

                foreach (GridColumn column in gridControl.Columns)
                {
                    // Find the column entry in the XML based on its FieldName
                    var columnElement = layoutXml.Descendants("property")
                        .Where(x => (string)x.Attribute("name") == "FieldName" && x.Value == column.FieldName)
                        .FirstOrDefault();

                    if (columnElement != null)
                    {
                        // Get the visibility setting from the XML
                        var visibleElement = columnElement.Parent.Descendants("property")
                            .Where(x => (string)x.Attribute("name") == "Visible")
                            .FirstOrDefault();

                        if (visibleElement != null)
                        {
                            bool isVisible = bool.Parse(visibleElement.Value);
                            column.Visible = isVisible;
                            GeosApplication.Instance.Logger.Log($"Column '{column.FieldName}' visibility set to: {isVisible}.", category: Category.Info, priority: Priority.Low);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in ApplyVisibilityFromXml: " + ex.Message, category: Category.Exception, priority: Priority.High);
            }
        }

        //[pramod.misal][04-10-2024][GEOS2-6520]
        private void OnAllowProperty(object sender, AllowPropertyEventArgs e)
        {
            try
            {
                if (e.Property.Name == "GroupCount")
                    e.Allow = false;

                if (e.DependencyProperty == UI.Helper.TableViewEx.SearchStringProperty)
                    e.Allow = false;

                if (e.Property.Name == "FormatConditions")
                    e.Allow = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OnAllowProperty() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [pramod.misal][19-12-2024][GEOS2-6463]
        void VisibleChangedLinkedOffer(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleChangedLinkedOffer ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                if (column.ShowInColumnChooser)
                {
                    var view = column.View as TableView;
                    GridControl gridControl = view.DataControl as GridControl;
                    gridControl.SaveLayoutToXml(OTM_PORequestLinkedOffersViewGridSettingFilePath);
                    ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_PORequestLinkedOffersViewGridSettingFilePath);
                }

                if (column.Visible == false)
                {
                    //  IsWorkOrderColumnChooserVisible = true;
                }

                GeosApplication.Instance.Logger.Log("Method VisibleChangedLinkedOffer() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleChangedLinkedOffer() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        // [pramod.misal][19-12-2024][GEOS2-6463]
        void VisibleIndexLinkedOfferChanged(object sender, EventArgs args)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method VisibleIndexLinkedOfferChanged ...", category: Category.Info, priority: Priority.Low);

                GridColumn column = sender as GridColumn;
                var view = column.View as TableView;
                GridControl gridControl = view.DataControl as GridControl;
                gridControl.SaveLayoutToXml(OTM_PORequestLinkedOffersViewGridSettingFilePath);
                ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_PORequestLinkedOffersViewGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexLinkedOfferChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexLinkedOfferChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void LinkedOfferViewAcceptButtonAction(object obj)
        {

            //SelectedIndexPOLinkedOffer=((DevExpress.Xpf.Grid.GridViewBase)obj).Grid.SelectedItems;

            SelectedIndexPOLinkedOffer = new ObservableCollection<Emdep.Geos.Data.Common.OTM.LinkedOffers>(((DevExpress.Xpf.Grid.GridViewBase)obj).Grid.SelectedItems.Cast<Emdep.Geos.Data.Common.OTM.LinkedOffers>());

            if (SelectedIndexPOLinkedOffer != null)
            {
                foreach (var linkedOffer in SelectedIndexPOLinkedOffer)
                {
                    //linkedOffer.IsNew = true;                  
                    SelectedLinkedOffersDetails = (LinkedOffers)linkedOffer;
                    linkedOffer.IsNewLinkedOffer = true;
                    IsAccepted = true;
                }


                RequestClose(null, null);
            }


        }

        //[ashish.malkhede][GEOS2-6463][19-12-2024]
        private void LinkedOfferOpenCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Start LoginWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
            try
            {
                
                //OTMService = new OTMServiceController("localhost:6699");
                string basePath = OTMService.GetCommercialOffersPath_V2640();
                EmdepSite emdepSite = CrmStartUp.GetEmdepSiteById(Convert.ToInt32(OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany));

                basePath = basePath.Replace("{0}", emdepSite.FileServerIP);

                basePath = basePath.Replace("{1}", emdepSite.ShortName);
                string path = OTMService.GetCommercialPath();
                if (obj != null)
                {

                    LinkedOffers Doc = (LinkedOffers)obj;
                    string oldpath = Path.Combine($"{path} {Doc.Year}", $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code);
                    string newPathPath = Path.Combine(basePath, Doc.Year, $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code);

                    if (Directory.Exists(newPathPath))
                    {
                        if (Directory.Exists(newPathPath))
                        {
                            //Directory.CreateDirectory(completePath);
                            string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
                            ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
                            info.Arguments = "/n," + "\"" + newPathPath + "\"";
                            info.WindowStyle = ProcessWindowStyle.Normal;
                            System.Diagnostics.Process.Start(info);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), newPathPath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked path: {newPathPath}", category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else if (Directory.Exists(oldpath))
                    {
                        if (Directory.Exists(oldpath))
                        {
                            //Directory.CreateDirectory(completePath);
                            string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
                            ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
                            info.Arguments = "/n," + "\"" + oldpath + "\"";
                            info.WindowStyle = ProcessWindowStyle.Normal;
                            System.Diagnostics.Process.Start(info);
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), oldpath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked path: {oldpath}", category: Category.Info, priority: Priority.Low);
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), oldpath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        string NewPath = newPathPath;
                        GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked NewPath: {NewPath}", category: Category.Warn, priority: Priority.Low);
                        string oldPath = oldpath;
                        //GeosApplication.Instance.Logger.Log($"Checking Offer Path: {oldPath}", category: Category.Info, priority: Priority.Low);
                        GeosApplication.Instance.Logger.Log($"Offer folder not found. Checked OldPath: {oldPath}", category: Category.Warn, priority: Priority.Low);
                    }

                }


            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("End LinkedOfferOpenCommandAction", category: Category.Info, priority: Priority.Low);
            }

        }




        #endregion

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }


    }
}
