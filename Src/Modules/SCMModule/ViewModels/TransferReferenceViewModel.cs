using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SCM;
using Emdep.Geos.Modules.SCM.Common_Classes;
using Emdep.Geos.Modules.SCM.Views;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.Commands;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.CustomControls;
using Emdep.Geos.UI.Helper;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.UI.Validations;
using Prism.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;

namespace Emdep.Geos.Modules.SCM.ViewModels
{
    //[nsatpute][16.07.2025][GEOS2-8090]
    public class TransferReferenceViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Service
        ISCMService SCMService = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        ISCMService SCMServiceCompanyWise = new SCMServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        //ISCMService SCMServiceLocal = new SCMServiceController("localhost:6699");
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

        #region Declaration
        private string windowHeader;
        private ObservableCollection<Emdep.Geos.UI.Helper.Column> columns;
        private DataTable dtDrawing;
        private DataTable dtDrawingCopy;
        private ObservableCollection<BandItem> bands;
        private string reference;
        private string fromConnector;
        private string toConnector;
        private bool isBusy;
        private bool isAcceptEnable;
        private string myFilterString;
        private string backgroundColorName;
        private ObservableCollection<ScmDrawing> drawingList;
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
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsBusy"));
            }
        }
        public ObservableCollection<Emdep.Geos.UI.Helper.Column> Columns
        {
            get { return columns; }
            set
            {
                columns = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Columns"));
            }
        }
        public DataTable DtDrawing
        {
            get { return dtDrawing; }
            set
            {
                dtDrawing = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtDrawing"));
            }
        }
        public DataTable DtDrawingCopy
        {
            get { return dtDrawingCopy; }
            set
            {
                dtDrawingCopy = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DtDrawingCopy"));
            }
        }

        public ObservableCollection<BandItem> Bands
        {
            get { return bands; }
            set
            {
                bands = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Bands"));
            }
        }

        public string Reference
        {
            get { return reference; }
            set
            {
                reference = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Reference"));
            }
        }
        public string FromConnector
        {
            get { return fromConnector; }
            set
            {
                fromConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FromConnector"));
            }
        }
        public string ToConnector
        {
            get { return toConnector; }
            set
            {
                toConnector = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ToConnector"));
            }
        }
        public string MyFilterString
        {
            get { return myFilterString; }
            set
            {
                myFilterString = value;
                OnPropertyChanged(new PropertyChangedEventArgs("MyFilterString"));
            }
        }

        public string BackgroundColorName
        {
            get { return backgroundColorName; }
            set
            {
                backgroundColorName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BackgroundColorName"));
            }
        }
        public bool IsAcceptEnable
        {
            get
            {
                return isAcceptEnable;
            }
            set
            {
                isAcceptEnable = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAcceptEnable"));
            }
        }
		//[nsatpute][24.07.2025][GEOS2-8090]
        public ObservableCollection<ScmDrawing> DrawingList

        {
            get
            {
                return drawingList;
            }
            set
            {
                drawingList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DrawingList"));
            }
        }


        #endregion

        #region Public ICommand
        public ICommand AcceptButtonCommand { get; set; }
        public ICommand CancelButtonCommand { get; set; }
        public ICommand OpenIDrawingPathCommand { get; set; }
        public ICommand OpenArticleByDrawingCommand { get; set; }
        public ICommand ToConnectorTextLeaveCommand { get; set; }
        #endregion

        #region Constructor
        public TransferReferenceViewModel()
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Constructor TransferReferenceViewModel ...", category: Category.Info, priority: Priority.Low);
                if (!DXSplashScreen.IsActive) { DXSplashScreen.Show<SplashScreenView>(); }
                AcceptButtonCommand = new RelayCommand(new Action<object>(AcceptButtonCommandAction));
                CancelButtonCommand = new RelayCommand(new Action<object>(CloseWindow));
                OpenIDrawingPathCommand = new DelegateCommand<object>(OpenIDrawingPath);
                OpenArticleByDrawingCommand = new DelegateCommand<object>(OpenArticleByDrawingActionCommand);
                ToConnectorTextLeaveCommand = new DelegateCommand<object>(ToConnectorTextLeaveCommandAction);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Constructor TransferReferenceViewModel() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in TransferReferenceViewModel() Constructor " + ex.Message, category: Category.Info, priority: Priority.Low);
            }
        }
        #endregion

        #region Methods
		//[nsatpute][24.07.2025][GEOS2-8090]
        public void Init(string reference, string connector)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method Init().... "), category: Category.Info, priority: Priority.Low);

                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
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
                    return new SplashScreenCustomMessageView() { DataContext = new SplashScreenViewModel() };
                }, null, null);
                GeosApplication.Instance.CustomeSplashScreenInformationMessage = $"Collecting the drawing data from all plants {Environment.NewLine}in which customer reference is applied in OT...";
                this.Reference = reference;
                this.FromConnector = connector;
                if (!string.IsNullOrEmpty(connector))
                {
                    List<ScmDrawing> lstDrawing = GetAllPlantDrawingsByCustomerRererence(reference);
                    if (lstDrawing != null && lstDrawing.Count > 0)
                    {
                        DrawingList = new ObservableCollection<ScmDrawing>(lstDrawing);
                        FillDrawingColumns(DrawingList);
                        FillDrawingData(DrawingList);
                    }
                }
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
            }
            GeosApplication.Instance.Logger.Log(string.Format("Method Init().... Executed"), category: Category.Info, priority: Priority.Low);
        }

        //[nsatpute][GEOS2-8090][22.07.2025]
        private List<ScmDrawing> GetAllPlantDrawingsByCustomerRererence(string reference)
        {
            GeosApplication.Instance.Logger.Log(string.Format("Method GetAllPlantDrawingsByCustomerRererence().... "), category: Category.Info, priority: Priority.Low);
            List<ScmDrawing> lstAllSiteDrawings = new List<ScmDrawing>();
            ConcurrentBag<ScmDrawing> lstDrawing = new ConcurrentBag<ScmDrawing>();            
            try
            {
                //SCMService = new SCMServiceController("localhost:6699");
                List<Data.Common.Company> lstCompany = SCMService.GetAllCompaniesWithServiceProvider_V2660();
                
                // Configuration for parallel processing
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    CancellationToken = CancellationToken.None
                };
                try
                {
                    Parallel.ForEach(lstCompany, parallelOptions, company =>
                    {
                        try
                        {
                            SCMServiceCompanyWise = new SCMServiceController(company.ServiceProviderUrl);
                            //SCMServiceCompanyWise = new SCMServiceController("localhost:6699");

                            List<ScmDrawing> dr = SCMServiceCompanyWise.GetDrawingsByCustomerRef_V2660(reference);
                            if (dr != null && dr.Count > 0)
                            {
                                foreach (var drawing in dr)
                                {
                                    drawing.SiteName = company.ShortName;  
                                    drawing.CountryIconUrl = company.CountryIconUrl;
                                    lstDrawing.Add(drawing);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            GeosApplication.Instance.Logger.Log($"Get an error in GetAllPlantDrawingsByCustomerRererence() Method. Parallel processing error: Error processing company {company.Name}: {ex.Message}", category: Category.Exception, priority: Priority.Low);
                        }
                    });
                }
                catch (AggregateException ae)
                {
                    foreach (var ex in ae.InnerExceptions)
                        GeosApplication.Instance.Logger.Log("Get an error in GetAllPlantDrawingsByCustomerRererence() Method. Parallel processing error:" + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllPlantDrawingsByCustomerRererence() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in GetAllPlantDrawingsByCustomerRererence() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method GetAllPlantDrawingsByCustomerRererence() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            lstAllSiteDrawings.AddRange(lstDrawing.ToList());
            return lstAllSiteDrawings;
        }
        //[nsatpute][GEOS2-8090][22.07.2025]
        private void FillDrawingColumns(ObservableCollection<ScmDrawing> DrawingList)
        {
            try
            {
                GeosApplication.Instance.Logger.Log(string.Format("Method FillDrawingColumns().... "), category: Category.Info, priority: Priority.Low);
                //[GEOS2-6080][05.12.2024][rdixit]
                List<BandItem> bandsLocal = new List<BandItem>();
                BandItem bandAll = new BandItem() { BandName = "All", BandHeader = "", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandAll.Columns = new ObservableCollection<ColumnItem>();
                Columns = new ObservableCollection<Column>();
                DtDrawingCopy = new DataTable();
                Column c = new Column();

                // Add CountryName column with grouping (now first column)
                bandAll.Columns.Add(new ColumnItem()
                {
                    ColumnFieldName = "CountryName",
                    HeaderText = "",
                    Width = 120,
                    IsVertical = false,
                    DrawingSettings = DrawingSettingsType.IsGrouped,
                    Visible = false,
                    GroupIndex = 0,
                    AllowEditing = false,
                    AllowGrouping = true,
                    AllowSorting = true,
                    FixedStyle = FixedStyle.Left
                });

                // Add IdDrawing column
                bandAll.Columns.Add(new ColumnItem()
                {
                    ColumnFieldName = "IdDrawing",
                    HeaderText = "IdDrawing",
                    Width = 90,
                    IsVertical = false,
                    DrawingSettings = DrawingSettingsType.IdDrawing,
                    Visible = true
                });



                bandsLocal.Add(bandAll);
                DtDrawingCopy.Columns.Add("CountryName", typeof(string));
                DtDrawingCopy.Columns.Add("CountryUrl", typeof(string));                
                DtDrawingCopy.Columns.Add("IdDrawing", typeof(int));

                #region Detections
                List<string> Detections = new List<string>();
                var test = DrawingList.Where(item => item?.DetectionList != null).SelectMany(item => item.DetectionList.Select(det => det)).Distinct().ToList();

                Detections = DrawingList.Where(item => item?.DetectionList != null).SelectMany(item => item.DetectionList.Select(det => det.Name?.ToLower())).Distinct().ToList();
                BandItem bandDetection = new BandItem() { BandName = "Detection", MinWidth = 150, BandHeader = "Detection", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandDetection.Columns = new ObservableCollection<ColumnItem>();
                foreach (var item in Detections)
                {
                    DtDrawingCopy.Columns.Add(item, typeof(int));
                    bandDetection.Columns.Add(new ColumnItem() { ColumnFieldName = item, HeaderText = item, DrawingSettings = DrawingSettingsType.Default, Width = 50, Visible = true, IsVertical = true });
                }

                bandsLocal.Add(bandDetection);
                List<Tuple<byte, string>> CptypeColumns = DrawingList?.Where(i => i.CptypeName != null)?.Select(i => Tuple.Create(i.IdCPType, i.CptypeName))?.Distinct()?.ToList();

                BandItem bandCptype = new BandItem() { BandName = "CP_Type", MinWidth = 150, BandHeader = "CP Type", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandCptype.Columns = new ObservableCollection<ColumnItem>();

                if (CptypeColumns?.Count > 0)
                {
                    foreach (var item in CptypeColumns)
                    {
                        DtDrawingCopy.Columns.Add(item.Item2, typeof(string));
                        bandCptype.Columns.Add(new ColumnItem() { ColumnFieldName = item.Item2, HeaderText = item.Item2, Width = 50, DrawingSettings = DrawingSettingsType.Default, Visible = true, IsVertical = true });
                    }
                }
                bandsLocal.Add(bandCptype);

                List<Tuple<byte, string>> TemplateColumns = DrawingList?.Where(i => i.TemplateName != null)?.Select(i => Tuple.Create(i.IdTemplate, i.TemplateName))?.Distinct()?.ToList();

                BandItem bandTemplate = new BandItem() { BandName = "Template", MinWidth = 150, BandHeader = "Template", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandTemplate.Columns = new ObservableCollection<ColumnItem>();

                if (TemplateColumns?.Count > 0)
                {
                    foreach (var item in TemplateColumns)
                    {
                        DtDrawingCopy.Columns.Add(item.Item2, typeof(string));
                        bandTemplate.Columns.Add(new ColumnItem() { ColumnFieldName = item.Item2, Width = 50, HeaderText = item.Item2, DrawingSettings = DrawingSettingsType.Default, Visible = true, IsVertical = true });
                    }
                }
                bandsLocal.Add(bandTemplate);

                BandItem bandLast = new BandItem() { BandName = "", BandHeader = "", FixedStyle = FixedStyle.Left, OverlayHeaderByChildren = true };
                bandLast.Columns = new ObservableCollection<ColumnItem>();
                DtDrawingCopy.Columns.Add("Comments", typeof(string));
                DtDrawingCopy.Columns.Add("Path", typeof(string));
                DtDrawingCopy.Columns.Add("Site", typeof(string));
                DtDrawingCopy.Columns.Add("Created By", typeof(string));
                DtDrawingCopy.Columns.Add("Modified By", typeof(string));
                DtDrawingCopy.Columns.Add("Debugged", typeof(bool));
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Comments", HeaderText = "Comments", Width = 200, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Path", HeaderText = "Path", Width = 45, IsVertical = true, DrawingSettings = DrawingSettingsType.Image, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Site", HeaderText = "Site", Width = 150, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Created By", HeaderText = "Created By", Width = 300, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Modified By", HeaderText = "Modified By", Width = 300, IsVertical = true, DrawingSettings = DrawingSettingsType.Default, Visible = true });
                bandLast.Columns.Add(new ColumnItem() { ColumnFieldName = "Debugged", HeaderText = "Debugged", Width = 40, IsVertical = true, DrawingSettings = DrawingSettingsType.IsChecked, Visible = true });

                bandsLocal.Add(bandLast);
                #endregion
                Bands = new ObservableCollection<BandItem>(bandsLocal);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDrawingColumns() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log(string.Format("Method FillDrawingColumns().... Executed"), category: Category.Info, priority: Priority.Low);
        }
        //[nsatpute][GEOS2-8090][22.07.2025]
        private void FillDrawingData(ObservableCollection<ScmDrawing> DrawingList)
        {
            for (int i = 0; i < DrawingList.Count; i++)
            {
                try
                {
                    DataRow dr = DtDrawingCopy.NewRow();
                    dr["CountryName"] = DrawingList[i].SiteName;
                    dr["CountryUrl"] = DrawingList[i].CountryIconUrl;
                    dr["IdDrawing"] = DrawingList[i].IdDrawing;
                    dr["Comments"] = DrawingList[i].Comments;
                    dr["Path"] = "W:" + DrawingList[i].Path;
                    dr["Site"] = DrawingList[i].SiteName;
                    //[GEOS2-6080][05.12.2024][rdixit]
                    dr["Created By"] = DrawingList[i].CreatedBy + " " + DrawingList[i].CreatedIn?.ToShortDateString();
                    dr["Modified By"] = DrawingList[i].ModifiedBy + " " + DrawingList[i].ModifiedIn?.ToShortDateString();
                    dr["Debugged"] = DrawingList[i].Debugged;
                    dr[DrawingList[i].CptypeName] = "X";
                    dr[DrawingList[i].TemplateName] = "X";
                    if (DrawingList[i].DetectionList != null)
                    {
                        foreach (var item in DrawingList[i].DetectionList)
                        {
                            dr[item.Name] = item.Quantity;
                        }
                    }
                    DtDrawingCopy.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    GeosApplication.Instance.Logger.Log(string.Format("Error in method FillDrawingData() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                }
            }
            DtDrawing = DtDrawingCopy;
        }
        private void CloseWindow(object obj)
        {
            RequestClose(null, null);
        }

        private void AcceptButtonCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()...", category: Category.Info, priority: Priority.Low);

                bool isSave = false;
                //isSave = SCMService.UpdateLookupKey_V2420(UpdateLookUpValuesDetails);
                if (isSave == true)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ValueListUpdatedSuccess").ToString()), Application.Current.Resources["PopUpSuccessColor"].ToString(), CustomMessageBox.MessageImagePath.Ok, MessageBoxButton.OK);
                }

                RequestClose(null, null);

                GeosApplication.Instance.Logger.Log("Method AcceptButtonCommandAction()....executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), Application.Current.Resources["PopUpWarningColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in AcceptButtonCommandAction() Method " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method AcceptButtonCommandAction() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void OpenIDrawingPath(object folder)
        {
            string strExplorerEXE = String.Format("{0}\\explorer.exe", Environment.GetEnvironmentVariable("windir"));
            ProcessStartInfo info = new ProcessStartInfo(strExplorerEXE);
            info.Arguments = "/n," + "\"" + folder + "\"";
            info.WindowStyle = ProcessWindowStyle.Normal;
            System.Diagnostics.Process.Start(info);
        }
        private void OpenArticleByDrawingActionCommand(object obj)
        {
            try
            {
                TextBlock IdDrawing_TextBox = (TextBlock)obj;
                ArticlesbyDrawingView articlesByDrawingView = new ArticlesbyDrawingView();
                ArticlesByDrawingViewModel articlesByDrawingViewModel = new ArticlesByDrawingViewModel();
                EventHandler handle = delegate { articlesByDrawingView.Close(); };
                articlesByDrawingViewModel.RequestClose += handle;
                articlesByDrawingViewModel.Init(Convert.ToUInt32(IdDrawing_TextBox.Text));
                articlesByDrawingView.DataContext = articlesByDrawingViewModel;
                if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                articlesByDrawingView.Show();
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log(string.Format("Error in method Init() - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        private void ToConnectorTextLeaveCommandAction(object obj)
        {
            try
            {
                GeosApplication.Instance.Logger.Log("Method ToConnectorTextLeaveCommandAction...", category: Category.Info, priority: Priority.Low);
                try
                {
                    if (ToConnector == null || ToConnector == "" || (FromConnector.Trim() == ToConnector.Trim()))
                    {
                        BackgroundColorName = "Red";
                        IsAcceptEnable = false;
                    }
                    else
                    {
                        if (SCMService.IsConnectorExists(ToConnector.Trim()))
                        {
                            BackgroundColorName = "Green";
                            IsAcceptEnable = true;
                        }
                        else
                        {
                            BackgroundColorName = "Red";
                            IsAcceptEnable = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (DXSplashScreen.IsActive) { DXSplashScreen.Close(); }
                    GeosApplication.Instance.Logger.Log("Error in Method ToConnectorTextLeaveCommandAction()...." + ex.Message, category: Category.Exception, priority: Priority.Low);
                }
                GeosApplication.Instance.Logger.Log("Method ToConnectorTextLeaveCommandAction executed successfully.", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ToConnectorTextLeaveCommandAction() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ToConnectorTextLeaveCommandAction() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString(), null);
            }
            catch (Exception ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in ToConnectorTextLeaveCommandAction() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }


        #endregion
        #region Validation

        bool allowValidation = false;

        string EnableValidationAndGetError()
        {
            allowValidation = true;
            string error = ((IDataErrorInfo)this).Error;
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }
            return null;
        }

        string IDataErrorInfo.Error
        {
            get
            {
                /*
                if (!allowValidation) return null;
                IDataErrorInfo me = (IDataErrorInfo)this;
              
                string error =
                me[BindableBase.GetPropertyName(() => Name)] +
                me[BindableBase.GetPropertyName(() => InformationError)];
              

                if (!string.IsNullOrEmpty(error))
                    return "Please check inputted data.";
                */
                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                /*
                if (!allowValidation) return null;

                string name = BindableBase.GetPropertyName(() => Name);
                string headerInformtionError = BindableBase.GetPropertyName(() => InformationError);

                if (columnName == name)
                {
                    return AddEditCustompropertyValidation.GetErrorMessage(name,null, Name);
                }

                if (columnName == headerInformtionError)
                {
                    return AddEditCustompropertyValidation.GetErrorMessage(headerInformtionError,null, InformationError);
                }
                */
                return null;
            }
        }



        #endregion
    }
}
