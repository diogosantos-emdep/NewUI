using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.TSM.ViewModels
{
    //[GEOS2-8965][pallavi.kale][28.11.2025]
    public class AddWorkflowStatusCommentViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {

        }
        #region Service
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ITSMService TSMService = new TSMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ITSMService TSMService = new TSMServiceController("localhost:6699");


        #endregion

        #region Public Events

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // end public events region

        #region Declaration
        private string comment;
        private bool isSaveChanges;
        #endregion

        #region Properties
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
        public bool IsSaveChanges
        {
            get
            {
                return isSaveChanges;
            }

            set
            {
                isSaveChanges = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSaveChanges"));
            }
        }
        #endregion

        #region ICommands

        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }

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
                    me[BindableBase.GetPropertyName(() => Comment)];

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
                string CommentProp = BindableBase.GetPropertyName(() => Comment);

                if (columnName == CommentProp)
                {
                    return POWorkflowStatusAddCommentValidation.GetErrorMessage(CommentProp, Comment);
                }
                return null;
            }
        }
        #endregion

        #region Constructor

        public AddWorkflowStatusCommentViewModel()
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddWorkflowStatusCommentViewModel()...", category: Category.Info, priority: Priority.Low);

                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                AcceptButtonCommand = new DelegateCommand<object>(AcceptButtonAction);
                GeosApplication.Instance.Logger.Log("Constructor AddWorkflowStatusCommentViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddWorkflowStatusCommentViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region methods
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Comment"));

                if (error != null)
                {
                    return;
                }
                Comment = Comment.Trim();
                IsSaveChanges = true;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
