using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class OtRequestTemplateLocationFields : ModelBase, IDisposable
    {

        #region Fields

        int idOTRequestTemplateLocationField;
        int idOTRequestFieldOption;
        string coordinates;
        int createdBy;
        DateTime createdIn;
        int? updatedBy;
        DateTime updatedIn;

        #endregion

        #region Properties
        [DataMember]
        public int IdOTRequestTemplateLocationField
        {
            get { return idOTRequestTemplateLocationField; }
            set { idOTRequestTemplateLocationField = value; OnPropertyChanged("IdOTRequestTemplateLocationField"); }
        }

        [DataMember]
        public int IdOTRequestFieldOption
        {
            get { return idOTRequestFieldOption; }
            set { idOTRequestFieldOption = value; OnPropertyChanged("IdOTRequestFieldOption"); }
        }

        [DataMember]
        public string Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; OnPropertyChanged("Coordinates"); }
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
