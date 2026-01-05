using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using DevExpress.Mvvm;
using Emdep.Geos.UI.Common;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using System.Collections.ObjectModel;
using Emdep.Geos.Data.Common.PLM;
using Emdep.Geos.Data.Common;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;
using System.Drawing;
using Prism.Logging;
using DevExpress.Xpf.LayoutControl;
using System.ComponentModel;
using Emdep.Geos.Utility;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.Common.Epc;

namespace Emdep.Geos.Modules.PLM.CommonClasses
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PLMCommon : Prism.Mvvm.BindableBase
    {
        #region  Declaration

        private static readonly PLMCommon instance = new PLMCommon();
        MaximizedElementPosition maximizedElementPosition;
        private string pLM_Appearance;
        MaximizedElementPosition selectedAppearanceItem;


        DateTime minDate;
        DateTime maxDate;
        List<CurrencyConversion> currencyConversionList; 
        List<CurrencyConversion> currencyConversionList_ByPreviousDate;
        #endregion

        #region Public Properties

        public static PLMCommon Instance
        {
            get { return instance; }
        }

        public MaximizedElementPosition MaximizedElementPosition
        {
            get { return maximizedElementPosition; }
            set
            {
                maximizedElementPosition = value;
                OnPropertyChanged("MaximizedElementPosition");
            }
        }

        public string PLM_Appearance
        {
            get
            {
                return pLM_Appearance;
            }

            set
            {
                pLM_Appearance = value;
                OnPropertyChanged("PLM_Appearance");
            }
        }

        public DateTime MinDate
        {
            get
            {
                return minDate;
            }

            set
            {
                minDate = value;
                OnPropertyChanged("MinDate");
            }
        }

        public DateTime MaxDate
        {
            get
            {
                return maxDate;
            }

            set
            {
                maxDate = value;
                OnPropertyChanged("MaxDate");
            }
        }

        public List<CurrencyConversion> CurrencyConversionList
        {
            get
            {
                return currencyConversionList;
            }

            set
            {
                currencyConversionList = value;
                OnPropertyChanged("CurrencyConversionList");
            }
        }
        public List<CurrencyConversion> CurrencyConversionList_ByPreviousDate
        {
            get
            {
                return currencyConversionList_ByPreviousDate;
            }

            set
            {
                currencyConversionList_ByPreviousDate = value;
                OnPropertyChanged("CurrencyConversionList_ByPreviousDate");
            }
        }

        private ObservableCollection<Language> languages;
        public ObservableCollection<Language> Languages
        {
            get { return languages; }
            set
            {
                languages = value;
                OnPropertyChanged("Languages");
                
            }
        }

        private Language languageSelected;

        public Language LanguageSelected
        {
            get
            {
                return languageSelected;
            }
            set
            {
                languageSelected = value;
                OnPropertyChanged("LanguageSelected");
                
            }
        }

        private ObservableCollection<Site> plantList;
        public ObservableCollection<Site> PlantList
        {
            get
            {
                return plantList;
            }

            set
            {
                plantList = value;
                OnPropertyChanged("PlantList");
                
            }
        }

        private ObservableCollection<Currency> currencyList;
        public ObservableCollection<Currency> CurrencyList
        {
            get
            {
                return currencyList;
            }

            set
            {
                currencyList = value;
                OnPropertyChanged("CurrencyList");
                
            }
        }

        private ObservableCollection<Currency> saleCurrencyList;
        public ObservableCollection<Currency> SaleCurrencyList
        {
            get
            {
                return saleCurrencyList;
            }
            set
            {
                saleCurrencyList = value;
                OnPropertyChanged("SaleCurrencyList");
                
            }
        }
        private string selectedBasePriceCode;
        public string SelectedBasePriceCode
        {
            get
            {
                return selectedBasePriceCode;
            }
            set
            {
                selectedBasePriceCode = value;
                OnPropertyChanged("SelectedBasePriceCode");
                
            }
        }

        private string duplicateCode;
        public string DuplicateCode
        {
            get
            {
                return duplicateCode;
            }

            set
            {
                duplicateCode = value;
                OnPropertyChanged("DuplicateCode");
               
            }
        }

        private IList<LookupValue> tempStatusList; //[pramod.misal][16.04.2025][GEOS2-6688]
        public IList<LookupValue> TempStatusList
        {
            get { return tempStatusList; }
            set
            {
                tempStatusList = value;
                OnPropertyChanged("TempStatusList");
               
            }
        }

       
        private IList<LookupValue> tempExchangeRateList; //[pramod.misal][16.04.2025][GEOS2-6688]
        public IList<LookupValue> TempExchangeRateList
        {
            get { return tempExchangeRateList; }
            set
            {
                tempExchangeRateList = value;
                OnPropertyChanged("TempExchangeRateList");

            }
        }

        //TempLogicList

        private IList<LookupValue> tempLogicList; //[pramod.misal][16.04.2025][GEOS2-6688]
        public IList<LookupValue> TempLogicList
        {
            get { return tempLogicList; }
            set
            {
                tempLogicList = value;
                OnPropertyChanged("TempLogicList");

            }
        }

        private ObservableCollection<PCMArticleCategory> articleMenuListtemp; //[pramod.misal][16.04.2025][GEOS2-6688]
        public ObservableCollection<PCMArticleCategory> ArticleMenuListtemp
        {
            get
            {
                return articleMenuListtemp;
            }

            set
            {
                articleMenuListtemp = value;
                OnPropertyChanged("ArticleMenuListtemp");
                
            }
        }

        private ObservableCollection<BPLDetection> detectionMenuListtemp; //[pramod.misal][16.04.2025][GEOS2-6688]
        public ObservableCollection<BPLDetection> DetectionMenuListtemp
        {
            get
            {
                return detectionMenuListtemp;
            }

            set
            {
                detectionMenuListtemp = value;
                OnPropertyChanged("DetectionMenuListtemp");
                
            }
        }

        //ObservableCollection<BPLModule>


        private ObservableCollection<BPLModule> moduleMenuListtemp; //[pramod.misal][16.04.2025][GEOS2-6688]
        public ObservableCollection<BPLModule> ModuleMenuListtemp
        {
            get
            {
                return moduleMenuListtemp;
            }

            set
            {
                moduleMenuListtemp = value;
                OnPropertyChanged("ModuleMenuListtemp");

            }
        }

        List<ArticleCostPrice> articleCostPriceList;
        public List<ArticleCostPrice> ArticleCostPriceList
        {
            get
            {
                return articleCostPriceList;
            }
            set
            {
                articleCostPriceList = value;
                OnPropertyChanged("ArticleCostPriceList");
            }
        }
        #endregion


        #region Constructor

        public PLMCommon()
        {
        }

        #endregion

        #region Common methods

        public MaximizedElementPosition SetMaximizedElementPosition()
        {

            if (GeosApplication.Instance.UserSettings != null)
            {
                if (GeosApplication.Instance.UserSettings.ContainsKey("PLM_Appearance"))
                {
                    if (string.IsNullOrEmpty(GeosApplication.Instance.UserSettings["PLM_Appearance"].ToString()))
                    {
                        MaximizedElementPosition = MaximizedElementPosition.Right;
                        return MaximizedElementPosition;
                    }
                    else
                    {
                        MaximizedElementPosition = (MaximizedElementPosition)Enum.Parse(typeof(MaximizedElementPosition), GeosApplication.Instance.UserSettings["PLM_Appearance"].ToString(), true);
                        return MaximizedElementPosition;
                    }
                }
                else
                {
                    MaximizedElementPosition = MaximizedElementPosition.Right;
                    return MaximizedElementPosition;
                }
            }
            return MaximizedElementPosition;
        }

       

        #endregion
    }
}
