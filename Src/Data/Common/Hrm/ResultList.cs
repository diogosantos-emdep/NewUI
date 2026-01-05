using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.Hrm
{
    [Table("resultlist")]
    [DataContract]
    public class ResultList : ModelBase, IDisposable
    {
        #region Fields

        Int32 idResult;
        string code;
        string name;
        string description;
        #endregion

        #region Properties

        [Key]
        [Column("IdResult")]
        [DataMember]
        public Int32 IdResult
        {
            get { return idResult; }
            set
            {
                idResult = value;
                OnPropertyChanged("IdResult");
            }
        }

        [Column("Code")]
        [DataMember]
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [Column("Name")]
        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
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
        #endregion

        #region Constructor

        public ResultList()
        {
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
