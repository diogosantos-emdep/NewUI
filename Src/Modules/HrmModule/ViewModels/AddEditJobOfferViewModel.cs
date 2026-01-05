using DevExpress.CodeParser;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.CodeView;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Layout.Core;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Printing;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Model;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Modules.Hrm.Reports;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Emdep.Geos.Utility;
using Prism.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    // [nsatpute][28-04-2025][GEOS2-6502]
    public class AddEditJobOfferViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // IHrmService HrmService = new HrmServiceController("localhost:6699");
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

        #endregion // End Of Events

        #region Declaration
        
        bool isEnableField = false;
        private bool isNew;
        private bool isBusy;
        private string windowHeader;
        MaximizedElementPosition maximizedElementPosition;
        private double dialogHeight;
        private double dialogWidth;
       
        #endregion

        #region 
        private WorkflowStatus workflowStatus;
        private List<WorkflowStatus> workflowStatusList;
        private List<WorkflowStatus> workflowStatusButtons;
        private WorkflowStatus selectedWorkflowStatusButton;
        private List<WorkflowTransition> workflowTransitionList;
        private bool isAllControlDisabled;
        private Visibility clearButtonVisibility;
        #endregion

        #region Properties
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MaximizedElementPosition"));
            }
        }
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }
        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public bool IsEnableField
        {
            get
            {
                return isEnableField;
            }
            set
            {
                isEnableField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsEnableField"));
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
        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }
        #region [nsatpute][24-09-2024][GOES2-6473]
        public WorkflowStatus WorkflowStatus
        {
            get
            {
                return workflowStatus;
            }

            set
            {
                workflowStatus = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatus"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsControlEnabled"));                
            }
        }
        
        List<WorkflowStatus> originalWorkflowStatusList;
        public List<WorkflowStatus> OriginalWorkflowStatusList
        {
            get
            {
                return originalWorkflowStatusList;
            }
            set
            {
                originalWorkflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OriginalWorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusList
        {
            get
            {
                return workflowStatusList;
            }
            set
            {
                workflowStatusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusList"));
            }
        }
        public List<WorkflowStatus> WorkflowStatusButtons
        {
            get
            {
                return workflowStatusButtons;
            }
            set
            {
                workflowStatusButtons = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowStatusButtons"));
            }
        }
        public WorkflowStatus SelectedWorkflowStatusButton
        {
            get
            {
                return selectedWorkflowStatusButton;
            }
            set
            {
                selectedWorkflowStatusButton = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedWorkflowStatusButton"));
            }
        }
        public List<WorkflowTransition> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }
            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
       
        
        #endregion
        #endregion

        #region Data members
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public string GuidCode { get; set; }
           
        #endregion

        #region ICommand              
        public ICommand AcceptButtonCommand { get; set; }        
        public ICommand CancelButtonCommand { get; set; }
                #endregion

        #region Constructor
        public AddEditJobOfferViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditJobOfferViewModel ...", category: Category.Info, priority: Priority.Low);
                DialogWidth = SystemParameters.WorkArea.Width - 20;
                DialogHeight = SystemParameters.WorkArea.Height - (SystemParameters.WorkArea.Height < 840 ? 90 : 190);               
                CancelButtonCommand = new RelayCommand(new Action<object>(CancelButtonCommandAction));
                GeosApplication.Instance.Logger.Log("Constructor AddEditJobOfferViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditJobOfferViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods  
        public void EditInit(UInt32 idEmployeeTrip)
        {
            try
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Error in Constructor EditInit() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        public void Init()
        {
            try
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }


        private void CancelButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()...", category: Category.Info, priority: Priority.Low);                
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method CancelButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                /*
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                                me[BindableBase.GetPropertyName(() => SelectedTraveller)] +
                                 me[BindableBase.GetPropertyName(() => SelectedTraveller)];

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                */

                return null;
            }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                /*
                
                string accommodationCoordinates = BindableBase.GetPropertyName(() => AccommodationCoordinates);
                string selectedResponsible = BindableBase.GetPropertyName(() => SelectedResponsible);

                if (columnName == selectedType)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedType, SelectedType, string.Empty);
                } 
                //[GEOS2-6760][rdixit][14.01.2025]
                if (columnName == selectedResponsible)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedResponsible, SelectedResponsible, string.Empty);
                }
                if (columnName == selectedPropose)
                {
                    return AddEditTripValidations.GetErrorMessage(selectedPropose, SelectedPropose, string.Empty);
                }
                if (columnName == selectedTraveller)
                {
                    return AddEditTripValidations.GetErrorMessage(SelectedTraveller?.FullName, selectedTraveller, CustomTraveler);
                }
                if (columnName == customTraveler)
                {
                    return AddEditTripValidations.GetErrorMessage(customTraveler, CustomTraveler, SelectedTraveller?.FullName);
                }
                if (columnName == travelerEmail)
                {
                    return AddEditTripValidations.GetErrorMessage(travelerEmail, TravelerEmail, SelectedTraveller?.FullName);
                }
                if (columnName == arrivalTransportationNumber)
                {
                    return AddEditTripValidations.GetErrorMessage(arrivalTransportationNumber, ArrivalTransportationNumber, string.Empty);
                }
          */
               
                return null;
            }
        }


        #endregion
    }
}
