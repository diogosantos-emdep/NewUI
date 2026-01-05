using DevExpress.Data.Browsing;
using DevExpress.DataProcessing.ExtractStorage;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Pdf;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking.Platform;
using DevExpress.Xpf.Scheduler.Drawing;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.OTM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using OfficeOpenXml;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using ModelBase = Emdep.Geos.Data.Common.ModelBase;
using DevExpress.DataProcessing;
using DevExpress.Xpf.PdfViewer;
using Emdep.Geos.Modules.OTM.CommonClass;
using DevExpress.Mvvm.Native;
using DevExpress.PivotGrid.QueryMode;
using DevExpress.Xpf;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.UI.Helper;
using static DevExpress.XtraBars.Docking2010.Views.BaseRegistrator;
using DevExpress.XtraPrinting.Preview;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    public class AddEditNewOtTemplateViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Services
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
        IOTMService OTMService = new OTMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
    //    IOTMService OTMService = new OTMServiceController("localhost:6699");
        #endregion

        #region Declaration
        private bool isNew;
        public bool tempreadonly = false;
        public bool groupIsEnabled = true;
        private string windowHeader;
        private byte[] fileInBytes;
        private ObservableCollection<TemplateAttachements> templateAttachementsList;
        private ObservableCollection<Customer> groupList;
        private ObservableCollection<Regions> regionList;
        private ObservableCollection<Country> countryList;
    
        private string templateAttachementSavedFileName;
        private string fileName;
        string FileTobeSavedByName = "";
        private List<Object> attachmentObjectList;
        private TemplateAttachements selectedTemplateFile;
        private byte[] templateAttachementsDocInBytes;
        private MemoryStream pdfDoc;
        private MemoryStream excelDoc;
        private Visibility isPdf = Visibility.Hidden;
        private Visibility isExcel = Visibility.Hidden;
        private Visibility isCanvasVisible = Visibility.Hidden;
        private string templateName;
        private int selectedIndexGroup;
        private int selectedIndexRegion;
        private int selectedIndexCountry;
        private int selectedIndexPlant;
        private bool isMappingFieldsActive = false;
        private ObservableCollection<CustomerPlant> entireCompanyPlantList;
        private ObservableCollection<CustomerPlant> customerPlant;
        private string registrationNumber;
        private System.Windows.Point? startPoint;
        private System.Windows.Point? endpoint;
        private SelectionAdorner selectionAdorner;
        private ObservableCollection<string> textList;
        private string selectedPONumberText;
        private string selectedDateText;
        private string selectedCustomerText;
        private string selectedContactText;
        private string selectedAmountText;
        private string selectedCurrencyText;
        private string selectedShipTOText;
        private string selectedPaymentTermsText;
        private string selectedIncotermsText;
        private Visibility isPONumberTextVisibility;
        private Visibility isPOnumberLocationVisibility;
        private Visibility isPODateTextVisibility;
        private Visibility isPODateLocationVisibility;
        private Visibility isPOCustomerTextVisibility;
        private Visibility isPOCustomerLocationVisibility;
        private Visibility isPOContactTextVisibility;
        private Visibility isPOContactLocationVisibility;
        private Visibility isPOAmountTextVisibility;
        private Visibility isPOAmountLocationVisibility;
        private Visibility isPOCurrencyTextVisibility;
        private Visibility isPOCurrencyLocationVisibility;
        private Visibility isPOShipToTextVisibility;
        private Visibility isPOShipToLocationVisibility;
        private Visibility isPOIncotermsTextVisibility;
        private Visibility isPOIncotermsLocationVisibility;
        private Visibility isPOPaymentTermsTextVisibility;
        private Visibility isPOPaymentTermsLocationVisibility;
        private PdfDocumentProcessor PdfProcessor;
        private Visibility isMappingFildsVisible;
        private ObservableCollection<Rectangle> rectangles;


        private string pdfCoordinates;
        private System.Windows.Shapes.Rectangle _previewRectangle;
        //PoNumber
        private string pONumberRangeExcel;
        private string pONumberUpdatedRangeExcel;
        private string pONumberKeywordExcel;
        private string updatedPONumberKeywordExcel;
        private string pONumberDelimiterExcel;
        private string updatedPONumberDelimiterExcel;

        //Podate

        private string pODateRangeExcel;
        private string updatedpODateRangeExcel;
        private string pODateKeywordExcel;
        private string updatedpODateKeywordExcel;
        private string pODateDelimiterExcel;
        private string updatedpODateDelimiterExcel;

        //customer

        private string customerRangeExcel;
        private string updatedcustomerRangeExcel;
        private string customerKeywordExcel;
        private string updatedcustomerKeywordExcel;
        private string customerDelimiterExcel;
        private string updatedcustomerDelimiterExcel;

        //Contact
        private string contactRangeExcel;
        private string updatedcontactRangeExcel;
        private string contactKeywordExcel;
        private string updatedcontactKeywordExcel;
        private string contactDelimiterExcel;
        private string updatedcontactDelimiterExcel;

        //Amount

        private string amountRangeExcel;
        private string updatedamountRangeExcel;
        private string amountKeywordExcel;
        private string updatedamountKeywordExcel;
        private string amountDelimiterExcel;
        private string updatedamountDelimiterExcel;

        //Currency

        private string currencyRangeExcel;
        private string updatedcurrencyRangeExcel;
        private string currencyKeywordExcel;
        private string updatedcurrencyKeywordExcel;
        private string currencyDelimiterExcel;
        private string updatedcurrencyDelimiterExcel;

        //ship to
        private string shipToRangeExcel;
        private string updatedshipToRangeExcel;
        private string shipToKeywordExcel;
        private string updatedshipToKeywordExcel;
        private string shipToDelimiterExcel;
        private string updatedshipToDelimiterExcel;

        //IncotermsRangeExcel

        private string incotermsRangeExcel;
        private string updatedincotermsRangeExcel;

        private string incotermsKeywordExcel;
        private string updatedincotermsKeywordExcel;

        private string incotermsDelimiterExcel;
        private string updatedincotermsDelimiterExcel;

        //PaymentTermsRangeExcel

        private string paymentTermsRangeExcel;
        private string updatedpaymentTermsRangeExcel;

        private string paymentTermsKeywordExcel;
        private string updatedPaymentTermsKeywordExcel;

        private string paymentTermsDelimiterExcel;
        private string updatedPaymentTermsDelimiterExcel;
        private string informationError;
        private bool isPONumSelectedArea;
        private bool isSelectedArea;

        private string fileExtension;
        public OtRequestTemplates templates;
        private bool isSave;
        private bool isDeleted;
        int inuse;
        public bool isSelectedIndexEdited = false;
        public bool IsSelectedIndexEdited
        {
            get { return isSelectedIndexEdited; }
            set
            {
                isSelectedIndexEdited = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedIndexEdited"));
            }
        }
        public int InUse
        {
            get { return inuse; }
            set
            {
                inuse = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InUse"));

            }
        }

        int idOTRequestTemplateFieldOptionPoNumber;
        public int IdOTRequestTemplateFieldOptionPoNumber
        {
            get { return idOTRequestTemplateFieldOptionPoNumber; }
            set
            {
                idOTRequestTemplateFieldOptionPoNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionPoNumber"));
            }
        }
        //PoNumber
        int idOTRequestTemplateTextFieldPoNumber;
        public int  IdOTRequestTemplateTextFieldPoNumber
        {
            get { return idOTRequestTemplateTextFieldPoNumber; }
            set { idOTRequestTemplateTextFieldPoNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldPoNumber")); }
        }
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        int idOTRequestTemplateLocationFieldPoNumber;
        public int IdOTRequestTemplateLocationFieldPoNumber
        {
            get { return idOTRequestTemplateLocationFieldPoNumber; }
            set {idOTRequestTemplateLocationFieldPoNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldPoNumber")); }
        }
        int idOTRequestTemplateCellFieldPoNumber;
        public int IdOTRequestTemplateCellFieldPoNumber
        {
            get { return idOTRequestTemplateCellFieldPoNumber; }
            set { idOTRequestTemplateCellFieldPoNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldPoNumber")); }
        }
        public OtRequestTemplates Template
        {
            get { return templates; }
            set
            {
                templates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtRequestTemplatesList"));
            }
        }
        public string FileExtension
        {
            get { return fileExtension; }
            set
            {
                fileExtension = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileExtension"));
            }
        }
        // Properties for Date
        int idOTRequestTemplateFieldOptionDate;
        public int IdOTRequestTemplateFieldOptionDate
        {
            get { return idOTRequestTemplateFieldOptionDate; }
            set
            {
                idOTRequestTemplateFieldOptionDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionDate"));
            }
        }

        int idOTRequestTemplateTextFieldDate;
        public int IdOTRequestTemplateTextFieldDate
        {
            get { return idOTRequestTemplateTextFieldDate; }
            set
            {
                idOTRequestTemplateTextFieldDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldDate"));
            }
        }

        int idOTRequestTemplateLocationFieldDate;
        public int IdOTRequestTemplateLocationFieldDate
        {
            get { return idOTRequestTemplateLocationFieldDate; }
            set
            {
                idOTRequestTemplateLocationFieldDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldDate"));
            }
        }

        int idOTRequestTemplateCellFieldDate;
        public int IdOTRequestTemplateCellFieldDate
        {
            get { return idOTRequestTemplateCellFieldDate; }
            set
            {
                idOTRequestTemplateCellFieldDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldDate"));
            }
        }

        // Properties for Customer
        int idOTRequestTemplateFieldOptionCustomer;
        public int IdOTRequestTemplateFieldOptionCustomer
        {
            get { return idOTRequestTemplateFieldOptionCustomer; }
            set
            {
                idOTRequestTemplateFieldOptionCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionCustomer"));
            }
        }

        int idOTRequestTemplateTextFieldCustomer;
        public int IdOTRequestTemplateTextFieldCustomer
        {
            get { return idOTRequestTemplateTextFieldCustomer; }
            set
            {
                idOTRequestTemplateTextFieldCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldCustomer"));
            }
        }

        int idOTRequestTemplateLocationFieldCustomer;
        public int IdOTRequestTemplateLocationFieldCustomer
        {
            get { return idOTRequestTemplateLocationFieldCustomer; }
            set
            {
                idOTRequestTemplateLocationFieldCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldCustomer"));
            }
        }

        int idOTRequestTemplateCellFieldCustomer;
        public int IdOTRequestTemplateCellFieldCustomer
        {
            get { return idOTRequestTemplateCellFieldCustomer; }
            set
            {
                idOTRequestTemplateCellFieldCustomer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldCustomer"));
            }
        }

        // Properties for Contact
        int idOTRequestTemplateFieldOptionContact;
        public int IdOTRequestTemplateFieldOptionContact
        {
            get { return idOTRequestTemplateFieldOptionContact; }
            set
            {
                idOTRequestTemplateFieldOptionContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionContact"));
            }
        }

        int idOTRequestTemplateTextFieldContact;
        public int IdOTRequestTemplateTextFieldContact
        {
            get { return idOTRequestTemplateTextFieldContact; }
            set
            {
                idOTRequestTemplateTextFieldContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldContact"));
            }
        }

        int idOTRequestTemplateLocationFieldContact;
        public int IdOTRequestTemplateLocationFieldContact
        {
            get { return idOTRequestTemplateLocationFieldContact; }
            set
            {
                idOTRequestTemplateLocationFieldContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldContact"));
            }
        }

        int idOTRequestTemplateCellFieldContact;
        public int IdOTRequestTemplateCellFieldContact
        {
            get { return idOTRequestTemplateCellFieldContact; }
            set
            {
                idOTRequestTemplateCellFieldContact = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldContact"));
            }
        }

        // Properties for Amount
        int idOTRequestTemplateFieldOptionAmount;
        public int IdOTRequestTemplateFieldOptionAmount
        {
            get { return idOTRequestTemplateFieldOptionAmount; }
            set
            {
                idOTRequestTemplateFieldOptionAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionAmount"));
            }
        }

        int idOTRequestTemplateTextFieldAmount;
        public int IdOTRequestTemplateTextFieldAmount
        {
            get { return idOTRequestTemplateTextFieldAmount; }
            set
            {
                idOTRequestTemplateTextFieldAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldAmount"));
            }
        }

        int idOTRequestTemplateLocationFieldAmount;
        public int IdOTRequestTemplateLocationFieldAmount
        {
            get { return idOTRequestTemplateLocationFieldAmount; }
            set
            {
                idOTRequestTemplateLocationFieldAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldAmount"));
            }
        }

        int idOTRequestTemplateCellFieldAmount;
        public int IdOTRequestTemplateCellFieldAmount
        {
            get { return idOTRequestTemplateCellFieldAmount; }
            set
            {
                idOTRequestTemplateCellFieldAmount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldAmount"));
            }
        }

        // Properties for Currency
        int idOTRequestTemplateFieldOptionCurrency;
        public int IdOTRequestTemplateFieldOptionCurrency
        {
            get { return idOTRequestTemplateFieldOptionCurrency; }
            set
            {
                idOTRequestTemplateFieldOptionCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionCurrency"));
            }
        }

        int idOTRequestTemplateTextFieldCurrency;
        public int IdOTRequestTemplateTextFieldCurrency
        {
            get { return idOTRequestTemplateTextFieldCurrency; }
            set
            {
                idOTRequestTemplateTextFieldCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldCurrency"));
            }
        }

        int idOTRequestTemplateLocationFieldCurrency;
        public int IdOTRequestTemplateLocationFieldCurrency
        {
            get { return idOTRequestTemplateLocationFieldCurrency; }
            set
            {
                idOTRequestTemplateLocationFieldCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldCurrency"));
            }
        }

        int idOTRequestTemplateCellFieldCurrency;
        public int IdOTRequestTemplateCellFieldCurrency
        {
            get { return idOTRequestTemplateCellFieldCurrency; }
            set
            {
                idOTRequestTemplateCellFieldCurrency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldCurrency"));
            }
        }

        // Properties for ShipTo
        int idOTRequestTemplateFieldOptionShipTo;
        public int IdOTRequestTemplateFieldOptionShipTo
        {
            get { return idOTRequestTemplateFieldOptionShipTo; }
            set
            {
                idOTRequestTemplateFieldOptionShipTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionShipTo"));
            }
        }

        int idOTRequestTemplateTextFieldShipTo;
        public int IdOTRequestTemplateTextFieldShipTo
        {
            get { return idOTRequestTemplateTextFieldShipTo; }
            set
            {
                idOTRequestTemplateTextFieldShipTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldShipTo"));
            }
        }

        int idOTRequestTemplateLocationFieldShipTo;
        public int IdOTRequestTemplateLocationFieldShipTo
        {
            get { return idOTRequestTemplateLocationFieldShipTo; }
            set
            {
                idOTRequestTemplateLocationFieldShipTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldShipTo"));
            }
        }

        int idOTRequestTemplateCellFieldShipTo;
        public int IdOTRequestTemplateCellFieldShipTo
        {
            get { return idOTRequestTemplateCellFieldShipTo; }
            set
            {
                idOTRequestTemplateCellFieldShipTo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldShipTo"));
            }
        }

        // Properties for Incoterms
        int idOTRequestTemplateFieldOptionIncoterms;
        public int IdOTRequestTemplateFieldOptionIncoterms
        {
            get { return idOTRequestTemplateFieldOptionIncoterms; }
            set
            {
                idOTRequestTemplateFieldOptionIncoterms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionIncoterms"));
            }
        }

        int idOTRequestTemplateTextFieldIncoterms;
        public int IdOTRequestTemplateTextFieldIncoterms
        {
            get { return idOTRequestTemplateTextFieldIncoterms; }
            set
            {
                idOTRequestTemplateTextFieldIncoterms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldIncoterms"));
            }
        }

        int idOTRequestTemplateLocationFieldIncoterms;
        public int IdOTRequestTemplateLocationFieldIncoterms
        {
            get { return idOTRequestTemplateLocationFieldIncoterms; }
            set
            {
                idOTRequestTemplateLocationFieldIncoterms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldIncoterms"));
            }
        }

        int idOTRequestTemplateCellFieldIncoterms;
        public int IdOTRequestTemplateCellFieldIncoterms
        {
            get { return idOTRequestTemplateCellFieldIncoterms; }
            set
            {
                idOTRequestTemplateCellFieldIncoterms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldIncoterms"));
            }
        }

        // Properties for PaymentTerms
        int idOTRequestTemplateFieldOptionPaymentTerms;
        public int IdOTRequestTemplateFieldOptionPaymentTerms
        {
            get { return idOTRequestTemplateFieldOptionPaymentTerms; }
            set
            {
                idOTRequestTemplateFieldOptionPaymentTerms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateFieldOptionPaymentTerms"));
            }
        }

        int idOTRequestTemplateTextFieldPaymentTerms;
        public int IdOTRequestTemplateTextFieldPaymentTerms
        {
            get { return idOTRequestTemplateTextFieldPaymentTerms; }
            set
            {
                idOTRequestTemplateTextFieldPaymentTerms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateTextFieldPaymentTerms"));
            }
        }

        int idOTRequestTemplateLocationFieldPaymentTerms;
        public int IdOTRequestTemplateLocationFieldPaymentTerms
        {
            get { return idOTRequestTemplateLocationFieldPaymentTerms; }
            set
            {
                idOTRequestTemplateLocationFieldPaymentTerms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateLocationFieldPaymentTerms"));
            }
        }

        int idOTRequestTemplateCellFieldPaymentTerms;
        public int IdOTRequestTemplateCellFieldPaymentTerms
        {
            get { return idOTRequestTemplateCellFieldPaymentTerms; }
            set
            {
                idOTRequestTemplateCellFieldPaymentTerms = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOTRequestTemplateCellFieldPaymentTerms"));
            }
        }
        public bool IsSave
        {
            get { return isSave; }
            set
            {
                isSave = value;
            }
        }
        public bool IsSelectedArea
        {
            get { return isSelectedArea; }
            set
            {
                isSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSelectedArea"));
            }
        }

        public bool IsPONumSelectedArea
        {
            get { return isPONumSelectedArea; }
            set
            {
                isPONumSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPONumSelectedArea"));
            }
        }
        public string InformationError
        {
            get { return informationError; }
            set
            {
                informationError = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InformationError"));
            }
        }
        private bool isDateSelectedArea;

        public bool IsDateSelectedArea
        {
            get { return isDateSelectedArea; }
            set
            {
                isDateSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDateSelectedArea"));
            }
        }
        private bool isCustomerSelectedArea;
        public bool IsCustomerSelectedArea
        {
            get { return isCustomerSelectedArea; }
            set
            {
                isCustomerSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCustomerSelectedArea"));
            }
        }
        private bool isContactSelectedArea;
        public bool IsContactSelectedArea
        {
            get { return isContactSelectedArea; }
            set
            {
                isContactSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsContactSelectedArea"));
            }
        }
        //amount
        private bool isAmountSelectedArea;

        public bool IsAmountSelectedArea
        {
            get { return isAmountSelectedArea; }
            set
            {
                isAmountSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAmountSelectedArea"));
            }
        }

        private bool isCurrencySelectedArea;

        public bool IsCurrencySelectedArea
        {
            get { return isCurrencySelectedArea; }
            set
            {
                isCurrencySelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCurrencySelectedArea"));
            }
        }

        private bool isShipToSelectedArea;

        public bool IsShipToSelectedArea
        {
            get { return isShipToSelectedArea; }
            set
            {
                isShipToSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsShipToSelectedArea"));
            }
        }

        private bool isIncotermsSelectedArea;

        public bool IsIncotermsSelectedArea
        {
            get { return isIncotermsSelectedArea; }
            set
            {
                isIncotermsSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsIncotermsSelectedArea"));
            }
        }

        private bool isPayTermsSelectedArea;

        public bool IsPayTermsSelectedArea
        {
            get { return isPayTermsSelectedArea; }
            set
            {
                isPayTermsSelectedArea = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPayTermsSelectedArea"));
            }
        }

        //public string PaymentTermsDelimiterExcel
        //{
        //    get { return paymentTermsDelimiterExcel; }
        //    set
        //    {
        //        if (paymentTermsDelimiterExcel != value)
        //        {
        //            paymentTermsDelimiterExcel = value;
        //            OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsRangeExcel"));
        //            HandlePaymentTermsDelimiterExcell(value);
        //        }
        //    }
        //}

        //public string UpdatedPaymentTermsDelimiterExcel
        //{
        //    get { return updatedPaymentTermsDelimiterExcel; }
        //    set
        //    {
        //        updatedPaymentTermsDelimiterExcel = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("UpdatedPaymentTermsDelimiterExcel"));
        //    }
        //}

        //private void HandlePaymentTermsDelimiterExcell(string updatedValue)
        //{

        //    if (!string.IsNullOrEmpty(updatedValue))
        //    {
        //        UpdatedPaymentTermsDelimiterExcel = updatedValue;
        //    }
        //}

        FileInfo Excelfile;
        List<PORequestDetails> extractedResults;
        public List<PORequestDetails> ExtractedResults
        {
            get
            {
                return extractedResults;
            }
            set
            {
                extractedResults = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExtractedResults"));
            }
        }
        private ObservableCollection<GraphicsCoordinates> _overlayElements;
        public ObservableCollection<GraphicsCoordinates> OverlayElements
        {
            get => _overlayElements;
            set
            {
                _overlayElements = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OverlayElements"));

            }
        }
        //public List<GraphicsCoordinates> RectangleCoordinates { get; set; } = new List<GraphicsCoordinates>();
        //public GraphicsCoordinates CurrentCoordinates { get; set; }
        public bool IsDrawing { get; set; }

        private System.Windows.Point _startPoint;
        //private GraphicsCoordinates _currentRectangle;
        private string _x;
        private string _y;
        private string _width;
        private string _height;
        public string X
        {
            get { return _x; }
            set
            {
                _x = value;
                OnPropertyChanged(new PropertyChangedEventArgs("X"));
            }
        }
        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Y"));
            }
        }
        public string Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Width"));
            }
        }
        public string Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Height"));
            }
        }
        private long idOtRequestTemplate;
        private ObservableCollection<OtRequestTemplates> otrequestMappingList;
        #endregion

        #region Properties
        public long IdOtRequestTemplate
        {
            get
            {
                return idOtRequestTemplate;
            }
            set
            {
                idOtRequestTemplate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdOtRequestTemplate"));
            }
        }
        OtRequestTemplates Addedtemplate
        {
            get;
            set;
        }
        public ObservableCollection<OtRequestTemplates> OtRequestMappingList
        {
            get { return otrequestMappingList; }
            set
            {
                otrequestMappingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtRequestMappingList"));
            }
        }
        //PaymentTermsCoordinates

        //[Rahul.Gadhave][GEOS2-6734][Date:16-01-2024]
        // Mapping Fields for Pdf
        private string ponumberkeyword;
        public string PONumberKeyword
        {
            get { return ponumberkeyword; }
            set
            {
                ponumberkeyword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumberKeyword"));
            }
        }
        private string ponumberdelimiter;
        public string PONumberDelimiter
        {
            get { return ponumberdelimiter; }
            set
            {
                ponumberdelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumberDelimiter"));
            }
        }
        private string pONumberCoordinates;
        public string PONumberCoordinates
        {
            get { return pONumberCoordinates; }
            set
            {
                pONumberCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumberCoordinates"));
            }
        }
        //Date
        private string datekeyword;
        public string DateKeyword
        {
            get { return datekeyword; }
            set
            {
                datekeyword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateKeyword"));
            }
        }
        private string datedelimiter;
        public string DateDelimiter
        {
            get { return datedelimiter; }
            set
            {
                datedelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateDelimiter"));
            }
        }
        private string dateCoordinates;
        public string DateCoordinates
        {
            get { return dateCoordinates; }
            set
            {
                dateCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DateCoordinates"));
            }
        }
        //  Customer
        private string customerkeyWord;
        public string CustomerKeyWord
        {
            get { return customerkeyWord; }
            set
            {
                customerkeyWord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerKeyWord"));
            }
        }
        private string customerdelimiter;
        public string CustomerDelimiter
        {
            get { return customerdelimiter; }
            set
            {
                customerdelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerDelimiter"));
            }
        }
        private string customerCoordinates;
        public string CustomerCoordinates
        {
            get { return customerCoordinates; }
            set
            {
                customerCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerCoordinates"));
            }
        }
        //Contact

        private string contactkeyWord;
        public string ContactKeyWord
        {
            get { return contactkeyWord; }
            set
            {
                contactkeyWord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactKeyWord"));
            }
        }
        private string contactdelimiter;
        public string ContactDelimiter
        {
            get { return contactdelimiter; }
            set
            {
                contactdelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactDelimiter"));
            }
        }
        private string contactCoordinates;
        public string ContactCoordinates
        {
            get { return contactCoordinates; }
            set
            {
                contactCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ContactCoordinates"));
            }
        }

        //Amount  
        private string amountkeyWord;
        public string AmountKeyWord
        {
            get { return amountkeyWord; }
            set
            {
                amountkeyWord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AmountKeyWord"));
            }
        }
        private string amountdelimiter;
        public string AmountDelimiter
        {
            get { return amountdelimiter; }
            set
            {
                amountdelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AmountDelimiter"));
            }
        }
        private string amountCoordinates;
        public string AmountCoordinates
        {
            get { return amountCoordinates; }
            set
            {
                amountCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AmountCoordinates"));
            }
        }
        //Currency 
        private string currencykeyword;
        public string CurrencyKeyword
        {
            get { return currencykeyword; }
            set
            {
                currencykeyword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyKeyword"));
            }
        }
        private string currencydelimiter;
        public string CurrencyDelimiter
        {
            get { return currencydelimiter; }
            set
            {
                currencydelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyDelimiter"));
            }
        }
        private string currencyCoordinates;
        public string CurrencyCoordinates
        {
            get { return currencyCoordinates; }
            set
            {
                currencyCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyCoordinates"));
            }
        }
        //Ship To  
        private string shiptokeyword;
        public string ShipTOKeyword
        {
            get { return shiptokeyword; }
            set
            {
                shiptokeyword = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShipTOKeyword"));
            }
        }
        private string shiptodelimiter;
        public string ShipTODelimiter
        {
            get { return shiptodelimiter; }
            set
            {
                shiptodelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShipTODelimiter"));
            }
        }
        private string shipTOCoordinates;
        public string ShipTOCoordinates
        {
            get { return shipTOCoordinates; }
            set
            {
                shipTOCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShipTOCoordinates"));
            }
        }
        //Incoterms
        private string incotermsKeyWord;
        public string IncotermsKeyWord
        {
            get { return incotermsKeyWord; }
            set
            {
                incotermsKeyWord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncotermsKeyWord"));
            }
        }
        private string incotermsDelimiter;
        public string IncotermsDelimiter
        {
            get { return incotermsDelimiter; }
            set
            {
                incotermsDelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncotermsDelimiter"));
            }
        }
        private string incotermsCoordinates;
        public string IncotermsCoordinates
        {
            get { return incotermsCoordinates; }
            set
            {
                incotermsCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IncotermsCoordinates"));
            }
        }
        //Payment Terms 
        private string paymenttermskeyWord;
        public string PaymentTermsKeyWord
        {
            get { return paymenttermskeyWord; }
            set
            {
                paymenttermskeyWord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsKeyWord"));
            }
        }
        private string paymenttermsdelimiter;
        public string PaymentTermsDelimiter
        {
            get { return paymenttermsdelimiter; }
            set
            {
                paymenttermsdelimiter = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsDelimiter"));
            }
        }
        private string paymentTermsCoordinates;
        public string PaymentTermsCoordinates
        {
            get { return paymentTermsCoordinates; }
            set
            {
                paymentTermsCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsCoordinates"));
            }
        }
        public string PaymentTermsDelimiterExcel
        {
            get { return paymentTermsDelimiterExcel; }
            set
            {
                if (paymentTermsDelimiterExcel != value)
                {
                    paymentTermsDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsRangeExcel"));
                    HandlePaymentTermsDelimiterExcell(value);
                }
            }
        }

        public string UpdatedPaymentTermsDelimiterExcel
        {
            get { return updatedPaymentTermsDelimiterExcel; }
            set
            {
                updatedPaymentTermsDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedPaymentTermsDelimiterExcel"));
            }
        }

        public string PaymentTermsKeywordExcel
        {
            get { return paymentTermsKeywordExcel; }
            set
            {
                if (paymentTermsKeywordExcel != value)
                {
                    paymentTermsKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsRangeExcel"));
                    HandlePaymentTermsKeywordExcel(value);
                }
            }
        }

        public string UpdatedPaymentTermsKeywordExcel
        {
            get { return updatedPaymentTermsKeywordExcel; }
            set
            {
                updatedPaymentTermsKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedPaymentTermsKeywordExcel"));
            }
        }


        public string PaymentTermsRangeExcel
        {
            get { return paymentTermsRangeExcel; }
            set
            {
                if (paymentTermsRangeExcel != value)
                {
                    paymentTermsRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PaymentTermsRangeExcel"));
                    HandlePaymentTermsRangeExcel(value);
                }
            }
        }

        public string UpdatedpaymentTermsRangeExcel
        {
            get { return updatedpaymentTermsRangeExcel; }
            set
            {
                updatedpaymentTermsRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedpaymentTermsRangeExcel"));
            }
        }

        public string IncotermsDelimiterExcel
        {
            get { return incotermsDelimiterExcel; }
            set
            {
                if (incotermsDelimiterExcel != value)
                {
                    incotermsDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IncotermsDelimiterExcel"));
                    HandleIncotermsDelimiterExcel(value);
                }
            }
        }

        public string UpdatedincotermsDelimiterExcel
        {
            get { return updatedincotermsDelimiterExcel; }
            set
            {
                updatedincotermsDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedincotermsDelimiterExcel"));
            }
        }


        public string IncotermsKeywordExcel
        {
            get { return incotermsKeywordExcel; }
            set
            {
                if (incotermsKeywordExcel != value)
                {
                    incotermsKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IncotermsKeywordExcel"));
                    HandleIncotermsKeywordExcel(value);
                }
            }
        }

        public string UpdatedincotermsKeywordExcel
        {
            get { return updatedincotermsKeywordExcel; }
            set
            {
                updatedincotermsKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedincotermsKeywordExcel"));
            }
        }

        public string IncotermsRangeExcel
        {
            get { return incotermsRangeExcel; }
            set
            {
                if (incotermsRangeExcel != value)
                {
                    incotermsRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IncotermsRangeExcel"));
                    HandleIncotermsRangeExcel(value);
                }
            }
        }

        public string UpdatedincotermsRangeExcel
        {
            get { return updatedincotermsRangeExcel; }
            set
            {
                updatedincotermsRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedincotermsRangeExcel"));
            }
        }



        public string ShipToDelimiterExcel
        {
            get { return shipToDelimiterExcel; }
            set
            {
                if (shipToDelimiterExcel != value)
                {
                    shipToDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ShipToDelimiterExcel"));
                    HandleShipToDelimiterExcel(value);
                }
            }
        }

        public string UpdatedshipToDelimiterExcel
        {
            get { return updatedshipToDelimiterExcel; }
            set
            {
                updatedshipToDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedshipToDelimiterExcel"));
            }
        }

        public string ShipToKeywordExcel
        {
            get { return shipToKeywordExcel; }
            set
            {
                if (shipToKeywordExcel != value)
                {
                    shipToKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ShipToKeywordExcel"));
                    HandleShipToKeywordExcel(value);
                }
            }
        }

        public string UpdatedshipToKeywordExcel
        {
            get { return updatedshipToKeywordExcel; }
            set
            {
                updatedshipToKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedshipToKeywordExcel"));
            }
        }

        public string ShipToRangeExcel
        {
            get { return shipToRangeExcel; }
            set
            {
                if (shipToRangeExcel != value)
                {
                    shipToRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ShipToRangeExcel"));
                    HandleShipToRangeExcel(value);
                }
            }
        }

        public string UpdatedshipToRangeExcel
        {
            get { return updatedshipToRangeExcel; }
            set
            {
                updatedshipToRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedshipToRangeExcel"));
            }
        }

        public string CurrencyDelimiterExcel
        {
            get { return currencyDelimiterExcel; }
            set
            {
                if (currencyDelimiterExcel != value)
                {
                    currencyDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrencyDelimiterExcel"));
                    HandleCurrencyDelimiterExcel(value);
                }
            }
        }

        public string UpdatedcurrencyDelimiterExcel
        {
            get { return updatedcurrencyDelimiterExcel; }
            set
            {
                updatedcurrencyDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcurrencyDelimiterExcel"));
            }
        }


        public string CurrencyKeywordExcel
        {
            get { return currencyKeywordExcel; }
            set
            {
                if (currencyKeywordExcel != value)
                {
                    currencyKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrencyKeywordExcel"));
                    HandleCurrencyKeywordExcel(value);
                }
            }
        }

        public string UpdatedcurrencyKeywordExcel
        {
            get { return updatedcurrencyKeywordExcel; }
            set
            {
                updatedcurrencyKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcurrencyKeywordExcel"));
            }
        }

        public string CurrencyRangeExcel
        {
            get { return currencyRangeExcel; }
            set
            {
                if (currencyRangeExcel != value)
                {
                    currencyRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrencyRangeExcel"));
                    HandleCurrencyRangeExcel(value);
                }
            }
        }

        public string UpdatedcurrencyRangeExcel
        {
            get { return updatedcurrencyRangeExcel; }
            set
            {
                updatedcurrencyRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcurrencyRangeExcel"));
            }
        }

        public string AmountDelimiterExcel
        {
            get { return amountDelimiterExcel; }
            set
            {
                if (amountDelimiterExcel != value)
                {
                    amountDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AmountDelimiterExcel"));
                    HandleAmountDelimiterExcel(value);
                }
            }
        }

        public string UpdatedamountDelimiterExcel
        {
            get { return updatedamountDelimiterExcel; }
            set
            {
                updatedamountDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedamountDelimiterExcel"));
            }
        }

        public string AmountKeywordExcel
        {
            get { return amountKeywordExcel; }
            set
            {
                if (amountKeywordExcel != value)
                {
                    amountKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AmountKeywordExcel"));
                    HandleAmountKeywordExcell(value);
                }
            }
        }

        public string UpdatedamountKeywordExcel
        {
            get { return updatedamountKeywordExcel; }
            set
            {
                updatedamountKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedamountKeywordExcel"));
            }
        }


        public string AmountRangeExcel
        {
            get { return amountRangeExcel; }
            set
            {
                if (amountRangeExcel != value)
                {
                    amountRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("AmountRangeExcel"));
                    HandleAmountRangeExcel(value);
                }
            }
        }

        public string UpdatedamountRangeExcel
        {
            get { return updatedamountRangeExcel; }
            set
            {
                updatedamountRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedamountRangeExcel"));
            }
        }


        public string ContactDelimiterExcel
        {
            get { return contactDelimiterExcel; }
            set
            {
                if (contactDelimiterExcel != value)
                {
                    contactDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ContactDelimiterExcel"));
                    HandleContactDelimiterExcel(value);
                }
            }
        }

        public string UpdatedcontactDelimiterExcel
        {
            get { return updatedcontactDelimiterExcel; }
            set
            {
                updatedcontactDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcontactDelimiterExcel"));
            }
        }

        public string ContactKeywordExcel
        {
            get { return contactKeywordExcel; }
            set
            {
                if (contactKeywordExcel != value)
                {
                    contactKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ContactKeywordExcel"));
                    HandleContactKeywordExcel(value);
                }
            }
        }

        public string UpdatedcontactKeywordExcel
        {
            get { return updatedcontactKeywordExcel; }
            set
            {
                updatedcontactKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcontactKeywordExcel"));
            }
        }

        public string ContactRangeExcel
        {
            get { return contactRangeExcel; }
            set
            {
                if (contactRangeExcel != value)
                {
                    contactRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ContactRangeExcel"));
                    HandleContactRangeExcel(value);
                }
            }
        }

        public string UpdatedcontactRangeExcel
        {
            get { return updatedcontactRangeExcel; }
            set
            {
                updatedcontactRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcontactRangeExcel"));
            }
        }



        public string CustomerDelimiterExcel
        {
            get { return customerDelimiterExcel; }
            set
            {
                if (customerDelimiterExcel != value)
                {
                    customerDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerDelimiterExcel"));
                    HandleCustomerDelimiterExcel(value);
                }
            }
        }

        public string UpdatedcustomerDelimiterExcel
        {
            get { return updatedcustomerDelimiterExcel; }
            set
            {
                updatedcustomerDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcustomerDelimiterExcel"));
            }
        }

        public string CustomerKeywordExcel
        {
            get { return customerKeywordExcel; }
            set
            {
                if (customerKeywordExcel != value)
                {
                    customerKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerKeywordExcel"));
                    HandleCustomerKeywordExcel(value);
                }
            }
        }

        public string UpdatedcustomerKeywordExcel
        {
            get { return updatedcustomerKeywordExcel; }
            set
            {
                updatedcustomerKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcustomerKeywordExcel"));
            }
        }


        public string CustomerRangeExcel
        {
            get { return customerRangeExcel; }
            set
            {
                if (customerRangeExcel != value)
                {
                    customerRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomerRangeExcel"));
                    HandleCustomerRangeExcel(value);
                }
            }
        }

        public string UpdatedcustomerRangeExcel
        {
            get { return updatedcustomerRangeExcel; }
            set
            {
                updatedcustomerRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedcustomerRangeExcel"));
            }
        }

        public string PODateDelimiterExcel
        {
            get { return pODateDelimiterExcel; }
            set
            {
                if (pODateDelimiterExcel != value)
                {
                    pODateDelimiterExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PODateDelimiterExcel"));
                    HandlePODateDelimiterExcel(value);
                }
            }
        }



        public string UpdatedpODateDelimiterExcel
        {
            get { return updatedpODateDelimiterExcel; }
            set
            {
                updatedpODateDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedpODateDelimiterExcel"));
            }
        }


        public string PODateKeywordExcel
        {
            get { return pODateRangeExcel; }
            set
            {
                if (pODateRangeExcel != value)
                {
                    pODateKeywordExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PODateKeywordExcel"));
                    HandlePODateKeywordExcel(value);
                }
            }
        }

        public string UpdatedpODateKeywordExcel
        {
            get { return updatedpODateKeywordExcel; }
            set
            {
                updatedpODateKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedpODateKeywordExcel"));
            }
        }

        public string PODateRangeExcel
        {
            get { return pODateRangeExcel; }
            set
            {
                if (pODateRangeExcel != value)
                {
                    pODateRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PODateRangeExcel"));
                    HandlePODateRangeExcel(value);
                }
            }
        }

        public string UpdatedpODateRangeExcel
        {
            get { return updatedpODateRangeExcel; }
            set
            {
                updatedpODateRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedpODateRangeExcel"));
            }
        }

        public string PONumberDelimiterExcel
        {
            get { return pONumberDelimiterExcel; }
            set
            {
                pONumberDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumberDelimiterExcel"));
                HandlePONumberDelimiterExcelChange(value);

            }
        }

        public string UpdatedPONumberDelimiterExcel
        {
            get { return updatedPONumberDelimiterExcel; }
            set
            {
                updatedPONumberDelimiterExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedPONumberDelimiterExcel"));
            }
        }

        public string UpdatedPONumberKeywordExcel
        {
            get { return updatedPONumberKeywordExcel; }
            set
            {
                updatedPONumberKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdatedPONumberKeywordExcel"));
            }
        }

        public string PONumberKeywordExcel
        {
            get { return pONumberKeywordExcel; }
            set
            {
                pONumberKeywordExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumberKeywordExcel"));
                HandlePONumberKeywordChange(value);

            }
        }

        public string PONumberUpdatedRangeExcel
        {
            get { return pONumberUpdatedRangeExcel; }
            set
            {
                pONumberUpdatedRangeExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PONumberUpdatedRangeExcel"));
            }
        }

        public string PONumberRangeExcel
        {
            get { return pONumberRangeExcel; }
            set
            {
                if (pONumberRangeExcel != value)
                {
                    pONumberRangeExcel = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("PONumberRangeExcel"));
                    HandlePONumberRangeChange(value); // Handle logic when the value changes
                }
            }
        }
        public string PdfCoordinates
        {
            get { return pdfCoordinates; }
            set
            {
                pdfCoordinates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfCoordinates"));
            }
        }

        public ObservableCollection<Rectangle> Rectangles
        {
            get { return rectangles; }
            set
            {
                rectangles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Rectangles"));
            }
        }

        public Visibility IsCanvasVisible
        {
            get { return isCanvasVisible; }
            set
            {
                isCanvasVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCanvasVisible"));
            }
        }

        public Visibility IsMappingFildsVisible
        {
            get { return isMappingFildsVisible; }
            set
            {
                isMappingFildsVisible = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMappingFildsVisible"));
            }
        }

        public Visibility IsPOPaymentTermsTextVisibility
        {
            get { return isPOPaymentTermsTextVisibility; }
            set
            {
                isPOPaymentTermsTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOPaymentTermsTextVisibility"));
            }
        }

        public Visibility IsPOPaymentTermsLocationVisibility
        {
            get { return isPOPaymentTermsLocationVisibility; }
            set
            {
                isPOPaymentTermsLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOPaymentTermsLocationVisibility"));
            }
        }

        public Visibility IsPOIncotermsTextVisibility
        {
            get { return isPOIncotermsTextVisibility; }
            set
            {
                isPOIncotermsTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOIncotermsTextVisibility"));
            }
        }

        public Visibility IsPOIncotermsLocationVisibility
        {
            get { return isPOIncotermsLocationVisibility; }
            set
            {
                isPOIncotermsLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOIncotermsLocationVisibility"));
            }
        }

        public Visibility IsPOShipToTextVisibility
        {
            get { return isPOShipToTextVisibility; }
            set
            {
                isPOShipToTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOShipToTextVisibility"));
            }
        }


        public Visibility IsPOShipToLocationVisibility
        {
            get { return isPOShipToLocationVisibility; }
            set
            {
                isPOShipToLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOShipToLocationVisibility"));
            }
        }

        public Visibility IsPOCurrencyTextVisibility
        {
            get { return isPOCurrencyTextVisibility; }
            set
            {
                isPOCurrencyTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOCurrencyTextVisibility"));
            }
        }

        public Visibility IsPOCurrencyLocationVisibility
        {
            get { return isPOCurrencyLocationVisibility; }
            set
            {
                isPOCurrencyLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOCurrencyLocationVisibility"));
            }
        }
        public Visibility IsPOAmountTextVisibility
        {
            get { return isPOAmountTextVisibility; }
            set
            {
                isPOAmountTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOAmountTextVisibility"));
            }
        }

        public Visibility IsPOAmountLocationVisibility
        {
            get { return isPOAmountLocationVisibility; }
            set
            {
                isPOAmountLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOAmountLocationVisibility"));
            }
        }
        public Visibility IsPOContactTextVisibility
        {
            get { return isPOContactTextVisibility; }
            set
            {
                isPOContactTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOContactTextVisibility"));
            }
        }
        public Visibility IsPOContactLocationVisibility
        {
            get { return isPOContactLocationVisibility; }
            set
            {
                isPOContactLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOContactLocationVisibility"));
            }
        }
        public Visibility IsPOCustomerTextVisibility
        {
            get { return isPOCustomerTextVisibility; }
            set
            {
                isPOCustomerTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOCustomerTextVisibility"));
            }
        }
        public Visibility IsPOCustomerLocationVisibility
        {
            get { return isPOCustomerLocationVisibility; }
            set
            {
                isPOCustomerLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOCustomerLocationVisibility"));
            }
        }
        public Visibility IsPONumberTextVisibility
        {
            get { return isPONumberTextVisibility; }
            set
            {
                isPONumberTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPONumberTextVisibility"));
            }
        }
        public Visibility IsPOnumberLocationVisibility
        {
            get { return isPOnumberLocationVisibility; }
            set
            {
                isPOnumberLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPOnumberLocationVisibility"));
            }
        }
        public Visibility IsPODateTextVisibility
        {
            get { return isPODateTextVisibility; }
            set
            {
                isPODateTextVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPODateTextVisibility"));
            }
        }
        public Visibility IsPODateLocationVisibility
        {
            get { return isPODateLocationVisibility; }
            set
            {
                isPODateLocationVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPODateLocationVisibility"));
            }
        }
        public string SelectedIncotermsText
        {
            get { return selectedIncotermsText; }
            set
            {

                selectedIncotermsText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIncotermsText"));

            }
        }
        public string SelectedPaymentTermsText
        {
            get { return selectedPaymentTermsText; }
            set
            {

                selectedPaymentTermsText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPaymentTermsText"));

            }
        }
        public string SelectedShipTOText
        {
            get { return selectedShipTOText; }
            set
            {

                selectedShipTOText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedShipTOText"));

            }
        }
        public string SelectedCurrencyText
        {
            get { return selectedCurrencyText; }
            set
            {

                selectedCurrencyText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCurrencyText"));

            }
        }
        public string SelectedAmountText
        {
            get { return selectedAmountText; }
            set
            {

                selectedAmountText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedAmountText"));

            }
        }

        public string SelectedContactText
        {
            get { return selectedContactText; }
            set
            {

                selectedContactText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedContactText"));

            }
        }
        public string SelectedCustomerText
        {
            get { return selectedCustomerText; }
            set
            {

                selectedCustomerText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCustomerText"));

            }
        }
        public string SelectedDateText
        {
            get { return selectedDateText; }
            set
            {

                selectedDateText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedDateText"));

            }
        }
        public string SelectedPONumberText
        {
            get { return selectedPONumberText; }
            set
            {

                selectedPONumberText = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedPONumberText"));

            }
        }

        public ObservableCollection<string> TextList
        {
            get { return textList; }
            set
            {

                textList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TextList"));

            }
        }
        public System.Windows.Point? StartPoint
        {
            get { return startPoint; }
            set
            {

                startPoint = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartPoint"));

            }
        }
        public System.Windows.Point? Endpoint
        {
            get { return endpoint; }
            set
            {

                endpoint = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Endpoint"));

            }
        }
        public SelectionAdorner SelectionAdorner
        {
            get { return selectionAdorner; }
            set
            {
                // Only set if the value is different (avoid unnecessary updates)
                if (selectionAdorner != value)
                {
                    selectionAdorner = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectionAdorner)));
                }
            }
        }
        public bool IsMappingFieldsActive
        {
            get { return isMappingFieldsActive; }
            set
            {

                isMappingFieldsActive = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsMappingFieldsActive"));

            }
        }
        public int SelectedIndexPlant
        {
            get { return selectedIndexPlant; }
            set
            {

                selectedIndexPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexPlant"));

            }
        }
        public int SelectedIndexCountry
        {
            get { return selectedIndexCountry; }
            set
            {

                selectedIndexCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexCountry"));
                if (selectedIndexCountry > 0)
                {
                    //SelectedIndexPlant = -1;
                    //CustomerPlants = new ObservableCollection<CustomerPlant>();
                    //CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == GroupList[SelectedIndexGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList());
                    //CustomerPlants.Insert(0, new CustomerPlant { CustomerPlantName = "All" });
                    CustomerPlants = new ObservableCollection<CustomerPlant>(OTMService.GetPlantByCustomerAndCountry(GroupList[SelectedIndexGroup].IdCustomer, CountryList[selectedIndexCountry].IdCountry));
                    CustomerPlants.Insert(0, new CustomerPlant { CustomerPlantName = "All" });
                }
                else
                {
                    //SelectedIndexPlant = -1;
                    CustomerPlants = new ObservableCollection<CustomerPlant>
                    {
                       new CustomerPlant { CustomerPlantName = "All" } // Add "All" by default
                    };
                }

            }
        }
        public int SelectedIndexRegion
        {
            get { return selectedIndexRegion; }
            set
            {

                selectedIndexRegion = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexRegion"));
                if (SelectedIndexGroup > 0 && SelectedIndexRegion > 0)
                {
                    CountryList = new ObservableCollection<Country>(OTMService.GetCountriesByCustomerAndRegion(GroupList[SelectedIndexGroup].IdCustomer, RegionList[SelectedIndexRegion].IdRegion));
                    CountryList.Insert(0, new Country { Name = "All" });
                }
                else
                {
                    CountryList = new ObservableCollection<Country>
                    {
                        new Country { IdCountry=0, Name = "All" } // Add "All" by default
                    };
                }

            }
        }
        public ObservableCollection<CustomerPlant> CustomerPlants
        {
            get
            {
                return customerPlant;
            }

            set
            {
                customerPlant = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CustomerPlants"));
            }
        }
        public int SelectedIndexGroup
        {
            get
            {
                return selectedIndexGroup;
            }

            set
            {
                selectedIndexGroup = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedIndexGroup"));
                if (selectedIndexGroup > 0)
                {
                    //CustomerPlants = new ObservableCollection<CustomerPlant>();
                    //CustomerPlants = new ObservableCollection<CustomerPlant>(EntireCompanyPlantList.Where(cpl => cpl.IdCustomer == GroupList[SelectedIndexGroup].IdCustomer || cpl.CustomerPlantName == "---").ToList());
                    //CustomerPlants.Insert(0, new CustomerPlant { CustomerPlantName = "All" });
                    FillRegionList(GroupList[SelectedIndexGroup].IdCustomer);

                    if (IsSelectedIndexEdited == false)
                    {
                        if (SelectedIndexRegion == 0)
                        {
                            SelectedIndexRegion = 1;
                        }
                        if (SelectedIndexPlant == 0)
                        {
                            SelectedIndexPlant = 1;
                        }
                        if (SelectedIndexCountry == 0)
                        {
                            SelectedIndexCountry = 1;
                        }
                    }
                }

                else
                {
                    // SelectedIndexPlant = -1;
                    CustomerPlants = new ObservableCollection<CustomerPlant>
                    {
                       new CustomerPlant { CustomerPlantName = "All" } // Add "All" by default
                    };
                }
                if (SelectedIndexGroup > 0)
                {
                    CountryList = new ObservableCollection<Country>(OTMService.GetCountriesByCustomerAndRegion(GroupList[SelectedIndexGroup].IdCustomer, RegionList[SelectedIndexRegion].IdRegion));
                    CountryList.Insert(0, new Country { Name = "All" });

                    CustomerPlants = new ObservableCollection<CustomerPlant>(OTMService.GetPlantByCustomerAndCountry(GroupList[SelectedIndexGroup].IdCustomer, CountryList[SelectedIndexCountry].IdCountry));
                    CustomerPlants.Insert(0, new CustomerPlant { CustomerPlantName = "All" });
                }
                else
                {
                    CountryList = new ObservableCollection<Country>();
                    CountryList.Insert(0, new Country { Name = "All" });
                    
                }
            }
        }
        public string TemplateName
        {
            get { return templateName; }
            set
            {

                templateName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateName"));

            }
        }
        public Visibility IsExcel
        {
            get { return isExcel; }
            set
            {
                isExcel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsExcel"));
            }
        }
        public Visibility IsPdf
        {
            get { return isPdf; }
            set
            {
                isPdf = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPdf"));
            }
        }
        public MemoryStream PdfDoc
        {
            get { return pdfDoc; }
            set
            {
                pdfDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PdfDoc"));
            }
        }
        public MemoryStream ExcelDoc
        {
            get { return excelDoc; }
            set
            {
                excelDoc = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExcelDoc"));
            }
        }
        public TemplateAttachements SelectedTemplateFile
        {
            get
            {
                return selectedTemplateFile;
            }

            set
            {
                selectedTemplateFile = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedTemplateFile"));

            }
        }

        public List<object> AttachmentObjectList
        {
            get { return attachmentObjectList; }
            set
            {
                attachmentObjectList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AttachmentObjectList"));
            }
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileName"));
            }
        }
        public string TemplateAttachementSavedFileName
        {
            get
            {
                return templateAttachementSavedFileName;
            }
            set
            {
                templateAttachementSavedFileName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateAttachementSavedFileName"));
            }
        }

        public byte[] TemplateAttachementsDocInBytes
        {
            get
            {
                return templateAttachementsDocInBytes;
            }
            set
            {
                templateAttachementsDocInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateAttachementsDocInBytes"));
            }
        }
        public byte[] FileInBytes
        {
            get
            {
                return fileInBytes;
            }

            set
            {
                fileInBytes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FileInBytes"));
            }
        }
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public bool TempReadOnly
        {
            get { return tempreadonly; }
            set
            {
                tempreadonly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempReadOnly"));
            }
        }
        public bool GroupIsEnabled
        {
            get { return groupIsEnabled; }
            set
            {
                groupIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupIsEnabled"));
            }
        }
        public string WindowHeader
        {
            get { return windowHeader; }
            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }


        public ObservableCollection<TemplateAttachements> TemplateAttachementsList
        {
            get
            {
                return templateAttachementsList;
            }

            set
            {
                templateAttachementsList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TemplateAttachementsList"));

            }
        }

        public ObservableCollection<Customer> GroupList
        {
            get
            {
                return groupList;
            }

            set
            {
                groupList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("GroupList"));

            }
        }

        public ObservableCollection<Regions> RegionList
        {
            get
            {
                return regionList;
            }

            set
            {
                regionList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegionList"));

            }
        }

        public ObservableCollection<Country> CountryList
        {
            get
            {
                return countryList;
            }

            set
            {
                countryList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CountryList"));

            }
        }

        public ObservableCollection<CustomerPlant> EntireCompanyPlantList
        {
            get
            {
                return entireCompanyPlantList;
            }

            set
            {
                entireCompanyPlantList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EntireCompanyPlantList"));
            }
        }

        public OtRequestTemplates otrequesttemplates { get; set; }
        public OtRequestTemplates initialOtRequestTemplatesDetails { get; set; }

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set
            {
                registrationNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RegistrationNumber"));
            }
        }

        #endregion

        #region Public ICommand

        public ICommand AddEditNewOtTemplateViewAcceptButtonCommand { get; set; }
        public ICommand AddEditNewOtTemplateViewCancelButtonCommand { get; set; }
        public ICommand ChooseFileActionCommand { get; set; }
        public ICommand ChangeSelectedTextListCommand { get; set; }
        public ICommand SelectAreaActionCommand { get; set; }
        public ICommand SelectedMouseDownCommand { get; set; }
        public ICommand SelectedMouseMovedCommand { get; set; }
        public ICommand SelectedMouseUpCommand { get; set; }
        public ICommand PONumberSelectAreaActionCommand { get; set; }
        public ICommand DateSelectAreaActionCommand { get; set; }
        public ICommand CustomerSelectAreaActionCommand { get; set; }
        public ICommand ContactSelectAreaActionCommand { get; set; }
        public ICommand AmountSelectAreaActionCommand { get; set; }
        public ICommand CurrencySelectAreaActionCommand { get; set; }
        public ICommand ShipTOSelectAreaActionCommand { get; set; }
        public ICommand IncotermsSelectAreaActionCommand { get; set; }
        public ICommand PaymentTermsSelectAreaActionCommand { get; set; }

        #endregion

        #region public Events

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

        #region Constructor

        public AddEditNewOtTemplateViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditNewOtTemplateViewModel()...", category: Category.Info, priority: Priority.Low);

                AddEditNewOtTemplateViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddEditNewOtTemplateViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddEditNewOtTemplateViewAcceptButtonCommandAction));
                ChooseFileActionCommand = new DelegateCommand<object>(BrowseFileAction);

                ChangeSelectedTextListCommand = new DelegateCommand<object>(ChangeSelectedTextListCommandCommandAction);
                PONumberSelectAreaActionCommand = new RelayCommand(new Action<object>(PONumberSelectAreaActionCommandButtonCommandAction));
                DateSelectAreaActionCommand = new RelayCommand(new Action<object>(DateSelectAreaActionCommandButtonCommandAction));
                CustomerSelectAreaActionCommand = new RelayCommand(new Action<object>(CustomerSelectAreaActionCommandButtonCommandAction));
                ContactSelectAreaActionCommand = new RelayCommand(new Action<object>(ContactSelectAreaActionCommandButtonCommandAction));
                AmountSelectAreaActionCommand = new RelayCommand(new Action<object>(AmountSelectAreaActionCommandButtonCommandAction));
                CurrencySelectAreaActionCommand = new RelayCommand(new Action<object>(CurrencySelectAreaActionCommandButtonCommandAction));
                ShipTOSelectAreaActionCommand = new RelayCommand(new Action<object>(ShipTOSelectAreaActionCommandButtonCommandAction));
                IncotermsSelectAreaActionCommand = new RelayCommand(new Action<object>(IncotermsSelectAreaActionCommandAction));
                PaymentTermsSelectAreaActionCommand = new RelayCommand(new Action<object>(PaymentTermsSelectAreaActionCommandAction));


                SelectedMouseDownCommand = new DelegateCommand<MouseEventArgs>(OnMouseDown);
                SelectedMouseMovedCommand = new DelegateCommand<MouseEventArgs>(OnMouseMove);
                SelectedMouseUpCommand = new DelegateCommand<MouseEventArgs>(OnMouseUp);

                FillGroupList();
                //FillRegionList();
                FillCountryList();
                FillRegistrationNumber();
                FillTextList();
                IsPdf = Visibility.Hidden;
                IsExcel = Visibility.Hidden;
                IsMappingFildsVisible = Visibility.Hidden;
                IsCanvasVisible = Visibility.Hidden;
                PdfProcessor = new PdfDocumentProcessor();

                OverlayElements = new ObservableCollection<GraphicsCoordinates>();
                RegionList = new ObservableCollection<Regions>();
                RegionList.Insert(0, new Regions { RegionName = "All" });

                X = "0";
                Y = "0";
                Height = "0";
                Width = "0";


                GeosApplication.Instance.Logger.Log("Constructor AddEditNewOtTemplateViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddEditNewOtTemplateViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }
        #endregion

        #region Methods

        private void HandlePaymentTermsDelimiterExcell(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedPaymentTermsDelimiterExcel = updatedValue;
            }
        }

        private void HandlePaymentTermsKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedPaymentTermsKeywordExcel = updatedValue;
            }
        }

        private void HandlePaymentTermsRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedpaymentTermsRangeExcel = updatedValue;
            }
        }

        private void HandleIncotermsDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedincotermsDelimiterExcel = updatedValue;
            }
        }


        private void HandleIncotermsKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedincotermsKeywordExcel = updatedValue;
            }
        }

        private void HandleIncotermsRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedincotermsRangeExcel = updatedValue;
            }
        }

        private void HandleShipToDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedshipToDelimiterExcel = updatedValue;
            }
        }



        private void HandleShipToKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedshipToKeywordExcel = updatedValue;
            }
        }


        private void HandleShipToRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedshipToRangeExcel = updatedValue;
            }
        }

        private void HandleCurrencyDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcurrencyDelimiterExcel = updatedValue;
            }
        }

        private void HandleCurrencyKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcurrencyKeywordExcel = updatedValue;
            }
        }

        private void HandleCurrencyRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcurrencyRangeExcel = updatedValue;
            }
        }

        private void HandleAmountDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedamountDelimiterExcel = updatedValue;
            }
        }


        private void HandleAmountKeywordExcell(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedamountKeywordExcel = updatedValue;
            }
        }


        private void HandleAmountRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedamountRangeExcel = updatedValue;
            }
        }


        private void HandleContactDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcontactDelimiterExcel = updatedValue;
            }
        }


        private void HandleContactKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcontactKeywordExcel = updatedValue;
            }
        }
        private void HandleContactRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcontactRangeExcel = updatedValue;
            }
        }

        private void HandleCustomerDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcustomerDelimiterExcel = updatedValue;
            }
        }

        private void HandleCustomerKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcustomerKeywordExcel = updatedValue;
            }
        }


        private void HandleCustomerRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedcustomerRangeExcel = updatedValue;
            }
        }

        private void HandlePODateDelimiterExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedpODateDelimiterExcel = updatedValue;
            }
        }

        private void HandlePODateKeywordExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedpODateKeywordExcel = updatedValue;
            }
        }

        private void HandlePODateRangeExcel(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedpODateRangeExcel = updatedValue;
            }
        }


        private void HandlePONumberDelimiterExcelChange(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedPONumberDelimiterExcel = updatedValue;
            }
        }

        private void HandlePONumberKeywordChange(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                UpdatedPONumberKeywordExcel = updatedValue;
            }
        }


        private void HandlePONumberRangeChange(string updatedValue)
        {

            if (!string.IsNullOrEmpty(updatedValue))
            {
                PONumberUpdatedRangeExcel = updatedValue;
            }
        }



        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        private void FillRegistrationNumber()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegistrationNumber()....", category: Category.Info, priority: Priority.Low);

                RegistrationNumber = OTMService.GetCode();

                GeosApplication.Instance.Logger.Log("Method FillRegistrationNumber() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegistrationNumber() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegistrationNumber() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegistrationNumber() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }


        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        private void FillCountryList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillCountryList()....", category: Category.Info, priority: Priority.Low);

                //CountryList = new ObservableCollection<Country>(OTMService.GetCountriesDetails_V2600());
                //EntireCompanyPlantList = new ObservableCollection<CustomerPlant>(OTMService.OTM_GetCustomerPlant_V2580());
                // CountryList.Insert(0, new Country { Name = "All" });

                GeosApplication.Instance.Logger.Log("Method FillCountryList() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountryList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillCountryList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillCountryList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }


        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        private void FillRegionList(int idCustomer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillRegionList()....", category: Category.Info, priority: Priority.Low);
               // OTMService = new OTMServiceController("localhost:6699");
                //RegionList = new ObservableCollection<Regions>(OTMService.GetRegions_V2600());
                RegionList = new ObservableCollection<Regions>(OTMService.GetRegions_V2610(idCustomer));
                RegionList.Insert(0, new Regions { RegionName = "All" });

                GeosApplication.Instance.Logger.Log("Method FillRegionList() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegionList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillRegionList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillRegionList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        private void FillGroupList()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method FillGroupList()....", category: Category.Info, priority: Priority.Low);
                //OTMService = new OTMServiceController("localhost:6699");
                GroupList = new ObservableCollection<Customer>(OTMService.GetCustomerDetails_V2600());
                GroupList.Insert(0, new Customer { CustomerName = "---" });
                CountryList = new ObservableCollection<Country>();
                CountryList.Insert(0, new Country { Name = "All" });
                customerPlant = new ObservableCollection<CustomerPlant>();
                customerPlant.Insert(0, new CustomerPlant { CustomerPlantName = "All" });
                GeosApplication.Instance.Logger.Log("Method FillGroupList() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in FillGroupList() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }


        private void FillTextList()
        {
            TextList = new ObservableCollection<string> { "Text", "Location" };
            SelectedPONumberText = TextList[0];
            SelectedDateText = TextList[0];
            SelectedCustomerText = TextList[0];
            SelectedContactText = TextList[0];
            SelectedAmountText = TextList[0];
            SelectedCurrencyText = TextList[0];
            SelectedShipTOText = TextList[0];
            SelectedIncotermsText = TextList[0];
            SelectedPaymentTermsText = TextList[0];

            IsPONumberTextVisibility = Visibility.Visible;
            IsPOnumberLocationVisibility = Visibility.Hidden;

            IsPODateTextVisibility = Visibility.Visible;
            IsPODateLocationVisibility = Visibility.Hidden;

            IsPOCustomerTextVisibility = Visibility.Visible;
            IsPOCustomerLocationVisibility = Visibility.Hidden;


            IsPOContactTextVisibility = Visibility.Visible;
            IsPOContactLocationVisibility = Visibility.Hidden;

            IsPOAmountTextVisibility = Visibility.Visible;
            IsPOAmountLocationVisibility = Visibility.Hidden;

            IsPOCurrencyTextVisibility = Visibility.Visible;
            IsPOCurrencyLocationVisibility = Visibility.Hidden;

            IsPOShipToTextVisibility = Visibility.Visible;
            IsPOShipToLocationVisibility = Visibility.Hidden;

            IsPOIncotermsTextVisibility = Visibility.Visible;
            IsPOIncotermsLocationVisibility = Visibility.Hidden;

            IsPOPaymentTermsTextVisibility = Visibility.Visible;
            IsPOPaymentTermsLocationVisibility = Visibility.Hidden;


        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>

        private void OnMouseDown(MouseEventArgs e)
        {


            // Capture the start point
            StartPoint = e.GetPosition((IInputElement)OTMCommon.Instance.PdfViewer);

            // Now you can access the X and Y coordinates
            double xCoordinate = StartPoint.Value.X;
            double yCoordinate = StartPoint.Value.Y;

            X = Convert.ToString(xCoordinate);

            if (X != null && Y != null && Width != null && Height != null)
            {
                X = null;
                Y = null;
                Width = null;
                Height = null;

            }




        }

        private void OnMouseMove(MouseEventArgs e)
        {

            if (StartPoint.HasValue)
            {

                Endpoint = e.GetPosition((IInputElement)OTMCommon.Instance.PdfViewer);
                // Now you can access the X and Y coordinates
                double xCoordinate = Endpoint.Value.X;
                double yCoordinate = Endpoint.Value.Y;

                //X = Convert.ToString(StartPoint.Value.X);
                //Y = Convert.ToString(StartPoint.Value.Y);
                //Height = Convert.ToString(Endpoint.Value.Y - StartPoint.Value.Y);
                //Width = Convert.ToString(Endpoint.Value.X - StartPoint.Value.X);



            }
        }
        private void OnMouseUp(MouseEventArgs e)
        {

            if (StartPoint.HasValue && Endpoint.HasValue && IsSelectedArea)
            {
                X = Convert.ToString(StartPoint.Value.X);
                Y = Convert.ToString(StartPoint.Value.Y);
                Height = Convert.ToString(Endpoint.Value.Y - StartPoint.Value.Y);
                Width = Convert.ToString(Endpoint.Value.X - StartPoint.Value.X);

                IsCanvasVisible = Visibility.Visible;
                // Ensure the shape is a square
                double sideLength = Math.Min(Math.Abs(Endpoint.Value.X - StartPoint.Value.X), Math.Abs(Endpoint.Value.Y - StartPoint.Value.Y));
                double adjustedX = StartPoint.Value.X + (Endpoint.Value.X > StartPoint.Value.X ? sideLength : -sideLength);
                double adjustedY = StartPoint.Value.Y + (Endpoint.Value.Y > StartPoint.Value.Y ? sideLength : -sideLength);
                Endpoint = new System.Windows.Point(adjustedX, adjustedY);

                // Convert to a rectangle
                var rect = new Rectangle
                {
                    //X = (int)Math.Min(StartPoint.Value.X, Endpoint.Value.X),
                    //Y = (int)Math.Min(StartPoint.Value.Y, Endpoint.Value.Y),
                    //Width = (int)sideLength,
                    //Height = (int)sideLength

                    X = (int)StartPoint.Value.X,
                    Y = (int)StartPoint.Value.Y,
                    Width = (int)(Endpoint.Value.X - StartPoint.Value.X),
                    Height = (int)(Endpoint.Value.Y - StartPoint.Value.Y)


                };


                // Assign coordinates of all four corners separated by semicolons PONumberCoordinates
                //po number
                if (IsPONumSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    PONumberCoordinates = PdfCoordinates;
                    IsPONumSelectedArea = false;

                }
                //date
                if (IsDateSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    DateCoordinates = PdfCoordinates;
                    IsDateSelectedArea = false;

                }
                //customer
                if (IsCustomerSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    CustomerCoordinates = PdfCoordinates;
                    IsCustomerSelectedArea = false;

                }
                //contact
                if (IsContactSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    ContactCoordinates = PdfCoordinates;
                    IsContactSelectedArea = false;

                }
                //Amount
                if (IsAmountSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    AmountCoordinates = PdfCoordinates;
                    IsAmountSelectedArea = false;

                }

                //Currency

                if (IsCurrencySelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    CurrencyCoordinates = PdfCoordinates;
                    IsCurrencySelectedArea = false;

                }
                //IsShipToSelectedArea

                if (IsShipToSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    ShipTOCoordinates = PdfCoordinates;
                    IsShipToSelectedArea = false;

                }

                //IsIncotermsSelectedArea

                if (IsIncotermsSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    IncotermsCoordinates = PdfCoordinates;
                    IsIncotermsSelectedArea = false;

                }

                //payment terms

                if (IsPayTermsSelectedArea)
                {
                    PdfCoordinates = $"{rect.X}, {rect.Y}; " +
                                     $"{rect.X + rect.Width}, {rect.Y}; " +
                                     $"{rect.X}, {rect.Y + rect.Height}; " +
                                     $"{rect.X + rect.Width}, {rect.Y + rect.Height}";

                    PaymentTermsCoordinates = PdfCoordinates;
                    IsPayTermsSelectedArea = false;

                }

                // Add the new rectangle to OverlayElements collection
                // Convert the rectangle to GraphicsCoordinates
                var graphicCoord = new GraphicsCoordinates
                {
                    X = rect.X,
                    Y = rect.Y,
                    Width = rect.Width,
                    Height = rect.Height,
                    BorderThickness = 2
                };


                //StartPoint = null;
                //Endpoint = null;
            }




        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>

        private void ChangeSelectedTextListCommandCommandAction(object obj)
        {
            if (SelectedPONumberText == "Location")
            {
                IsPONumberTextVisibility = Visibility.Hidden;
                IsPOnumberLocationVisibility = Visibility.Visible;
            }
            if (SelectedPONumberText == "Text")
            {
                IsPONumberTextVisibility = Visibility.Visible;
                IsPOnumberLocationVisibility = Visibility.Hidden;
            }


            if (SelectedDateText == "Location")
            {
                IsPODateTextVisibility = Visibility.Hidden;
                IsPODateLocationVisibility = Visibility.Visible;
            }
            if (SelectedDateText == "Text")
            {
                IsPODateTextVisibility = Visibility.Visible;
                IsPODateLocationVisibility = Visibility.Hidden;
            }

            if (SelectedCustomerText == "Location")
            {
                IsPOCustomerTextVisibility = Visibility.Hidden;
                IsPOCustomerLocationVisibility = Visibility.Visible;
            }
            if (SelectedCustomerText == "Text")
            {
                IsPOCustomerTextVisibility = Visibility.Visible;
                IsPOCustomerLocationVisibility = Visibility.Hidden;
            }

            if (SelectedContactText == "Location")
            {
                IsPOContactTextVisibility = Visibility.Hidden;
                IsPOContactLocationVisibility = Visibility.Visible;
            }
            if (SelectedContactText == "Text")
            {
                IsPOContactTextVisibility = Visibility.Visible;
                IsPOContactLocationVisibility = Visibility.Hidden;
            }

            if (SelectedAmountText == "Location")
            {
                IsPOAmountTextVisibility = Visibility.Hidden;
                IsPOAmountLocationVisibility = Visibility.Visible;
            }
            if (SelectedAmountText == "Text")
            {
                IsPOAmountTextVisibility = Visibility.Visible;
                IsPOAmountLocationVisibility = Visibility.Hidden;
            }
            if (SelectedCurrencyText == "Location")
            {
                IsPOCurrencyTextVisibility = Visibility.Hidden;
                IsPOCurrencyLocationVisibility = Visibility.Visible;
            }
            if (SelectedCurrencyText == "Text")
            {
                IsPOCurrencyTextVisibility = Visibility.Visible;
                IsPOCurrencyLocationVisibility = Visibility.Hidden;
            }

            if (SelectedShipTOText == "Location")
            {
                IsPOShipToTextVisibility = Visibility.Hidden;
                IsPOShipToLocationVisibility = Visibility.Visible;
            }
            if (SelectedShipTOText == "Text")
            {
                IsPOShipToTextVisibility = Visibility.Visible;
                IsPOShipToLocationVisibility = Visibility.Hidden; ;
            }
            if (SelectedIncotermsText == "Location")
            {
                IsPOIncotermsTextVisibility = Visibility.Hidden;
                IsPOIncotermsLocationVisibility = Visibility.Visible;
            }
            if (SelectedIncotermsText == "Text")
            {
                IsPOIncotermsTextVisibility = Visibility.Visible;
                IsPOIncotermsLocationVisibility = Visibility.Hidden;
            }
            if (SelectedPaymentTermsText == "Location")
            {
                IsPOPaymentTermsTextVisibility = Visibility.Hidden;
                IsPOPaymentTermsLocationVisibility = Visibility.Visible;
            }
            if (SelectedPaymentTermsText == "Text")
            {
                IsPOPaymentTermsTextVisibility = Visibility.Visible;
                IsPOPaymentTermsLocationVisibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void PaymentTermsSelectAreaActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PaymentTermsSelectAreaActionCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsPayTermsSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsPayTermsSelectedArea)
                {
                    PdfCoordinates = null;
                }

                PaymentTermsCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method PaymentTermsSelectAreaActionCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PaymentTermsSelectAreaActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PaymentTermsSelectAreaActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PaymentTermsSelectAreaActionCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>

        private void IncotermsSelectAreaActionCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method IncotermsSelectAreaActionCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsIncotermsSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsIncotermsSelectedArea)
                {
                    PdfCoordinates = null;
                }

                IncotermsCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method IncotermsSelectAreaActionCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in IncotermsSelectAreaActionCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in IncotermsSelectAreaActionCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in IncotermsSelectAreaActionCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void ShipTOSelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ShipTOSelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsShipToSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsShipToSelectedArea)
                {
                    PdfCoordinates = null;
                }

                ShipTOCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method ShipTOSelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ShipTOSelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ShipTOSelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ShipTOSelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void CurrencySelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CurrencySelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsCurrencySelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsCurrencySelectedArea)
                {
                    PdfCoordinates = null;
                }

                CurrencyCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method CurrencySelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CurrencySelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CurrencySelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CurrencySelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void AmountSelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AmountSelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsAmountSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsAmountSelectedArea)
                {
                    PdfCoordinates = null;
                }

                AmountCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method AmountSelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AmountSelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AmountSelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AmountSelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void ContactSelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ContactSelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsContactSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsContactSelectedArea)
                {
                    PdfCoordinates = null;
                }

                ContactCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method ContactSelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ContactSelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ContactSelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in ContactSelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void CustomerSelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CustomerSelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsCustomerSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsCustomerSelectedArea)
                {
                    PdfCoordinates = null;
                }

                CustomerCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method CustomerSelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CustomerSelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in CustomerSelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in CustomerSelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void DateSelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method DateSelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                IsDateSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsDateSelectedArea)
                {
                    PdfCoordinates = null;
                }


                DateCoordinates = PdfCoordinates;


                GeosApplication.Instance.Logger.Log("Method DateSelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DateSelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in DateSelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in DateSelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        /// <summary>
        /// //[pramod.misal][09-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>
        private void PONumberSelectAreaActionCommandButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method PONumberSelectAreaActionCommandButtonCommandAction()....", category: Category.Info, priority: Priority.Low);


                IsPONumSelectedArea = true;
                IsSelectedArea = true;
                IsCanvasVisible = Visibility.Hidden;
                if (IsPONumSelectedArea)
                {
                    PdfCoordinates = null;
                }

                PONumberCoordinates = PdfCoordinates;





                GeosApplication.Instance.Logger.Log("Method PONumberSelectAreaActionCommandButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PONumberSelectAreaActionCommandButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in PONumberSelectAreaActionCommandButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PONumberSelectAreaActionCommandButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }

        }

        private void activateDrawingButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// //[pramod.misal][07-01-2025][GEOS2-6734]
        /// </summary>
        /// <param name="obj"></param>

        private void AddEditNewOtTemplateViewAcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddEditNewOtTemplateViewAcceptButtonCommandAction()....", category: Category.Info, priority: Priority.Low);
                string error = EnableValidationAndGetError();
                if (string.IsNullOrEmpty(error))
                    InformationError = null;
                else
                    InformationError = "";
                PropertyChanged(this, new PropertyChangedEventArgs("TemplateName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexGroup"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexRegion"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexCountry"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexPlant"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedTemplateFile"));
                //[Rahul.Gadhave][GEOS2-6734][Date:16-01-2024]
                //Mapping Fields for Excel
                if (FileExtension == ".xls" || FileExtension == ".xlsx")
                {
                    //PropertyChanged(this, new PropertyChangedEventArgs("PONumberRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("PODateRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("CustomerRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("ContactRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("AmountRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("CurrencyRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("ShipToRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("IncotermsRangeExcel"));
                    //PropertyChanged(this, new PropertyChangedEventArgs("PaymentTermsRangeExcel"));
                }
                if (FileExtension == ".pdf")
                {
                    if (SelectedPONumberText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PONumberKeyword"));
                        PropertyChanged(this, new PropertyChangedEventArgs("PONumberDelimiter"));
                    }
                    else
                    {
                        if (SelectedPONumberText == "Location")
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("PONumberCoordinates"));
                        }
                    }
                    ///////////////////////////////////////////////
                    // Mapping Fields for Pdf- PoDate
                    if (SelectedDateText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateKeyword"));
                        PropertyChanged(this, new PropertyChangedEventArgs("DateDelimiter"));
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateCoordinates"));
                    }
                    // Mapping Fields for Pdf- PoCustomerKey
                    if (SelectedCustomerText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CustomerKeyWord"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CustomerDelimiter"));
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CustomerCoordinates"));
                    }
                    // Mapping Fields for Pdf- PoContact
                    if (SelectedContactText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ContactKeyWord"));
                        PropertyChanged(this, new PropertyChangedEventArgs("ContactDelimiter"));
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ContactCoordinates"));
                    }
                    // Mapping Fields for Pdf- PoAmount
                    if (SelectedAmountText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AmountKeyWord"));
                        PropertyChanged(this, new PropertyChangedEventArgs("AmountDelimiter"));
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AmountCoordinates"));
                    }
                    // Mapping Fields for Pdf- PoCurrency
                    if (SelectedCurrencyText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CurrencyKeyword"));
                        PropertyChanged(this, new PropertyChangedEventArgs("CurrencyDelimiter"));
                    }
                    else
                    {

                        PropertyChanged(this, new PropertyChangedEventArgs("CurrencyCoordinates"));
                    }

                    // Mapping Fields for Pdf- PoShipTo
                    if (SelectedShipTOText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ShipTOKeyword"));
                        PropertyChanged(this, new PropertyChangedEventArgs("ShipTODelimiter"));
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ShipTOCoordinates"));
                    }

                    // Mapping Fields for Pdf- PoIncoterms
                    if (SelectedIncotermsText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IncotermsKeyWord"));
                        PropertyChanged(this, new PropertyChangedEventArgs("IncotermsDelimiter"));
                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IncotermsCoordinates"));
                    }
                    // Mapping Fields for Pdf- PoPaymentTerms
                    if (SelectedPaymentTermsText == "Text")
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PaymentTermsKeyWord"));
                        PropertyChanged(this, new PropertyChangedEventArgs("PaymentTermsDelimiter"));

                    }
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PaymentTermsCoordinates"));
                    }
                }
                if (error != null)
                {
                    return;
                }

                if (!DXSplashScreen.IsActive)
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
                        DevExpress.Mvvm.UI.WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                        win.Topmost = false;
                        return win;
                    }, x =>
                    {
                        return new Views.SplashScreenView() { DataContext = new SplashScreenViewModel() };
                    }, null, null);
                }
                Template = new OtRequestTemplates();
                if (FileExtension==".pdf" && IdOTRequestTemplateCellFieldPoNumber != 0 && IdOTRequestTemplateCellFieldPoNumber !=0)
                {
                //   OTMService = new OTMServiceController("localhost:6699");
                    IsDeleted = OTMService.DeletetCellFields_V2610(IdOtRequestTemplate);
                    if (IsDeleted == true)
                    {
                        Template.ChangingField = true;
                    }
                }
                else
                {
                   if(FileExtension == ".xls" || FileExtension == ".xlsx"&& IdOTRequestTemplateTextFieldPoNumber!=0 && IdOTRequestTemplateTextFieldPoNumber !=0)
                    {
                       //  OTMService = new OTMServiceController("localhost:6699");
                        IsDeleted = OTMService.DeletetTextAndLocation_V2610(IdOtRequestTemplate);
                        if(IsDeleted == true)
                        {
                            Template.ChangingField = true;
                        }
                    }
                }
                if (IsNew)
                {
                    string cellsPONumber = PONumberUpdatedRangeExcel;
                    string cellsDate = UpdatedpODateRangeExcel;
                    string cellsCustomer = UpdatedcustomerRangeExcel;
                    string cellsAmount = UpdatedamountRangeExcel;
                    string cellsCurrency = UpdatedcurrencyRangeExcel;
                    string cellsShipTo = UpdatedshipToRangeExcel;
                    string cellsIncoterms = UpdatedincotermsRangeExcel;
                    string cellsPaymentTerms = UpdatedPaymentTermsKeywordExcel;

                    //[pooja.jadhav][20-01-2025][GEOS2-6734]
                  //  Template = new OtRequestTemplates();

                    Template.Code = RegistrationNumber;
                    Template.TemplateName = TemplateName;
                    Template.IdGroup = GroupList[SelectedIndexGroup].IdCustomer;
                    Template.IdCustomer = GroupList[SelectedIndexGroup].IdCustomer;
                    Template.Group = GroupList[SelectedIndexGroup].CustomerName;
                    Template.IdRegion = RegionList[SelectedIndexRegion].IdRegion;
                    Template.Region = RegionList[SelectedIndexRegion].RegionName;
                    Template.IdCountry = CountryList[SelectedIndexCountry].IdCountry;
                    Template.Country = CountryList[SelectedIndexCountry].Name;
                    Template.IdPlant = CustomerPlants[SelectedIndexPlant].IdCustomerPlant;
                    //
                    Template.IdCustomerPlant = CustomerPlants[SelectedIndexPlant].IdCustomerPlant;
                    Template.Plant = CustomerPlants[SelectedIndexPlant].CustomerPlantName;
                    Template.File = SelectedTemplateFile.SavedFileName;
                    Template.fileExtension = FileExtension;
                    Template.InUse = 1;
                    Template.FileDocInBytes = SelectedTemplateFile.TemplateAttachementsDocInBytes;
                    Template.IdCreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    //Template.CreatedBy = GeosApplication.Instance.ActiveUser.FullName;
                    Template.CreatedBy = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName;
                    //GeosApplication.Instance.EmployeeCode
                    Template.CreatedAt = DateTime.Now;

                    Template.TransactionOperation = ModelBase.TransactionOperations.Add;

                    Dictionary<int, string> PoAnalizerTag = OTMService.GetPoAnalizerTag_V2600();
                    Dictionary<int, string> PORequestTemplateFieldType = OTMService.GetPORequestTemplateFieldType_V2600();
                    Dictionary<int, string> PORequestTemplateFieldTypeForPDF = OTMService.GetPORequestTemplateFieldTypeForPDF_V2600();

                    Template.OtRequestTemplateFeildOptions = new ObservableCollection<OtRequestTemplateFeildOptions>();

                    foreach (var kvp in PoAnalizerTag)
                    {
                        if (FileExtension == ".xls" || FileExtension == ".xlsx")
                        {
                            OtRequestTemplateFeildOptions otRequestTemplateFeildOption = new OtRequestTemplateFeildOptions();
                            otRequestTemplateFeildOption.IdField = kvp.Key;
                            otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldType.Keys.First();
                            otRequestTemplateFeildOption.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            otRequestTemplateFeildOption.TransactionOperation = ModelBase.TransactionOperations.Add;

                            if (kvp.Value == "PO_Number")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = PONumberRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = PONumberKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = PONumberDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Date")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = PODateRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = PODateKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = PODateDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Customer")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = CustomerRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = CustomerKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = CustomerDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Email")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = ContactRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = ContactKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = ContactDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Total")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = AmountRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = AmountKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = AmountDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Currency")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = CurrencyRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = CurrencyKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = CurrencyDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Ship To")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = ShipToRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = ShipToKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = ShipToDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Incoterm")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = IncotermsRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = IncotermsKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = IncotermsDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Payment Terms")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = PaymentTermsRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = PaymentTermsKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = PaymentTermsDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }



                            if (otRequestTemplateFeildOption.OtRequestTemplateCellField != null)
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.TransactionOperation = ModelBase.TransactionOperations.Add;
                                Template.OtRequestTemplateFeildOptions.Add(otRequestTemplateFeildOption);
                            }

                        }

                        if (FileExtension == ".pdf")
                        {
                            OtRequestTemplateFeildOptions otRequestTemplateFeildOption = new OtRequestTemplateFeildOptions();
                            otRequestTemplateFeildOption.IdField = kvp.Key;

                            otRequestTemplateFeildOption.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            otRequestTemplateFeildOption.TransactionOperation = ModelBase.TransactionOperations.Add;

                            if (kvp.Value == "PO_Number")
                            {
                                if (SelectedPONumberText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = PONumberKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = PONumberDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = PONumberCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Date")
                            {
                                if (SelectedDateText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = DateKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = DateDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = DateCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Customer")
                            {
                                if (SelectedCustomerText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = CustomerKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = CustomerDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = CustomerCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Email")
                            {
                                if (SelectedContactText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = ContactKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = ContactDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = ContactCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Total")
                            {
                                if (SelectedAmountText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = AmountKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = AmountDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = AmountCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Currency")
                            {
                                if (SelectedCurrencyText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = CurrencyKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = CurrencyDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = CurrencyCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Ship To")
                            {
                                if (SelectedShipTOText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = ShipTOKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = ShipTODelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = ShipTOCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Incoterm")
                            {
                                if (SelectedIncotermsText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = IncotermsKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = IncotermsDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = IncotermsCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Payment Terms")
                            {
                                if (SelectedPaymentTermsText == "Text")
                                {
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = PaymentTermsKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = PaymentTermsDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = PaymentTermsCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }


                            if (otRequestTemplateFeildOption.OtRequestTemplateTextField != null)
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateTextField.TransactionOperation = ModelBase.TransactionOperations.Add;
                                Template.OtRequestTemplateFeildOptions.Add(otRequestTemplateFeildOption);
                            }

                            if (otRequestTemplateFeildOption.OtRequestTemplateLocationField != null)
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateLocationField.TransactionOperation = ModelBase.TransactionOperations.Add;
                                Template.OtRequestTemplateFeildOptions.Add(otRequestTemplateFeildOption);
                            }
                        }

                    }

                   //  OTMService = new OTMServiceController("localhost:6699");
                    Addedtemplate = OTMService.AddUpdateOTRequestTemplates_V2610(Template);
                    Template.IdOTRequestTemplate = Addedtemplate.IdOTRequestTemplate;
                    if (Addedtemplate != null)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OTTemplateAddedSuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }

                }
                else
                {
                    string cellsPONumber = PONumberUpdatedRangeExcel;
                    string cellsDate = UpdatedpODateRangeExcel;
                    string cellsCustomer = UpdatedcustomerRangeExcel;
                    string cellsAmount = UpdatedamountRangeExcel;
                    string cellsCurrency = UpdatedcurrencyRangeExcel;
                    string cellsShipTo = UpdatedshipToRangeExcel;
                    string cellsIncoterms = UpdatedincotermsRangeExcel;
                    string cellsPaymentTerms = UpdatedPaymentTermsKeywordExcel;

                    //[pooja.jadhav][20-01-2025][GEOS2-6734]
                    //Template = new OtRequestTemplates();
                    Template.IdOtRequestTemplate = (int)IdOtRequestTemplate;
                    Template.Code = RegistrationNumber;
                    Template.TemplateName = TemplateName;
                    Template.IdGroup = GroupList[SelectedIndexGroup].IdCustomer;
                    Template.IdCustomer = GroupList[SelectedIndexGroup].IdCustomer;
                    Template.Group = GroupList[SelectedIndexGroup].CustomerName;
                    Template.IdRegion = RegionList[SelectedIndexRegion].IdRegion;
                    Template.Region = RegionList[SelectedIndexRegion].RegionName;
                    Template.IdCountry = CountryList[SelectedIndexCountry].IdCountry;
                    Template.Country = CountryList[SelectedIndexCountry].Name;
                    Template.IdPlant = CustomerPlants[SelectedIndexPlant].IdCustomerPlant;
                    Template.IdCustomerPlant = CustomerPlants[SelectedIndexPlant].IdCustomerPlant;
                    Template.Plant = CustomerPlants[SelectedIndexPlant].CustomerPlantName;
                    Template.File = SelectedTemplateFile.SavedFileName;
                    Template.fileExtension = FileExtension;
                    Template.InUse = InUse;
                    Template.FileDocInBytes = SelectedTemplateFile.TemplateAttachementsDocInBytes;
                    //Template.FileDocInBytes = SelectedTemplateFile.FileDocInBytes;
                    Template.IdUpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                    Template.UpdatedBy = GeosApplication.Instance.ActiveUser.FirstName + " " + GeosApplication.Instance.ActiveUser.LastName;
                    Template.UpdatedAt = DateTime.Now;
                    Template.TransactionOperation = ModelBase.TransactionOperations.Update;

                    Dictionary<int, string> PoAnalizerTag = OTMService.GetPoAnalizerTag_V2600();
                    Dictionary<int, string> PORequestTemplateFieldType = OTMService.GetPORequestTemplateFieldType_V2600();
                    Dictionary<int, string> PORequestTemplateFieldTypeForPDF = OTMService.GetPORequestTemplateFieldTypeForPDF_V2600();

                    Template.OtRequestTemplateFeildOptions = new ObservableCollection<OtRequestTemplateFeildOptions>();

                    foreach (var kvp in PoAnalizerTag)
                    {
                        if (FileExtension == ".xls" || FileExtension == ".xlsx")
                        {
                            OtRequestTemplateFeildOptions otRequestTemplateFeildOption = new OtRequestTemplateFeildOptions();
                            otRequestTemplateFeildOption.IdOTRequestTemplate = (int)IdOtRequestTemplate;
                            otRequestTemplateFeildOption.IdField = kvp.Key;
                            otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldType.Keys.First();
                            otRequestTemplateFeildOption.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            otRequestTemplateFeildOption.TransactionOperation = ModelBase.TransactionOperations.Update;
                            otRequestTemplateFeildOption.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                            if (kvp.Value == "PO_Number")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPoNumber;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldPoNumber;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = PONumberRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = PONumberKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = PONumberDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Date")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionDate;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldDate;

                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = PODateRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = PODateKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = PODateDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Customer")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();

                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCustomer;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldCustomer;

                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = CustomerRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = CustomerKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = CustomerDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Email")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionContact;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldContact;

                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = ContactRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = ContactKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = ContactDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Total")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionAmount;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldAmount;


                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = AmountRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = AmountKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = AmountDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Currency")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();

                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCurrency;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldCurrency;


                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = CurrencyRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = CurrencyKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = CurrencyDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Ship To")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();
                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionShipTo;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldShipTo;

                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = ShipToRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = ShipToKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = ShipToDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Incoterm")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();

                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionIncoterms;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldIncoterms;

                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = IncotermsRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = IncotermsKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = IncotermsDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }
                            else if (kvp.Value == "Payment Terms")
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField = new OtRequestTemplateCellFields();

                                otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPaymentTerms;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.IdOTRequestTemplateCellField = IdOTRequestTemplateCellFieldPaymentTerms;

                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Cells = PaymentTermsRangeExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Keyword = PaymentTermsKeywordExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.Delimiter = PaymentTermsDelimiterExcel;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            }

                            if (otRequestTemplateFeildOption.OtRequestTemplateCellField != null)
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateCellField.TransactionOperation = ModelBase.TransactionOperations.Update;
                                Template.OtRequestTemplateFeildOptions.Add(otRequestTemplateFeildOption);
                            }

                        }

                        if (FileExtension == ".pdf")
                        {
                            OtRequestTemplateFeildOptions otRequestTemplateFeildOption = new OtRequestTemplateFeildOptions();
                            otRequestTemplateFeildOption.IdField = kvp.Key;
                            otRequestTemplateFeildOption.IdOTRequestTemplate = (int)IdOtRequestTemplate;
                            otRequestTemplateFeildOption.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                            otRequestTemplateFeildOption.TransactionOperation = ModelBase.TransactionOperations.Update;

                            if (kvp.Value == "PO_Number")
                            {

                                if (SelectedPONumberText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldPoNumber!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPoNumber;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldPoNumber;

                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = PONumberCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                       
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPoNumber;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldPoNumber;

                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = PONumberKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = PONumberDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;

                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldPoNumber!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPoNumber;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldPoNumber;

                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = PONumberKeyword;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = PONumberDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                       

                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPoNumber;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldPoNumber;

                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = PONumberCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Date")
                            {
                                if (SelectedDateText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldDate!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionDate;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldDate;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = DateCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                        
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionDate;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldDate;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = DateKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = DateDelimiter;
                                   
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldDate!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionDate;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldDate;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = DateKeyword;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = DateDelimiter;
                                  
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionDate;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldDate;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = DateCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Customer")
                            {
                                if (SelectedCustomerText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldCustomer!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCustomer;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldCustomer;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = CustomerCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                       
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCustomer;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldCustomer;

                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = CustomerKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = CustomerDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldCustomer!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCustomer;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldCustomer;

                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = CustomerKeyWord;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = CustomerDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCustomer;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldCustomer;

                                    
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = CustomerCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Email")
                            {
                                if (SelectedContactText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldContact!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionContact;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldContact;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = ContactCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                        
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionContact;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldContact;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = ContactKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = ContactDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldContact!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionContact;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldContact;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = ContactKeyWord;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = ContactDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                       
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionContact;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldContact;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = ContactCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Total")
                            {
                                if (SelectedAmountText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldAmount!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionAmount;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldAmount;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = AmountCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                       
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionAmount;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldAmount;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = AmountKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = AmountDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldAmount!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionAmount;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldAmount;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = AmountKeyWord;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = AmountDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                   
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionAmount;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldAmount;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = AmountCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Currency")
                            {
                                if (SelectedCurrencyText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldCurrency!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCurrency;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldCurrency;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = CurrencyCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                       
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCurrency;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldCurrency;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = CurrencyKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = CurrencyDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldCurrency!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCurrency;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldCurrency;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = CurrencyKeyword;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = CurrencyDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                     
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionCurrency;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldCurrency;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = CurrencyCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Ship To")
                            {
                                if (SelectedShipTOText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldShipTo!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionShipTo;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldShipTo;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = ShipTOCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                      
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionShipTo;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldShipTo;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = ShipTOKeyword;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = ShipTODelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldShipTo!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionShipTo;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldShipTo;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = ShipTOKeyword;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = ShipTODelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                        
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionShipTo;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldShipTo;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = ShipTOCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Incoterm")
                            {
                                if (SelectedIncotermsText == "Text")
                                {
                                    if (IdOTRequestTemplateLocationFieldIncoterms != 0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionIncoterms;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldIncoterms;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = IncotermsCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionIncoterms;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldIncoterms;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = IncotermsKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = IncotermsDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldIncoterms!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionIncoterms;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldIncoterms;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = IncotermsKeyWord;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = IncotermsDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionIncoterms;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldIncoterms;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = IncotermsCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            else if (kvp.Value == "Payment Terms")
                            {
                                if (SelectedPaymentTermsText == "Text")
                                {
                                    if(IdOTRequestTemplateLocationFieldPaymentTerms!=0)
                                    {
                                        string valueToFind1 = "Location";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPaymentTerms;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldPaymentTerms;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = PaymentTermsCoordinates;
                                        otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    }
                                    string valueToFind = "Text";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPaymentTerms;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldPaymentTerms;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = PaymentTermsKeyWord;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = PaymentTermsDelimiter;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateTextField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                                else
                                {
                                    if(IdOTRequestTemplateTextFieldPaymentTerms!=0)
                                    {
                                        string valueToFind1 = "Text";
                                        otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind1).Key;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField = new OtRequestTemplateTextFields();
                                        otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPaymentTerms;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.IdOTRequestTemplateTextField = IdOTRequestTemplateTextFieldPaymentTerms;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Keyword = PaymentTermsKeyWord;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.Delimiter = PaymentTermsDelimiter;
                                        otRequestTemplateFeildOption.OtRequestTemplateTextField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    }
                                    string valueToFind = "Location";
                                    otRequestTemplateFeildOption.IdFieldType = PORequestTemplateFieldTypeForPDF.FirstOrDefault(x => x.Value == valueToFind).Key;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField = new OtRequestTemplateLocationFields();
                                    otRequestTemplateFeildOption.IdOTRequestTemplateFieldOption = IdOTRequestTemplateFieldOptionPaymentTerms;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.IdOTRequestTemplateLocationField = IdOTRequestTemplateLocationFieldPaymentTerms;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.Coordinates = PaymentTermsCoordinates;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.UpdatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                    otRequestTemplateFeildOption.OtRequestTemplateLocationField.CreatedBy = GeosApplication.Instance.ActiveUser.IdUser;
                                }
                            }
                            if (otRequestTemplateFeildOption.OtRequestTemplateTextField != null)
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateTextField.TransactionOperation = ModelBase.TransactionOperations.Update;
                                Template.OtRequestTemplateFeildOptions.Add(otRequestTemplateFeildOption);
                            }

                            if (otRequestTemplateFeildOption.OtRequestTemplateLocationField != null)
                            {
                                otRequestTemplateFeildOption.OtRequestTemplateLocationField.TransactionOperation = ModelBase.TransactionOperations.Update;
                                Template.OtRequestTemplateFeildOptions.Add(otRequestTemplateFeildOption);
                            }
                        }

                    }

                    // OTMService = new OTMServiceController("localhost:6699");
                    Addedtemplate = OTMService.AddUpdateOTRequestTemplates_V2610(Template);

                    if (Addedtemplate != null)
                    {
                        CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("OTTemplateUpdatedSuccessfully").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                        RequestClose(null, null);
                    }
                }

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddEditNewOtTemplateViewAcceptButtonCommandAction() executed Successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddEditNewOtTemplateViewAcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Info, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddEditNewOtTemplateViewAcceptButtonCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Info, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AddEditNewOtTemplateViewAcceptButtonCommandAction() Method " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }

        private void ExtractDataFromCell(string cell, string keyword, string delimiter)
        {



        }

        /// <summary>
        /// //[pramod.misal][07-01-2025][GEOS2-]
        /// </summary>
        /// <param name="obj"></param>
        /// 
        public void BrowseFileAction(object obj)
        {
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() ...", category: Category.Info, priority: Priority.Low);
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                //dlg.DefaultExt = "Pdf Files|*.pdf";
                //dlg.Filter = "Pdf Files|*.pdf";
                dlg.DefaultExt = "*.*"; // Default extension if none is selected
                //dlg.Filter = "All Supported Files|*.pdf;*.tif;*.tiff;*.jpg;*.jpeg;*.docx;*.xlsx|PDF Files|*.pdf|TIFF Files|*.tif;*.tiff|Image Files|*.jpg;*.jpeg|Word Documents|*.docx|Excel Spreadsheets|*.xlsx";
                //dlg.Filter = "All Supported Files|*.pdf;*.tif;*.tiff;*.jpg;*.jpeg;*.docx;*.png|PDF Files|*.pdf|TIFF Files|*.tif;*.tiff|Image Files|*.jpg;*.jpeg;*.png|Word Documents|*.docx";
                dlg.Filter = "PDF and Excel Files|*.pdf;*.xls;*.xlsx|PDF Files|*.pdf|Excel Files|*.xls;*.xlsx";

               



                // string fileExtension1 = System.IO.Path.GetExtension(dlg.FileName).ToLower();
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    if (IsNew == false)
                    {
                        string selectedFile = dlg.FileName; // Get the selected file name
                        string fileExtension1 = System.IO.Path.GetExtension(selectedFile).ToLower();
                        if (fileExtension1 == ".pdf")
                        {
                            PONumberKeyword = string.Empty;
                            PONumberDelimiter = string.Empty;
                            PONumberCoordinates=string.Empty;

                            DateKeyword = string.Empty;
                            DateDelimiter = string.Empty;
                            DateCoordinates = string.Empty;

                            CustomerKeyWord = string.Empty;
                            CustomerDelimiter = string.Empty;
                            CustomerCoordinates = string.Empty;

                            ContactKeyWord = string.Empty;
                            ContactDelimiter = string.Empty;
                            ContactCoordinates = string.Empty;

                            AmountKeyWord = string.Empty;
                            AmountDelimiter = string.Empty;
                            AmountCoordinates = string.Empty;

                            CurrencyKeyword = string.Empty;
                            CurrencyDelimiter = string.Empty;
                            CurrencyCoordinates = string.Empty;

                            ShipTOKeyword = string.Empty;
                            ShipTODelimiter = string.Empty;
                            ShipTOCoordinates = string.Empty;

                            IncotermsKeyWord = string.Empty;
                            IncotermsDelimiter = string.Empty;
                            IncotermsCoordinates = string.Empty;

                            PaymentTermsKeyWord = string.Empty;
                            PaymentTermsDelimiter = string.Empty;
                            PaymentTermsCoordinates = string.Empty;

                            IsPONumberTextVisibility = Visibility.Visible;
                            IsPODateTextVisibility = Visibility.Visible;
                            IsPOCustomerTextVisibility = Visibility.Visible;
                            IsPOContactTextVisibility = Visibility.Visible;
                            IsPOAmountTextVisibility = Visibility.Visible;
                            IsPOCurrencyTextVisibility = Visibility.Visible;
                            IsPOShipToTextVisibility = Visibility.Visible;
                            IsPOIncotermsTextVisibility = Visibility.Visible;
                            IsPOPaymentTermsTextVisibility = Visibility.Visible;

                            IsPOnumberLocationVisibility = Visibility.Hidden;
                            IsPODateLocationVisibility = Visibility.Hidden;
                            IsPOCustomerLocationVisibility = Visibility.Hidden;
                            IsPOContactLocationVisibility = Visibility.Hidden;
                            IsPOAmountLocationVisibility = Visibility.Hidden;
                            IsPOCurrencyLocationVisibility = Visibility.Hidden;
                            IsPOShipToLocationVisibility = Visibility.Hidden;
                            IsPOIncotermsLocationVisibility = Visibility.Hidden;
                            IsPOPaymentTermsLocationVisibility = Visibility.Hidden;

                            SelectedPONumberText = "Text";
                            SelectedDateText = "Text";
                            SelectedCustomerText ="Text";
                            SelectedContactText = "Text";
                            SelectedAmountText = "Text";
                            SelectedCurrencyText ="Text";
                            SelectedShipTOText ="Text";
                            SelectedIncotermsText = "Text";
                            SelectedPaymentTermsText ="Text";

                        }
                        else
                        {
                            PONumberRangeExcel = string.Empty;
                            PONumberKeywordExcel = string.Empty;
                            PONumberDelimiterExcel = string.Empty;

                            PODateRangeExcel = string.Empty;
                            PODateKeywordExcel = string.Empty;
                            PODateDelimiterExcel = string.Empty;

                            CustomerRangeExcel = string.Empty;
                            CustomerKeywordExcel = string.Empty;
                            CustomerDelimiterExcel = string.Empty;

                            ContactRangeExcel = string.Empty;
                            ContactKeywordExcel = string.Empty;
                            ContactDelimiterExcel = string.Empty;

                            AmountRangeExcel = string.Empty;
                            AmountKeywordExcel = string.Empty;
                            AmountDelimiterExcel = string.Empty;

                            CurrencyRangeExcel = string.Empty;
                            CurrencyKeywordExcel = string.Empty;
                            CurrencyDelimiterExcel = string.Empty;

                            ShipToRangeExcel = string.Empty;
                            ShipToKeywordExcel = string.Empty;
                            ShipToDelimiterExcel = string.Empty;

                            IncotermsRangeExcel = string.Empty;
                            IncotermsKeywordExcel = string.Empty;
                            IncotermsDelimiterExcel = string.Empty;


                            PaymentTermsRangeExcel = string.Empty;
                            PaymentTermsKeywordExcel = string.Empty;
                            PaymentTermsDelimiterExcel = string.Empty;
                        }
                    }
                    if (IsNew == true)
                    {
                        string selectedFile = dlg.FileName; // Get the selected file name
                        string fileExtension2 = System.IO.Path.GetExtension(selectedFile).ToLower();
                        if (fileExtension2 == ".pdf")
                        {
                            PONumberKeyword = string.Empty;
                            PONumberDelimiter = string.Empty;
                            PONumberCoordinates = string.Empty;

                            DateKeyword = string.Empty;
                            DateDelimiter = string.Empty;
                            DateCoordinates = string.Empty;

                            CustomerKeyWord = string.Empty;
                            CustomerDelimiter = string.Empty;
                            CustomerCoordinates = string.Empty;

                            ContactKeyWord = string.Empty;
                            ContactDelimiter = string.Empty;
                            ContactCoordinates = string.Empty;

                            AmountKeyWord = string.Empty;
                            AmountDelimiter = string.Empty;
                            AmountCoordinates = string.Empty;

                            CurrencyKeyword = string.Empty;
                            CurrencyDelimiter = string.Empty;
                            CurrencyCoordinates = string.Empty;

                            ShipTOKeyword = string.Empty;
                            ShipTODelimiter = string.Empty;
                            ShipTOCoordinates = string.Empty;

                            IncotermsKeyWord = string.Empty;
                            IncotermsDelimiter = string.Empty;
                            IncotermsCoordinates = string.Empty;

                            PaymentTermsKeyWord = string.Empty;
                            PaymentTermsDelimiter = string.Empty;
                            PaymentTermsCoordinates = string.Empty;

                            IsPONumberTextVisibility = Visibility.Visible;
                            IsPODateTextVisibility = Visibility.Visible;
                            IsPOCustomerTextVisibility = Visibility.Visible;
                            IsPOContactTextVisibility = Visibility.Visible;
                            IsPOAmountTextVisibility = Visibility.Visible;
                            IsPOCurrencyTextVisibility = Visibility.Visible;
                            IsPOShipToTextVisibility = Visibility.Visible;
                            IsPOIncotermsTextVisibility = Visibility.Visible;
                            IsPOPaymentTermsTextVisibility = Visibility.Visible;

                            IsPOnumberLocationVisibility = Visibility.Hidden;
                            IsPODateLocationVisibility = Visibility.Hidden;
                            IsPOCustomerLocationVisibility = Visibility.Hidden;
                            IsPOContactLocationVisibility = Visibility.Hidden;
                            IsPOAmountLocationVisibility = Visibility.Hidden;
                            IsPOCurrencyLocationVisibility = Visibility.Hidden;
                            IsPOShipToLocationVisibility = Visibility.Hidden;
                            IsPOIncotermsLocationVisibility = Visibility.Hidden;
                            IsPOPaymentTermsLocationVisibility = Visibility.Hidden;

                            SelectedPONumberText = "Text";
                            SelectedDateText = "Text";
                            SelectedCustomerText = "Text";
                            SelectedContactText = "Text";
                            SelectedAmountText = "Text";
                            SelectedCurrencyText = "Text";
                            SelectedShipTOText = "Text";
                            SelectedIncotermsText = "Text";
                            SelectedPaymentTermsText = "Text";

                        }
                        else
                        {
                            PONumberRangeExcel = string.Empty;
                            PONumberKeywordExcel = string.Empty;
                            PONumberDelimiterExcel = string.Empty;

                            PODateRangeExcel = string.Empty;
                            PODateKeywordExcel = string.Empty;
                            PODateDelimiterExcel = string.Empty;

                            CustomerRangeExcel = string.Empty;
                            CustomerKeywordExcel = string.Empty;
                            CustomerDelimiterExcel = string.Empty;

                            ContactRangeExcel = string.Empty;
                            ContactKeywordExcel = string.Empty;
                            ContactDelimiterExcel = string.Empty;

                            AmountRangeExcel = string.Empty;
                            AmountKeywordExcel = string.Empty;
                            AmountDelimiterExcel = string.Empty;

                            CurrencyRangeExcel = string.Empty;
                            CurrencyKeywordExcel = string.Empty;
                            CurrencyDelimiterExcel = string.Empty;

                            ShipToRangeExcel = string.Empty;
                            ShipToKeywordExcel = string.Empty;
                            ShipToDelimiterExcel = string.Empty;

                            IncotermsRangeExcel = string.Empty;
                            IncotermsKeywordExcel = string.Empty;
                            IncotermsDelimiterExcel = string.Empty;


                            PaymentTermsRangeExcel = string.Empty;
                            PaymentTermsKeywordExcel = string.Empty;
                            PaymentTermsDelimiterExcel = string.Empty;
                        }



                    }
                    FileInBytes = System.IO.File.ReadAllBytes(dlg.FileName);
                    templateAttachementsList = new ObservableCollection<TemplateAttachements>();

                    FileInfo file = new FileInfo(dlg.FileName);
                    Excelfile = new FileInfo(dlg.FileName);
                    TemplateAttachementSavedFileName = file.Name;
                    // Set flags based on file extension
                    string fileExtension = file.Extension.ToLower();

                    FileExtension = fileExtension;
                    if (fileExtension == ".pdf")
                    {
                        IsPdf = Visibility.Visible;
                        IsMappingFieldsActive = true;
                        IsMappingFildsVisible = Visibility.Visible;
                        IsExcel = Visibility.Hidden;
                    }
                    else if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        IsPdf = Visibility.Hidden;
                        IsMappingFieldsActive = true;
                        IsMappingFildsVisible = Visibility.Visible;
                        IsExcel = Visibility.Visible;
                    }
                    if (string.IsNullOrEmpty(FileName))
                    {
                        FileName = file.Name;
                        int index = FileName.LastIndexOf('.');
                        FileTobeSavedByName = index == -1 ? FileName : FileName.Substring(0, index);
                        FileName = FileTobeSavedByName;
                    }
                    ObservableCollection<TemplateAttachements> newTemplateAttachementsList = new ObservableCollection<TemplateAttachements>();
                    TemplateAttachements templateAttachements = new TemplateAttachements();
                    templateAttachements.SavedFileName = file.Name;
                    templateAttachements.TemplateAttachementsDocInBytes = FileInBytes;
                    TemplateAttachementsDocInBytes = templateAttachements.TemplateAttachementsDocInBytes;
                    if (TemplateAttachementsDocInBytes != null)
                    {
                        if (fileExtension == ".pdf")
                            PdfDoc = new MemoryStream(TemplateAttachementsDocInBytes);

                        if (fileExtension == ".xls" || fileExtension == ".xlsx")
                            ExcelDoc = new MemoryStream(TemplateAttachementsDocInBytes);


                    }
                    AttachmentObjectList = new List<object>();
                    AttachmentObjectList.Add(templateAttachements);

                    newTemplateAttachementsList.Add(templateAttachements);
                    TemplateAttachementsList = newTemplateAttachementsList;

                    if (TemplateAttachementsList.Count > 0)
                    {
                        SelectedTemplateFile = TemplateAttachementsList[0];
                    }
                }
                GeosApplication.Instance.Logger.Log("Method BrowseFileAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log("Method BrowseFileAction() executed successfully", category: Category.Info, priority: Priority.Low);
        }
        //private void ExtractDataFromExcel(string filePath, string range, string keyword, string delimiter)
        //{
        //    try
        //    {
        //        // Variable to store the extracted data
        //        ExtractedResults = new List<PORequestDetails>();
        //        PORequestDetails test = new PORequestDetails();

        //         // Load the workbook
        //         Workbook workbook = new Workbook();
        //        workbook.LoadDocument(filePath);

        //        // Select the worksheet (first worksheet by default)
        //        Worksheet worksheet = workbook.Worksheets[0];

        //        // Split the disjointed range (e.g., "K7;Z7") into individual ranges
        //        string[] individualRanges = range.Split(';');

        //        foreach (string individualRange in individualRanges)
        //        {
        //            // Define the individual range to extract data
        //            CellRange cellRange = worksheet.Range[individualRange.Trim()];

        //            foreach (Cell cell in cellRange)
        //            {
        //                string cellValue = cell.DisplayText; // Retrieve the displayed text of the cell
        //                if (!string.IsNullOrEmpty(cellValue))
        //                {
        //                    // Find the positions of the keyword and delimiter
        //                    int keywordIndex = cellValue.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
        //                    int delimiterIndex = cellValue.IndexOf(delimiter, StringComparison.OrdinalIgnoreCase);

        //                    // Check if both the keyword and delimiter exist in the cell value
        //                    if (keywordIndex != -1 && delimiterIndex != -1 && keywordIndex < delimiterIndex)
        //                    {
        //                        // Extract the substring from the keyword to the delimiter (inclusive)
        //                        //string extractedPart = cellValue.Substring(keywordIndex, delimiterIndex - keywordIndex + delimiter.Length);
        //                        if (!string.IsNullOrEmpty(UpdatedRangeExcel))
        //                        {
        //                            test.PONumber = cellValue.Substring(keywordIndex, delimiterIndex - keywordIndex + delimiter.Length);
        //                        }

        //                        // Add the extracted part to the results
        //                        ExtractedResults.Add(test);
        //                    }
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void ExtractDataFromExcelold(string filePath, string cell, string keyword, string delimiter)
        {
            try
            {
                // Set the license context correctly (static property of ExcelPackage)
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; // For non-commercial use
                                                                                          // Or if you have a commercial license:
                                                                                          // ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var workbook = package.Workbook;
                    var worksheet = workbook.Worksheets[0]; // Assuming first worksheet; you can change it as needed

                    for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                    {
                        for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                        {
                            string cellValue = worksheet.Cells[row, col].Text; // Get the text value of the cell

                            if (cellValue != "")
                            {
                                var TEST = cellValue;
                            }// Call your ExtractDataFromCell method here
                            ExtractDataFromCell(cellValue, keyword, delimiter); // Example parameters
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Method to Close Window
        /// </summary>
        /// <param name="obj"></param>
        /// 
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }
        public void Init()
        {

        }
        public void EditInit(OtRequestTemplates ObjOtRequestTemplates)
        {
            GeosApplication.Instance.Logger.Log("Method EditInIt ...", category: Category.Info, priority: Priority.Low);
        //  OTMService = new OTMServiceController("localhost:6699");
            OtRequestMappingList = new ObservableCollection<OtRequestTemplates>(OTMService.OTM_GetAllMappingFieldsData_V2610(ObjOtRequestTemplates));
            otrequesttemplates = new OtRequestTemplates();
            initialOtRequestTemplatesDetails = new OtRequestTemplates();
            otrequesttemplates = ObjOtRequestTemplates;
            initialOtRequestTemplatesDetails = (OtRequestTemplates)ObjOtRequestTemplates.Clone();
            IdOtRequestTemplate = ObjOtRequestTemplates.IdOTRequestTemplate;
            RegistrationNumber = otrequesttemplates.Code;
            TemplateName = otrequesttemplates.TemplateName;
            IsSelectedIndexEdited = true;
            SelectedIndexGroup = GroupList.IndexOf(GroupList.FirstOrDefault(i => i.IdCustomer == otrequesttemplates.IdCustomer));
            SelectedIndexRegion = RegionList.IndexOf(RegionList.FirstOrDefault(i => i.IdRegion == otrequesttemplates.IdRegion));
            SelectedIndexCountry = CountryList.IndexOf(CountryList.FirstOrDefault(i => i.IdCountry == otrequesttemplates.IdCountry));
            SelectedIndexPlant = CustomerPlants.IndexOf(CustomerPlants.FirstOrDefault(i => i.IdCustomerPlant == otrequesttemplates.IdCustomerPlant));
            InUse = otrequesttemplates.InUse;
            if (TemplateAttachementsList == null)
            {
                TemplateAttachementsList = new ObservableCollection<TemplateAttachements>();
            }
            SelectedTemplateFile = TemplateAttachementsList.FirstOrDefault(i => i.SavedFileName == otrequesttemplates.File);
            if (SelectedTemplateFile != null)
            {
                SelectedTemplateFile.SavedFileName = otrequesttemplates.File;
            }
            else
            {
                SelectedTemplateFile = new TemplateAttachements { SavedFileName = otrequesttemplates.File };
                TemplateAttachementsList.Add(SelectedTemplateFile);
                SelectedTemplateFile.TemplateAttachementsDocInBytes = otrequesttemplates.FileDocInBytes;
            }
            if (otrequesttemplates.FileDocInBytes != null&& otrequesttemplates.fileExtension == ".pdf")
            {
                PdfDoc = new MemoryStream(otrequesttemplates.FileDocInBytes);
                
            }
            else
            {
                if (otrequesttemplates.FileDocInBytes != null)
                {
                    if (otrequesttemplates.fileExtension == ".xls" || otrequesttemplates.fileExtension == ".xlsx")
                    {
                        ExcelDoc = new MemoryStream(otrequesttemplates.FileDocInBytes);
                    }
                }
            }
            FileExtension = otrequesttemplates.fileExtension;
            if (otrequesttemplates.fileExtension == ".pdf")
            {
                IsPdf = Visibility.Visible;
                IsMappingFieldsActive = true;
                IsMappingFildsVisible = Visibility.Visible;
                IsExcel = Visibility.Hidden;
                foreach (var mapping in OtRequestMappingList)
                {
                    if (mapping.otRequestTemplateFeildOption == null) continue;

                    var idField = mapping.otRequestTemplateFeildOption.IdField;
                    var idFieldType = mapping.otRequestTemplateFeildOption.IdFieldType.ToString();

                    string fieldType = idFieldType == "2212" ? "Text" : idFieldType == "2213" ? "Location" : "";

                    switch (idField)
                    {
                        case 2065: // PO Number
                            SelectedPONumberText = fieldType;
                            break;
                        case 2066: // Customer
                            SelectedCustomerText = fieldType;
                            break;
                        case 2067: // Email (assuming it's Contact)
                            SelectedContactText = fieldType;
                            break;
                        case 2068: // Total (assuming it's Amount)
                            SelectedAmountText = fieldType;
                            break;
                        case 2069: // Incoterm
                            SelectedIncotermsText = fieldType;
                            break;
                        case 2070: // Date
                            SelectedDateText = fieldType;
                            break;
                        case 2072: // Currency
                            SelectedCurrencyText = fieldType;
                            break;
                        case 2077: // Ship To
                            SelectedShipTOText = fieldType;
                            break;
                        case 2205: // Payment Terms
                            SelectedPaymentTermsText = fieldType;
                            break;
                    }
                }
                var poNumber = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2065);
                var customer = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2066);
                var email = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2067);
                var total = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2068);
                var incoterm = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2069);
                var date = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2070);
                var currency = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2072);
                var shipTo = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2077);
                var paymentTerms = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2205);
                // Assign values for each field based on IdField
                if (poNumber != null)
                {
                    if (SelectedPONumberText == "Text")
                    {
                        PONumberKeyword = poNumber.otRequestTemplateTextField?.Keyword;
                        PONumberDelimiter = poNumber.otRequestTemplateTextField?.Delimiter;
                        IsPONumberTextVisibility = Visibility.Visible;
                        IsPOnumberLocationVisibility = Visibility.Hidden;

                        IdOTRequestTemplateTextFieldPoNumber = (int)(poNumber.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionPoNumber = (int)(poNumber.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        PONumberCoordinates = poNumber.otRequestTemplateLocationField?.Coordinates;
                        IsPONumberTextVisibility = Visibility.Hidden;
                        IsPOnumberLocationVisibility = Visibility.Visible;

                        IdOTRequestTemplateLocationFieldPoNumber = (int)poNumber.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionPoNumber = (int)(poNumber.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (date != null)
                {
                    if (SelectedDateText == "Text")
                    {
                        DateKeyword = date.otRequestTemplateTextField?.Keyword;
                        DateDelimiter = date.otRequestTemplateTextField?.Delimiter;
                        IsPODateTextVisibility = Visibility.Visible;
                        IsPODateLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldDate = (int)(date.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionDate = (int)(date.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        DateCoordinates = date.otRequestTemplateLocationField?.Coordinates;
                        IsPODateTextVisibility = Visibility.Hidden;
                        IsPODateLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldDate = (int)date.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionDate = (int)(date.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (customer != null)
                {
                    if (SelectedCustomerText == "Text")
                    {
                        CustomerKeyWord = customer.otRequestTemplateTextField?.Keyword;
                        CustomerDelimiter = customer.otRequestTemplateTextField?.Delimiter;
                        IsPOCustomerTextVisibility = Visibility.Visible;
                        IsPOCustomerLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldCustomer = (int)(customer.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionCustomer = (int)(customer.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        CustomerCoordinates = customer.otRequestTemplateLocationField?.Coordinates;
                        IsPOCustomerTextVisibility = Visibility.Hidden;
                        IsPOCustomerLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldCustomer = (int)customer.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionCustomer = (int)(customer.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (email != null)
                {
                    if (SelectedContactText == "Text")
                    {
                        ContactKeyWord = email.otRequestTemplateTextField?.Keyword;
                        ContactDelimiter = email.otRequestTemplateTextField?.Delimiter;
                        IsPOContactTextVisibility = Visibility.Visible;
                        IsPOContactLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldContact = (int)(email.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionContact = (int)(email.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        ContactCoordinates = email.otRequestTemplateLocationField?.Coordinates;
                        IsPOContactTextVisibility = Visibility.Hidden;
                        IsPOContactLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldContact = (int)email.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionContact = (int)(email.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (total != null)
                {
                    if (SelectedAmountText == "Text")
                    {
                        AmountKeyWord = total.otRequestTemplateTextField?.Keyword;
                        AmountDelimiter = total.otRequestTemplateTextField?.Delimiter;
                        IsPOAmountTextVisibility = Visibility.Visible;
                        IsPOAmountLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldAmount = (int)(total.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionAmount = (int)(total.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        AmountCoordinates = total.otRequestTemplateLocationField?.Coordinates;
                        IsPOAmountTextVisibility = Visibility.Hidden;
                        IsPOAmountLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldAmount = (int)total.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionAmount = (int)(total.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (currency != null)
                {
                    if (SelectedCurrencyText == "Text")
                    {
                        CurrencyKeyword = currency.otRequestTemplateTextField?.Keyword;
                        CurrencyDelimiter = currency.otRequestTemplateTextField?.Delimiter;
                        IsPOCurrencyTextVisibility = Visibility.Visible;
                        IsPOCurrencyLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldCurrency = (int)(currency.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionCurrency = (int)(currency.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        CurrencyCoordinates = currency.otRequestTemplateLocationField?.Coordinates;
                        IsPOCurrencyTextVisibility = Visibility.Hidden;
                        IsPOCurrencyLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldCurrency = (int)currency.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionCurrency = (int)(currency.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (shipTo != null)
                {
                    if (SelectedShipTOText == "Text")
                    {
                        ShipTOKeyword = shipTo.otRequestTemplateTextField?.Keyword;
                        ShipTODelimiter = shipTo.otRequestTemplateTextField?.Delimiter;
                        IsPOShipToTextVisibility = Visibility.Visible;
                        IsPOShipToLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldShipTo = (int)(shipTo.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionShipTo = (int)(shipTo.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        ShipTOCoordinates = shipTo.otRequestTemplateLocationField?.Coordinates;
                        IsPOShipToTextVisibility = Visibility.Hidden;
                        IsPOShipToLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldShipTo = (int)shipTo.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionShipTo = (int)(shipTo.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
                if (incoterm != null)
                {
                    if (SelectedIncotermsText == "Text")
                    {
                        IncotermsKeyWord = incoterm.otRequestTemplateTextField?.Keyword;
                        IncotermsDelimiter = incoterm.otRequestTemplateTextField?.Delimiter;
                        IsPOIncotermsTextVisibility = Visibility.Visible;
                        IsPOIncotermsLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldIncoterms = (int)(incoterm.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionIncoterms = (int)(incoterm.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        IncotermsCoordinates = incoterm.otRequestTemplateLocationField?.Coordinates;
                        IsPOIncotermsTextVisibility = Visibility.Hidden;
                        IsPOIncotermsLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldIncoterms = (int)incoterm.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionIncoterms = (int)(incoterm.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }

                }

                if (paymentTerms != null)
                {
                    if (SelectedPaymentTermsText == "Text")
                    {
                        PaymentTermsKeyWord = paymentTerms.otRequestTemplateTextField?.Keyword;
                        PaymentTermsDelimiter = paymentTerms.otRequestTemplateTextField?.Delimiter;
                        IsPOPaymentTermsTextVisibility = Visibility.Visible;
                        IsPOPaymentTermsLocationVisibility = Visibility.Hidden;
                        IdOTRequestTemplateTextFieldPaymentTerms = (int)(paymentTerms.otRequestTemplateTextField?.IdOTRequestTemplateTextField);
                        IdOTRequestTemplateFieldOptionPaymentTerms = (int)(paymentTerms.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                    else
                    {
                        PaymentTermsCoordinates = paymentTerms.otRequestTemplateLocationField?.Coordinates;
                        IsPOPaymentTermsTextVisibility = Visibility.Hidden;
                        IsPOPaymentTermsLocationVisibility = Visibility.Visible;
                        IdOTRequestTemplateLocationFieldPaymentTerms = (int)paymentTerms.otRequestTemplateLocationField?.IdOTRequestTemplateLocationField;
                        IdOTRequestTemplateFieldOptionPaymentTerms = (int)(paymentTerms.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                    }
                }
            }
            else
            {
                IsPdf = Visibility.Hidden;
                IsMappingFieldsActive = true;
                IsMappingFildsVisible = Visibility.Visible;
                IsExcel = Visibility.Visible;

                var poNumber = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2065);
                var customer = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2066);
                var email = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2067);
                var total = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2068);
                var incoterm = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2069);
                var date = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2070);
                var currency = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2072);
                var shipTo = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2077);
                var paymentTerms = OtRequestMappingList.FirstOrDefault(i => i.otRequestTemplateFeildOption.IdField == 2205);

                // Excel Fields Mapping

                // PONumber
                if (poNumber != null)
                {
                    PONumberKeywordExcel = poNumber.otRequestTemplateCellField?.Keyword;
                    PONumberDelimiterExcel = poNumber.otRequestTemplateCellField?.Delimiter;
                    PONumberRangeExcel = poNumber.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldPoNumber = (int)poNumber.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionPoNumber = (int)(poNumber.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // PODate
                if (date != null)
                {
                    PODateKeywordExcel = date.otRequestTemplateCellField?.Keyword;
                    PODateDelimiterExcel = date.otRequestTemplateCellField?.Delimiter;
                    PODateRangeExcel = date.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldDate = (int)date.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionDate = (int)(date.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // Customer
                if (customer != null)
                {
                    CustomerKeywordExcel = customer.otRequestTemplateCellField?.Keyword;
                    CustomerDelimiterExcel = customer.otRequestTemplateCellField?.Delimiter;
                    CustomerRangeExcel = customer.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldCustomer = (int)customer.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionCustomer = (int)(customer.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // Contact (Email)
                if (email != null)
                {
                    ContactKeywordExcel = email.otRequestTemplateCellField?.Keyword;
                    ContactDelimiterExcel = email.otRequestTemplateCellField?.Delimiter;
                    ContactRangeExcel = email.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldContact = (int)email.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionContact = (int)(email.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // Amount
                if (total != null)
                {
                    AmountKeywordExcel = total.otRequestTemplateCellField?.Keyword;
                    AmountDelimiterExcel = total.otRequestTemplateCellField?.Delimiter;
                    AmountRangeExcel = total.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldAmount = (int)total.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionAmount = (int)(total.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // Currency
                if (currency != null)
                {
                    CurrencyKeywordExcel = currency.otRequestTemplateCellField?.Keyword;
                    CurrencyDelimiterExcel = currency.otRequestTemplateCellField?.Delimiter;
                    CurrencyRangeExcel = currency.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldCurrency = (int)currency.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionCurrency = (int)(currency.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // ShipTo
                if (shipTo != null)
                {
                    ShipToKeywordExcel = shipTo.otRequestTemplateCellField?.Keyword;
                    ShipToDelimiterExcel = shipTo.otRequestTemplateCellField?.Delimiter;
                    ShipToRangeExcel = shipTo.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldShipTo = (int)shipTo.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionShipTo = (int)(shipTo.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // Incoterms
                if (incoterm != null)
                {
                    IncotermsKeywordExcel = incoterm.otRequestTemplateCellField?.Keyword;
                    IncotermsDelimiterExcel = incoterm.otRequestTemplateCellField?.Delimiter;
                    IncotermsRangeExcel = incoterm.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldIncoterms = (int)incoterm.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionIncoterms = (int)(incoterm.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

                // PaymentTerms
                if (paymentTerms != null)
                {
                    PaymentTermsKeywordExcel = paymentTerms.otRequestTemplateCellField?.Keyword;
                    PaymentTermsDelimiterExcel = paymentTerms.otRequestTemplateCellField?.Delimiter;
                    PaymentTermsRangeExcel = paymentTerms.otRequestTemplateCellField?.Cells;
                    IdOTRequestTemplateCellFieldPaymentTerms = (int)paymentTerms.otRequestTemplateCellField?.IdOTRequestTemplateCellField;
                    IdOTRequestTemplateFieldOptionPaymentTerms = (int)(paymentTerms.otRequestTemplateFeildOption?.IdOTRequestTemplateFieldOption);
                }

            }


            initialOtRequestTemplatesDetails = (OtRequestTemplates)ObjOtRequestTemplates.Clone();
        }
        #endregion
        #region Validation
        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }
        string IDataErrorInfo.Error
        {
            get
            {
                if (FileExtension == ".xls" || FileExtension == ".xlsx")
                {
                    //if (!allowValidation) return null;
                    //IDataErrorInfo me = (IDataErrorInfo)this;
                    //string error =
                    //    me[BindableBase.GetPropertyName(() => TemplateName)] + me[BindableBase.GetPropertyName(() => SelectedIndexGroup)] +
                    //    me[BindableBase.GetPropertyName(() => SelectedIndexCountry)] + me[BindableBase.GetPropertyName(() => SelectedTemplateFile)] +
                    //    me[BindableBase.GetPropertyName(() => PONumberRangeExcel)] + me[BindableBase.GetPropertyName(() => PODateRangeExcel)] +
                    //    me[BindableBase.GetPropertyName(() => CustomerRangeExcel)] + me[BindableBase.GetPropertyName(() => ContactRangeExcel)] +
                    //    me[BindableBase.GetPropertyName(() => AmountRangeExcel)] + me[BindableBase.GetPropertyName(() => CurrencyRangeExcel)] +
                    //    me[BindableBase.GetPropertyName(() => ShipToRangeExcel)] + me[BindableBase.GetPropertyName(() => IncotermsRangeExcel)] +
                    //    me[BindableBase.GetPropertyName(() => PaymentTermsRangeExcel)];


                    //if (!string.IsNullOrEmpty(error))
                    //    return "Please check inputted data.";

                   
                }
                else
                {
                    if(FileExtension == ".pdf")
                    {
                        if (!allowValidation) return null;
                        IDataErrorInfo me = (IDataErrorInfo)this;
                        string error =
                            me[BindableBase.GetPropertyName(() => TemplateName)] + me[BindableBase.GetPropertyName(() => SelectedIndexGroup)] +
                            me[BindableBase.GetPropertyName(() => SelectedIndexCountry)] + me[BindableBase.GetPropertyName(() => SelectedTemplateFile)] +
                            me[BindableBase.GetPropertyName(() => PONumberKeyword)] +
                            me[BindableBase.GetPropertyName(() => PONumberDelimiter)] + me[BindableBase.GetPropertyName(() => DateKeyword)] +
                            me[BindableBase.GetPropertyName(() => DateDelimiter)] + me[BindableBase.GetPropertyName(() => CustomerKeyWord)] +
                            me[BindableBase.GetPropertyName(() => CustomerDelimiter)] + me[BindableBase.GetPropertyName(() => ContactKeyWord)] +
                            me[BindableBase.GetPropertyName(() => ContactDelimiter)] + me[BindableBase.GetPropertyName(() => AmountKeyWord)] +
                            me[BindableBase.GetPropertyName(() => AmountDelimiter)] + me[BindableBase.GetPropertyName(() => CurrencyKeyword)] +
                            me[BindableBase.GetPropertyName(() => ShipTODelimiter)] + me[BindableBase.GetPropertyName(() => IncotermsKeyWord)] +
                            me[BindableBase.GetPropertyName(() => IncotermsDelimiter)] + me[BindableBase.GetPropertyName(() => PaymentTermsKeyWord)] +
                            me[BindableBase.GetPropertyName(() => PaymentTermsDelimiter)] + me[BindableBase.GetPropertyName(() => PONumberCoordinates)] +
                            me[BindableBase.GetPropertyName(() => DateCoordinates)] + me[BindableBase.GetPropertyName(() => CustomerCoordinates)] +
                            me[BindableBase.GetPropertyName(() => ContactCoordinates)] + me[BindableBase.GetPropertyName(() => AmountCoordinates)] +
                            me[BindableBase.GetPropertyName(() => CurrencyCoordinates)] + me[BindableBase.GetPropertyName(() => ShipTOCoordinates)] +
                            me[BindableBase.GetPropertyName(() => IncotermsCoordinates)] + me[BindableBase.GetPropertyName(() => PaymentTermsCoordinates)]
                            ;


                        if (!string.IsNullOrEmpty(error))
                            return "Please check inputted data.";

                    }
                    if(FileExtension==null)
                    {
                        if (!allowValidation) return null;
                        IDataErrorInfo me = (IDataErrorInfo)this;
                        string error =
                            me[BindableBase.GetPropertyName(() => TemplateName)] + me[BindableBase.GetPropertyName(() => SelectedIndexGroup)] +
                            me[BindableBase.GetPropertyName(() => SelectedIndexCountry)] + me[BindableBase.GetPropertyName(() => SelectedTemplateFile)];

                        if (!string.IsNullOrEmpty(error))
                            return "Please check inputted data.";
                    }
                }
                return null;
            }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string Name = BindableBase.GetPropertyName(() => TemplateName);
                string Group = BindableBase.GetPropertyName(() => SelectedIndexGroup);
                string Region = BindableBase.GetPropertyName(() => SelectedIndexRegion);
                string Country = BindableBase.GetPropertyName(() => SelectedIndexCountry);
                string Plant = BindableBase.GetPropertyName(() => SelectedIndexPlant);
                string File = BindableBase.GetPropertyName(() => SelectedTemplateFile);
                //Mapping Fields For Excel
                string PoNumber = BindableBase.GetPropertyName(() => PONumberRangeExcel);
                string PoDate = BindableBase.GetPropertyName(() => PODateRangeExcel);
                string Customer = BindableBase.GetPropertyName(() => CustomerRangeExcel);
                string Contact = BindableBase.GetPropertyName(() => ContactRangeExcel);
                string Amount = BindableBase.GetPropertyName(() => AmountRangeExcel);
                string Currency = BindableBase.GetPropertyName(() => CurrencyRangeExcel);
                string ShipTo = BindableBase.GetPropertyName(() => ShipToRangeExcel);
                string Incoterms = BindableBase.GetPropertyName(() => IncotermsRangeExcel);
                string PaymentTerms= BindableBase.GetPropertyName(() => PaymentTermsRangeExcel);
                //Mapping Fields For Pdf-PoNumber
                string PoNumberKey = BindableBase.GetPropertyName(() => PONumberKeyword);
                string PONumberDel= BindableBase.GetPropertyName(()=> PONumberDelimiter);
                //Mapping Fields For Pdf-PoDate
                string DateKey = BindableBase.GetPropertyName(() => DateKeyword);
                string DateDeli = BindableBase.GetPropertyName(() => DateDelimiter);
                //Mapping Fields For Pdf-Customer
                string PoCustomerKey = BindableBase.GetPropertyName(() => CustomerKeyWord);
                string PoCustomerDeli = BindableBase.GetPropertyName(() => CustomerDelimiter);
                //Mapping Fields For Pdf-Contact
                string PoContactKey = BindableBase.GetPropertyName(() => ContactKeyWord);
                string PoContactDeli = BindableBase.GetPropertyName(() => ContactDelimiter);
                //Mapping Fields For Pdf-Amount
                string PoAmountKey = BindableBase.GetPropertyName(() => AmountKeyWord);
                string PoAmountDeli = BindableBase.GetPropertyName(() => AmountDelimiter);
                //Mapping Fields For Pdf-Currency
                string PoCurrencyKey = BindableBase.GetPropertyName(() => CurrencyKeyword);
                string PoCurrencyDeli = BindableBase.GetPropertyName(() => CurrencyDelimiter);
                //Mapping Fields For Pdf-ShipTo
                string PoShipToKey = BindableBase.GetPropertyName(() => ShipTOKeyword);
                string PoShipToDeli = BindableBase.GetPropertyName(() => ShipTODelimiter);
                //Mapping Fields For Pdf-Incoterms
                string PoIncotermsKey = BindableBase.GetPropertyName(() => IncotermsKeyWord);
                string PoIncotermsDeli = BindableBase.GetPropertyName(() => IncotermsDelimiter);
                //Mapping Fields For Pdf-PaymentTerms
                string PoPaymentTermsKey = BindableBase.GetPropertyName(() => PaymentTermsKeyWord);
                string PoPaymentTermsDeli = BindableBase.GetPropertyName(() => PaymentTermsDelimiter);
                //Mapping Fields For Pdf Co-ordinates- PoNumber
                string PoNumberCor = BindableBase.GetPropertyName(() => PONumberCoordinates);
                string PoDateco = BindableBase.GetPropertyName(() => DateCoordinates);
                string PoCustomerCo = BindableBase.GetPropertyName(() => CustomerCoordinates);
                string PoContactCo = BindableBase.GetPropertyName(() => ContactCoordinates);
                string PoAmountCo = BindableBase.GetPropertyName(() => AmountCoordinates);
                string PoCurrencyCo = BindableBase.GetPropertyName(() => CurrencyCoordinates);
                string PoShipToCo = BindableBase.GetPropertyName(() => ShipTOCoordinates);
                string PoIncotermsCo = BindableBase.GetPropertyName(() => IncotermsCoordinates);
                string PoPaymentTermsCo = BindableBase.GetPropertyName(() => PaymentTermsCoordinates);
                if (columnName == Name)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Name, TemplateName);
                }
                if (columnName == Group)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Group, SelectedIndexGroup);
                }
                if (columnName == Region)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Region, SelectedIndexRegion);
                }
                if (columnName == Country)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Country, SelectedIndexCountry);
                }

                if (columnName == Plant)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Plant, SelectedIndexPlant);
                }

                if (columnName == File)
                {
                    return NewOtTemplateValidation.GetErrorMessage(File, SelectedTemplateFile);
                }
                //[Rahul.Gadhave][GEOS2-6734][Date:16-01-2024]
                // Mapping Fields for Excel
                if (columnName == PoNumber)
                {
                    return NewOtTemplateValidation.GetErrorMessage(PoNumber, PONumberRangeExcel);
                }
                if (columnName == PoDate)
                {
                    return NewOtTemplateValidation.GetErrorMessage(PoDate, PODateRangeExcel);
                }
                if (columnName == Customer)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Customer, CustomerRangeExcel);
                }
                if (columnName == Contact)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Contact, ContactRangeExcel);
                }
                if (columnName == Amount)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Amount, AmountRangeExcel);
                }
                if (columnName == Currency)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Currency, CurrencyRangeExcel);
                }
                if (columnName == ShipTo)
                {
                    return NewOtTemplateValidation.GetErrorMessage(ShipTo, ShipToRangeExcel);
                }
                if (columnName == Incoterms)
                {
                    return NewOtTemplateValidation.GetErrorMessage(Incoterms, IncotermsRangeExcel);
                }
                if (columnName == PaymentTerms)
                {
                    return NewOtTemplateValidation.GetErrorMessage(PaymentTerms, PaymentTermsRangeExcel);
                }
                //[Rahul.Gadhave][GEOS2-6734][Date:16-01-2024]
                // Mapping Fields for Pdf- PONumber
                if (SelectedPONumberText == "Text")
                {
                    if (columnName == PoNumberKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoNumberKey, PONumberKeyword);
                    }
                    if (columnName == PONumberDel)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PONumberDel, PONumberDelimiter);
                    }
                }
                else
                {
                    if (columnName == PoNumberCor)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoNumberCor, PONumberCoordinates);
                    }
                }
                // Mapping Fields for Pdf- PoDate
                if (SelectedDateText == "Text")
                {
                    if (columnName == DateKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(DateKey, DateKeyword);
                    }
                    if (columnName == DateDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(DateDeli, DateDelimiter);
                    }
                 
                }
                else
                {
                    if (columnName == PoDateco)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoDateco, DateCoordinates);
                    }
                }
                // Mapping Fields for Pdf- PoCustomerKey
                if (SelectedCustomerText == "Text")
                {
                    if (columnName == PoCustomerKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoCustomerKey, CustomerKeyWord);
                    }
                    if (columnName == PoCustomerDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoCustomerDeli, CustomerDelimiter);
                    }

                }
                else
                {
                    if (columnName == PoCustomerCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoCustomerCo, CustomerCoordinates);
                    }
                }
                // Mapping Fields for Pdf- PoContact
                if (SelectedContactText == "Text")
                {
                    if (columnName == PoContactKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoContactKey, ContactKeyWord);
                    }
                    if (columnName == PoContactDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoContactDeli, ContactDelimiter);
                    }

                }
                else
                {
                    if (columnName == PoContactCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoContactCo, ContactCoordinates);
                    }
                }
                // Mapping Fields for Pdf- PoAmount
                if (SelectedAmountText == "Text")
                {
                    if (columnName == PoAmountKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoAmountKey, AmountKeyWord);
                    }
                    if (columnName == PoAmountDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoAmountDeli, AmountDelimiter);
                    }

                }
                else
                {
                    if (columnName == PoAmountCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoAmountCo, AmountCoordinates);
                    }
                }
                // Mapping Fields for Pdf- PoCurrency
                if (SelectedCurrencyText == "Text")
                {
                    if (columnName == PoCurrencyKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoCurrencyKey, CurrencyKeyword);
                    }
                    if (columnName == PoCurrencyDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoAmountDeli, CurrencyDelimiter);
                    }

                }
                else
                {
                    if (columnName == PoCurrencyCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoCurrencyCo, CurrencyCoordinates);
                    }
                }

                // Mapping Fields for Pdf- PoShipTo
                if (SelectedShipTOText == "Text")
                {
                    if (columnName == PoShipToKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoShipToKey, ShipTOKeyword);
                    }
                    if (columnName == PoShipToDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoShipToDeli, ShipTODelimiter);
                    }

                }
                else
                {
                    if (columnName == PoShipToCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoShipToCo, ShipTOCoordinates);
                    }
                }

                // Mapping Fields for Pdf- PoIncoterms
                if (SelectedIncotermsText == "Text")
                {
                    if (columnName == PoIncotermsKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoIncotermsKey, IncotermsKeyWord);
                    }
                    if (columnName == PoIncotermsDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoIncotermsDeli, IncotermsDelimiter);
                    }

                }
                else
                {
                    if (columnName == PoIncotermsCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoIncotermsCo, IncotermsCoordinates);
                    }
                }
                // Mapping Fields for Pdf- PoPaymentTerms
                if (SelectedPaymentTermsText == "Text")
                {
                    if (columnName == PoPaymentTermsKey)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoPaymentTermsKey, PaymentTermsKeyWord);
                    }
                    if (columnName == PoPaymentTermsDeli)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoPaymentTermsDeli, PaymentTermsDelimiter);
                    }

                }
                else
                {
                    if (columnName == PoPaymentTermsCo)
                    {
                        return NewOtTemplateValidation.GetErrorMessage(PoPaymentTermsCo, PaymentTermsCoordinates);
                    }
                }
                return null;
            }
        }

        #endregion


    }
}
