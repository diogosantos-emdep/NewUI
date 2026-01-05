using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
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

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AddNewTagViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Service

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        public bool IsSave { get; set; }
        private string tagName;
        private int selectedIndexCarOEM;
        public Tag NewTag { get; set; }
        private bool isBusy;
        private List<Tag> tagNameList;

        private Visibility alertVisibility;
        private List<string> tagNameStrList;
        private List<Tag> tempTagList;
        private string visible;
        #endregion

        #region Properties

        public List<Tag> TempTagList
        {
            get { return tempTagList; }
            set { tempTagList = value; OnPropertyChanged(new PropertyChangedEventArgs("TempTagList")); }
        }

        public List<Tag> TagNameList
        {
            get { return tagNameList; }
            set { tagNameList = value; OnPropertyChanged(new PropertyChangedEventArgs("TagNameList")); }
        }

        public List<string> TagNameStrList
        {
            get { return tagNameStrList; }
            set { tagNameStrList = value; OnPropertyChanged(new PropertyChangedEventArgs("TagNameStrList")); }
        }

        public string TagName
        {
            get { return tagName; }
            set
            {
                if (value != null)
                {
                    tagName = value.TrimStart();
                    OnPropertyChanged(new PropertyChangedEventArgs("TagName"));
                    ShowPopupAsPerTagName(TagName);
                }
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

        public Visibility AlertVisibility
        {
            get
            {
                return alertVisibility;
            }

            set
            {
                alertVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AlertVisibility"));
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
                    me[BindableBase.GetPropertyName(() => TagName)];

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
                string ProjectNameProp = BindableBase.GetPropertyName(() => TagName);


                if (columnName == ProjectNameProp)
                    return ActivityValidation.GetErrorMessage(ProjectNameProp, TagName);
                return null;
            }
        }

        #endregion

        #region ICommands

        public ICommand AddNewTagViewAcceptButtonCommand { get; set; }
        public ICommand AddNewTagViewCancelButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }

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

        public AddNewTagViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddNewTagViewModel ...", category: Category.Info, priority: Priority.Low);

                AddNewTagViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AddNewTagAccept);
                AddNewTagViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
                AlertVisibility = Visibility.Hidden;

                string error = EnableValidationAndGetError();
                OnPropertyChanged(new PropertyChangedEventArgs("TagName"));
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
                GeosApplication.Instance.Logger.Log("Constructor AddNewTagViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods



        /// <summary>
        /// Method for add new tag.
        /// </summary>
        /// <param name="obj"></param>
        public void AddNewTagAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(" AddNewTagAccept() Method ...", category: Category.Info, priority: Priority.Low);

                if (!string.IsNullOrEmpty(TagName.Trim()))
                {
                    TagName = TagName.Trim();
                    string error = EnableValidationAndGetError();
                    OnPropertyChanged(new PropertyChangedEventArgs("TagName"));

                    if (error != null)
                    {
                        IsBusy = false;
                        return;
                    }
                    try
                    {
                        bool isExist = CrmStartUp.IsExistTagName(TagName.Trim());

                        if (!isExist)
                        {
                            var taglist = new List<string>();
                            Tag NewTag = new Tag();
                            NewTag.Name = TagName.Trim();
                            taglist = TempTagList.Select(t => t.Name).ToList();

                            if (!taglist.Contains(TagName, StringComparer.OrdinalIgnoreCase))
                            {
                                TempTagList.Insert(0, NewTag);
                                IsSave = true;
                            }
                            else
                            {
                                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddNewTagExistMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                return;
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddNewTagFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method AddNewTagAccept() executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddNewTagAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
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
        private void ShowPopupAsPerTagName(string ProjectName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName ...", category: Category.Info, priority: Priority.Low);

            TagNameList = CrmStartUp.GetAllTags().ToList();

            if (TagNameList != null && !string.IsNullOrEmpty(TagName))
            {
                if (TagName.Length > 1)
                {
                    TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 2).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
                    TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();
                }
                else
                {
                    TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 1).ToUpper())
                                                            || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
                    TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();

                }
            }

            else
            {
                TagNameList = new List<Tag>();
                TagNameStrList = new List<string>();
            }

            //For alert Icon visibility
            if (TagNameStrList.Count > 0)
            {
                AlertVisibility = Visibility.Visible;
            }
            else
                AlertVisibility = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            TagName = string.Empty;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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
        #endregion
    }
}
