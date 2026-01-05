using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.RichEdit;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using Emdep.Geos.UI.Helper;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    [POCOViewModel]
    public class EditContactViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog
        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]
        #endregion

        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion // Services

        #region Declaration

        private bool isSave;
        byte[] UserProfileImageByte = null;

        private string firstName;
        private string lastName;
        private string firstNameMsgStr;
        private string lastNameMsgStr;
        private List<PeopleDetails> allPeopleFirstNameList;
        private List<PeopleDetails> allPeopleLastNameList;
        private List<string> allPeopleFirstNameSrtList;
        private List<string> allPeopleLastNameSrtList;
        private Visibility alertVisibilityFirstName;
        private Visibility alertVisibilityLastName;
        private string informationError;

        public bool IsFromFirstnameCmb { get; set; }
        public bool IsFromLastnameCmb { get; set; }

        private string email;
        private string firstNameMsgStrError = string.Empty;
        private string lastNameMsgStrError = string.Empty;
        private Regex regex;
        bool isBusy;
        public bool ShouldKeepValueForFirstName { get; set; }
        public bool ShouldKeepValueforLastName { get; set; }

        private ObservableCollection<LookupValue> userGenderList;
        //private ObservableCollection<Country> listCountry;
        private Country country;
        private ObservableCollection<Customer> listGroup;
        private ObservableCollection<Company> listPlant;

        private int selectedIndexGender;
        //private int selectedIndexCountry = 0;
        private int selectedIndexCompanyGroup;
        private int selectedIndexCompanyPlant;

        private string salesOwnersIds = "";
        private ObservableCollection<People> selectedContact = new ObservableCollection<People>();
        private List<Activity> contactActivityList;
        private Activity selectedActivity;

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
        //End Comments

        //[Contact Image, Department, Job Title,Qualification and Change Log ]
        private ObservableCollection<LookupValue> listDepartment;
        private int selectedIndexDepartment = -1;
        private string jobTitle;
        private ImageSource contactImage;
        private ObservableCollection<LookupValue> listProductInvolved;
        private int selectedIndexProductInvolved = -1;
        private ObservableCollection<LookupValue> listEmdepAffinity;
        private int selectedIndexEmdepAffinity = -1;
        private ObservableCollection<LookupValue> listInfluenceLevel;
        private int selectedIndexInfluenceLevel = -1;
        private ObservableCollection<Competitor> listCompetitorAffinity;
        private int selectedIndexCompetitorAffinity = -1;
        private ObservableCollection<LogEntriesByContact> listContactChangeLog = new ObservableCollection<LogEntriesByContact>();
        private ObservableCollection<LogEntriesByContact> tempContactChangeLog = new ObservableCollection<LogEntriesByContact>();
        private string phone;
        //[Contact Image, Department, Job Title,Qualification and Change Log ]

        private ObservableCollection<Company> entireCompanyPlantList;

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

        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Phone"));
            }
        }

        public ObservableCollection<LogEntriesByContact> TempContactChangeLog
        {
            get { return tempContactChangeLog; }
            set
            {
                tempContactChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempContactChangeLog"));
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

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
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

        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                jobTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobTitle"));
            }
        }

        // End Department & Job Title

        //Comments 
        public virtual string ResultFileName { get; protected set; }
        public virtual bool DialogResult { get; protected set; }

        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }

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
        public Activity SelectedActivity
        {
            get { return selectedActivity; }
            set
            {
                selectedActivity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedActivity"));
            }
        }

        public string FirstNameMsgStr
        {
            get { return firstNameMsgStr; }
            set
            {
                firstNameMsgStr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FirstNameMsgStr"));
            }
        }

        public string LastNameMsgStr
        {
            get { return lastNameMsgStr; }
            set
            {
                lastNameMsgStr = value;
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

        public ObservableCollection<People> SelectedContact
        {
            get { return selectedContact; }
            set
            {
                //selectedContact = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("SelectedContact"));
                SetProperty(ref selectedContact, value, () => SelectedContact);
            }
        }

        public People Contact { get; set; }

        public People initialContact { get; set; }

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
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
                ShowPopupAsPerLastName(LastName);
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

                try
                {
                    GeosApplication.Instance.Logger.Log("Public Property SelectedIndexCompanyGroup ...", category: Category.Info, priority: Priority.Low);

                    if (selectedIndexCompanyGroup > 0)
                    {
                        ListPlant = new ObservableCollection<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == ListGroup[selectedIndexCompanyGroup].IdCustomer || cpl.Name == "---").ToList().AsEnumerable());
                        SelectedIndexCompanyPlant = ListPlant.IndexOf(ListPlant.FirstOrDefault(i => i.IdCompany == SelectedContact[0].Company.IdCompany));

                        if (SelectedIndexCompanyPlant == -1)
                            SelectedIndexCompanyPlant = 1;
                    }
                    else
                    {
                        SelectedIndexCompanyPlant = -1;
                        ListPlant = null;
                    }

                    GeosApplication.Instance.Logger.Log("Public Property SelectedIndexCompanyGroup executed successfully", category: Category.Info, priority: Priority.Low);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in SelectedIndexCompanyGroup Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    SelectedContact[0].Company.Address = null;
                    SelectedContact[0].Company.City = null;
                    SelectedContact[0].Company.Region = null;
                    SelectedContact[0].Company.ZipCode = null;
                    SelectedContact[0].Company.Telephone = null;
                    Country = null;
                }
                else
                {
                    Company tempCompany = ListPlant[SelectedIndexCompanyPlant];

                    SelectedContact[0].Company.Address = tempCompany.Address;
                    SelectedContact[0].Company.City = tempCompany.City;
                    SelectedContact[0].Company.Region = tempCompany.Region;
                    SelectedContact[0].Company.ZipCode = tempCompany.ZipCode;
                    SelectedContact[0].Company.Telephone = tempCompany.Telephone;
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

        public List<Activity> ContactActivityList
        {
            get { return contactActivityList; }
            set
            {
                contactActivityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactActivityList"));
            }
        }

        #endregion // Properties

        #region ICommands

        public ICommand EditContactAcceptButtonCommand { get; set; }
        public ICommand EditContactCancelButtonCommand { get; set; }
        public ICommand HyperlinkForEmail { get; set; }
        public ICommand OnTextEditValueChangingFirstNameCommand { get; set; }
        public ICommand OnTextEditValueChangingLastNameCommand { get; set; }
        public ICommand ActivitiesGridDoubleClickCommand { get; set; }
        public ICommand AddNewActivityCommand { get; set; }


        //Comments
        public ICommand CommentButtonCheckedCommand { get; set; }
        public ICommand CommentButtonUncheckedCommand { get; set; }
        public ICommand AddNewCommentCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand RichTextResizingCommand { get; set; }
        public ICommand ExcelexportButtonCheckedCommand { get; set; }

        //public ICommand DeleteContactImageCommand { get; set; }

        public ICommand EditCustomerDetailsCommand { get; set; }

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

        #region Constructor

        public EditContactViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EditContactViewModel...", category: Category.Info, priority: Priority.Low);

                regex = new Regex(@"[~`!@#$%^&*()-_+=|\{}':;.,<>/?" + Convert.ToChar(34) + "]");

                HyperlinkForEmail = new DelegateCommand<object>(new Action<object>((obj) =>
                {
                    SendMailtoPerson(obj);
                }));

                EditContactAcceptButtonCommand = new DelegateCommand<object>(EditContactAcceptAction);
                EditContactCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                OnTextEditValueChangingFirstNameCommand = new DelegateCommand<EditValueChangingEventArgs>(OnEditValueChangingForFirstName);
                OnTextEditValueChangingLastNameCommand = new DelegateCommand<EditValueChangingEventArgs>(OnEditValueChangingForLastName);

                ActivitiesGridDoubleClickCommand = new DelegateCommand<object>(EditActivityViewWindowShow);
                AddNewActivityCommand = new DelegateCommand<object>(AddActivityViewWindowShow);

                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                }

                //Comments

                ContactCommentsList = new ObservableCollection<LogEntriesByContact>();
                CommentButtonCheckedCommand = new DelegateCommand<object>(CommentButtonCheckedCommandAction);
                CommentButtonUncheckedCommand = new DelegateCommand<object>(CommentButtonUncheckedCommandAction);
                AddNewCommentCommand = new DelegateCommand<object>(AddCommentCommandAction);
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);
                RichTextResizingCommand = new DelegateCommand<object>(ResizeRichTextEditor);
                ExcelexportButtonCheckedCommand = new DelegateCommand<object>(ExportToExcel);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                //End Comments

                FillLookup();

                EditCustomerDetailsCommand = new DelegateCommand<object>(EditCustomerDetailsCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor EditContactViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor EditContactViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region validation

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
                    me[BindableBase.GetPropertyName(() => SelectedIndexCompanyPlant)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexDepartment)] +
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
                        else
                            if (IsFromLastnameCmb && string.IsNullOrEmpty(erroStr))
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

        public void EditCustomerDetailsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditCustomerDetailsCommandAction...", category: Category.Info, priority: Priority.Low);

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
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                List<Company> TempCompany = new List<Company>();
                int companyId = ListPlant[SelectedIndexCompanyPlant].IdCompany;
                TempCompany.Add(CrmStartUp.GetCompanyDetailsById(companyId));
                EditCustomerViewModel editCustomerViewModel = new EditCustomerViewModel();
                EditCustomerView editCustomerView = new EditCustomerView();
                editCustomerViewModel.InIt(TempCompany);
                EventHandler handle = delegate { editCustomerView.Close(); };
                editCustomerViewModel.RequestClose += handle;
                editCustomerView.DataContext = editCustomerViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                editCustomerView.ShowDialog();

                if (editCustomerViewModel.IsCustomerDetailsModified == true)
                {
                    try
                    {
                        if (GeosApplication.Instance.IdUserPermission == 21)
                        {
                            if (GeosApplication.Instance.SelectedSalesOwnerUsersList != null)
                            {
                                List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                                var salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));

                                ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP21");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", ListGroup);
                                EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                                GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT21");
                                GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                            }
                        }
                        else
                        {
                            ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                            GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYGROUP");
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", ListGroup);
                            EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                            GeosApplication.Instance.ObjectPool.Remove("CRM_COMPANYPLANT");
                            GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in EditCustomerDetailsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                    int indexGroup = editCustomerViewModel.SelectedIndexCompanyGroup;
                    SelectedIndexCompanyGroup = indexGroup;
                    SelectedIndexCompanyPlant = ListPlant.IndexOf(ListPlant.FirstOrDefault(j => j.IdCompany == editCustomerViewModel.SelectedCompanyList[0].IdCompany));
                }
                GeosApplication.Instance.Logger.Log("Method EditCustomerDetailsCommandAction() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditCustomerDetailsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillLookup()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLookup ...", category: Category.Info, priority: Priority.Low);

                //Gender
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_USERGENDER"))
                {
                    UserGenderList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_USERGENDER"];
                }
                else
                {
                    UserGenderList = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(1).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_USERGENDER", UserGenderList);
                }

                //Department 
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_ACCOUNTDEPARTMENT"))
                {
                    ListDepartment = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_ACCOUNTDEPARTMENT"];
                }
                else
                {
                    ListDepartment = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(21).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_ACCOUNTDEPARTMENT", ListDepartment);
                }


                //PRODUCTINVOLVED
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_CONTACTPRODUCTINVOLVED"))
                {
                    ListProductInvolved = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_CONTACTPRODUCTINVOLVED"];
                }
                else
                {
                    ListProductInvolved = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(24).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_CONTACTPRODUCTINVOLVED", ListProductInvolved);
                }

                //EMDEPAFFINITY
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_CONTACTEMDEPAFFINITY"))
                {
                    ListEmdepAffinity = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_CONTACTEMDEPAFFINITY"];
                }
                else
                {
                    ListEmdepAffinity = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(23).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_CONTACTEMDEPAFFINITY", ListEmdepAffinity);
                }


                //INFLUENCELEVEL
                if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_CONTACTINFLUENCELEVEL"))
                {
                    ListInfluenceLevel = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["CRM_CONTACTINFLUENCELEVEL"];
                }
                else
                {
                    ListInfluenceLevel = new ObservableCollection<Data.Common.Epc.LookupValue>(CrmStartUp.GetLookupValues(22).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("CRM_CONTACTINFLUENCELEVEL", ListInfluenceLevel);
                }

                // Get Competitor List
                ListCompetitorAffinity = new ObservableCollection<Competitor>(CrmStartUp.GetCompetitors().AsEnumerable());
                ListCompetitorAffinity.Insert(0, new Competitor() { Name = "---" });

                GeosApplication.Instance.Logger.Log("Method FillLookup() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(String.Format("Error in FillLookup() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
            string TempOldComment = string.Empty;
            string TempNewComment = string.Empty;

            if (IsRtf)
            {
                var document = ((RichTextBox)gcComments).Document;
                NewContactComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                TempNewComment = NewContactComment;
                string convertedText = string.Empty;

                if (!string.IsNullOrEmpty(NewContactComment.Trim()))
                {
                    // if (IsRtf)
                    // {
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
                    //}
                    //else if (IsNormal)
                    //{
                    //    using (MemoryStream ms = new MemoryStream())
                    //    {
                    //        TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                    //        range2.Save(ms, DataFormats.Text);
                    //        ms.Seek(0, SeekOrigin.Begin);
                    //        using (StreamReader sr = new StreamReader(ms))
                    //        {
                    //            convertedText = sr.ReadToEnd();
                    //        }
                    //    }
                    //}
                }
                NewContactComment = convertedText;
            }
            else
            {
                var document = ((RichTextBox)gcComments).Document;
                NewContactComment = new TextRange(document.ContentStart, document.ContentEnd).Text.Trim();
                TempNewComment = NewContactComment;
                //string convertedText = string.Empty;
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    TextRange range2 = new TextRange(document.ContentStart, document.ContentEnd);
                //    range2.Save(ms, DataFormats.Text);
                //    ms.Seek(0, SeekOrigin.Begin);
                //    using (StreamReader sr = new StreamReader(ms))
                //    {
                //        convertedText = sr.ReadToEnd();
                //    }
                //}
                //TempNewComment = NewContactComment;
            }


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

                        if (comment.IsRtfText)
                        {
                            TextRange range = null;
                            var rtb = new RichTextBox();
                            var doc = new FlowDocument();
                            MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(OldContactComment.ToString()));
                            range = new TextRange(doc.ContentStart, doc.ContentEnd);
                            range.Load(stream, DataFormats.Rtf);

                            if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                                TempOldComment = range.Text;
                        }
                        else
                        {
                            TempOldComment = OldContactComment;
                        }

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

                        TempOldComment = "'" + TempOldComment + "'";
                        TempOldComment = TempOldComment.Replace("\r\n", "").TrimEnd();

                        TempNewComment = "'" + TempNewComment + "'";
                        TempNewComment = TempNewComment.TrimEnd();

                        TempContactChangeLog.Add(new LogEntriesByContact() { IdLogEntryByContact = comment.IdLogEntryByContact, IdContact = SelectedContact[0].IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentUpdated").ToString(), TempOldComment, TempNewComment), IdLogEntryType = 2 });
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
                        comment.User.OwnerImage = SetUserProfileImage();
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

                        comment.User.OwnerImage = SetUserProfileImage();

                        ContactCommentsList.Add(comment);
                        SelectedComment = comment;
                        RtfToPlaintext();
                    }

                    OldContactComment = null;
                    NewContactComment = null;
                }
            }

            //document.Blocks.Clear();
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
                return;

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


        // Export Excel .xlsx
        private void ExportToExcel(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportContactCommentGridButtonCommandAction ...", category: Category.Info, priority: Priority.Low);

                TableView tblViewGrid = obj as TableView;
                if (tblViewGrid.Grid.VisibleRowCount > 0)
                {
                    SaveFileDialogService.DefaultExt = "xlsx";
                    SaveFileDialogService.DefaultFileName = "ContactComments";
                    SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();

                    if (!DialogResult)
                        ResultFileName = string.Empty;
                    else
                    {
                        IsBusy = true;
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
                                return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }

                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        TextRange range = null;
                        SpreadsheetControl control = new SpreadsheetControl();
                        Worksheet ws = control.ActiveWorksheet;
                        ws.Name = "ActivityComments";
                        ws.Cells[0, 0].Value = "User";
                        ws.Cells[0, 0].Font.Bold = true;
                        ws.Cells[0, 0].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                        ws.Cells[0, 0].ColumnWidth = 400;
                        ws.Cells[0, 0].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        ws.Cells[0, 1].Value = "Comments Date";
                        ws.Cells[0, 1].Font.Bold = true;
                        ws.Cells[0, 1].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                        ws.Cells[0, 1].ColumnWidth = 400;
                        ws.Cells[0, 1].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);

                        ws.Cells[0, 2].Value = "Comments";
                        ws.Cells[0, 2].Font.Bold = true;
                        ws.Cells[0, 2].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                        ws.Cells[0, 2].ColumnWidth = 1000;
                        ws.Cells[0, 2].Borders.SetAllBorders(System.Drawing.Color.Black, DevExpress.Spreadsheet.BorderLineStyle.Thin);
                        int counter = 1;

                        if (ContactCommentsList.Count > 0)
                        {
                            for (int i = 0; i < ContactCommentsList.Count; i++)
                            {
                                var rtb = new RichTextBox();
                                var doc = new FlowDocument();
                                MemoryStream stream = new MemoryStream(ASCIIEncoding.Default.GetBytes(ContactCommentsList[i].Comments.ToString()));
                                range = new TextRange(doc.ContentStart, doc.ContentEnd);
                                range.Load(stream, DataFormats.Rtf);
                                ws.Cells[counter, 0].Value = ContactCommentsList[i].User.FullName;
                                ws.Cells[counter, 0].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                                ws.Cells[counter, 1].Value = ContactCommentsList[i].Datetime;
                                ws.Cells[counter, 1].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                                ws.Cells[counter, 2].Value = range.Text;
                                ws.Cells[counter, 2].Alignment.Vertical = SpreadsheetVerticalAlignment.Top;
                                ws.Cells[counter, 2].Alignment.WrapText = true;
                                counter++;
                            }
                        }

                        control.SaveDocument(ResultFileName);
                        IsBusy = false;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        System.Diagnostics.Process.Start(ResultFileName);
                    }
                }

                GeosApplication.Instance.Logger.Log("Method ExportContactCommentGridButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportContactCommentGridButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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
        private void SetUserProfileImage(ObservableCollection<LogEntriesByContact> ContactCommentsList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);

                foreach (var item in ContactCommentsList)
                {
                    UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(item.User.Login);

                    if (UserProfileImageByte != null)
                        item.User.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (item.User.IdPersonGender == 1)
                                item.User.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else
                                item.User.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (item.User.IdPersonGender == 1)
                                item.User.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else
                                item.User.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }

                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SetUserProfileImage() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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
        /// Method for add new activity.
        /// </summary>
        /// <param name="obj"></param>
        private void AddActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);
                DXSplashScreen.Show<SplashScreenView>();
                AddActivityView addActivityView = new AddActivityView();
                AddActivityViewModel addActivityViewModel = new AddActivityViewModel();

                List<Activity> activityList = new List<Activity>();

                //code for add Contact Detail.

                Activity activity = new Activity();
                activity.ActivityLinkedItem = new List<ActivityLinkedItem>();

                //Account
                ActivityLinkedItem activityLinkedItemAccount = new ActivityLinkedItem();
                activityLinkedItemAccount.IdLinkedItemType = 42; //Account
                activityLinkedItemAccount.IdSite = ListPlant[SelectedIndexCompanyPlant].IdCompany;
                activityLinkedItemAccount.Company = ListPlant[SelectedIndexCompanyPlant];
                activityLinkedItemAccount.Customer = ListGroup[SelectedIndexCompanyGroup];
                activityLinkedItemAccount.Name = ListGroup[SelectedIndexCompanyGroup].CustomerName + " - " + ListPlant[SelectedIndexCompanyPlant].SiteNameWithoutCountry;

                activityLinkedItemAccount.LinkedItemType = new LookupValue();
                activityLinkedItemAccount.LinkedItemType.IdLookupValue = 42; //Account
                activityLinkedItemAccount.LinkedItemType.Value = System.Windows.Application.Current.FindResource("ActivityAccount").ToString();
                activityLinkedItemAccount.IsVisible = false;

                activity.ActivityLinkedItem.Add(activityLinkedItemAccount);

                //Contact
                ActivityLinkedItem activityLinkedItemContact = new ActivityLinkedItem();
                activityLinkedItemContact.IdLinkedItemType = 43; //Contact
                activityLinkedItemContact.People = SelectedContact[0];
                activityLinkedItemContact.IdPerson = SelectedContact[0].IdPerson;
                activityLinkedItemContact.Name = SelectedContact[0].Name + " - " + SelectedContact[0].Surname;

                activityLinkedItemContact.LinkedItemType = new LookupValue();
                activityLinkedItemContact.LinkedItemType.IdLookupValue = 43; //Contact
                activityLinkedItemContact.LinkedItemType.Value = "Contact";
                // CompanyGroupList.FindIndex(x => x.IdCustomer == _ActivityLinkedItem.IdCustomer);

                addActivityViewModel.IsAddedFromOutSide = true;
                addActivityViewModel.SelectedIndexCompanyGroup = addActivityViewModel.CompanyGroupList.IndexOf(addActivityViewModel.CompanyGroupList.FirstOrDefault(x => x.IdCustomer == ListGroup[SelectedIndexCompanyGroup].IdCustomer));
                addActivityViewModel.SelectedIndexCompanyPlant = addActivityViewModel.CompanyPlantList.IndexOf(addActivityViewModel.CompanyPlantList.FirstOrDefault(x => x.IdCompany == ListPlant[SelectedIndexCompanyPlant].IdCompany));

                activityLinkedItemContact.IsVisible = false;

                activity.ActivityLinkedItem.Add(activityLinkedItemContact);
                activityList.Add(activity);
                addActivityViewModel.IsInternalEnable = false;
                addActivityViewModel.Init(activityList);

                //code for add Contact Detail.

                EventHandler handle = delegate { addActivityView.Close(); };
                addActivityViewModel.RequestClose += handle;
                addActivityView.DataContext = addActivityViewModel;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                addActivityView.ShowDialog();

                if (addActivityViewModel.IsActivitySave)
                {
                    foreach (Activity newActivity in addActivityViewModel.NewCreatedActivityList)
                    {
                        if (newActivity.IsCompleted == 1)
                        {
                            newActivity.ActivityGridStatus = "Completed";
                            newActivity.CloseDate = GeosApplication.Instance.ServerDateTime;
                        }
                        else
                        {
                            newActivity.ActivityGridStatus = newActivity.ActivityStatus != null ? newActivity.ActivityStatus.Value : "";
                            newActivity.CloseDate = null;
                        }

                        ContactActivityList.Add(newActivity);
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = SelectedContact[0].IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ActivityAddedToContactChangeLog").ToString(), newActivity.Subject), IdLogEntryType = 2 });

                    }

                    ContactActivityList = new List<Activity>(ContactActivityList);
                    SelectedActivity = ContactActivityList.Last();

                    TableView detailView = ((TableView)obj);
                    detailView.Focus();
                }

                GeosApplication.Instance.Logger.Log("Method AddActivityViewWindowShow() executed successfully...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddActivityViewWindowShow() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
            AllPeopleFirstNameList = GeosApplication.Instance.PeopleList.ToList();

            IsFromFirstnameCmb = true;
            IsFromLastnameCmb = false;

            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName ...", category: Category.Info, priority: Priority.Low);

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
                }
                else
                    AlertVisibilityFirstName = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPopupAsPerFirstName() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for search similar last name.
        /// </summary>
        /// <param name="lName"></param>
        private void ShowPopupAsPerLastName(string lName)
        {
            AllPeopleLastNameList = GeosApplication.Instance.PeopleList.ToList();
            IsFromFirstnameCmb = false;
            IsFromLastnameCmb = true;

            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerLastName ...", category: Category.Info, priority: Priority.Low);

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
                }
                else
                    AlertVisibilityLastName = Visibility.Hidden;

                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerLastName() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPopupAsPerLastName() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for avoid enter digit and special charactar on name. 
        /// </summary>
        /// <param name="e"></param>

        public void OnEditValueChangingForFirstName(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnEditValueChangingForFirstName ...", category: Category.Info, priority: Priority.Low);

            firstNameMsgStrError = string.Empty;

            int msgcount = 0;
            var newInput = (string)e.NewValue;
            var oldInput = (string)e.OldValue;

            if (!string.IsNullOrEmpty(newInput) && !ShouldKeepValueForFirstName)
            {
                if (newInput.Count(char.IsDigit) > 0)
                {
                    firstNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewDigitMsg").ToString();
                    AlertVisibilityFirstName = Visibility.Visible;
                    AlertVisibilityLastName = Visibility.Hidden;
                    msgcount++;

                    e.Handled = true;
                }

                MatchCollection matches = regex.Matches(newInput.ToLower().ToString());
                if (matches.Count > 0)
                {
                    firstNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewspecialcharacterMsg").ToString();
                    AlertVisibilityFirstName = Visibility.Visible;
                    AlertVisibilityLastName = Visibility.Hidden;
                    msgcount++;

                    if (msgcount == 2)
                        firstNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewdigitAndspecialcharacterMsg").ToString();

                    e.Handled = true;
                }

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            }
            else ShouldKeepValueForFirstName = false;

            GeosApplication.Instance.Logger.Log("Method OnEditValueChangingForFirstName() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for avoid enter digit and special charactar on name. 
        /// </summary>
        /// <param name="e"></param>
        public void OnEditValueChangingForLastName(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnEditValueChangingForLastName ...", category: Category.Info, priority: Priority.Low);

            lastNameMsgStrError = string.Empty;

            int msgcount = 0;
            var newInput = (string)e.NewValue;
            var oldInput = (string)e.OldValue;
            if (!string.IsNullOrEmpty(newInput) && !ShouldKeepValueforLastName)
            {
                if (newInput.Count(char.IsDigit) > 0)
                {
                    lastNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewDigitMsg").ToString();
                    AlertVisibilityFirstName = Visibility.Hidden;
                    AlertVisibilityLastName = Visibility.Visible;
                    msgcount++;

                    e.Handled = true;
                }

                MatchCollection matches = regex.Matches(newInput.ToLower().ToString());

                if (matches.Count > 0)
                {
                    lastNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewspecialcharacterMsg").ToString();
                    AlertVisibilityFirstName = Visibility.Hidden;
                    AlertVisibilityLastName = Visibility.Visible;
                    msgcount++;

                    if (msgcount == 2)
                        lastNameMsgStrError = System.Windows.Application.Current.FindResource("EditContactViewdigitAndspecialcharacterMsg").ToString();

                    e.Handled = true;
                }

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            }
            else ShouldKeepValueforLastName = false;

            GeosApplication.Instance.Logger.Log("Method OnEditValueChangingForLastName() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void InIt(People ObjContact)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method InIt ...", category: Category.Info, priority: Priority.Low);

                Contact = new People();
                initialContact = new People();

                Contact = ObjContact;
                initialContact = (People)ObjContact.Clone();

                FirstName = Contact.Name.Trim();
                LastName = Contact.Surname.Trim();
                ShouldKeepValueforLastName = true;
                ShouldKeepValueForFirstName = true;
                JobTitle = Contact.JobTitle;
                SelectedIndexDepartment = ListDepartment.IndexOf(ListDepartment.FirstOrDefault(i => i.IdLookupValue == Contact.IdCompanyDepartment));
                SelectedIndexProductInvolved = ListProductInvolved.IndexOf(ListProductInvolved.FirstOrDefault(i => i.IdLookupValue == Contact.IdContactProductInvolved));
                SelectedIndexEmdepAffinity = ListEmdepAffinity.IndexOf(ListEmdepAffinity.FirstOrDefault(i => i.IdLookupValue == Contact.IdContactEmdepAffinity));
                SelectedIndexInfluenceLevel = ListInfluenceLevel.IndexOf(ListInfluenceLevel.FirstOrDefault(i => i.IdLookupValue == Contact.IdContactInfluenceLevel));

                if (Contact.IdCompetitor != null)
                    SelectedIndexCompetitorAffinity = ListCompetitorAffinity.IndexOf(ListCompetitorAffinity.FirstOrDefault(i => i.IdCompetitor == Contact.IdCompetitor));
                else
                    SelectedIndexCompetitorAffinity = 0;

                ContactImage = Contact.OwnerImage;
                Email = Contact.Email.Trim();
                if (!string.IsNullOrEmpty(Contact.Phone))
                    Phone = Contact.Phone.Trim();
                Country = Contact.Company.Country;

                SelectedContact.Add(
                     new People()
                     {
                         IdPerson = Contact.IdPerson,
                         Name = Contact.Name,
                         Surname = Contact.Surname,
                         FullName = Contact.FullName,
                         Phone = Contact.Phone,
                         Email = Contact.Email,
                         OwnerImage = Contact.OwnerImage,
                         Observations = Contact.Observations,
                         IdSite = Contact.IdSite,
                         IdPersonGender = Contact.IdPersonGender,
                         CreatedIn = Contact.CreatedIn,
                         PeopleType = Contact.PeopleType,
                         JobTitle = Contact.JobTitle,
                         ImageText = Contact.ImageText,
                         CompanyDepartment = Contact.CompanyDepartment,
                         ProductInvolved = Contact.ProductInvolved,
                         InfluenceLevel = Contact.InfluenceLevel,
                         EmdepAffinity = Contact.EmdepAffinity,
                         Company = new Company()
                         {
                             IdCompany = Contact.Company.IdCompany,
                             Customers = Contact.Company.Customers,
                         }
                     }
                );

                if (SelectedIndexCompetitorAffinity > -1)
                    SelectedContact[0].Competitor = new Competitor() { IdCompetitor = ListCompetitorAffinity[SelectedIndexCompetitorAffinity].IdCompetitor, Name = ListCompetitorAffinity[SelectedIndexCompetitorAffinity].Name };

                if (SelectedContact[0] != null)
                {
                    if (SelectedContact[0].OwnerImage == null)
                    {
                        if (!string.IsNullOrEmpty(SelectedContact[0].ImageText))
                        {
                            byte[] imageBytes = Convert.FromBase64String(SelectedContact[0].ImageText);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            SelectedContact[0].OwnerImage = byteArrayToImage(imageBytes);
                            ContactImage = SelectedContact[0].OwnerImage;
                            Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = SelectedContact[0].ImageText;
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (SelectedContact[0].IdPersonGender == 1)
                                {
                                    SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (SelectedContact[0].IdPersonGender == 2)
                                {
                                    SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                            else
                            {
                                if (SelectedContact[0].IdPersonGender == 1)
                                {
                                    SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (SelectedContact[0].IdPersonGender == 2)
                                {
                                    SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                        }
                    }

                    if (SelectedContact[0].IdPersonGender == 1)
                    {
                        SelectedIndexGender = 0;
                    }
                    else if (SelectedContact[0].IdPersonGender == 2)
                    {
                        SelectedIndexGender = 1;
                    }
                    else
                    {
                        SelectedIndexGender = -1;
                    }

                    FillCompanyPlantList();
                    FillGroupList();

                    AlertVisibilityFirstName = Visibility.Hidden;
                    AlertVisibilityLastName = Visibility.Hidden;

                    // to display activity as per contact
                    try
                    {
                        if (SelectedContact[0].IdPerson != 0)
                        {
                            ContactActivityList = CrmStartUp.GetActivitiesByIdContact(SelectedContact[0].IdPerson);
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log(string.Format("Error in Init() Method...", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                    FillContactComments(SelectedContact[0].IdPerson);
                    FillContactChangeLog(SelectedContact[0].IdPerson);
                }

                GeosApplication.Instance.Logger.Log("Method InIt() executed successfully ..", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in InIt() Method..." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill comment by contact
        /// </summary>
        /// <param name="obj"></param>
        private void FillContactComments(long idContact)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactComments ...", category: Category.Info, priority: Priority.Low);

                ContactCommentsList = new ObservableCollection<LogEntriesByContact>(CrmStartUp.GetLogEntriesByContact(SelectedContact[0].IdPerson, 1).OrderByDescending(acl => acl.Datetime)); // 1 for Comment
                Contact.CommentsByContact = new List<LogEntriesByContact>();
                foreach (LogEntriesByContact item in ContactCommentsList)
                {
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
                }

                SetUserProfileImage(ContactCommentsList);
                RtfToPlaintext();

                GeosApplication.Instance.Logger.Log("Method FillContactComments() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactComments() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactComments() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillContactComments() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method to fill ChangLog
        /// </summary>
        /// <param name="obj"></param>
        private void FillContactChangeLog(int idContact)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillContactChangeLog ...", category: Category.Info, priority: Priority.Low);

                ListContactChangeLog = new ObservableCollection<LogEntriesByContact>(CrmStartUp.GetLogEntriesByContact(idContact, 2).OrderByDescending(i => i.IdLogEntryByContact).ToList());   //2 for ChangeLog

                GeosApplication.Instance.Logger.Log("Method FillContactChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactChangeLog() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillContactChangeLog() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillContactChangeLog() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void EditContactAcceptAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method EditContactAcceptAction()....", category: Category.Info, priority: Priority.Low);
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

            if (SelectedContact[0] != null)
            {
                AddChangeLogGroupAndPlant(initialContact);

                if (!string.IsNullOrEmpty(FirstName))
                    SelectedContact[0].Name = FirstName.Trim();

                if (!string.IsNullOrEmpty(LastName))
                    SelectedContact[0].Surname = LastName.Trim();

                SelectedContact[0].Email = Email.Trim();

                if (Phone != null)
                    SelectedContact[0].Phone = Phone.Trim();
                else
                    SelectedContact[0].Phone = string.Empty;

                SelectedContact[0].IdPersonGender = Convert.ToByte(UserGenderList[SelectedIndexGender].IdLookupValue);
                SelectedContact[0].UserGender = UserGenderList[SelectedIndexGender].Value;
                SelectedContact[0].IdSite = ListPlant[SelectedIndexCompanyPlant].IdCompany;

                SelectedContact[0].Company.IdCompany = ListPlant[SelectedIndexCompanyPlant].IdCompany;
                SelectedContact[0].Company.Name = ListPlant[SelectedIndexCompanyPlant].Name;

                SelectedContact[0].Company.IdCustomer = ListGroup[SelectedIndexCompanyGroup].IdCustomer;
                SelectedContact[0].Company.Customers[0] = ListGroup[SelectedIndexCompanyGroup];

                SelectedContact[0].Company.Country = Country; //ListCountry[SelectedIndexCountry];
                SelectedContact[0].IdSite = ListPlant[SelectedIndexCompanyPlant].IdCompany;
                SelectedContact[0].Company.Name = ListPlant[SelectedIndexCompanyPlant].SiteNameWithoutCountry;

                SelectedContact[0].Company.IdCountry = SelectedContact[0].Company.Country.IdCountry;
                SelectedContact[0].Company.ZipCode = SelectedContact[0].Company.ZipCode;

                SelectedContact[0].Company.ModifiedBy = GeosApplication.Instance.ActiveUser.IdUser;
                SelectedContact[0].Company.ModifiedIn = GeosApplication.Instance.ServerDateTime;

                SelectedContact[0].IdCompanyDepartment = ListDepartment[SelectedIndexDepartment].IdLookupValue;
                SelectedContact[0].JobTitle = JobTitle.Trim();
                SelectedContact[0].IdContactProductInvolved = ListProductInvolved[SelectedIndexProductInvolved].IdLookupValue;
                SelectedContact[0].IdContactEmdepAffinity = ListEmdepAffinity[SelectedIndexEmdepAffinity].IdLookupValue;
                SelectedContact[0].IdContactInfluenceLevel = ListInfluenceLevel[SelectedIndexInfluenceLevel].IdLookupValue;

                if (SelectedIndexCompetitorAffinity > 0)
                {
                    SelectedContact[0].IdCompetitor = ListCompetitorAffinity[SelectedIndexCompetitorAffinity].IdCompetitor;
                    SelectedContact[0].Competitor = new Competitor() { IdCompetitor = ListCompetitorAffinity[SelectedIndexCompetitorAffinity].IdCompetitor, Name = ListCompetitorAffinity[SelectedIndexCompetitorAffinity].Name };
                }
                else
                {
                    SelectedContact[0].IdCompetitor = null;
                    SelectedContact[0].Competitor = null;
                }

                SelectedContact[0].CompanyDepartment = new LookupValue() { IdLookupKey = ListDepartment[SelectedIndexDepartment].IdLookupKey, IdLookupValue = ListDepartment[SelectedIndexDepartment].IdLookupValue, Value = ListDepartment[SelectedIndexDepartment].Value };
                SelectedContact[0].ProductInvolved = new LookupValue() { IdLookupKey = ListProductInvolved[SelectedIndexProductInvolved].IdLookupKey, IdLookupValue = ListProductInvolved[SelectedIndexProductInvolved].IdLookupValue, Value = ListProductInvolved[SelectedIndexProductInvolved].Value };
                SelectedContact[0].InfluenceLevel = new LookupValue() { IdLookupKey = ListInfluenceLevel[SelectedIndexInfluenceLevel].IdLookupKey, IdLookupValue = ListInfluenceLevel[SelectedIndexInfluenceLevel].IdLookupValue, Value = ListInfluenceLevel[SelectedIndexInfluenceLevel].Value };
                SelectedContact[0].EmdepAffinity = new LookupValue() { IdLookupKey = ListEmdepAffinity[SelectedIndexEmdepAffinity].IdLookupKey, IdLookupValue = ListEmdepAffinity[SelectedIndexEmdepAffinity].IdLookupValue, Value = ListEmdepAffinity[SelectedIndexEmdepAffinity].Value };

                if (SelectedContact[0].OwnerImage != null)
                {

                    SelectedContact[0].ImageText = Emdep.Geos.UI.Helper.ImageEditHelper.Base64String;

                    if (!string.IsNullOrEmpty(SelectedContact[0].ImageText))
                    {
                        byte[] imageBytes = Convert.FromBase64String(SelectedContact[0].ImageText);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                        ContactImage = byteArrayToImage(imageBytes);
                    }
                    else
                    {
                        if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                        {
                            if (SelectedContact[0].IdPersonGender == 1)
                                SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                            else if (SelectedContact[0].IdPersonGender == 2)
                                SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        }
                        else
                        {
                            if (SelectedContact[0].IdPersonGender == 1)
                                SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                            else if (SelectedContact[0].IdPersonGender == 2)
                                SelectedContact[0].OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        }
                    }
                }

                SelectedContact[0].OwnerImage = null;
                SelectedContact[0].CommentsByContact = new List<LogEntriesByContact>();

                foreach (LogEntriesByContact item in Contact.CommentsByContact)
                {
                    if (!ContactCommentsList.Any(x => x.IdLogEntryByContact == item.IdLogEntryByContact))
                    {
                        item.IsDeleted = true;
                        ContactCommentsList.Add(item);
                    }
                }

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
                    SelectedContact[0].CommentsByContact.Add(newobj);

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
                            TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = SelectedContact[0].IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });

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
                            TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = SelectedContact[0].IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentAdded").ToString(), newTempComment), IdLogEntryType = 2 });
                        }
                    }
                    else if (item.IsDeleted)
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
                            TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = SelectedContact[0].IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentRemoved").ToString(), newTempComment), IdLogEntryType = 2 });

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
                            TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = SelectedContact[0].IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCommentRemoved").ToString(), newTempComment), IdLogEntryType = 2 });
                        }
                    }
                }

                AddChangeLog(initialContact, SelectedContact[0]);
                SelectedContact[0].LogEntriesByContact = TempContactChangeLog.ToList();
                try
                {
                    IsSave = CrmStartUp.UpdateContact(SelectedContact[0]);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditContactViewUpdateSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method EditContactAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
                }
                catch (FaultException<ServiceException> ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in EditContactAcceptAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                    CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                catch (ServiceUnexceptedException ex)
                {
                    IsBusy = false;
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Get an error in EditContactAcceptAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                    GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in EditContactAcceptAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                }
            }

            IsBusy = false;

        }

        /// <summary>
        /// Method for Edit Activity View
        /// </summary>
        /// <param name="obj"></param>
        private void EditActivityViewWindowShow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow...", category: Category.Info, priority: Priority.Low);

                if (obj == null) return;

                Activity activity = ((Activity)obj);

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
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }

                Activity tempActivity = new Activity();

                tempActivity = CrmStartUp.GetActivityByIdActivity_V2035(activity.IdActivity);

                EditActivityViewModel editActivityViewModel = new EditActivityViewModel();
                EditActivityView editActivityView = new EditActivityView();

                editActivityViewModel.IsEditedFromOutSide = true;

                foreach (var item in tempActivity.ActivityLinkedItem)
                {
                    if (item.IdLinkedItemType == 43 && item.IdPerson == SelectedContact[0].IdPerson)
                        item.IsVisible = false;
                    if (item.IdLinkedItemType == 42)
                        item.IsVisible = false;
                }
                editActivityViewModel.IsInternalEnable = false;
                editActivityViewModel.Init(tempActivity);

                EventHandler handle = delegate { editActivityView.Close(); };
                editActivityViewModel.RequestClose += handle;
                editActivityView.DataContext = editActivityViewModel;

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                editActivityView.ShowDialogWindow();

                if (editActivityViewModel.objActivity != null)
                {
                    activity.Subject = editActivityViewModel.objActivity.Subject;
                    activity.Description = editActivityViewModel.objActivity.Description;
                    activity.LookupValue = editActivityViewModel.objActivity.LookupValue;
                    activity.ToDate = editActivityViewModel.objActivity.ToDate;
                    activity.FromDate = editActivityViewModel.objActivity.FromDate;
                    activity.IsCompleted = editActivityViewModel.objActivity.IsCompleted;
                    //activity.ActivityLinkedItem[0] = editActivityViewModel.objActivity.ActivityLinkedItem[0].Customer;
                    activity.People = editActivityViewModel.objActivity.People;
                    activity.ActivityLinkedItem = editActivityViewModel.objActivity.ActivityLinkedItem;

                    if (activity.IsCompleted == 1)
                    {
                        activity.ActivityGridStatus = "Completed";
                        activity.CloseDate = GeosApplication.Instance.ServerDateTime;
                    }
                    else
                    {
                        activity.ActivityGridStatus = editActivityViewModel.objActivity.ActivityStatus != null ? editActivityViewModel.objActivity.ActivityStatus.Value : "";
                        activity.CloseDate = null;
                    }
                    TableView detailView = ((TableView)obj);
                    detailView.Focus();
                }

                GeosApplication.Instance.Logger.Log("Method EditActivityViewWindowShow executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in EditActivityViewWindowShow() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
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
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

            return imgSrc;
        }

        public void SendMailtoPerson(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SendMailtoPerson().....", category: Category.Info, priority: Priority.Low);
                if (string.IsNullOrEmpty(Convert.ToString(obj)))
                {
                    return;
                }

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method For Contact ChangeLog
        /// </summary>
        private void AddChangeLog(People oldContact, People newContact)
        {
            GeosApplication.Instance.Logger.Log("Method AddChangeLog ...", category: Category.Info, priority: Priority.Low);

            if (oldContact != null && newContact != null)
            {
                if (oldContact.FullName != newContact.FullName)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogNameEdit").ToString(), oldContact.FullName, newContact.FullName), IdLogEntryType = 2 });

                if (oldContact.IdPersonGender != null && oldContact.IdPersonGender != newContact.IdPersonGender)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogGenderEdit").ToString(), oldContact.UserGender, newContact.UserGender), IdLogEntryType = 2 });
                else
                {
                    if (oldContact.IdPersonGender == null)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogGenderEdit").ToString(), "None", newContact.UserGender), IdLogEntryType = 2 });
                }

                if (oldContact.CompanyDepartment != null && oldContact.CompanyDepartment.IdLookupValue != newContact.CompanyDepartment.IdLookupValue)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogDepartmentEdit").ToString(), oldContact.CompanyDepartment.Value, newContact.CompanyDepartment.Value), IdLogEntryType = 2 });
                else
                {
                    if (oldContact.CompanyDepartment == null)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogDepartmentEdit").ToString(), "None", newContact.CompanyDepartment.Value), IdLogEntryType = 2 });
                }

                if (oldContact.JobTitle != newContact.JobTitle)
                {
                    if (oldContact.JobTitle == "")
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogJobTitleEdit").ToString(), "None", newContact.JobTitle), IdLogEntryType = 2 });
                    else
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogJobTitleEdit").ToString(), oldContact.JobTitle, newContact.JobTitle), IdLogEntryType = 2 });

                }

                if (oldContact.Phone != null && newContact.Phone != null && oldContact.Phone != newContact.Phone)
                {
                    string oldPhoneNumber = oldContact.Phone.Trim();
                    string newPhoneNumber = newContact.Phone.Trim();

                    if (oldPhoneNumber == "" && newPhoneNumber != "")
                        // TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogPhoneAdded").ToString(), newPhoneNumber), IdLogEntryType = 2 });
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogPhoneEdit").ToString(), "None", newPhoneNumber), IdLogEntryType = 2 });
                    if (oldPhoneNumber != "" && newPhoneNumber == "")
                        //TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogPhoneRemoved").ToString(), oldPhoneNumber), IdLogEntryType = 2 });
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogPhoneEdit").ToString(), oldPhoneNumber, "None"), IdLogEntryType = 2 });
                    if (oldPhoneNumber != "" && newPhoneNumber != "")
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogPhoneEdit").ToString(), oldPhoneNumber, newPhoneNumber), IdLogEntryType = 2 });
                }

                if (oldContact.Email != newContact.Email)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogEmailEdit").ToString(), oldContact.Email, newContact.Email), IdLogEntryType = 2 });
                if (oldContact.ProductInvolved != null && oldContact.ProductInvolved.IdLookupValue != newContact.ProductInvolved.IdLookupValue)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogProductInvolvedEdit").ToString(), oldContact.ProductInvolved.Value, newContact.ProductInvolved.Value), IdLogEntryType = 2 });
                else
                {
                    if (oldContact.ProductInvolved == null)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogProductInvolvedEdit").ToString(), "None", newContact.ProductInvolved.Value), IdLogEntryType = 2 });
                }
                if (oldContact.InfluenceLevel != null && oldContact.InfluenceLevel.IdLookupValue != newContact.InfluenceLevel.IdLookupValue)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogInfluenceLevelEdit").ToString(), oldContact.InfluenceLevel.Value, newContact.InfluenceLevel.Value), IdLogEntryType = 2 });
                else
                {
                    if (oldContact.InfluenceLevel == null)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogInfluenceLevelEdit").ToString(), "None", newContact.InfluenceLevel.Value), IdLogEntryType = 2 });
                }

                if (oldContact.EmdepAffinity != null && oldContact.EmdepAffinity.IdLookupValue != newContact.EmdepAffinity.IdLookupValue)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogEmdepAffinityEdit").ToString(), oldContact.EmdepAffinity.Value, newContact.EmdepAffinity.Value), IdLogEntryType = 2 });
                else
                {
                    if (oldContact.EmdepAffinity == null)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogEmdepAffinityEdit").ToString(), "None", newContact.EmdepAffinity.Value), IdLogEntryType = 2 });
                }

                if (oldContact.IdCompetitor != null && oldContact.IdCompetitor != newContact.IdCompetitor)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCompetitorAffinityEdit").ToString(), oldContact.Competitor.Name, newContact.Competitor != null ? newContact.Competitor.Name : "None"), IdLogEntryType = 2 });
                else
                {
                    if (oldContact.IdCompetitor == null && newContact.IdCompetitor != null)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogCompetitorAffinityEdit").ToString(), "None", newContact.Competitor.Name), IdLogEntryType = 2 });
                }

                //Contact Image Change Log
                if (string.IsNullOrEmpty(oldContact.ImageText) && newContact.ImageText != null)
                {
                    if (oldContact.ImageText != newContact.ImageText)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageAddedChangeLog").ToString()), IdLogEntryType = 2 });
                }
                else
                {
                    if (!string.IsNullOrEmpty(oldContact.ImageText) && string.IsNullOrEmpty(newContact.ImageText))
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageRemovedChangeLog").ToString()), IdLogEntryType = 2 });
                    else if (!string.IsNullOrEmpty(oldContact.ImageText) && !string.IsNullOrEmpty(newContact.ImageText) && oldContact.ImageText != newContact.ImageText)
                        TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageChangedChangeLog").ToString()), IdLogEntryType = 2 });
                }
            }

            GeosApplication.Instance.Logger.Log("Method AddChangeLog() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for ChangeLog with Group and Plant
        /// </summary>
        /// <param name="oldContact"></param>
        /// <param name="newContact"></param>
        private void AddChangeLogGroupAndPlant(People oldContact)
        {
            GeosApplication.Instance.Logger.Log("Method AddChangeLogGroupAndPlant ...", category: Category.Info, priority: Priority.Low);
            //if(TempContactChangeLog == null)
            //    TempContactChangeLog = new ObservableCollection<LogEntriesByContact>();
            if (oldContact != null)
            {
                if (oldContact.Company.Customers[0].IdCustomer != ListGroup[SelectedIndexCompanyGroup].IdCustomer)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogGroupEdit").ToString(), oldContact.Company.Customers[0].CustomerName.Trim(), ListGroup[SelectedIndexCompanyGroup].CustomerName.Trim()), IdLogEntryType = 2 });
                if (oldContact.Company.IdCompany != ListPlant[SelectedIndexCompanyPlant].IdCompany)
                    TempContactChangeLog.Add(new LogEntriesByContact() { IdContact = Contact.IdPerson, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactChangeLogPlantEdit").ToString(), oldContact.Company.Name.Trim(), ListPlant[SelectedIndexCompanyPlant].SiteNameWithoutCountry.Trim()), IdLogEntryType = 2 });
            }

            GeosApplication.Instance.Logger.Log("Method AddChangeLogGroupAndPlant() executed successfully", category: Category.Info, priority: Priority.Low);
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
                        ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", ListGroup);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                        ListGroup = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                    else
                    {
                        ListGroup = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", ListGroup);
                    }
                }
                SelectedIndexCompanyGroup = ListGroup.IndexOf(ListGroup.FirstOrDefault(i => i.IdCustomer == SelectedContact[0].Company.Customers[0].IdCustomer));
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
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT21", EntireCompanyPlantList);
                    }
                }
                else
                {
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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
                if (GeosApplication.Instance.IsPermissionReadOnly)
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

        #endregion
    }
}
