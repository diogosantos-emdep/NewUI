using DevExpress.Mvvm;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Commands;
using DevExpress.Xpf.LayoutControl;
using Emdep.Geos.Utility;
using Emdep.Geos.Modules.PCM.Common_Classes;
using Emdep.Geos.Data.Common;
using Prism.Logging;
using System.Linq;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.PLM;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class MyPreferencesViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Services

        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IPLMService PLMService = new PLMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());



        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        // new public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Declarations

        MaximizedElementPosition selectedAppearanceItem;
        private List<Currency> currencies;
        private Currency selectedCurrency;
        private ObservableCollection<BasePrice> basePricelist;
        private ObservableCollection<BasePrice> basePricelistFinal;
        private int idCurrency;
        private int imageSelectedIndex;//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        private int attachmentSelectedIndex;//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        private int linksSelectedIndex;//[Sudhir.Jangra][Geos2-3132][17/02/2023]
        private List<string> lstImage;//[Sudhir.Jangra][Geos2-3132][17/02/2023]
        private List<string> lstAttachment;//[Sudhir.Jangra][Geos2-3132][17/02/2023]
        private List<string> lstLinks;//[Sudhir.Jangra][Geos2-3132][17/02/2023]
        #endregion

        #region Properties

        public MaximizedElementPosition SelectedAppearanceItem
        {
            get { return selectedAppearanceItem; }
            set
            {
                selectedAppearanceItem = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAppearanceItem"));
            }
        }

        public List<Currency> Currencies
        {
            get
            {
                return currencies;
            }

            set
            {
                currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
            }
        }

        public Currency SelectedCurrency
        {
            get
            {
                return selectedCurrency;
            }

            set
            {
                selectedCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrency"));
            }
        }

        public ObservableCollection<BasePrice> BasePricelist
        {
            get { return basePricelist; }
            set
            {
                basePricelist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BasePricelist"));
            }
        }

        public ObservableCollection<BasePrice> BasePricelistFinal
        {
            get { return basePricelistFinal; }
            set
            {
                basePricelistFinal = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BasePricelistFinal"));
            }
        }

        public int IdCurrency
        {
            get { return idCurrency; }
            set
            {
                idCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCurrency"));
            }
        }

        public int ImageSelectedIndex//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        {
            get { return imageSelectedIndex; }
            set
            {
                imageSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageSelectedIndex"));

            }
        }
        public int AttachmentSelectedIndex//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        {
            get { return attachmentSelectedIndex; }
            set
            {
                attachmentSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentSelectedIndex"));
            }
        }
        public int LinksSelectedIndex//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
        {
            get { return linksSelectedIndex; }
            set
            {
                linksSelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinksSelectedIndex"));
            }
        }
        public List<string> LstImage//[Sudhir.Jangra][GEOS2-1960][02/03/2023]
        {
            get { return lstImage; }
            set
            {
                lstImage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstImage"));
            }
        }
        public List<string> LstAttachment//[Sudhir.Jangra][GEOS2-1960][02/03/2023]
        {
            get { return lstAttachment; }
            set
            {
                lstAttachment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstAttachment"));
            }
        }
        public List<string> LstLinks//[Sudhir.Jangra][GEOS2-1960][02/03/2023]
        {
            get { return lstLinks; }
            set
            {
                lstLinks = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LstLinks"));
            }
        }
        #endregion

        #region ICommand

        public ICommand MyPreferencesViewCancelButtonCommand { get; set; }
        public ICommand MyPreferencesViewAcceptButtonCommand { get; set; }


        #endregion

        #region Constructor

        public MyPreferencesViewModel()
        {
            MyPreferencesViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
            MyPreferencesViewAcceptButtonCommand = new RelayCommand(new Action<object>(SaveMyPreference));


            //Appearance
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
            {
                if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString()))
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
                }
                else
                {
                    SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["PCM_Appearance"].ToString(), true);
                }
            }
            else
            {
                SelectedAppearanceItem = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), "Right", true);
            }

            FillCurrencyDetails();

            #region Sudhir.Jangra GEOS2-3132 17/02/2023
            FillImageList();//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
            FillAttachmentList();//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
            FillLinksList();//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
            if (GeosApplication.Instance.UserSettings.ContainsKey("PCMImage"))
            {
                ImageSelectedIndex = LstImage.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["PCMImage"].ToString()));
            }
            if (GeosApplication.Instance.UserSettings.ContainsKey("PCMAttachment"))
            {
                AttachmentSelectedIndex = LstAttachment.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["PCMAttachment"].ToString()));
            }
            if (GeosApplication.Instance.UserSettings.ContainsKey("PCMLinks"))
            {
                LinksSelectedIndex = LstAttachment.FindIndex(i => i.Contains(GeosApplication.Instance.UserSettings["PCMLinks"].ToString()));
            }
            #endregion
        }



        #endregion

        #region Methods

        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void SaveMyPreference(object obj)
        {
            //Appearance

            List<Tuple<string, string>> userConfigurations = new List<Tuple<string, string>>();

            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_Appearance"))
            {
                GeosApplication.Instance.UserSettings["PCM_Appearance"] = SelectedAppearanceItem.ToString();
            }

            //selected currency
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_SelectedCurrency"))
            {
                GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"] = SelectedCurrency.Name;
            }
            //Image Sudhir.Jangra GEOS2-1960 17/02/2023
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMImage"))
            {
                GeosApplication.Instance.UserSettings["PCMImage"] = LstImage[ImageSelectedIndex].ToString();
            }
            else
            {
                GeosApplication.Instance.UserSettings["PCMImage"] = LstImage[ImageSelectedIndex].ToString();
            }
            //Attachment Sudhir.Jangra GEOS2-1960 17/02/2023
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMAttachment"))
            {
                GeosApplication.Instance.UserSettings["PCMAttachment"] = LstAttachment[AttachmentSelectedIndex].ToString();
            }
            else
            {
                GeosApplication.Instance.UserSettings["PCMAttachment"] = LstAttachment[AttachmentSelectedIndex].ToString();
            }
            //Links Sudhir.Jangra GEOS2-1960 17/02/2023
            if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCMLinks"))
            {
                GeosApplication.Instance.UserSettings["PCMLinks"] = LstLinks[LinksSelectedIndex].ToString();
            }
            else
            {
                GeosApplication.Instance.UserSettings["PCMLinks"] = LstLinks[LinksSelectedIndex].ToString();
            }

            foreach (KeyValuePair<string, string> setting in GeosApplication.Instance.UserSettings)
            {
                userConfigurations.Add(new Tuple<string, string>(setting.Key.ToString(), setting.Value.ToString()));
            }

            ApplicationOperation.CreateNewSetting(userConfigurations, GeosApplication.Instance.UserSettingFilePath, "UserSettings");
            GeosApplication.Instance.SelectedPrinter = Convert.ToString(GeosApplication.Instance.UserSettings["SelectedPrinter"]);
            GeosApplication.Instance.LabelPrinter = Convert.ToString(GeosApplication.Instance.UserSettings["LabelPrinter"]);
            GeosApplication.Instance.LabelPrinterModel = Convert.ToString(GeosApplication.Instance.UserSettings["LabelPrinterModel"]);
            GeosApplication.Instance.ParallelPort = Convert.ToString(GeosApplication.Instance.UserSettings["ParallelPort"]);
            GeosApplication.Instance.PCMImage = Convert.ToString(GeosApplication.Instance.UserSettings["PCMImage"]);//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
            GeosApplication.Instance.PCMAttachment = Convert.ToString(GeosApplication.Instance.UserSettings["PCMAttachment"]);//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
            GeosApplication.Instance.PCMLinks = Convert.ToString(GeosApplication.Instance.UserSettings["PCMLinks"]);//[Sudhir.Jangra][GEOS2-3132][17/02/2023]
            PCMCommon.Instance.PCM_Appearance = SelectedAppearanceItem.ToString();
            PCMCommon.Instance.PCM_SelectedCurrencySymbol = SelectedCurrency.Symbol;
            PCMCommon.Instance.SelectedCurrency = SelectedCurrency;
            GeosApplication.Instance.PCMCurrentCurrency = SelectedCurrency;//[rdixit][04.12.2023][GEOS2-4897]
            RequestClose(null, null);
        }


        /// <summary>
        /// Method for get fill currency list and get IdCurrency By current System Region Culture.
        /// </summary>
        public void FillCurrencyDetails()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails ...", category: Category.Info, priority: Priority.Low);

                BasePricelist = new ObservableCollection<BasePrice>(PLMService.GetBasePrices_V2180());

                //BasePricelistFinal = BasePricelist.Where(x => x.IdStatus == 223).ToList();
                var ActiveBasePricelist = BasePricelist.Where(x => x.IdStatus == 223).ToList();

                foreach (var temp in ActiveBasePricelist)
                {
                    IdCurrency = temp.Currency.IdCurrency;
                    if (Currencies == null)
                    {
                        Currencies = new List<Currency>(GeosApplication.Instance.Currencies.Where(x => x.IdCurrency == IdCurrency).ToList());
                    }
                    else
                    {
                        var tempObj = GeosApplication.Instance.Currencies.Where(x => x.IdCurrency == IdCurrency);
                        Currencies.AddRange(tempObj);
                        //Currencies.Add(GeosApplication.Instance.Currencies.Where(x => x.IdCurrency == IdCurrency).ToList())
                    }
                }
                Currencies = Currencies.Distinct().ToList();
                //Currencies = new List<Currency>(GeosApplication.Instance.Currencies.Where(x=> x.IdCurrency == ActiveBasePricelist. abc. .ToList();

                if (GeosApplication.Instance.UserSettings != null && GeosApplication.Instance.UserSettings.ContainsKey("PCM_SelectedCurrency"))
                {
                    SelectedCurrency = Currencies.FirstOrDefault(i => i.Name == GeosApplication.Instance.UserSettings["PCM_SelectedCurrency"]);
                }

                GeosApplication.Instance.Logger.Log("Method FillCurrencyDetails() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCurrencyDetails() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #region Sudhir.Jangra GEOS2-3132 17/02/2023
        private void FillImageList()
        {
            GeosApplication.Instance.Logger.Log("Constructor FillImageList ...", category: Category.Info, priority: Priority.Low);
            LstImage = new List<string>();
            LstImage.Add("1");
            LstImage.Add("2");
            LstImage.Add("3");
            LstImage.Add("4");
            LstImage.Add("5");
            LstImage.Add("6");
            LstImage.Add("7");
            LstImage.Add("8");
            LstImage.Add("9");
            LstImage.Add("10");
            LstImage.Add("11");
            LstImage.Add("12");
            LstImage.Add("13");
            LstImage.Add("14");
            LstImage.Add("15");
            LstImage.Add("16");
            LstImage.Add("17");
            LstImage.Add("18");
            LstImage.Add("19");
            LstImage.Add("20");
            LstImage.Add("All");
            GeosApplication.Instance.Logger.Log("Constructor FillImageList() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        private void FillAttachmentList()
        {
            GeosApplication.Instance.Logger.Log("Constructor FillAttachmentList ...", category: Category.Info, priority: Priority.Low);
            LstAttachment = new List<string>();
            LstAttachment.Add("1");
            LstAttachment.Add("2");
            LstAttachment.Add("3");
            LstAttachment.Add("4");
            LstAttachment.Add("5");
            LstAttachment.Add("6");
            LstAttachment.Add("7");
            LstAttachment.Add("8");
            LstAttachment.Add("9");
            LstAttachment.Add("10");
            LstAttachment.Add("11");
            LstAttachment.Add("12");
            LstAttachment.Add("13");
            LstAttachment.Add("14");
            LstAttachment.Add("15");
            LstAttachment.Add("16");
            LstAttachment.Add("17");
            LstAttachment.Add("18");
            LstAttachment.Add("19");
            LstAttachment.Add("20");
            LstAttachment.Add("All");
            GeosApplication.Instance.Logger.Log("Constructor FillAttachmentList() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        private void FillLinksList()
        {
            GeosApplication.Instance.Logger.Log("Constructor FillLinksList ...", category: Category.Info, priority: Priority.Low);
            LstLinks = new List<string>();
            LstLinks.Add("1");
            LstLinks.Add("2");
            LstLinks.Add("3");
            LstLinks.Add("4");
            LstLinks.Add("5");
            LstLinks.Add("6");
            LstLinks.Add("7");
            LstLinks.Add("8");
            LstLinks.Add("9");
            LstLinks.Add("10");
            LstLinks.Add("11");
            LstLinks.Add("12");
            LstLinks.Add("13");
            LstLinks.Add("14");
            LstLinks.Add("15");
            LstLinks.Add("16");
            LstLinks.Add("17");
            LstLinks.Add("18");
            LstLinks.Add("19");
            LstLinks.Add("20");
            LstLinks.Add("All");
            GeosApplication.Instance.Logger.Log("Constructor FillLinksList() executed successfully", category: Category.Info, priority: Priority.Low);
        }

        #endregion

        #endregion

    }
}
