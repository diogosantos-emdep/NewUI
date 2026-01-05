using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Modules.Crm.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.ServiceProcess;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class ArticlesReportViewModel : NavigationViewModelBase, INotifyPropertyChanged, IDisposable
    {
        #region Services
        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }

        #endregion // Services

        #region Constructor
        /// [001][cpatil][GEOS2-5299][26-02-2024]
        public ArticlesReportViewModel()
        {
            

            ScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 340;
            ArticlesReportAcceptButtonCommand = new RelayCommand(new Action<object>(ArticlesReportAcceptAction));
            ArticlesReportCancelButtonCommand = new RelayCommand(new Action<object>(ArticlesReportCancelAction));
            SelectedItemChangedCommand = new RelayCommand(new Action<object>(SelectedItemChangedAction));
            CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

            ReferenceCategories = new List<string>();
            ReferenceCategories.Add(Convert.ToString(Application.Current.FindResource("ArticlesReportWarehouseCategory")));
            ReferenceCategories.Add(Convert.ToString(Application.Current.FindResource("ArticlesReportSupplier")));

            FromDate = GeosApplication.Instance.SelectedyearStarDate;
            ToDate = GeosApplication.Instance.SelectedyearEndDate;
            // [001] Changed Service method 
            //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
            CompanyList = new ObservableCollection<Company>(CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser));

            SelectedItems = new List<object>();
            SelectedItems.Clear();
            SelectedItems = new List<object>(CompanyList);

            SelectedIndexReference = 0;
            //set hide/show shortcuts on permissions
            Visible = Visibility.Visible.ToString();
            if (GeosApplication.Instance.IsPermissionReadOnly)
            {
                Visible = Visibility.Hidden.ToString();
            }
            else
            {
                Visible = Visibility.Visible.ToString();
            }
        }

        #endregion // Constructor

        #region ICommands
        public ICommand ArticlesReportAcceptButtonCommand { get; set; }
        public ICommand ArticlesReportCancelButtonCommand { get; set; }
        public ICommand SelectedItemChangedCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
        #endregion

        #region Declaration

        DateTime fromDate;
        DateTime toDate;

        private List<string> referenceCategories;
        ObservableCollection<WarehouseCategory> warehouseCategories;
        List<ArticleBySupplier> articlesBySupplier;
        Int16 selectedIndexReference;
        private Visibility isWarehouseCategoryVisible;
        private Visibility isSupplierVisible;
        private bool isBusy;
        private int screenHeight;
        private string visible;
        private ObservableCollection<Company> companyList;
        private Company selectedCompanyIndex;

        #endregion // Declaration


        #region Properties
        public virtual List<object> SelectedItems { get; set; }
        public ObservableCollection<Company> CompanyList
        {
            get
            {
                return companyList;
            }

            set
            {
                companyList = value;
                OnPropertyChanged("CompanyList");
            }
        }
        public Company SelectedCompanyIndex
        {
            get
            {
                return selectedCompanyIndex;
            }

            set
            {
                selectedCompanyIndex = value;
                OnPropertyChanged("SelectedCompanyIndex");
            }
        }

        public int ScreenHeight
        {
            get
            {
                return screenHeight;
            }

            set
            {
                screenHeight = value;
                OnPropertyChanged("ScreenHeight");
            }
        }
        public List<string> ReferenceCategories
        {
            get { return referenceCategories; }
            set
            {
                referenceCategories = value;
                OnPropertyChanged("ReferenceCategories");
            }
        }

        public ObservableCollection<WarehouseCategory> WarehouseCategories
        {
            get { return warehouseCategories; }
            set
            {
                warehouseCategories = value;
                OnPropertyChanged("WarehouseCategories");
            }
        }

        public List<ArticleBySupplier> ArticlesBySupplier
        {
            get { return articlesBySupplier; }
            set
            {
                articlesBySupplier = value;
                OnPropertyChanged("ArticlesBySupplier");
            }
        }

        public short SelectedIndexReference
        {
            get { return selectedIndexReference; }
            set
            {
                selectedIndexReference = value;
                OnPropertyChanged("SelectedIndexReference");
            }
        }

        public Visibility IsWarehouseCategoryVisible
        {
            get { return isWarehouseCategoryVisible; }
            set
            {
                isWarehouseCategoryVisible = value;
                OnPropertyChanged("IsWarehouseCategoryVisible");
            }
        }

        public Visibility IsSupplierVisible
        {
            get { return isSupplierVisible; }
            set
            {
                isSupplierVisible = value;
                OnPropertyChanged("IsSupplierVisible");
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public DateTime FromDate
        {
            get { return fromDate; }
            set
            {
                fromDate = value;
                OnPropertyChanged("FromDate");
            }
        }

        public DateTime ToDate
        {
            get { return toDate; }
            set
            {
                toDate = value;
                OnPropertyChanged("ToDate");
            }
        }
        public string Visible
        {
            get
            {
                return visible;
            }

            set
            {
                visible = value;
                OnPropertyChanged("Visible");
            }
        }
        #endregion // Properties

        #region public Events

        public event EventHandler RequestClose;

        // Property Change Logic  
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // Public Events

        #region Methods
        public void GetArticleByCategory(Int64? IdParent, List<WarehouseCategory> tempIdList)
        {
            foreach (WarehouseCategory item in tempIdList)
            {
                if (!WarehouseCategories.Any(cc => cc.IdParent == item.IdArticleCategory))
                {
                    WarehouseCategory RemoveWC = WarehouseCategories.Where(cc => cc.IdArticleCategory == item.IdArticleCategory).SingleOrDefault();
                    WarehouseCategories.Remove(RemoveWC);
                    if (RemoveWC.IdParent != 0)
                    {
                        tempIdList.Remove(item);
                        GetArticleByCategory(RemoveWC.IdParent, tempIdList);
                    }
                    else
                    {
                        WarehouseCategory RemoveWCParent = WarehouseCategories.Where(cc => cc.IdArticleCategory == item.IdParent).SingleOrDefault();
                        WarehouseCategories.Remove(RemoveWCParent);
                    }
                }
            }
        }
        /// [001][cpatil][GEOS2-5299][26-02-2024]
        public void SelectedItemChangedAction(object obj)
        {
            if (obj == null) return;

            try
            {
                ListBoxEdit listBoxEdit = (ListBoxEdit)((RoutedEventArgs)obj).Source;

                DXSplashScreen.Show(x =>
                {
                    Window win = new Window()
                    {
                        ShowActivated = false,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        AllowsTransparency = true,
                        Background = new SolidColorBrush(Colors.Transparent),
                        ShowInTaskbar = false,
                        Topmost = true,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    };
                    WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                    win.Topmost = false;
                    return win;
                }, x =>
                {
                    return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                }, null, null);

                if (listBoxEdit.SelectedIndex == 0)
                {
                    if (WarehouseCategories == null)
                    {
                        WarehouseCategories = new ObservableCollection<WarehouseCategory>(CrmStartUp.GetWarehouseCategories().OrderBy(c => c.BaseName).ToList());
                        List<WarehouseCategory> tempIdList = new List<WarehouseCategory>();
                        tempIdList = WarehouseCategories.Where(c => c.IdArticle == 0).ToList();
                        GetArticleByCategory(0, tempIdList);
                    }
                    IsSupplierVisible = Visibility.Collapsed;
                    IsWarehouseCategoryVisible = Visibility.Visible;
                }
                else if (listBoxEdit.SelectedIndex == 1)
                {
                    if (ArticlesBySupplier == null)
                    {
                        // [001] Changed Service method
                        //Service GetAllCompaniesDetails_V2490 updated with GetAllCompaniesDetails_V2500 by [GEOS2-5556][27.03.2024][rdixit]
                        List<Company> CompanyList = CrmStartUp.GetAllCompaniesDetails_V2500(GeosApplication.Instance.ActiveUser.IdUser);

                        string idCompanies = string.Join(",", CompanyList.Select(i => i.ConnectPlantId));
                        List<ArticleBySupplier> temp = CrmStartUp.GetArticlesBySupplier(idCompanies);

                        //Group elements and add as a parent
                        var groupedElements = temp.GroupBy(x => x.IdArticleSupplier);
                        foreach (var item in groupedElements)
                        {
                            ArticleBySupplier abys = new ArticleBySupplier();
                            abys.IdArticleSupplier = 0;
                            abys.Article = new Article();
                            abys.Article.Reference = item.First().ArticleSupplier.Name;
                            abys.IdChild = item.Key;
                            temp.Add(abys);
                        }

                        ArticlesBySupplier = new List<ArticleBySupplier>(temp);
                    }

                    IsWarehouseCategoryVisible = Visibility.Collapsed;
                    IsSupplierVisible = Visibility.Visible;
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Reference SelectedItemChangedAction() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in Reference SelectedItemChangedAction() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                IsBusy = false;
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in Reference SelectedItemChangedAction() method {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
        }

        public void ArticlesReportAcceptAction(object obj)
        {
            List<Offer> offers = new List<Offer>();
            string idArticles = string.Empty;

            if (SelectedIndexReference == 0)
            {
                List<WarehouseCategory> whCategories = WarehouseCategories.Where(x => x.IsChecked && x.IdArticle > 0).ToList();
                idArticles = string.Join(",", whCategories.Select(i => i.IdArticle));
            }
            else if (SelectedIndexReference == 1)
            {
                List<ArticleBySupplier> articles = ArticlesBySupplier.Where(x => x.IsChecked && x.IdArticle > 0).ToList();
                idArticles = string.Join(",", articles.Select(i => i.IdArticle));
            }

            try
            {
                string ResultFileName;
                SaveFileDialogService.DefaultExt = "xlsx";
                SaveFileDialogService.DefaultFileName = "Articles Report";
                SaveFileDialogService.Filter = "Excel Files (.xlsx)|*.xlsx|All Files (*.*)|*.*";
                SaveFileDialogService.FilterIndex = 1;
                bool DialogResult = SaveFileDialogService.ShowDialog();

                if (!DialogResult)
                {
                    ResultFileName = string.Empty;
                    return;
                }
                else
                {
                    if (!DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime)
                    {
                        DXSplashScreen.Show(x =>
                        {
                            System.Windows.Window win = new System.Windows.Window()
                            {
                                ShowActivated = false,
                                WindowStyle = WindowStyle.None,
                                ResizeMode = ResizeMode.NoResize,
                                AllowsTransparency = true,
                                Background = new SolidColorBrush(Colors.Transparent),
                                ShowInTaskbar = false,
                                Topmost = true,
                                SizeToContent = SizeToContent.WidthAndHeight,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                            };
                            WindowFadeAnimationBehavior.SetEnableAnimation(win, true);
                            win.Topmost = false;
                            return win;
                        }, x =>
                        {
                            return new SplashScreenView() { DataContext = new SplashScreenViewModel() };
                        }, null, null);
                    }

                    ResultFileName = (SaveFileDialogService.File).DirectoryName + "\\" + (SaveFileDialogService.File).Name;
                }


                //Sprint 44--CRM  M044-03--Articles report generated for selected plants----sdesai

                if (GeosApplication.Instance.IdUserPermission == 22 && GeosApplication.Instance.SelectedPlantOwnerUsersList != null)
                {
                    //List<Company> CompanyList = GeosApplication.Instance.SelectedPlantOwnerUsersList.Cast<Company>().ToList();
                    foreach (Company company in SelectedItems)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + company.Alias;
                            //offers.AddRange(CrmStartUp.GetArticlesReportDetails_V2220(GeosApplication.Instance.ActiveUser.IdUser, company, FromDate, ToDate, GeosApplication.Instance.IdCurrencyByRegion, idArticles));
                            // shubham[skadam]GEOS2-4052 Add a new column “PO Date” in Articles Report 30 11 2022
                            offers.AddRange(CrmStartUp.GetArticlesReportDetails_V2620(GeosApplication.Instance.ActiveUser.IdUser, company, FromDate, ToDate, GeosApplication.Instance.IdCurrencyByRegion, idArticles)); //[nsatpute][19-03-2025][GEOS2-6991]
                            GeosApplication.Instance.SplashScreenMessage = "";
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                            GeosApplication.Instance.Logger.Log(string.Format("Get an error in ArticlesReportAcceptAction() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                            GeosApplication.Instance.Logger.Log(string.Format("Get an error in ArticlesReportAcceptAction() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                else
                {
                   // List<Company> CompanyList = CrmStartUp.GetAllCompaniesDetails_V2130(GeosApplication.Instance.ActiveUser.IdUser);
                    foreach (Company company in SelectedItems)
                    {
                        try
                        {
                            GeosApplication.Instance.SplashScreenMessage = "Connecting to " + company.Alias;
                            //offers.AddRange(CrmStartUp.GetArticlesReportDetails_V2220(GeosApplication.Instance.ActiveUser.IdUser, company, FromDate, ToDate, GeosApplication.Instance.IdCurrencyByRegion, idArticles));
                            // shubham[skadam]GEOS2-4052 Add a new column “PO Date” in Articles Report 30 11 2022
                            offers.AddRange(CrmStartUp.GetArticlesReportDetails_V2620(GeosApplication.Instance.ActiveUser.IdUser, company, FromDate, ToDate, GeosApplication.Instance.IdCurrencyByRegion, idArticles)); //[nsatpute][19-03-2025][GEOS2-6991]
                            GeosApplication.Instance.SplashScreenMessage = "";
                        }
                        catch (FaultException<ServiceException> ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                            GeosApplication.Instance.Logger.Log(string.Format("Get an error in ArticlesReportAcceptAction() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (ServiceUnexceptedException ex)
                        {
                            GeosApplication.Instance.SplashScreenMessage = String.Concat("Connecting to ", company.Alias, " Failed");
                            GeosApplication.Instance.Logger.Log(string.Format("Get an error in ArticlesReportAcceptAction() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }



                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GenerateArticlesReport(ResultFileName, offers, obj);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in ArticlesReportAcceptAction() method ", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                GeosApplication.Instance.Logger.Log(string.Format("Get an error in ArticlesReportAcceptAction() Method - ServiceUnexceptedException ", ex.Message), category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.SplashScreenMessage = string.Empty;
                IsBusy = false;
                GeosApplication.Instance.Logger.Log(String.Format("Get an error in ArticlesReportAcceptAction() method {0} ", ex.Message), category: Category.Exception, priority: Priority.Low);
            }

            if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
            GeosApplication.Instance.SplashScreenMessage = string.Empty;
        }

        public void ArticlesReportCancelAction(object obj)
        {
            RequestClose(null, null);
        }

        public void GenerateArticlesReport(string ResultFileName, List<Offer> offers, object obj)
        {
            Workbook workbook = new Workbook();

            workbook.Worksheets.Insert(0, "First");
            Worksheet ws = workbook.Worksheets[0];

            ws.Cells[0, 0].Value = "GROUP";
            ws.Cells[0, 0].ColumnWidth = 350;

            ws.Cells[0, 1].Value = "PLANT";
            ws.Cells[0, 1].ColumnWidth = 600;

            ws.Cells[0, 2].Value = "REGION";
            ws.Cells[0, 2].ColumnWidth = 250;

            ws.Cells[0, 3].Value = "OFFER";
            ws.Cells[0, 3].ColumnWidth = 350;

            ws.Cells[0, 4].Value = "BUSINESS UNIT";
            ws.Cells[0, 4].ColumnWidth = 400;

            ws.Cells[0, 5].Value = "CATEGORY1";
            ws.Cells[0, 5].ColumnWidth = 400;

            ws.Cells[0, 6].Value = "CATEGORY2";
            ws.Cells[0, 6].ColumnWidth = 600;

            ws.Cells[0, 7].Value = "WORK ORDER";
            ws.Cells[0, 7].ColumnWidth = 350;

            ws.Cells[0, 8].Value = "ITEM";
            ws.Cells[0, 8].ColumnWidth = 150;

            ws.Cells[0, 9].Value = "TEMPLATE";
            ws.Cells[0, 9].ColumnWidth = 500;

            ws.Cells[0, 10].Value = "REFERENCE";
            ws.Cells[0, 10].ColumnWidth = 400;

            ws.Cells[0, 11].Value = "DESCRIPTION";
            ws.Cells[0, 11].ColumnWidth = 400;

            ws.Cells[0, 12].Value = "SUPPLIER";
            ws.Cells[0, 12].ColumnWidth = 500;

            ws.Cells[0, 13].Value = "QUANTITY";
            ws.Cells[0, 13].ColumnWidth = 250;
            // shubham[skadam]GEOS2-4052 Add a new column “PO Date” in Articles Report 30 11 2022
            ws.Cells[0, 14].Value = "PO DATE";
            ws.Cells[0, 14].ColumnWidth = 350;

            ws.Cells[0, 15].Value = "SHIPPING DATE";
            ws.Cells[0, 15].ColumnWidth = 350;

            ws.Cells[0, 16].Value = "COST PRICE";
            ws.Cells[0, 16].ColumnWidth = 300;

            ws.Cells[0, 17].Value = "SELL PRICE";
            ws.Cells[0, 17].ColumnWidth = 300;

            ws.Cells[0, 18].Value = "OT PRICE";
            ws.Cells[0, 18].ColumnWidth = 300;

            ws.Cells[0, 19].Value = "SITE";
            ws.Cells[0, 19].ColumnWidth = 300;

            ws.Range["A1:T1"].Font.Bold = true;
            ws.Range["A1:T1"].Fill.BackgroundColor = System.Drawing.Color.LightGray;

            int counter = 1;
            bool isNumberFormatSupported = true;
            foreach (Offer offer in offers)
            {
                // "GROUP";
                ws.Cells[counter, 0].Value = offer.Site.Customer.CustomerName;

                // "PLANT"
                ws.Cells[counter, 1].Value = offer.Site.Name;

                // "Region"
                ws.Cells[counter, 2].Value = offer.Site.Country.Zone.Name;

                // "OFFER";
                ws.Cells[counter, 3].Value = offer.Code;

                // "Business Unit";
                ws.Cells[counter, 4].Value = offer.BusinessUnit.Value;

                // "Category1";
                ws.Cells[counter, 5].Value = offer.Category1;

                // "Category2";
                ws.Cells[counter, 6].Value = offer.Category2;

                // "WORK ORDER";
                ws.Cells[counter, 7].Value = string.Format("{0}{1}{2}", offer.Quotations[0].Ots[0].Code, " OT ", offer.Quotations[0].Ots[0].NumOT);

                // "ITEM";
                ws.Cells[counter, 8].Value = Convert.ToInt32(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.NumItem);    // offer.Quotations[0].Ots[0].NumOT;

                //  "TEMPLATE";
                ws.Cells[counter, 9].Value = offer.Quotations[0].Template.Name;

                // "REFERENCE";
                ws.Cells[counter, 10].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article.Reference;  // "01CT05";

                // "DESCRIPTION";
                ws.Cells[counter, 11].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article.Description; // "C5 Card";

                // "SUPPLIER";
                ws.Cells[counter, 12].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article.ArticleBySupplier.ArticleSupplier.Name;

                // "QUANTITY";
                ws.Cells[counter, 13].Value = Convert.ToInt32(offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.Quantity);

                // shubham[skadam]GEOS2-4052 Add a new column “PO Date” in Articles Report 30 11 2022
                //PO Date
                ws.Cells[counter, 14].Value = offer.Quotations[0].Ots[0].OtItems[0].PoDate;
                // "SHIPPING DATE";
                ws.Cells[counter, 15].Value = offer.Quotations[0].Ots[0].OtItems[0].ShippingDate;

                // "COST PRICE";
                ws.Cells[counter, 16].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article.ArticleBySupplier.CostPrice;

                // "SELL PRICE";
                ws.Cells[counter, 17].Value = offer.Quotations[0].Ots[0].OtItems[0].RevisionItem.WarehouseProduct.Article.SellPrice;

                // "OT PRICE";
                ws.Cells[counter, 18].Value = offer.OtPrice;

                try
                {
                    if (isNumberFormatSupported)
                    {
                        ws.Cells[counter, 16].NumberFormat = "\"" + GeosApplication.Instance.CurrentCurrencySymbol + "\"" + " #,##,##0.00";
                        ws.Cells[counter, 17].NumberFormat = "\"" + GeosApplication.Instance.CurrentCurrencySymbol + "\"" + " #,##,##0.00";
                        ws.Cells[counter, 18].NumberFormat = "\"" + GeosApplication.Instance.CurrentCurrencySymbol + "\"" + " #,##,##0.00";
                    }
                }
                catch (Exception ex)
                {
                    isNumberFormatSupported = false;
                    GeosApplication.Instance.Logger.Log(string.Format("Error in GenerateArticlesReport() NumberFormat - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }

                // "SITE";
                ws.Cells[counter, 19].Value = offer.Site.Alias;

                counter++;
            }

            ws.Columns.AutoFit(1, 6);
            ws.Columns.AutoFit(10, 14);

            try
            {
                using (FileStream stream = new FileStream(ResultFileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                }

                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }

                CustomMessageBox.Show(string.Format(string.Format(System.Windows.Application.Current.FindResource("ArticlesReportExportedSuccessfully").ToString())), "Green", CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK, (ArticlesReportView)obj);
                System.Diagnostics.Process.Start(ResultFileName);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                CustomMessageBox.Show("Failed to Generate Excel - " + ex.Message.ToString(), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
        }

        public void Dispose()
        {
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
                if (Visible == Visibility.Hidden.ToString())
                {
                    return;
                }
                CRMCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion // Methods
    }
}
