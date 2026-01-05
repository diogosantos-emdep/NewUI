using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using System.Windows;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class ProjectScopeViewModel : NavigationViewModelBase
    {
        //protected override void OnParameterChanged(object parameter)
        //{
        //    ProjectData =(Project)parameter;
        //    base.OnParameterChanged(parameter);
        //}

        #region Services
        //private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IEpcService epcControl;
        #endregion

        #region ICommands

        public ICommand SaveProjectScopeCommand { get; set; }

        #endregion

        #region Collection

        private string copyright;
        private CMYKColor BlueColorPdf;
        private iTextSharp.text.Font font9;
        private iTextSharp.text.Font font11;

        public bool ISave { get; set; }
        public bool IsNewProjectScope { get; set; }

        private ProjectScope projectScopeData;
        public ProjectScope ProjectScopeData
        {
            get { return projectScopeData; }
            set
            {
                SetProperty(ref projectScopeData, value, () => ProjectScopeData);
            }
        }

        private Project projectData;
        public Project ProjectData
        {
            get { return projectData; }
            set
            {
                SetProperty(ref projectData, value, () => ProjectData);
            }
        }

        #endregion

        #region Consturctor

        public ProjectScopeViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            SaveProjectScopeCommand = new Prism.Commands.DelegateCommand<object>(SaveProjectScopeAction);

            BlueColorPdf = new CMYKColor(150, 87, 0, 61);
            copyright = char.ConvertFromUtf32(int.Parse("00A9", System.Globalization.NumberStyles.HexNumber)).ToString();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            font9 = new iTextSharp.text.Font(bfTimes, 9, iTextSharp.text.Font.NORMAL, BlueColorPdf);
            font11 = new iTextSharp.text.Font(bfTimes, 11, iTextSharp.text.Font.NORMAL, BlueColorPdf);
        }

        #endregion

        #region Navigation Methods

        protected override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

            ProjectData = (Project)this.Parameter;
            ProjectScopeData = epcControl.GetProjectScopeByProjectId(((Project)this.Parameter).IdProject);

            if (ProjectScopeData == null)
            {
                ProjectScopeData = new ProjectScope();
                IsNewProjectScope = true;
            }
        }

        #endregion

        #region Methods

        public void SaveProjectScopeAction(object obj)
        {
            string pdfFilePath = generateProjectScopePdf(ProjectScopeData);

            if (!string.IsNullOrEmpty(pdfFilePath))
            {
                ProjectScopeData.ScopeFileBytes = System.IO.File.ReadAllBytes(pdfFilePath);
            }

            if (IsNewProjectScope)
            {
                ProjectScopeData.IdProject = ProjectData.IdProject;
                ProjectScopeData.IdOffer = ProjectData.IdOffer;
                ProjectScopeData.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                ProjectScopeData.IdProductVersion = null;

                ProjectScopeData = epcControl.AddProjectScope(ProjectScopeData);
                CustomMessageBox.Show(String.Format(System.Windows.Application.Current.FindResource("ProjectScopeAdded").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                IsNewProjectScope = false;
            }
            else
            {
                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(System.Windows.Application.Current.FindResource("ProjectScopeBeforeUpdate").ToString(), "Transparent", CustomMessageBox.MessageImagePath.Question, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    ISave = epcControl.UpdateProjectScope(ProjectScopeData);
                    if (ISave == true)
                    {
                        CustomMessageBox.Show(String.Format(System.Windows.Application.Current.FindResource("ProjectScopeAfterUpdated").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    }
                }
            }

            // Open pdf file.
            System.Diagnostics.Process.Start(pdfFilePath);
        }

        internal string generateProjectScopePdf(ProjectScope ProjectScopeData)
        {
            try
            {
                //* Chequeamos si existe el directorio */
                string folder = Path.GetTempPath();

                #region Unused Code
                //if (ProjectScopeData.Project.ProjectPath == null)
                //{
                //    folder = Path.GetTempPath();
                //    CustomMessageBox.Show("Is not possible create dotproject folder because project code is empty. Creating pdf file in temporary folder", "Blue", CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                //}
                //else
                //{
                //    if (!System.IO.Directory.Exists(ProjectScopeData.Project.ProjectPath + @"\Analysis"))
                //    {
                //        MessageBoxResult MessageBoxResult = CustomMessageBox.Show("Do you want to create project folder?", "Blue", CustomMessageBox.MessageImagePath.Info, MessageBoxButton.YesNo);
                //        if (MessageBoxResult == MessageBoxResult.No)
                //        {
                //            folder = Path.GetTempPath();
                //            CustomMessageBox.Show("Creating pdf file in temporary folder", "Blue", CustomMessageBox.MessageImagePath.Info, MessageBoxButton.OK);
                //        }
                //        else if (MessageBoxResult == MessageBoxResult.Yes)
                //        {
                //            Directory.CreateDirectory(ProjectScopeData.Project.ProjectPath + @"\Analysis");
                //            folder = ProjectScopeData.Project.ProjectPath + @"\Analysis\";
                //            //clsCommon.FrmMain.uc.txtRddotprojecFolderContent.Text = ProjectScopeData.DotprojectFolder;
                //        }
                //    }
                //    else
                //    {
                //        folder = ProjectScopeData.Project.ProjectPath + @"\Analysis\";
                //    }
                //}

                /////*Chequeamos si existe el pdf*/
                //if (System.IO.File.Exists(folder + ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf"))
                //{
                //    /*Si existe el pdf, guardamos un histórico en la carpeta \old*/
                //    if (!System.IO.Directory.Exists(folder + @"\Old"))
                //    {
                //        Directory.CreateDirectory(folder + @"\Old");
                //    }

                //    System.IO.File.Move(Path.Combine(folder, ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf"),
                //                        Path.Combine(folder + @"\Old\", ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf"));
                //}
                #endregion

                /*Creamos el pdf*/
                using (StringWriter html = new StringWriter())
                {
                    XslCompiledTransform xsl = new XslCompiledTransform();

                    // xsl.Load(Directory.GetCurrentDirectory() + EPC.Properties.Settings.Default.FOLDER_XSL_TEMPLATE);
                    // xsl.Load(@"E:\Repo\Working\EPC\Trunk\EPC\bin\Debug\Templates\ENG_TEMPLATE_SCOPE_EN_v1_body.xsl");

                    xsl.Load(Directory.GetCurrentDirectory() + @"\Templates\ENG_TEMPLATE_SCOPE_EN_v1_body.xsl");
                    xsl.Transform(getProjectScopeXml(ProjectScopeData), null, html);

                    using (MemoryStream stream = createPDF(html.ToString(), 30, 30, 120, 40, PageSize.A4))
                    {
                        using (PdfReader readPdf = new PdfReader(stream.ToArray()))
                        {
                            using (MemoryStream streamOut = new MemoryStream())
                            {
                                using (PdfStamper stamper = new PdfStamper(readPdf, streamOut))
                                {
                                    // Loop over the pages and add header and footer to each page
                                    for (int i = 1; i <= readPdf.NumberOfPages; i++)
                                    {
                                        iTextSharp.text.Rectangle pageSize = readPdf.GetPageSize(i);
                                        PdfContentByte contentByte = stamper.GetUnderContent(i);
                                        addHeaderProjectScopePdf(ProjectScopeData, pageSize, contentByte);

                                        // addFooterPdf(pageSize, contentByte, i, readPdf.NumberOfPages, ProjectScopeData.Ot + "_" + EPC.Properties.Settings.Default.PDF_NAME_STRING);
                                        addFooterPdf(pageSize, contentByte, i, readPdf.NumberOfPages, ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0");
                                    }
                                }

                                //using (FileStream fs = File.Create(folder + ProjectScopeData.Ot + "_" + EPC.Properties.Settings.Default.PDF_NAME_STRING + ".pdf"))
                                using (FileStream fs = File.Create(folder + ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf"))
                                {
                                    byte[] content = streamOut.ToArray();
                                    fs.Write(content, 0, (int)content.Length);

                                    //System.Diagnostics.Process.Start(folder);
                                    //System.Diagnostics.Process.Start(folder + ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf");
                                }

                                return (folder + ((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : "") + "_" + "SCOPE_EN_V1.0" + ".pdf");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CustomMessageBox.Show("Error: GenerateProjectScopePdf() : " + e.Message, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return "";
            }
        }

        internal void addFooterPdf(iTextSharp.text.Rectangle pageSize, PdfContentByte contentByte, int pageNumber, int numberOfPages, string fileName)
        {
            try
            {
                contentByte.SetColorStroke(BlueColorPdf);
                contentByte.SetLineWidth(1);
                contentByte.MoveTo(48, 30);
                contentByte.LineTo(pageSize.Width - 48, 30);
                contentByte.Stroke();

                ColumnText.ShowTextAligned(contentByte,
                    Element.ALIGN_CENTER, new Phrase(pageNumber + "/" + numberOfPages, font11), 535f, 18f, 0);

                //ColumnText.ShowTextAligned(contentByte, Element.ALIGN_CENTER, new Phrase(fileName + " - " + copyright + EPC.Properties.Settings.Default.PDF_FOOTER_STRING_2 + DateTime.Now.Year.ToString(), font11), pageSize.Width / 2, 15f, 0);
                ColumnText.ShowTextAligned(contentByte, Element.ALIGN_CENTER, new Phrase(fileName + " - " + copyright + " EMDEP " + DateTime.Now.Year.ToString(), font11), pageSize.Width / 2, 15f, 0);

                //ColumnText.ShowTextAligned(contentByte, Element.ALIGN_CENTER, new Phrase(EPC.Properties.Settings.Default.PDF_FOOTER_STRING_3, font9), 90f, 20f, 0);
                ColumnText.ShowTextAligned(contentByte, Element.ALIGN_CENTER, new Phrase("www.emdep.com", font9), 90f, 20f, 0);
            }
            catch (Exception e)
            {
                CustomMessageBox.Show("Error: addFooterPdf() : " + e.Message + "\n\n" + e.StackTrace, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                // MessageBox.Show(e.Message + "\n\n" + e.StackTrace, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        internal void addHeaderProjectScopePdf(ProjectScope project, iTextSharp.text.Rectangle pageSize, PdfContentByte contentByte)
        {
            try
            {
                float[] widths = { 40f, 30f, 30f, 20f, 18f, 40f };
                PdfPTable table = new PdfPTable(widths);
                table.TotalWidth = 500f;
                //PdfPCell header = new PdfPCell(new Phrase(EPC.Properties.Settings.Default.PDF_HEADER_STRING_1, font11));
                PdfPCell header = new PdfPCell(new Phrase("Project Scope Statement", font11));
                header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                header.Colspan = 6;
                header.HorizontalAlignment = 1;
                header.FixedHeight = 20f;
                table.AddCell(header);
                //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Directory.GetCurrentDirectory() + EPC.Properties.Settings.Default.IMG_XSL_TEMPLATE);
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Directory.GetCurrentDirectory() + @"\Templates\emdep.jpg");
                img.ScalePercent(11f);
                PdfPCell imgCell = new PdfPCell(img);
                imgCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                imgCell.Rowspan = 2;
                table.AddCell(imgCell);
                PdfPCell Cell = new PdfPCell(new Phrase("Internal ID", font9));
                //PdfPCell Cell = new PdfPCell(new Phrase(EPC.Properties.Settings.Default.PDF_HEADER_STRING_2, font9));
                Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                Cell.FixedHeight = 15f;
                table.AddCell(Cell);

                //Cell = new PdfPCell(new Phrase(EPC.Properties.Settings.Default.PDF_HEADER_STRING_3, font9));
                Cell = new PdfPCell(new Phrase("Commercial ID", font9));
                Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                //Cell = new PdfPCell(new Phrase(EPC.Properties.Settings.Default.PDF_HEADER_STRING_4, font9));
                Cell = new PdfPCell(new Phrase("Date", font9));
                Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                //Cell = new PdfPCell(new Phrase(EPC.Properties.Settings.Default.PDF_HEADER_STRING_5, font9));
                Cell = new PdfPCell(new Phrase("Version", font9));
                Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                Cell = new PdfPCell(new Phrase("Written by", font9));
                Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                Cell = new PdfPCell(new Phrase(project.Project.ProjectCode, font9));
                Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                Cell.FixedHeight = 20f;
                table.AddCell(Cell);

                //Cell = new PdfPCell(new Phrase(project.Ot, font9));
                Cell = new PdfPCell(new Phrase(((ProjectScopeData.Offer != null) ? ProjectScopeData.Offer.Code : ""), font9));
                Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                Cell = new PdfPCell(new Phrase(DateTime.Now.ToString("dd/MM/yyyy"), font9));
                Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                //Cell = new PdfPCell(new Phrase(EPC.Properties.Settings.Default.PDF_HEADER_STRING_7, font9));
                Cell = new PdfPCell(new Phrase("1.0", font9));
                Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                Cell = new PdfPCell(new Phrase(GeosApplication.Instance.ActiveUser.FullName, font9));
                Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Cell.BorderColor = BlueColorPdf;
                Cell.HorizontalAlignment = 1;
                table.AddCell(Cell);

                Cell = new PdfPCell(new Phrase(project.Project.ProjectName, font11));
                Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Cell.HorizontalAlignment = 1;
                Cell.FixedHeight = 20f;
                Cell.Colspan = 6;
                table.AddCell(Cell);

                table.WriteSelectedRows(0, 4, (pageSize.Width / 2) - (table.TotalWidth / 2), pageSize.Height - 30, contentByte);
                contentByte.RoundRectangle(((pageSize.Width / 2) - (table.TotalWidth / 2)) - 8, pageSize.Height - 110, table.TotalWidth + 20, 85f, 10f);
            }
            catch (Exception e)
            {
                CustomMessageBox.Show("Error: addHeaderProjectScopePdf() : " + e.Message + "\n\n" + e.StackTrace, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        private MemoryStream createPDF(string html, float marginLeft, float marginRight, float marginTop, float marginBottom, Rectangle pagesize)
        {
            try
            {
                MemoryStream msOutput = new MemoryStream();

                // step 1: creation of a document-object
                Document document = new Document(pagesize, marginLeft, marginRight, marginTop, marginBottom);

                // step 2:
                // we create a writer that listens to the document
                // and directs a XML-stream to a file
                PdfWriter writer = PdfWriter.GetInstance(document, msOutput);

                // step 3: we create a worker parse the document
                XMLWorkerHelper worker = XMLWorkerHelper.GetInstance();

                // step 4: we open document and start the worker on the document
                document.Open();
                worker.ParseXHtml(writer, document, new StringReader(html));
                document.Close();

                writer.Close();

                return msOutput;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show("Error: createPDF() : " + e.Message + "\n\n" + e.StackTrace, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return null;
            }
        }

        private XmlDocument getProjectScopeXml(ProjectScope project)
        {
            try
            {
                XmlDocument xml = new XmlDocument();

                XmlNode tagNode = xml.CreateNode(XmlNodeType.Element, "project", null);
                xml.AppendChild(tagNode);

                XmlNode subTagNode4 = xml.CreateNode(XmlNodeType.Element, "ScopeDescription", null);
                tagNode.AppendChild(subTagNode4);
                XmlText subTagNode4Value = xml.CreateTextNode(project.ProductScopeDescription);    //ScopeDescription);
                subTagNode4.AppendChild(subTagNode4Value);

                XmlNode subTagNode5 = xml.CreateNode(XmlNodeType.Element, "DeliverablesDescription", null);
                tagNode.AppendChild(subTagNode5);
                XmlText subTagNode5Value = xml.CreateTextNode(project.ProjectDeliverables);   // DeliverablesDescription);
                subTagNode5.AppendChild(subTagNode5Value);

                XmlNode subTagNode6 = xml.CreateNode(XmlNodeType.Element, "AcceptanceCriteriaDescription", null);
                tagNode.AppendChild(subTagNode6);
                XmlText subTagNode6Value = xml.CreateTextNode(project.ProjectAcceptanceCriteria);  //   AcceptanceCriteriaDescription);
                subTagNode6.AppendChild(subTagNode6Value);

                XmlNode subTagNode7 = xml.CreateNode(XmlNodeType.Element, "ExclusionsDescription", null);
                tagNode.AppendChild(subTagNode7);
                XmlText subTagNode7Value = xml.CreateTextNode(project.ProjectExclusions);    //    ExclusionsDescription);
                subTagNode7.AppendChild(subTagNode7Value);

                XmlNode subTagNode8 = xml.CreateNode(XmlNodeType.Element, "ConstraintsDescription", null);
                tagNode.AppendChild(subTagNode8);
                XmlText subTagNode8Value = xml.CreateTextNode(project.ProjectConstraints);  //  ConstraintsDescription);
                subTagNode8.AppendChild(subTagNode8Value);

                XmlNode subTagNode9 = xml.CreateNode(XmlNodeType.Element, "AssumptionsDescription", null);
                tagNode.AppendChild(subTagNode9);
                XmlText subTagNode9Value = xml.CreateTextNode(project.ProjectAssumptions); //  AssumptionsDescription);
                subTagNode9.AppendChild(subTagNode9Value);

                return xml;
            }
            catch (Exception e)
            {
                CustomMessageBox.Show("Error: getProjectScopeXml() : " + e.Message + "\n\n" + e.StackTrace, "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                return null;
            }
        }

        #endregion
    }
}
