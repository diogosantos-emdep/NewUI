using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{

    [DataContract]
    public class UserShortDetail : ModelBase, IDisposable
    {
        #region Fields
        Int32 idUser;
        string login;
        string userName;
        byte? idUserGender;
        byte[] userImageInBytes;
        string companyCode;
        string companyEmail;
        ImageSource userImage;
        string phoneNo;
        #endregion

        #region Constructor
        public UserShortDetail()
        {

        }
        #endregion

        #region Properties


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
        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        [DataMember]
        public byte? IdUserGender
        {
            get { return idUserGender; }
            set
            {
                idUserGender = value;
                OnPropertyChanged("IdUserGender");
            }
        }


        [DataMember]
        public byte[] UserImageInBytes
        {
            get { return userImageInBytes; }
            set
            {
                userImageInBytes = value;
                OnPropertyChanged("UserImageInBytes");
            }
        }

        [DataMember]
        public string CompanyCode
        {
            get { return companyCode; }
            set
            {
                companyCode = value;
                OnPropertyChanged("CompanyCode");
            }
        }


        [DataMember]
        public string CompanyEmail
        {
            get { return companyEmail; }
            set
            {
                companyEmail = value;
                OnPropertyChanged("CompanyEmail");
            }
        }

        [DataMember]
        public ImageSource UserImage
        {
            get { return userImage; }
            set
            {
                userImage = value;
                OnPropertyChanged("UserImage");
            }
        }

        [DataMember]
        public string PhoneNo
        {
            get { return phoneNo; }
            set
            {
                phoneNo = value;
                OnPropertyChanged("PhoneNo");
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
