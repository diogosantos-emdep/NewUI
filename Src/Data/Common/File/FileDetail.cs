using Emdep.Geos.Data.Common.SAM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel.DataAnnotations.Schema;
namespace Emdep.Geos.Data.Common.File

{
    [DataContract]
    public class FileDetail : ModelBase, IDisposable
    {
        #region Fields

        string fileName;
        string referenceName;
        Byte[] fileByte;
        string filePath;// [GEOS2-6727][pallavi.kale][14-04-2025]
        ImageSource attachmentImage;// [GEOS2-6727][pallavi.kale][14-04-2025]
        private Int64 idElectricalDiagram;// [GEOS2-6727][pallavi.kale][14-04-2025]
        private string savedFileName;// [GEOS2-6727][pallavi.kale][14-04-2025]
        Int32 idUser;// [GEOS2-6727][pallavi.kale][14-04-2025]
        DateTime? datetime;// [GEOS2-6727][pallavi.kale][14-04-2025]
        string comments;// [GEOS2-6727][pallavi.kale][14-04-2025]
        Int32 idDrawing;// [GEOS2-6727][pallavi.kale][14-04-2025]
        OperationDb operation;
        private string description;// [GEOS2-6727][pallavi.kale][26-05-2025]
        private string status; //[nsatpute][25-06-2025][GEOS2-8641]
        private string connectingSites; //[nsatpute][25-06-2025][GEOS2-8641]
        #endregion

        #region Constructor
        public FileDetail()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }
		//[nsatpute][25-06-2025][GEOS2-8641]
        [DataMember]
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        [DataMember]
        public string ConnectingSites
        {
            get { return connectingSites; }
            set
            {
                connectingSites = value;
                OnPropertyChanged("ConnectingSites");
            }
        }

        [DataMember]
        public string ReferenceName
        {
            get { return referenceName; }
            set
            {
                referenceName = value;
                OnPropertyChanged("ReferenceName");
            }
        }

        [DataMember]
        public Byte[] FileByte
        {
            get { return fileByte; }
            set
            {
                fileByte = value;
                OnPropertyChanged("FileByte");
            }
        }
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        [NotMapped]
        [DataMember]
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        [NotMapped]
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
        // [GEOS2-6727][pallavi.kale][14-04-2025]
        [NotMapped]
        [DataMember]
        public Int64 IdElectricalDiagram
        {
            get { return idElectricalDiagram; }
            set
            {
                idElectricalDiagram = value;
                OnPropertyChanged("IdElectricalDiagram");
            }
        }
       // [GEOS2-6727][pallavi.kale][14-04-2025]
        [NotMapped]
        [DataMember]
        public string SavedFileName
        {
            get { return savedFileName; }
            set
            {
                savedFileName = value;
                OnPropertyChanged("SavedFileName");
            }
        }
       // [GEOS2-6727][pallavi.kale][14-04-2025]
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
      // [GEOS2-6727][pallavi.kale][14-04-2025]
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
       // [GEOS2-6727][pallavi.kale][14-04-2025]
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
		// [nsatpute][13-05-2025][GEOS2-6728]
        [DataMember]
        [NotMapped]
        public OperationDb Operation
        {
            get { return operation; }
            set { operation = value; OnPropertyChanged("Operation"); }
        }
        // [GEOS2-6727][pallavi.kale][26-05-2025]
        [DataMember]
        [NotMapped]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        #endregion

        #region Methods

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
