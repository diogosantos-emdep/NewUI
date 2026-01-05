using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Emdep.Geos.Data.Common
{
    [Table("user_contacts")]
    [DataContract]
   public class UserContact
   {
       #region Fields
       Int32 idContact;
        string contactNumber;
        string address;
        Int32? idUser;
       #endregion

       #region Properties
        [Key]
       [Column("IdContact")]
       [DataMember]
        public Int32 IdContact
       {
           get { return idContact; }
           set { idContact = value; }
       }

       [Column("ContactNumber")]
       [DataMember]
       public string ContactNumber
       {
           get { return contactNumber; }
           set { contactNumber = value; }
       }

       [Column("Address")]
       [DataMember]
       public string Address
       {
           get { return address; }
           set { address = value; }
       }

       [DataMember]
       public Int32? IdUser
       {
           get { return idUser; }
           set { idUser = value; }
       }

       [ForeignKey("IdUser")]
       [DataMember]
       public virtual User Users { get; set; }
        #endregion
   }
}
