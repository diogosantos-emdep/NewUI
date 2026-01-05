using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.SRM.ViewModels
{
    public class ExportContactViewModel : NavigationViewModelBase, IDisposable
    {

        #region Services

       
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISRMService SRMService = new SRMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
       // ISRMService SRMService = new SRMServiceController("localhost:6699");

        #endregion

        #region Declaration

        bool isVcf = true;
        bool isSuccess = false;
        bool isCommaSeparated = true;
        string filePath;
        DevExpress.Xpf.Grid.TableView tableViewObject;
        byte[] UserProfileImageByte = null;
        ObservableCollection<Contacts> peopleContacts = new ObservableCollection<Contacts>();
        ObservableCollection<Contacts> exportedContacts = new ObservableCollection<Contacts>();

        public virtual bool ShowNewFolderButton { get; set; }
        public virtual string ResultPath { get; set; }
        public IFolderBrowserDialogService FolderBrowserDialogService
        { get { return this.GetService<IFolderBrowserDialogService>(); } }

        #endregion

        #region Properties
        public bool IsVcf
        {
            get { return isVcf; }
            set
            {
                SetProperty(ref isVcf, value, () => IsVcf);
            }
        }

        public bool IsCommaSeparated
        {
            get { return isCommaSeparated; }
            set
            {
                SetProperty(ref isCommaSeparated, value, () => IsCommaSeparated);
            }
        }
        public string FilePath
        {
            get { return filePath; }
            set
            {
                SetProperty(ref filePath, value, () => FilePath);
            }
        }
        public ObservableCollection<Contacts> PeopleContacts
        {
            get { return peopleContacts; }
            set
            {
                SetProperty(ref peopleContacts, value, () => PeopleContacts);
            }
        }

        public ObservableCollection<Contacts> ExportedContacts
        {
            get { return exportedContacts; }
            set
            {
                SetProperty(ref exportedContacts, value, () => ExportedContacts);
            }
        }
        public DevExpress.Xpf.Grid.TableView TableViewObject
        {
            get { return tableViewObject; }
            set { tableViewObject = value; }
        }

        #endregion

        #region ICommands

        public ICommand ExportContactAcceptButtonCommand { get; set; }
        public ICommand ExportContactCancelButtonCommand { get; set; }
        public ICommand SaveFolderCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
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

        #region Constructor

        public ExportContactViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ExportContactViewModel...", category: Category.Info, priority: Priority.Low);
                ExportContactAcceptButtonCommand = new Prism.Commands.DelegateCommand<DevExpress.Xpf.Grid.TableView>(ExportContacts);
                ExportContactCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                SaveFolderCommand = new DelegateCommand<string>(SaveFolderCommandAction);
                CommandTextInput = new DelegateCommand<System.Windows.Input.KeyEventArgs>(ShortcutAction);
                ShowNewFolderButton = true;
                FilePath = string.Empty;

                GeosApplication.Instance.Logger.Log("Constructor ExportContactViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExportContactViewModel() Constructor...", category: Category.Info, priority: Priority.Low);
            }

        }

        #endregion

        #region Methods


        public void SaveFolderCommandAction(string Path)
        {
            GeosApplication.Instance.Logger.Log("Method SaveFolderCommandAction ...", category: Category.Info, priority: Priority.Low);

            try
            {
                if (FolderBrowserDialogService.ShowDialog())
                    ResultPath = FolderBrowserDialogService.ResultPath;
                FilePath = ResultPath;

                GeosApplication.Instance.Logger.Log("Method SaveFolderCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("SaveFolder").ToString(), "Transparent", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in SaveFolderCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }


        public void ExportContacts(DevExpress.Xpf.Grid.TableView obj)
        {
            GeosApplication.Instance.Logger.Log("Method ExportContacts ...", category: Category.Info, priority: Priority.Low);

            if (IsVcf)
            {
                ExportTovCards(TableViewObject);

            }
            else
            {
                ExportToCsv(TableViewObject);
            }

            RequestClose(null, null);

            GeosApplication.Instance.Logger.Log("Method ExportContacts() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public void ExportTovCards(DevExpress.Xpf.Grid.TableView obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ExportTovCards Method...", category: Category.Info, priority: Priority.Low);
                ExportedContacts = new ObservableCollection<Data.Common.SRM.Contacts>();
                if (obj is DevExpress.Xpf.Grid.TableView)
                {
                    GridControl grid = ((DevExpress.Xpf.Grid.TableView)(obj)).Grid;
                    for (int i = 0; i < ((DevExpress.Xpf.Grid.TableView)(obj)).Grid.VisibleRowCount; i++)
                    {
                        object row = grid.GetRow(i);
                        Contacts objPeople = new Contacts();
                        objPeople = (Contacts)row;
                        ExportedContacts.Add(objPeople);
                    }
                }



                if (ExportedContacts.Count > 0)
                {
                    foreach (var item in ExportedContacts)
                    {
                        var vcf = new StringBuilder();
                        vcf.AppendLine("BEGIN:VCARD");
                        vcf.AppendLine("VERSION:2.1");
                        // Name
                        vcf.AppendLine("N:" + item.Firstname + ";" + item.Lastname);
                        // Full name
                        vcf.AppendLine("FN:" + item.FullName);
                        vcf.AppendLine("ORG:" + item.CompanyDepartment.Value);
                        vcf.AppendLine("TITLE:" + item.JobTitle);
                        // Address
                        vcf.Append("ADR;HOME;PREF:;;");
                        vcf.Append(item.Address + ";");
                        vcf.Append(item.City + ";;");
                        vcf.Append(item.PostCode + ";");
                        vcf.Append(item.CountryList.Name);
                        // Other data
                        vcf.AppendLine("TEL;WORK;VOICE:" + item.Phone);
                        vcf.AppendLine("TEL;CELL;VOICE:" + item.Phone2);
                       // vcf.AppendLine("URL:" + item.Company.Website);
                        vcf.AppendLine("EMAIL;PREF;INTERNET:" + item.Email);

                        vcf.AppendLine("END:VCARD");

                        var filename = item.FullName + ".vcf";

                        try
                        {
                            string path = FilePath + '\\' + filename;

                            if (!File.Exists(path))
                            {
                                File.WriteAllText(path, vcf.ToString());
                            }

                            GeosApplication.Instance.Logger.Log("Method ExportTovCards() executed successfully", category: Category.Info, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log("Get an error in ExportTovCards() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
                        }

                    }
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ExportContactViewSuccessMessage").ToString()), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                    //isSuccess = true;
                }
                else
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ExportContactViewFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                }
                GeosApplication.Instance.Logger.Log("ExportTovCards Method() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ExportContactViewFailMessageIllegalChar").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                GeosApplication.Instance.Logger.Log("Get an error in ExportTovCards Method()...", category: Category.Info, priority: Priority.Low);

            }
        }

        private void ExportToCsv(DevExpress.Xpf.Grid.TableView obj)
        {
            GeosApplication.Instance.Logger.Log("Method ExportToCsv ...", category: Category.Info, priority: Priority.Low);

            if (IsCommaSeparated)
            {
                obj.ExportToCsv(FilePath + ".csv", new DevExpress.XtraPrinting.CsvExportOptions(";", Encoding.Default));

            }
            else
            {
                obj.ExportToCsv(FilePath + ".csv", new DevExpress.XtraPrinting.CsvExportOptions(",", Encoding.Default));
            }

            GeosApplication.Instance.Logger.Log("Method ExportToCsv() executed successfully", category: Category.Info, priority: Priority.Low);

        }



        protected override void OnParameterChanged(object parameter)
        {
            TableViewObject = (DevExpress.Xpf.Grid.TableView)parameter;
            base.OnParameterChanged(parameter);
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        private void ShortcutAction(System.Windows.Input.KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (GeosApplication.Instance.IsPermissionReadOnly)
                {
                    return;
                }
                SRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
