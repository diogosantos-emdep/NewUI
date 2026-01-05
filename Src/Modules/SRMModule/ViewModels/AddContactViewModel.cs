using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.SRM.Views;
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
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class AddContactViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region TaskLog

        //[CRM-M052-17] No message displaying the user about missing mandatory fields [adadibathina]

        #endregion

        #region Services
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());


        //ISRMService SRMService = new SRMServiceController("localhost:6699");

        #endregion // Services

        #region Commands
        public ICommand AddContactAcceptButtonCommand { get; set; }
        public ICommand AddContactCancelButtonCommand { get; set; }
        public ICommand EditSupplierDetailsCommand { get; set; }
        public ICommand OnTextEditValueChangingCommand { get; set; }
        //Comments //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public ICommand DeleteCommentRowCommand { get; set; }
        public ICommand AddCommentsCommand { get; set; }
        public ICommand CommentsGridDoubleClickCommand { get; set; }
        public ICommand AddTabClickCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        //End Comments
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion // Commands

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
        private string oldMatchName = "";
        private Int64 idArticleSupplier;
        private string firstName;
        private string lastName;
        private Contacts contacts;
        private ImageSource userProfileImage;
        private ObservableCollection<LookupValue> userGenderList;
        private string email;
        private int selectedIndexGender;
        private ImageSource contactImage;
        private string jobTitle;
        private ObservableCollection<LookupValue> listDepartment;

        public bool ShouldKeepValue { get; set; }
        private string firstNameMsgStr;
        private string lastNameMsgStr;
        private string firstNameMsgStrError;
        private string lastNameMsgStrError;
        private Regex regex;

        private Visibility alertVisibilityFirstName;
        private Visibility alertVisibilityLastName;
        private List<Contacts> allContactsList;
        private List<Contacts> allContactsFirstNameList;
        private List<Contacts> allContactsLastNameList;
        private List<string> allContactsFirstNameSrtList;
        private List<string> allContactsLastNameSrtList;
        private int selectedIndexDepartment;
        private string informationError;
        private string sName;
        private string eMDEPCode;
        private string address;
        private string country;
        private string region;
        private string city;
        private string phone;
        private string phone2; //[pramod.misal][GEOS2-4673][22-08-2023]
        private string postCode;
        private bool isBusy;
        private bool isSave;
        private bool isNew;
        private Visibility isEditSupplierVisible;

        ObservableCollection<ArticleSupplier> supplierList;//[Sudhir.Jangra][GEOS2-4676]

        public ArticleSupplier articleSupplier { get; set; }
        public bool IsFromFirstnameCmb { get; private set; }
        public bool IsFromLastnameCmb { get; private set; }
        string remarks;

        Visibility isSupplierTextEditVisible;//[Sudhir.Jangra][GEOS2-4676]
        Visibility isSupplierComboboxVisible;//[Sudhir.Jangra][GEOS2-4676]
        ArticleSupplier selectedSupplierList;//[Sudhir.Jangra][GEOS2-4676]
        Int32 idArticleSupplierContact;//[Sudhir.jangra][GEOS2-4676]
        Int32 idContact;//[Sudhir.jangra][GEOS2-4676]

        bool isAddSupplierView;//[Sudhir.Jangra][GEOS2-4738][27/09/2023]

        Contacts contactForIsSave;//[Sudhir.jangra][GEOS2-4738]
        //COMMENTS
        ObservableCollection<LogEntriesByArticleSuppliers> commentsList;
        List<LogEntriesByArticleSuppliers> addcommentsList;
        List<LogEntriesByArticleSuppliers> updatedCommentsList;
        ObservableCollection<LogEntriesByArticleSuppliers> deleteCommentsList;                                                         
        LogEntriesByArticleSuppliers srmComments;
        private bool isDeleted;
        private LogEntriesByArticleSuppliers selectedComment;
        ObservableCollection<LogEntriesByArticleSuppliers> newcommentsList;
        ObservableCollection<LogEntriesByArticleSuppliers> idlogentrybyitem;
        List<LogEntriesByArticleSuppliers> articleSuppliersComments;
        bool isUpdated;
        ObservableCollection<LogEntriesByArticleSuppliers> listContactChangeLog;
        private string fullName;
        private string commentText = string.Empty;
        DateTime? commentdatetimeText = null;
        string commentfullNameText = string.Empty;
        private string changeLogText;
        string changeLogfullNameText = string.Empty;
        DateTime? changeLogdatetimeText = null;
        //END COMMENTS
        #endregion // Declaration

        #region Properties

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value.TrimStart();
                OnPropertyChanged(new PropertyChangedEventArgs("FirstName"));
                ShowPopupAsPerFirstName(FirstName);
                FullName = FirstName + " " + LastName;
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                //[rdixit][GEOS2-4312][28.04.2023]
                if (value != null)
                    lastName = value.TrimStart();
                else
                    lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastName"));
                ShowPopupAsPerLastName(LastName);
                FullName = FirstName + " " + LastName;
            }
        }
        public string JobTitle
        {
            get { return jobTitle; }
            set
            {
                //jobTitle = value != null ? value.Trim() : value;
                //[pramod.misal][geos2-4656][13.07.2023]
                jobTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JobTitle"));
            }
        }
        public Contacts SelectedContacts
        {
            get { return contacts; }
            set
            {
                // contact = value;
                SetProperty(ref contacts, value, () => SelectedContacts);
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
        public string Phone
        {
            get { return phone; }
            set { phone = value; OnPropertyChanged(new PropertyChangedEventArgs("Phone")); }
        }

        //[pramod.misal][GEOS2-4673][22-08-2023]
        public string Phone2
        {
            get { return phone2; }
            set { phone2 = value; OnPropertyChanged(new PropertyChangedEventArgs("Phone2")); }
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
                selectedIndexGender = value; OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexGender"));
            }
        }
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

        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set { idArticleSupplier = value; OnPropertyChanged(new PropertyChangedEventArgs("IdArticleSupplier")); }
        }
        public string SName
        {
            get { return sName; }
            set
            {
                sName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SName"));
            }
        }
        public string EMDEPCode
        {
            get { return eMDEPCode; }
            set
            {
                eMDEPCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EMDEPCode"));

            }
        }
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(new PropertyChangedEventArgs("Address")); }
        }
        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; OnPropertyChanged(new PropertyChangedEventArgs("PostCode")); }
        }
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }

        }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remarks"));
            }
        }
        public List<Contacts> AllContactsList
        {
            get { return allContactsList; }
            set
            {
                allContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllContactsList"));
            }
        }
        public List<Contacts> AllContactsFirstNameList
        {
            get { return allContactsFirstNameList; }
            set
            {
                allContactsFirstNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllContactsFirstNameList"));
            }
        }
        public List<string> AllContactsFirstNameSrtList
        {
            get { return allContactsFirstNameSrtList; }
            set
            {
                allContactsFirstNameSrtList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllContactsFirstNameSrtList"));
            }
        }
        public List<string> AllContactsLastNameSrtList
        {
            get { return allContactsLastNameSrtList; }
            set
            {
                allContactsLastNameSrtList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllContactsLastNameSrtList"));
            }
        }
        public List<Contacts> AllContactsLastNameList
        {
            get { return allContactsLastNameList; }
            set
            {
                allContactsLastNameList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllContactsLastNameList"));
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

        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
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
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; OnPropertyChanged(new PropertyChangedEventArgs("IsNew")); }
        }

        public Visibility IsEditSupplierVisible
        {
            get { return isEditSupplierVisible; }
            set { isEditSupplierVisible = value; OnPropertyChanged(new PropertyChangedEventArgs("IsEditSupplierVisible")); }
        }

        public Contacts contactDetalis { get; set; }
        public List<LogEntriesByArticleSuppliers> ArticleSuppliersChangeLogList { get; set; }
        ArticleSupplier ArticleSupplierDetalis { get; set; }
        private string windowHeader;
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public ObservableCollection<ArticleSupplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupplierList"));
            }
        }
        public Visibility IsSupplierTextEditVisible
        {
            get { return isSupplierTextEditVisible; }
            set
            {
                isSupplierTextEditVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSupplierTextEditVisible"));
            }
        }
        public Visibility IsSupplierComboBoxVisible
        {
            get { return isSupplierComboboxVisible; }
            set
            {
                isSupplierComboboxVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSupplierComboBoxVisible"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4676]
        public ArticleSupplier SelectedSupplierList
        {
            get { return selectedSupplierList; }
            set
            {
                OldSelectedSupplierList = selectedSupplierList;
                selectedSupplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedSupplierList"));
                FillSupplierListBasedOnEmployeeCode();
            }
        }

        //[Sudhir.jangra][GEOS2-4676]
        public Int32 IdArticleSupplierContact
        {
            get { return idArticleSupplierContact; }
            set
            {
                idArticleSupplierContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticleSupplierContact"));
            }
        }
        //[Sudhir.Jangra][GEOS2-4676]
        public Int32 IdContact
        {
            get { return idContact; }
            set
            {
                idContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdContact"));
            }
        }

        //[Sudhir.Jangra][GEOS2-4738][27/09/2023]
        public bool IsAddSupplierView
        {
            get { return isAddSupplierView; }
            set
            {
                isAddSupplierView = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddSupplierView"));
            }
        }
        //[Sudhir.jangra][GEOS2-4738]
        public Contacts ContactForIsSave
        {
            get { return contactForIsSave; }
            set
            {
                contactForIsSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactForIsSave"));
            }
        }

        //COMMENTS
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public ObservableCollection<LogEntriesByArticleSuppliers> DeleteCommentsList
        {
            get { return deleteCommentsList; }
            set
            {
                deleteCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteCommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public List<LogEntriesByArticleSuppliers> ArticleSuppliersComments
        {
            get { return articleSuppliersComments; }
            set
            {
                articleSuppliersComments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleSuppliersComments"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public List<LogEntriesByArticleSuppliers> UpdatedCommentsList
        {
            get { return updatedCommentsList; }
            set
            {
                updatedCommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedCommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public ObservableCollection<LogEntriesByArticleSuppliers> CommentsList
        {
            get { return commentsList; }
            set
            {
                commentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public List<LogEntriesByArticleSuppliers> AddCommentsList
        {
            get { return addcommentsList; }
            set
            {
                addcommentsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddCommentsList"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public LogEntriesByArticleSuppliers SrmComments
        {
            get { return srmComments; }
            set
            {
                srmComments = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SrmComments"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public LogEntriesByArticleSuppliers SelectedComment
        {
            get { return selectedComment; }
            set
            {
                selectedComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedComment"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsUpdated"));
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public ObservableCollection<LogEntriesByArticleSuppliers> ListContactChangeLog
        {
            get { return listContactChangeLog; }
            set
            {
                listContactChangeLog = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListContactChangeLog"));
            }
        }
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullName"));
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

        public DateTime? CommentDatetimeText
        {
            get { return commentdatetimeText; }
            set
            {
                commentdatetimeText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DatetimeText"));
            }
        }
        public string CommentFullNameText
        {
            get { return commentfullNameText; }
            set
            {
                commentfullNameText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullNameText"));
            }
        }

        public DateTime? ChangeLogDatetimeText
        {
            get { return changeLogdatetimeText; }
            set
            {
                changeLogdatetimeText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DatetimeText"));
            }
        }

        public string ChangeLogFullNameText
        {
            get { return changeLogfullNameText; }
            set
            {
                changeLogfullNameText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FullNameText"));
            }
        }
        public string ChangeLogText
        {
            get { return changeLogText; }
            set
            {
                changeLogText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ChangeLogText"));
            }
        }

        ObservableCollection<Article_Supplier_Contacts> tabItems;
        public ObservableCollection<Article_Supplier_Contacts> TabItems
        {
            get { return tabItems; }
            set
            {
                tabItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TabItems"));
            }
        }

        Article_Supplier_Contacts selectedTabItem;
        public Article_Supplier_Contacts SelectedTabItem
        {
            get { return selectedTabItem; }
            set
            {
                selectedTabItem = value;
                if (selectedTabItem!=null)
                {
                    EMDEPCode = selectedTabItem.EMDEPCode;
                    SName = selectedTabItem.Name;
                    Address = selectedTabItem.Address;
                    City = selectedTabItem.City;
                    Region = selectedTabItem.Region;
                    Country = selectedTabItem.Country;
                    PostCode = selectedTabItem.PostCode;
                    SelectedSupplierList = SupplierList.FirstOrDefault(x => x.IdArticleSupplier == selectedTabItem.IdArticleSupplier);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTabItem"));
            }
        }

        ObservableCollection<ArticleSupplier> supplierListByIdContact;
        public ObservableCollection<ArticleSupplier> SupplierListByIdContact
        {
            get { return supplierListByIdContact; }
            set
            {
                supplierListByIdContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SupplierListByIdContact"));
            }
        }

        public string shortName;
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShortName"));
            }
        }

        List<Article_Supplier_Contacts> articleSupplierContactsList;
        public List<Article_Supplier_Contacts> AddArticleSupplierContactsList
        {
            get { return articleSupplierContactsList; }
            set
            {
                articleSupplierContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AddArticleSupplierContactsList"));
            }
        }

        List<Article_Supplier_Contacts> updatearticleSupplierContactsList;
        public List<Article_Supplier_Contacts> UpdateArticleSupplierContactsList
        {
            get { return updatearticleSupplierContactsList; }
            set
            {
                updatearticleSupplierContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateArticleSupplierContactsList"));
            }
        }

        List<Article_Supplier_Contacts>deletearticleSupplierContactsList;
        public List<Article_Supplier_Contacts> DeleteArticleSupplierContactsList
        {
            get { return deletearticleSupplierContactsList; }
            set
            {
                deletearticleSupplierContactsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DeleteArticleSupplierContactsList"));
            }
        }

        ArticleSupplier oldSelectedSupplierList;
        public ArticleSupplier OldSelectedSupplierList
        {
            get { return oldSelectedSupplierList; }
            set
            {
                oldSelectedSupplierList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OldSelectedSupplierList"));
            }
        }
        //END COMMENTS
        #endregion // Properties

        #region Constructor

        public AddContactViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddContactViewModel...", category: Category.Info, priority: Priority.Low);

                ShouldKeepValue = true;
                regex = new Regex(@"[~`!@#$%^&*()-_+=|\{}':;.,<>/?" + Convert.ToChar(34) + "]");

                AddContactAcceptButtonCommand = new RelayCommand(new Action<object>(AddAcceptAction));
                AddContactCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EditSupplierDetailsCommand = new RelayCommand(new Action<object>(EditCustomerDetailsCommandAction));
                OnTextEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnEditValueChanging);
                DeleteCommentRowCommand = new DelegateCommand<object>(DeleteCommentCommandAction);//[chitra.girigosavi][GEOS2-4692][09.10.2023]
                AddCommentsCommand = new DelegateCommand<object>(AddCommentsCommandAction);//[chitra.girigosavi][GEOS2-4692][09.10.2023]
                CommentsGridDoubleClickCommand = new DelegateCommand<object>(CommentDoubleClickCommandAction);//[chitra.girigosavi][GEOS2-4692][09.10.2023]

                SelectedContacts = new Contacts();
                SelectedIndexGender = 0;
                FillList();
                ContactForIsSave = new Contacts();
                AddTabClickCommand =new DelegateCommand<object>(AddTab_Click);
                if (TabItems == null)
                {
                    TabItems = new  ObservableCollection<Article_Supplier_Contacts>();
                }
                if (SupplierList == null)
                {
                    FillSupplierList();
                }
                DeleteCommand = new DelegateCommand<object>(DeleteCommandAction);
                AddArticleSupplierContactsList = new List<Article_Supplier_Contacts>();
                UpdateArticleSupplierContactsList = new List<Article_Supplier_Contacts>();
                DeleteArticleSupplierContactsList = new List<Article_Supplier_Contacts>();
                GeosApplication.Instance.Logger.Log("Constructor AddContactViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (System.Exception ex)
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
                    me[BindableBase.GetPropertyName(() => Phone)] +
                    me[BindableBase.GetPropertyName(() => Email)] +
                    me[BindableBase.GetPropertyName(() => JobTitle)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexGender)] +           // SelectedIndexGender
                    me[BindableBase.GetPropertyName(() => SelectedIndexDepartment)] +       // SelectedIndexDepartment
                    me[BindableBase.GetPropertyName(() => InformationError)];

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
                string PhoneProp = BindableBase.GetPropertyName(() => Phone);
                string emailProp = BindableBase.GetPropertyName(() => Email);
                string jobTitleProp = BindableBase.GetPropertyName(() => JobTitle);
                string selectedIndexGenderProp = GetPropertyName(() => SelectedIndexGender);                // SelectedIndexGender          
                string selectedIndexDepartmentProp = BindableBase.GetPropertyName(() => SelectedIndexDepartment);   // SelectedIndexDepartment
                string informationError = BindableBase.GetPropertyName(() => InformationError);
                string selectedSupplier = BindableBase.GetPropertyName(() => SelectedSupplierList);
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
                    else if (columnName == PhoneProp)
                        return ContactValidation.GetErrorMessage(PhoneProp, Phone);
                    else if (columnName == emailProp)
                        return ContactValidation.GetErrorMessage(emailProp, Email);
                    else if (columnName == jobTitleProp)
                        return ContactValidation.GetErrorMessage(jobTitleProp, JobTitle);
                    else if (columnName == selectedIndexGenderProp) // Lead Source
                        return ContactValidation.GetErrorMessage(selectedIndexGenderProp, SelectedIndexGender);
                    else if (columnName == selectedIndexDepartmentProp)
                        return ContactValidation.GetErrorMessage(selectedIndexDepartmentProp, SelectedIndexDepartment);
                    else if (columnName == informationError)
                        return ContactValidation.GetErrorMessage(informationError, InformationError);
                    else if (columnName == selectedSupplier)
                        return ContactValidation.GetErrorMessage(selectedSupplier, SelectedSupplierList);
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
                    else if (columnName == PhoneProp)
                        return ContactValidation.GetErrorMessage(PhoneProp, Phone);
                    else if (columnName == emailProp)
                        return ContactValidation.GetErrorMessage(emailProp, Email);
                    else if (columnName == jobTitleProp)
                        return ContactValidation.GetErrorMessage(jobTitleProp, JobTitle);
                    else if (columnName == selectedIndexGenderProp) // Lead Source
                        return ContactValidation.GetErrorMessage(selectedIndexGenderProp, SelectedIndexGender);
                    else if (columnName == selectedIndexDepartmentProp)
                        return ContactValidation.GetErrorMessage(selectedIndexDepartmentProp, SelectedIndexDepartment);
                    else if (columnName == informationError)
                        return ContactValidation.GetErrorMessage(informationError, InformationError);
                    else if (columnName == selectedSupplier)
                        return ContactValidation.GetErrorMessage(selectedSupplier, SelectedSupplierList);
                }
                return null;
            }
        }

        #endregion

        #region Methods

        private void AddAcceptAction(object obj)
        {


            GeosApplication.Instance.Logger.Log("Method AddAcceptAction()...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            InformationError = null;

            string error = EnableValidationAndGetError();
            if (string.IsNullOrEmpty(error))
                InformationError = null;
            else
                InformationError = "";

            PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
            PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
            PropertyChanged(this, new PropertyChangedEventArgs("Phone"));
            PropertyChanged(this, new PropertyChangedEventArgs("Phone2")); //[pramod.misal][GEOS2-4673][22-08-2023]Phone2 added.
            PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            PropertyChanged(this, new PropertyChangedEventArgs("JobTitle"));
            PropertyChanged(this, new PropertyChangedEventArgs("Remarks"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGender"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexDepartment"));
            PropertyChanged(this, new PropertyChangedEventArgs("InformationError"));
            PropertyChanged(this, new PropertyChangedEventArgs("SelectedSupplierList"));
            if (error != null)
            {
                IsBusy = false;
                return;
            }
            if (IsNew == true)
                if (IsEmailAddressExist())
                {
                    IsBusy = false;
                    return;
                }

            if (!DXSplashScreen.IsActive)
            {
                //  DXSplashScreen.Show<SplashScreenView>(); 
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
            try
            {
                SelectedContacts.IdContact = IdContact;
                SelectedContacts.Firstname = FirstName;
                SelectedContacts.Lastname = LastName;
                SelectedContacts.Email = Email;
                SelectedContacts.Phone = Phone;
                SelectedContacts.Phone2 = Phone2; //[pramod.misal][GEOS2-4673][22-08-2023]Phone2 added.
                SelectedContacts.JobTitle = jobTitle;
                SelectedContacts.Remarks = Remarks;
                SelectedContacts.IdGender = (byte)UserGenderList[SelectedIndexGender].IdLookupValue;
                SelectedContacts.IdDepartment = ListDepartment[SelectedIndexDepartment].IdLookupValue;
                SelectedContacts.OwnerImage = ContactImage;
                SelectedContacts.FullName = FullName;

                SelectedContacts.ImageText = null;
                if (Emdep.Geos.UI.Helper.ImageEditHelper.Base64String != null)
                {
                    SelectedContacts.ImageText = Emdep.Geos.UI.Helper.ImageEditHelper.Base64String;
                    byte[] imageBytes = Convert.FromBase64String(SelectedContacts.ImageText);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    SelectedContacts.OwnerImage = byteArrayToImage(imageBytes);
                    ContactImage = SelectedContacts.OwnerImage;
                }
                else
                    SelectedContacts.ImageText = null;

                SelectedContacts.OwnerImage = null;
                //shubham[skadam] for GEOS2-3432  [15-Mar-2022]
                if (ArticleSuppliersChangeLogList == null)
                {
                    ArticleSuppliersChangeLogList = new List<LogEntriesByArticleSuppliers>();
                }

                AddContactsChangedLogDetails();
                Contacts contact = new Contacts();
                SRMService = new SRMServiceController((SRMCommon.Instance.Selectedwarehouse != null && SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? SRMCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //SRMService = new SRMServiceController("localhost:6699");

                if (IsNew == true)
                {
                    if (SelectedContacts.ArticleSuppliersComments == null)
                        SelectedContacts.ArticleSuppliersComments = new List<LogEntriesByArticleSuppliers>();
                    if (AddCommentsList != null)
                    {
                        foreach (var item in AddCommentsList)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            SelectedContacts.ArticleSuppliersComments.Add(item);
                        }
                    }
                    try
                    {
                        if (SelectedContacts.ArticleSupplierContacts == null)
                            SelectedContacts.ArticleSupplierContacts = new  List<Article_Supplier_Contacts>();
                        foreach (Article_Supplier_Contacts item in TabItems)
                        {
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            SelectedContacts.ArticleSupplierContacts.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in AddAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    #region Old Code
                    ////contact = SRMService.AddContact_V2300(SelectedContacts, ArticleSuppliersChangeLogList);

                    //if (!IsAddSupplierView)//[Sudhir.jangra][GEOS2-4738]
                    //{
                    //    // contact = SRMService.AddContact_V2430(SelectedContacts, ArticleSuppliersChangeLogList);
                    //    //Changed Version Wise[Sudhir.Jangra][GEOS2-4738]
                    //    contact = SRMService.AddContact_V2440(SelectedContacts, ArticleSuppliersChangeLogList);
                    //    if (IsSupplierComboBoxVisible == Visibility.Visible)//[Sudhir.Jangra][GEOS2-4676]
                    //    {
                    //        SRMService.ArticleSupplierContacts_Insert_V2300(contact.IdContact, SelectedSupplierList.IdArticleSupplier);
                    //    }
                    //    else
                    //    {
                    //        SRMService.ArticleSupplierContacts_Insert_V2300(contact.IdContact, IdArticleSupplier);
                    //    }
                    //}
                    //else
                    //{             
                    //    //Changed Version Wise[Sudhir.Jangra][GEOS2-4738]
                    //    contact = SRMService.AddContact_V2440(SelectedContacts, null);

                    //}
                    #endregion
                    try
                    {
                        //contact = SRMService.AddContact_V2450(SelectedContacts, ArticleSuppliersChangeLogList);
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        //contact = SRMService.AddContact_V2480(SelectedContacts, ArticleSuppliersChangeLogList);
                        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
                        //SRMService = new SRMServiceController("localhost:6699");
                        contact = SRMService.AddContact_V2560(SelectedContacts, ArticleSuppliersChangeLogList);
                        if (!IsAddSupplierView)
                        {
                            if (IsSupplierComboBoxVisible == Visibility.Visible)//[Sudhir.Jangra][GEOS2-4676]
                            {
                                //SRMService.ArticleSupplierContacts_Insert_V2300(contact.IdContact, SelectedSupplierList.IdArticleSupplier);

                                //[pramod.misal][GEOS2-5136][22.01.2024] service updated from ArticleSupplierContacts_Insert_V2300 to ArticleSupplierContacts_Insert_V2480
                                //SRMService.ArticleSupplierContacts_Insert_V2480(contact.IdContact, SelectedSupplierList.IdArticleSupplier);
                                SRMService.ArticleSupplierContacts_Insert_V2560(contact.IdContact, SelectedSupplierList.IdArticleSupplier);
                            }
                            else
                            {
                                //SRMService.ArticleSupplierContacts_Insert_V2300(contact.IdContact, IdArticleSupplier);
                                //[pramod.misal][GEOS2-5136][22.01.2024] service updated from ArticleSupplierContacts_Insert_V2300 to ArticleSupplierContacts_Insert_V2480
                                //SRMService.ArticleSupplierContacts_Insert_V2480(contact.IdContact, IdArticleSupplier);
                                SRMService.ArticleSupplierContacts_Insert_V2560(contact.IdContact, IdArticleSupplier);
                            }
                        }
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AddAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    }
                }
                else
                {
                    try
                    {
                        #region //[chitra.girigosavi][GEOS2-4692][18.10.2023]
                        if (SelectedContacts.ArticleSuppliersComments == null)
                            SelectedContacts.ArticleSuppliersComments = new List<LogEntriesByArticleSuppliers>();
                        if (AddCommentsList != null)
                        {
                            foreach (var item in AddCommentsList)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Add;
                                SelectedContacts.ArticleSuppliersComments.Add(item);
                            }
                        }

                        if (UpdatedCommentsList != null)
                        {
                            foreach (var item in UpdatedCommentsList)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                SelectedContacts.ArticleSuppliersComments.Add(item);
                            }
                        }

                        if (DeleteCommentsList != null)
                        {
                            foreach (var item in DeleteCommentsList)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                SelectedContacts.ArticleSuppliersComments.Add(item);
                            }
                        }
                        #endregion
                        try
                        {
                            if (SelectedContacts.ArticleSupplierContacts == null)
                                SelectedContacts.ArticleSupplierContacts = new List<Article_Supplier_Contacts>();
                            foreach (Article_Supplier_Contacts item in TabItems)
                            {
                                ArticleSupplier articleSupplier= contactDetalis.SupplierListByIdContact.Where(w=>w.IdArticleSupplier== item.IdArticleSupplier).FirstOrDefault();
                                if (articleSupplier==null)
                                {
                                    if (UpdateArticleSupplierContactsList.Any(w=>w.IdArticleSupplier == item.IdArticleSupplier))
                                    {

                                    }
                                    else
                                    {
                                        if (item.IdArticleSupplier!=0)
                                        {
                                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                                            SelectedContacts.ArticleSupplierContacts.Add(item);

                                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                            {
                                                IdContact = SelectedContacts.IdContact,
                                                Datetime = GeosApplication.Instance.ServerDateTime,
                                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                                Comments = string.Format(System.Windows.Application.Current.FindResource("SRMContactSupplierAddChangeLog").ToString(), item.Name),
                                                IdLogEntryType = 258
                                            });
                                        }
                                    }
                                }
                            }
                            //UpdateArticleSupplierContactsList
                            foreach (Article_Supplier_Contacts item in UpdateArticleSupplierContactsList)
                            {
                                try
                                {
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    SelectedContacts.ArticleSupplierContacts.Add(item);
                                    //ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                    //{
                                    //    IdContact = SelectedContacts.IdContact,
                                    //    Datetime = GeosApplication.Instance.ServerDateTime,
                                    //    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    //    Comments = string.Format(System.Windows.Application.Current.FindResource("SRMContactSupplierUpdateChangeLog").ToString(), item.UpdatedName, item.Name),
                                    //    IdLogEntryType = 258
                                    //});

                                    Article_Supplier_Contacts tempArticle_Supplier_Contacts = new Article_Supplier_Contacts();
                                    tempArticle_Supplier_Contacts.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                    tempArticle_Supplier_Contacts.IdArticleSupplier = item.OldSelectedSupplierList.IdArticleSupplier;
                                    tempArticle_Supplier_Contacts.Name = item.OldSelectedSupplierList.Name;
                                    //DeleteArticleSupplierContactsList.Add(tempArticle_Supplier_Contacts);
                                    SelectedContacts.ArticleSupplierContacts.Add(tempArticle_Supplier_Contacts);
                                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                    {
                                        IdContact = SelectedContacts.IdContact,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("SRMContactSupplierDeleteChangeLog").ToString(), item.UpdatedName),
                                        IdLogEntryType = 258
                                    });

                                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                    {
                                        IdContact = SelectedContacts.IdContact,
                                        Datetime = GeosApplication.Instance.ServerDateTime,
                                        IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                        Comments = string.Format(System.Windows.Application.Current.FindResource("SRMContactSupplierAddChangeLog").ToString(), item.Name),
                                        IdLogEntryType = 258
                                    });

                                   
                                }
                                catch (Exception ex)
                                {
                                    GeosApplication.Instance.Logger.Log("Get an error in AddAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }

                            }
                            //DeleteArticleSupplierContactsList
                            foreach (Article_Supplier_Contacts item in DeleteArticleSupplierContactsList)
                            {
                                item.TransactionOperation = ModelBase.TransactionOperations.Delete;
                                SelectedContacts.ArticleSupplierContacts.Add(item);
                                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                                {
                                    IdContact = SelectedContacts.IdContact,
                                    Datetime = GeosApplication.Instance.ServerDateTime,
                                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                    Comments = string.Format(System.Windows.Application.Current.FindResource("SRMContactSupplierDeleteChangeLog").ToString(), item.Name),
                                    IdLogEntryType = 258
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in AddAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        //Changed Version Wise[Sudhir.Jangra][GEOS2-4738]
                        //contact = SRMService.AddContact_V2450(SelectedContacts, ArticleSuppliersChangeLogList);//[chitra.girigosavi][GEOS2-4692][18.10.2023]
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        //contact = SRMService.AddContact_V2480(SelectedContacts, ArticleSuppliersChangeLogList);
                        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
                        //SRMService = new SRMServiceController("localhost:6699");
                        contact = SRMService.AddContact_V2560(SelectedContacts, ArticleSuppliersChangeLogList);
                        if (!IsAddSupplierView)
                        {
                            if (IsSupplierComboBoxVisible == Visibility.Visible)//[Sudhir.Jangra][GEOS2-4676]
                            {
                                SRMService.ArticleSupplierContacts_Update_V2430(IdArticleSupplierContact, SelectedSupplierList.IdArticleSupplier);
                            }
                        }
                        #region Old Code       
                        //if (!IsAddSupplierView)//[Sudhir.jangra][GEOS2-4738]
                        //{
                        //    if (SelectedContacts.ArticleSuppliersComments == null)
                        //        SelectedContacts.ArticleSuppliersComments = new List<LogEntriesByArticleSuppliers>();
                        //        if (AddCommentsList != null)
                        //        {
                        //            foreach (var item in AddCommentsList)
                        //            {
                        //                item.TransactionOperation = ModelBase.TransactionOperations.Add;
                        //            SelectedContacts.ArticleSuppliersComments.Add(item);
                        //            }
                        //        }

                        //    if (UpdatedCommentsList != null)
                        //    {
                        //        foreach (var item in UpdatedCommentsList)
                        //        {
                        //            item.TransactionOperation = ModelBase.TransactionOperations.Update;
                        //            SelectedContacts.ArticleSuppliersComments.Add(item);
                        //        }
                        //    }

                        //    if (DeleteCommentsList != null)
                        //    {
                        //        foreach (var item in DeleteCommentsList)
                        //        {
                        //            item.TransactionOperation = ModelBase.TransactionOperations.Delete;
                        //            SelectedContacts.ArticleSuppliersComments.Add(item);
                        //        }
                        //    }
                        //    //Changed Version Wise[Sudhir.Jangra][GEOS2-4738]
                        //    contact = SRMService.AddContact_V2440(SelectedContacts, ArticleSuppliersChangeLogList);
                        //    if (IsSupplierComboBoxVisible == Visibility.Visible)//[Sudhir.Jangra][GEOS2-4676]
                        //    {
                        //        SRMService.ArticleSupplierContacts_Update_V2430(IdArticleSupplierContact, SelectedSupplierList.IdArticleSupplier);                 
                        //    }
                        //}
                        //else
                        //{
                        //    // contact = SRMService.AddContact_V2430(SelectedContacts, ArticleSuppliersChangeLogList);
                        //    //Changed Version Wise[Sudhir.Jangra][GEOS2-4738]
                        //    contact = SRMService.AddContact_V2440(SelectedContacts, null);
                        //}




                        #endregion
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("EditContactViewUpdateSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.Logger.Log("Get an error in AddAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                ContactForIsSave = contact;//[Sudhir.jangra][GEOS2-4738]
                IsSave = true;
                RequestClose(null, null);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AddAcceptAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in AddAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            IsBusy = false;
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void EditCustomerDetailsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GridDoubleClickCommandAction....", category: Category.Info, priority: Priority.Low);
                if (SelectedTabItem.IdArticleSupplier == 0)
                {
                    return;
                }
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

                EditArticleSupplierViewModel editArticleSupplierViewModel = new EditArticleSupplierViewModel();
                EditArticleSupplierView editArticleSupplierView = new EditArticleSupplierView();

                EventHandler handle = delegate { editArticleSupplierView.Close(); };
                editArticleSupplierViewModel.RequestClose += handle;

                Warehouses warehouse = SRM.SRMCommon.Instance.Selectedwarehouse;
                //editArticleSupplierViewModel.Init((ulong)articleSupplier.IdArticleSupplier, warehouse);
                editArticleSupplierViewModel.Init((ulong)SelectedTabItem.IdArticleSupplier, warehouse);
                editArticleSupplierView.DataContext = editArticleSupplierViewModel;
                editArticleSupplierView.ShowDialog();

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }

                GeosApplication.Instance.Logger.Log("Method GridDoubleClickCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method GridDoubleClickCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void OnEditValueChanging(EditValueChangingEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging ...", category: Category.Info, priority: Priority.Low);

            int msgcount = 0;
            var newInput = (string)e.NewValue;
            var oldInput = (string)e.OldValue;

            if (!string.IsNullOrEmpty(newInput))
            {
                if (newInput.Count(char.IsDigit) > 0)
                {
                    if (IsFromFirstnameCmb)
                    {
                        AlertVisibilityFirstName = Visibility.Visible;
                        AlertVisibilityLastName = Visibility.Hidden;
                        msgcount++;
                    }

                    if (IsFromLastnameCmb)
                    {
                        AlertVisibilityFirstName = Visibility.Hidden;
                        AlertVisibilityLastName = Visibility.Visible;
                        msgcount++;
                    }
                }

                MatchCollection matches = regex.Matches(newInput.ToLower().ToString());

                if (matches.Count > 0)
                {
                    if (IsFromFirstnameCmb)
                    {
                        AlertVisibilityFirstName = Visibility.Visible;
                        AlertVisibilityLastName = Visibility.Hidden;
                        msgcount++;
                    }

                    if (IsFromLastnameCmb)
                    {
                        AlertVisibilityFirstName = Visibility.Hidden;
                        AlertVisibilityLastName = Visibility.Visible;
                        msgcount++;
                    }
                    e.Handled = true;

                }

                string error = EnableValidationAndGetError();
                if (IsFromFirstnameCmb)
                {
                    ShowPopupAsPerFirstName(FirstName);
                }
                if (IsFromLastnameCmb)
                {
                    ShowPopupAsPerLastName(LastName);
                }
            }
            else ShouldKeepValue = false;

            GeosApplication.Instance.Logger.Log("Method OnEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void EditInit(Contacts contactData)
        {
            try
            {
                if (contactData != null)
                {
                    try
                    {
                        contactDetalis = (Contacts)contactData.Clone();
                    }
                    catch (Exception ex)
                    {
                    }
                    IdContact = contactData.IdContact;
                    IdArticleSupplierContact = contactData.IdArticleSupplierContact;
                    SelectedIndexGender = UserGenderList.IndexOf(UserGenderList.FirstOrDefault(x => x.IdLookupValue == contactData.IdGender));
                    FirstName = contactData.Firstname;
                    LastName = contactData.Lastname;
                    SelectedIndexDepartment = ListDepartment.IndexOf(ListDepartment.FirstOrDefault(i => i.IdLookupValue == contactData.IdDepartment));
                    JobTitle = contactData.JobTitle;
                    Phone = contactData.Phone;
                    Phone2 = contactData.Phone2;
                    Email = contactData.Email;
                    Remarks = contactData.Remarks;
                    FullName = contactData.FullName;
                    ContactImage = contactData.OwnerImage;
                    
                    //[chitra.girigosavi][GEOS2-4692][18.10.2023]
                    CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(SRMService.GetArticleSuppliersContactsComments_V2450(contactData.IdContact));
                    try
                    {
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(CommentsList.OrderByDescending(x => x.Datetime));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    if (CommentsList?.Count > 0)
                    {
                        CommentText = CommentsList[CommentsList.Count - 1].Comments; // Get the last added comment
                        CommentDatetimeText = CommentsList[CommentsList.Count - 1].Datetime;
                        CommentFullNameText = CommentsList[CommentsList.Count - 1].People.FullName;
                    }
                    else
                    {
                        CommentText = string.Empty; // or some default value if there are no comments
                        CommentDatetimeText = null;
                        CommentFullNameText = string.Empty;
                    }
                    foreach (var item in CommentsList)
                    {
                        SetUserProfileImage(item);
                    }
                    ListContactChangeLog = new ObservableCollection<LogEntriesByArticleSuppliers>(SRMService.GetArticleSuppliersContactsChangelog_V2450(contactData.IdContact));
                    try
                    {
						//Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        ListContactChangeLog = new ObservableCollection<LogEntriesByArticleSuppliers>(ListContactChangeLog.OrderByDescending(x => x.IdLogEntryByContact));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }

                    if (ListContactChangeLog.Count > 0)
                    {
                        ChangeLogText = ListContactChangeLog[ListContactChangeLog.Count - 1].Comments; // Get the last added comment
                        ChangeLogDatetimeText = ListContactChangeLog[ListContactChangeLog.Count - 1].Datetime;
                        ChangeLogFullNameText = ListContactChangeLog[ListContactChangeLog.Count - 1].People.FullName;
                    }
                    else
                    {
                        ChangeLogText = string.Empty; // or some default value if there are no comments
                        ChangeLogDatetimeText = null;
                        ChangeLogFullNameText = string.Empty;
                    }
                    #region

                    if (contactData.OwnerImage == null)
                    {
                        if (!string.IsNullOrEmpty(contactData.ImageText))
                        {
                            byte[] imageBytes = Convert.FromBase64String(contactData.ImageText);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            contactData.OwnerImage = byteArrayToImage(imageBytes);
                            ContactImage = contactData.OwnerImage;
                            Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = contactData.ImageText;
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (contactData.IdGender == 1)
                                {
                                    contactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (contactData.IdGender == 2)
                                {
                                    contactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                            else
                            {
                                if (contactData.IdGender == 1)
                                {
                                    contactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (contactData.IdGender == 2)
                                {
                                    contactData.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    IsEditSupplierVisible = Visibility.Collapsed;
                    IsNew = true;
                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                }

                //  FillSupplierList();
                SelectedSupplierList = SupplierList.FirstOrDefault(x => x.IdArticleSupplier == contactData.IdArticleSupplier);
                articleSupplier = SelectedSupplierList;
                IdArticleSupplier = SelectedSupplierList.IdArticleSupplier;
                EMDEPCode = SelectedSupplierList.Code;
                SName = SelectedSupplierList.Name;
                Address = SelectedSupplierList.Address;
                City = SelectedSupplierList.City;
                Region = SelectedSupplierList.Region;
                Country = SelectedSupplierList.Country.Name;
                PostCode = SelectedSupplierList.PostCode;

                #region //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
                try
                {
                    SupplierListByIdContact = new ObservableCollection<ArticleSupplier>();
                    if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                    {
                        List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                        foreach (Warehouses item in plantOwners)
                        {
                            //SRMService = new SRMServiceController("localhost:6699");
                            ObservableCollection<ArticleSupplier> tempArticleSupplier =  new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSupplierContactsByIdContact_V2560(item, contactData.IdContact));
                            SupplierListByIdContact.AddRange(tempArticleSupplier);
                        }
                        if (contactDetalis.SupplierListByIdContact == null)
                        {
                            contactDetalis.SupplierListByIdContact = new List<ArticleSupplier>();
                        }
                        SupplierListByIdContact = new ObservableCollection<ArticleSupplier>(SupplierListByIdContact .GroupBy(w => w.IdArticleSupplier) .Select(g => g.First()));
                        contactDetalis.SupplierListByIdContact.AddRange(SupplierListByIdContact.ToList());
                    }
                    foreach (ArticleSupplier item in SupplierListByIdContact)
                    {
                        Article_Supplier_Contacts supplierRecord = new Article_Supplier_Contacts();
                        supplierRecord.TransactionOperation = ModelBase.TransactionOperations.Update;
                        supplierRecord.Name = item.Name;
                        supplierRecord.ShortName = FormatTabName(item.Name);
                        supplierRecord.EMDEPCode = item.Code;
                        supplierRecord.Address = item.Address;
                        supplierRecord.City = item.City;
                        supplierRecord.Region = item.Region;
                        supplierRecord.Country = item.Country.Name;
                        supplierRecord.PostCode = item.PostCode;
                        supplierRecord.SupplierList = new List<ArticleSupplier>();
                        supplierRecord.SupplierList.AddRange(SupplierList.ToList());
                        supplierRecord.IdArticleSupplier = item.IdArticleSupplier;
                        supplierRecord.SelectedSupplierList = SupplierList.FirstOrDefault(x => x.IdArticleSupplier == item.IdArticleSupplier);
                        if (supplierRecord.SelectedSupplierList == null)
                        {
                            supplierRecord.SelectedSupplierList = SupplierListByIdContact.FirstOrDefault(x => x.IdArticleSupplier == item.IdArticleSupplier);
                            SupplierList.Add(supplierRecord.SelectedSupplierList);
                        }
                        TabItems.Add(supplierRecord);
                    }
                    SelectedTabItem = TabItems.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #endregion
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }


        public void Init(ArticleSupplier ArticleSupplier, Contacts contactsData)
        {
            if (!IsAddSupplierView)
            {
                //shubham[skadam] for GEOS2-3432  [15-Mar-2022]
                if (contactsData != null)
                {
                    contactDetalis = (Contacts)contactsData.Clone();

                }

                ArticleSupplierDetalis = (ArticleSupplier)ArticleSupplier.Clone();
                if (contactsData != null)
                {
                    IdContact = contactsData.IdContact;
                    IsEditSupplierVisible = Visibility.Visible;
                    SelectedContacts = contactsData;
                    FirstName = SelectedContacts.Firstname;
                    LastName = SelectedContacts.Lastname;
                    JobTitle = SelectedContacts.JobTitle;
                    Phone = SelectedContacts.Phone;//[pramod.misal][GEOS2-4673][22-08-2023]
                    Phone2 = SelectedContacts.Phone2;//[pramod.misal][GEOS2-4674][23-08-2023]
                    Email = SelectedContacts.Email;
                    Remarks = SelectedContacts.Remarks;
                    SelectedIndexDepartment = ListDepartment.IndexOf(ListDepartment.FirstOrDefault(i => i.IdLookupValue == SelectedContacts.IdDepartment));
                    SelectedIndexGender = UserGenderList.IndexOf(UserGenderList.FirstOrDefault(x => x.IdLookupValue == SelectedContacts.IdGender));
                    FullName = SelectedContacts.FullName;
                    ContactImage = SelectedContacts.OwnerImage;

                    //[chitra.girigosavi][GEOS2-4692][18.10.2023]
                    CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(SRMService.GetArticleSuppliersContactsComments_V2450(contactsData.IdContact));
                    foreach (var item in CommentsList)
                    {
                        SetUserProfileImage(item);
                    }
                    if (CommentsList?.Count > 0)
                    {
                        CommentText = CommentsList[CommentsList.Count - 1].Comments; // Get the last added comment
                        CommentDatetimeText = CommentsList[CommentsList.Count - 1].Datetime;
                        CommentFullNameText = CommentsList[CommentsList.Count - 1].People.FullName;
                    }
                    else
                    {
                        CommentText = string.Empty; // or some default value if there are no comments
                        CommentDatetimeText = null;
                        CommentFullNameText = string.Empty;
                    }
                    ListContactChangeLog = new ObservableCollection<LogEntriesByArticleSuppliers>(SRMService.GetArticleSuppliersContactsChangelog_V2450(contactsData.IdContact));
                    if (ListContactChangeLog.Count > 0)
                    {
                        ChangeLogText = ListContactChangeLog[ListContactChangeLog.Count - 1].Comments; // Get the last added comment
                        ChangeLogDatetimeText = ListContactChangeLog[ListContactChangeLog.Count - 1].Datetime;
                        ChangeLogFullNameText = ListContactChangeLog[ListContactChangeLog.Count - 1].People.FullName;
                    }
                    else
                    {
                        ChangeLogText = string.Empty; // or some default value if there are no comments
                        ChangeLogDatetimeText = null;
                        ChangeLogFullNameText = string.Empty;
                    }

                    if (SelectedContacts.OwnerImage == null)
                    {
                        if (!string.IsNullOrEmpty(SelectedContacts.ImageText))
                        {
                            byte[] imageBytes = Convert.FromBase64String(SelectedContacts.ImageText);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            SelectedContacts.OwnerImage = byteArrayToImage(imageBytes);
                            ContactImage = SelectedContacts.OwnerImage;
                            Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = SelectedContacts.ImageText;
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (SelectedContacts.IdGender == 1)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (SelectedContacts.IdGender == 2)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                            else
                            {
                                if (SelectedContacts.IdGender == 1)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (SelectedContacts.IdGender == 2)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    IsEditSupplierVisible = Visibility.Collapsed;
                    IsNew = true;
                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                }



                //Supplier   
                articleSupplier = ArticleSupplier;
                IdArticleSupplier = ArticleSupplier.IdArticleSupplier;
                EMDEPCode = ArticleSupplier.Code;
                SName = ArticleSupplier.Name;
                Address = ArticleSupplier.Address;
                City = ArticleSupplier.City;
                Region = ArticleSupplier.Region;
                Country = ArticleSupplier.Country.Name;
                PostCode = ArticleSupplier.PostCode;
            }
            else
            {
                if (contactsData != null)
                {
                    contactDetalis = (Contacts)contactsData.Clone();

                }

                ArticleSupplierDetalis = (ArticleSupplier)ArticleSupplier.Clone();
                if (contactsData != null)
                {
                    IdContact = contactsData.IdContact;
                    IsEditSupplierVisible = Visibility.Visible;
                    SelectedContacts = contactsData;
                    FirstName = SelectedContacts.Firstname;
                    LastName = SelectedContacts.Lastname;
                    JobTitle = SelectedContacts.JobTitle;
                    Phone = SelectedContacts.Phone;
                    Phone2 = SelectedContacts.Phone2;
                    Email = SelectedContacts.Email;
                    Remarks = SelectedContacts.Remarks;
                    SelectedIndexDepartment = ListDepartment.IndexOf(ListDepartment.FirstOrDefault(i => i.IdLookupValue == SelectedContacts.IdDepartment));
                    SelectedIndexGender = UserGenderList.IndexOf(UserGenderList.FirstOrDefault(x => x.IdLookupValue == SelectedContacts.IdGender));
                    FullName = SelectedContacts.FullName;
                    ContactImage = SelectedContacts.OwnerImage;

                    if (SelectedContacts.OwnerImage == null)
                    {
                        if (!string.IsNullOrEmpty(SelectedContacts.ImageText))
                        {
                            byte[] imageBytes = Convert.FromBase64String(SelectedContacts.ImageText);
                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                            ms.Write(imageBytes, 0, imageBytes.Length);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                            SelectedContacts.OwnerImage = byteArrayToImage(imageBytes);
                            ContactImage = SelectedContacts.OwnerImage;
                            Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = SelectedContacts.ImageText;
                        }
                        else
                        {
                            if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                            {
                                if (SelectedContacts.IdGender == 1)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (SelectedContacts.IdGender == 2)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                            else
                            {
                                if (SelectedContacts.IdGender == 1)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                                else if (SelectedContacts.IdGender == 2)
                                {
                                    SelectedContacts.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    ContactImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    IsEditSupplierVisible = Visibility.Collapsed;
                    IsNew = true;
                    Emdep.Geos.UI.Helper.ImageEditHelper.Base64String = null;
                }
                //Supplier   
                articleSupplier = ArticleSupplier;
                IdArticleSupplier = ArticleSupplier.IdArticleSupplier;
                EMDEPCode = ArticleSupplier.Code;
                SName = ArticleSupplier.Name;
                Address = ArticleSupplier.Address;
                City = ArticleSupplier.City;
                Region = ArticleSupplier.Region;
                Country = ArticleSupplier.Country.Name;
                PostCode = ArticleSupplier.PostCode;
                
            }
            #region //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
            try
            {
                SupplierListByIdContact = new ObservableCollection<ArticleSupplier>();
                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    foreach (Warehouses item in plantOwners)
                    {
                        if (contactsData!=null)
                        {
                            //SRMService = new SRMServiceController("localhost:6699");
                            ObservableCollection<ArticleSupplier> tempArticleSupplier = new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSupplierContactsByIdContact_V2560(item, contactsData.IdContact));
                            SupplierListByIdContact.AddRange(tempArticleSupplier);
                        }
                    }
                    if (contactsData != null)
                    {
                        if (contactDetalis.SupplierListByIdContact == null)
                        {
                            contactDetalis.SupplierListByIdContact = new List<ArticleSupplier>();
                        }
                        SupplierListByIdContact = new ObservableCollection<ArticleSupplier>(SupplierListByIdContact.GroupBy(w => w.IdArticleSupplier).Select(g => g.First()));
                        contactDetalis.SupplierListByIdContact.AddRange(SupplierListByIdContact.ToList());
                    }
                    else
                    {
                        AddTab();
                    }
                }
                foreach (ArticleSupplier item in SupplierListByIdContact)
                {
                    Article_Supplier_Contacts supplierRecord = new Article_Supplier_Contacts();
                    supplierRecord.TransactionOperation = ModelBase.TransactionOperations.Update;
                    supplierRecord.Name = item.Name;
                    supplierRecord.ShortName = FormatTabName(item.Name);
                    supplierRecord.EMDEPCode = item.Code;
                    supplierRecord.Address = item.Address;
                    supplierRecord.City = item.City;
                    supplierRecord.Region = item.Region;
                    supplierRecord.Country = item.Country.Name;
                    supplierRecord.PostCode = item.PostCode;
                    supplierRecord.SupplierList = new List<ArticleSupplier>();
                    if (SupplierList==null)
                    {
                        FillSupplierList();
                    }
                    supplierRecord.SupplierList.AddRange(SupplierList.ToList());
                    supplierRecord.IdArticleSupplier = item.IdArticleSupplier;
                    supplierRecord.SelectedSupplierList = SupplierList.FirstOrDefault(x => x.IdArticleSupplier == item.IdArticleSupplier);
                    if (supplierRecord.SelectedSupplierList == null)
                    {
                        supplierRecord.SelectedSupplierList = SupplierListByIdContact.FirstOrDefault(x => x.IdArticleSupplier == item.IdArticleSupplier);
                        SupplierList.Add(supplierRecord.SelectedSupplierList);
                    }
                    TabItems.Add(supplierRecord);
                }
                SelectedTabItem = TabItems.FirstOrDefault();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            #endregion
        }

        public void FillList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLookup ...", category: Category.Info, priority: Priority.Low);

                //Gender
                if (GeosApplication.Instance.ObjectPool.ContainsKey("SRM_GENDER"))
                {
                    UserGenderList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["SRM_GENDER"];
                    SelectedIndexGender = -1;
                }
                else
                {
                    UserGenderList = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(1).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("SRM_GENDER", UserGenderList);
                    SelectedIndexGender = -1;
                }

                //Department 
                if (GeosApplication.Instance.ObjectPool.ContainsKey("SRM_DEPARTMENT"))
                {
                    ListDepartment = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["SRM_DEPARTMENT"];
                    SelectedIndexDepartment = -1;
                }
                else
                {
                    ListDepartment = new ObservableCollection<LookupValue>(CrmStartUp.GetLookupValues(21).AsEnumerable());
                    GeosApplication.Instance.ObjectPool.Add("SRM_DEPARTMENT", ListDepartment);
                    SelectedIndexDepartment = -1;
                }

                AllContactsList = SRMService.GetAllContacts_V2250();

                GeosApplication.Instance.Logger.Log("Method FillList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (System.Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

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
        /// Method for check email address is already exist or not.
        /// </summary>
        /// <returns></returns>
        //shubham[skadam] for GEOS2-3432  [22-Mar-2022]
        private bool IsEmailAddressExist()
        {
            GeosApplication.Instance.Logger.Log("Method IsEmailAddressExist() ...", category: Category.Info, priority: Priority.Low);
            bool isEmailExist = false;
            try
            {
                isEmailExist = AllContactsList.Any(pl => pl.Email.ToUpper().Equals(Email.ToUpper()));

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

        /// <summary>
        /// Method for search similar first name.
        /// </summary>
        /// <param name="fName"></param>
        private void ShowPopupAsPerFirstName(string fName)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerFirstName ...", category: Category.Info, priority: Priority.Low);

                IsFromFirstnameCmb = true;
                IsFromLastnameCmb = false;
                AllContactsFirstNameList = AllContactsList;
                if (AllContactsFirstNameList != null && !string.IsNullOrEmpty(fName))
                {
                    if (fName.Length > 1)
                    {
                        AllContactsFirstNameList = AllContactsFirstNameList.Where(h => h.Firstname.ToUpper().Contains(fName.ToUpper()) || h.Firstname.ToUpper().StartsWith(fName.Substring(0, 2).ToUpper())
                                                                || h.Firstname.ToUpper().EndsWith(fName.Substring(fName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Firstname, fName)).ToList();
                        AllContactsFirstNameSrtList = AllContactsFirstNameList.Select(pn => pn.Firstname).ToList();
                    }
                    else
                    {
                        AllContactsFirstNameList = AllContactsFirstNameList.Where(h => h.Firstname.ToUpper().Contains(fName.ToUpper()) || h.Firstname.ToUpper().StartsWith(fName.Substring(0, 1).ToUpper())
                                                                || h.Firstname.ToUpper().EndsWith(fName.Substring(fName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Firstname, fName)).ToList();
                        AllContactsFirstNameSrtList = AllContactsFirstNameList.Select(pn => pn.Firstname).ToList();
                    }
                }
                else
                {
                    AllContactsFirstNameList = new List<Contacts>();
                    AllContactsFirstNameSrtList = new List<string>();
                    AlertVisibilityFirstName = Visibility.Hidden;
                }

                if (AllContactsFirstNameList.Count > 0)
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
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPopupAsPerFirstName() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Warning if same fullname name is exist in database.
        /// </summary>
        /// <param name="obj"></param>
        private bool FullNameExist()
        {
            GeosApplication.Instance.Logger.Log("Method FullNameExist() ...", category: Category.Info, priority: Priority.Low);
            List<Contacts> LstContact = AllContactsList;
            bool isFullNameExist = false;
            try
            {
                string contactname = string.Empty;
                if (FirstName != null && LastName != null)
                {
                    contactname = FirstName.Trim() + " " + LastName.Trim();

                    //this condition execute only once.
                    if (string.IsNullOrEmpty(oldMatchName))
                    {

                        bool isNameExistOnDB = LstContact.Any(pl => pl.FullName.ToUpper().Equals(contactname.ToUpper()));
                        oldMatchName = contactname;
                        if (isNameExistOnDB)
                        {
                            isFullNameExist = true;
                            // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddContactNameExist").ToString(), customername), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            isBusy = false;
                            GeosApplication.Instance.Logger.Log("Method FullNameExist() executed Successfully.", category: Category.Info, priority: Priority.Low);
                            return isFullNameExist;
                        }
                    }

                    if (!oldMatchName.Equals(contactname))
                    {
                        bool isNameExistOnDB = LstContact.Any(pl => pl.FullName.ToUpper().Equals(contactname.ToUpper()));
                        oldMatchName = contactname;
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
        /// Method for search similar last name.
        /// </summary>
        /// <param name="lName"></param>
        private void ShowPopupAsPerLastName(string lName)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerLastName ...", category: Category.Info, priority: Priority.Low);
                AllContactsLastNameList = AllContactsList.ToList();
                IsFromFirstnameCmb = false;
                IsFromLastnameCmb = true;

                if (AllContactsLastNameList != null && !string.IsNullOrEmpty(lName))
                {
                    if (lName.Length > 1)
                    {
                        AllContactsLastNameList = AllContactsLastNameList.Where(h => h.Lastname.ToUpper().Contains(lName.ToUpper()) || h.Lastname.ToUpper().StartsWith(lName.Substring(0, 2).ToUpper())
                                                                || h.Lastname.ToUpper().EndsWith(lName.Substring(lName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Lastname, lName)).ToList();
                        AllContactsLastNameSrtList = AllContactsLastNameList.Select(pn => pn.Lastname).ToList();
                    }
                    else
                    {
                        AllContactsLastNameList = AllContactsLastNameList.Where(h => h.Lastname.ToUpper().Contains(lName.ToUpper()) || h.Lastname.ToUpper().StartsWith(lName.Substring(0, 1).ToUpper())
                                                                || h.Lastname.ToUpper().EndsWith(lName.Substring(lName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Lastname, lName)).ToList();
                        AllContactsLastNameSrtList = AllContactsLastNameList.Select(pn => pn.Lastname).ToList();
                    }
                }
                else
                {
                    AllContactsLastNameList = new List<Contacts>();
                    AllContactsLastNameSrtList = new List<string>();
                    AlertVisibilityLastName = Visibility.Hidden;
                }

                if (AllContactsLastNameList.Count > 0)
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
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShowPopupAsPerLastName() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        double StringSimilarityScore(string name, string searchString)
        {
            if (name.Contains(searchString))
            {
                return (double)searchString.Length / (double)name.Length;
            }
            return 0;
        }

        public void Dispose()
        {
        }

        //[Sudhir.jangra][GEOS2-4676]
        //public void FillSupplierList()
        //{
        //    try
        //    {
        //        if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
        //        {
        //            DXSplashScreen.Show(x =>
        //            {
        //                Window win = new Window()
        //                {
        //                    ShowActivated = false,
        //                    WindowStyle = WindowStyle.None,
        //                    ResizeMode = ResizeMode.NoResize,
        //                    AllowsTransparency = true,
        //                    Background = new SolidColorBrush(Colors.Transparent),
        //                    ShowInTaskbar = false,
        //                    Topmost = true,
        //                    SizeToContent = SizeToContent.WidthAndHeight,
        //                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
        //                };
        //                WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
        //                win.Topmost = false;
        //                return win;
        //            }, x =>
        //            {
        //                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
        //            }, null, null);
        //        }

        //        if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
        //        {
        //            SupplierList = new ObservableCollection<ArticleSupplier>();
        //            List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
        //            foreach (var item in plantOwners)
        //            {
        //                var temp = new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSuppliersForSRMContact_V2430(item));
        //                SupplierList.AddRange(temp);
        //            }
        //        }


        //       // SupplierList = new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSuppliersForSRMContact_V2430(SRMCommon.Instance.Selectedwarehouse));

        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an error in Method FillSupplierList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //[sudhir.jangra][GEOS2-4676]

        private void FillSupplierListBasedOnEmployeeCodeNew()
        {
            try
            {
                if (SelectedSupplierList!=null)
                {
                    EMDEPCode = SelectedSupplierList.Code;
                    SName = SelectedSupplierList.Name;
                    //ShortName = FormatTabName(SelectedSupplierList.Name);
                    Address = SelectedSupplierList.Address;
                    City = SelectedSupplierList.City;
                    Region = SelectedSupplierList.Region;
                    Country = SelectedSupplierList.Country.Name;
                    PostCode = SelectedSupplierList.PostCode;
                }
                else
                {
                    //AddTab();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillSupplierListBasedOnEmployeeCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillSupplierListBasedOnEmployeeCode()
        {
            try
            {
                if (SelectedSupplierList!=null)
                {
                    EMDEPCode = SelectedSupplierList.Code;
                    SName = SelectedSupplierList.Name;
                    ShortName = FormatTabName(SelectedSupplierList.Name);
                    Address = SelectedSupplierList.Address;
                    City = SelectedSupplierList.City;
                    Region = SelectedSupplierList.Region;
                    Country = SelectedSupplierList.Country.Name;
                    PostCode = SelectedSupplierList.PostCode;
                    if (SelectedTabItem !=null)
                    {
                        SelectedTabItem.ShortName = ShortName;
                        Article_Supplier_Contacts article_Supplier_Contacts = TabItems.Where(w => w == SelectedTabItem).FirstOrDefault();
                        article_Supplier_Contacts.EMDEPCode = SelectedSupplierList.Code;
                        article_Supplier_Contacts.Name = SelectedSupplierList.Name;
                        article_Supplier_Contacts.ShortName = ShortName;
                        article_Supplier_Contacts.Address = SelectedSupplierList.Address;
                        article_Supplier_Contacts.City = SelectedSupplierList.City;
                        article_Supplier_Contacts.Region = SelectedSupplierList.Region;
                        article_Supplier_Contacts.Country = SelectedSupplierList.Country.Name;
                        article_Supplier_Contacts.PostCode = SelectedSupplierList.PostCode;
                        article_Supplier_Contacts.IdArticleSupplier = SelectedSupplierList.IdArticleSupplier;
                        if (UpdateArticleSupplierContactsList.Any(a=>a.IdArticleSupplier== article_Supplier_Contacts.IdArticleSupplier))
                        {
                            Article_Supplier_Contacts updatearticleSupplierContacts= UpdateArticleSupplierContactsList.Where(a => a.IdArticleSupplier == article_Supplier_Contacts.IdArticleSupplier).FirstOrDefault();
                            updatearticleSupplierContacts.IdArticleSupplier = SelectedSupplierList.IdArticleSupplier;
                            article_Supplier_Contacts.TransactionOperation = ModelBase.TransactionOperations.Update;
                            //UpdateArticleSupplierContactsList.Add(article_Supplier_Contacts);
                            if (article_Supplier_Contacts.IdArticleSupplier== article_Supplier_Contacts.OldSelectedSupplierList.IdArticleSupplier)
                            {
                                UpdateArticleSupplierContactsList.RemoveAll(r=>r.IdArticleSupplier== article_Supplier_Contacts.OldSelectedSupplierList.IdArticleSupplier);
                            }
                        }
                        else
                        {
                           if (contactDetalis!=null)
                            if (contactDetalis.SupplierListByIdContact!=null)
                            {
                                if (contactDetalis.SupplierListByIdContact.Any(w=>w.IdArticleSupplier == SelectedSupplierList.IdArticleSupplier))
                                {
                                }
                                else
                                {
                                    
                                    if (string.IsNullOrEmpty(article_Supplier_Contacts.UpdatedName))
                                    {
                                        if (OldSelectedSupplierList==null || article_Supplier_Contacts.TransactionOperation == ModelBase.TransactionOperations.Add)
                                        {
                                            article_Supplier_Contacts.TransactionOperation = ModelBase.TransactionOperations.Add;
                                        }
                                        else
                                        {
                                            article_Supplier_Contacts.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            article_Supplier_Contacts.UpdatedName = OldSelectedSupplierList.Name;
                                            article_Supplier_Contacts.OldSelectedSupplierList = OldSelectedSupplierList;
                                            UpdateArticleSupplierContactsList.Add(article_Supplier_Contacts);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (SelectedTabItem!=null && SelectedTabItem.Name!= "New Supplier")
                    {
                        SelectedSupplierList = SupplierList.FirstOrDefault(x => x.Name.Equals(SelectedTabItem.Name));
                    }
                }
               
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillSupplierListBasedOnEmployeeCode() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        public void DeleteCommentCommandAction(object parameter)
        {

            GeosApplication.Instance.Logger.Log("Method convert DeleteCommentCommandAction ...", category: Category.Info, priority: Priority.Low);

            LogEntriesByArticleSuppliers commentObject = (LogEntriesByArticleSuppliers)parameter;


            bool result = false;
            if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 47))
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteComment"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (CommentsList != null && CommentsList?.Count > 0)
                    {
                        LogEntriesByArticleSuppliers Comment = (LogEntriesByArticleSuppliers)commentObject;
                        //result = SAMService.DeleteComment_V2340(Comment.IdComment,Site);
                        CommentsList.Remove(Comment);

                        if (DeleteCommentsList == null)
                            DeleteCommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>();

                        DeleteCommentsList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdUser = Comment.IdUser,
                            IdArticleSupplier = Comment.IdArticleSupplier,
                            Comments = Comment.Comments,
                            IsRtfText = Comment.IsRtfText,
                            IdLogEntryByContact = Comment.IdLogEntryByContact,
                            IdLogEntryByArticleSuppliers = Comment.IdLogEntryByArticleSuppliers
                        });
                        CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(CommentsList);
                        SrmComments = Comment;
                        IsDeleted = true;
                        if (CommentsList?.Count > 0)
                        {
                            CommentText = CommentsList[CommentsList.Count - 1].Comments; // Get the last added comment
                            CommentDatetimeText = CommentsList[CommentsList.Count - 1].Datetime;
                            CommentFullNameText = CommentsList[CommentsList.Count - 1].People.FullName;
                        }
                        else
                        {
                            CommentText = string.Empty; // or some default value if there are no comments
                            CommentDatetimeText = null;
                            CommentFullNameText = string.Empty;
                        }
                    }
                }
            }



            //NewItemComment = null;

            GeosApplication.Instance.Logger.Log("Method DeleteCommentCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        private void AddCommentsCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFile()...", category: Category.Info, priority: Priority.Low);
                GridControl gridControlView = (GridControl)obj;
                AddSRMContactsCommentsView addCommentsView = new AddSRMContactsCommentsView();
                AddSRMContactsCommentsViewModel addCommentsViewModel = new AddSRMContactsCommentsViewModel();
                EventHandler handle = delegate { addCommentsView.Close(); };
                addCommentsViewModel.RequestClose += handle;
                addCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("AddCommentsHeader").ToString();
                var ownerInfo = (gridControlView as FrameworkElement);
                addCommentsView.Owner = Window.GetWindow(ownerInfo);
                //addCommentsViewModel.IsNew = true;
                //addCommentsViewModel.Init();

                addCommentsView.DataContext = addCommentsViewModel;
                addCommentsView.ShowDialog();
                if (addCommentsViewModel.SelectedComment != null)
                {

                    if (CommentsList == null)
                        CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>();

                    addCommentsViewModel.SelectedComment.IdArticleSupplier = contacts.IdArticleSupplier;

                    if (AddCommentsList == null)
                        AddCommentsList = new List<LogEntriesByArticleSuppliers>();

                    AddCommentsList.Add(new LogEntriesByArticleSuppliers()
                    {
                        IdUser = addCommentsViewModel.SelectedComment.IdUser,
                        IdArticleSupplier = addCommentsViewModel.SelectedComment.IdArticleSupplier,
                        Comments = addCommentsViewModel.SelectedComment.Comments,
                        IsRtfText = addCommentsViewModel.SelectedComment.IsRtfText
                    });
                    CommentsList.Add(addCommentsViewModel.SelectedComment);
                    try
                    {
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(CommentsList.OrderByDescending(x => x.Datetime));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AddCommentsCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    SelectedComment = addCommentsViewModel.SelectedComment;
                    CommentText = SelectedComment.Comments;
                    CommentDatetimeText = DateTime.Now;
                    CommentFullNameText = GeosApplication.Instance.ActiveUser.FullName;
                }
                GeosApplication.Instance.Logger.Log("Method AddCommentsCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddCommentsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[chitra.girigosavi][GEOS2-4692][09.10.2023]
        private void CommentDoubleClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method CommentDoubleClickCommandAction()...", category: Category.Info, priority: Priority.Low);
            LogEntriesByArticleSuppliers logcomments = (LogEntriesByArticleSuppliers)obj;
            //int idlogentrybyidcontact = logcomments.IdLogEntryByContact;
            AddSRMContactsCommentsView editCommentsView = new AddSRMContactsCommentsView();
            AddSRMContactsCommentsViewModel editCommentsViewModel = new AddSRMContactsCommentsViewModel();
            EventHandler handle = delegate { editCommentsView.Close(); };
            editCommentsViewModel.RequestClose += handle;
            editCommentsViewModel.WindowHeader = System.Windows.Application.Current.FindResource("EditCommentsHeader").ToString();
            editCommentsViewModel.NewItemComment = SelectedComment.Comments;
            editCommentsViewModel.IdLogEntryByItem = SelectedComment.IdLogEntryByContact;
            string oldComments = CommentsList.FirstOrDefault(s => s.IdLogEntryByContact == SelectedComment.IdLogEntryByContact).Comments;
            editCommentsView.DataContext = editCommentsViewModel;
            editCommentsView.ShowDialog();

            if (editCommentsViewModel.SelectedComment != null)
            {
                SelectedComment.Comments = editCommentsViewModel.NewItemComment;
                if (UpdatedCommentsList == null)
                    UpdatedCommentsList = new List<LogEntriesByArticleSuppliers>();

                editCommentsViewModel.SelectedComment.IdArticleSupplier = SelectedContacts.IdArticleSupplier;
                //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                if (!oldComments.ToLower().Equals(editCommentsViewModel.NewItemComment.ToLower()))
                {
                    UpdatedCommentsList.Add(new LogEntriesByArticleSuppliers()
                    {
                        IdUser = SelectedComment.IdUser,
                        IdArticleSupplier = SelectedComment.IdArticleSupplier,
                        Comments = SelectedComment.Comments,
                        IsRtfText = SelectedComment.IsRtfText,
                        IdLogEntryByContact = SelectedComment.IdLogEntryByContact,
                        IdLogEntryByArticleSuppliers = SelectedComment.IdLogEntryByArticleSuppliers,
                        Datetime=DateTime.Now
                    });
                    
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByContact == SelectedComment.IdLogEntryByContact).Comments = editCommentsViewModel.NewItemComment;
                    CommentsList.FirstOrDefault(s => s.IdLogEntryByContact == SelectedComment.IdLogEntryByContact).Datetime = DateTime.Now;
                    try
                    {
                        //Shubham[skadam] GEOS2-5206 When we update comment updated date not show in grid. 22 01 2024.
                        CommentsList = new ObservableCollection<LogEntriesByArticleSuppliers>(CommentsList.OrderByDescending(x => x.Datetime));
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
        }

        public void SetUserProfileImage(LogEntriesByArticleSuppliers comment)
        {
            User user = new User();
            try
            {
                GeosApplication.Instance.Logger.Log("Method SetUserProfileImage ...", category: Category.Info, priority: Priority.Low);
                user = WorkbenchStartUp.GetUserById(Convert.ToInt32(comment.IdUser));
                // user = WorkbenchStartUp.GetUserById(GeosApplication.Instance.ActiveUser.IdUser);
               var UserProfileImageByte = GeosRepositoryServiceController.GetUserProfileImageWithoutException(GeosApplication.Instance.ActiveUser.Login);

                if (UserProfileImageByte != null)
                    comment.People.OwnerImage = ByteArrayToBitmapImage(UserProfileImageByte);
                else
                {
                    if (ApplicationThemeHelper.ApplicationThemeName == "BlackAndBlue")
                    {
                        if (user.IdUserGender == 1)
                            comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_White.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueFemale.png");
                        else if (user.IdUserGender == 2)
                            comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_White.png");
                        else if (user.IdUserGender == null)
                            comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/wUnknownGender.png");
                        //UserProfileImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/skyBlueMale.png");

                    }
                    else
                    {
                        if (user.IdUserGender == 1)
                            comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/FemaleUser_Blue.png");
                        else if (user.IdUserGender == 2)
                            comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/MaleUser_Blue.png");
                        else if (user.IdUserGender == null)
                            comment.People.OwnerImage = GetImage("/Emdep.Geos.Modules.Crm;component/Assets/Images/blueUnknownGender.png");
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

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new MemoryStream(byteArrayIn))
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
                return image;

                //GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }
        #endregion // Methods

        //#region ChangeLog
        ////shubham[skadam] for GEOS2-3432  [15-Mar-2022]
        //public void AddChangedTrainingLogDetails()
        //{
        //    try
        //    {
        //        GeosApplication.Instance.Logger.Log("Method AddChangedTrainingLogDetails()...", category: Category.Info, priority: Priority.Low);
        //        Contacts Update_ContactsDetails = SelectedContacts;
        //        Contacts New_ContactsDetails = SelectedContacts;
        //        //ArticleSupplier Update_ArticleSupplierDetalis = SelectedContacts;
        //        //Update Training
        //        if (IsNew == false)
        //        {
        //            //Firstname
        //            if (Update_ContactsDetails.Firstname != null && contactDetalis.Firstname != Update_ContactsDetails.Firstname)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogFirstname").ToString(), contactDetalis.Firstname, Update_ContactsDetails.Firstname.Trim())
        //                });
        //            }

        //            //Lastname
        //            if (Update_ContactsDetails.Lastname != null && contactDetalis.Lastname != Update_ContactsDetails.Lastname)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogLastname").ToString(), contactDetalis.Lastname, Update_ContactsDetails.Lastname.Trim())
        //                });
        //            }
        //            //IdDepartment  ListDepartment.FirstOrDefault(i => i.IdLookupValue == SelectedContacts.IdDepartment)
        //            if (Update_ContactsDetails.IdDepartment != null && contactDetalis.IdDepartment != Update_ContactsDetails.IdDepartment)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogDepartment").ToString(),
        //                    ListDepartment.Where(w => w.IdLookupValue == contactDetalis.IdDepartment).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault(),
        //                    ListDepartment.Where(w => w.IdLookupValue == Update_ContactsDetails.IdDepartment).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault())
        //                });
        //            }

        //            //JobTitle
        //            if (Update_ContactsDetails.JobTitle != null && contactDetalis.JobTitle != Update_ContactsDetails.JobTitle)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogJobTitle").ToString(), contactDetalis.JobTitle, Update_ContactsDetails.JobTitle.Trim())
        //                });
        //            }

        //            //Phone - //[pramod.misal][GEOS2-4673][22-08-2023]
        //            if (Update_ContactsDetails.Phone != null && contactDetalis.Phone != Update_ContactsDetails.Phone)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogPhone1").ToString(), string.IsNullOrEmpty(contactDetalis.Phone) ? "None" : contactDetalis.Phone, Update_ContactsDetails.Phone.Trim())
        //                });
        //            }
        //            else if (contactDetalis.Phone != null && contactDetalis.Phone != Update_ContactsDetails.Phone)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogPhone1").ToString(), contactDetalis.Phone, "None")
        //                });
        //            }

        //            //-------
        //            //[pramod.misal][GEOS2-4674][22-08-2023]
        //            //Phone2
        //            if (Update_ContactsDetails.Phone2 != null && contactDetalis.Phone2 != Update_ContactsDetails.Phone2)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogPhone2").ToString(), string.IsNullOrEmpty(contactDetalis.Phone2) ? "None" : contactDetalis.Phone2, Update_ContactsDetails.Phone2.Trim())
        //                });
        //            }
        //            else if (contactDetalis.Phone2 != null && contactDetalis.Phone2 != Update_ContactsDetails.Phone2)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogPhone2").ToString(), contactDetalis.Phone2, "None")
        //                });
        //            }

        //            //Email
        //            if (Update_ContactsDetails.Email != null && contactDetalis.Email != Update_ContactsDetails.Email)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogEmail").ToString(), contactDetalis.Email, Update_ContactsDetails.Email.Trim())
        //                });
        //            }

        //            //IdGender
        //            if (Update_ContactsDetails.IdGender != null && contactDetalis.IdGender != Update_ContactsDetails.IdGender)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogGender").ToString(),
        //                    UserGenderList.Where(w => w.IdLookupValue == contactDetalis.IdGender).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault(),
        //                    UserGenderList.Where(w => w.IdLookupValue == Update_ContactsDetails.IdGender).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault())
        //                });
        //            }


        //            //Remarks
        //            if (Update_ContactsDetails.Remarks != null && contactDetalis.Remarks != Update_ContactsDetails.Remarks)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogRemarks").ToString(), string.IsNullOrEmpty(contactDetalis.Remarks) ? "None" : contactDetalis.Remarks, Update_ContactsDetails.Remarks.Trim())
        //                });
        //            }
        //            else if (contactDetalis.Remarks != null && contactDetalis.Remarks != Update_ContactsDetails.Remarks)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogRemarks").ToString(), contactDetalis.Remarks, "None")
        //                });
        //            }

        //            //Contact Image Change Log
        //            if (string.IsNullOrEmpty(contactDetalis.ImageText) && Update_ContactsDetails.ImageText != null)
        //            {
        //                if (contactDetalis.ImageText != Update_ContactsDetails.ImageText)
        //                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers() { IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageAddedChangeLog").ToString()) });
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(contactDetalis.ImageText) && string.IsNullOrEmpty(Update_ContactsDetails.ImageText))
        //                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers() { IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageRemovedChangeLog").ToString()) });
        //                else if (!string.IsNullOrEmpty(contactDetalis.ImageText) && !string.IsNullOrEmpty(Update_ContactsDetails.ImageText) && contactDetalis.ImageText != Update_ContactsDetails.ImageText)
        //                    ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers() { IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageChangedChangeLog").ToString()) });
        //            }
        //        }
        //        else
        //        {
        //            #region Add
        //            //Firstname
        //            if (Update_ContactsDetails.Firstname != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogFirstnameAdd").ToString(), Update_ContactsDetails.Firstname.Trim())
        //                });
        //            }

        //            //Lastname
        //            if (Update_ContactsDetails.Lastname != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogLastnameAdd").ToString(), Update_ContactsDetails.Lastname.Trim())
        //                });
        //            }
        //            //IdDepartment  ListDepartment.FirstOrDefault(i => i.IdLookupValue == SelectedContacts.IdDepartment)
        //            if (Update_ContactsDetails.IdDepartment != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogDepartmentAdd").ToString(),
        //                    ListDepartment.Where(w => w.IdLookupValue == Update_ContactsDetails.IdDepartment).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault())
        //                });
        //            }

        //            //JobTitle
        //            if (Update_ContactsDetails.JobTitle != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogJobTitleAdd").ToString(), Update_ContactsDetails.JobTitle.Trim())
        //                });
        //            }

        //            //Phone1
        //            if (Update_ContactsDetails.Phone != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogPhoneAdd1").ToString(), Update_ContactsDetails.Phone.Trim())
        //                });
        //            }

        //            //[pramod.misal][GEOS2-4673][22-08-2023]
        //            //Phone2
        //            if (Update_ContactsDetails.Phone2 != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogPhoneAdd2").ToString(), Update_ContactsDetails.Phone.Trim())
        //                });
        //            }

        //            //Email
        //            if (Update_ContactsDetails.Email != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogEmailAdd").ToString(), Update_ContactsDetails.Email.Trim())
        //                });
        //            }

        //            //IdGender
        //            if (Update_ContactsDetails.IdGender != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogGenderAdd").ToString(),
        //                    UserGenderList.Where(w => w.IdLookupValue == Update_ContactsDetails.IdGender).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault())
        //                });
        //            }

        //            //Remarks
        //            if (Update_ContactsDetails.Remarks != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogRemarksAdd").ToString(), Update_ContactsDetails.Remarks.Trim())
        //                });
        //            }

        //            //ImageText
        //            if (Update_ContactsDetails.ImageText != null)
        //            {
        //                ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
        //                {
        //                    IdArticleSupplier = ArticleSupplierDetalis.IdArticleSupplier,
        //                    IdUser = GeosApplication.Instance.ActiveUser.IdUser,
        //                    Datetime = GeosApplication.Instance.ServerDateTime,
        //                    Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageAddedChangeLog").ToString())
        //                });
        //            }
        //            #endregion
        //        }


        //        GeosApplication.Instance.Logger.Log("Method AddChangedTrainingLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
        //    }
        //    catch (Exception ex)
        //    {
        //        GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedTrainingLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
        //    }
        //}
        //#endregion ChangeLog

        #region ChangeLog
        //[chitra.girigosavi][GEOS2-4692][13/10/2023]
        public void AddContactsChangedLogDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddChangedTrainingLogDetails()...", category: Category.Info, priority: Priority.Low);
                Contacts Update_ContactsDetails = SelectedContacts;
                Contacts New_ContactsDetails = SelectedContacts;
                //Update Contacts
                if (IsNew == false)
                {
                    //Firstname
                    if (Update_ContactsDetails.Firstname != null && contactDetalis.Firstname != Update_ContactsDetails.Firstname)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,                       
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogFirstname").ToString(), contactDetalis.Firstname, Update_ContactsDetails.Firstname.Trim()),
                            IdLogEntryType = 258
                        });
                    }
                    //Lastname
                    if (Update_ContactsDetails.Lastname != null && contactDetalis.Lastname != Update_ContactsDetails.Lastname)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogLastname").ToString(), contactDetalis.Lastname, Update_ContactsDetails.Lastname.Trim()),
                            IdLogEntryType = 258
                        });
                    }
                    //IdDepartment  ListDepartment.FirstOrDefault(i => i.IdLookupValue == SelectedContacts.IdDepartment)
                    if (Update_ContactsDetails.IdDepartment != null && contactDetalis.IdDepartment != Update_ContactsDetails.IdDepartment)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogDepartment").ToString(),
                            ListDepartment.Where(w => w.IdLookupValue == contactDetalis.IdDepartment).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault(),
                            ListDepartment.Where(w => w.IdLookupValue == Update_ContactsDetails.IdDepartment).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault()),
                            IdLogEntryType = 258
                        });
                    }
                    //JobTitle
                    if (Update_ContactsDetails.JobTitle != null && contactDetalis.JobTitle != Update_ContactsDetails.JobTitle)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogJobTitle").ToString(), contactDetalis.JobTitle, Update_ContactsDetails.JobTitle.Trim()),
                            IdLogEntryType = 258,
                        });
                    }
                    //Phone
                    if (Update_ContactsDetails.Phone != null && contactDetalis.Phone != Update_ContactsDetails.Phone)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogPhone1").ToString(), string.IsNullOrEmpty(contactDetalis.Phone) ? "None" : contactDetalis.Phone, Update_ContactsDetails.Phone.Trim()),
                            IdLogEntryType = 258
                        });
                    }
                    else if (contactDetalis.Phone != null && contactDetalis.Phone != Update_ContactsDetails.Phone)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogPhone1").ToString(), contactDetalis.Phone, "None"),
                            IdLogEntryType = 258
                        });
                    }
                    //Phone2
                    if (Update_ContactsDetails.Phone2 != null && contactDetalis.Phone2 != Update_ContactsDetails.Phone2)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogPhone2").ToString(), string.IsNullOrEmpty(contactDetalis.Phone2) ? "None" : contactDetalis.Phone2, Update_ContactsDetails.Phone2.Trim()),
                            IdLogEntryType = 258
                        });
                    }
                    else if (contactDetalis.Phone2 != null && contactDetalis.Phone2 != Update_ContactsDetails.Phone2)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogPhone2").ToString(), contactDetalis.Phone2, "None"),
                            IdLogEntryType = 258
                        });
                    }

                    //Email
                    if (Update_ContactsDetails.Email != null && contactDetalis.Email != Update_ContactsDetails.Email)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogEmail").ToString(), contactDetalis.Email, Update_ContactsDetails.Email.Trim()),
                            IdLogEntryType = 258
                        });
                    }

                    //IdGender
                    if (Update_ContactsDetails.IdGender != null && contactDetalis.IdGender != Update_ContactsDetails.IdGender)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogGender").ToString(),
                            UserGenderList.Where(w => w.IdLookupValue == contactDetalis.IdGender).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault(),
                            UserGenderList.Where(w => w.IdLookupValue == Update_ContactsDetails.IdGender).Select(s => s.Value).DefaultIfEmpty(null).FirstOrDefault()),
                            IdLogEntryType = 258
                        });
                    }


                    //Remarks
                    if (Update_ContactsDetails.Remarks != null && contactDetalis.Remarks != Update_ContactsDetails.Remarks)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            IdLogEntryType = 258,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogRemarks").ToString(), string.IsNullOrEmpty(contactDetalis.Remarks) ? "None" : contactDetalis.Remarks, Update_ContactsDetails.Remarks.Trim())
                        });
                    }
                    else if (contactDetalis.Remarks != null && contactDetalis.Remarks != Update_ContactsDetails.Remarks)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdLogEntryType = 258,
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ContactsChangeLogRemarks").ToString(), contactDetalis.Remarks, "None")
                        });
                    }

                    //Contact Image Change Log
                    if (string.IsNullOrEmpty(contactDetalis.ImageText) && Update_ContactsDetails.ImageText != null)
                    {
                        if (contactDetalis.ImageText != Update_ContactsDetails.ImageText)
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers() { IdLogEntryType = 258, IdContact = SelectedContacts.IdContact, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageAddedChangeLog").ToString()) });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(contactDetalis.ImageText) && string.IsNullOrEmpty(Update_ContactsDetails.ImageText))
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers() { IdLogEntryType = 258, IdContact = SelectedContacts.IdContact, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageRemovedChangeLog").ToString()) });
                        else if (!string.IsNullOrEmpty(contactDetalis.ImageText) && !string.IsNullOrEmpty(Update_ContactsDetails.ImageText) && contactDetalis.ImageText != Update_ContactsDetails.ImageText)
                            ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers() { IdLogEntryType = 258, IdContact = SelectedContacts.IdContact, IdUser = GeosApplication.Instance.ActiveUser.IdUser, Datetime = GeosApplication.Instance.ServerDateTime, Comments = string.Format(System.Windows.Application.Current.FindResource("ContactImageChangedChangeLog").ToString()) });
                    }
                }
                else
                {
                    #region Add
                    if (Update_ContactsDetails != null)
                    {
                        ArticleSuppliersChangeLogList.Add(new LogEntriesByArticleSuppliers()
                        {
                            IdLogEntryType = 258,
                            IdContact = SelectedContacts.IdContact,
                            Datetime = GeosApplication.Instance.ServerDateTime,
                            IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                            Comments = string.Format(System.Windows.Application.Current.FindResource("ArticleSuppliersChangeLogContactAdded").ToString(), Update_ContactsDetails.FullName.Trim())
                        });
                    }
                    #endregion
                }


                GeosApplication.Instance.Logger.Log("Method AddChangedTrainingLogDetails()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an Error in Method AddChangedTrainingLogDetails()........" + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion ChangeLog

        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        // Handle Add Tab button click
        private void AddTab_Click(object obj)
        {
            try
            {
                Article_Supplier_Contacts supplierRecord = new Article_Supplier_Contacts();
                supplierRecord.TransactionOperation = ModelBase.TransactionOperations.Add;
                supplierRecord.Name = "New Supplier";
                supplierRecord.ShortName = FormatTabName(supplierRecord.Name);
                supplierRecord.EMDEPCode = string.Empty;
                supplierRecord.Address = string.Empty;
                supplierRecord.City = string.Empty;
                supplierRecord.Region = string.Empty;
                supplierRecord.Country = string.Empty;
                supplierRecord.PostCode = string.Empty;
                supplierRecord.SupplierList = new List<ArticleSupplier>();
                supplierRecord.SupplierList.AddRange(SupplierList.ToList());
                TabItems.Add(supplierRecord);
                AddArticleSupplierContactsList.Add(supplierRecord);
                SelectedTabItem = supplierRecord;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddTab_Click() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }


            //Article_Supplier_Contacts supplierRecord = new Article_Supplier_Contacts();
            //supplierRecord.Name = SelectedSupplierList.Name;
            //supplierRecord.SupplierList = new List<ArticleSupplier>();
            //supplierRecord.SupplierList.AddRange(SupplierList.ToList());
            //TabItems.Add(supplierRecord);
            // Assuming ViewModel is properly set as DataContext
            //var viewModel = DevExpress.Data.Browsing.DataContext as YourViewModel;

            //// Create a new record or model to bind to a new TabItem
            //var newTab = new ArticleSupplier
            //{
            //    SName = "New Supplier",  // Set default or custom name
            //    SupplierList = new List<Supplier>()  // Initialize list or other properties
            //};

            //// Add the new TabItem to the collection in the ViewModel
            //viewModel.TabItems.Add(newTab);
        }
        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        public void AddTab()
        {
            try
            {
                Article_Supplier_Contacts supplierRecord = new Article_Supplier_Contacts();
                supplierRecord.TransactionOperation = ModelBase.TransactionOperations.Add;
                supplierRecord.Name = "New Supplier";
                supplierRecord.EMDEPCode = string.Empty;
                supplierRecord.Address = string.Empty;
                supplierRecord.City = string.Empty;
                supplierRecord.Region = string.Empty;
                supplierRecord.Country = string.Empty;
                supplierRecord.PostCode = string.Empty;
                supplierRecord.SupplierList = new List<ArticleSupplier>();
                supplierRecord.SupplierList.AddRange(SupplierList.ToList());
                TabItems.Add(supplierRecord);
                AddArticleSupplierContactsList.Add(supplierRecord);
                SelectedTabItem = TabItems.FirstOrDefault();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddTab() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        public string FormatTabName(string name)
        {
            if (name.Length > 15)
            {
                // Truncate the name to 15 characters
                return name.Substring(0, 15);
            }
            return name;
        }
        //Shubham[skadam] GEOS2-5903 REQUEST IMPROVEMENTS 21 09 2024
        private void DeleteCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(String.Format(Application.Current.Resources["SRMContactSupplier_delete"].ToString(), SelectedTabItem.Name, (FirstName + " " + LastName).Trim()), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (SelectedTabItem.TransactionOperation != ModelBase.TransactionOperations.Add)
                    {
                        DeleteArticleSupplierContactsList.Add(SelectedTabItem);
                    }
                    TabItems.Remove(SelectedTabItem);
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SRMContactSupplier_DeletedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    SelectedTabItem = TabItems.FirstOrDefault();
                    if (TabItems.Count == 0)
                    {
                        AddTab();
                    }
                }
                GeosApplication.Instance.Logger.Log("Method DeleteCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in DeleteCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method DeleteCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[sudhir.jangra][GEOS2-4676]
        public void FillSupplierList()
        {
            try
            {
                if (SRMCommon.Instance.SelectedAuthorizedWarehouseList != null)
                {
                    SupplierList = new ObservableCollection<ArticleSupplier>();
                    List<Warehouses> plantOwners = SRMCommon.Instance.SelectedAuthorizedWarehouseList.Cast<Warehouses>().ToList();
                    foreach (var item in plantOwners)
                    {
                        var temp = new ObservableCollection<ArticleSupplier>(SRMService.GetArticleSuppliersForSRMContact_V2430(item));
                        SupplierList.AddRange(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillSupplierList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }
}

