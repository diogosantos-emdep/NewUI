using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Emdep.Geos.Data.Common
{
    public class Languages
    {
        private int _IdLanguage = 0;
        [DataMember(Order = 1)]
        public int IdLanguage
        {
            get { return _IdLanguage; }
            set { _IdLanguage = value; }
        }
        private string _TwoLetterISOLanguage = string.Empty;
        [DataMember(Order = 2)]
        public string TwoLetterISOLanguage
        {
            get { return _TwoLetterISOLanguage; }
            set { _TwoLetterISOLanguage = value; }
        }

        private string _Name = string.Empty;
        [DataMember(Order = 3)]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _CultureName = string.Empty;
        [DataMember(Order = 4)]
        public string CultureName
        {
            get { return _CultureName; }
            set { _CultureName = value; }
        }
    }
}