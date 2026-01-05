using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Spreadsheet;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
//using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Workbench.Views;
//using NsExcel = Microsoft.Office.Interop.Excel;

namespace Workbench.ViewModels
{
    public class AboutWindowViewModel : INotifyPropertyChanged, IDisposable, ISupportServices
    {
        #region Services

        IWorkbenchStartUp control = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return serviceContainer.GetService<ISaveFileDialogService>(); } }

        #endregion  // Services

        #region Declaration
        private ObservableCollection<GeosWorkbenchVersion> releaseNotesList;    //list of ReleaseNotes
        private string cultureInfo;
        public XmlLanguage CurrentLanguage { get; set; }
        public bool IsInit { get; set; }
        private GeosWorkbenchVersion workbenchVersionNumber;    // get  Workbench Version Number

        // Export Excel .xlsx
        IServiceContainer serviceContainer = null;
        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }

        private GeosWorkbenchVersion geosWorkbenchVersionNumber;

        private double dialogHeight;
        private double dialogWidth;

        #endregion // Declaration

        #region  public Properties

        IServiceContainer ISupportServices.ServiceContainer
        {
            get
            {
                if (serviceContainer == null)
                    serviceContainer = new ServiceContainer(this);
                return serviceContainer;
            }
        }

        public string CultureInfo
        {
            get { return cultureInfo; }
            set
            {
                cultureInfo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CultureInfo"));
            }
        }

        public ObservableCollection<GeosWorkbenchVersion> ReleaseNotesList
        {
            get { return releaseNotesList; }
            set
            {
                releaseNotesList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReleaseNotesList"));
            }
        }

        public GeosWorkbenchVersion WorkbenchVersionNumber
        {
            get { return workbenchVersionNumber; }
            set
            {
                workbenchVersionNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkbenchVersionNumber"));
            }
        }

        public GeosWorkbenchVersion GeosWorkbenchVersionNumber
        {
            get { return geosWorkbenchVersionNumber; }
            set { geosWorkbenchVersionNumber = value; }
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
        #endregion // Properties

        #region  Command

        public ICommand AboutWindowCancelButtonCommand { get; set; }
        public ICommand ExportButtonCommand { get; set; }

        #endregion  // Command

        #region Events

        public event EventHandler RequestClose;     //AboutWindow for close window

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion  // Events

        #region Constructor

        public AboutWindowViewModel()
        {
            GeosApplication.Instance.Logger.Log("Start AboutWindowViewModel Constructor", category: Category.Info, priority: Priority.Low);
            System.Windows.Forms.Screen screen = GeosApplication.Instance.GetWorkingScreenFrom();
            DialogWidth = screen.Bounds.Width - 100;
            dialogHeight = screen.Bounds.Height - 350;
            IsInit = true;

            GetReleaseNoteDetail();
            AboutWindowCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ExportButtonCommand = new RelayCommand(new Action<object>(ExportReleaseDetailsButtonCommandAction));

            GeosApplication.Instance.Logger.Log("End AboutWindowViewModel Constructor", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #region  Methods

        public void GetReleaseNoteDetail()
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Getting GetReleaseNoteDetail ", category: Category.Info, priority: Priority.Low);

                GeosWorkbenchVersionNumber = GeosApplication.Instance.GeosWorkbenchVersionNumber;

                if (GeosWorkbenchVersionNumber != null && GeosWorkbenchVersionNumber.IsBeta == 1)
                {
                    List<GeosWorkbenchVersion> releaseNotes = control.GetAllWorkbenchVersion().OrderByDescending(rls => rls.IdGeosWorkbenchVersion).ToList();                   
                    ReleaseNotesList = new ObservableCollection<GeosWorkbenchVersion>(releaseNotes.ToList());
                }
                else
                {
                    List<GeosWorkbenchVersion> releaseNotes = control.GetAllWorkbenchVersion().OrderByDescending(rls => rls.IdGeosWorkbenchVersion).ToList();
                    releaseNotes.RemoveAll(x => x.IsPublish != 1);
                    ReleaseNotesList = new ObservableCollection<GeosWorkbenchVersion>(releaseNotes.ToList());
                }
              

                //ReleaseNotesList = new ObservableCollection<GeosWorkbenchVersion>(control.GetAllWorkbenchVersion().OrderByDescending(rls => rls.IdGeosWorkbenchVersion).ToList());

                //ReleaseNotesList = new ObservableCollection<GeosWorkbenchVersion>(ReleaseNotesList.OrderByDescending(rls => rls.IdGeosWorkbenchVersion).ToList());

                GeosApplication.Instance.Logger.Log("Getting GetReleaseNoteDetail successfully ", category: Category.Info, priority: Priority.Low);
                //GeosApplication.Instance.ServerActiveMethod();
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetReleaseNoteDetail() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On GetReleaseNoteDetail Method", category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);

                if (!GeosApplication.Instance.IsServiceActive) { GeosApplication.Instance.ServerDeactiveMethod(); }
                IsInit = false;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetReleaseNoteDetail() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("End GetReleaseNoteDetail Method", category: Category.Info, priority: Priority.Low);
        }

        /// <summary>
        /// This method for export changLog
        /// </summary>
        /// <param name="obj"></param>
        private void ExportReleaseDetailsButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ExportReleaseDetailsButtonCommandAction ...", category: Category.Info, priority: Priority.Low);
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "ChangLog";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                }
                else
                {
                    if (!DXSplashScreen.IsActive)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            System.Windows.Window win = new System.Windows.Window()
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

                    SpreadsheetControl control = new SpreadsheetControl();
                    Worksheet ws = control.ActiveWorksheet;

                    if (ReleaseNotesList.Count > 0)
                    {

                        ws.Cells[0, 0].Value = "VERSION";
                        ws.Cells[0, 0].ColumnWidth = 200;
                        ws.Cells[0, 1].Value = "MODULE";
                        ws.Cells[0, 1].ColumnWidth = 500;
                        ws.Cells[0, 2].Value = "TYPE";
                        ws.Cells[0, 2].ColumnWidth = 200;
                        ws.Cells[0, 3].Value = "DESCRIPTION";
                        ws.Cells[0, 3].ColumnWidth = 1000;
                        ws.Cells[0, 4].Value = "RELEASE DATE";
                        ws.Cells[0, 4].ColumnWidth = 300;
                        ws.Range["A1:E1"].Font.Bold = true;
                        ws.Range["A1:E1"].Fill.BackgroundColor = System.Drawing.Color.Khaki;

                    }
                    int counter = 1;
                    foreach (var item1 in ReleaseNotesList)
                    {
                        int rowCounter = counter;
                        if (rowCounter <= counter)
                        {
                            ws.Cells[counter, 0].Value = item1.VersionNumber;
                            ws.Cells[counter, 4].Value = item1.ReleasedIn;
                            ws.Cells[counter, 4].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        }
                        foreach (var item2 in item1.GeosReleaseNotes)
                        {
                            ws.Cells[rowCounter, 0].Value = item1.VersionNumber;
                            if (item2.GeosModule == null)
                            {
                                ws.Cells[rowCounter, 1].Value = null;
                            }
                            else
                            {


                                ws.Cells[rowCounter, 1].Value = item2.GeosModule.Acronym; ;
                            }

                            if (item2.IdType == 0)
                                ws.Cells[rowCounter, 2].Value = "New";
                            else if (item2.IdType == 1)
                                ws.Cells[rowCounter, 2].Value = "Fixed";
                            ws.Cells[rowCounter, 3].Value = item2.Description;
                            rowCounter++;
                        }
                        if (rowCounter > counter)
                            counter = rowCounter;
                        else
                            counter++;

                    }

                    control.SaveDocument(ResultFileName);

                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AboutChangelogExportSuccessfull").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    System.Diagnostics.Process.Start(ResultFileName);
                }
                GeosApplication.Instance.Logger.Log("Method ExportReleaseDetailsButtonCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportReleaseDetailsButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        /// <summary>
        /// this mathod for close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            GeosApplication.Instance.Logger.Log("Close ", category: Category.Info, priority: Priority.Low);
            RequestClose(null, null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Environment.Exit(0);
        }

        #endregion
    }

}
