using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Emdep.Geos.Data.Common.Epc
{
    [Table("projects")]
    [DataContract]
    public class Project : ModelBase,IDisposable
    {
        #region Fields
        Int64 idProject;
        string projectCode;
        string projectName;
        string description;
        Int64 idProduct;
        Product product;
        Int32 idCustomer;
        Customer customer;
        Int32? idOwner;
        User owner;
        Int32 idProjectPriority;
        Int32 idProjectType;
        LookupValue projectType;
        LookupValue projectPriority;
        Int32 idProjectStatus;
        LookupValue projectStatus;
        Int32 idCategory;
        LookupValue category;
        string projectPath;
        DateTime? dueDate;
        DateTime? startDate;
        //List<Offer> offers;
        List<ProjectTask> projectTasks;
        Int64 idOffer;
        Offer offer;
        byte? idTeam;
        Int32? idProjectTeam;
        Team team;
        string geosPath;
        Int64? idGeosStatus;
        GeosStatus geosStatus;
        List<ProjectAnalysis> projectAnalysis;
        ObservableCollection<ProjectFollowup> projectFollowups;
        ObservableCollection<ProjectMilestone> projectMilestones;
        DateTime? creationDate;
        Int32? idCreator;
        User creator;
       List<ProjectTeam> projectTeams;
        #endregion

        #region Constructor
        public Project()
        {
            this.ProjectAnalysis = new List<ProjectAnalysis>();
            this.ProjectTasks = new List<ProjectTask>();
            this.ProjectTasks = new List<ProjectTask>();
            //this.ProjectFollowups = new List<ProjectFollowup>();
           // this.ProjectMilestones = new List<ProjectMilestone>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("IdProject")]
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


        [Column("ProjectCode")]
        [DataMember]
        public string ProjectCode
        {
            get
            {
                return projectCode;
            }

            set
            {
                projectCode = value;
                OnPropertyChanged("ProjectCode");
            }
        }

        [Column("ProjectName")]
        [DataMember]
        public string ProjectName
        {
            get
            {
                return projectName;
            }

            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        
        [Column("Description")]
        [DataMember]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        [Column("IdProduct")]
        [ForeignKey("Product")]
        [DataMember]
        public Int64 IdProduct
        {
            get
            {
                return idProduct;
            }

            set
            {
                idProduct = value;
                OnPropertyChanged("IdProduct");
            }
        }

        [Column("IdCustomer")]
        [ForeignKey("Customer")]
        [DataMember]
        public Int32 IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }


        [Column("IdOwner")]
        [ForeignKey("Owner")]
        [DataMember]
        public Int32? IdOwner
        {
            get
            {
                return idOwner;
            }

            set
            {
                idOwner = value;
                OnPropertyChanged("IdOwner");
            }
        }


        [Column("IdProjectPriority")]
        [ForeignKey("ProjectPriority")]
        [DataMember]
        public Int32 IdProjectPriority
        {
            get
            {
                return idProjectPriority;
            }

            set
            {
                idProjectPriority = value;
                OnPropertyChanged("IdProjectPriority");
            }
        }

        [Column("IdProjectType")]
        [ForeignKey("ProjectType")]
        [DataMember]
        public Int32 IdProjectType
        {
            get
            {
                return idProjectType;
            }

            set
            {
                idProjectType = value;
                OnPropertyChanged("IdProjectType");
            }
        }

        [Column("IdProjectStatus")]
        [ForeignKey("ProjectStatus")]
        [DataMember]
        public Int32 IdProjectStatus
        {
            get
            {
                return idProjectStatus;
            }

            set
            {
                idProjectStatus = value;
                OnPropertyChanged("IdProjectStatus");
            }
        }

        
        [Column("IdCategory")]
        [ForeignKey("Category")]
        [DataMember]
        public Int32 IdCategory
        {
            get
            {
                return idCategory;
            }

            set
            {
                idCategory = value;
                OnPropertyChanged("IdCategory");
            }
        }

        [Column("IdGeosStatus")]
        [ForeignKey("GeosStatus")]
        [DataMember]
        public Int64? IdGeosStatus
        {
            get
            {
                return idGeosStatus;
            }

            set
            {
                idGeosStatus = value;
                OnPropertyChanged("IdGeosStatus");
            }
        }

        [Column("ProjectPath")]
        [DataMember]
        public string ProjectPath
        {
            get
            {
                return projectPath;
            }

            set
            {
                projectPath = value;
                OnPropertyChanged("ProjectPath");
            }
        }

        [Column("GeosPath")]
        [DataMember]
        public string GeosPath
        {
            get
            {
                return geosPath;
            }

            set
            {
                geosPath = value;
                OnPropertyChanged("GeosPath");
            }
        }

        [Column("DueDate")]
        [DataMember]
        public DateTime? DueDate
        {
            get
            {
                return dueDate;
            }

            set
            {
                dueDate = value;
                OnPropertyChanged("DueDate");
            }
        }

        [Column("StartDate")]
        [DataMember]
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
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

        [Column("IdCreator")]
        [ForeignKey("Creator")]
        [DataMember]
        public Int32? IdCreator
        {
            get
            {
                return idCreator;
            }

            set
            {
                idCreator = value;
                OnPropertyChanged("IdCreator");
            }
        }

        [Column("IdProjectTeam")]
       // [ForeignKey("ProjectTeam")]
        [DataMember]
        public Int32? IdProjectTeam
        {
            get
            {
                return idProjectTeam;
            }

            set
            {
                idProjectTeam = value;
                OnPropertyChanged("IdProjectTeam");
            }
        }

        [Column("IdOffer")]
        [ForeignKey("Offer")]
        [DataMember]
        public Int64 IdOffer
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

        [Column("IdTeam")]
        [ForeignKey("Team")]
        [DataMember]
        public byte? IdTeam
        {
            get
            {
                return idTeam;
            }

            set
            {
                idTeam = value;
                OnPropertyChanged("IdTeam");
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
        public virtual GeosStatus GeosStatus
        {
            get
            {
                return geosStatus;
            }

            set
            {
                geosStatus = value;
                OnPropertyChanged("GeosStatus");
            }
        }

        [DataMember]
        public virtual List<ProjectTeam> ProjectTeams
        {
            get
            {
                return projectTeams;
            }
            set
            {
                projectTeams = value;
                OnPropertyChanged("ProjectTeams");
            }
        }

        [DataMember]
        public virtual User Creator
        {
            get
            {
                return creator;
            }

            set
            {
                creator = value;
                OnPropertyChanged("Creator");
            }
        }


        [DataMember]
        public virtual Team Team
        {
            get
            {
                return team;
            }

            set
            {
                team = value;
                OnPropertyChanged("Team");
            }
        }

        [DataMember]
        public virtual Product Product
        {
            get
            {
                return product;
            }

            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }

        [DataMember]
        public virtual Customer Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [DataMember]
        public virtual User Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
                OnPropertyChanged("Owner");
            }
        }

        [DataMember]
        public virtual ObservableCollection<ProjectMilestone> ProjectMilestones
        {
            get
            {
                return projectMilestones;
            }

            set
            {
                projectMilestones = value;
                OnPropertyChanged("ProjectMilestones");
            }
        }

        [DataMember]
        public virtual LookupValue ProjectStatus
        {
            get
            {
                return projectStatus;
            }

            set
            {
                projectStatus = value;
                OnPropertyChanged("ProjectStatus");
            }
        }

        [DataMember]
        public virtual LookupValue Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }


        [DataMember]
        public virtual LookupValue ProjectPriority
        {
            get
            {
                return projectPriority;
            }

            set
            {
                projectPriority = value;
                OnPropertyChanged("ProjectPriority");
            }
        }

        [DataMember]
        public virtual LookupValue ProjectType
        {
            get
            {
                return projectType;
            }

            set
            {
                projectType = value;
                OnPropertyChanged("ProjectType");
            }
        }

        [DataMember]
        public virtual List<ProjectTask> ProjectTasks
        {
            get
            {
                return projectTasks;
            }

            set
            {
                projectTasks = value;
                OnPropertyChanged("ProjectTasks");
            }
        }

        [DataMember]
        public virtual List<ProjectAnalysis> ProjectAnalysis
        {
            get
            {
                return projectAnalysis;
            }

            set
            {
                projectAnalysis = value;
                OnPropertyChanged("ProjectAnalysis");
            }
        }

        [DataMember]
        public virtual ObservableCollection<ProjectFollowup> ProjectFollowups
        {
            get
            {
                return projectFollowups;
            }

            set
            {
                projectFollowups = value;
                OnPropertyChanged("ProjectFollowups");
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
