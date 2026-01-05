using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.RichEdit;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using Emdep.Geos.UI.Helper;
using Microsoft.Win32;
using DevExpress.Mvvm.UI;
using System.Windows.Documents;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Spreadsheet;
using System.Windows.Controls;

namespace Emdep.Geos.Modules.SRM.ViewModels
{//[Sudhir.jangra][GEOS2-4676][29/08/2023]
    [POCOViewModel]
    public class AddSRMContactViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
         ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISRMService SRMService = new SRMServiceController("localhost:6699");
        #endregion // Services


        #region ICommands 
        public ICommand AddContactAcceptButtonCommand { get; set; }
        public ICommand AddContactCancelButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand UsernameLostFocusCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }

        //Comments
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand RichTextResizingCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        //End Comments

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

        #region Declaration
        private bool isGroupPlantEnable;
        private People contact;
        private People contactData;
        private bool isSave;
        byte[] UserProfileImageByte = null;
        private string firstName;
        private string lastName;
        private string oldMatchName = "";
        private string firstNameMsgStr;
        private string lastNameMsgStr;
        private string informationError;

        private string firstNameMsgStrError = string.Empty;
        private string lastNameMsgStrError = string.Empty;
        private Regex regex;

        public string LocationAddress { get; set; }
        private ObservableCollection<LookupValue> userGenderList;
        Country country;


        private ObservableCollection<Customer> listGroup;
        private ObservableCollection<Company> listPlant;

        private int selectedIndexGender = -1;

        private int selectedIndexCompanyGroup = -1;
        private int selectedIndexCompanyPlant = -1;

        private string salesOwnersIds = "";
        private ObservableCollection<People> selectedContact = new ObservableCollection<People>();
        private string email;
        bool isBusy;

        public bool IsFromFirstnameCmb { get; set; }
        public bool IsFromLastnameCmb { get; set; }
        public bool ShouldKeepValue { get; set; }

        private List<PeopleDetails> allPeopleFirstNameList;
        private List<PeopleDetails> allPeopleLastNameList;
        private List<string> allPeopleFirstNameSrtList;
        private List<string> allPeopleLastNameSrtList;
        private Visibility alertVisibilityFirstName;
        private Visibility alertVisibilityLastName;

        //Comments 
        private ObservableCollection<LogEntriesByContact> contactCommentsList;
        private Object selectedComment;
        private bool showCommentsFlyout;
        private string oldContactComment;
        private string newContactComment;
        private string commentButtonText;
        private bool isRtf;
        private bool isNormal = true;
        private ImageSource userProfileImage;
        private bool isAdd;
        private string commentText;
        private ImageSource contactImage;

        //End Comments

        //[Contact Image, Department, Job Title,Qualification and Change Log ]
        private ObservableCollection<LookupValue> listDepartment;
        private int selectedIndexDepartment = -1;
        private string jobTitle;
        private ObservableCollection<LookupValue> listProductInvolved;
        private int selectedIndexProductInvolved = -1;
        private ObservableCollection<LookupValue> listEmdepAffinity;
        private int selectedIndexEmdepAffinity = -1;
        private ObservableCollection<LookupValue> listInfluenceLevel;
        private int selectedIndexInfluenceLevel = -1;
        private ObservableCollection<Competitor> listCompetitorAffinity;
        private int selectedIndexCompetitorAffinity = -1;
        private ObservableCollection<LogEntriesByContact> listContactChangeLog = new ObservableCollection<LogEntriesByContact>();
        //[Contact Image, Department, Job Title,Qualification and Change Log ]
        private ObservableCollection<Company> entireCompanyPlantList;
        private string visible;


 
        private string phone2;
        #endregion // Declaration

        #region Properties

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }

        public ObservableCollection<LogEntriesByContact> ListContactChangeLog
        {
            get { return listContactChangeLog; }
            set
            {
                listContactChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListContactChangeLog"));
            }
        }

        public ObservableCollection<LookupValue> ListProductInvolved
        {
            get { return listProductInvolved; }
            set
            {
                listProductInvolved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListProductInvolved"));

            }
        }

        public int SelectedIndexProductInvolved
        {
            get { return selectedIndexProductInvolved; }
            set
            {
                selectedIndexProductInvolved = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexProductInvolved"));
            }
        }

        public ObservableCollection<LookupValue> ListEmdepAffinity
        {
            get { return listEmdepAffinity; }
            set
            {
                listEmdepAffinity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListEmdepAffinity"));
            }
        }

        public int SelectedIndexEmdepAffinity
        {
            get { return selectedIndexEmdepAffinity; }
            set
            {
                selectedIndexEmdepAffinity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexEmdepAffinity"));
            }
        }

        public ObservableCollection<LookupValue> ListInfluenceLevel
        {
            get { return listInfluenceLevel; }
            set
            {
                listInfluenceLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListInfluenceLevel"));
            }
        }

        public int SelectedIndexInfluenceLevel
        {
            get { return selectedIndexInfluenceLevel; }
            set
            {
                selectedIndexInfluenceLevel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexInfluenceLevel"));
            }
        }

        public ObservableCollection<Competitor> ListCompetitorAffinity
        {
            get { return listCompetitorAffinity; }
            set
            {
                listCompetitorAffinity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListCompetitorAffinity"));
            }
        }

        public int SelectedIndexCompetitorAffinity
        {
            get { return selectedIndexCompetitorAffinity; }
            set
            {
                selectedIndexCompetitorAffinity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompetitorAffinity"));
            }
        }

        public ImageSource ContactImage
        {
            get { return contactImage; }
            set
            {
                contactImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactImage"));
            }
        }

        // Department & Job Title

        public ObservableCollection<LookupValue> ListDepartment
        {
            get { return listDepartment; }
            set
            {
                listDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListDepartment"));
            }
        }

        public int SelectedIndexDepartment
        {
            get { return selectedIndexDepartment; }
            set
            {
                selectedIndexDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexDepartment"));
            }
        }

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }

        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                //jobTitle = value!=null?value.Trim():value;
                jobTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobTitle"));
            }
        }
        // End Department & Job Title

        //Comments  
        public ObservableCollection<LogEntriesByContact> ContactCommentsList
        {
            get { return contactCommentsList; }
            set
            {
                contactCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactCommentsList"));
            }
        }

        public Object SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }

        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
            }
        }

        public string OldContactComment
        {
            get { return oldContactComment; }
            set
            {
                oldContactComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldContactComment"));
            }
        }

        public string NewContactComment
        {
            get { return newContactComment; }
            set
            {
                newContactComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewContactComment"));
            }
        }

        public string CommentButtonText
        {
            get { return commentButtonText; }
            set
            {
                commentButtonText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentButtonText"));
            }
        }

        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentText"));
            }
        }

        public bool IsAdd
        {
            get { return isAdd; }
            set
            {
                isAdd = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdd"));
            }
        }

        public ImageSource UserProfileImage
        {
            get { return userProfileImage; }
            set
            {
                userProfileImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserProfileImage"));
            }
        }

        public bool IsRtf
        {
            get { return isRtf; }
            set
            {
                isRtf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsRtf"));
            }
        }

        public bool IsNormal
        {
            get { return isNormal; }
            set
            {
                isNormal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNormal"));
            }
        }

        //End Comment

        public bool IsGroupPlantEnable
        {
            get { return isGroupPlantEnable; }
            set
            {
                isGroupPlantEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGroupPlantEnable"));
            }
        }

        public string FirstNameMsgStr
        {
            get { return firstNameMsgStr; }
            set
            {
                firstNameMsgStr = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("FirstNameMsgStr"));
            }
        }

        public string LastNameMsgStr
        {
            get { return lastNameMsgStr; }
            set
            {
                lastNameMsgStr = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("LastNameMsgStr"));
            }
        }

        public Visibility AlertVisibilityFirstName
        {
            get { return alertVisibilityFirstName; }
            set
            {
                alertVisibilityFirstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertVisibilityFirstName"));
            }
        }

        public Visibility AlertVisibilityLastName
        {
            get { return alertVisibilityLastName; }
            set
            {
                alertVisibilityLastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertVisibilityLastName"));
            }
        }

        public List<PeopleDetails> AllPeopleFirstNameList
        {
            get { return allPeopleFirstNameList; }
            set
            {
                allPeopleFirstNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPeopleFirstNameList"));
            }
        }

        public List<PeopleDetails> AllPeopleLastNameList
        {
            get { return allPeopleLastNameList; }
            set
            {
                allPeopleLastNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPeopleLastNameList"));
            }
        }

        public List<string> AllPeopleFirstNameSrtList
        {
            get { return allPeopleFirstNameSrtList; }
            set
            {
                allPeopleFirstNameSrtList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPeopleFirstNameSrtList"));
            }
        }

        public List<string> AllPeopleLastNameSrtList
        {
            get { return allPeopleLastNameSrtList; }
            set
            {
                allPeopleLastNameSrtList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllPeopleLastNameSrtList"));
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                //firstName = value;
                firstName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
                ShowPopupAsPerFirstName(firstName);
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                //lastName = value;
                lastName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
                ShowPopupAsPerLastName(lastName);
            }
        }

        public People Contact
        {
            get { return contact; }
            set
            {
                // contact = value;
                SetProperty(ref contact, value, () => Contact);
            }
        }

        public People ContactData
        {
            get { return contactData; }
            set { contactData = value; }
        }

        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value; }
        }

        public ObservableCollection<LookupValue> UserGenderList
        {
            get { return userGenderList; }
            set
            {
                userGenderList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserGenderList"));
            }
        }

        public int SelectedIndexGender
        {
            get { return selectedIndexGender; }
            set
            {
                selectedIndexGender = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGender"));
            }
        }

        public Country Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }

        public int SelectedIndexCompanyGroup
        {
            get { return selectedIndexCompanyGroup; }
            set
            {
                selectedIndexCompanyGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));

                if (selectedIndexCompanyGroup > 0)
                {
                    //if (GeosApplication.Instance.IdUserPermission == 21)
                    //{
                    //    ListPlant = CrmStartUp.GetSelectedUserCompanyPlantByCustomerId(ListGroup[selectedIndexCompanyGroup].IdCustomer, salesOwnersIds);
                    //}
                    //else
                    //{
                    //    ListPlant = CrmStartUp.GetCompanyPlantByCustomerId(ListGroup[selectedIndexCompanyGroup].IdCustomer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    //}
                    // SelectedIndexCompanyPlant = 0;

                    //List<Company> TempcompanyPlantList = new List<Company>();
                    //TempcompanyPlantList.Insert(0, new Company() { Name = "---" });
                    //TempcompanyPlantList.AddRange(new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == ListGroup[selectedIndexCompanyGroup].IdCustomer).ToList()));
                    ListPlant = new ObservableCollection<Company>();
                    //ListPlant = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == ListGroup[selectedIndexCompanyGroup].IdCustomer));
                    ListPlant = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == ListGroup[SelectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList().GroupBy(cpl => cpl.IdCompany).Select(group => group.First()).ToList());
                    if (ListPlant.Count > 0)
                        SelectedIndexCompanyPlant = 1;
                    else
                        SelectedIndexCompanyPlant = 0;
                }
                else
                {
                    SelectedIndexCompanyPlant = -1;
                    ListPlant = null;
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

                // Added this bcoz These fields are readonly and change on plant index change.
                if (selectedIndexCompanyPlant == -1)
                {
                    Contact.Company.Address = null;
                    Contact.Company.City = null;
                    Contact.Company.Region = null;
                    Contact.Company.ZipCode = null;
                    Contact.Company.Telephone = null;
                    Country = null;
                }
                else
                {
                    Company tempCompany = ListPlant[SelectedIndexCompanyPlant];

                    Contact.Company.Address = tempCompany.Address;
                    Contact.Company.City = tempCompany.City;
                    Contact.Company.Region = tempCompany.Region;
                    Contact.Company.ZipCode = tempCompany.ZipCode;
                    Contact.Company.Telephone = tempCompany.Telephone;
                    Country = tempCompany.Country;
                }
            }
        }

        public ObservableCollection<Customer> ListGroup
        {
            get { return listGroup; }
            set
            {
                listGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListGroup"));
            }
        }

        public ObservableCollection<Company> ListPlant
        {
            get { return listPlant; }
            set
            {
                listPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListPlant"));
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Email"));
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
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

      
        public string Phone2
        {
            get { return phone2; }
            set { phone2 = value; OnPropertyChanged(new PropertyChangedEventArgs("Phone2")); }
        }
        #endregion // Properties

        #region Constructor

        public AddSRMContactViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddContactViewModel...", category: Category.Info, priority: Priority.Low);

                ShouldKeepValue = true;
                regex = new Regex(@"[~`!@#$%^&*()-_+=|\{}':;.,<>/?" + Convert.ToChar(34) + "]");
                Contact = new People();
                Contact.Company = new Company();
                AlertVisibilityFirstName = Visibility.Hidden;
                AlertVisibilityLastName = Visibility.Hidden;

                FillCompanyPlantList();
                FillGroupList();

                AddContactAcceptButtonCommand = new RelayCommand(new Action<object>(AddContactAcceptAction));

                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) =>
                {
                    SendMailtoPerson(obj);
                }));

                AddContactCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnEditValueChanging);
                IsGroupPlantEnable = true;

                //Comments
                ContactCommentsList = new ObservableCollection<LogEntriesByContact>();
                CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
                CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
                AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                RichTextResizingCommand = new DelegateCommand<object>(ResizeRichTextEditor);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //End Comments
                Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                FillLookup();

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
                GeosApplication.Instance.Logger.Log("Constructor AddContactViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in AddContactViewModel() Constructor - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
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
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                    me[BindableBase.GetPropertyName(() => FirstName)] +
                    me[BindableBase.GetPropertyName(() => LastName)] +
                    me[BindableBase.GetPropertyName(() => Email)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexGender)] +           // SelectedIndexGender
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup)] +     // Group
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +     // Plant
                    me[BindableBase.GetPropertyName(() => SelectedIndexDepartment)] +       // Department
                    me[BindableBase.GetPropertyName(() => JobTitle)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexProductInvolved)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexEmdepAffinity)] +
                    me[BindableBase.GetPropertyName(() => InformationError)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexInfluenceLevel)];

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
                string firstNameProp = BindableBase.GetPropertyName(() => FirstName);
                string lastNameProp = BindableBase.GetPropertyName(() => LastName);
                string selectedIndexGenderProp = BindableBase.GetPropertyName(() => SelectedIndexGender);               // SelectedIndexGender
                string selectedIndexCompanyGroupProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyGroup);   // SelectedIndexCompanyGroup
                string selectedIndexCompanyPlantProp = BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant);
                string selectedIndexDepartmentProp = BindableBase.GetPropertyName(() => SelectedIndexDepartment);
                string jobTitleProp = BindableBase.GetPropertyName(() => JobTitle);
                string emailProp = BindableBase.GetPropertyName(() => Email);
                string selectedIndexProductInvolvedProp = BindableBase.GetPropertyName(() => SelectedIndexProductInvolved);
                string selectedIndexEmdepAffinityProp = BindableBase.GetPropertyName(() => SelectedIndexEmdepAffinity);
                string selectedIndexInfluenceLevelProp = BindableBase.GetPropertyName(() => SelectedIndexInfluenceLevel);
                string informationError = BindableBase.GetPropertyName(() => InformationError);

                if (IsFromFirstnameCmb || IsFromLastnameCmb)
                {
                    if (columnName == firstNameProp)
                    {
                        string erroStr = ContactValidation.GetErrorMessage(firstNameProp, FirstName);

                        if (!string.IsNullOrEmpty(erroStr))
                            return erroStr;
                        else
                        if (IsFromFirstnameCmb && string.IsNullOrEmpty(erroStr))
                        {
                            if (!string.IsNullOrEmpty(firstNameMsgStrError))
                            {
                                AlertVisibilityFirstName = Visibility.Hidden;
                                AlertVisibilityLastName = Visibility.Hidden;
                            }

                            return firstNameMsgStrError;
                        }
                    }

                    if (columnName == lastNameProp)
                    {
                        string erroStr = ContactValidation.GetErrorMessage(lastNameProp, LastName);

                        if (!string.IsNullOrEmpty(erroStr))
                            return erroStr;
                        else if (IsFromLastnameCmb && string.IsNullOrEmpty(erroStr))
                        {
                            if (!string.IsNullOrEmpty(lastNameMsgStrError))
                            {
                                AlertVisibilityFirstName = Visibility.Hidden;
                                AlertVisibilityLastName = Visibility.Hidden;
                            }
                            return lastNameMsgStrError;
                        }
                    }
                }
                else
                {
                    if (columnName == firstNameProp)
                    {
                        if (!string.IsNullOrEmpty(firstNameMsgStrError))
                            return firstNameMsgStrError;
                        else
                            return ContactValidation.GetErrorMessage(firstNameProp, FirstName);
                    }
                    if (columnName == lastNameProp)
                    {
                        if (!string.IsNullOrEmpty(lastNameMsgStrError))
                            return lastNameMsgStrError;
                        else
                            return ContactValidation.GetErrorMessage(lastNameProp, LastName);
                    }
                    else if (columnName == selectedIndexGenderProp) // Lead Source
                        return ContactValidation.GetErrorMessage(selectedIndexGenderProp, SelectedIndexGender);
                    else if (columnName == selectedIndexCompanyGroupProp)
                        return ContactValidation.GetErrorMessage(selectedIndexCompanyGroupProp, SelectedIndexCompanyGroup);
                    else if (columnName == selectedIndexCompanyPlantProp)
                        return ContactValidation.GetErrorMessage(selectedIndexCompanyPlantProp, SelectedIndexCompanyPlant);
                    else if (columnName == selectedIndexDepartmentProp)
                        return ContactValidation.GetErrorMessage(selectedIndexDepartmentProp, SelectedIndexDepartment);
                    else if (columnName == jobTitleProp)
                        return ContactValidation.GetErrorMessage(jobTitleProp, JobTitle);
                    else if (columnName == emailProp)
                        return ContactValidation.GetErrorMessage(emailProp, Email);
                    else if (columnName == selectedIndexProductInvolvedProp)
                        return ContactValidation.GetErrorMessage(selectedIndexProductInvolvedProp, SelectedIndexProductInvolved);
                    else if (columnName == selectedIndexEmdepAffinityProp)
                        return ContactValidation.GetErrorMessage(selectedIndexEmdepAffinityProp, SelectedIndexEmdepAffinity);
                    else if (columnName == selectedIndexInfluenceLevelProp)
                        return ContactValidation.GetErrorMessage(selectedIndexInfluenceLevelProp, SelectedIndexInfluenceLevel);
                    else if (columnName == informationError)
                        return ContactValidation.GetErrorMessage(informationError, InformationError);
                }

                return null;
            }
        }

        #endregion

        #region Methods

        public void FillLookup()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLookup ...", category: Category.Info, priority: Priority.Low);

                //Gender
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_USERGENDER"))
                {
                    UserGenderList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_USERGENDER"];
                    SelectedIndexGender = -1;
                }
                else
                {
                    UserGenderList = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(1).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_USERGENDER", UserGenderList);
                    SelectedIndexGender = -1;
                }

                //Department 
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_ACCOUNTDEPARTMENT"))
                {
                    ListDepartment = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_ACCOUNTDEPARTMENT"];
                    SelectedIndexDepartment = -1;
                }
                else
                {
                    ListDepartment = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(21).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_ACCOUNTDEPARTMENT", ListDepartment);
                    SelectedIndexDepartment = -1;
                }

                //PRODUCTINVOLVED 
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_CONTACTPRODUCTINVOLVED"))
                {
                    ListProductInvolved = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_CONTACTPRODUCTINVOLVED"];
                    SelectedIndexProductInvolved = -1;
                }
                else
                {
                    ListProductInvolved = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(24).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_CONTACTPRODUCTINVOLVED", ListProductInvolved);
                    SelectedIndexProductInvolved = -1;
                }

                //EMDEPAFFINITY
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_CONTACTEMDEPAFFINITY"))
                {
                    ListEmdepAffinity = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_CONTACTEMDEPAFFINITY"];
                    SelectedIndexEmdepAffinity = -1;
                }
                else
                {
                    ListEmdepAffinity = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(23).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_CONTACTEMDEPAFFINITY", ListEmdepAffinity);
                    SelectedIndexEmdepAffinity = -1;
                }

                //INFLUENCELEVEL
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_CONTACTINFLUENCELEVEL"))
                {
                    ListInfluenceLevel = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_CONTACTINFLUENCELEVEL"];
                    SelectedIndexInfluenceLevel = -1;
                }
                else
                {
                    ListInfluenceLevel = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(22).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_CONTACTINFLUENCELEVEL", ListInfluenceLevel);
                    SelectedIndexInfluenceLevel = -1;
                }

                // Get Competitor List
                ListCompetitorAffinity = new ObservableCollection<Competitor>(SRMService.GetCompetitors().AsEnumerable());
                ListCompetitorAffinity.Insert(0, new Competitor() { Name = "---" });
                SelectedIndexCompetitorAffinity = 0;

                GeosApplication.Instance.Logger.Log("Method FillLookup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillLookup() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //Comments
        private void CommentButtonCheckedCommandAction(object obj)
        {
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            OldContactComment = "";
            NewContactComment = "";
        }

        private void CommentButtonUncheckedCommandAction(object obj)
        {
            CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString();
            IsAdd = true;
            ShowCommentsFlyout = (ShowCommentsFlyout == true) ? false : true;
            OldContactComment = null;
            NewContactComment = null;
            IsRtf = false;
            IsNormal = true;
        }

        /// <summary>
        /// Method for Add Comment
        /// </summary>
        /// <param name="leadComment"></param>
        public void AddCommentCommandAction(object gcComments)
        {
            var document = ((RichTextBox)gcComments).Document;
            NewContactComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
            string convertedText = string.Empty;
            if (!string.IsNullOrEmpty(NewContactComment.Trim()))
            {
                if (IsRtf)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Rtf);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            convertedText = sr.ReadToEnd();
                        }
                    }
                }
                else if (IsNormal)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                        range2.Save(ms, DataFormats.Text);
                        ms.Seek(0, SeekOrigin.Begin);
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            convertedText = sr.ReadToEnd();
                        }
                    }
                }
            }
            NewContactComment = convertedText;

            if (OldContactComment != null && !string.IsNullOrEmpty(OldContactComment.Trim()) && OldContactComment.Equals(NewContactComment.Trim()))
            {
                ShowCommentsFlyout = false;
                return;
            }

            // Update comment.
            if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString())
            {
                if (!string.IsNullOrEmpty(NewContactComment) && !string.IsNullOrEmpty(NewContactComment.Trim()))
                {
                    LogEntriesByContact comment = ContactCommentsList.FirstOrDefault(x => x.Comments == OldContactComment);
                    if (comment != null)
                    {
                        comment.Comments = string.Copy(NewContactComment.Trim());
                        comment.Datetime = GeosApplication.Instance.ServerDateTime;
                        SelectedComment = comment;
                        if (comment.IdLogEntryByContact != 0)
                            comment.IsUpdated = true;
                        else
                            comment.IsUpdated = false;

                        comment.IsDeleted = false;
                        comment.IsRtfText = comment.IsRtfText;
                        if (IsRtf)
                        {
                            comment.IsRtfText = true;
                            IsRtf = false;
                            RtfToPlaintext();
                        }
                        else if (IsNormal)
                        {
                            comment.IsRtfText = false;
                            CommentText = comment.Comments;
                        }
                    }
                    OldContactComment = null;
                    NewContactComment = null;
                }
            }
            else if (CommentButtonText == System.Windows.Application.Current.FindResource("LeadsEditViewAddComment").ToString()) //Add comment.
            {
                if (!string.IsNullOrEmpty(NewContactComment) && !string.IsNullOrEmpty(NewContactComment.Trim())) // Add Comment
                {
                    if (IsRtf)
                    {
                        LogEntriesByContact comment = new LogEntriesByContact()
                        {
                            User = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Copy(NewContactComment.Trim()),
                            IdContact = Contact.IdPerson,
                            IdLogEntryType = 1,
                            IsUpdated = false,
                            IsDeleted = false,
                            IsRtfText = true
                        };
                        //comment.User.OwnerImage = SetUserProfileImage();
                        ContactCommentsList.Add(comment);
                        SelectedComment = comment;
                        RtfToPlaintext();
                    }
                    else if (IsNormal)
                    {
                        LogEntriesByContact comment = new LogEntriesByContact()
                        {
                            User = new People { IdPerson = GeosApplication.Instance.ActiveUser.IdUser, Name = GeosApplication.Instance.ActiveUser.FirstName, Surname = GeosApplication.Instance.ActiveUser.LastName },
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            Comments = string.Copy(NewContactComment.Trim()),
                            IdContact = Contact.IdPerson,
                            IdLogEntryType = 1,
                            IsUpdated = false,
                            IsDeleted = false,
                            IsRtfText = false
                        };
                        //comment.User.OwnerImage = SetUserProfileImage();
                        ContactCommentsList.Add(comment);
                        SelectedComment = comment;
                        RtfToPlaintext();
                    }

                    OldContactComment = null;
                    NewContactComment = null;
                }
            }
            document.Blocks.Clear();
            ShowCommentsFlyout = false;
            NewContactComment = "";
            IsRtf = false;
            IsNormal = true;
        }

        /// <summary>
        /// Method for Comment Double click
        /// </summary>
        /// <param name="obj"></param>
        private void CommentDoubleClickCommandAction(object obj)
        {
            IsBusy = true;
            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                return;
            }

            if (obj == null) return;

            LogEntriesByContact commentOffer = (LogEntriesByContact)obj;

            if (commentOffer.IdUser == GeosApplication.Instance.ActiveUser.IdUser)
            {
                CommentButtonText = System.Windows.Application.Current.FindResource("LeadsEditViewUpdateComment").ToString();
                IsAdd = false;
                OldContactComment = String.Copy(commentOffer.Comments);
                NewContactComment = String.Copy(commentOffer.Comments);

                if (commentOffer.IsRtfText == true)
                    IsRtf = true;
                else
                    IsNormal = true;
                ShowCommentsFlyout = true;
            }
            else
            {
                OldContactComment = null;
                NewContactComment = null;
                ShowCommentsFlyout = false;
                CustomMessageBox.Show("Not Allowed to update comment!", "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            IsBusy = false;
        }

        /// <summary>
        /// Method For Deleting Comments
        /// </summary>
        /// <param name="obj"></param>
        public void DeleteCommentCommandAction(object parameter)
        {
            LogEntriesByContact commentObject = (LogEntriesByContact)parameter;

            if (GeosApplication.Instance.IsPermissionReadOnly)
                return;

            if (commentObject.IdUser != GeosApplication.Instance.ActiveUser.IdUser)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("EditActivityDeleteComment").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return;
            }

            MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DeleteComment").ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                if (ContactCommentsList != null && ContactCommentsList.Count > 0)
                    ContactCommentsList.Remove(ContactCommentsList.FirstOrDefault(x => x.IdLogEntryByContact == commentObject.IdLogEntryByContact && x.Comments == commentObject.Comments));
            }
            ShowCommentsFlyout = false;
            NewContactComment = null;
        }

        /// <summary>
        /// Method For Resizing Rich Text Box on Load and Content Changed
        /// </summary>
        /// <param name="obj"></param>
        public void ResizeRichTextEditor(object obj)
        {
            RichEditControl edit = (RichEditControl)obj;
            Document currentDocument = edit.Document;
            DocumentLayout currentDocumentLayout = edit.DocumentLayout;

            edit.BeginInvoke(() =>
            {
                SubDocument subDocument = currentDocument.CaretPosition.BeginUpdateDocument();
                DocumentPosition docPosition = subDocument.CreatePosition(((currentDocument.CaretPosition.ToInt() == 0) ? 0 : currentDocument.CaretPosition.ToInt() - 1));

                double height = 0;
                System.Drawing.Point pos = PageLayoutHelper.GetInformationAboutCurrentPage(currentDocumentLayout, docPosition);
                height = DevExpress.Office.Utils.Units.TwipsToPixels(pos, edit.DpiX, edit.DpiY).Y;

                edit.Height = height + 10;
                edit.VerticalScrollValue = 0;
                currentDocument.CaretPosition.EndUpdateDocument(subDocument);
            });
        }

        public void RtfToPlaintext()
        {
            TextRange range = null;
            if (ContactCommentsList.Count > 0)
            {
                if (ContactCommentsList[0].IsRtfText)
                {
                    var rtb = new RichTextBox();
                    var doc = new FlowDocument();
                    MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ContactCommentsList[0].Comments.ToString()));
                    range = new TextRange(doc.ContentStart, doc.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
                else
                {
                    CommentText = ContactCommentsList[0].Comments.ToString();
                }
            }

            if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                CommentText = range.Text;
        }

        /// <summary>
        /// Method for Set Comment User  image.
        /// </summary>
        private ImageSource SetUserProfileImage()
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
                UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(GeosApplication.Instance.ActiveUser.Login);

                if (UserProfileImageByte != null)
                    UserProfileImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        else
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else
                            UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return UserProfileImage;
        }

        /// <summary>
        /// Method for Set Comment User  image.
        /// </summary>
        private void SetUserProfileImage(ObservableCollection<LogEntriesByActivity> ContactCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in ContactCommentsList)
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.People.Login);

                    if (UserProfileImageByte != null)
                        item.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (item.People.IdPersonGender == 1)
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else
                                item.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
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

        //End Comments

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="people"></param>
        public void Init(People people)
        {
            FillCompanyPlantList();
            FillGroupList();

            if (ListGroup != null)
            {
                SelectedIndexCompanyGroup = ListGroup.IndexOf(ListGroup.FirstOrDefault(i => i.IdCustomer == people.Company.Customers[0].IdCustomer));
            }

            if (ListPlant != null)
            {
                SelectedIndexCompanyPlant = ListPlant.IndexOf(ListPlant.FirstOrDefault(i => i.IdCompany == people.Company.IdCompany));
            }

            IsGroupPlantEnable = false;
        }

        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        double StringSimilarityScore(string name, string searchString)
        {
            if (name.Contains(searchString))
            {
                return (double)searchString.Length / (double)name.Length;
            }

            return 0;
        }

        /// <summary>
        /// Method for search similar first name.
        /// </summary>
        /// <param name="fName"></param>
        private void ShowPopupAsPerFirstName(string fName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.PeopleList?.Count > 0)
            {
                AllPeopleFirstNameList = GeosApplication.Instance.PeopleList.ToList();
            }
            else
            {
                AllPeopleFirstNameList = new List<PeopleDetails>();
            }

            IsFromFirstnameCmb = true;
            IsFromLastnameCmb = false;

            if (AllPeopleFirstNameList != null && !string.IsNullOrEmpty(fName))
            {
                if (fName.Length > 1)
                {
                    AllPeopleFirstNameList = AllPeopleFirstNameList.Where(h => h.Name.ToUpper().Contains(fName.ToUpper()) || h.Name.ToUpper().StartsWith(fName.Substring(0, 2).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(fName.Substring(fName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, fName)).ToList();
                    AllPeopleFirstNameSrtList = AllPeopleFirstNameList.Select(pn => pn.Name).ToList();
                }
                else
                {
                    AllPeopleFirstNameList = AllPeopleFirstNameList.Where(h => h.Name.ToUpper().Contains(fName.ToUpper()) || h.Name.ToUpper().StartsWith(fName.Substring(0, 1).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(fName.Substring(fName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, fName)).ToList();
                    AllPeopleFirstNameSrtList = AllPeopleFirstNameList.Select(pn => pn.Name).ToList();
                }
            }
            else
            {
                AllPeopleFirstNameList = new List<PeopleDetails>();
                AllPeopleFirstNameSrtList = new List<string>();
                AlertVisibilityFirstName = Visibility.Hidden;
            }

            if (AllPeopleFirstNameList.Count > 0)
            {
                AlertVisibilityFirstName = Visibility.Visible;
                AlertVisibilityLastName = Visibility.Hidden;
                FirstNameMsgStr = System.Windows.Application.Current.FindResource("AddContactFirstNameMgr").ToString();

                if (FullNameExist())
                {
                    FirstNameMsgStr = string.Format(System.Windows.Application.Current.FindResource("AddContactNameExist").ToString(), FirstName.Trim() + " " + LastName.Trim());
                }
            }
            else
                AlertVisibilityFirstName = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for search similar last name.
        /// </summary>
        /// <param name="lName"></param>
        private void ShowPopupAsPerLastName(string lName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerLastName ...", category: Category.Info, priority: Priority.Low);

            if (GeosApplication.Instance.PeopleList?.Count > 0)
            {
                AllPeopleFirstNameList = GeosApplication.Instance.PeopleList.ToList();
            }
            else
            {
                AllPeopleFirstNameList = new List<PeopleDetails>();
            }
            IsFromFirstnameCmb = false;
            IsFromLastnameCmb = true;

            if (AllPeopleLastNameList != null && !string.IsNullOrEmpty(lName))
            {
                if (lName.Length > 1)
                {
                    AllPeopleLastNameList = AllPeopleLastNameList.Where(h => h.Surname.ToUpper().Contains(lName.ToUpper()) || h.Surname.ToUpper().StartsWith(lName.Substring(0, 2).ToUpper())
                                                            || h.Surname.ToUpper().EndsWith(lName.Substring(lName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Surname, lName)).ToList();
                    AllPeopleLastNameSrtList = AllPeopleLastNameList.Select(pn => pn.Surname).ToList();
                }
                else
                {
                    AllPeopleLastNameList = AllPeopleLastNameList.Where(h => h.Surname.ToUpper().Contains(lName.ToUpper()) || h.Surname.ToUpper().StartsWith(lName.Substring(0, 1).ToUpper())
                                                            || h.Surname.ToUpper().EndsWith(lName.Substring(lName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Surname, lName)).ToList();
                    AllPeopleLastNameSrtList = AllPeopleLastNameList.Select(pn => pn.Surname).ToList();
                }
            }
            else
            {
                AllPeopleLastNameList = new List<PeopleDetails>();
                AllPeopleLastNameSrtList = new List<string>();
                AlertVisibilityLastName = Visibility.Hidden;
            }

            if (AllPeopleLastNameList.Count > 0)
            {
                AlertVisibilityLastName = Visibility.Visible;
                AlertVisibilityFirstName = Visibility.Hidden;
                LastNameMsgStr = System.Windows.Application.Current.FindResource("AddContactLastNameMgr").ToString();
                if (FullNameExist())
                {
                    LastNameMsgStr = string.Format(System.Windows.Application.Current.FindResource("AddContactNameExist").ToString(), FirstName.Trim() + " " + LastName.Trim());
                }
            }
            else
                AlertVisibilityLastName = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerLastName() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for avoid enter digit and special charactar on name. 
        /// </summary>
        /// <param name="e"></param>
        public void OnEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging ...", category: Category.Info, priority: Priority.Low);

            //if (IsFromFirstnameCmb)
            //    firstNameMsgStrError = string.Empty;

            //if (IsFromLastnameCmb)
            //    lastNameMsgStrError = string.Empty;

            int msgcount = 0;
            var newInput = (string)e.NewValue;
            var oldInput = (string)e.OldValue;

            if (!string.IsNullOrEmpty(newInput))
            {
                if (newInput.Count(char.IsDigit) > 0)
                {
                    if (IsFromFirstnameCmb)
                    {
                        //firstNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewDigitMsg").ToString();
                        AlertVisibilityFirstName = Visibility.Visible;
                        AlertVisibilityLastName = Visibility.Hidden;
                        msgcount++;
                    }

                    if (IsFromLastnameCmb)
                    {
                        //lastNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewDigitMsg").ToString();
                        AlertVisibilityFirstName = Visibility.Hidden;
                        AlertVisibilityLastName = Visibility.Visible;
                        msgcount++;
                    }
                }

                MatchCollection matches = regex.Matches(newInput.ToLower().ToString());

                //if (newInput.Count(char.IsPunctuation) > 0)
                if (matches.Count > 0)
                {
                    if (IsFromFirstnameCmb)
                    {
                        //firstNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewspecialcharacterMsg").ToString();
                        AlertVisibilityFirstName = Visibility.Visible;
                        AlertVisibilityLastName = Visibility.Hidden;
                        msgcount++;

                        //if (msgcount == 2)

                        //firstNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewdigitAndspecialcharacterMsg").ToString();
                    }

                    if (IsFromLastnameCmb)
                    {
                        //lastNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewspecialcharacterMsg").ToString();
                        AlertVisibilityFirstName = Visibility.Hidden;
                        AlertVisibilityLastName = Visibility.Visible;
                        msgcount++;

                        //if (msgcount == 2)

                        //lastNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewdigitAndspecialcharacterMsg").ToString();
                    }
                    e.Handled = true;

                }

                string error = EnableValidationAndGetError();
                if (IsFromFirstnameCmb)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                }
                if (IsFromLastnameCmb)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                }
            }
            else ShouldKeepValue = false;

            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Warning if same fullname name is exist in database.
        /// </summary>
        /// <param name="obj"></param>
        private bool FullNameExist()
        {
            GeosApplication.Instance.Logger.Log("Method FullNameExist() ...", category: Category.Info, priority: Priority.Low);

            bool isFullNameExist = false;
            try
            {
                string customername = string.Empty;
                if (FirstName != null && LastName != null)
                {
                    customername = FirstName.Trim() + " " + LastName.Trim();

                    //this condition execute only once.
                    if (string.IsNullOrEmpty(oldMatchName))
                    {

                        bool isNameExistOnDB = GeosApplication.Instance.PeopleList.Any(pl => pl.FullName.ToUpper().Equals(customername.ToUpper()));
                        oldMatchName = customername;
                        if (isNameExistOnDB)
                        {
                            isFullNameExist = true;
                            // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactNameExist").ToString(), customername), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            isBusy = false;
                            GeosApplication.Instance.Logger.Log("Method FullNameExist() executed Successfully.", category: Category.Info, priority: Priority.Low);
                            return isFullNameExist;
                        }
                    }

                    if (!oldMatchName.Equals(customername))
                    {
                        bool isNameExistOnDB = GeosApplication.Instance.PeopleList.Any(pl => pl.FullName.ToUpper().Equals(customername.ToUpper()));
                        oldMatchName = customername;
                        if (isNameExistOnDB)
                        {
                            isFullNameExist = true;
                            // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactNameExist").ToString(), customername), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            isBusy = false;
                            GeosApplication.Instance.Logger.Log("Method UsernameLostFocusCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
                            return isFullNameExist;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFullNameExist = false;
                GeosApplication.Instance.Logger.Log(string.Format("Error in UsernameLostFocusCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            return isFullNameExist;
        }

        /// <summary>
        /// Method for check email address is already exist or not.
        /// </summary>
        /// <returns></returns>
        private bool IsEmailAddressExist()
        {
            GeosApplication.Instance.Logger.Log("Method IsEmailAddressExist() ...", category: Category.Info, priority: Priority.Low);
            bool isEmailExist = false;
            try
            {
                isEmailExist = GeosApplication.Instance.PeopleList.Any(pl => pl.Email.ToUpper().Equals(Email.ToUpper()));

                if (isEmailExist)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactEmailExist").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    GeosApplication.Instance.Logger.Log("Method IsEmailAddressExist() executed Successfully.", category: Category.Info, priority: Priority.Low);
                }

                return isEmailExist;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in IsEmailAddressExist() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                return isEmailExist;
            }
        }

        public void AddContactAcceptAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method AddContactAcceptAction()...", category: Category.Info, priority: Priority.Low);

            IsFromFirstnameCmb = false;
            IsFromLastnameCmb = false;
            IsBusy = true;
            InformationError = null;

            string error = EnableValidationAndGetError();
            if (string.IsNullOrEmpty(error))
                InformationError = null;
            else
                InformationError = "";

            PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGender"));         // Gender
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyGroup"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCompanyPlant"));   // Plant
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCountry"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexDepartment"));
            PropertyChanged(this, new PropertyChangedEventArgs("JobTitle"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexProductInvolved"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexEmdepAffinity"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexInfluenceLevel"));
            PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));

            if (error != null)
            {
                IsBusy = false;
                return;
            }

            if (IsEmailAddressExist())
            {
                IsBusy = false;
                return;
            }

            // Contact
            Contact.IdPersonType = 1;
            if (!string.IsNullOrEmpty(FirstName))
                Contact.Name = FirstName.Trim();
            if (!string.IsNullOrEmpty(LastName))
                Contact.Surname = LastName.Trim();
            if (!string.IsNullOrEmpty(Phone2))//[Sudhir.Jangra][GEOS2-4676]
            {
                Contact.Phone2 = Phone2;
            }
            Contact.IdPersonGender = (byte)UserGenderList[SelectedIndexGender].IdLookupValue;

            Contact.Company.IdCustomer = ListGroup[SelectedIndexCompanyGroup].IdCustomer;
            Contact.IdSite = ListPlant[SelectedIndexCompanyPlant].IdCompany;
            Contact.Company.IdCompany = ListPlant[SelectedIndexCompanyPlant].IdCompany;
            Contact.Company.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
            Contact.Email = Email.Trim();
            Contact.IdCompanyDepartment = ListDepartment[SelectedIndexDepartment].IdLookupValue;
            Contact.JobTitle = JobTitle.Trim();
            Contact.IdContactProductInvolved = ListProductInvolved[SelectedIndexProductInvolved].IdLookupValue;
            Contact.IdContactEmdepAffinity = ListEmdepAffinity[SelectedIndexEmdepAffinity].IdLookupValue;
            Contact.CompanyDepartment = new LookupValue() { IdLookupKey = ListDepartment[SelectedIndexDepartment].IdLookupKey, IdLookupValue = ListDepartment[SelectedIndexDepartment].IdLookupValue, Value = ListDepartment[SelectedIndexDepartment].Value };
            Contact.ProductInvolved = new LookupValue() { IdLookupKey = ListProductInvolved[SelectedIndexProductInvolved].IdLookupKey, IdLookupValue = ListProductInvolved[SelectedIndexProductInvolved].IdLookupValue, Value = ListProductInvolved[SelectedIndexProductInvolved].Value };
            Contact.InfluenceLevel = new LookupValue() { IdLookupKey = ListInfluenceLevel[SelectedIndexInfluenceLevel].IdLookupKey, IdLookupValue = ListInfluenceLevel[SelectedIndexInfluenceLevel].IdLookupValue, Value = ListInfluenceLevel[SelectedIndexInfluenceLevel].Value };
            Contact.EmdepAffinity = new LookupValue() { IdLookupKey = ListEmdepAffinity[SelectedIndexEmdepAffinity].IdLookupKey, IdLookupValue = ListEmdepAffinity[SelectedIndexEmdepAffinity].IdLookupValue, Value = ListEmdepAffinity[SelectedIndexEmdepAffinity].Value };
            Contact.IdContactInfluenceLevel = ListInfluenceLevel[SelectedIndexInfluenceLevel].IdLookupValue;

            if (SelectedIndexCompetitorAffinity > 0)
                Contact.IdCompetitor = ListCompetitorAffinity[SelectedIndexCompetitorAffinity].IdCompetitor;
            else
                Contact.IdCompetitor = null;

            if (Emdep.Geos.UI.Helper.ImageEditHelper.Base64String != null)
            {
                Contact.ImageText = Emdep.Geos.UI.Helper.ImageEditHelper.Base64String;
                byte[] imageBytes = Convert.FromBase64String(Contact.ImageText);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                Contact.OwnerImage = byteArrayToImage(imageBytes);
                ContactImage = Contact.OwnerImage;
            }
            else
            {
                Contact.ImageText = null;
            }

            Contact.OwnerImage = null;
            Contact.CommentsByContact = new List<LogEntriesByContact>();

            List<LogEntriesByContact> newList = ContactCommentsList.Where(x => x.IdLogEntryByContact == 0 || x.IsDeleted || x.IsUpdated).ToList();
            foreach (LogEntriesByContact item in newList)
            {
                string oldTempComment = item.Comments;
                string newTempComment = string.Empty;
                string pattern = "[^\\w]";
                string[] words = null;
                int i = 0;
                int count = 0;

                LogEntriesByContact newobj = new LogEntriesByContact();
                newobj.IdLogEntryByContact = item.IdLogEntryByContact;
                newobj.IdContact = item.IdContact;
                newobj.IdUser = item.IdUser;
                newobj.Comments = item.Comments;
                newobj.IsRtfText = item.IsRtfText;
                newobj.Datetime = item.Datetime;
                newobj.IdLogEntryType = item.IdLogEntryType;
                newobj.IsDeleted = item.IsDeleted;
                newobj.IsUpdated = item.IsUpdated;
                Contact.CommentsByContact.Add(newobj);

                if (item.IdLogEntryByContact == 0)
                {
                    if (item.IsRtfText)
                    {
                        TextRange range = null;
                        var rtb = new RichTextBox();
                        var doc = new FlowDocument();
                        MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(item.Comments.ToString()));
                        range = new TextRange(doc.ContentStart, doc.ContentEnd);
                        range.Load(stream, DataFormats.Rtf);

                        if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                            oldTempComment = range.Text;

                        words = Regex.Split(oldTempComment, pattern, RegexOptions.IgnoreCase);
                        for (i = words.GetLowerBound(0); i <= words.GetUpperBound(0); i++)
                        {
                            if (words[i].ToString() == string.Empty)
                                count = count - 1;
                            count = count + 1;
                        }
                        if (count > 10)
                        {
                            for (int j = 0; j <= 10; j++)
                            {
                                newTempComment += words[j] + " ";
                            }

                            newTempComment = newTempComment.TrimEnd();
                            newTempComment += "...";
                        }
                        else
                        {
                            newTempComment = oldTempComment;
                            newTempComment = newTempComment.TrimEnd();
                        }

                        ListContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                    }
                    else
                    {
                        words = Regex.Split(oldTempComment, pattern, RegexOptions.IgnoreCase);
                        for (i = words.GetLowerBound(0); i <= words.GetUpperBound(0); i++)
                        {
                            if (words[i].ToString() == string.Empty)
                                count = count - 1;
                            count = count + 1;
                        }

                        if (count > 10)
                        {
                            for (int j = 0; j <= 10; j++)
                            {
                                newTempComment += words[j] + " ";
                            }

                            newTempComment = newTempComment.TrimEnd();
                            newTempComment += "...";
                        }
                        else
                        {
                            newTempComment = oldTempComment;
                            newTempComment = newTempComment.TrimEnd();
                        }
                        ListContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                    }
                }
            }

            AddChangeLog(Contact);

            Contact.LogEntriesByContact = new List<LogEntriesByContact>();
            Contact.LogEntriesByContact = ListContactChangeLog.ToList();

            if (!IsSave)
            {
                try
                {
                    Contact.IdCreator = GeosApplication.Instance.ActiveUser.IdUser;
                    // ContactData = SRMService.AddContact_V2033(Contact);
                    //[sudhir.jangra][GEOS2-4676][Added Phone2 Field]
                    ContactData = SRMService.AddSRMContact_V2430(Contact);
                    IsSave = true;
                    ContactData.Company.Name = ListPlant[SelectedIndexCompanyPlant].Name;
                    ContactData.Company.Customers.Add(ListGroup[SelectedIndexCompanyGroup]);
                    ContactData.Company.Name = ListPlant[SelectedIndexCompanyPlant].SiteNameWithoutCountry;
                    ContactData.IdPersonGender = (byte)UserGenderList[SelectedIndexGender].IdLookupValue;
                    ContactData.UserGender = UserGenderList[SelectedIndexGender].Value;
                    ContactData.Company.Country = Country;

                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method AddContactAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AddContactAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in AddContactAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AddContactAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            else
            {
                ContactData = null;
                IsSave = false;
            }

            IsBusy = true;
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        /// <summary>
        ///  This method is for to get image in bitmap.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        /// <summary>
        /// This method is for to convert from Bytearray to ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public ImageSource byteArrayToImage(byte[] byteArrayIn)
        {
            ImageSource imgSrc = null;
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(byteArrayIn);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                biImg.DecodePixelHeight = 10;
                biImg.DecodePixelWidth = 10;

                imgSrc = biImg as ImageSource;

                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return imgSrc;
        }

        /// <summary>
        /// Method for send mail.
        /// </summary>
        /// <param name="obj"></param>
        public void SendMailtoPerson(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(obj)))
                {
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson ...", category: Category.Info, priority: Priority.Low);
                IsBusy = true;
                string emailAddess = Convert.ToString(obj);
                string command = "mailto:" + emailAddess;
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo.FileName = command;
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.RedirectStandardOutput = false;
                myProcess.Start();
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SendMailtoPerson() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Method For Contact ChangeLog
        /// </summary>
        private void AddChangeLog(People objContact)
        {
            GeosApplication.Instance.Logger.Log("Method AddChangeLog ...", category: Category.Info, priority: Priority.Low);

            if (objContact != null)
            {
                string ContactName = string.Empty;
                string CustomerName = string.Empty;
                ContactName = "'" + objContact.FullName + "'";
                CustomerName = "'" + ListGroup[SelectedIndexCompanyGroup].CustomerName + "-" + ListPlant[SelectedIndexCompanyPlant].SiteNameWithoutCountry + "'";
                ListContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("AddedContactChangeLog").ToString(), ContactName, CustomerName), IdLogEntryType = 2 });
            }

            GeosApplication.Instance.Logger.Log("Method AddChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for fill emdep Group list.
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                    else
                    {
                        //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                        ListGroup = new ObservableCollection<Customer>(SRMService.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", ListGroup);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    else
                    {
                        //ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));

                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        ListGroup = new ObservableCollection<Customer>(SRMService.GetCompanyGroup_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", ListGroup);
                    }
                }
                SelectedIndexCompanyGroup = 0;
                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for fill Company list.
        /// </summary>
        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT21"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT21"];
                    else
                    {
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(SRMService.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)(GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"]);
                    else
                    {
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));

                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(SRMService.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                SRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
