using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using DevExpress.Xpf.Core;
using Prism.Logging;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.SAM;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using Emdep.Geos.Modules.SAM.Reports;
using System.Drawing;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using DevExpress.Utils.Svg;
using System.Data;
using System.Net;
using Emdep.Geos.Data.Common;
using DevExpress.XtraPrinting;
using DevExpress.Pdf;
using Emdep.Geos.Data.Common.PCM;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Emdep.Geos.Modules.SAM.Views
{
    //[nsatpute][04-07-2024][GEOS2-5408]
    public class DocumentsIdentificationViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        ISAMService SAMService = new SAMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISAMService SAMService = new SAMServiceController("localhost:6699");
        ISAMService SAMServiceToGetFiles = null;
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Fields
        private bool isSave;
        private ObservableCollection<LookupValue> structureDocumentList;
        private ObservableCollection<OTAttachment> documentsIdentificationList;
        private bool isAcceptEnable;
        private Ots ot;
        public List<byte[]> StructuralPhotos;
        public bool isBusy;
        List<Detection> lstDetectionNames;
        #endregion

        #region Properties
        List<Detection> LstDetectionNames
        {
            get { return lstDetectionNames; }
            set
            {
                lstDetectionNames = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstDetectionNames"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
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
        public ObservableCollection<LookupValue> StructureDocumentList
        {
            get
            {
                return structureDocumentList;
            }
            set { structureDocumentList = value; OnPropertyChanged(new PropertyChangedEventArgs("StructureDocumentList")); }
        }

        public ObservableCollection<OTAttachment> DocumentsIdentificationList
        {
            get { return documentsIdentificationList; }
            set { documentsIdentificationList = value; OnPropertyChanged(new PropertyChangedEventArgs("DocumentsIdentificationList")); }
        }

        public bool IsAcceptEnable
        {
            get { return isAcceptEnable; }
            set { isAcceptEnable = value; OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable")); }
        }

        #endregion


        #region Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region public commands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand StructureDocumentValidateCommand { get; set; }
        #endregion

        #region Constructor
        // [nsatpute][18-07-2024] [GEOS2-5409] 
        public DocumentsIdentificationViewModel(Ots ot)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("DocumentsIdentificationViewModel Method  DocumentsIdentificationViewModel()...", category: Category.Info, priority: Priority.Low);
                ShowSplashScreen();
                InitCammands();
                this.ot = ot;
                SAMServiceToGetFiles = new SAMServiceController(ot.Site.ServiceProviderUrl);
                StructureDocumentList = new ObservableCollection<LookupValue>(SAMService.GetLookupValues(150));
                DocumentsIdentificationList = new ObservableCollection<OTAttachment>(SAMServiceToGetFiles.GetQualityOTAndItemAttachments(ot.Site, ot.IdOT).Where(x => x.FileExtension.ToUpper() == ".JPEG" || x.FileExtension.ToUpper() == ".JPG" || x.FileExtension.ToUpper() == ".PNG" || x.FileExtension.ToUpper() == ".PDF"));
                IsAcceptEnable = DocumentsIdentificationList.ToList().Count > 0;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("DocumentsIdentificationViewModel Method DocumentsIdentificationViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                IsSave = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DocumentsIdentificationViewModel Method DocumentsIdentificationViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion


        #region Methods
        // [nsatpute][18-07-2024] [GEOS2-5409] 
        private void InitCammands()
        {
            AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            StructureDocumentValidateCommand = new DelegateCommand<object>(StructureDocumentValidateCommandAction);
        }
        public void StructureDocumentValidateCommandAction(object obj)
        {
            DevExpress.Xpf.Editors.ValidationEventArgs e = (DevExpress.Xpf.Editors.ValidationEventArgs)obj;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = string.Format("You cannot leave the {0} field empty.", "Document Identification");
            }
        }
        // [nsatpute][18-07-2024] [GEOS2-5409] 
        // [nsatpute][05-09-2024] [IESD-116556]
        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AcceptButtonCommandAction Method  AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);
                if (Validation())
                    return;
                #region Report Generation
                ShowSplashScreen();
                IsBusy = true;
                DocumentsIdentificationReport report = new DocumentsIdentificationReport();
                report.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 9, System.Drawing.FontStyle.Bold);
                report.xrTitleDatasheet.Font = new System.Drawing.Font(Application.Current.MainWindow.FontFamily.Source, 20, System.Drawing.FontStyle.Bold);
                report.xrTitleDatasheet.ForeColor = System.Drawing.Color.White;
                report.bigRectangle.FillColor = ColorTranslator.FromHtml("#074792");
                report.smallRectangle.FillColor = ColorTranslator.FromHtml("#e4ebf3");
                report.lblUserGuide.ForeColor = ColorTranslator.FromHtml("#074792");
                report.lblOfferCode.Text = ot.OfferCode;
                Bitmap BitmapimgLogo = new Bitmap(Emdep.Geos.Modules.SAM.Properties.Resource.Emdep_logo_mini);
                IndexSubReport index = new IndexSubReport(report);
                TechnicalPropertiesSubReport technicalSubReport = new TechnicalPropertiesSubReport(report);
                report.rptIndex.ReportSource = index;
                report.xrTitleDatasheet.Text = "";
                report.imgLogo.Image = BitmapimgLogo;
                report.imgLogo.Height = BitmapimgLogo.Height;
                report.imgLogo.WidthF = BitmapimgLogo.Width;
                string fileName = $"UserGuide_{ot.OfferCode}.pdf";
                string bookMarkName = $"UserGuide_{ot.OfferCode}";
                string tempPath = Path.GetTempPath();
                report.DisplayName = bookMarkName;
                DevExpress.XtraPrinting.PdfExportOptions options = new DevExpress.XtraPrinting.PdfExportOptions();
                options.DocumentOptions.Title = bookMarkName;
                #region Get Report Image
                byte[] customerLogoBytes = SAMServiceToGetFiles.GetCustomerLogoById(ot.Quotation.Site.Customer.IdCustomer);
                if(customerLogoBytes?.Length > 0)
                {
                    Image customerLogo = null;
                    byte[] resizedImageBytes = ResizeImage(customerLogoBytes, 300, 300);
                    using (MemoryStream ms = new MemoryStream(resizedImageBytes))
                    {
                        customerLogo = Image.FromStream(ms);
                    }
                    report.pbCompanyLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(customerLogo);
                }

                ProductTypes productType = SAMServiceToGetFiles.GetstructureOtProductDetails(ot.Site, ot.IdOT);
                if (productType != null)
                {
                    report.lblAbbreviation.Text = productType.Code;
                }
                if (productType?.Image?.ProductTypeImageInBytes?.Length > 0)
                {
                    Image productImage = null;
                    using (MemoryStream ms = new MemoryStream(productType.Image.ProductTypeImageInBytes))
                    {
                        productImage = Image.FromStream(ms);
                    }
                    report.pbUserGuideImage.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(productImage);
                }
                #endregion
                #region Technical Specification
                TechnicalSpecifications tech = SAMService.GetTechnicalSpecificationForReport(ot.Site, ot.IdOT);
                report.txtProject.Text = tech.ProjectName;
                report.txtCategory.Text = tech.Category;
                report.txtReference.Text = tech.Reference;
                report.txtModel.Text = tech.Model;
                report.txtVoltage.Text = tech.Voltage;
                report.txtAmperage.Text = tech.Amperage;
                report.txtFrequency.Text = tech.Frequency;
                report.txtPressure.Text = tech.Pressure;
                report.txtOem.Text = tech.Oem;

                byte[] imageInByte = null;
                Image image = null;
                /*
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        //imageInByte = webClient.DownloadData("https://ebes.emdep.com/GEOS/Images?filepath=./Images/Caroems/" + tech.Oem + ".png");
                        imageInByte = Utility.ImageUtil.GetImageByWebClient("https://ebes.emdep.com/GEOS/Images?filepath=./Images/Caroems/" + tech.Oem + ".png");
                        using (MemoryStream ms = new MemoryStream(imageInByte))
                        {
                            image = Image.FromStream(ms);
                        }
                        report.pbOemLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(image);
                    }
                    catch (Exception ex)
                    {
                        GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                */
                try
                {
                    imageInByte = Utility.ImageUtil.GetImageByWebClient("https://ebes.emdep.com/GEOS/Images?filepath=./Images/Caroems/" + tech.Oem + ".png");
                    using (MemoryStream ms = new MemoryStream(imageInByte))
                    {
                        image = Image.FromStream(ms);
                    }
                    report.pbOemLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(image);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                #region Fill Properties
                if (tech.Properties != null)
                {

                    List<Detection> rearrangedList = new List<Detection>();
                    var groupedData = tech.Properties.GroupBy(d => d.IdGroup);
                    string circle = "\u25CB"; // ○
                    string darkCircle = "\u25CF"; // ●
                    foreach (var group in groupedData)
                    {
                        string groupName = group.First().GroupName;
                        // Add a new entry for the group header (GroupName filled, DetectionName and Quantity empty)
                        rearrangedList.Add(new Detection { GroupName = $"{circle}  {groupName} ", Name = "", Quantity = 0 });

                        // Add all detections for the current group
                        foreach (var detection in group)
                        {
                            rearrangedList.Add(new Detection
                            {
                                IdGroup = detection.IdGroup,
                                GroupName = "", // Keep GroupName empty for subsequent entries
                                Name = $"{darkCircle}  {detection.Name}",
                                Quantity = detection.Quantity,
                                IdDetectionType = detection.IdDetectionType
                            });
                        }
                    }
                    rearrangedList.ForEach(x =>
                    {
                        string qty = "";
                        if (x.IdDetectionType == 3)
                        {
                            if (x.Quantity > 0)
                                qty = "x";
                            else
                                qty = "";
                        }
                        else
                        {
                            if (x.Quantity == 0)
                                qty = "";
                            else
                                qty = x.Quantity.ToString();
                        }
                        x.QuantityInString = qty;
                    });

                    technicalSubReport.objectDataSource2.DataSource = rearrangedList;


                    #endregion
                    #endregion

                    #region structure photos

                    tech.StructurePhotos = GetStructurePhotosForReport();
                    StructurePhotosSubReport structurePhotos = new StructurePhotosSubReport(report);
                    int midPoint = tech.StructurePhotos.Count / 2;
                    structurePhotos.Detail.HeightF = tech.StructurePhotos.Skip(midPoint).ToList().Count() * 300F + 15F;
                    AddStructurePhotosToTheReport(structurePhotos.tblStructurePhotos, tech.StructurePhotos.Skip(midPoint).ToList(), tech.StructurePhotos.Take(midPoint).ToList());
                    report.rptStructurePhotos.ReportSource = structurePhotos;
                    #endregion

                    #region Fill Modules references

                    List<ModuleReference> lstModuleReference = SAMService.GetModuleReferencesForReport(ot.Site, ot.IdOT);

                    lstModuleReference.ForEach(x =>
                    {
                        //using (WebClient webClient = new WebClient())
                        //{
                        //    try
                        //    {
                        //        x.ConnectorImageBytes = webClient.DownloadData(x.ConnectorImageApiUrl);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        //    }
                        //}
                        
                        try
                        {
                            x.ConnectorImageBytes = Utility.ImageUtil.GetImageByWebClient(x.ConnectorImageApiUrl);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                    });

                    ModuleReferencesSubReport reportModuleRef = new ModuleReferencesSubReport(report);
                    DataTable dataTable = new DataTable();

                    // Create datatable with fields
                    dataTable.Columns.Add("Item", typeof(string));
                    dataTable.Columns.Add("Connector", typeof(byte[]));
                    dataTable.Columns.Add("Reference", typeof(string));
                    dataTable.Columns.Add("Type", typeof(string));
                    dataTable.Columns.Add("Quantity", typeof(string));
                    LstDetectionNames = new List<Detection>();
                    foreach (ModuleReference mref in lstModuleReference)
                    {
                        foreach (Detection dt in mref.Detections)
                        {
                            if (!LstDetectionNames.Contains(dt))
                            {
                                LstDetectionNames.Add(dt);
                            }
                        }
                    }
                    LstDetectionNames.OrderBy(x => x.IdDetectionType).ToList().ForEach(y =>
                     {
                         if (!dataTable.Columns.Contains(y.IdDetectionType + "_" + y.Name))
                         {
                             dataTable.Columns.Add(y.IdDetectionType + "_" + y.Name, typeof(string));
                         }
                     });

                    dataTable.Columns.Add("ECOS", typeof(string));

                    foreach (var mref in lstModuleReference)
                    {
                        DataRow row = dataTable.NewRow();
                        row["Item"] = mref.NumItem;
                        row["Connector"] = mref.ConnectorImageBytes;
                        row["Reference"] = mref.Reference;
                        row["Type"] = mref.Type;
                        row["Quantity"] = mref.Quantity;
                        row["ECOS"] = mref.EcosNavigateUrl;

                        foreach (Detection dt in mref.Detections.OrderBy(x => x.IdDetectionType))
                        {
                            foreach (DataColumn dc in dataTable.Columns)
                            {
                                if (dc.ColumnName.Replace("1_", "").Replace("2_", "").Replace("3_", "") == dt.Name)
                                {
                                    if (dt.IdDetectionType == 3)
                                        row[dc.ColumnName] = "x";
                                    else
                                        row[dc.ColumnName] = dt.Quantity;
                                }
                            }
                        }
                        dataTable.Rows.Add(row);
                    }
                    float detailWidth = reportModuleRef.PageWidth - reportModuleRef.Margins.Left - reportModuleRef.Margins.Right;
                    XRTable tbl = CreateDynamicModuleReferenceTable(dataTable, detailWidth);
                    reportModuleRef.Detail.Controls.Add(tbl);
                    report.moduleReferenceSubReport.ReportSource = reportModuleRef;

                    #endregion

                    #region Fill Articles
                    List<Article> lstArticles = SAMService.GetTestboardElectrificationOtArticles(ot.Site, ot.IdOT);
                    ArticlesSubReport artReport = new ArticlesSubReport(report);
                    if(lstArticles.Count == 0)
                    {
                        artReport.tblArticleHeader.Visible = false;
                    }
                    artReport.tblArticles.BeginInit();
                    artReport.tblArticles.Rows.Clear();
                    List<string> lstGroups = new List<string>();
                    List<string> lstfirstParent = new List<string>();
                    List<string> lstSecondParent = new List<string>();
                    foreach (var article in lstArticles)
                    {
                        #region [rdixit][29.08.2024][GEOS2-6100]
                        if ((!string.IsNullOrEmpty(article.FirstParent)) && (!lstfirstParent.Contains(article.FirstParent)))
                        {
                            XRTableRow groupRow = new XRTableRow();
                            groupRow.HeightF = 35;
                            XRTableCell mergedCell = new XRTableCell();
                            mergedCell.BackColor = System.Drawing.Color.DodgerBlue;
                            mergedCell.Text = article.FirstParent.ToUpper();
                            mergedCell.Font = new Font("Calibri", 12, System.Drawing.FontStyle.Bold);
                            mergedCell.ForeColor = System.Drawing.Color.White;
                            mergedCell.WidthF = 82.17F + 544.67F + 91.33F + 79.83F;
                            groupRow.Cells.Add(mergedCell);
                            artReport.tblArticles.Rows.Add(groupRow);
                            lstfirstParent.Add(article.FirstParent);
                        }

                        if ((!string.IsNullOrEmpty(article.ParentSecond)) && (!lstSecondParent.Contains(article.ParentSecond)))
                        {
                            XRTableRow groupRow = new XRTableRow();
                            groupRow.HeightF = 35;
                            XRTableCell mergedCell = new XRTableCell();
                            mergedCell.BackColor = System.Drawing.Color.AliceBlue;
                            mergedCell.Text = "  " + article.ParentSecond.ToUpper();
                            mergedCell.Font = new Font("Calibri", 12, System.Drawing.FontStyle.Bold);
                            mergedCell.ForeColor = System.Drawing.Color.Black;
                            mergedCell.WidthF = 82.17F + 544.67F + 91.33F + 79.83F;
                            groupRow.Cells.Add(mergedCell);
                            artReport.tblArticles.Rows.Add(groupRow);
                            lstSecondParent.Add(article.ParentSecond);
                        }
                        #endregion

                        if (!lstGroups.Contains(article.ArticleCategory.CategoryName))
                        {
                            // Create a new row
                            XRTableRow groupRow = new XRTableRow();
                            groupRow.HeightF = 35;

                            // Create a cell to span all columns
                            XRTableCell mergedCell = new XRTableCell();
                            mergedCell.Text = "   > " + article.ArticleCategory.CategoryName;
                            mergedCell.Font = new Font("Calibri", 12, System.Drawing.FontStyle.Bold);
                            mergedCell.WidthF = 82.17F + 544.67F + 91.33F + 79.83F; // Total width of all cells

                            // Add the merged cell to the row
                            groupRow.Cells.Add(mergedCell);

                            // Add the row to the table
                            artReport.tblArticles.Rows.Add(groupRow);

                            // Track the group name
                            lstGroups.Add(article.ArticleCategory.CategoryName);
                        }


                        XRTableRow row = new XRTableRow();
                        row.HeightF = 77.5F;
                        Image img = null;
                        XRTableCell cell1 = new XRTableCell();
                        XRTableCell cell2 = new XRTableCell { Text = article.Reference + Environment.NewLine + article.Description };
                        cell2.Multiline = true;
                        XRTableCell cell3 = new XRTableCell { Text = article.Quantity.ToString() };
                        XRTableCell cell4 = new XRTableCell();
                        cell1.WidthF = 82.17F;
                        cell2.WidthF = 544.67F;
                        cell3.WidthF = 91.33F;
                        cell4.WidthF = 79.83F;


                        XRPictureBox pictureBox = new XRPictureBox();
                        try
                        {
                            /*
                            using (WebClient webClient = new WebClient())
                            {
                                string url = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/ARTICLE%20VISUAL%20AIDS/" + article.ImagePath;
                                //byte[] i = webClient.DownloadData(url);
                                byte[] i = Utility.ImageUtil.GetImageByWebClient(url);
                                if (i.Count() > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream(i))
                                    {
                                        img = Image.FromStream(ms);
                                    }
                                }
                            }
                            */
                            string url = "https://api.emdep.com/GEOS/Images.aspx?FilePath=/ARTICLE%20VISUAL%20AIDS/" + article.ImagePath;
                            byte[] i = Utility.ImageUtil.GetImageByWebClient(url);
                            if (i.Count() > 0)
                            {
                                using (MemoryStream ms = new MemoryStream(i))
                                {
                                    img = Image.FromStream(ms);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                        }
                        if (img == null)
                            img = Properties.Resource.ImageEditLogo; //[rdixit][28.08.2024][GEOS2-5410]
                        if (img != null)
                            pictureBox.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(img);
                        pictureBox.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                        pictureBox.HeightF = row.HeightF;
                        pictureBox.WidthF = cell1.WidthF;
                        cell1.Controls.Add(pictureBox);
                        XRPictureBox pb = new XRPictureBox();
                        pb.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(new Bitmap(Emdep.Geos.Modules.SAM.Properties.Resource.ECOS_logo));
                        pb.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                        pb.HeightF = 30.67F;
                        pb.WidthF = 38.33F;
                        pb.LocationF = new DevExpress.Utils.PointFloat((cell4.WidthF - pb.WidthF) / 2, (row.HeightF - pb.HeightF) / 2 + 10 );
                        pb.Borders = DevExpress.XtraPrinting.BorderSide.None;
                        cell4.Controls.Add(pb);
                        pb.NavigateUrl = "https://ecos.emdep.com/?ref=" + article.Reference;
                        cell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        cell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                        cell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                        cell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

                        row.Cells.AddRange(new XRTableCell[] { cell1, cell2, cell3, cell4 });

                        artReport.tblArticles.Rows.Add(row);
                    }
                    artReport.tblArticles.EndInit();
                    report.rptArticles.ReportSource = artReport;
                    #endregion

                    #region Add Quality Certification

                    List<Bitmap> lstQualityCertifications = GetQualityCertifications();
                    QualityCertificationsSubReport qcReport = new QualityCertificationsSubReport(report);

                    DetailBand detailBand = qcReport.QualityCertificationsDetails ?? new DetailBand();
                    detailBand.Controls.Clear();
                    detailBand.HeightF = qcReport.PageHeight - qcReport.Margins.Top - qcReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom;
                    if (!qcReport.Bands.Contains(detailBand))
                    {
                        qcReport.Bands.Add(detailBand);
                    }
                    float currentY = 0;
                    foreach (var im in lstQualityCertifications)
                    {
                        XRPictureBox pictureBox = new XRPictureBox
                        {
                            Image = im,
                            Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                            WidthF = qcReport.PageWidth - qcReport.Margins.Left - qcReport.Margins.Right,
                            HeightF = qcReport.PageHeight - qcReport.Margins.Top - qcReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom - 10F
                        };

                        pictureBox.TopF = currentY;
                        pictureBox.LeftF = qcReport.Margins.Left;

                        currentY += pictureBox.HeightF;

                        detailBand.Controls.Add(pictureBox);
                    }

                    report.rptQCSubReport.ReportSource = qcReport;
                    #endregion

                    #region Attached Item Documents
                    List<OTAttachment> lstItemAttachment = SAMServiceToGetFiles.GetItemAttachments(ot.Site, ot.IdOT);
                    List<Bitmap> lstAttachedDoc = GetAttachedDocuments(lstItemAttachment);

                    AttachedDocumentsSubReport attDocReport = new AttachedDocumentsSubReport(report);

                    detailBand = attDocReport.Detail ?? new DetailBand();

                    detailBand.Controls.Clear();

                    detailBand.HeightF = attDocReport.PageHeight - attDocReport.Margins.Top - attDocReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom;

                    if (!attDocReport.Bands.Contains(detailBand))
                    {
                        attDocReport.Bands.Add(detailBand);
                    }

                    currentY = 0;
                    foreach (var im in lstAttachedDoc)
                    {
                        XRPictureBox pictureBox = new XRPictureBox
                        {
                            Image = im,
                            Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                            WidthF = attDocReport.PageWidth - attDocReport.Margins.Left - attDocReport.Margins.Right,
                            HeightF = attDocReport.PageHeight - attDocReport.Margins.Top - attDocReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom - 10
                        };

                        pictureBox.TopF = currentY;
                        pictureBox.LeftF = attDocReport.Margins.Left;
                        currentY += pictureBox.HeightF;
                        detailBand.Controls.Add(pictureBox);
                    }

                    report.rptAttachedDoc.ReportSource = attDocReport;

                    #endregion

                    #region Add Electrical Diagrams

                    List<byte[]> lstElectricalDiagram = SAMServiceToGetFiles.GetstructureOtElectricalDiagramsInBytes(ot.Site, ot.IdOT);
                    List<Bitmap> lstElectricalDiagramImage = GetElectricalDiagramDocuments(lstElectricalDiagram);

                    ElectricalDiagramSubReport rptelectricDiagram = new ElectricalDiagramSubReport(report);

                    detailBand = rptelectricDiagram.Detail ?? new DetailBand();

                    detailBand.Controls.Clear();

                    detailBand.HeightF = rptelectricDiagram.PageHeight - rptelectricDiagram.Margins.Top - rptelectricDiagram.Margins.Bottom - report.Margins.Top - report.Margins.Bottom;

                    if (!rptelectricDiagram.Bands.Contains(detailBand))
                    {
                        rptelectricDiagram.Bands.Add(detailBand);
                    }

                    currentY = 0;
                    foreach (var im in lstElectricalDiagramImage)
                    {
                        XRPictureBox pictureBox = new XRPictureBox
                        {
                            Image = im,
                            Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                            WidthF = rptelectricDiagram.PageWidth - rptelectricDiagram.Margins.Left - rptelectricDiagram.Margins.Right,
                            HeightF = rptelectricDiagram.PageHeight - rptelectricDiagram.Margins.Top - rptelectricDiagram.Margins.Bottom - report.Margins.Top - report.Margins.Bottom - 10
                        };

                        pictureBox.TopF = currentY;
                        pictureBox.LeftF = rptelectricDiagram.Margins.Left;
                        currentY += pictureBox.HeightF;
                        detailBand.Controls.Add(pictureBox);
                    }

                    report.rptElectricalDiagram.ReportSource = rptelectricDiagram;

                    #endregion

                    #region Add Drawing

                    byte[] planolFile = SAMServiceToGetFiles.GetstructureOtPlanolInBytes(ot.Site, ot.IdOT);

                    List<Bitmap> lstDrawingPlanol = GetPlanolDocuments(planolFile);

                    DrawingSubReport drawingSubReport = new DrawingSubReport(report);

                    detailBand = drawingSubReport.Detail ?? new DetailBand();

                    detailBand.Controls.Clear();

                    detailBand.HeightF = drawingSubReport.PageHeight - drawingSubReport.Margins.Top - drawingSubReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom;

                    if (!drawingSubReport.Bands.Contains(detailBand))
                    {
                        drawingSubReport.Bands.Add(detailBand);
                    }

                    currentY = 0;
                    foreach (var im in lstDrawingPlanol)
                    {
                        XRPictureBox pictureBox = new XRPictureBox
                        {
                            Image = im,
                            Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                            WidthF = drawingSubReport.PageWidth - drawingSubReport.Margins.Left - drawingSubReport.Margins.Right,
                            HeightF = drawingSubReport.PageHeight - drawingSubReport.Margins.Top - drawingSubReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom - 10
                        };

                        pictureBox.TopF = currentY;
                        pictureBox.LeftF = drawingSubReport.Margins.Left;
                        currentY += pictureBox.HeightF;
                        detailBand.Controls.Add(pictureBox);
                    }

                    report.rptDrawing.ReportSource = drawingSubReport;

                    #endregion
                    

                    #region Add declaration of conformity

                    List<Bitmap> lstStandardDeclaration = GetStandardDeclaration();
                    if(lstStandardDeclaration.Count == 0 )
                    {
                        lstStandardDeclaration = GenerateCustomDelcaration();
                    }
                    StandardDeclarationSubReport standardDeclarationReport = new StandardDeclarationSubReport(report);
                    

                    detailBand = standardDeclarationReport.Detail ?? new DetailBand();

                    detailBand.Controls.Clear();

                    detailBand.HeightF = standardDeclarationReport.PageHeight - standardDeclarationReport.Margins.Top - standardDeclarationReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom;

                    if (!standardDeclarationReport.Bands.Contains(detailBand))
                    {
                        standardDeclarationReport.Bands.Add(detailBand);
                    }

                    currentY = 0;
                    foreach (var im in lstStandardDeclaration)
                    {
                        XRPictureBox pictureBox = new XRPictureBox
                        {
                            Image = im,
                            Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                            WidthF = standardDeclarationReport.PageWidth - standardDeclarationReport.Margins.Left - standardDeclarationReport.Margins.Right,
                            HeightF = standardDeclarationReport.PageHeight - standardDeclarationReport.Margins.Top - standardDeclarationReport.Margins.Bottom - report.Margins.Top - report.Margins.Bottom - 10
                        };

                        pictureBox.TopF = currentY;
                        pictureBox.LeftF = standardDeclarationReport.Margins.Left;
                        currentY += pictureBox.HeightF;
                        detailBand.Controls.Add(pictureBox);
                    }

                    report.rptStandardDeclaration.ReportSource = standardDeclarationReport;

                    #endregion
                    report.TechProperties.ReportSource = technicalSubReport;
                    using (FileStream pdfFileStream = new FileStream(tempPath + fileName, FileMode.Create))
                    {
                        report.ExportToPdf(pdfFileStream, options);
                    }
                    #endregion

                    string destinationFilePath = $@"{ot.Quotation.Year}\{ot.Quotation.Code}\QA Documents";
                    SaveFileToServer(tempPath, destinationFilePath, fileName);
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("DocumentsIdentificationView_Reportgenerated").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    IsSave = true;
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("AcceptButtonCommandAction Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                IsSave = false;
                IsBusy = false;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                if(ex.ToString().Contains("System.IO.IOException"))
                {
                    CustomMessageBox.Show(System.Windows.Application.Current.FindResource("DocumentsIdentificationView_Fileisbeingused").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction Method AcceptButtonCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        private XRTable CreateDynamicModuleReferenceTable(DataTable dataTable, float detailWidth)
        {
            detailWidth = detailWidth - 10F; // Adjust width as needed
            float tblHeight = 0F;
            float rowH = 77.5F;
            XRTable xrTable = new XRTable
            {
                WidthF = detailWidth
            };
            xrTable.BeginInit();

            // Create the top row with a merged cell
            XRTableRow topRow = new XRTableRow
            {
                HeightF = 70F // Set height as needed
            };
            tblHeight += 70F;

            // Dictionary to store column widths
            Dictionary<string, float> columnWidths = new Dictionary<string, float>();

            // Create the header row based on DataTable column names
            XRTableRow headerRow = new XRTableRow
            {
                HeightF = 190F // Set a reasonable default height
            };
            tblHeight += 190F;

            float detectionWidth = 30F;
            float totalDetectionWidth = 0;
            List<string> lstWays = new List<string>();
            List<string> lstDetections = new List<string>();
            List<string> lstOptions = new List<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                if (!new string[] { "ITEM", "CONNECTOR", "REFERENCE", "TYPE", "ECOS" }.Contains(column.ColumnName.ToUpper()))
                {
                    totalDetectionWidth += detectionWidth;
                    if (column.ColumnName.Contains("1_"))
                    {
                        string originalColName = column.ColumnName.Replace("1_", "");
                        lstWays.Add(originalColName);
                        column.ColumnName = originalColName;
                    }
                    else if (column.ColumnName.Contains("2_"))
                    {
                        string originalColName = column.ColumnName.Replace("2_", "");
                        lstDetections.Add(originalColName);
                        column.ColumnName = originalColName;
                    }
                    else if (column.ColumnName.Contains("3_"))
                    {
                        string originalColName = column.ColumnName.Replace("3_", "");
                        lstOptions.Add(originalColName);
                        column.ColumnName = originalColName;
                    }
                }
            }
            float remainingWidth = detailWidth - totalDetectionWidth;
            float colItemWidth = 100F;
            float colConnectorWidth = 120.33F;
            float colTypeWidth = 129.49F;
            float colEcosWidth = 60F;
            float colReferenceWidth = remainingWidth - colItemWidth - colConnectorWidth - colTypeWidth - colEcosWidth;
            if (colReferenceWidth < 131.99F)
                colReferenceWidth = 131.99F;

            // Calculate the merged width for the top row cells
            float mergedWaysWidth = lstWays.Count * detectionWidth;
            float mergedDetectionsWidth = lstDetections.Count * detectionWidth;
            float mergedOptionsWidth = lstOptions.Count * detectionWidth;


            // Create the header cells
            foreach (DataColumn column in dataTable.Columns)
            {
                XRTableCell headerCell = new XRTableCell
                {
                    Text = column.ColumnName,
                    Font = new Font("Calibri", 10.08F, System.Drawing.FontStyle.Bold),
                    Borders = BorderSide.All,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, System.Drawing.GraphicsUnit.Pixel)
                };
                XRTableCell topCell = new XRTableCell
                {
                    Text = "",
                    Font = new Font("Calibri", 10.08F, System.Drawing.FontStyle.Bold),
                    Borders = BorderSide.Top,
                    TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter,
                    Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, System.Drawing.GraphicsUnit.Pixel)
                };

                switch (column.ColumnName.ToUpper())
                {
                    case "ITEM":
                        headerCell.WidthF = colItemWidth;
                        topCell.WidthF = colItemWidth;
                        break;
                    case "CONNECTOR":
                        headerCell.WidthF = colConnectorWidth;
                        topCell.WidthF = colConnectorWidth;
                        break;
                    case "REFERENCE":
                        headerCell.WidthF = colReferenceWidth;
                        topCell.WidthF = colReferenceWidth;
                        break;
                    case "TYPE":
                        headerCell.WidthF = colTypeWidth;
                        topCell.WidthF = colTypeWidth;
                        break;
                    case "ECOS":
                        headerCell.WidthF = colEcosWidth;
                        topCell.WidthF = colEcosWidth;
                        break;
                    default:
                        headerCell.WidthF = detectionWidth;
                        headerCell.Angle = 90; // Rotate text if needed
                        headerCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;

                        topCell.WidthF = detectionWidth;
                        topCell.Angle = 90; // Rotate text if needed
                        topCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;

                        #region [rdixit][28.08.2024][GEOS2-5410]
                        if (LstDetectionNames?.FirstOrDefault(i => i.Name == column.ColumnName)?.IdDetectionType ==1)
                        {
                            headerCell.BackColor = System.Drawing.Color.LightGray;
                            topCell.BackColor = System.Drawing.Color.LightGray;
                        }
                        if (LstDetectionNames?.FirstOrDefault(i => i.Name == column.ColumnName)?.IdDetectionType == 2)
                        {
                            headerCell.BackColor = System.Drawing.Color.LightGreen;
                            topCell.BackColor = System.Drawing.Color.LightGreen;
                        }
                        if (LstDetectionNames?.FirstOrDefault(i => i.Name == column.ColumnName)?.IdDetectionType == 3)
                        {
                            headerCell.BackColor = System.Drawing.Color.LightSkyBlue;
                            topCell.BackColor = System.Drawing.Color.LightSkyBlue;
                        }
                        if (LstDetectionNames?.FirstOrDefault(i => i.Name == column.ColumnName)?.IdDetectionType == 4)
                        {
                            headerCell.BackColor = System.Drawing.Color.LightCoral;
                            topCell.BackColor = System.Drawing.Color.LightCoral;
                        }
                        #endregion
                        break;
                }

                columnWidths[column.ColumnName] = headerCell.WidthF;
                headerRow.Cells.Add(headerCell);
                topRow.Cells.Add(topCell);
            }
            topRow.Cells[0].Borders = BorderSide.None;
            topRow.Cells[1].Borders = BorderSide.None;
            topRow.Cells[2].Borders = BorderSide.None;
            topRow.Cells[3].Borders = BorderSide.None;
            topRow.Cells[4].Borders = BorderSide.Right;
            if (lstWays.Count > 0)
            {
                topRow.Cells[5].Text = "Ways";
                topRow.Cells[5].Borders = BorderSide.Top;
                for (int i = 6; i < 5 + lstWays.Count + 1; i++)
                {
                    topRow.Cells[i].Borders = BorderSide.Top;
                }
                topRow.Cells[5 + lstWays.Count].Borders = BorderSide.Top | BorderSide.Right;
            }
            if (lstDetections.Count > 0)
            {
                topRow.Cells[5 + lstWays.Count].Text = "Detections";
                topRow.Cells[5 + lstWays.Count].Borders = BorderSide.Left | BorderSide.Top;
                for (int i = 5 + lstWays.Count + 1; i < 5 + lstWays.Count + lstDetections.Count + 1; i++)
                {
                    topRow.Cells[i].Borders = BorderSide.Top;
                }
                topRow.Cells[5 + lstWays.Count + lstDetections.Count].Borders = BorderSide.Top | BorderSide.Right;
            }
            if (lstOptions.Count > 0)
            {
                topRow.Cells[5 + lstWays.Count + lstDetections.Count].Text = "Options";
                topRow.Cells[5 + lstWays.Count + lstDetections.Count].Borders = BorderSide.Left | BorderSide.Top;
                for (int i = 5 + lstWays.Count + lstDetections.Count + 1; i < 5 + lstWays.Count + lstDetections.Count + lstOptions.Count + 1; i++)
                {
                    topRow.Cells[i].Borders = BorderSide.Top;
                }
            }
            topRow.Cells[lstWays.Count + lstDetections.Count + lstOptions.Count + 4].Borders = BorderSide.Top | BorderSide.Right;
            topRow.Cells[lstWays.Count + lstDetections.Count + lstOptions.Count + 5].Borders = BorderSide.None;

            xrTable.Rows.Add(topRow);
            xrTable.Rows.Add(headerRow);


            // Create rows based on DataTable rows
            foreach (DataRow dataRow in dataTable.Rows)
            {
                XRTableRow row = new XRTableRow
                {
                    HeightF = rowH // Set a reasonable default height
                };

                tblHeight += rowH;
                foreach (DataColumn column in dataTable.Columns)
                {
                    XRTableCell cell = new XRTableCell
                    {
                        Borders = BorderSide.All, // Optional: Set border style
                        WidthF = columnWidths[column.ColumnName],
                        Font = new Font("Calibri", 10.08F),
                        HeightF = row.HeightF,
                        Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, System.Drawing.GraphicsUnit.Pixel)
                    };

                    if (column.ColumnName.ToUpper() == "ECOS")
                    {
                        XRPictureBox pictureBox = new XRPictureBox
                        {
                            Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                            NavigateUrl = dataRow[column].ToString(),
                            ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(new Bitmap(Emdep.Geos.Modules.SAM.Properties.Resource.ECOS_logo)),
                            HeightF = cell.HeightF - 5,
                            WidthF = cell.WidthF - 20,
                            Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2),
                            Borders = BorderSide.None
                        };
                        pictureBox.LocationF = new DevExpress.Utils.PointFloat(1, (row.HeightF - pictureBox.HeightF) / 2 + 10);
                        cell.Controls.Add(pictureBox);
                    }
                    else if (column.ColumnName.ToUpper() == "CONNECTOR")
                    {
                        if (dataRow[column].ToString().Length > 0 && ((byte[])dataRow[column]).Length > 0)
                        {
                            Image img = null;
                            byte[] imgSource = (byte[])dataRow[column];
                            using (MemoryStream ms = new MemoryStream(imgSource))
                            {
                                img = Image.FromStream(ms);
                            }
                            XRPictureBox pictureBox = new XRPictureBox
                            {
                                Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage,
                                ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(img),
                                HeightF = row.HeightF,
                                WidthF = cell.WidthF,
                                Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2),
                                Borders = BorderSide.None
                            };
                            cell.Controls.Add(pictureBox);
                        }
                    }
                    else
                    {
                        cell.Text = dataRow[column].ToString();
                        cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    }

                    row.Cells.Add(cell);
                }
                xrTable.Rows.Add(row);
            }
            xrTable.HeightF = tblHeight;
            xrTable.EndInit();

            return xrTable;
        }

        // [nsatpute][18-07-2024] [GEOS2-5409] 
        private void SaveFileToServer(string sourceFilePath, string destinationFilePath, string fileName)
        {
            const int partSize = 1048576; // 1 MB
            byte[] buffer = new byte[partSize];
            FileInfo fileInfo = new FileInfo(sourceFilePath + fileName);
            SAMServiceToGetFiles.StartSavingFile(destinationFilePath, fileInfo.Name);

            using (FileStream fileStream = new FileStream(sourceFilePath + fileName, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, partSize)) > 0)
                {
                    SAMServiceToGetFiles.SavePartData(destinationFilePath, fileName, buffer.Take(bytesRead).ToArray());
                }
            }
            if (File.Exists(sourceFilePath + fileName))
            {
                File.Delete(sourceFilePath + fileName);
            }
        }
        private bool Validation()
        {
            if (DocumentsIdentificationList.Where(x => x.IsSelected).Any(y => y.StructureDocumenttype == null))
                return true;

            if (DocumentsIdentificationList.Where(x => x.IsSelected).ToList().Count == 0)
            {
                CustomMessageBox.Show(Application.Current.Resources["DocumentsIdentificationView_Norecords"].ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return true;
            }

            return false;
        }
        public void AddStructurePhotosToTheReport(XRTable xrTable, List<Image> column1Images, List<Image> column2Images)
        {
            xrTable.Rows.Clear();
            int rowCount = Math.Max(column1Images.Count, column2Images.Count);
            float consistentRowHeight = 300F;
            xrTable.HeightF = rowCount * consistentRowHeight + 10F;
            for (int i = 0; i < rowCount; i++)
            {
                XRTableRow dataRow = new XRTableRow();
                dataRow.HeightF = consistentRowHeight;

                // Cell for the first image column (left side)
                XRTableCell cell1 = new XRTableCell();
                cell1.CanGrow = false;
                cell1.CanShrink = false;
                if (i < column1Images.Count)
                {
                    XRPictureBox pictureBox1 = new XRPictureBox();
                    pictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(column1Images[i]);
                    pictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                    pictureBox1.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Both;
                    pictureBox1.AnchorHorizontal = DevExpress.XtraReports.UI.HorizontalAnchorStyles.Both;
                    pictureBox1.SizeF = cell1.SizeF;
                    cell1.Controls.Add(pictureBox1);
                }
                dataRow.Cells.Add(cell1);

                // Cell for the second image column (right side)
                XRTableCell cell2 = new XRTableCell();
                cell2.CanGrow = false;
                cell2.CanShrink = false;
                if (i < column2Images.Count)
                {
                    XRPictureBox pictureBox2 = new XRPictureBox();
                    pictureBox2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(column2Images[i]);
                    pictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                    pictureBox2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Both;
                    pictureBox2.AnchorHorizontal = DevExpress.XtraReports.UI.HorizontalAnchorStyles.Both;
                    pictureBox2.SizeF = cell2.SizeF;
                    cell2.Controls.Add(pictureBox2);
                }
                dataRow.Cells.Add(cell2);

                xrTable.Rows.Add(dataRow);
            }
            foreach (XRTableRow row in xrTable.Rows)
            {
                row.HeightF = consistentRowHeight;
            }
        }
        private void ShowSplashScreen()
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
        }
        //[nsatpute][05-08-2024][GEOS2-5410]
        private List<Image> GetStructurePhotosForReport()
        {
            List<Image> lstImages = new List<Image>();
            //List<OTAttachment> otAttachmentList = SAMServiceToGetFiles.GetStructureOTAttachment(ot.Site, ot.IdOT);
            DocumentsIdentificationList.Where(x=> x.IsSelected).ToList().ForEach(x =>
            {
                if (x.StructureDocumenttype.IdLookupValue == 1973 && x.FileExtension.ToUpper() != ".PDF") // Equipment Photo
                {
                    byte[] imageBytes = SAMServiceToGetFiles.GetOTAttachmentInBytes(x.FileName, x.QuotationYear, x.QuotationCode);
                    if (imageBytes?.Length > 0)
                    {
                        byte[] resizedImageBytes = ResizeImage(imageBytes, 300, 300);
                        using (MemoryStream ms = new MemoryStream(resizedImageBytes))
                        {
                            Image image = Image.FromStream(ms);
                            lstImages.Add(image);
                        }
                    }
                }
            });
            return lstImages;
        }
        private List<Bitmap> GetQualityCertifications()
        {
            List<Bitmap> lstImages = new List<Bitmap>();
            DocumentsIdentificationList.ToList().ForEach(x =>
            {
                if (x.StructureDocumenttype?.IdLookupValue == 1971 && x.FileExtension.ToUpper() == ".PDF") // QC Checklist
                {
                    byte[] imageBytes = SAMServiceToGetFiles.GetOTAttachmentInBytes(x.FileName, x.QuotationYear, x.QuotationCode);
                    if (imageBytes?.Length > 0)
                    {
                        PdfDocumentProcessor pdfProcessor = new PdfDocumentProcessor();
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            pdfProcessor.LoadDocument(stream);
                            for (int i = 0; i < pdfProcessor.Document.Pages.Count; i++)
                            {
                                Bitmap pageImage = pdfProcessor.CreateBitmap(i + 1, 2000);
                                if (pageImage != null)
                                {
                                    // Crop the white space
                                    Bitmap croppedImage = CropWhiteSpace(pageImage);
                                    lstImages.Add(croppedImage);
                                }
                            }
                        }
                    }
                }
            });
            return lstImages;
        }
        private List<Bitmap> GetStandardDeclaration()
        {
            List<Bitmap> lstImages = new List<Bitmap>();
            DocumentsIdentificationList.Where(x => x.IsSelected).ToList().ForEach(x =>
            {
                if (x.StructureDocumenttype?.IdLookupValue == 1970 && x.FileExtension.ToUpper() == ".PDF") // Declaration of Conformity
                {
                    byte[] imageBytes = SAMServiceToGetFiles.GetOTAttachmentInBytes(x.FileName, x.QuotationYear, x.QuotationCode);
                    if (imageBytes?.Length > 0)
                    {
                        PdfDocumentProcessor pdfProcessor = new PdfDocumentProcessor();
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            pdfProcessor.LoadDocument(stream);
                            for (int i = 0; i < pdfProcessor.Document.Pages.Count; i++)
                            {
                                Bitmap pageImage = pdfProcessor.CreateBitmap(i + 1, 2000);
                                if (pageImage != null)
                                {
                                    // Crop the white space
                                    Bitmap croppedImage = CropWhiteSpace(pageImage);
                                    lstImages.Add(croppedImage);
                                }
                            }
                        }
                    }
                }
            });
            return lstImages;
        }
        private List<Bitmap> GenerateCustomDelcaration()
        {
            List<Bitmap> lstImages = new List<Bitmap>();

            byte[] imageBytes = SAMServiceToGetFiles.GenerateAndGetDeclarationOfConformity(ot.Site, ot.IdOT);
            if (imageBytes?.Length > 0)
            {
                PdfDocumentProcessor pdfProcessor = new PdfDocumentProcessor();
                using (MemoryStream stream = new MemoryStream(imageBytes))
                {
                    pdfProcessor.LoadDocument(stream);
                    for (int i = 0; i < pdfProcessor.Document.Pages.Count; i++)
                    {
                        Bitmap pageImage = pdfProcessor.CreateBitmap(i + 1, 2000);
                        if (pageImage != null)
                        {
                            // Crop the white space
                            Bitmap croppedImage = CropWhiteSpace(pageImage);
                            lstImages.Add(croppedImage);
                        }
                    }
                }
            }

            return lstImages;
        }
        private List<Bitmap> GetAttachedDocuments(List<OTAttachment> lstItemAttachment)
        {
            List<Bitmap> lstImages = new List<Bitmap>();
            foreach (OTAttachment oa in lstItemAttachment)
            {
                if (DocumentsIdentificationList.Any(x => x.FileName == oa.FileName && x.IsSelected && x.StructureDocumenttype != null && x.StructureDocumenttype.IdLookupValue != 1971) && oa.FileExtension.ToUpper() == ".PDF")
                {
                    byte[] imageBytes = SAMServiceToGetFiles.GetOTAttachmentInBytes(oa.FileName, oa.QuotationYear, oa.QuotationCode);
                    if (imageBytes?.Length > 0)
                    {
                        PdfDocumentProcessor pdfProcessor = new PdfDocumentProcessor();
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            pdfProcessor.LoadDocument(stream);
                            for (int i = 0; i < pdfProcessor.Document.Pages.Count; i++)
                            {
                                Bitmap pageImage = pdfProcessor.CreateBitmap(i + 1, 2000);
                                if (pageImage != null)
                                {
                                    // Crop the white space
                                    Bitmap croppedImage = CropWhiteSpace(pageImage);
                                    lstImages.Add(croppedImage);
                                }
                            }
                        }
                    }
                }
            }

            return lstImages;
        }
        private List<Bitmap> GetElectricalDiagramDocuments(List<byte[]> lstItemAttachment)
        {
            List<Bitmap> lstImages = new List<Bitmap>();
            foreach (byte[] file in lstItemAttachment)
            {
                if (file?.Length > 0)
                {
                    PdfDocumentProcessor pdfProcessor = new PdfDocumentProcessor();
                    using (MemoryStream stream = new MemoryStream(file))
                    {
                        pdfProcessor.LoadDocument(stream);
                        for (int i = 0; i < pdfProcessor.Document.Pages.Count; i++)
                        {
                            Bitmap pageImage = pdfProcessor.CreateBitmap(i + 1, 2000);
                            if (pageImage != null)
                            {
                                lstImages.Add(pageImage);
                            }
                        }
                    }
                }
            }
            return lstImages;
        }
        private List<Bitmap> GetPlanolDocuments(byte[] planol)
        {
            List<Bitmap> lstImages = new List<Bitmap>();

            if (planol?.Length > 0)
            {
                PdfDocumentProcessor pdfProcessor = new PdfDocumentProcessor();
                using (MemoryStream stream = new MemoryStream(planol))
                {
                    pdfProcessor.LoadDocument(stream);
                    for (int i = 0; i < pdfProcessor.Document.Pages.Count; i++)
                    {
                        Bitmap pageImage = pdfProcessor.CreateBitmap(i + 1, 2000);
                        if (pageImage != null)
                        {
                            // Crop the white space
                            Bitmap croppedImage = CropWhiteSpace(pageImage);
                            lstImages.Add(croppedImage);
                        }
                    }
                }
            }
            return lstImages;
        }
        private Bitmap CropWhiteSpace(Bitmap bmp)
        {
            // Get the bounds of the content
            Rectangle contentBounds = GetContentBounds(bmp);

            // Crop the image to the content bounds
            if (contentBounds != Rectangle.Empty)
            {
                Bitmap croppedBmp = bmp.Clone(contentBounds, bmp.PixelFormat);
                bmp.Dispose(); // Dispose the original image to free up resources
                return croppedBmp;
            }

            return bmp; // Return original bitmap if no cropping is needed
        }

        private Rectangle GetContentBounds(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            int left = 0;
            int top = 0;
            int right = width - 1;
            int bottom = height - 1;

            // Find the left boundary
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != System.Drawing.Color.White.ToArgb())
                    {
                        left = x;
                        break;
                    }
                }
                if (left != 0) break;
            }

            // Find the right boundary
            for (int x = width - 1; x >= left; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != System.Drawing.Color.White.ToArgb())
                    {
                        right = x;
                        break;
                    }
                }
                if (right != width - 1) break;
            }

            // Find the top boundary
            for (int y = 0; y < height; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != System.Drawing.Color.White.ToArgb())
                    {
                        top = y;
                        break;
                    }
                }
                if (top != 0) break;
            }

            // Find the bottom boundary
            for (int y = height - 1; y >= top; y--)
            {
                for (int x = left; x <= right; x++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() != System.Drawing.Color.White.ToArgb())
                    {
                        bottom = y;
                        break;
                    }
                }
                if (bottom != height - 1) break;
            }

            // Create a rectangle with the found boundaries
            return new Rectangle(left, top, right - left + 1, bottom - top + 1);
        }
        private byte[] ResizeImage(byte[] imageBytes, int maxWidth, int maxHeight)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                using (var originalImage = Image.FromStream(ms))
                {
                    int newWidth = originalImage.Width;
                    int newHeight = originalImage.Height;

                    // Calculate new dimensions
                    if (originalImage.Width > maxWidth || originalImage.Height > maxHeight)
                    {
                        float aspectRatio = (float)originalImage.Width / originalImage.Height;

                        if (originalImage.Width > originalImage.Height)
                        {
                            newWidth = maxWidth;
                            newHeight = (int)(maxWidth / aspectRatio);
                        }
                        else
                        {
                            newHeight = maxHeight;
                            newWidth = (int)(maxHeight * aspectRatio);
                        }
                    }

                    using (var resizedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (var graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighQuality;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.SmoothingMode = SmoothingMode.HighQuality;

                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

                            using (var outputStream = new MemoryStream())
                            {
                                resizedImage.Save(outputStream, ImageFormat.Png);
                                return outputStream.ToArray();
                            }
                        }
                    }
                }
            }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}