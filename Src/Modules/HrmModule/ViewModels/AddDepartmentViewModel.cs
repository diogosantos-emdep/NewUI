using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
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
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddDepartmentViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        // This View Model Created By Amit
        //[HRM-M040-05] New configuration section Departments

        #region Service
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Public ICommand
        public ICommand AddDepartmentViewCancelButtonCommand { get; set; }
        public ICommand AddDepartmentViewAcceptButtonCommand { get; set; }
        public ICommand OnColorValueChangingCommand { get; set; }

        public ICommand CommandTextInput { get; set; }
        #endregion

        #region public Events
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
        private bool isSave;
        private bool isNew;
        private string windowHeader;
        private string departmentName;
        private int selectedIndexParentDepartment;
        private int selectedIndexArea;
        private string hTMLColor;
        private bool isIsolated;
        private bool inUse;
        private int Isolated;
        private int Use;
        private uint DepartmentId;
        private ObservableCollection<Department> existDepartmentList;
        private string departmentAbbreviation;
        private bool isReadOnlyField;
        private bool isAcceptEnabled;


        #endregion

        #region Properties
        public List<LookupValue> DepartmentAreaList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public Department NewDepartment { get; set; }
        public Department EditDepartment { get; set; }
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeContractSituation"));
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

        public int SelectedIndexParentDepartment
        {
            get
            {
                return selectedIndexParentDepartment;
            }

            set
            {
                selectedIndexParentDepartment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexParentDepartment"));
            }
        }

        public int SelectedIndexArea
        {
            get
            {
                return selectedIndexArea;
            }

            set
            {
                selectedIndexArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexArea"));
            }
        }

        public ObservableCollection<Department> ExistDepartmentList
        {
            get
            {
                return existDepartmentList;
            }

            set
            {
                existDepartmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistDepartmentList"));
            }
        }

        public string DepartmentName
        {
            get
            {
                return departmentName;
            }

            set
            {
                departmentName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentName"));
            }
        }

        public string HTMLColor
        {
            get
            {
                return hTMLColor;
            }

            set
            {
                hTMLColor = value;
                //if (hTMLColor.StartsWith("#"))
                //    hTMLColor = HTMLColor.Remove(1, 2);
                OnPropertyChanged(new PropertyChangedEventArgs("HTMLColor"));
            }
        }

        public bool IsIsolated
        {
            get
            {
                return isIsolated;
            }

            set
            {
                isIsolated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsIsolated"));
            }
        }

        public bool InUse
        {
            get
            {
                return inUse;
            }

            set
            {
                inUse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));
            }
        }

        public string DepartmentAbbreviation
        {
            get
            {
                return departmentAbbreviation;
            }

            set
            {
                departmentAbbreviation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DepartmentAbbreviation"));
            }
        }
        public bool IsReadOnlyField
        {
            get { return isReadOnlyField; }
            set
            {
                isReadOnlyField = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsReadOnlyField"));
            }
        }

        public bool IsAcceptEnabled
        {
            get { return isAcceptEnabled; }
            set
            {
                isAcceptEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnabled"));
            }
        }


        #endregion

        #region Constructor
        public AddDepartmentViewModel()
        {
            try
            {
                SetUserPermission();
                GeosApplication.Instance.Logger.Log("Constructor AddDepartmentViewModel()...", category: Category.Info, priority: Priority.Low);

                AddDepartmentViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddDepartmentViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddDepartment));
                OnColorValueChangingCommand = new DelegateCommand<CustomDisplayTextEventArgs>(OnColorEditValueChanging);
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);



                GeosApplication.Instance.Logger.Log("Constructor AddDepartmentViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddDepartmentViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method for Initialization .
        /// [001][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        public void Init(ObservableCollection<Department> DepartmentDetailList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                ExistDepartmentList = new ObservableCollection<Department>(DepartmentDetailList);
                WindowHeader = System.Windows.Application.Current.FindResource("AddDepartmentInformation").ToString();
                //IsIsolated = true; //[001] Removed
                FillDepartmentList();
                FillDepartmentAreaList();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Edit Department Initialization .
        /// [001][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        public void EditInit(ObservableCollection<Department> DepartmentDetailList, Department department)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                ExistDepartmentList = new ObservableCollection<Department>(DepartmentDetailList);
                ExistDepartmentList.Remove(department);
                FillDepartmentList();
                FillDepartmentAreaList();
                WindowHeader = System.Windows.Application.Current.FindResource("EditDepartmentInformation").ToString();
                DepartmentId = department.IdDepartment;
                Department dept = DepartmentList.FirstOrDefault(x=>x.IdDepartment== DepartmentId);
                DepartmentList.Remove(dept);
                DepartmentName = department.DepartmentName;
                DepartmentAbbreviation = department.Abbreviation;
                SelectedIndexParentDepartment = DepartmentList.FindIndex(x => x.IdDepartment == department.IdDepartmentParent);
                SelectedIndexArea = DepartmentAreaList.FindIndex(x => x.IdLookupValue == department.IdDepartmentArea);
                if (!string.IsNullOrEmpty(department.DepartmentHtmlColor.Trim()))
                {
                    HTMLColor = department.DepartmentHtmlColor;
                    HTMLColor = HTMLColor.Insert(1, "F");
                    HTMLColor = HTMLColor.Insert(1, "F");
                }
                //Isolated = department.DepartmentIsIsolated; //[001] Removed
                Use = department.DepartmentInUse;

                //[001] Start removing
                //if (Isolated == 1)
                //{ IsIsolated = true; }
                //else
                //{ IsIsolated = false; }
                //[001] End removing

                if (Use == 1)
                { InUse = true; }
                else
                { InUse = false; }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        ///InitReadOnly Method for Read only users. 
        ///[HRM-M046-07] Add new permission ReadOnly--By Amit
        ///[001][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        public void InitReadOnly(ObservableCollection<Department> DepartmentDetailList, Department department)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = System.Windows.Application.Current.FindResource("EditDepartmentInformation").ToString();
                DepartmentList = new List<Department>();
                DepartmentList.Add(department.ParentDepartment);
                DepartmentId = department.IdDepartment;            
                DepartmentName = department.DepartmentName;
                DepartmentAbbreviation = department.Abbreviation;
                DepartmentAreaList = new List<LookupValue>();
                DepartmentAreaList.Add(department.DepartmentArea);
                SelectedIndexParentDepartment =0;
                SelectedIndexArea =0;
                if (!string.IsNullOrEmpty(department.DepartmentHtmlColor.Trim()))
                {
                    HTMLColor = department.DepartmentHtmlColor;
                    HTMLColor = HTMLColor.Insert(1, "F");
                    HTMLColor = HTMLColor.Insert(1, "F");
                }
                //Isolated = department.DepartmentIsIsolated; //[001] Removed
                Use = department.DepartmentInUse;

                //[001] Start removing
                //if (Isolated == 1)
                //{ IsIsolated = true; }
                //else
                //{ IsIsolated = false; }
                //[001] End removing

                if (Use == 1)
                { InUse = true; }
                else
                { InUse = false; }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }

            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Department List. 
        /// </summary>
        private void FillDepartmentList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartmentList()...", category: Category.Info, priority: Priority.Low);

                IList<Department> tempList = HrmService.GetAllDepartments();
                DepartmentList = new List<Department>();
                DepartmentList = new List<Department>(tempList);
                DepartmentList.Insert(0, new Department() { DepartmentName = "---", IdDepartment = 0, DepartmentInUse = 1 });

                GeosApplication.Instance.Logger.Log("Method FillDepartmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentList()...."+ ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentList()...."+ ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Fill Department Area List. 
        /// </summary>
        private void FillDepartmentAreaList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillDepartmentAreaList()...", category: Category.Info, priority: Priority.Low);

                IList<LookupValue> tempList = CrmStartUp.GetLookupValues(27);
                DepartmentAreaList = new List<LookupValue>();
                DepartmentAreaList = new List<LookupValue>(tempList);
                DepartmentAreaList.Insert(0, new LookupValue() { Value = "---", InUse = true });

                GeosApplication.Instance.Logger.Log("Method FillDepartmentAreaList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentAreaList()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentAreaList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentAreaList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for Add New Department. 
        /// [001][avpawar][GEOS2-2438][Identify the Job descriptions working remotely]
        /// </summary>
        private void AddDepartment(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDepartment()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                
                PropertyChanged(this, new PropertyChangedEventArgs("DepartmentName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexArea"));
                PropertyChanged(this, new PropertyChangedEventArgs("HTMLColor"));


                if (error != null)
                {
                    return;
                }

                //[001] start removing
                //if (IsIsolated == true)
                //{ Isolated = 1; }
                //else
                //{ Isolated = 0; }
                //[001] End removing

                if (InUse == true)
                { Use = 1; }
                else
                { Use = 0; }
                if (!string.IsNullOrEmpty(DepartmentName))
                    DepartmentName = DepartmentName.Trim();
                if (HTMLColor.StartsWith("#"))
                    HTMLColor = HTMLColor.Remove(1, 2);
                Department TempDepartment = new Department();
                TempDepartment = ExistDepartmentList.FirstOrDefault(x => x.DepartmentName == DepartmentName);
                if (TempDepartment == null)
                {
                    if (IsNew == true)
                    {
                        NewDepartment = new Department()
                        {
                            DepartmentName = DepartmentName,
                            IdDepartmentParent = DepartmentList[SelectedIndexParentDepartment].IdDepartment,
                            Abbreviation =DepartmentAbbreviation,
                            ParentDepartment = DepartmentList[SelectedIndexParentDepartment],
                            IdDepartmentArea = (uint)DepartmentAreaList[SelectedIndexArea].IdLookupValue,
                            DepartmentArea = DepartmentAreaList[SelectedIndexArea],
                            DepartmentHtmlColor = HTMLColor,
                            //DepartmentIsIsolated = (byte)Isolated, //[001] Removed
                            DepartmentInUse = (byte)Use,
                            TransactionOperation = ModelBase.TransactionOperations.Add
                        };
                       Department dept= HrmService.AddDepartment(NewDepartment);
                       NewDepartment.IdDepartment = dept.IdDepartment;
                        IsSave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DepartmentInformationAddedSuccess").ToString()),Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);

                    }
                    else
                    {
                        EditDepartment = new Department()
                        {
                            IdDepartment = (uint)DepartmentId,
                            DepartmentName = DepartmentName,
                            Abbreviation =DepartmentAbbreviation,
                            IdDepartmentParent = DepartmentList[SelectedIndexParentDepartment].IdDepartment,
                            ParentDepartment = DepartmentList[SelectedIndexParentDepartment],
                            IdDepartmentArea = (uint)DepartmentAreaList[SelectedIndexArea].IdLookupValue,
                            DepartmentArea = DepartmentAreaList[SelectedIndexArea],
                            DepartmentHtmlColor = HTMLColor,
                            //DepartmentIsIsolated = (byte)Isolated, //[001] Removed
                            DepartmentInUse = (byte)Use,
                            TransactionOperation = ModelBase.TransactionOperations.Update
                        };
                        HrmService.UpdateDepartment(EditDepartment);
                        IsSave = true;
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DepartmentInformationUpdatedSuccess").ToString()),Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                }
                else
                {
                    IsSave = false;
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DepartmentNameExist").ToString()), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }

                GeosApplication.Instance.Logger.Log("Method AddDepartment()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AddDepartment()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AddDepartment()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method AddDepartment()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Display Color Text  As Per Database Entry. Done by Mayuri
        /// </summary>
        
        public void OnColorEditValueChanging(CustomDisplayTextEventArgs e)
        {
            GeosApplication.Instance.Logger.Log("Method OnColorEditValueChanging ...", category: Category.Info, priority: Priority.Low);
           
            if (e.DisplayText.StartsWith("#"))
            {
                e.DisplayText = e.DisplayText.Remove(1, 2);
                e.EditValue = e.EditValue.ToString().Remove(1, 2);
            }
         
            e.Handled = true;
            GeosApplication.Instance.Logger.Log("Method OnColorEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// Method for Close Dialog Window . 
        /// </summary>
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
               
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => DepartmentName)] +
                    me[BindableBase.GetPropertyName(() => SelectedIndexArea)] +
                    me[BindableBase.GetPropertyName(() => HTMLColor)];
                   



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
                string departmentName = BindableBase.GetPropertyName(() => DepartmentName);
                string selectedIndexArea = BindableBase.GetPropertyName(() => SelectedIndexArea);
                string htmlColor = BindableBase.GetPropertyName(() => HTMLColor);                

                if (columnName == departmentName)
                {
                    return DepartmentValidation.GetErrorMessage(departmentName, DepartmentName);
                }
                if (columnName == selectedIndexArea)
                {
                    return DepartmentValidation.GetErrorMessage(selectedIndexArea, SelectedIndexArea);
                }
                if (columnName == htmlColor)
                {
                    return DepartmentValidation.GetErrorMessage(htmlColor, HTMLColor);
                }
               
                return null;
            }
        }
        #endregion

        private void SetUserPermission()
        {
            //HrmCommon.Instance.UserPermission = PermissionManagement.PlantViewer;

            switch (HrmCommon.Instance.UserPermission)
            {
                case PermissionManagement.SuperAdmin:
                    IsAcceptEnabled = true;
                    IsReadOnlyField = false;
                    break;

                case PermissionManagement.Admin:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.PlantViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                case PermissionManagement.GlobalViewer:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;

                default:
                    IsAcceptEnabled = false;
                    IsReadOnlyField = true;
                    break;
            }
        }
    }
}
