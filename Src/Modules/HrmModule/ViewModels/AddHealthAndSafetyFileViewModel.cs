using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Validations;
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

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddHealthAndSafetyFileViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services

        #endregion

        #region Events
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

        #region Declaration
        private string windowHeader;
        string[] fileUploadDargDropFromFileExplorer;
        private byte[] fileInBytes;
        private bool isNew;
        private bool isSave;
        private List<Company> plantList;
        private List<object> selectedPlantList;
        private string healthAndSafetyTitle;
        private string healthAndSafetyRemarks;
        private ObservableCollection<HealthAndSafetyAttachedDoc> healthAndSafetyAttachmentList;
        private string healthAndSafetySavedFileName;
        private string fileName;
        string FileTobeSavedByName = "";
        private List<object> attachmentObjectList;
        private HealthAndSafetyAttachedDoc selectedHealthAndSafetyFile;
        private string error = string.Empty;
        private string editPlantName;
        private string editPlantIds;
        #endregion

        #region Properties
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public string[] HealthAndSafetyFileUploadDargDropFromFileExplorer
        {
            get { return fileUploadDargDropFromFileExplorer; }
            set
            {
                fileUploadDargDropFromFileExplorer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileUploadDargDropFromFileExplorer"));
                BrowseFileActionFileUploaded(HealthAndSafetyFileUploadDargDropFromFileExplorer);
            }
        }

        public byte[] FileInBytes
        {
            get
            {
                return fileInBytes;
            }

            set
            {
                fileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileInBytes"));
            }
        }

        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
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

        public List<Company> PlantList
        {
            get { return plantList; }
            set
            {
                plantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PlantList"));
            }
        }

        public List<object> SelectedPlantList
        {
            get { return selectedPlantList; }
            set
            {
                var bothlistsItemsAreEqual = true;

                if (value == null)
                {
                    selectedPlantList = null;
                }
                else if (selectedPlantList == null ||
                    selectedPlantList.Count != value.Count
                    )
                {
                    bothlistsItemsAreEqual = false;
                }
                else
                {
                    foreach (var item1 in selectedPlantList)
                    {
                        var item1IdCompany = ((Company)item1).IdCompany;
                        var founditem1IdCompanyInValueList = false;
                        foreach (var item2 in value)
                        {
                            var item2IdCompany = ((Company)item2).IdCompany;
                            if (item1IdCompany == item2IdCompany)
                            {
                                founditem1IdCompanyInValueList = true;
                            }

                        }
                        if (!founditem1IdCompanyInValueList)
                        {
                            bothlistsItemsAreEqual = false;
                            break;
                        }

                    }

                    if (!bothlistsItemsAreEqual)
                    {
                        foreach (var item1 in value)
                        {
                            var item1IdCompany = ((Company)item1).IdCompany;
                            var founditem1IdCompanyInValueList = false;
                            foreach (var item2 in selectedPlantList)
                            {
                                var item2IdCompany = ((Company)item2).IdCompany;
                                if (item1IdCompany == item2IdCompany)
                                {
                                    founditem1IdCompanyInValueList = true;
                                }

                            }
                            if (!founditem1IdCompanyInValueList)
                            {
                                bothlistsItemsAreEqual = false;
                                break;
                            }

                        }
                    }
                }

                if (!bothlistsItemsAreEqual)
                {
                    selectedPlantList = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedPlantList"));
                }
            }
        }

        public string HealthAndSafetyTitle
        {
            get { return healthAndSafetyTitle; }
            set
            {
                healthAndSafetyTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HealthAndSafetyTitle"));
            }
        }

        public string HealthAndSafetyRemarks
        {
            get { return healthAndSafetyRemarks; }
            set
            {
                healthAndSafetyRemarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HealthAndSafetyRemarks"));
            }
        }

        public ObservableCollection<HealthAndSafetyAttachedDoc> HealthAndSafetyAttachmentList
        {
            get { return healthAndSafetyAttachmentList; }
            set
            {
                healthAndSafetyAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HealthAndSafetyAttachmentList"));
            }
        }

        public string HealthAndSafetySavedFileName
        {
            get { return healthAndSafetySavedFileName; }
            set
            {
                healthAndSafetySavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HealthAndSafetySavedFileName"));
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

        public List<object> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }

        public HealthAndSafetyAttachedDoc SelectedHealthAndSafetyFile
        {
            get { return selectedHealthAndSafetyFile; }
            set
            {
                selectedHealthAndSafetyFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedHealthAndSafetyFile"));
            }
        }

        public string EditPlantName
        {
            get { return editPlantName; }
            set
            {
                editPlantName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditPlantName"));
            }
        }

        public string EditPlantIds
        {
            get { return editPlantIds; }
            set
            {
                editPlantIds = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EditPlantIds"));
            }
        }
        #endregion 

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }

        public ICommand AcceptFileActionCommand { get; set; }
        #endregion

        #region Constructor
        public AddHealthAndSafetyFileViewModel()
        {
            AcceptFileActionCommand = new RelayCommand(new Action<object>(AcceptFileActionCommandAction));
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ChooseFileActionCommand = new RelayCommand(new Action<object>(BrowseFileAction));
        }
        #endregion

        #region Method
        public void EditInit(HealthAndSafetyAttachedDoc healthAndSafetyAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                // IdCPTypeAttachedDoc = productTypeAttachedDoc.IdCPTypeAttachedDoc;
                FileName = healthAndSafetyAttachedDoc.OriginalFileName;
                HealthAndSafetyRemarks = healthAndSafetyAttachedDoc.Remarks;
                HealthAndSafetySavedFileName = healthAndSafetyAttachedDoc.SavedFileName;
                FileInBytes = healthAndSafetyAttachedDoc.HealthAndSafetyAttachedDocInBytes;
                HealthAndSafetyTitle = healthAndSafetyAttachedDoc.Title;
                PlantList = new List<Company>();
                // PlantList.Insert(0, new Company() { ShortName = "---" });
                PlantList.AddRange(HrmCommon.Instance.UserAuthorizedPlantsList);
                if (healthAndSafetyAttachedDoc.Plant == "All")
                {
                    SelectedPlantList = new List<object>(PlantList);
                }
                else
                {
                    var plantIds = healthAndSafetyAttachedDoc.IdPlants.Split(',');
                    var trimmedPlantIds = plantIds.Select(name => name.Trim()).ToList();
                    var plantNames = healthAndSafetyAttachedDoc.Plant.Split(',');
                    var trimmedPlantNames = plantNames.Select(name => name.Trim()).ToList();
                    SelectedPlantList = new List<object>(PlantList.Where(x => trimmedPlantIds.Contains(x.IdCompany.ToString())));
                }

                if (healthAndSafetyAttachedDoc.SavedFileName!=null||!string.IsNullOrEmpty(healthAndSafetyAttachedDoc.SavedFileName))
                {
                    HealthAndSafetyAttachmentList = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                    HealthAndSafetyAttachmentList.Add(healthAndSafetyAttachedDoc);

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add((object)healthAndSafetyAttachedDoc);
                    SelectedHealthAndSafetyFile = healthAndSafetyAttachedDoc;
                }
             

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptFileActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptFileActionCommandAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("HealthAndSafetyTitle"));
                // PropertyChanged(this, new PropertyChangedEventArgs("SelectedHealthAndSafetyFile"));

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                HealthAndSafetyRemarks = HealthAndSafetyRemarks == null ? "" : HealthAndSafetyRemarks;

                if (HealthAndSafetyRemarks != null)
                {
                    if (HealthAndSafetyRemarks.Contains("\r\n"))
                    {
                        HealthAndSafetyRemarks = HealthAndSafetyRemarks.TrimEnd(trimChars);
                        HealthAndSafetyRemarks = HealthAndSafetyRemarks.TrimStart(trimChars);
                    }
                }

                if (IsNew)
                {
                    SelectedHealthAndSafetyFile = new HealthAndSafetyAttachedDoc();
                    if (AttachmentObjectList == null || AttachmentObjectList.Count == 0)
                    {
                        FileInBytes = null;
                    }

                    if (!string.IsNullOrEmpty(HealthAndSafetySavedFileName) && FileInBytes != null)
                    {
                        if (string.IsNullOrEmpty(FileName))
                        {
                            int index = HealthAndSafetySavedFileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? HealthAndSafetySavedFileName : HealthAndSafetySavedFileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        SelectedHealthAndSafetyFile.OriginalFileName = FileName;
                        SelectedHealthAndSafetyFile.SavedFileName = HealthAndSafetySavedFileName;
                        SelectedHealthAndSafetyFile.HealthAndSafetyAttachedDocInBytes = FileInBytes;
                    }


                    SelectedHealthAndSafetyFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedHealthAndSafetyFile.Remarks = HealthAndSafetyRemarks;
                    SelectedHealthAndSafetyFile.Title = HealthAndSafetyTitle;
                    if (SelectedPlantList != null)
                    {
                        List<Company> plantOwners = SelectedPlantList.Cast<Company>().ToList();
                        var plantIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                        var plantname = string.Join(",", plantOwners.Select(i => i.ShortName));
                        if (plantname != null)
                        {
                            if (plantname != "---")
                            {
                                if (plantOwners.Count() == HrmCommon.Instance.UserAuthorizedPlantsList.Count())
                                {
                                    SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                                    SelectedHealthAndSafetyFile.Plant = "All";
                                    EditPlantName = "All";
                                    EditPlantIds = string.Empty;
                                }
                                else
                                {
                                    if (plantname != "---")
                                    {
                                        SelectedHealthAndSafetyFile.IdPlants = plantIds;
                                        SelectedHealthAndSafetyFile.Plant = plantname;
                                    }
                                    else
                                    {
                                        SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                                        SelectedHealthAndSafetyFile.Plant = "All";
                                    }
                                }
                            }
                            else
                            {
                                SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                                SelectedHealthAndSafetyFile.Plant = "All";
                            }
                        }
                    }
                    else
                    {
                        SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                        SelectedHealthAndSafetyFile.Plant = "All";
                    }
                    IsSave = true;
                }
                else
                {
                    SelectedHealthAndSafetyFile = new HealthAndSafetyAttachedDoc();

                    if (AttachmentObjectList==null|| AttachmentObjectList.Count==0)
                    {
                        FileInBytes = null;
                    }

                    if (!string.IsNullOrEmpty(HealthAndSafetySavedFileName))
                    {
                        if (string.IsNullOrEmpty(FileName))
                        {
                            int index = HealthAndSafetySavedFileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? HealthAndSafetySavedFileName : HealthAndSafetySavedFileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        SelectedHealthAndSafetyFile.OriginalFileName = FileName;
                        SelectedHealthAndSafetyFile.SavedFileName = HealthAndSafetySavedFileName;
                    }

                    SelectedHealthAndSafetyFile.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedHealthAndSafetyFile.HealthAndSafetyAttachedDocInBytes = FileInBytes;
                    if (FileInBytes == null)
                    {
                        SelectedHealthAndSafetyFile.OriginalFileName = string.Empty;
                        SelectedHealthAndSafetyFile.SavedFileName = string.Empty;
                    }
                    SelectedHealthAndSafetyFile.Remarks = HealthAndSafetyRemarks;
                    SelectedHealthAndSafetyFile.Title = HealthAndSafetyTitle;
                    if (SelectedPlantList != null)
                    {
                        List<Company> plantOwners = SelectedPlantList.Cast<Company>().ToList();
                        var plantIds = string.Join(",", plantOwners.Select(i => i.IdCompany));
                        var plantname = string.Join(",", plantOwners.Select(i => i.ShortName));
                        if (plantname != null)
                        {
                            if (plantname != "---")
                            {
                                if (plantOwners.Count() == HrmCommon.Instance.UserAuthorizedPlantsList.Count())
                                {
                                    SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                                    SelectedHealthAndSafetyFile.Plant = "All";
                                    EditPlantName = "All";
                                    EditPlantIds = string.Empty;
                                }
                                else
                                {
                                    if (plantname != "---")
                                    {
                                        SelectedHealthAndSafetyFile.IdPlants = plantIds;
                                        SelectedHealthAndSafetyFile.Plant = plantname;
                                        EditPlantName = plantname;
                                        EditPlantIds = plantIds;
                                    }
                                    else
                                    {
                                        SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                                        SelectedHealthAndSafetyFile.Plant = "All";
                                        EditPlantName = "All";
                                        EditPlantIds = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                                SelectedHealthAndSafetyFile.Plant = "All";
                                EditPlantName = "All";
                                EditPlantIds = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        SelectedHealthAndSafetyFile.IdPlants = string.Empty;
                        SelectedHealthAndSafetyFile.Plant = "All";
                        EditPlantName = "All";
                        EditPlantIds = string.Empty;
                    }
                    IsSave = true;
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptFileActionCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptFileActionCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            PlantList = new List<Company>();
            // PlantList.Insert(0, new Company() { ShortName = "---" });
            PlantList.AddRange(HrmCommon.Instance.UserAuthorizedPlantsList);
            //  SelectedPlantList = new List<object>(PlantList.Where(x => x.ShortName == "---"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                #region Old Code
                //pramod.misal 18.06.2024
                //dlg.DefaultExt = "*.*"; // Default extension if none is selected
                //dlg.Filter = "All Supported Files|*.pdf;*.tif;*.tiff;*.jpg;*.jpeg;*.docx;*.png|PDF Files|*.pdf|TIFF Files|*.tif;*.tiff|Image Files|*.jpg;*.jpeg;*.png|Word Documents|*.docx";
                #endregion
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    HealthAndSafetyAttachmentList = new ObservableCollection<HealthAndSafetyAttachedDoc>();


                    FileInfo file = new FileInfo(dlg.FileName);
                    HealthAndSafetySavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<HealthAndSafetyAttachedDoc> newAttachmentList = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                    HealthAndSafetyAttachedDoc attachment = new HealthAndSafetyAttachedDoc();
                    attachment.SavedFileName = file.Name;
                    attachment.HealthAndSafetyAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    HealthAndSafetyAttachmentList = newAttachmentList;

                    if (HealthAndSafetyAttachmentList.Count > 0)
                    {
                        SelectedHealthAndSafetyFile = HealthAndSafetyAttachmentList[0];
                    }
                }
                GeosApplication.Instance.Logger.Log("Method BrowseFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void BrowseFileActionFileUploaded(string[] fileUploaded)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);
            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                foreach (string item in fileUploaded)
                {
                    string extension = System.IO.Path.GetExtension(item);
                    if (extension == ".pdf" || extension == ".tif" || extension == ".tiff" || extension == ".jpg" || extension == ".jpeg" || extension == ".docx" || extension == ".png")
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(item);
                        HealthAndSafetyAttachmentList = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                        FileInfo file = new FileInfo(item);
                        HealthAndSafetySavedFileName = file.Name;
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                        ObservableCollection<HealthAndSafetyAttachedDoc> newAttachmentList = new ObservableCollection<HealthAndSafetyAttachedDoc>();
                        HealthAndSafetyAttachedDoc attachment = new HealthAndSafetyAttachedDoc();
                        attachment.SavedFileName = file.Name;
                        attachment.HealthAndSafetyAttachedDocInBytes = FileInBytes;
                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);
                        newAttachmentList.Add(attachment);
                        HealthAndSafetyAttachmentList = newAttachmentList;
                        if (HealthAndSafetyAttachmentList.Count > 0)
                        {
                            SelectedHealthAndSafetyFile = HealthAndSafetyAttachmentList[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method BrowseFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Validations
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

                string error =
                    me[BindableBase.GetPropertyName(() => HealthAndSafetyTitle)];

                // +me[BindableBase.GetPropertyName(() => SelectedHealthAndSafetyFile)]

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

                string healthAndSafetyTitle = BindableBase.GetPropertyName(() => HealthAndSafetyTitle);
                //string selectedHealthAndSafetyFile = BindableBase.GetPropertyName(() => SelectedHealthAndSafetyFile);

                if (columnName == healthAndSafetyTitle)
                {
                    return AddEditHealthAndSafetyValidation.GetErrorMessage(healthAndSafetyTitle, HealthAndSafetyTitle);
                }
                //if (columnName == selectedHealthAndSafetyFile)
                //{
                //    return AddEditHealthAndSafetyValidation.GetErrorMessage(selectedHealthAndSafetyFile, SelectedHealthAndSafetyFile);
                //}
                return null;
            }
        }

        #endregion

    }
}
