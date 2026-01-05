using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.APM
{//[Sudhir.Jangra][GEOS2-6015]
    [DataContract]
    public class CommentsByTask : ModelBase, IDisposable
    {
        #region Fields
        private Int64 idActionPlanTaskComment;
        private Int64 idTask;
        private string comments;
        private Int32 idUser;
        private DateTime createdIn;
        private People people;
        private bool isDeleted;
        private bool isUpdated;
        private bool isRtfText;
        private string realText;
        private Int64 idActionPlan;//[Sudhir.Jangra][GEOS2-6616]
        private Int32 taskNumber;//[Sudhir.Jangra][GEOS2-6616]
        private List<LogEntriesByActionPlan> changeLogList;//[Sudhir.Jangra][GEOS2-6616]
        #endregion

        #region Properties
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlanTaskComment
        {
            get { return idActionPlanTaskComment; }
            set
            {
                idActionPlanTaskComment = value;
                OnPropertyChanged("IdActionPlanTaskComment");
            }
        }

        [NotMapped]
        [DataMember]
        public Int64 IdTask
        {
            get { return idTask; }
            set
            {
                idTask = value;
                OnPropertyChanged("IdTask");
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
        public Int32 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [NotMapped]
        [DataMember]
        public DateTime CreatedIn
        {
            get { return createdIn; }
            set
            {
                createdIn = value;
                OnPropertyChanged("CreatedIn");
            }
        }

        [NotMapped]
        [DataMember]
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdated
        {
            get { return isUpdated; }
            set
            {
                isUpdated = value;
                OnPropertyChanged("IsUpdated");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsRtfText
        {
            get { return isRtfText; }
            set
            {
                isRtfText = value;
                OnPropertyChanged("IsRtfText");
            }
        }

        [NotMapped]
        [DataMember]
        public string RealText
        {
            get { return realText; }
            set
            {
                realText = value;
                OnPropertyChanged("RealText");
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        [NotMapped]
        [DataMember]
        public Int64 IdActionPlan
        {
            get { return idActionPlan; }
            set
            {
                idActionPlan = value;
                OnPropertyChanged("IdActionPlan");
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        [NotMapped]
        [DataMember]
        public Int32 TaskNumber
        {
            get { return taskNumber; }
            set
            {
                taskNumber = value;
                OnPropertyChanged("TaskNumber");
            }
        }

        //[Sudhir.Jangra][GEOS2-6616]
        [NotMapped]
        [DataMember]
        public List<LogEntriesByActionPlan> ChangeLogList
        {
            get { return changeLogList; }
            set
            {
                changeLogList = value;
                OnPropertyChanged("ChangeLogList");
            }
        }

        #endregion

        #region Constructor
        public CommentsByTask()
        {

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
