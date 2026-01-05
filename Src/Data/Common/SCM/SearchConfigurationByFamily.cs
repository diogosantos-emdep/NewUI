using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SCM
{//[Sudhir.Jangra][GEOS2-4971]
 //  [Table("scm_search_configuration_by_family")]
    [DataContract]
    public class SearchConfigurationByFamily : ModelBase, IDisposable
    {
        #region Declarations
        private uint idSearchConfiguration;
        private uint idFamily;
        private double numberOfPages;
        private string referencePages;
        private string colorPages;
        private string sizePages;
        private string componentsPages;
        private string waysPages;
        #endregion

        #region Properties
        [DataMember]
        public uint IdSearchConfiguration
        {
            get { return idSearchConfiguration; }
            set
            {
                idSearchConfiguration = value;
                OnPropertyChanged("IdSearchConfiguration");
            }
        }
        [DataMember]
        public uint IdFamily
        {
            get { return idFamily; }
            set
            {
                idFamily = value;
                OnPropertyChanged("IdFamily");
            }
        }
        [DataMember]
        public double NumberOfPages
        {
            get { return numberOfPages; }
            set
            {
                numberOfPages = value;
                OnPropertyChanged("NumberOfPages");
            }
        }

        [DataMember]
        public string ReferencePages
        {
            get { return referencePages; }
            set
            {
                referencePages = value;
                OnPropertyChanged("ReferencePages");
            }
        }
        [DataMember]
        public string ColorPages
        {
            get { return colorPages; }
            set
            {
                colorPages = value;
                OnPropertyChanged("ColorPages");
            }
        }
        [DataMember]
        public string SizePages
        {
            get { return sizePages; }
            set
            {
                sizePages = value;
                OnPropertyChanged("SizePages");
            }
        }
        [DataMember]
        public string ComponentsPages
        {
            get { return componentsPages; }
            set
            {
                componentsPages = value;
                OnPropertyChanged("ComponentsPages");
            }
        }
        [DataMember]
        public string WaysPages
        {
            get { return waysPages; }
            set
            {
                waysPages = value;
                OnPropertyChanged("WaysPages");
            }
        }
        #endregion

        #region Constructor
        public SearchConfigurationByFamily()
        {

        }
        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
