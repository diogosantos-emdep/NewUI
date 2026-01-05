using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
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
using System.Windows.Media;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class EmployeeDocumentViewModel : INotifyPropertyChanged
    {


        #region TaskLog

        /// <summary>
        /// [M049-36][20182210][Pdf viewer always maximized][adadibathina]
        /// </summary>
        /// 
        #endregion

        #region Services   
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private bool isInIt = false;
        private MemoryStream employeeEducationalPdfDoc;
        private string fileName;
        #endregion

        #region Properties    
        public MemoryStream EmployeeEducationalPdfDoc
        {
            get { return employeeEducationalPdfDoc; }
            set
            {
                employeeEducationalPdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeEducationalPdfDoc"));
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }

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

        public EmployeeDocumentViewModel()
        {

        }

        #region Methods
        /// <summary>
        /// [001][skale][2019-22-05][HRM][S63][GEOS2-1468] Add polyvalence section in employee profile.
        /// </summary>
        /// <param name="empCode"></param>
        /// <param name="empRecord"></param>
        /// <param name="isJobCode"></param>
        public void OpenPdfByEmployeeCode(string empCode, object empRecord, bool isJobCode = false)
        {
            //string FileName = string.Empty;
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdfByEmployeeCode()...", category: Category.Info, priority: Priority.Low);
                isInIt = true;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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

                byte[] temp = null;

                if (!isJobCode)
                {
                    if (empRecord is EmployeeDocument)
                    {
                        EmployeeDocument employeeDocument = (EmployeeDocument)empRecord;
                        FileName = employeeDocument.EmployeeDocumentFileName;
                        temp = employeeDocument.EmployeeDocumentFileInBytes;
                    }
                    else if (empRecord is EmployeeEducationQualification)
                    {
                        EmployeeEducationQualification employeeEducationQualification = (EmployeeEducationQualification)empRecord;
                        FileName = employeeEducationQualification.QualificationFileName;
                        temp = employeeEducationQualification.QualificationFileInBytes;
                    }
                    else if (empRecord is EmployeeContractSituation)
                    {
                        EmployeeContractSituation employeeContractSituation = (EmployeeContractSituation)empRecord;
                        FileName = employeeContractSituation.ContractSituationFileName;
                        temp = employeeContractSituation.ContractSituationFileInBytes;
                    }
                    else if (empRecord is EmployeeProfessionalEducation)
                    {
                        EmployeeProfessionalEducation employeeEducationQualification = (EmployeeProfessionalEducation)empRecord;
                        FileName = employeeEducationQualification.FileName;
                        temp = employeeEducationQualification.ProfessionalFileInBytes;
                    }
                    else if (empRecord is EmployeeJobDescription)
                    {
                        EmployeeJobDescription employeeJobDescription = (EmployeeJobDescription)empRecord;
                        empRecord = employeeJobDescription.JobDescription;
                        FileName = employeeJobDescription.JobDescription.JobDescriptionCode;
                    }
                    else if (empRecord is JobDescription)
                    {
                        FileName = ((JobDescription)empRecord).JobDescriptionCode;
                    }
                    // [001] added
                    else if (empRecord is EmployeePolyvalence)
                    {
                        EmployeePolyvalence employeePolyvalence = (EmployeePolyvalence)empRecord;
                        empRecord = employeePolyvalence.JobDescription;
                        FileName = employeePolyvalence.JobDescription.JobDescriptionCode;
                    }
                    //end
                }
                else
                {
                    FileName = empRecord.ToString();
                    JobDescription jobDescription = new JobDescription();
                    jobDescription.JobDescriptionCode = empRecord.ToString();
                    empRecord = jobDescription;

                }

                if (temp == null)
                {
                    temp = HrmService.GetEmployeeDocumentFile(empCode, empRecord);

                }

                EmployeeEducationalPdfDoc = new MemoryStream(temp);

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdfByEmployeeCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByEmployeeCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                //employeeEducationQualification.PdfFilePath
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method to open pdf from bytes
        /// </summary>
        /// <param name="bytesArray"></param>
        /// <param name="LeaveFileName"></param>
        public void OpenPdfFromBytes(byte[] bytesArray, string LeaveFileName)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenPdfByEmployeeCode()...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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


                FileName = LeaveFileName;
                EmployeeEducationalPdfDoc = new MemoryStream(bytesArray);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdfByEmployeeCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByEmployeeCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in OpenEmployeeEducationDocument() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("Could not find file {0}", FileName), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                throw;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenEmployeeEducationDocument()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
