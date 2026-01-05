using DevExpress.DataProcessing;
using DevExpress.Map.Kml.Model;
using DevExpress.Mvvm;
using DevExpress.Utils.Extensions;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class LinkedOffersViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        #endregion
        #endregion


        #region Properties
        private Int32 selectedIdCustomerPlant;
        private ObservableCollection<LinkedOffers> linkedofferlist;
        private LinkedOffers selectedlinkedoffer;
        private LinkedOffers selectedIndexLinkedOffer;
        private bool isAccepted;
        private GeosAppSetting geosAppSetting;
        private bool isLinkedOffersViewColumnChooserVisible;
        public string OTM_LinkedOffersViewGridSettingFilePath = GeosApplication.Instance.UserSettingFolderName + "OTM_LinkedOffersGridSetting.Xml";

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

        public Int32 SelectedIdCustomerPlant
        {
            get
            {
                return selectedIdCustomerPlant;
            }

            set
            {
                selectedIdCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIdCustomerPlant"));
            }
        }
        private Int64 selectedidpo;
        public Int64 SelectedIdPO
        {
            get
            {
                return selectedidpo;
            }

            set
            {
                selectedidpo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedidpo"));
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

        #region ICommand
        public ICommand LinkedOfferViewCancelButtonCommand { get; set; }
        public ICommand LinkedOfferViewAcceptButtonCommand { get; set; }
 
        public ICommand LinkedOfferOpenCommand { get; set; }
        public ICommand TableViewLoadedCommand { get; set; }


        #endregion

        #region Constructor
        public LinkedOffersViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LinkedOffersViewModel()...", category: Category.Info, priority: Priority.Low);

                LinkedOfferViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                LinkedOfferViewAcceptButtonCommand = new RelayCommand(new Action<object>(LinkedOfferViewAcceptButtonAction));
                LinkedOfferOpenCommand = new RelayCommand(new Action<object>(LinkedOfferOpenCommandAction));
                TableViewLoadedCommand = new DelegateCommand<RoutedEventArgs>(TableViewLoadedLinkedOfferCommandAction);


                GeosApplication.Instance.Logger.Log("Method LinkedOffersViewModel()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LinkedOffersViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method


        public void InIt(PORegisteredDetails poregistereddetails)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt()...", category: Category.Info, priority: Priority.Low);
                SelectedIdCustomerPlant = poregistereddetails.IdCustomerPlant;
                SelectedIdPO = poregistereddetails.IdPO;

                if (OTMCommon.Instance.SelectedPlantForRegisteredPO == null)
                {
                    string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                    OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedPlantForRegisteredPO.IdCompany);
                    OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                }

                GeosAppSettings = OTMService.GetGeosAppSettings(136);
                //OTMService = new OTMServiceController("localhost:6699");
                //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdCustomerPlant(SelectedIdCustomerPlant, SelectedIdPO, GeosAppSettings));

                Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdCustomerPlant_V2630(SelectedIdCustomerPlant, SelectedIdPO, GeosAppSettings));

                if (SelectedIdPO == 0)
                {
                    GeosAppSettings = OTMService.GetGeosAppSettings(153);
                    //Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2640(poregistereddetails.IdSite.ToString(), SelectedIdCustomerPlant.ToString(), GeosAppSettings));
                    Linkedofferlist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetLinkedofferByIdPlantAndGroup_V2660(poregistereddetails.IdSite.ToString(), SelectedIdCustomerPlant.ToString(), GeosAppSettings));



                }
                //[pramod.misal][06-12-2024][GEOS2-6463]  https://helpdesk.emdep.com/browse/GEOS2-6463
                if (poregistereddetails.OffersLinked != null && poregistereddetails.OffersLinked.Count > 0)
                {
                    foreach (var offer in poregistereddetails.OffersLinked)
                    {
                        var itemToRemove = Linkedofferlist.FirstOrDefault(x => x.IdOffer == offer.IdOffer); 
                        if (itemToRemove != null)
                        {
                            Linkedofferlist.Remove(itemToRemove);
                        }
                    }
                }
                // Process each linked offer to set the image bytes based on the customer name.
                foreach (var linkedOffer in Linkedofferlist)
                {
                    string customerName = linkedOffer.CutomerName;

                    if (!string.IsNullOrEmpty(customerName))
                    {
                        byte[] bytes = GeosRepositoryServiceController.GetCustomerIconFileInBytes(customerName);

                        if (bytes != null)
                        {
                            // Assuming linkedOffer has a property to hold image data (e.g., LinkedOfferImage).
                            linkedOffer.ActivityLinkedItemImage = ByteArrayToBitmapImage(bytes);
                        }
                        else
                        {
                            // Set a default image based on the theme.
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/wAccount.png");
                            }
                            else
                            {
                                linkedOffer.ActivityLinkedItemImage = GetImage("/Emdep.Geos.Modules.OTM;component/Assets/Images/blueAccount.png");
                            }
                        }
                    }
                }
                GeosApplication.Instance.Logger.Log("Method InIt()...", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method InIt()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        // [pramod.misal][19-12-2024][GEOS2-6463]
        public void TableViewLoadedLinkedOfferCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method TableViewLoadedLinkedOfferCommandAction ...", category: Category.Info, priority: Priority.Low);
                int visibleFalseCoulumn = 0;

                if (File.Exists(OTM_LinkedOffersViewGridSettingFilePath))
                {
                    ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.RestoreLayoutFromXml(OTM_LinkedOffersViewGridSettingFilePath);
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
                ((DevExpress.Xpf.Grid.GridViewBase)obj.OriginalSource).Grid.SaveLayoutToXml(OTM_LinkedOffersViewGridSettingFilePath);

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
                XDocument layoutXml = XDocument.Load(OTM_LinkedOffersViewGridSettingFilePath);

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
                    gridControl.SaveLayoutToXml(OTM_LinkedOffersViewGridSettingFilePath);
                    ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_LinkedOffersViewGridSettingFilePath);
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
                gridControl.SaveLayoutToXml(OTM_LinkedOffersViewGridSettingFilePath);
                ((GridControl)column.View.DataControl).SaveLayoutToXml(OTM_LinkedOffersViewGridSettingFilePath);

                GeosApplication.Instance.Logger.Log("Method VisibleIndexLinkedOfferChanged() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in VisibleIndexLinkedOfferChanged() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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

        //[pramod.misal][GEOS2-6460][28-11-2024]
        BitmapImage GetImage(string path)
        {
            //return new BitmapImage(new Uri(path, UriKind.Relative));
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            image.EndInit();
            return image;
        }

        //[pramod.misal][GEOS2-6460][28-11-2024]
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void LinkedOfferViewAcceptButtonAction(object obj)
        {
            if (SelectedIndexLinkedOffer != null)
            { 
                SelectedIndexLinkedOffer.IsNew = true;
                SelectedLinkedOffersDetails = SelectedIndexLinkedOffer;
                IsAccepted = true;

                
            }
            RequestClose(null, null);
        }
        //[ashish.malkhede][GEOS2-6463][19-12-2024]
        private void LinkedOfferOpenCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Start LoginWindowViewModel constructor", category: Category.Info, priority: Priority.Low);
            try
            {
               // OTMService = new OTMServiceController("localhost:6699");
                string path = OTMService.GetCommercialPath();
                if (SelectedIndexLinkedOffer != null)
                {
                    LinkedOffers Doc = SelectedIndexLinkedOffer;
                    string completePath = Path.Combine($"{path} {Doc.Year}", $"{Doc.CustomerGroup} - {Doc.Name}", Doc.Code);
                    //string filePath = completePath + "\\" + offerslink.AttachmentFileName;

                    if (Directory.Exists(completePath))
                    {
                        //Directory.CreateDirectory(completePath);
                        string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
                        ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
                        info.Arguments = "/n," + "\"" + completePath + "\"";
                        info.WindowStyle = ProcessWindowStyle.Normal;
                        Process.Start(info);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LinkedOfferDocFileNotFound").ToString(), completePath), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                }

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("End LinkedOfferOpenCommandAction", category: Category.Info, priority: Priority.Low);
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
