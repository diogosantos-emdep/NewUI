using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    class AddEditReferenceViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Task log
        //pramod.misal GEOS2-5474 08.07.2024
        #endregion

        #region Services

        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");

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
        private string windowHeader;
        private bool isNew;
        private decimal quantity;
        private ObservableCollection<Article> referenceList;
        private Article selectedReference;
        private ImageSource articleImage;
        private decimal oldQuantity;

        #endregion

        #region Properties
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

        public decimal Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Quantity"));
            }
        }

        
        public ObservableCollection<Article> ReferenceList
        {
            get { return referenceList; }
            set
            {
                referenceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReferenceList"));
            }
        }

        public Article SelectedReference
        {
            get
            {
                return selectedReference;
            }

            set
            {
                selectedReference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedReference"));
            }
        }

        

        public ImageSource ArticleImage
        {
            get { return articleImage; }
            set
            {
                articleImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleImage"));
            }
        }

        

        private int selectedIdArticle;

        public int SelectedIdArticle
        {
            get { return selectedIdArticle; }
            set
            {
                selectedIdArticle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIdArticle"));
            }
        }

        private string selectedImgPath;
        public string SelectedImgPath
        {
            get { return selectedImgPath; }
            set
            {
                selectedImgPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedImgPath"));
            }
        }
        private ObservableCollection<OtItem> otItem;
        public ObservableCollection<OtItem> OtItem
        {
            get { return otItem; }
            set
            {
                otItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtItem"));
            }
        }

        private OtItem updatedOtItem;

        public OtItem UpdatedOtItem
        {
            get { return updatedOtItem; }
            set
            {
                updatedOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedOtItem"));
            }
        }

        private OtItem newOtItem;

        public OtItem NewOtItem
        {
            get { return newOtItem; }
            set
            {
                newOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewOtItem"));
            }
        }
        public decimal OldQuantity
        {
            get { return oldQuantity; }
            set { oldQuantity = value; OnPropertyChanged(new PropertyChangedEventArgs("OldQuantity")); }
        }
        #endregion

        #region ICommand

        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptAddreferenceActionCommand { get; set; }
        public ICommand ChangeReferenceCommand { get; set; }

        #endregion

        #region Constructor
        public AddEditReferenceViewModel(ObservableCollection<Article> referenceList)
        {
            try
            {
                AcceptAddreferenceActionCommand = new DelegateCommand<object>(AddReferenceAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                ChangeReferenceCommand = new DelegateCommand<object>(ChangeReferenceCommandAction);
                ReferenceList = referenceList;
                SelectedReference = ReferenceList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditReferenceViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method
        public void Init(ObservableCollection<OtItem> OtItems)
        {
            try
            {
                SelectedImgPath = SelectedReference.ImagePath;
                OtItem = new ObservableCollection<Data.Common.OtItem>();
                OtItem.AddRange(OtItems);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
           

        }

        public void EditInitItem(OtItem SelectedOtItem)
        {
            try
            {
                SelectedReference = ReferenceList.FirstOrDefault(x => x.IdArticle == SelectedOtItem.RevisionItem.WarehouseProduct.Article.IdArticle);
                SelectedImgPath = SelectedReference.ImagePath;
                Quantity = SelectedOtItem.RevisionItem.Quantity;
                UpdatedOtItem = SelectedOtItem;

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitItem() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in EditInitItem() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInitItem() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ChangeReferenceCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ChangeReferenceCommandAction()...", category: Category.Info, priority: Priority.Low);
                var eventArgs = obj as ClosePopupEventArgs;

                if (((ClosePopupEventArgs)obj).CloseMode == PopupCloseMode.Normal && eventArgs != null)
                {
                    var editValue = eventArgs.Source as ComboBoxEdit;
                    if (editValue != null) { SelectedIdArticle = (int)editValue.EditValue; }
                    
                    if (SelectedReference != null)
                    {
                        Article SelectedArticle = ReferenceList.FirstOrDefault(x => x.IdArticle == SelectedReference.IdArticle);
                        SelectedImgPath = SelectedArticle.ImagePath;
                    }
                }
                GeosApplication.Instance.Logger.Log("Method ChangeReferenceCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeReferenceCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AddReferenceAction(object obj)
         {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddReferenceAction()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("Quantity"));

                if (error != null)
                {
                    return;
                }
                if (IsNew)
                {
                    string numItem = string.Empty;
                    var lstrecord=OtItem.LastOrDefault();
                    int lstparatid = lstrecord.ParentId;
                    int lastkeyid = lstrecord.KeyId;
                    NewOtItem = new Data.Common.OtItem();
                    NewOtItem.RevisionItem = new RevisionItem();
                    NewOtItem.ParentId = -1;
                    if (lstrecord.RevisionItem.NumItem.Contains("."))
                        numItem = lstrecord.RevisionItem.NumItem.Split('.')[0];
                    else
                        numItem = lstrecord.RevisionItem.NumItem;
                    NewOtItem.RevisionItem.NumItem = (Convert.ToInt32(numItem) + 1).ToString();
                    NewOtItem.KeyId = lastkeyid + 1;
                    NewOtItem.RevisionItem.Quantity = Quantity;
                    NewOtItem.Status = new ItemOTStatusType(); 
                    NewOtItem.Status.Name = "READY";
                    NewOtItem.RevisionItem.WarehouseProduct = new WarehouseProduct();
                    NewOtItem.RevisionItem.WarehouseProduct.Article = SelectedReference;
                }
                else
                {
                    //UpdatedOtItem = new Data.Common.OtItem();
                    //UpdatedOtItem.RevisionItem =  new RevisionItem();
                    UpdatedOtItem.RevisionItem.Quantity = Quantity;         
                    UpdatedOtItem.RevisionItem.WarehouseProduct = new WarehouseProduct();
                    UpdatedOtItem.RevisionItem.WarehouseProduct.Article = SelectedReference;
                }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddReferenceAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddReferenceAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                //IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation [pramod.misal][08.07.2024]
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
                   
                    me[BindableBase.GetPropertyName(() => Quantity)];

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

                string quantity = BindableBase.GetPropertyName(() => Quantity);
               
            
                if (columnName == "Quantity")
                {
                    if (IsNew)
                    {
                        return AddEditQuantityValidation.GetErrorMessage(quantity, Quantity);
                    }
                   
                }
                
                return null;
            }
        }

        #endregion

    }
}
