using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ImportContactViewModel : NavigationViewModelBase, IDisposable
    {

        #region Declaration

        string filePath;
        bool isImport;

        #endregion

        #region  Properties
        public string FilePath
        {
            get { return filePath; }
            set
            {
                SetProperty(ref filePath, value, () => FilePath);
            }
        }
        public bool IsImport
        {
            get { return isImport; }
            set { isImport = value; }
        }
        #endregion

        #region ICommands

        public ICommand ImportContactAcceptButtonCommand { get; set; }
        public ICommand ImportContactCancelButtonCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }


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
        public ImportContactViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ImportContactViewModel...", category: Category.Info, priority: Priority.Low);
                ImportContactAcceptButtonCommand = new DelegateCommand<string>(ImportContacts);
                ImportContactCancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                OpenFolderCommand = new DelegateCommand<string>(OpenFolderCommandAction);

                GeosApplication.Instance.Logger.Log("Constructor ExportContactViewModel() executed successfully Constructor...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ExportContactViewModel() Constructor..." + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }
        #endregion

        #region Methods


        public void ImportContacts(string path)
        {

            IsImport = true;
            RequestClose(null, null);

        }

        public void OpenFolderCommandAction(string Path)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenFolderCommandAction ...", category: Category.Info, priority: Priority.Low);

                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    FilePath = openFileDialog.FileName;
                }


                GeosApplication.Instance.Logger.Log("Method OpenFolderCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in OpenFolderCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
