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
        ModuleFamily selectedFamily;
        ModuleSubFamily selectedSubFamily;
        ObservableCollection<Customer> groupList;
        List<Company> plantList;
        List<Template> templates;

        List<CpType> cpTypes;
        List<Detection> detections;
        List<object> selectedOptionList;

        Int16 selectedIndexGroup;
        Int16 selectedIndexPlant;

        ObservableCollection<ModuleFamily> familyList;
        ObservableCollection<ModuleSubFamily> subFamilyList;
        Int16 selectedIndexTemplate;
        Int16 selectedIndexCpType;
        Int16 selectedIndexOption;
        private DateTime fromDate;
        private DateTime toDate;
        private ObservableCollection<Company> entireCompanyPlantList;
        private ObservableCollection<Company> companyList;
        private Company selectedCompanyIndex;

        #endregion // Declaration

        #region Properties

        public ModuleFamily SelectedFamily
        {
            get { return selectedFamily; }
            set
            {
                selectedFamily = value;
                OnPropertyChanged("SelectedFamily");
                SelectedFamilyItemChangedAction();
            }
        }
        public ModuleSubFamily SelectedSubFamily
        {
            get { return selectedSubFamily; }
            set
            {
                selectedSubFamily = value;
                OnPropertyChanged("SelectedSubFamily");
            }
        }
        public ObservableCollection<ModuleFamily> FamilyList
        {
            get { return familyList; }
            set
            {
                familyList = value;
                OnPropertyChanged("FamilyList");
            }
        }
        public ObservableCollection<ModuleSubFamily> SubFamilyList
        {
            get { return subFamilyList; }
            set
            {
                subFamilyList = value;
                OnPropertyChanged("SubFamilyList");
            }
        }
        public virtual List<object> SelectedItems { get; set; }
        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged("CompanyList");
            }
        }
        public Company SelectedCompanyIndex
        {
            get
            {
                return selectedCompanyIndex;
            }

            set
            {
                selectedCompanyIndex = value;
                OnPropertyChanged("SelectedCompanyIndex");
            }
        }
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
        /// [001][cpatil][GEOS2-5299][26-02-2024]
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
            // [001] Changed Service method
            //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
            CompanyList = new ObservableCollection<Company>(CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser));

            SelectedItems = new List<object>();
            SelectedItems.Clear();
            SelectedItems = new List<object>(CompanyList);

            //FillCpTypes();
            FillTemplate();
            FillFamilies();
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
                        //[rdixit][GEOS2-4682][08-08-2023] Service GetSelectedUserCompanyGroup updated with GetSelectedUserCompanyGroup_V2420
                        GroupList = new ObservableCollection<Customer>(CrmStartUp.GetSelectedUserCompanyGroup_V2420(salesOwnersIds, true));
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
                        // GroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));

                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        //[19.10.2023][GEOS2-4903][rdixit] Service GetCompanyGroup_V2420 updated with GetCompanyGroup_V2450
                        GroupList = new ObservableCollection<Customer>(CrmStartUp.GetCompanyGroup_V2450(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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
               // TempCpTypeList = CrmStartUp.GetCpTypesByTemplate(Templates[SelectedIndexTemplate].IdTemplate);

                TempCpTypeList = CrmStartUp.GetCpTypesByTemplate_V2530(Templates[SelectedIndexTemplate].IdTemplate);  //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]


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

               // Detections = CrmStartUp.GetDetectionByCpTypeAndTemplate(Templates[SelectedIndexTemplate].IdTemplate, 0);


                Detections = CrmStartUp.GetDetectionByCpTypeAndTemplate_V2530(Templates[SelectedIndexTemplate].IdTemplate, 0);  	//[rushikesh.gaikwad][GEOS2-5583][20.06.2024]


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
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser(salesOwnersIds, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetSelectedUserCompanyPlantByIdUser_V2420(salesOwnersIds, true));
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
                        //EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
                        //[pramod.misal][GEOS2-4682][08-08-2023]
                        EntireCompanyPlantList = new ObservableCollection<Company>(CrmStartUp.GetCompanyPlantByUserId_V2420(GeosApplication.Instance.ActiveUser.IdUser, GeosApplication.Instance.ActiveUser.Company.Country.IdZone, GeosApplication.Instance.IdUserPermission, true));
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
        /// <summary>
        /// [001][ashish.malkhede][19-07-2024] CRM - Modules Report Improvements https://helpdesk.emdep.com/browse/GEOS2-5897
        /// </summary>
        /// <param name="obj"></param>
        public void GenerateModuleReport(object obj)
        {
            try
            {
                //[001] Rename Module report name
                string ResultFileName;
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd_HH-mm");
                string ReportName = "Module_Report_" + formattedDateTime + "";
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = ReportName;
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

                    ws.Cells[0, 4].Value = "BUSINESS UNIT";
                    ws.Cells[0, 4].ColumnWidth = 400;

                    ws.Cells[0, 5].Value = "CATEGORY1";
                    ws.Cells[0, 5].ColumnWidth = 400;

                    ws.Cells[0, 6].Value = "CATEGORY2";
                    ws.Cells[0, 6].ColumnWidth = 600;

                    ws.Cells[0, 7].Value = "WORK ORDER";
                    ws.Cells[0, 7].ColumnWidth = 400;

                    ws.Cells[0, 8].Value = "CUSTOMER SPECIFICATIONS";
                    ws.Cells[0, 8].ColumnWidth = 550;

                    ws.Cells[0, 9].Value = "ITEM";
                    ws.Cells[0, 9].ColumnWidth = 200;

                    ws.Cells[0, 10].Value = "REFERENCE1";
                    ws.Cells[0, 10].ColumnWidth = 100;

                    ws.Cells[0, 11].Value = "REFERENCE2";
                    ws.Cells[0, 11].ColumnWidth = 100;

                    ws.Cells[0, 12].Value = "TEMPLATE";
                    ws.Cells[0, 12].ColumnWidth = 400;

                    ws.Cells[0, 13].Value = "TYPE";
                    ws.Cells[0, 13].ColumnWidth = 400;

                    ws.Cells[0, 14].Value = "FAMILY";
                    ws.Cells[0, 14].ColumnWidth = 400;

                    //[rdixit][GEOS2-4128][11.01.2023]
                    ws.Cells[0, 15].Value = "SUBFAMILY";
                    ws.Cells[0, 15].ColumnWidth = 400;

                    ws.Cells[0, 16].Value = "CAVITIES";
                    ws.Cells[0, 16].ColumnWidth = 200;

                    ws.Cells[0, 17].Value = "SERIAL NUMBER";
                    ws.Cells[0, 17].ColumnWidth = 400;

                    ws.Cells[0, 18].Value = "ID DRAWING";
                    ws.Cells[0, 18].ColumnWidth = 270;

                    ws.Cells[0, 19].Value = "ITEM QUANTITY";
                    ws.Cells[0, 19].ColumnWidth = 220;

                    ws.Cells[0, 20].Value = "QUANTITY";
                    ws.Cells[0, 20].ColumnWidth = 220;

                    ws.Cells[0, 21].Value = "SHIPPING DATE";
                    ws.Cells[0, 21].ColumnWidth = 400;

                    ws.Cells[0, 22].Value = "OT PRICE";
                    ws.Cells[0, 22].ColumnWidth = 400;

                    ws.Cells[0, 23].Value = "SITE";
                    ws.Cells[0, 23].ColumnWidth = 200;

                    //[001] Added selected options in header 
                    int cellsColumn = 24;
                    if (selectedOptionList != null)
                    {
                        foreach (Detection d in selectedOptionList)
                        {
                            ws.Cells[0, cellsColumn].Value = d.Name;
                            ws.Cells[0, cellsColumn].Font.Bold = true;
                            ws.Cells[0, cellsColumn].Fill.BackgroundColor = System.Drawing.Color.LightGray;
                            cellsColumn++;
                        }
                        ws.Columns.AutoFit(24, cellsColumn);
                    }//[001] End


                    ws.Range["A1:X1"].Font.Bold = true;
                    ws.Range["A1:X1"].Fill.BackgroundColor = System.Drawing.Color.LightGray;

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

                        ////Type
                        //byte? idCpType = null;
                        //if (CpTypes != null && SelectedIndexCpType != -1 && CpTypes[SelectedIndexCpType].IdCPType != 0)
                        //    idCpType = CpTypes[SelectedIndexCpType].IdCPType;


                        //Type
                          long? idCpType = null;
                        if (CpTypes != null && SelectedIndexCpType != -1 && CpTypes[SelectedIndexCpType].IdCPTypenew != 0)
                            idCpType = CpTypes[SelectedIndexCpType].IdCPTypenew;                 //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]

                        //options (detections)
                        string optionsString = string.Empty;
                        if (SelectedOptionList != null)
                            optionsString = string.Join(",", SelectedOptionList.Cast<Detection>().ToList().Select(i => i.IdDetection));

                        //Family [rdixit][10.02.2023][GEOS2-4127]
                        byte? idFamily = null;
                        if (FamilyList != null && SelectedFamily.IdModuleFamily != 0)
                            idFamily = (byte?)SelectedFamily.IdModuleFamily;

                        //SubFamily  [rdixit][10.02.2023][GEOS2-4127]
                        byte? idSubFamily = null;
                        if (SubFamilyList != null && SelectedSubFamily.IdModuleSubfamily != 0)
                            idSubFamily = (byte?)SelectedSubFamily.IdModuleSubfamily;

                        //Sprint 44--CRM  M044-03--Modules report generated for selected plants----sdesai

                        if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                        {
                            //List<Company> CompanyList = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                            foreach (Company company in SelectedItems)
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + company.Alias;
                                    //offers.AddRange(CrmStartUp.GetModulesReportDetails_V2130(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString, GeosApplication.Instance.IdCurrencyByRegion, idCustomer));
                                    //Service Methd changed from GetModulesReportDetails_V2220 to GetModulesReportDetails_V2350 by [rdixit][GEOS2-4128][11.01.2023]
                                    //Service Methd changed from GetModulesReportDetails_V2350 to GetModulesReportDetails_V2360 by [rdixit][GEOS2-4127][10.02.2023]

                                    //offers.AddRange(CrmStartUp.GetModulesReportDetails_V2360(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString,
                                    //    GeosApplication.Instance.IdCurrencyByRegion, idFamily, idSubFamily, idCustomer));


                                    //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]
                                    //offers.AddRange(CrmStartUp.GetModulesReportDetails_V2530(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString,
                                    //   GeosApplication.Instance.IdCurrencyByRegion, idFamily, idSubFamily, idCustomer));

                                    //[001]
                                    offers.AddRange(CrmStartUp.GetModulesReportDetails_V2550(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString,
                                  GeosApplication.Instance.IdCurrencyByRegion, idFamily, idSubFamily, idCustomer));
                                    //[001] End
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

                            //List<Company> CompanyList = CrmStartUp.GetAllCompaniesDetails_V2130(GeosApplication.Instance.ActiveUser.IdUser);

                            foreach (Company company in SelectedItems)
                            {
                                try
                                {
                                    GeosApplication.Instance.SplashScreenMessage = "Connecting to " + company.Alias;
                                    // offers.AddRange(CrmStartUp.GetModulesReportDetails_V2130(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString, GeosApplication.Instance.IdCurrencyByRegion, idCustomer));
                                    //Service Methd changed from GetModulesReportDetails_V2220 to GetModulesReportDetails_V2360 by [rdixit][GEOS2-4127][10.02.2023]

                                    //offers.AddRange(CrmStartUp.GetModulesReportDetails_V2360(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString,
                                    //    GeosApplication.Instance.IdCurrencyByRegion, idFamily, idSubFamily, idCustomer));

                                    //offers.AddRange(CrmStartUp.GetModulesReportDetails_V2530(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString,
                                    //   GeosApplication.Instance.IdCurrencyByRegion, idFamily, idSubFamily, idCustomer));        //[rushikesh.gaikwad][GEOS2-5583][20.06.2024]
                                    //[001]
                                    offers.AddRange(CrmStartUp.GetModulesReportDetails_V2550(1, company, FromDate, ToDate, idCompany, idTemplate, idCpType, optionsString,
                                       GeosApplication.Instance.IdCurrencyByRegion, idFamily, idSubFamily, idCustomer));
                                    //[001] End
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

                        if (offer.BusinessUnit != null)
                        {
                            // "Business Unit";
                            ws.Cells[counter, 4].Value = offer.BusinessUnit.Value;
                        }
                        // "Category1";
                        ws.Cells[counter, 5].Value = offer.Category1;

                        // "Category2";
                        ws.Cells[counter, 6].Value = offer.Category2;

                        // "WORK ORDER";
                        ws.Cells[counter, 7].Value = string.Format("{0}{1}{2}", offer.Quotations[0].Ots[0].Code, " OT ", offer.Quotations[0].Ots[0].NumOT);

                        // "CUSTOMER SPECIFICATIONS";
                        if (offer.Quotations[0].TechnicalTemplate != null)
                            ws.Cells[counter, 8].Value = offer.Quotations[0].TechnicalTemplate.Name;

                        // "ITEM";
                        ws.Cells[counter, 9].Value = Convert.ToInt32(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.NumItem);


                        // "REFERENCE1";
                        ws.Cells[counter, 10].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CPProduct.Reference;

                        // "REFERENCE2";
                        ws.Cells[counter, 11].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CPProduct.OtherReference;

                        //  "TEMPLATE";
                        ws.Cells[counter, 12].Value = offer.Quotations[0].Template.Name;

                        //"TYPE"
                        ws.Cells[counter, 13].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CpType.Name;

                        //"FAMILY"
                        ws.Cells[counter, 14].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.ConnectorFamily;

                        //"SUBFAMILY" [rdixit][GEOS2-4128][11.01.2023]
                        ws.Cells[counter, 15].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.ConnectorSubFamily;

                        //"CAVITIES"
                        ws.Cells[counter, 16].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.Ways;

                        //"SERIAL NUMBER"
                        ws.Cells[counter, 17].Value = offer.Quotations[0].Ots[0].OtItems[0].Counterparts[0].Code;

                        //"ID DRAWING"
                        ws.Cells[counter, 18].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.IdDrawing;

                        // "QUANTITY";
                        ws.Cells[counter, 19].Value = Convert.ToInt32(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.Quantity);

                        // "ITEM QUANTITY";
                        ws.Cells[counter, 20].Value = 1;

                        // "SHIPPING DATE";
                        ws.Cells[counter, 21].Value = offer.Quotations[0].Ots[0].OtItems[0].ShippingDate;

                        // "OT PRICE"
                        ws.Cells[counter, 22].SetValue(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.UnitPrice);

                        try
                        {
                            if (isNumberFormatSupported)
                            {
                                ws.Cells[counter, 22].NumberFormat = "\"" + GeosApplication.Instance.CurrentCurrencySymbol + "\"" + " #,##,##0.00";
                            }
                        }
                        catch (Exception ex)
                        {
                            isNumberFormatSupported = false;
                            GeosApplication.Instance.Logger.Log(string.Format("Error in GenerateModuleReport() NumberFormat - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }

                        // "SITE"
                        ws.Cells[counter, 23].Value = offer.Site.ShortName;

                        //[001] "Add Options"
                        int optionColumn = 24;
                        if (selectedOptionList != null)
                        {
                            foreach (Detection det in selectedOptionList)
                            {
                                var numDetection = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.CPProduct.LstCPDetection.Where(d => d.DetectionID == det.IdDetection)
                                                .Select(s => s.NumDetections)
                                                .FirstOrDefault();

                                if (numDetection != null)
                                    if (numDetection != 0)
                                        ws.Cells[counter, optionColumn].Value = Convert.ToInt16(numDetection);
                                    else
                                        ws.Cells[counter, optionColumn].Value = "";
                                else
                                    ws.Cells[counter, optionColumn].Value = "";
                                optionColumn++;
                            }
                        }

                        counter++;
                    }

                    ws.Columns.AutoFit(1, 23);

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
        #region [rdixit][10.02.2023][GEOS2-4127]
        private void FillFamilies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillFamilies ...", category: Category.Info, priority: Priority.Low);

                FamilyList = new ObservableCollection<ModuleFamily>(CrmStartUp.GetModuleFamilies());
                FamilyList.Insert(0, new ModuleFamily() { Name = "---", IdModuleFamily = 0 });
                SelectedFamily = FamilyList.FirstOrDefault();
                GeosApplication.Instance.Logger.Log("Method FillFamilies() executed successfully", category: Category.Exception, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFamilies() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillFamilies() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillFamilies() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SelectedFamilyItemChangedAction()
        {
            try
            {
                SubFamilyList = new ObservableCollection<Data.Common.ModuleSubFamily>();                
                if (SelectedFamily.IdModuleFamily > 0 && FamilyList.FirstOrDefault(i => i.IdModuleFamily == SelectedFamily.IdModuleFamily).ModuleSubFamilies!=null)
                { 
                   SubFamilyList = new ObservableCollection<ModuleSubFamily>(FamilyList.FirstOrDefault(i => i.IdModuleFamily == SelectedFamily.IdModuleFamily).ModuleSubFamilies);         
                }
                SubFamilyList.Insert(0,new ModuleSubFamily() { NameSubfamily = "---", IdModuleSubfamily = 0 });
                SelectedSubFamily = SubFamilyList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in SelectedFamilyItemChangedAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #endregion // Methods
    }
}
