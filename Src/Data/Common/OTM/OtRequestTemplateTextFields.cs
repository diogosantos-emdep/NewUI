using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class OtRequestTemplateTextFields : ModelBase, IDisposable
    {

        #region Fields

        int idOTRequestTemplateTextField;
        int idOTRequestFieldOption;
        string keyword;
        string delimiter;
        int createdBy;
        DateTime createdIn;
        int? updatedBy;
        DateTime updatedIn;

        #endregion

        #region Properties

        [DataMember]
        public int IdOTRequestTemplateTextField
        {
            get { return idOTRequestTemplateTextField; }
            set { idOTRequestTemplateTextField = value; OnPropertyChanged("IdOTRequestTemplateTextField"); }
        }

        [DataMember]
        public int IdOTRequestFieldOption
        {
            get { return idOTRequestFieldOption; }
            set { idOTRequestFieldOption = value; OnPropertyChanged("IdOTRequestFieldOption"); }
        }

        [DataMember]
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; OnPropertyChanged("Keyword"); }
        }

        [DataMember]
        public string Delimiter
        {
            get { return delimiter; }
            set { delimiter = value; OnPropertyChanged("Delimiter"); }
        }

        [DataMember]
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; OnPropertyChanged("CreatedBy"); }
        }

        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set { createdIn = value; OnPropertyChanged("CreatedIn"); }
        }

        [DataMember]
        public int? UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; OnPropertyChanged("UpdatedBy"); }
        }

        [DataMember]
        public DateTime UpdatedIn
        {
            get { return updatedIn; }
            set { updatedIn = value; OnPropertyChanged("UpdatedIn"); }
        }

        #endregion


        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
