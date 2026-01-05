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
  public class EmployeeEducationDocumentViewModel:INotifyPropertyChanged
    {

        #region Services   
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private bool isInIt = false;     
        private MemoryStream employeeEdicationalPdfDoc;
        private string fileName;
        #endregion

        #region Properties    
        public MemoryStream EmployeeEdicationalPdfDoc
        {
            get { return employeeEdicationalPdfDoc; }
            set
            {
                employeeEdicationalPdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeEdicationalPdfDoc"));
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

        public EmployeeEducationDocumentViewModel()
        {

        }

        #region Methods
        public void OpenPdfByEmployeeCode(string empCode, object empRecord)
        {
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

                var objectType = empRecord.GetType();
                byte[] temp=null;
                if (objectType.Name == "EmployeeDocument")
                {
                    EmployeeDocument employeeDocument = (EmployeeDocument)empRecord;
                    FileName = employeeDocument.EmployeeDocumentFileName;
                    temp = employeeDocument.EmployeeDocumentFileInBytes;
                }
                if (objectType.Name == "EmployeeEducationQualification")
                {
                    EmployeeEducationQualification employeeEducationQualification = (EmployeeEducationQualification)empRecord;
                    FileName = employeeEducationQualification.QualificationFileName;
                    temp = employeeEducationQualification.QualificationFileInBytes;
                }
                if (objectType.Name == "EmployeeContractSituation")
                {
                    EmployeeContractSituation employeeContractSituation = (EmployeeContractSituation)empRecord;
                    FileName = employeeContractSituation.ContractSituationFileName;
                    temp = employeeContractSituation.ContractSituationFileInBytes;
                }
                if (objectType.Name == "EmployeeProfessionalEducation")
                {
                    EmployeeProfessionalEducation employeeEducationQualification = (EmployeeProfessionalEducation)empRecord;
                    FileName = employeeEducationQualification.FileName;
                    temp = employeeEducationQualification.ProfessionalFileInBytes;
                }
                if (temp == null)
                {
                    temp = HrmService.GetEmployeeDocumentFile(empCode, empRecord);
                }
                EmployeeEdicationalPdfDoc = new MemoryStream(temp);

                isInIt = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method OpenPdfByEmployeeCode()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OpenPdfByEmployeeCode()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(string.Format("File Not Found..."), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

     
#endregion
    }
}
