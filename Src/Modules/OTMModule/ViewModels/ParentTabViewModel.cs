using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Emdep.Geos.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.OTM.ViewModels
{
    //[GEOS2-7036][rdixit][28.02.205]
    public interface ISupportTabs
    {
        ObservableCollection<ITabViewModel> Tabs { get; }
        void AddNewTab();
        void CloseTab(ITabViewModel tab);
    }

    public interface ITabViewModel : ISupportParentViewModel
    {
        string TabName { get; set; }
        object TabContent { get; }

        int Position { get; set; }
    }
    public class ParentTabViewModel : ISupportTabs
    {
        private ITabViewModel _selectedTab;
        public ITabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedTab"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public ICommand CancelButtonCommand { get; private set; }
        public virtual ObservableCollection<ITabViewModel> Tabs { get; protected set; }
        public ParentTabViewModel()
        {
            Tabs = new ObservableCollection<ITabViewModel>();
            CancelButtonCommand = new DelegateCommand<ITabViewModel>(CloseTab);
            LoadTabs();
        }

        public void AddNewTab()
        {
            Tabs.Add(ViewModelSource.Create(() => new OTMImportTemplateViewModel
            {
                TabName = "New Tab " + (Tabs.Count + 1),
                ParentViewModel = this
            }));
        }

        //public void CloseTab(ITabViewModel tab)
        //{
        //    Tabs.Remove(tab);
        //}

        public void CloseTab(ITabViewModel tab)
        {
            if (Tabs.Contains(tab))
            {
                Tabs.Remove(tab);

                // Ensure the UI updates (since IList<T> doesn't notify changes)
                Tabs = new ObservableCollection<ITabViewModel>(Tabs);

                
            }
        }


        protected virtual void LoadTabs()
        {
        }
        public void Sort()
        {
            Tabs = new ObservableCollection<ITabViewModel>(Tabs.OrderBy(x => x.Position));
            SelectedTab = Tabs.LastOrDefault();
        }
    }
}
