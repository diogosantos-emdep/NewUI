using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.XtraRichEdit;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.Crm.ViewModels;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Modules.OTM.CommonClass;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using static DevExpress.Mvvm.DataAnnotations.PredefinedMasks;

namespace Emdep.Geos.Modules.OTM.ViewModels
 {  /// <summary>
    /// [pramod.misal][GEOS2-8643][26-05-2025] https://helpdesk.emdep.com/browse/GEOS2-8643
    /// </summary>
    public class EmailPreviewWindowModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        CrmRestServiceController CrmRestStartUp = new CrmRestServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion
        #region Declarations
        private string subject;
        public string senderName;
        private string toRecipientName;
        private string cCName;
        private System.DateTime dateTime;
        private string senderNameEmployeeCodesWithInitialLetters;
        private string emailbody;
        private string htmlemailbody;
        private string zoomfactor;
        private string newZoomfactor;
        private List<ToRecipientName> toRecipientNameList;//[pramod.misal][GEOS2-9896][11.11.2025]https://helpdesk.emdep.com/browse/GEOS2-9896
        private List<ToCCName> cCNameList;//[pramod.misal][GEOS2-9896][11.11.2025]https://helpdesk.emdep.com/browse/GEOS2-9896
        public People PeopleContact { get; set; }//[pramod.misal][GEOS2-9896][11.11.2025]https://helpdesk.emdep.com/browse/GEOS2-9896
        private int selectedIndexCustomerGroup = -1;
        private int selectedIndexCompanyPlant = -1;
        private ObservableCollection<CustomerPlant> customerPlant;
        private ObservableCollection<CustomerPlant> entireCompanyPlantList;
        private ObservableCollection<Customer> customers;
        string serviceUrl;

        #endregion

        #region Properties 

        public string Zoomfactor
        {
            get
            {
                return zoomfactor; ;
            }

            set
            {
                zoomfactor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Zoomfactor"));
            }
        }

        public string NewZoomfactor
        {
            get
            {
                return newZoomfactor; ;
            }

            set
            {
                newZoomfactor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewZoomfactor"));
            }
        }

        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Subject"));
            }
        }

        public string SenderName
        {
            get { return senderName; }
            set
            {
                senderName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SenderName"));
            }
        }

        public string ToRecipientName
        {
            get { return toRecipientName; }
            set
            {
                toRecipientName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToRecipientName"));
            }
        }

        public string CCName
        {
            get { return cCName; }
            set
            {
                cCName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CCName"));
            }
        }

        public System.DateTime DateTime
        {
            get { return dateTime; }
            set
            {
                dateTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateTime"));
            }
        }
        public string SenderNameEmployeeCodesWithInitialLetters
        {
            get { return senderNameEmployeeCodesWithInitialLetters; }
            set
            {
                senderNameEmployeeCodesWithInitialLetters = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SenderNameEmployeeCodesWithInitialLetters"));
            }
        }

        public string Emailbody
        {
            get { return emailbody; }
            set
            {
                emailbody = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Emailbody"));
            }
        }

        public string HtmlEmailbody
        {
            get { return htmlemailbody; }
            set
            {
                htmlemailbody = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HtmlEmailbody"));
            }
        }

        public List<ToRecipientName> ToRecipientNameList
        {
            get { return toRecipientNameList; }
            set
            {
                toRecipientNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToRecipientNameList"));
            }
        }


        public List<ToCCName> CCNameList
        {
            get { return cCNameList; }
            set
            {
                cCNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CCNameList"));
            }
        }

        Int64 idCustomerGroup;
        public Int64 IdCustomerGroup
        {
            get
            {
                return idCustomerGroup;
            }
            set
            {
                idCustomerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCustomerGroup"));
            }
        }

        Int64 idCustomerPlant;
        public Int64 IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {
                idCustomerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCustomerPlant"));
            }
        }

        public int SelectedIndexCustomerGroup
        {
            get { return selectedIndexCustomerGroup; }
            set
            {
                selectedIndexCustomerGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCustomerGroup"));

                if (selectedIndexCustomerGroup >= 0)
                {
                    CustomerPlants = new ObservableCollection<CustomerPlant>(); CustomerPlants = new ObservableCollection<CustomerPlant>(
                    EntireCompanyPlantList
                    .Where(cpl => cpl.IdCustomer == Customers[SelectedIndexCustomerGroup].IdCustomer || cpl.CustomerPlantName == "---")
                    .OrderBy(cpl => cpl.City)
                    .ToList()
                    );

                    if (CustomerPlants.Count >= 0)
                        SelectedIndexCompanyPlant = -1;
                    else
                        SelectedIndexCompanyPlant = 0;
                    if (SelectedIndexCustomerGroup != -1)
                    {
                        IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                    }
                }
            }
        }

        public int SelectedIndexCompanyPlant
        {
            get { return selectedIndexCompanyPlant; }
            set
            {
                selectedIndexCompanyPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));

                if (selectedIndexCompanyPlant == -1)
                {
                    IdCustomerPlant = 0;
                    CustomerPlants.FirstOrDefault();
                }
                if (SelectedIndexCompanyPlant != -1)
                {
                    IdCustomerPlant = CustomerPlants[selectedIndexCompanyPlant].IdCustomerPlant;
                }
            }
        }
        public ObservableCollection<Customer> Customers
        {
            get
            {
                return customers;
            }

            set
            {
                customers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Customers"));
            }
        }

        public ObservableCollection<CustomerPlant> CustomerPlants
        {
            get
            {
                return customerPlant;
            }

            set
            {
                customerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlants"));
            }
        }

        public ObservableCollection<CustomerPlant> EntireCompanyPlantList
        {
            get
            {
                return entireCompanyPlantList;
            }

            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }
        #endregion

        #region public ICommand

        public ICommand POToClickCommand { get; set; }//[pramod.misal][GEOS2-9324][06-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9324
        public ICommand POCcClickCommand { get; set; }//[pramod.misal][GEOS2-9324][06-10-2025] https://helpdesk.emdep.com/browse/GEOS2-9324



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
        public EmailPreviewWindowModel()
        {
            //[pramod.misal][GEOS2-9896][21-01-2025] https://helpdesk.emdep.com/browse/GEOS2-9896
            POToClickCommand = new RelayCommand(new Action<object>(POToClickCommandAction)); //[pramod.misal][GEOS2-8643][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-8643
            POCcClickCommand = new RelayCommand(new Action<object>(POCcClickCommandAction)); //[pramod.misal][GEOS2-8643][26 - 05 - 2025] https://helpdesk.emdep.com/browse/GEOS2-8643

            //FillCustomerGroupList();

        }
        #endregion

        #region  Method



        private void FillEntireCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEntireCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                EntireCompanyPlantList = new ObservableCollection<CustomerPlant>();
                //EntireCompanyPlantList = new ObservableCollection<CustomerPlant>(OTMService.OTM_GetCustomerPlant_V2590());

                //OTMService = new OTMServiceController("localhost:6699");
                //[pramod.misal][GEOS2-7036][27-02-2025] https://helpdesk.emdep.com/browse/GEOS2-7036
                EntireCompanyPlantList = new ObservableCollection<CustomerPlant>(OTMServiceThread.OTM_GetCustomerPlant_V2620());

                GeosApplication.Instance.Logger.Log("Method FillEntireCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillEntireCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillEntireCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);

            }
        }

        public void FillServiceUrl()
        {
            try
            {
                if (OTMCommon.Instance.SelectedSinglePlantForPO == null)
                {
                    string serviceurl = serviceUrl = GeosApplication.Instance.GeosServiceProviders.GeosServiceProvider.Where(x => x.IsSelected == true).Select(xy => xy.Name).FirstOrDefault();
                    OTMCommon.Instance.SelectedSinglePlantForPO = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(x => x.Alias == serviceurl);
                }
                else
                {
                    Data.Common.Company selectedPlant = OTMCommon.Instance.UserAuthorizedPlantsList.FirstOrDefault(s => s.IdCompany == OTMCommon.Instance.SelectedSinglePlantForPO.IdCompany);
                    string serviceurl = serviceUrl = ((OTMCommon.Instance.UserAuthorizedPlantsList != null && selectedPlant.ServiceProviderUrl != null) ?
                        selectedPlant.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    OTMService = new OTMServiceController(serviceurl);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void FillTOCCInIt(List<ToRecipientName> toRecipientNameList, List<ToCCName> cCNameList,int selectedIndexCustomerGroup,int selectedIndexCompanyPlant)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTOCCInIt()...", category: Category.Info, priority: Priority.Low);
                FillServiceUrl();
                FillEntireCompanyPlantList();
                FillCustomerGroupList();
                if (toRecipientNameList != null && toRecipientNameList.Count > 0)
                    ToRecipientNameList = toRecipientNameList;

                if (cCNameList != null && cCNameList.Count > 0)
                    CCNameList = cCNameList;
                SelectedIndexCustomerGroup = selectedIndexCustomerGroup;
                SelectedIndexCompanyPlant = selectedIndexCompanyPlant;

                GeosApplication.Instance.Logger.Log(string.Format("Method FillTOCCInIt()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                
                GeosApplication.Instance.Logger.Log("Get an error in Method InIt()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }



        }

        public void InIt(object emailcontent, string EmailbodyToExpand)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt()...", category: Category.Info, priority: Priority.Low);
                if (emailcontent != null)
                {
                    Subject = ((EditPORequestsViewModel)emailcontent).Subject;
                    SenderName = ((EditPORequestsViewModel)emailcontent).SenderName;
                    ToRecipientName = ((EditPORequestsViewModel)emailcontent).ToRecipientName;
                    CCName = ((EditPORequestsViewModel)emailcontent).CCName;
                    DateTime = ((EditPORequestsViewModel)emailcontent).DateTime;
                    SenderNameEmployeeCodesWithInitialLetters = ((EditPORequestsViewModel)emailcontent).SenderNameEmployeeCodesWithInitialLetters;
                    Emailbody = ((EditPORequestsViewModel)emailcontent).Emailbody;
                    Emailbody = EmailbodyToExpand;
                    //[pramod.misal][GEOS2-9789][11-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9189
                    //HtmlEmailbody = ((EditPORequestsViewModel)emailcontent).Emailbody;
                    //Emailbody = PreloadEmailImagesBlocking(HtmlEmailbody);
                    FillCustomerGroupList();
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method InIt()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Get an error in Method InIt()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        //[pramod.misal][GEOS2-9789][11-08-2025]https://helpdesk.emdep.com/browse/GEOS2-9189
        private string PreloadEmailImagesBlocking(string htmlBody)
        {

            GeosApplication.Instance.Logger.Log("Method PreloadEmailImagesBlocking()...", category: Category.Info, priority: Priority.Low);
            if (string.IsNullOrWhiteSpace(htmlBody))
                return htmlBody;

            var imgRegex = new Regex("<img[^>]+src\\s*=\\s*['\"](?<src>[^'\"]+)['\"][^>]*>", RegexOptions.IgnoreCase);
            var matches = imgRegex.Matches(htmlBody);

            if (matches.Count == 0)
                return htmlBody;

            var client = new HttpClient(); // Reuse same client
            var replacements = new Dictionary<string, string>();

            // Prepare download tasks
            var tasks = new List<Task>();
            foreach (Match match in matches)
            {
                var src = match.Groups["src"].Value;
                if (src.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !replacements.ContainsKey(src))
                {
                    tasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            var bytes = client.GetByteArrayAsync(src).Result; // Still sync style, but parallel tasks
                            var base64 = Convert.ToBase64String(bytes);

                            var ext = Path.GetExtension(src).ToLower();
                            string mime;
                            if (ext == ".jpg" || ext == ".jpeg") mime = "image/jpeg";
                            else if (ext == ".png") mime = "image/png";
                            else if (ext == ".gif") mime = "image/gif";
                            else mime = "application/octet-stream";

                            var dataUri = $"data:{mime};base64,{base64}";
                            lock (replacements)
                            {
                                replacements[src] = dataUri;
                            }
                        }
                        catch
                        {
                            // Ignore failed downloads
                        }
                    }));
                }
            }

            // Wait for all downloads
            Task.WaitAll(tasks.ToArray());

            // Replace in one pass
            var sb = new StringBuilder(htmlBody);
            foreach (var kv in replacements)
            {
                sb.Replace(kv.Key, kv.Value);
            }

            GeosApplication.Instance.Logger.Log("Method PreloadEmailImagesBlocking()...", category: Category.Info, priority: Priority.Low);
            return sb.ToString();
        }
        /// <summary>
        /// <!--[pramod.misal][GEOS2-9896][11.11.2025]-->https://helpdesk.emdep.com/browse/GEOS2-9896
        /// </summary>
        /// <param name="parameter"></param>
        private void POToClickCommandAction(object parameter)
        {
            try
            {
                ToRecipientName ToObject = parameter as ToRecipientName;
                GeosApplication.Instance.Logger.Log("Method POToClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                if (ToObject != null)
                {
                    if (ToObject.IsEmdepContact == Visibility.Hidden)
                    {
                        AddContactViewModel addPOContactViewModel = new AddContactViewModel();
                        AddContactView addPOContactView = new AddContactView();
                        int IdCustomerGroup = 0;
                        int Idplant = 0;
                        if (SelectedIndexCustomerGroup != -1)
                        {
                            IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                        }
                        if (SelectedIndexCompanyPlant != -1)
                        {
                            Idplant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
                        }
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        addPOContactViewModel.InitCCFromEmail(ToObject, IdCustomerGroup, Idplant);
                        EventHandler handle = delegate { addPOContactView.Close(); };
                        addPOContactViewModel.RequestClose += handle;
                        addPOContactView.DataContext = addPOContactViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        addPOContactView.ShowDialogWindow();
                        if (addPOContactViewModel.IsSave == true)
                        {
                            ToObject.IsEmdepContact = Visibility.Visible;
                            ToObject.IsNotEmdepContact = Visibility.Hidden;
                        }

                    }
                    else
                    {
                        List<Company> salesOwners = OTMCommon.Instance.UserAuthorizedPlantsList.Cast<Company>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdCompany));
                        int t = GeosApplication.Instance.ActiveUser.IdUser;
                        //OTMService = new OTMServiceController("localhost:6699");
                        int IdPerson = OTMService.GetPeopleDetailsbyEmpCode_V2680(ToObject.RecipientName);
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        //PeopleContact = new People();
                        //OTMService = new OTMServiceController("localhost:6699");
                        PeopleContact = (OTMService.GetContactsByIdPermission_V2680(GeosApplication.Instance.ActiveUser.IdUser, null, salesOwnersIds, GeosApplication.Instance.IdUserPermission, IdPerson));
                        //int personId = Convert.ToInt32(((People)detailView.DataControl.CurrentItem).IdPerson);
                        EditContactViewModel editContactViewModel = new EditContactViewModel();
                        EditContactView editContactView = new EditContactView();
                        editContactViewModel.InIt(PeopleContact);
                        editContactViewModel.OTMInIt();
                        EventHandler handle = delegate { editContactView.Close(); };
                        editContactViewModel.RequestClose += handle;
                        editContactView.DataContext = editContactViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                        //var ownerInfo = (detailView as FrameworkElement);
                        //editContactView.Owner = Window.GetWindow(ownerInfo);
                        editContactView.ShowDialogWindow();
                    }



                }


                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method POToClickCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POToClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        /// <summary>
        /// <!--[pramod.misal][GEOS2-9896][11.11.2025]-->https://helpdesk.emdep.com/browse/GEOS2-9896
        /// </summary>
        /// <param name="parameter"></param>
        private void POCcClickCommandAction(object parameter)
        {
            try
            {
                ToCCName ToObject = parameter as ToCCName;
                GeosApplication.Instance.Logger.Log("Method POToClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                Processing();
                if (ToObject != null)
                {
                    if (ToObject.IsEmdepContact == Visibility.Hidden)
                    {
                        AddContactViewModel addPOContactViewModel = new AddContactViewModel();
                        AddContactView addPOContactView = new AddContactView();
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        int IdCustomerGroup = 0;
                        int IdCustomerplant = 0;

                        if ((SelectedIndexCustomerGroup != -1))
                        {
                            IdCustomerGroup = Customers[SelectedIndexCustomerGroup].IdCustomer;
                        }
                        if ((SelectedIndexCompanyPlant != -1))
                        {
                            IdCustomerplant = CustomerPlants[SelectedIndexCompanyPlant].IdCustomerPlant;
                        }


                        addPOContactViewModel.InitTOFromEmail(ToObject, IdCustomerGroup, IdCustomerplant);
                        EventHandler handle = delegate { addPOContactView.Close(); };
                        addPOContactViewModel.RequestClose += handle;
                        addPOContactView.DataContext = addPOContactViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        addPOContactView.ShowDialogWindow();
                        if (addPOContactViewModel.IsSave == true)
                        {
                            ToObject.IsEmdepContact = Visibility.Visible;
                            ToObject.IsNotEmdepContact = Visibility.Hidden;
                        }

                    }
                    else
                    {

                        List<Company> salesOwners = OTMCommon.Instance.UserAuthorizedPlantsList.Cast<Company>().ToList();
                        var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdCompany));
                        int t = GeosApplication.Instance.ActiveUser.IdUser;
                        //OTMService = new OTMServiceController("localhost:6699");
                        int IdPerson = OTMService.GetPeopleDetailsbyEmpCode_V2680(ToObject.CCName);
                        GeosApplication.Instance.PeopleList = CrmRestStartUp.GetPeoples().ToList();
                        //PeopleContact = new People();
                        PeopleContact = (OTMService.GetContactsByIdPermission_V2680(GeosApplication.Instance.ActiveUser.IdUser, null, salesOwnersIds, GeosApplication.Instance.IdUserPermission, IdPerson));
                        //int personId = Convert.ToInt32(((People)detailView.DataControl.CurrentItem).IdPerson)                                               
                        EditContactViewModel editContactViewModel = new EditContactViewModel();
                        EditContactView editContactView = new EditContactView();
                        editContactViewModel.InIt(PeopleContact);
                        EventHandler handle = delegate { editContactView.Close(); };
                        editContactViewModel.RequestClose += handle;
                        editContactView.DataContext = editContactViewModel;
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                        editContactView.ShowDialogWindow();


                    }



                }


                CloseProcessing();
                GeosApplication.Instance.Logger.Log(string.Format("Method POToClickCommandAction()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method POToClickCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private Action Processing()
        {
            if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                    DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }
            return null;
        }

        void CloseProcessing()
        {
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()...", category: Category.Info, priority: Priority.Low);
            if (DXSplashScreen.IsActive)
            {
                DXSplashScreen.Close();
            }
            GeosApplication.Instance.Logger.Log("Method CloseProcessing()....executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillCustomerGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomerGroupList..."), category: Category.Info, priority: Priority.Low);
                FillServiceUrl();
                IOTMService OTMServiceThread = new OTMServiceController(serviceUrl);
                var customerList = OTMServiceThread.GetAllCustomers_V2630();
                Customers = new ObservableCollection<Customer>(customerList.OrderBy(c => c.CustomerName));
                GeosApplication.Instance.Logger.Log(string.Format("Method FillCustomerGroupList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillCustomerGroupList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillCustomerGroupList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        public string this[string columnName] => throw new NotImplementedException();
        public string Error => throw new NotImplementedException();
        #endregion
    }
}
