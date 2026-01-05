using DevExpress.Mvvm;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Adapters.Logging;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Emdep.Geos.Modules.Epc.ViewModels
{
    public class MainViewModel : NavigationViewModelBase
    {
        #region Services
        IEpcService epcControl;
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }

        #endregion

        #region Collections
        public ObservableCollection<TileBarItemsHelper> TileCollection { get; set; }
        public ObservableCollection<Product> ListProducts { get; set; }

        public TileBarItemsHelper ProductTilebar = new TileBarItemsHelper();
        #endregion

        #region Methods
        /// <summary>
        /// Methods for creating product tiles' children hirarchy dynamically.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="tilebar"></param>
        public void FillChildProduct(Product product, TileBarItemsHelper tilebar)
        {
            foreach (Product item in product.Childrens)
            {
                TileBarItemsHelper citem = new TileBarItemsHelper()
                {
                    Caption = item.ProductName,
                    BackColor = item.HtmlColor,
                    
                    Height = 40,                  
                    NavigateCommand = new DelegateCommand(() => {Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProductView",item, this);})
                };
                if (item.Childrens!=null)
                FillChildProduct(item, citem);
                tilebar.Children.Add(citem);
            }
        }
        /// <summary>
        /// Method for fill product tiles dynamically.
        /// </summary>
        public void Fill( )
        {
            ProductTilebar = new TileBarItemsHelper();
            foreach (var item in ListProducts)
            {
                TileBarItemsHelper pitem = new TileBarItemsHelper()
                  {
                      Caption = item.ProductName,
                      BackColor = item.HtmlColor,
                      Height = 40,
                      NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProductView", item, this); })
                  };                
                ProductTilebar.Children.Add(pitem);
                FillChildProduct(item, pitem);
            }
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            #region LogCreation
            // To create Log file in specified path under system
            string logfilepath = GeosApplication.Instance.ApplicationLogFilePath;
            if (!File.Exists(GeosApplication.Instance.ApplicationLogFilePath))
            {
                if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                    Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);
                File.Copy(Directory.GetCurrentDirectory() + @"\" + GeosApplication.Instance.ApplicationLogFileName, logfilepath);
            }
            FileInfo file = new FileInfo(GeosApplication.Instance.ApplicationLogFilePath);

            GeosApplication.Instance.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
            #endregion

            GeosApplication.Instance.Logger.Log("EPC MainView Model Start...", category: Category.Info, priority: Priority.Low);

            GeosApplication.Instance.Logger.Log("EPC MainView Model initializing ....", category: Category.Info, priority: Priority.Low);

            ListProducts = new ObservableCollection<Product>();
            try
            {
                string p = GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP);
                string s = GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort).ToString();

                epcControl = new EpcServiceController(GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderIP), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServiceProviderPort), GeosApplication.Instance.GetApplicationSetting(ApplicationSetting.ServicePath));
                GeosApplication.Instance.Logger.Log(" EPC getting  product. ", category: Category.Info, priority: Priority.Low);
                ListProducts = new ObservableCollection<Product>(epcControl.GetProducts(null,true).AsEnumerable());
                GeosApplication.Instance.Logger.Log(" EPC getting  product succesfully. ", category: Category.Info, priority: Priority.Low);
            }
            catch (Emdep.Geos.Services.Contracts.ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error On GetProducts Method", category: Category.Info, priority: Priority.Low);

            }
     

            Fill();
            
            /// This is for creating Main Tiles Statically
            TileCollection = new ObservableCollection<TileBarItemsHelper>();
            TileCollection.Add(new TileBarItemsHelper()
            {
                Caption = "BIDashBoard",
                // Caption = System.Windows.Application.Current.FindResource("MainViewTileBIDashboard").ToString(),
                BackColor = "#00879C",
                GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/BIDashboard.png", 
                Children = new ObservableCollection<TileBarItemsHelper>(){
                new TileBarItemsHelper() 
                { 
                    Caption = "Strategic Map", 
                    BackColor="#566573",
                    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/StrategicMap.png", 
                   // Height = 40,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.StrategicMapView", null, this); }) 
                },    
                new TileBarItemsHelper()
                {
                    Caption = "Projects Board",
                    BackColor="#566573",
                    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/ProjectBoard.png",
                   // Height = 40,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProjectBoardView", null, this); })

                },
                new TileBarItemsHelper() 
                { 
                    Caption = "Project Scheduler", 
                    BackColor="#566573",
                    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/ProjectScheduler.png",
                   // Height = 40,               
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.ProjectSchedulerView", null, this); }) 
                },
                }
            });

            TileCollection.Add(new TileBarItemsHelper()
            {
                Caption = "MyWork",
               // BackColor = "#BABAAB",
                BackColor = "#0073C4",
              
                GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/MyWork.png",
                Children = new ObservableCollection<TileBarItemsHelper>() { 
                new TileBarItemsHelper()
                {
                    Caption = "Task",
                    BackColor = "#566573",
                    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/Tasks.png",
                   // Height = 40,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.TaskView", this, this); })
                },
                new TileBarItemsHelper()
                {
                    Caption = "Watcher",
                    BackColor = "#566573",
                    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/Watcher.png",
                   // Height = 40,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.WatcherView", null, this); })
                },  
                new TileBarItemsHelper()
                {
                    Caption = "Request Assistance",
                    BackColor = "#566573",
                    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/RequestAssistance.png",
                   // Height = 40,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.RequestAssistanceView", null, this); })
                },
                // new TileBarItemsHelper()
                //{
                //    Caption = "Milestones",
                //    BackColor = "#566573",
                //    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/Milestone.png",
                //   // Height = 40,
                //    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.MilestonesView", null, this); })
                //},
                new TileBarItemsHelper()
                {
                    Caption = "Calendar",
                    BackColor = "#566573",
                     GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/Calendar.png",
                   // Height = 40,
                    NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.CalendarView", null, this); })
                },
                }});

            TileCollection.Add(new TileBarItemsHelper()
            {
                Caption = "Product",
                BackColor = "#D4AC0D",
                GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/Products.png",
                Children = ProductTilebar.Children
             });
            TileCollection.Add(new TileBarItemsHelper()
            {
                Caption = "Validation",
                //BackColor = "#00BFFF",
                BackColor = "#3E7038",
                
                GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/Validation.png",
                NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.ValidationView", null, this); })


            });
          
            GeosApplication.Instance.Logger.Log("EPC MainView Model initialize successfully", category: Category.Info, priority: Priority.Low);

            //TileCollection.Add(new TileBarItemsHelper()
            //{
            //    Caption = "Working Order",               
            //    BackColor = "#827B60",
            //    GlyphUri = @"/Emdep.Geos.Modules.Epc;component/Images/WorkOrders.png",
            //    Children = new ObservableCollection<TileBarItemsHelper>() 
            //    { 
            //    new TileBarItemsHelper()
            //    {
            //        Caption = "R&D Analysis",
            //        BackColor = "#566573",
            //        Height = 40,
            //        NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.AnalysisRequestView", null, this); })
            //    },
            //    new TileBarItemsHelper()
            //    {
            //        Caption = "Work Orders",
            //        BackColor = "#566573",
            //        Height = 40,
            //        NavigateCommand = new DelegateCommand(() => { Service.Navigate("Emdep.Geos.Modules.Epc.Views.WorkingOrderView", null, this); })
            //    },
               
            //    },
            //});
        }
        #endregion
    }
}
