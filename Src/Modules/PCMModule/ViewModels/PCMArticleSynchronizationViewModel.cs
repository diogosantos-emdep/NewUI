using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.Export.Xl;
using Emdep.Geos.UI.Helper;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using System.IO;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Data.Common.SynchronizationClass;
using Newtonsoft.Json;
using DevExpress.Data.Filtering;
using Microsoft.Win32;
using System.Drawing;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    public class PCMArticleSynchronizationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region TaskLog
        //[rdixit][22.02.2023][GEOS2-4176]
        #endregion

        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IPLMService PLMService = new PLMServiceController("localhost:6699");
        #endregion

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

        #region Declaration
        ObservableCollection<PCMArticleSynchronization> clonedPriceList;
        string windowHeader;
        private ObservableCollection<PCMArticleSynchronization> priceList;
        ObservableCollection<Currency> priceSaleCurrencyList;
        ObservableCollection<Site> pricePlantList;
        ObservableCollection<BPLPlantCurrencyDetail> bPLPlantCurrencyList;
        string myFilterString;
        #endregion

        #region ICommands
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand SaleCurrencyChangeCommand { get; set; }
        public ICommand PlantsChangeCommand { get; set; }
        public ICommand PriceRowUnselectCommand { get; set; }
        public ICommand PriceRowSelectCommand { get; set; }
        #endregion

        #region Properties

        public ObservableCollection<PCMArticleSynchronization> ClonedPriceList
        {
            get
            {
                return clonedPriceList;
            }

            set
            {
                clonedPriceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClonedPriceList"));
            }
        }
        public bool IsSave
        {
            get; set;
        }
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }
        public ObservableCollection<PCMArticleSynchronization> PriceList
        {
            get
            {
                return priceList;
            }

            set
            {
                priceList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PriceList"));
            }
        }
        public ObservableCollection<Currency> PriceSaleCurrencyList
        {
            get
            {
                return priceSaleCurrencyList;
            }

            set
            {
                priceSaleCurrencyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PriceSaleCurrencyList"));
            }
        }
        public ObservableCollection<Site> PricePlantList
        {
            get
            {
                return pricePlantList;
            }

            set
            {
                pricePlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PricePlantList"));
            }
        }
        public ObservableCollection<BPLPlantCurrencyDetail> BPLPlantCurrencyList
        {
            get
            {
                return bPLPlantCurrencyList;
            }

            set
            {
                bPLPlantCurrencyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BPLPlantCurrencyList"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }
        #endregion

        #region Constructor
        public PCMArticleSynchronizationViewModel()
        {
            EscapeButtonCommand = new DelegateCommand<object>(CancelAction);
            CancelButtonCommand = new DelegateCommand<object>(CancelAction);
            AcceptButtonCommand = new DelegateCommand<object>(AcceptAction);
            PriceRowUnselectCommand = new DelegateCommand<object>(PriceRowUnselect);
            SaleCurrencyChangeCommand = new DelegateCommand<object>(ChangeSaleCurrencyCommandAction);
            PlantsChangeCommand = new RelayCommand(new Action<object>((PlantsCommandAction)));
            PriceRowSelectCommand = new DelegateCommand<object>(PriceRowSelect);
            WindowHeader = "Select the plant and currency that you want run the Synchronization";
            GetSaleCurrencies();
            GetPlants();
        }
        #endregion

        #region Methods
        public void DetectionInit(ObservableCollection<PLMDetectionPrice> IncludedPriceList, DetectionDetails UpdatedDetection)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ViewModel PCMArticleSynchronizationViewModel Method DetectionInit()...", category: Category.Info, priority: Priority.Low);
                List<PLMDetectionPrice> UpdatedDetectionPriceList = UpdatedDetection.ModifiedPLMDetectionList.Where(a => a.IdStatus == 223).ToList();
                PriceList = new ObservableCollection<PCMArticleSynchronization>();
                if (UpdatedDetectionPriceList != null)
                {
                    UpdatedDetectionPriceList.ForEach(i => PriceList.Add(new PCMArticleSynchronization()
                    {
                        IdBasePriceList = (i.Type == "BPL" ? i.IdCustomerOrBasePriceList : 0),
                        IdCustomerPriceList = (i.Type == "CPL" ? i.IdCustomerOrBasePriceList : 0),
                        Code = i.Code,
                        Name = i.Name,
                        Country = PricePlantList[0].Name,
                        Currency = i.Currency,
                        PriceCurrencies = new ObservableCollection<Currency>(),
                        PricePlants = new ObservableCollection<Site>(),
                        NewSelectedPricePlants = new List<object>(),
                        NewSelectedPriceCurrencies = new List<object>()
                    }));

                    string IdBPL = string.Join(",", UpdatedDetectionPriceList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                    string IdCPL = string.Join(",", UpdatedDetectionPriceList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                    string filterString = "Detection";
                    ObservableCollection<PCMArticleSynchronization> TempPriceList = new ObservableCollection<PCMArticleSynchronization>(PCMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Convert.ToInt32(UpdatedDetection.IdDetections), IdBPL, IdCPL, filterString));
                    if (TempPriceList != null)
                    {
                        foreach (PLMDetectionPrice item in UpdatedDetection.ModifiedPLMDetectionList)
                        {
                            if (item.Type == "BPL")
                            {
                                PCMArticleSynchronization BPLPrice = TempPriceList.FirstOrDefault(i => i.IdBasePriceList == item.IdCustomerOrBasePriceList);
                                PriceList.FirstOrDefault(j => j.IdBasePriceList == item.IdCustomerOrBasePriceList).Currency = PriceSaleCurrencyList.FirstOrDefault(r => r.IdCurrency == item.Currency.IdCurrency);
                                foreach (Currency cur in BPLPrice.PriceCurrencies)
                                {
                                    PriceList.FirstOrDefault(j => j.IdBasePriceList == BPLPrice.IdBasePriceList).PriceCurrencies.Add(PriceSaleCurrencyList.FirstOrDefault(r => r.Name == cur.Name));
                                }
                                foreach (Site plant in BPLPrice.PricePlants)
                                {
                                    PriceList.FirstOrDefault(j => j.IdBasePriceList == BPLPrice.IdBasePriceList).PricePlants.Add(PricePlantList.FirstOrDefault(r => r.Name == plant.Name));
                                }
                            }
                            else if (item.Type == "CPL")
                            {
                                PCMArticleSynchronization CPLPrice = TempPriceList.FirstOrDefault(i => i.IdCustomerPriceList == item.IdCustomerOrBasePriceList);
                                PriceList.FirstOrDefault(j => j.IdCustomerPriceList == item.IdCustomerOrBasePriceList).Currency = PriceSaleCurrencyList.FirstOrDefault(r => r.IdCurrency == item.Currency.IdCurrency);
                                foreach (Currency cur in CPLPrice.PriceCurrencies)
                                {
                                    PriceList.FirstOrDefault(j => j.IdCustomerPriceList == CPLPrice.IdCustomerPriceList).PriceCurrencies.Add(PriceSaleCurrencyList.FirstOrDefault(r => r.Name == cur.Name));
                                }
                                foreach (Site plant in CPLPrice.PricePlants)
                                {
                                    PriceList.FirstOrDefault(j => j.IdCustomerPriceList == CPLPrice.IdCustomerPriceList).PricePlants.Add(PricePlantList.FirstOrDefault(r => r.Name == plant.Name));
                                }
                            }
                        }
                        ClonedPriceList = new ObservableCollection<PCMArticleSynchronization>(PriceList.Select(i => (PCMArticleSynchronization)i.Clone()).ToList());
                    }
                }
                GeosApplication.Instance.Logger.Log("ViewModel PCMArticleSynchronizationViewModel Method DetectionInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewModel PCMArticleSynchronizationViewModel Method DetectionInit() " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewModel PCMArticleSynchronizationViewModel Method DetectionInit()" + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ViewModel PCMArticleSynchronizationViewModel Method DetectionInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public void Init(ObservableCollection<PLMArticlePrice> IncludedPLMArticlePriceList, Articles UpdatedArticle)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("ViewModel PCMArticleSynchronizationViewModel Method Init()...", category: Category.Info, priority: Priority.Low);
                List<PLMArticlePrice> UpdatedPLMArticlePriceList = UpdatedArticle.ModifiedPLMArticleList.Where(a => a.IdStatus == 223).ToList();
                PriceList = new ObservableCollection<PCMArticleSynchronization>();
                if (UpdatedPLMArticlePriceList != null)
                {
                    UpdatedPLMArticlePriceList.ForEach(i => PriceList.Add(new PCMArticleSynchronization()
                    {
                        IdBasePriceList = (i.Type == "BPL" ? i.IdCustomerOrBasePriceList : 0),
                        IdCustomerPriceList = (i.Type == "CPL" ? i.IdCustomerOrBasePriceList : 0),
                        Code = i.Code,
                        Name = i.Name,
                        Country = PricePlantList[0].Name,
                        Currency = i.Currency,
                        PriceCurrencies = new ObservableCollection<Currency>(),
                        PricePlants = new ObservableCollection<Site>(),
                        NewSelectedPricePlants = new List<object>(),
                        NewSelectedPriceCurrencies = new List<object>()
                    }));

                    string IdBPL = string.Join(",", UpdatedPLMArticlePriceList.Where(pd => pd.Type == "BPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                    string IdCPL = string.Join(",", UpdatedPLMArticlePriceList.Where(pd => pd.Type == "CPL" && pd.IdStatus == 223).Select(pd => pd.IdCustomerOrBasePriceList).ToList());
                    string filterString = "Article";
                    ObservableCollection<PCMArticleSynchronization> TempArticlePriceList = new ObservableCollection<PCMArticleSynchronization>(PCMService.GetBPLPlantCurrencyDetailByIdBPLAndIdCPL(Convert.ToInt32(UpdatedArticle.IdArticle), IdBPL, IdCPL, filterString));
                    if (TempArticlePriceList != null)
                    {
                        foreach (PLMArticlePrice item in UpdatedArticle.ModifiedPLMArticleList)
                        {
                            if (item.Type == "BPL")
                            {
                                PCMArticleSynchronization BPLPrice = TempArticlePriceList.FirstOrDefault(i => i.IdBasePriceList == item.IdCustomerOrBasePriceList);
                                PriceList.FirstOrDefault(j => j.IdBasePriceList == item.IdCustomerOrBasePriceList).Currency = PriceSaleCurrencyList.FirstOrDefault(r => r.IdCurrency == item.Currency.IdCurrency);
                                foreach (Currency cur in BPLPrice.PriceCurrencies)
                                {
                                    PriceList.FirstOrDefault(j => j.IdBasePriceList == BPLPrice.IdBasePriceList).PriceCurrencies.Add(PriceSaleCurrencyList.FirstOrDefault(r => r.Name == cur.Name));
                                }
                                foreach (Site plant in BPLPrice.PricePlants)
                                {
                                    PriceList.FirstOrDefault(j => j.IdBasePriceList == BPLPrice.IdBasePriceList).PricePlants.Add(PricePlantList.FirstOrDefault(r => r.Name == plant.Name));
                                }
                            }
                            else if (item.Type == "CPL")
                            {
                                PCMArticleSynchronization CPLPrice = TempArticlePriceList.FirstOrDefault(i => i.IdCustomerPriceList == item.IdCustomerOrBasePriceList);
                                PriceList.FirstOrDefault(j => j.IdCustomerPriceList == item.IdCustomerOrBasePriceList).Currency = PriceSaleCurrencyList.FirstOrDefault(r => r.IdCurrency == item.Currency.IdCurrency);
                                foreach (Currency cur in CPLPrice.PriceCurrencies)
                                {
                                    PriceList.FirstOrDefault(j => j.IdCustomerPriceList == CPLPrice.IdCustomerPriceList).PriceCurrencies.Add(PriceSaleCurrencyList.FirstOrDefault(r => r.Name == cur.Name));
                                }
                                foreach (Site plant in CPLPrice.PricePlants)
                                {
                                    PriceList.FirstOrDefault(j => j.IdCustomerPriceList == CPLPrice.IdCustomerPriceList).PricePlants.Add(PricePlantList.FirstOrDefault(r => r.Name == plant.Name));
                                }
                            }
                        }
                        ClonedPriceList = new ObservableCollection<PCMArticleSynchronization>(PriceList.Select(i => (PCMArticleSynchronization)i.Clone()).ToList());
                    }
                }
                GeosApplication.Instance.Logger.Log("ViewModel PCMArticleSynchronizationViewModel Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewModel PCMArticleSynchronizationViewModel Method Init() " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ViewModel PCMArticleSynchronizationViewModel Method Init()" + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method ViewModel PCMArticleSynchronizationViewModel Method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void CancelAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CancelAction()..."), category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CancelAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CancelAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void AcceptAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptAction()..."), category: Category.Info, priority: Priority.Low);
                IsSave = true;
                if (PriceList?.Count > 0)
                {
                    //List<PCMArticleSynchronization> SelectedPriceList = PriceList.Where(i => i.IsChecked).ToList();
                    List<PCMArticleSynchronization> SelectedPriceList = PriceList.Where(i => i.NewSelectedPriceCurrencies?.Count > 0 || i.NewSelectedPricePlants?.Count > 0).ToList();
                    if (SelectedPriceList != null)
                    {
                        if (SelectedPriceList != null && SelectedPriceList?.Count > 0)
                        {
                            foreach (PCMArticleSynchronization item in SelectedPriceList)
                            {
                                List<BasePriceListByPlantCurrency> BasePriceListByPlantCurrency = new List<Data.Common.PLM.BasePriceListByPlantCurrency>();
                                if (item.IdBasePriceList != 0)
                                    BasePriceListByPlantCurrency = new List<BasePriceListByPlantCurrency>(PLMService.GetPlantCurrencyByIdBasePrice(item.IdBasePriceList));


                                if (item.NewSelectedPricePlants == null || item.NewSelectedPricePlants?.Count == 0)
                                {
                                    CustomMessageBox.Show(string.Format(Application.Current.Resources["PricePlantValidationMsg"].ToString(), item.Code), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                    return;
                                }
                                foreach (var item1 in item.NewSelectedPricePlants)
                                {
                                    if (item.NewSelectedPriceCurrencies == null || item.NewSelectedPriceCurrencies?.Count == 0)
                                    {
                                        CustomMessageBox.Show(string.Format(Application.Current.Resources["PriceSaleCurrencyValidationMsg"].ToString(), item.Code), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                        return;
                                    }
                                    foreach (var item2 in item.NewSelectedPriceCurrencies)
                                    {
                                        if (item.IdBasePriceList == 0)
                                        {
                                            BPLPlantCurrencyDetail BPLPlantCurrency = new BPLPlantCurrencyDetail();
                                            BPLPlantCurrency.CompanyName = (item1 as string);
                                            BPLPlantCurrency.CurrencyName = (item2 as string);
                                            if (BPLPlantCurrencyList == null)
                                                BPLPlantCurrencyList = new ObservableCollection<BPLPlantCurrencyDetail>();
                                            if (!BPLPlantCurrencyList.Any(bpl => bpl.CompanyName == BPLPlantCurrency.CompanyName && bpl.CurrencyName == BPLPlantCurrency.CurrencyName))
                                                BPLPlantCurrencyList.Add(BPLPlantCurrency);
                                        }
                                        else
                                        {
                                            List<BasePriceListByPlantCurrency> BPLByPC = BasePriceListByPlantCurrency.Where(i => i.PlantName == (string)item1 && i.PlantbyCurrency == 1).ToList();
                                            if (BPLByPC != null)
                                            {
                                                if (BPLByPC.Any(i => i.CurrencyName == ((string)item2)))
                                                {
                                                    BPLPlantCurrencyDetail BPLPlantCurrency = new BPLPlantCurrencyDetail();
                                                    BPLPlantCurrency.CompanyName = (item1 as string);
                                                    BPLPlantCurrency.CurrencyName = (item2 as string);
                                                    if (BPLPlantCurrencyList == null)
                                                        BPLPlantCurrencyList = new ObservableCollection<BPLPlantCurrencyDetail>();
                                                    if (!BPLPlantCurrencyList.Any(bpl => bpl.CompanyName == BPLPlantCurrency.CompanyName && bpl.CurrencyName == BPLPlantCurrency.CurrencyName))
                                                        BPLPlantCurrencyList.Add(BPLPlantCurrency);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method AcceptAction()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetSaleCurrencies()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetSaleCurrencies()...", category: Category.Info, priority: Priority.Low);

                PriceSaleCurrencyList = new ObservableCollection<Currency>(PLMService.GetCurrencies_V2160());
                if (PriceSaleCurrencyList != null)
                {
                    foreach (var bpItem in PriceSaleCurrencyList.GroupBy(tpa => tpa.Name))
                    {
                        ImageSource currencyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().CurrencyIconbytes);
                        bpItem.ToList().Where(oti => oti.Name == bpItem.Key).ToList().ForEach(oti => oti.CurrencyIconImage = currencyFlagImage);
                    }
                }
                GeosApplication.Instance.Logger.Log("Method GetSaleCurrencies()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetSaleCurrencies() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetSaleCurrencies() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetCurrencies() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void GetPlants()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method GetPlants()...", category: Category.Info, priority: Priority.Low);
                PricePlantList = new ObservableCollection<Site>(PLMService.GetPlants_V2120());
                GeosApplication.Instance.Logger.Log("Method GetPlants()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetPlants() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Method GetPlants() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void ChangeSaleCurrencyCommandAction(object obj)
        {
            #region Old
            //try
            //{
            //    if (obj == null)
            //        return;
            //    GeosApplication.Instance.Logger.Log("Method ChangeSaleCurrencyCommandAction()...", category: Category.Info, priority: Priority.Low);
            //    PCMArticleSynchronization CurruntArticleBasePrice = (PCMArticleSynchronization)((object[])obj)[0];
            //    var values = ((object[])obj)[1];
            //    List<object> SelectedCurrencies = (List<object>)values;
            //    CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>();
            //    foreach (var item in SelectedCurrencies)
            //    {
            //        CurruntArticleBasePrice.NewSelectedPriceCurrencies.Add(item as Currency);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            //    GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeSaleCurrencyCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            //}
            #endregion
            try
            {
                if (obj == null)
                    return;
                GeosApplication.Instance.Logger.Log("Method ChangeSaleCurrencyCommandAction()...", category: Category.Info, priority: Priority.Low);
                PCMArticleSynchronization CurruntArticleBasePrice = (PCMArticleSynchronization)((object[])obj)[0];
                if (CurruntArticleBasePrice.NewSelectedPriceCurrencies == null)
                    CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>();
                if (CurruntArticleBasePrice.NewSelectedPriceCurrencies.Count > 0)
                {
                    if (CurruntArticleBasePrice.IdBasePriceList > 0)
                    {
                        //[rdixit][GEOS2-4520][05.10.2023]
                        ObservableCollection<Site> SiteList = new ObservableCollection<Site>();
                        List<BasePriceListByPlantCurrency> BasePriceListByPlantCurrency = new List<BasePriceListByPlantCurrency>(PLMService.GetPlantCurrencyByIdBasePrice(CurruntArticleBasePrice.IdBasePriceList));
                        PCMArticleSynchronization original = ClonedPriceList.FirstOrDefault(i => i.IdBasePriceList == CurruntArticleBasePrice.IdBasePriceList);

                        foreach (var item in CurruntArticleBasePrice.NewSelectedPriceCurrencies)
                        {
                            List<BasePriceListByPlantCurrency> BPLByPC = BasePriceListByPlantCurrency.Where(i => i.CurrencyName == Convert.ToString(item) && i.PlantbyCurrency == 1).ToList();
                            if (BPLByPC != null)
                            {
                                foreach (var item1 in BPLByPC)
                                {
                                    Site plant = original.PricePlants.FirstOrDefault(j => j.IdSite == item1.IdPlant);
                                    if (plant != null)
                                    {
                                        if (!SiteList.Any(i => i.IdSite == plant.IdSite))
                                            SiteList.Add(plant);
                                    }
                                }
                            }
                        }
                        CurruntArticleBasePrice.PricePlants = new ObservableCollection<Site>(SiteList);
                    }
                }
                else
                {
                    CurruntArticleBasePrice.NewSelectedPricePlants = new List<object>();
                    if (CurruntArticleBasePrice.IdBasePriceList > 0)
                    {
                        PCMArticleSynchronization original = ClonedPriceList.FirstOrDefault(i => i.IdBasePriceList == CurruntArticleBasePrice.IdBasePriceList);
                        CurruntArticleBasePrice.PricePlants = new ObservableCollection<Site>(original.PricePlants);
                    }
                }
                //CurruntArticleBasePrice.IsChecked = true;
                //if ((CurruntArticleBasePrice.NewSelectedPricePlants == null || CurruntArticleBasePrice.NewSelectedPricePlants?.Count == 0) && (CurruntArticleBasePrice.NewSelectedPriceCurrencies == null || CurruntArticleBasePrice.NewSelectedPriceCurrencies?.Count == 0))
                //    CurruntArticleBasePrice.IsChecked = false;
                GeosApplication.Instance.Logger.Log("Method ChangeSaleCurrencyCommandAction()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ChangeSaleCurrencyCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        private void PlantsCommandAction(object obj)
        {
            try
            {
                if (obj == null)
                    return;
                GeosApplication.Instance.Logger.Log("Method PlantsCommandAction()...", category: Category.Info, priority: Priority.Low);
                PCMArticleSynchronization CurruntArticleBasePrice = (PCMArticleSynchronization)((object[])obj)[0];
                if (CurruntArticleBasePrice.NewSelectedPricePlants == null)
                    CurruntArticleBasePrice.NewSelectedPricePlants = new List<object>();
                if (CurruntArticleBasePrice.NewSelectedPricePlants.Count > 0)
                {
                    if (CurruntArticleBasePrice.IdBasePriceList > 0)
                    {
                        //[rdixit][GEOS2-4520][05.10.2023]
                        ObservableCollection<Currency> CurrenciesList = new ObservableCollection<Currency>();
                        List<BasePriceListByPlantCurrency> BasePriceListByPlantCurrency = new List<BasePriceListByPlantCurrency>(PLMService.GetPlantCurrencyByIdBasePrice(CurruntArticleBasePrice.IdBasePriceList));
                        PCMArticleSynchronization original = ClonedPriceList.FirstOrDefault(i => i.IdBasePriceList == CurruntArticleBasePrice.IdBasePriceList);

                        foreach (var item in CurruntArticleBasePrice.NewSelectedPricePlants)
                        {
                            long idPlant = CurruntArticleBasePrice.PricePlants.FirstOrDefault(p => p.Name == Convert.ToString(item)).IdSite;
                            List<BasePriceListByPlantCurrency> BPLByPC = BasePriceListByPlantCurrency.Where(i => i.IdPlant == idPlant && i.PlantbyCurrency == 1).ToList();
                            if (BPLByPC != null)
                            {
                                foreach (var item1 in BPLByPC)
                                {
                                    Currency cur = original.PriceCurrencies.FirstOrDefault(j => j.IdCurrency == item1.IdCurrency);
                                    if (cur != null)
                                    {
                                        if (!CurrenciesList.Any(i => i.IdCurrency == cur.IdCurrency))
                                            CurrenciesList.Add(cur);
                                    }
                                }
                            }
                        }
                        CurruntArticleBasePrice.PriceCurrencies = new ObservableCollection<Currency>(CurrenciesList);
                    }
                    if (CurruntArticleBasePrice.NewSelectedPricePlants.Count == CurruntArticleBasePrice.PricePlants.Count)
                    {
                        foreach (var item in CurruntArticleBasePrice.PriceCurrencies)
                        {
                            CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>(CurruntArticleBasePrice.PriceCurrencies.Select(i => (object)i.Name).ToList());
                        }
                    }
                }
                else
                {
                    CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>();
                    if (CurruntArticleBasePrice.IdBasePriceList > 0)
                    {
                        PCMArticleSynchronization original = ClonedPriceList.FirstOrDefault(i => i.IdBasePriceList == CurruntArticleBasePrice.IdBasePriceList);
                        CurruntArticleBasePrice.PriceCurrencies = new ObservableCollection<Currency>(original.PriceCurrencies);
                    }
                }
                //CurruntArticleBasePrice.IsChecked = true;
                //if ((CurruntArticleBasePrice.NewSelectedPricePlants == null || CurruntArticleBasePrice.NewSelectedPricePlants?.Count == 0) && (CurruntArticleBasePrice.NewSelectedPriceCurrencies == null || CurruntArticleBasePrice.NewSelectedPriceCurrencies?.Count == 0))
                //    CurruntArticleBasePrice.IsChecked = false;
                GeosApplication.Instance.Logger.Log("Method PlantsCommandAction()...", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PlantsCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        private void PriceRowUnselect(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method PriceRowUnselect()..."), category: Category.Info, priority: Priority.Low);
                if (obj != null)
                {
                    RoutedEventArgs routed = (RoutedEventArgs)obj;
                    CheckEdit check = (CheckEdit)routed.OriginalSource;
                    EditGridCellData cell = (EditGridCellData)check.DataContext;
                    PCMArticleSynchronization CurruntArticleBasePrice = (PCMArticleSynchronization)cell.Row;
                    if (CurruntArticleBasePrice.IdBasePriceList > 0)
                    {
                        PCMArticleSynchronization temp = ClonedPriceList.FirstOrDefault(i => i.IdBasePriceList == CurruntArticleBasePrice.IdBasePriceList);
                        CurruntArticleBasePrice.PriceCurrencies = new ObservableCollection<Currency>(temp.PriceCurrencies);
                        CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>();
                        CurruntArticleBasePrice.PricePlants = new ObservableCollection<Site>(temp.PricePlants);
                        CurruntArticleBasePrice.NewSelectedPricePlants = new List<object>();
                    }
                    else
                    {
                        PCMArticleSynchronization temp = ClonedPriceList.FirstOrDefault(i => i.IdCustomerPriceList == CurruntArticleBasePrice.IdCustomerPriceList);
                        CurruntArticleBasePrice.PriceCurrencies = new ObservableCollection<Currency>(temp.PriceCurrencies);
                        CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>();
                        CurruntArticleBasePrice.PricePlants = new ObservableCollection<Site>(temp.PricePlants);
                        CurruntArticleBasePrice.NewSelectedPricePlants = new List<object>();
                    }
                }
                GeosApplication.Instance.Logger.Log(string.Format("Method PriceRowUnselect()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PriceRowUnselect() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void PriceRowSelect(object obj)
        {
            try
            {
                if (obj != null)
                {
                    RoutedEventArgs routed = (RoutedEventArgs)obj;
                    CheckEdit check = (CheckEdit)routed.OriginalSource;
                    EditGridCellData cell = (EditGridCellData)check.DataContext;
                    PCMArticleSynchronization CurruntArticleBasePrice = (PCMArticleSynchronization)cell.Row;
                    GeosApplication.Instance.Logger.Log(string.Format("Method PriceRowSelect()..."), category: Category.Info, priority: Priority.Low);

                    if (CurruntArticleBasePrice.IdBasePriceList > 0)
                    {
                        PCMArticleSynchronization temp = ClonedPriceList.FirstOrDefault(i => i.IdBasePriceList == CurruntArticleBasePrice.IdBasePriceList);
                        CurruntArticleBasePrice.PriceCurrencies = new ObservableCollection<Currency>(temp.PriceCurrencies);
                        CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>(temp.PriceCurrencies.Select(i => (object)i.Name).ToList());
                        CurruntArticleBasePrice.PricePlants = new ObservableCollection<Site>(temp.PricePlants);
                        CurruntArticleBasePrice.NewSelectedPricePlants = new List<object>(temp.PricePlants.Select(i => (object)i.Name).ToList());
                    }
                    else
                    {
                        PCMArticleSynchronization temp = ClonedPriceList.FirstOrDefault(i => i.IdCustomerPriceList == CurruntArticleBasePrice.IdCustomerPriceList);
                        CurruntArticleBasePrice.PriceCurrencies = new ObservableCollection<Currency>(temp.PriceCurrencies);
                        CurruntArticleBasePrice.NewSelectedPriceCurrencies = new List<object>(temp.PriceCurrencies.Select(i => (object)i.Name).ToList());
                        CurruntArticleBasePrice.PricePlants = new ObservableCollection<Site>(temp.PricePlants);
                        CurruntArticleBasePrice.NewSelectedPricePlants = new List<object>(temp.PricePlants.Select(i => (object)i.Name).ToList());
                    }
                    GeosApplication.Instance.Logger.Log(string.Format("Method PriceRowSelect()....executed successfully"), category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method PriceRowSelect() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
