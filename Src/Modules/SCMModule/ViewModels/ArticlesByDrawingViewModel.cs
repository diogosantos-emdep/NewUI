using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
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

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    public class ArticlesByDrawingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //[rdixit][GEOS2-5389][26.04.2024]
        #region Service      
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMService = new SCMServiceController("localhost:6699");
        private INavigationService Service { get { return this.GetService<INavigationService>(); } }
     
        #endregion

        #region Public Events
        public event EventHandler RequestClose;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion // End Of Events 

        #region Fields
        ArticlesbyDrawing articlesByDrawing;
        string title;
        ObservableCollection<ArticlesbyDrawing> articlesByDrawingList;
        #endregion

        #region Properties
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        public ObservableCollection<ArticlesbyDrawing> ArticlesByDrawingList
        {
            get
            {
                return articlesByDrawingList;
            }

            set
            {
                articlesByDrawingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesByDrawingList"));
            }
        }

        public ArticlesbyDrawing ArticlesByDrawing
        {
            get
            {
                return articlesByDrawing;
            }

            set
            {
                articlesByDrawing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ArticlesByDrawing"));
            }
        }
        #endregion

        #region ICommand
        public ICommand CommandTextInput { get; set; }  //[shweta.thube][GEOS2-6630][04.04.2025]
        #endregion

        public ArticlesByDrawingViewModel()
        {
            try
            {
                CommandTextInput = new DelegateCommand<KeyEventArgs>(ShortcutAction);  //[shweta.thube][GEOS2-6630][04.04.2025]
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor ArticlesByDrawingViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        
        public void Init(uint idDrawing)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method ConnectorViewModel()...."), category: Category.Info, priority: Priority.Low);
                Title = idDrawing.ToString();
                ArticlesByDrawingList = new ObservableCollection<ArticlesbyDrawing>(SCMService.GetArticlesByDrawing(idDrawing));
                if (ArticlesByDrawingList != null)
                    ArticlesByDrawing = ArticlesByDrawingList.FirstOrDefault();
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log(string.Format("Method ConnectorViewModel()....executed successfully"), category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in Constructor ArticlesByDrawingViewModel() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }
        //[shweta.thube][GEOS2-6630][04.04.2025]
        private void ShortcutAction(KeyEventArgs obj)
        {

            GeosApplication.Instance.Logger.Log("Method ShortcutAction ...", category: Category.Info, priority: Priority.Low);
            try
            {

                SCMShortcuts.Instance.OpenWindowClickOnShortcutKey(obj);
                if (SCMShortcuts.Instance.IsActive)
                {
                    RequestClose(null, null);
                }
                GeosApplication.Instance.Logger.Log("Method ShortcutAction....executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Method ShortcutAction...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
    }
}
