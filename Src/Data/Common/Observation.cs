using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("observations")]
    [DataContract]
    public class Observation : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idComment;
        Int64 idRevisionItem;
        sbyte idStage;
        Int16 idSupervisor;
        sbyte idType;
       Int32 idUser;
        string text;
        DateTime creationDate;
        #endregion

        #region Constructor
        public Observation()
        {
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdComment")]
        [DataMember]
        public Int64 IdComment
        {
            get
            {
                return idComment;
            }
            set
            {
                idComment = value;
                OnPropertyChanged("IdComment");
            }
        }

        [Column("IdRevisionItem")]
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

        [Column("IdStage")]
        [DataMember]
        public sbyte IdStage
        {
            get
            {
                return idStage;
            }
            set
            {
                idStage = value;
                OnPropertyChanged("IdStage");
            }
        }

        [Column("IdSupervisor")]
        [DataMember]
        public Int16 IdSupervisor
        {
            get
            {
                return idSupervisor;
            }
            set
            {
                idSupervisor = value;
                OnPropertyChanged("IdSupervisor");
            }
        }

        [Column("IdType")]
        [DataMember]
        public sbyte IdType
        {
            get
            {
                return idType;
            }
            set
            {
                idType = value;
                OnPropertyChanged("IdType");
            }
        }

        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get
            {
                return idUser;
            }
            set
            {
               idUser = value;
                OnPropertyChanged("IdUser");
            }
        }


        [Column("Text")]
        [DataMember]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        #endregion
        #region Methods
        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
