using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.Epc;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Data.DataAccess;
using MySql.Data.MySqlClient;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class PCMManager
    {
        #region Product Catalogue Manager

        #region Add methods

        /// <summary>
        /// This method is used to insert Catalogue Item
        /// </summary>
        /// <param name="catalogueItem">Get catalogue item details.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public CatalogueItem AddCatalogueItem(CatalogueItem catalogueItem, string MainServerConnectionString, string CatalogueItemAttachedDocPath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    string lastestCatalogueItemCode = "";
                    lastestCatalogueItemCode = GetLatestCatalogueItemCode(MainServerConnectionString);
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItem_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        if (!string.IsNullOrEmpty(lastestCatalogueItemCode) && lastestCatalogueItemCode != catalogueItem.Code)
                        {
                            mySqlCommand.Parameters.AddWithValue("_Code", lastestCatalogueItemCode);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_Code", catalogueItem.Code);
                        }

                        mySqlCommand.Parameters.AddWithValue("_IdTemplate", catalogueItem.IdTemplate);
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", catalogueItem.IdCPType);
                        mySqlCommand.Parameters.AddWithValue("_Name", catalogueItem.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", catalogueItem.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", catalogueItem.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", catalogueItem.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", catalogueItem.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", catalogueItem.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", catalogueItem.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", catalogueItem.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", catalogueItem.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", catalogueItem.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", catalogueItem.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", catalogueItem.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", catalogueItem.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", catalogueItem.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", catalogueItem.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_parent", catalogueItem.Parent);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", catalogueItem.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsEnabled", catalogueItem.IsEnabled);

                        catalogueItem.IdCatalogueItem = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (catalogueItem.IdCatalogueItem > 0)
                    {
                        AddWaysByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, 1, catalogueItem.WayList);
                        AddDetectionsByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, 2, catalogueItem.DetectionList);
                        AddOptionsByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, 3, catalogueItem.OptionList);
                        AddSparePartsByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, 4, catalogueItem.SparePartList);
                        AddConnectorFamiliesByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, catalogueItem.FamilyList);
                        AddCatalogueItemAttachedDocByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, catalogueItem.FileList, CatalogueItemAttachedDocPath);
                        AddCatalogueItemAttachedLinkByCatalogueItem(MainServerConnectionString, catalogueItem.IdCatalogueItem, catalogueItem.CatalogueItemAttachedLinkList);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddCatalogueItem(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return catalogueItem;
        }

        /// <summary>
        /// This method is to insert ways by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get detection type id.</param>
        /// <param name="WayList">Get way list details.</param>
        public void AddWaysByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<Ways> WayList)
        {
            try
            {
                if (WayList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Ways wayList in WayList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", wayList.IdWays);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddWaysByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to insert detections by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get detection type id.</param>
        /// <param name="DetectionList">Get detection list details.</param>
        public void AddDetectionsByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<Detections> DetectionList)
        {
            try
            {
                if (DetectionList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Detections detectionList in DetectionList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionsByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to insert options by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get detection type id.</param>
        /// <param name="OptionList">Get option list details.</param>
        public void AddOptionsByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<Options> OptionList)
        {
            try
            {
                if (OptionList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Options optionList in OptionList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddOptionsByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to insert spare parts by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get detection type id.</param>
        /// <param name="SparePartList">Get spare part list details.</param>
        public void AddSparePartsByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<SpareParts> SparePartList)
        {
            try
            {
                if (SparePartList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (SpareParts sparePartsList in SparePartList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", sparePartsList.IdSpareParts);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddSparePartsByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to insert connector families by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="FamilyList">Get family list details.</param>
        public void AddConnectorFamiliesByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, List<ConnectorFamilies> FamilyList)
        {
            try
            {
                if (FamilyList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ConnectorFamilies familyList in FamilyList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCatalogueItem_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_IdFamily", familyList.IdFamily);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddConnectorFamiliesByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert Product Type
        /// </summary>
        /// <param name="productType">Get product type details.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public ProductTypes AddProductType(ProductTypes productType, string MainServerConnectionString, string ProductTypeImagePath, string ProductTypeAttachedDocPath, string PCMConnectionString)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
            {
                try
                {
                    string lastestProductTypeReference = "";
                    lastestProductTypeReference = GetLatestProuductTypeReference(MainServerConnectionString);

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        if (!string.IsNullOrEmpty(lastestProductTypeReference) && lastestProductTypeReference != productType.Reference)
                        {
                            mySqlCommand.Parameters.AddWithValue("_Reference", lastestProductTypeReference);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_Reference", productType.Reference);
                        }

                        mySqlCommand.Parameters.AddWithValue("_Name", productType.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", productType.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", productType.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", productType.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", productType.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", productType.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", productType.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", productType.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", productType.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", productType.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", productType.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", productType.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", productType.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", productType.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", productType.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", productType.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_ExtraCost", productType.ExtraCost);
                        mySqlCommand.Parameters.AddWithValue("_MinimumTestPoints", productType.MinimumTestPoints);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", productType.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDefaultWayType", productType.IdDefaultWayType);
                        mySqlCommand.Parameters.AddWithValue("_Standard", productType.Standard);
                        mySqlCommand.Parameters.AddWithValue("_Code", productType.Code);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", productType.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", productType.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsEnabled", productType.IsEnabled);

                        productType.IdCPType = Convert.ToUInt64(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (productType.IdCPType > 0)
                    {
                        AddProductTypeByTemplate(MainServerConnectionString, productType.IdTemplate, productType.IdCPType);
                        AddWaysByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.WayList);
                        AddDetectionsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.DetectionList);
                        AddOptionsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.OptionList);
                        AddSparePartsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.SparePartList);
                        AddConnectorFamiliesByProductType(MainServerConnectionString, productType.IdCPType, productType.FamilyList);
                        AddProductTypeAttachedDocByProductType(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedDocList, ProductTypeAttachedDocPath);
                        AddProductTypeAttachedLinkByProductType(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedLinkList);
                        AddProductTypeImageByProductType(MainServerConnectionString, productType.IdCPType, productType.ProductTypeImageList, ProductTypeImagePath);
                        AddCustomersRegionsByProductType(MainServerConnectionString, productType.IdCPType, productType.CustomerList, productType.CreatedBy);
                        AddSitesByProductType(MainServerConnectionString, productType.IdCPType, productType.CustomerList, PCMConnectionString);
                        AddProductTypeLogEntry(MainServerConnectionString, productType.IdCPType, productType.ProductTypeLogEntryList);

                        AddCompatibilitiesByProductType(MainServerConnectionString, (byte)productType.IdCPType, productType.ProductTypeCompatibilityList);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return productType;
        }

        /// <summary>
        /// This method is used to insert ways, detections, options, spare parts
        /// </summary>
        /// <param name="detectionDetails">Get detection details.</param>
        /// <param name="DetectionAttachedDocPath">Get file path.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public DetectionDetails AddDetection(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_createdBy", detectionDetails.CreatedBy);
                        mySqlCommand.Parameters.AddWithValue("_createdIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);
                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        detectionDetails.IdDetections = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    if (detectionDetails.IdDetections > 0)
                    {
                        AddDetectionAttachedDocByDetection(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                        AddDetectionAttachedLinkByDetection(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                        AddDetectionImageByDetection(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                        AddCustomersRegionsByDetection(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerList, detectionDetails.IdDetectionType, detectionDetails.CreatedBy);
                        AddDetectionLogEntry(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return detectionDetails;
        }

        /// <summary>
        /// This method is used to insert product type by template
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="IdCPType">Get product type id.</param>
        public void AddProductTypeByTemplate(string MainServerConnectionString, UInt64 IdTemplate, UInt64 IdCPType)
        {
            try
            {
                if (IdTemplate > 0)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeByTemplate(). IdTemplate- {0} ErrorMessage- {1}", IdTemplate, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert product type Attached Doc
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeAttachedDoc">Get product Type Attached Doc details.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type Attached Doc Path.</param>
        public void AddProductTypeAttachedDocByProductType(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeAttachedDoc> ProductTypeAttachedDocList, string ProductTypeAttachedDocPath)
        {
            try
            {
                if (ProductTypeAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeAttachedDoc productTypeAttachedDocList in ProductTypeAttachedDocList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeAttachedDocList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedDocList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeAttachedDocList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeAttachedDocList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IdDocType", productTypeAttachedDocList.IdDocType);

                            productTypeAttachedDocList.IdCPTypeAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (productTypeAttachedDocList.IdCPTypeAttachedDoc > 0)
                            {
                                AddProductTypeAttachedDocToPath(productTypeAttachedDocList, ProductTypeAttachedDocPath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeAttachedDocByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add product type Attached Doc to path
        /// </summary>
        /// <param name="productTypeAttachedDoc">Get product Type Attached Doc details.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type Attached Doc Path.</param>
        public bool AddProductTypeAttachedDocToPath(ProductTypeAttachedDoc productTypeAttachedDoc, string ProductTypeAttachedDocPath)
        {
            if (productTypeAttachedDoc.ProductTypeAttachedDocInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ProductTypeAttachedDocPath, productTypeAttachedDoc.IdCPTypeAttachedDoc);
                    string filePath = completePath + "\\" + productTypeAttachedDoc.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }

                    File.WriteAllBytes(filePath, productTypeAttachedDoc.ProductTypeAttachedDocInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeAttachedDocToPath()- Filename - {0}. ErrorMessage- {1}", productTypeAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to insert files by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="CatalogueItemAttachedDocList">Get File list details.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get Catalogue Item Attached Doc Path.</param>
        public void AddCatalogueItemAttachedDocByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, List<CatalogueItemAttachedDoc> CatalogueItemAttachedDocList, string CatalogueItemAttachedDocPath)
        {
            try
            {
                if (CatalogueItemAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CatalogueItemAttachedDoc catalogueItemAttachedDocList in CatalogueItemAttachedDocList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItemAttachedDocs_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", catalogueItemAttachedDocList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemAttachedDocList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", catalogueItemAttachedDocList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", catalogueItemAttachedDocList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IdDocType", catalogueItemAttachedDocList.IdDocType);

                            catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc > 0)
                            {
                                AddCatalogueItemAttachedDocToPath(catalogueItemAttachedDocList, CatalogueItemAttachedDocPath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCatalogueItemAttachedDocByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add Catalogue Item Attached Doc to path
        /// </summary>
        /// <param name="CatalogueItemAttachedDoc">Get Catalogue Item Attached Doc details.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get Catalogue Item Attached Doc Path.</param>
        public bool AddCatalogueItemAttachedDocToPath(CatalogueItemAttachedDoc CatalogueItemAttachedDoc, string CatalogueItemAttachedDocPath)
        {
            if (CatalogueItemAttachedDoc.CatalogueItemAttachedDocInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", CatalogueItemAttachedDocPath, CatalogueItemAttachedDoc.IdCatalogueItemAttachedDoc);
                    string filePath = completePath + "\\" + CatalogueItemAttachedDoc.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, CatalogueItemAttachedDoc.CatalogueItemAttachedDocInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddCatalogueItemAttachedDocToPath()- Filename - {0}. ErrorMessage- {1}", CatalogueItemAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to insert files by Detection
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionAttachedDocList">Get File list details.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection Attached Doc Path.</param>
        public void AddDetectionAttachedDocByDetection(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedDoc> DetectionAttachedDocList, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedDoc detectionAttachedDocList in DetectionAttachedDocList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedDocList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);

                            detectionAttachedDocList.IdDetectionAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                            {
                                AddDetectionAttachedDocToPath(detectionAttachedDocList, DetectionAttachedDocPath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedDocByDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add Detection Attached Doc to path
        /// </summary>
        /// <param name="DetectionAttachedDoc">Get Detection Attached Doc details.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection Attached Doc Path.</param>
        public bool AddDetectionAttachedDocToPath(DetectionAttachedDoc DetectionAttachedDoc, string DetectionAttachedDocPath)
        {
            if (DetectionAttachedDoc.DetectionAttachedDocInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionAttachedDocPath, DetectionAttachedDoc.IdDetectionAttachedDoc);
                    string filePath = completePath + "\\" + DetectionAttachedDoc.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, DetectionAttachedDoc.DetectionAttachedDocInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedDocToPath()- Filename - {0}. ErrorMessage- {1}", DetectionAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to insert product type Attached Link
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product Type ID.</param>
        /// <param name="ProductTypeAttachedLinkList">Get product Type Attached Link details.</param>
        public void AddProductTypeAttachedLinkByProductType(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeAttachedLink> ProductTypeAttachedLinkList)
        {
            try
            {
                if (ProductTypeAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeAttachedLink productTypeAttachedLinkList in ProductTypeAttachedLinkList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_AttachedLinks_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_Name", productTypeAttachedLinkList.Name);
                            mySqlCommand.Parameters.AddWithValue("_Address", productTypeAttachedLinkList.Address);
                            mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedLinkList.Description);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeAttachedLinkList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeAttachedLink(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert Catalogue Item Attached Link
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get Catalogue Item ID.</param>
        /// <param name="CatalogueItemAttachedLinkList">Get Catalogue Item Attached Link details.</param>
        public void AddCatalogueItemAttachedLinkByCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem, List<CatalogueItemAttachedLink> CatalogueItemAttachedLinkList)
        {
            try
            {
                if (CatalogueItemAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CatalogueItemAttachedLink catalogueItemAttachedLinkList in CatalogueItemAttachedLinkList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItems_AttachedLinks_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                            mySqlCommand.Parameters.AddWithValue("_Name", catalogueItemAttachedLinkList.Name);
                            mySqlCommand.Parameters.AddWithValue("_Address", catalogueItemAttachedLinkList.Address);
                            mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemAttachedLinkList.Description);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", catalogueItemAttachedLinkList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCatalogueItemAttachedLinkByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert Detection Attached Link
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection ID.</param>
        /// <param name="DetectionAttachedLinkList">Get Detection Attached Link details.</param>
        public void AddDetectionAttachedLinkByDetection(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedLink> DetectionAttachedLinkList)
        {
            try
            {
                if (DetectionAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedLink detectionAttachedLinkList in DetectionAttachedLinkList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_Name", detectionAttachedLinkList.Name);
                            mySqlCommand.Parameters.AddWithValue("_Address", detectionAttachedLinkList.Address);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedLinkList.Description);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedLinkList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionAttachedLinkByDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert ways by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="WayList">Get way list details.</param>
        public void AddWaysByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Ways> WayList)
        {
            try
            {
                if (WayList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Ways wayList in WayList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", wayList.IdWays);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddWaysByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert detections by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="DetectionList">Get detection list details.</param>
        public void AddDetectionsByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Detections> DetectionList)
        {
            try
            {
                if (DetectionList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Detections detectionList in DetectionList)
                        {
                            if (detectionList.IdDetections > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);
                                mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionList.OrderNumber);

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert options by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="OptionList">Get option list details.</param>
        public void AddOptionsByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Options> OptionList)
        {
            try
            {
                if (OptionList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Options optionList in OptionList)
                        {
                            if (optionList.IdOptions > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);
                                mySqlCommand.Parameters.AddWithValue("_OrderNumber", optionList.OrderNumber);

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddOptionsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert spare parts by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="SparePartList">Get spare part list details.</param>
        public void AddSparePartsByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<SpareParts> SparePartList)
        {
            try
            {
                if (SparePartList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (SpareParts sparePartsList in SparePartList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", sparePartsList.IdSpareParts);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddSparePartsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert connector families by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="FamilyList">Get family list details.</param>
        public void AddConnectorFamiliesByProductType(string MainServerConnectionString, UInt64 IdCPType, List<ConnectorFamilies> FamilyList)
        {
            try
            {
                if (FamilyList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ConnectorFamilies familyList in FamilyList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCPType_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdFamily", familyList.IdFamily);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddConnectorFamiliesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert product type by template
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="TemplateList">Get template list details.</param>
        /// <param name="IdCPType">Get product type id.</param>
        public void AddTemplatesByProductType(string MainServerConnectionString, List<Template> TemplateList, UInt64 IdCPType)
        {
            try
            {
                if (TemplateList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Template templateList in TemplateList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", templateList.IdTemplate);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddTemplatesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert product type image
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeImage">Get product Type Image details.</param>
        /// <param name="ProductTypeImagePath">Get product type Image Path.</param>
        public void AddProductTypeImageByProductType(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeImage> ProductTypeImageList, string ProductTypeImagePath)
        {
            try
            {
                if (ProductTypeImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeImage productTypeImageList in ProductTypeImageList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeImageList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", productTypeImageList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeImageList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeImageList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Position", productTypeImageList.Position);

                            productTypeImageList.IdCPTypeImage = Convert.ToUInt64(mySqlCommand.ExecuteScalar());

                            if (productTypeImageList.IdCPTypeImage > 0)
                            {
                                AddProductTypeImageToPath(productTypeImageList, ProductTypeImagePath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeImageByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add product type image to path
        /// </summary>
        /// <param name="productTypeImage">Get product Type Image details.</param>
        /// <param name="ProductTypeImagePath">Get product type Image Path.</param>
        public bool AddProductTypeImageToPath(ProductTypeImage productTypeImage, string ProductTypeImagePath)
        {
            if (productTypeImage.ProductTypeImageInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ProductTypeImagePath, productTypeImage.IdCPTypeImage);
                    string filePath = completePath + "\\" + productTypeImage.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, productTypeImage.ProductTypeImageInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeImageToPath()- Filename - {0}. ErrorMessage- {1}", productTypeImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to insert detection image
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="DetectionImageList">Get detection Image details.</param>
        /// <param name="DetectionImagePath">Get detection Image Path.</param>
        public void AddDetectionImageByDetection(string MainServerConnectionString, UInt32 IdDetection, List<DetectionImage> DetectionImageList, string DetectionImagePath)
        {
            try
            {
                if (DetectionImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionImage detectionImageList in DetectionImageList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionImageList.SavedFileName);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionImageList.Description);
                            mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionImageList.OriginalFileName);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionImageList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Position", detectionImageList.Position);

                            detectionImageList.IdDetectionImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                            if (detectionImageList.IdDetectionImage > 0)
                            {
                                AddDetectionImageToPath(detectionImageList, DetectionImagePath);
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionImageByDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add detection image to path
        /// </summary>
        /// <param name="detectionImage">Get detection Image details.</param>
        /// <param name="DetectionImagePath">Get detection Image Path.</param>
        public bool AddDetectionImageToPath(DetectionImage detectionImage, string DetectionImagePath)
        {
            if (detectionImage.DetectionImageInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionImagePath, detectionImage.IdDetectionImage);
                    string filePath = completePath + "\\" + detectionImage.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    else
                    {
                        DirectoryInfo di = new DirectoryInfo(completePath);
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                    }
                    File.WriteAllBytes(filePath, detectionImage.DetectionImageInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDetectionImageToPath()- Filename - {0}. ErrorMessage- {1}", detectionImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        /// <summary>
        /// This method is used to insert product type log entry
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="LogList">Get log list details.</param>
        public void AddProductTypeLogEntry(string MainServerConnectionString, UInt64 IdCPType, List<ProductTypeLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByCptype_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddProductTypeLogEntry(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert detection group
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetectionType">Get detection type.</param>
        /// <param name="DetectionGroupList">Get detection Group details.</param>
        public void AddDetectionGroup(string MainServerConnectionString, UInt32 IdDetectionType, List<DetectionGroup> DetectionGroupList)
        {
            try
            {
                if (DetectionGroupList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionGroup detectionGroupList in DetectionGroupList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                            mySqlCommand.Parameters.AddWithValue("_Name", detectionGroupList.Name);
                            mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionGroupList.OrderNumber);
                            mySqlCommand.Parameters.AddWithValue("_Name_es", detectionGroupList.Name_es);
                            mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionGroupList.Name_fr);
                            mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionGroupList.Name_pt);
                            mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionGroupList.Name_ro);
                            mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionGroupList.Name_zh);
                            mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionGroupList.Name_ru);
                            mySqlCommand.Parameters.AddWithValue("_Description", detectionGroupList.Description);
                            mySqlCommand.Parameters.AddWithValue("_Description_es", detectionGroupList.Description_es);
                            mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionGroupList.Description_fr);
                            mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionGroupList.Description_pt);
                            mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionGroupList.Description_ro);
                            mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionGroupList.Description_zh);
                            mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionGroupList.Description_ru);
                            mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionGroupList.CreatedBy);
                            mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            detectionGroupList.IdGroup = Convert.ToUInt32(mySqlCommand.ExecuteScalar());
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionGroup(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert customers and regions by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="CustomerList">Get customer list details.</param>
        public void AddCustomersRegionsByProductType(string MainServerConnectionString, UInt64 IdCPType, List<RegionsByCustomer> CustomerList, UInt32 IdCreator)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (RegionsByCustomer customerList in CustomerList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_cptype_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomersRegionsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }


        /// <summary>
        /// This method is used to insert customers and regions by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="CustomerList">Get customer list details.</param>
        public void AddSitesByProductType(string MainServerConnectionString, UInt64 IdCPType, List<RegionsByCustomer> CustomersList, string PCMConnectionString)
        {
            try
            {
                if (CustomersList != null)
                {
                    List<Site> SitesAddCustomerList = new List<Site>();

                    try
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetSitesByCustomerAndRegion", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            foreach (RegionsByCustomer customerList in CustomersList)
                            {
                                mySqlCommand.Parameters.Clear();
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        if (reader["IdSite"] != DBNull.Value)
                                        {
                                            Site site = new Site();
                                            site.IdSite = Convert.ToUInt32(reader["IdSite"]);
                                            SitesAddCustomerList.Add(site);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error AddSitesByProductType - PCM_GetSitesByCustomerAndRegion. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        throw;
                    }

                    if (SitesAddCustomerList != null && SitesAddCustomerList.Count > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_SitesByCPType_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            SitesAddCustomerList = SitesAddCustomerList.Distinct().ToList();
                            foreach (Site site in SitesAddCustomerList)
                            {
                                try
                                {
                                    mySqlCommand.Parameters.Clear();
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdSite", site.IdSite);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error AddSitesByProductType - PCM_SitesByCPType_Insert. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddSitesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert customers and regions by detection
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="CustomerList">Get customer list details.</param>
        /// <param name="IdDetectiontype">Get detection type id.</param>
        public void AddCustomersRegionsByDetection(string MainServerConnectionString, UInt32 IdDetection, List<RegionsByCustomer> CustomerList, UInt32 IdDetectiontype, UInt32 IdCreator)
        {
            try
            {
                if (CustomerList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (RegionsByCustomer customerList in CustomerList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                            mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCustomersRegionsByDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to insert detection log entry
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="IdDetectionType">Get detection type id.</param>
        /// <param name="LogList">Get log list details.</param>
        public void AddDetectionLogEntry(string MainServerConnectionString, UInt32 IdDetection, List<DetectionLogEntry> LogList, UInt32 IdDetectionType)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByDetection_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDetectionLogEntry(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }


        /// <summary>
        /// This method is used to insert pcm article categories
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="pcmArticleCategory">get pcm article category details.</param>
        public PCMArticleCategory AddPCMArticleCategory(string MainServerConnectionString, PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", pcmArticleCategory.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", pcmArticleCategory.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", pcmArticleCategory.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", pcmArticleCategory.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", pcmArticleCategory.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", pcmArticleCategory.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", pcmArticleCategory.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", pcmArticleCategory.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", pcmArticleCategory.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", pcmArticleCategory.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", pcmArticleCategory.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", pcmArticleCategory.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", pcmArticleCategory.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", pcmArticleCategory.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                        mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", pcmArticleCategory.IdArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                        mySqlCommand.Parameters.AddWithValue("_IsLeaf", pcmArticleCategory.IsLeaf);
                        mySqlCommand.Parameters.AddWithValue("_IdCreator", pcmArticleCategory.IdCreator);

                        pcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    //update position of pcm article categories
                    UpdatePCMArticleCategoryPosition(MainServerConnectionString, pcmArticleCategoryList, pcmArticleCategory.IdCreator);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return pcmArticleCategory;
        }


        /// <summary>
        /// This method is used to insert Compatibilities by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="CompatibilityList">Get Compatibility List details.</param>
        public bool AddCompatibilitiesByProductType(string MainServerConnectionString, byte IdCPType, List<ProductTypeCompatibility> CompatibilityList)
        {
            bool isSave = false;
            try
            {
                if (CompatibilityList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeCompatibility productTypeCompatibility in CompatibilityList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_cptype_compatibilities_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdCPtype", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdCPtypeCompatibility", productTypeCompatibility.IdCPtypeCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_IdArticleCompatibility", productTypeCompatibility.IdArticleCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_IdTypeCompatibility", productTypeCompatibility.IdTypeCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_MinimumElements", productTypeCompatibility.MinimumElements);
                            mySqlCommand.Parameters.AddWithValue("_MaximumElements", productTypeCompatibility.MaximumElements);
                            if (productTypeCompatibility.IdRelationshipType == 0)
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", productTypeCompatibility.IdRelationshipType);
                            }
                            mySqlCommand.Parameters.AddWithValue("_Quantity", productTypeCompatibility.Quantity);
                            mySqlCommand.Parameters.AddWithValue("_Remarks", productTypeCompatibility.Remarks);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", productTypeCompatibility.IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                    isSave = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCompatibilitiesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isSave;
        }

        /// <summary>
        /// This method is used to insert Compatibilities by Article
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdArticle">Get Article id.</param>
        /// <param name="CompatibilityList">Get Compatibility List details.</param>
        public bool AddCompatibilitiesByArticle(string MainServerConnectionString, UInt32 IdArticle, List<ArticleCompatibility> CompatibilityList)
        {
            bool isSave = false;
            try
            {
                if (CompatibilityList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ArticleCompatibility articleCompatibility in CompatibilityList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdArticleCompatibility", articleCompatibility.IdArticleCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_IdTypeCompatibility", articleCompatibility.IdTypeCompatibility);
                            mySqlCommand.Parameters.AddWithValue("_MinimumElements", articleCompatibility.MinimumElements);
                            mySqlCommand.Parameters.AddWithValue("_MaximumElements", articleCompatibility.MaximumElements);
                            if (articleCompatibility.IdRelationshipType == 0)
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                            }
                            else
                            {
                                mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", articleCompatibility.IdRelationshipType);
                            }
                            mySqlCommand.Parameters.AddWithValue("_Quantity", articleCompatibility.Quantity);
                            mySqlCommand.Parameters.AddWithValue("_Remarks", articleCompatibility.Remarks);
                            mySqlCommand.Parameters.AddWithValue("_IdCreator", articleCompatibility.IdCreator);
                            mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCompatibilitiesByArticle(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isSave;
        }

        public void AddPCMArticleLogEntry(string MainServerConnectionString, UInt32 IdArticle, List<PCMArticleLogEntry> LogList)
        {
            try
            {
                if (LogList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleLogEntry logList in LogList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_LogEntriesByPCMArticle_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logList.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", logList.Datetime);
                            mySqlCommand.Parameters.AddWithValue("_Comments", logList.Comments);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleLogEntry(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public bool AddPCMArticleImageToPath(PCMArticleImage Image, string ImagePath, string ArticleReference)
        {
            if (Image.PCMArticleImageInBytes != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}\{1}", ImagePath, ArticleReference);
                    string filePath = completePath + "\\" + Image.IdPCMArticleImage + "_" + Image.SavedFileName;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    string[] filePaths = Directory.GetFiles(completePath, Image.IdPCMArticleImage + "_*", SearchOption.AllDirectories);
                    foreach (string file in filePaths)
                    {
                        File.Delete(file);
                    }
                    File.WriteAllBytes(filePath, Image.PCMArticleImageInBytes);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleImageToPath()- Filename - {0}. ErrorMessage- {1}", Image.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }
        #endregion

        #region Update methods

        /// <summary>
        /// This method is used to Update Catalogue Item
        /// </summary>
        /// <param name="catalogueItemModel">Get catalogue item details.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool UpdateCatalogueItem(CatalogueItem catalogueItemModel, string MainServerConnectionString, string CatalogueItemAttachedDocPath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItem_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", catalogueItemModel.IdCatalogueItem);
                        mySqlCommand.Parameters.AddWithValue("_IdTemplate", catalogueItemModel.IdTemplate);
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", catalogueItemModel.IdCPType);
                        mySqlCommand.Parameters.AddWithValue("_Name", catalogueItemModel.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", catalogueItemModel.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", catalogueItemModel.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", catalogueItemModel.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", catalogueItemModel.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", catalogueItemModel.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", catalogueItemModel.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemModel.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", catalogueItemModel.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", catalogueItemModel.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", catalogueItemModel.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", catalogueItemModel.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", catalogueItemModel.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", catalogueItemModel.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", catalogueItemModel.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_parent", catalogueItemModel.Parent);
                        mySqlCommand.Parameters.AddWithValue("_modifiedBy", catalogueItemModel.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_modifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add delete ways, detections, options, spare parts & families
                    AddDeleteWays(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, 1, catalogueItemModel.WayList);
                    AddDeleteDetections(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, 2, catalogueItemModel.DetectionList);
                    AddDeleteOptions(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, 3, catalogueItemModel.OptionList);
                    AddDeleteSpareParts(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, 4, catalogueItemModel.SparePartList);
                    AddDeleteFamilies(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, catalogueItemModel.FamilyList);
                    AddUpdateDeleteCatalogueItemFiles(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, catalogueItemModel.FileList, CatalogueItemAttachedDocPath);
                    AddUpdateDeleteCatalogueItemLinks(MainServerConnectionString, catalogueItemModel.IdCatalogueItem, catalogueItemModel.CatalogueItemAttachedLinkList);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", catalogueItemModel.IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update Product Type
        /// </summary>
        /// <param name="productType">Get product type details.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool UpdateProductType(ProductTypes productType, string MainServerConnectionString, string ProductTypeImagePath, string ProductTypeAttachedDocPath, string PCMConnectionString)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(10)))
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdCPType", productType.IdCPType);
                        mySqlCommand.Parameters.AddWithValue("_Name", productType.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", productType.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", productType.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", productType.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", productType.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", productType.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", productType.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", productType.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", productType.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", productType.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", productType.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", productType.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", productType.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", productType.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", productType.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", productType.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_ExtraCost", productType.ExtraCost);
                        mySqlCommand.Parameters.AddWithValue("_MinimumTestPoints", productType.MinimumTestPoints);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", productType.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDefaultWayType", productType.IdDefaultWayType);
                        mySqlCommand.Parameters.AddWithValue("_Standard", productType.Standard);
                        mySqlCommand.Parameters.AddWithValue("_Code", productType.Code);
                        mySqlCommand.Parameters.AddWithValue("_IdStatus", productType.IdStatus);
                        mySqlCommand.Parameters.AddWithValue("_modifiedBy", productType.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_modifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add delete template, ways, detections, options, spare parts & families & log
                    //AddDeleteTemplatesByProductType(MainServerConnectionString, productType.IdCPType, productType.TemplateList);

                    IsUpdateTemplateByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.IdTemplate_old);
                    AddDeleteWaysByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.WayList);
                    AddUpdateDeleteDetectionsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.DetectionList);
                    AddUpdateDeleteOptionsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.OptionList);
                    AddDeleteSparePartsByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate, productType.SparePartList);
                    AddDeleteFamiliesByProductType(MainServerConnectionString, productType.IdCPType, productType.FamilyList);
                    AddUpdateDeleteProductTypeFiles(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedDocList, ProductTypeAttachedDocPath);
                    AddUpdateDeleteProductTypeLinks(MainServerConnectionString, productType.IdCPType, productType.ProductTypeAttachedLinkList);
                    AddUpdateDeleteProductTypeImages(MainServerConnectionString, productType.IdCPType, productType.ProductTypeImageList, ProductTypeImagePath);
                    AddDeleteCustomersRegionsByProductType(MainServerConnectionString, productType.IdCPType, productType.CustomerList, productType.ModifiedBy);
                    AddDeleteSitesByProductType(MainServerConnectionString, productType.IdCPType, productType.CustomerList, PCMConnectionString);
                    AddProductTypeLogEntry(MainServerConnectionString, productType.IdCPType, productType.ProductTypeLogEntryList);
                    AddUpdateDeleteProductTypeCompatibilities(MainServerConnectionString, (byte)productType.IdCPType, productType.ProductTypeCompatibilityList);
                    if (productType.IdTemplate != productType.IdTemplate_old)
                    {
                        IsUpdateDWOSTemplateByProductType(MainServerConnectionString, productType.IdCPType, productType.IdTemplate);
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateProductType(). IdCPType- {0} ErrorMessage- {1}", productType.IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update ways, detections, options, spare parts
        /// </summary>
        /// <param name="detectionDetails">Get detection details.</param>
        /// <param name="DetectionAttachedDocPath">Get file path.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool UpdateDetection(DetectionDetails detectionDetails, string MainServerConnectionString, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    UInt32 IdGroup = 0;
                    if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3)
                    {
                        IdGroup = AddUpdateDeleteDetectionGroup(MainServerConnectionString, detectionDetails.IdDetectionType, detectionDetails.DetectionGroupList, detectionDetails.DetectionOrderGroup);
                    }

                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionDetails.IdDetections);
                        mySqlCommand.Parameters.AddWithValue("_Name", detectionDetails.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", detectionDetails.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionDetails.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionDetails.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionDetails.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionDetails.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionDetails.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", detectionDetails.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", detectionDetails.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionDetails.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionDetails.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionDetails.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionDetails.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionDetails.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionDetails.OrderNumber);
                        mySqlCommand.Parameters.AddWithValue("_NameToShow", detectionDetails.NameToShow);
                        mySqlCommand.Parameters.AddWithValue("_Family", detectionDetails.Family);
                        mySqlCommand.Parameters.AddWithValue("_WeldOrder", detectionDetails.WeldOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdTestType", detectionDetails.IdTestType);
                        mySqlCommand.Parameters.AddWithValue("_InfoLink", detectionDetails.InfoLink);
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", detectionDetails.IdDetectionType);
                        mySqlCommand.Parameters.AddWithValue("_Code", detectionDetails.Code);
                        mySqlCommand.Parameters.AddWithValue("_Orientation", detectionDetails.Orientation);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionDetails.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IsMandatoryVisualAid", detectionDetails.IsMandatoryVisualAid);

                        if (IdGroup == 0)
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionDetails.IdGroup);
                        }
                        else
                        {
                            mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);
                        }

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }

                    //add/update/delete ways, detections, options, spare parts
                    AddUpdateDeleteDetectionFiles(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedDocList, DetectionAttachedDocPath);
                    AddUpdateDeleteDetectionLinks(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionAttachedLinkList);
                    AddUpdateDeleteDetectionImages(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionImageList, DetectionImagePath);
                    AddDeleteCustomersRegionsByDetection(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.CustomerList, detectionDetails.IdDetectionType, detectionDetails.ModifiedBy);
                    AddDetectionLogEntry(MainServerConnectionString, detectionDetails.IdDetections, detectionDetails.DetectionLogEntryList, detectionDetails.IdDetectionType);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateDetection(). IdDetection- {0} ErrorMessage- {1}", detectionDetails.IdDetections, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update product type image
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeImage">Get product Type Image details.</param>
        /// <param name="ProductTypeImagePath">Get product type Image Path.</param>
        public bool IsUpdateProductTypeImage(ProductTypeImage productTypeImage, string ProductTypeImagePath, string MainServerConnectionString)
        {
            bool isUpdated = false;
            try
            {
                if (productTypeImage.ProductTypeImageInBytes != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCPTypeImage", productTypeImage.IdCPTypeImage);
                        mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeImage.SavedFileName);
                        mySqlCommand.Parameters.AddWithValue("_Description", productTypeImage.Description);
                        mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeImage.OriginalFileName);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", productTypeImage.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_Position", productTypeImage.Position);

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    AddProductTypeImageToPath(productTypeImage, ProductTypeImagePath);
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsUpdateProductTypeImage(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update product type Attached Doc
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeAttachedDoc">Get product Type Attached Doc details.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type Attached Doc Path.</param>
        public bool IsUpdateProductTypeAttachedDoc(ProductTypeAttachedDoc productTypeAttachedDoc, string ProductTypeAttachedDocPath, string MainServerConnectionString)
        {
            bool isUpdated = false;
            try
            {
                if (productTypeAttachedDoc.ProductTypeAttachedDocInBytes != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedDoc", productTypeAttachedDoc.IdCPTypeAttachedDoc);
                        mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeAttachedDoc.SavedFileName);
                        mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedDoc.Description);
                        mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeAttachedDoc.OriginalFileName);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", productTypeAttachedDoc.ModifiedBy);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_IdDocType", productTypeAttachedDoc.IdDocType);

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    AddProductTypeAttachedDocToPath(productTypeAttachedDoc, ProductTypeAttachedDocPath);
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsUpdateProductTypeAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update template by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product Type Id.</param>
        /// <param name="IdTemplate">Get template Id.</param>
        public bool IsUpdateTemplateByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, UInt64 IdTemplate_Old)
        {
            bool isUpdated = false;
            try
            {
                if (IdTemplate > 0)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        if (IdTemplate_Old == 0)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Update", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                            mySqlCommand.Parameters.AddWithValue("_IdTemplate_old", IdTemplate_Old);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsUpdateTemplateByProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }


        /// <summary>
        /// This method is used to Add/Update/Delete Product Type Links
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdPCMArticleCategory">Get PCM Article Category id.</param>
        /// <param name="ArticleList">Article List details.</param>
        public bool IsUpdatePCMArticleCategoryInArticle(string MainServerConnectionString, uint IdPCMArticleCategory, List<Articles> ArticleList)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (ArticleList != null)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            foreach (Articles Article in ArticleList)
                            {
                                if (Article.IdArticle > 0)
                                {
                                    if (Article.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                    {
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Update", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", null);
                                        mySqlCommand.Parameters.AddWithValue("_idArticle", Article.IdArticle);

                                        mySqlCommand.ExecuteNonQuery();
                                    }
                                    else if (Article.TransactionOperation == ModelBase.TransactionOperations.Add)
                                    {
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Update", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                                        mySqlCommand.Parameters.AddWithValue("_idArticle", Article.IdArticle);

                                        mySqlCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategoryInArticle(). IdPCMArticleCategory- {0} ErrorMessage- {1}", IdPCMArticleCategory, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update pcm article category
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="pcmArticleCategory">Get pcm article category details.</param>
        public bool IsUpdatePCMArticleCategory(string MainServerConnectionString, PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Name", pcmArticleCategory.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", pcmArticleCategory.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", pcmArticleCategory.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", pcmArticleCategory.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", pcmArticleCategory.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", pcmArticleCategory.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", pcmArticleCategory.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", pcmArticleCategory.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", pcmArticleCategory.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", pcmArticleCategory.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", pcmArticleCategory.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", pcmArticleCategory.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", pcmArticleCategory.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", pcmArticleCategory.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                        mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", pcmArticleCategory.IdArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                        mySqlCommand.Parameters.AddWithValue("_IsLeaf", pcmArticleCategory.IsLeaf);
                        mySqlCommand.Parameters.AddWithValue("_IdModifier", pcmArticleCategory.IdModifier);
                        mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    //update position of pcm article categories
                    UpdatePCMArticleCategoryPosition(MainServerConnectionString, pcmArticleCategoryList, pcmArticleCategory.IdModifier);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to Update PCM Article Category In Article With Status
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdPCMArticleCategory">Get PCM Article Category id.</param>
        /// <param name="Article">Article details.</param>
        public bool IsUpdatePCMArticleCategoryInArticleWithStatus(string MainServerConnectionString, uint IdPCMArticleCategory, Articles Article, string PCMArticleImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (Article != null && Article.IdArticle > 0 && IdPCMArticleCategory > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_ArticleCategoryWithStatus_Update", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                            mySqlCommand.Parameters.AddWithValue("_idArticle", Article.IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdPCMStatus", Article.IdPCMStatus);

                            mySqlCommand.ExecuteNonQuery();

                            mySqlConnection.Close();

                            isUpdated = true;
                        }

                        AddUpdateDeleteArticleCompatibilities(MainServerConnectionString, Article.IdArticle, Article.ArticleCompatibilityList);
                        AddPCMArticleLogEntry(MainServerConnectionString, Article.IdArticle, Article.PCMArticleLogEntiryList);
                        AddUpdateDeletePCMArticleImages(MainServerConnectionString, Article.IdArticle, Article.PCMArticleImageList, PCMArticleImagePath, Article.Reference);

                        isUpdated = true;
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategoryInArticleWithStatus(). IdArticle- {0} ErrorMessage- {1}", Article.IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        /// <summary>
        /// This method is used to update DWOS template by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product Type Id.</param>
        /// <param name="IdTemplate">Get template Id.</param>
        public bool IsUpdateDWOSTemplateByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            bool isUpdated = false;
            try
            {
                if (IdTemplate > 0)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_DWOSTemplateByCPType_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                        mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    isUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsUpdateDWOSTemplateByProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isUpdated;
        }
        #endregion

        #region Delete Methods

        /// <summary>
        /// This method is used to Delete Catalogue Item
        /// </summary>
        /// <param name="idCatalogueItem">Get catalogue item id.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool IsDeletedCatalogueItem(UInt32 idCatalogueItem, string MainServerConnectionString)
        {
            bool isDeleted = false;
            if (idCatalogueItem > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItem_Delete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", idCatalogueItem);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        isDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsDeletedCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", idCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to Delete Detections by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        public void DeleteDetectionsByIdCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Delete", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteDetectionsByIdCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to Delete connector families by catalogue item
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        public void DeleteConnectorFamiliesByIdCatalogueItem(string MainServerConnectionString, UInt32 IdCatalogueItem)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCatalogueItem_Delete", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteConnectorFamiliesByIdCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is to delete product type image
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeImage">Get product Type Image details.</param>
        /// <param name="ProductTypeImagePath">Get product type Image Path.</param>
        public bool IsDeleteProductTypeImage(ProductTypeImage productTypeImage, string ProductTypeImagePath, string MainServerConnectionString)
        {
            bool isDeleted = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Delete", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeImage", productTypeImage.IdCPTypeImage);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
                IsDeleteProductTypeImageFromPath(productTypeImage, ProductTypeImagePath);
                isDeleted = true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteProductTypeImage(). IdCPTypeImage- {0} ErrorMessage- {1}", productTypeImage.IdCPTypeImage, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete product type image from path
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeImage">Get product Type Image details.</param>
        /// <param name="ProductTypeImagePath">Get product type Image Path.</param>
        public bool IsDeleteProductTypeImageFromPath(ProductTypeImage productTypeImage, string ProductTypeImagePath)
        {
            try
            {
                if (ProductTypeImagePath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", ProductTypeImagePath, productTypeImage.IdCPTypeImage);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteProductTypeImageFromPath()- Filename - {0}. ErrorMessage- {1}", productTypeImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        /// <summary>
        /// This method is to delete product type Attached Doc
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeAttachedDoc">Get product Type Attached Doc details.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type Attached Doc Path.</param>
        public bool IsDeleteProductTypeAttachedDoc(ProductTypeAttachedDoc productTypeAttachedDoc, string ProductTypeAttachedDocPath, string MainServerConnectionString)
        {
            bool isDeleted = false;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Delete", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedDoc", productTypeAttachedDoc.IdCPTypeAttachedDoc);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                }
                IsDeleteProductTypeAttachedDocFromPath(productTypeAttachedDoc, ProductTypeAttachedDocPath);
                isDeleted = true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteProductTypeAttachedDoc(). IdCPTypeAttachedDoc- {0} ErrorMessage- {1}", productTypeAttachedDoc.IdCPTypeAttachedDoc, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete product type Attached Doc from path
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="productTypeAttachedDoc">Get product Type Attached Doc details.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type Attached Doc Path.</param>
        public bool IsDeleteProductTypeAttachedDocFromPath(ProductTypeAttachedDoc productTypeAttachedDoc, string ProductTypeAttachedDocPath)
        {
            try
            {
                if (ProductTypeAttachedDocPath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", ProductTypeAttachedDocPath, productTypeAttachedDoc.IdCPTypeAttachedDoc);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteProductTypeAttachedDocFromPath()- Filename - {0}. ErrorMessage- {1}", productTypeAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        /// <summary>
        /// This method is to delete catalogue Item Attached Doc from path
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="catalogueItemAttachedDoc">Get catalogue Item Attached Doc details.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get catalogue Item Attached Doc Path.</param>
        public bool IsDeleteCatalogueItemAttachedDocFromPath(CatalogueItemAttachedDoc catalogueItemAttachedDoc, string CatalogueItemAttachedDocPath)
        {
            try
            {
                if (CatalogueItemAttachedDocPath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", CatalogueItemAttachedDocPath, catalogueItemAttachedDoc.IdCatalogueItemAttachedDoc);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteCatalogueItemAttachedDocFromPath()- Filename - {0}. ErrorMessage- {1}", catalogueItemAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        /// <summary>
        /// This method is to delete Detection Attached Doc from path
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="DetectionAttachedDoc">Get Detection Attached Doc details.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection Attached Doc Path.</param>
        public bool IsDeleteDetectionAttachedDocFromPath(DetectionAttachedDoc DetectionAttachedDoc, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocPath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionAttachedDocPath, DetectionAttachedDoc.IdDetectionAttachedDoc);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteDetectionAttachedDocFromPath()- Filename - {0}. ErrorMessage- {1}", DetectionAttachedDoc.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        /// <summary>
        /// This method is used to Delete product type
        /// </summary>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool IsDeletedProductType(UInt64 IdCPType, string MainServerConnectionString)
        {
            bool isDeleted = false;
            if (IdCPType > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_Delete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        isDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsDeletedProductType(). IdProductType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }

        /// <summary>
        /// This method is to delete detection image from path
        /// </summary>
        /// <param name="DetectionImage">Get detection Image details.</param>
        /// <param name="DetectionImagePath">Get detection Image Path.</param>
        public bool IsDeleteDetectionImageFromPath(DetectionImage DetectionImage, string DetectionImagePath)
        {
            try
            {
                if (DetectionImagePath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", DetectionImagePath, DetectionImage.IdDetectionImage);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        Directory.Delete(completePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeleteDetectionImageFromPath()- Filename - {0}. ErrorMessage- {1}", DetectionImage.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }

        /// <summary>
        /// This method is used to Delete detection
        /// </summary>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        public bool IsDeletedDetection(UInt32 IdDetection, string MainServerConnectionString)
        {
            bool isDeleted = false;
            if (IdDetection > 0)
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Delete", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        isDeleted = true;
                    }
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsDeletedDetection(). IdProductType- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }
            return isDeleted;
        }

        public bool IsDeletePCMArticleImageFromPath(PCMArticleImage Image, string ImagePath, string ArticleReference)
        {
            try
            {
                if (ImagePath != null)
                {
                    string completePath = string.Format(@"{0}\{1}", ImagePath, ArticleReference);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        completePath = completePath + "\\" + Image.IdPCMArticleImage + "_" + Image.SavedFileName;

                        if (File.Exists(completePath))
                        {
                            File.Delete(completePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleImageFromPath()- Filename - {0}. ErrorMessage- {1}", Image.SavedFileName, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }
        #endregion

        #region Add / Update / Delete methods

        /// <summary>
        /// This method is used to Add/Delete families
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="FamilyList">The list of family.</param>
        public void AddDeleteFamilies(string MainServerConnectionString, UInt32 IdCatalogueItem, List<ConnectorFamilies> Families)
        {
            try
            {
                if (Families != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ConnectorFamilies familyList in Families)
                        {
                            if (familyList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCatalogueItem_By_IdFamily_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_idFamily", familyList.IdFamily);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (familyList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCatalogueItem_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_IdFamily", familyList.IdFamily);

                                familyList.IdFamily = Convert.ToUInt64(mySqlCommand.ExecuteScalar());
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteWays(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete ways
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get catalogue type id.</param>
        /// <param name="WaysList">The list of ways.</param>
        public void AddDeleteWays(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<Ways> WaysList)
        {
            try
            {
                if (WaysList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Ways wayList in WaysList)
                        {
                            if (wayList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_By_IdDetection_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_idDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_idDetection", wayList.IdWays);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (wayList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", wayList.IdWays);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteWays(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete detections
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get catalogue type id.</param>
        /// <param name="DetectionsList">The list of detections.</param>
        public void AddDeleteDetections(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<Detections> DetectionsList)
        {
            try
            {
                if (DetectionsList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Detections detectionList in DetectionsList)
                        {
                            if (detectionList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_By_IdDetection_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_idDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_idDetection", detectionList.IdDetections);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (detectionList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteDetections(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete options
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get catalogue type id.</param>
        /// <param name="OptionsList">The list of options.</param>
        public void AddDeleteOptions(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<Options> OptionsList)
        {
            try
            {
                Detections obj = new Detections();
                if (OptionsList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Options optionList in OptionsList)
                        {
                            if (optionList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_By_IdDetection_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_idDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_idDetection", optionList.IdOptions);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (optionList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteOptions(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete spare parts
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="IdDetectionType">Get catalogue type id.</param>
        /// <param name="SparePartsList">The list of spare parts.</param>
        public void AddDeleteSpareParts(string MainServerConnectionString, UInt32 IdCatalogueItem, UInt32 IdDetectionType, List<SpareParts> SparePartsList)
        {
            try
            {
                if (SparePartsList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (SpareParts sparePartsList in SparePartsList)
                        {
                            if (sparePartsList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_By_IdDetection_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_idCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_idDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_idDetection", sparePartsList.IdSpareParts);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (sparePartsList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCatalogueItem_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", sparePartsList.IdSpareParts);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSpareParts(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Catalogue Item Files
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get file path.</param>
        /// <param name="CatalogueItemAttachedDocList">The list of files.</param>
        public void AddUpdateDeleteCatalogueItemFiles(string MainServerConnectionString, UInt32 IdCatalogueItem, List<CatalogueItemAttachedDoc> CatalogueItemAttachedDocList, string CatalogueItemAttachedDocPath)
        {
            try
            {
                if (CatalogueItemAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CatalogueItemAttachedDoc catalogueItemAttachedDocList in CatalogueItemAttachedDocList)
                        {
                            if (catalogueItemAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItemAttachedDocs_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItemAttachedDoc", catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteCatalogueItemAttachedDocFromPath(catalogueItemAttachedDocList, CatalogueItemAttachedDocPath);
                            }
                            else if (catalogueItemAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItemAttachedDocs_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItemAttachedDoc", catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", catalogueItemAttachedDocList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemAttachedDocList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", catalogueItemAttachedDocList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", catalogueItemAttachedDocList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_IdDocType", catalogueItemAttachedDocList.IdDocType);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddCatalogueItemAttachedDocToPath(catalogueItemAttachedDocList, CatalogueItemAttachedDocPath);
                                }
                            }
                            else if (catalogueItemAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItemAttachedDocs_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", catalogueItemAttachedDocList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemAttachedDocList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", catalogueItemAttachedDocList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", catalogueItemAttachedDocList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_IdDocType", catalogueItemAttachedDocList.IdDocType);

                                catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (catalogueItemAttachedDocList.IdCatalogueItemAttachedDoc > 0)
                                {
                                    AddCatalogueItemAttachedDocToPath(catalogueItemAttachedDocList, CatalogueItemAttachedDocPath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteCatalogueItemFiles(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete product type Files
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCpType">Get product type id.</param>
        /// <param name="ProductTypeAttachedDocPath">Get file path.</param>
        /// <param name="ProductTypeAttachedDocList">The list of files.</param>
        public void AddUpdateDeleteProductTypeFiles(string MainServerConnectionString, UInt64 IdCpType, List<ProductTypeAttachedDoc> ProductTypeAttachedDocList, string ProductTypeAttachedDocPath)
        {
            try
            {
                if (ProductTypeAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeAttachedDoc productTypeAttachedDocList in ProductTypeAttachedDocList)
                        {
                            if (productTypeAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedDoc", productTypeAttachedDocList.IdCPTypeAttachedDoc);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteProductTypeAttachedDocFromPath(productTypeAttachedDocList, ProductTypeAttachedDocPath);
                            }
                            else if (productTypeAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (productTypeAttachedDocList.IdCPTypeAttachedDoc > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedDoc", productTypeAttachedDocList.IdCPTypeAttachedDoc);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeAttachedDocList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedDocList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeAttachedDocList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", productTypeAttachedDocList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_IdDocType", productTypeAttachedDocList.IdDocType);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddProductTypeAttachedDocToPath(productTypeAttachedDocList, ProductTypeAttachedDocPath);
                                }
                            }
                            else if (productTypeAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeAttachedDocs_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCpType);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeAttachedDocList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedDocList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeAttachedDocList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeAttachedDocList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_IdDocType", productTypeAttachedDocList.IdDocType);

                                productTypeAttachedDocList.IdCPTypeAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (productTypeAttachedDocList.IdCPTypeAttachedDoc > 0)
                                {
                                    AddProductTypeAttachedDocToPath(productTypeAttachedDocList, ProductTypeAttachedDocPath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteProductTypeFiles(). IdCpType- {0} ErrorMessage- {1}", IdCpType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Detection Files
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionAttachedDocPath">Get file path.</param>
        /// <param name="DetectionAttachedDocList">The list of files.</param>
        public void AddUpdateDeleteDetectionFiles(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedDoc> DetectionAttachedDocList, string DetectionAttachedDocPath)
        {
            try
            {
                if (DetectionAttachedDocList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedDoc detectionAttachedDocList in DetectionAttachedDocList)
                        {
                            if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", detectionAttachedDocList.IdDetectionAttachedDoc);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteDetectionAttachedDocFromPath(detectionAttachedDocList, DetectionAttachedDocPath);
                            }
                            else if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", detectionAttachedDocList.IdDetectionAttachedDoc);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionAttachedDocList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddDetectionAttachedDocToPath(detectionAttachedDocList, DetectionAttachedDocPath);
                                }
                            }
                            else if (detectionAttachedDocList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionAttachedDocs_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionAttachedDocList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedDocList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionAttachedDocList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedDocList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_IdDocType", detectionAttachedDocList.IdDocType);

                                detectionAttachedDocList.IdDetectionAttachedDoc = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (detectionAttachedDocList.IdDetectionAttachedDoc > 0)
                                {
                                    AddDetectionAttachedDocToPath(detectionAttachedDocList, DetectionAttachedDocPath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionFiles(). IdCatalogueItem- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Catalogue Item Links
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <param name="CatalogueItemAttachedLinkList">The list of links.</param>
        public void AddUpdateDeleteCatalogueItemLinks(string MainServerConnectionString, UInt32 IdCatalogueItem, List<CatalogueItemAttachedLink> CatalogueItemAttachedLinkList)
        {
            try
            {
                if (CatalogueItemAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (CatalogueItemAttachedLink catalogueItemAttachedLinkList in CatalogueItemAttachedLinkList)
                        {
                            if (catalogueItemAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItems_AttachedLinks_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItemAttachedLink", catalogueItemAttachedLinkList.IdCatalogueItemAttachedLink);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (catalogueItemAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (catalogueItemAttachedLinkList.IdCatalogueItemAttachedLink > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItems_AttachedLinks_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItemAttachedLink", catalogueItemAttachedLinkList.IdCatalogueItemAttachedLink);
                                    mySqlCommand.Parameters.AddWithValue("_Name", catalogueItemAttachedLinkList.Name);
                                    mySqlCommand.Parameters.AddWithValue("_Address", catalogueItemAttachedLinkList.Address);
                                    mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemAttachedLinkList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", catalogueItemAttachedLinkList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (catalogueItemAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CatalogueItems_AttachedLinks_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                                mySqlCommand.Parameters.AddWithValue("_Name", catalogueItemAttachedLinkList.Name);
                                mySqlCommand.Parameters.AddWithValue("_Address", catalogueItemAttachedLinkList.Address);
                                mySqlCommand.Parameters.AddWithValue("_Description", catalogueItemAttachedLinkList.Description);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", catalogueItemAttachedLinkList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteCatalogueItemLinks(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Product Type Links
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCpType">Get Product Type id.</param>
        /// <param name="ProductTypeAttachedLinkList">The list of links.</param>
        public void AddUpdateDeleteProductTypeLinks(string MainServerConnectionString, UInt64 IdCpType, List<ProductTypeAttachedLink> ProductTypeAttachedLinkList)
        {
            try
            {
                if (ProductTypeAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeAttachedLink productTypeAttachedLinkList in ProductTypeAttachedLinkList)
                        {
                            if (productTypeAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_AttachedLinks_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedLink", productTypeAttachedLinkList.IdCPTypeAttachedLink);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (productTypeAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (productTypeAttachedLinkList.IdCPTypeAttachedLink > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_AttachedLinks_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedLink", productTypeAttachedLinkList.IdCPTypeAttachedLink);
                                    mySqlCommand.Parameters.AddWithValue("_Name", productTypeAttachedLinkList.Name);
                                    mySqlCommand.Parameters.AddWithValue("_Address", productTypeAttachedLinkList.Address);
                                    mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedLinkList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", productTypeAttachedLinkList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (productTypeAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Cptypes_AttachedLinks_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCpType);
                                mySqlCommand.Parameters.AddWithValue("_Name", productTypeAttachedLinkList.Name);
                                mySqlCommand.Parameters.AddWithValue("_Address", productTypeAttachedLinkList.Address);
                                mySqlCommand.Parameters.AddWithValue("_Description", productTypeAttachedLinkList.Description);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeAttachedLinkList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteProductTypeLinks(). IdCpType- {0} ErrorMessage- {1}", IdCpType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Detection Links
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionAttachedLinkList">The list of links.</param>
        public void AddUpdateDeleteDetectionLinks(string MainServerConnectionString, UInt32 IdDetection, List<DetectionAttachedLink> DetectionAttachedLinkList)
        {
            try
            {
                if (DetectionAttachedLinkList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionAttachedLink detectionAttachedLinkList in DetectionAttachedLinkList)
                        {
                            if (detectionAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedLink", detectionAttachedLinkList.IdDetectionAttachedLink);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (detectionAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionAttachedLinkList.IdDetectionAttachedLink > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedLink", detectionAttachedLinkList.IdDetectionAttachedLink);
                                    mySqlCommand.Parameters.AddWithValue("_Name", detectionAttachedLinkList.Name);
                                    mySqlCommand.Parameters.AddWithValue("_Address", detectionAttachedLinkList.Address);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedLinkList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionAttachedLinkList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (detectionAttachedLinkList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_AttachedLinks_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_Name", detectionAttachedLinkList.Name);
                                mySqlCommand.Parameters.AddWithValue("_Address", detectionAttachedLinkList.Address);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionAttachedLinkList.Description);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionAttachedLinkList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionLinks(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete families by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="FamilyList">The list of family.</param>
        public void AddDeleteFamiliesByProductType(string MainServerConnectionString, UInt64 IdCPType, List<ConnectorFamilies> Families)
        {
            try
            {
                if (Families != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ConnectorFamilies familyList in Families)
                        {
                            if (familyList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCPType_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdFamily", familyList.IdFamily);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (familyList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_ConnectorFamiliesByCPType_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdFamily", familyList.IdFamily);

                                familyList.IdFamily = Convert.ToUInt64(mySqlCommand.ExecuteScalar());
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteFamiliesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete ways by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="WaysList">The list of ways.</param>
        public void AddDeleteWaysByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Ways> WaysList)
        {
            try
            {
                if (WaysList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Ways wayList in WaysList)
                        {
                            if (wayList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", wayList.IdWays);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (wayList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", wayList.IdWays);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteWaysByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete detections by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="DetectionsList">The list of detections.</param>
        public void AddUpdateDeleteDetectionsByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Detections> DetectionsList)
        {
            try
            {
                if (DetectionsList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Detections detectionList in DetectionsList)
                        {
                            if (detectionList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                if (IdCPType > 0 && detectionList.IdDetections > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Delete", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (detectionList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (IdCPType > 0 && detectionList.IdDetections > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);
                                    mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionList.OrderNumber);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (detectionList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                if (IdCPType > 0 && detectionList.IdDetections > 0 && IdTemplate > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", detectionList.IdDetections);
                                    mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionList.OrderNumber);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteDetectionsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete options by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="OptionsList">The list of options.</param>
        public void AddUpdateDeleteOptionsByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<Options> OptionsList)
        {
            try
            {
                Detections obj = new Detections();
                if (OptionsList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Options optionList in OptionsList)
                        {
                            if (optionList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                if (IdCPType > 0 && optionList.IdOptions > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Delete", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (optionList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (IdCPType > 0 && optionList.IdOptions > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);
                                    mySqlCommand.Parameters.AddWithValue("_OrderNumber", optionList.OrderNumber);

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (optionList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                if (IdCPType > 0 && optionList.IdOptions > 0 && IdTemplate > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsAndOptionsByCPType_Insert", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetection", optionList.IdOptions);
                                    mySqlCommand.Parameters.AddWithValue("_OrderNumber", optionList.OrderNumber);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteOptionsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete spare parts by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="IdTemplate">Get template id.</param>
        /// <param name="SparePartsList">The list of spare parts.</param>
        public void AddDeleteSparePartsByProductType(string MainServerConnectionString, UInt64 IdCPType, UInt64 IdTemplate, List<SpareParts> SparePartsList)
        {
            try
            {
                if (SparePartsList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (SpareParts sparePartsList in SparePartsList)
                        {
                            if (sparePartsList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", sparePartsList.IdSpareParts);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (sparePartsList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionsByCPType_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", sparePartsList.IdSpareParts);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSparePartsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete templates by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="TemplateList">The list of template.</param>
        public void AddDeleteTemplatesByProductType(string MainServerConnectionString, UInt64 IdCPType, List<Template> TemplateList)
        {
            try
            {
                if (TemplateList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (Template templateList in TemplateList)
                        {
                            if (templateList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (templateList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypesByTemplate_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdTemplate", templateList.IdTemplate);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteTemplatesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Product Type Images
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCpType">Get product type id.</param>
        /// <param name="ProductTypeImagePath">Get image path.</param>
        /// <param name="ProductTypeImageList">The list of images.</param>
        public void AddUpdateDeleteProductTypeImages(string MainServerConnectionString, UInt64 IdCpType, List<ProductTypeImage> ProductTypeImageList, string ProductTypeImagePath)
        {
            try
            {
                if (ProductTypeImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeImage productTypeImageList in ProductTypeImageList)
                        {
                            if (productTypeImageList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPTypeImage", productTypeImageList.IdCPTypeImage);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteProductTypeImageFromPath(productTypeImageList, ProductTypeImagePath);
                            }
                            else if (productTypeImageList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (productTypeImageList.IdCPTypeImage > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeImage", productTypeImageList.IdCPTypeImage);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeImageList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", productTypeImageList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeImageList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", productTypeImageList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_Position", productTypeImageList.Position);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddProductTypeImageToPath(productTypeImageList, ProductTypeImagePath);
                                }
                            }
                            else if (productTypeImageList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_CptypeImages_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCpType);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", productTypeImageList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", productTypeImageList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", productTypeImageList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", productTypeImageList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_Position", productTypeImageList.Position);

                                productTypeImageList.IdCPTypeImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (productTypeImageList.IdCPTypeImage > 0)
                                {
                                    AddProductTypeImageToPath(productTypeImageList, ProductTypeImagePath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteProductTypeImages(). IdCpType- {0} ErrorMessage- {1}", IdCpType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Detection Images
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <param name="DetectionImageList">The list of images.</param>
        public void AddUpdateDeleteDetectionImages(string MainServerConnectionString, UInt32 IdDetection, List<DetectionImage> DetectionImageList, string DetectionImagePath)
        {
            try
            {
                if (DetectionImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (DetectionImage detectionImageList in DetectionImageList)
                        {
                            if (detectionImageList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionImage", detectionImageList.IdDetectionImage);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeleteDetectionImageFromPath(detectionImageList, DetectionImagePath);
                            }
                            else if (detectionImageList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionImageList.IdDetectionImage > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionImage", detectionImageList.IdDetectionImage);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionImageList.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionImageList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionImageList.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionImageList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_Position", detectionImageList.Position);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddDetectionImageToPath(detectionImageList, DetectionImagePath);
                                }
                            }
                            else if (detectionImageList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_DetectionImages_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", detectionImageList.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionImageList.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", detectionImageList.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionImageList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_Position", detectionImageList.Position);

                                detectionImageList.IdDetectionImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (detectionImageList.IdDetectionImage > 0)
                                {
                                    AddDetectionImageToPath(detectionImageList, DetectionImagePath);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionImages(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Detection group
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetectionType">Get Detection type.</param>
        /// <param name="DetectionGroupList">The list of group.</param>
        public UInt32 AddUpdateDeleteDetectionGroup(string MainServerConnectionString, UInt32 IdDetectionType, List<DetectionGroup> DetectionGroupList, DetectionOrderGroup DetectionOrderGroup)
        {
            UInt32 idGroup = 0;
            try
            {
                if (DetectionGroupList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (DetectionGroup detectionGroupList in DetectionGroupList)
                        {
                            if (detectionGroupList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand1 = new MySqlCommand("PCM_DetectionGroupsByGroup_Update", mySqlConnection);
                                mySqlCommand1.CommandType = CommandType.StoredProcedure;
                                mySqlCommand1.Parameters.AddWithValue("_IdGroup", detectionGroupList.IdGroup);
                                mySqlCommand1.ExecuteNonQuery();

                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionGroupList.IdGroup);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (detectionGroupList.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (detectionGroupList.IdGroup > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                                    mySqlCommand.Parameters.AddWithValue("_IdGroup", detectionGroupList.IdGroup);
                                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                    mySqlCommand.Parameters.AddWithValue("_Name", detectionGroupList.Name);
                                    mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionGroupList.OrderNumber);
                                    mySqlCommand.Parameters.AddWithValue("_Name_es", detectionGroupList.Name_es);
                                    mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionGroupList.Name_fr);
                                    mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionGroupList.Name_pt);
                                    mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionGroupList.Name_ro);
                                    mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionGroupList.Name_zh);
                                    mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionGroupList.Name_ru);
                                    mySqlCommand.Parameters.AddWithValue("_Description", detectionGroupList.Description);
                                    mySqlCommand.Parameters.AddWithValue("_Description_es", detectionGroupList.Description_es);
                                    mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionGroupList.Description_fr);
                                    mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionGroupList.Description_pt);
                                    mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionGroupList.Description_ro);
                                    mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionGroupList.Description_zh);
                                    mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionGroupList.Description_ru);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", detectionGroupList.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (detectionGroupList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Detections_Groups_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                                mySqlCommand.Parameters.AddWithValue("_Name", detectionGroupList.Name);
                                mySqlCommand.Parameters.AddWithValue("_OrderNumber", detectionGroupList.OrderNumber);
                                mySqlCommand.Parameters.AddWithValue("_Name_es", detectionGroupList.Name_es);
                                mySqlCommand.Parameters.AddWithValue("_Name_fr", detectionGroupList.Name_fr);
                                mySqlCommand.Parameters.AddWithValue("_Name_pt", detectionGroupList.Name_pt);
                                mySqlCommand.Parameters.AddWithValue("_Name_ro", detectionGroupList.Name_ro);
                                mySqlCommand.Parameters.AddWithValue("_Name_zh", detectionGroupList.Name_zh);
                                mySqlCommand.Parameters.AddWithValue("_Name_ru", detectionGroupList.Name_ru);
                                mySqlCommand.Parameters.AddWithValue("_Description", detectionGroupList.Description);
                                mySqlCommand.Parameters.AddWithValue("_Description_es", detectionGroupList.Description_es);
                                mySqlCommand.Parameters.AddWithValue("_Description_fr", detectionGroupList.Description_fr);
                                mySqlCommand.Parameters.AddWithValue("_Description_pt", detectionGroupList.Description_pt);
                                mySqlCommand.Parameters.AddWithValue("_Description_ro", detectionGroupList.Description_ro);
                                mySqlCommand.Parameters.AddWithValue("_Description_zh", detectionGroupList.Description_zh);
                                mySqlCommand.Parameters.AddWithValue("_Description_ru", detectionGroupList.Description_ru);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", detectionGroupList.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                detectionGroupList.IdGroup = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (DetectionOrderGroup != null && DetectionOrderGroup.Name == detectionGroupList.Name)
                                {
                                    idGroup = detectionGroupList.IdGroup;
                                }
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteDetectionGroup(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return idGroup;
        }

        /// <summary>
        /// This method is used to Add/Delete customers and regions by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="CustomersList">The list of customer.</param>
        public void AddDeleteCustomersRegionsByProductType(string MainServerConnectionString, UInt64 IdCPType, List<RegionsByCustomer> CustomersList, UInt32 IdCreator)
        {
            try
            {
                if (CustomersList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (RegionsByCustomer customersList in CustomersList)
                        {
                            if (customersList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_cptype_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (customersList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_cptype_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteCustomersRegionsByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete customers and regions by product type
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCPType">Get product type id.</param>
        /// <param name="CustomersList">The list of customer.</param>
        public void AddDeleteSitesByProductType(string MainServerConnectionString, UInt64 IdCPType, List<RegionsByCustomer> CustomersList, string PCMConnectionString)
        {
            try
            {
                if (CustomersList != null)
                {
                    List<Site> SitesDeleteCustomerList = new List<Site>();
                    List<Site> SitesAddCustomerList = new List<Site>();
                    if (CustomersList.Any(a => a.TransactionOperation == ModelBase.TransactionOperations.Delete))
                    {
                        try
                        {
                            using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                            {
                                mySqlConnection.Open();
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetSitesByCustomerAndRegion", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                foreach (RegionsByCustomer customerList in CustomersList.Where(a => a.TransactionOperation == ModelBase.TransactionOperations.Delete).ToList())
                                {
                                    mySqlCommand.Parameters.Clear();
                                    mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader["IdSite"] != DBNull.Value)
                                            {
                                                Site site = new Site();
                                                site.IdSite = Convert.ToUInt32(reader["IdSite"]);
                                                SitesDeleteCustomerList.Add(site);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSitesByProductType-PCM_GetSitesByCustomerAndRegion. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                    if (CustomersList.Any(a => a.TransactionOperation == ModelBase.TransactionOperations.Add))
                    {
                        try
                        {
                            using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                            {
                                mySqlConnection.Open();
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetSitesByCustomerAndRegionByCPType", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                foreach (RegionsByCustomer customerList in CustomersList.Where(a => a.TransactionOperation == ModelBase.TransactionOperations.Add).ToList())
                                {
                                    mySqlCommand.Parameters.Clear();
                                    mySqlCommand.Parameters.AddWithValue("_IdRegion", customerList.IdRegion);
                                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", customerList.IdGroup);
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            if (reader["IdSite"] != DBNull.Value)
                                            {
                                                Site site = new Site();
                                                site.IdSite = Convert.ToUInt32(reader["IdSite"]);
                                                SitesAddCustomerList.Add(site);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSitesByProductType-PCM_GetSitesByCustomerAndRegionByCPType. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }

                    if (SitesDeleteCustomerList != null && SitesDeleteCustomerList.Count > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_SitesByCPType_Delete", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            SitesDeleteCustomerList = SitesDeleteCustomerList.Distinct().ToList();
                            foreach (Site site in SitesDeleteCustomerList)
                            {
                                try
                                {
                                    mySqlCommand.Parameters.Clear();
                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdSite", site.IdSite);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSitesByProductType-PCM_SitesByCPType_Delete. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }


                    if (SitesAddCustomerList != null && SitesAddCustomerList.Count > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_SitesByCPType_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            SitesAddCustomerList = SitesAddCustomerList.Distinct().ToList();
                            foreach (Site site in SitesAddCustomerList)
                            {
                                try
                                {
                                    mySqlCommand.Parameters.Clear();

                                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                                    mySqlCommand.Parameters.AddWithValue("_IdSite", site.IdSite);
                                    mySqlCommand.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSitesByProductType-PCM_SitesByCPType_Insert. ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                                    throw;
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteSitesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Delete customers and regions by detection
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="CustomersList">The list of customer.</param>
        /// <param name="IdDetectiontype">Get detection type id.</param>
        public void AddDeleteCustomersRegionsByDetection(string MainServerConnectionString, UInt32 IdDetection, List<RegionsByCustomer> CustomersList, UInt32 IdDetectiontype, UInt32 IdCreator)
        {
            try
            {
                if (CustomersList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (RegionsByCustomer customersList in CustomersList)
                        {
                            if (customersList.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (customersList.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_customers_regions_by_detection_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                                mySqlCommand.Parameters.AddWithValue("_IdCustomer", customersList.IdGroup);
                                mySqlCommand.Parameters.AddWithValue("_IdRegion", customersList.IdRegion);
                                mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectiontype);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", IdCreator);
                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddDeleteCustomersRegionsByDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }


        /// <summary>
        /// This method is used to Update PCM article category position
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="pcmArticleCategoryList">The list of pcm article category.</param>
        public void UpdatePCMArticleCategoryPosition(string MainServerConnectionString, List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier)
        {
            try
            {
                if (pcmArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategoryList)
                        {
                            if (pcmArticleCategory.IdPCMArticleCategory > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Position_Update", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                                mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                                mySqlCommand.Parameters.AddWithValue("_IdModifier", IdModifier);
                                mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePCMArticleCategoryPosition(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add/Update/Delete Product Type Capabilities
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdCpType">Get Product Type id.</param>
        /// <param name="CompatibilityList">The list of Compatibility.</param>
        public void AddUpdateDeleteProductTypeCompatibilities(string MainServerConnectionString, byte IdCpType, List<ProductTypeCompatibility> CompatibilityList)
        {
            try
            {
                if (CompatibilityList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ProductTypeCompatibility productTypeCompatibility in CompatibilityList)
                        {
                            if (productTypeCompatibility.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_cptype_compatibilities_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCompatibility", productTypeCompatibility.IdCompatibility);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (productTypeCompatibility.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (productTypeCompatibility.IdCompatibility > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_cptype_compatibilities_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCompatibility", productTypeCompatibility.IdCompatibility);
                                    mySqlCommand.Parameters.AddWithValue("_MinimumElements", productTypeCompatibility.MinimumElements);
                                    mySqlCommand.Parameters.AddWithValue("_MaximumElements", productTypeCompatibility.MaximumElements);
                                    if (productTypeCompatibility.IdRelationshipType == 0)
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                                    }
                                    else
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", productTypeCompatibility.IdRelationshipType);
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_Quantity", productTypeCompatibility.Quantity);
                                    mySqlCommand.Parameters.AddWithValue("_Remarks", productTypeCompatibility.Remarks);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", productTypeCompatibility.IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (productTypeCompatibility.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_cptype_compatibilities_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCPtype", IdCpType);
                                mySqlCommand.Parameters.AddWithValue("_IdCPtypeCompatibility", productTypeCompatibility.IdCPtypeCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_IdArticleCompatibility", productTypeCompatibility.IdArticleCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_IdTypeCompatibility", productTypeCompatibility.IdTypeCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_MinimumElements", productTypeCompatibility.MinimumElements);
                                mySqlCommand.Parameters.AddWithValue("_MaximumElements", productTypeCompatibility.MaximumElements);
                                if (productTypeCompatibility.IdRelationshipType == 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", productTypeCompatibility.IdRelationshipType);
                                }
                                mySqlCommand.Parameters.AddWithValue("_Quantity", productTypeCompatibility.Quantity);
                                mySqlCommand.Parameters.AddWithValue("_Remarks", productTypeCompatibility.Remarks);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", productTypeCompatibility.IdCreator);
                                mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteProductTypeCapabilities(). IdCpType- {0} ErrorMessage- {1}", IdCpType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }


        /// <summary>
        /// This method is used to Add/Update/Delete Article Capabilities
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdArticle">Get Article id.</param>
        /// <param name="CompatibilityList">The list of Compatibility.</param>
        public void AddUpdateDeleteArticleCompatibilities(string MainServerConnectionString, UInt32 IdArticle, List<ArticleCompatibility> CompatibilityList)
        {
            try
            {
                if (CompatibilityList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (ArticleCompatibility articleCompatibility in CompatibilityList)
                        {
                            if (articleCompatibility.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdCompatibility", articleCompatibility.IdCompatibility);
                                mySqlCommand.ExecuteNonQuery();
                            }
                            else if (articleCompatibility.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (articleCompatibility.IdCompatibility > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdCompatibility", articleCompatibility.IdCompatibility);
                                    mySqlCommand.Parameters.AddWithValue("_MinimumElements", articleCompatibility.MinimumElements);
                                    mySqlCommand.Parameters.AddWithValue("_MaximumElements", articleCompatibility.MaximumElements);
                                    if (articleCompatibility.IdRelationshipType == 0)
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                                    }
                                    else
                                    {
                                        mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", articleCompatibility.IdRelationshipType);
                                    }
                                    mySqlCommand.Parameters.AddWithValue("_Quantity", articleCompatibility.Quantity);
                                    mySqlCommand.Parameters.AddWithValue("_Remarks", articleCompatibility.Remarks);
                                    mySqlCommand.Parameters.AddWithValue("_IdModifier", articleCompatibility.IdModifier);
                                    mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                    mySqlCommand.ExecuteNonQuery();
                                }
                            }
                            else if (articleCompatibility.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_article_compatibilities_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                mySqlCommand.Parameters.AddWithValue("_IdArticleCompatibility", articleCompatibility.IdArticleCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_IdCPtypeCompatibility", articleCompatibility.IdCPtypeCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_IdTypeCompatibility", articleCompatibility.IdTypeCompatibility);
                                mySqlCommand.Parameters.AddWithValue("_MinimumElements", articleCompatibility.MinimumElements);
                                mySqlCommand.Parameters.AddWithValue("_MaximumElements", articleCompatibility.MaximumElements);
                                if (articleCompatibility.IdRelationshipType == 0)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_IdRelationshipType", articleCompatibility.IdRelationshipType);
                                }
                                mySqlCommand.Parameters.AddWithValue("_Quantity", articleCompatibility.Quantity);
                                mySqlCommand.Parameters.AddWithValue("_Remarks", articleCompatibility.Remarks);
                                mySqlCommand.Parameters.AddWithValue("_IdCreator", articleCompatibility.IdCreator);
                                mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeleteArticleCompatibilities(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public void AddUpdateDeletePCMArticleImages(string MainServerConnectionString, UInt32 IdArticle, List<PCMArticleImage> ImageList, string ImagePath, string ArticleReference)
        {
            try
            {
                if (ImageList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleImage Image in ImageList)
                        {
                            if (Image.TransactionOperation == ModelBase.TransactionOperations.Delete)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Images_Delete", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdPCMArticleImage", Image.IdPCMArticleImage);
                                mySqlCommand.ExecuteNonQuery();

                                IsDeletePCMArticleImageFromPath(Image, ImagePath, ArticleReference);
                            }
                            else if (Image.TransactionOperation == ModelBase.TransactionOperations.Update)
                            {
                                if (Image.IdPCMArticleImage > 0)
                                {
                                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Images_Update", mySqlConnection);
                                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                                    mySqlCommand.Parameters.AddWithValue("_IdPCMArticleImage", Image.IdPCMArticleImage);
                                    mySqlCommand.Parameters.AddWithValue("_SavedFileName", Image.SavedFileName);
                                    mySqlCommand.Parameters.AddWithValue("_Description", Image.Description);
                                    mySqlCommand.Parameters.AddWithValue("_OriginalFileName", Image.OriginalFileName);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", Image.ModifiedBy);
                                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    mySqlCommand.Parameters.AddWithValue("_Position", Image.Position);

                                    mySqlCommand.ExecuteNonQuery();

                                    AddPCMArticleImageToPath(Image, ImagePath, ArticleReference);
                                }
                            }
                            else if (Image.TransactionOperation == ModelBase.TransactionOperations.Add)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Images_Insert", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                                mySqlCommand.Parameters.AddWithValue("_SavedFileName", Image.SavedFileName);
                                mySqlCommand.Parameters.AddWithValue("_Description", Image.Description);
                                mySqlCommand.Parameters.AddWithValue("_OriginalFileName", Image.OriginalFileName);
                                mySqlCommand.Parameters.AddWithValue("_CreatedBy", Image.CreatedBy);
                                mySqlCommand.Parameters.AddWithValue("_CreatedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                mySqlCommand.Parameters.AddWithValue("_Position", Image.Position);

                                Image.IdPCMArticleImage = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                                if (Image.IdPCMArticleImage > 0)
                                {
                                    AddPCMArticleImageToPath(Image, ImagePath, ArticleReference);
                                }
                            }
                        }

                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddUpdateDeletePCMArticleImages(). IdArticle- {0} ErrorMessage- {1}", IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }
        #endregion

        #region Get Methods

        /// <summary>
        /// This method is used to get all catalogue items.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of catalogue items.</returns>
        public List<CatalogueItem> GetAllCatalogueItems(string PCMConnectionString)
        {
            List<CatalogueItem> catalogueItems = new List<CatalogueItem>();
            try
            {
                using (MySqlConnection connCatalogueItems = new MySqlConnection(PCMConnectionString))
                {
                    connCatalogueItems.Open();

                    MySqlCommand catalogueitemCommand = new MySqlCommand("PCM_GetAllCatalogueItems", connCatalogueItems);
                    catalogueitemCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader catalogueitemReader = catalogueitemCommand.ExecuteReader())
                    {
                        while (catalogueitemReader.Read())
                        {
                            CatalogueItem catalogueitem = new CatalogueItem();

                            catalogueitem.IdCatalogueItem = Convert.ToUInt32(catalogueitemReader["idCatalogueItem"]);

                            if (catalogueitemReader["Code"] != DBNull.Value)
                            {
                                catalogueitem.Code = Convert.ToString(catalogueitemReader["Code"]);
                            }
                            if (catalogueitemReader["IdTemplate"] != DBNull.Value)
                            {
                                catalogueitem.IdTemplate = Convert.ToUInt64(catalogueitemReader["IdTemplate"]);
                                catalogueitem.Template = new Template();
                                catalogueitem.Template.IdTemplate = Convert.ToByte(catalogueitemReader["IdTemplate"]);
                                if (catalogueitemReader["TemplateName"] != DBNull.Value)
                                {
                                    catalogueitem.Template.Name = Convert.ToString(catalogueitemReader["TemplateName"]);
                                }
                            }
                            if (catalogueitemReader["IdCPType"] != DBNull.Value)
                            {
                                catalogueitem.IdCPType = Convert.ToUInt64(catalogueitemReader["IdCPType"]);
                                catalogueitem.ProductType = new ProductTypes();
                                catalogueitem.ProductType.IdCPType = Convert.ToUInt64(catalogueitemReader["IdCPType"]);
                                if (catalogueitemReader["ProductType"] != DBNull.Value)
                                {
                                    catalogueitem.ProductType.Name = Convert.ToString(catalogueitemReader["ProductType"]);
                                }
                            }
                            if (catalogueitemReader["IdStatus"] != DBNull.Value)
                            {
                                catalogueitem.IdStatus = Convert.ToInt32(catalogueitemReader["IdStatus"]);
                                catalogueitem.Status = new LookupValue();
                                catalogueitem.Status.IdLookupValue = Convert.ToInt32(catalogueitemReader["IdStatus"]);
                                if (catalogueitemReader["Status"] != DBNull.Value)
                                {
                                    catalogueitem.Status.Value = Convert.ToString(catalogueitemReader["Status"]);
                                }
                            }
                            if (catalogueitemReader["Name"] != DBNull.Value)
                            {
                                catalogueitem.Name = Convert.ToString(catalogueitemReader["Name"]);
                            }
                            if (catalogueitemReader["CreatedIn"] != DBNull.Value)
                            {
                                catalogueitem.CreatedIn = Convert.ToDateTime(catalogueitemReader["CreatedIn"]);
                            }
                            if (catalogueitemReader["LastUpdate"] != DBNull.Value)
                            {
                                catalogueitem.LastUpdate = Convert.ToDateTime(catalogueitemReader["LastUpdate"]);
                            }
                            if (catalogueitemReader["Description"] != DBNull.Value)
                            {
                                catalogueitem.Description = Convert.ToString(catalogueitemReader["Description"]);
                            }

                            catalogueItems.Add(catalogueitem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCatalogueItems(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return catalogueItems;
        }

        /// <summary>
        /// This method is used to get all Product Types.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Product Types.</returns>
        public List<ProductTypes> GetAllProductTypes(string PCMConnectionString)
        {
            List<ProductTypes> cpTypes = new List<ProductTypes>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllProductTypes", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypes cpType = new ProductTypes();
                            cpType.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["Reference"] != DBNull.Value)
                            {
                                cpType.Reference = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Name"] != DBNull.Value)
                            {
                                cpType.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["IdTemplate"] != DBNull.Value)
                            {
                                cpType.IdTemplate = Convert.ToUInt64(reader["IdTemplate"]);
                                cpType.Template = new Template();
                                cpType.Template.IdTemplate = Convert.ToByte(reader["IdTemplate"]);
                                if (reader["TemplateName"] != DBNull.Value)
                                {
                                    cpType.Template.Name = Convert.ToString(reader["TemplateName"]);
                                }
                            }
                            if (reader["IdStatus"] != DBNull.Value)
                            {
                                cpType.IdStatus = Convert.ToInt32(reader["IdStatus"]);
                                cpType.Status = new LookupValue();
                                cpType.Status.IdLookupValue = Convert.ToInt32(reader["IdStatus"]);
                                if (reader["Status"] != DBNull.Value)
                                {
                                    cpType.Status.Value = Convert.ToString(reader["Status"]);
                                }
                            }
                            if (reader["createdIn"] != DBNull.Value)
                            {
                                cpType.CreatedIn = Convert.ToDateTime(reader["createdIn"]);
                            }
                            if (reader["LastUpdate"] != DBNull.Value)
                            {
                                cpType.LastUpdate = Convert.ToDateTime(reader["LastUpdate"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                cpType.Description = Convert.ToString(reader["Description"]);
                            }

                            cpType.WayList = GetWayListByProductType(PCMConnectionString, cpType.IdCPType, cpType.IdTemplate);
                            cpType.DetectionList = GetDetectionListByProductType(PCMConnectionString, cpType.IdCPType, cpType.IdTemplate);
                            cpType.OptionList = GetOptionListByProductType(PCMConnectionString, cpType.IdCPType, cpType.IdTemplate);
                            cpType.SparePartList = GetSparePartListByProductType(PCMConnectionString, cpType.IdCPType, cpType.IdTemplate);
                            cpTypes.Add(cpType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllProductTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return cpTypes;
        }

        /// <summary>
        /// This method is used to get all Templates.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Templates.</returns>
        public List<Template> GetAllTemplates(string PCMConnectionString)
        {
            List<Template> Templates = new List<Template>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllTemplates", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Template Template = new Template();
                            Template.IdTemplate = Convert.ToByte(reader["IdTemplate"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Template.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["IsObsolete"] != DBNull.Value)
                            {
                                Template.IsObsolete = Convert.ToByte(reader["IsObsolete"]);
                            }

                            if (reader["HtmlColor"] != DBNull.Value)
                            {
                                Template.HtmlColor = Convert.ToString(reader["HtmlColor"]);
                            }

                            Templates.Add(Template);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllTemplates(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Templates;
        }

        /// <summary>
        /// This method is used to get all Families.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Families.</returns>
        public List<ConnectorFamilies> GetAllFamilies(string PCMConnectionString)
        {
            List<ConnectorFamilies> ConnectorFamilies = new List<ConnectorFamilies>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllFamilies", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ConnectorFamilies ConnectorFamily = new ConnectorFamilies();
                            ConnectorFamily.IdFamily = Convert.ToUInt64(reader["IdFamily"]);
                            if (reader["Name"] != DBNull.Value)
                            {
                                ConnectorFamily.Name = Convert.ToString(reader["Name"]);
                            }
                            ConnectorFamilies.Add(ConnectorFamily);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllFamilies(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ConnectorFamilies;
        }

        /// <summary>
        /// This method is used to get all Detection Types.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Detection Types.</returns>
        public List<DetectionTypes> GetAllDetectionTypes(string PCMConnectionString)
        {
            List<DetectionTypes> DetectionTypes = new List<DetectionTypes>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllDetectionTypes", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionTypes DetectionType = new DetectionTypes();

                            DetectionType.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionType.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["SortOrder"] != DBNull.Value)
                            {
                                DetectionType.SortOrder = Convert.ToUInt64(reader["SortOrder"]);
                            }

                            if (reader["Color"] != DBNull.Value)
                            {
                                DetectionType.Color = Convert.ToString(reader["Color"]);
                            }

                            DetectionTypes.Add(DetectionType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllDetectionTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return DetectionTypes;
        }

        /// <summary>
        /// This method is used to get Way List.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Ways.</returns>
        public List<Ways> GetAllWayList(string PCMConnectionString)
        {
            List<Ways> Ways = new List<Ways>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 1); //Detetction type: 1 for ways. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ways Way = new Ways();

                            Way.IdWays = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Way.Name = Convert.ToString(reader["Name"]);
                            }

                            Ways.Add(Way);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWayList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Ways;
        }

        /// <summary>
        /// This method is used to get Detection List.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Detections.</returns>
        public List<Detections> GetAllDetectionList(string PCMConnectionString)
        {
            List<Detections> Detections = new List<Detections>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 2); //Detetction type: 2 for detections. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections Detection = new Detections();

                            Detection.IdDetections = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Detection.Name = Convert.ToString(reader["Name"]);
                            }
                            Detections.Add(Detection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllDetectionList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Detections;
        }

        /// <summary>
        /// This method is used to get Option List.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Options.</returns>
        public List<Options> GetAllOptionList(string PCMConnectionString)
        {
            List<Options> Options = new List<Options>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 3); //Detetction type: 3 for options. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options Option = new Options();

                            Option.IdOptions = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Option.Name = Convert.ToString(reader["Name"]);
                            }
                            Options.Add(Option);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOptionList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Options;
        }

        /// <summary>
        /// This method is used to get Spare Part List.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Spare Parts.</returns>
        public List<SpareParts> GetAllSparePartList(string PCMConnectionString)
        {
            List<SpareParts> SpareParts = new List<SpareParts>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 4); //Detetction type: 4 for spare parts. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SpareParts SparePart = new SpareParts();

                            SparePart.IdSpareParts = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                SparePart.Name = Convert.ToString(reader["Name"]);
                            }

                            SpareParts.Add(SparePart);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllSparePartList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return SpareParts;
        }

        /// <summary>
        /// This method is to get lookup values
        /// </summary>
        /// <param name="key">Get key</param>
        /// <returns>List of Lookup values</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          PCMManager mgr = new PCMManager();
        ///          list = mgr.GetLookupValues(key); // using EF WorkbenchContext
        ///                   
        ///     }
        /// }
        /// </code>
        /// </example>
        public IList<LookupValue> GetLookupValues(byte key)
        {
            IList<LookupValue> list = null;

            using (var context = new WorkbenchContext())
            {
                list = (from records in context.LookupValues where records.IdLookupKey == key select records).OrderBy(y => y.Position).ToList();
            }

            return list;
        }

        /// <summary>
        /// This method is used to get Families by Catalogue Item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <returns>The list of Families by catalogue item.</returns>
        public List<ConnectorFamilies> GetFamiliesByCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem)
        {
            List<ConnectorFamilies> ConnectorFamilies = new List<ConnectorFamilies>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetFamiliesByCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ConnectorFamilies ConnectorFamily = new ConnectorFamilies();
                            ConnectorFamily.IdFamily = Convert.ToUInt64(reader["IdFamily"]);
                            if (reader["Name"] != DBNull.Value)
                            {
                                ConnectorFamily.Name = Convert.ToString(reader["Name"]);
                            }
                            ConnectorFamilies.Add(ConnectorFamily);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetFamiliesByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ConnectorFamilies;
        }

        /// <summary>
        /// This method is used to get Way List by catalogue item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <returns>The list of Ways by catalogue item.</returns>
        public List<Ways> GetWayListByCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem)
        {
            List<Ways> Ways = new List<Ways>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 1); //Detetction type: 1 for ways. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ways Way = new Ways();

                            Way.IdWays = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Way.Name = Convert.ToString(reader["Name"]);
                            }

                            Ways.Add(Way);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWayListByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Ways;
        }

        /// <summary>
        /// This method is used to get Detection List by catalogue item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <returns>The list of Detections by catalogue item.</returns>
        public List<Detections> GetDetectionListByCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem)
        {
            List<Detections> Detections = new List<Detections>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 2); //Detetction type: 2 for detections. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections Detection = new Detections();

                            Detection.IdDetections = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Detection.Name = Convert.ToString(reader["Name"]);
                            }

                            Detections.Add(Detection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionListByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Detections;
        }

        /// <summary>
        /// This method is used to get Option List by catalogue item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <returns>The list of Options by catalogue item.</returns>
        public List<Options> GetOptionListByCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem)
        {
            List<Options> Options = new List<Options>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 3); //Detetction type: 3 for options. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options Option = new Options();

                            Option.IdOptions = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Option.Name = Convert.ToString(reader["Name"]);
                            }

                            Options.Add(Option);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOptionListByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Options;
        }

        /// <summary>
        /// This method is used to get Spare Part List by catalogue item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <returns>The list of Spare Parts by catalogue item.</returns>
        public List<SpareParts> GetSparePartListByCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem)
        {
            List<SpareParts> SpareParts = new List<SpareParts>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 4); //Detetction type: 4 for spare parts. 

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SpareParts SparePart = new SpareParts();

                            SparePart.IdSpareParts = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                SparePart.Name = Convert.ToString(reader["Name"]);
                            }

                            SpareParts.Add(SparePart);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetSparePartListByCatalogueItem(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return SpareParts;
        }

        /// <summary>
        /// This method is used to get Catalogue Item by ID.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get catalogue item id.</param>
        /// <returns>The data of catalogue item by id.</returns>
        public CatalogueItem GetCatalogueItemByIdCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem, string CatalogueItemAttachedDocPath)
        {
            CatalogueItem catalogueItem = new CatalogueItem();
            try
            {
                using (MySqlConnection connCatalogueItems = new MySqlConnection(PCMConnectionString))
                {
                    connCatalogueItems.Open();

                    MySqlCommand catalogueitemCommand = new MySqlCommand("PCM_GetCatalogueItemByIdCatalogueItem", connCatalogueItems);
                    catalogueitemCommand.CommandType = CommandType.StoredProcedure;
                    catalogueitemCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);

                    using (MySqlDataReader catalogueitemReader = catalogueitemCommand.ExecuteReader())
                    {
                        while (catalogueitemReader.Read())
                        {
                            catalogueItem.IdCatalogueItem = Convert.ToUInt32(catalogueitemReader["idCatalogueItem"]);

                            if (catalogueitemReader["Code"] != DBNull.Value)
                            {
                                catalogueItem.Code = Convert.ToString(catalogueitemReader["Code"]);
                            }
                            if (catalogueitemReader["IdTemplate"] != DBNull.Value)
                            {
                                catalogueItem.IdTemplate = Convert.ToUInt64(catalogueitemReader["IdTemplate"]);
                                catalogueItem.Template = new Template();
                                catalogueItem.Template.IdTemplate = Convert.ToByte(catalogueitemReader["IdTemplate"]);
                                if (catalogueitemReader["TemplateName"] != DBNull.Value)
                                {
                                    catalogueItem.Template.Name = Convert.ToString(catalogueitemReader["TemplateName"]);
                                }
                            }
                            if (catalogueitemReader["IdCPType"] != DBNull.Value)
                            {
                                catalogueItem.IdCPType = Convert.ToUInt64(catalogueitemReader["IdCPType"]);
                                catalogueItem.ProductType = new ProductTypes();
                                catalogueItem.ProductType.IdCPType = Convert.ToUInt64(catalogueitemReader["IdCPType"]);
                                if (catalogueitemReader["ProductType"] != DBNull.Value)
                                {
                                    catalogueItem.ProductType.Name = Convert.ToString(catalogueitemReader["ProductType"]);
                                }
                            }
                            if (catalogueitemReader["Name"] != DBNull.Value)
                            {
                                catalogueItem.Name = Convert.ToString(catalogueitemReader["Name"]);
                            }
                            if (catalogueitemReader["Description"] != DBNull.Value)
                            {
                                catalogueItem.Description = Convert.ToString(catalogueitemReader["Description"]);
                            }
                            if (catalogueitemReader["IdStatus"] != DBNull.Value)
                            {
                                catalogueItem.IdStatus = Convert.ToInt32(catalogueitemReader["IdStatus"]);
                                catalogueItem.Status = new LookupValue();
                                catalogueItem.Status.IdLookupValue = Convert.ToInt32(catalogueitemReader["IdStatus"]);
                                if (catalogueitemReader["Status"] != DBNull.Value)
                                {
                                    catalogueItem.Status.Value = Convert.ToString(catalogueitemReader["Status"]);
                                }
                            }
                            if (catalogueitemReader["CreatedIn"] != DBNull.Value)
                            {
                                catalogueItem.CreatedIn = Convert.ToDateTime(catalogueitemReader["CreatedIn"]);
                            }
                            if (catalogueitemReader["LastUpdate"] != DBNull.Value)
                            {
                                catalogueItem.LastUpdate = Convert.ToDateTime(catalogueitemReader["LastUpdate"]);
                            }

                            if (catalogueitemReader["Name_es"] != DBNull.Value)
                            {
                                catalogueItem.Name_es = Convert.ToString(catalogueitemReader["Name_es"]);
                            }
                            if (catalogueitemReader["Name_fr"] != DBNull.Value)
                            {
                                catalogueItem.Name_fr = Convert.ToString(catalogueitemReader["Name_fr"]);
                            }
                            if (catalogueitemReader["Name_pt"] != DBNull.Value)
                            {
                                catalogueItem.Name_pt = Convert.ToString(catalogueitemReader["Name_pt"]);
                            }
                            if (catalogueitemReader["Name_ro"] != DBNull.Value)
                            {
                                catalogueItem.Name_ro = Convert.ToString(catalogueitemReader["Name_ro"]);
                            }
                            if (catalogueitemReader["Name_ru"] != DBNull.Value)
                            {
                                catalogueItem.Name_ru = Convert.ToString(catalogueitemReader["Name_ru"]);
                            }
                            if (catalogueitemReader["Name_zh"] != DBNull.Value)
                            {
                                catalogueItem.Name_zh = Convert.ToString(catalogueitemReader["Name_zh"]);
                            }
                            if (catalogueitemReader["Description_es"] != DBNull.Value)
                            {
                                catalogueItem.Description_es = Convert.ToString(catalogueitemReader["Description_es"]);
                            }
                            if (catalogueitemReader["Description_fr"] != DBNull.Value)
                            {
                                catalogueItem.Description_fr = Convert.ToString(catalogueitemReader["Description_fr"]);
                            }
                            if (catalogueitemReader["Description_pt"] != DBNull.Value)
                            {
                                catalogueItem.Description_pt = Convert.ToString(catalogueitemReader["Description_pt"]);
                            }
                            if (catalogueitemReader["Description_ro"] != DBNull.Value)
                            {
                                catalogueItem.Description_ro = Convert.ToString(catalogueitemReader["Description_ro"]);
                            }
                            if (catalogueitemReader["Description_ru"] != DBNull.Value)
                            {
                                catalogueItem.Description_ru = Convert.ToString(catalogueitemReader["Description_ru"]);
                            }
                            if (catalogueitemReader["Description_zh"] != DBNull.Value)
                            {
                                catalogueItem.Description_zh = Convert.ToString(catalogueitemReader["Description_zh"]);
                            }
                        }
                    }
                }

                catalogueItem.WayList = GetWayListByCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem);
                catalogueItem.DetectionList = GetDetectionListByCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem);
                catalogueItem.OptionList = GetOptionListByCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem);
                catalogueItem.SparePartList = GetSparePartListByCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem);
                catalogueItem.FamilyList = GetFamiliesByCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem);
                catalogueItem.FileList = GetCatalogueItemAttachedDocsByIdCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem, CatalogueItemAttachedDocPath);
                catalogueItem.CatalogueItemAttachedLinkList = GetCatalogueItemAttachedLinksByIdCatalogueItem(PCMConnectionString, catalogueItem.IdCatalogueItem);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCatalogueItemByID(). IdCatalogueItem- {0} ErrorMessage- {1}", IdCatalogueItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return catalogueItem;
        }

        /// <summary>
        /// This method is used to get all product Types by template.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Product Types By Template.</returns>
        public List<ProductTypes> GetProductTypesByTemplate(string PCMConnectionString, UInt64 idTemplate)
        {
            List<ProductTypes> cpTypes = new List<ProductTypes>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetProductTypesByTemplate", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", idTemplate);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypes cpType = new ProductTypes();
                            cpType.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                cpType.Name = Convert.ToString(reader["Name"]);
                            }
                            cpTypes.Add(cpType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypesByTemplate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return cpTypes;
        }

        /// <summary>
        /// This method is to get latest Catalogue Item code
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>latest Catalogue Item code</returns>
        public string GetLatestCatalogueItemCode(string PCMConnectionString)
        {
            UInt32 idCatalogueItem;
            string CatalogueItemCode;
            string Characters = "CP";
            Int16 Year = 0;
            string Seperator = ".";
            Int16 Number = 0;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetLatestCatalogueItemCode", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["idCatalogueItem"] != DBNull.Value)
                                idCatalogueItem = Convert.ToUInt32(reader["idCatalogueItem"]);

                            if (reader["CatalogueItemCode"] != DBNull.Value)
                                CatalogueItemCode = Convert.ToString(reader["CatalogueItemCode"]);

                            if (reader["Characters"] != DBNull.Value)
                                Characters = Convert.ToString(reader["Characters"]);

                            if (reader["Year"] != DBNull.Value)
                                Year = Convert.ToInt16(reader["Year"]);

                            if (reader["Seperator"] != DBNull.Value)
                                Seperator = Convert.ToString(reader["Seperator"]);

                            if (reader["Number"] != DBNull.Value)
                                Number = Convert.ToInt16(reader["Number"]);
                        }
                    }
                }

                Int16 currentYear = Convert.ToInt16(DateTime.Now.ToString("yy"));

                if (Year == currentYear)
                {
                    Number += 1;
                    return string.Format("{0}{1}{2}{3}", Characters, currentYear, Seperator, Number.ToString("0000"));
                }
                else if (currentYear > Year)
                {
                    return string.Format("{0}{1}{2}{3}", "CP", currentYear, Seperator, 1.ToString("0000"));
                }
                else
                {
                    return string.Format("{0}{1}{2}{3}", "CP", currentYear, Seperator, 1.ToString("0000"));
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLatestCatalogueItemCode(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get all Test Types.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Test Types.</returns>
        public List<TestTypes> GetAllTestTypes(string PCMConnectionString)
        {
            List<TestTypes> TestTypes = new List<TestTypes>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllTestTypes", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TestTypes TestType = new TestTypes();
                            TestType.IdTestType = Convert.ToUInt64(reader["IdTestType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                TestType.Name = Convert.ToString(reader["Name"]);
                            }
                            TestTypes.Add(TestType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllTestTypes(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return TestTypes;
        }

        /// <summary>
        /// This method is used to get Product type by ID.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCpType">Get product type id.</param>
        /// <param name="ProductTypeAttachedDocPath">Get file path.</param>
        /// <param name="ProductTypeImagePath">Get image path.</param>
        /// <returns>The data of product type by id.</returns>
        public ProductTypes GetProductTypeByIdCpType(string PCMConnectionString, UInt64 IdCpType, string ProductTypeAttachedDocPath, string ProductTypeImagePath, UInt64 IdTemplate)
        {
            ProductTypes productTypes = new ProductTypes();
            try
            {
                using (MySqlConnection connProductTypes = new MySqlConnection(PCMConnectionString))
                {
                    connProductTypes.Open();

                    string SP = "";
                    if (IdTemplate > 0)
                    {
                        SP = "PCM_GetProductTypeByIdCpTypeAndIdTemplate";
                    }
                    else
                    {
                        SP = "PCM_GetProductTypeByIdCpType";
                    }

                    MySqlCommand ProductTypeCommand = new MySqlCommand(SP, connProductTypes);
                    ProductTypeCommand.CommandType = CommandType.StoredProcedure;
                    if (IdTemplate > 0)
                    {
                        ProductTypeCommand.Parameters.AddWithValue("_IdCPType", IdCpType);
                        ProductTypeCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);
                    }
                    else
                    {
                        ProductTypeCommand.Parameters.AddWithValue("_IdCPType", IdCpType);
                    }


                    using (MySqlDataReader ProductTypeReader = ProductTypeCommand.ExecuteReader())
                    {
                        while (ProductTypeReader.Read())
                        {
                            productTypes.IdCPType = Convert.ToUInt64(ProductTypeReader["IdCPType"]);

                            if (ProductTypeReader["Reference"] != DBNull.Value)
                            {
                                productTypes.Reference = Convert.ToString(ProductTypeReader["Reference"]);
                            }
                            if (ProductTypeReader["Name"] != DBNull.Value)
                            {
                                productTypes.Name = Convert.ToString(ProductTypeReader["Name"]);
                            }
                            if (ProductTypeReader["Name_es"] != DBNull.Value)
                            {
                                productTypes.Name_es = Convert.ToString(ProductTypeReader["Name_es"]);
                            }
                            if (ProductTypeReader["Name_fr"] != DBNull.Value)
                            {
                                productTypes.Name_fr = Convert.ToString(ProductTypeReader["Name_fr"]);
                            }
                            if (ProductTypeReader["Name_pt"] != DBNull.Value)
                            {
                                productTypes.Name_pt = Convert.ToString(ProductTypeReader["Name_pt"]);
                            }
                            if (ProductTypeReader["Name_ro"] != DBNull.Value)
                            {
                                productTypes.Name_ro = Convert.ToString(ProductTypeReader["Name_ro"]);
                            }
                            if (ProductTypeReader["Name_ru"] != DBNull.Value)
                            {
                                productTypes.Name_ru = Convert.ToString(ProductTypeReader["Name_ru"]);
                            }
                            if (ProductTypeReader["Name_zh"] != DBNull.Value)
                            {
                                productTypes.Name_zh = Convert.ToString(ProductTypeReader["Name_zh"]);
                            }
                            if (ProductTypeReader["Description"] != DBNull.Value)
                            {
                                productTypes.Description = Convert.ToString(ProductTypeReader["Description"]);
                            }
                            if (ProductTypeReader["Description_es"] != DBNull.Value)
                            {
                                productTypes.Description_es = Convert.ToString(ProductTypeReader["Description_es"]);
                            }
                            if (ProductTypeReader["Description_fr"] != DBNull.Value)
                            {
                                productTypes.Description_fr = Convert.ToString(ProductTypeReader["Description_fr"]);
                            }
                            if (ProductTypeReader["Description_pt"] != DBNull.Value)
                            {
                                productTypes.Description_pt = Convert.ToString(ProductTypeReader["Description_pt"]);
                            }
                            if (ProductTypeReader["Description_ro"] != DBNull.Value)
                            {
                                productTypes.Description_ro = Convert.ToString(ProductTypeReader["Description_ro"]);
                            }
                            if (ProductTypeReader["Description_ru"] != DBNull.Value)
                            {
                                productTypes.Description_ru = Convert.ToString(ProductTypeReader["Description_ru"]);
                            }
                            if (ProductTypeReader["Description_zh"] != DBNull.Value)
                            {
                                productTypes.Description_zh = Convert.ToString(ProductTypeReader["Description_zh"]);
                            }
                            if (ProductTypeReader["IdDefaultWayType"] != DBNull.Value)
                            {
                                productTypes.IdDefaultWayType = Convert.ToUInt32(ProductTypeReader["IdDefaultWayType"]);
                                productTypes.DefaultWayType = new DefaultWayType();
                                productTypes.DefaultWayType.IdDefaultWayType = Convert.ToUInt32(ProductTypeReader["IdDefaultWayType"]);
                                if (ProductTypeReader["DefaultWayType"] != DBNull.Value)
                                {
                                    productTypes.DefaultWayType.Name = Convert.ToString(ProductTypeReader["DefaultWayType"]);
                                }
                            }
                            if (ProductTypeReader["Code"] != DBNull.Value)
                            {
                                productTypes.Code = Convert.ToString(ProductTypeReader["Code"]);
                            }
                            if (ProductTypeReader["Standard"] != DBNull.Value)
                            {
                                productTypes.Standard = Convert.ToUInt64(ProductTypeReader["Standard"]);
                            }
                            if (ProductTypeReader["IdTemplate"] != DBNull.Value)
                            {
                                productTypes.IdTemplate = Convert.ToUInt64(ProductTypeReader["IdTemplate"]);
                                productTypes.Template = new Template();
                                productTypes.Template.IdTemplate = Convert.ToByte(ProductTypeReader["IdTemplate"]);
                                if (ProductTypeReader["TemplateName"] != DBNull.Value)
                                {
                                    productTypes.Template.Name = Convert.ToString(ProductTypeReader["TemplateName"]);
                                }
                            }
                            if (ProductTypeReader["IdStatus"] != DBNull.Value)
                            {
                                productTypes.IdStatus = Convert.ToInt32(ProductTypeReader["IdStatus"]);
                                productTypes.Status = new LookupValue();
                                productTypes.Status.IdLookupValue = Convert.ToInt32(ProductTypeReader["IdStatus"]);
                                if (ProductTypeReader["Status"] != DBNull.Value)
                                {
                                    productTypes.Status.Value = Convert.ToString(ProductTypeReader["Status"]);
                                }
                            }
                            if (ProductTypeReader["createdIn"] != DBNull.Value)
                            {
                                productTypes.CreatedIn = Convert.ToDateTime(ProductTypeReader["createdIn"]);
                            }
                            if (ProductTypeReader["LastUpdate"] != DBNull.Value)
                            {
                                productTypes.LastUpdate = Convert.ToDateTime(ProductTypeReader["LastUpdate"]);
                            }
                        }
                    }
                }
                productTypes.TemplateList = GetTemplatesByProductType(PCMConnectionString, productTypes.IdCPType);
                productTypes.WayList = GetWayListByProductType(PCMConnectionString, productTypes.IdCPType, productTypes.IdTemplate);
                //productTypes.DetectionList = GetDetectionListByProductType(PCMConnectionString, productTypes.IdCPType);
                //productTypes.OptionList = GetOptionListByProductType(PCMConnectionString, productTypes.IdCPType);
                productTypes.SparePartList = GetSparePartListByProductType(PCMConnectionString, productTypes.IdCPType, productTypes.IdTemplate);
                productTypes.FamilyList = GetFamiliesByProductType(PCMConnectionString, productTypes.IdCPType);
                productTypes.ProductTypeAttachedDocList = GetProductTypeAttachedDocsByIdProductType(PCMConnectionString, productTypes.IdCPType, ProductTypeAttachedDocPath);
                productTypes.ProductTypeAttachedLinkList = GetProductTypeAttachedLinksByIdProductType(PCMConnectionString, productTypes.IdCPType);
                productTypes.ProductTypeImageList = GetProductTypeImagesByIdProductType(PCMConnectionString, productTypes.IdCPType, ProductTypeImagePath);
                productTypes.ProductTypeLogEntryList = GetProductTypeLogEntriesByProductType(PCMConnectionString, productTypes.IdCPType);

                productTypes.DetectionList_Group = GetDetectionsWithGroupsByCpType(PCMConnectionString, productTypes.IdCPType, productTypes.IdTemplate);
                productTypes.OptionList_Group = GetOptionsWithGroupsByCpType(PCMConnectionString, productTypes.IdCPType, productTypes.IdTemplate);

                productTypes.CustomerList = GetCustomersWithRegionsByCPType(PCMConnectionString, productTypes.IdCPType);

                productTypes.ProductTypeCompatibilityList = GetCompatibilitiesByProductType(PCMConnectionString, (byte)productTypes.IdCPType);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeByIdCpType(). IdCpType- {0} ErrorMessage- {1}", IdCpType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return productTypes;
        }

        /// <summary>
        /// This method is used to get Detection by ID.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetection">Get detection id.</param>
        /// <param name="DetectionAttachedDocPath">Get attached doc path.</param>
        /// <param name="DetectionImagePath">Get image path.</param>
        /// <returns>The data of detection by id.</returns>
        public DetectionDetails GetDetectionByIdDetection(string PCMConnectionString, UInt32 IdDetection, string DetectionAttachedDocPath, string DetectionImagePath)
        {
            DetectionDetails detectionDetails = new DetectionDetails();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand DetectionDetailCommand = new MySqlCommand("PCM_GetDetectionByIdDetection", connDetectionDetails);
                    DetectionDetailCommand.CommandType = CommandType.StoredProcedure;
                    DetectionDetailCommand.Parameters.AddWithValue("_IdDetection", IdDetection);

                    using (MySqlDataReader DetectionDetailReader = DetectionDetailCommand.ExecuteReader())
                    {
                        while (DetectionDetailReader.Read())
                        {
                            detectionDetails.IdDetections = Convert.ToUInt32(DetectionDetailReader["IdDetection"]);

                            if (DetectionDetailReader["Name"] != DBNull.Value)
                            {
                                detectionDetails.Name = Convert.ToString(DetectionDetailReader["Name"]);
                            }
                            if (DetectionDetailReader["Description"] != DBNull.Value)
                            {
                                detectionDetails.Description = Convert.ToString(DetectionDetailReader["Description"]);
                            }
                            if (DetectionDetailReader["WeldOrder"] != DBNull.Value)
                            {
                                detectionDetails.WeldOrder = Convert.ToUInt32(DetectionDetailReader["WeldOrder"]);
                            }
                            if (DetectionDetailReader["Code"] != DBNull.Value)
                            {
                                detectionDetails.Code = Convert.ToString(DetectionDetailReader["Code"]);
                            }
                            if (DetectionDetailReader["IdTestType"] != DBNull.Value)
                            {
                                detectionDetails.IdTestType = Convert.ToUInt64(DetectionDetailReader["IdTestType"]);
                                detectionDetails.TestTypes = new TestTypes();
                                detectionDetails.TestTypes.IdTestType = Convert.ToUInt64(DetectionDetailReader["IdTestType"]);
                                if (DetectionDetailReader["TestName"] != DBNull.Value)
                                {
                                    detectionDetails.TestTypes.Name = Convert.ToString(DetectionDetailReader["TestName"]);
                                }
                            }
                            if (DetectionDetailReader["IdDetectionType"] != DBNull.Value)
                            {
                                detectionDetails.IdDetectionType = Convert.ToUInt32(DetectionDetailReader["IdDetectionType"]);
                                detectionDetails.DetectionTypes = new DetectionTypes();
                                detectionDetails.DetectionTypes.IdDetectionType = Convert.ToUInt32(DetectionDetailReader["IdDetectionType"]);
                                if (DetectionDetailReader["DetectionName"] != DBNull.Value)
                                {
                                    detectionDetails.DetectionTypes.Name = Convert.ToString(DetectionDetailReader["DetectionName"]);
                                }
                            }
                            if (DetectionDetailReader["Name_es"] != DBNull.Value)
                            {
                                detectionDetails.Name_es = Convert.ToString(DetectionDetailReader["Name_es"]);
                            }
                            if (DetectionDetailReader["Name_fr"] != DBNull.Value)
                            {
                                detectionDetails.Name_fr = Convert.ToString(DetectionDetailReader["Name_fr"]);
                            }
                            if (DetectionDetailReader["Name_pt"] != DBNull.Value)
                            {
                                detectionDetails.Name_pt = Convert.ToString(DetectionDetailReader["Name_pt"]);
                            }
                            if (DetectionDetailReader["Name_ro"] != DBNull.Value)
                            {
                                detectionDetails.Name_ro = Convert.ToString(DetectionDetailReader["Name_ro"]);
                            }
                            if (DetectionDetailReader["Name_ru"] != DBNull.Value)
                            {
                                detectionDetails.Name_ru = Convert.ToString(DetectionDetailReader["Name_ru"]);
                            }
                            if (DetectionDetailReader["Name_zh"] != DBNull.Value)
                            {
                                detectionDetails.Name_zh = Convert.ToString(DetectionDetailReader["Name_zh"]);
                            }
                            if (DetectionDetailReader["Description_es"] != DBNull.Value)
                            {
                                detectionDetails.Description_es = Convert.ToString(DetectionDetailReader["Description_es"]);
                            }
                            if (DetectionDetailReader["Description_fr"] != DBNull.Value)
                            {
                                detectionDetails.Description_fr = Convert.ToString(DetectionDetailReader["Description_fr"]);
                            }
                            if (DetectionDetailReader["Description_pt"] != DBNull.Value)
                            {
                                detectionDetails.Description_pt = Convert.ToString(DetectionDetailReader["Description_pt"]);
                            }
                            if (DetectionDetailReader["Description_ro"] != DBNull.Value)
                            {
                                detectionDetails.Description_ro = Convert.ToString(DetectionDetailReader["Description_ro"]);
                            }
                            if (DetectionDetailReader["Description_ru"] != DBNull.Value)
                            {
                                detectionDetails.Description_ru = Convert.ToString(DetectionDetailReader["Description_ru"]);
                            }
                            if (DetectionDetailReader["Description_zh"] != DBNull.Value)
                            {
                                detectionDetails.Description_zh = Convert.ToString(DetectionDetailReader["Description_zh"]);
                            }

                            if (DetectionDetailReader["IdGroup"] != DBNull.Value)
                            {
                                detectionDetails.IdGroup = Convert.ToUInt32(DetectionDetailReader["IdGroup"]);
                                detectionDetails.DetectionGroup = new DetectionGroup();
                                detectionDetails.DetectionGroup.IdGroup = Convert.ToUInt32(DetectionDetailReader["IdGroup"]);
                                if (DetectionDetailReader["GroupName"] != DBNull.Value)
                                {
                                    detectionDetails.DetectionGroup.Name = Convert.ToString(DetectionDetailReader["GroupName"]);
                                }
                            }
                        }
                    }
                }
                detectionDetails.DetectionAttachedDocList = GetDetectionAttachedDocsByIdDetection(PCMConnectionString, detectionDetails.IdDetections, DetectionAttachedDocPath);
                detectionDetails.DetectionAttachedLinkList = GetDetectionAttachedLinksByIdDetection(PCMConnectionString, detectionDetails.IdDetections);
                detectionDetails.DetectionImageList = GetDetectionImagesByIdDetection(PCMConnectionString, detectionDetails.IdDetections, DetectionImagePath);
                //option and detection group list
                if (detectionDetails.IdDetectionType == 2 || detectionDetails.IdDetectionType == 3)
                {
                    detectionDetails.DetectionGroupList = GetDetectionGroupsByDetectionType(PCMConnectionString, detectionDetails.IdDetectionType);
                }

                detectionDetails.CustomerList = GetCustomersWithRegionsByDetection(PCMConnectionString, detectionDetails.IdDetections, detectionDetails.IdDetectionType);
                detectionDetails.DetectionLogEntryList = GetDetectionLogEntriesByDetection(PCMConnectionString, detectionDetails.IdDetections, detectionDetails.IdDetectionType);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionByIdDetection(). IdDetection- {0} ErrorMessage- {1}", IdDetection, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detectionDetails;
        }

        /// <summary>
        /// This method is used to get all Languages.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Language.</returns>
        public List<Language> GetAllLanguages(string PCMConnectionString)
        {
            List<Language> Languages = new List<Language>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllLanguages", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Language Language = new Language();
                            Language.IdLanguage = Convert.ToInt32(reader["IdLanguage"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Language.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["TwoLetterIsoLanguage"] != DBNull.Value)
                            {
                                Language.TwoLetterISOLanguage = Convert.ToString(reader["TwoLetterIsoLanguage"]);
                            }
                            Languages.Add(Language);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllLanguages(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Languages;
        }

        /// <summary>
        /// This method is used to get Product Type Images by id product type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdProductType">Get product type image id.</param>
        /// <param name="ProductTypeImagePath">Get product type image path.</param>
        /// <returns>The list of Product Type Image by id product type.</returns>
        public List<ProductTypeImage> GetProductTypeImagesByIdProductType(string PCMConnectionString, UInt64 IdProductType, string ProductTypeImagePath)
        {
            List<ProductTypeImage> ProductTypeImages = new List<ProductTypeImage>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCptypeImagesByIdCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdProductType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypeImage ProductTypeImage = new ProductTypeImage();

                            ProductTypeImage.IdCPTypeImage = Convert.ToUInt64(reader["IdCPTypeImage"]);
                            ProductTypeImage.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                ProductTypeImage.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ProductTypeImage.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                ProductTypeImage.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                ProductTypeImage.Position = Convert.ToUInt64(reader["Position"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                ProductTypeImage.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            ProductTypeImage.ProductTypeImageInBytes = GetProductTypeImage(Convert.ToString(ProductTypeImage.IdCPTypeImage), ProductTypeImagePath, ProductTypeImage.SavedFileName);
                            ProductTypeImages.Add(ProductTypeImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeImagesByIdProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ProductTypeImages;
        }

        /// <summary>
        /// This method is used to get product type image by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdProductTypeImage">Get product type image id.</param>
        /// <param name="ProductTypeImagePath">Get product type image path.</param>
        /// <returns>The data of product type image by id.</returns>
        public ProductTypeImage GetProductTypeImagesByIdProductTypeImage(string PCMConnectionString, UInt64 IdProductTypeImage, string ProductTypeImagePath)
        {
            ProductTypeImage productTypeImage = new ProductTypeImage();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCptypeImagesByIdCPTypeImage", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeImage", IdProductTypeImage);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productTypeImage.IdCPTypeImage = Convert.ToUInt64(reader["IdCPTypeImage"]);
                            productTypeImage.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                productTypeImage.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                productTypeImage.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                productTypeImage.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                productTypeImage.Position = Convert.ToUInt64(reader["Position"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                productTypeImage.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            productTypeImage.ProductTypeImageInBytes = GetProductTypeImage(Convert.ToString(productTypeImage.IdCPTypeImage), ProductTypeImagePath, productTypeImage.SavedFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeImagesByIdProductTypeImage(). IdProductTypeImage- {0} ErrorMessage- {1}", IdProductTypeImage, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return productTypeImage;
        }

        /// <summary>
        /// This method is used to get product type image by id.
        /// </summary>
        /// <param name="IdProductTypeImage">Get product type image id.</param>
        /// <param name="ProductTypeImagePath">Get Image Path.</param>
        /// <param name="SavedFileName">Get File Name.</param>
        public byte[] GetProductTypeImage(string IdProductTypeImage, string ProductTypeImagePath, string SavedFileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", ProductTypeImagePath, IdProductTypeImage, SavedFileName);

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeImage(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get Detection Images by id Detection.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetection">Get Detection image id.</param>
        /// <param name="DetectionImagePath">Get Detection image path.</param>
        /// <returns>The list of Detection Image by id Detection.</returns>
        public List<DetectionImage> GetDetectionImagesByIdDetection(string PCMConnectionString, UInt32 IdDetection, string DetectionImagePath)
        {
            List<DetectionImage> DetectionImages = new List<DetectionImage>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionImagesByIdDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionImage DetectionImage = new DetectionImage();

                            DetectionImage.IdDetectionImage = Convert.ToUInt32(reader["IdDetectionImage"]);
                            DetectionImage.IdDetection = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                DetectionImage.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionImage.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                DetectionImage.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                DetectionImage.Position = Convert.ToUInt64(reader["Position"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionImage.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            DetectionImage.DetectionImageInBytes = GetDetectionImage(Convert.ToString(DetectionImage.IdDetectionImage), DetectionImagePath, DetectionImage.SavedFileName);
                            DetectionImages.Add(DetectionImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionImagesByIdDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionImages;
        }

        /// <summary>
        /// This method is used to get Detection image by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionImage">Get Detection image id.</param>
        /// <param name="DetectionImagePath">Get Detection image path.</param>
        /// <returns>The data of Detection image by id.</returns>
        public DetectionImage GetDetectionImagesByIdDetectionImage(string PCMConnectionString, UInt32 IdDetectionImage, string DetectionImagePath)
        {
            DetectionImage DetectionImage = new DetectionImage();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionImagesByIdDetectionImage", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionImage", IdDetectionImage);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionImage.IdDetectionImage = Convert.ToUInt32(reader["IdDetectionImage"]);
                            DetectionImage.IdDetection = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                DetectionImage.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionImage.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                DetectionImage.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                DetectionImage.Position = Convert.ToUInt64(reader["Position"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionImage.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            DetectionImage.DetectionImageInBytes = GetDetectionImage(Convert.ToString(DetectionImage.IdDetectionImage), DetectionImagePath, DetectionImage.SavedFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionImagesByIdDetectionImage(). IdDetectionImage- {0} ErrorMessage- {1}", IdDetectionImage, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionImage;
        }

        /// <summary>
        /// This method is used to get detection image by id.
        /// </summary>
        /// <param name="IdDetectionImage">Get detection image id.</param>
        /// <param name="DetectionImagePath">Get Image Path.</param>
        /// <param name="SavedFileName">Get File Name.</param>
        public byte[] GetDetectionImage(string IdDetectionImage, string DetectionImagePath, string SavedFileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", DetectionImagePath, IdDetectionImage, SavedFileName);

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionImage(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get Product Type Attached Docs by id product type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdProductType">Get product type Attached Doc id.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type attached doc path.</param>
        /// <returns>The list of Product Type Attached Doc by id product type.</returns>
        public List<ProductTypeAttachedDoc> GetProductTypeAttachedDocsByIdProductType(string PCMConnectionString, UInt64 IdProductType, string ProductTypeAttachedDocPath)
        {
            List<ProductTypeAttachedDoc> ProductTypeAttachedDocs = new List<ProductTypeAttachedDoc>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCptypeAttachedDocsByIdCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdProductType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypeAttachedDoc ProductTypeAttachedDoc = new ProductTypeAttachedDoc();

                            ProductTypeAttachedDoc.IdCPTypeAttachedDoc = Convert.ToUInt32(reader["IdCPTypeAttachedDoc"]);
                            ProductTypeAttachedDoc.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                ProductTypeAttachedDoc.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ProductTypeAttachedDoc.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                ProductTypeAttachedDoc.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["IdDocType"] != DBNull.Value)
                            {
                                ProductTypeAttachedDoc.IdDocType = Convert.ToInt32(reader["IdDocType"]);
                                ProductTypeAttachedDoc.DocumentType = new DocumentType();
                                ProductTypeAttachedDoc.DocumentType.IdDocumentType = Convert.ToByte(reader["IdDocType"]);
                                if (reader["DocumentType"] != DBNull.Value)
                                {
                                    ProductTypeAttachedDoc.DocumentType.Name = Convert.ToString(reader["DocumentType"]);
                                }
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                ProductTypeAttachedDoc.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            ProductTypeAttachedDoc.ProductTypeAttachedDocInBytes = GetProductTypeAttachedDoc(Convert.ToString(ProductTypeAttachedDoc.IdCPTypeAttachedDoc), ProductTypeAttachedDocPath, ProductTypeAttachedDoc.SavedFileName);
                            ProductTypeAttachedDocs.Add(ProductTypeAttachedDoc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeAttachedDocsByIdProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ProductTypeAttachedDocs;
        }

        /// <summary>
        /// This method is used to get product type Attached Doc by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdProductTypeAttachedDoc">Get product type Attached Doc id.</param>
        /// <param name="ProductTypeAttachedDocPath">Get product type attached doc path.</param>
        /// <returns>The data of product type Attached Doc by id.</returns>
        public ProductTypeAttachedDoc GetProductTypeAttachedDocsByIdProductTypeAttachedDoc(string PCMConnectionString, Int32 IdProductTypeAttachedDoc, string ProductTypeAttachedDocPath)
        {
            ProductTypeAttachedDoc productTypeAttachedDoc = new ProductTypeAttachedDoc();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCptypeAttachedDocsByIdCPTypeAttachedDoc", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedDoc", IdProductTypeAttachedDoc);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productTypeAttachedDoc.IdCPTypeAttachedDoc = Convert.ToUInt32(reader["IdCPTypeAttachedDoc"]);
                            productTypeAttachedDoc.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                productTypeAttachedDoc.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                productTypeAttachedDoc.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                productTypeAttachedDoc.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["IdDocType"] != DBNull.Value)
                            {
                                productTypeAttachedDoc.IdDocType = Convert.ToInt32(reader["IdDocType"]);
                                productTypeAttachedDoc.DocumentType = new DocumentType();
                                productTypeAttachedDoc.DocumentType.IdDocumentType = Convert.ToByte(reader["IdDocType"]);
                                if (reader["DocumentType"] != DBNull.Value)
                                {
                                    productTypeAttachedDoc.DocumentType.Name = Convert.ToString(reader["DocumentType"]);
                                }
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                productTypeAttachedDoc.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            productTypeAttachedDoc.ProductTypeAttachedDocInBytes = GetProductTypeAttachedDoc(Convert.ToString(productTypeAttachedDoc.IdCPTypeAttachedDoc), ProductTypeAttachedDocPath, productTypeAttachedDoc.SavedFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeAttachedDocsByIdProductTypeAttachedDoc(). IdProductTypeAttachedDoc- {0} ErrorMessage- {1}", IdProductTypeAttachedDoc, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return productTypeAttachedDoc;
        }

        /// <summary>
        /// This method is used to get product type Attached Doc by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdProductTypeAttachedDoc">Get product type Attached Doc id.</param>
        /// <param name="ProductTypeAttachedDocPath">Get Attached Doc Path.</param>
        /// <param name="SavedFileName">Get File Name.</param>
        public byte[] GetProductTypeAttachedDoc(string IdProductTypeAttachedDoc, string ProductTypeAttachedDocPath, string SavedFileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", ProductTypeAttachedDocPath, IdProductTypeAttachedDoc, SavedFileName);

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get Catalogue Item Attached Docs by id Catalogue Item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get Catalogue Item id.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get Catalogue Item attached doc path.</param>
        /// <returns>The list of Catalogue Item Attached Doc by id Catalogue Item.</returns>
        public List<CatalogueItemAttachedDoc> GetCatalogueItemAttachedDocsByIdCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem, string CatalogueItemAttachedDocPath)
        {
            List<CatalogueItemAttachedDoc> CatalogueItemAttachedDocs = new List<CatalogueItemAttachedDoc>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCatalogueItemAttachedDocsByIdCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatalogueItemAttachedDoc CatalogueItemAttachedDoc = new CatalogueItemAttachedDoc();

                            CatalogueItemAttachedDoc.IdCatalogueItemAttachedDoc = Convert.ToUInt32(reader["IdCatalogueItemAttachedDoc"]);
                            CatalogueItemAttachedDoc.IdCatalogueItem = Convert.ToUInt32(reader["IdCatalogueItem"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["idDocType"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.IdDocType = Convert.ToUInt32(reader["idDocType"]);
                                CatalogueItemAttachedDoc.DocumentType = new DocumentType();
                                CatalogueItemAttachedDoc.DocumentType.IdDocumentType = Convert.ToByte(reader["idDocType"]);
                                if (reader["DocumentType"] != DBNull.Value)
                                {
                                    CatalogueItemAttachedDoc.DocumentType.Name = Convert.ToString(reader["DocumentType"]);
                                }
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            CatalogueItemAttachedDoc.CatalogueItemAttachedDocInBytes = GetCatalogueItemAttachedDoc(Convert.ToString(CatalogueItemAttachedDoc.IdCatalogueItemAttachedDoc), CatalogueItemAttachedDocPath, CatalogueItemAttachedDoc.SavedFileName);
                            CatalogueItemAttachedDocs.Add(CatalogueItemAttachedDoc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCatalogueItemAttachedDocsByIdCatalogueItem(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return CatalogueItemAttachedDocs;
        }

        /// <summary>
        /// This method is used to get catalogue item Attached Doc by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItemAttachedDoc">Get catalogue item Attached Doc id.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get catalogue item attached doc path.</param>
        /// <returns>The data of catalogue item Attached Doc by id.</returns>
        public CatalogueItemAttachedDoc GetCatalogueItemAttachedDocsById(string PCMConnectionString, UInt32 IdCatalogueItemAttachedDoc, string CatalogueItemAttachedDocPath)
        {
            CatalogueItemAttachedDoc CatalogueItemAttachedDoc = new CatalogueItemAttachedDoc();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCatalogueItemAttachedDocsByIdCatalogueItemAttachedDoc", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItemAttachedDoc", IdCatalogueItemAttachedDoc);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatalogueItemAttachedDoc.IdCatalogueItemAttachedDoc = Convert.ToUInt32(reader["IdCatalogueItemAttachedDoc"]);
                            CatalogueItemAttachedDoc.IdCatalogueItem = Convert.ToUInt32(reader["IdCatalogueItem"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["idDocType"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.IdDocType = Convert.ToUInt32(reader["idDocType"]);
                                CatalogueItemAttachedDoc.DocumentType = new DocumentType();
                                CatalogueItemAttachedDoc.DocumentType.IdDocumentType = Convert.ToByte(reader["idDocType"]);
                                if (reader["DocumentType"] != DBNull.Value)
                                {
                                    CatalogueItemAttachedDoc.DocumentType.Name = Convert.ToString(reader["DocumentType"]);
                                }
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                CatalogueItemAttachedDoc.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            CatalogueItemAttachedDoc.CatalogueItemAttachedDocInBytes = GetCatalogueItemAttachedDoc(Convert.ToString(CatalogueItemAttachedDoc.IdCatalogueItemAttachedDoc), CatalogueItemAttachedDocPath, CatalogueItemAttachedDoc.SavedFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCatalogueItemAttachedDocsById(). IdCatalogueItemAttachedDoc- {0} ErrorMessage- {1}", IdCatalogueItemAttachedDoc, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return CatalogueItemAttachedDoc;
        }

        /// <summary>
        /// This method is used to get catalogue item Attached Doc by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItemAttachedDoc">Get catalogue item Attached Doc id.</param>
        /// <param name="CatalogueItemAttachedDocPath">Get Attached Doc Path.</param>
        /// <param name="SavedFileName">Get File Name.</param>
        public byte[] GetCatalogueItemAttachedDoc(string IdCatalogueItemAttachedDoc, string CatalogueItemAttachedDocPath, string SavedFileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", CatalogueItemAttachedDocPath, IdCatalogueItemAttachedDoc, SavedFileName);

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCatalogueItemAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get Detection Attached Docs by id Detection.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetection">Get Detection Attached Doc id.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection attached doc path.</param>
        /// <returns>The list of Detection Attached Doc by id Detection.</returns>
        public List<DetectionAttachedDoc> GetDetectionAttachedDocsByIdDetection(string PCMConnectionString, UInt32 IdDetection, string DetectionAttachedDocPath)
        {
            List<DetectionAttachedDoc> DetectionAttachedDocs = new List<DetectionAttachedDoc>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionAttachedDocsByIdDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionAttachedDoc DetectionAttachedDoc = new DetectionAttachedDoc();

                            DetectionAttachedDoc.IdDetectionAttachedDoc = Convert.ToUInt32(reader["IdDetectionAttachedDoc"]);
                            DetectionAttachedDoc.IdDetection = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["IdDocType"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.IdDocType = Convert.ToUInt32(reader["IdDocType"]);
                                DetectionAttachedDoc.DocumentType = new DocumentType();
                                DetectionAttachedDoc.DocumentType.IdDocumentType = Convert.ToByte(reader["IdDocType"]);
                                if (reader["DocumentType"] != DBNull.Value)
                                {
                                    DetectionAttachedDoc.DocumentType.Name = Convert.ToString(reader["DocumentType"]);
                                }
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            DetectionAttachedDoc.DetectionAttachedDocInBytes = GetDetectionAttachedDoc(Convert.ToString(DetectionAttachedDoc.IdDetectionAttachedDoc), DetectionAttachedDocPath, DetectionAttachedDoc.SavedFileName);
                            DetectionAttachedDocs.Add(DetectionAttachedDoc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDocsByIdDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionAttachedDocs;
        }

        /// <summary>
        /// This method is used to get Detection Attached Doc by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionAttachedDoc">Get Detection Attached Doc id.</param>
        /// <param name="DetectionAttachedDocPath">Get Detection attached doc path.</param>
        /// <returns>The data of Detection Attached Doc by id.</returns>
        public DetectionAttachedDoc GetDetectionAttachedDocsById(string PCMConnectionString, UInt32 IdDetectionAttachedDoc, string DetectionAttachedDocPath)
        {
            DetectionAttachedDoc DetectionAttachedDoc = new DetectionAttachedDoc();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionAttachedDocsByIdDetectionAttachedDoc", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedDoc", IdDetectionAttachedDoc);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionAttachedDoc.IdDetectionAttachedDoc = Convert.ToUInt32(reader["IdDetectionAttachedDoc"]);
                            DetectionAttachedDoc.IdDetection = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["IdDocType"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.IdDocType = Convert.ToUInt32(reader["IdDocType"]);
                                DetectionAttachedDoc.DocumentType = new DocumentType();
                                DetectionAttachedDoc.DocumentType.IdDocumentType = Convert.ToByte(reader["IdDocType"]);
                                if (reader["DocumentType"] != DBNull.Value)
                                {
                                    DetectionAttachedDoc.DocumentType.Name = Convert.ToString(reader["DocumentType"]);
                                }
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionAttachedDoc.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            DetectionAttachedDoc.DetectionAttachedDocInBytes = GetDetectionAttachedDoc(Convert.ToString(DetectionAttachedDoc.IdDetectionAttachedDoc), DetectionAttachedDocPath, DetectionAttachedDoc.SavedFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDocsById(). IdDetectionAttachedDoc- {0} ErrorMessage- {1}", IdDetectionAttachedDoc, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionAttachedDoc;
        }

        /// <summary>
        /// This method is used to get Detection Attached Doc by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionAttachedDoc">Get Detection Attached Doc id.</param>
        /// <param name="DetectionAttachedDocPath">Get Attached Doc Path.</param>
        /// <param name="SavedFileName">Get File Name.</param>
        public byte[] GetDetectionAttachedDoc(string IdDetectionAttachedDoc, string DetectionAttachedDocPath, string SavedFileName)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", DetectionAttachedDocPath, IdDetectionAttachedDoc, SavedFileName);

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedDoc(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get Catalogue Item Attached Links by id Catalogue Item.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItem">Get Catalogue Item id.</param>
        /// <returns>The list of Catalogue Item Attached Link by id Catalogue Item.</returns>
        public List<CatalogueItemAttachedLink> GetCatalogueItemAttachedLinksByIdCatalogueItem(string PCMConnectionString, UInt32 IdCatalogueItem)
        {
            List<CatalogueItemAttachedLink> CatalogueItemAttachedLinks = new List<CatalogueItemAttachedLink>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCatalogueItemsAttachedLinksByIdCatalogueItem", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItem", IdCatalogueItem);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatalogueItemAttachedLink CatalogueItemAttachedLink = new CatalogueItemAttachedLink();

                            CatalogueItemAttachedLink.IdCatalogueItemAttachedLink = Convert.ToUInt32(reader["IdCatalogueItemAttachedLink"]);
                            CatalogueItemAttachedLink.IdCatalogueItem = Convert.ToUInt32(reader["IdCatalogueItem"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Address"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.Address = Convert.ToString(reader["Address"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            CatalogueItemAttachedLinks.Add(CatalogueItemAttachedLink);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCatalogueItemAttachedLinksByIdCatalogueItem(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return CatalogueItemAttachedLinks;
        }

        /// <summary>
        /// This method is used to get catalogue item Attached Link by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCatalogueItemAttachedLink">Get catalogue item Attached Link id.</param>
        /// <returns>The data of catalogue item Attached Link by id.</returns>
        public CatalogueItemAttachedLink GetCatalogueItemAttachedLinkById(string PCMConnectionString, UInt32 IdCatalogueItemAttachedLink)
        {
            CatalogueItemAttachedLink CatalogueItemAttachedLink = new CatalogueItemAttachedLink();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCatalogueItemsAttachedLinksByIdCatalogueItemAttachedLink", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCatalogueItemAttachedLink", IdCatalogueItemAttachedLink);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatalogueItemAttachedLink.IdCatalogueItemAttachedLink = Convert.ToUInt32(reader["IdCatalogueItemAttachedLink"]);
                            CatalogueItemAttachedLink.IdCatalogueItem = Convert.ToUInt32(reader["IdCatalogueItem"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Address"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.Address = Convert.ToString(reader["Address"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                CatalogueItemAttachedLink.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCatalogueItemAttachedLinkById(). IdCatalogueItemAttachedLink- {0} ErrorMessage- {1}", IdCatalogueItemAttachedLink, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return CatalogueItemAttachedLink;
        }

        /// <summary>
        /// This method is used to get Product Type Attached Links by id Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get Product Type Attached Link id.</param>
        /// <returns>The list of Product Type Attached Link by id Product Type.</returns>
        public List<ProductTypeAttachedLink> GetProductTypeAttachedLinksByIdProductType(string PCMConnectionString, UInt64 IdCPType)
        {
            List<ProductTypeAttachedLink> ProductTypeAttachedLinks = new List<ProductTypeAttachedLink>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCptypesAttachedLinksByIdCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypeAttachedLink ProductTypeAttachedLink = new ProductTypeAttachedLink();

                            ProductTypeAttachedLink.IdCPTypeAttachedLink = Convert.ToUInt32(reader["IdCPTypeAttachedLink"]);
                            ProductTypeAttachedLink.IdCPType = Convert.ToUInt32(reader["IdCPType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Address"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.Address = Convert.ToString(reader["Address"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            ProductTypeAttachedLinks.Add(ProductTypeAttachedLink);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeAttachedLinksByIdProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ProductTypeAttachedLinks;
        }

        /// <summary>
        /// This method is used to get catalogue item Attached Link by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdProductTypeAttachedLink">Get catalogue item Attached Link id.</param>
        /// <returns>The data of catalogue item Attached Link by id.</returns>
        public ProductTypeAttachedLink GetProductTypeAttachedLinkById(string PCMConnectionString, UInt32 IdCPTypeAttachedLink)
        {
            ProductTypeAttachedLink ProductTypeAttachedLink = new ProductTypeAttachedLink();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCptypesAttachedLinksByIdCPTypeAttachedLink", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPTypeAttachedLink", IdCPTypeAttachedLink);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypeAttachedLink.IdCPTypeAttachedLink = Convert.ToUInt32(reader["IdCPTypeAttachedLink"]);
                            ProductTypeAttachedLink.IdCPType = Convert.ToUInt32(reader["IdCPType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Address"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.Address = Convert.ToString(reader["Address"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                ProductTypeAttachedLink.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeAttachedLinkById(). IdCPTypeAttachedLink- {0} ErrorMessage- {1}", IdCPTypeAttachedLink, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ProductTypeAttachedLink;
        }

        /// <summary>
        /// This method is used to get Detection Attached Links by id Detection.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetection">Get Detection id.</param>
        /// <returns>The list of Detection Attached Link by id Detection.</returns>
        public List<DetectionAttachedLink> GetDetectionAttachedLinksByIdDetection(string PCMConnectionString, UInt32 IdDetection)
        {
            List<DetectionAttachedLink> DetectionAttachedLinks = new List<DetectionAttachedLink>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsAttachedLinksByIdDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionAttachedLink DetectionAttachedLink = new DetectionAttachedLink();

                            DetectionAttachedLink.IdDetectionAttachedLink = Convert.ToUInt32(reader["IdDetectionAttachedLink"]);
                            DetectionAttachedLink.IdDetection = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionAttachedLink.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Address"] != DBNull.Value)
                            {
                                DetectionAttachedLink.Address = Convert.ToString(reader["Address"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionAttachedLink.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionAttachedLink.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            DetectionAttachedLinks.Add(DetectionAttachedLink);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedLinksByIdDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionAttachedLinks;
        }

        /// <summary>
        /// This method is used to get catalogue item Attached Link by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionAttachedLink">Get catalogue item Attached Link id.</param>
        /// <returns>The data of catalogue item Attached Link by id.</returns>
        public DetectionAttachedLink GetDetectionAttachedLinkById(string PCMConnectionString, UInt32 IdDetectionAttachedLink)
        {
            DetectionAttachedLink DetectionAttachedLink = new DetectionAttachedLink();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsAttachedLinksByIdDetectionAttachedLink", connDetectionDetails);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionAttachedLink", IdDetectionAttachedLink);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionAttachedLink.IdDetectionAttachedLink = Convert.ToUInt32(reader["IdDetectionAttachedLink"]);
                            DetectionAttachedLink.IdDetection = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionAttachedLink.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Address"] != DBNull.Value)
                            {
                                DetectionAttachedLink.Address = Convert.ToString(reader["Address"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionAttachedLink.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionAttachedLink.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionAttachedLinkById(). IdDetectionAttachedLink- {0} ErrorMessage- {1}", IdDetectionAttachedLink, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionAttachedLink;
        }

        /// <summary>
        /// This method is to get latest Product Type Reference
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>latest Product Type Reference</returns>
        public string GetLatestProuductTypeReference(string PCMConnectionString)
        {
            UInt32 idCPType;
            string cptypeReference;
            string Characters = "CP";
            Int16 Year = 0;
            string Seperator = ".";
            Int16 Number = 0;

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetLatestProuductTypeReference", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["IdCPType"] != DBNull.Value)
                                idCPType = Convert.ToUInt32(reader["IdCPType"]);

                            if (reader["cptypeReference"] != DBNull.Value)
                                cptypeReference = Convert.ToString(reader["cptypeReference"]);

                            if (reader["Characters"] != DBNull.Value)
                                Characters = Convert.ToString(reader["Characters"]);

                            if (reader["Year"] != DBNull.Value)
                                Year = Convert.ToInt16(reader["Year"]);

                            if (reader["Seperator"] != DBNull.Value)
                                Seperator = Convert.ToString(reader["Seperator"]);

                            if (reader["Number"] != DBNull.Value)
                                Number = Convert.ToInt16(reader["Number"]);
                        }
                    }
                }

                Int16 currentYear = Convert.ToInt16(DateTime.Now.ToString("yy"));

                if (Year == currentYear)
                {
                    Number += 1;
                    return string.Format("{0}{1}{2}{3}", Characters, currentYear, Seperator, Number.ToString("0000"));
                }
                else if (currentYear > Year)
                {
                    return string.Format("{0}{1}{2}{3}", "CP", currentYear, Seperator, 1.ToString("0000"));
                }
                else
                {
                    return string.Format("{0}{1}{2}{3}", "CP", currentYear, Seperator, 1.ToString("0000"));
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLatestProuductTypeReference(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        /// <summary>
        /// This method is used to get Families by Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get Product Type id.</param>
        /// <returns>The list of Families by Product Type.</returns>
        public List<ConnectorFamilies> GetFamiliesByProductType(string PCMConnectionString, UInt64 IdCPType)
        {
            List<ConnectorFamilies> ConnectorFamilies = new List<ConnectorFamilies>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetFamiliesByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ConnectorFamilies ConnectorFamily = new ConnectorFamilies();
                            ConnectorFamily.IdFamily = Convert.ToUInt64(reader["IdFamily"]);
                            if (reader["Name"] != DBNull.Value)
                            {
                                ConnectorFamily.Name = Convert.ToString(reader["Name"]);
                            }
                            ConnectorFamilies.Add(ConnectorFamily);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetFamiliesByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return ConnectorFamilies;
        }

        /// <summary>
        /// This method is used to get Way List by Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get Product Type id.</param>
        /// <returns>The list of Ways by Product Type.</returns>
        public List<Ways> GetWayListByProductType(string PCMConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Ways> Ways = new List<Ways>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 1); //Detetction type: 1 for ways. 
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Ways Way = new Ways();

                            Way.IdWays = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Way.Name = Convert.ToString(reader["Name"]);
                            }

                            Ways.Add(Way);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetWayListByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Ways;
        }

        /// <summary>
        /// This method is used to get Detection List by Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get Product Type id.</param>
        /// <returns>The list of Detections by Product Type.</returns>
        public List<Detections> GetDetectionListByProductType(string PCMConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Detections> Detections = new List<Detections>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 2); //Detetction type: 2 for detections. 
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections Detection = new Detections();

                            Detection.IdDetections = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Detection.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["IdGroup"] != DBNull.Value)
                            {
                                Detection.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            }
                            if (reader["GroupName"] != DBNull.Value)
                            {
                                Detection.GroupName = Convert.ToString(reader["GroupName"]);
                            }
                            if (reader["GroupOrder"] != DBNull.Value)
                            {
                                Detection.OrderNumber = Convert.ToInt32(reader["GroupOrder"]);
                            }

                            Detections.Add(Detection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionListByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Detections;
        }

        /// <summary>
        /// This method is used to get Option List by Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get Product Type id.</param>
        /// <returns>The list of Options by Product Type.</returns>
        public List<Options> GetOptionListByProductType(string PCMConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Options> Options = new List<Options>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 3); //Detetction type: 3 for options. 
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options Option = new Options();

                            Option.IdOptions = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Option.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["IdGroup"] != DBNull.Value)
                            {
                                Option.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            }
                            if (reader["GroupName"] != DBNull.Value)
                            {
                                Option.GroupName = Convert.ToString(reader["GroupName"]);
                            }
                            if (reader["GroupOrder"] != DBNull.Value)
                            {
                                Option.OrderNumber = Convert.ToInt32(reader["GroupOrder"]);
                            }

                            Options.Add(Option);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOptionListByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Options;
        }

        /// <summary>
        /// This method is used to get Spare Part List by Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get Product Type id.</param>
        /// <returns>The list of Spare Parts by Product Type.</returns>
        public List<SpareParts> GetSparePartListByProductType(string PCMConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<SpareParts> SpareParts = new List<SpareParts>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 4); //Detetction type: 4 for spare parts. 
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SpareParts SparePart = new SpareParts();

                            SparePart.IdSpareParts = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                SparePart.Name = Convert.ToString(reader["Name"]);
                            }

                            SpareParts.Add(SparePart);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetSparePartListByProductType(). IdCPType- {0} ErrorMessage- {1}", IdCPType, ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return SpareParts;
        }

        /// <summary>
        /// This method is used to get default way type List.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of default way type.</returns>
        public List<DefaultWayType> GetAllDefaultWayTypeList(string PCMConnectionString)
        {
            List<DefaultWayType> defaultWayTypes = new List<DefaultWayType>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllDefaultWayTypes", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DefaultWayType defaultWayType = new DefaultWayType();

                            defaultWayType.IdDefaultWayType = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                defaultWayType.Name = Convert.ToString(reader["Name"]);
                            }

                            defaultWayTypes.Add(defaultWayType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllDefaultWayTypeList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return defaultWayTypes;
        }

        /// <summary>
        /// This method is used to get all templates by product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of templates by product Type.</returns>
        public List<Template> GetTemplatesByProductType(string PCMConnectionString, UInt64 IdCPType)
        {
            List<Template> templates = new List<Template>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetTemplatesByProductType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Template template = new Template();
                            template.IdTemplate = Convert.ToByte(reader["IdTemplate"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                template.Name = Convert.ToString(reader["Name"]);
                            }
                            templates.Add(template);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTemplatesByProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return templates;
        }

        /// <summary>
        /// This method is used to get log entries by product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get product type id</param>
        /// <returns>The list of log by product Type.</returns>
        public List<ProductTypeLogEntry> GetProductTypeLogEntriesByProductType(string PCMConnectionString, UInt64 IdCPType)
        {
            List<ProductTypeLogEntry> CPTypeLogEntries = new List<ProductTypeLogEntry>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetLogEntriesByIdCptype", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypeLogEntry ProductTypeLogEntry = new ProductTypeLogEntry();

                            ProductTypeLogEntry.IdLogEntryByCptype = Convert.ToUInt32(reader["IdLogEntryByCptype"]);

                            ProductTypeLogEntry.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["IdUser"] != DBNull.Value)
                            {
                                ProductTypeLogEntry.IdUser = Convert.ToUInt32(reader["IdUser"]);
                            }
                            if (reader["UserName"] != DBNull.Value)
                            {
                                ProductTypeLogEntry.UserName = Convert.ToString(reader["UserName"]);
                            }
                            if (reader["Datetime"] != DBNull.Value)
                            {
                                ProductTypeLogEntry.Datetime = Convert.ToDateTime(reader["Datetime"]);
                            }
                            if (reader["Comments"] != DBNull.Value)
                            {
                                ProductTypeLogEntry.Comments = Convert.ToString(reader["Comments"]);
                            }
                            CPTypeLogEntries.Add(ProductTypeLogEntry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypeLogEntriesByProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return CPTypeLogEntries;
        }

        /// <summary>
        /// This method is used to get Detection Groups.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The list of Detection Groups.</returns>
        public List<DetectionOrderGroup> GetDetectionOrderGroup(string PCMConnectionString, UInt32 IdDetectionType)
        {
            List<DetectionOrderGroup> DetectionOrderGroups = new List<DetectionOrderGroup>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionOrderGroups", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionOrderGroup DetectionOrderGroup = new DetectionOrderGroup();

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                DetectionOrderGroup.Key = Convert.ToString(reader["KeyName"]);
                            }

                            DetectionOrderGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionOrderGroup.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                DetectionOrderGroup.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }

                            DetectionOrderGroups.Add(DetectionOrderGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionOrderGroup(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionOrderGroups;
        }

        /// <summary>
        /// This method is used to get Detection order Groups with detections.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The list of Detection Groups with detctions.</returns>
        public List<DetectionOrderGroup> GetDetectionOrderGroupsWithDetections(string PCMConnectionString, UInt32 IdDetectionType)
        {
            List<DetectionOrderGroup> detectionOrderGroups = new List<DetectionOrderGroup>();
            try
            {
                detectionOrderGroups = GetDetectionOrderGroup(PCMConnectionString, IdDetectionType);
                if (detectionOrderGroups.Count > 0)
                {
                    int i = 1;

                    using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByDetectionGroups", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                        using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DetectionOrderGroup detectionOrderGroup = new DetectionOrderGroup();

                                detectionOrderGroup.Key = Convert.ToString(i);

                                detectionOrderGroup.IdDetection = Convert.ToUInt32(reader["IdDetection"]);
                                detectionOrderGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                                if (reader["DetectionName"] != DBNull.Value)
                                {
                                    detectionOrderGroup.Name = Convert.ToString(reader["DetectionName"]);
                                }

                                if (reader["Parent"] != DBNull.Value)
                                {
                                    detectionOrderGroup.Parent = Convert.ToString(reader["Parent"]);
                                }
                                detectionOrderGroups.Add(detectionOrderGroup);

                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionOrderGroupsWithDetections(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detectionOrderGroups;
        }

        /// <summary>
        /// This method is used to get Detection Groups by detection type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The Detection Groups by detection type.</returns>
        public List<DetectionGroup> GetDetectionGroupsByDetectionType(string PCMConnectionString, UInt32 IdDetectionType)
        {
            List<DetectionGroup> DetectionGroups = new List<DetectionGroup>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsGroupsByDetectionType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionGroup DetectionGroup = new DetectionGroup();

                            DetectionGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            DetectionGroup.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionGroup.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                DetectionGroup.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["Name_es"] != DBNull.Value)
                            {
                                DetectionGroup.Name_es = Convert.ToString(reader["Name_es"]);
                            }
                            if (reader["Name_fr"] != DBNull.Value)
                            {
                                DetectionGroup.Name_fr = Convert.ToString(reader["Name_fr"]);
                            }
                            if (reader["Name_pt"] != DBNull.Value)
                            {
                                DetectionGroup.Name_pt = Convert.ToString(reader["Name_pt"]);
                            }
                            if (reader["Name_ro"] != DBNull.Value)
                            {
                                DetectionGroup.Name_ro = Convert.ToString(reader["Name_ro"]);
                            }
                            if (reader["Name_zh"] != DBNull.Value)
                            {
                                DetectionGroup.Name_zh = Convert.ToString(reader["Name_zh"]);
                            }
                            if (reader["Name_ru"] != DBNull.Value)
                            {
                                DetectionGroup.Name_ru = Convert.ToString(reader["Name_ru"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionGroup.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Description_es"] != DBNull.Value)
                            {
                                DetectionGroup.Description_es = Convert.ToString(reader["Description_es"]);
                            }
                            if (reader["Description_fr"] != DBNull.Value)
                            {
                                DetectionGroup.Description_fr = Convert.ToString(reader["Description_fr"]);
                            }
                            if (reader["Description_pt"] != DBNull.Value)
                            {
                                DetectionGroup.Description_pt = Convert.ToString(reader["Description_pt"]);
                            }
                            if (reader["Description_ro"] != DBNull.Value)
                            {
                                DetectionGroup.Description_ro = Convert.ToString(reader["Description_ro"]);
                            }
                            if (reader["Description_zh"] != DBNull.Value)
                            {
                                DetectionGroup.Description_zh = Convert.ToString(reader["Description_zh"]);
                            }
                            if (reader["Description_ru"] != DBNull.Value)
                            {
                                DetectionGroup.Description_ru = Convert.ToString(reader["Description_ru"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionGroup.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            DetectionGroups.Add(DetectionGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionGroupsByDetectionType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionGroups;
        }

        /// <summary>
        /// This method is used to get all Detection Groups.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The list of Detection Groups.</returns>
        public List<DetectionGroup> GetDetectionGroupsList(string PCMConnectionString, UInt32 IdDetectionType)
        {
            List<DetectionGroup> DetectionGroups = new List<DetectionGroup>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionGroupsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionGroup DetectionGroup = new DetectionGroup();

                            DetectionGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            DetectionGroup.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionGroup.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                DetectionGroup.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["OriginalName"] != DBNull.Value)
                            {
                                DetectionGroup.OriginalName = Convert.ToString(reader["OriginalName"]);
                            }
                            DetectionGroups.Add(DetectionGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionGroupsList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionGroups;
        }

        /// <summary>
        /// This method is used to get Detection Groups by id group.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdGroup">Get id group</param>
        /// <returns>The Detection Groups by id group.</returns>
        public DetectionGroup GetDetectionGroupsByIdGroup(string PCMConnectionString, UInt32 IdGroup)
        {
            DetectionGroup DetectionGroups = new DetectionGroup();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionGroupsByIdGroup", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionGroups.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            DetectionGroups.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                DetectionGroups.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                DetectionGroups.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["Name_es"] != DBNull.Value)
                            {
                                DetectionGroups.Name_es = Convert.ToString(reader["Name_es"]);
                            }
                            if (reader["Name_fr"] != DBNull.Value)
                            {
                                DetectionGroups.Name_fr = Convert.ToString(reader["Name_fr"]);
                            }
                            if (reader["Name_pt"] != DBNull.Value)
                            {
                                DetectionGroups.Name_pt = Convert.ToString(reader["Name_pt"]);
                            }
                            if (reader["Name_ro"] != DBNull.Value)
                            {
                                DetectionGroups.Name_ro = Convert.ToString(reader["Name_ro"]);
                            }
                            if (reader["Name_zh"] != DBNull.Value)
                            {
                                DetectionGroups.Name_zh = Convert.ToString(reader["Name_zh"]);
                            }
                            if (reader["Name_ru"] != DBNull.Value)
                            {
                                DetectionGroups.Name_ru = Convert.ToString(reader["Name_ru"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                DetectionGroups.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Description_es"] != DBNull.Value)
                            {
                                DetectionGroups.Description_es = Convert.ToString(reader["Description_es"]);
                            }
                            if (reader["Description_fr"] != DBNull.Value)
                            {
                                DetectionGroups.Description_fr = Convert.ToString(reader["Description_fr"]);
                            }
                            if (reader["Description_pt"] != DBNull.Value)
                            {
                                DetectionGroups.Description_pt = Convert.ToString(reader["Description_pt"]);
                            }
                            if (reader["Description_ro"] != DBNull.Value)
                            {
                                DetectionGroups.Description_ro = Convert.ToString(reader["Description_ro"]);
                            }
                            if (reader["Description_zh"] != DBNull.Value)
                            {
                                DetectionGroups.Description_zh = Convert.ToString(reader["Description_zh"]);
                            }
                            if (reader["Description_ru"] != DBNull.Value)
                            {
                                DetectionGroups.Description_ru = Convert.ToString(reader["Description_ru"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                DetectionGroups.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionGroupsByIdGroup(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return DetectionGroups;
        }

        /// <summary>
        /// This method is used to get Detection concat by id group.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdGroup">Get id group</param>
        /// <returns>The Detection concat by id group.</returns>
        public string GetDetectionsConcatByIdGroup(string PCMConnectionString, UInt32 IdGroup)
        {
            string detections = "";
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetConcatDetectionsByDetectionGroup", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdGroup", IdGroup);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Names"] != DBNull.Value)
                            {
                                detections = Convert.ToString(reader["Names"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionGroupsByIdGroup(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detections;
        }

        /// <summary>
        /// This method is used to get Detection Groups.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The list of Detection Groups.</returns>
        public List<Detections> GetDetectionGroups(string PCMConnectionString, UInt32 IdDetectionType)
        {
            List<Detections> Detections = new List<Detections>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetGroupsWithDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections Detection = new Detections();

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                Detection.Key = Convert.ToString(reader["KeyName"]);
                            }

                            Detection.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Detection.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                Detection.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                Detection.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            Detections.Add(Detection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionGroups(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Detections;
        }

        /// <summary>
        /// This method is used to get Options Groups.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get Options type</param>
        /// <returns>The list of option Groups.</returns>
        public List<Options> GetOptionGroups(string PCMConnectionString, UInt32 IdDetectionType)
        {
            List<Options> Options = new List<Options>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetGroupsWithDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options Option = new Options();

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                Option.Key = Convert.ToString(reader["KeyName"]);
                            }

                            Option.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Option.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                Option.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                Option.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            Options.Add(Option);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOptionGroups(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Options;
        }

        /// <summary>
        /// This method is used to get Detection Groups by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <param name="IdCPType">Get product type</param>
        /// <returns>The list of Detection Groups by cptype.</returns>
        public List<Detections> GetDetectionGroupsByCPType(string PCMConnectionString, UInt32 IdDetectionType, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Detections> Detections = new List<Detections>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetGroupsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections Detection = new Detections();

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                Detection.Key = Convert.ToString(reader["KeyName"]);
                            }

                            Detection.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Detection.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                Detection.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                Detection.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            Detections.Add(Detection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionGroupsByCPType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Detections;
        }

        /// <summary>
        /// This method is used to get Options Groups by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get Options type</param>
        /// <param name="IdCPType">Get product type</param>
        /// <returns>The list of option Groups by cptype.</returns>
        public List<Options> GetOptionGroupsByCPType(string PCMConnectionString, UInt32 IdDetectionType, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Options> Options = new List<Options>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetGroupsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options Option = new Options();

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                Option.Key = Convert.ToString(reader["KeyName"]);
                            }

                            Option.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Option.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                Option.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }
                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                Option.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            Options.Add(Option);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOptionGroupsByCPType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Options;
        }

        /// <summary>
        /// This method is used to get Detection order Groups with detections by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The list of Detection Groups with detctions by cptype.</returns>
        public List<Detections> GetDetectionsWithGroupsByCpType(string PCMConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Detections> detectionGroups = new List<Detections>();
            try
            {
                detectionGroups = GetDetectionGroupsByCPType(PCMConnectionString, 2, IdCPType, IdTemplate);

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsWithGroupsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 2);
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections detectionGroup = new Detections();

                            detectionGroup.Key = Convert.ToString(reader["IdDetection"]);

                            detectionGroup.IdDetections = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["IdGroup"] != DBNull.Value)
                            {
                                detectionGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            }

                            if (reader["Name"] != DBNull.Value)
                            {
                                detectionGroup.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                detectionGroup.Parent = Convert.ToString(reader["Parent"]);
                            }
                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                detectionGroup.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }

                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                detectionGroup.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            detectionGroups.Add(detectionGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionsWithGroupsByCpType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detectionGroups;
        }

        /// <summary>
        /// This method is used to get Option order Groups with Options by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get product type id</param>
        /// <returns>The list of Option Groups with detctions by cptype.</returns>
        public List<Options> GetOptionsWithGroupsByCpType(string PCMConnectionString, UInt64 IdCPType, UInt64 IdTemplate)
        {
            List<Options> OptionGroups = new List<Options>();
            try
            {
                OptionGroups = GetOptionGroupsByCPType(PCMConnectionString, 3, IdCPType, IdTemplate);

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsWithGroupsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 3);
                    mySqlCommand.Parameters.AddWithValue("_IdTemplate", IdTemplate);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options OptionGroup = new Options();

                            OptionGroup.Key = Convert.ToString(reader["IdDetection"]);

                            OptionGroup.IdOptions = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["IdGroup"] != DBNull.Value)
                            {
                                OptionGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            }

                            if (reader["Name"] != DBNull.Value)
                            {
                                OptionGroup.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                OptionGroup.Parent = Convert.ToString(reader["Parent"]);
                            }

                            if (reader["OrderNumber"] != DBNull.Value)
                            {
                                OptionGroup.OrderNumber = Convert.ToInt32(reader["OrderNumber"]);
                            }

                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                OptionGroup.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            OptionGroups.Add(OptionGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetOptionsWithGroupsByCpType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return OptionGroups;
        }

        /// <summary>
        /// This method is used to get All Detection Groups with detections.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get detection type</param>
        /// <returns>The list of Detection Groups with detctions.</returns>
        public List<Detections> GetAllDetectionsWithGroups(string PCMConnectionString)
        {
            List<Detections> detectionGroups = new List<Detections>();
            try
            {
                detectionGroups = GetDetectionGroups(PCMConnectionString, 2);

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 2);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detections detectionGroup = new Detections();

                            detectionGroup.Key = Convert.ToString(reader["IdDetection"]);

                            detectionGroup.IdDetections = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["IdGroup"] != DBNull.Value)
                            {
                                detectionGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            }

                            if (reader["Name"] != DBNull.Value)
                            {
                                detectionGroup.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                detectionGroup.Parent = Convert.ToString(reader["Parent"]);
                            }

                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                detectionGroup.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }
                            detectionGroups.Add(detectionGroup);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllDetectionsWithGroups(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detectionGroups;
        }

        /// <summary>
        /// This method is used to get Option order Groups with Options by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetectionType">Get Option type</param>
        /// <returns>The list of Option Groups with detctions by cptype.</returns>
        public List<Options> GetAllOptionsWithGroups(string PCMConnectionString)
        {
            List<Options> OptionGroups = new List<Options>();
            try
            {
                OptionGroups = GetOptionGroups(PCMConnectionString, 3);

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetDetectionsByType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", 3);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Options OptionGroup = new Options();

                            OptionGroup.Key = Convert.ToString(reader["IdDetection"]);

                            OptionGroup.IdOptions = Convert.ToUInt32(reader["IdDetection"]);

                            if (reader["IdGroup"] != DBNull.Value)
                            {
                                OptionGroup.IdGroup = Convert.ToUInt32(reader["IdGroup"]);
                            }

                            if (reader["Name"] != DBNull.Value)
                            {
                                OptionGroup.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                OptionGroup.Parent = Convert.ToString(reader["Parent"]);
                            }

                            if (reader["IdDetectionType"] != DBNull.Value)
                            {
                                OptionGroup.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);
                            }

                            OptionGroups.Add(OptionGroup);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllOptionsWithGroups(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return OptionGroups;
        }

        /// <summary>
        /// This method is used to get all customers
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of customers.</returns>
        public List<RegionsByCustomer> GetAllCustomers(string PCMConnectionString)
        {
            List<RegionsByCustomer> regionsByCustomerList = new List<RegionsByCustomer>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllCustomers", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RegionsByCustomer RegionsByCustomer = new RegionsByCustomer();

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                RegionsByCustomer.Key = Convert.ToString(reader["KeyName"]);
                            }

                            RegionsByCustomer.IdGroup = Convert.ToUInt32(reader["IdGroup"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                RegionsByCustomer.GroupName = Convert.ToString(reader["Name"]);
                            }

                            regionsByCustomerList.Add(RegionsByCustomer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllCustomers(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return regionsByCustomerList;
        }

        /// <summary>
        /// This method is used to get customers with regions.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of customers with regions.</returns>
        public List<RegionsByCustomer> GetCustomersWithRegions(string PCMConnectionString)
        {
            List<RegionsByCustomer> regionsByCustomerList = new List<RegionsByCustomer>();
            try
            {
                //regionsByCustomerList = GetAllCustomers(PCMConnectionString);
                //int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCustomersWithRegions", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RegionsByCustomer regionsByCustomer = new RegionsByCustomer();

                            //regionsByCustomer.Key = Convert.ToString(i);

                            regionsByCustomer.IdRegion = Convert.ToUInt32(reader["IdRegion"]);
                            regionsByCustomer.IdGroup = Convert.ToUInt32(reader["IdCustomer"]);

                            if (reader["CustomerName"] != DBNull.Value)
                            {
                                regionsByCustomer.GroupName = Convert.ToString(reader["CustomerName"]);
                            }

                            if (reader["RegionName"] != DBNull.Value)
                            {
                                regionsByCustomer.RegionName = Convert.ToString(reader["RegionName"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                regionsByCustomer.Parent = Convert.ToString(reader["Parent"]);
                            }
                            regionsByCustomerList.Add(regionsByCustomer);

                            //i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCustomersWithRegions(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return regionsByCustomerList;
        }

        /// <summary>
        /// This method is used to get customers with regions by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">CPType id.</param>
        /// <returns>The list of customers with regions by cptype.</returns>
        public List<RegionsByCustomer> GetCustomersWithRegionsByCPType(string PCMConnectionString, UInt64 IdCPType)
        {
            List<RegionsByCustomer> regionsByCustomerList = new List<RegionsByCustomer>();
            try
            {
                //regionsByCustomerList = GetAllCustomers(PCMConnectionString);
                //int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCustomersWithRegionsByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RegionsByCustomer regionsByCustomer = new RegionsByCustomer();

                            //regionsByCustomer.Key = Convert.ToString(i);

                            regionsByCustomer.IdRegion = Convert.ToUInt32(reader["IdRegion"]);
                            regionsByCustomer.IdGroup = Convert.ToUInt32(reader["IdCustomer"]);
                            if (reader["CustomerName"] != DBNull.Value)
                            {
                                regionsByCustomer.GroupName = Convert.ToString(reader["CustomerName"]);
                            }
                            if (reader["RegionName"] != DBNull.Value)
                            {
                                regionsByCustomer.RegionName = Convert.ToString(reader["RegionName"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                regionsByCustomer.Parent = Convert.ToString(reader["Parent"]);
                            }
                            if (reader["IdCPType"] != DBNull.Value)
                            {
                                regionsByCustomer.IsChecked = true;
                            }
                            if (reader["UniqueId"] != DBNull.Value)
                            {
                                regionsByCustomer.UniqueId = Convert.ToString(reader["UniqueId"]);
                            }
                            regionsByCustomerList.Add(regionsByCustomer);

                            //i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCustomersWithRegionsByCPType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return regionsByCustomerList;
        }

        /// <summary>
        /// This method is used to get customers with regions by detection.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetection">Detection id.</param>
        /// <param name="IdDetectionType">Detection type id.</param>
        /// <returns>The list of customers with regions by detection.</returns>
        public List<RegionsByCustomer> GetCustomersWithRegionsByDetection(string PCMConnectionString, UInt32 IdDetection, UInt32 IdDetectionType)
        {
            List<RegionsByCustomer> regionsByCustomerList = new List<RegionsByCustomer>();
            try
            {
                //regionsByCustomerList = GetAllCustomers(PCMConnectionString);
                //int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCustomersWithRegionsByDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RegionsByCustomer regionsByCustomer = new RegionsByCustomer();

                            //regionsByCustomer.Key = Convert.ToString(i);

                            regionsByCustomer.IdRegion = Convert.ToUInt32(reader["IdRegion"]);
                            regionsByCustomer.IdGroup = Convert.ToUInt32(reader["IdCustomer"]);

                            if (reader["CustomerName"] != DBNull.Value)
                            {
                                regionsByCustomer.GroupName = Convert.ToString(reader["CustomerName"]);
                            }

                            if (reader["RegionName"] != DBNull.Value)
                            {
                                regionsByCustomer.RegionName = Convert.ToString(reader["RegionName"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                regionsByCustomer.Parent = Convert.ToString(reader["Parent"]);
                            }
                            if (reader["IdDetection"] != DBNull.Value)
                            {
                                regionsByCustomer.IsChecked = true;
                            }
                            regionsByCustomerList.Add(regionsByCustomer);

                            //i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCustomersWithRegionsByDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return regionsByCustomerList;
        }

        /// <summary>
        /// This method is used to get article categories.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Article categories.</returns>
        public List<ArticleCategories> GetArticleCategories(string connectionstring)
        {

            List<ArticleCategories> articleCategories = new List<ArticleCategories>();
            try
            {
                int i = 0;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticleCategories", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArticleCategories ArticleCategories = new ArticleCategories();
                            ArticleCategories.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                                ArticleCategories.Name = Convert.ToString(reader["Name"]);

                            if (reader["Parent"] != DBNull.Value)
                                ArticleCategories.Parent = Convert.ToUInt64(reader["Parent"]);

                            if (reader["IsLeaf"] != DBNull.Value)
                                ArticleCategories.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);

                            if (reader["Position"] != DBNull.Value)
                                ArticleCategories.Position = Convert.ToUInt32(reader["Position"]);

                            if (reader["TaricCode"] != DBNull.Value)
                                ArticleCategories.TaricCode = Convert.ToString(reader["TaricCode"]);

                            if (reader["NCM_Code"] != DBNull.Value)
                                ArticleCategories.NCM_Code = Convert.ToString(reader["NCM_Code"]);

                            if (reader["HS_Code"] != DBNull.Value)
                                ArticleCategories.HS_Code = Convert.ToString(reader["HS_Code"]);

                            if (reader["KeyName"] != DBNull.Value)
                                ArticleCategories.KeyName = Convert.ToString(reader["KeyName"]);
                            else
                            {
                                i++;
                                ArticleCategories.KeyName = Convert.ToString(i);
                            }

                            if (reader["Parent_Category"] != DBNull.Value)
                                ArticleCategories.ParentName = Convert.ToString(reader["Parent_Category"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                            {
                                ArticleCategories.Article_count = Convert.ToInt32(reader["Articles_Count"]);
                            }
                            if (reader["Name"] != DBNull.Value && reader["Articles_Count"] != DBNull.Value)
                            {
                                ArticleCategories.NameWithArticleCount = Convert.ToString(ArticleCategories.Name + " [" + ArticleCategories.Article_count + "]");
                                if (ArticleCategories.Parent != null)
                                {
                                    articleCategories.Where(a => a.Parent == null && a.IdArticleCategory == ArticleCategories.Parent).ToList().ForEach(a => { a.NameWithArticleCount = Convert.ToString(a.Name + " [" + Convert.ToInt32(ArticleCategories.Article_count + a.Article_count) + "]"); a.Article_count = Convert.ToInt32(ArticleCategories.Article_count + a.Article_count); });
                                }
                            }
                            else
                            {
                                ArticleCategories.NameWithArticleCount = Convert.ToString(ArticleCategories.Name + " [" + ArticleCategories.Article_count + "]");
                                if (ArticleCategories.Parent != null)
                                {
                                    //if(ArticleCategories.Article_count==0)
                                    //{
                                    //    ArticleCategories.Article_count = 1;
                                    //}
                                    ArticleCategories.NameWithArticleCount = Convert.ToString(ArticleCategories.Name + " [" + ArticleCategories.Article_count + "]");

                                    articleCategories.Where(a => a.Parent == null && a.IdArticleCategory == ArticleCategories.Parent).ToList().ForEach(a => { a.NameWithArticleCount = Convert.ToString(a.Name + " [" + Convert.ToInt32(ArticleCategories.Article_count + a.Article_count) + "]"); a.Article_count = Convert.ToInt32(ArticleCategories.Article_count + a.Article_count); });
                                }
                            }

                            articleCategories.Add(ArticleCategories);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetArticleCategories(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articleCategories;
        }

      

        /// <summary>
        /// This method is used to get all article.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Article.</returns>
        public List<Articles> GetAllArticles(string connectionString, string ArticleVisualAidsPath)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllArticles", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdArticleCategory"] != DBNull.Value)
                            {
                                article.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                article.ArticleCategory = new ArticleCategories();
                                article.ArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    article.ArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                    article.PcmArticleCategory = new PCMArticleCategory();
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllArticles(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return articles;
        }

        /// <summary>
        /// This method is used to get all article.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Article.</returns>
        public List<Articles> GetAllActiveArticles(string connectionString, string ArticleVisualAidsPath)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    MySqlCommand concommand = new MySqlCommand("PCM_GetAllActiveArticles", con);
                    concommand.CommandType = CommandType.StoredProcedure;
                    concommand.CommandTimeout = 8000;
                    using (MySqlDataReader reader = concommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);
                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.IsPCMArticle = true;
                            }

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdArticleCategory"] != DBNull.Value)
                            {
                                article.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                article.ArticleCategory = new ArticleCategories();
                                article.ArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    article.ArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                    article.PcmArticleCategory = new PCMArticleCategory();
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }

                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllActiveArticles(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return articles;
        }

        /// <summary>
        /// This method is used to get articles by category.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdArticleCategory">Article category id.</param>
        /// <returns>The list of Articles by category.</returns>
        public List<Articles> GetArticlesByCategory(string connectionString, string ArticleVisualAidsPath, uint IdArticleCategory)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticlesByIdArticleCategory", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", IdArticleCategory);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            //if (reader["Length"] != DBNull.Value)
                            //    article.Length = Convert.ToDouble(reader["Length"]);

                            //if (reader["Width"] != DBNull.Value)
                            //    article.Width = Convert.ToDouble(reader["Width"]);

                            //if (reader["Height"] != DBNull.Value)
                            //    article.Height = Convert.ToDouble(reader["Height"]);

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdArticleCategory"] != DBNull.Value)
                            {
                                article.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                article.ArticleCategory = new ArticleCategories();
                                article.ArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    article.ArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                    article.PcmArticleCategory = new PCMArticleCategory();
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetArticlesByCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return articles;
        }

        /// <summary>
        /// This method is used to get articles by category.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdArticleCategory">Article category id.</param>
        /// <returns>The list of Articles by category.</returns>
        public List<Articles> GetActiveArticlesByCategory(string connectionString, string ArticleVisualAidsPath, uint IdArticleCategory)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetActiveArticlesByIdArticleCategory", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", IdArticleCategory);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            //if (reader["Length"] != DBNull.Value)
                            //    article.Length = Convert.ToDouble(reader["Length"]);

                            //if (reader["Width"] != DBNull.Value)
                            //    article.Width = Convert.ToDouble(reader["Width"]);

                            //if (reader["Height"] != DBNull.Value)
                            //    article.Height = Convert.ToDouble(reader["Height"]);

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdArticleCategory"] != DBNull.Value)
                            {
                                article.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                article.ArticleCategory = new ArticleCategories();
                                article.ArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    article.ArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                    article.PcmArticleCategory = new PCMArticleCategory();
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetActiveArticlesByCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return articles;
        }

      
        /// <summary>
        /// This method is used to get pcm article categories.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Article categories.</returns>
        public List<PCMArticleCategory> GetActivePCMArticleCategories(string connectionstring)
        {
            List<PCMArticleCategory> pcmArticleCategory = new List<PCMArticleCategory>();
            try
            {
                int i = 0;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetActivePCMArticleCategories", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory PCMArticleCategory = new PCMArticleCategory();
                            PCMArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                                PCMArticleCategory.Name = Convert.ToString(reader["Name"]);

                            if (reader["Parent"] != DBNull.Value)
                                PCMArticleCategory.Parent = Convert.ToUInt64(reader["Parent"]);

                            if (reader["IsLeaf"] != DBNull.Value)
                                PCMArticleCategory.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);

                            if (reader["Position"] != DBNull.Value)
                                PCMArticleCategory.Position = Convert.ToUInt32(reader["Position"]);

                            if (reader["KeyName"] != DBNull.Value)
                                PCMArticleCategory.KeyName = Convert.ToString(reader["KeyName"]);
                            else
                            {
                                i++;
                                PCMArticleCategory.KeyName = Convert.ToString(i);
                            }

                            if (reader["Parent_Category"] != DBNull.Value)
                                PCMArticleCategory.ParentName = Convert.ToString(reader["Parent_Category"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                                PCMArticleCategory.Article_count = Convert.ToInt32(reader["Articles_Count"]);

                            if (reader["Name"] != DBNull.Value && reader["Articles_Count"] != DBNull.Value)
                            {
                                PCMArticleCategory.NameWithArticleCount = Convert.ToString(PCMArticleCategory.Name + " [" + PCMArticleCategory.Article_count + "]");
                                if (PCMArticleCategory.Parent != null)
                                {
                                    pcmArticleCategory.Where(a => a.Parent == null && a.IdPCMArticleCategory == PCMArticleCategory.Parent).ToList().ForEach(a => { a.NameWithArticleCount = Convert.ToString(a.Name + " [" + Convert.ToInt32(PCMArticleCategory.Article_count + a.Article_count) + "]"); a.Article_count = Convert.ToInt32(PCMArticleCategory.Article_count + a.Article_count); });
                                }
                            }
                            else
                            {
                                PCMArticleCategory.NameWithArticleCount = Convert.ToString(PCMArticleCategory.Name + " [" + PCMArticleCategory.Article_count + "]");
                                if (PCMArticleCategory.Parent != null)
                                {
                                    //if(PCMArticleCategory.Article_count==0)
                                    //{
                                    //    PCMArticleCategory.Article_count = 1;
                                    //}
                                    PCMArticleCategory.NameWithArticleCount = Convert.ToString(PCMArticleCategory.Name + " [" + PCMArticleCategory.Article_count + "]");

                                    pcmArticleCategory.Where(a => a.Parent == null && a.IdPCMArticleCategory == PCMArticleCategory.Parent).ToList().ForEach(a => { a.NameWithArticleCount = Convert.ToString(a.Name + " [" + Convert.ToInt32(PCMArticleCategory.Article_count + a.Article_count) + "]"); a.Article_count = Convert.ToInt32(PCMArticleCategory.Article_count + a.Article_count); });
                                }
                            }

                            pcmArticleCategory.Add(PCMArticleCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetActivePCMArticleCategories(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticleCategory;
        }

        /// <summary>
        /// This method is used to get all PCM article.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Article.</returns>
        public List<Articles> GetAllPCMArticles(string connectionString, string ArticleVisualAidsPath)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllPCMArticles", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["PCMArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                    article.ArticleCategory = new ArticleCategories();
                                    article.ArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllPCMArticles(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articles;
        }

        /// <summary>
        /// This method is used to get all PCM article.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Article.</returns>
        public List<Articles> GetAllActivePCMArticles(string connectionString, string ArticleVisualAidsPath)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllActivePCMArticles", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["PCMArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                    article.ArticleCategory = new ArticleCategories();
                                    article.ArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }

                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllActivePCMArticles(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articles;
        }

        /// <summary>
        /// This method is used to get PCM article by category.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Article by category.</returns>
        public List<Articles> GetPCMArticlesByCategory(string connectionString, string ArticleVisualAidsPath, uint IdPCMArticleCategory)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetPCMArticlesByIdPCMArticleCategory", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["PCMArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                    article.ArticleCategory = new ArticleCategories();
                                    article.ArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticlesByCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articles;
        }

        /// <summary>
        /// This method is used to get PCM article by category.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Article by category.</returns>
        public List<Articles> GetActivePCMArticlesByCategory(string connectionString, string ArticleVisualAidsPath, uint IdPCMArticleCategory)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetActivePCMArticlesByIdPCMArticleCategory", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);


                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["PCMArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                    article.ArticleCategory = new ArticleCategories();
                                    article.ArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetActivePCMArticlesByCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articles;
        }

        /// <summary>
        /// This method is used to get log entries by detection.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdDetection">Get detection id</param>
        /// <param name="IdDetectionType">Get detection type id</param>
        /// <returns>The list of log by detection.</returns>
        public List<DetectionLogEntry> GetDetectionLogEntriesByDetection(string PCMConnectionString, UInt32 IdDetection, UInt32 IdDetectionType)
        {
            List<DetectionLogEntry> detectionLogEntryList = new List<DetectionLogEntry>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetLogEntriesByIdDetection", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdDetection", IdDetection);
                    mySqlCommand.Parameters.AddWithValue("_IdDetectionType", IdDetectionType);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetectionLogEntry detectionLogEntry = new DetectionLogEntry();

                            detectionLogEntry.IdLogEntryByDetection = Convert.ToUInt32(reader["IdLogEntryByDetection"]);

                            detectionLogEntry.IdDetection = Convert.ToUInt32(reader["IdDetection"]);
                            detectionLogEntry.IdDetectionType = Convert.ToUInt32(reader["IdDetectionType"]);

                            if (reader["IdUser"] != DBNull.Value)
                            {
                                detectionLogEntry.IdUser = Convert.ToUInt32(reader["IdUser"]);
                            }
                            if (reader["UserName"] != DBNull.Value)
                            {
                                detectionLogEntry.UserName = Convert.ToString(reader["UserName"]);
                            }
                            if (reader["Datetime"] != DBNull.Value)
                            {
                                detectionLogEntry.Datetime = Convert.ToDateTime(reader["Datetime"]);
                            }
                            if (reader["Comments"] != DBNull.Value)
                            {
                                detectionLogEntry.Comments = Convert.ToString(reader["Comments"]);
                            }
                            detectionLogEntryList.Add(detectionLogEntry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetDetectionLogEntriesByDetection(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detectionLogEntryList;
        }

        /// <summary>
        /// This method is used to get sites by customer and region.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdRegion">get region id.</param>
        /// <param name="IdCustomer">get customer id.</param>
        /// <returns>The list of site by customer and region.</returns>
        public List<Site> GetSitesByCustomerAndRegion(string PCMConnectionString, UInt32 IdRegion, UInt32 IdCustomer)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetSitesByCustomerAndRegion", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdRegion", IdRegion);
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomer);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Site site = new Site();

                            site.IdSite = Convert.ToUInt32(reader["IdSite"]);

                            sites.Add(site);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetSitesByCustomerAndRegion(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return sites;
        }

        /// <summary>
        /// This method is used to get sites by customer and region by cptype.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdRegion">get region id.</param>
        /// <param name="IdCustomer">get customer id.</param>
        /// <param name="IdCPType">get cptype id.</param>
        /// <returns>The list of site by customer and region by cptype.</returns>
        public List<Site> GetSitesByCustomerAndRegionByCPType(string PCMConnectionString, UInt32 IdRegion, UInt32 IdCustomer, UInt64 IdCPType)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetSitesByCustomerAndRegionByCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdRegion", IdRegion);
                    mySqlCommand.Parameters.AddWithValue("_IdCustomer", IdCustomer);
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Site site = new Site();

                            site.IdSite = Convert.ToUInt32(reader["IdSite"]);

                            sites.Add(site);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetSitesByCustomerAndRegion(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return sites;
        }

        /// <summary>
        /// This method is to get articles image in bytes
        /// </summary>
        /// <param name="ArticleVisualAidsPath">Get articles image path</param>
        /// <param name="article">Get articles details</param>
        /// <returns>Article image bytes</returns>
        public byte[] GetArticleImageInBytes(string ArticleVisualAidsPath, Articles article)
        {
            try
            {


                if (!Directory.Exists(ArticleVisualAidsPath))
                {
                    return null;
                }

                string fileUploadPath = ArticleVisualAidsPath + article.ImagePath;

                if (!File.Exists(fileUploadPath))
                {
                    return null;
                }

                if (article != null && !string.IsNullOrEmpty(article.ImagePath))
                {

                    byte[] bytes = null;

                    try
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            bytes = new byte[stream.Length];
                            int numBytesToRead = (int)stream.Length;
                            int numBytesRead = 0;

                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead.
                                int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                                // Break when the end of the file is reached.
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                        }

                        return bytes;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetArticleImageInBytes() article ImagePath-{0}. ErrorMessage- {1}", article.ImagePath, ex.Message), category: Category.Exception, priority: Priority.Low);
                        //throw;
                    }
                }
            }
            catch (Exception ex)
            {


            }

            return null;
        }

        /// <summary>
        /// This method is used to get pcm article category by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="idPCMArticleCategory">get pcm article category details.</param>
        /// <returns>The pcm article category by id.</returns>
        public PCMArticleCategory GetPCMArticleCategoryById(string PCMConnectionString, uint idPCMArticleCategory)
        {
            PCMArticleCategory pcmArticleCategory = new PCMArticleCategory();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetPCMArticleCategoryByIdPCMArticleCategory", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", idPCMArticleCategory);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Name_es"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_es = Convert.ToString(reader["Name_es"]);
                            }
                            if (reader["Name_fr"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_fr = Convert.ToString(reader["Name_fr"]);
                            }
                            if (reader["Name_pt"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_pt = Convert.ToString(reader["Name_pt"]);
                            }
                            if (reader["Name_ro"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_ro = Convert.ToString(reader["Name_ro"]);
                            }
                            if (reader["Name_zh"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_zh = Convert.ToString(reader["Name_zh"]);
                            }
                            if (reader["Name_ru"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_ru = Convert.ToString(reader["Name_ru"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Description_es"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_es = Convert.ToString(reader["Description_es"]);
                            }
                            if (reader["Description_fr"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_fr = Convert.ToString(reader["Description_fr"]);
                            }
                            if (reader["Description_pt"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_pt = Convert.ToString(reader["Description_pt"]);
                            }
                            if (reader["Description_ro"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_ro = Convert.ToString(reader["Description_ro"]);
                            }
                            if (reader["Description_zh"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_zh = Convert.ToString(reader["Description_zh"]);
                            }
                            if (reader["Description_ru"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_ru = Convert.ToString(reader["Description_ru"]);
                            }
                            if (reader["Parent"] != DBNull.Value)
                            {
                                pcmArticleCategory.Parent = Convert.ToUInt64(reader["Parent"]);
                            }
                            if (reader["IsLeaf"] != DBNull.Value)
                            {
                                pcmArticleCategory.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                pcmArticleCategory.Position = Convert.ToUInt32(reader["Position"]);
                            }
                            if (reader["IdArticleCategory"] != DBNull.Value)
                            {
                                pcmArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                pcmArticleCategory.ArticleCategory = new ArticleCategories();
                                pcmArticleCategory.ArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    pcmArticleCategory.ArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                                if (reader["ArticleCategoryParent"] != DBNull.Value)
                                {
                                    pcmArticleCategory.ArticleCategory.Parent = Convert.ToUInt64(reader["ArticleCategoryParent"]);
                                }
                                if (reader["ArticleCategoryIsLeaf"] != DBNull.Value)
                                {
                                    pcmArticleCategory.ArticleCategory.IsLeaf = Convert.ToInt64(reader["ArticleCategoryIsLeaf"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleCategoryById(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticleCategory;
        }

        /// <summary>
        /// This method is used to get article by id.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>Get Article by id.</returns>
        public Articles GetArticleByIdArticle(string PCMConnectionString, string ArticleVisualAidsPath, UInt32 IdArticle, string ImagePath)
        {
            Articles article = new Articles();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticleByIdArticle", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_idArticle", IdArticle);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);
                        }
                    }
                }

                article.ArticleCompatibilityList = GetCompatibilitiesByArticle(PCMConnectionString, IdArticle);
                article.PCMArticleLogEntiryList = GetLogEntriesByIdPCMArticle(PCMConnectionString, IdArticle);
                article.PCMArticleImageList = GetPCMArticleImagesByIdPCMArticle(PCMConnectionString, IdArticle, ImagePath, article.Reference);
                article.PCMArticleAttachmentList = GetArticleAttachmentByIdArticle(PCMConnectionString, IdArticle);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetArticleByIdArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return article;
        }

        /// <summary>
        /// This method is used to get all Templates.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Templates.</returns>
        public List<ProductTypesTemplate> GetTemplateList(string PCMConnectionString)
        {
            List<ProductTypesTemplate> Templates = new List<ProductTypesTemplate>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllTemplates", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ProductTypesTemplate Template = new ProductTypesTemplate();
                            Template.IdTemplate = Convert.ToByte(reader["IdTemplate"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                Template.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                Template.Key = Convert.ToString(reader["KeyName"]);
                            }

                            Templates.Add(Template);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetTemplateList(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return Templates;
        }

        /// <summary>
        /// This method is used to Get Product Types With Template.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of Product Types With Template.</returns>
        public List<ProductTypesTemplate> GetProductTypesWithTemplate(string PCMConnectionString)
        {
            List<ProductTypesTemplate> templateList = new List<ProductTypesTemplate>();
            try
            {
                templateList = GetTemplateList(PCMConnectionString);
                int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetProductTypesWithTemplate", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypesTemplate template = new ProductTypesTemplate();
                            template.Key = Convert.ToString(i);

                            template.IdCPType = Convert.ToUInt64(reader["IdCPType"]);

                            if (reader["IdTemplate"] != DBNull.Value)
                            {
                                template.IdTemplate = Convert.ToByte(reader["IdTemplate"]);
                            }

                            if (reader["ProductType"] != DBNull.Value)
                            {
                                template.Name = Convert.ToString(reader["ProductType"]);
                                template.ProductType = new ProductTypes();
                                template.ProductType.IdCPType = Convert.ToUInt64(reader["IdCPType"]);
                                template.ProductType.Name = Convert.ToString(reader["ProductType"]);
                                template.ProductType.Reference = Convert.ToString(reader["Reference"]);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                template.Parent = Convert.ToString(reader["Parent"]);
                            }
                            templateList.Add(template);

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetProductTypesWithTemplate(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return templateList;
        }


        /// <summary>
        /// This method is used to get Compatibilities By Product Type.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdCPType">Get detection id</param>
        /// <returns>The list of Compatibilities By Product Type.</returns>
        public List<ProductTypeCompatibility> GetCompatibilitiesByProductType(string PCMConnectionString, byte IdCPType)
        {
            List<ProductTypeCompatibility> productTypeCompatibilityList = new List<ProductTypeCompatibility>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetCPTypeCompatibilitiesByIdCPType", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdCPType", IdCPType);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductTypeCompatibility productTypeCompatibility = new ProductTypeCompatibility();

                            productTypeCompatibility.IdCPType = IdCPType;
                            productTypeCompatibility.IdCompatibility = Convert.ToUInt32(reader["IdCompatibility"]);

                            if (reader["IdCPtypeCompatibility"] != DBNull.Value)
                            {
                                productTypeCompatibility.IdCPtypeCompatibility = Convert.ToByte(reader["IdCPtypeCompatibility"]);
                                if (reader["CPTypeName"] != DBNull.Value)
                                {
                                    productTypeCompatibility.Name = Convert.ToString(reader["CPTypeName"]);
                                    productTypeCompatibility.ProductType = new ProductTypes();
                                    if (reader["CPTypeReference"] != DBNull.Value)
                                    {
                                        productTypeCompatibility.Code = Convert.ToString(reader["CPTypeReference"]);
                                        productTypeCompatibility.ProductType.Reference = Convert.ToString(reader["CPTypeReference"]);
                                    }
                                    productTypeCompatibility.ProductType.IdCPType = Convert.ToUInt64(reader["IdCPtypeCompatibility"]);
                                    productTypeCompatibility.ProductType.Name = Convert.ToString(reader["CPTypeName"]);
                                }
                            }
                            if (reader["IdArticleCompatibility"] != DBNull.Value)
                            {
                                productTypeCompatibility.IdArticleCompatibility = Convert.ToUInt32(reader["IdArticleCompatibility"]);
                                if (reader["ArticleDesc"] != DBNull.Value)
                                {
                                    productTypeCompatibility.Name = Convert.ToString(reader["ArticleDesc"]);
                                    productTypeCompatibility.Article = new Articles();
                                    if (reader["ArticleReference"] != DBNull.Value)
                                    {
                                        productTypeCompatibility.Code = Convert.ToString(reader["ArticleReference"]);
                                        productTypeCompatibility.Article.Reference = Convert.ToString(reader["ArticleReference"]);
                                    }
                                    productTypeCompatibility.Article.IdArticle = Convert.ToUInt32(reader["IdArticleCompatibility"]);
                                    productTypeCompatibility.Article.Description = Convert.ToString(reader["ArticleDesc"]);
                                }
                            }
                            if (reader["IdTypeCompatibility"] != DBNull.Value)
                            {
                                productTypeCompatibility.IdTypeCompatibility = Convert.ToUInt32(reader["IdTypeCompatibility"]);
                            }
                            if (reader["MinimumElements"] != DBNull.Value)
                            {
                                productTypeCompatibility.MinimumElements = Convert.ToInt32(reader["MinimumElements"]);
                            }
                            if (reader["MaximumElements"] != DBNull.Value)
                            {
                                productTypeCompatibility.MaximumElements = Convert.ToInt32(reader["MaximumElements"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {
                                productTypeCompatibility.Quantity = Convert.ToInt32(reader["Quantity"]);
                            }
                            if (reader["Remarks"] != DBNull.Value)
                            {
                                productTypeCompatibility.Remarks = Convert.ToString(reader["Remarks"]);
                            }
                            if (reader["IdRelationshipType"] != DBNull.Value)
                            {
                                productTypeCompatibility.IdRelationshipType = Convert.ToInt32(reader["IdRelationshipType"]);
                                if (reader["RelationshipType"] != DBNull.Value)
                                {
                                    productTypeCompatibility.RelationshipType = new LookupValue();
                                    productTypeCompatibility.RelationshipType.IdLookupValue = Convert.ToInt32(reader["IdRelationshipType"]);
                                    productTypeCompatibility.RelationshipType.Value = Convert.ToString(reader["RelationshipType"]);
                                }
                            }
                            productTypeCompatibilityList.Add(productTypeCompatibility);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCompatibilitiesByProductType(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return productTypeCompatibilityList;
        }

        /// <summary>
        /// This method is used to get Compatibilities By Article.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <param name="IdArticle">Get article id</param>
        /// <returns>The list of Compatibilities By Article.</returns>
        public List<ArticleCompatibility> GetCompatibilitiesByArticle(string PCMConnectionString, UInt32 IdArticle)
        {
            List<ArticleCompatibility> articleCompatibilityList = new List<ArticleCompatibility>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticleCompatibilitiesByIdArticle", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArticleCompatibility articleCompatibility = new ArticleCompatibility();

                            articleCompatibility.IdArticle = IdArticle;
                            articleCompatibility.IdCompatibility = Convert.ToUInt32(reader["IdCompatibility"]);

                            if (reader["IdArticleCompatibility"] != DBNull.Value)
                            {
                                articleCompatibility.IdArticleCompatibility = Convert.ToUInt32(reader["IdArticleCompatibility"]);
                                if (reader["ArticleDesc"] != DBNull.Value)
                                {
                                    articleCompatibility.Name = Convert.ToString(reader["ArticleDesc"]);
                                    articleCompatibility.Article = new Articles();
                                    if (reader["ArticleReference"] != DBNull.Value)
                                    {
                                        articleCompatibility.Code = Convert.ToString(reader["ArticleReference"]);
                                        articleCompatibility.Article.Reference = Convert.ToString(reader["ArticleReference"]);
                                    }
                                    articleCompatibility.Article.IdArticle = Convert.ToUInt32(reader["IdArticleCompatibility"]);
                                    articleCompatibility.Article.Description = Convert.ToString(reader["ArticleDesc"]);
                                }
                            }

                            if (reader["IdCPtypeCompatibility"] != DBNull.Value)
                            {
                                articleCompatibility.IdCPtypeCompatibility = Convert.ToByte(reader["IdCPtypeCompatibility"]);
                                if (reader["CPTypeName"] != DBNull.Value)
                                {
                                    articleCompatibility.Name = Convert.ToString(reader["CPTypeName"]);
                                    articleCompatibility.ProductType = new ProductTypes();
                                    if (reader["CPTypeReference"] != DBNull.Value)
                                    {
                                        articleCompatibility.Code = Convert.ToString(reader["CPTypeReference"]);
                                        articleCompatibility.ProductType.Reference = Convert.ToString(reader["CPTypeReference"]);
                                    }
                                    articleCompatibility.ProductType.IdCPType = Convert.ToUInt64(reader["IdCPtypeCompatibility"]);
                                    articleCompatibility.ProductType.Name = Convert.ToString(reader["CPTypeName"]);
                                }
                            }

                            if (reader["IdTypeCompatibility"] != DBNull.Value)
                            {
                                articleCompatibility.IdTypeCompatibility = Convert.ToUInt32(reader["IdTypeCompatibility"]);
                            }
                            if (reader["MinimumElements"] != DBNull.Value)
                            {
                                articleCompatibility.MinimumElements = Convert.ToInt32(reader["MinimumElements"]);
                            }
                            if (reader["MaximumElements"] != DBNull.Value)
                            {
                                articleCompatibility.MaximumElements = Convert.ToInt32(reader["MaximumElements"]);
                            }
                            if (reader["Quantity"] != DBNull.Value)
                            {
                                articleCompatibility.Quantity = Convert.ToInt32(reader["Quantity"]);
                            }
                            if (reader["Remarks"] != DBNull.Value)
                            {
                                articleCompatibility.Remarks = Convert.ToString(reader["Remarks"]);
                            }
                            if (reader["IdRelationshipType"] != DBNull.Value)
                            {
                                articleCompatibility.IdRelationshipType = Convert.ToInt32(reader["IdRelationshipType"]);
                                if (reader["RelationshipType"] != DBNull.Value)
                                {
                                    articleCompatibility.RelationshipType = new LookupValue();
                                    articleCompatibility.RelationshipType.IdLookupValue = Convert.ToInt32(reader["IdRelationshipType"]);
                                    articleCompatibility.RelationshipType.Value = Convert.ToString(reader["RelationshipType"]);
                                }
                            }
                            articleCompatibilityList.Add(articleCompatibility);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetCompatibilitiesByArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return articleCompatibilityList;
        }


        /// <summary>
        /// This method is used to get all pcm article categories.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of pcm article categories.</returns>
        public List<PCMArticlesWithCategory> GetPCMArticleCategoryListForCompatibility(string PCMConnectionString)
        {
            List<PCMArticlesWithCategory> pcmArticlesWithCategoryList = new List<PCMArticlesWithCategory>();
            try
            {
                int i = 1;
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Compatibility_GetAllPCMArticleCategories", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            PCMArticlesWithCategory pcmArticlesWithCategory = new PCMArticlesWithCategory();
                            pcmArticlesWithCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Name = Convert.ToString(reader["Name"]);
                            }

                            if (reader["KeyName"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.KeyName = Convert.ToString(reader["KeyName"]);
                            }
                            else
                            {
                                pcmArticlesWithCategory.KeyName = Convert.ToString(i);
                            }

                            if (reader["Parent"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Parent = Convert.ToUInt32(reader["Parent"]);
                            }

                            if (reader["ParentName"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.ParentName = Convert.ToString(reader["ParentName"]);
                            }

                            pcmArticlesWithCategoryList.Add(pcmArticlesWithCategory);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleCategoryListForCompatibility(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticlesWithCategoryList;
        }

        /// <summary>
        /// This method is used to Get PCM Articles With Category.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Articles With Category.</returns>
        public List<PCMArticleCategory> GetPCMArticlesWithCategory(string PCMConnectionString)
        {
            List<PCMArticleCategory> pcmArticlesWithCategoryList = new List<PCMArticleCategory>();
            try
            {
                pcmArticlesWithCategoryList = GetPCMArticleCategories(PCMConnectionString);
                int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Compatibility_GetAllPCMArticles", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory pcmArticlesWithCategory = new PCMArticleCategory();
                            pcmArticlesWithCategory.KeyName = Convert.ToString(i);
                            pcmArticlesWithCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            pcmArticlesWithCategory.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Reference = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Name = Convert.ToString(reader["Description"]);
                                pcmArticlesWithCategory.Description = Convert.ToString(reader["Description"]);
                            }

                            if (reader["ParentName"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.ParentName = Convert.ToString(reader["ParentName"]);
                            }
                            pcmArticlesWithCategoryList.Add(pcmArticlesWithCategory);

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticlesWithCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticlesWithCategoryList;
        }

        /// <summary>
        /// This method is used to Get All Detections Ways Options Spare Parts.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The Get List of Detections Ways Options Spare Parts.</returns>
        public List<DetectionDetails> GetAllDetectionsWaysOptionsSpareParts(string PCMConnectionString)
        {
            List<DetectionDetails> detectionList = new List<DetectionDetails>();
            try
            {
                using (MySqlConnection connDetectionDetails = new MySqlConnection(PCMConnectionString))
                {
                    connDetectionDetails.Open();

                    MySqlCommand DetectionDetailCommand = new MySqlCommand("PCM_GetAllDetections", connDetectionDetails);
                    DetectionDetailCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader DetectionDetailReader = DetectionDetailCommand.ExecuteReader())
                    {
                        while (DetectionDetailReader.Read())
                        {
                            DetectionDetails detectionDetails = new DetectionDetails();
                            detectionDetails.IdDetections = Convert.ToUInt32(DetectionDetailReader["IdDetection"]);

                            if (DetectionDetailReader["Name"] != DBNull.Value)
                            {
                                detectionDetails.Name = Convert.ToString(DetectionDetailReader["Name"]);
                            }
                            if (DetectionDetailReader["Description"] != DBNull.Value)
                            {
                                detectionDetails.Description = Convert.ToString(DetectionDetailReader["Description"]);
                            }
                            if (DetectionDetailReader["WeldOrder"] != DBNull.Value)
                            {
                                detectionDetails.WeldOrder = Convert.ToUInt32(DetectionDetailReader["WeldOrder"]);
                            }
                            if (DetectionDetailReader["Code"] != DBNull.Value)
                            {
                                detectionDetails.Code = Convert.ToString(DetectionDetailReader["Code"]);
                            }
                            if (DetectionDetailReader["IdTestType"] != DBNull.Value)
                            {
                                detectionDetails.IdTestType = Convert.ToUInt64(DetectionDetailReader["IdTestType"]);
                                detectionDetails.TestTypes = new TestTypes();
                                detectionDetails.TestTypes.IdTestType = Convert.ToUInt64(DetectionDetailReader["IdTestType"]);
                                if (DetectionDetailReader["TestName"] != DBNull.Value)
                                {
                                    detectionDetails.TestTypes.Name = Convert.ToString(DetectionDetailReader["TestName"]);
                                }
                            }
                            if (DetectionDetailReader["IdDetectionType"] != DBNull.Value)
                            {
                                detectionDetails.IdDetectionType = Convert.ToUInt32(DetectionDetailReader["IdDetectionType"]);
                                detectionDetails.DetectionTypes = new DetectionTypes();
                                detectionDetails.DetectionTypes.IdDetectionType = Convert.ToUInt32(DetectionDetailReader["IdDetectionType"]);
                                if (DetectionDetailReader["DetectionName"] != DBNull.Value)
                                {
                                    detectionDetails.DetectionTypes.Name = Convert.ToString(DetectionDetailReader["DetectionName"]);
                                }
                            }
                            if (DetectionDetailReader["IdGroup"] != DBNull.Value)
                            {
                                detectionDetails.IdGroup = Convert.ToUInt32(DetectionDetailReader["IdGroup"]);
                                detectionDetails.DetectionGroup = new DetectionGroup();
                                detectionDetails.DetectionGroup.IdGroup = Convert.ToUInt32(DetectionDetailReader["IdGroup"]);
                                if (DetectionDetailReader["GroupName"] != DBNull.Value)
                                {
                                    detectionDetails.DetectionGroup.Name = Convert.ToString(DetectionDetailReader["GroupName"]);
                                }
                            }
                            if (DetectionDetailReader["LastUpdate"] != DBNull.Value)
                            {
                                detectionDetails.LastUpdate = Convert.ToDateTime(DetectionDetailReader["LastUpdate"]);
                            }
                            if (DetectionDetailReader["IdStatus"] != DBNull.Value)
                            {
                                detectionDetails.IdStatus = Convert.ToUInt32(DetectionDetailReader["IdStatus"]);
                                detectionDetails.Status = new LookupValue();
                                detectionDetails.Status.IdLookupValue = Convert.ToInt32(DetectionDetailReader["IdStatus"]);
                                if (DetectionDetailReader["Status"] != DBNull.Value)
                                {
                                    detectionDetails.Status.Value = Convert.ToString(DetectionDetailReader["Status"]);
                                }
                            }
                            detectionList.Add(detectionDetails);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllDetectionsWaysOptionsSpareParts().ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return detectionList;
        }


        public List<PCMArticleLogEntry> GetLogEntriesByIdPCMArticle(string PCMConnectionString, UInt32 IdArticle)
        {
            List<PCMArticleLogEntry> pCMArticleLogEntries = new List<PCMArticleLogEntry>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetLogEntriesByIdPCMArticle", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleLogEntry pCMArticleLogEntry = new PCMArticleLogEntry();

                            pCMArticleLogEntry.IdLogEntryByPCMArticle = Convert.ToUInt32(reader["IdLogEntryByPCMArticle"]);

                            pCMArticleLogEntry.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["IdUser"] != DBNull.Value)
                            {
                                pCMArticleLogEntry.IdUser = Convert.ToUInt32(reader["IdUser"]);
                            }
                            if (reader["UserName"] != DBNull.Value)
                            {
                                pCMArticleLogEntry.UserName = Convert.ToString(reader["UserName"]);
                            }
                            if (reader["Datetime"] != DBNull.Value)
                            {
                                pCMArticleLogEntry.Datetime = Convert.ToDateTime(reader["Datetime"]);
                            }
                            if (reader["Comments"] != DBNull.Value)
                            {
                                pCMArticleLogEntry.Comments = Convert.ToString(reader["Comments"]);
                            }
                            pCMArticleLogEntries.Add(pCMArticleLogEntry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetLogEntriesByIdPCMArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pCMArticleLogEntries;
        }

        public List<PCMArticleImage> GetPCMArticleImagesByIdPCMArticle(string PCMConnectionString, UInt32 IdArticle, string ImagePath, string ArticleReference)
        {
            List<PCMArticleImage> pCMArticleImages = new List<PCMArticleImage>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticleImagesByIdPCMArticle", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleImage image = new PCMArticleImage();

                            image.IdPCMArticleImage = Convert.ToUInt32(reader["IdPCMArticleImage"]);
                            image.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["SavedFileName"] != DBNull.Value)
                            {
                                image.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                image.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["OriginalFileName"] != DBNull.Value)
                            {
                                image.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                image.Position = Convert.ToUInt64(reader["Position"]);
                            }
                            if (reader["UpdatedDate"] != DBNull.Value)
                            {
                                image.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                            }
                            image.PCMArticleImageInBytes = GetPCMArticleImage(Convert.ToString(image.IdPCMArticleImage), ImagePath, image.SavedFileName, ArticleReference);
                            pCMArticleImages.Add(image);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleImagesByIdPCMArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pCMArticleImages;
        }

        public byte[] GetPCMArticleImage(string IdPCMArticleImage, string ImagePath, string SavedFileName, string ArticleReference)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}\{2}", ImagePath, ArticleReference, IdPCMArticleImage + "_" + SavedFileName);

            try
            {
                if (File.Exists(fileUploadPath))
                {
                    using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[stream.Length];
                        int numBytesToRead = (int)stream.Length;
                        int numBytesRead = 0;

                        while (numBytesToRead > 0)
                        {
                            // Read may return anything from 0 to numBytesToRead.
                            int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                            // Break when the end of the file is reached.
                            if (n == 0)
                                break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }
                    }
                }

                return bytes;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleImage(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public List<ArticleDocument> GetArticleAttachmentByIdArticle(string connectionString, UInt32 idArticle)
        {
            List<ArticleDocument> articleDocumentLst = new List<ArticleDocument>();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("articledocs_GetArticleAttachmentDetailsByIdArticle", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdArticle", idArticle);


                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArticleDocument articleDocument = new ArticleDocument();
                        try
                        {
                            if (reader["IdArticleDoc"] != DBNull.Value)
                            {
                                articleDocument.IdArticleDoc = Convert.ToInt64(reader["IdArticleDoc"]);
                            }

                            if (reader["IdArticle"] != DBNull.Value)
                            {
                                articleDocument.IdArticle = Convert.ToInt32(reader["IdArticle"]);
                            }

                            articleDocument.OriginalFileName = Convert.ToString(reader["OriginalFileName"]);
                            articleDocument.SavedFileName = Convert.ToString(reader["SavedFileName"]);
                            articleDocument.Description = Convert.ToString(reader["Description"]);

                            if (reader["CreatedIn"] != DBNull.Value)
                            {
                                articleDocument.CreatedIn = Convert.ToDateTime(reader["CreatedIn"]);
                            }

                            if (reader["ModifiedIn"] != DBNull.Value)
                            {
                                articleDocument.ModifiedIn = Convert.ToDateTime(reader["ModifiedIn"]);
                            }

                            if (reader["ModifiedBy"] != DBNull.Value)
                            {
                                articleDocument.ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]);

                                articleDocument.DocumentModifiedBy = new People();
                                articleDocument.DocumentModifiedBy.IdPerson = Convert.ToInt32(reader["ModifiedBy"]);
                                articleDocument.DocumentModifiedBy.Name = Convert.ToString(reader["ModifiedByName"]);
                                articleDocument.DocumentModifiedBy.Surname = Convert.ToString(reader["ModifiedBySurname"]);

                                if (reader["ModifiedByIdSite"] != DBNull.Value)
                                {
                                    articleDocument.DocumentModifiedBy.Company = new Company();
                                    articleDocument.DocumentModifiedBy.Company.IdCompany = Convert.ToInt32(reader["ModifiedByIdSite"]);
                                    articleDocument.DocumentModifiedBy.Company.Name = Convert.ToString(reader["ModifiedBySiteName"]);
                                }
                            }

                            if (reader["CreatedBy"] != DBNull.Value)
                            {
                                articleDocument.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);

                                articleDocument.DocumentCreatedBy = new People();
                                articleDocument.DocumentCreatedBy.IdPerson = Convert.ToInt32(reader["CreatedBy"]);
                                articleDocument.DocumentCreatedBy.Name = Convert.ToString(reader["CreatedByName"]);
                                articleDocument.DocumentCreatedBy.Surname = Convert.ToString(reader["CreatedBySurname"]);

                                if (reader["CreatedByIdSite"] != DBNull.Value)
                                {
                                    articleDocument.DocumentCreatedBy.Company = new Company();
                                    articleDocument.DocumentCreatedBy.Company.IdCompany = Convert.ToInt32(reader["CreatedByIdSite"]);
                                    articleDocument.DocumentCreatedBy.Company.Name = Convert.ToString(reader["CreatedBySiteName"]);
                                }

                            }

                            if (reader["idDocType"] != DBNull.Value)
                            {
                                articleDocument.IdDocType = Convert.ToInt64(reader["idDocType"]);
                                articleDocument.ArticleDocumentType = new ArticleDocumentType();
                                articleDocument.ArticleDocumentType.IdDocType = Convert.ToInt64(reader["idDocType"]);
                                articleDocument.ArticleDocumentType.DocumentType = Convert.ToString(reader["DocumentType"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticleAttachmentByIdArticle(). IdArticleDoc- {0} IdArticle-{1}  ErrorMessage- {2}", articleDocument.IdArticleDoc, articleDocument.IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                        articleDocumentLst.Add(articleDocument);
                    }

                }
            }

            return articleDocumentLst;
        }

        public bool IsUpdatedPCMArticleCategoryOrder(string MainServerConnectionString, List<PCMArticleCategory> pcmArticleCategoryList, uint? IdModifier)
        {
            bool isupdate = false;
            try
            {
                if (pcmArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        foreach (PCMArticleCategory pcmArticleCategory in pcmArticleCategoryList)
                        {
                            if (pcmArticleCategory.IdPCMArticleCategory > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("PCM_ArticleCategoryPosition_Update", mySqlConnection);
                                mySqlCommand.CommandType = CommandType.StoredProcedure;

                                mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                                mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                                if (pcmArticleCategory.Parent == 0 || pcmArticleCategory.Parent == null)
                                {
                                    mySqlCommand.Parameters.AddWithValue("_Parent", null);
                                }
                                else
                                {
                                    mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                                }
                                mySqlCommand.Parameters.AddWithValue("_IdModifier", IdModifier);
                                mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                mySqlCommand.ExecuteNonQuery();
                            }
                        }
                        mySqlConnection.Close();
                        isupdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error UpdatePCMArticleCategoryPosition(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isupdate;
        }


        /// <summary>
        /// This method is used to Get PCM Articles With Category.
        /// </summary>
        /// <param name="PCMConnectionString">The pcm connection string.</param>
        /// <returns>The list of PCM Articles With Category.</returns>
        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference(string PCMConnectionString)
        {
            List<PCMArticleCategory> pcmArticlesWithCategoryList = new List<PCMArticleCategory>();
            try
            {
                pcmArticlesWithCategoryList = GetPCMArticleCategories(PCMConnectionString);
                int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Compatibility_GetAllPCMArticles", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory pcmArticlesWithCategory = new PCMArticleCategory();
                            pcmArticlesWithCategory.KeyName = Convert.ToString(i);
                            pcmArticlesWithCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            pcmArticlesWithCategory.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Reference = Convert.ToString(reader["Reference"]);
                                pcmArticlesWithCategory.Name = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                               
                                pcmArticlesWithCategory.Description = Convert.ToString(reader["Description"]);
                            }

                            if (reader["ParentName"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.ParentName = Convert.ToString(reader["ParentName"]);
                            }
                            pcmArticlesWithCategoryList.Add(pcmArticlesWithCategory);

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticlesWithCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticlesWithCategoryList;
        }

        public List<ArticleCategories> GetActiveArticleCategories(string connectionstring)
        {

            List<ArticleCategories> articleCategories = new List<ArticleCategories>();
            try
            {
                int i = 0;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetActiveArticleCategories", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ArticleCategories ArticleCategories = new ArticleCategories();
                            ArticleCategories.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                                ArticleCategories.Name = Convert.ToString(reader["Name"]);

                            if (reader["Parent"] != DBNull.Value)
                                ArticleCategories.Parent = Convert.ToUInt64(reader["Parent"]);

                            if (reader["IsLeaf"] != DBNull.Value)
                                ArticleCategories.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);

                            if (reader["Position"] != DBNull.Value)
                                ArticleCategories.Position = Convert.ToUInt32(reader["Position"]);

                            if (reader["TaricCode"] != DBNull.Value)
                                ArticleCategories.TaricCode = Convert.ToString(reader["TaricCode"]);

                            if (reader["NCM_Code"] != DBNull.Value)
                                ArticleCategories.NCM_Code = Convert.ToString(reader["NCM_Code"]);

                            if (reader["HS_Code"] != DBNull.Value)
                                ArticleCategories.HS_Code = Convert.ToString(reader["HS_Code"]);

                            if (reader["KeyName"] != DBNull.Value)
                                ArticleCategories.KeyName = Convert.ToString(reader["KeyName"]);
                            else
                            {
                                i++;
                                ArticleCategories.KeyName = Convert.ToString(i);
                            }

                            if (reader["Parent_Category"] != DBNull.Value)
                                ArticleCategories.ParentName = Convert.ToString(reader["Parent_Category"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                            {
                                ArticleCategories.Article_count = Convert.ToInt32(reader["Articles_Count"]);
                            }
                            if (reader["Articles_Count"] != DBNull.Value)
                            {
                                ArticleCategories.Article_count_original = Convert.ToInt32(reader["Articles_Count"]);
                            }
                            articleCategories.Add(ArticleCategories);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetActiveArticleCategories(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articleCategories;
        }







        public List<PCMArticleCategory> GetPCMArticleCategories(string connectionstring)
        {
            List<PCMArticleCategory> pcmArticleCategory = new List<PCMArticleCategory>();
            try
            {
                int i = 0;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllPCMArticleCategories", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory PCMArticleCategory = new PCMArticleCategory();
                            PCMArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                                PCMArticleCategory.Name = Convert.ToString(reader["Name"]);

                            if (reader["Parent"] != DBNull.Value)
                                PCMArticleCategory.Parent = Convert.ToUInt64(reader["Parent"]);

                            if (reader["IsLeaf"] != DBNull.Value)
                                PCMArticleCategory.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);

                            if (reader["Position"] != DBNull.Value)
                                PCMArticleCategory.Position = Convert.ToUInt32(reader["Position"]);

                            if (reader["KeyName"] != DBNull.Value)
                                PCMArticleCategory.KeyName = Convert.ToString(reader["KeyName"]);
                            else
                            {
                                i++;
                                PCMArticleCategory.KeyName = Convert.ToString(i);
                            }

                            if (reader["Parent_Category"] != DBNull.Value)
                                PCMArticleCategory.ParentName = Convert.ToString(reader["Parent_Category"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                                PCMArticleCategory.Article_count = Convert.ToInt32(reader["Articles_Count"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                                PCMArticleCategory.Article_count_original = Convert.ToInt32(reader["Articles_Count"]);

                            pcmArticleCategory.Add(PCMArticleCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleCategories(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return pcmArticleCategory;
        }

        public bool IsDeletePCMArticleCategory(string MainServerConnectionString, List<PCMArticleCategory> PCMArticleCategoryList)
        {
            bool isDeleted = false;
            try
            {
                if (PCMArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleCategory pCMArticleCategory in PCMArticleCategoryList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticleCategory_Delete", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pCMArticleCategory.IdPCMArticleCategory);
                            mySqlCommand.ExecuteNonQuery();

                            isDeleted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isDeleted;
        }

        #region pcm Article table change
        public bool IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(string MainServerConnectionString, uint IdPCMArticleCategory, Articles Article, string PCMArticleImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (Article != null && Article.IdArticle > 0 && IdPCMArticleCategory > 0)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_ArticleCategoryWithStatus_Update_V2060", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                            mySqlCommand.Parameters.AddWithValue("_idArticle", Article.IdArticle);
                            mySqlCommand.Parameters.AddWithValue("_IdPCMStatus", Article.IdPCMStatus);
                            mySqlCommand.Parameters.AddWithValue("_IdModifier", Article.IdModifier);
                            mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", Article.IsRtfText);
                            mySqlCommand.Parameters.AddWithValue("_PCMDescription", Article.PCMDescription);

                            mySqlCommand.ExecuteNonQuery();

                            mySqlConnection.Close();

                            isUpdated = true;
                        }

                        AddUpdateDeleteArticleCompatibilities(MainServerConnectionString, Article.IdArticle, Article.ArticleCompatibilityList);
                        AddPCMArticleLogEntry(MainServerConnectionString, Article.IdArticle, Article.PCMArticleLogEntiryList);
                        AddUpdateDeletePCMArticleImages(MainServerConnectionString, Article.IdArticle, Article.PCMArticleImageList, PCMArticleImagePath, Article.Reference);

                        isUpdated = true;
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategoryInArticleWithStatus_V2060(). IdArticle- {0} ErrorMessage- {1}", Article.IdArticle, ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        public List<PCMArticleCategory> GetPCMArticleCategories_V2060(string connectionstring)
        {
            List<PCMArticleCategory> pcmArticleCategory = new List<PCMArticleCategory>();
            try
            {
                int i = 0;
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionstring))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllPCMArticleCategories_V2060", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory PCMArticleCategory = new PCMArticleCategory();
                            PCMArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                                PCMArticleCategory.Name = Convert.ToString(reader["Name"]);

                            if (reader["Parent"] != DBNull.Value)
                                PCMArticleCategory.Parent = Convert.ToUInt64(reader["Parent"]);

                            if (reader["IsLeaf"] != DBNull.Value)
                                PCMArticleCategory.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);

                            if (reader["Position"] != DBNull.Value)
                                PCMArticleCategory.Position = Convert.ToUInt32(reader["Position"]);

                            if (reader["KeyName"] != DBNull.Value)
                                PCMArticleCategory.KeyName = Convert.ToString(reader["KeyName"]);
                            else
                            {
                                i++;
                                PCMArticleCategory.KeyName = Convert.ToString(i);
                            }

                            if (reader["Parent_Category"] != DBNull.Value)
                                PCMArticleCategory.ParentName = Convert.ToString(reader["Parent_Category"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                                PCMArticleCategory.Article_count = Convert.ToInt32(reader["Articles_Count"]);

                            if (reader["Articles_Count"] != DBNull.Value)
                                PCMArticleCategory.Article_count_original = Convert.ToInt32(reader["Articles_Count"]);

                            pcmArticleCategory.Add(PCMArticleCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleCategories_V2060(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return pcmArticleCategory;
        }


        public List<Articles> GetAllPCMArticles_V2060(string connectionString, string ArticleVisualAidsPath)
        {
            List<Articles> articles = new List<Articles>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetAllPCMArticles_V2060", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articles article = new Articles();

                            article.IdPCMArticle = Convert.ToUInt32(reader["IdPCMArticle"]);

                            if (reader["IdArticle"] != DBNull.Value)
                                article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            article.Visibility = "Hidden";

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                //article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                                //if (article.ArticleImageInBytes != null)
                                //{
                                //    article.Visibility = "Visible";
                                //}
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["PCMArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                    article.ArticleCategory = new ArticleCategories();
                                    article.ArticleCategory.Name = Convert.ToString(reader["PCMArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllPCMArticles(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return articles;
        }


        public Articles GetArticleByIdArticle_V2060(string PCMConnectionString, string ArticleVisualAidsPath, uint IdArticle, string ImagePath)
        {
            Articles article = new Articles();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetArticleByIdArticle_V2060", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticle", IdArticle);

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            article.IdPCMArticle = Convert.ToUInt32(reader["IdPCMArticle"]);

                            if (reader["IdArticle"] != DBNull.Value)
                                article.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                                article.Reference = Convert.ToString(reader["Reference"]);

                            if (reader["Description"] != DBNull.Value)
                                article.Description = Convert.ToString(reader["Description"]);

                            if (reader["Description_es"] != DBNull.Value)
                                article.Description_es = Convert.ToString(reader["Description_es"]);

                            if (reader["Description_fr"] != DBNull.Value)
                                article.Description_fr = Convert.ToString(reader["Description_fr"]);

                            if (reader["Description_ro"] != DBNull.Value)
                                article.Description_ro = Convert.ToString(reader["Description_ro"]);

                            if (reader["Description_zh"] != DBNull.Value)
                                article.Description_zh = Convert.ToString(reader["Description_zh"]);

                            if (reader["Description_pt"] != DBNull.Value)
                                article.Description_pt = Convert.ToString(reader["Description_pt"]);

                            if (reader["Description_ru"] != DBNull.Value)
                                article.Description_ru = Convert.ToString(reader["Description_ru"]);

                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                article.ImagePath = Convert.ToString(reader["ImagePath"]);
                                article.ArticleImageInBytes = GetArticleImageInBytes(ArticleVisualAidsPath, article);
                            }

                            if (reader["Length"] != DBNull.Value)
                                article.Length = (float)reader["Length"];

                            if (reader["Width"] != DBNull.Value)
                                article.Width = (float)reader["Width"];

                            if (reader["Height"] != DBNull.Value)
                                article.Height = (float)reader["Height"];

                            if (reader["Weight"] != DBNull.Value)
                                article.Weight = Math.Round(Convert.ToDecimal(reader["Weight"]), 3);

                            if (reader["IdPCMArticleCategory"] != DBNull.Value)
                            {
                                article.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                article.PcmArticleCategory = new PCMArticleCategory();
                                article.PcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    article.PcmArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                            }

                            if (reader["idArticleSupplier"] != DBNull.Value)
                                article.IdArticleSupplier = Convert.ToUInt32(reader["idArticleSupplier"]);

                            if (reader["SupplierName"] != DBNull.Value)
                                article.SupplierName = Convert.ToString(reader["SupplierName"]);

                            if (reader["IsObsolete"] != DBNull.Value)
                                article.IsObsolete = Convert.ToInt64(reader["IsObsolete"]);

                            if (article.IsObsolete == 0)
                            {
                                article.WarehouseStatus = WarehouseStatus.Active;
                            }
                            else
                            {
                                article.WarehouseStatus = WarehouseStatus.Inactive;
                            }


                            if (reader["IdPCMStatus"] != DBNull.Value)
                                article.IdPCMStatus = Convert.ToInt32(reader["IdPCMStatus"]);

                            if (reader["PCMStatus"] != DBNull.Value)
                                article.PCMStatus = Convert.ToString(reader["PCMStatus"]);

                            if (reader["IsRtfText"] != DBNull.Value)
                                article.IsRtfText = Convert.ToBoolean(reader["IsRtfText"]);

                            if (reader["PCMDescription"] != DBNull.Value)
                                article.PCMDescription = Convert.ToString(reader["PCMDescription"]);
                        }
                    }
                }

                article.ArticleCompatibilityList = GetCompatibilitiesByArticle(PCMConnectionString, IdArticle);
                article.PCMArticleLogEntiryList = GetLogEntriesByIdPCMArticle(PCMConnectionString, IdArticle);
                article.PCMArticleImageList = GetPCMArticleImagesByIdPCMArticle(PCMConnectionString, IdArticle, ImagePath, article.Reference);
                article.PCMArticleAttachmentList = GetArticleAttachmentByIdArticle(PCMConnectionString, IdArticle);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetArticleByIdArticle(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return article;
        }


        public List<PCMArticleCategory> GetPCMArticlesWithCategory_V2060(string PCMConnectionString)
        {
            List<PCMArticleCategory> pcmArticlesWithCategoryList = new List<PCMArticleCategory>();
            try
            {
                pcmArticlesWithCategoryList = GetPCMArticleCategories_V2060(PCMConnectionString);
                int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Compatibility_GetAllPCMArticles_V2060", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory pcmArticlesWithCategory = new PCMArticleCategory();
                            pcmArticlesWithCategory.KeyName = Convert.ToString(i);
                            pcmArticlesWithCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            pcmArticlesWithCategory.IdPCMArticle = Convert.ToUInt32(reader["IdPCMArticle"]);
                            pcmArticlesWithCategory.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Reference = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Name = Convert.ToString(reader["Description"]);
                                pcmArticlesWithCategory.Description = Convert.ToString(reader["Description"]);
                            }

                            if (reader["ParentName"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.ParentName = Convert.ToString(reader["ParentName"]);
                            }
                            pcmArticlesWithCategoryList.Add(pcmArticlesWithCategory);

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticlesWithCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticlesWithCategoryList;
        }


        public List<PCMArticleCategory> GetPCMArticlesWithCategoryForReference_V2060(string PCMConnectionString)
        {
            List<PCMArticleCategory> pcmArticlesWithCategoryList = new List<PCMArticleCategory>();
            try
            {
                pcmArticlesWithCategoryList = GetPCMArticleCategories_V2060(PCMConnectionString);
                int i = 1;

                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_Compatibility_GetAllPCMArticles_V2060", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PCMArticleCategory pcmArticlesWithCategory = new PCMArticleCategory();
                            pcmArticlesWithCategory.KeyName = Convert.ToString(i);
                            pcmArticlesWithCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            pcmArticlesWithCategory.IdPCMArticle = Convert.ToUInt32(reader["IdPCMArticle"]);
                            pcmArticlesWithCategory.IdArticle = Convert.ToUInt32(reader["IdArticle"]);

                            if (reader["Reference"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.Reference = Convert.ToString(reader["Reference"]);
                                pcmArticlesWithCategory.Name = Convert.ToString(reader["Reference"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {

                                pcmArticlesWithCategory.Description = Convert.ToString(reader["Description"]);
                            }

                            if (reader["ParentName"] != DBNull.Value)
                            {
                                pcmArticlesWithCategory.ParentName = Convert.ToString(reader["ParentName"]);
                            }
                            pcmArticlesWithCategoryList.Add(pcmArticlesWithCategory);

                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticlesWithCategoryForReference_V2060(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticlesWithCategoryList;
        }


        public bool AddDeletePCMArticle(string MainServerConnectionString, uint IdPCMArticleCategory, List<Articles> ArticleList, int IdUser)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (ArticleList != null)
                    {
                        using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                        {
                            mySqlConnection.Open();

                            foreach (Articles Article in ArticleList)
                            {
                                if (Article.IdArticle > 0)
                                {
                                    if (Article.TransactionOperation == ModelBase.TransactionOperations.Delete)
                                    {
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticles_Delete", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                                        mySqlCommand.Parameters.AddWithValue("_IdArticle", Article.IdArticle);

                                        mySqlCommand.ExecuteNonQuery();
                                    }
                                    else if (Article.TransactionOperation == ModelBase.TransactionOperations.Add)
                                    {
                                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticles_Insert", mySqlConnection);
                                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                                        mySqlCommand.Parameters.AddWithValue("_IdArticle", Article.IdArticle);
                                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", IdPCMArticleCategory);
                                        mySqlCommand.Parameters.AddWithValue("_IdCreator", IdUser);
                                        mySqlCommand.Parameters.AddWithValue("_CreationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                                        mySqlCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                            mySqlConnection.Close();
                        }
                    }
                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddDeletePCMArticle().  ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }

        public byte[] GetPCMArticleCategoryImageInBytes(PCMArticleCategory PCMArticleCategory, string PCMArticleCategoryFolderPath)
        {
            try
            {
                if (!Directory.Exists(PCMArticleCategoryFolderPath))
                {
                    return null;
                }

                string fileUploadPath = PCMArticleCategoryFolderPath + PCMArticleCategory.IdPCMArticleCategory + "_" + PCMArticleCategory.ImagePath;

                if (!File.Exists(fileUploadPath))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(PCMArticleCategory.ImagePath))
                {

                    byte[] bytes = null;

                    try
                    {
                        using (System.IO.FileStream stream = new System.IO.FileStream(fileUploadPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            bytes = new byte[stream.Length];
                            int numBytesToRead = (int)stream.Length;
                            int numBytesRead = 0;

                            while (numBytesToRead > 0)
                            {
                                // Read may return anything from 0 to numBytesToRead.
                                int n = stream.Read(bytes, numBytesRead, numBytesToRead);

                                // Break when the end of the file is reached.
                                if (n == 0)
                                    break;

                                numBytesRead += n;
                                numBytesToRead -= n;
                            }
                        }

                        return bytes;
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleCategoryImageInBytes() pcm article category ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                        //throw;
                    }
                }
            }
            catch (Exception ex)
            {


            }

            return null;
        }


        public bool IsDeletePCMArticleCategoryImageFromPath(PCMArticleCategory PCMArticleCategory, string ImageFolderPath)
        {
            try
            {
                if (ImageFolderPath != null)
                {
                    string completePath = string.Format(@"{0}", ImageFolderPath);

                    if (!Directory.Exists(completePath))
                    {
                        return false;
                    }
                    else
                    {
                        completePath = completePath + "\\" + PCMArticleCategory.IdPCMArticleCategory + "_" + PCMArticleCategory.ImagePath;

                        if (File.Exists(completePath))
                        {
                            File.Delete(completePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleCategoryImageFromPath()-  ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return true;
        }


        public bool IsDeletePCMArticleCategory(string MainServerConnectionString, List<PCMArticleCategory> PCMArticleCategoryList, string ImagePath)
        {
            bool isDeleted = false;
            try
            {
                if (PCMArticleCategoryList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (PCMArticleCategory pCMArticleCategory in PCMArticleCategoryList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("PCM_pcmArticleCategory_Delete", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pCMArticleCategory.IdPCMArticleCategory);
                            mySqlCommand.ExecuteNonQuery();

                            IsDeletePCMArticleCategoryImageFromPath(pCMArticleCategory, ImagePath);
                            isDeleted = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error IsDeletePCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return isDeleted;
        }


        public bool IsUpdatePCMArticleCategory(string MainServerConnectionString, PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList, string ImagePath)
        {
            bool isUpdated = false;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Update", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", pcmArticleCategory.IdPCMArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Name", pcmArticleCategory.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", pcmArticleCategory.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", pcmArticleCategory.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", pcmArticleCategory.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", pcmArticleCategory.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", pcmArticleCategory.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", pcmArticleCategory.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", pcmArticleCategory.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", pcmArticleCategory.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", pcmArticleCategory.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", pcmArticleCategory.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", pcmArticleCategory.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", pcmArticleCategory.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", pcmArticleCategory.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                        mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", pcmArticleCategory.IdArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                        mySqlCommand.Parameters.AddWithValue("_IsLeaf", pcmArticleCategory.IsLeaf);
                        mySqlCommand.Parameters.AddWithValue("_IdModifier", pcmArticleCategory.IdModifier);
                        mySqlCommand.Parameters.AddWithValue("_ModificationDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.Parameters.AddWithValue("_ImagePath", pcmArticleCategory.ImagePath);

                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                    }
                    //update position of pcm article categories
                    UpdatePCMArticleCategoryPosition(MainServerConnectionString, pcmArticleCategoryList, pcmArticleCategory.IdModifier);
                    AddPCMArticleCategoryImageToPath(pcmArticleCategory, ImagePath);

                    transactionScope.Complete();
                    transactionScope.Dispose();
                    isUpdated = true;
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error IsUpdatePCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return isUpdated;
        }


        public bool AddPCMArticleCategoryImageToPath(PCMArticleCategory PCMArticleCategory, string ImageFolderPath)
        {
            if (PCMArticleCategory.PCMArticleCategoryImageInByte != null)
            {
                try
                {
                    string completePath = string.Format(@"{0}", ImageFolderPath);
                    string filePath = completePath + "\\" + PCMArticleCategory.IdPCMArticleCategory + "_" + PCMArticleCategory.ImagePath;

                    if (!Directory.Exists(completePath))
                    {
                        Directory.CreateDirectory(completePath);
                    }
                    string[] filePaths = Directory.GetFiles(completePath, PCMArticleCategory.IdPCMArticleCategory + "_*", SearchOption.AllDirectories);
                    foreach (string file in filePaths)
                    {
                        File.Delete(file);
                    }
                    File.WriteAllBytes(filePath, PCMArticleCategory.PCMArticleCategoryImageInByte);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleCategoryImageToPath()- ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                    throw;
                }
            }

            return true;
        }

        public PCMArticleCategory AddPCMArticleCategory(string MainServerConnectionString, PCMArticleCategory pcmArticleCategory, List<PCMArticleCategory> pcmArticleCategoryList, string ImagePath)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        MySqlCommand mySqlCommand = new MySqlCommand("PCM_Article_Category_Insert", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;

                        mySqlCommand.Parameters.AddWithValue("_Name", pcmArticleCategory.Name);
                        mySqlCommand.Parameters.AddWithValue("_Name_es", pcmArticleCategory.Name_es);
                        mySqlCommand.Parameters.AddWithValue("_Name_fr", pcmArticleCategory.Name_fr);
                        mySqlCommand.Parameters.AddWithValue("_Name_ro", pcmArticleCategory.Name_ro);
                        mySqlCommand.Parameters.AddWithValue("_Name_zh", pcmArticleCategory.Name_zh);
                        mySqlCommand.Parameters.AddWithValue("_Name_pt", pcmArticleCategory.Name_pt);
                        mySqlCommand.Parameters.AddWithValue("_Name_ru", pcmArticleCategory.Name_ru);
                        mySqlCommand.Parameters.AddWithValue("_Description", pcmArticleCategory.Description);
                        mySqlCommand.Parameters.AddWithValue("_Description_es", pcmArticleCategory.Description_es);
                        mySqlCommand.Parameters.AddWithValue("_Description_fr", pcmArticleCategory.Description_fr);
                        mySqlCommand.Parameters.AddWithValue("_Description_ro", pcmArticleCategory.Description_ro);
                        mySqlCommand.Parameters.AddWithValue("_Description_zh", pcmArticleCategory.Description_zh);
                        mySqlCommand.Parameters.AddWithValue("_Description_pt", pcmArticleCategory.Description_pt);
                        mySqlCommand.Parameters.AddWithValue("_Description_ru", pcmArticleCategory.Description_ru);
                        mySqlCommand.Parameters.AddWithValue("_Position", pcmArticleCategory.Position);
                        mySqlCommand.Parameters.AddWithValue("_IdArticleCategory", pcmArticleCategory.IdArticleCategory);
                        mySqlCommand.Parameters.AddWithValue("_Parent", pcmArticleCategory.Parent);
                        mySqlCommand.Parameters.AddWithValue("_IsLeaf", pcmArticleCategory.IsLeaf);
                        mySqlCommand.Parameters.AddWithValue("_IdCreator", pcmArticleCategory.IdCreator);
                        mySqlCommand.Parameters.AddWithValue("_ImagePath", pcmArticleCategory.ImagePath);

                        pcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(mySqlCommand.ExecuteScalar());

                        mySqlConnection.Close();
                    }
                    //update position of pcm article categories
                    UpdatePCMArticleCategoryPosition(MainServerConnectionString, pcmArticleCategoryList, pcmArticleCategory.IdCreator);
                    AddPCMArticleCategoryImageToPath(pcmArticleCategory, ImagePath);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error AddPCMArticleCategory(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return pcmArticleCategory;
        }


        public PCMArticleCategory GetPCMArticleCategoryById(string PCMConnectionString, uint idPCMArticleCategory, string ImagePath)
        {
            PCMArticleCategory pcmArticleCategory = new PCMArticleCategory();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(PCMConnectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("PCM_GetPCMArticleCategoryByIdPCMArticleCategory", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdPCMArticleCategory", idPCMArticleCategory);
                    using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pcmArticleCategory.IdPCMArticleCategory = Convert.ToUInt32(reader["IdPCMArticleCategory"]);

                            if (reader["Name"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name = Convert.ToString(reader["Name"]);
                            }
                            if (reader["Name_es"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_es = Convert.ToString(reader["Name_es"]);
                            }
                            if (reader["Name_fr"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_fr = Convert.ToString(reader["Name_fr"]);
                            }
                            if (reader["Name_pt"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_pt = Convert.ToString(reader["Name_pt"]);
                            }
                            if (reader["Name_ro"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_ro = Convert.ToString(reader["Name_ro"]);
                            }
                            if (reader["Name_zh"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_zh = Convert.ToString(reader["Name_zh"]);
                            }
                            if (reader["Name_ru"] != DBNull.Value)
                            {
                                pcmArticleCategory.Name_ru = Convert.ToString(reader["Name_ru"]);
                            }
                            if (reader["Description"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description = Convert.ToString(reader["Description"]);
                            }
                            if (reader["Description_es"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_es = Convert.ToString(reader["Description_es"]);
                            }
                            if (reader["Description_fr"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_fr = Convert.ToString(reader["Description_fr"]);
                            }
                            if (reader["Description_pt"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_pt = Convert.ToString(reader["Description_pt"]);
                            }
                            if (reader["Description_ro"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_ro = Convert.ToString(reader["Description_ro"]);
                            }
                            if (reader["Description_zh"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_zh = Convert.ToString(reader["Description_zh"]);
                            }
                            if (reader["Description_ru"] != DBNull.Value)
                            {
                                pcmArticleCategory.Description_ru = Convert.ToString(reader["Description_ru"]);
                            }
                            if (reader["Parent"] != DBNull.Value)
                            {
                                pcmArticleCategory.Parent = Convert.ToUInt64(reader["Parent"]);
                            }
                            if (reader["IsLeaf"] != DBNull.Value)
                            {
                                pcmArticleCategory.IsLeaf = Convert.ToInt64(reader["IsLeaf"]);
                            }
                            if (reader["Position"] != DBNull.Value)
                            {
                                pcmArticleCategory.Position = Convert.ToUInt32(reader["Position"]);
                            }
                            if (reader["IdArticleCategory"] != DBNull.Value)
                            {
                                pcmArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                pcmArticleCategory.ArticleCategory = new ArticleCategories();
                                pcmArticleCategory.ArticleCategory.IdArticleCategory = Convert.ToUInt32(reader["IdArticleCategory"]);
                                if (reader["ArticleCategoryName"] != DBNull.Value)
                                {
                                    pcmArticleCategory.ArticleCategory.Name = Convert.ToString(reader["ArticleCategoryName"]);
                                }
                                if (reader["ArticleCategoryParent"] != DBNull.Value)
                                {
                                    pcmArticleCategory.ArticleCategory.Parent = Convert.ToUInt64(reader["ArticleCategoryParent"]);
                                }
                                if (reader["ArticleCategoryIsLeaf"] != DBNull.Value)
                                {
                                    pcmArticleCategory.ArticleCategory.IsLeaf = Convert.ToInt64(reader["ArticleCategoryIsLeaf"]);
                                }
                            }
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                pcmArticleCategory.ImagePath = Convert.ToString(reader["ImagePath"]);
                                GetPCMArticleCategoryImageInBytes(pcmArticleCategory, ImagePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetPCMArticleCategoryById(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }

            return pcmArticleCategory;
        }
        #endregion

        #endregion

        #region connection methods

        /// <summary>
        /// This method is used to get connection string name exists or not by name.
        /// </summary>
        public bool IsConnectionStringNameExist(string Name)
        {
            bool isExist = false;
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.Name == Name)
                    {
                        isExist = true;
                        return isExist;
                    }
                }
            }
            return isExist;
        }

        #endregion

        #endregion
    }
}
