using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.ERM;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using Emdep.Geos.Modules.ERM.Views;

namespace Emdep.Geos.Modules.ERM.ViewModels
{
    //[Pallavi Jadhav][Geos-4332][14 04 2023]
    class AddEditEquivalentWeightViewModel : NavigationViewModelBase, IDisposable, INotifyPropertyChanged, IDataErrorInfo
    {
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

        #region Declaration
        DateTime? originalStartDate;
        DateTime? originalEndDate;
        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private float? equivalentWeight;
        private DateTime? startDate;
        private DateTime? endDate;
        private EquivalencyWeight lastEndDate;
        private EquivalencyWeight nextStartDate;
        private EquivalencyWeight selectedEquivalentWeight;
        private string error = string.Empty;
        public DateTime? endDateRecord;
        public DateTime? startDateRecord;
        private EquivalencyWeight recordStartDate;
        private EquivalencyWeight recordEndDate;
        private int listCount;
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

        public float? EquivalentWeight
        {
            get
            {
                return equivalentWeight;
            }

            set
            {
                equivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EquivalentWeight"));
            }
        }

       
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

       
        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }

            set
            {
                endDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }
        public EquivalencyWeight SelectedEquivalentWeight
        {
            get
            {
                return selectedEquivalentWeight;
            }

            set
            {
                selectedEquivalentWeight = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedEquivalentWeight"));
            }
        }
        public EquivalencyWeight LastEndDate
        {
            get
            {
                return lastEndDate;
            }

            set
            {
                lastEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LastEndDate"));
            }
        }
        public EquivalencyWeight NextStartDate
        {
            get
            {
                return nextStartDate;
            }

            set
            {
                nextStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("NextStartDate"));
            }
        }
        public DateTime? StartDateRecord
        {
            get
            {
                return startDateRecord;
            }

            set
            {
                startDateRecord = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDateRecord"));
            }
        }
        //public EquivalencyWeight NextStartDate
        //{
        //    get
        //    {
        //        return nextStartDate;
        //    }

        //    set
        //    {
        //        nextStartDate = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("NextStartDate"));
        //    }
        //}
        public EquivalencyWeight RecordStartDate
        {
            get
            {
                return recordStartDate;
            }

            set
            {
                recordStartDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RecordStartDate"));
            }
        }
        public EquivalencyWeight RecordEndDate
        {
            get
            {
                return recordEndDate;
            }

            set
            {
                recordEndDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RecordEndDate"));
            }
        }
        public int ListCount
        {
            get
            {
                return listCount;
            }

            set
            {
                listCount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ListCount"));
            }
        }
        ObservableCollection<EquivalencyWeight> tempModulesEquivalentWeightList;
        public ObservableCollection<EquivalencyWeight> TempModulesEquivalentWeightList
        {
            get
            {
                return tempModulesEquivalentWeightList;
            }
            set
            {
                tempModulesEquivalentWeightList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TempModulesEquivalentWeightList"));
            }
        }

        #endregion

        #region Command
        public ICommand CancelButtonCommand { get; set; }
        public ICommand AcceptEquivalentWeightActionCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        #endregion

        #region Constructor
        public AddEditEquivalentWeightViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddEditEquivalentWeightViewModel ...", category: Category.Info, priority: Priority.Low);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                AcceptEquivalentWeightActionCommand = new DelegateCommand<object>(AcceptEquivalentWeightAction);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Constructor AddEditEquivalentWeightViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
        }
        #endregion

        #region Method
        public void EditInit(EquivalencyWeight modulesEquivalencyWeight, ModulesEquivalencyWeight CloneData,int GridListCount, ObservableCollection<EquivalencyWeight> ModulesEquivalentWeightList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                EquivalentWeight = modulesEquivalencyWeight.EquivalentWeight;
                StartDate = Convert.ToDateTime(modulesEquivalencyWeight.StartDate.Value.Date);
                originalStartDate = Convert.ToDateTime(modulesEquivalencyWeight.StartDate.Value.Date);
                if (modulesEquivalencyWeight.EndDate != null)
                {
                    originalEndDate = Convert.ToDateTime(modulesEquivalencyWeight.EndDate.Value.Date);
                }
                if (modulesEquivalencyWeight.EndDate!=null)
                {
                    EndDate = Convert.ToDateTime(modulesEquivalencyWeight.EndDate.Value.Date);
                }
                ListCount = GridListCount;
                  LastEndDate = CloneData.LstEquivalencyWeight.OrderByDescending(a => a.EndDate).FirstOrDefault();
                if(CloneData.LstEquivalencyWeight.OrderBy(a => a.StartDate).Skip(1)!=null)
                // DateTime NextStartDate1 = CloneData.LstEquivalencyWeight.OrderBy(a => a.StartDate).Skip(1).Select(b=>b.EndDate);
                NextStartDate = ModulesEquivalentWeightList.OrderByDescending(i=>i.EndDate).FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }

        public void AddInit(ModulesEquivalencyWeight CloneData, ObservableCollection<EquivalencyWeight> ModulesEquivalentWeightList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AddInit()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                EquivalentWeight = null;
                StartDate = null;
                EndDate = null;
                LastEndDate = CloneData.LstEquivalencyWeight.OrderByDescending(a => a.EndDate).FirstOrDefault();
                RecordStartDate = ModulesEquivalentWeightList.OrderByDescending(a => a.StartDate).FirstOrDefault();
               // TempModulesEquivalentWeightList = ModulesEquivalentWeightList.ToList();

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }

                GeosApplication.Instance.Logger.Log("Method AddInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AddInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

            }
        }
        private void AcceptEquivalentWeightAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptEquivalentWeightAction()...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }   //[GEOS2-4636][rupali sarode][04-07-2023]
                // TableView detailView = (TableView)obj;
                //// GridControl gridControl = (detailView).Grid;
                // EquivalencyWeight SelectedRow = (EquivalencyWeight)detailView.DataControl.CurrentItem;
                //  StartDateRecord = SelectedRow.StartDate;
                //  EndDateRecord = SelectedRow.EndDate;
                allowValidation = true;
                error = EnableValidationAndGetError();

                PropertyChanged(this, new PropertyChangedEventArgs("EquivalentWeight"));
                PropertyChanged(this, new PropertyChangedEventArgs("StartDate"));
                PropertyChanged(this, new PropertyChangedEventArgs("EndDate"));
                if (error != null)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][19-07-2023]
                    return;
                }


                SelectedEquivalentWeight = new EquivalencyWeight();
                SelectedEquivalentWeight.EquivalentWeight = EquivalentWeight;
                SelectedEquivalentWeight.StartDate = StartDate;
                if (EndDate != null)
                {
                    SelectedEquivalentWeight.EndDate = EndDate;
                }
                IsSave = true;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                RequestClose(null, null);
                //if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Method AcceptEquivalentWeightAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); } //[GEOS2-4636][rupali sarode][04-07-2023]
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptEquivalentWeightAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()..."), category: Category.Info, priority: Priority.Low);
                //IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log(string.Format("Method CloseWindow()....executed successfully"), category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion

        #region Validation

        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
                return error;
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;

                string error =
                    me[BindableBase.GetPropertyName(() => EquivalentWeight)] +
                    me[BindableBase.GetPropertyName(() => StartDate)] +
                    me[BindableBase.GetPropertyName(() => EndDate)];
                     


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

                string selectedEquivalentWeight = BindableBase.GetPropertyName(() => EquivalentWeight);
                string selectedStartDate = BindableBase.GetPropertyName(() => StartDate);
                string selectedEndDate = BindableBase.GetPropertyName(() => EndDate);
                string selectedLastEndDate = BindableBase.GetPropertyName(() => LastEndDate);
                string selectedNextStartDate = BindableBase.GetPropertyName(() => NextStartDate);
                if (selectedEndDate != null)
                {
                    if (LastEndDate == null)
                    {
                        LastEndDate = new EquivalencyWeight();
                    }
                    if (RecordStartDate == null)
                    {
                        RecordStartDate = new EquivalencyWeight();
                    }
                    if (NextStartDate == null)
                    {
                        NextStartDate = new EquivalencyWeight();
                    }
                    
                    if (columnName == selectedEquivalentWeight)
                    {
                        return AddEditEquivalencyWeightValidation.GetErrorMessage(selectedEquivalentWeight, EquivalentWeight, NextStartDate.EndDate, EndDate, NextStartDate.StartDate, LastEndDate.EndDate, RecordStartDate.EndDate, ListCount, TempModulesEquivalentWeightList);
                    }

                    if (columnName == selectedStartDate)
                    {                       
                        return AddEditEquivalencyWeightValidation.GetErrorMessage(selectedStartDate, StartDate, NextStartDate.EndDate, EndDate, originalStartDate, LastEndDate.EndDate, RecordStartDate.EndDate, ListCount, TempModulesEquivalentWeightList);
                    }
                    
                    if (columnName == selectedEndDate)
                    {
                        return AddEditEquivalencyWeightValidation.GetErrorMessage(selectedEndDate, StartDate, NextStartDate.EndDate, EndDate, originalEndDate, LastEndDate.EndDate, RecordStartDate.EndDate, ListCount, TempModulesEquivalentWeightList);
                    }
                }
                return null;
            }
        }

        public void Dispose()
        {
        }


        #endregion
    }
}
