using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
namespace Emdep.Geos.Data.Common.OTM
{
    /// <summary>
    /// [001][ashish.malkhede][03102024] PO Registration (PO requests list)(2/2) https://helpdesk.emdep.com/browse/GEOS2-6520
    /// </summary>
    public class PORequestDetails : ModelBase, IDisposable
    {
        #region Fields
        long idEmail;
        Int64 idPORequest;
        Int64 idPORequestStatus;
        private DateTime dateTime;
        private DateTime date;
        private string time;
        private DateTime? dateIssued;
        private string dateIssuedString;
        private string sender;
        private string recipient;
        private string subject;
        private string requester;
        private string offer;
        private Int16 attachmentCount;
        private string attachments;
        private string poFound;
        private string customer;
        private long number;
        private string contact;
        private double amount;
        private string currency;
        private string path;
        private string poShipTo;
        private string poIncoterms;
        private string poPaymentTerms;
        private LookupValue poRequestStatus;
        UInt32 idStatus;
        ImageSource attachmentImage;
        private string pONumber;
        private DateTime pOdate;
        private double? transferAmount;
        private string transferAmountString;
        private string email;
        private DateTime integrationDate;
        private Int64 idIntegrationUser;
        private byte[] countryIconbytes;
        private List<Emailattachment> emailAttachmentList;
        private List<ToRecipientName> toRecipientNameList;//[pramod.misal][04.02.2025][GEOS2-6726]
        private List<ToRecipientName> tempToRecipientNameList;//[pramod.misal][04.02.2025][GEOS2-6726]
        private List<ToCCName> cCNameList;//[pramod.misal][04.02.2025][GEOS2-6726]
        //[rahul.gadhave][GEOS2-6720][24-12-2024]
        private string ccrecipient;
        private string inbox;
        //[rahul.gadhave][GEOS2-6718][29 -01-2025]
        private string invoioceTO;
        private string offers;
        public string senderName;
        private string senderFullName;
        private string toRecipientName;
        private string cCName;
        public bool isTooltipVisible;// Control tooltip visibility
        private string senderEmployeeCode;
        private string senderJobDescriptionTitle;
        private string toRecipientNameEmployeeCodes;
        private string toRecipientNameJobDescriptionTitles;
        private string cCNameEmployeeCodes;
        private string cCNameJobDescriptionTitles;
        private string senderEmployeeCodeWithInitialLetters;
        private string group;//[pramod.misal][17.02.2025][GEOS2-6719]
        private string plant;//[pramod.misal][17.02.2025][GEOS2-6719]
        private string attachmentCnt;//[pooja.jadhav][19.02.2024][GEOS2-6724]
        private string emailbody;//[pramod.misal][17.02.2025][GEOS2-6719]
        private string senderIdPerson;
        private string toIdPerson;
        private string ccIdPerson;

        private POLinkedOffers pOLinkedOffers;
        private bool isPOExist;
        private bool isNewPO= false;

        private LinkedOffers offerInfo;
        #region [nsatpute][19-02-2025][GEOS2-6722]
        private List<POData> poData = new List<POData>();
		private List<EmailAttachmentDetails> emailAttachmentDetailsList;
        Int64 idAttachementType;
        string idAttachment;


        #endregion
        #endregion
        #region Constructor
        public PORequestDetails()
        {
        }
        #endregion
        #region Properties
        private ObservableCollection<Emailattachment> pOAttachementsList;
        [DataMember]
        public ObservableCollection<Emailattachment> POAttachementsList
        {
            get { return pOAttachementsList; }
            set
            {
                pOAttachementsList = value;
                OnPropertyChanged("POAttachementsList");
            }
        }

        [DataMember]
        public POLinkedOffers POLinkedOffers
        {
            get { return pOLinkedOffers; }
            set
            {
                pOLinkedOffers = value;
                OnPropertyChanged("POLinkedOffers");
            }
        }

        [DataMember]
        public List<EmailAttachmentDetails> EmailAttachmentDetailsList
        {
            get { return emailAttachmentDetailsList; }
            set
            {
                emailAttachmentDetailsList = value;
                OnPropertyChanged("EmailAttachmentDetailsList");
            }
        }
        [DataMember]
        public string Emailbody
        {
            get
            {
                return emailbody;
            }
            set
            {
                emailbody = value;
                OnPropertyChanged("Emailbody");
            }
        }
        [DataMember]
        public string CCNameJobDescriptionTitles
        {
            get
            {
                return cCNameJobDescriptionTitles;
            }
            set
            {
                cCNameJobDescriptionTitles = value;
                OnPropertyChanged("CCNameJobDescriptionTitles");
            }
        }
        [DataMember]
        public string CCNameEmployeeCodes
        {
            get
            {
                return cCNameEmployeeCodes;
            }
            set
            {
                cCNameEmployeeCodes = value;
                OnPropertyChanged("CCNameEmployeeCodes");
            }
        }
        [DataMember]
        public string ToRecipientNameJobDescriptionTitles
        {
            get
            {
                return toRecipientNameJobDescriptionTitles;
            }
            set
            {
                toRecipientNameJobDescriptionTitles = value;
                OnPropertyChanged("ToRecipientNameJobDescriptionTitles");
            }
        }
        [DataMember]
        public string ToRecipientNameEmployeeCodes
        {
            get
            {
                return toRecipientNameEmployeeCodes;
            }
            set
            {
                toRecipientNameEmployeeCodes = value;
                OnPropertyChanged("ToRecipientNameEmployeeCodes");
            }
        }
        [DataMember]
        public string SenderJobDescriptionTitle
        {
            get
            {
                return senderJobDescriptionTitle;
            }
            set
            {
                senderJobDescriptionTitle = value;
                OnPropertyChanged("SenderJobDescriptionTitle");
            }
        }
        [DataMember]
        public string SenderEmployeeCode
        {
            get
            {
                return senderEmployeeCode;
            }
            set
            {
                senderEmployeeCode = value;
                OnPropertyChanged("SenderEmployeeCode");
            }
        }
		//[nsatpute][11-02-2025][GEOS2-6726]
        [DataMember]
        public string SenderEmployeeCodeWithInitialLetters
        {
            get
            {
                return $"{senderEmployeeCode}_{GetInitials(SenderName)}";
            }
            //set
            //{
            //    senderEmployeeCode = value;
            //    OnPropertyChanged("SenderEmployeeCode");
            //}
        }
        public string ToRecipientNameEmployeeCodesWithInitialLetters
        {
            get
            {
                return $"{toRecipientNameEmployeeCodes}_{GetInitials(toRecipientName)}";
            }
            //set
            //{
            //    toRecipientNameEmployeeCodes = value;
            //    OnPropertyChanged("ToRecipientNameEmployeeCodes");
            //}
        }
        public string CCNameEmployeeCodesWithInitialLetters
        {
            get
            {
                return $"{cCNameEmployeeCodes}_{GetInitials(cCName)}";
            }
            //set
            //{
            //    toRecipientNameEmployeeCodes = value;
            //    OnPropertyChanged("CCNameEmployeeCodes");
            //}
        }
        [DataMember]
        public bool IsTooltipVisible
        {
            get
            {
                return isTooltipVisible;
            }
            set
            {
                isTooltipVisible = value;
                OnPropertyChanged("IsTooltipVisible");
            }
        }
        [DataMember]
        public string CCName
        {
            get
            {
                return cCName;
            }
            set
            {
                cCName = value;
                OnPropertyChanged("CCName");
            }
        }
        [DataMember]
        public string ToRecipientName
        {
            get
            {
                return toRecipientName;
            }
            set
            {
                toRecipientName = value;
                OnPropertyChanged("ToRecipientName");
            }
        }
        [DataMember]
        public string SenderName
        {
            get
            {
                return senderName;
            }
            set
            {
                senderName = value;
                OnPropertyChanged("SenderName");
            }
        }
        [DataMember]
        public string SenderFullName
        {
            get
            {
                return senderFullName;
            }
            set
            {
                senderFullName = value;
                OnPropertyChanged("SenderFullName");
            }
        }
        [DataMember]
        public Int64 IdIntegrationUser
        {
            get { return idIntegrationUser; }
            set
            {
                idIntegrationUser = value;
                OnPropertyChanged("IdIntegrationUser");
            }
        }
        [DataMember]
        public DateTime IntegrationDate
        {
            get { return integrationDate; }
            set { integrationDate = value; OnPropertyChanged("IntegrationDate"); }
        }
        [DataMember]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        [DataMember]
        public double? TransferAmount
        {
            get { return transferAmount; }
            set
            {
                transferAmount = value;
                OnPropertyChanged("TransferAmount");
            }
        }

        [DataMember]
        public string TransferAmountString
        {
            get { return transferAmountString; }
            set
            {
                transferAmountString = value;
                OnPropertyChanged("TransferAmountString");
            }
        }
        [DataMember]
        public DateTime POdate
        {
            get { return pOdate; }
            set { pOdate = value; OnPropertyChanged("POdate"); }
        }
        [DataMember]
        public string PONumber
        {
            get
            {
                return pONumber;
            }
            set
            {
                pONumber = value;
                OnPropertyChanged("PONumber");
            }
        }
        [DataMember]
        public ImageSource AttachmentImage
        {
            get
            {
                return attachmentImage;
            }
            set
            {
                attachmentImage = value;
                OnPropertyChanged("AttachmentImage");
            }
        }
        [DataMember]
        public UInt32 IdStatus
        {
            get { return idStatus; }
            set
            {
                idStatus = value;
                OnPropertyChanged("IdStatus");
            }
        }

        [DataMember]
        public Int64 IdPORequestStatus
        {
            get
            {
                return idPORequestStatus;
            }
            set
            {
                idPORequestStatus = value;
                OnPropertyChanged("IdPORequestStatus");
            }
        }

        [DataMember]
        public Int64 IdPORequest
        {
            get
            {
                return idPORequest;
            }
            set
            {
                idPORequest = value;
                OnPropertyChanged("IdPORequest");
            }
        }
        [DataMember]
        public Int64 IdEmail
        {
            get
            {
                return idEmail;
            }
            set
            {
                idEmail = value;
                OnPropertyChanged("IdEmail");
            }
        }
        [DataMember]
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; OnPropertyChanged("DateTime"); }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; OnPropertyChanged("Date"); }
        }


        [DataMember]
        public string Time
        {
            get { return time; }
            set 
            { 
                time = value; OnPropertyChanged("Time");
                
            }
        }

        [DataMember]
        public DateTime? DateIssued
        {
            get { return dateIssued; }
            set { dateIssued = value; OnPropertyChanged("DateIssued"); }
        }
        [DataMember]
        public string DateIssuedString
        {
            get { return dateIssuedString; }
            set { dateIssuedString = value; OnPropertyChanged("DateIssuedString"); }
        }
        [DataMember]
        public string Sender
        {
            get { return sender; }
            set { sender = value; OnPropertyChanged("Sender"); }
        }
        [DataMember]
        public string Recipient
        {
            get { return recipient; }
            set { recipient = value; OnPropertyChanged("Recipient"); }
        }
        [DataMember]
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                OnPropertyChanged("Subject");
            }
        }
        [DataMember]
        public string Requester
        {
            get
            {
                return requester;
            }
            set
            {
                requester = value;
                OnPropertyChanged("Requester");
            }
        }
        [DataMember]
        public string Offer
        {
            get
            {
                return offer;
            }
            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }
        [DataMember]
        public Int16 AttachmentCount
        {
            get
            {
                return attachmentCount;
            }
            set
            {
                attachmentCount = value;
                OnPropertyChanged("AttachmentCount");
            }
        }
        [DataMember]
        public string AttachmentCnt
        {
            get
            {
                return attachmentCnt;
            }
            set
            {
                attachmentCnt = value;
                OnPropertyChanged("AttachmentCnt");
            }
        }
        [DataMember]
        public string Attachments
        {
            get
            {
                return attachments;
            }
            set
            {
                attachments = value;
                OnPropertyChanged("Attachments");
            }
        }
        [DataMember]
        public string POFound
        {
            get
            {
                return poFound;
            }
            set
            {
                poFound = value;
                OnPropertyChanged("PoFound");
            }
        }
        [DataMember]
        public string Customer
        {
            get
            {
                return customer;
            }
            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }
        [DataMember]
        public long Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }
        [DataMember]
        public string Contact
        {
            get
            {
                return contact;
            }
            set
            {
                contact = value;
                OnPropertyChanged("Contact");
            }
        }
        [DataMember]
        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                OnPropertyChanged("amount");
            }
        }
        [DataMember]
        public string Currency
        {
            get
            {
                return currency;
            }
            set
            {
                currency = value;
                OnPropertyChanged("Currency");
            }
        }
        public string Path
        {
            get { return path; }
            set { path = value; OnPropertyChanged("Path"); }
        }
        [DataMember]
        public LookupValue PORequestStatus
        {
            get
            {
                return poRequestStatus;
            }
            set
            {
                poRequestStatus = value;
                OnPropertyChanged("PORequestStatus");
            }
        }
        [DataMember]
        public string ShipTo
        {
            get { return poShipTo; }
            set { poShipTo = value; OnPropertyChanged("ShipTo"); }
        }
        [DataMember]
        public string POIncoterms
        {
            get { return poIncoterms; }
            set { poIncoterms = value; OnPropertyChanged("POIncoterms"); }
        }
        [DataMember]
        public string POPaymentTerm
        {
            get { return poPaymentTerms; }
            set { poPaymentTerms = value; OnPropertyChanged("POIncoterms"); }
        }
        public byte[] CountryIconBytes
        {
            get { return countryIconbytes; }
            set
            {
                countryIconbytes = value;
                OnPropertyChanged("CountryIconBytes");
            }
        }
        [DataMember]
        public List<Emailattachment> EmailAttachmentList
        {
            get { return emailAttachmentList; }
            set
            {
                emailAttachmentList = value;
                OnPropertyChanged("EmailAttachmentList");
            }
        }
        [DataMember]
        public List<ToRecipientName> ToRecipientNameList
        {
            get { return toRecipientNameList; }
            set
            {
                toRecipientNameList = value;
                OnPropertyChanged("ToRecipientNameList");
            }
        }
        [DataMember]
        public List<ToCCName> CCNameList
        {
            get { return cCNameList; }
            set
            {
                cCNameList = value;
                OnPropertyChanged("CCNameList");
            }
        }
        //[rahul.gadhave][GEOS2-6720][29-01-2025]
        [DataMember]
        public string CCRecipient
        {
            get { return ccrecipient; }
            set { ccrecipient = value; OnPropertyChanged("CCRecipient"); }
        }
        [DataMember]
        public string Inbox
        {
            get { return inbox; }
            set { inbox = value; OnPropertyChanged("Inbox"); }
        }
        //[rahul.gadhave][GEOS2-6718][29 -01-2025]
        [DataMember]
        public string InvoioceTO
        {
            get
            {
                return invoioceTO;
            }
            set
            {
                invoioceTO = value;
                OnPropertyChanged("InvoioceTO");
            }
        }
        [DataMember]
        public string Offers
        {
            get
            {
                return offers;
            }
            set
            {
                offers = value;
                OnPropertyChanged("Offers");
            }
        }
        [DataMember]
        public string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }
        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }
        [DataMember]
        public string SenderIdPerson
        {
            get
            {
                return senderIdPerson;
            }
            set
            {
                senderIdPerson = value;
                OnPropertyChanged("SenderIdPerson");
            }
        }

        [DataMember]
        public string ToIdPerson
        {
            get
            {
                return toIdPerson;
            }
            set
            {
                toIdPerson = value;
                OnPropertyChanged("ToIdPerson");
            }
        }

        public string CCIdPerson
        {
            get
            {
                return ccIdPerson;
            }
            set
            {
                ccIdPerson = value;
                OnPropertyChanged("CCIdPerson");
            }
        }

        [DataMember]
        public Int64 IdAttachementType
        {
            get
            {
                return idAttachementType;
            }
            set
            {
                idAttachementType = value;
                OnPropertyChanged("IdAttachementType");
            }
        }

       

        [DataMember]
        public string IdAttachment
        {
            get
            {
                return idAttachment;
            }
            set
            {
                idAttachment = value;
                OnPropertyChanged("IdAttachment");
            }
        }

        [DataMember]
        public bool IsPOExist
        {
            get
            {
                return isPOExist;
            }
            set
            {
                isPOExist = value;
                OnPropertyChanged("IsPOExist");
            }
        }

        [DataMember]
        public bool IsNewPO
        {
            get
            {
                return isNewPO;
            }
            set
            {
                isNewPO = value;
                OnPropertyChanged("IsNewPO");
            }
        }

        [DataMember]
        public LinkedOffers OfferInfo
        {
            get { return offerInfo; }
            set
            {
                offerInfo = value;
                OnPropertyChanged("OfferInfo");
            }
        }
        long idCountry;
        [DataMember]
        public Int64 IdCountry
        {
            get
            {
                return idCountry;
            }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }
        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        long idCustomerPlant;
        [DataMember]
        public Int64 IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {
                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
            }
        }
        //[rahul.gadhave][GEOS2-9020][23.07.2025] 
        long idCustomerGroup;
        [DataMember]
        public Int64 IdCustomerGroup
        {
            get
            {
                return idCustomerGroup;
            }
            set
            {
                idCustomerGroup = value;
                OnPropertyChanged("IdCustomerGroup");
            }
        }
        #region [nsatpute][19-02-2025][GEOS2-6722]
        [DataMember]
        public List<POData> PoData
        {
            get => poData;
            set
            {
                poData = value;
                OnPropertyChanged(nameof(PoData));
            }
        }
        #endregion
        #endregion
        #region Methods
        
        //[nsatpute][11-02-2025][GEOS2-6726]
        //static string GetInitials(string fullName)
        //{
        //    if (string.IsNullOrEmpty(fullName))
        //        return string.Empty;
        //    // Split the full name into words
        //    var words = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (words.Length == 1)
        //    {
        //        // If there is only one name, return the first initial followed by 'A'
        //        return new string(fullName.Take(2).ToArray()).ToUpper();
        //    }

        //    string firstInitial = words.First()[0].ToString().ToUpper(CultureInfo.InvariantCulture);
        //    string lastInitial = words.Last()[0].ToString().ToUpper(CultureInfo.InvariantCulture);

        //    return firstInitial + lastInitial;
        //}

        //[pramod.misal] [28.04.2025][[GEOS2 - 6726]]
        static string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return string.Empty;

            var words = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1)
            {
                // Only one word: return first letter
                return words[0][0].ToString().ToUpper(CultureInfo.InvariantCulture);
            }

            // Multiple words: return first letter of first word + first letter of last word
            string firstInitial = words.First()[0].ToString().ToUpper(CultureInfo.InvariantCulture);
            string lastInitial = words.Last()[0].ToString().ToUpper(CultureInfo.InvariantCulture);

            return firstInitial + lastInitial;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}