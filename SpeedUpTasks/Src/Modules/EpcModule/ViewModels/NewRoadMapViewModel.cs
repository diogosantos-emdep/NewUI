using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class NewRoadMapViewModel : NavigationViewModelBase, IDisposable
    {
        #region Services
        IEpcService epcControl;
        #endregion

        #region Fields
        public bool ISave { get; set; }
        #endregion

        private ProductRoadmap roadMapData;
        public ProductRoadmap RoadMapData
        {
            get { return roadMapData; }
            set
            {
                SetProperty(ref roadMapData, value, () => RoadMapData);
            }
        }

        private LookupValue selectedRoadmapSource;
        public LookupValue SelectedRoadmapSource
        {
            get
            {
                return selectedRoadmapSource;
            }
            set
            {
                SetProperty(ref selectedRoadmapSource, value, () => SelectedRoadmapSource);
            }
        }

        private LookupValue selectedRoadmapType;

        public LookupValue SelectedRoadmapType
        {
            get
            {
                return selectedRoadmapType;
            }

            set
            {
                SetProperty(ref selectedRoadmapType, value, () => SelectedRoadmapType);
            }
        }

        private string strCode;

        public string StrCode
        {
            get
            {
                return strCode;
            }

            set
            {
                //strCode = value;
                //RaisePropertyChanged();
                SetProperty(ref strCode, value, () => StrCode);
            }
        }

        private ObservableCollection<string> autoFillRoadmapList;
        public ObservableCollection<string> AutoFillRoadmapList
        {
            get
            {
                return autoFillRoadmapList;
            }

            set
            {
                SetProperty(ref autoFillRoadmapList, value, () => AutoFillRoadmapList);
            }
        }

        bool isVEnable = false;
        public bool IsVEnable
        {
            get
            {
                return isVEnable;
            }

            set
            {
                SetProperty(ref isVEnable, value, () => IsVEnable);
            }
        }

        protected override void OnParameterChanged(object parameter)
        {
            RoadMapData = (ProductRoadmap)parameter;
            base.OnParameterChanged(parameter);
        }


        #region ICommands
        public ICommand NewRoadMapAcceptButtonCommand { get; set; }
        public ICommand NewRoadMapCancelButtonCommand { get; set; }

        public ICommand RoadmapSourceSelectedIndexChangedCommand { get; set; }

        public ICommand AutoFillTextCommand { get; set; }
        #endregion

        #region Collection
        public ObservableCollection<LookupValue> RoadMapPriorityList { get; set; }
        public ObservableCollection<LookupValue> RoadMapTypeList { get; set; }
        public ObservableCollection<LookupValue> RoadMapSourceList { get; set; }
        public ObservableCollection<LookupValue> RoadMapStatusList { get; set; }



        #endregion

        #region Constructor
        public NewRoadMapViewModel(int idLookUpValue)
        {
            //   try
            //  {

            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort),GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_ROADMAPTYPE"))
            {
                RoadMapTypeList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_ROADMAPTYPE"];
            }
            else
            {
                RoadMapTypeList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(6).AsEnumerable());

                GeosApplication.Instance.ObjectPool.Add("EPC_ROADMAPTYPE", RoadMapTypeList);

            }
            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_ROADMAPSOURCE"))
            {
                RoadMapSourceList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_ROADMAPSOURCE"];
            }
            else
            {
                RoadMapSourceList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(7).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_ROADMAPSOURCE", RoadMapSourceList);
            }
            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_ROADMAPSTATUS"))
            {
                RoadMapStatusList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_ROADMAPSTATUS"];
            }
            else
            {
                RoadMapStatusList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(8).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_ROADMAPSTATUS", RoadMapStatusList);
            }
            if (GeosApplication.Instance.ObjectPool.ContainsKey("EPC_ROADMAPPRIORITY"))
            {
                RoadMapPriorityList = (ObservableCollection<LookupValue>)GeosApplication.Instance.ObjectPool["EPC_ROADMAPPRIORITY"];
            }
            else
            {
                RoadMapPriorityList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(13).AsEnumerable());
                GeosApplication.Instance.ObjectPool.Add("EPC_ROADMAPPRIORITY", RoadMapPriorityList);
            }


            //var RoadMapTypesList = new ObservableCollection<LookupValue>(epcControl.GetLookupValues(6).AsEnumerable());
            // RoadMapTypeList = new ObservableCollection<LookupValue>();
            // foreach (var item in RoadMapTypesList)
            // {

            //     if (item.IdLookupValue == 66)
            //     {
            //         RoadMapTypesList.Remove(item);
            //         break;

            //     }

            //     RoadMapTypeList.Add(item);
            // }

            //IList<String> codeList = epcControl.GetCode("000", "Proposals", "SR");


            NewRoadMapAcceptButtonCommand = new RelayCommand(new Action<object>(SaveNewRoadMap));
            NewRoadMapCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            RoadmapSourceSelectedIndexChangedCommand = new DelegateCommand<object>(SelectedIndexChangedRoadmapSource);

            //  AutoFillTextCommand = new DelegateCommand<object>(AutoFillTextForRoadmap);

            //  }
            //catch(Exception ex)
            //{

            //}

            if (RoadMapTypeList != null)
            {
                selectedRoadmapType = RoadMapTypeList.FirstOrDefault(x => x.IdLookupValue == idLookUpValue);
            }
        }

        #endregion

        #region Methods
        public event EventHandler RequestClose;
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        private void SaveNewRoadMap(object obj)
        {
            RoadMapData.IdRoadmapStatus = 37;

            RoadMapData.IdProductRoadmap = epcControl.AddProductRoadmap(RoadMapData);


            var v = RoadMapPriorityList.FirstOrDefault(x => x.IdLookupValue == RoadMapData.IdRoadmapPriority);
            if (v != null)
            {
                RoadMapData.RoadmapPriority = v;
            }
            var v1 = RoadMapStatusList.FirstOrDefault(x => x.IdImage == RoadMapData.IdRoadmapStatus);
            if (v1 != null)
            {
                RoadMapData.RoadmapStatus = v1;
            }
            var v2 = RoadMapSourceList.FirstOrDefault(x => x.IdLookupValue == RoadMapData.IdRoadmapSource);
            if (v2 != null)
            {
                RoadMapData.RoadmapSource = v2;
            }


            if (RoadMapData.IdProductRoadmap > 0)
            {
                ISave = true;

                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewRoadMapSaved").ToString(), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);

            }
            else
                CustomMessageBox.Show(System.Windows.Application.Current.FindResource("NewRoadMapNotSaved").ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);

            RequestClose(null, null);

        }


        public void SelectedIndexChangedRoadmapSource(object obj)
        {
            strCode = "";

            try
            {
                if (SelectedRoadmapSource.Value == "Others")
                    IsVEnable = false;
                else
                {
                    IsVEnable = true;
                    AutoFillRoadmapList = new ObservableCollection<string>(epcControl.GetCode(StrCode, SelectedRoadmapType.Value, SelectedRoadmapSource.Value).AsEnumerable());
                }
            }
            catch (Exception ex)
            {



            }



            //  IList<String> codeList = epcControl.GetCode(StrCode, SelectedRoadmapType.Value, SelectedRoadmapSource.Value);


        }


        //public void AutoFillTextForRoadmap(object parameter)
        //{

        //    strCode = "";
        //    AutoFillRoadmapList = new ObservableCollection<string>(epcControl.GetCode(StrCode, SelectedRoadmapType.Value, SelectedRoadmapSource.Value).AsEnumerable());


        //}
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
