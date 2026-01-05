using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Modules.SAM.ViewModels;
using Prism.Logging;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Core;

namespace Workbench.ViewModels
{
    public class SAMWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Declaration

        private string showHideMenuButtonToolTip;

        private string moduleName;
        private string moduleShortName;

        private TableView view;

        public static TableView ColnedView;
        public WorkPlanningViewModel objWorkPlanningViewModel;

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
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleName"));
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

        #endregion  // Public Commands

        #region Constructor

        public SAMWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu

            BackButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(BackButtonClickCommandAction);

            if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
            {
                ModuleShortName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 9).Select(s => s.Acronym).FirstOrDefault();
                ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 9).Select(s => s.Name).FirstOrDefault();
            }
            else
            {

                ModuleShortName = "SAM";
                ModuleName = "Structure Assembly Management";
            }
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

        public void BackButtonClickCommandAction(RoutedEventArgs obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method BackButtonClickCommandAction...", category: Category.Info, priority: Priority.Low);

                if (MultipleCellEditHelperSAMWorkPlanning.IsValueChanged)
                {
                    SaveChangesInSAMWorkPlanning();
                }

                GeosApplication.NavigationServiceOnGeosWorkbenchScreen.GoBack();

                GeosApplication.Instance.Logger.Log("Method BackButtonClickCommandAction executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in BackButtonClickCommandAction Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        public void SaveChangesInSAMWorkPlanning()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SaveChangesInSAMWorkPlanning...", category: Category.Info, priority: Priority.Low);

                MessageBoxResult MessageBoxResult = CustomMessageBox.Show(string.Format(Application.Current.Resources["OteditUpdateWarning"].ToString()), "Yellow", CustomMessageBox.MessageImagePath.Warning, MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    if (SAMMainViewModel.ObjWorkPlanningView != null)
                    {
                        WorkPlanningViewModel workPlanningViewModel = (WorkPlanningViewModel)SAMMainViewModel.ObjWorkPlanningView.DataContext;
                        workPlanningViewModel.UpdateMultipleRowsCommandAction(MultipleCellEditHelperSAMWorkPlanning.Viewtableview);
                        MultipleCellEditHelperSAMWorkPlanning.SetIsValueChanged(MultipleCellEditHelperSAMWorkPlanning.Viewtableview, false);
                        MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;
                    }
                }
                else
                {
                    MultipleCellEditHelperSAMWorkPlanning.SetIsValueChanged(MultipleCellEditHelperSAMWorkPlanning.Viewtableview, false);
                    MultipleCellEditHelperSAMWorkPlanning.IsValueChanged = false;
                    return;
                }

                GeosApplication.Instance.Logger.Log("Method SaveChangesInSAMWorkPlanning executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in SaveChangesInSAMWorkPlanning Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

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

        public void Dispose()
        {
        }

        #endregion // Methods
    }
}
