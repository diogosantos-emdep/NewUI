using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class OTShippingDetail
    {
        #region Fields
        List<OtItemShippingDetail> deliveringItems;
        List<OtItemShippingDetail> notDeliveringItems;
        string offerAssigneeName;
        string offerAssigneeEmail;
        string customerCountryCode;
        Int64 idOT;
        byte idTemplate;
        Int32 idSite;
        string siteShortName;
        string template;
        string emailTo;
        string emailCC;
        string mailSubject;
        string currentWeek;
        string htmlFileName;
        string otCode;
        string poCode;
        bool isDeliveringItems;
        bool isNotDeliveringItems;
        string carriageMethod;
        string offerCode;
        #endregion

        #region Properties

        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set { idOT = value; }
        }

        [DataMember]
        public string OfferCode
        {
            get { return offerCode; }
            set { offerCode = value; }
        }

        [DataMember]
        public string OfferAssigneeName
        {
            get { return offerAssigneeName; }
            set { offerAssigneeName = value; }
        }

        [DataMember]
        public string OfferAssigneeEmail
        {
            get { return offerAssigneeEmail; }
            set { offerAssigneeEmail = value; }
        }

        [DataMember]
        public string CustomerCountryCode
        {
            get { return customerCountryCode; }
            set { customerCountryCode = value; }
        }

        [DataMember]
        public List<OtItemShippingDetail> DeliveringItems
        {
            get { return deliveringItems; }
            set { deliveringItems = value; }
        }

        [DataMember]
        public List<OtItemShippingDetail> NotDeliveringItems
        {
            get { return notDeliveringItems; }
            set { notDeliveringItems = value; }
        }


        [DataMember]
        public byte IdTemplate
        {
            get { return idTemplate; }
            set { idTemplate = value; }
        }

        [DataMember]
        public Int32 IdSite
        {
            get { return idSite; }
            set { idSite = value; }
        }

        [DataMember]
        public string SiteShortName
        {
            get { return siteShortName; }
            set { siteShortName = value; }
        }

        [DataMember]
        public string Template
        {
            get { return template; }
            set { template = value; }
        }

        [DataMember]
        public string EmailTo
        {
            get { return emailTo; }
            set { emailTo = value; }
        }

        [DataMember]
        public string EmailCC
        {
            get { return emailCC; }
            set { emailCC = value; }
        }

        [DataMember]
        public string MailSubject
        {
            get { return mailSubject; }
            set { mailSubject = value; }
        }

        [DataMember]
        public string CurrentWeek
        {
            get { return currentWeek; }
            set { currentWeek = value; }
        }

        [DataMember]
        public string HTMLFileName
        {
            get { return htmlFileName; }
            set { htmlFileName = value; }
        }


        [DataMember]
        public string OTCode
        {
            get { return otCode; }
            set { otCode = value; }
        }

        [DataMember]
        public bool IsDeliveringItems
        {
            get { return isDeliveringItems; }
            set { isDeliveringItems = value; }
        }

        [DataMember]
        public bool IsNotDeliveringItems
        {
            get { return isNotDeliveringItems; }
            set { isNotDeliveringItems = value; }
        }


        [DataMember]
        public string POCode
        {
            get { return poCode; }
            set { poCode = value; }
        }

        [DataMember]
        public string CarriageMethod
        {
            get { return carriageMethod; }
            set { carriageMethod = value; }
        }
        #endregion





    }
}

