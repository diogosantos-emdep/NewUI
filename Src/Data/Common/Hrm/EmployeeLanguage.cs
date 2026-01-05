using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("employee_languages")]
    [DataContract]
    public class EmployeeLanguage : ModelBase, IDisposable
    {
        #region Fields

        UInt64 idEmployeeLanguage;
        Int32 idEmployee;
        Int32 idLanguage;
        Int32 understandingIdLanguageLevel;
        Int32 speakingIdLanguageLevel;
        Int32 writingIdLanguageLevel;
        string languageRemarks;

        LookupValue language;
        LookupValue understandingLevel;
        LookupValue speakingLevel;
        LookupValue writingLevel;

        #endregion

        #region Constructor

        public EmployeeLanguage()
        {
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdEmployeeLanguage")]
        [DataMember]
        public ulong IdEmployeeLanguage
        {
            get { return idEmployeeLanguage; }
            set
            {
                idEmployeeLanguage = value;
                OnPropertyChanged("IdEmployeeLanguage");
            }
        }

        [Column("IdEmployee")]
        [DataMember]
        public int IdEmployee
        {
            get { return idEmployee; }
            set
            {
                idEmployee = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Column("IdEmployee")]
        [DataMember]
        public int IdLanguage
        {
            get { return idLanguage; }
            set
            {
                idLanguage = value;
                OnPropertyChanged("IdEmployee");
            }
        }

        [Column("UnderstandingIdLanguageLevel")]
        [DataMember]
        public int UnderstandingIdLanguageLevel
        {
            get { return understandingIdLanguageLevel; }
            set
            {
                understandingIdLanguageLevel = value;
                OnPropertyChanged("UnderstandingIdLanguageLevel");
            }
        }

        [Column("SpeakingIdLanguageLevel")]
        [DataMember]
        public int SpeakingIdLanguageLevel
        {
            get { return speakingIdLanguageLevel; }
            set
            {
                speakingIdLanguageLevel = value;
                OnPropertyChanged("SpeakingIdLanguageLevel");
            }
        }

        [Column("WritingIdLanguageLevel")]
        [DataMember]
        public int WritingIdLanguageLevel
        {
            get { return writingIdLanguageLevel; }
            set
            {
                writingIdLanguageLevel = value;
                OnPropertyChanged("WritingIdLanguageLevel");
            }
        }

        [Column("LanguageRemarks")]
        [DataMember]
        public string LanguageRemarks
        {
            get { return languageRemarks; }
            set
            {
                languageRemarks = value;
                OnPropertyChanged("LanguageRemarks");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged("Language");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue UnderstandingLevel
        {
            get { return understandingLevel; }
            set
            {
                understandingLevel = value;
                OnPropertyChanged("UnderstandingLevel");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue SpeakingLevel
        {
            get { return speakingLevel; }
            set
            {
                speakingLevel = value;
                OnPropertyChanged("SpeakingLevel");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue WritingLevel
        {
            get { return writingLevel; }
            set
            {
                writingLevel = value;
                OnPropertyChanged("WritingLevel");
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
            //return this.MemberwiseClone();
            EmployeeLanguage employeeLanguage = (EmployeeLanguage)this.MemberwiseClone();

            if (employeeLanguage.Language != null)
                employeeLanguage.Language = (LookupValue)this.Language.Clone();

            if (employeeLanguage.UnderstandingLevel != null)
                employeeLanguage.UnderstandingLevel = (LookupValue)this.UnderstandingLevel.Clone();

            if (employeeLanguage.WritingLevel != null)
                employeeLanguage.WritingLevel = (LookupValue)this.WritingLevel.Clone();

            if (employeeLanguage.SpeakingLevel != null)
                employeeLanguage.SpeakingLevel = (LookupValue)this.SpeakingLevel.Clone();

            return employeeLanguage;
        }

        #endregion
    }
}
