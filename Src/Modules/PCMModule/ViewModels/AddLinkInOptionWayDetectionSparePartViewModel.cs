using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Xpf.Editors;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Xpf.Core;
using System.Windows;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using System.Threading;
using System.Globalization;
using Emdep.Geos.Data.Common;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Modules.PCM.Views;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.WindowsUI;
using WindowsUIDemo;
using Prism.Logging;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Data.Common.PCM;
using System.ServiceModel;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.UI.Validations;
using System.IO;

namespace Emdep.Geos.Modules.PCM.ViewModels
{
    class AddLinkInOptionWayDetectionSparePartViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Service
        IPCMService PCMService = new PCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
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

        #region Declarations

        private string windowHeader;
        private bool isNew;
        private bool isSave;
        private string linkAddress;
        private string linkName;
        private string linkDescription;
        private uint idDetectionsItemAttachedLink;

        private DetectionAttachedLink selectedOptionWayDetectionSparePartLink;
        private uint idDetectionAttachedLink;
        private DateTime updateDate;
        private int linkFlag = 0;

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

        public DetectionAttachedLink SelectedOptionWayDetectionSparePartLink
        {
            get
            {
                return selectedOptionWayDetectionSparePartLink;
            }

            set
            {
                selectedOptionWayDetectionSparePartLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedOptionWayDetectionSparePartLink"));
            }
        }

        public string LinkAddress
        {
            get
            {
                return linkAddress;
            }

            set
            {
                linkAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkAddress"));
            }
        }

        public string LinkName
        {
            get
            {
                return linkName;
            }

            set
            {
                linkName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkName"));
            }
        }

        public string LinkDescription
        {
            get
            {
                return linkDescription;
            }

            set
            {
                linkDescription = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkDescription"));
            }
        }

        public uint IdDetectionsItemAttachedLink
        {
            get
            {
                return idDetectionsItemAttachedLink;
            }

            set
            {
                idDetectionsItemAttachedLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionsItemAttachedLink"));
            }
        }

        public uint IdDetectionAttachedLink
        {
            get
            {
                return idDetectionAttachedLink;
            }

            set
            {
                idDetectionAttachedLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdDetectionAttachedLink"));
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                return updateDate;
            }

            set
            {
                updateDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UpdateDate"));
            }
        }

        #endregion


        #region ICommand
        public ICommand AcceptLinkActionCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand EscapeButtonCommand { get; set; }
        public ICommand EditValueChangedCommand { get; set; }

        #endregion

        #region Constructor
        public AddLinkInOptionWayDetectionSparePartViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddLinkInOptionWayDetectionSparePartViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptLinkActionCommand = new DelegateCommand<object>(AcceptLinkAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                EditValueChangedCommand = new DelegateCommand<object>(CopyLinkAddessAsLinkName);

                GeosApplication.Instance.Logger.Log("Constructor AddLinkInOptionWayDetectionSparePartViewModel() executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddLinkInOptionWayDetectionSparePartViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods

        private void CloseWindow(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CloseWindow()...", category: Category.Info, priority: Priority.Low);
                IsSave = false;
                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method CloseWindow()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CloseWindow() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void AcceptLinkAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptLinkAction()...", category: Category.Info, priority: Priority.Low);

                char[] trimChars = { '\r', '\n' };
                LinkDescription = LinkDescription == null ? "" : LinkDescription;
                if (LinkDescription.Contains("\r\n"))
                {
                    LinkDescription = LinkDescription.TrimEnd(trimChars);
                    LinkDescription = LinkDescription.TrimStart(trimChars);
                }

                if (IsNew)
                {
                    SelectedOptionWayDetectionSparePartLink = new DetectionAttachedLink();
                    SelectedOptionWayDetectionSparePartLink.Name = LinkName;
                    SelectedOptionWayDetectionSparePartLink.Address = LinkAddress;
                    SelectedOptionWayDetectionSparePartLink.Description = LinkDescription;
                    SelectedOptionWayDetectionSparePartLink.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedOptionWayDetectionSparePartLink.IdDetectionAttachedLink = IdDetectionsItemAttachedLink;
                    SelectedOptionWayDetectionSparePartLink.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                }
                else
                {
                    SelectedOptionWayDetectionSparePartLink = new DetectionAttachedLink();
                    SelectedOptionWayDetectionSparePartLink.Name = LinkName;
                    SelectedOptionWayDetectionSparePartLink.Address = LinkAddress;
                    SelectedOptionWayDetectionSparePartLink.Description = LinkDescription;
                    SelectedOptionWayDetectionSparePartLink.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedOptionWayDetectionSparePartLink.IdDetectionAttachedLink = IdDetectionsItemAttachedLink;
                    SelectedOptionWayDetectionSparePartLink.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                }
                IsSave = true;

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptLinkAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptLinkAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        public void EditInit(DetectionAttachedLink detectionAttachedLink)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit()...", category: Category.Info, priority: Priority.Low);

                IdDetectionAttachedLink = detectionAttachedLink.IdDetectionAttachedLink;
                LinkName = detectionAttachedLink.Name;
                LinkAddress = detectionAttachedLink.Address;
                LinkDescription = detectionAttachedLink.Description;
                UpdateDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;

                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void CopyLinkAddessAsLinkName(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method CopyLinkAddessAsLinkName()...", category: Category.Info, priority: Priority.Low);

                if (string.IsNullOrEmpty(LinkName))
                {
                    LinkName = LinkAddress;
                    linkFlag = 1;
                }
                else if (linkFlag == 1)
                    LinkName = LinkAddress;

                GeosApplication.Instance.Logger.Log("Method CopyLinkAddessAsLinkName()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method CopyLinkAddessAsLinkName() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        #endregion
    }
}
