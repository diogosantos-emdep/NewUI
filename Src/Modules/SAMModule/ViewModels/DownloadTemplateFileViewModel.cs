using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class DownloadTemplateFileViewModel: NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
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

        #endregion // End Of Events 
        #region Services
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion
        #region Declaration
        private ObservableCollection<Language> listLanguage;
        private Language selectedLanguage;
        private string excelFilePath = string.Empty;
        private Ots ots;
        #endregion

        #region Properties

        public Ots OTS
        {
            get { return ots; }
            set { ots = value; }
        }

        public string ExcelFilePath
        {
            get { return excelFilePath; }
            set { excelFilePath = value; }
        }

        public ObservableCollection<Language> ListLanguage
        {
            get { return listLanguage; }

            set
            {
                listLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListLanguage"));
            }
        }
        public Language SelectedLanguage
        {
            get { return selectedLanguage; }

            set
            {
                selectedLanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedLanguage"));
            }
        }
        #endregion

        #region Icommands
        public ICommand AcceptCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region Constructor
        public DownloadTemplateFileViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor DownloadTemplateFileViewModel()...", category: Category.Info, priority: Priority.Low);
                CancelCommand = new RelayCommand(new Action<object>(CloseWindow));
                AcceptCommand = new RelayCommand(new Action<object>(DownloadTemplateFile));
                GeosApplication.Instance.Logger.Log("Constructor DownloadTemplateFileViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
             }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor DownloadTemplateFileViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
}
        #endregion
        #region Methods
        private void DownloadTemplateFile(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method DownloadTemplateFile()...", category: Category.Info, priority: Priority.Low);
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
                    return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
            }

            try
            {
           
            Workbook workbook = new Workbook();
            FileStream stream = null;
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "xlsx";
            saveFile.FileName = "QCTestboard CheckList Template";
            saveFile.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.Title = "Save Testboard CheckList Template";
                if (!(Boolean)saveFile.ShowDialog())
                {
                    ExcelFilePath = string.Empty;
                }
                else
                {
                    byte[] fileBytes = GeosRepositoryServiceController.GetQCTemplate();

                    if (ots.OtItems != null && OTS.OtItems.Count() > 0)
                    {
                        ExcelFilePath = (saveFile.FileName);
                        workbook.LoadDocument(fileBytes, DocumentFormat.Xlsx);
                        foreach (OtItem item in OTS.OtItems)
                        {
                            if (workbook.Worksheets.Any(i => i.Name == item.RevisionItem.WarehouseProduct.Article.Reference))
                            {
                                workbook.Worksheets.Where(i => i.Name == item.RevisionItem.WarehouseProduct.Article.Reference).FirstOrDefault().Visible = true;


                            }
                        }
                        Worksheet wsDl = workbook.Worksheets[workbook.Worksheets.Where(i => i.Name == "RawData").FirstOrDefault().Index];
                        Int32 _Board_Date = 0;
                        Int32 _Board_Customer = 0;
                        Int32 _Board_Order = 0;
                        Int32 _Board_EmdepPlant = 0;
                        Int32 _Board_ModulesCount = 0;
                        Int32 _Language = 0;

                        using (stream = new FileStream(ExcelFilePath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            for (int s = 0; s < 100; s++)
                            {
                               
                                if ((wsDl.Cells[s, 0].Value.ToString() == "Board_Date"))
                                {
                                    _Board_Date = s;
                                }
                                else if ((wsDl.Cells[s, 0].Value.ToString() == "Board_Customer"))
                                {
                                    _Board_Customer = s;
                                }
                                else if ((wsDl.Cells[s, 0].Value.ToString() == "Board_Order"))
                                {
                                    _Board_Order = s;
                                }
                                else if ((wsDl.Cells[s, 0].Value.ToString() == "Board_EmdepPlant"))
                                {
                                    _Board_EmdepPlant = s;
                                }
                                else if ((wsDl.Cells[s, 0].Value.ToString() == "Board_ModulesCount"))
                                {
                                    _Board_ModulesCount = s;
                                }
                                else if ((wsDl.Cells[s, 0].Value.ToString() == "Language"))
                                {
                                    _Language = s;
                                }
                                else if ((wsDl.Cells[s, 0].Value.ToString() == null || wsDl.Cells[s, 0].Value.ToString() == string.Empty ))
                                {
                                    break;
                                }

                            }
                            wsDl.Cells[_Board_Date, 1].Value = OTS.DeliveryDate.Value;
                            wsDl.Cells[_Board_Customer, 1].Value = OTS.Quotation.Site.Name;
                            wsDl.Cells[_Board_Order, 1].Value = OTS.OfferCode;
                            wsDl.Cells[_Board_EmdepPlant, 1].Value = OTS.Site.Alias;
                            wsDl.Cells[_Board_ModulesCount, 1].Value = OTS.Modules;
                            wsDl.Cells[_Language, 1].Value = SelectedLanguage.TwoLetterISOLanguage;

                            workbook.SaveDocument(stream, DocumentFormat.Xlsx);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DownloadTemplateFile() Method " + ex.ToString(), category: Category.Exception, priority: Priority.Low);
            }
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            GeosApplication.Instance.Logger.Log("Method DownloadTemplateFile()...", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
        }
        private void CloseWindow(object obj)
        {
           
            RequestClose(null, null);
        }
        public void Init(Ots Ot)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                FillLanguages();
                OTS = Ot;
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Init() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to fill Languages Section
        /// </summary>
        /// <param name="company"></param>
        /// <param name="idOT"></param>
        private void FillLanguages()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillLanguages ...", category: Category.Info, priority: Priority.Low);

                ListLanguage = new ObservableCollection<Language>( WorkbenchService.GetAllLanguage().ToList());
             
                SelectedLanguage = ListLanguage.FirstOrDefault();

                GeosApplication.Instance.Logger.Log("Method FillLanguages() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillLanguages() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
           

        #endregion
    }
}