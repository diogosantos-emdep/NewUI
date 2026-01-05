using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emdep.Geos.Data.Common.SRM
{
    public class Article_Supplier_Contacts : ModelBase, IDisposable
    {

        public string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                ShortName = FormatTabName(name);
                OnPropertyChanged("Name");
            }
        }

        public string shortName;
        public string ShortName
        {
            get { return shortName; }
            set
            {
                shortName = value;
               
                OnPropertyChanged("ShortName");

            }
        }

        string eMDEPCode;
        public string EMDEPCode
        {
            get { return eMDEPCode; }
            set
            {
                eMDEPCode = value;
                OnPropertyChanged("EMDEPCode");

            }
        }
        string country;
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }
        string address;
        public string Address
        {
            get { return address; }
            set {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        string postCode;
        public string PostCode
        {
            get { return postCode; }
            set {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }

        string region;
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        string city;
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }

        }

        Int64 idArticleSupplier;
        public Int64 IdArticleSupplier
        {
            get { return idArticleSupplier; }
            set
            {
                idArticleSupplier = value;
                OnPropertyChanged("IdArticleSupplier");
            }
        }
        List<ArticleSupplier> supplierList;
        public List<ArticleSupplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged("SupplierList");
            }

        }

        ArticleSupplier selectedSupplierList;
        public ArticleSupplier SelectedSupplierList
        {
            get { return selectedSupplierList; }
            set
            {
                selectedSupplierList = value;
                OnPropertyChanged("SupplierList");
            }

        }

        public string updatedName=string.Empty;
        public string UpdatedName
        {
            get { return updatedName; }
            set
            {
                updatedName = value;
                OnPropertyChanged("UpdatedName");
            }
        }

        ArticleSupplier oldSelectedSupplierList;
        public ArticleSupplier OldSelectedSupplierList
        {
            get { return oldSelectedSupplierList; }
            set
            {
                oldSelectedSupplierList = value;
                OnPropertyChanged("OldSelectedSupplierList");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            Article_Supplier_Contacts article_Supplier_Contacts = (Article_Supplier_Contacts)this.MemberwiseClone();
            return article_Supplier_Contacts;
        }

        public string FormatTabName(string name)
        {
            if (name.Length > 15)
            {
                // Truncate the name to 15 characters
                return name.Substring(0, 15);
            }
            return name;
        }
    }

}
