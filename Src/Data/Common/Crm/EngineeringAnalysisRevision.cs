using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Crm
{
    [DataContract]
    public class EngineeringAnalysisRevision : ModelBase, IDisposable, IDataErrorInfo
    {

        #region Fields
        private string header;
        List<EngineeringAnalysisType> engineeringAnalysisTypes;
        List<object> selectedEngineeringAnalysisTypes;
        private bool isEngineeringAnalysisTypesReadOnly = false;
        private string comments;
        private bool isCommentsReadOnly = false;
        private List<object> tempAttachmentList;
        private int selectedIndexAttachment = -1;
        private bool isEngAnalysisChooseFileEnable = true;
        private bool isDateReadOnly = false;
        private DateTime dueDate;
        private Int32 revNumber;
        private Int64 idRevision;
        private Int64 idOT;
        private List<Int64> idsArticle;
        #endregion

        #region Constructor

        public EngineeringAnalysisRevision()
        {
        }

        #endregion

        #region Properties
        [DataMember]
        public Int64 IdOT
        {
            get
            {
                return idOT;
            }

            set
            {
                idOT = value;
                OnPropertyChanged("IdOT");
            }
        }


        [DataMember]
        public Int64 IdRevision
        {
            get
            {
                return idRevision;
            }

            set
            {
                idRevision = value;
                OnPropertyChanged("IdRevision");
            }
        }

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
        [DataMember]
        public Int32 RevNumber
        {
            get
            {
                return revNumber;
            }

            set
            {
                revNumber = value;
                OnPropertyChanged("RevNumber");
            }
        }

        [DataMember]
        public List<EngineeringAnalysisType> EngineeringAnalysisTypes
        {
            get
            {
                return engineeringAnalysisTypes;
            }

            set
            {
                engineeringAnalysisTypes = value;
                OnPropertyChanged("EngineeringAnalysisTypes");
            }
        }

        [DataMember]
        public List<object> SelectedEngineeringAnalysisTypes
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

        [DataMember]
        public bool IsEngineeringAnalysisTypesReadOnly
        {
            get
            {
                return isEngineeringAnalysisTypesReadOnly;
            }

            set
            {
                isEngineeringAnalysisTypesReadOnly = value;
                OnPropertyChanged("IsEngineeringAnalysisTypesReadOnly");
            }
        }

        [DataMember]
        public string Comments
        {
            get
            {
                return comments;
            }

            set
            {
                comments = value;
                OnPropertyChanged("Comments");
            }
        }

        [DataMember]
        public bool IsCommentsReadOnly
        {
            get
            {
                return isCommentsReadOnly;
            }

            set
            {
                isCommentsReadOnly = value;
                OnPropertyChanged("IsCommentsReadOnly");
            }
        }

        [DataMember]
        public List<object> TempAttachmentList
        {
            get
            {
                return tempAttachmentList;
            }

            set
            {
                tempAttachmentList = value;
                OnPropertyChanged("TempAttachmentList");
            }
        }

        [DataMember]
        public int SelectedIndexAttachment
        {
            get
            {
                return selectedIndexAttachment;
            }

            set
            {
                selectedIndexAttachment = value;
                OnPropertyChanged("SelectedIndexAttachment");
            }
        }

        [DataMember]
        public bool IsEngAnalysisChooseFileEnable
        {
            get
            {
                return isEngAnalysisChooseFileEnable;
            }

            set
            {
                isEngAnalysisChooseFileEnable = value;
                OnPropertyChanged("IsEngAnalysisChooseFileEnable");
            }
        }

        [DataMember]
        public bool IsDateReadOnly
        {
            get
            {
                return isDateReadOnly;
            }

            set
            {
                isDateReadOnly = value;
                OnPropertyChanged("IsDateReadOnly");
            }
        }

        [DataMember]
        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }

            set
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
            }
        }

        [DataMember]
        public string Header
        {
            get
            {
                return header;
            }

            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }

        private bool isNewRevision;
        [DataMember]
        public bool IsNewRevision
        {
            get
            {
                return isNewRevision;
            }

            set
            {
                isNewRevision = value;
                OnPropertyChanged("IsNewRevision");
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
            EngineeringAnalysisRevision engineeringAnalysisRevision = (EngineeringAnalysisRevision)this.MemberwiseClone();
            if (engineeringAnalysisRevision.EngineeringAnalysisTypes != null)
                engineeringAnalysisRevision.EngineeringAnalysisTypes = EngineeringAnalysisTypes.Select(x => (EngineeringAnalysisType)x.Clone()).ToList();

           

            return engineeringAnalysisRevision;
        }

        #endregion


        #region Validation

        public string Error
        {
            get { return GetError(); }
        }

        public string this[string columnName]
        {
            get { return GetError(columnName); }
        }

        string GetError(string name = null)
        {
            switch (name)
            {
                case "Comments":
                    return Comments == null ? "Comment is mandatory.": Comments ==""? "Comment is mandatory." : null;

                case "DueDate":
                    return DueDate == DateTime.MinValue ? "Due Date is mandatory." : null;

                case "SelectedEngineeringAnalysisTypes":
                    return (!(EngineeringAnalysisTypes!=null && EngineeringAnalysisTypes.Where(i=>i.IsChecked).Count()>1)) ? "Type is mandatory." : null;
                default:
                    return null;
            }
        }

        #endregion
    }
}
