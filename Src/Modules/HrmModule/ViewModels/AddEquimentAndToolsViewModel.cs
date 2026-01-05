using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Modules.Hrm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
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
    public class AddEquimentAndToolsViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        #region Declarations
        private string windowHeader;
        string[] fileUploadDargDropFromFileExplorer;
        private byte[] fileInBytes;
        private bool isNew;
        private bool isSave;
        private bool isMandatory;
        private DateTime? startDate;
        private DateTime? endDate;
        private string equipmentAndToolsRemarks;
        private ObservableCollection<EquipmentAndToolsAttachedDoc> equipmentAndToolsAttachmentList;
        private string equipmentAndToolsSavedFileName;
        private string fileName;
        string FileTobeSavedByName = "";
        private List<object> attachmentObjectList;
        private EquipmentAndToolsAttachedDoc selectedEquipmentAndToolsFile;
        private string error = string.Empty;

        private string startDateErrorMessage = string.Empty;
        private string endDateErrorMessage = string.Empty;
        private List<LookupValue> equipmentList;
        private LookupValue selectedEquipment;
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

        public string[] EquipmentAndToolsFileUploadDargDropFromFileExplorer
        {
            get { return fileUploadDargDropFromFileExplorer; }
            set
            {
                fileUploadDargDropFromFileExplorer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileUploadDargDropFromFileExplorer"));
                BrowseFileActionFileUploaded(EquipmentAndToolsFileUploadDargDropFromFileExplorer);
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

        public bool IsMandatory
        {
            get { return isMandatory; }
            set
            {
                isMandatory = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMandatory"));
            }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        public string EquipmentAndToolsRemarks
        {
            get { return equipmentAndToolsRemarks; }
            set
            {
                equipmentAndToolsRemarks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquipmentAndToolsRemarks"));
            }
        }

        public ObservableCollection<EquipmentAndToolsAttachedDoc> EquipmentAndToolsAttachmentList
        {
            get { return equipmentAndToolsAttachmentList; }
            set
            {
                equipmentAndToolsAttachmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquipmentAndToolsAttachmentList"));
            }
        }

        public string EquipmentAndToolsSavedFileName
        {
            get { return equipmentAndToolsSavedFileName; }
            set
            {
                equipmentAndToolsSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquipmentAndToolsSavedFileName"));
            }
        }

        public string FileName
        {
            get { return fileName; }
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

        public EquipmentAndToolsAttachedDoc SelectedEquipmentAndToolsFile
        {
            get { return selectedEquipmentAndToolsFile; }
            set
            {
                selectedEquipmentAndToolsFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEquipmentAndToolsFile"));
            }
        }

        public List<LookupValue> EquipmentList
        {
            get { return equipmentList; }
            set
            {
                equipmentList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquipmentList"));
            }
        }

        public LookupValue SelectedEquipment
        {
            get { return selectedEquipment; }
            set
            {
                selectedEquipment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEquipment"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }

        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand AcceptFileActionCommand { get; set; }

        public ICommand OnDateEditValueChangingCommand { get; set; }
        #endregion

        #region Constructor
        public AddEquimentAndToolsViewModel()
        {
            AcceptFileActionCommand = new DelegateCommand<object>(EquipmentAndToolsFileAction);
            CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);
            OnDateEditValueChangingCommand = new DelegateCommand<EditValueChangingEventArgs>(OnDateEditValueChanging);
        }
        #endregion

        #region Methods
        public void EditInit(EquipmentAndToolsAttachedDoc equipmentAndToolsAttachedDoc)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                FileName = equipmentAndToolsAttachedDoc.OriginalFileName;
                EquipmentAndToolsRemarks = equipmentAndToolsAttachedDoc.Remarks;
                EquipmentAndToolsSavedFileName = equipmentAndToolsAttachedDoc.SavedFileName;
                FileInBytes = equipmentAndToolsAttachedDoc.EquipmentAndToolsAttachedDocInBytes;
                FillEquipmentList();
                SelectedEquipment = EquipmentList.FirstOrDefault(x => x.IdLookupValue == equipmentAndToolsAttachedDoc.IdLookupValue);
                StartDate = equipmentAndToolsAttachedDoc.StartDate;
                EndDate = equipmentAndToolsAttachedDoc.EndDate;
                IsMandatory = equipmentAndToolsAttachedDoc.IsMandatory;

                if (equipmentAndToolsAttachedDoc.SavedFileName!=null||!string.IsNullOrEmpty(equipmentAndToolsAttachedDoc.SavedFileName))
                {
                    EquipmentAndToolsAttachmentList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();
                    EquipmentAndToolsAttachmentList.Add(equipmentAndToolsAttachedDoc);

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add((object)equipmentAndToolsAttachedDoc);
                    SelectedEquipmentAndToolsFile = equipmentAndToolsAttachedDoc;
                }

               
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void EquipmentAndToolsFileAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EquipmentAndToolsFileAction()...", category: Category.Info, priority: Priority.Low);
                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedEquipment"));

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                EquipmentAndToolsRemarks = EquipmentAndToolsRemarks == null ? "" : EquipmentAndToolsRemarks;
                if (EquipmentAndToolsRemarks != null)
                {
                    if (EquipmentAndToolsRemarks.Contains("\r\n"))
                    {
                        EquipmentAndToolsRemarks = EquipmentAndToolsRemarks.TrimEnd(trimChars);
                        EquipmentAndToolsRemarks = EquipmentAndToolsRemarks.TrimStart(trimChars);
                    }
                }

                if (IsNew)
                {
                    SelectedEquipmentAndToolsFile = new EquipmentAndToolsAttachedDoc();
                    if (AttachmentObjectList == null || AttachmentObjectList.Count == 0)
                    {
                        FileInBytes = null;
                    }
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        if (string.IsNullOrEmpty(FileName))
                        {
                            int index = EquipmentAndToolsSavedFileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? EquipmentAndToolsSavedFileName : EquipmentAndToolsSavedFileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        SelectedEquipmentAndToolsFile.OriginalFileName = FileName;
                        SelectedEquipmentAndToolsFile.SavedFileName = EquipmentAndToolsSavedFileName;
                    }

                    SelectedEquipmentAndToolsFile.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedEquipmentAndToolsFile.EquipmentAndToolsAttachedDocInBytes = FileInBytes;
                    SelectedEquipmentAndToolsFile.Remarks = EquipmentAndToolsRemarks;
                    SelectedEquipmentAndToolsFile.StartDate = StartDate;
                    SelectedEquipmentAndToolsFile.EndDate = EndDate;
                    SelectedEquipmentAndToolsFile.IsMandatory = IsMandatory;
                    if (SelectedEquipment != null)
                    {
                        SelectedEquipmentAndToolsFile.IdLookupValue = SelectedEquipment.IdLookupValue;
                        SelectedEquipmentAndToolsFile.EquipmentType = SelectedEquipment.EquipmentType;
                        SelectedEquipmentAndToolsFile.IdParent = SelectedEquipment.IdParent ?? 0;
                        SelectedEquipmentAndToolsFile.CategoryType = SelectedEquipment.CategoryType;
                    }


                    IsSave = true;
                }
                else
                {

                    SelectedEquipmentAndToolsFile = new EquipmentAndToolsAttachedDoc();
                    if (AttachmentObjectList == null || AttachmentObjectList.Count == 0)
                    {
                        FileInBytes = null;
                    }
                    if (!string.IsNullOrEmpty(FileName))
                    {
                        if (string.IsNullOrEmpty(FileName))
                        {
                            int index = EquipmentAndToolsSavedFileName.LastIndexOf('.');
                            FileTobeSavedByName = index == -1 ? EquipmentAndToolsSavedFileName : EquipmentAndToolsSavedFileName.Substring(0, index);
                            FileName = FileTobeSavedByName;
                        }
                        SelectedEquipmentAndToolsFile.OriginalFileName = FileName;
                        SelectedEquipmentAndToolsFile.SavedFileName = EquipmentAndToolsSavedFileName;
                    }

                    SelectedEquipmentAndToolsFile.ModifiedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedEquipmentAndToolsFile.EquipmentAndToolsAttachedDocInBytes = FileInBytes;
                    if (FileInBytes==null)
                    {
                        SelectedEquipmentAndToolsFile.OriginalFileName = string.Empty;
                        SelectedEquipmentAndToolsFile.SavedFileName = string.Empty;
                    }
                    SelectedEquipmentAndToolsFile.Remarks = EquipmentAndToolsRemarks;
                    SelectedEquipmentAndToolsFile.StartDate = StartDate;
                    SelectedEquipmentAndToolsFile.EndDate = EndDate;
                    SelectedEquipmentAndToolsFile.IsMandatory = IsMandatory;
                    if (SelectedEquipment != null)
                    {
                        SelectedEquipmentAndToolsFile.IdLookupValue = SelectedEquipment.IdLookupValue;
                        SelectedEquipmentAndToolsFile.EquipmentType = SelectedEquipment.EquipmentType;
                        SelectedEquipmentAndToolsFile.IdParent = SelectedEquipment.IdParent ?? 0;
                        SelectedEquipmentAndToolsFile.CategoryType = SelectedEquipment.CategoryType;
                    }


                    IsSave = true;
                }

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method EquipmentAndToolsFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EquipmentAndToolsFileAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMonths(6).Date;
            FillEquipmentList();
        }

        private void FillEquipmentList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillEquipmentList()...", category: Category.Info, priority: Priority.Low);
                IList<LookupValue> temp = CrmStartUp.GetLookupValues(149);


                if (EquipmentList == null)
                {
                    EquipmentList = new List<LookupValue>();
                }

                Dictionary<int, string> categoryTypeMapping = new Dictionary<int, string>();
                foreach (var item in temp)
                {
                    if (item.IdParentNew == 0 || item.IdParentNew == null)
                    {
                        LookupValue lv = new LookupValue();
                        lv = item;
                        lv.CategoryType = item.Value;
                        if (item.IdLookupValue != null)
                        {
                            categoryTypeMapping[item.IdLookupValue] = item.Value;
                        }
                    }
                }
                foreach (var item in temp)
                {
                    if (item.IdParentNew != 0 && item.IdParentNew != null)
                    {
                        LookupValue lv = new LookupValue();
                        lv = item;
                        lv.EquipmentType = item.Value;
                        if (categoryTypeMapping.ContainsKey(item.IdParentNew.Value))
                        {
                            lv.CategoryType = categoryTypeMapping[item.IdParentNew.Value];
                        }

                        EquipmentList.Add(lv);
                    }
                }
                //  SelectedEquipment = EquipmentList.FirstOrDefault();

                if (IsNew == true)
                {
                    EquipmentList = new List<LookupValue>(EquipmentList.Where(x => x.InUse == true));
                }

                GeosApplication.Instance.Logger.Log("Method FillEquipmentList()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method FillEquipmentList()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);

            DXSplashScreen.Show<SplashScreenView>();
            if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = "Pdf Files|*.pdf";
                dlg.Filter = "Pdf Files|*.pdf";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    EquipmentAndToolsAttachmentList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    EquipmentAndToolsSavedFileName = file.Name;
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<EquipmentAndToolsAttachedDoc> newAttachmentList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();
                    EquipmentAndToolsAttachedDoc attachment = new EquipmentAndToolsAttachedDoc();
                    attachment.SavedFileName = file.Name;
                    attachment.EquipmentAndToolsAttachedDocInBytes = FileInBytes;

                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(attachment);

                    newAttachmentList.Add(attachment);
                    EquipmentAndToolsAttachmentList = newAttachmentList;

                    if (EquipmentAndToolsAttachmentList.Count > 0)
                    {
                        SelectedEquipmentAndToolsFile = EquipmentAndToolsAttachmentList[0];
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
                    if (extension == ".pdf")
                    {
                        FileInBytes = System.IO.File.ReadAllBytes(item);
                        EquipmentAndToolsAttachmentList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();
                        FileInfo file = new FileInfo(item);
                        EquipmentAndToolsSavedFileName = file.Name;
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                        ObservableCollection<EquipmentAndToolsAttachedDoc> newAttachmentList = new ObservableCollection<EquipmentAndToolsAttachedDoc>();
                        EquipmentAndToolsAttachedDoc attachment = new EquipmentAndToolsAttachedDoc();
                        attachment.SavedFileName = file.Name;
                        attachment.EquipmentAndToolsAttachedDocInBytes = FileInBytes;
                        AttachmentObjectList = new List<object>();
                        AttachmentObjectList.Add(attachment);
                        newAttachmentList.Add(attachment);
                        EquipmentAndToolsAttachmentList = newAttachmentList;
                        if (EquipmentAndToolsAttachmentList.Count > 0)
                        {
                            SelectedEquipmentAndToolsFile = EquipmentAndToolsAttachmentList[0];
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

        private void OnDateEditValueChanging(EditValueChangingEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging ...", category: Category.Info, priority: Priority.Low);

                startDateErrorMessage = string.Empty;

                if (StartDate != null && EndDate != null)
                {
                    if (StartDate.Value.Date > EndDate.Value.Date)
                    {
                        startDateErrorMessage = System.Windows.Application.Current.FindResource("AddHolidayStartDateError").ToString();
                        endDateErrorMessage = System.Windows.Application.Current.FindResource("AddHolidayEndDateError").ToString();
                    }
                    else
                    {
                        startDateErrorMessage = string.Empty;
                        endDateErrorMessage = string.Empty;
                    }

                }
                else
                {
                    startDateErrorMessage = string.Empty;
                    endDateErrorMessage = string.Empty;
                }



                GeosApplication.Instance.Logger.Log("Method OnDateEditValueChanging() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method OnDateEditValueChanging()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
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
                    me[BindableBase.GetPropertyName(() => StartDate)] +
                    me[BindableBase.GetPropertyName(() => EndDate)] +
                    me[BindableBase.GetPropertyName(() => SelectedEquipment)];


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

                string startDate = BindableBase.GetPropertyName(() => StartDate);
                string endDate = BindableBase.GetPropertyName(() => EndDate);
                string selectedEquipment = BindableBase.GetPropertyName(() => SelectedEquipment);

                if (columnName == startDate)
                {
                    if (!string.IsNullOrEmpty(startDateErrorMessage))
                    {
                        return startDateErrorMessage;
                    }
                    else
                    {
                        return AddEditEquipmentAndToolsValidations.GetErrorMessage(startDate, StartDate);
                    }

                }

                if (columnName == endDate)
                {
                    if (!string.IsNullOrEmpty(endDateErrorMessage))
                    {
                        return endDateErrorMessage;
                    }
                    else
                    {
                        return AddEditEquipmentAndToolsValidations.GetErrorMessage(endDate, EndDate);
                    }
                }


                if (columnName == selectedEquipment)
                {
                    return AddEditEquipmentAndToolsValidations.GetErrorMessage(selectedEquipment, SelectedEquipment);
                }

                return null;
            }
        }
        #endregion
    }
}
