using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [DataContract]
   public class EmployeeTraineeDetails : ModelBase, IDisposable
    {
        #region Fields
        string employeeCode;
        string trainingCode;
        string trainercode;
        string trainerName;
        string courseName;
        string traineeName;
        string description;
        string site;
        DateTime startdate;
        DateTime end_date;
        int courseDuration;
        int courseEvaluation;
        string employeeDepartments;
        string trainerDepartment;
        string companyAlias;
        string jdCodeTrainer;
        string jdcodeTrainee;
        #endregion
        #region Properties
        [Column("JdCodeTrainer")]
        [DataMember]
        public string JdCodeTrainer
        {
            get { return jdCodeTrainer; }
            set
            {
                jdCodeTrainer = value;
                OnPropertyChanged("JdCodeTrainer");
            }
        }
        [Column("JdcodeTrainee")]
        [DataMember]
        public string JdcodeTrainee
        {
            get { return jdcodeTrainee; }
            set
            {
                jdcodeTrainee = value;
                OnPropertyChanged("jdcodeTrainee");
            }
        }

        [Column("EmployeeDepartments")]
        [DataMember]
        public string EmployeeDepartments
        {
            get { return employeeDepartments; }
            set
            {
                employeeDepartments = value;
                OnPropertyChanged("EmployeeDepartments");
            }
        }
        [Column("TrainerDepartment")]
        [DataMember]
        public string TrainerDepartment
        {
            get { return trainerDepartment; }
            set
            {
                trainerDepartment = value;
                OnPropertyChanged("TrainerDepartment");
            }
        }
        [Column("CompanyAlias")]
        [DataMember]
        public string CompanyAlias
        {
            get { return companyAlias; }
            set
            {
                companyAlias = value;
                OnPropertyChanged("CompanyAlias");
            }
        }
        [Column("CourseDuration")]
        [DataMember]
        public int CourseDuration
        {
            get { return courseDuration; }
            set
            {
                courseDuration = value;
                OnPropertyChanged("CourseDuration");
            }
        }
        [Column("Startdate")]
        [DataMember]
        public DateTime Startdate
        {
            get { return startdate; }
            set
            {
                startdate = value;
                OnPropertyChanged("Startdate");
            }
        }
        [Column("End_date")]
        [DataMember]
        public DateTime End_date
        {
            get { return end_date; }
            set
            {
                end_date = value;
                OnPropertyChanged("End_date");
            }
        }



        [Column("TraineeName")]
        [DataMember]
        public string TraineeName
        {
            get { return traineeName; }
            set
            {
                traineeName = value;
                OnPropertyChanged("TraineeName");
            }
        }
        [Column("Description")]
        [DataMember]
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }
        [Column("Site")]
        [DataMember]
        public string Site
        {
            get { return site; }
            set
            {
                site = value;
                OnPropertyChanged("Site");
            }
        }
        [Column("EmployeeCode")]
        [DataMember]
        public string EmployeeCode
        {
            get { return employeeCode; }
            set
            {
                employeeCode = value;
                OnPropertyChanged("EmployeeCode");
            }
        }
        [Column("CourseEvaluation")]
        [DataMember]
        public int CourseEvaluation
        {
            get { return courseEvaluation; }
            set
            {
                courseEvaluation = value;
                OnPropertyChanged("CourseEvaluation");
            }
        }
        [Column("CourseName")]
        [DataMember]
        public string CourseName
        {
            get { return courseName; }
            set
            {
                courseName = value;
                OnPropertyChanged("CourseName");
            }
        }
        [Column("TrainingCode")]
        [DataMember]
        public string TrainingCode
        {
            get { return trainingCode; }
            set
            {
                trainingCode = value;
                OnPropertyChanged("TrainingCode");
            }
        }
        [Column("TrainerName")]
        [DataMember]
        public string TrainerName
        {
            get { return trainerName; }
            set
            {
                trainerName = value;
                OnPropertyChanged("TrainerName");
            }
        }
        [Column("Trainercode")]
        [DataMember]
        public string Trainercode
        {
            get { return trainercode; }
            set
            {
                trainercode = value;
                OnPropertyChanged("Trainercode");
            }
        }
        #endregion
        #region Constructor

        public EmployeeTraineeDetails()
        {
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
