using DevExpress.Mvvm;
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

namespace Workbench.ViewModels
{
    public class TSMWindowViewModel : ViewModelBase, INotifyPropertyChanged
    { 
        // [pallavi.kale][07-01-2025] [GEOS2-5385]
        #region Declarations

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
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleName"));
            }
        }

        public string ModuleShortName
        {
            get { return moduleShortName; }
            set
            {
                moduleShortName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModuleShortName"));
            }
        }


        #endregion

        #region Public Commands

        public ICommand HideTileBarButtonClickCommand { get; set; }

        #endregion

        #region Constructor
        public TSMWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu

            if (GeosApplication.Instance.ObjectPool.ContainsKey("GeosModuleNameList"))
            {
                ModuleShortName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 13).Select(s => s.Acronym).FirstOrDefault();
                ModuleName = ((List<GeosModule>)GeosApplication.Instance.ObjectPool["GeosModuleNameList"]).Where(s => s.IdGeosModule == 13).Select(s => s.Name).FirstOrDefault();
            }
            else
            {
                ModuleShortName = "Technical Service Mangement";
                ModuleName = "TSM";
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
        #endregion


    }
}
