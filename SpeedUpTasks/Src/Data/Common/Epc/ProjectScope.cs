using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("project_scope")]
    [DataContract(IsReference = true)]
    public class ProjectScope : ModelBase, IDisposable
    {
        #region  Fields
        Int64 idProjectScope;
        Int64 idProject;
        Int64? idOffer;
        Int64? idProductVersion;
        DateTime? creationDate;
        Int32? createdBy;
        DateTime? modificationDate;
        string productScopeDescription;
        string projectAcceptanceCriteria;
        string projectAssumptions;
        string projectConstraints;
        string projectDeliverables;
        string projectExclusions;
        Project project;
        Offer offer;
        ProductVersion productVersion;
        byte[] scopeFileBytes;
        User user;
        #endregion

        #region Constructor
        public ProjectScope()
        {
            //  this.ProjectMilestoneDates = new List<ProjectMilestoneDate>();
        }
        #endregion

        #region Properties

        [Key]
        [Column("IdProjectScope")]
        [DataMember]
        public Int64 IdProjectScope
        {
            get
            {
                return idProjectScope;
            }
            set
            {
                idProjectScope = value;
                OnPropertyChanged("IdProjectScope");
            }
        }

        [Column("IdProject")]
        [ForeignKey("Project")]
        [DataMember]
        public Int64 IdProject
        {
            get
            {
                return idProject;
            }
            set
            {
                idProject = value;
                OnPropertyChanged("IdProject");
            }
        }

        [Column("IdOffer")]
        [ForeignKey("Offer")]
        [DataMember]
        public Int64? IdOffer
        {
            get
            {
                return idOffer;
            }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("IdProductVersion")]
        [ForeignKey("ProductVersion")]
        [DataMember]
        public Int64? IdProductVersion
        {
            get
            {
                return idProductVersion;
            }
            set
            {
                idProductVersion = value;
                OnPropertyChanged("IdProductVersion");
            }
        }

        [Column("CreatedBy")]
        [ForeignKey("User")]
        [DataMember]
        public Int32? CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
            }
        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get
            {
                return modificationDate;
            }
            set
            {
                modificationDate = value;
                OnPropertyChanged("ModificationDate");
            }
        }

        [Column("ProductScopeDescription")]
        [DataMember]
        public string ProductScopeDescription
        {
            get
            {
                return productScopeDescription;
            }
            set
            {
                productScopeDescription = value;
                OnPropertyChanged("ProductScopeDescription");
            }
        }

        [Column("ProjectAcceptanceCriteria")]
        [DataMember]
        public string ProjectAcceptanceCriteria
        {
            get
            {
                return projectAcceptanceCriteria;
            }
            set
            {
                projectAcceptanceCriteria = value;
                OnPropertyChanged("ProjectAcceptanceCriteria");
            }
        }

        [Column("ProjectAssumptions")]
        [DataMember]
        public string ProjectAssumptions
        {
            get
            {
                return projectAssumptions;
            }
            set
            {
                projectAssumptions = value;
                OnPropertyChanged("ProjectAssumptions");
            }
        }

        [Column("ProjectConstraints")]
        [DataMember]
        public string ProjectConstraints
        {
            get
            {
                return projectConstraints;
            }
            set
            {
                projectConstraints = value;
                OnPropertyChanged("ProjectConstraints");
            }
        }

        [Column("ProjectDeliverables")]
        [DataMember]
        public string ProjectDeliverables
        {
            get
            {
                return projectDeliverables;
            }
            set
            {
                projectDeliverables = value;
                OnPropertyChanged("ProjectDeliverables");
            }
        }

        [Column("ProjectExclusions")]
        [DataMember]
        public string ProjectExclusions
        {
            get
            {
                return projectExclusions;
            }
            set
            {
                projectExclusions = value;
                OnPropertyChanged("ProjectExclusions");
            }
        }

        [NotMapped]
        [DataMember]
        public byte[] ScopeFileBytes
        {
            get
            {
                return scopeFileBytes;
            }
            set
            {
                scopeFileBytes = value;
                OnPropertyChanged("ScopeFileBytes");
            }
        }

        [DataMember]
        public virtual Project Project
        {
            get
            {
                return project;
            }
            set
            {
                project = value;
                OnPropertyChanged("Project");
            }
        }


        [DataMember]
        public virtual User User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        [DataMember]
        public virtual Offer Offer
        {
            get
            {
                return offer;
            }
            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }

        [DataMember]
        public virtual ProductVersion ProductVersion
        {
            get
            {
                return productVersion;
            }
            set
            {
                productVersion = value;
                OnPropertyChanged("ProductVersion");
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
