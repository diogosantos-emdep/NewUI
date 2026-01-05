using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Data.Common.SCM;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    // [rdixit][GEOS2-5378][GEOS2-5379][GEOS2-5380][02.04.2024]
    public class ConnectorWorkflowDiagramViewModel : NavigationViewModelBase, INotifyPropertyChanged
    {
        #region Public Events
        public event EventHandler RequestClose;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Declarations 

        public virtual string ResultFileName { get; set; }
        public virtual bool DialogResult { get; set; }
        private double dialogHeight;
        private double dialogWidth;
        List<ConnectorWorkflowStatus> statusList;
        List<ConnectorWorkflowTransitions> workflowTransitionList;
        #endregion

        #region Properties
        public double DialogHeight
        {
            get { return dialogHeight; }
            set
            {
                dialogHeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
            }
        }
        public double DialogWidth
        {
            get { return dialogWidth; }
            set
            {
                dialogWidth = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
            }
        }
        public List<ConnectorWorkflowStatus> StatusList
        {
            get
            {
                return statusList;
            }

            set
            {
                statusList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StatusList"));
            }
        }
        public List<ConnectorWorkflowTransitions> WorkflowTransitionList
        {
            get
            {
                return workflowTransitionList;
            }

            set
            {
                workflowTransitionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WorkflowTransitionList"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CancelButtonCommand { get; set; }
        #endregion

        #region Constructor
        public ConnectorWorkflowDiagramViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor WorkflowDiagramViewModel....", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                {
                    DXSplashScreen.Show(x =>
                    {
                        Window win = new Window()
                        {
                            ShowActivated = false,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            AllowsTransparency = true,
                            Background = new SolidColorBrush(Colors.Transparent),
                            ShowInTaskbar = false,
                            Topmost = true,
                            SizeToContent = SizeToContent.WidthAndHeight,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        };
                        WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                DialogHeight = System.Windows.SystemParameters.PrimaryScreenHeight - 130;
                DialogWidth = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
                CancelButtonCommand = new DelegateCommand<object>(CancelButtonAction);
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor WorkflowDiagramViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in WorkflowDiagramViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region methods
        private void CancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()...", category: Category.Info, priority: Priority.Low);

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method CancelButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
