using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    public class SRMOtItemsComment : ModelBase
    {
        #region Constructor

        public SRMOtItemsComment()
        {

        }
        #endregion

        #region Fields
        //private Status itemStatus;
        Int32 idotitem;
        string comment;
        string userName;
        Int32 idUser;
        Int32 idComment;
        Int64 idrevisionitem;
        DateTime commentDate;
        Int32 idEntryType;

        List<LogEntriesByWarehousePO> commentListDelete;
        #endregion

        #region Properties


        [DataMember]
        public Int32 IdComment
        {
            get { return idComment; }
            set
            {
                idComment = value;
                OnPropertyChanged("IdComment");
            }
        }

        [Column("IdRevisionItem")]
        [DataMember]
        public Int64 Idrevisionitem
        {
            get { return idrevisionitem; }
            set
            {
                idrevisionitem = value;
                OnPropertyChanged("Idrevisionitem");
            }
        }
        [DataMember]
        public Int32 Idotitem
        {
            get { return idotitem; }
            set
            {
                idotitem = value;
                OnPropertyChanged("Idotitem");
            }
        }

        [DataMember]
        public string Comments
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged("Comment");
            }
        }

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

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }


        [DataMember]
        public DateTime CommentDate
        {
            get { return commentDate; }
            set
            {
                commentDate = value;
                OnPropertyChanged("CommentDate");
            }
        }
        [DataMember]
        public Int32 IdEntryType
        {
            get { return idEntryType; }
            set
            {
                idEntryType = value;
                OnPropertyChanged("IdEntryType");
            }
        }

        [DataMember]
        public List<LogEntriesByWarehousePO> CommentListDelete
        {
            get { return commentListDelete; }
            set
            {
                commentListDelete = value;
                OnPropertyChanged("CommentListDelete");
            }
        }
        #endregion

    }
}
