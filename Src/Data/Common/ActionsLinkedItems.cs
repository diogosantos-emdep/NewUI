using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Emdep.Geos.Data.Common.Epc;
using System.Windows.Media;

namespace Emdep.Geos.Data.Common
{
    public class ActionsLinkedItems : ModelBase, IDisposable
    {
        #region Fields
        Int64 idActionLinkedItem;
        Int64 idActionPlanItem;
        Int32? idLinkedItemType;
        Int32? idCustomer;
        Int32? idSite;
        LookupValue linkedItemType;
        Customer customer;
        Company company;
        string name;
        bool isUpdated;
        bool isDeleted;
        Int64? idCarProject;
        Int32? idPerson;
        CarProject carProject;
        People people;
        ImageSource actionLinkedItemImage;
        bool isVisible = true;
        Int64? idOffer;
        Int32? idEmdepSite;
        Offer offer;
        Int32? idCompetitor;
        Competitor competitor;
        DateTime creationDate;
        #endregion

        #region Constructor
        public ActionsLinkedItems()
        {

        }
        #endregion

        #region Properties
        [Key]
        [Column("IdActivityLinkedItem")]
        [DataMember]
        public Int64 IdActionsLinkedItem
        {
            get
            {
                return idActionLinkedItem;
            }

            set
            {
                idActionLinkedItem = value;
                OnPropertyChanged("IdActivityLinkedItem");
            }
        }

        [Column("IdActivity")]
        [DataMember]
        public Int64 IdActionPlanItem
        {
            get
            {
                return idActionPlanItem;
            }

            set
            {
                idActionPlanItem = value;
                OnPropertyChanged("IdActivity");
            }
        }

        [Column("IdLinkedItemType")]
        [DataMember]
        public Int32? IdLinkedItemType
        {
            get
            {
                return idLinkedItemType;
            }

            set
            {
                idLinkedItemType = value;
                OnPropertyChanged("IdLinkedItemType");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdCustomer
        {
            get
            {
                return idCustomer;
            }

            set
            {
                idCustomer = value;
                OnPropertyChanged("IdCustomer");
            }
        }

        [Column("IdSite")]
        [DataMember]
        public Int32? IdSite
        {
            get
            {
                return idSite;
            }

            set
            {
                idSite = value;
                OnPropertyChanged("IdSite");
            }
        }

        [NotMapped]
        [DataMember]
        public LookupValue LinkedItemType
        {
            get
            {
                return linkedItemType;
            }

            set
            {
                linkedItemType = value;
                OnPropertyChanged("LinkedItemType");
            }
        }

        [NotMapped]
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

        [NotMapped]
        [DataMember]
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }

            set
            {
                isDeleted = value;
                OnPropertyChanged("IsDeleted");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsUpdated
        {
            get
            {
                return isUpdated;
            }

            set
            {
                isUpdated = value;
                OnPropertyChanged("IsUpdated");
            }
        }


        [NotMapped]
        [DataMember]
        public Customer Customer
        {
            get
            {
                return customer;
            }

            set
            {
                customer = value;
                OnPropertyChanged("Customer");
            }
        }

        [NotMapped]
        [DataMember]
        public Company Company
        {
            get
            {
                return company;
            }

            set
            {
                company = value;
                OnPropertyChanged("Company");
            }
        }


        [NotMapped]
        [DataMember]
        public Int32? IdPerson
        {
            get
            {
                return idPerson;
            }

            set
            {
                idPerson = value;
                OnPropertyChanged("IdPerson");
            }
        }


        [NotMapped]
        [DataMember]
        public Int64? IdCarProject
        {
            get
            {
                return idCarProject;
            }

            set
            {
                idCarProject = value;
                OnPropertyChanged("IdCarProject");
            }
        }


        [NotMapped]
        [DataMember]
        public People People
        {
            get
            {
                return people;
            }

            set
            {
                people = value;
                OnPropertyChanged("People");
            }
        }


        [NotMapped]
        [DataMember]
        public CarProject CarProject
        {
            get
            {
                return carProject;
            }

            set
            {
                carProject = value;
                OnPropertyChanged("CarProject");
            }
        }

        [NotMapped]
        [DataMember]
        public ImageSource ActionLinkedItemImage
        {
            get
            {
                return actionLinkedItemImage;
            }

            set
            {
                actionLinkedItemImage = value;
                OnPropertyChanged("ActionLinkedItemImage");
            }
        }

        [NotMapped]
        [DataMember]
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        [Column("IdOffer")]
        [DataMember]
        public Int64? IdOffer
        {
            get { return idOffer; }
            set
            {
                idOffer = value;
                OnPropertyChanged("IdOffer");
            }
        }

        [Column("IdEmdepSite")]
        [DataMember]
        public Int32? IdEmdepSite
        {
            get { return idEmdepSite; }
            set { idEmdepSite = value; }
        }

        [NotMapped]
        [DataMember]
        public Offer Offer
        {
            get { return offer; }
            set
            {
                offer = value;
                OnPropertyChanged("Offer");
            }
        }

        [NotMapped]
        [DataMember]
        public Int32? IdCompetitor
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

        [NotMapped]
        [DataMember]
        public Competitor Competitor
        {
            get
            {
                return competitor;
            }

            set
            {
                competitor = value;
                OnPropertyChanged("Competitor");
            }
        }


        [NotMapped]
        [DataMember]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
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
