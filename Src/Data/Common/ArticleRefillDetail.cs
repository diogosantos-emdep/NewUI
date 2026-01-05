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
    [Table("ArticleRefillDetail")]
    [DataContract]
    //[sudhir.jangra][GEOS2-3959][07/11/2022]
    public class ArticleRefillDetail : ModelBase, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #region Declaration
        string reference;
        string description;
        string category;
        double numberOfRefills;
        DateTime latestRefillDate;
        int idArticle;
        Warehouses warehouse;
        #endregion

        #region Properties
        [Key]
        [Column("IdArticle")]
        [DataMember]
        public int IdArticle
        {
            get { return idArticle; }
            set
            {
                idArticle = value;
                OnPropertyChanged("IdArticle");
            }
        }
        [Key]
        [Column("Reference")]
        [DataMember]
        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged("Reference");
            }
        }
        [Key]
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
        [Key]
        [Column("ArticleCategory")]
        [DataMember]
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        [Key]
        [Column("NoOfRefill")]
        [DataMember]
        public double NumberOfRefills
        {
            get { return numberOfRefills; }
            set
            {
                numberOfRefills = value;
                OnPropertyChanged("NumberOfRefills");
            }
        }
        [Key]
        [Column("LatestRefillDate")]
        [DataMember]
        public DateTime LatestRefillDate
        {
            get { return latestRefillDate; }
            set
            {
                latestRefillDate = value;
                OnPropertyChanged("LatestRefillDate");
            }
        }


        [NotMapped]
        [DataMember]
        public Warehouses Warehouses
        {
            get
            {
                return warehouse;
            }

            set
            {
                warehouse = value;
                OnPropertyChanged("Warehouses");
            }
        }
        #endregion
    }
}
