using Emdep.Geos.Data.Common.Epc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class POAnalyzerOTTemplate : ModelBase, IDisposable
    {
        #region Fields
        long idOTRequestTemplate;
       
        string templateName;
        int idCustomer;
        string customer;
        int idRegion;
        string region;
        int idCountries;
        string countries;
        int idplant;
        string plant;
        //List<LookupValue> valueList;
        List<TextFileTemplateValue> txtValue;
        //List<LocationFileTemplateValue> locationValue;
        List<ExcelFileTemplateValue> excelRangeValue; //[pramod.misal][GEOS2-6735][23.01.2025]
        #endregion

        #region Constructor
        public POAnalyzerOTTemplate()
        {

        }
        #endregion

        #region Properties
        [DataMember]
        public Int64 IdOTRequestTemplate
        {
            get
            {
                return idOTRequestTemplate;
            }

            set
            {
                idOTRequestTemplate = value;
                OnPropertyChanged("IdOTRequestTemplate");
            }
        }


        [DataMember]
        public string TemplateName
        {
            get
            {
                return templateName;
            }

            set
            {
                templateName = value;
                OnPropertyChanged("TemplateName");
            }
        }



        [DataMember]
        public List<TextFileTemplateValue> TextValue
        {
            get
            {
                return txtValue;
            }

            set
            {
                txtValue = value;
                OnPropertyChanged("TextValue");
            }
        }

        //[DataMember]
        //public List<LocationFileTemplateValue> LocationValue
        //{
        //    get
        //    {
        //        return locationValue;
        //    }

        //    set
        //    {
        //        locationValue = value;
        //        OnPropertyChanged("LocationValue");
        //    }
        //}
        [DataMember]
        public int IdCustomer
        {
            get { return idCustomer; }
            set { idCustomer = value; OnPropertyChanged("IdCustomer"); }
        }
        [DataMember]
        public string Customer
        {
            get { return customer; }
            set { customer = value; OnPropertyChanged("Customer"); }
        }
        [DataMember]
        public int IdRegion
        {
            get { return idRegion; }
            set { idRegion = value; OnPropertyChanged("IdRegion"); }
        }
        [DataMember]
        public string Region
        {
            get { return region; }
            set { region = value; OnPropertyChanged("Region"); }
        }
        [DataMember]
        public int IdCountries
        {
            get { return idCountries; }
            set { idCountries = value; OnPropertyChanged("IdCountries"); }
        }
        [DataMember]
        public string Countries
        {
            get { return countries; }
            set { countries = value; OnPropertyChanged("Countries"); }
        }
        [DataMember]
        public int IdPlant
        {
            get { return idplant; }
            set { idplant = value; OnPropertyChanged("IdPlant"); }
        }
        [DataMember]
        public string Plant
        {
            get { return plant; }
            set { plant = value; OnPropertyChanged("Plant"); }
        }
        [DataMember]
        public List<ExcelFileTemplateValue> ExcelRangeValue
        {
            get
            {
                return excelRangeValue;
            }

            set
            {
                excelRangeValue = value;
                OnPropertyChanged("ExcelRangeValue");
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
