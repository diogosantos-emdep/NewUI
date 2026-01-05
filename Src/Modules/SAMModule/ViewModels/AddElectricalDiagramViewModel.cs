using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.Helper;
using System.Collections.ObjectModel;
using System.Data;
using Emdep.Geos.Modules.SAM.Views;
using System.Windows.Media;
using DevExpress.Xpf.Grid;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.File;
using Emdep.Geos.Utility;
using DevExpress.Compression;
using DevExpress.Xpf.Editors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraSpreadsheet.Commands;
using System.Management;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Data.Common.SAM;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static DevExpress.Mvvm.DataAnnotations.PredefinedMasks;
namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class AddElectricalDiagramViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        #region Service
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISAMService SAMService = new SAMServiceController("localhost:6699");
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

        #endregion

        #region Declarations
        private double dialogHeight;
        private double dialogWidth;
        string myFilterString;
        private bool isSAMEditElectricalDiagram;
        private ImageSource attachmentImage;
        private ObservableCollection<ElectricalDiagram> listElectricalDiagram;
        private ElectricalDiagram selectedElectricalDiagram;
        byte[] structureElectricalDiagramPdf;
        private String electricalDiagramFileName;
        private String electricalDiagramFilePath;
        private bool isPreview;
        private MemoryStream pdfDoc;
        private ElectricalDiagram selectedObject;
        private long idDrawing;
        private long idRevisionItem;
        Visibility isPdfVisible;
        private bool isElectricalDiagramInsert;
        private List<ElectricalDiagram> selectedList;
        #endregion

        #region Properties
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
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        public bool IsSAMEditElectricalDiagram
        {
            get { return isSAMEditElectricalDiagram; }
            set
            {

                isSAMEditElectricalDiagram = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSAMEditElectricalDiagram"));

            }
        }
        public ImageSource AttachmentImage
        {
            get
            {
                return attachmentImage;
            }

            set
            {
                attachmentImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentImage"));
            }
        }
        public ObservableCollection<ElectricalDiagram> ListElectricalDiagram
        {
            get { return listElectricalDiagram; }
            set { listElectricalDiagram = value; OnPropertyChanged(new PropertyChangedEventArgs("ListElectricalDiagram")); }
        }
        public ElectricalDiagram SelectedElectricalDiagram
        {
            get { return selectedElectricalDiagram; }
            set
            {
                selectedElectricalDiagram = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedElectricalDiagram"));
            }
        }
        public byte[] StructureElectricalDiagramPdf
        {
            get
            {
                return structureElectricalDiagramPdf;
            }
            set
            {
                structureElectricalDiagramPdf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("structureElectricalDigramPdf"));
            }
        }

        public string ElectricalDiagramFileName
        {
            get { return electricalDiagramFileName; }
            set { electricalDiagramFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("ElectricalDiagramFileName")); }
        }

        public string ElectricalDiagramFilePath
        {
            get { return electricalDiagramFilePath; }
            set { electricalDiagramFilePath = value; OnPropertyChanged(new PropertyChangedEventArgs("ElectricalDiagramFilePath")); }
        }
        public bool IsPreview
        {
            get
            {
                return isPreview;
            }
            set
            {
                isPreview = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPreview"));
                FillAddElectricalDiagramGrid();
             
            }
        }
        public MemoryStream PdfDoc
        {
            get { return pdfDoc; }
            set
            {
                pdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfDoc"));
            }
        }
        public ElectricalDiagram SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
                
            }
        }
        bool isCancel = false;
        public bool IsCancel
        {
            get { return isCancel; }
            set
            {
                isCancel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCancal"));
            }
        }
        public long IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDrawing"));
            }
        }
        public long IdRevisionItem
        {
            get { return idRevisionItem; }
            set
            {
                idRevisionItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdRevisionItem"));
            }
        }
        public Visibility IsPdfVisible

        {
            get
            {
                return isPdfVisible;
            }

            set
            {
                isPdfVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPdfVisible"));
            }

        }
        public bool isRecordAdded
        {
            get
            {
                return isElectricalDiagramInsert;
            }
            set
            {
                isElectricalDiagramInsert = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsElectricalDiagramInsert"));
            

            }
        }
        public List<ElectricalDiagram> SelectedList
        {
            get
            {
                return selectedList;
            }
            set
            {
                selectedList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedList"));
            }
        }
        #endregion

        #region ICommands
        public ICommand CommandAddElectricalDiagramsCancel { get; set; }

        public ICommand CurrentItemChangedCommand { get; set; }
        public ICommand AddElectricalDigramViewAcceptButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddElectricalDiagramViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddElectricalDiagramViewModel ...", category: Category.Info, priority: Priority.Low);

                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 130;
                CommandAddElectricalDiagramsCancel = new RelayCommand(new Action<object>(CommandAddElectricalDiagramsCancelAction));
                CurrentItemChangedCommand = new RelayCommand(new Action<object>(CurrentItemChangedCommandAction));
                AddElectricalDigramViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddElectricalDigramViewAcceptButtonCommandAction));
                MyFilterString = string.Empty;
                Init(IdDrawing,IdRevisionItem);
                IsPdfVisible = Visibility.Hidden;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor AddElectricalDiagramViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddElectricalDiagramViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        #endregion

        #region Method
        public void Init(long SelectedIdDrawing, long SelectedIdRevisionItem)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                IdDrawing = SelectedIdDrawing;
                IdRevisionItem = SelectedIdRevisionItem;
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
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
                AttachmentImage = null;

                List<ElectricalDiagram> tmpList = SAMService.GetElectricalDiagram();
                ListElectricalDiagram = new ObservableCollection<ElectricalDiagram>(tmpList);
                SelectedElectricalDiagram = ListElectricalDiagram.FirstOrDefault();
                MyFilterString = string.Empty;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
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
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommandAddElectricalDiagramsCancelAction(object obj)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandAddElectricalDiagramsCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CurrentItemChangedCommandAction(object obj)
        {

            try
            {
                    if (SelectedObject != null)
                    {
                        if (StructureElectricalDiagramPdf != null)
                        {
                            IsPdfVisible = Visibility.Hidden;
                        if (IsPreview == false)
                        {
                            IsPdfVisible = Visibility.Hidden;
                        }
                        if (IsPreview == true)
                        {
                            IsPdfVisible = Visibility.Hidden;
                        }
                        if (IsPdfVisible == Visibility.Hidden)
                        {
                            IsPreview = false;
                        }
                        if (IsPdfVisible == Visibility.Visible)
                        {
                            IsPreview = false;
                            IsPdfVisible = Visibility.Hidden;
                        }
                    }
                       else
                        {
                             if (IsPreview == false)
                             {
                                  IsPdfVisible = Visibility.Hidden;
                             }
                            if (IsPreview == true)
                            {
                                IsPdfVisible = Visibility.Hidden;
                            }
                            if (IsPdfVisible == Visibility.Hidden)
                            {
                                IsPreview = false;
                             }
                    }

                }
                    GeosApplication.Instance.Logger.Log("Method ElectricalDiagramPdfIconClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            #endregion

    }
        private void FillAddElectricalDiagramGrid()
        {

            try
            {
                if (SelectedObject != null && isPreview == true)
                {
                   // SAMService = new SAMServiceController("localhost:6699");

                    StructureElectricalDiagramPdf = SAMService.GetElectricalDiagramsFileImageInBytes(SelectedObject.FilePath);
                    if (StructureElectricalDiagramPdf != null)
                    {
                        PdfDoc = new MemoryStream(StructureElectricalDiagramPdf);
                    }
                }
                if (isPreview == true && StructureElectricalDiagramPdf != null)
                {
                    IsPdfVisible = Visibility.Visible;

                }
                GeosApplication.Instance.Logger.Log("Method FillAddElectricalDiagramGrid()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method FillAddElectricalDiagramGrid() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }


        }
        // [nsatpute][13-05-2025][GEOS2-6728]
        private void AddElectricalDigramViewAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddElectricalDigramViewAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (ListElectricalDiagram != null && ListElectricalDiagram.Count > 0)
                {
                    SelectedList = new List<ElectricalDiagram>(ListElectricalDiagram.Where(x => x.IsSelected && x.Parent != x.Key && x.ParentElectricalDiagramID != null).ToList());
                    foreach (var item in SelectedList)
                    {
                        item.IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                        item.Datetime = GeosApplication.Instance.ServerDateTime;
                        item.Comments = string.Format(System.Windows.Application.Current.FindResource("AddElctricalDiagramChangeLog").ToString(), item.Name, IdDrawing);
                    }
                    SAMCommon.Instance.SelectedListCommon = SelectedList;
                    isRecordAdded = true;
                }                
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddElectricalDigramViewAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddElectricalDigramViewAcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }
} 
