using Emdep.Geos.Data.Common.OTM;
using Emdep.Geos.Data.Common;
using Emdep.Geos.UI.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Emdep.Geos.Modules.OTM.CommonClass
{
    // [Rahul.Gadhave][GEOS2-6962][Date:25-02-2025]
    public class POTemplate : ModelBase, INotifyPropertyChanged
    {
        #region Fields
        string header;
        #endregion


        #region Declaration
        private ObservableCollection<OtRequestTemplates> OtRequestTemplateslist;
        private OtRequestTemplates selectedOtRequestTemplates;
        #endregion

        #region Properties
        public ObservableCollection<OtRequestTemplates> OtRequestTemplatesList
        {
            get { return OtRequestTemplateslist; }
            set
            {
                OtRequestTemplateslist = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OtRequestTemplatesList"));
            }
        }
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }
        public OtRequestTemplates SelectedOtRequestTemplates
        {
            get { return selectedOtRequestTemplates; }
            set
            {
                selectedOtRequestTemplates = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOtRequestTemplates"));
            }
        }
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

        #endregion

        #region Methods
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
