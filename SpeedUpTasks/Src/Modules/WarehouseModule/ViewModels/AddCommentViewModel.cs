using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Data.Common.Epc;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    class AddCommentViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services

        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion // Services

        #region Declaration
        private string windowHeader;
        public ArticleComment NewComment { get; set; }
        public ArticleComment UpdatedComment { get; set; }
        private bool isNew;
        private bool isSave;
        private Article articleData;
        private string comment;
        //private string stage;
        private long idArticleComment;
        private Stage stage;
        private string error = string.Empty;
        private ArticleComment tempComment;
        private DateTime? dateOfExpiry;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set { windowHeader = value; OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader")); }
        }

        public ObservableCollection<ArticleComment> ArticleWarehouseCommentsList
        { get; set; }
        public ArticleComment TempComment
        {
            get { return tempComment; }
            set { tempComment = value; }
        }
        public Article ArticleData
        {
            get { return articleData; }
            set
            {
                articleData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleData"));
            }
        }

        private List<Stage> stageList;

        public Stage Stage
        {
            get
            {
                return stage;
            }

            set
            {
                stage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Stage"));
            }
        }
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public string Comment
        {
            get
            {
                return comment;
            }

            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }

        public DateTime? DateOfExpiry
        {
            get
            {
                return dateOfExpiry;
            }

            set
            {
                dateOfExpiry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateOfExpiry"));
            }
        }
        public long IdArticleComment
        {
            get
            {
                return idArticleComment;
            }

            set
            {
                idArticleComment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdArticleComment"));
            }
        }

        //public string Stage
        //{
        //    get
        //    {
        //        return stage;
        //    }

        //    set
        //    {
        //        stage = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("Stage"));
        //    }
        //}
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

        public List<Stage> StageList
        {
            //get { return stageList; }
            //set { stageList = value; }

            get
            {
                return stageList;
            }

            set
            {
                stageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StageList"));
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

        #region Command       

        public ICommand CommandCancelButton { get; set; }

        public ICommand CloseWindowCommand { get; set; }

        public ICommand AcceptButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }

        #endregion

        #region Constructor 


        public AddCommentViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor AddCommentViewModel....", category: Category.Info, priority: Priority.Low);
            CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
            EscapeButtonCommand = new DelegateCommand<object>(CloseWindowAction);
            AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonAction);
            //CommandPrintButton = new DelegateCommand<object>(PrintAction);
            //this.WarehouseDeliveryNote = warehouseDeliveryNote;
            //this.ArticleData = ArticleData;
            //FillStageList();
            GeosApplication.Instance.Logger.Log("Constructor AddCommentViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region methods

        public void Init()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Init AddCommentViewModel....", category: Category.Info, priority: Priority.Low);
                FillStageList();


                GeosApplication.Instance.Logger.Log("Init AddCommentViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Init(ArticleComment SelectedComment)
        {
            GeosApplication.Instance.Logger.Log("AddCommentViewModelMethod Init(SelectedComment) ...", category: Category.Info, priority: Priority.Low);
            try
            {
                TempComment = SelectedComment;
                FillStageList();
                Comment = SelectedComment.Comment;
                Stage = StageList.FirstOrDefault(x => x.IdStage == SelectedComment.IdStage);
                if (SelectedComment.DateOfExpiry != null)
                {
                    DateOfExpiry = SelectedComment.DateOfExpiry;
                }
                else
                {
                    DateOfExpiry = null;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddCommentViewModel Method Init(SelectedComment)...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("AddCommentViewModelMethod Init(SelectedComment) executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void FillStageList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillStageList()...", category: Category.Info, priority: Priority.Low);

                //StageList= WarehouseService.GetStagesByWarehouseStageIds();

                StageList = new List<Stage>(WarehouseService.GetStagesByWarehouseStageIds());
                //if (StageList.Count > 0)
                //    SelectedStage = StageList.FirstOrDefault(x => x.IdStage == ArticleData.c);


                GeosApplication.Instance.Logger.Log("Method FillStageList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillStageList() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonAction()..."), category: Category.Info, priority: Priority.Low);

                //if (IsCheckedCopyNameAndDescription == false && Name_en == null)
                //{
                //    LanguageSelected = Languages.FirstOrDefault(a => a.IdLanguage == 2);
                //    Description = Description_en;
                //    Name = Name_en;
                //}

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Comment"));
                PropertyChanged(this, new PropertyChangedEventArgs("Stage"));

                if (error != null)
                {
                    return;
                }

                //ArticleWarehouseCommentsList = new ObservableCollection<ArticleComment>();
                bool val = true;
                if (ArticleWarehouseCommentsList != null)
                {
                    foreach (ArticleComment comment in ArticleWarehouseCommentsList)
                    {
                        if (comment.IdStage == Stage.IdStage)               // && tempComment.IdArticleComment!=IdArticleComment
                        {
                            try
                            {
                                if (tempComment.IdArticleComment != comment.IdArticleComment)
                                {
                                    val = false;
                                    break;
                                }
                            }
                            catch
                            {
                                val = false;
                                break;
                            }
                        }
                    }
                }



                if (val)
                {
                    if (IsNew)
                    {
                        NewComment = new ArticleComment();
                        NewComment.Comment = Comment;
                        NewComment.IdStage = Stage.IdStage;
                        NewComment.Stage = new Data.Common.Stage();
                        NewComment.Stage.Name = Stage.Name;

                        if (DateOfExpiry == null)
                        {
                            NewComment.DateOfExpiry = null;
                        }
                        NewComment.DateOfExpiry = DateOfExpiry;

                        NewComment.CreationDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                        IsSave = true;

                        //NewArticleCategory.Position = SelectedOrderCategory.Position;
                        //NewArticleCategory.Article_count = 0;
                        //NewArticleCategory.IdCreator = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);

                        //OrderCategoryList.Add(NewArticleCategory);
                        //List<PCMArticleCategory> pcmArticleCategory_ForSetOrder = OrderCategoryList.Where(a => a.Parent == NewArticleCategory.Parent && a.Name != "---").OrderBy(a => a.Position).ToList();
                        //uint pos = 1;
                        //uint Old_Position_set = 0;
                        //NewComment= WarehouseService.Addcomm(NewComment, pcmArticleCategory_ForSetOrder_new);

                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddCommentSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    else
                    {
                        UpdatedComment = new ArticleComment();
                        UpdatedComment.Comment = Comment;
                        UpdatedComment.IdStage = Stage.IdStage;
                        UpdatedComment.Stage = new Data.Common.Stage();
                        UpdatedComment.Stage.Name = Stage.Name;
                        UpdatedComment.IdArticleComment = IdArticleComment;
                        if (DateOfExpiry == null)
                        {
                            UpdatedComment.DateOfExpiry = null;
                        }
                        UpdatedComment.DateOfExpiry = DateOfExpiry;
                        //UpdatedComment.IdCreator= IdCreator;

                        //UpdatedComment.KeyName = KeyName;
                        //UpdatedComment.NameWithArticleCount = Name_en + " [" + Article_count + "]";

                        //UpdatedItem.Article_count = Article_count;

                        //UpdatedItem.IdModifier = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                        UpdatedComment.ModificationDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;



                        IsSave = true;
                        //IsSave = WarehouseService.IsUpdatePCMArticleCategory(UpdatedComment, pcmArticleCategory_ForSetOrder_new);
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("UpdateCommentSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                    GeosApplication.Instance.Logger.Log(string.Format("Method AcceptButtonAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
                else
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DoubleCommentMsg").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindowAction(object obj)
        {
            try
            {
                //GeosApplication.Instance.Logger.Log("Method CloseWindowAction()...", category: Category.Info, priority: Priority.Low);
                RequestClose(null, null);
                //GeosApplication.Instance.Logger.Log("Method CloseWindowAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation

        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }
        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => Comment)]
                +
                me[BindableBase.GetPropertyName(() => Stage)];

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

                string comment = BindableBase.GetPropertyName(() => Comment);
                string stage = BindableBase.GetPropertyName(() => Stage);

                if (columnName == comment)
                {
                    return AddEditCommentValidation.GetErrorMessage(comment, Comment);
                }

                if (columnName == stage)
                {
                    return AddEditCommentValidation.GetErrorMessage(stage, Stage);
                }

                return null;
            }
        }
        #endregion
    }
}