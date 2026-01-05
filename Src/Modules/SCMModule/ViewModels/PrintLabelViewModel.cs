using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DevExpress.Xpf.Grid;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class PrintLabelViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        //[GEOS2-8448][rdixit][23.09.2025]
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ICrmService CRMService = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        // ISCMService SCMService = new SCMServiceController("localhost:6699");

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
        bool isSave = false;
        private string error = string.Empty;
        List<PrintLabel> printLabelList;
        #endregion

        #region Properties
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public List<PrintLabel> PrintLabelList
        {
            get { return printLabelList; }
            set
            {
                printLabelList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PrintLabelList"));
            }
        }
        #endregion

        #region Public Icommand
        public ICommand ImageClickCommand { get; set; }
        #endregion

        #region Constructor

        public PrintLabelViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddNewConnectorViewModel()...", category: Category.Info, priority: Priority.Low);
                ImageClickCommand = new DelegateCommand<object>(ImageClickAction);
                GeosApplication.Instance.Logger.Log("Method AddNewConnectorViewModel()...Executed", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewConnectorViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Method

        public void Init(ConnectorDetailViewMode connector)
        {         
            PrintLabelList = new List<PrintLabel>();
            PrintLabel PrintLabel = new PrintLabel();
            PrintLabel.Reference = connector.Reference;
            PrintLabel.IdConnector = connector.IdConnector;
            //PrintLabel.Location = connector.LocationList?.First(i=>i.CreatorId==GeosApplication.Instance.ActiveUser.IdUser)?.Location;
            //PrintLabel.Plant = GeosApplication.Instance.ActiveUser.SiteUserPermissions.First(i => i.IdUser == GeosApplication.Instance.ActiveUser.IdUser)?.Company.SiteNameWithoutCountry;
            PrintLabel.Quantity = 1;
            PrintLabel.ToPrint = 1;
        }
        private void ImageClickAction(object sender)
        {
            try
            {
                Connectors connectors = new Connectors();
                if (sender != null)
                {
                    PrintLabel GridControl = (PrintLabel)sender;                    
                    if (!string.IsNullOrEmpty(GridControl?.Reference))
                    {
                        if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                        Connectors SelectedConnectors = new Connectors();
                        SelectedConnectors.Ref = connectors.Ref;                     
                        ConnectorGridImageView connectorGridImageView = new ConnectorGridImageView();
                        ConnectorGridImageViewModel connectorGridImageViewModel = new ConnectorGridImageViewModel();
                        EventHandler handle = delegate { connectorGridImageView.Close(); };
                        connectorGridImageViewModel.RequestClose += handle;
                        connectorGridImageViewModel.Init(SelectedConnectors);
                        connectorGridImageView.DataContext = connectorGridImageViewModel;
                        if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                        connectorGridImageView.ShowDialogWindow();
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ImageClickAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

        }
        #endregion

        #region Validation
        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

      

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }
        #endregion
    }
}
