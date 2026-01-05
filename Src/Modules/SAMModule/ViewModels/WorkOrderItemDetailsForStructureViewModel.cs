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
using Emdep.Geos.Data.Common.SAM;
using DevExpress.Utils.Extensions;
namespace Emdep.Geos.Modules.SAM.ViewModels
{
    public class WorkOrderItemDetailsForStructureViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWorkbenchStartUp WorkbenchService = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");
        #endregion // Services
        #region Declaration
        private double dialogHeight;
        private double dialogWidth;
        private Company otSite;
        private Ots ot;
        private OtItem selectedOtItem;
        Ots objOT = new Ots();
        private MaximizedElementPosition maximizedElementPosition;
        private ObservableCollection<ColumnSAM> columns;
        public DataRowView selectedObject;
        public FileDetail selectedFile;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private List<Detection> detections;
        private ObservableCollection<OTAttachment> listAttachment;
        private DataTable dttable;
        private DataTable dttableCopy;
        private string infoTooltipBackColor;
        byte[] structureDrawingPdf;//[pramod.misal][GEOS2-5407][04.07.2024]
        //Rahul[Rgadhave][GEOS2-5407][Date:04-07-2024]
        private bool showCommentsFlyout;
        private bool isBusy;
        private String fileName;
        private List<object> otAttachmentList;
        private string uniqueFileName;
        private OTAttachment attachment;
        private string fileNameString;
        private ObservableCollection<OTAttachment> listAddedAttachment;
        private string remark;
        private OTAttachment selectedAttachment;
        private ObservableCollection<FileDetail> electricalDiagramFileList;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        byte[] structureElectricalDiagramPdf;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private String electricalDiagramFileName;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private String electricalDiagramFilePath;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private FileDetail selectedElectricalDiagramFile;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        public List<FileDetail> newTempElectricalDiagramFileList;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private bool isAddButtonEnabled;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private bool isDeleteButtonEnabled;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private bool isSave;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private long idDrawing;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private long parentElectricalDiagramId;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private List<OtItem> oTItems;  // [GEOS2-6728][pallavi.kale][14-04-2025]                
        private long idElectricalDiagram;  // [GEOS2-6728][pallavi.kale][14-04-2025]
        private Visibility electricalDiagramVisibility;
        #endregion // Declaration
        #region Properties
        //Rahul[Rgadhave][GEOS2-5407][Date:04-07-2024]
        public bool ShowCommentsFlyout
        {
            get { return showCommentsFlyout; }
            set
            {
                showCommentsFlyout = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowCommentsFlyout"));
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
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged(new PropertyChangedEventArgs("FileName")); }
        }
        public List<object> OTAttachmentList
        {
            get { return otAttachmentList; }
            set
            {
                otAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OTAttachmentList"));
            }
        }
        public string UniqueFileName
        {
            get { return uniqueFileName; }
            set { uniqueFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("UniqueFileName")); }
        }
        public OTAttachment Attachment
        {
            get { return attachment; }
            set { attachment = value; OnPropertyChanged(new PropertyChangedEventArgs("Attachment")); }
        }
        public string FileNameString
        {
            get { return fileNameString; }
            set { fileNameString = value; OnPropertyChanged(new PropertyChangedEventArgs("FileNameString")); }
        }
        protected ISaveFileDialogService SaveFileDialogService
        {
            get
            {
                return this.GetService<ISaveFileDialogService>();
            }
        }
        public ObservableCollection<OTAttachment> ListAddedAttachment
        {
            get { return listAddedAttachment; }
            set
            {
                listAddedAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAddedAttachment"));
            }
        }
        public string Remark
        {
            get
            {
                return remark;
            }
            set
            {
                remark = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Remark"));
            }
        }
        public virtual bool DialogResult { get; set; }
        public virtual string ResultFileName { get; set; }
        public string GuidCode { get; set; }
        public string InfoTooltipBackColor
        {
            get
            {
                return infoTooltipBackColor;
            }
            set
            {
                infoTooltipBackColor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InfoTooltipBackColor"));
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
        public Company OtSite
        {
            get { return otSite; }
            set
            {
                otSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtSite"));
            }
        }
        public Ots OT
        {
            get { return ot; }
            set
            {
                ot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OT"));
            }
        }
        public DataTable Dttable
        {
            get { return dttable; }
            set
            {
                dttable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Dttable"));
            }
        }
        public DataTable DttableCopy
        {
            get { return dttableCopy; }
            set
            {
                dttableCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DttableCopy"));
            }
        }
        public DataRowView SelectedObject
        {
            get { return selectedObject; }
            set
            {
                selectedObject = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedObject"));
                CurrentItemChangedCommand.Execute(selectedObject);
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public FileDetail SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedFile"));
            }
        }
        public OtItem SelectedOtItem
        {
            get { return selectedOtItem; }
            set
            {
                selectedOtItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtItem"));
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
        public ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public List<Detection> Detections
        {
            get { return detections; }
            set
            {
                detections = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Detections"));
            }
        }
        public ObservableCollection<OTAttachment> ListAttachment
        {
            get { return listAttachment; }
            set
            {
                listAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListAttachment"));
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
        public OTAttachment SelectedAttachment
        {
            get { return selectedAttachment; }
            set
            {
                selectedAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAttachment"));
            }
        }
        //[pramod.misal][GEOS2-5407][04.07.2024]
        public byte[] StructureDrawingPdf
        {
            get
            {
                return structureDrawingPdf;
            }
            set
            {
                structureDrawingPdf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StructureDrawingPdf"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public ObservableCollection<FileDetail> ElectricalDiagramFileList
        {
            get { return electricalDiagramFileList; }
            set
            {
                electricalDiagramFileList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ElectricalDiagramFileList"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
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
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public string ElectricalDiagramFileName
        {
            get { return electricalDiagramFileName; }
            set { electricalDiagramFileName = value; OnPropertyChanged(new PropertyChangedEventArgs("ElectricalDiagramFileName")); }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public string ElectricalDiagramFilePath
        {
            get { return electricalDiagramFilePath; }
            set { electricalDiagramFilePath = value; OnPropertyChanged(new PropertyChangedEventArgs("ElectricalDiagramFilePath")); }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public FileDetail SelectedElectricalDiagramFile
        {
            get { return selectedElectricalDiagramFile; }
            set
            {
                selectedElectricalDiagramFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedElectricalDiagramFile"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public List<FileDetail> NewTempElectricalDiagramFileList
        {
            get { return newTempElectricalDiagramFileList; }
            set
            {
                newTempElectricalDiagramFileList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewTempElectricalDiagramFileList"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public bool IsAddButtonEnabled
        {
            get { return isAddButtonEnabled; }
            set
            {
                isAddButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAddButtonEnabled"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public bool IsDeleteButtonEnabled
        {
            get { return isDeleteButtonEnabled; }
            set
            {
                isDeleteButtonEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleteButtonEnabled"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public long IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDrawing"));
            }
        }
        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public long ParentElectricalDiagramId
        {
            get { return parentElectricalDiagramId; }
            set
            {
                parentElectricalDiagramId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentElectricalDiagramId"));
            }
        }

        // [GEOS2-6728][pallavi.kale][14-04-2025]
        public long IdElectricalDiagram
        {
            get { return idElectricalDiagram; }
            set
            {
                idElectricalDiagram = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdElectricalDiagram"));
            }
        }
        //[nsatpute][14-05-2025][GEOS2-6728]
        public Visibility ElectricalDiagramVisibility
        {
            get { return electricalDiagramVisibility; }
            set
            {
                electricalDiagramVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ElectricalDiagramVisibility"));
            }
        }
        #endregion
        #region Commands
        public ICommand CommandWorkOrderForStructureViewCancel { get; set; }
        public ICommand EditWorkOrderForStructureViewAcceptButtonCommand { get; set; }
        public ICommand ImageClickCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand PdfIconClickCommand { get; set; } //[pramod.misal][GEOS2-5407][04.07.2024]
        //Attachment
        //Rahul[Rgadhave][GEOS2-5407][Date:04-07-2024]
        public ICommand DownloadFileCommand { get; set; }
        public ICommand ChooseFileCommand { get; set; }
        public ICommand UploadFileCommand { get; set; }
        public ICommand DeleteFileCommand { get; set; }
        public ICommand AddNewElectricalDiagramCommand { get; set; }  // [GEOS2-6728][pallavi.kale][14-04-2025]
        public ICommand ElectricalDiagramPdfIconClickCommand { get; set; }  // [GEOS2-6728][pallavi.kale][14-04-2025]
        public ICommand CurrentItemChangedCommand { get; set; }  // [GEOS2-6727][pallavi.kale][14-04-2025]
        public ICommand DeleteElectricalDiagramFileCommand { get; set; }  // [GEOS2-6729][pallavi.kale][14-04-2025]
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
        public WorkOrderItemDetailsForStructureViewModel()
        {
            try
            {
                IsCancel = false;
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsForStructureViewModel()...", category: Category.Info, priority: Priority.Low);
                DialogHeight = System.Windows.SystemParameters.WorkArea.Height - 95;
                DialogWidth = System.Windows.SystemParameters.WorkArea.Width - 100;
                CommandWorkOrderForStructureViewCancel = new DelegateCommand<object>(CommandWorkOrderForStructureViewCancelAction);
                EditWorkOrderForStructureViewAcceptButtonCommand = new RelayCommand(new Action<object>(EditWorkOrderForStructureViewAcceptButtonCommandAction));
                ImageClickCommand = new DelegateCommand<object>(ImageClickCommandAction);
                //[pramod.misal][GEOS2-5407][04.07.2024]
                PdfIconClickCommand = new DelegateCommand<object>(PdfIconClickCommandAction);
                DownloadFileCommand = new DelegateCommand<object>(DownloadFileCommandAction);
                ChooseFileCommand = new RelayCommand(new Action<object>(BrowseFile));
                UploadFileCommand = new DelegateCommand<object>(UploadFileCommandAction);
                OTAttachmentList = new List<object>();
                ListAttachment = new ObservableCollection<OTAttachment>();
                DeleteFileCommand = new DelegateCommand<object>(DeleteAttachmentFile);
                CurrentItemChangedCommand = new RelayCommand(new Action<object>(CurrentItemChangedCommandAction));  // [GEOS2-6728][pallavi.kale][14-04-2025]
                AddNewElectricalDiagramCommand = new RelayCommand(new Action<object>(AddNewElectricalDiagram));  // [GEOS2-6727][pallavi.kale][14-04-2025]
                DeleteElectricalDiagramFileCommand = new RelayCommand(new Action<object>(DeleteElectricalDiagramFileCommandAction));  // [GEOS2-6729][pallavi.kale][14-04-2025]
                ElectricalDiagramPdfIconClickCommand = new RelayCommand(new Action<object>(ElectricalDiagramPdfIconClickCommandAction));  // [GEOS2-6728][pallavi.kale][14-04-2025]


                //EditCommand = new RelayCommand(new Action<object>(EditAction));
                GeosApplication.Instance.Logger.Log("Constructor WorkOrderItemDetailsForStructureViewModel()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor WorkOrderItemDetailsForStructureViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
        #region method
        //[001][cpatil][02-08-2021][GEOS2-2906] Include the “STRUCTURE” orders in the Orders grid
        public void Init(Ots ot)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                MaximizedElementPosition = MaximizedElementPosition.Right;
                OtSite = ot.Site;
                //[001]
                //[pramod.misal][GEOS2-5327][22.02.2024]
                //SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == ot.Site.IdCompany);
                //SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                //  OT = SAMService.GetStructureWorkOrderByIdOt_V2170(ot.IdOT, ot.Site);
                #region Service comiited for GEOS2-5472
                /// [rgadhave][18-06-2024][GEOS2-5583]
                // OT = SAMService.GetStructureWorkOrderByIdOt_V2530(ot.IdOT, ot.Site);
                #endregion
                //[pramod.misal][GEOS2-5472][04.07.2024]
                //SAMService = new SAMServiceController("localhost:6699");
                //OT = SAMService.GetStructureWorkOrderByIdOt_V2540(ot.IdOT, ot.Site);

                OT = SAMService.GetStructureWorkOrderByIdOt_V2640(ot.IdOT, ot.Site);  // [GEOS2-6727][pallavi.kale][14-04-2025]
                OT.OfferCode = ot.OfferCode;
                OT.Modules = ot.Modules;
                Remark = OT.Observations;
                // [GEOS2-6728][pallavi.kale][14-04-2025]
                if (OT.OtItems != null && OT.OtItems.Count > 0)
                {
                    long idDrawing = OT.OtItems[0].RevisionItem?.IdDrawing ?? 0;
                    // Now you can use idDrawing
                }
                SelectedOtItem = OT.OtItems.FirstOrDefault();
                //  LoadData();
                AddDataTableColumns();
                FillDataTable();
                FillListAttachment(ot.Site, ot.IdOT);
                //Ots objOT = new Ots();
                objOT.IdOT = ot.IdOT;
                objOT.Site = ot.Site;
                //[nsatpute][14-05-2025][GEOS2-6728]
                if (ot.Quotation.IdDetectionsTemplate == 24)
                    ElectricalDiagramVisibility = Visibility.Visible;
                else
                    ElectricalDiagramVisibility = Visibility.Collapsed;

                //set info tooltip back color
                GeosAppSetting GeosAppSetting = WorkbenchService.GetGeosAppSettings(37);
                if (GeosAppSetting != null)
                    InfoTooltipBackColor = GeosAppSetting.DefaultValue;
                // [GEOS2-6728][pallavi.kale][14-04-2025]
                if (GeosApplication.Instance.ActiveUser.UserPermissions.Any(up => up.IdPermission == 145 && up.Permission.IdGeosModule == 9))
                {
                    IsAddButtonEnabled = true;
                    IsDeleteButtonEnabled = true;
                }

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        //[nsatpute][18-02-2025][GEOS2-6997]
        public void InitRemoteService(string serviceProviderUrl)
        {
            if (!string.IsNullOrEmpty(serviceProviderUrl))
            {
                SAMService = new SAMServiceController(serviceProviderUrl);
            }
        }
        private void LoadData()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method LoadData()...", category: Category.Info, priority: Priority.Low);
                FillOtItemList();
                GeosApplication.Instance.Logger.Log("Method LoadData()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method LoadData()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void FillOtItemList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillOtItemList()...", category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.Logger.Log("Method FillOtItemList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillOtItemList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to add columns Dynamically to ITEMs grid
        /// </summary>
        private void AddDataTableColumns()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns ...", category: Category.Info, priority: Priority.Low);
                Columns = new ObservableCollection<Emdep.Geos.UI.Helper.ColumnSAM>()
                {
                     new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Item",HeaderText="#Item", SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Description, AllowCellMerge=false, Width=60,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                     new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Reference",HeaderText="Reference", SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Description, AllowCellMerge=false, Width=300,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                     new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName="Type",HeaderText="Type", SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Description, AllowCellMerge=false, Width=150,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  },
                     new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName= "Comment",HeaderText=" ", SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Comment, AllowCellMerge=false, Width=45,AllowEditing=false,Visible=true,IsVertical= false,FixedWidth=false  }
                };
                Dttable = new DataTable();
                Dttable.Columns.Add("Item", typeof(string));
                Dttable.Columns.Add("Reference", typeof(string));
                Dttable.Columns.Add("Comment", typeof(string));
                Dttable.Columns.Add("Type", typeof(string));
                Dttable.Columns.Add("ShowComment", typeof(bool));
                Dttable.Columns.Add("IdOTItem", typeof(Int64));
                Dttable.Columns.Add("IdDrawing", typeof(Int64));  // [GEOS2-6728][pallavi.kale][14-04-2025]
                Dttable.Columns.Add("IdRevisionItem", typeof(Int64));  // [GEOS2-6728][pallavi.kale][14-04-2025]
                //Detection List to be add here 
                Detections = new List<Detection>();
                int detcolumn = 0;
                foreach (Detection item in OT.LstDetection)
                {
                    Detections.Insert(detcolumn, new Detection() { Name = item.Name, ColumnNo = detcolumn, IdDetection = item.IdDetection, NameToShow = item.Name });
                    detcolumn = detcolumn + 1;
                }
                for (int i = 0; i < Detections.Count; i++)
                {
                    if (!Dttable.Columns.Contains(Detections[i].Name))
                    {
                        Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM()
                        {
                            FieldName = Detections[i].Name.ToString(),
                            HeaderText = Detections[i].Name.ToString(),
                            SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Array,
                            AllowCellMerge = false,
                            Width = 45,
                            AllowEditing = false,
                            FixedWidth = false,
                            Visible = true,
                            IsVertical = true
                        });
                        Dttable.Columns.Add(Detections[i].Name.ToString(), typeof(string));
                    }
                }
                Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName = "Quantity", HeaderText = "Quantity", SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Description, AllowCellMerge = false, Width = 80, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = false });
                Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM() { FieldName = "Situation", HeaderText = "Situation", SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Description, AllowCellMerge = false, Width = 150, AllowEditing = false, Visible = true, IsVertical = false, FixedWidth = false });
                Dttable.Columns.Add("Quantity", typeof(decimal));
                Dttable.Columns.Add("Situation", typeof(string));
                //[pramod.misal][GEOS2-5407][04.07.2024]
                Columns.Add(new Emdep.Geos.UI.Helper.ColumnSAM
                {
                    FieldName = "Drawing",
                    HeaderText = "Drawing",
                    SettingsWOStructureItemGrid = SettingsTypeWOStructureItemsGrid.Drawing,
                    AllowCellMerge = false,
                    Width = 45,
                    AllowEditing = false,
                    Visible = true,
                    IsVertical = true,
                    FixedWidth = false
                });
                Dttable.Columns.Add("Drawing", typeof(bool));
                GeosApplication.Instance.Logger.Log("Method AddDataTableColumns executed Successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddDataTableColumns() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to fill Data into the column dynamically 
        /// </summary>
        private void FillDataTable()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);
                Dttable.Rows.Clear();
                DttableCopy = Dttable.Copy();
                foreach (OtItem item in OT.OtItems)
                {
                    DataRow dr = DttableCopy.NewRow();
                    dr["IdOTItem"] = item.IdOTItem;
                    dr["Item"] = item.RevisionItem.NumItem;
                    dr["Reference"] = item.RevisionItem.CPProduct.Reference;
                    dr["Type"] = item.RevisionItem.CPProduct.ProductTypeName;
                    dr["Comment"] = item.RevisionItem.InternalComment;
                    dr["ShowComment"] = item.ShowComment;
                    dr["Quantity"] = item.RevisionItem.Quantity;
                    dr["Situation"] = item.Status.Name;
                    dr["IdDrawing"] = item.RevisionItem.IdDrawing;  // [GEOS2-6728][pallavi.kale][14-04-2025]
                    dr["IdRevisionItem"] = item.RevisionItem.IdRevisionItem;  // [GEOS2-6728][pallavi.kale][14-04-2025]
                    // Only show the icon if IdDrawing is neither null nor 0
                    long? id = item.RevisionItem.IdDrawing;
                    if (id != null)
                    {
                        dr["Drawing"] = true;
                    }
                    else
                    {
                        dr["Drawing"] = false;
                    }
                    try
                    {
                        foreach (CPDetection itemCPDetection in item.RevisionItem.CPProduct.LstCPDetection)
                        {
                            if (DttableCopy.Columns[itemCPDetection.DetectionName.ToString()].ToString() == itemCPDetection.DetectionName.ToString())
                            {
                                var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == itemCPDetection.DetectionName.ToString().Trim().ToUpper());
                                int indexc = Columns.IndexOf(column);
                                Columns[indexc].Visible = true;
                                dr[itemCPDetection.DetectionName.ToString()] = itemCPDetection.NumDetections;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in FillDataTable() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                    DttableCopy.Rows.Add(dr);
                }
                Dttable = DttableCopy;
                Dttable.AcceptChanges();
                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to fill Attachments Section
        /// </summary>
        /// <param name="company"></param>
        /// <param name="idOT"></param>
        private void FillListAttachment(Company company, long idOT)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillListAttachment ...", category: Category.Info, priority: Priority.Low);
                //[pramod.misal][GEOS2-5327][05.03.2024]
                SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == company.IdCompany);
                SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                ListAttachment = new ObservableCollection<OTAttachment>(SAMService.GetOTAttachment(company, idOT).ToList());
                //[pramod.misal][GEOS2-5327][05.03.2024]
                SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == company.IdCompany);
                foreach (OTAttachment items in ListAttachment)
                {
                    ImageSource imageObj = FileExtensionToFileIcon.FindIconForFilename(items.FileName, true);
                    items.AttachmentImage = imageObj;
                }
                GeosApplication.Instance.Logger.Log("Method FillListAttachment() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillListAttachment() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CommandWorkOrderForStructureViewCancelAction(object obj)
        {
            try
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CommandWorkOrderForStructureViewCancelAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Dispose()
        {
        }
        private void ImageClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                //  OtItem otItem = (OtItem)obj;
                if (obj == null) return;
                DataRowView drparameter = (System.Data.DataRowView)obj;
                DataRow drRow = drparameter.Row;
                if (drRow != null)
                {
                    Int64 IdOTItem = Convert.ToInt64(drRow.ItemArray[5].ToString());
                    try
                    {
                        GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction()...", category: Category.Info, priority: Priority.Low);
                        Dttable.Rows.Clear();
                        DttableCopy = Dttable.Copy();
                        foreach (OtItem item in OT.OtItems)
                        {
                            DataRow dr = DttableCopy.NewRow();
                            dr["Item"] = item.RevisionItem.NumItem;
                            dr["IdOTItem"] = item.IdOTItem;
                            dr["Reference"] = item.RevisionItem.CPProduct.Reference;
                            dr["Type"] = item.RevisionItem.CPProduct.ProductTypeName;
                            dr["Comment"] = item.RevisionItem.InternalComment;
                            if (item.IdOTItem != IdOTItem && item.ShowComment == true)
                            {
                                dr["ShowComment"] = false;
                            }
                            if (item.IdOTItem == IdOTItem)
                            {
                                dr["ShowComment"] = !item.ShowComment;
                            }
                            dr["Quantity"] = item.RevisionItem.Quantity;
                            dr["Situation"] = item.Status.Name;
                            try
                            {
                                foreach (CPDetection itemCPDetection in item.RevisionItem.CPProduct.LstCPDetection)
                                {
                                    if (DttableCopy.Columns[itemCPDetection.DetectionName.ToString()].ToString() == itemCPDetection.DetectionName.ToString())
                                    {
                                        var column = Columns.FirstOrDefault(c => c.FieldName.Trim().ToUpper() == itemCPDetection.DetectionName.ToString().Trim().ToUpper());
                                        int indexc = Columns.IndexOf(column);
                                        Columns[indexc].Visible = true;
                                        dr[itemCPDetection.DetectionName.ToString()] = itemCPDetection.NumDetections;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                GeosApplication.Instance.Logger.Log("Get an error in ImageClickCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                            }
                            DttableCopy.Rows.Add(dr);
                        }
                        Dttable = DttableCopy;
                        Dttable.AcceptChanges();
                        OT.OtItems.Where(a => a.IdOTItem != IdOTItem && a.ShowComment == true).ToList().ForEach(a => { a.ShowComment = false; });
                        OT.OtItems.Where(a => a.IdOTItem == IdOTItem).ToList().ForEach(a => { a.ShowComment = !a.ShowComment; });
                        GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ImageClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method ImageClickCommandAction....executed ", category: Category.Info, priority: Priority.Low);
        }
        private void EditWorkOrderForStructureViewAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditWorkOrderForStructureViewAcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                IsCancel = true;
                string attachmentfiles = null;
                if (!string.IsNullOrEmpty(Remark))
                    Remark = Remark.Trim();
                if (ListAddedAttachment != null && ListAddedAttachment.Count > 0)
                {
                    ListAddedAttachment.ToList().ForEach(x => x.AttachmentImage = null);
                    bool isupload = false;
                    if (ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).Count() > 0)
                        isupload = UploadOTAttachmentFiles(OT.Quotation.Year.ToString(), OT.Quotation.Code);               // Upload attachment on Server method
                    if (isupload == true || ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete).Count() > 0 || (OT.Observations != Remark))
                    {
                        if (ListAttachment == null || ListAttachment.Count == 0)
                        {
                            attachmentfiles = null;
                        }
                        else
                        {
                            attachmentfiles = string.Join(";", ListAttachment.Select(i => i.FileName).ToList());
                        }
                        //[pramod.misal][GEOS2-5327][06-03-2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        // SAMService = new SAMServiceController("localhost:6699");
                        SAMService.UpdateOTQuality(objOT.Site, OT.IdOT, GeosApplication.Instance.ActiveUser.IdUser, attachmentfiles, GuidCode, OT.Quotation.Year.ToString(), OT.Quotation.Code, ((ListAddedAttachment == null || ListAddedAttachment.Count == 0) ? null : ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete).ToList()), OT.Observations, Remark);
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        ListAddedAttachment = new ObservableCollection<OTAttachment>();
                    }
                }
                else if (OT.Observations != Remark)
                {
                    //[pramod.misal][GEOS2-5327][06-03-2024]
                    SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                    SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    // SAMService = new SAMServiceController("localhost:6699");
                    SAMService.UpdateOTQuality(objOT.Site, OT.IdOT, GeosApplication.Instance.ActiveUser.IdUser, attachmentfiles, GuidCode, OT.Quotation.Year.ToString(), OT.Quotation.Code, ((ListAddedAttachment == null || ListAddedAttachment.Count == 0) ? null : ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Delete).ToList()), OT.Observations, Remark);
                    //[pramod.misal][GEOS2-5327][05.03.2024]
                    SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                }

                //SAMService = new SAMServiceController("localhost:6699");

                // [nsatpute][14-04-2025][GEOS2-6729]
                bool isInserted = SAMService.AddEditDeleteElectricalDiagramForIdDrawing(OT.OtItems);

                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("WorkOrderEditViewEditedSuccessfully").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method EditWorkOrderForStructureViewAcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method EditWorkOrderForStructureViewAcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public static Icon IconFromFilePath(string filePath)
        {
            var result = (Icon)null;
            try
            {
                result = Icon.ExtractAssociatedIcon(filePath);
            }
            catch (System.Exception)
            {
                // swallow and return nothing. You could supply a default Icon here as well
            }
            return result;
        }
        #endregion
        //Rahul[Rgadhave][GEOS2-5407][Date:04-07-2024]
        /// <summary>
        /// Method for to browse and add document into the list. 
        /// </summary>
        /// <param name="obj"></param>
        private void BrowseFile(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFile() ...", category: Category.Info, priority: Priority.Low);
            //DXSplashScreen.Show<SplashScreenView>();
            //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            if (!DXSplashScreen.IsActive)
            {
                DXSplashScreen.Show<SplashScreenView>();
            }
            IsBusy = true;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".*";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    FileInfo file = new FileInfo(dlg.FileName);
                    FileName = file.FullName;
                    var newFileList = OTAttachmentList != null ? new List<object>(OTAttachmentList) : new List<object>();
                    UniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Attachment = new OTAttachment();
                    Attachment.FileType = file.Extension;
                    Attachment.FilePath = file.FullName;
                    if (file.Name.Contains("."))
                    {
                        string[] a = file.Name.Split('.');
                        FileNameString = a[0];
                    }
                    else
                    {
                        FileNameString = file.Name;
                    }
                    Attachment.FileName = file.Name;
                    Attachment.OriginalFileName = FileNameString;
                    Attachment.SavedFileName = UniqueFileName + file.Extension;
                    Attachment.UploadedIn = GeosApplication.Instance.ServerDateTime;
                    Attachment.FileSizeInInt = file.Length;
                    //Attachment.FileSize = (Attachment.FileSizeInInt / 1024).ToString()+"KB";
                    if (Attachment.FileSizeInInt >= 1024 * 1024)
                    {
                        // If the file size is 1 MB or larger, display in MB
                        Attachment.FileSize = Math.Ceiling((double)Attachment.FileSizeInInt / (1024 * 1024)).ToString("0") + " MB";
                    }
                    else
                    {
                        // If the file size is less than 1 MB, display in KB
                        Attachment.FileSize = Math.Ceiling((double)Attachment.FileSizeInInt / 1024).ToString("0") + " KB";
                    }
                    Attachment.FileExtension = file.Extension;
                    Attachment.FileUploadName = file.Name;
                    Attachment.IsUploaded = true;
                    var theIcon = IconFromFilePath(FileName);
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\Images\";
                    if (theIcon != null)
                    {
                        // Save it to disk, or do whatever you want with it.
                        if (!Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }
                        if (!File.Exists(tempPath + UniqueFileName + file.Extension + ".ico"))
                        {
                            using (var stream = new System.IO.FileStream(tempPath + UniqueFileName + file.Extension + ".ico", System.IO.FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                theIcon.Save(stream);
                                stream.Close();
                                stream.Dispose();
                            }
                        }
                        theIcon.Dispose();
                    }
                    // useful to get icon end process of temp. used imgage 
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(tempPath + UniqueFileName + file.Extension + ".ico", UriKind.RelativeOrAbsolute);
                    image.EndInit();
                    Attachment.AttachmentImage = image;
                    // not allow to add same files
                    List<OTAttachment> fooList = newFileList.OfType<OTAttachment>().ToList();
                    if (!fooList.Any(x => x.OriginalFileName == Attachment.OriginalFileName))
                    {
                        newFileList.Add(Attachment);
                    }
                    OTAttachmentList = newFileList;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFile() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void EditAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditAction()", category: Category.Info, priority: Priority.Low);
                if (obj == null) return;
                EditItemView editItemView = new EditItemView();
                EditItemViewModel editItemViewModel = new EditItemViewModel();
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                EventHandler handle = delegate { editItemView.Close(); };
                editItemViewModel.RequestClose += handle;
                editItemViewModel.IsNew = false;
                editItemView.DataContext = editItemViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                DevExpress.Xpf.Grid.TreeListView tableView = (DevExpress.Xpf.Grid.TreeListView)obj;
                var ownerInfo = (tableView as FrameworkElement);
                OtItem tempOtItem = (OtItem)tableView.DataControl.CurrentItem;
                editItemView.Owner = Window.GetWindow(ownerInfo);
                editItemViewModel.EditInitItem(tempOtItem, OT, OtSite);
                editItemView.ShowDialog();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method EditAction()..", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method for Upload File
        /// </summary>
        public void UploadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);

                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show<SplashScreenView>();
                }

                List<FileInfo> FileDetail = new List<FileInfo>();

                if (OTAttachmentList != null && OTAttachmentList.Count > 0)
                {
                    foreach (OTAttachment item in OTAttachmentList)
                    {
                        // Check if a file with the same name already exists in ListAttachment
                        var existingAttachment = ListAttachment?.FirstOrDefault(x =>
                            x.FileName != null &&
                            x.FileName.Equals(item.FileName, StringComparison.OrdinalIgnoreCase));

                        if (existingAttachment != null)
                        {
                            
                            existingAttachment.TransactionOperation = ModelBase.TransactionOperations.Modify;
                            
                            existingAttachment.FilePath = item.FilePath;
                            existingAttachment.FileSize = item.FileSize;
                            existingAttachment.FileType = item.FileType;
                            
                            

                            var existingInAdded = ListAddedAttachment?.FirstOrDefault(x =>
                                x.FileName != null &&
                                x.FileName.Equals(item.FileName, StringComparison.OrdinalIgnoreCase));

                            if (existingInAdded != null)
                            {
                                existingInAdded.FilePath = item.FilePath;
                                existingInAdded.FileSize = item.FileSize;
                                existingInAdded.FileType = item.FileType;
                                existingInAdded.TransactionOperation = ModelBase.TransactionOperations.Modify;
                            }
                            else
                            {                                
                                item.TransactionOperation = ModelBase.TransactionOperations.Modify;
                                if (ListAddedAttachment == null)
                                    ListAddedAttachment = new ObservableCollection<OTAttachment>();
                                ListAddedAttachment.Add(item);
                            }

                            // Log the replacement
                            GeosApplication.Instance.Logger.Log($"Replaced existing file: {item.FileName}",
                                category: Category.Info, priority: Priority.Low);
                        }
                        else
                        {
                            // Add new attachment
                            item.TransactionOperation = ModelBase.TransactionOperations.Add;
                            ListAttachment.Add(item);

                            if (ListAddedAttachment == null)
                                ListAddedAttachment = new ObservableCollection<OTAttachment>();
                            ListAddedAttachment.Add(item);

                            // Log the addition
                            GeosApplication.Instance.Logger.Log($"Added new file: {item.FileName}",
                                category: Category.Info, priority: Priority.Low);
                        }
                    }
                }

                OTAttachmentList = new List<object>();
                IsBusy = false;

                // Optional: Update UI or notify about changes
                RaisePropertyChanged(nameof(ListAttachment));
                RaisePropertyChanged(nameof(ListAddedAttachment));

                GeosApplication.Instance.Logger.Log("Method UploadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Close();
                }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// Method to download the file
        /// </summary>
        /// <param name="obj"></param>
        public void DownloadFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() ...", category: Category.Info, priority: Priority.Low);
                //DXSplashScreen.Show<SplashScreenView>();
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                //if (!DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Show<SplashScreenView>();
                //}
                IsBusy = true;
                bool isDownload = false;
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["AttachmentsFileDownloadMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    OTAttachment attachmentObject = (OTAttachment)obj;
                    SaveFileDialogService.DefaultExt = attachmentObject.FileExtension;
                    SaveFileDialogService.DefaultFileName = attachmentObject.FileName;
                    SaveFileDialogService.Filter = "All Files|*.*";
                    SaveFileDialogService.FilterIndex = 1;
                    DialogResult = SaveFileDialogService.ShowDialog();
                    if (!DialogResult)
                    {
                        ResultFileName = string.Empty;
                    }
                    else
                    {
                        IsBusy = true;
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
                                //WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                                win.Topmost = false;
                                return win;
                            }, x =>
                            {
                                return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                            }, null, null);
                        }
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        SAMService = new SAMServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                        Byte[] byteObj = SAMService.GetOTAttachmentInBytes(attachmentObject.FileName, attachmentObject.QuotationYear, attachmentObject.QuotationCode);
                        //[pramod.misal][GEOS2-5327][05.03.2024]
                        SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                        ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                        isDownload = SaveData(ResultFileName, byteObj);
                    }
                    if (isDownload)
                    {
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadSuccess").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        GeosApplication.Instance.Logger.Log("Method DownloadFileCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
                        IsBusy = false;
                    }
                    else
                    {
                        if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AttachmentsFileDownloadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        IsBusy = false;
                    }
                }
                else
                {
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                    IsBusy = false;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log("Get an error in DownloadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        protected bool SaveData(string FileName, byte[] Data)
        {
            BinaryWriter Writer = null;
            string Name = FileName;
            try
            {
                // Create a new stream to write to the file
                Writer = new BinaryWriter(File.OpenWrite(Name));
                // Writer raw data
                Writer.Write(Data);
                Writer.Flush();
                Writer.Close();
            }
            catch
            {
                //...
                return false;
            }
            return true;
        }
        public bool UploadOTAttachmentFiles(string year, string quotationCode)
        {
            bool isupload = false;
            try
            {
                GeosApplication.Instance.Logger.Log("Method UploadFile() ...", category: Category.Info, priority: Priority.Low);
                //DXSplashScreen.Show<SplashScreenView>();
                //if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                if (!DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Show<SplashScreenView>();
                }
                IsBusy = true;
                FileUploadReturnMessage fileUploadReturnMessage = new FileUploadReturnMessage();
                List<FileInfo> FileDetail = new List<FileInfo>();
                FileOTAttachmentUploader otAttachmentFileUploader = new FileOTAttachmentUploader();
                otAttachmentFileUploader.FileUploadName = GUIDCode.GUIDCodeString();
                GuidCode = otAttachmentFileUploader.FileUploadName;
                otAttachmentFileUploader.Year = year;
                otAttachmentFileUploader.QuotationCode = quotationCode;
                if (ListAddedAttachment != null && ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).Count() > 0)
                {
                    foreach (OTAttachment fs in ListAddedAttachment.Where(i => i.TransactionOperation == ModelBase.TransactionOperations.Add).ToList())
                    {
                        FileInfo file = new FileInfo(fs.FilePath);
                        fs.AttachmentImage = null;
                        FileDetail.Add(file);
                    }
                    otAttachmentFileUploader.FileByte = ConvertZipToByte(FileDetail, otAttachmentFileUploader.FileUploadName);
                    GeosApplication.Instance.Logger.Log("Getting Upload ot Attachment FileUploader Zip File ", category: Category.Info, priority: Priority.Low);
                    //[pramod.misal][GEOS2-5327][06-03-2024]
                    SAMCommon.Instance.SelectedPlantOwner = SAMCommon.Instance.PlantOwnerList.FirstOrDefault(s => s.IdCompany == objOT.Site.IdCompany);
                    GeosRepositoryServiceController = new GeosRepositoryServiceController((SAMCommon.Instance.PlantOwnerList != null && SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl != null) ? SAMCommon.Instance.SelectedPlantOwner.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    // GeosRepositoryServiceController = new GeosRepositoryServiceController("localhost:6699");
                    fileUploadReturnMessage = GeosRepositoryServiceController.UploaderOTAttachmentZipFile(otAttachmentFileUploader);
                    GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
                    GeosApplication.Instance.Logger.Log("Getting Upload ot Attachment FileUploader Zip File successfully", category: Category.Info, priority: Priority.Low);
                }
                if (fileUploadReturnMessage.IsFileUpload == true)
                {
                    isupload = true;
                    IsBusy = false;
                    string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
                    if (!string.IsNullOrEmpty(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    GeosApplication.Instance.Logger.Log("Method UploadFile() executed successfully", category: Category.Info, priority: Priority.Low);
                }
                else
                {
                    IsBusy = false;
                    //CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ActivityFileUploadFailed").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFile() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in UploadFileCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            return isupload;
        }
        private byte[] ConvertZipToByte(List<FileInfo> filesDetail, string GuidCode)
        {
            byte[] filedetails = null;
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\";
            string tempfolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\TempFolder\";
            if (!Directory.Exists(tempfolderPath))
            {
                System.IO.Directory.CreateDirectory(tempfolderPath);
            }
            try
            {
                GeosApplication.Instance.Logger.Log("add files into zip", category: Category.Info, priority: Priority.Low);
                using (ZipArchive archive = new ZipArchive())
                {
                    if (filesDetail.Count > 0)
                    {
                        for (int i = 0; i < filesDetail.Count; i++)
                        {
                            if (!File.Exists(tempfolderPath + filesDetail[i].Name))
                            {
                                System.IO.File.Copy(filesDetail[i].FullName, tempfolderPath + filesDetail[i].Name);
                            }
                            string s = tempfolderPath + filesDetail[i].Name;
                            archive.AddFile(s, @"/");
                            // filesDetail[i].FilePath = s;
                        }
                        archive.Save(tempPath + GuidCode + ".zip");
                        filedetails = File.ReadAllBytes(tempPath + GuidCode + ".zip");
                    }
                }
                GeosApplication.Instance.Logger.Log("zip created successfully", category: Category.Info, priority: Priority.Low);
                return filedetails;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On ConvertZipToByte Method", category: Category.Exception, priority: Priority.Low);
                DeleteTempFolder();
                return filedetails;
            }
        }
        private void DeleteTempFolder()
        {
            string tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + GuidCode + @".zip";
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
        //[pramod.misal][GEOS2-5407][04.07.2024]
        private void PdfIconClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method PdfIconClickCommandAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                //SAMService = new SAMServiceController("localhost:6699");
                RevisionItem rev = OT.OtItems.FirstOrDefault(x => x.IdOTItem == Convert.ToInt64(((System.Data.DataRowView)obj).Row.ItemArray[5])).RevisionItem;
                if (rev.IdDrawing == null)
                {
                    CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (rev != null)
                {
                    //[pramod.misal][GEOS2-5407][04.07.2024]
                    //SAMService = new SAMServiceController("localhost:6699");
                    //[pramod.misal][GEOS2-6366][02.09.2024]                                 
                    SAMService = new SAMServiceController(OtSite.ServiceProviderUrl);
                    //END [GEOS2-6366]OtSite
                    structureDrawingPdf = SAMService.GetSolidworksDrawingFileImageInBytes(rev.DrawingPath, rev.SolidworksDrawingFileName);
                    if (structureDrawingPdf != null)
                    {
                        PdfStructureDrawingView PdfStructureDrawingView = new PdfStructureDrawingView();
                        PdfStructureDrawingViewModel PdfStructureDrawingViewModel = new PdfStructureDrawingViewModel();
                        PdfStructureDrawingViewModel.OpenPdf(structureDrawingPdf, rev.SolidworksDrawingFileName);
                        if (PdfStructureDrawingViewModel.IsPresent)
                        {
                            PdfStructureDrawingView.DataContext = PdfStructureDrawingViewModel;
                            PdfStructureDrawingView.Show();
                        }
                    }
                    else
                    {
                        CustomMessageBox.Show(string.Format("File Not Found..."), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        return;
                    }
                    GeosApplication.Instance.Logger.Log("Method PdfIconClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in PdfIconClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void DeleteAttachmentFile(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    SelectedAttachment = (OTAttachment)obj;
                    SelectedAttachment.TransactionOperation = ModelBase.TransactionOperations.Delete;
                    if (ListAddedAttachment == null)
                        ListAddedAttachment = new ObservableCollection<OTAttachment>();
                    //SelectedAttachment.AttachmentImage = null;
                    //ListAddedAttachment.Add(SelectedAttachment);
                    SelectedAttachment = (OTAttachment)obj;
                    ListAddedAttachment.Add(SelectedAttachment);
                    ListAttachment.Remove(SelectedAttachment);
                }
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAttachmentFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        // [nsatpute][14-04-2025][GEOS2-6728]
        private void CurrentItemChangedCommandAction(object obj)
        {
            try
            {
                if (SelectedObject != null)
                {
                    if (SelectedObject.Row["IdOtItem"] != null && Convert.ToUInt64(SelectedObject.Row["IdOtItem"]) > 0 && OT.OtItems.FirstOrDefault(x => x.IdOTItem == Convert.ToInt32(SelectedObject.Row["IdOtItem"]))?.RevisionItem.Files != null)
                    {
                        int idOtitem = Convert.ToInt32(SelectedObject.Row["IdOtItem"]);
                        ElectricalDiagramFileList = new ObservableCollection<FileDetail>(OT.OtItems.FirstOrDefault(x => x.IdOTItem == idOtitem)?.RevisionItem.Files.Where(y => y.Operation != OperationDb.Delete));
                    }
                    else
                    {
                        ElectricalDiagramFileList = new ObservableCollection<FileDetail>();
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Errorfpinas in Method CurrentItemChangedCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        private void AddNewElectricalDiagram(object obj)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddNewElectricalDiagram()...", category: Category.Info, priority: Priority.Low);
                if (SelectedObject != null)
                {
                    AddElectricalDiagramView addElectricalDiagramView = new AddElectricalDiagramView();
                    AddElectricalDiagramViewModel addElectricalDiagramViewModel = new AddElectricalDiagramViewModel();
                    EventHandler handle = delegate { addElectricalDiagramView.Close(); };
                    addElectricalDiagramViewModel.RequestClose += handle;
                    addElectricalDiagramView.DataContext = addElectricalDiagramViewModel;
                    long idDrawing = Convert.ToInt64(SelectedObject.Row["IdDrawing"]);
                    long idRevisionItem = Convert.ToInt64(SelectedObject.Row["IdRevisionItem"]);
                    addElectricalDiagramViewModel.Init(idDrawing, idRevisionItem);
                    var ownerInfo = (obj as FrameworkElement);
                    addElectricalDiagramView.Owner = Window.GetWindow(ownerInfo);
                    addElectricalDiagramView.ShowDialog();

                    if (addElectricalDiagramViewModel.isRecordAdded)
                    {
                        var fileDetails = addElectricalDiagramViewModel.SelectedList
                            .Select(diagram => new FileDetail
                            {

                                IdDrawing = diagram.IdDrawing,
                                FileName = diagram.Name,
                                ReferenceName = diagram.Name,
                                Description = diagram.Description,
                                FilePath = diagram.FilePath,
                                IdElectricalDiagram = diagram.ParentElectricalDiagramID,
                                IdUser = GeosApplication.Instance.ActiveUser.IdUser,
                                Comments = diagram.Comments
                            });
                        fileDetails.ForEach(x =>
                        {
                            if (!ElectricalDiagramFileList.Any(y => y.ReferenceName == x.ReferenceName))
                            {
                                x.Operation = OperationDb.New;
                                ElectricalDiagramFileList.Add(x);
                            }
                        });

                        if (SelectedObject != null)
                        {
                            if (SelectedObject.Row["IdOtItem"] != null && Convert.ToUInt64(SelectedObject.Row["IdOtItem"]) > 0
                                && OT.OtItems.FirstOrDefault(x => x.IdOTItem == Convert.ToInt64(SelectedObject.Row["IdOtItem"])).RevisionItem != null)
                            {
                                int idOtitem = Convert.ToInt32(SelectedObject.Row["IdOtItem"]);
                                OT.OtItems.FirstOrDefault(x => x.IdOTItem == idOtitem).RevisionItem.Files = ElectricalDiagramFileList.ToList();
                            }
                        }

                    }
                    GeosApplication.Instance.Logger.Log("Method AddNewElectricalDiagram()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddNewElectricalDiagram() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        // [GEOS2-6728][pallavi.kale][14-04-2025]
        private void ElectricalDiagramPdfIconClickCommandAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method ElectricalDiagramPdfIconClickCommandAction....", category: Category.Info, priority: Priority.Low);
            try
            {
                if (obj != null)
                {
                    StructureElectricalDiagramPdf = ((Emdep.Geos.Data.Common.File.FileDetail)obj).FileByte;
                    ElectricalDiagramFileName = ((Emdep.Geos.Data.Common.File.FileDetail)obj).FileName;
                    ElectricalDiagramFilePath = ((Emdep.Geos.Data.Common.File.FileDetail)obj).FilePath;


                    if (StructureElectricalDiagramPdf == null)
                    {
                        //  SAMService = new SAMServiceController("localhost:6699");
                        StructureElectricalDiagramPdf = SAMService.GetElectricalDiagramsFileImageInBytes(ElectricalDiagramFilePath);


                        PdfStructureDrawingView PdfStructureDrawingView = new PdfStructureDrawingView();
                        PdfStructureDrawingViewModel PdfStructureDrawingViewModel = new PdfStructureDrawingViewModel();
                        PdfStructureDrawingViewModel.OpenPdf(StructureElectricalDiagramPdf, ElectricalDiagramFileName);
                        if (PdfStructureDrawingViewModel.IsPresent)
                        {
                            PdfStructureDrawingView.DataContext = PdfStructureDrawingViewModel;
                            PdfStructureDrawingView.Show();
                        }
                        GeosApplication.Instance.Logger.Log("Method ElectricalDiagramPdfIconClickCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in ElectricalDiagramPdfIconClickCommandAction. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        // [nsatpute][13-05-2025][GEOS2-6728]
        public void DeleteElectricalDiagramFileCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DeleteElectricalDiagramFileCommandAction()...", category: Category.Info, priority: Priority.Low);
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(Application.Current.Resources["DeleteDocumentMessage"].ToString(), Application.Current.Resources["PopUpDeleteColor"].ToString(), CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);

                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    FileDetail file = (FileDetail)obj;
                    List<FileDetail> fileDetails = new List<FileDetail>(ElectricalDiagramFileList);
                    fileDetails.Remove(file);
                    ElectricalDiagramFileList.Clear();
                    ElectricalDiagramFileList = new ObservableCollection<FileDetail>(fileDetails);
                    int idOtitem = Convert.ToInt32(SelectedObject.Row["IdOtItem"]);
                    OT.OtItems.FirstOrDefault(x => x.IdOTItem == idOtitem).RevisionItem.Files.FirstOrDefault(y => y.ReferenceName == file.ReferenceName).Operation = OperationDb.Delete;
                    OT.OtItems.FirstOrDefault(x => x.IdOTItem == idOtitem).RevisionItem.Files.FirstOrDefault(y => y.ReferenceName == file.ReferenceName).Comments = string.Format(System.Windows.Application.Current.FindResource("RemoveElctricalDiagramChangeLog").ToString(), file.ReferenceName, OT.OtItems.FirstOrDefault(x => x.IdOTItem == idOtitem).RevisionItem.IdDrawing);
                    OT.OtItems.FirstOrDefault(x => x.IdOTItem == idOtitem).RevisionItem.Files.FirstOrDefault(y => y.ReferenceName == file.ReferenceName).IdUser = GeosApplication.Instance.ActiveUser.IdUser;
                }
                GeosApplication.Instance.Logger.Log("Method DeleteAttachmentFile()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method DeleteAttachmentFile()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }
}