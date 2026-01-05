using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{
    //[rushikesh.gaikwad][GEOS2-5801][12.09.2024]
    public class UserRoles: ModelBase
    {
        #region Fields
        string name;
        Int32 idrole;
        Int32 idmodule;
        Int32 iduser;

        #endregion

        #region Properties

        [Column("IdUser")]
        [DataMember]
        public Int32 IdUser
        {
            get { return iduser; }
            set { iduser = value; }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("IdRole")]
        [DataMember]
        public Int32 IdRole
        {
            get { return idrole; }
            set { idrole = value; }
        }

        [Column("IdUser")]
        [DataMember]
        public Int32 IdModule
        {
            get { return idmodule; }
            set { idmodule = value; }
        }


        #endregion
    }
}
