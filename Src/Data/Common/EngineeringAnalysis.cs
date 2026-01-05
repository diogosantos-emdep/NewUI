using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    //[DataContract(IsReference = true)]
    public class EngineeringAnalysis : ModelBase                 // INotifyPropertyChanged, ICloneable
    {
        #region Fields

        string comments;
        DateTime dueDate;
        string guidString;
        List<Attachment> attachments;
        List<object> tempAttachmentListUI;
        bool isCompleted;
        List<EngineeringAnalysisType> engineeringAnalysisTypes;
        List<EngineeringAnalysisType> selectedEngineeringAnalysisTypes;
        List<object> objselectedEngineeringAnalysisTypes;
        private bool isNew;
        Int32 revNumber;
        Int64 idOT;
        Int64 idRevision;
        Int32 revisionCreatedBy;
        DateTime revisionCreatedIn;
        List<Int64> idsArticle;
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public List<object> ObjselectedEngineeringAnalysisTypes
        {
            get
            {
                return objselectedEngineeringAnalysisTypes;
            }

            set
            {
                objselectedEngineeringAnalysisTypes = value;
                OnPropertyChanged("ObjselectedEngineeringAnalysisTypes");
            }
        }

        [NotMapped]
        [DataMember]
        public List<object> TempAttachmentListUI
        {
            get
            {
                return tempAttachmentListUI;
            }

            set
            {
                tempAttachmentListUI = value;
                OnPropertyChanged("TempAttachmentListUI");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EngineeringAnalysisType> SelectedEngineeringAnalysisTypes
        {
            get
            {
                return selectedEngineeringAnalysisTypes;
            }

            set
            {
                selectedEngineeringAnalysisTypes = value;
                OnPropertyChanged("SelectedEngineeringAnalysisTypes");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Int64> IdsArticle
        {
            get
            {
                return idsArticle;
            }

            set
            {
                idsArticle = value;
                OnPropertyChanged("IdsArticle");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32 RevisionCreatedBy
        {
            get { return revisionCreatedBy; }
            set
            {
               revisionCreatedBy = value;
                OnPropertyChanged("RevisionCreatedBy");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime RevisionCreatedIn
        {
            get { return revisionCreatedIn; }
            set
            {
                revisionCreatedIn = value;
                OnPropertyChanged("revisionCreatedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdOT
        {
            get { return idOT; }
            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdRevision
        {
            get { return idRevision; }
            set
            {
                idRevision = value;
                OnPropertyChanged("IdRevision");
            }
        }

        [NotMapped]
        [DataMember]
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }



        [NotMapped]
        [DataMember]
        public Int32 RevNumber
        {
            get { return revNumber; }
            set
            {
                revNumber = value;
                OnPropertyChanged("RevNumber");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
            }
        }

        [NotMapped]
        [DataMember]
        public string GUIDString
        {
            get { return guidString; }
            set
            {
                guidString = value;
                OnPropertyChanged("GUIDString");
            }
        }

        [NotMapped]
        [DataMember]
        public List<Attachment> Attachments
        {
            get { return attachments; }
            set
            {
                attachments = value;
                OnPropertyChanged("Attachments");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged("IsCompleted");
            }
        }

        [NotMapped]
        [DataMember]
        public List<EngineeringAnalysisType> EngineeringAnalysisTypes
        {
            get { return engineeringAnalysisTypes; }
            set
            {
                engineeringAnalysisTypes = value;
                OnPropertyChanged("EngineeringAnalysisTypes");
            }
        }
        [NotMapped]
        [DataMember]
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged("IsNew");
            }
        }

        #endregion

        #region Constructor

        public EngineeringAnalysis()
        {
        }

        #endregion

        //#region Events

        ///// <summary>
        ///// Occurs when a property value changes.
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        ///// Called when [property changed].
        ///// </summary>
        ///// <param name="name">The name.</param>
        //protected void OnPropertyChanged(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}

        //#endregion

        #region Methods

        public override object Clone()
        {
            EngineeringAnalysis engAnalysis = (EngineeringAnalysis)this.MemberwiseClone();

            if (engAnalysis.Attachments != null)
                engAnalysis.Attachments = Attachments.Select(x => (Attachment)x.Clone()).ToList();

            if (engAnalysis.EngineeringAnalysisTypes != null)
                engAnalysis.EngineeringAnalysisTypes = EngineeringAnalysisTypes.Select(x => (EngineeringAnalysisType)x.Clone()).ToList();

            if (engAnalysis.SelectedEngineeringAnalysisTypes != null)
                engAnalysis.SelectedEngineeringAnalysisTypes = SelectedEngineeringAnalysisTypes.Select(x => (EngineeringAnalysisType)x.Clone()).ToList();

            return engAnalysis;
        }

        #endregion



       
    }
}
