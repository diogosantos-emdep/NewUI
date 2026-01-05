using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SAM
{
    [DataContract]
   public class TechnicalSpecifications : ModelBase, IDisposable
    {
        #region Fields

        private string projectName;
        private string category;
        private string reference;
        private string model;
        private string oem;
        private string voltage;
        private string amperage;
        private string frequency;
        private string pressure;
        private List<Detection> properties;
        private List<Image> structurePhotos;
        private int idOem;
        #endregion Properties

        #region Properties
        [DataMember]
        public int IdOem
        {
            get { return idOem; }
            set { idOem = value; OnPropertyChanged("IdOem"); }
        }

        [DataMember]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; OnPropertyChanged("ProjectName"); }
        }

        [DataMember]
        public string Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged("Category"); }
        }

        [DataMember]
        public string Reference
        {
            get { return reference; }
            set { reference = value; OnPropertyChanged("Reference"); }
        }

        [DataMember]
        public string Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged("Model"); }
        }
        [DataMember]
        public string Oem
        {
            get { return oem; }
            set { oem = value; OnPropertyChanged("Oem"); }
        }
        
        [DataMember]
        public string Voltage
        {
            get { return voltage; }
            set { voltage = value; OnPropertyChanged("Voltage"); }
        }

        [DataMember]
        public string Amperage
        {
            get { return amperage; }
            set { amperage = value; OnPropertyChanged("Amperage"); }
        }

        [DataMember]
        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; OnPropertyChanged("Frequency"); }
        }

        [DataMember]
        public string Pressure
        {
            get { return pressure; }
            set { pressure = value; OnPropertyChanged("Pressure"); }
        }

        [DataMember]
        public List<Detection> Properties
        {
            get { return properties; }
            set { properties = value; OnPropertyChanged("Properties"); }
        }

        [DataMember]
        public List<Image> StructurePhotos
        {
            get { return structurePhotos; }
            set { structurePhotos = value; OnPropertyChanged("StructurePhotos"); }
        }
        #endregion

        #region Constructor
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
