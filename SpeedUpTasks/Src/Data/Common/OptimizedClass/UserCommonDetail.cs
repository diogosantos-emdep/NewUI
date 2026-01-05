using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common.OptimizedClass
{
    [DataContract]
    public class UserCommonDetail
    {
        #region Fields
        Int32 idUser;
        string login;
        byte[] imageBytes;
        ImageSource ownerImage;
        byte? idPersonGender;
        #endregion

        #region Properties
        [DataMember]
        public Int32 IdUser
        {
            get
            {
                return idUser;
            }

            set
            {
                idUser = value;
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
            }
        }

        [DataMember]
        public byte[] ImageBytes
        {
            get
            {
                return imageBytes;
            }

            set
            {
                imageBytes = value;
            }
        }

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
            }
        }

        [DataMember]
        public byte? IdPersonGender
        {
            get
            {
                return idPersonGender;
            }

            set
            {
                idPersonGender = value;
            }
        }
        #endregion
    }
}
