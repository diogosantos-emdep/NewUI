using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TSM
{
    //[GEOS2-8965][pallavi.kale][28.11.2025]
    public class TSMLogEntries : ModelBase, IDisposable
    {
        #region Fields

        Int64 idLogEntryByOT;
        Int64 idOT;
        Int32 idUser;
        Int64 idComment;
        DateTime? datetime;
        string comments;
        byte? idLogEntryType;
        Int32 idEntryType;
        People people;
        bool isRtfText;
        string realText;
        TransactionOperations transactionOperation;
        #endregion

        #region Constructor

        public TSMLogEntries()
        {

        }

        #endregion

        #region Properties

        [DataMember]
        public Int64 IdLogEntryByOT
        {
            get { return idLogEntryByOT; }
            set
            {
                idLogEntryByOT = value;
                OnPropertyChanged("IdLogEntryByOT");
            }
        }

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
        public Int64 IdComment
        {
            get { return idComment; }
            set
            {
                idComment = value;
                OnPropertyChanged("IdComment");
            }
        }
        [DataMember]
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
        public byte? IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
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
        public People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }

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

        Int64 idRevisionItem;
        [DataMember]
        public Int64 IdRevisionItem
        {
            get { return idRevisionItem; }
            set
            {
                idRevisionItem = value;
                OnPropertyChanged("IdRevisionItem");
            }
        }
        Int32 idPerson;
        [Key]
        [Column("IdPerson")]
        [DataMember]
        public Int32 IdPerson
        {
            get { return idPerson; }
            set
            {
                idPerson = value;
                OnPropertyChanged("IdPerson");
            }
        }
        string name;
        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        string surname;
        [Column("Surname")]
        [DataMember]
        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return Name + ' ' + Surname; }
            set { }
        }
        [NotMapped]
        [DataMember]
        public string UserName
        {
            get { return Name + ' ' + Surname; }
            set { }
        }

        [DataMember]
        public TransactionOperations TransactionOperation
        {
            get
            {
                return transactionOperation;
            }
            set
            {
                transactionOperation = value;
                this.OnPropertyChanged("TransactionOperation");
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
