using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
	// [nsatpute][12-05-2025][GEOS2-5707]
    public class Recruitment : ModelBase, IDisposable
    {
        #region Private Fields

        private string code;
        private string type;
        private string status;
        private string title;
        private string jobDescription;
        private string plant;
        private string experience;
        private string employee;
        private string publishedInSite;
        private string internalRecruitment;
        private string createdBy;

        private DateTime closeDate;
        private DateTime createdAt;

        #endregion

        #region Public Properties

        [NotMapped]
        [DataMember]
        public string Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged(nameof(Code)); }
        }

        [NotMapped]
        [DataMember]
        public string Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged(nameof(Type)); }
        }

        [NotMapped]
        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(nameof(Status)); }
        }

        [NotMapped]
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(nameof(Title)); }
        }

        [NotMapped]
        [DataMember]
        public string JobDescription
        {
            get { return jobDescription; }
            set { jobDescription = value; OnPropertyChanged(nameof(JobDescription)); }
        }

        [NotMapped]
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set { plant = value; OnPropertyChanged(nameof(Plant)); }
        }

        [NotMapped]
        [DataMember]
        public string Experience
        {
            get { return experience; }
            set { experience = value; OnPropertyChanged(nameof(Experience)); }
        }

        [NotMapped]
        [DataMember]
        public string Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged(nameof(Employee)); }
        }

        [NotMapped]
        [DataMember]
        public string PublishedInSite
        {
            get { return publishedInSite; }
            set { publishedInSite = value; OnPropertyChanged(nameof(PublishedInSite)); }
        }

        [NotMapped]
        [DataMember]
        public string Internal
        {
            get { return internalRecruitment; }
            set { internalRecruitment = value; OnPropertyChanged(nameof(Internal)); }
        }

        [NotMapped]
        [DataMember]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; OnPropertyChanged(nameof(CreatedBy)); }
        }

        [NotMapped]
        [DataMember]
        public DateTime CloseDate
        {
            get { return closeDate; }
            set { closeDate = value; OnPropertyChanged(nameof(CloseDate)); }
        }

        [NotMapped]
        [DataMember]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; OnPropertyChanged(nameof(CreatedAt)); }
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
