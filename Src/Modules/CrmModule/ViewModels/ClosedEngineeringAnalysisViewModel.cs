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
    public class ClosedEngineeringAnalysisViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ICrmService CrmStartUp = new CrmServiceController("localhost:6699");
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion  //Services

        #region Declaration
        private string closedOffersText;
        private DataTable dataTableClosedOffers;
        
        #endregion

        #region public Properties
        public DataTable DataTableClosedOffers
        {
            get { return dataTableClosedOffers; }
            set
            {
                dataTableClosedOffers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DataTableClosedOffers"));
            }
        }
        public string ClosedOffersText
        {
            get { return closedOffersText; }
            set { closedOffersText = value; OnPropertyChanged(new PropertyChangedEventArgs("ClosedOffersText")); }
        }
        #endregion

        #region  public Commands
        public ICommand WeekandDelayDaysClosedUnboundDataCommand { get; set; }
        #endregion

        #region Constructor
        public ClosedEngineeringAnalysisViewModel()
        {
            WeekandDelayDaysClosedUnboundDataCommand = new DevExpress.Mvvm.DelegateCommand<object>(WeekandDelayDaysClosedUnboundDataCommandAction);
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
        public void Init(DataTable DataCloseOffers, string CloseOfferText)
        {
            try
            {
                if (DataCloseOffers != null)
                {
                    DataTableClosedOffers = DataCloseOffers;
                    ClosedOffersText = CloseOfferText;
                }
                GeosApplication.Instance.Logger.Log("Method Init ...", category: Category.Info, priority: Priority.Low);


                GeosApplication.Instance.Logger.Log("Method Init() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init()", category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Display week number and Delay days by date in Closed Engineering Analysis.
        /// </summary>
        /// <param name="obj"></param>
        public void WeekandDelayDaysClosedUnboundDataCommandAction(object obj)
        {
            if (obj == null) return;

            GridColumnDataEventArgs e = obj as GridColumnDataEventArgs;
            if (e.Column.FieldName == "Delay(d)" && e.IsGetData)
            {
                DateTime deliverydate = Convert.ToDateTime(e.GetListSourceFieldValue("DeliveryDate"));
                DateTime closedate = Convert.ToDateTime(e.GetListSourceFieldValue("CloseDate"));
                e.Value = deliverydate.Date.Subtract(Convert.ToDateTime(closedate.Date)).Days;
            }

            if (e.Column.FieldName == "Week" && e.IsGetData)
            {
                var price = e.GetListSourceFieldValue("DeliveryDate");

                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                e.Value = currentCulture.Calendar.GetWeekOfYear(
                            Convert.ToDateTime(price),
                            currentCulture.DateTimeFormat.CalendarWeekRule,
                            currentCulture.DateTimeFormat.FirstDayOfWeek).ToString("00");
            }
        }
        #endregion
    }
}
