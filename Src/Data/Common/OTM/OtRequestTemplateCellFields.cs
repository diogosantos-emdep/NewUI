using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class OtRequestTemplateCellFields : ModelBase, IDisposable
    {

        #region Fields

        int idOTRequestTemplateCellField;
        int idOTRequestFieldOption;
        string cells;
        string keyword;
        string delimiter;
        int createdBy;
        DateTime createdIn;
        int? updatedBy;
        DateTime updatedIn;

        #endregion
        [DataMember]
        public int IdOTRequestTemplateCellField
        {
            get { return idOTRequestTemplateCellField; }
            set { idOTRequestTemplateCellField = value; OnPropertyChanged("IdOTRequestTemplateCellField"); }
        }

        [DataMember]
        public int IdOTRequestFieldOption
        {
            get { return idOTRequestFieldOption; }
            set { idOTRequestFieldOption = value; OnPropertyChanged("IdOTRequestFieldOption"); }
        }

        [DataMember]
        public string Cells
        {
            get { return cells; }
            set { cells = value; OnPropertyChanged("Cells"); }
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

        #region Properties

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
