using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    public class MicroSiga_Article:ModelBase
    {
        #region Declaration
        Int32 idArticle;
        string reference;
        string description;
        string nCM_Code;
        #endregion
        [DataMember]
        public Int32 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }
        [DataMember]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        [DataMember]
        public string NCM_Code
        {
            get
            {
                return nCM_Code;
            }

            set
            {
                nCM_Code = value;
                OnPropertyChanged("NCM_Code");
            }
        }
    }
}
