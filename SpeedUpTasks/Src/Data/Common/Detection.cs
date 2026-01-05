using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common
{
    [Table("detections")]
    [DataContract]
    public class Detection : ModelBase, IDisposable
    {
        #region Fields

        UInt32 idDetection;
        string name;
        string nameToShow;
        string description;

        CpType cpType;

        #endregion

        #region Constructor

        public Detection()
        {

        }

        #endregion

        #region Properties

        [Key]
        [Column("IdDetection")]
        [DataMember]
        public uint IdDetection
        {
            get { return idDetection; }
            set
            {
                idDetection = value;
                OnPropertyChanged("IdDetection");
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

        [Column("NameToShow")]
        [DataMember]
        public string NameToShow
        {
            get { return nameToShow; }
            set
            {
                nameToShow = value;
                OnPropertyChanged("NameToShow");
            }
        }

        [Column("CpType")]
        [DataMember]
        public CpType CpType
        {
            get { return cpType; }
            set
            {
                cpType = value;
                OnPropertyChanged("CpType");
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
