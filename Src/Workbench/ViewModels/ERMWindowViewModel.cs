using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.ERM.CommonClasses;
using Emdep.Geos.Modules.ERM.ViewModels;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Logging;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.File;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Workbench.ViewModels
{
    class ERMWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
		//[nsatpute][25-06-2025][GEOS2-8641]
        // Import the Windows Shell function
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern int SHOpenFolderAndSelectItems(
            IntPtr pidlFolder,
            uint cidl,
            IntPtr[] apidl,
            uint dwFlags);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern void ILFree(IntPtr pidl);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr ILCreateFromPathW([MarshalAs(UnmanagedType.LPWStr)] string pszPath);


        #region Declaration
        //IERMService ERMService = new ERMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        private string showHideMenuButtonToolTip;

        private string moduleName;
        private string moduleShortName;

        public string ShowHideMenuButtonToolTip
        {
            get { return showHideMenuButtonToolTip; }
            set
            {
                showHideMenuButtonToolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        public string ModuleName
        {
            get { return moduleName; }
            set
            {
                moduleName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        private WorkOperationsViewModel objWorkOperationsViewModel;
        public WorkOperationsViewModel ObjWorkOperationsViewModell
        {
            get { return objWorkOperationsViewModel; }
            set
            {
                objWorkOperationsViewModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ObjWorkOperationsViewModel"));
            }
        }


        public string ModuleShortName
        {
            get
            {
                return moduleShortName;
            }

            set
            {
                moduleShortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleShortName"));
            }
        }
        #endregion

        #region Public Commands

        public ICommand HideTileBarButtonClickCommand { get; set; }
        public ICommand BackButtonClickCommand { get; set; }
        public ICommand OpenFileLocationCommand { get; set; } //[nsatpute][25-06-2025][GEOS2-8641]

        #endregion  // Public Commands

        #region Constructor

        public ERMWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            BackButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(BackButtonClickCommandAction);
            OpenFileLocationCommand = new DelegateCommand<object>(OpenFileLocationCommandAction);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu

            if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
            {
                ModuleShortName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 12).Select(s => s.Acronym).FirstOrDefault();
                ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 12).Select(s => s.Name).FirstOrDefault();
            }
            else
            {
                ModuleShortName = "ERM";
                ModuleName = "EMDEP Resources Management";
            }
            GeosApplication.Instance.DownloadedReportFiles = new ObservableCollection<FileDetail>();
        }

        #endregion

        #region public Events

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // Events

        #region Methods

        private void HideTileBarButtonClickCommandAction(RoutedEventArgs obj)
        {
            if (GeosApplication.Instance.TileBarVisibility == Visibility.Collapsed)
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Visible;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString(); //Hide menu
            }
            else
            {
                GeosApplication.Instance.TileBarVisibility = Visibility.Collapsed;
                ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("ShowMenuButtonToolTip").ToString(); // ShowMenu
            }

        }

        public void BackButtonClickCommandAction(RoutedEventArgs obj)
        {
            try
            {
                if (ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged)
                    SavechangesInWorkOperationGrid();

                GeosApplication.NavigationServiceOnGeosWorkbenchScreen.GoBack();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method BackButtonClickCommandAction in PCM WindowViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void SavechangesInWorkOperationGrid()
        {
            try
            {
                if (!GeosApplication.Instance.IsAdminPermissionERM) {
                    MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["WorkOperationGridUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                    if (MessageBoxResult == MessageBoxResult.Yes)
                    {
                        if (ERMWorkOperationViewMultipleCellEditHelper.Checkview == "WorkOperationsListTableView")
                        {
                            ObjWorkOperationsViewModell.UpdateMultipleRowsWorkOperationGridCommandAction(ERMWorkOperationViewMultipleCellEditHelper.Viewtableview);
                        }
                    }
                    ERMWorkOperationViewMultipleCellEditHelper.IsValueChanged = false;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }
		//[nsatpute][25-06-2025][GEOS2-8641]
        private void OpenFileLocationCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method OpenFileLocationCommandAction ...", category: Category.Info, priority: Priority.Low);
                string filePath = ((Emdep.Geos.Data.Common.File.FileDetail)obj).FilePath;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    // For Vista and later - use the more reliable API call
                    IntPtr pidl = ILCreateFromPathW(filePath);
                    if (pidl != IntPtr.Zero)
                    {
                        try
                        {
                            SHOpenFolderAndSelectItems(pidl, 0, null, 0);
                        }
                        finally
                        {
                            ILFree(pidl);
                        }
                    }
                }
                else
                {
                    // Fallback for older Windows versions
                    string argument = "/select, \"" + filePath + "\"";
                    Process.Start("explorer.exe", argument);
                }
                GeosApplication.Instance.Logger.Log("Method OpenFileLocationCommandAction() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {                
                GeosApplication.Instance.Logger.Log("Error in OpenFileLocationCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void Dispose()
        {
        }

        #endregion // Methods






    }
    
}
