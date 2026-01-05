using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
   
    [DataContract]
    public class CPDetection : ModelBase, IDisposable
    {
        #region Fields

        Int64 cpProductID;
        Int32 detectionID;
        Int32 numDetections;
        string detectionName;
        //[Rahul.Gadhave][GEOS2-9141][Date:02-08-2025]
        Int64 idRevisionItem;
        private string emdepComment;
        private string customerComment;
        String attachedFiles;
        Int32 createdBy;
        private uint hecDocumentID;
        #endregion

        #region Constructor

        public CPDetection()
        {

        }

        #endregion

        #region Properties

     
        [DataMember]
        public Int64 CPProductID
        {
            get { return cpProductID; }
            set
            {
                cpProductID = value;
                OnPropertyChanged("cpProductID");
            }
        }

       
        [DataMember]
        public Int32 DetectionID
        {
            get { return detectionID; }
            set
            {
                detectionID = value;
                OnPropertyChanged("DetectionID");
            }
        }

      
        [DataMember]
        public Int32 NumDetections
        {
            get { return numDetections; }
            set
            {
                numDetections = value;
                OnPropertyChanged("NumDetections");
            }
        }


        [DataMember]
        public string DetectionName
        {
            get { return detectionName; }
            set
            {
                detectionName = value;
                OnPropertyChanged("DetectionName");
            }
        }
        [DataMember]
        public Int64 IdRevisionItem
        {
            get
            {
                return idRevisionItem;
            }

            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }
        [DataMember]
        public string EmdepComment
        {
            get
            {
                return emdepComment;
            }
            set
            {
                emdepComment = value;
            }
        }
        [DataMember]
        public string CustomerComment
        {
            get
            {
                return customerComment;
            }
            set
            {
                customerComment = value;
            }
        }
        [DataMember]
        public string AttachedFiles
        {
            get
            {
                return attachedFiles;
            }

            set
            {
                attachedFiles = value;
                OnPropertyChanged("AttachedFiles");
            }
        }

        [DataMember]
        public Int32 CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }
        [DataMember]
        public uint HEC_DocumentID
        {
            get
            {
                return hecDocumentID;
            }
            set
            {
                hecDocumentID = value;
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
