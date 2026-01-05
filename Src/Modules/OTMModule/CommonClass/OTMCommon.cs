using Emdep.Geos.Data.Common;
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

namespace Emdep.Geos.Modules.OTM.CommonClass
{
    public sealed class OTMCommon : Prism.Mvvm.BindableBase
    {
        #region Services
        IOTMService APMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration
        private static readonly OTMCommon instance = new OTMCommon();
        private ObservableCollection<Company> userAuthorizedPlantsList;
        private ObservableCollection<Company> isCompanyList;
        private List<object> selectedAuthorizedPlantsList;
        private Data.Common.Company selectedSinglePlant;
        private Data.Common.Company selectedSinglePlantForPO;

        
        string toDatePOreg;
        string fromDatePOreg;
        string settingWindowLanguageSelectedIndex;
        public bool IsNewPoForOffer = false;
        string toDate;
        string fromDate;
        string zoomfactor;//[pramod.misal][10-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9297]
        string newzoomfactor;//[pramod.misal][10-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9297]

        public bool isPorequest = false;
        public bool isRegisterPO = false;


        public object PdfViewer;
        #endregion

        #region Properties

        public bool IsRegisterPO
        {
            get
            {
                return isRegisterPO; ;
            }

            set
            {
                isRegisterPO = value;
                OnPropertyChanged("IsRegisterPO");
            }
        }

        public bool IsPorequest
        {
            get
            {
                return isPorequest; ;
            }

            set
            {
                isPorequest = value;
                OnPropertyChanged("IsPorequest");
            }
        }

        //[pramod.misal][10-09-2025][https://helpdesk.emdep.com/browse/GEOS2-9297]
        public string Zoomfactor
        {
            get
            {
                return zoomfactor; ;
            }

            set
            {
                zoomfactor = value;
                OnPropertyChanged("Zoomfactor");
            }
        }

        public string NewZoomfactor
        {
            get
            {
                return newzoomfactor; ;
            }

            set
            {
                newzoomfactor = value;
                OnPropertyChanged("NewZoomfactor");
            }
        }
        public string SettingWindowLanguageSelectedIndex
        {
            get { return settingWindowLanguageSelectedIndex; }
            set
            {

                settingWindowLanguageSelectedIndex = value;
                OnPropertyChanged("settingWindowLanguageSelectedIndex");




            }
        }

        private string pORequestsTitle;
        public string PORequestsTitle
        {
            get
            {
                return pORequestsTitle; ;
            }

            set
            {
                pORequestsTitle = value;
                OnPropertyChanged("PORequestsTitle");
            }
        }

        private string registeredPOTitle;
        public string RegisteredPOTitle
        {
            get
            {
                return registeredPOTitle; ;
            }

            set
            {
                registeredPOTitle = value;
                OnPropertyChanged("RegisteredPOTitle");
            }
        }
        public bool IsPlantUpdated { get; set; }
        public Data.Common.Company SelectedSinglePlant
        {
            get { return selectedSinglePlant; }
            set
            {
                selectedSinglePlant = value;
                OnPropertyChanged("SelectedSinglePlant");
                IsPlantUpdated = true;

            }

        }

        public Data.Common.Company SelectedSinglePlantForPO
        {
            get { return selectedSinglePlantForPO; }
            set
            {
                selectedSinglePlantForPO = value;
                OnPropertyChanged("SelectedSinglePlantForPO");
                IsPlantUpdated = true;

            }

        }

        Data.Common.Company selectedPlantForRegisteredPO;
        public Data.Common.Company SelectedPlantForRegisteredPO
        {
            get { return selectedPlantForRegisteredPO; }
            set
            {
                selectedPlantForRegisteredPO = value;
                OnPropertyChanged("SelectedPlantForRegisteredPO");
                IsPlantUpdated = true;
            }
        }

        public bool IsPlantChangedEventCanOccur { get; set; }
        public ObservableCollection<Company> UserAuthorizedPlantsList
        {
            get { return userAuthorizedPlantsList; }
            set
            {
                userAuthorizedPlantsList = value;
                this.OnPropertyChanged("UserAuthorizedPlantsList");
            }
        }

        public ObservableCollection<Company> IsCompanyList
        {
            get
            {
                return isCompanyList;
            }

            set
            {
                isCompanyList = value;
                OnPropertyChanged("IsCompanyList");
            }
        }
        public string FromDatePOreg
        {
            get
            {
                return fromDatePOreg; ;
            }

            set
            {
                fromDatePOreg = value;
                OnPropertyChanged("FromDatePOreg");
            }
        }

        public string ToDatePOreg
        {
            get
            {
                return toDatePOreg;
            }

            set
            {
                toDatePOreg = value;
                OnPropertyChanged("ToDatePOreg");
            }
        }

        public string FromDate
        {
            get
            {
                return fromDate; ;
            }

            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public string ToDate
        {
            get
            {
                return toDate;
            }

            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }





        public List<object> SelectedAuthorizedPlantsList
        {
            get
            {
                return selectedAuthorizedPlantsList;
            }

            set
            {
                var bothlistsItemsAreEqual = true;

                if (value == null)
                {
                    return;
                }
                else if (selectedAuthorizedPlantsList == null ||
                    selectedAuthorizedPlantsList.Count != value.Count
                    )
                {
                    bothlistsItemsAreEqual = false;
                }
                else
                {
                    foreach (var item1 in selectedAuthorizedPlantsList)
                    {
                        var item1IdCompany = ((Company)item1).IdCompany;
                        var founditem1IdCompanyInValueList = false;
                        foreach (var item2 in value)
                        {
                            var item2IdCompany = ((Company)item2).IdCompany;
                            if (item1IdCompany == item2IdCompany)
                            {
                                founditem1IdCompanyInValueList = true;
                            }

                        }
                        if (!founditem1IdCompanyInValueList)
                        {
                            bothlistsItemsAreEqual = false;
                            break;
                        }

                    }

                    if (!bothlistsItemsAreEqual)
                    {
                        foreach (var item1 in value)
                        {
                            var item1IdCompany = ((Company)item1).IdCompany;
                            var founditem1IdCompanyInValueList = false;
                            foreach (var item2 in selectedAuthorizedPlantsList)
                            {
                                var item2IdCompany = ((Company)item2).IdCompany;
                                if (item1IdCompany == item2IdCompany)
                                {
                                    founditem1IdCompanyInValueList = true;
                                }

                            }
                            if (!founditem1IdCompanyInValueList)
                            {
                                bothlistsItemsAreEqual = false;
                                break;
                            }

                        }
                    }
                }

                if (!bothlistsItemsAreEqual)
                {
                    selectedAuthorizedPlantsList = value;
                    OnPropertyChanged("SelectedAuthorizedPlantsList");
                }
            }
        }


        public static OTMCommon Instance
        {
            get { return instance; }
        }
        #endregion

        #region Constructor
        public OTMCommon()
        {
            IsPlantChangedEventCanOccur = true;
        }
        #endregion

        #region Methods

        #endregion

    }
}
