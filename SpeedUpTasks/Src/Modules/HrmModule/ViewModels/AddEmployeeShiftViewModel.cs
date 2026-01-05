using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Hrm;
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
using System.Windows;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Hrm.ViewModels
{
    public class AddEmployeeShiftViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        IHrmService HrmService = new HrmServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        #endregion

        #region Declaration

        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private ObservableCollection<CompanyShift> companyShiftList;
        private List<EmployeeShift> employeeNewShiftList;
        private List<EmployeeShift> employeeExistShiftList;

        private int indexSun;
        private int indexMon;
        private int indexTue;
        private int indexWed;
        private int indexThu;
        private int indexFri;
        private int indexSat;

        //private double dialogHeight;
        //private double dialogWidth;

        #endregion

        #region Public ICommands
        public ICommand AddEmployeeShiftViewCancelButtonCommand { get; set; }
        public ICommand AddEmployeeShiftViewAcceptButtonCommand { get; set; }
        public ICommand CommandTextInput { get; set; }
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

        #region Properties
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
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNew"));
            }
        }
        public bool IsSave
        {
            get
            {
                return isSave;
            }

            set
            {
                isSave = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsSave"));
            }
        }

        public ObservableCollection<CompanyShift> CompanyShiftList
        {
            get
            {
                return companyShiftList;
            }

            set
            {
                companyShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CompanyShiftList"));
            }
        }

        public List<EmployeeShift> EmployeeNewShiftList
        {
            get
            {
                return employeeNewShiftList;
            }
            set
            {
                employeeNewShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeNewShiftList"));
            }
        }

        public List<EmployeeShift> EmployeeExistShiftList
        {
            get
            {
                return employeeExistShiftList;
            }
            set
            {
                employeeExistShiftList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EmployeeExistShiftList"));
            }
        }

        public int IndexSun
        {
            get { return indexSun; }
            set { indexSun = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexSun")); }
        }
        public int IndexMon
        {
            get { return indexMon; }
            set { indexMon = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexMon")); }
        }
        public int IndexTue
        {
            get { return indexTue; }
            set { indexTue = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexTue")); }
        }
        public int IndexWed
        {
            get { return indexWed; }
            set { indexWed = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexWed")); }
        }
        public int IndexThu
        {
            get { return indexThu; }
            set { indexThu = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexThu")); }
        }
        public int IndexFri
        {
            get { return indexFri; }
            set { indexFri = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexFri")); }
        }
        public int IndexSat
        {
            get { return indexSat; }
            set { indexSat = value; OnPropertyChanged(new PropertyChangedEventArgs("IndexSat")); }
        }
        //public double DialogWidth
        //{
        //    get { return dialogWidth; }
        //    set
        //    {
        //        dialogWidth = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("DialogWidth"));
        //    }
        //}
        //public double DialogHeight
        //{
        //    get { return dialogHeight; }
        //    set
        //    {
        //        dialogHeight = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("DialogHeight"));
        //    }
        //}
        #endregion

        #region Constructor

        public AddEmployeeShiftViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddNewEmployeeShiftViewModel()...", category: Category.Info, priority: Priority.Low);
                System.Windows.Forms.Screen screen = GeosApplication.Instance.GetWorkingScreenFrom();
                //DialogWidth = screen.Bounds.Width - 150;
                //DialogHeight = screen.Bounds.Height - 300;
                WindowHeader = Application.Current.FindResource("ProfileDetailViewAddEmployeeShift").ToString();
                AddEmployeeShiftViewCancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                AddEmployeeShiftViewAcceptButtonCommand = new RelayCommand(new Action<object>(AddNewShift));
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);

                GeosApplication.Instance.Logger.Log("Constructor AddNewEmployeeShiftViewModel()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Constructor AddNewEmployeeShiftViewModel()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Method
        /// <summary>
        /// [000][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// </summary>
        /// <param name="EmployeeShifts"></param>
        /// <param name="Shift"></param>
        public void Init(ObservableCollection<EmployeeShift> EmployeeShifts, Employee EmployeeDetails, ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptionList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method Init()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("ProfileDetailViewAddEmployeeShift").ToString();

                EmployeeExistShiftList = new List<EmployeeShift>();
                EmployeeExistShiftList = EmployeeDetails.EmployeeShifts.Select(x => (EmployeeShift)x.Clone()).ToList();

                string idsEmployeeJobDescriptionCompanyId = string.Empty;

                List<object> Days = GeosApplication.Instance.GetWeekNames();

                if (EmployeeJobDescriptionList != null && EmployeeJobDescriptionList.Count > 0)
                {
                    // here is check job description end date is null or end date less than today date and groupby Job description company id
                    idsEmployeeJobDescriptionCompanyId = string.Join(",", EmployeeJobDescriptionList.Where(x => x.JobDescriptionEndDate == null || x.JobDescriptionEndDate >= DateTime.Now.Date).ToList().GroupBy(x => x.Company.IdCompany).ToList().Select(i => i.Key.ToString()));

                    CompanyShiftList = new ObservableCollection<CompanyShift>(HrmService.GetAllCompanyShiftsByIdCompany_V2035(idsEmployeeJobDescriptionCompanyId));

                    if (EmployeeShifts.Count > 0)
                    {
                        List<Int32> EmployeeShiftIdList = EmployeeShifts.Select(x => x.IdCompanyShift).ToList();
                        foreach (CompanyShift item in CompanyShiftList.Where(x => EmployeeShiftIdList.Contains(x.IdCompanyShift)).ToList())
                        {
                            CompanyShiftList.Remove(item);
                        }
                    }
                }

                IndexSun = Days.FindIndex(x => x.ToString() == "Sunday") + 4;
                IndexMon = Days.FindIndex(x => x.ToString() == "Monday") + 4;
                IndexTue = Days.FindIndex(x => x.ToString() == "Tuesday") + 4;
                IndexWed = Days.FindIndex(x => x.ToString() == "Wednesday") + 4;
                IndexThu = Days.FindIndex(x => x.ToString() == "Thursday") + 4;
                IndexFri = Days.FindIndex(x => x.ToString() == "Friday") + 4;
                IndexSat = Days.FindIndex(x => x.ToString() == "Saturday") + 4;

                GeosApplication.Instance.Logger.Log("Method Init()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Init()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        public void AddNewEmployeeShiftInit(ObservableCollection<EmployeeShift> EmployeeShifts, ObservableCollection<EmployeeJobDescription> EmployeeJobDescriptions)
        {
            try
            {

                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeShiftInit()...", category: Category.Info, priority: Priority.Low);

                WindowHeader = Application.Current.FindResource("ProfileDetailViewAddEmployeeShift").ToString();

                EmployeeExistShiftList = new List<EmployeeShift>();
                EmployeeExistShiftList = EmployeeShifts.Select(x => (EmployeeShift)x.Clone()).ToList();

                string idsEmployeeJobDescriptionCompanyId = string.Empty;

                List<object> Days = GeosApplication.Instance.GetWeekNames();

                if (EmployeeJobDescriptions != null)
                {
                    // here is check job description end date is null or end date less than today date and groupby Job description company id
                    idsEmployeeJobDescriptionCompanyId = string.Join(",", EmployeeJobDescriptions.Where(x => x.JobDescriptionEndDate == null || x.JobDescriptionEndDate >= DateTime.Now.Date).ToList().GroupBy(x => x.Company.IdCompany).ToList().Select(i => i.Key.ToString()));
                    //CompanyShiftList = new ObservableCollection<CompanyShift>(HrmService.GetAllCompanyShiftsByIdCompany_V2032(idsEmployeeJobDescriptionCompanyId));
                    CompanyShiftList = new ObservableCollection<CompanyShift>(HrmService.GetAllCompanyShiftsByIdCompany_V2035(idsEmployeeJobDescriptionCompanyId));

                    if (EmployeeShifts.Count > 0)
                    {
                        List<Int32> EmployeeShiftIdList = EmployeeShifts.Select(x => x.IdCompanyShift).ToList();

                        foreach (CompanyShift item in CompanyShiftList.Where(x => EmployeeShiftIdList.Contains(x.IdCompanyShift)).ToList())
                        {
                            CompanyShiftList.Remove(item);
                        }
                    }
                }

                IndexSun = Days.FindIndex(x => x.ToString() == "Sunday") + 4;
                IndexMon = Days.FindIndex(x => x.ToString() == "Monday") + 4;
                IndexTue = Days.FindIndex(x => x.ToString() == "Tuesday") + 4;
                IndexWed = Days.FindIndex(x => x.ToString() == "Wednesday") + 4;
                IndexThu = Days.FindIndex(x => x.ToString() == "Thursday") + 4;
                IndexFri = Days.FindIndex(x => x.ToString() == "Friday") + 4;
                IndexSat = Days.FindIndex(x => x.ToString() == "Saturday") + 4;

                GeosApplication.Instance.Logger.Log("Method AddNewEmployeeShiftInit()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewEmployeeShiftInit()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// [000][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// </summary>
        /// <param name="obj"></param>
        private void AddNewShift(object obj)
        {
            try
            {
                EmployeeNewShiftList = new List<EmployeeShift>();
                GeosApplication.Instance.Logger.Log("Method AddNewShift()...", category: Category.Info, priority: Priority.Low);
                if (IsNew == true)
                {
                    foreach (CompanyShift item in CompanyShiftList.Where(x => x.IsExistShift == true).ToList())
                    {
                        if (EmployeeExistShiftList.Any(x => x.IdCompanyShift == item.IdCompanyShift))
                        {
                            EmployeeShift employeeShift = EmployeeExistShiftList.Where(x => x.IdCompanyShift == item.IdCompanyShift).FirstOrDefault();
                            EmployeeNewShiftList.Add(employeeShift);
                        }
                        else
                            EmployeeNewShiftList.Add(new EmployeeShift() { CompanyShift = item, TransactionOperation = ModelBase.TransactionOperations.Add, IdCompanyShift = item.IdCompanyShift });

                    }
                    IsSave = true;
                    RequestClose(null, null);

                    GeosApplication.Instance.Logger.Log("Method AddNewShift....executed successfully", category: Category.Info, priority: Priority.Low);
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method AddNewShift()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        /// <summary>
        /// [000][skale][31-07-2019][GEOS2-1679]Add improvements in the shift features
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            IsSave = false;
            RequestClose(null, null);
        }
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {
             
                HrmCommon.Instance.OpenWindowClickOnShortcutKey(obj);

                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion
    }
}
