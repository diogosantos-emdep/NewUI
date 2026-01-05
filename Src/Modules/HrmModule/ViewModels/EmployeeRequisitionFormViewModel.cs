using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Modules.Hrm.Reports;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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
using System.Windows.Media;
using System.Drawing.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;
using System.Windows.Media.Imaging;
using System.IO;
using Emdep.Geos.Data.Common.Hrm;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using Emdep.Geos.UI.Validations;
using DevExpress.Charts.Model;
using DevExpress.Xpf.Editors.Helpers;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeRequisitionFormViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        #region Service
        ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion
        private LookupValue selectedType;
        private List<LookupValue> typeINOUTList;
        private bool isBusy;
        private Employee employeeDetail;
        private string employeeDeptname;
        private string error = string.Empty;
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.Today;
        private string LengthOfService = string.Empty;
        public List<LookupValue> TypeINOUTList
        {
            get { return typeINOUTList; }
            set
            {
                typeINOUTList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TypeINOUTList"));
            }
        }
        public LookupValue SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedType"));
            }
        }
        public Employee EmployeeDetail
        {
            get { return employeeDetail; }
            set
            {
                employeeDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDetail"));
            }
        }

        public string EmployeeDeptname
        {
            get { return employeeDeptname; }
            set
            {
                employeeDeptname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeDeptname"));
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

        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand AcceptButtonCommand { get; set; }
        #endregion // Commands

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

        #endregion // Events

        #region Constructor

        public EmployeeRequisitionFormViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor EmployeeRequisitionFormViewModel()...", category: Category.Info, priority: Priority.Low);
                FilltypeINOUTList();
                //  EmployeeProfileDetailViewModel employeeProfileDetailViewModel = new EmployeeProfileDetailViewModel();
                CloseWindowCommand = new DelegateCommand<object>(CloseWindowAction);
                AcceptButtonCommand = new DelegateCommand<object>(PrintERFFormCommandAction);
                GeosApplication.Instance.Logger.Log("Constructor EmployeeRequisitionFormViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor EmployeeRequisitionFormViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method
        public void Init(Employee employeeDetail, string employeeDeptName)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init....", category: Category.Info, priority: Priority.Low);

                //  FilltypeINOUTList();

                EmployeeDetail = employeeDetail;
                EmployeeDeptname = employeeDeptName;
                //[rdixit][GEOS2-5658][17.03.2025]
                CalculateLengthOfService(EmployeeDetail.EmployeeContractSituations, EmployeeDetail.EmployeeExitEvents);
                GeosApplication.Instance.Logger.Log("Method Init....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        //[rdixit][GEOS2-5658][17.03.2025]
        private void CalculateLengthOfService(List<EmployeeContractSituation> EmployeeContractSituationList,List<EmployeeExitEvent> ExitEventList)
        {
            try
            {
                //[rdixit][GEOS2-5657][11.03.2025]
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()...", category: Category.Info, priority: Priority.Low);


                // Calculate year and month difference
                if (EmployeeContractSituationList != null)
                {
                    List<EmployeeContractSituation> ContractList = EmployeeContractSituationList.Select(i => (EmployeeContractSituation)i.Clone()).OrderBy(j => j.ContractSituationStartDate).ToList();

                    if (ContractList.Count > 0)
                    {
                        var lastExitEvent = ExitEventList?.OrderByDescending(i => i.ExitDate).FirstOrDefault();

                        if (lastExitEvent?.ExitDate != null)
                        {
                            var Newcontract = ContractList.Where(i => i.ContractSituationStartDate.Value.Date > lastExitEvent.ExitDate.Value.Date).FirstOrDefault();

                            if (Newcontract == null)
                            {
                                startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                                var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                endDate = contract?.ContractSituationEndDate ?? DateTime.MinValue; // or handle null properly
                            }
                            else
                            {
                                startDate = Convert.ToDateTime(Newcontract.ContractSituationStartDate);
                                var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                                //[rdixit][GEOS2-7877][16.04.2025]
                                if (startDate > DateTime.Today)
                                    endDate = startDate;
                                else
                                    endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                            }
                        }
                        else
                        {
                            startDate = Convert.ToDateTime(ContractList.FirstOrDefault().ContractSituationStartDate);
                            var contract = ContractList.OrderByDescending(i => i.ContractSituationEndDate ?? DateTime.Today).FirstOrDefault();
                            //[rdixit][GEOS2-7877][16.04.2025]
                            if (startDate > DateTime.Today)
                                endDate = startDate;
                            else
                                endDate = (contract?.ContractSituationEndDate > DateTime.Today) ? DateTime.Today : contract?.ContractSituationEndDate ?? DateTime.Today;
                        }
                        int year = endDate.Year - startDate.Year;
                        int month = endDate.Month - startDate.Month;
                        int day = endDate.Day - startDate.Day;
                        if (day < 0)
                        {
                            month -= 1;
                            DateTime previousMonth = endDate.AddMonths(-1);
                            day += DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                        }
                        if (month < 0)
                        {
                            year -= 1;
                            month += 12;
                        }
                        LengthOfService = Convert.ToString(year) + "y  " + Convert.ToString(month) + "M";
                    }
                }
                GeosApplication.Instance.Logger.Log("Method CalculateLengthOfService()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CalculateLengthOfService()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FilltypeINOUTList()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method FilltypeINOUTList()...", category: Category.Info, priority: Priority.Low);

                //IList<LookupValue> tempList = CrmService.GetLookupValues(71);
                //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
                IList<LookupValue> tempList = HrmService.GetEnumeratedList(71);
                TypeINOUTList = new List<LookupValue>();
                TypeINOUTList = new List<LookupValue>(tempList);
                //  SelectedType = TypeINOUTList[0];
                TypeINOUTList.Insert(0, new LookupValue() { Value = "---", InUse = true });
                SelectedType = TypeINOUTList.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method FilltypeINOUTList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FilltypeINOUTList()...." + ex.Detail.ErrorMessage + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FilltypeINOUTList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Method FillDepartmentAreaList()...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Info, priority: Priority.Low);
            }
        }

        private void CloseWindowAction(object obj)
        {
            try
            {
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CloseWindowAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// [001][avpawar][GEOS2-3075][Add Exit Event in OUT ERF]
		/// [002][nsatpute][08-11-2024] HRM - Improve ERF . GEOS2-6475
        /// </summary>
        /// <param name="obj"></param>
        private void PrintERFFormCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PrintERFFormCommandAction()...", category: Category.Info, priority: Priority.Low);


                allowValidation = true;
                if (SelectedType.IdLookupValue == 0)
                {
                    error = EnableValidationAndGetError();

                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedType"));

                    if (error != null)
                    {
                        return;
                    }
                }
                //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
                else if (SelectedType.IdLookupValue == 1483 || SelectedType.IdLookupValue == 1484 || SelectedType.IdLookupValue == 1622 || SelectedType.IdLookupValue == 1623)
                {
                    #region [GEOS2-4568][rdixit][27.09.2023]
                    if (SelectedType.IdLookupValue == 1484)
                    {
                        if (EmployeeDetail.EmployeeExitEvents != null)
                        {
                            if (EmployeeDetail.EmployeeContractSituations != null)
                            {
                                DateTime? MaxStartDate = EmployeeDetail.EmployeeContractSituations.Select(j => j.ContractSituationStartDate).Max();

                                if (!(EmployeeDetail.EmployeeExitEvents.Any(i => DateTime.Compare(Convert.ToDateTime(i.ExitDate), Convert.ToDateTime(MaxStartDate)) > 0)))
                                {
                                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ERFExistEventWorningMsg").ToString()), "yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ERFExistEventWorningMsg").ToString()), "yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.OK);
                            return;
                        }
                    }
                    #endregion
                    IsBusy = true;
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
                    string plantName = "";

                    EmployeeERFReport ERFReport = new EmployeeERFReport();
                    ERFReport.ReportFooter = new ReportFooterBand();
                    ERFReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    ERFReport.xrLblReportName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 12, System.Drawing.FontStyle.Bold);
                    ERFReport.xrLblEmpCode.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpHireDate.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpPosition.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpCompanySkype.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    //ERFReport.xrLblEmpStatus.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpNativeName.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpLengthOfService.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    EmployeeDetail.LengthOfServiceString = LengthOfService; //[rdixit][GEOS2-5658][17.03.2025]
                    ERFReport.xrLblEmpContractSituation.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpGender.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpDOB.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                    ERFReport.xrLblEmpCompanyEmail.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Underline);
                    ERFReport.lblDepartmentInfo.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);

                    EmployeeEducationReport EmpEducationQualificationReport = new EmployeeEducationReport();
                    EmpEducationQualificationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    EmpEducationQualificationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    EmployeeTrainingReport EmployeeTrainingReport = new EmployeeTrainingReport();
                    EmployeeTrainingReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    EmployeeTrainingReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    EmployeeJobDescriptionReport EmpJobDescriptionReport = new EmployeeJobDescriptionReport();
                    EmpJobDescriptionReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    EmpJobDescriptionReport.xrTable2.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    EmployeeContractSituationReport EmpContratSituationReport = new EmployeeContractSituationReport();
                    EmpContratSituationReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    EmpContratSituationReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    EmployeePolyvalenceReport EmpPolyvalenceReport = new EmployeePolyvalenceReport();
                    EmpPolyvalenceReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                    EmpPolyvalenceReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                    ERFReport.lblDepartmentInfo.Text = $"Department Information : {EmployeeDeptname}".ToUpper();
                    EmployeeERF_DepartmentInfo docInfo = new EmployeeERF_DepartmentInfo();
                    List<EmployeeDepartmentSituation> data = HrmService.GetEmployeeDepartmentSituation(EmployeeDeptname);
                    if (data != null)
                    {
                        docInfo.xrChart.Titles.Clear();
                        DevExpress.XtraCharts.ChartTitle chartTitle = new DevExpress.XtraCharts.ChartTitle() { Text = $"{EmployeeDeptname} Group total {data[0].TotalActiveCadEmployees} Count" };
                        chartTitle.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 12, System.Drawing.FontStyle.Regular);
                        chartTitle.TextColor = System.Drawing.Color.Black;

                        docInfo.xrChart.Titles.Add(chartTitle);
                        
                        docInfo.xrChart.Series[0].DataSource = data;
                        docInfo.xrChart.Series[0].ArgumentDataMember = "Company";
                        docInfo.xrChart.Series[0].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                        docInfo.xrChart.Series[0].ValueDataMembers.AddRange("ActiveEmployees");

                        docInfo.xrChart.Series[1].DataSource = data;
                        docInfo.xrChart.Series[1].ArgumentDataMember = "Company";
                        docInfo.xrChart.Series[1].Label.LineVisibility =  DevExpress.Utils.DefaultBoolean.False;
                        docInfo.xrChart.Series[1].Label.BackColor = System.Drawing.Color.Transparent;
                        docInfo.xrChart.Series[1].ValueDataMembers.AddRange("ActiveCadEmployees");
                        docInfo.xrChart.Series[1].Name = employeeDeptname;

                        docInfo.xrChart.Series[2].DataSource = data;
                        docInfo.xrChart.Series[2].ArgumentDataMember = "Company";
                        docInfo.xrChart.Series[2].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                        docInfo.xrChart.Series[2].ValueDataMembers.AddRange("TotalActiveCadEmployees");
                        docInfo.xrChart.Series[2].Name = $"{employeeDeptname} Group Total";

                        docInfo.xrChart.Series[3].DataSource = data;
                        docInfo.xrChart.Series[3].ArgumentDataMember = "Company";
                        docInfo.xrChart.Series[3].LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
                        docInfo.xrChart.Series[3].ValueDataMembers.AddRange("CadAverageYearsService");
                    }
                    ERFReport.xrDepartmentInfoChart.ReportSource = docInfo;
                    CultureInfo CultureInfo = Thread.CurrentThread.CurrentCulture;
                    ERFReport.xrLabel18.Text = GeosApplication.Instance.ServerDateTime.Date.ToString("d", CultureInfo);
                    

                    Employee PrintEmployee = new Employee();
                    PrintEmployee = EmployeeDetail;
                    //rajashri GEOS2-4668
                    // PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.OrderByDescending(x => x.IsMainJobDescription).First();
                    PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate != null || x.JobDescriptionEndDate == null || x.IsMainVisibleGreyImageVisible == Visibility.Visible).First();//rajashri GEOS2-4668
                    if (EmployeeDetail.EmployeeJobDescriptions.Any(a => a.JobDescriptionEndDate == null ||a.IsMainVisible == Visibility.Visible))
                    {
                        if (EmployeeDetail.EmployeeJobDescriptions.Any(a => a.JobDescriptionEndDate == null && a.IsMainVisible == Visibility.Visible))
                        {
                            PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.Where(a => a.JobDescriptionEndDate == null && 
                            a.IsMainVisible == Visibility.Visible).OrderByDescending(x => x.JobDescriptionUsage).First();
                        }
                        else
                        {
                            PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate == null).FirstOrDefault();//rajashri GEOS2-4668
                        }
                    }
                    else
                    {
                        PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.OrderByDescending(x => x.JobDescriptionEndDate ?? DateTime.MaxValue)
                            .ThenByDescending(x => x.IsMainJobDescription).ThenByDescending(x => x.JobDescriptionUsage).FirstOrDefault();//[GEOS2-6006][rdixit][30.07.2024]
                    }
                    if (PrintEmployee.EmployeeContractSituations.Count > 0)
                    {
                        //[001]
                        EmployeeContractSituation contractSituation = PrintEmployee.EmployeeContractSituations.OrderByDescending(i => i.ContractSituationStartDate).FirstOrDefault();
                        if (contractSituation != null)
                        {
                            DateTime? startDate = contractSituation.ContractSituationStartDate;

                            DateTime zeroTime = new DateTime(1, 1, 1);
                            //(cpatil) Solved bug if contract is not started yet set length of service =0 
                            if (contractSituation.ContractSituationStartDate.Value.Date >= GeosApplication.Instance.ServerDateTime.Date)
                            {
                                PrintEmployee.LengthOfService = 0;
                            }
                            else
                            {
                                TimeSpan span = DateTime.Now - (DateTime)contractSituation.ContractSituationStartDate;
                                int Los = (zeroTime + span).Year - 1;
                                PrintEmployee.LengthOfService = Los;
                            }

                            ERFReport.xrLblEmpContractSituation.Text = contractSituation.ContractSituation.Name;
                        }
                    }

                    PrintEmployee.ProfileImageInBytes = GeosRepositoryServiceController.GetEmployeesImage(PrintEmployee.EmployeeCode);
                    if (PrintEmployee.ProfileImageInBytes != null)
                    {
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(PrintEmployee.ProfileImageInBytes))
                        {
                            BitmapImage image = new BitmapImage();

                            image.BeginInit();
                            image.StreamSource = ms;
                            image.EndInit();
                            Bitmap img = new Bitmap(image.StreamSource);
                            ERFReport.xrPbEmpProfileImage.Image = img;
                        }
                    }
                    else
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            BitmapEncoder enc = new BmpBitmapEncoder();
                            enc.Frames.Add(BitmapFrame.Create(new BitmapImage(new Uri("pack://application:,,,/Emdep.Geos.Modules.Hrm;component/Assets/Images/No_Photo.png", UriKind.RelativeOrAbsolute))));
                            enc.Save(outStream);
                            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                            Bitmap image = new Bitmap(bitmap);
                            ERFReport.xrPbEmpProfileImage.Image = image;
                        }
                    }
                    if (PrintEmployee.EmployeeProfessionalContacts.Count > 0)
                    {
                        if (PrintEmployee.EmployeeProfessionalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 88).ToList().Count > 0)
                        {
                            ERFReport.xrLblEmpCompanyEmail.Text = PrintEmployee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 88).EmployeeContactValue;
                        }

                        if (PrintEmployee.EmployeeProfessionalContacts.Where(x => x.EmployeeContactType.IdLookupValue == 87).ToList().Count > 0)
                        {
                            ERFReport.xrLblEmpCompanySkype.Text = PrintEmployee.EmployeeProfessionalContacts.FirstOrDefault(x => x.EmployeeContactType.IdLookupValue == 87).EmployeeContactValue;
                        }

                    }
                    //[rdixit][GEOS2-5658][17.03.2025]
                    ERFReport.xrLblEmpHireDate.Text = startDate.ToShortDateString();//PrintEmployee.EmployeeContractSituations.Min(x => x.ContractSituationStartDate.Value).ToShortDateString();
                    List<Employee> empq = new List<Employee>();
                    empq.Add(PrintEmployee);
                    List<string> companyNameList = new List<string>();

                    if (PrintEmployee.EmployeeJobDescriptions.Count > 0)
                    {
                        companyNameList = PrintEmployee.EmployeeJobDescriptions.Select(x => x.Company.Alias).Distinct().ToList();
                        int k = 740;
                        foreach (string item in companyNameList)
                        {
                            XRLabel label = new XRLabel() { Text = item };
                            label.WidthF = 60;
                            label.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, ERFReport.Font.Size);
                            ERFReport.Bands[BandKind.TopMargin].Controls.Add(label);
                            label.LocationF = new System.Drawing.PointF(k, 15);
                            label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            label.ForeColor = System.Drawing.Color.White;
                            label.BackColor = System.Drawing.Color.Black;
                            k = k - 70;
                        }
                        //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
                        if (SelectedType.IdLookupValue == 1483 || SelectedType.IdLookupValue == 1622 || SelectedType.IdLookupValue == 1623)
                        {
                            XRLabel label = new XRLabel() { Text = SelectedType.Value };
                            label.WidthF = 60;
                            label.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, ERFReport.Font.Size);
                            ERFReport.Bands[BandKind.TopMargin].Controls.Add(label);
                            label.LocationF = new System.Drawing.PointF(k, 15);
                            label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            label.ForeColor = System.Drawing.Color.White;
                            //label.BackColor = System.Drawing.Color.Green;
                            //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
                            label.BackColor = System.Drawing.ColorTranslator.FromHtml(SelectedType.HtmlColor);
                            ERFReport.xrLblGeneralInfo.Text = "EDUCATION";
                            ERFReport.xrSubreport1.ReportSource = EmpEducationQualificationReport;
                            EmpEducationQualificationReport.bindingSource1.DataSource = PrintEmployee.EmployeeEducationQualifications;

                            ERFReport.xrLblPC.Text = "CONTRACT SITUATION";
                            ERFReport.xrContractPolyvalSubRpt.ReportSource = EmpContratSituationReport;
                            EmpContratSituationReport.bindingSource1.DataSource = PrintEmployee.EmployeeContractSituations;
                        }
                        else
                        {
                            XRLabel label = new XRLabel() { Text = SelectedType.Value };
                            label.WidthF = 60;
                            label.Font = new System.Drawing.Font(GeosApplication.Instance.FontFamilyAsPerTheme.Source, ERFReport.Font.Size);
                            ERFReport.Bands[BandKind.TopMargin].Controls.Add(label);

                            label.LocationF = new System.Drawing.PointF(k, 15);
                            label.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                            label.ForeColor = System.Drawing.Color.White;
                            // label.BackColor = System.Drawing.Color.Red;
                            //shubham[skadam] GEOS2-3899 Add support for lookupvalues fields (inuse, backcolor, icon) in the ERF report 27 Sep 2022
                            label.BackColor = System.Drawing.ColorTranslator.FromHtml(SelectedType.HtmlColor);

                            ERFReport.xrLblGeneralInfo.Text = "COMPANY TRAINING";
                            ERFReport.xrSubreport1.ReportSource = EmployeeTrainingReport;
                            EmployeeTrainingReport.bindingSource1.DataSource = PrintEmployee.EmployeeProfessionalEducations;



                            ERFReport.xrLblPC.Text = "POLYVALENCE";
                            ERFReport.xrContractPolyvalSubRpt.ReportSource = EmpPolyvalenceReport;
                            EmpPolyvalenceReport.bindingSource1.DataSource = PrintEmployee.EmployeePolyvalences;

                            //[001] start
                            if (PrintEmployee.ExitDate != null)
                            {

                                EmployeeExitEventReport EmpExitEventReport = new EmployeeExitEventReport();
                                EmpExitEventReport.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                                //EmpExitEventReport.xrTable1.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);

                                //EmpExitEventReport.xrExitEventExitLabel.Text = "EXIT";
                                //EmpExitEventReport.xrExitEventExitLabel.WidthF = 60;
                                //EmpExitEventReport.xrExitEventExitLabel.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                                EmpExitEventReport.xrExitEventExitLabel.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 400F);

                                // EXIT Information Panel
                                //XRPanel panel = new XRPanel();
                                //EmpExitEventReport.xrExitEventPanel.Borders = DevExpress.XtraPrinting.BorderSide.All;
                                //panel.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 15F);
                                // panel.SizeF = new System.Drawing.SizeF(794.9995F, 100.75018F);
                                EmpExitEventReport.xrExitEventPanel.SizeF = new System.Drawing.SizeF(794.9999F, 80.8334F);
                                EmpExitEventReport.xrExitEventPanel.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 415F);
                                // Date Label
                                //EmpExitEventReport.xrExitEventDateLabel.Text = "Date:";
                                //EmpExitEventReport.xrExitEventDateLabel.WidthF = 60;
                                //EmpExitEventReport.xrExitEventDateLabel.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                                //exitDateLabel.LocationF = new DevExpress.Utils.PointFloat(6.708002F, 7F);
                                //EmpExitEventReport.xrExitEventExitDateValueLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;

                                // XRLabel label4 = new XRLabel();
                                EmpExitEventReport.xrExitEventExitDateValueLabel.Text = PrintEmployee.ExitDate.Value.Date.ToString("d", CultureInfo);
                                EmpExitEventReport.xrExitEventExitDateValueLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                                EmpExitEventReport.xrExitEventExitDateValueLabel.WidthF = 60;
                                EmpExitEventReport.xrExitEventExitDateValueLabel.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                                //label4.LocationF = new DevExpress.Utils.PointFloat(55.708002F, 7F);
                                //label4.Borders = DevExpress.XtraPrinting.BorderSide.None;

                                // Remark Label
                                EmpExitEventReport.xrExitEventRemarkLabel.Text = "Remarks:";
                                EmpExitEventReport.xrExitEventRemarkLabel.WidthF = 60;
                                EmpExitEventReport.xrExitEventRemarkLabel.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                                //label2.LocationF = new DevExpress.Utils.PointFloat(300.708002F, 7F);
                                //label2.Borders = DevExpress.XtraPrinting.BorderSide.None;

                                //XRLabel label5 = new XRLabel();
                                EmpExitEventReport.xrExitEventRemarkValueLabel.Text = PrintEmployee.ExitRemarks;
                                EmpExitEventReport.xrExitEventRemarkValueLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                                EmpExitEventReport.xrExitEventRemarkValueLabel.WidthF = 60;
                                EmpExitEventReport.xrExitEventRemarkValueLabel.SizeF = new System.Drawing.SizeF(405.208F, 35.83335F);
                                //label5.LocationF = new DevExpress.Utils.PointFloat(360.708002F, 7F);
                                //label5.Borders = DevExpress.XtraPrinting.BorderSide.None;

                                // Reason Label
                                EmpExitEventReport.xrExitEventReasonLabel.Text = "Reason:";
                                EmpExitEventReport.xrExitEventReasonLabel.WidthF = 60;
                                EmpExitEventReport.xrExitEventReasonLabel.SizeF = new System.Drawing.SizeF(105.208F, 18.83335F);
                                //label3.LocationF = new DevExpress.Utils.PointFloat(6.708002F, 50F);
                                //label3.Borders = DevExpress.XtraPrinting.BorderSide.None;

                                //XRLabel label6 = new XRLabel();
                                if (PrintEmployee.ExitReason != null)
                                    EmpExitEventReport.xrExitEventReasonValueLabel.Text = PrintEmployee.ExitReason.Value.ToString();

                                EmpExitEventReport.xrExitEventReasonValueLabel.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Regular);
                                EmpExitEventReport.xrExitEventReasonValueLabel.WidthF = 60;
                                EmpExitEventReport.xrExitEventReasonValueLabel.SizeF = new System.Drawing.SizeF(710.208F, 35.83335F);

                                ERFReport.xrExitEventSubRpt.ReportSource = EmpExitEventReport;
                                ERFReport.xrExitEventSubRpt.Band.Controls.Add(EmpExitEventReport.xrExitEventExitLabel);
                                ERFReport.xrExitEventSubRpt.Band.Controls.Add(EmpExitEventReport.xrExitEventPanel);
                                ERFReport.xrExitLabel.Text = "";


                                //rajashri
                                //// TEST Panel
                                //ERFReport.xrPanel2.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 550F);

                                //// Approval Label
                                //ERFReport.xrLabel2.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 790F);

                                //// MD Label
                                //ERFReport.xrLabel4.LocationF = new DevExpress.Utils.PointFloat(72.31157F, 790F);
                                //ERFReport.xrLabel12.LocationF = new DevExpress.Utils.PointFloat(72.31157F, 815F);

                                //// Option1 Label
                                //ERFReport.xrLabel5.LocationF = new DevExpress.Utils.PointFloat(244.624F, 790F);
                                //ERFReport.xrLabel13.LocationF = new DevExpress.Utils.PointFloat(244.624F, 815F);

                                //// Option2 Label
                                //ERFReport.xrLabel7.LocationF = new DevExpress.Utils.PointFloat(427.1448F, 790F);
                                //ERFReport.xrLabel14.LocationF = new DevExpress.Utils.PointFloat(427.1448F, 815F);

                                //// PM Label
                                //ERFReport.xrLabel8.LocationF = new DevExpress.Utils.PointFloat(609.8948F, 790F);
                                //ERFReport.xrLabel15.LocationF = new DevExpress.Utils.PointFloat(609.8948F, 815F);
                            }
                            //[001] End
                        }
                    }
                    ERFReport.xrLabel19.Text = plantName;

                    ERFReport.DataSource = empq;

                    // EmpEducationQualificationReport.bindingSource1.DataSource = PrintEmployee.EmployeeEducationQualifications;
                    EmpJobDescriptionReport.bindingSource1.DataSource = PrintEmployee.EmployeeJobDescriptions;
                    //    EmpContratSituationReport.bindingSource1.DataSource = PrintEmployee.EmployeeContractSituations;
                    // EmployeeTrainingReport.bindingSource1.DataSource = PrintEmployee.EmployeeProfessionalEducations;
                    ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

                    GeosAppSetting GeosAppSetting = CrmService.GetGeosAppSettings(48);
                    if (GeosAppSetting != null)
                        ERFReport.xrLabel11.Multiline = true;
                    //string[] words = GeosAppSetting.DefaultValue.Split('(');
                    //string result = string.Join("\r\n", words);
                    ERFReport.xrLabel11.Text = GeosAppSetting.DefaultValue;

                    GeosAppSetting GeosAppSettingERFDocumentName = CrmService.GetGeosAppSettings(93);
                    if (GeosAppSettingERFDocumentName != null)
                        ERFReport.xrLabel9.Text = GeosAppSettingERFDocumentName.DefaultValue;

                    ERFReport.xrJobDescSubRpt.ReportSource = EmpJobDescriptionReport;

                    //   ERFReport.xrContractPolyvalSubRpt.ReportSource = EmpContratSituationReport;
                    //[15-04-2019] [sdesai] (#66045) Name of EmployeeERFReport and automatic email
                    //set DefaultFileName and Email.Subject
                    XRLabel label1 = new XRLabel();
                    //label1.LocationF= new DevExpress.Utils.PointFloat(6.70836F, 100F);
                    XRPanel panel1 = new XRPanel();
                    //if (PrintEmployee.ExitDate != null)
                    //{
                    //    label1.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 500F);
                    //    panel1.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 500F);
                    //}
                    // panel1.LocationF = new DevExpress.Utils.PointFloat(6.70836F, 110F);
                    //ReportFooterBand band = new ReportFooterBand();
                    //band.PrintAtBottom = true;
                    //band.Controls.Add(label1);
                    //band.Controls.Add(panel1);

                    //ERFReport.Band.Controls.Add(band);
                    EmployeeJobDescription tempEmployeeJobDescription;
                   // PrintEmployee.EmployeeJobDescription = EmployeeDetail.EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate != null || x.JobDescriptionEndDate == null || x.IsMainVisibleGreyImageVisible == Visibility.Visible).First();//rajashri GEOS2-4668
                   

                    List<EmployeeJobDescription> tempEmployeeJobDescriptionList = PrintEmployee.EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate == null).ToList();

                    if (tempEmployeeJobDescriptionList.Count == 0)
                    {

                        if (tempEmployeeJobDescriptionList.Any(a => a.JobDescriptionEndDate == null || a.IsMainVisible == Visibility.Visible))
                        {
                            if (tempEmployeeJobDescriptionList.Any(a => a.JobDescriptionEndDate == null && a.IsMainVisible == Visibility.Visible))
                            {
                                tempEmployeeJobDescription = tempEmployeeJobDescriptionList
              .Where(a => a.JobDescriptionEndDate == null && a.IsMainVisible == Visibility.Visible).OrderByDescending(x => x.JobDescriptionUsage)
              .FirstOrDefault();
                            }
                            else
                            {
                                tempEmployeeJobDescription = tempEmployeeJobDescriptionList.Where(x => x.JobDescriptionEndDate == null).OrderByDescending(x => x.JobDescriptionUsage).FirstOrDefault();//rajashri GEOS2-4668
                            }

                        }

                        else
                        {
                            tempEmployeeJobDescription = tempEmployeeJobDescriptionList.OrderByDescending(x => x.JobDescriptionUsage).FirstOrDefault();//rajashri GEOS2-4668
                        }
                    }
                    else
                    {
                        tempEmployeeJobDescription = tempEmployeeJobDescriptionList.OrderByDescending(x => x.JobDescriptionUsage).FirstOrDefault();//rajashri GEOS2-4668
                    }
                            string FileName = "EmployeeERFReport";

                    if (tempEmployeeJobDescription != null)
                    {
                        if (SelectedType.IdLookupValue == 1483 || SelectedType.IdLookupValue == 1622 || SelectedType.IdLookupValue == 1623)
                            FileName = "ERF_" + SelectedType.Value + "_" + tempEmployeeJobDescription.Company.Alias + "_" + tempEmployeeJobDescription.JobDescription.JobDescriptionCode + "_" + EmployeeDetail.EmployeeCode + "_" + GeosApplication.Instance.ServerDateTime.ToString("yyyyMMdd");

                        //FileName = "ERF_"+ SelectedType.Value +"_" + tempEmployeeJobDescription.Company.Alias + "_" + tempEmployeeJobDescription.JobDescription.JobDescriptionCode + "_" + Regex.Replace(GeosApplication.Instance.ServerDateTime.ToString("d", CultureInfo), @"[^0-9a-zA-Z]+", "");
                        else
                            FileName = "ERF_" + SelectedType.Value + "_" + tempEmployeeJobDescription.Company.Alias + "_" + tempEmployeeJobDescription.JobDescription.JobDescriptionCode + "_" + EmployeeDetail.EmployeeCode + "_" + GeosApplication.Instance.ServerDateTime.ToString("yyyyMMdd");
                        //FileName = "ERF_" + SelectedType.Value+ "_" + tempEmployeeJobDescription.Company.Alias + "_" + tempEmployeeJobDescription.JobDescription.JobDescriptionCode + "_" + Regex.Replace(GeosApplication.Instance.ServerDateTime.ToString("d", CultureInfo), @"[^0-9a-zA-Z]+", "");
                    }
                    ERFReport.ExportOptions.PrintPreview.DefaultFileName = FileName;
                    ERFReport.ExportOptions.Email.Subject = FileName;


                    DocumentPreviewWindow window = new DocumentPreviewWindow() { WindowState = WindowState.Maximized };

                    window.PreviewControl.DocumentSource = ERFReport;
                    ERFReport.CreateDocument();
                    window.Show();
                    //window.Activate();
                    IsBusy = false;

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Method PrintERFFormCommandAction....executed successfully", category: Category.Info, priority: Priority.Low);

                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method PrintERFFormCommandAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }

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
                string error = me[BindableBase.GetPropertyName(() => SelectedType)];


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

                string selectedType = BindableBase.GetPropertyName(() => SelectedType);

                if (columnName == selectedType)
                    return ERFValidation.GetErrorMessage(selectedType, SelectedType);

                return null;
            }
        }
        /// <summary>
        /// If any feild is of Information has error set isInformationError = true;
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns


        #endregion


    }
    #endregion
}


