using DevExpress.Mvvm;
using Emdep.Geos.Modules.Epc.Common.EPC;
using Emdep.Geos.Modules.Epc.Views;
using Emdep.Geos.UI.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Emdep.Geos.Modules.Epc.ViewModels
{
   public class MilestonesViewModel:ViewModelBase
    {

        public ObservableCollection<Milestone> source;
        public ObservableCollection<Milestone> selection = new ObservableCollection<Milestone>();
        public ObservableCollection<Milestone> Source { get { return this.source; } }
        public ObservableCollection<Milestone> Selection { get { return this.selection; } }


        //public ICommand ShowDialogWindowCommand { get; private set; }

        public ICommand selectedRowMilestone { get; set; }
        ObservableCollection<Milestone> listMilestones = new ObservableCollection<Milestone>();

        public ObservableCollection<Milestone> ListMilestones
        {
            get { return listMilestones; }
            set { listMilestones = value; }
        }
        public ICommand MilestoneMailButtonClickCommand { get; set; }
       
       public MilestonesViewModel()
       {

           source = new ObservableCollection<Milestone>();
           Selection.CollectionChanged += Selection_CollectionChanged;
           //ShowDialogWindowCommand = new DelegateCommand(ShowDialogWindow);
           MilestoneMailButtonClickCommand = new RelayCommand(new Action<object>(SendMilestoneMail));
           source.Add(new Milestone { WorkingOrder = "2015-04048", ProjectCode = "2015-PR0404", ProjectName = "GEOS2", ProjectType = Common.EPC.ProjectType.Short, Owner = "sbambrule", MilestoneName = "Released Installer", MilestoneDate = new DateTime(2016, 04, 08), Attempts = 1 });
           source.Add(new Milestone { WorkingOrder = "2016-04048", ProjectCode = "2016-PR0404", ProjectName = "EPC2 Visualization", ProjectType = Common.EPC.ProjectType.Long, Owner = "lsharma", MilestoneName = "Created Prototypes", MilestoneDate = new DateTime(2016, 03, 31), Attempts = 1 });
           source.Add(new Milestone { WorkingOrder = "2014-04369", ProjectCode = "2014-PR2302", ProjectName = "AT Rack v2015", ProjectType = Common.EPC.ProjectType.Strategical, Owner = "JMC", MilestoneName = "Tested Prototypes", MilestoneDate = new DateTime(2016, 03, 31), Attempts = 1 });

       }
       void Selection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
       {

       }

       public void SendMilestoneMail(object obj)
       {
           MilestoneDialogView MilestoneDialogView = new Views.MilestoneDialogView();
           MilestoneDialogViewModel v = new MilestoneDialogViewModel();
           //v.ProjectMilestoneData = Selection.FirstOrDefault();
           EventHandler handle = delegate { MilestoneDialogView.Close(); };
           v.RequestClose += handle;
           MilestoneDialogView.DataContext = v;
           MilestoneDialogView.ShowDialogWindow();


       }

       private string mySelectedRow;

       public string MySelectedRow
       {
           get { return mySelectedRow; }
           set
           {
               mySelectedRow = value;
               SetProperty(ref mySelectedRow, value, () => MySelectedRow);
           }
       }

       public void MySelectedRowValue(object obj)
       {
           Selection.Clear();
           for (int i = 0; i < Source.Count; i++)
               Selection.Add(Source[i]);

       }
    

    }
}
