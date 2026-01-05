using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.TSM
{
    //[GEOS2-8965][pallavi.kale][28.11.2025]
    [DataContract]
    public class TSMLogEntriesByOT : ModelBase, IDisposable
    {
        #region Fields

        Int64 idLogEntryByOT;
        Int64 idOT;
        Int32 idUser;
        DateTime? datetime;
        string comments;
        byte? idLogEntryType;
        People people;
        bool isRtfText;
        string realText;

        #endregion

        #region Constructor

        public TSMLogEntriesByOT()
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
