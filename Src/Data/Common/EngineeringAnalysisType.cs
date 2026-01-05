using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class EngineeringAnalysisType : ModelBase, IDisposable
    {
        #region Fields
        Int64 idArticle;
        string reference;
        bool isSelected;
        string quantity;
        byte idRevisionItemStatus;
        bool isArticleEnabled;
        private bool isChecked;
        Int64 idRevision;
        Int64 idRevisionItem;
        Int64 assignedToUser;
        private User selectedAssignee;
        #endregion

        #region Constructor
        public EngineeringAnalysisType()
        {

        }
        #endregion

        #region Properties

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
            }
        }

        [DataMember]
        public Int64 IdArticle
        {
            get
            {
                return idArticle;
            }

            set
            {
                idArticle = value;
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
            }
        }


        [DataMember]
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }


        [DataMember]
        public string Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        [DataMember]
        public byte IdRevisionItemStatus
        {
            get
            {
                return idRevisionItemStatus;
            }

            set
            {
                idRevisionItemStatus = value;
            }
        }

        [DataMember]
        public bool IsArticleEnabled
        {
            get
            {
                return isArticleEnabled;
            }

            set
            {
                isArticleEnabled = value;
            }
        }
        [DataMember]
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }

            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
       //[rahul.gadhave][16 - 10 - 2025][GEOS2 - 8438]
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
        [NotMapped]
        [DataMember]
        public Int64 AssignedToUser
        {
            get { return assignedToUser; }
            set
            {
                assignedToUser = value;
                OnPropertyChanged("AssignedToUser");
            }
        }


        [NotMapped]
        [DataMember]
        public User SelectedAssignee
        {
        
            get { return selectedAssignee; }
            set
            {
                if (selectedAssignee != value)
                {
                    selectedAssignee = value;
                    OnPropertyChanged("SelectedAssignee");
                    AssignedToUser = selectedAssignee?.IdUser ?? 0;
                }
            }
        }

        private int selectedIndexAssignee;
        [NotMapped]
        [DataMember]
        public int SelectedIndexAssignee
        {
            get { return selectedIndexAssignee; }
            set
            {
                selectedIndexAssignee = value;
                OnPropertyChanged("SelectedIndexAssignee");
            }
        }
        private ObservableCollection<User> assigneeList;
        [DataMember]
        public ObservableCollection<User> AssigneeList
        {
            get { return assigneeList; }
            set
            {
                assigneeList = value;
                OnPropertyChanged("AssigneeList");
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
            EngineeringAnalysisType engineeringAnalysisType = (EngineeringAnalysisType)this.MemberwiseClone();

            return engineeringAnalysisType;
        }


        #endregion
    }
}
