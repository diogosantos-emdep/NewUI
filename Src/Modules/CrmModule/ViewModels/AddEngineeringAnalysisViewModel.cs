using DevExpress.Compression;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Crm;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Modules.Crm.Views;
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
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddEngineeringAnalysisViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged
    {
        #region Services

        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private bool isBusy;
        private bool isAssignee;
        private bool isNewRecord;
        private string fileName;
        private string uniqueFileName;
        private string fileNameString;
        private List<Attachment> engineeringAnalysisAttachmentList;
        private List<object> tempAttachmentList;
        private Attachment attachment;
        private string comments;
        private DateTime dueDate;
        private int selectedIndexAttachment = -1;
        FileUploader engAnalysisAttachmentFileUploaderIndicator;
        private List<EngineeringAnalysis> engAnalysis;
        private List<EngineeringAnalysis> engAnalysisDuplicate;

        private bool isEngAnalysisChooseFileEnable = true;
        private bool isAcceptEnable = true;

        private bool isCompleted = true;
        private bool isCompletedReadOnly = true;

        private bool isCommentsReadOnly = false;
        private bool isAttachmentReadOnly = false;
        private bool isDateReadOnly = false;

        List<EngineeringAnalysisType> engineeringAnalysisTypes;
        List<object> selectedEngineeringAnalysisTypes;
        private bool isEngineeringAnalysisTypesReadOnly = false;
        private string visible;
        private string offerCode;
        private Int16 revNumber;
        private List<EngineeringAnalysisRevision> revisionList;
        private EngineeringAnalysisRevision selectedRevision;
        private bool isAddedTab;
        private bool isNew;

        private EngineeringAnalysis engAnalysisForEdit;
        private string revError;
        private bool isAssigneeReadOnly = false;
        private ObservableCollection<User> assigneeList;
        private User selectedIndexAssignee;
        #endregion

        public ObservableCollection<User> AssigneeList
        {
            get { return assigneeList; }
            set
            {
                assigneeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AssigneeList"));
            }
        }
        public bool IsAssignee
        {
            get { return isAssignee; }
            set
            {
                isAssignee = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAssignee"));
            }
        }
        public bool IsNewRecord
        {
            get { return isNewRecord; }
            set
            {
                isNewRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNewRecord"));
            }
        }
        public bool IsAssigneeReadOnly
        {
            get { return isAssigneeReadOnly; }
            set
            {
                isAssigneeReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAssigneeReadOnly"));
            }
        }

        #region Properties
        public Int32 RevNumber { get; set; }
        public bool IsSave { get; set; }
        public bool IsEdit { get; set; }

        public string GuidCode { get; set; }

        public FileUploader EngAnalysisAttachmentFileUploaderIndicator
        {
            get { return engAnalysisAttachmentFileUploaderIndicator; }
            set
            {
                engAnalysisAttachmentFileUploaderIndicator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysisAttachmentFileUploaderIndicator"));
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

        public string Comments
        {
            get { return comments; }
            set
            {
                if (value != null)
                {
                    comments = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
                }
                else
                {
                    comments = value;
                    string error = EnableValidationAndGetError();
                    OnPropertyChanged(new PropertyChangedEventArgs("Comments"));
                }
            }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DueDate"));
            }
        }

        public int SelectedIndexAttachment
        {
            get { return selectedIndexAttachment; }
            set
            {
                selectedIndexAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAttachment"));
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set
            {
                uniqueFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName"));
            }
        }

        public string FileNameString
        {
            get { return fileNameString; }
            set
            {
                fileNameString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileNameString"));
            }
        }

        public Attachment Attachment
        {
            get { return attachment; }
            set
            {
                attachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attachment"));
            }
        }

        public List<Attachment> EngineeringAnalysisAddedAttachmentList
        {
            get { return engineeringAnalysisAttachmentList; }
            set
            {
                engineeringAnalysisAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngineeringAnalysisAddedAttachmentList"));
            }
        }

        public List<object> TempAttachmentListUI
        {
            get { return tempAttachmentList; }
            set
            {
                tempAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempAttachmentListUI"));
            }
        }

        public List<EngineeringAnalysis> EngAnalysis
        {
            get { return engAnalysis; }
            set
            {
                engAnalysis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysis"));
            }
        }

        public List<EngineeringAnalysis> EngAnalysisDuplicate
        {
            get { return engAnalysisDuplicate; }
            set
            {
                engAnalysisDuplicate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysisDuplicate"));
            }
        }

        public bool IsEngAnalysisChooseFileEnable
        {
            get { return isEngAnalysisChooseFileEnable; }
            set
            {
                isEngAnalysisChooseFileEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngAnalysisChooseFileEnable"));
            }
        }

        public bool IsAcceptEnable
        {
            get { return isAcceptEnable; }
            set
            {
                isAcceptEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable"));
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompleted"));
            }
        }

        public bool IsCompletedReadOnly
        {
            get { return isCompletedReadOnly; }
            set
            {
                isCompletedReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCompletedReadOnly"));
            }
        }

        public bool IsCommentsReadOnly
        {
            get { return isCommentsReadOnly; }
            set
            {
                isCommentsReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCommentsReadOnly"));
            }
        }

        public bool IsAttachmentReadOnly
        {
            get { return isAttachmentReadOnly; }
            set
            {
                isAttachmentReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAttachmentReadOnly"));
            }
        }

        public bool IsDateReadOnly
        {
            get { return isDateReadOnly; }
            set
            {
                isDateReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDateReadOnly"));
            }
        }

        public List<EngineeringAnalysisType> EngineeringAnalysisTypes
        {
            get { return engineeringAnalysisTypes; }
            set
            {
                engineeringAnalysisTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngineeringAnalysisTypes"));
            }
        }

        public List<object> SelectedEngineeringAnalysisTypes
        {
            get { return selectedEngineeringAnalysisTypes; }
            set
            {
                selectedEngineeringAnalysisTypes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEngineeringAnalysisTypes"));
            }
        }

        public bool IsEngineeringAnalysisTypesReadOnly
        {
            get { return isEngineeringAnalysisTypesReadOnly; }
            set
            {
                isEngineeringAnalysisTypesReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEngineeringAnalysisTypesReadOnly"));
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

        public string OfferCode
        {
            get
            {
                return offerCode;
            }

            set
            {
               offerCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferCode"));
            }
        }
        public List<EngineeringAnalysisRevision> RevisionList
        {
            get
            {
                return revisionList;
            }

            set
            {
                revisionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RevisionList"));
            }
        }

        public EngineeringAnalysisRevision SelectedRevision
        {
            get
            {
                return selectedRevision;
            }

            set
            {
                selectedRevision = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedRevision"));
            }
        }

        public bool IsAddedTab
        {
            get
            {
                return isAddedTab;
            }

            set
            {
                isAddedTab = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddedTab"));
            }
        }

        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }

        public EngineeringAnalysis EngAnalysisForEdit
        {
            get
            {
                return engAnalysisForEdit;
            }

            set
            {
                engAnalysisForEdit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EngAnalysisForEdit"));
            }
        }

        public string RevError
        {
            get
            {
                return revError;
            }

            set
            {
                revError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RevError"));
            }
        }

        //public User SelectedIndexAssignee
        //{
        //    get { return selectedIndexAssignee; }
        //    set
        //    {
        //        selectedIndexAssignee = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAssignee"));
        //    }
        //}
        private List<object> selectedIndexAssigneelist;
        public List<object> SelectedIndexAssigneeList
        {
            get { return selectedIndexAssigneelist; }
            set
            {
                selectedIndexAssigneelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexAssigneeList"));
            }
        }
        #endregion

        #region Commands

        public ICommand AddEngineeringAnalysisCancelButtonCommand { get; set; }
        public ICommand AddEngineeringAnalysisAcceptButtonCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        public ICommand TabAddingCommand { get; set; }
        public ICommand TabPreviousCommand { get; set; }
        

        public ICommand SelectedItemChangedCommand { get; set; }
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
        
        public AddEngineeringAnalysisViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEngineeringAnalysisViewModel ...", category: Category.Info, priority: Priority.Low);
                IsAssignee = true;
                IsCompleted = false;
                IsCompletedReadOnly = !GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27);
                IsAssigneeReadOnly = GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27);
                FillAssigneeList();
                DueDate = DateTime.Now;
                RevNumber = 0;
                GetEngAnalysisArticles();
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    IsEngineeringAnalysisTypesReadOnly = true;
                    IsCommentsReadOnly = true;
                    IsAttachmentReadOnly = true;
                    IsDateReadOnly = true;
                }
                else
                {
                    IsEngineeringAnalysisTypesReadOnly = false;
                    IsCommentsReadOnly = false;
                    IsAttachmentReadOnly = false;
                    IsDateReadOnly = false;
                }
                AddEngineeringAnalysisCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddEngineeringAnalysisAcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFileCommandAction));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                TabAddingCommand= new RelayCommand(new Action<object>(TabAddingCommandAction));
                TabPreviousCommand = new RelayCommand(new Action<object>(TabPreviousCommandAction));
                SelectedItemChangedCommand = new RelayCommand(new Action<object>(SelectedItemChangedCommandAction));
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

               
                GeosApplication.Instance.Logger.Log("Constructor AddEngineeringAnalysisViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddEngineeringAnalysisViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion

        #region Method

        private void TabAddingCommandAction(object obj)
        {
            if (IsAddedTab == true || IsNew == true)
            {
                //if msg required
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("RevisionAllowOnlyOne").ToString(), RevisionList.Count - 1), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                RevisionList = new List<EngineeringAnalysisRevision>(RevisionList);
                SelectedRevision = RevisionList.LastOrDefault();
                ((DevExpress.Xpf.Core.TabControlTabAddingEventArgs)obj).Item = SelectedRevision;

                return;
            }
            SelectedEngineeringAnalysisTypes = new List<object>();
            IsAssignee = false;
            IsAddedTab = true;
            foreach (EngineeringAnalysisType item in EngAnalysisDuplicate.LastOrDefault().EngineeringAnalysisTypes)
            {
                if(!item.IsChecked)
                {
                    item.IsArticleEnabled = false;
                }
            }
            EngineeringAnalysisTypes = new List<EngineeringAnalysisType>();
            SelectedEngineeringAnalysisTypes = new List<object>();
            foreach (EngineeringAnalysisType item in RevisionList.LastOrDefault().EngineeringAnalysisTypes)
            {
                if (!item.IsChecked && item.IsArticleEnabled == false)
                {
                    EngineeringAnalysisType engineeringAnalysisType = new EngineeringAnalysisType();
                    engineeringAnalysisType.IdArticle = item.IdArticle;
                    engineeringAnalysisType.Reference = item.Reference;
                    engineeringAnalysisType.IsArticleEnabled = false;
                    engineeringAnalysisType.IsChecked = false;
                    engineeringAnalysisType.TransactionOperation = ModelBase.TransactionOperations.Add;
                    EngineeringAnalysisTypes.Add(engineeringAnalysisType);
                    SelectedEngineeringAnalysisTypes.Add(engineeringAnalysisType);
                }
                else if (!item.IsChecked || item.IsSelected == true)
                {
                    item.IsArticleEnabled = false;
                    EngineeringAnalysisType engineeringAnalysisType = new EngineeringAnalysisType();
                    engineeringAnalysisType.IdArticle = item.IdArticle;
                    engineeringAnalysisType.Reference = item.Reference;
                    engineeringAnalysisType.IsArticleEnabled = true;
                    engineeringAnalysisType.IsChecked = false;
                    engineeringAnalysisType.IsSelected = false;
                    engineeringAnalysisType.TransactionOperation = ModelBase.TransactionOperations.Add;
                    EngineeringAnalysisTypes.Add(engineeringAnalysisType);
                    SelectedEngineeringAnalysisTypes.Add(engineeringAnalysisType);
                }
                else
                {
                    EngineeringAnalysisType engineeringAnalysisType = new EngineeringAnalysisType();
                    engineeringAnalysisType.IdArticle = item.IdArticle;
                    engineeringAnalysisType.Reference = item.Reference;
                    engineeringAnalysisType.IsArticleEnabled = false;
                    engineeringAnalysisType.IsChecked = false;
                    engineeringAnalysisType.TransactionOperation = ModelBase.TransactionOperations.Add;
                    EngineeringAnalysisTypes.Add(engineeringAnalysisType);
                    SelectedEngineeringAnalysisTypes.Add(engineeringAnalysisType);
                }
              
               
            }


            RevisionList.Add(new EngineeringAnalysisRevision()
            {
                RevNumber = RevisionList.Count,
                Header = "Rev." + RevisionList.Count,
                Comments = "",
                DueDate = DateTime.Now,
                EngineeringAnalysisTypes = EngineeringAnalysisTypes,
                IsCommentsReadOnly = IsCommentsReadOnly,
                IsDateReadOnly = IsDateReadOnly,
                IsEngAnalysisChooseFileEnable = IsEngAnalysisChooseFileEnable,
                IsEngineeringAnalysisTypesReadOnly = IsEngineeringAnalysisTypesReadOnly,
                SelectedEngineeringAnalysisTypes = SelectedEngineeringAnalysisTypes,
                SelectedIndexAttachment = 0,
                TempAttachmentList = TempAttachmentListUI,
                TransactionOperation= ModelBase.TransactionOperations.Add,
                IsNewRevision = true
            });

            //foreach (EngineeringAnalysisRevision item in RevisionList.Where(rl=>rl.Header!= "Rev." + (RevisionList.Count()-1)))
            //{
            //    foreach (EngineeringAnalysisType itemSelected in item.EngineeringAnalysisTypes.Where(i=>i.IsChecked=false))
            //    {
            //        itemSelected.IsArticleEnabled = false;
            //    }
           
            //}
            IsAddedTab = true;
            RevisionList = new List<EngineeringAnalysisRevision>(RevisionList);
            SelectedRevision = RevisionList.LastOrDefault();
            ((DevExpress.Xpf.Core.TabControlTabAddingEventArgs)obj).Item = SelectedRevision;
        }
        private void TabPreviousCommandAction(object obj)
        {
            var selectedTab = (obj as DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs)?
                          .NewSelectedItem as EngineeringAnalysisRevision;

            if (selectedTab != null)
            {
                if (selectedTab.IsNewRevision)
                {
                    // 🔹 Any new (unaccepted) revision → hide Assignee
                    IsAssignee = false;
                }
                else
                {
                    // 🔹 Old/previous revisions → show Assignee
                    IsAssignee = true;
                }
            }
        }

        private void SelectedItemChangedCommandAction(object obj)
        {
            EngineeringAnalysisType row = (EngineeringAnalysisType)obj;
            if (row.IsChecked == true)
                row.IsSelected = false;
        }

        /// <summary>
        /// Method for Initialization.
        /// [001][cpatil][08-04-2020] GEOS2-2224 Partnumber not generated after EDIT Engineering Analysis
        /// [002][cpatil][15-05-2020] GEOS2-2279 Error when trying to modify offers with eng.analysis
        /// </summary>
        public void InIt(GeosStatus selectedGeosStatus)
        {
            //  [CRM-M040-02] (#49016) Validate Eng. Analysis
            if (selectedGeosStatus.IdOfferStatusType != 1 && selectedGeosStatus.IdOfferStatusType != 2)
            {
                IsEngineeringAnalysisTypesReadOnly = true;
                IsCommentsReadOnly = true;
                IsAttachmentReadOnly = true;
                IsEngAnalysisChooseFileEnable = false;
                IsDateReadOnly = true;
                IsAcceptEnable = false;
            }

            if (GeosApplication.Instance.IsPermissionReadOnly && GeosApplication.Instance.ActiveUser.UserPermissions.Any(x => x.IdPermission == 27))
            {
                //IsAcceptControlEnableorder = false;
                IsEngAnalysisChooseFileEnable = false;
                IsAcceptEnable = true;
            }
            else if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                //IsAcceptControlEnableorder = false;
                IsEngAnalysisChooseFileEnable = false;
                IsAcceptEnable = false;
            }
            ////[001] Added User not able to unselected Article in status shipped
            //if(!IsEngineeringAnalysisTypesReadOnly)
            //{
            //    if (IsCompleted)
            //        IsEngineeringAnalysisTypesReadOnly = true;
            //    else
            //        IsEngineeringAnalysisTypesReadOnly = false;
            //}

            FillEngineeringAnalysisDetails();

        }

        /// <summary>
        /// [001][2018-07-09][skhade][CRM-M042-18] Eng. analysis option in Orders.
        /// This init method is called always from order.
        /// </summary>
        /// <param name="selectedGeosStatus">Selected geos status of order.</param>
        public void InitOrder(GeosStatus selectedGeosStatus)
        {
            FillEngineeringAnalysisDetails();

            IsEngineeringAnalysisTypesReadOnly = true;
            IsCommentsReadOnly = true;
            IsAttachmentReadOnly = true;
            IsEngAnalysisChooseFileEnable = false;
            IsDateReadOnly = true;
            IsCompletedReadOnly = true;
            IsAcceptEnable = false;
        }

        /// <summary>
        /// Method For Fill Analysis Details back
        /// </summary>
        public void FillEngineeringAnalysisDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysisDetails() ...", category: Category.Info, priority: Priority.Low);

                if (EngAnalysisDuplicate != null && EngAnalysisDuplicate.Count > 0)
                {
                    Int16 rev = 0;
                    foreach (EngineeringAnalysis engAnalysisDuplicate in EngAnalysisDuplicate)
                    {

                        //Comments = engAnalysisDuplicate.Comments;
                        //DueDate = engAnalysisDuplicate.DueDate;
                        engAnalysisDuplicate.TempAttachmentListUI = new List<object>();

                        if (engAnalysisDuplicate.Attachments != null)
                        {
                            foreach (Attachment item in engAnalysisDuplicate.Attachments.Where(i => i.IsDeleted != true).ToList())
                            {
                                engAnalysisDuplicate.TempAttachmentListUI.Add((Attachment)item.Clone());
                            }

                            SelectedIndexAttachment = 0;
                        }

                        if (engAnalysisDuplicate.EngineeringAnalysisTypes != null && EngineeringAnalysisTypes != null)
                        {
                            if (SelectedEngineeringAnalysisTypes == null)
                                SelectedEngineeringAnalysisTypes = new List<object>();
                            if (engAnalysisDuplicate.ObjselectedEngineeringAnalysisTypes == null)
                                engAnalysisDuplicate.ObjselectedEngineeringAnalysisTypes = new List<object>();
                            engAnalysisDuplicate.ObjselectedEngineeringAnalysisTypes.AddRange(engAnalysisDuplicate.SelectedEngineeringAnalysisTypes);

                        }
                        else
                        {
                            if (SelectedEngineeringAnalysisTypes == null)
                                SelectedEngineeringAnalysisTypes = new List<object>();
                            if (engAnalysisDuplicate.ObjselectedEngineeringAnalysisTypes == null)
                                engAnalysisDuplicate.ObjselectedEngineeringAnalysisTypes = new List<object>();
                            engAnalysisDuplicate.EngineeringAnalysisTypes = new List<EngineeringAnalysisType>();
                            engAnalysisDuplicate.SelectedEngineeringAnalysisTypes = new List<EngineeringAnalysisType>();

                            engAnalysisDuplicate.EngineeringAnalysisTypes = EngineeringAnalysisTypes;
                        }


                        //IsCompleted = EngAnalysisDuplicate.IsCompleted;
                        //if (IsCompleted)
                        //    IsCompletedReadOnly = true;
                        IsEdit = true;

                        //retrive saved list
                        if (RevisionList == null)
                            RevisionList = new List<EngineeringAnalysisRevision>();

                        RevNumber = RevisionList.Count();

                        RevisionList.Add(new EngineeringAnalysisRevision()
                        {
                            Header = "Rev." + RevisionList.Count,
                            RevNumber = RevisionList.Count,
                            IdRevision = engAnalysisDuplicate.IdRevision,
                            IdsArticle = engAnalysisDuplicate.IdsArticle,
                            IdOT = engAnalysisDuplicate.IdOT,
                            TransactionOperation = ModelBase.TransactionOperations.Modify,
                            Comments = engAnalysisDuplicate.Comments,
                            DueDate = engAnalysisDuplicate.DueDate,
                            EngineeringAnalysisTypes = engAnalysisDuplicate.EngineeringAnalysisTypes.Select(x => (EngineeringAnalysisType)x.Clone()).ToList(),
                            IsCommentsReadOnly = IsCommentsReadOnly,
                            IsDateReadOnly = IsDateReadOnly,
                            IsEngAnalysisChooseFileEnable = IsEngAnalysisChooseFileEnable,
                            IsEngineeringAnalysisTypesReadOnly = IsEngineeringAnalysisTypesReadOnly,
                            SelectedEngineeringAnalysisTypes = engAnalysisDuplicate.ObjselectedEngineeringAnalysisTypes,
                            SelectedIndexAttachment = 0,
                            TempAttachmentList = engAnalysisDuplicate.TempAttachmentListUI
                        });

                        RevisionList = new List<EngineeringAnalysisRevision>(RevisionList);
                        SelectedRevision = RevisionList.LastOrDefault();
                    }
                }
                else
                {
                    if (RevisionList == null)
                        RevisionList = new List<EngineeringAnalysisRevision>();

                    RevisionList.Add(new EngineeringAnalysisRevision()
                    {
                        Header = "Rev." + RevisionList.Count,
                        Comments = Comments,
                        DueDate = DueDate,
                        EngineeringAnalysisTypes = EngineeringAnalysisTypes,
                        IsCommentsReadOnly = IsCommentsReadOnly,
                        IsDateReadOnly = IsDateReadOnly,
                        IsEngAnalysisChooseFileEnable = IsEngAnalysisChooseFileEnable,
                        IsEngineeringAnalysisTypesReadOnly = IsEngineeringAnalysisTypesReadOnly,
                        SelectedEngineeringAnalysisTypes = SelectedEngineeringAnalysisTypes,
                        SelectedIndexAttachment = 0,
                        TempAttachmentList = TempAttachmentListUI
                    });

                    RevisionList = new List<EngineeringAnalysisRevision>(RevisionList);
                    SelectedRevision = RevisionList.LastOrDefault();
                }

                if (SelectedRevision?.SelectedEngineeringAnalysisTypes != null)
                {
                    //foreach (EngineeringAnalysisType item in SelectedRevision.SelectedEngineeringAnalysisTypes)
                    //{
                    //    Int64 IdRevisionItem = item.IdRevisionItem;
                    //    CrmStartUp = new CrmServiceController("localhost:6699");
                    //    EngineeringAnalysisType a = CrmStartUp.CRM_GetAssignedToFromOtitems_V2680(IdRevisionItem);

                    //    if (a != null && a.AssignedToUser > 0 && AssigneeList != null && AssigneeList.Count > 0)
                    //    {
                            
                    //        if (AssigneeList == null)
                    //            AssigneeList = new ObservableCollection<User>();
                         
                    //        var matchedUser = AssigneeList.FirstOrDefault(u => u.IdUser == a.AssignedToUser);
                    //        if (matchedUser == null)
                    //        {
                    //            CrmStartUp = new CrmServiceController("localhost:6699");
                    //            User fetchedUser = CrmStartUp.GetAssigneeUserIdUserwise_V2680(a.AssignedToUser);
                    //            if (fetchedUser != null)
                    //            {
                    //                AssigneeList.Add(fetchedUser);
                    //                matchedUser = fetchedUser;
                    //            }
                    //        }
                    //        if (matchedUser != null)
                    //        {
                    //            // Find index within list
                    //            int index = AssigneeList.IndexOf(matchedUser);
                    //            item.SelectedAssignee = matchedUser;


                    //            // for index 
                    //            // Get index of matchedUser within AssigneeList
                    //            item.SelectedIndexAssignee = AssigneeList.IndexOf(matchedUser);
                    //        }
                    //    }
                    //    //else
                    //    //{
                    //    //    // Clear for non-engineering types
                    //    //    item.SelectedAssignee = null;
                    //    //    item.SelectedIndexAssignee = -1;
                    //    //}
                    //}
                    //foreach (EngineeringAnalysis engAnalysisDuplicate in EngAnalysisDuplicate)
                    //{
                    //    foreach (EngineeringAnalysisType item in engAnalysisDuplicate.EngineeringAnalysisTypes)
                    //    {
                    //        Int64 IdRevisionItem = item.IdRevisionItem;
                    //        //CrmStartUp = new CrmServiceController("localhost:6699");
                    //        EngineeringAnalysisType a = CrmStartUp.CRM_GetAssignedToFromOtitems_V2680(IdRevisionItem);
                    //        if (a != null && a.AssignedToUser > 0)
                    //        {


                    //            if (AssigneeList == null)
                    //                AssigneeList = new ObservableCollection<User>();

                    //            item.AssigneeList = AssigneeList;

                    //            var matchedUser = AssigneeList.FirstOrDefault(u => u.IdUser == a.AssignedToUser);
                    //            if (matchedUser == null)
                    //            {
                    //               // CrmStartUp = new CrmServiceController("localhost:6699");
                    //                User fetchedUser = CrmStartUp.GetAssigneeUserIdUserwise_V2680(a.AssignedToUser);

                    //                if (fetchedUser != null)
                    //                {
                    //                    AssigneeList.Add(fetchedUser);
                    //                    matchedUser = fetchedUser;
                    //                }
                    //            }
                    //            if (matchedUser != null)
                    //            {
                    //                int index = AssigneeList.IndexOf(matchedUser);
                    //                var ListAss= AssigneeList.Where(u => u.IdUser == matchedUser.IdUser).ToList();
                    //                //AssigneeList.FirstOrDefault(u => u.IdUser == matchedUser.IdUser).SelectedIndexAssignee= index;
                    //                item.SelectedAssignee = matchedUser;
                    //                item.SelectedIndexAssignee = index;
                    //                SelectedIndexAssigneeList = new List<object>(ListAss.ToList());
                    //            }
                    //        }
                    //    }
                    //}
                    foreach (EngineeringAnalysisRevision engAnalysisDuplicate in RevisionList)
                    {
                        foreach (EngineeringAnalysisType item in engAnalysisDuplicate.EngineeringAnalysisTypes)
                        {
                            Int64 IdRevisionItem = item.IdRevisionItem;
                            //CrmStartUp = new CrmServiceController("localhost:6699");
                            EngineeringAnalysisType a = CrmStartUp.CRM_GetAssignedToFromOtitems_V2680(IdRevisionItem);

                            if (a != null && a.AssignedToUser > 0)
                            {
                                if (AssigneeList == null)
                                    AssigneeList = new ObservableCollection<User>();

                                item.AssigneeList = AssigneeList;
                                var matchedUser = AssigneeList.FirstOrDefault(u => u.IdUser == a.AssignedToUser);
                                if (matchedUser == null)
                                {
                                    // CrmStartUp = new CrmServiceController("localhost:6699");
                                    User fetchedUser = CrmStartUp.GetAssigneeUserIdUserwise_V2680(a.AssignedToUser);

                                    if (fetchedUser != null)
                                    {
                                        AssigneeList.Add(fetchedUser);
                                        matchedUser = fetchedUser;
                                    }
                                }
                                if (matchedUser != null)
                                {
                                    int index = AssigneeList.IndexOf(matchedUser);
                                    var ListAss = AssigneeList.Where(u => u.IdUser == matchedUser.IdUser).ToList();
                                    //AssigneeList.FirstOrDefault(u => u.IdUser == matchedUser.IdUser).SelectedIndexAssignee= index;
                                    item.SelectedAssignee = matchedUser;
                                    item.SelectedIndexAssignee = index;
                                    SelectedIndexAssigneeList = new List<object>(ListAss.ToList());
                                }
                            } 
                            else
                            {
                                if (AssigneeList == null)
                                    AssigneeList = new ObservableCollection<User>();

                                item.AssigneeList = AssigneeList;

                                item.SelectedAssignee =null;
                                item.SelectedIndexAssignee =-1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in FillEngineeringAnalysisDetails() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method FillEngineeringAnalysisDetails() executed successfully", category: Category.Info, priority: Priority.Low);
        }

       

        /// [001][GEOS2-2217][cpatil][03-04-2020]Eng. Analysis type field not working as expected
        private void GetEngAnalysisArticles()
        {
            try
            {
                //[001] Changed service method GetEngAnalysisArticles to GetEngAnalysisArticles_V2041
                EngineeringAnalysisTypes = CrmStartUp.GetEngAnalysisArticles_V2041();
                EngineeringAnalysisTypes.ToList().ForEach(eat => { eat.IsArticleEnabled = true;eat.TransactionOperation = ModelBase.TransactionOperations.Modify; });


            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetEngAnalysisArticles() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetEngAnalysisArticles() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in GetEngAnalysisArticles() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for to browse and add document into the list. 
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);

            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;

                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                    Attachment = new Attachment();
                    Attachment.FilePath = file.FullName;
                    Attachment.OriginalFileName = file.Name;
                    Attachment.IsDeleted = false;
                    Attachment.IsNew = true;

                    var newFileList = (SelectedRevision.TempAttachmentList != null) ? new List<object>(SelectedRevision.TempAttachmentList) : new List<object>();

                    // not allow to add same files
                    List<Attachment> tmpList = newFileList.OfType<Attachment>().ToList();
                    if (!tmpList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }

                    TempAttachmentListUI = newFileList;

                                      
                    SelectedRevision.TempAttachmentList=TempAttachmentListUI;
                    SelectedRevision.SelectedIndexAttachment = 0;


                    RevisionList = new List<EngineeringAnalysisRevision>(RevisionList);
                    SelectedRevision = RevisionList.FirstOrDefault(a => a.Header == SelectedRevision.Header);
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in BrowseFile() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method For Upload File
        /// </summary>
        /// <param name="obj"></param>
        public bool UploadFileEngineeringAnalysis(List<EngineeringAnalysisRevision> RevisionList)
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);

                IsBusy = true;
                
                foreach (EngineeringAnalysisRevision itemRev in RevisionList)
                {

                    if (itemRev.TempAttachmentList != null)
                    {
                        EngineeringAnalysisAddedAttachmentList = new List<Attachment>();

                        foreach (Attachment item in itemRev.TempAttachmentList)
                        {
                            if (item.IsNew)
                            {
                                
                                EngineeringAnalysisAddedAttachmentList.Add(item);
                            }

                        }
                    }
                }

                    if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                    {
                        FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                        List<FileInfo> FileDetail = new List<FileInfo>();
                        FileUploader EngAnalysisAttachmentFileUploader = new FileUploader();
                        EngAnalysisAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                        GuidCode = EngAnalysisAttachmentFileUploader.FileUploadName;

                        foreach (Attachment fs in EngineeringAnalysisAddedAttachmentList)
                        {
                            FileInfo file = new FileInfo(fs.FilePath);
                            FileDetail.Add(file);
                        }

                        EngAnalysisAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, EngAnalysisAttachmentFileUploader.FileUploadName);
                        EngAnalysisAttachmentFileUploaderIndicator = EngAnalysisAttachmentFileUploader;
                    if (EngAnalysisDuplicate != null && EngAnalysis != null)
                    {
                        EngAnalysisDuplicate.ForEach(ead => { ead.GUIDString = GuidCode; });
                        EngAnalysis.ForEach(ead => { ead.GUIDString = GuidCode; });
                    }
                    GeosApplication.Instance.Logger.Log("Getting Upload Engineering Analysis Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    }
               

                if (IsEdit)
                {
                  
                    if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                    {
                        isupload = true;
                    }
                }

                isupload = true;
            }
            catch (FaultException<ServiceException> ex)
            {
                isupload = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                isupload = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                isupload = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }

        /// <summary>
        /// Method For Accept Button
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptButtonCommandAction(object obj)
        {
            List<Attachment> listFinal = new List<Attachment>();
            //bool isDeleted = false;

            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction ...", category: Category.Info, priority: Priority.Low);


                if (RevisionList != null)
                {
                    RevisionList.Where(x => x.Comments == null).ToList().ForEach(a => { a.Comments = ""; });
                    if (RevisionList.Any(x => x.Comments.Trim() == ""))
                    {
                        RevError = null;
                        PropertyChanged(this, new PropertyChangedEventArgs("RevError"));

                        string error = EnableValidationAndGetError();

                        if (string.IsNullOrEmpty(error))
                        {
                            return;
                        }
                        else
                        {
                           RevError = " "; 
                        }
                        return;
                    }
                    else if (!RevisionList.Any(x => x.EngineeringAnalysisTypes.Any(i=>i.IsChecked==true)))
                    {
                        List<string> revisions = new List<string>();
                        foreach (EngineeringAnalysisRevision item in RevisionList)
                        {
                            if(!item.EngineeringAnalysisTypes.Any(i=>i.IsChecked==true))
                            revisions.Add("Rev." + item.RevNumber);
                        }
                        
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("SelectRevisionType").ToString(), string.Join(",", revisions.ToList()).ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return ;
                    }
                }

                else
                {
                    RevError = " ";
                }

                //string error = EnableValidationAndGetError();
                //PropertyChanged(this, new PropertyChangedEventArgs("Comments"));
                //PropertyChanged(this, new PropertyChangedEventArgs("DueDate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedEngineeringAnalysisTypes"));

                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexAttachment"));

                //if (error != null)
                //{
                //    IsBusy = false;
                //    return;
                //}
                //else
                {
                    if (IsNew)
                    {
                        EngAnalysisDuplicate = new List<EngineeringAnalysis>();
                        EngAnalysis = new List<EngineeringAnalysis>();
                        EngineeringAnalysis duplicate = new EngineeringAnalysis();
                        EngineeringAnalysis newrec = new EngineeringAnalysis();

                        EngineeringAnalysisRevision newRev = RevisionList.LastOrDefault();

                        duplicate.Comments = newRev.Comments;
                        duplicate.DueDate = newRev.DueDate;
                        //duplicate.IsCompleted = newRev.IsChecked;
                        duplicate.TransactionOperation = ModelBase.TransactionOperations.Add;
                        newRev.TransactionOperation = ModelBase.TransactionOperations.Add;
                        if (newRev.EngineeringAnalysisTypes != null)
                        {


                            duplicate.EngineeringAnalysisTypes = newRev.EngineeringAnalysisTypes;
                            duplicate.EngineeringAnalysisTypes.Where(a => a.IsChecked == true).ToList().ForEach(eat => { eat.Quantity = "1"; });
                            duplicate.SelectedEngineeringAnalysisTypes = new List<EngineeringAnalysisType>(duplicate.EngineeringAnalysisTypes.Where(a => a.IsChecked == true));
                        }




                        newrec.Comments = newRev.Comments;
                        newrec.DueDate = newRev.DueDate;
                        //EngAnalysis.IsCompleted = newRev.IsChecked;

                        if (newRev.EngineeringAnalysisTypes != null)
                        {

                            newrec.EngineeringAnalysisTypes = newRev.EngineeringAnalysisTypes;
                            newrec.EngineeringAnalysisTypes.Where(a => a.IsChecked == true).ToList().ForEach(eat => { eat.Quantity = "1"; });
                            newrec.SelectedEngineeringAnalysisTypes = new List<EngineeringAnalysisType>(newRev.EngineeringAnalysisTypes.Where(a => a.IsChecked == true));
                        }


                        if (newRev.TempAttachmentList == null || newRev.TempAttachmentList.Count == 0)
                        {
                            if (newrec.Attachments != null && newrec.Attachments.Count > 0)
                                newrec.Attachments.ForEach(i => i.IsDeleted = true);

                            duplicate.Attachments = new List<Attachment>();
                        }
                        else if (newRev.TempAttachmentList != null && newRev.TempAttachmentList.Count > 0)
                        {
                            List<Attachment> listAsUIAttachment = newRev.TempAttachmentList.Cast<Attachment>().ToList();

                            if (newrec.Attachments != null)
                                foreach (Attachment item in newrec.Attachments)
                                {
                                    if (!listAsUIAttachment.Any(x => x.OriginalFileName == item.OriginalFileName))
                                    {
                                        item.IsDeleted = true;
                                    }
                                  
                                }

                            if (listAsUIAttachment.Any(x => x.IsNew == true))
                            {
                                if (duplicate.Attachments == null)
                                    duplicate.Attachments = new List<Attachment>();

                                if (newrec.Attachments == null)
                                    newrec.Attachments = new List<Attachment>();

                                duplicate.Attachments.AddRange(listAsUIAttachment.Where(x => x.IsNew == true));
                                newrec.Attachments.AddRange(listAsUIAttachment.Where(x => x.IsNew == true));
                            }

                            duplicate.Attachments = newRev.TempAttachmentList.Cast<Attachment>().ToList();
                            newrec.Attachments = newRev.TempAttachmentList.Cast<Attachment>().ToList();
                        }

                      
                        IsSave = true;

                        EngAnalysisDuplicate.Add(duplicate);
                        EngAnalysis.Add(newrec);
                    }
                    else
                    {
                        foreach (EngineeringAnalysisRevision item in RevisionList)
                        {
                            if (EngAnalysisDuplicate!=null && EngAnalysisDuplicate.Any(i => i.RevNumber == item.RevNumber))
                            {
                               
                                EngineeringAnalysis duplicate = EngAnalysisDuplicate.Where(i => i.RevNumber == item.RevNumber).FirstOrDefault();
                                EngineeringAnalysis newrec = EngAnalysis.Where(i => i.RevNumber == item.RevNumber).FirstOrDefault();
                                foreach (EngineeringAnalysisType itemEngAnaType in item.EngineeringAnalysisTypes)
                                {
                                    
                                   
                                     EngineeringAnalysisType itemEngAnaTypeDuplicate = duplicate.EngineeringAnalysisTypes.Where(i => i.IdArticle == itemEngAnaType.IdArticle).FirstOrDefault();
                                    EngineeringAnalysisType itemEngAnaTypenewRec = newrec.EngineeringAnalysisTypes.Where(i => i.IdArticle == itemEngAnaType.IdArticle).FirstOrDefault();
                                    if (itemEngAnaTypeDuplicate != null)
                                    {
                                        bool needUpdate = false;

                                        if (itemEngAnaTypeDuplicate.IsChecked != itemEngAnaType.IsChecked || itemEngAnaTypeDuplicate.IsSelected != itemEngAnaType.IsSelected)
                                        {
                                            itemEngAnaTypeDuplicate.IsSelected = itemEngAnaType.IsSelected;
                                            itemEngAnaTypeDuplicate.IsChecked = itemEngAnaType.IsChecked;
                                            itemEngAnaTypeDuplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            item.TransactionOperation=ModelBase.TransactionOperations.Update;
                                            newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            if (itemEngAnaTypenewRec != null)
                                            {
                                                itemEngAnaTypenewRec.IsSelected = itemEngAnaType.IsSelected;
                                                itemEngAnaTypenewRec.IsChecked = itemEngAnaType.IsChecked;
                                                itemEngAnaTypenewRec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            }
                                            else
                                            {
                                                newrec.EngineeringAnalysisTypes.Add(itemEngAnaType);
                                            }
                                        }
                                        // Compare SelectedIndexAssignee
                                        if (itemEngAnaTypeDuplicate.SelectedIndexAssignee != itemEngAnaType.SelectedIndexAssignee)
                                        {
                                            itemEngAnaTypeDuplicate.SelectedIndexAssignee = itemEngAnaType.SelectedIndexAssignee;

                                            // Also update SelectedAssignee from AssigneeList
                                            if (AssigneeList != null &&
                                                itemEngAnaType.SelectedIndexAssignee >= 0 &&
                                                itemEngAnaType.SelectedIndexAssignee < AssigneeList.Count)
                                            {
                                                itemEngAnaTypeDuplicate.SelectedAssignee = AssigneeList[itemEngAnaType.SelectedIndexAssignee];
                                            }
                                            else
                                            {
                                                itemEngAnaTypeDuplicate.SelectedAssignee = null;
                                            }

                                            if (itemEngAnaTypenewRec != null)
                                            {
                                                itemEngAnaTypenewRec.SelectedIndexAssignee = itemEngAnaType.SelectedIndexAssignee;
                                                itemEngAnaTypenewRec.SelectedAssignee = itemEngAnaTypeDuplicate.SelectedAssignee;
                                                itemEngAnaTypenewRec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            }

                                            needUpdate = true;
                                        }

                                        // Mark transactions if needed
                                        if (needUpdate)
                                        {
                                            itemEngAnaTypeDuplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                            duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        }
                                    }
                                    else
                                    {
                                        if (itemEngAnaType.IsChecked == true)
                                        {
                                            duplicate.EngineeringAnalysisTypes.Add(itemEngAnaType);
                                            newrec.EngineeringAnalysisTypes.Add(itemEngAnaType);
                                        }
                                    }
                                }

                                if (duplicate.Comments != item.Comments)
                                {
                                   
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    duplicate.Comments = item.Comments;
                                    newrec.Comments = item.Comments;
                                }
                                if (duplicate.DueDate.Date != item.DueDate.Date)
                                {
                                    duplicate.DueDate = item.DueDate;
                                    newrec.DueDate = item.DueDate;
                                    item.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                    duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                }



                                if (item.TempAttachmentList == null || item.TempAttachmentList.Count == 0)
                                {
                                    if (duplicate.Attachments != null && duplicate.Attachments.Count > 0)
                                    {
                                        newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        duplicate.Attachments.ForEach(i => i.IsDeleted = true);
                                    }
                                       

                                    if (newrec.Attachments != null && newrec.Attachments.Count > 0)
                                    {
                                        newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        newrec.Attachments.ForEach(i => i.IsDeleted = true);
                                    }
                                     

                                 }
                                else if (item.TempAttachmentList != null && item.TempAttachmentList.Count > 0)
                                {
                                    List<Attachment> listAsUIAttachment = item.TempAttachmentList.Cast<Attachment>().ToList();

                                    if (duplicate.Attachments != null)
                                        foreach (Attachment itemAtt in duplicate.Attachments)
                                        {
                                            if (!listAsUIAttachment.Any(x => x.OriginalFileName == itemAtt.OriginalFileName))
                                            {
                                                newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                itemAtt.IsDeleted = true;
                                            }
                                        }

                                    if (newrec.Attachments != null)
                                        foreach (Attachment itemAtt in newrec.Attachments)
                                        {
                                            if (!listAsUIAttachment.Any(x => x.OriginalFileName == itemAtt.OriginalFileName))
                                            {
                                                newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                                itemAtt.IsDeleted = true;
                                            }
                                        }

                                    if(listAsUIAttachment.Any(x => x.IsNew == true))
                                    {
                                        if (duplicate.Attachments == null)
                                            duplicate.Attachments = new List<Attachment>();

                                        if (newrec.Attachments == null)
                                            newrec.Attachments = new List<Attachment>();

                                        newrec.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        duplicate.TransactionOperation = ModelBase.TransactionOperations.Update;
                                        foreach(Attachment attach in listAsUIAttachment.Where(x => x.IsNew == true))
                                        {
                                            if(!duplicate.Attachments.Any(i=>i.OriginalFileName== attach.OriginalFileName))
                                            {
                                                duplicate.Attachments.AddRange(listAsUIAttachment.Where(x => x.IsNew == true));
                                               
                                            }
                                            if (!newrec.Attachments.Any(i => i.OriginalFileName == attach.OriginalFileName))
                                            {
                                               
                                                newrec.Attachments.AddRange(listAsUIAttachment.Where(x => x.IsNew == true));
                                            }
                                        }
                                       
                                    }
                                 
                                    //    bool isupload = UploadFileEngineeringAnalysis(item.TempAttachmentList);

                                    //    if (!isupload)
                                    //    {
                                    //        IsBusy = false;
                                    //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    //        return;
                                    //    }

                                    //    if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                                    //    {
                                    //        if (duplicate.Attachments == null)
                                    //            duplicate.Attachments = new List<Attachment>();

                                    //        if (newrec.Attachments == null)
                                    //            newrec.Attachments = new List<Attachment>();

                                    //        duplicate.Attachments.AddRange(EngineeringAnalysisAddedAttachmentList);
                                    //        newrec.Attachments.AddRange(EngineeringAnalysisAddedAttachmentList);
                                    //    }

                                    //    duplicate.Attachments = item.TempAttachmentList.Cast<Attachment>().ToList();
                                    //    newrec.Attachments = item.TempAttachmentList.Cast<Attachment>().ToList();
                                    }

                                    //duplicate.GUIDString = GuidCode;
                                    //newrec.GUIDString = GuidCode;
                                    IsSave = true;
                            }
                            else
                            {

                                //EngAnalysisDuplicate = new List<EngineeringAnalysis>();
                                //EngAnalysis = new List<EngineeringAnalysis>();
                                EngineeringAnalysis duplicate = new EngineeringAnalysis();
                                EngineeringAnalysis newrec = new EngineeringAnalysis();

                                EngineeringAnalysisRevision newRev = RevisionList.Where(i=>i.RevNumber==item.RevNumber).LastOrDefault();

                                duplicate.Comments = newRev.Comments;
                                duplicate.DueDate = newRev.DueDate;
                                duplicate.RevNumber = item.RevNumber;
                                newrec.RevNumber = item.RevNumber;
                                duplicate.IdRevision = newRev.EngineeringAnalysisTypes.FirstOrDefault().IdRevision;                  //duplicate.IsCompleted = newRev.IsChecked;
                                duplicate.TransactionOperation = ModelBase.TransactionOperations.Add;
                                newrec.TransactionOperation = ModelBase.TransactionOperations.Add;

                                if (newRev.EngineeringAnalysisTypes != null)
                                {


                                    duplicate.EngineeringAnalysisTypes = newRev.EngineeringAnalysisTypes;
                                    duplicate.EngineeringAnalysisTypes.Where(a => a.IsChecked == true).ToList().ForEach(eat => { eat.Quantity = "1"; });
                                    duplicate.SelectedEngineeringAnalysisTypes = new List<EngineeringAnalysisType>(duplicate.EngineeringAnalysisTypes.Where(a => a.IsChecked == true));
                                }




                                newrec.Comments = newRev.Comments;
                                newrec.DueDate = newRev.DueDate;
                                //EngAnalysis.IsCompleted = newRev.IsChecked;

                                if (newRev.EngineeringAnalysisTypes != null)
                                {

                                    newrec.EngineeringAnalysisTypes = newRev.EngineeringAnalysisTypes;
                                    newrec.EngineeringAnalysisTypes.Where(a => a.IsChecked == true).ToList().ForEach(eat => { eat.Quantity = "1"; });
                                    newrec.SelectedEngineeringAnalysisTypes = new List<EngineeringAnalysisType>(newRev.EngineeringAnalysisTypes.Where(a => a.IsChecked == true));
                                }


                                if (newRev.TempAttachmentList == null || newRev.TempAttachmentList.Count == 0)
                                {
                                    if (newrec.Attachments != null && newrec.Attachments.Count > 0)
                                        newrec.Attachments.ForEach(i => i.IsDeleted = true);

                                    duplicate.Attachments = new List<Attachment>();
                                }
                                else if (newRev.TempAttachmentList != null && newRev.TempAttachmentList.Count > 0)
                                {
                                    List<Attachment> listAsUIAttachment = newRev.TempAttachmentList.Cast<Attachment>().ToList();

                                    if (newrec.Attachments != null)
                                        foreach (Attachment itemAtt in newrec.Attachments)
                                        {
                                            if (!listAsUIAttachment.Any(x => x.OriginalFileName == itemAtt.OriginalFileName))
                                            {
                                                itemAtt.IsDeleted = true;
                                            }
                                        }

                                    if (listAsUIAttachment.Any(x => x.IsNew == true))
                                    {
                                        if (duplicate.Attachments == null)
                                            duplicate.Attachments = new List<Attachment>();

                                        if (newrec.Attachments == null)
                                            newrec.Attachments = new List<Attachment>();

                                        duplicate.Attachments.AddRange(listAsUIAttachment.Where(x => x.IsNew == true));
                                        newrec.Attachments.AddRange(listAsUIAttachment.Where(x => x.IsNew == true));
                                    }
                                }

                                    //    bool isupload = UploadFileEngineeringAnalysis(newRev.TempAttachmentList);

                                    //    if (!isupload)
                                    //    {
                                    //        IsBusy = false;
                                    //        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    //        return;
                                    //    }

                                    //    if (EngineeringAnalysisAddedAttachmentList != null && EngineeringAnalysisAddedAttachmentList.Count > 0)
                                    //    {
                                    //        if (newrec.Attachments == null)
                                    //            newrec.Attachments = new List<Attachment>();

                                    //        newrec.Attachments.AddRange(EngineeringAnalysisAddedAttachmentList);
                                    //    }

                                    //    duplicate.Attachments = newRev.TempAttachmentList.Cast<Attachment>().ToList();
                                    //}

                                    //newrec.GUIDString = GuidCode;
                                    IsSave = true;
                                if (EngAnalysisDuplicate == null)
                                    EngAnalysisDuplicate = new List<EngineeringAnalysis>();

                                if (EngAnalysis == null)
                                    EngAnalysis = new List<EngineeringAnalysis>();

                                EngAnalysisDuplicate.Add(duplicate);
                                EngAnalysis.Add(newrec);
                            }
                        }
                      
                      

                    }

                    bool isupload = UploadFileEngineeringAnalysis(revisionList);
                  
                    if (!isupload)
                    {
                        IsBusy = false;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }

                    //update
                    RequestClose(null, null);
                }

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;

            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // could supply a default Icon here as well
            }

            return result;
        }

        /// <summary>
        /// Method for convert zip to byte.
        /// </summary>
        /// <param name="filesDetail"></param>
        /// <param name="GuidCode"></param>
        /// <returns></returns>
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;

            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolder\";

            if (!Directory.Exists(tempfolderPath))
            {
                System.IO.Directory.CreateDirectory(tempfolderPath);
            }

            try
            {
                GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);
                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            if (!File.Exists(tempfolderPath + EngineeringAnalysisAddedAttachmentList[i].OriginalFileName))
                            {
                                System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + EngineeringAnalysisAddedAttachmentList[i].OriginalFileName);
                            }

                            string s = tempfolderPath + EngineeringAnalysisAddedAttachmentList[i].OriginalFileName;
                            archive.AddFile(s, @"/");
                            EngineeringAnalysisAddedAttachmentList[i].FilePath = s;
                        }

                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }
                GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error On ConvertZipToByte Method - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                DeleteTempFolder();
                return filedetails;
            }
        }

        /// <summary>
        /// Method for delete TempFolder folders.
        /// </summary>
        private void DeleteTempFolder()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }

        /// <summary>
        /// Method for close Window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
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
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillAssigneeList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillAssigneeList..."), category: Category.Info, priority: Priority.Low);
                //CrmStartUp = new CrmServiceController("localhost:6699");
                AssigneeList = new ObservableCollection<User>();
                AssigneeList = CrmStartUp.GetAssigneeUserList_V2680();

                GeosApplication.Instance.Logger.Log(string.Format("Method FillAssigneeList()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillAssigneeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillAssigneeList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                foreach (EngineeringAnalysisRevision rev in RevisionList)
                {
                    string error =
                        me[BindableBase.GetPropertyName(() => rev.Comments)] +
                        me[BindableBase.GetPropertyName(() => rev.DueDate)] +
                        me[BindableBase.GetPropertyName(() => rev.SelectedEngineeringAnalysisTypes)];

                    //me[BindableBase.GetPropertyName(() => SelectedIndexAttachment)] +

                    if (!string.IsNullOrEmpty(error))
                        return "Please check inputted data.";
                }

                return null;
            }
        }

        

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                foreach (EngineeringAnalysisRevision rev in RevisionList)
                {
                    string comments = BindableBase.GetPropertyName(() => rev.Comments);
                    string dueDate = BindableBase.GetPropertyName(() => rev.DueDate);
                    string selectedEngineeringAnalysisTypes = BindableBase.GetPropertyName(() => rev.SelectedEngineeringAnalysisTypes);

                    //string selectedIndexAttachment = BindableBase.GetPropertyName(() => SelectedIndexAttachment);

                    if (columnName == comments)                                 //Comments
                    {
                        return EngineeringAnalysisValidation.GetErrorMessage(comments, rev.Comments);
                    }
                    else if (columnName == dueDate)                             //Due date.
                    {
                        return EngineeringAnalysisValidation.GetErrorMessage(dueDate, rev.DueDate);
                    }
                    else if (columnName == selectedEngineeringAnalysisTypes)    // Selected EngineeringAnalysis Types.
                    {
                        return EngineeringAnalysisValidation.GetErrorMessage(selectedEngineeringAnalysisTypes, rev.SelectedEngineeringAnalysisTypes);
                    }

                    //else if (columnName == selectedIndexAttachment)           //AttachmentType
                    //{
                    //    return EngineeringAnalysisValidation.GetErrorMessage(selectedIndexAttachment, SelectedIndexAttachment);
                    //}
                }
                return null;
            }
        }

        #endregion
    }
}
