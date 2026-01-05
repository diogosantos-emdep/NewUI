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
        Int64 columnNo;
        int idGroup;
        string groupName;
        int quantity;
        int idDetectionType;
        string quantityInString;
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


        [DataMember]
        public Int64 ColumnNo
        {
            get { return columnNo; }
            set
            {
                columnNo = value;
                OnPropertyChanged("ColumnNo");
            }
        }
        [DataMember]
        public int IdGroup
        {
            get { return idGroup; }
            set { idGroup = value; OnPropertyChanged("IdGroup"); }
        }

        [DataMember]
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; OnPropertyChanged("GroupName"); }
        }

        [DataMember]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged("Quantity"); }
        }

        [DataMember]
        public string QuantityInString
        {
            get { return quantityInString; }
            set { quantityInString = value; OnPropertyChanged("QuantityInString"); }
        }

        [DataMember]
        public int IdDetectionType
        {
            get { return idDetectionType; }
            set { idDetectionType = value; OnPropertyChanged("IdDetectionType"); }
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
