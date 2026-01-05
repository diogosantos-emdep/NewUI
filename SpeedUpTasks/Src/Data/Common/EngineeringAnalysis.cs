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
        bool isCompleted;
        List<EngineeringAnalysisType> engineeringAnalysisTypes;
        #endregion

        #region Properties

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

            return engAnalysis;
        }

        #endregion
    }
}
