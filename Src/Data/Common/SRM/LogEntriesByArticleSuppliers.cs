using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    [DataContract]
    public class LogEntriesByArticleSuppliers : ModelBase, IDisposable
    {
        #region Declaration
        Int64 idLogEntryByArticleSuppliers;
        Int64 idArticleSupplier;
        Int64 idUser;
        DateTime? datetime;
        string comments;
        string changeLogUser;
        bool isRtfText;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        string userName;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        Int32 idLogEntryType;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        People people;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        int idEntryType;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        Int64 idLogEntryByContact;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        Int32 idcontact;//[chitra.girigosavi][GEOS2-4692][09.10.2023]
        #endregion

        #region Properties
        [Key]
        [Column("IdLogEntryByArticleSuppliers")]
        [DataMember]
        public Int64 IdLogEntryByArticleSuppliers
        {
            get { return idLogEntryByArticleSuppliers; }
            set
            {
                idLogEntryByArticleSuppliers = value;
                OnPropertyChanged("IdLogEntryByArticleSuppliers");
            }
        }

        [Key]
        [Column("IdArticleSupplier")]
        [DataMember]
        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }

        [Key]
        [Column("IdUser")]
        [DataMember]
        public Int64 IdUser
        {
            get { return idUser; }
            set
            {
                idUser = value;
                OnPropertyChanged("IdUser");
            }
        }

        [Key]
        [Column("Datetime")]
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

        [Key]
        [Column("Comments")]
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
        [Key]
        [Column("ChangeLogUser")]

        [DataMember]
        public string ChangeLogUser
        {
            get { return changeLogUser; }
            set
            {
                changeLogUser = value;
                OnPropertyChanged("ChangeLogUser");
            }
        }

        //[chitra.girigosavi][GEOS2-4692][09/10/2023]
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
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        [DataMember]
        public Int32 IdLogEntryType
        {
            get { return idLogEntryType; }
            set
            {
                idLogEntryType = value;
                OnPropertyChanged("IdLogEntryType");
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
        public int IdEntryType
        {
            get { return idEntryType; }
            set
            {
                idEntryType = value;
                OnPropertyChanged("IdEntryType");
            }
        }

        [DataMember]
        public Int64 IdLogEntryByContact
        {
            get { return idLogEntryByContact; }
            set
            {
                idLogEntryByContact = value;
                OnPropertyChanged("IdLogEntryByContact");
            }
        }
        [DataMember]
        public Int32 IdContact
        {
            get { return idcontact; }
            set
            {
                idcontact = value;
                OnPropertyChanged("Idcontact");
            }
        }
        #endregion

        #region Constructor

        public LogEntriesByArticleSuppliers()
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
