using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class ContractSituationSplitViewModel : INotifyPropertyChanged
    {
        public ICommand MergeAcceptButtonCommand { get; set; }
        public ICommand MergeCancelButtonCommand { get; set; }

        public EmployeeContractSituation EditEmployeeContractSituation { get; set; }

        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public bool isSave;
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        private ObservableCollection<EmployeeContractSituation> existEmployeeContractSituation;
        public ObservableCollection<EmployeeContractSituation> ExistEmployeeContractSituation
        {
            get
            {
                return existEmployeeContractSituation;
            }

            set
            {
                existEmployeeContractSituation = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExistEmployeeContractSituation"));
            }
        }

        private string contractSituationFileName;
        public string ContractSituationFileName
        {
            get
            {
                return contractSituationFileName;
            }

            set
            {
                contractSituationFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContractSituationFileName"));
            }
        }

        ContractSituationSplitView view = new ContractSituationSplitView();
        EditContractSituationView editview = new EditContractSituationView();

        public ContractSituationSplitViewModel()
        {
            GeosApplication.Instance.Logger.Log("Constructor AddLanguagesViewModel()...", category: Category.Info, priority: Priority.Low);
            MergeCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MergeAcceptButtonCommand = new RelayCommand(new Action<object>(MergeAcceptBtn));
        }

        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }

        private void MergeAcceptBtn(object obj)
        {
            try
            {
                List<string> Fileslist = new List<string>();
                string outputPdfPath = @"D:\Sana\Sample.pdf";
                string tempfilepath = CreateTempFilePath();
                CopyContentFromOriginalToTempPDFFile(outputPdfPath, tempfilepath);
                Fileslist.Add(tempfilepath);  // Intially Temparory File should be in list[0]
                Fileslist.Add(@"D:\Sana\Emdep Skype Format.pdf");
                Fileslist.Add(@"D:\Sana\wpf_tutorial.pdf");
                MergeMultiplePDFFiles(Fileslist, outputPdfPath);
                //MessageBox.Show("done");
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLanguageInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                //MessageBox.Show(ex.Message);
            }
        }

        // Create a Temp PDF File Path Method
        public string CreateTempFilePath()
        {
            return Path.Combine(Path.GetTempPath(), "Temp.pdf");
        }

        // Copy Original File to Temp File Method
        public void CopyContentFromOriginalToTempPDFFile(string originalFile, string tempFile)
        {
            try
            {
                // step 1: creation of a document-object
                Document document = new Document();
                //create newFileStream object which will be disposed at the end
                using (FileStream newFileStream = new FileStream(tempFile, FileMode.Create))
                {
                    // step 2: we create a writer that listens to the document
                    PdfCopy writer = new PdfCopy(document, newFileStream);
                    if (writer == null)
                    {
                        return;
                    }
                    // step 3: we open the document
                    document.Open();
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(originalFile);
                    reader.ConsolidateNamedDestinations();
                    // step 4: we import each page file content and add the PDFImportedPage
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }
                    PRAcroForm form = reader.AcroForm;
                    if (form != null && writer != null)
                    {
                        //writer.CopyAcroForm(reader);
                    }
                    reader.Close();
                    // step 5: we close the document
                    if (document != null)
                    {
                        document.Close();
                    }
                    // step 6: we close the writer
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLanguageInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                //MessageBox.Show(ex.Message);
            }
        }

        // Merge Multiple PDF Files including the Original file
        public void MergeMultiplePDFFiles(List<string> fileNamesList, string outFile)
        {
            try
            {
                int pageOffset = 0;
                int FilesCount = 0;
                Document document = null;
                PdfCopy writer = null;
                while (FilesCount < fileNamesList.Count)
                {
                    if (File.Exists(fileNamesList[FilesCount]))
                    {
                        // we create a reader for a certain document
                        PdfReader reader = new PdfReader(fileNamesList[FilesCount]);
                        reader.ConsolidateNamedDestinations();
                        // we retrieve the total number of pages
                        int n = reader.NumberOfPages;
                        pageOffset += n;
                        if (FilesCount == 0)
                        {
                            // step 1: creation of a document-object
                            document = new Document(reader.GetPageSizeWithRotation(1));
                            // step 2: we create a writer that listens to the document
                            writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
                            // step 3: we open the document
                            document.Open();
                        }
                        // step 4: we add content
                        for (int i = 0; i < n;)
                        {
                            ++i;
                            if (writer != null)
                            {
                                PdfImportedPage page = writer.GetImportedPage(reader, i);
                                writer.AddPage(page);
                            }
                        }
                        PRAcroForm form = reader.AcroForm;
                        if (form != null && writer != null)
                        {
                            //writer.CopyAcroForm(reader);
                        }
                    }
                    FilesCount++;
                }
                // step 5: we close the document
                if (document != null)
                {
                    document.Close();
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddLanguageInformation()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
