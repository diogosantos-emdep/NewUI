using Emdep.Geos.Data.Common;
using Emdep.Geos.Services.Contracts;
using Emdep.Geos.UI.ServiceProcess;
using GeosWarehouseOrderMonitor.Logger;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;


namespace GeosWarehouseOrderMonitor
{
    public partial class GeosWarehouseOrderMonitor : ServiceBase
    {
        #region Services

        //IWarehouseService WarehouseService = new WarehouseServiceController((WarehouseCommon.Instance.Selectedwarehouse != null && WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl != null) ? WarehouseCommon.Instance.Selectedwarehouse.Company.ServiceProviderUrl : GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());
        IWarehouseService WarehouseService = new WarehouseServiceController(Properties.Settings.Default.SERVICE_PROVIDER_URL);
        IWorkbenchStartUp WorkbenchStartUp = new WorkbenchServiceController(Properties.Settings.Default.SERVICE_PROVIDER_URL);
        #endregion //End Of Services

        #region Declaration
        private Timer timer = null;
        static readonly object _object = new object();
        string TimeZoneSetting = string.Empty;
        DateTime timeInTz;
        List<Warehouses> warehouseList;
        List<GeosProvider> geosServiceProvider;
        #endregion

        #region Properties
        public List<Warehouses> WarehouseList
        {
            get { return warehouseList; }
            set
            {
                warehouseList = value;
            }
        }

        public List<GeosProvider> GeosServiceProvider
        {
            get
            {
                return geosServiceProvider;
            }
            set
            {
                geosServiceProvider = value;
            }
        }

        List<WarehousePurchaseOrder> purchaseOrderList;
        public List<WarehousePurchaseOrder> PurchaseOrderList
        {
            get
            {
                return purchaseOrderList;
            }
            set
            {
                purchaseOrderList = value;
            }
        }
        
        #endregion

        #region Constructor
        public GeosWarehouseOrderMonitor()
        {
            InitializeComponent();
            CreateIfNotExists();
            //ServiceMethod();
            Log4NetLogger.Logger.Log(string.Format("GeosWarehouseOrderMonitor Constructor Executed.... "), category: Category.Info, priority: Priority.Low);
        }
        #endregion

        #region Methods
        private void ServiceMethod()
        {
            try
            {
                Log4NetLogger.Logger.Log("ServiceMethod() started.", Category.Info, Priority.Low);
                FillWarehouseList();
                ProcessPurchaseOrdersByPlant();
                Log4NetLogger.Logger.Log("ServiceMethod() executed.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"ServiceMethod() - Error: {ex}", Category.Exception, Priority.Low);
            }
            finally
            {
                if (timer != null)
                    timer.Enabled = true;
            }

        }
        private void FillWarehouseList()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method FillWarehouseList...", category: Category.Info, priority: Priority.Low);

                if (WarehouseList == null)
                    WarehouseList = WarehouseService.GetAllWarehouseForScheduleEvents();

                var activePlantSetting = WarehouseService.GetGeosAppSettings(168);
                List<long> IdActiveCompany = new List<long>();
                if (string.IsNullOrEmpty(activePlantSetting.DefaultValue))
                {
                    Log4NetLogger.Logger.Log("No active plants found in settings.", Category.Info, priority: Priority.Low);
                    WarehouseList = new List<Warehouses>();
                    return;
                }
                else
                {
                    activePlantSetting.DefaultValue.Split(',').ToList().ForEach(s => IdActiveCompany.Add(Convert.ToInt32(s)));
                    WarehouseList = WarehouseList.Where(w => IdActiveCompany.Contains(w.IdSite)).ToList();
                }
                Log4NetLogger.Logger.Log("Method FillWarehouseList() executed successfully", Category.Info, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error in FillWarehouseList() - {0}", ex.ToString()), Category.Exception, Priority.Low);
            }
        }
        private void ProcessPurchaseOrdersByPlant()
        {
            try
            {
                Log4NetLogger.Logger.Log("Method ProcessPurchaseOrdersByPlant...", category: Category.Info, priority: Priority.Low);
                if (WarehouseList != null && WarehouseList?.Count > 0)
                {
                    try
                    {
                        foreach (var warehouse in WarehouseList)
                        {
                            // Fetch all purchase orders for the current warehouse
                            WarehouseService = new WarehouseServiceController(warehouse.Company.ServiceProviderUrl);
                            PurchaseOrderList = WarehouseService.GetPurchaseOrdersByRegionalWarehouse_V2680(warehouse.IdWarehouse);

                            if (PurchaseOrderList == null)
                                continue;

                            foreach (WarehousePurchaseOrder po in PurchaseOrderList)
                            {
                                try
                                {
                                        WarehouseService = new WarehouseServiceController(po.Warehouse.Company.ServiceProviderUrl);

                                        if (!WarehouseService.CheckCustomerPurchasOrderExists(po.Code))
                                        {
                                            // Initialize quotation and related objects
                                            Quotation quotation = new Quotation { Offer = new Offer() };
                                            var companyname = po.Warehouse.Company.Name;
                                            po.Warehouse = warehouse;

                                            Offer offer = new Offer();

                                            int offerType = (warehouse.IdSite == po.IdSite) ? 9 : 1;
                                            string prefix = (offerType == 9) ? "IW" : "OT";

                                            offer.IdOfferType = (byte?)offerType;
                                            quotation.Offer.IdOfferType = (byte?)offerType;


                                        if (prefix == "OT")
                                        {
                                            quotation.Offer.Number = WarehouseService.GetNextNumberOfOfferFromCounters(
                                                        $"{prefix}{DateTime.Now:yyyy}{(po.EmdepSite?.Code ?? "")}",
                                                        offerType
                                                    );
                                            quotation.Offer.Code = $"{DateTime.Now:yyyy}{(po.EmdepSite?.Code ?? "")}{"-"}{quotation.Offer.Number}";
                                        }
                                        else
                                        {
                                            quotation.Offer.Number = WarehouseService.GetNextNumberOfOfferFromCounters(
                                                                    $"{prefix}{DateTime.Now:yy}{(po.EmdepSite?.Code ?? "")}",
                                                                    offerType
                                                                );
                                            quotation.Offer.Code = $"{prefix}{DateTime.Now:yy}{(po.EmdepSite?.Code ?? "")}{quotation.Offer.Number}";
                                        }
                                        //quotation.Offer.Number = WarehouseService.GetNextNumberOfOfferFromCounters($"{prefix}{DateTime.Now:yy}{po.EmdepSite.Code}", offerType);
                                        //quotation.Offer.Code = $"{prefix}{DateTime.Now:yy}{po.EmdepSite.Code}{quotation.Offer.Number}";
                                        // Populate offer details
                                        quotation.Offer.IdCurrency = po.IdCurrency;
                                            quotation.Offer.Year = DateTime.Now.Year;
                                            quotation.Offer.IdCustomer = Convert.ToInt32(warehouse.IdSite);
                                            quotation.Offer.CustomerGroup = po.CustomerName;
                                            quotation.Offer.Rfq = po.Code;
                                            quotation.Offer.IdSalesOwner = Convert.ToInt32(po.CreatedBy);
                                            quotation.Offer.AttachmentBytes = po.AttachmentBytes;
                                            quotation.Offer.AttachmentFileName = "PO_" + po.Code + ".pdf";
                                            quotation.Offer.IDSiteCreatedBy = po.IdSite;
                                            quotation.Offer.Site = new Company
                                            {
                                                IdCompany = Convert.ToInt32(warehouse.IdSite),
                                                IdCurrency = Convert.ToInt32(po.IdCurrency),
                                                FullName = companyname
                                            };

                                            quotation.Offer.Value = po.WarehousePurchaseOrderItems?.Sum(i => i.TotalPrice) ?? 0;
                                            if (offer.IdOfferType == 9)
                                                quotation.Offer.Title = $"Material planta" + $" (TER-{po.CreatedIn:MMM}".ToUpper() + $"-{po.Code}-CLIENTE) + 1 Material Plantas";
                                            else
                                                quotation.Offer.Title = $"Material planta" + $" ({po.CreatedIn:MMM}".ToUpper() + $"-{po.Code}-CLIENTE) + 1 Material Plantas";

                                            // Initialize revisions
                                            quotation.Revisions = new List<Revision>();

                                            Revision revision = new Revision
                                            {
                                                NumRevision = 1,
                                                IdCurrency = po.IdCurrency,
                                                Items = new Dictionary<string, RevisionItem>()
                                            };

                                            // Process purchase order items
                                            if (po.WarehousePurchaseOrderItems != null)
                                            {
                                                int itemIndex = 1;
                                                foreach (var poItem in po.WarehousePurchaseOrderItems)
                                                {
                                                    try
                                                    {
                                                        RevisionItem revisionItem = new RevisionItem
                                                        {
                                                            WarehouseProduct = new WarehouseProduct(),
                                                            CustomerComment = poItem.Description,
                                                            Quantity = (decimal)poItem.Quantity,
                                                            UnitPrice = (decimal)poItem.UnitPrice,
                                                            ArticlewarehouseProduct = new ArticleProduct
                                                            {
                                                                Article = new Article
                                                                {
                                                                    IdArticle = poItem.IdArticle,
                                                                    Quantity = poItem.Quantity,
                                                                    Description = poItem.Description,
                                                                    IsBatch = poItem.Article.IsBatch,
                                                                }
                                                            }
                                                        };

                                                        revision.Items.Add(itemIndex.ToString(), revisionItem);
                                                        itemIndex++;
                                                    }
                                                    catch (ServiceUnexceptedException ex)
                                                    {
                                                        Log4NetLogger.Logger.Log("Get an error in ProcessPurchaseOrdersByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                                    }
                                                }
                                            }

                                            Revision revision0 = (Revision)revision.Clone();
                                            revision0.NumRevision = 0;
                                            revision0.Closed = DateTime.Now;
                                            revision0.ReviewedBy = 164;
                                            quotation.Revisions.Add(revision0);
                                            quotation.Revisions.Add(revision);

                                            // Fill quotation header details
                                            quotation.Code = quotation.Offer.Code + "M";
                                            if (offer.IdOfferType == 9)
                                                quotation.Description = $"Material planta" + $" (TER-{po.CreatedIn:MMM}".ToUpper() + $"-{po.Code}-CLIENTE) + 1 Material Plantas";
                                            else
                                                quotation.Description = $"Material planta" + $" ({po.CreatedIn:MMM}".ToUpper() + $"-{po.Code}-CLIENTE) + 1 Material Plantas";

                                            quotation.ProjectName = po.Code;
                                            quotation.IdCustomer = Convert.ToInt32(warehouse.IdSite);
                                            quotation.IdCurrency = po.IdCurrency;
                                            quotation.Year = po.CreatedIn.Year;
                                            quotation.Number = quotation.Offer.Number;
                                            quotation.Ots = new List<Ots>();

                                            // Initialize OTs
                                            Ots ot = new Ots
                                            {
                                                Number = Convert.ToInt32(quotation.Offer.Number),
                                                IdCurrency = po.IdCurrency,
                                                Code = GetOTCode(po.EmdepSite?.Code ?? "", quotation),
                                                IdSite = Convert.ToInt32(po.IdSite),
                                                NumOT = 1
                                            };
                                            quotation.Ots.Add(ot);
                                            // Create corresponding customer purchase order
                                            CustomerPurchaseOrder customerPO = new CustomerPurchaseOrder
                                            {
                                                IdSite = Convert.ToInt32(warehouse.IdSite),
                                                Code = po.Code,
                                                Value = po.WarehousePurchaseOrderItems?.Sum(i => i.TotalPrice) ?? 0,
                                                IdCurrency = Convert.ToByte(po.IdCurrency),
                                                ReceivedBy = po.Creator?.Name,
                                                IdSender = Convert.ToInt32(po.CreatedBy)
                                            };

                                            // Insert offer if not already existing
                                            if (!WarehouseService.CheckIfOfferExists(customerPO))
                                            {
                                                var idoffer = WarehouseService.InsertOffer_V2680(quotation, customerPO);
                                                if (idoffer > 0)
                                                {
                                                    WarehouseService.UpdateNextOfferNumberToCounters(Convert.ToInt32(offer.IdOfferType));
                                                }
                                            }
                                        }
                                    }
                                catch (ServiceUnexceptedException ex)
                                {
                                    Log4NetLogger.Logger.Log("Get an error in ProcessPurchaseOrdersByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                                }
                            }
                        }
                    }
                    catch (FaultException<ServiceException> ex)
                    {
                        Log4NetLogger.Logger.Log("Get an error in ProcessPurchaseOrdersByPlant() method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                    }
                    catch (ServiceUnexceptedException ex)
                    {
                        Log4NetLogger.Logger.Log("Get an error in ProcessPurchaseOrdersByPlant() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                    }
                }
                else
                {
                    PurchaseOrderList = new List<WarehousePurchaseOrder>();
                }

            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log("Get an error in ProcessPurchaseOrdersByPlant() method " + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
        }
        public string GetOTCode(string plantCode, Quotation q)
        {
            string OTCode = q.Code;            

            if (IsSupply(OTCode))
            {
                OTCode = q.Code.Substring(0, 4);
                OTCode += plantCode + "-" + q.Code.Substring(q.Code.IndexOf("-") + 1);
            }
            else if (IsComplaint(OTCode))
            {
                OTCode = q.Code.Substring(0, 3);
                if (Char.IsDigit(Convert.ToChar(q.Code.Substring(q.Code.Length - 1))))
                    OTCode += plantCode + q.Code.Substring(q.Code.Length - 3);
                else if (q.Code.Length == 8 || q.Code.Length == 10)
                    OTCode += plantCode + q.Code.Substring(q.Code.Length - 5);
                else
                    OTCode += plantCode + q.Code.Substring(q.Code.Length - 4);
            }
            else if ((IsInternalOrder(OTCode)) || (IsInternalEmdep(OTCode)) || (IsEngenieeringDepartment(OTCode)) || IsITO(OTCode) || IsIS(OTCode))
            {
                OTCode = q.Code.Substring(0, 4);
                if (Char.IsDigit(Convert.ToChar(q.Code.Substring(q.Code.Length - 1))))
                    OTCode += plantCode + q.Code.Substring(q.Code.Length - 3);
                else if (q.Code.Length == 9 || q.Code.Length == 11)
                    OTCode += plantCode + q.Code.Substring(q.Code.Length - 5);
                else
                    OTCode += plantCode + q.Code.Substring(q.Code.Length - 4);
            }
            else if (IsTechnicianMaterials(OTCode))
            {
                OTCode = q.Code.Substring(0, 3);
                OTCode += plantCode + q.Code.Substring(q.Code.IndexOf("-") + 1);
            }

            return OTCode;
        }
        private bool IsSupply(string code)
        {
            Regex exp = new Regex(@"^\d{4}-\d{4}$|^\d{4}-\d{4}[A-z]$|^\d{4}[A-z]{2}-\d{4}$|^\d{4}[A-z]{2}-\d{4}[A-z]$|^\d{4}-\d{5}$|^\d{4}-\d{5}[A-z]$|^\d{4}[A-z]{2}-\d{5}$|^\d{4}[A-z]{2}-\d{5}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsComplaint(string code)
        {
            Regex exp = new Regex(@"^C\d{5}$|^C\d{5}[A-z]$|^C\d{2}[A-z]{2}\d{3}$|^C\d{2}[A-z]{2}\d{3}[A-z]$|^C\d{6}[A-z]$|^C\d{2}[A-z]{2}\d{4}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsInternalOrder(string code)
        {
            Regex exp = new Regex(@"^IC\d{5}$|^IC\d{5}[A-z]$|^IC\d{2}[A-z]{2}\d{3}$|^IC\d{2}[A-z]{2}\d{3}[A-z]$|^IC\d{2}[A-z]{2}\d{4}[A-z]$|^IC\d{6}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsTechnicianMaterials(string code)
        {
            Regex exp = new Regex(@"^T\d{2}-d{3}$|^T\d{2}-d{3}[A-z]$|^T\d{2}[A-z]{2}-\d{3}$|^T\d{2}[A-z]{2}-\d{3}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsInternalEmdep(string code)
        {
            Regex exp = new Regex(@"^IE\d{5}$|^IE\d{5}[A-z]$|^IE\d{2}[A-z]{2}\d{3}$|^IE\d{2}[A-z]{2}\d{3}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsITO(string code)
        {
            Regex exp = new Regex(@"^IT\d{5}$|^IT\d{5}[A-z]$|^IT\d{2}[A-z]{2}\d{3}$|^IT\d{2}[A-z]{2}\d{3}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsIS(string code)
        {
            Regex exp = new Regex(@"^IS\d{5}$|^IS\d{5}[A-z]$|^IS\d{2}[A-z]{2}\d{3}$|^IS\d{2}[A-z]{2}\d{3}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        private bool IsEngenieeringDepartment(string code)
        {
            Regex exp = new Regex(@"^RD\d{5}$|^RD\d{5}[A-z]$|^RD\d{2}[A-z]{2}\d{3}$|^RD\d{2}[A-z]{2}\d{3}[A-z]$");
            if (exp.IsMatch(code))
                return true;
            else
                return false;
        }
        void CreateIfNotExists()
        {
            try
            {
                if (Log4NetLogger.Logger == null)
                {
                    string ApplicationLogFilePath = System.IO.Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "log4net.config");
                    string log4netConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                        <configuration>
                                          <configSections>
                                            <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net"" />
                                          </configSections>
                                          <log4net debug=""true"">
                                            <appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
                                              <file value=""C:\Temp\Emdep\Geos\GeosWarehouseOrderMonitorLogs.txt""/>
                                              <appendToFile value=""true"" />
                                              <rollingStyle value=""Size"" />
                                              <maxSizeRollBackups value=""10"" />
                                              <maximumFileSize value=""10MB"" />
                                              <staticLogFileName value=""true"" />
                                              <layout type=""log4net.Layout.PatternLayout"">
                                                <conversionPattern value=""%-5p %d %5rms - %m%n"" />
                                              </layout>
                                            </appender>
                                            <root>
                                              <level value=""Info"" />
                                              <appender-ref ref=""RollingLogFileAppender"" />
                                            </root>
                                          </log4net>
                                        </configuration>";

                    FileInfo file = new FileInfo(ApplicationLogFilePath);
                    Log4NetLogger.Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);

                    if (!File.Exists(ApplicationLogFilePath))
                    {
                        File.WriteAllText(ApplicationLogFilePath, log4netConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("ERROR in  GeosWarehouseOrderMonitor() - CreateIfNotExists(). ErrorMessage- {0}", ex.ToString()), category: Category.Exception, priority: Priority.Low);
            }
        }
        #endregion

        #region Startup Methods
        protected override void OnStart(string[] args)
        {
            Log4NetLogger.Logger.Log(string.Format("OnStart() Service Starting"), category: Category.Info, priority: Priority.Low);

            InitiaizeOnStartService();

            Log4NetLogger.Logger.Log(string.Format("OnStart() Service Executed"), category: Category.Info, priority: Priority.Low);
        }

        internal void InitiaizeOnStartService()
        {
            try
            {
                timer = new Timer();
                this.timer.Interval = Convert.ToInt32(Properties.Settings.Default.INTERVAL_REFRESH);
                this.timer.Elapsed += new ElapsedEventHandler(this.GeosWarehouseOrderMonitor_Tick);
                timer.Enabled = true;
            }
            catch (FaultException<ServiceException> ex)
            {
                Log4NetLogger.Logger.Log(string.Format("InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Detail.ErrorMessage), category: Category.Exception, priority: Priority.Low);
            }
            catch (ServiceUnexceptedException ex)
            {
                Log4NetLogger.Logger.Log(string.Format("InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("InitiaizeOnStartService() -Timer- FaultException - {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
            }
        }

        protected override void OnStop()
        {
            Log4NetLogger.Logger.Log(string.Format("OnStop() Service Stopping"), category: Category.Info, priority: Priority.Low);

            if (timer != null)
                timer.Enabled = false;

            Log4NetLogger.Logger.Log(string.Format("OnStop() Service Stoped"), category: Category.Info, priority: Priority.Low);
        }


        private void GeosWarehouseOrderMonitor_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                Log4NetLogger.Logger.Log("GeosWarehouseOrderMonitor_Tick() started.", Category.Info, Priority.Low);
                if (timer != null)
                    timer.Enabled = false;

                lock (_object)
                {
                    ServiceMethod();
                }
                Log4NetLogger.Logger.Log("GeosWarehouseOrderMonitor_Tick() executed.", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log($"GeosWarehouseOrderMonitor_Tick() - Error: {ex}", Category.Exception, Priority.Low);
            }
            finally
            {
                if (timer != null)
                    timer.Enabled = true;
            }

        }

        #endregion
    }
}
