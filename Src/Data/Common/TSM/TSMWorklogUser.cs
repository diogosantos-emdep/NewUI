using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.TSM
{
    //[GEOS2-8965][pallavi.kale][28.11.2025]
    public class TSMWorklogUser : ModelBase, IDisposable
    {
        #region  Fields

        private Int32 idUser;
        private string login;
        private string firstName;
        private string lastName;
        private string hours;
        private ImageSource image;
        private byte[] profileImageInBytes;
        private Int32 idGender;
        private int seconds;
        private int extraSeconds;
        private People people;
        private string employeeCodeWithIdGender;
     
        #endregion

        #region Constructor
        public TSMWorklogUser()
        {

        }
        #endregion

        #region Properties

        [DataMember]
        public int IdUser
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
        [DataMember]
        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        [DataMember]
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        [DataMember]
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        [DataMember]
        public string FullName
        {
            get { return FirstName + ' ' + LastName; }
            set { }
        }

        [DataMember]
        public string Hours
        {
            get
            {
                return hours;
            }

            set
            {
                hours = value;
                OnPropertyChanged("Hours");
            }
        }

        [DataMember]
        public ImageSource Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }
        [DataMember]
        public byte[] ProfileImageInBytes
        {
            get { return profileImageInBytes; }

            set
            {
                profileImageInBytes = value;
                OnPropertyChanged("ProfileImageInBytes");
            }
        }

        [DataMember]
        public int IdGender
        {
            get
            {
                return idGender;
            }

            set
            {
                idGender = value;
                OnPropertyChanged("IdGender");
            }
        }

        [DataMember]
        public int Seconds
        {
            get
            {
                return seconds;
            }

            set
            {
                seconds = value;
                OnPropertyChanged("Seconds");
            }
        }

        [DataMember]
        public int ExtraSeconds
        {
            get
            {
                return extraSeconds;
            }

            set
            {
                extraSeconds = value;
                OnPropertyChanged("ExtraSeconds");
            }
        }
        [DataMember]
        public  People People
        {
            get { return people; }
            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }
        [DataMember]
        public string EmployeeCodeWithIdGender
        {
            get { return employeeCodeWithIdGender; }

            set
            {
                employeeCodeWithIdGender = value;
                OnPropertyChanged("EmployeeCodeWithIdGender");
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
