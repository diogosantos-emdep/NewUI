using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.OTM
{
    public class POLinkedOffers
    {
        #region Fields
        private string code;
        private Int32 idPORequest;
        private string groupname;
        private string plant;

        #endregion


        #region property
        [DataMember]
        public Int32 IdPORequest
        {
            get
            {
                return idPORequest;
            }
            set
            {
                idPORequest = value;
                OnPropertyChanged("IdPORequest");
            }
        }

        [DataMember]
        public string Groupname
        {
            get
            {
                return groupname;
            }
            set
            {
                groupname = value;
                OnPropertyChanged("Groupname");
            }
        }

        [DataMember]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        [DataMember]
        public string Plant
        {
            get
            {
                return plant;
            }
            set
            {
                plant = value;
                OnPropertyChanged("Plant");
            }
        }

        long idCountry;
        [DataMember]
        public Int64 IdCountry
        {
            get
            {
                return idCountry;
            }
            set
            {
                idCountry = value;
                OnPropertyChanged("IdCountry");
            }
        }
        //[Rahul.Gadhave][GEOS2-9041][Date:02-09-2025] 
        long idCustomerPlant;
        [DataMember]
        public Int64 IdCustomerPlant
        {
            get
            {
                return idCustomerPlant;
            }
            set
            {
                idCustomerPlant = value;
                OnPropertyChanged("IdCustomerPlant");
            }
        }
      //[Rahul.Gadhave][GEOS2-9041][Date:02-09-2025] 
        long idCustomerGroup;
        [DataMember]
        public Int64 IdCustomerGroup
        {
            get
            {
                return idCustomerGroup;
            }
            set
            {
                idCustomerGroup = value;
                OnPropertyChanged("IdCustomerGroup");
            }
        }
        #endregion

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="name">The name.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
