using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Modules.Epc.Common.EPC;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
   public class WbsViewModel:NavigationViewModelBase
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IEpcService epcControl;

        #endregion

        #region ICommands
    //    public ICommand CommandWbsResourcesCombo { get; set; }

        #endregion

        #region Collections

        private ObservableCollection<Data.Common.Epc.LookupValue> wbsResourcesList;
        public ObservableCollection<Data.Common.Epc.LookupValue> WbsResourcesList
        {
            get { return wbsResourcesList; }
            set { SetProperty(ref wbsResourcesList, value, () => WbsResourcesList); }
        }

        ObservableCollection<WBS> listWbs = new ObservableCollection<WBS>();
        public ObservableCollection<WBS> ListWbs
        {
            get { return listWbs; }
            set
            {
                listWbs = value;
            }
        }
        //public ObservableCollection<String> Entities { get; set; }
        //WBS currentItem;
        //public WBS CurrentItem
        //{
        //    get { return currentItem; }
        //    set
        //    {
        //        if (currentItem == value) return;
        //        currentItem = value;
        //        RaisePropertyChanged("CurrentItem");
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        #endregion
       
        #region Constructor
        public WbsViewModel()
       {

           epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_WBSRESOURCESTYPES"))
            {
                WbsResourcesList = (ObservableCollection<Data.Common.Epc.LookupValue>)GeosApplication.Instance.ObjectPool["EPC_WBSRESOURCESTYPES"];
            }
            else
            {
                WbsResourcesList = new ObservableCollection<Data.Common.Epc.LookupValue>(epcControl.GetLookupValues(21).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_WBSRESOURCESTYPES", WbsResourcesList);
            }

           ListWbs.Add(new WBS() { ID = 1, ParentID = 0, Group = "WBS" });
           ListWbs.Add(new WBS() { ID = 2, ParentID = 1, Group = "Management" });
           ListWbs.Add(new WBS() { ID = 3, ParentID = 2, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 4, ParentID = 2, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 5, ParentID = 1, Group = "Analysis" });
           ListWbs.Add(new WBS() { ID = 6, ParentID = 5, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 7, ParentID = 5, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 8, ParentID = 1, Group = "Development" });
           ListWbs.Add(new WBS() { ID = 9, ParentID = 8, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 10, ParentID = 8, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 11, ParentID = 1, Group = "Documentation" });
           ListWbs.Add(new WBS() { ID = 12, ParentID = 11, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 13, ParentID = 11, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 14, ParentID = 1, Group = "StartUp" });
           ListWbs.Add(new WBS() { ID = 15, ParentID = 14, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 16, ParentID = 14, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 17, ParentID = 1, Group = "Training" });
           ListWbs.Add(new WBS() { ID = 18, ParentID = 17, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 19, ParentID = 17, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 20, ParentID = 1, Group = "Validation" });
           ListWbs.Add(new WBS() { ID = 21, ParentID = 20, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 22, ParentID = 20, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 23, ParentID = 1, Group = "Support" });
           ListWbs.Add(new WBS() { ID = 24, ParentID = 23, Group = "Software" });
           ListWbs.Add(new WBS() { ID = 25, ParentID = 23, Group = "Firmware" });
           ListWbs.Add(new WBS() { ID = 26, ParentID = 1, Group = "Purchasing" });

         //  CommandWbsResourcesCombo = new DelegateCommand<object>(CommandMethod);

            // Entities = new ObservableCollection<string>();
            //for (int i = 0; i < 10; i++)
            //{

            //    Entities.Add("Item" + i);
            //}
        }
        #endregion
        #region Methods     
        protected override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
        }
        //void CommandMethod(object o)
        //{
        //    EditValueChangedEventArgs evc = o as EditValueChangedEventArgs;
        //    if (evc.OldValue == null)
        //    {
        //        return;
        //    }
        //}
        #endregion
    }
}
