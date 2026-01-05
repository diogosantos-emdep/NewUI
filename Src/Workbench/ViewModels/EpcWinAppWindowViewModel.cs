using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Emdep.Geos.UI.Common;
using System.Windows.Forms;

using System.Windows.Controls;

namespace Workbench.ViewModels
{
 public   class EpcWinAppWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region Declaration

        private string showHideMenuButtonToolTip;

        public string ShowHideMenuButtonToolTip
        {
            get { return showHideMenuButtonToolTip; }
            set
            {
                showHideMenuButtonToolTip = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowHideMenuButtonToolTip"));
            }
        }

        #endregion

        #region Public Commands

        public ICommand HideTileBarButtonClickCommand { get; set; }
        public ICommand ButtonClickCommand { get; set; }
        #endregion  // Public Commands

        #region Constructor
        public EpcWinAppWindowViewModel()
        {
            HideTileBarButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(HideTileBarButtonClickCommandAction);
            ShowHideMenuButtonToolTip = System.Windows.Application.Current.FindResource("HideMenuButtonToolTip").ToString();  //Hide menu
            ButtonClickCommand = new DevExpress.Mvvm.DelegateCommand<RoutedEventArgs>(ButtonClickCommandAction);

            // bool? validUser = clsCommon.Db.validUser("jmarchal", "1");
            //Emdep.EPC.frmLogin obj = new Emdep.EPC.frmLogin();
            //obj.clickAccept();


            //WindowsFormsHost neWindows = new WindowsFormsHost();

            //neWindows.Child = Emdep.EPC.clsCommon.FrmMain;

            //      Grid WindowsControl;
            //WindowsControl = new Grid();
            //WindowsFormsHost host = new WindowsFormsHost();
            // frmMain f=new frmMain();
            // f.Init();
            //= new frmMain();
            //System.Windows.Forms.Control activeXControl = new frmMain();
            //f.TopLevel = false;
            // f.Init();
            // host.Child = activeXControl;
            //WindowsControl.Children.Add(host);
            // Emdep.EPC.clsCommon.FrmMain contenuto = new Emdep.EPC.clsCommon.FrmMain();
            // f.Show();
            //
            //clsCommon.FrmMain = new frmMain();
            //clsCommon.FrmMain.Init();
            //clsCommon.FrmMain.ShowDialog();
            //f.BringToFront();
            //host.Child = f;
            // f.Init();
            // host.Child..Show();
            // host.BringIntoView();
            //f.Show();
            //Emdep.EPC.clsCommon.FrmMain = new Emdep.EPC.frmMain();
            //Emdep.EPC.clsCommon.FrmMain.Init();
            //Emdep.EPC.clsCommon.FrmMain.ShowDialog();
            //bool? validUser = Emdep.EPC.clsCommon.Db.validUser("jmarchal", "1");
            // frmLogin frmlogin = new frmLogin();
            //frmlogin.Init();
            // frmMain frmobj = new frmMain();
            //frmobj.clickAccept("jmarchal", "1");
            // Emdep.EPC.clsCommon.FrmMain = new Emdep.EPC.frmMain();
            //Emdep.EPC.clsCommon.FrmMain.Init();
            //frmobj.ShowDialog();
            //Form myForm = new Form();
            //myForm.Text = "My Form";
            //myForm.SetBounds(10, 10, 200, 200);

            //myForm.Show();
            //// Determine if the form is modal.
            //if (myForm.Modal == false)
            //{
            //    // Change borderstyle and make it not a top level window.
            //    myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            //    myForm.TopLevel = false;
            //}
        }

        #endregion // Constructor

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

        private void ButtonClickCommandAction(RoutedEventArgs obj)
        {
            try
            {
                // bool? validUser = Emdep.EPC.clsCommon.Db.validUser("jmarchal", "1");
                // Emdep.EPC.frmLogin frmobj = new Emdep.EPC.frmLogin();
                //// frmobj.clickAccept("jmarchal", "1");
                // Emdep.EPC.clsCommon.FrmMain = new Emdep.EPC.frmMain();
                // Emdep.EPC.clsCommon.FrmMain.Init();
                // Emdep.EPC.clsCommon.FrmMain.ShowDialog();
            }
            catch { }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion // Methods
    }
}
