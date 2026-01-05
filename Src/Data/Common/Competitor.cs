using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Emdep.Geos.Data.Common
{
    [Table("competitors")]
    [DataContract]
   public class Competitor : ModelBase, IDisposable
    {

        #region Fields
        Int32 idCompetitor;
        string name;
        #endregion

        #region Constructor
        public Competitor()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdCompetitor")]
        [DataMember]
        public Int32 IdCompetitor
        {
            get
            {
                return idCompetitor;
            }

            set
            {
                idCompetitor = value;
                OnPropertyChanged("IdCompetitor");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
                OnPropertyChanged("Name");
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
