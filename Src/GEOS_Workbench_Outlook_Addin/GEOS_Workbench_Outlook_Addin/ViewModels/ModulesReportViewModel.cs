using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Emdep.Geos.UI.Validations;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ModulesReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {

        #region TaskLog

        //[M051-25]	Add a new column Reference2 to modules report [adadibathina]

        #endregion


        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        #endregion // Services

        #region ICommands
        public ICommand ModulesReportAcceptButtonCommand { get; set; }
        public ICommand ModulesReportCancelButtonCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand SelectedItemTemplateChangedCommand { get; set; }
        public ICommand OptionPopupClosedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion // Commands

        #region Declaration

        ObservableCollection<Customer> groupList;
        List<Company> plantList;
        List<Template> templates;

        List<CpType> cpTypes;
        List<Detection> detections;
        List<object> selectedOptionList;

        Int16 selectedIndexGroup;
        Int16 selectedIndexPlant;


        Int16 selectedIndexTemplate;
        Int16 selectedIndexCpType;
        Int16 selectedIndexOption;

        private DateTime fromDate;
        private DateTime toDate;
        private ObservableCollection<Company> entireCompanyPlantList;
        #endregion // Declaration

        #region Properties

        public ObservableCollection<Company> EntireCompanyPlantList
        {
            get { return entireCompanyPlantList; }
            set { entireCompanyPlantList = value; }
        }
        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }

        public ObservableCollection<Customer> GroupList
        {
            get { return groupList; }
            set
            {
                groupList = value;
                OnPropertyChanged("GroupList");

            }
        }

        public List<Company> PlantList
        {
            get { return plantList; }
            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
            }
        }

        public List<Template> Templates
        {
            get { return templates; }
            set
            {
                templates = value;
                OnPropertyChanged("Templates");
            }
        }

        public short SelectedIndexGroup
        {
            get { return selectedIndexGroup; }
            set
            {
                selectedIndexGroup = value;
                OnPropertyChanged("SelectedIndexGroup");
            }
        }

        public short SelectedIndexPlant
        {
            get { return selectedIndexPlant; }
            set
            {
                selectedIndexPlant = value;
                OnPropertyChanged("SelectedIndexPlant");
            }
        }

        public List<CpType> CpTypes
        {
            get { return cpTypes; }
            set
            {
                cpTypes = value;
                OnPropertyChanged("CpTypes");
            }
        }

        public List<Detection> Detections
        {
            get { return detections; }
            set
            {
                detections = value;
                OnPropertyChanged("Detections");
            }
        }

        public List<object> SelectedOptionList
        {
            get { return selectedOptionList; }
            set
            {
                selectedOptionList = value;
                OnPropertyChanged("SelectedOptionList");
            }
        }

        public short SelectedIndexTemplate
        {
            get { return selectedIndexTemplate; }
            set
            {
                selectedIndexTemplate = value;
                OnPropertyChanged("SelectedIndexTemplate");
            }
        }

        public short SelectedIndexCpType
        {
            get { return selectedIndexCpType; }
            set
            {
                selectedIndexCpType = value;
                OnPropertyChanged("SelectedIndexCpType");
            }
        }

        public short SelectedIndexOption
        {
            get { return selectedIndexOption; }
            set
            {
                selectedIndexOption = value;
                OnPropertyChanged("SelectedIndexOption");
            }
        }

        #endregion // Properties

        #region Constructor

        public ModulesReportViewModel()
        {
            //this.ModulesReportView = modulesReportView;
            ModulesReportAcceptButtonCommand = new RelayCommand(new Action<object>(ModulesReportAcceptAction));
            ModulesReportCancelButtonCommand = new RelayCommand(new Action<object>(ModulesReportCancelAction));
            SelectedItemChangedCommand = new RelayCommand(new Action<object>(SelectedItemChangedAction));
            SelectedItemTemplateChangedCommand = new RelayCommand(new Action<object>(SelectedItemTemplateChangedAction));
            OptionPopupClosedCommand = new DevExpress.Mvvm.DelegateCommand<object>(OptionPopupClosedCommandAction);
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);
            FromDate = GeosApplication.Instance.SelectedyearStarDate.Date;
            ToDate = GeosApplication.Instance.SelectedyearEndDate.Date;
            FillCompanyPlantList();
            FillGroupList();

            //FillCpTypes();
            FillTemplate();
        }

        #endregion // Constructor

        #region public Events

        public event EventHandler RequestClose;
        // Property Change Logic 
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // Public Events

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
                    me[BindableBase.GetPropertyName(() => FromDate)] +
                    me[BindableBase.GetPropertyName(() => ToDate)];

                //+
                //me[BindableBase.GetPropertyName(() => SelectedIndexPlant)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexGroup)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexTemplate)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexCpType)] +
                //me[BindableBase.GetPropertyName(() => SelectedIndexOption)];

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
                string fromDateProp = BindableBase.GetPropertyName(() => FromDate);
                string toDateProp = BindableBase.GetPropertyName(() => ToDate);

                //string selectedIndexGroupProp = BindableBase.GetPropertyName(() => SelectedIndexGroup);
                //string selectedIndexPlantProp = BindableBase.GetPropertyName(() => SelectedIndexPlant);
                //string selectedIndexTemplateProp = BindableBase.GetPropertyName(() => SelectedIndexTemplate);
                //string selectedIndexCpTypeProp = BindableBase.GetPropertyName(() => SelectedIndexCpType);
                //string selectedIndexOption = BindableBase.GetPropertyName(() => SelectedIndexOption);

                if (columnName == fromDateProp)
                    return ReportValidation.GetErrorMessage(fromDateProp, FromDate);
                else if (columnName == toDateProp)
                    return ReportValidation.GetErrorMessage(toDateProp, ToDate);

                //else if (columnName == selectedIndexGroupProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexGroupProp, SelectedIndexGroup);
                //else if (columnName == selectedIndexPlantProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexPlantProp, SelectedIndexPlant);
                //else if (columnName == selectedIndexTemplateProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexTemplateProp, SelectedIndexTemplate);
                //else if (columnName == selectedIndexCpTypeProp)
                //    return ReportValidation.GetErrorMessage(selectedIndexCpTypeProp, SelectedIndexCpType);
                //else if (columnName == selectedIndexOption)
                //    return ReportValidation.GetErrorMessage(selectedIndexOption, SelectedIndexOption);

                return null;
            }
        }

        #endregion

        #region Methods

        private void OptionPopupClosedCommandAction(object obj)
        {
            if (((DevExpress.Xpf.Editors.ClosePopupEventArgs)obj).CloseMode == DevExpress.Xpf.Editors.PopupCloseMode.Cancel)
            {
                return;
            }
        }

        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList ...", category: Category.Info, priority: Priority.Low);

                IList<Customer> TempCompanyGroupList = null;
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //TempCompanyGroupList = CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP21"))
                    {
                        GroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP21"];
                        //SelectedIndexCompanyGroup = 0;
                    }
                    else
                    {
                        GroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup(salesOwnersIds, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP21", GroupList);
                        //SelectedIndexCompanyGroup = 0;
                    }
                }
                else
                {
                    //TempCompanyGroupList = CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYGROUP"))
                    {
                        GroupList = (ObservableCollection<Customer>)GeosApplication.Instance.ObjectPool["CRM_COMPANYGROUP"];
                        //SelectedIndexCompanyGroup = 0;
                    }
                    else
                    {
                        GroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYGROUP", GroupList);
                        //SelectedIndexCompanyGroup = 0;
                    }
                }

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

        private void FillCpTypes()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCpTypes ...", category: Category.Info, priority: Priority.Low);

                IList<CpType> TempCpTypeList = null;
                //TempCpTypeList = CrmStartUp.GetAllCpTypes();
                TempCpTypeList = CrmStartUp.GetCpTypesByTemplate(Templates[SelectedIndexTemplate].IdTemplate);
                TempCpTypeList.Insert(0, new CpType() { Name = "---" });

                CpTypes = TempCpTypeList.ToList();

                GeosApplication.Instance.Logger.Log("Method FillCpTypes() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCpTypes() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCpTypes() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCpTypes() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void FillTemplate()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillTemplate ...", category: Category.Info, priority: Priority.Low);

                IList<Template> TempTemplateList = null;
                TempTemplateList = CrmStartUp.GetTemplatesByIdTemplateType(1).ToList();
                Templates = new List<Template>();
                Templates.Insert(0, new Template() { Name = "---" });
                Templates.AddRange(TempTemplateList);

                GeosApplication.Instance.Logger.Log("Method FillTemplate() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplate() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplate() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillTemplate() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void FillDetectionByTemplateAndAndCpType()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCpTypes ...", category: Category.Info, priority: Priority.Low);

                Detections = CrmStartUp.GetDetectionByCpTypeAndTemplate(Templates[SelectedIndexTemplate].IdTemplate, 0);

                GeosApplication.Instance.Logger.Log("Method FillCpTypes() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCpTypes() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCpTypes() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCpTypes() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SelectedItemChangedAction(object obj)
        {
            if (SelectedIndexGroup > 0)
            {
                //FillCompanyPlantList();
                //TempcompanyPlantList.AddRange(new List<Company>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == GroupList[SelectedIndexGroup].IdCustomer).ToList()));
                PlantList = new List<Company>();
                PlantList = EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == GroupList[SelectedIndexGroup].IdCustomer || cpl.Name == "---").ToList();
                SelectedIndexPlant = 0;
            }
            else
            {
                PlantList = new List<Company>();
                PlantList = EntireCompanyPlantList.Where(cpl => cpl.Name == "---").ToList();
                SelectedIndexPlant = 0;
            }
        }

        public void SelectedItemTemplateChangedAction(object obj)
        {
            FillCpTypes();

            if (Templates[SelectedIndexTemplate].IdTemplate != 0)
                FillDetectionByTemplateAndAndCpType();
            else
                Detections = new List<Detection>();
        }

        private void FillCompanyPlantList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList ...", category: Category.Info, priority: Priority.Low);
                List<Company> TempPlantList = new List<Company>();
                if (GeosApplication.Instance.IdUserPermission == 21)
                {
                    List<UserManagerDtl> salesOwners = GeosApplication.Instance.SelectedSalesOwnerUsersList.Cast<UserManagerDtl>().ToList();
                    string salesOwnersIds = string.Join(",", salesOwners.Select(i => i.IdUser));
                    //TempPlantList = CrmStartUp.GetSelectedUserCompanyPlantByCustomerId(GroupList[SelectedIndexGroup].IdCustomer, salesOwnersIds);
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
                    // TempPlantList = CrmStartUp.GetCompanyPlantByCustomerId(GroupList[SelectedIndexGroup].IdCustomer, GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission);
                    if (GeosApplication.Instance.ObjectPool.ContainsKey("CRM_COMPANYPLANT"))
                        EntireCompanyPlantList = (ObservableCollection<Company>)GeosApplication.Instance.ObjectPool["CRM_COMPANYPLANT"];
                    else
                    {
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        GeosApplication.Instance.ObjectPool.Add("CRM_COMPANYPLANT", EntireCompanyPlantList);

                    }
                }

                //TempPlantList.Insert(0, new Company() { Name = "---" });
                //PlantList = TempPlantList.ToList();

                GeosApplication.Instance.Logger.Log("Method FillCompanyPlantList() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCompanyPlantList() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void ModulesReportAcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ModulesReportAcceptAction ...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("FromDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("ToDate"));

                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGroup"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexPlant"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexTemplate"));
                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCpType"));

                //PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexOption"));

                if (error != null)
                    return;
                else
                {
                    //var result = SelectedOptionList.Cast<Detection>().ToList();
                    //optionsString = Detections[SelectedIndexOption].Name;

                    GenerateModuleReport(obj);
                }

                GeosApplication.Instance.Logger.Log("Method ModulesReportAcceptAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ModulesReportAcceptAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void ModulesReportCancelAction(object obj)
        {
            RequestClose(null, null);
        }

        public void GenerateModuleReport(object obj)
        {
            try
            {
                string ResultFileName;
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Modules Report";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                bool DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                    return;
                }
                else
                {
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
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;

                    Workbook workbook = new Workbook();
                    workbook.Worksheets.Insert(0, "First");
                    Worksheet ws = workbook.Worksheets[0];

                    ws.Cells[0, 0].Value = "GROUP";
                    ws.Cells[0, 0].ColumnWidth = 350;

                    ws.Cells[0, 1].Value = "PLANT";
                    ws.Cells[0, 1].ColumnWidth = 600;

                    ws.Cells[0, 2].Value = "REGION";
                    ws.Cells[0, 2].ColumnWidth = 250;

                    ws.Cells[0, 3].Value = "OFFER";
                    ws.Cells[0, 3].ColumnWidth = 400;

                    ws.Cells[0, 4].Value = "WORK ORDER";
                    ws.Cells[0, 4].ColumnWidth = 400;

                    ws.Cells[0, 5].Value = "CUSTOMER SPECIFICATIONS";
                    ws.Cells[0, 5].ColumnWidth = 550;

                    ws.Cells[0, 6].Value = "ITEM";
                    ws.Cells[0, 6].ColumnWidth = 200;

                    ws.Cells[0, 7].Value = "REFERENCE1";
                    ws.Cells[0, 7].ColumnWidth = 100;

                    ws.Cells[0, 8].Value = "REFERENCE2";
                    ws.Cells[0, 8].ColumnWidth = 100;

                    ws.Cells[0, 9].Value = "TEMPLATE";
                    ws.Cells[0, 9].ColumnWidth = 400;

                    ws.Cells[0, 10].Value = "TYPE";
                    ws.Cells[0, 10].ColumnWidth = 400;

                    ws.Cells[0, 11].Value = "FAMILY";
                    ws.Cells[0, 11].ColumnWidth = 400;

                    ws.Cells[0, 12].Value = "CAVITIES";
                    ws.Cells[0, 12].ColumnWidth = 200;

                    ws.Cells[0, 13].Value = "SERIAL NUMBER";
                    ws.Cells[0, 13].ColumnWidth = 400;

                    ws.Cells[0, 14].Value = "ID DRAWING";
                    ws.Cells[0, 14].ColumnWidth = 270;

                    ws.Cells[0, 15].Value = "ITEM QUANTITY";
                    ws.Cells[0, 15].ColumnWidth = 220;

                    ws.Cells[0, 16].Value = "QUANTITY";
                    ws.Cells[0, 16].ColumnWidth = 220;

                    ws.Cells[0, 17].Value = "SHIPPING DATE";
                    ws.Cells[0, 17].ColumnWidth = 400;

                    ws.Cells[0, 18].Value = "SELL PRICE";
                    ws.Cells[0, 18].ColumnWidth = 400;

                    ws.Cells[0, 19].Value = "SITE";
                    ws.Cells[0, 19].ColumnWidth = 200;

                    ws.Range["A1:T1"].Font.Bold = true;
                    ws.Range["A1:T1"].Fill.BackgroundColor = System.Drawing.Color.LightGray;

                    List<Offer> offers = new List<Offer>();

                    try
                    {
                        //Customer
                        Int32? idCustomer = null;
                        if (GroupList != null && SelectedIndexGroup != -1 && GroupList[SelectedIndexGroup].IdCustomer != 0)
                            idCustomer = GroupList[SelectedIndexGroup].IdCustomer;

                        //Company
                        Int32? idCompany = null;
                        if (PlantList != null && SelectedIndexPlant != -1 && PlantList[SelectedIndexPlant].IdCompany != 0)
                            idCompany = PlantList[SelectedIndexPlant].IdCompany;

                        //Template
                        byte? idTemplate = null;
                        if (Templates != null && SelectedIndexTemplate != -1 && Templates[SelectedIndexTemplate].IdTemplate != 0)
                            idTemplate = Templates[SelectedIndexTemplate].IdTemplate;

                        //Type
                        byte? idCpType = null;
                        if (CpTypes != null && SelectedIndexCpType != -1 && CpTypes[SelectedIndexCpType].IdCPType != 0)
                            idCpType = CpTypes[SelectedIndexCpType].IdCPType;

                        //options (detections)
                        string optionsString = string.Empty;
                        if (SelectedOptionList != null)
                            optionsString = string.Join(",", SelectedOptionList.Cast<Detection>().ToList().Select(i => i.IdDetection));

                        //Sprint 44--CRM  M044-03--Modules report generated for selected plants----sdesai

                        if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                        {
                            List<Company> CompanyList = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                            foreach (Company company in CompanyList)
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + company.Alias;
                                    offers.AddRange(CrmStartUp.GetModulesReportDetails(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString, GeosApplication.Instance.IdCurrencyByRegion, idCustomer));
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                        else
                        {
                            //Modules report generated for all plants

                            List<Company> CompanyList = CrmStartUp.GetAllCompaniesDetails(GeosApplication.Instance.ActiveUser.IdUser);

                            foreach (Company company in CompanyList)
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + company.Alias;
                                    offers.AddRange(CrmStartUp.GetModulesReportDetails(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString, GeosApplication.Instance.IdCurrencyByRegion, idCustomer));
                                    GeosApplication.Instance.SplashScreenMessage = string.Empty;
                                }
                                catch (FaultException<ServiceException> ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                                }
                                catch (ServiceUnexceptedException ex)
                                {
                                    GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                                    GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }

                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                        GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                        GeosApplication.Instance.Logger.Log(string.Format("Get an error in GenerateModuleReport() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                    }
                    catch (Exception ex)
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        GeosApplication.Instance.SplashScreenMessage = string.Empty;
                        GeosApplication.Instance.Logger.Log(String.Format("Get an error in GenerateModuleReport() method {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);
                    }

                    int counter = 1;
                    bool isNumberFormatSupported = true;
                    foreach (Offer offer in offers)
                    {
                        // "GROUP";
                        ws.Cells[counter, 0].Value = offer.Site.Customer.CustomerName; // + " - " + offer.Site.Name;

                        // "PLANT";
                        ws.Cells[counter, 1].Value = offer.Site.Name;

                        // "REGION";
                        ws.Cells[counter, 2].Value = offer.Site.Country.Zone.Name;

                        // "OFFER";
                        ws.Cells[counter, 3].Value = offer.Code;

                        // "WORK ORDER";
                        ws.Cells[counter, 4].Value = string.Format("{0}{1}{2}", offer.Quotations[0].Ots[0].Code, " OT ", offer.Quotations[0].Ots[0].NumOT);

                        // "CUSTOMER SPECIFICATIONS";
                        if (offer.Quotations[0].TechnicalTemplate != null)
                            ws.Cells[counter, 5].Value = offer.Quotations[0].TechnicalTemplate.Name;

                        // "ITEM";
                        ws.Cells[counter, 6].Value = Convert.ToInt32(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.NumItem);


                        // "REFERENCE1";
                        ws.Cells[counter, 7].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CPProduct.Reference;

                        // "REFERENCE2";
                        ws.Cells[counter, 8].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CPProduct.OtherReference;

                        //  "TEMPLATE";
                        ws.Cells[counter, 9].Value = offer.Quotations[0].Template.Name;

                        //"TYPE"
                        ws.Cells[counter, 10].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CpType.Name;

                        //"FAMILY"
                        ws.Cells[counter, 11].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.ConnectorFamily;

                        //"CAVITIES"
                        ws.Cells[counter, 12].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.Ways;

                        //"SERIAL NUMBER"
                        ws.Cells[counter, 13].Value = offer.Quotations[0].Ots[0].OtItems[0].Counterparts[0].Code;

                        //"ID DRAWING"
                        ws.Cells[counter, 14].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.IdDrawing;

                        // "QUANTITY";
                        ws.Cells[counter, 15].Value = Convert.ToInt32(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.Quantity);

                        // "ITEM QUANTITY";
                        ws.Cells[counter, 16].Value = 1;

                        // "SHIPPING DATE";
                        ws.Cells[counter, 17].Value = offer.Quotations[0].Ots[0].OtItems[0].ShippingDate;

                        // "SELL PRICE"
                        ws.Cells[counter, 18].SetValue(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.UnitPrice);

                        try
                        {
                            if (isNumberFormatSupported)
                            {
                                ws.Cells[counter, 18].NumberFormat = "\"" + GeosApplication.Instance.CurrentCurrencySymbol + "\"" + " #,##,##0.00";
                            }
                        }
                        catch (Exception ex)
                        {
                            isNumberFormatSupported = false;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in GenerateModuleReport() NumberFormat - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                        // "SITE"
                        ws.Cells[counter, 19].Value = offer.Site.ShortName;

                        counter++;
                    }

                    ws.Columns.AutoFit(1, 19);

                    //control.SaveDocument(ResultFileName);
                    using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                    }

                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show("Modules Report Exported Successfully", "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, (ModulesReportView)obj);

                    //CustomMessageBox.Show(System.Windows.Application.Current.FindResource("ModulesReportExportedSuccessfully").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ModulesReportExportedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

                    System.Diagnostics.Process.Start(ResultFileName);
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }

            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        public void Dispose()
        {
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
        #endregion // Methods
    }
}
