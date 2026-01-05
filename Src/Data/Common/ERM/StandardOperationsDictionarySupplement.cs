using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Emdep.Geos.Data.Common.PCM;

namespace Emdep.Geos.Data.Common.ERM
{
    /// <summary>
    /// StandardOperationsDictionarySupplement class is created 
    /// </summary>
    [DataContract]
    public class StandardOperationsDictionarySupplement : ModelBase, IDisposable
    {
        #region Declaration
        UInt64 idStandardOperationsDictionary;
        UInt64 idStandardOperationsDictionarySupplement;
        Int32 idLookupValue;
        string category;
        string name;
        Int32 idPlant;
        float? plantValue;
        string plantName;
        #endregion

        #region Properties

        [DataMember]
        public UInt64 IdStandardOperationsDictionary
        {
            get
            {
                return idStandardOperationsDictionary;
            }

            set
            {
                idStandardOperationsDictionary = value;
                OnPropertyChanged("IdStandardOperationsDictionary");
            }
        }
        [Key]
        [DataMember]
        public UInt64 IdStandardOperationsDictionarySupplement
        {
            get
            {
                return idStandardOperationsDictionarySupplement;
            }

            set
            {
                idStandardOperationsDictionarySupplement = value;
                OnPropertyChanged("idStandardOperationsDictionarySupplement");
            }
        }
        [DataMember]
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

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

        [DataMember]
        public Int32 IdPlant
        {
            get
            {
                return idPlant;
            }

            set
            {
                idPlant = value;
                OnPropertyChanged("IdPlant");
            }
        }

        [DataMember]
        public Int32 IdLookupValue
        {
            get
            {
                return idLookupValue;
            }

            set
            {
                idLookupValue = value;
                OnPropertyChanged("idLookupValue");
            }
        }

        [DataMember]
        public float? PlantValue
        {
            get
            {
                return plantValue;
            }
            set
            {
                plantValue = value;
                OnPropertyChanged("PlantValue");
            }

        }

        [DataMember]
        public string PlantName
        {
            get
            {
                return plantName;
            }
            set
            {
                plantName = value;
                OnPropertyChanged("PlantName");
            }

        }

        #endregion

        #region Constructor

        public StandardOperationsDictionarySupplement()
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
            var newSODClone = (StandardOperationsDictionarySupplement)this.MemberwiseClone();

           
            return newSODClone;
        }

        #endregion
    }
}
