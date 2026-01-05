using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
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
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Crm.ViewModels
{
    public class AttachToOpportunityViewModel : NavigationViewModelBase,  INotifyPropertyChanged, IDisposable
    {
        #region Service

        ICrmService CrmStartUp = new CrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IGeosRepositoryService GeosRepositoryServiceController = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        private int selectedIndexOpportunity;
        public bool IsSave { get; set; }
        private bool isBusy;
        private List<TimelineGrid> opportunityList;
        //private List<Tag> tempTagList;
        #endregion

        #region Properties

        public List<TimelineGrid> OpportunityList
        {
            get { return opportunityList; }
            set
            {
                opportunityList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OpportunityList"));
            }
        }
        public int SelectedIndexOpportunity
        {
            get { return selectedIndexOpportunity; }
            set
            {
                selectedIndexOpportunity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndexOpportunity"));

                //if (!IsInIt && SelectedIndexGeosProject != -1)
                //{
                //    if (GeosProjectsList != null && GeosProjectsList.Count != 0 && GeosProjectsList[SelectedIndexGeosProject] != null)
                //        SelectedIndexCarOEM = CaroemsList.FindIndex(cr => cr.IdCarOEM == GeosProjectsList[SelectedIndexGeosProject].IdCarOem);
                //}
                //else if (!IsInIt && SelectedIndexGeosProject == -1)
                //    SelectedIndexCarOEM = 0;
            }
        }
        //public List<Tag> TempTagList
        //{
        //    get { return tempTagList; }
        //    set { tempTagList = value; OnPropertyChanged(new PropertyChangedEventArgs("TempTagList")); }
        //}        
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        #endregion


        #region ICommands

        public ICommand AttachToOpportunityViewAcceptButtonCommand { get; set; }
        public ICommand AttachToOpportunityViewCancelButtonCommand { get; set; }


        #endregion

        #region Events

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

        #region Constructor

        public AttachToOpportunityViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AttachToOpportunityViewModel ...", category: Category.Info, priority: Priority.Low);

                TimelineParams objTimelineParams = new TimelineParams();
                objTimelineParams.idCurrency = GeosApplication.Instance.IdCurrencyByRegion;
                objTimelineParams.idUser = GeosApplication.Instance.ActiveUser.IdUser;
                objTimelineParams.idsSelectedUser = GeosApplication.Instance.ActiveUser.IdUser.ToString();
                objTimelineParams.idZone = GeosApplication.Instance.ActiveUser.Company.Country.IdZone;
                //[001] Added 
                objTimelineParams.activeSite = new ActiveSite { IdSite = 18, SiteAlias = "EAES", SiteServiceProvider = "10.0.9.7:80" };
                objTimelineParams.accountingYearFrom = GeosApplication.Instance.SelectedyearStarDate;
                objTimelineParams.accountingYearTo = GeosApplication.Instance.SelectedyearEndDate;
                objTimelineParams.Roles = RoleType.SalesGlobalManager;
                OpportunityList = CrmStartUp.GetOpportunities(objTimelineParams).ToList();


                AttachToOpportunityViewAcceptButtonCommand = new Prism.Commands.DelegateCommand<object>(AttachOpportunityAccept);
                AttachToOpportunityViewCancelButtonCommand = new Prism.Commands.DelegateCommand<object>(CloseWindow);
                //OnPropertyChanged(new PropertyChangedEventArgs("TagName"));

                GeosApplication.Instance.Logger.Log("Constructor AttachToOpportunityViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AttachToOpportunityViewModel() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Methods



        /// <summary>
        /// Method for add new tag.
        /// </summary>
        /// <param name="obj"></param>
        public void AttachOpportunityAccept(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("AttachOpportunityAccept() Method ...", category: Category.Info, priority: Priority.Low);

                //if (!string.IsNullOrEmpty(TagName.Trim()))
                //{
                    
                    try
                    {
                        //bool isExist = CrmStartUp.IsExistTagName(TagName.Trim());

                        //if (!isExist)
                        //{
                            var taglist = new List<string>();
                            Tag NewTag = new Tag();
                            //NewTag.Name = TagName.Trim();
                            //taglist = TempTagList.Select(t => t.Name).ToList();

                            //if (!taglist.Contains(TagName, StringComparer.OrdinalIgnoreCase))
                            //{
                            //    TempTagList.Insert(0, NewTag);
                            //    IsSave = true;
                            //}
                            //else
                            //{
                            //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddNewTagExistMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                            //    return;
                            //}
                        //}
                        //else
                        //{
                        //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("AddNewTagFailMessage").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                        //    return;
                        //}
                    }
                    catch (Exception ex)
                    {
                    }
                    RequestClose(null, null);
                    GeosApplication.Instance.Logger.Log("Method AttachOpportunityAccept() executed successfully", category: Category.Info, priority: Priority.Low);
                //}
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachOpportunityAccept() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in AttachOpportunityAccept() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AttachOpportunityAccept() Constructor " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        /// <summary>
        /// Method for search similar word.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
                //double StringSimilarityScore(string name, string searchString)
                //{
                //    if (name.Contains(searchString))
                //    {
                //        return (double)searchString.Length / (double)name.Length;
                //    }

                //    return 0;
                //}
        private void ShowPopupAsPerTagName(string ProjectName)
        {
            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName ...", category: Category.Info, priority: Priority.Low);

            //TagNameList = CrmStartUp.GetAllTags().ToList();

            //if (TagNameList != null && !string.IsNullOrEmpty(TagName))
            //{
            //    if (TagName.Length > 1)
            //    {
            //        TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 2).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 2).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
            //        TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();
            //    }
            //    else
            //    {
            //        TagNameList = TagNameList.Where(h => h.Name.ToUpper().Contains(TagName.ToUpper()) || h.Name.ToUpper().StartsWith(TagName.Substring(0, 1).ToUpper())
            //                                                || h.Name.ToUpper().EndsWith(TagName.Substring(TagName.Length - 1).ToUpper())).OrderByDescending(apn => StringSimilarityScore(apn.Name, TagName)).ToList();
            //        TagNameStrList = TagNameList.Select(pn => pn.Name).ToList();

            //    }
            //}

            //else
            //{
            //    TagNameList = new List<Tag>();
            //    TagNameStrList = new List<string>();
            //}

            ////For alert Icon visibility
            //if (TagNameStrList.Count > 0)
            //{
            //    AlertVisibility = Visibility.Visible;
            //}
            //else
            //    AlertVisibility = Visibility.Hidden;

            GeosApplication.Instance.Logger.Log("Method ShowPopupAsPerTagName() executed successfully", category: Category.Info, priority: Priority.Low);

        }

        /// <summary>
        /// Method for close window.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            //TagName = string.Empty;
            RequestClose(null, null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
