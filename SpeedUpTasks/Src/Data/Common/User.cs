using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using Emdep.Geos.Data.Common.Hrm;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    [Table("users")]
    [DataContract(IsReference = true)]
    public class User : ModelBase
    {
        #region Fields

        List<UserManagerDtl> userManagerDtls;
        Int32 idUser;
        string login;
        string password;
        Int32? idCompany;
        byte? isenabled;
        int? idCreator;
        DateTime? creationDate;
        int? idModifier;
        DateTime? modificationDate;
        string firstName;
        string lastName;
        string companyEmail;
        string companyCode;
        byte? isImpersonate;
        int? idDepartment;
        int? idJobDescription;
        byte[] userProfileImageInBytes;
        byte? isValidated;
        string shortName;
        byte? idUserGender;
        ImageSource ownerImage;
        string phone;

        #endregion

        #region Constructor

        public User()
        {
            this.SiteUserPermissions = new List<SiteUserPermission>();
            this.UserLogs = new List<UserLog>();
            this.UserContacts = new List<UserContact>();
            this.UserLoginEntries = new List<UserLoginEntry>();
            this.UserPermissions = new List<UserPermission>();
            this.GeosVersionBetaTesters = new List<GeosVersionBetaTester>();
            this.UserTeams = new List<UserTeam>();
            this.ProjectTasks = new List<ProjectTask>();
            this.TaskRequestFromAssistances = new List<TaskAssistance>();
            this.TaskRequestToAssistances = new List<TaskAssistance>();
            this.TaskWatchers = new List<TaskWatcher>();
            this.TaskLogs = new List<TaskLog>();
            this.TaskUsers = new List<TaskUser>();
            this.TaskWorkingTimes = new List<TaskWorkingTime>();
            this.TaskComments = new List<TaskComment>();
            this.ProductVersions = new List<ProductVersion>();
            this.ProjectFollowups = new List<ProjectFollowup>();
            this.ProductVersionValidations = new List<ProductVersionValidation>();
        }

        #endregion

        #region Properties

        [Key]
        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [Column("Login")]
        [DataMember]
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        [Column("Password")]
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Column("IdCompany")]
        [ForeignKey("Company")]
        [DataMember]
        public Int32? IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }

        [Column("IsEnabled")]
        [DataMember]
        public byte? IsEnabled
        {
            get { return isenabled; }
            set { isenabled = value; }
        }

        [Column("IdCreator")]
        [DataMember]
        public int? IdCreator
        {
            get { return idCreator; }
            set { idCreator = value; }
        }

        [Column("CreationDate")]
        [DataMember]
        public DateTime? CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [Column("IdModifier")]
        [DataMember]
        public int? IdModifier
        {
            get { return idModifier; }
            set { idModifier = value; }
        }

        [Column("ModificationDate")]
        [DataMember]
        public DateTime? ModificationDate
        {
            get { return modificationDate; }
            set { modificationDate = value; }
        }

        [Column("FirstName")]
        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [Column("LastName")]
        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        [NotMapped]
        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        [NotMapped]
        [DataMember]
        public string ShortName
        {
            get
            {
                if (FirstName != null && FirstName != "")
                {
                    if (FirstName.Length == 1)
                        shortName = (FirstName).ToUpper();
                    else if (FirstName.Length > 1)
                        shortName = (FirstName.Substring(0, 1)).ToUpper();

                }
                if (LastName != null && LastName != "")
                {
                    if (LastName.Length == 1)
                        shortName += (LastName).ToUpper();
                    else if (LastName.Length > 1)
                        shortName += (LastName.Substring(0, 1)).ToUpper();
                }

                return shortName;
            }
            set { }
        }

        [Column("CompanyEmail")]
        [DataMember]
        public string CompanyEmail
        {
            get { return companyEmail; }
            set { companyEmail = value; }
        }

        [Column("CompanyCode")]
        [DataMember]
        public string CompanyCode
        {
            get { return companyCode; }
            set { companyCode = value; }
        }

        [Column("IsImpersonate")]
        [DataMember]
        public byte? IsImpersonate
        {
            get { return isImpersonate; }
            set { isImpersonate = value; }
        }

        [Column("IdDepartment")]
        //[ForeignKey("Department")]
        [DataMember]
        public int? IdDepartment
        {
            get { return idDepartment; }
            set { idDepartment = value; }
        }

        [Column("IdJobDescription")]
        //[ForeignKey("JobDescription")]
        [DataMember]
        public int? IdJobDescription
        {
            get { return idJobDescription; }
            set { idJobDescription = value; }
        }

        [Column("IsValidated")]
        [DataMember]
        public byte? IsValidated
        {
            get { return isValidated; }
            set { isValidated = value; }
        }

        [Column("IdUserGender")]
        [DataMember]
        public byte? IdUserGender
        {
            get { return idUserGender; }
            set { idUserGender = value; }
        }

        [NotMapped]
        [DataMember]
        public byte[] UserProfileImageInBytes
        {
            get { return userProfileImageInBytes; }
            set { userProfileImageInBytes = value; }
        }

        [NotMapped]
        [DataMember]
        public ImageSource OwnerImage
        {
            get
            {
                return ownerImage;
            }

            set
            {
                ownerImage = value;
                OnPropertyChanged("OwnerImage");
            }
        }

        [NotMapped]
        [DataMember]
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }

        [DataMember]
        public virtual Company Company { get; set; }

        //[DataMember]
        //public virtual Department Department { get; set; }

        //[DataMember]
        //public virtual JobDescription JobDescription { get; set; }

        [DataMember]
        public virtual List<UserLog> UserLogs { get; set; }

        [DataMember]
        public virtual List<UserContact> UserContacts { get; set; }

        [DataMember]
        public virtual List<UserLoginEntry> UserLoginEntries { get; set; }

        [DataMember]
        public virtual List<UserPermission> UserPermissions { get; set; }

        [DataMember]
        public virtual List<SiteUserPermission> SiteUserPermissions { get; set; }

        [DataMember]
        public virtual List<GeosVersionBetaTester> GeosVersionBetaTesters { get; set; }

        [DataMember]
        public virtual List<UserTeam> UserTeams { get; set; }

        [DataMember]
        public virtual IList<ProjectTask> ProjectTasks { get; set; }

        [DataMember]
        public virtual IList<TaskAssistance> TaskRequestFromAssistances { get; set; }

        [DataMember]
        public virtual IList<TaskAssistance> TaskRequestToAssistances { get; set; }

        [DataMember]
        public virtual IList<TaskWatcher> TaskWatchers { get; set; }

        [DataMember]
        public virtual IList<TaskUser> TaskUsers { get; set; }

        [DataMember]
        public virtual IList<TaskWorkingTime> TaskWorkingTimes { get; set; }

        [DataMember]
        public virtual IList<TaskComment> TaskComments { get; set; }

        [DataMember]
        public virtual IList<ProductVersion> ProductVersions { get; set; }

        [DataMember]
        public virtual IList<TaskLog> TaskLogs { get; set; }

        [DataMember]
        public virtual IList<ProjectFollowup> ProjectFollowups { get; set; }

        [DataMember]
        public virtual IList<ProductVersionValidation> ProductVersionValidations { get; set; }

        #endregion

        #region Destructor

        ~User()
        {
        }

        #endregion


    }
}
