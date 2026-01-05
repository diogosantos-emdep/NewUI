using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class ValidationViewModel : NavigationViewModelBase, IDisposable
    {
        #region Services

        IEpcService epcControl;

        #endregion

        #region Commands

        public ICommand SelectedProductCommand { get; set; }
        public ICommand SelectedProductVersionCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<TileBarItemsHelper> TileCollectionProducts { get; set; }
        public ObservableCollection<TileBarItemsHelper> TileCollectionVersions { get; set; }

        private ObservableCollection<Product> productList = new ObservableCollection<Product>();
        public ObservableCollection<Product> ProductList
        {
            get { return productList; }
            set
            {
                SetProperty(ref productList, value, () => ProductList);
            }
        }

        private TileBarItemsHelper selectedProduct;
        public TileBarItemsHelper SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                //if (this.selectedProduct != value)
                //{
                //    this.selectedProduct = value;
                //    this.OnPropertyChanged("SelectedProduct");
                //}
                SetProperty(ref selectedProduct, value, () => SelectedProduct);
            }
        }

        private TileBarItemsHelper selectedVersion;
        public TileBarItemsHelper SelectedVersion
        {
            get { return selectedVersion; }
            set
            {
                if (this.selectedVersion != value)
                {
                    this.selectedVersion = value;
                    this.OnPropertyChanged("SelectedVersion");
                }
                // SetProperty(ref selectedVersion, value, () => SelectedVersion);
            }
        }

        private ProductVersion productVersionData;

        public ProductVersion ProductVersionData
        {
            get { return productVersionData; }
            set
            {
                SetProperty(ref productVersionData, value, () => ProductVersionData);
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region Constructor
        public ValidationViewModel()
        {
            epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
            var list = new ObservableCollection<Product>(epcControl.GetAllProducts().AsEnumerable());

            foreach (var item in list)
            {
                if (item.Childrens == null)
                {
                    ProductList.Add(item);
                }
            }

            TileCollectionProducts = new ObservableCollection<TileBarItemsHelper>();
            TileCollectionVersions = new ObservableCollection<TileBarItemsHelper>();

            foreach (var item2 in ProductList)
            {
                TileCollectionProducts.Add(new TileBarItemsHelper()
                {
                    Tag = item2,
                    Caption = item2.ProductName,
                    BackColor = item2.HtmlColor,
                    Height = 40
                });
            }

            SelectedProductCommand = new DelegateCommand<object>(SelectProductAction);
            SelectedProductVersionCommand = new DelegateCommand<object>(SelectedProductVersionAction);
        }

        #endregion

        #region Methods
        public void SelectProductAction(object obj)
        {
            TileCollectionVersions.Clear();
            Product product = (Product)SelectedProduct.Tag;

            foreach (var item3 in product.ProductVersions)
            {
                TileCollectionVersions.Add(new TileBarItemsHelper()
                {
                    Caption = "v " + item3.ProductVersionNumber,
                    Tag = item3,
                    Height = 40,
                    BackColor = "#7B68EE"
                });
            }

        }

        public void SelectedProductVersionAction(object obj)
        {
            if (SelectedVersion != null)
            {
                ProductVersionData = (ProductVersion)SelectedVersion.Tag;
            }
            //ProductVersionData.ProductVersionItems[]. ProductRoadmap.Title
            //ProductVersionData.ProductVersionItems[].ProductRoadmap.
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
