using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media.Imaging;
using Emdep.Geos.Data.Common.TechnicalRestService;

namespace Emdep.Geos.Data.Common.SAM
{
    [DataContract]
    public class ElectricalDiagram : ModelBase, IDisposable
    {
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        #region Fields
        string name;
        Int32 parentElectricalDiagramID;
        Int32 subElectricalDiagramID;
        string description;
        string reference;
        Int32 revision;
        DateTime? reviewedIn;
        Int32 isObsolete;
        Int32 pCBAllowed;
        string parent;
        string key;
        string filePath;
        bool isSelected;
        Int32 idUser;
        DateTime? datetime;
        string comments;
        Int32 idDrawing;
        #endregion

        #region Properties

        [NotMapped]
        [DataMember]
        public Int32 ParentElectricalDiagramID
        {
            get
            {
                return parentElectricalDiagramID;
            }

            set
            {
                parentElectricalDiagramID = value;
                OnPropertyChanged("ParentElectricalDiagramID");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 SubElectricalDiagramID
        {
            get
            {
                return subElectricalDiagramID;
            }

            set
            {
                subElectricalDiagramID = value;
                OnPropertyChanged("SubElectricalDiagramID");
            }
        }
        [NotMapped]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        [NotMapped]
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
        [NotMapped]
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
        [NotMapped]
        [DataMember]
        public Int32 Revision
        {
            get
            {
                return revision;
            }

            set
            {
                revision = value;
                OnPropertyChanged("Revision");
            }
        }
        [NotMapped]
        [DataMember]
        public DateTime? ReviewedIn
        {
            get
            {
                return reviewedIn;
            }

            set
            {
                reviewedIn = value;
                OnPropertyChanged("ReviewedIn");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 IsObsolete
        {
            get
            {
                return isObsolete;
            }

            set
            {
                isObsolete = value;
                OnPropertyChanged("IsObsolete");
            }
        }
        [NotMapped]
        [DataMember]
        public Int32 PCBAllowed
        {
            get
            {
                return pCBAllowed;
            }

            set
            {
                pCBAllowed = value;
                OnPropertyChanged("PCBAllowed");
            }
        }
        [NotMapped]
        [DataMember]
        public string Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
                OnPropertyChanged("Parent");
            }
        }

        [NotMapped]
        [DataMember]
        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }

        [NotMapped]
        [DataMember]
        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }
        [DataMember]
        [NotMapped]
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        [DataMember]
        [NotMapped]
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }
        [DataMember]
        [NotMapped]
        public DateTime? Datetime
        {
            get { return datetime; }
            set
            {
                datetime = value;
                OnPropertyChanged("Datetime");
            }
        }
        [DataMember]
        [NotMapped]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }
        [DataMember]
        [NotMapped]
        public Int32 IdDrawing
        {
            get { return idDrawing; }
            set
            {
                idDrawing = value;
                OnPropertyChanged("IdDrawing");
            }
        }
        #endregion

        #region Constructor
        public ElectricalDiagram()
        {

        }
        #endregion

        #region Method
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //public override object Clone()
        //{
        //    ElectricalDiagramCategory ElectricalDiagramCategory = (ElectricalDiagramCategory)this.MemberwiseClone();

        //    if (FamilyImagesList != null)
        //        connectorFamily.FamilyImagesList = FamilyImagesList.Select(x => (FamilyImage)x.Clone()).ToList();

        //    if (SubFamilyList != null)
        //        connectorFamily.SubFamilyList = SubFamilyList.Select(x => (ConnectorSubFamily)x.Clone()).ToList();

        //    return connectorFamily;
        //}
        #endregion
    }
}
