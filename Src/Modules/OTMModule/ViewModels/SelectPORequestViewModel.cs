using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.Data.Common.OTM;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.OTM.Views;
using System.Windows;
using DevExpress.Data.Extensions;
using DevExpress.XtraSpreadsheet.Model;
using Emdep.Geos.Data.Common.PCM;
using System.Windows.Media;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.UI.CustomControls;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class SelectPORequestViewModel : NavigationViewModelBase, INotifyPropertyChanged , IDisposable
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IOTMService OTMService = new OTMServiceController("localhost:6699");
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private ObservableCollection<LinkedOffers> linkedpolist;

        private ObservableCollection<Emailattachment> poTypePOAttachementsList;

        private int selectedAttachmentIndex;

        private string poNumber;
        private LinkedOffers SelectedofferInfo;
        private LinkedOffers newLinkedPo;


        #endregion

        #region Properties

        public ObservableCollection<LinkedOffers> LinkedPolist
        {
            get
            {
                return linkedpolist;
            }

            set
            {
                linkedpolist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedPolist"));
            }
        }


        public LinkedOffers NewLinkedPo
        {
            get
            {
                return newLinkedPo;
            }

            set
            {
                newLinkedPo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewLinkedPo"));
            }
        }

        public ObservableCollection<Emailattachment> PoTypePOAttachementsList
        {
            get { return poTypePOAttachementsList; }
            set
            {
                poTypePOAttachementsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PoTypePOAttachementsList"));
            }
        }


        public int SelectedAttachmentIndex
        {
            get { return selectedAttachmentIndex; }
            set
            {
                selectedAttachmentIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachmentIndex"));
            }
        }

       
        public string PONumber
        {
            get
            {
                return poNumber;
            }

            set
            {
                poNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumber"));
            }
        }
        #endregion

        #region public ICommand

        public ICommand SelectPOViewAcceptCommand { get; set; }

        public ICommand SelectPOViewCancelButtonCommand { get; set; }

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

        public SelectPORequestViewModel()
        {
            SelectPOViewAcceptCommand = new RelayCommand(new Action<object>(AcceptCommandAction));

            SelectPOViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));


        }


        #endregion

        #region Methods

        public void Init(object obj)
        {
            var Object = obj as LinkedOffers;
            SelectedofferInfo = obj as LinkedOffers;

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
            LinkedPolist = new ObservableCollection<LinkedOffers>(OTMService.OTM_GetPoRequestLinkedPO_V2640(Object.Code));
            PoTypePOAttachementsList = Object.PoTypePOAttachementsList;
            PoTypePOAttachementsList.Insert(0, new Emailattachment() { AttachmentName = "---" });
            PONumber = Object.LinkedPO;
        }
        /// <summary>
        /// [001][ashish.malkhede][GEOS2-9207] https://helpdesk.emdep.com/browse/GEOS2-9207
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptCommandAction(object obj)
        {
            List<string> poList = new List<string>();
            if (!string.IsNullOrWhiteSpace(PONumber))
            {
                poList = PONumber.Split(',').Select(p => p.Trim()).ToList();
            }

            LinkedOffers LinkedPO = null;
            if (LinkedPolist != null)
            {
                LinkedPO = LinkedPolist.FirstOrDefault(i => !string.IsNullOrEmpty(i.LinkedPO) && poList.Contains(i.LinkedPO));
            }

            if (LinkedPO == null)
            {
                LinkedPO = new LinkedOffers();
            }
            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
            {
                DXSplashScreen.Show(x =>
                {
                    Window win = new Window
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
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x => new OTM.Views.SplashScreenView { DataContext = new SplashScreenViewModel() }, null, null);
            }


            LinkedPO.Attachment = PoTypePOAttachementsList[SelectedAttachmentIndex];
            if (OTMCommon.Instance.SelectedSinglePlant == null)
            {
                string serviceurl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                Data.Common.Company selectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                OTMCommon.Instance.SelectedPlantForRegisteredPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
            }
            else
            {
                Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedSinglePlant.IdCompany);
                OTMService = new OTMServiceController((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                    selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
            }

            PORequestDetails PODetails = OTMService.GetPODetailsbyAttachment_V2680(Convert.ToInt32(LinkedPO.Attachment.IdAttachment));//[001]
            PODetails.OfferInfo = LinkedPO;
            EditRegisteredPOsViewModel editRegisteredPOsViewModel = new EditRegisteredPOsViewModel();
            EditRegisteredPOsView editRegisteredPOsView = new EditRegisteredPOsView();

            if (PODetails.IsPOExist)
            {

                editRegisteredPOsViewModel.EditInIt(LinkedPO);
                EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                editRegisteredPOsViewModel.RequestClose += handle;
                editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                editRegisteredPOsView.ShowDialogWindow();
            }
            else
            {
                PODetails.IsNewPO = true;
                SelectedofferInfo.Attachment = PoTypePOAttachementsList[SelectedAttachmentIndex];
                editRegisteredPOsViewModel.InitNewPO(PODetails, SelectedofferInfo);
                EventHandler handle = delegate { editRegisteredPOsView.Close(); };
                editRegisteredPOsViewModel.RequestClose += handle;
                editRegisteredPOsView.DataContext = editRegisteredPOsViewModel;
                //[Rahul.Gadhave][GEOS2-9326][Date:01-09-2025]
                if (SelectedofferInfo.Attachment.SelectedIndexAttachementType != 1)
                {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["GoAheadPOLinkedOffer"].ToString(),
                   Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        RequestClose(null, null);
                        editRegisteredPOsView.ShowDialogWindow();

                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    RequestClose(null, null);
                    editRegisteredPOsView.ShowDialogWindow();

                }
             
                
                //[pramod.misal][GEOS2-9109][1-08-2025]
                if (editRegisteredPOsViewModel.IsNewPo == true && editRegisteredPOsViewModel.IsSave==true)
                {
                    var details = editRegisteredPOsViewModel.poregistereddetails;
                    // Map properties manually
                    LinkedOffers newLinkedPO = new LinkedOffers
                    {
                        Code = details.Code,
                        ReceivedIn = details.ReceptionDateNew,
                        PoType = details.Type,
                        IdPOType = details.IdPOType,
                        Confirmation = "Email Not Send",
                        IdCustomerPurchaseOrder = Convert.ToInt32(details.IdPO)
                    };
                    NewLinkedPo = newLinkedPO;

                    LinkedPolist.Add(newLinkedPO);

                }

            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Dispose()
        {

        }

        #endregion
    }
}
