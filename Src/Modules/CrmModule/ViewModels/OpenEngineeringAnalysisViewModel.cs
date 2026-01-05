using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    //[Rahul.Gadhave][Date:09-10-2025][https://helpdesk.emdep.com/browse/GEOS2-8442]
    public class OpenEngineeringAnalysisViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  //Services

        #region Declaration
        private DataTable dataTableOpenOffers;
        private string openOffersText;
     
        #endregion

        #region public Properties

        public DataTable DataTableOpenOffers
        {
            get { return dataTableOpenOffers; }
            set
            {
                dataTableOpenOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableOpenOffers"));
            }
        }
        public string OpenOffersText
        {
            get { return openOffersText; }
            set { openOffersText = value; OnPropertyChanged(new PropertyChangedEventArgs("OpenOffersText")); }
        }

        #endregion

        #region  public Commands
        public ICommand DelayDaysOpenUnboundDataCommand { get; set; }
        #endregion

        #region Constructor
        public OpenEngineeringAnalysisViewModel()
        {
            DelayDaysOpenUnboundDataCommand = new DevExpress.Mvvm.DelegateCommand<object>(DelayDaysOpenUnboundDataCommandAction);
        }
        #endregion

        #region public event
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


        #region Methods
        public void Init(DataTable DataOpenOffers,string OpenOfferText)
        {
            try
            {
                if(DataOpenOffers!=null )
                {
                    DataTableOpenOffers=DataOpenOffers;
                    OpenOffersText = OpenOfferText;
                }
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);

               
                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
        }
        public void DelayDaysOpenUnboundDataCommandAction(object obj)
        {
            if (obj == null) return;

            GridColumnDataEventArgs e = obj as GridColumnDataEventArgs;
            if (e.IsGetData)
            {
                var date = e.GetListSourceFieldValue("DeliveryDate");
                e.Value = GeosApplication.Instance.ServerDateTime.Subtract(Convert.ToDateTime(date)).Negate().Days;
            }
        }
        #endregion
    }
}
