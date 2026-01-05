using DevExpress.Mvvm;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Warehouse.ViewModels
{
    public class AddManufacturerViewModel : NavigationViewModelBase, IDataErrorInfo, INotifyPropertyChanged, IDisposable
    {
        #region Services
        IWarehouseService WarehouseService = new WarehouseServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration

        private List<Country>listCountry;
        private Country selectedCountry=new Country();
        private string manufacturerName;
        private bool isManufacturerAdded = false;

        ManufacturersByArticle articleManufacturer;

        private List<ManufacturersByArticle> listManufacturer;

       // private int selectedIndexManufacturer;

        private List<Manufacturer> listExistManufacturer;

       // private List<Manufacturer> tempListExistManufacturer;
        private List<Country> listExistCountry;
       // private List<string> manufacturerNameStrList;
        #endregion

        #region Properties

        //public List<string> ManufacturerNameStrList
        //{
        //    get
        //    {
        //        return manufacturerNameStrList;
        //    }

        //    set
        //    {
        //        manufacturerNameStrList = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("ManufacturerNameStrList"));
        //    }
        //}


        public bool IsManufacturerAdded { get; set; }
        public List<Country> ListExistCountry 
        {
            get
            {
                return listExistCountry;
            }

            set
            {
                listExistCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListExistCountry"));
            }
        }
        public List<Manufacturer> ListExistManufacturer
        {
            get
            {
                return listExistManufacturer;
            }

            set
            {
                listExistManufacturer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListExistManufacturer"));
            }
        }
        //public List<Manufacturer> TempListExistManufacturer
        //{
        //    get
        //    {
        //        return tempListExistManufacturer;
        //    }

        //    set
        //    {
        //        tempListExistManufacturer = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("TempListExistManufacturer"));
        //    }
        //}
        public List<Country> ListCountry
        {
            get
            {
                return listCountry;
            }

            set
            {
                listCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListCountry"));
            }
        }

        public Country SelectedCountry
        {
            get
            {
                return selectedCountry;
            }

            set
            {
                selectedCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedCountry"));
            }
        }

        public string ManufacturerName
        {
            get
            {
                return manufacturerName;
            }

            set
            {
                manufacturerName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ManufacturerName"));
                //ShowPopupAsPerManufacturerName(manufacturerName);
                //foreach (var item in ListExistManufacturer)
                //{
                //    if (manufacturerName.ToLower() == item.Name.ToLower())
                //    {
                //        foreach (var item2 in ListExistCountry)
                //        {
                //            if(item2.Name==SelectedCountry.Name)
                //            {
                //                CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddManufacturerWarning").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //                manufacturerName = string.Empty;
                //            }
                //        }
                //    }
                //}

            }
        }

        public ManufacturersByArticle ArticleManufacturer
        {
            get
            {
                return articleManufacturer;
            }

            set
            {
                articleManufacturer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticleManufacturer"));
            }
        }

        public List<ManufacturersByArticle> ListManufacturer
        {
            get { return listManufacturer; }
            set
            {
                listManufacturer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListManufacturer"));
            }
        }

        //public int SelectedIndexManufacturer
        //{
        //    get { return selectedIndexManufacturer; }
        //    set
        //    {
        //        selectedIndexManufacturer = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexManufacturer"));
               
        //    }
        //}

        
        #endregion

        #region ICommand
        public ICommand AddManufacturerViewCancelButtonCommand { get; set; }
        public ICommand AddManufacturerViewAcceptButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddManufacturerViewModel()
        {

            try
            {

                GeosApplication.Instance.Logger.Log("Constructor AddManufacturerViewModel()...", category: Category.Info, priority: Priority.Low);
                AddManufacturerViewCancelButtonCommand = new DelegateCommand<object>(AddManufacturerViewCancelButtonAction);
                AddManufacturerViewAcceptButtonCommand = new DelegateCommand<object>(AddManufacturerViewAcceptButtonAction);

                //string error = EnableValidationAndGetError();
                //OnPropertyChanged(new PropertyChangedEventArgs("ManufacturerName"));

                GeosApplication.Instance.Logger.Log("Constructor AddManufacturerViewCancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddManufacturerViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }


        }

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



        #region Validation

        bool allowValidation = false;
        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
                string error =
                     me[BindableBase.GetPropertyName(() => ManufacturerName)]; //+
                    // me[BindableBase.GetPropertyName(() => SelectedIndexManufacturer)]; 

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";

                return null;
            }
        }
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (!allowValidation) return null;
                string ManufacturerNameProp = BindableBase.GetPropertyName(() => ManufacturerName);
               // string SelectedIndexManufacturerProp = BindableBase.GetPropertyName(() => SelectedIndexManufacturer);

                if (columnName == ManufacturerNameProp)
                {
                    foreach (var item in ListExistManufacturer)
                    {
                        if (manufacturerName.ToLower() == item.Name.ToLower())
                        {
                            foreach (var item2 in ListExistCountry)
                            {
                                if (item2.Name == SelectedCountry.Name)
                                {
                                    return "Manufacturer with country already exist";
                                    // CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddManufacturerWarning").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                                }
                            }
                        }
                    }
                    return WarehouseValidation.GetErrorMessage(ManufacturerNameProp, ManufacturerName);
                }
                //elseif (columnName == SelectedIndexManufacturerProp)
                //{
                //    return WarehouseValidation.GetErrorMessage(SelectedIndexManufacturerProp, SelectedIndexManufacturer);
                //}
                return null;
            }
        }

        #endregion


        #region Method

        public void Init(Int32 IdArticle,List<ManufacturersByArticle> ListProducer)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init() AddManufacturerViewModel ...", category: Category.Info, priority: Priority.Low);
                ArticleManufacturer = new ManufacturersByArticle();
                ArticleManufacturer.IdArticle = IdArticle;
                ListCountry = WarehouseService.GetAllCountries();
                SelectedCountry = ListCountry[0];

                ListManufacturer = new List<ManufacturersByArticle>();
                foreach (var item in ListProducer)
                {
                    ListManufacturer.Add(item);
                }
                ListManufacturer.RemoveAt(0);

               ListExistCountry = new List<Country>();
                ListExistManufacturer = new List<Manufacturer>();

                foreach (var item in ListManufacturer)
                {
                    ListExistManufacturer.Add(item.Manufacturer);
                    ListExistCountry.Add(item.Country);
                }

                GeosApplication.Instance.Logger.Log("Method Init() AddManufacturerViewModel....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch(Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Init() AddManufacturerViewModel...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        public void AddManufacturerViewAcceptButtonAction(object obj)
        {

            try
            {
                GeosApplication.Instance.Logger.Log("Method AddManufacturerViewAcceptButtonAction()...", category: Category.Info, priority: Priority.Low);

                string error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("ManufacturerName"));
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndexManufacturer"));
                if (error != null)
                {
                    return;
                }

                ArticleManufacturer.Country = SelectedCountry;             
                ArticleManufacturer.IdCountry = SelectedCountry.IdCountry ;
                ArticleManufacturer.Manufacturer = new Manufacturer();
                ArticleManufacturer.Manufacturer.Name = ManufacturerName.Trim();
                ArticleManufacturer.IsNew = true;
                ArticleManufacturer.ManufacturerWithCountry = ManufacturerName.Trim() + " | " + SelectedCountry.Name.Trim();
                IsManufacturerAdded = true;

                RequestClose(null, null);
                GeosApplication.Instance.Logger.Log("Method AddManufacturerViewAcceptButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AddManufacturerViewAcceptButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

        }

        /// <summary>
        /// Method for Close 
        /// </summary>
        /// <param name="obj"></param>
        private void AddManufacturerViewCancelButtonAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddManufacturerViewCancelButtonAction()...", category: Category.Info, priority: Priority.Low);

                IsManufacturerAdded = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AddManufacturerViewCancelButtonAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddManufacturerViewCancelButtonAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        double StringSimilarityScore(string name, string searchString)
        {
            if (name.Contains(searchString))
            {
                return (double)searchString.Length / (double)name.Length;
            }

            return 0;
        }

        /// <summary>
        /// Method for search similar customer name.
        /// </summary>
        /// <param name="Name"></param>
        private void ShowPopupAsPerManufacturerName(string Name)
        {
            //GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerManufacturerName ...", category: Category.Info, priority: Priority.Low);

            //TempListExistManufacturer = ListExistManufacturer.ToList();

            //if (TempListExistManufacturer != null && !string.IsNullOrEmpty(Name))
            //{
            //    if (Name.Length > 1)
            //    {
            //        TempListExistManufacturer = TempListExistManufacturer.Where(h => h.Name.ToUpper().Contains(Name.ToUpper()) || h.Name.ToUpper().StartsWith(Name.Substring(0, 2).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(Name.Substring(Name.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, Name)).ToList();
            //        ManufacturerNameStrList = TempListExistManufacturer.Select(pn => pn.Name).ToList();
            //    }
            //    else
            //    {
            //        TempListExistManufacturer = TempListExistManufacturer.Where(h => h.Name.ToUpper().Contains(Name.ToUpper()) || h.Name.ToUpper().StartsWith(Name.Substring(0, 1).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(Name.ToUpper().Substring(Name.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, Name)).ToList();
            //        ManufacturerNameStrList = TempListExistManufacturer.Select(pn => pn.Name).ToList();

            //    }

            //}

            //else
            //{
            //    TempListExistManufacturer = new List<Manufacturer>();
            //    ManufacturerNameStrList = new List<string>();
            //}

            

            //GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerManufacturerName() executed successfully", category: Category.Info, priority: Priority.Low);

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion 
    }
}
