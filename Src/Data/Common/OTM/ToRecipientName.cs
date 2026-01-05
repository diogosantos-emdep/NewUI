using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OTM
{
    //[pramod.misal][04.02.2025][GEOS2-6726]
    [DataContract]
    public class ToRecipientName : ModelBase, IDisposable
    {

        #region Fields
        private string toIdperson;

        private string recipientName;
        private string recipientmail;

        private string toRecipientNameEmployeeCodes;
        private string toRecipientNameJobDescriptionTitles;
        private bool ismatched;
        private Visibility isEmdepContact = Visibility.Hidden;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
        private Visibility isNotEmdepContact = Visibility.Hidden;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324


        #endregion


        #region Properties

        [DataMember]
        public string ToIdperson
        {
            get
            {
                return toIdperson;
            }

            set
            {
                toIdperson = value;
                OnPropertyChanged("ToIdperson");
            }
        }

        [DataMember]
        public Visibility IsEmdepContact
        {
            get
            {
                return isEmdepContact;
            }

            set
            {
                isEmdepContact = value;
                OnPropertyChanged("IsEmdepContact");
            }
        }

        [DataMember]
        public Visibility IsNotEmdepContact
        {
            get
            {
                return isNotEmdepContact;
            }

            set
            {
                isNotEmdepContact = value;
                OnPropertyChanged("IsNotEmdepContact");
            }
        }

        [DataMember]
        public bool Ismatched
        {
            get { return ismatched; }
            set
            {
                ismatched = value;
                OnPropertyChanged("Ismatched");
            }
        }

        [DataMember]
        public string RecipientName
        {
            get { return recipientName; }
            set
            {
                recipientName = value;
                OnPropertyChanged("RecipientName");
            }
        }

        [DataMember]
        public string Recipientmail
        {
            get { return recipientmail; }
            set
            {
                recipientmail = value;
                OnPropertyChanged("Recipientmail");
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

        public string ToRecipientNameEmployeeCodesWithInitialLetters
        {
            get
            {
                return $"{toRecipientNameEmployeeCodes}_{GetInitials(recipientName)}";
            }
            //set
            //{
            //    toRecipientNameEmployeeCodes = value;
            //    OnPropertyChanged("ToRecipientNameEmployeeCodes");
            //}
        }


        #endregion


        #region Constructor
        public ToRecipientName()
        {

        }
        #endregion

        #region Methods

        //static string GetInitials(string fullName)
        //{
        //    if (string.IsNullOrEmpty(fullName))
        //        return string.Empty;
        //    Split the full name into words
        //    var words = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (words.Length == 1)
        //    {
        //        If there is only one name, return the first initial followed by 'A'
        //        if (fullName.Length > 1)
        //        {
        //            return new string(fullName.Take(2).ToArray()).ToUpper();
        //        }

        //    }
        //    [pramod.misal] [25.02.2025][[GEOS2 - 6726]]
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
