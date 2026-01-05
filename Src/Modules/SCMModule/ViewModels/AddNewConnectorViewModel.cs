using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class AddNewConnectorViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
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
        bool isSave = false;
        ObservableCollection<LinkType> linkedTypeList;
        LinkType selectedLinkedType;
        string refBackground;
        string reference;
        Connectors connector;
        bool isAcceptEnable =false;
        #endregion

        #region Properties

        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public ObservableCollection<LinkType> LinkedTypeList
        {
            get { return linkedTypeList; }
            set
            {
                linkedTypeList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkedTypeList"));
            }
        }

        public LinkType SelectedLinkedType
        {
            get { return selectedLinkedType; }
            set
            {
                selectedLinkedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLinkedType"));
            }
        }
   
        public string RefBackground
        {
            get { return refBackground; }
            set
            {
                refBackground = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RefBackground"));
            }
        }

        public Connectors Connector
        {
            get { return connector; }
            set
            {
                connector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Connector"));
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
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
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }    
        public ICommand AcceptButtonommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand CheckButtonommand { get; set; }
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]

        #endregion

        #region Constructor
        public AddNewConnectorViewModel()
        {
            try
            {
                RefBackground = null;
                GeosApplication.Instance.Logger.Log("Method AddNewConnectorViewModel()...", category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);            
                AcceptButtonommand = new DelegateCommand<object>(AcceptButtonAction);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                CheckButtonommand = new DelegateCommand<object>(ReferenceValidityMethod);
                FillLinkedType();
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
                GeosApplication.Instance.Logger.Log("Method AddNewConnectorViewModel()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewConnectorViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        private void ReferenceValidityMethod(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ReferenceValidityMethod()...", category: Category.Info, priority: Priority.Low);
                Connector = SCMService.CheckLinkedRefIsValid(Reference);
                if (Connector?.IdConnector > 0)
                {
                    RefBackground = "green";
                    IsAcceptEnable = true;
                }
                else
                {
                    RefBackground = "Red";
                    IsAcceptEnable = false;
                }

                GeosApplication.Instance.Logger.Log("Method ReferenceValidityMethod()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ReferenceValidityMethod() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddFileAction()...", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLinkedType"));
                if (error != null)
                {
                    return;
                }
                if (Connector != null && SelectedLinkedType != null)
                {
                    Connector.Ref = Reference;
                    if (SelectedLinkedType.IdLinkType==1)
                    {
                        Connector.IdLinkdType = 1;
                        Connector.LinkdTypeName = LinkedTypeList.First(i=>i.IdLinkType==1).Name;
                    }
                    else if (SelectedLinkedType.IdLinkType == 2)
                    {
                        Connector.IdLinkdType = 2;
                        Connector.LinkdTypeName = LinkedTypeList.First(i => i.IdLinkType == 2).Name;
                    }
                    else if (SelectedLinkedType.IdLinkType == 3)
                    {
                        Connector.IdLinkdType = 3;
                        Connector.LinkdTypeName = LinkedTypeList.First(i => i.IdLinkType == 3).Name;
                    }
                    else
                    {
                        Connector.IdLinkdType = 4;
                        Connector.LinkdTypeName = LinkedTypeList.First(i => i.IdLinkType == 4).Name;
                    }                   
                    IsSave = true;
                }
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AddFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillLinkedType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLinkedType()...", category: Category.Info, priority: Priority.Low);
                LinkedTypeList = new ObservableCollection<LinkType>(SCMService.GetAllLinkTypes());
                SelectedLinkedType = null;
                GeosApplication.Instance.Logger.Log(string.Format("Method FillLinkedType()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillLinkedType() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                if (SCMShortcuts.Instance.IsActive)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validation [pramod.misal][15.05.2024]
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
                    me[BindableBase.GetPropertyName(() => SelectedLinkedType)];               

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

                string SelectedLinkedTypeValue = BindableBase.GetPropertyName(() => SelectedLinkedType);             

                if (columnName == SelectedLinkedTypeValue)
                {
                    return SCMConnectorValidation.GetErrorMessage(SelectedLinkedTypeValue, SelectedLinkedType, null);
                }              
                return null;
            }
        }
        #endregion

    }
}
