using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{

    /// <summary>
    /// Method for Initialization .
    /// [001]<!--[pramod.misal][GEOS2-5792][20.06.2024]-->[HRM - Travel Reports currency exchange based on ticket IESD-86619]
    /// </summary>
    /// 
    public class ExpenseReportsPlantsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        #region Services      
       
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //IHrmService HrmService = new HrmServiceController("localhost:6699");
        #endregion // End Services

        #region Public ICommand
        public ICommand PlantsViewCancelButtonCommand { get; set; }
        public ICommand PlantsViewAcceptButtonCommand { get; set; }

        #endregion

        #region public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Declaration
        private string windowHeader;
        ObservableCollection<Company> companyList;
        Company selectedcompany;
        Company newselectedcompany;
        private bool isPlantSelected;

        #endregion

        #region Properties
        public bool IsPlantSelected
        {
            get
            {
                return isPlantSelected;
            }

            set
            {
                isPlantSelected = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPlantSelected"));
            }
        }

        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyList"));
            }
        }
        public string WindowHeader
        {
            get
            {
                return windowHeader;
            }

            set
            {
                windowHeader = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WindowHeader"));
            }
        }

        public Company SelectedCompany
        {
            get
            {
                return selectedcompany;
            }

            set
            {
                selectedcompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCompany"));
            }
        }

        public Company NewSelectedCompany
        {
            get
            {
                return newselectedcompany;
            }

            set
            {
                newselectedcompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NewSelectedCompany"));
            }
        }
        #endregion

        #region Constructor
        public ExpenseReportsPlantsViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor ExpenseReportsPlantsViewModel()...", category: Category.Info, priority: Priority.Low);

                PlantsViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                PlantsViewAcceptButtonCommand = new RelayCommand(new Action<object>(SelectPlant));


                GeosApplication.Instance.Logger.Log("Constructor ExpenseReportsPlantsViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor ExpenseReportsPlantsViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
           

        }
        #endregion

        #region Methods
        /// <summary>
        /// Method for Initialization .
        /// [001]<!--[pramod.misal][GEOS2-5792][20.06.2024]-->[HRM - Travel Reports currency exchange based on ticket IESD-86619]
        /// </summary>
        /// 
        public void Init(List<int> UniqueIdCompanies)
        {
            WindowHeader = System.Windows.Application.Current.FindResource("ExpenseReportsPlantsViewTitle").ToString();
            GetCompanies(UniqueIdCompanies);

        }

        private void SelectPlant(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method SelectPlant()...", category: Category.Info, priority: Priority.Low);   
                          
                if (SelectedCompany != null)
                {
                    IsPlantSelected = true;
                    NewSelectedCompany = SelectedCompany;

                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Constructor SelectPlant()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {

                GeosApplication.Instance.Logger.Log("Get an error in  SelectPlant()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        private void GetCompanies(List<int> UniqueIdCompanies)
        {
            CompanyList = new ObservableCollection<Company>(HrmService.GetCompanies_V2420());

            CompanyList = new ObservableCollection<Company>(CompanyList.Where(x => UniqueIdCompanies.Contains(x.IdCompany)));

           
            if (CompanyList.Any())
            {
                
                foreach (var bpItem in CompanyList.GroupBy(tpa => tpa.Iso))
                {
                    
                    ImageSource CompanyFlagImage = ByteArrayToBitmapImage(bpItem.First().ImageInBytes);
 
                    foreach (var company in bpItem)
                    {
                        company.SiteImage = CompanyFlagImage;
                    }
                }

                SelectedCompany = CompanyList.FirstOrDefault();
            }

            //if (CompanyList != null)
            //{
            //    foreach (var bpItem in CompanyList.GroupBy(tpa => tpa.Iso))
            //    {
            //        ImageSource CompanyFlagImage = ByteArrayToBitmapImage(bpItem.ToList().FirstOrDefault().ImageInBytes);
            //        bpItem.ToList().Where(oti => oti.Iso == bpItem.Key).ToList().ForEach(oti => oti.SiteImage = CompanyFlagImage);
            //    }
            //}


            SelectedCompany = CompanyList.FirstOrDefault();

        }

        public ImageSource ByteArrayToBitmapImage(byte[] byteArrayIn)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method convert byteArrayToImage ...", category: Category.Info, priority: Priority.Low);

                if (byteArrayIn == null || byteArrayIn.Length == 0) return null;
                var image = new BitmapImage();
                using (var mem = new System.IO.MemoryStream(byteArrayIn))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }

                image.Freeze();
                GeosApplication.Instance.Logger.Log("Method byteArrayToImage() executed successfully", category: Category.Info, priority: Priority.Low);

                return image;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in byteArrayToImage() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            return null;
        }

        /// <summary>
        /// Method for Close Dialog Window . 
        /// </summary>
        private void CloseWindow(object obj)
        {          
            RequestClose(null, null);
        }

        #endregion

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        
    }
}
