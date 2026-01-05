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
    class AddLinkInProductTypeViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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
        private string description;
        private uint idCatalogueItemAttachedLink;
        private ProductTypeAttachedLink selectedProductTypeLink;
        private int linkFlag=0;
        private string error = string.Empty;
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

        public ProductTypeAttachedLink SelectedProductTypeLink
        {
            get
            {
                return selectedProductTypeLink;
            }

            set
            {
                selectedProductTypeLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedProductTypeLink"));
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
                if (string.IsNullOrEmpty(linkName))
                    linkFlag = 0;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkName"));
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public uint IdCatalogueItemAttachedLink
        {
            get
            {
                return idCatalogueItemAttachedLink;
            }

            set
            {
                idCatalogueItemAttachedLink = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdCatalogueItemAttachedLink"));
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
        public AddLinkInProductTypeViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor AddLinkInProductTypeViewModel ...", category: Category.Info, priority: Priority.Low);

                AcceptLinkActionCommand = new DelegateCommand<object>(ProductTypeLinkAction);
                CancelButtonCommand = new DelegateCommand<object>(CloseWindow);
                EscapeButtonCommand = new DelegateCommand<object>(CloseWindow);
                EditValueChangedCommand = new DelegateCommand<object>(CopyLinkAddessAsLinkName);

                GeosApplication.Instance.Logger.Log("Constructor AddLinkInProductTypeViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Error in Constructor AddLinkInProductTypeViewModel() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        
        #endregion


        #region Methods

        private void ProductTypeLinkAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ProductTypeLinkAction()...", category: Category.Info, priority: Priority.Low);

                allowValidation = true;
                error = EnableValidationAndGetError();
                PropertyChanged(this, new PropertyChangedEventArgs("LinkAddress"));

                if (error != null)
                {
                    return;
                }

                char[] trimChars = { '\r', '\n' };
                Description = Description == null ? "" : Description;
                if (Description != null)
                {
                    if (Description.Contains("\r\n"))
                    {
                        Description = Description.TrimEnd(trimChars);
                        Description = Description.TrimStart(trimChars);
                    }
                }

                if (IsNew)
                {
                    SelectedProductTypeLink = new ProductTypeAttachedLink();
                    SelectedProductTypeLink.Name = LinkName;
                    SelectedProductTypeLink.Address = LinkAddress;
                    SelectedProductTypeLink.Description = Description;
                    SelectedProductTypeLink.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedProductTypeLink.IdCPTypeAttachedLink = IdCatalogueItemAttachedLink;
                    SelectedProductTypeLink.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                }
                else
                {
                    SelectedProductTypeLink = new ProductTypeAttachedLink();
                    SelectedProductTypeLink.Name = LinkName;
                    SelectedProductTypeLink.Address = LinkAddress;
                    SelectedProductTypeLink.Description = Description;
                    SelectedProductTypeLink.CreatedBy = Convert.ToUInt32(GeosApplication.Instance.ActiveUser.IdUser);
                    SelectedProductTypeLink.IdCPTypeAttachedLink = IdCatalogueItemAttachedLink;
                    SelectedProductTypeLink.UpdatedDate = Emdep.Geos.UI.Common.GeosApplication.Instance.ServerDateTime.Date.Date;
                }
                IsSave = true;

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method ProductTypeLinkAction()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method ProductTypeLinkAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

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

        public void EditInit(ProductTypeAttachedLink catalogueItemAttachedLink)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method EditInit() ...", category: Category.Info, priority: Priority.Low);

                IdCatalogueItemAttachedLink = catalogueItemAttachedLink.IdCPTypeAttachedLink;
                LinkName = catalogueItemAttachedLink.Name;
                Description = catalogueItemAttachedLink.Description;
                LinkAddress = catalogueItemAttachedLink.Address;


                GeosApplication.Instance.Logger.Log("Method EditInit()....executed successfully", category: Category.Info, priority: Priority.Low);

            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method EditInit() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
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
                                me[BindableBase.GetPropertyName(() => LinkAddress)];

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

                string linkAddress = BindableBase.GetPropertyName(() => LinkAddress);

                if (columnName == linkAddress)
                {
                    return AddEditModuleValidation.GetErrorMessage(linkAddress, LinkAddress);
                }

                return null;
            }
        }

        #endregion
    }
}
