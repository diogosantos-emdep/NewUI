using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Data.Common.OTM
{
    //[pramod.misal][04.02.2025][GEOS2-6726]
    [DataContract]
    public class ToCCName : ModelBase, IDisposable
    {
        #region Fields
        private string cCIdperson;

        private string cCName;
        private string cCEmail;

        private string cCNameEmployeeCodes;
        private string cCNameJobDescriptionTitles;
        private Visibility isEmdepContact = Visibility.Hidden;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324
        private Visibility isNotEmdepContact = Visibility.Hidden;//[pramod.misal][GEOS2-9324][30.09.2025]https://helpdesk.emdep.com/browse/GEOS2-9324

        #endregion


        #region Properties

        [DataMember]
        public string CCIdperson
        {
            get
            {
                return cCIdperson;
            }

            set
            {
                cCIdperson = value;
                OnPropertyChanged("CCIdperson");
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
        public string CCEmail
        {
            get { return cCEmail; }
            set
            {
                cCEmail = value;
                OnPropertyChanged("CCEmail");
            }
        }

        [DataMember]
        public string CCName
        {
            get { return cCName; }
            set
            {
                cCName = value;
                OnPropertyChanged("CCName");
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

        public string CCNameEmployeeCodesWithInitialLetters
        {
            get
            {
                return $"{CCNameEmployeeCodes}_{GetInitials(cCName)}";
            }
            //set
            //{
            //    toRecipientNameEmployeeCodes = value;
            //    OnPropertyChanged("CCNameEmployeeCodes");
            //}
        }

        #endregion


        #region Constructor
        public ToCCName()
        {

        }
        #endregion

        #region Methods

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
        //    //[pramod.misal][25.02.2025][[GEOS2-6726]]
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
