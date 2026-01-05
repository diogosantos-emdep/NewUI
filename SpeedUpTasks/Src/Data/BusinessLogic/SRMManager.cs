using Emdep.Geos.Data.BusinessLogic.Logging;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.SRM;
using Emdep.Geos.Utility;
using MySql.Data.MySqlClient;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Emdep.Geos.Data.BusinessLogic
{
    public class SRMManager 
    {
       

        #region Insert methods
        /// <summary>
        /// This method is used to insert Comments Or Log Entries By PO
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="LogEntriesByWarehousePOList">Get Log entry or comment list details.</param>
        public void AddCommentsOrLogEntriesByPO(string MainServerConnectionString, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList)
        {
            try
            {
                if (LogEntriesByWarehousePOList != null)
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();

                        foreach (LogEntriesByWarehousePO logEntriesByWarehousePO in LogEntriesByWarehousePOList)
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("SRM_log_entries_by_warehouse_purchase_order_Insert", mySqlConnection);
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            mySqlCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrder", logEntriesByWarehousePO.IdWarehousePurchaseOrder);
                            mySqlCommand.Parameters.AddWithValue("_IdUser", logEntriesByWarehousePO.IdUser);
                            mySqlCommand.Parameters.AddWithValue("_Datetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            mySqlCommand.Parameters.AddWithValue("_Comments", logEntriesByWarehousePO.Comments);
                            mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", logEntriesByWarehousePO.IdLogEntryType);
                            mySqlCommand.Parameters.AddWithValue("_IsRtfText", logEntriesByWarehousePO.IsRtfText);

                            mySqlCommand.ExecuteNonQuery();
                        }
                        mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error AddCommentsOrLogEntriesByPO(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        #endregion

        #region Update methods
        /// <summary>
        /// This method is used to Update Workflow Status In PO
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="IdWarehousePurchaseOrder">Get IdWarehousePurchaseOrder.</param>
        /// <param name="IdWorkflowStatus">Get IdWorkflowStatus.</param>
        /// <param name="LogEntriesByWarehousePOList">Get Log entry or comment list details.</param>
        public bool UpdateWorkflowStatusInPO(string MainServerConnectionString, uint IdWarehousePurchaseOrder, UInt32 IdWorkflowStatus, int IdUser, List<LogEntriesByWarehousePO> LogEntriesByWarehousePOList)
        {
            bool status;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                    {
                        mySqlConnection.Open();
                        MySqlCommand mySqlCommand = new MySqlCommand("SRM_warehousepurchaseorders_UpdateWorkflowStatus", mySqlConnection);
                        mySqlCommand.CommandType = CommandType.StoredProcedure;
                        mySqlCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrder", IdWarehousePurchaseOrder);
                        mySqlCommand.Parameters.AddWithValue("_IdWorkflowStatus", IdWorkflowStatus);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                        mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        mySqlCommand.ExecuteNonQuery();
                        mySqlConnection.Close();
                        status = true;
                    }
                    AddCommentsOrLogEntriesByPO(MainServerConnectionString, LogEntriesByWarehousePOList);
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Logger.Log(string.Format("Error UpdateWorkflowStatusInPO(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);

                    if (Transaction.Current != null)
                        Transaction.Current.Rollback();

                    if (transactionScope != null)
                        transactionScope.Dispose();

                    throw;
                }
            }
            return status;
        }


        /// <summary>
        /// Get Article Suppliers By Warehouse
        /// </summary>
        /// <param name="warehouse">The warehouse for connection string</param>
        /// <returns>List of Article Suppliers By Warehouse</returns>
        public List<ArticleSupplier> GetArticleSuppliersByWarehouse(Warehouses warehouse)
        {
            string connectionString = warehouse.Company.ConnectPlantConstr;
            List<ArticleSupplier> articleSuppliers = new List<ArticleSupplier>();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetArticleSuppliersByWarehouse", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdWarehouse", warehouse.IdWarehouse);

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    while (mySqlDataReader.Read())
                    {
                        try
                        {
                            ArticleSupplier articleSupplier = new ArticleSupplier();

                            articleSupplier.IdArticleSupplier = Convert.ToInt64(mySqlDataReader["IdArticleSupplier"]);

                            if (mySqlDataReader["name"] != DBNull.Value)
                                articleSupplier.Name = Convert.ToString(mySqlDataReader["name"]);

                            if (mySqlDataReader["IdCountry"] != DBNull.Value)
                            {
                                articleSupplier.IdCountry = Convert.ToInt64(mySqlDataReader["IdCountry"]);

                                articleSupplier.Country = new Country();
                                articleSupplier.Country.IdCountry = Convert.ToByte(mySqlDataReader["IdCountry"]);
                                articleSupplier.Country.Name = Convert.ToString(mySqlDataReader["CountryName"]);
                            }
                            if (mySqlDataReader["Cif"] != DBNull.Value)
                                articleSupplier.Cif = Convert.ToString(mySqlDataReader["Cif"]);

                            if (mySqlDataReader["City"] != DBNull.Value)
                                articleSupplier.City = Convert.ToString(mySqlDataReader["City"]);

                            if (mySqlDataReader["PostCode"] != DBNull.Value)
                                articleSupplier.PostCode = Convert.ToString(mySqlDataReader["PostCode"]);

                            if (mySqlDataReader["Address"] != DBNull.Value)
                                articleSupplier.Address = Convert.ToString(mySqlDataReader["Address"]);

                            if (mySqlDataReader["Email"] != DBNull.Value)
                                articleSupplier.Email = Convert.ToString(mySqlDataReader["Email"]);

                            if (mySqlDataReader["Web"] != DBNull.Value)
                                articleSupplier.Web = Convert.ToString(mySqlDataReader["Web"]);

                            if (mySqlDataReader["Phone1"] != DBNull.Value)
                                articleSupplier.Phone1 = Convert.ToString(mySqlDataReader["Phone1"]);

                            if (mySqlDataReader["Phone2"] != DBNull.Value)
                                articleSupplier.Phone2 = Convert.ToString(mySqlDataReader["Phone2"]);

                            if (mySqlDataReader["serie"] != DBNull.Value)
                                articleSupplier.Serie = Convert.ToChar(mySqlDataReader["serie"]);

                            if (mySqlDataReader["CreatedIn"] != DBNull.Value)
                                articleSupplier.CreatedIn = Convert.ToDateTime(mySqlDataReader["CreatedIn"].ToString());

                            articleSuppliers.Add(articleSupplier);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticleSuppliersByWarehouse(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetArticleSuppliersByWarehouse().", category: Category.Info, priority: Priority.Low);
            return articleSuppliers;
        }



        /// <summary>
        /// This method is used to Delete Article Supplier
        /// </summary>
        /// <param name="MainServerConnectionString">The main server connection string.</param>
        /// <param name="idArticleSupplier">Get idArticleSupplier.</param>
        public bool DeleteArticleSupplier(string MainServerConnectionString, Int64 idArticleSupplier, int IdUser)
        {
            bool status;
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(MainServerConnectionString))
                {
                    mySqlConnection.Open();
                    MySqlCommand mySqlCommand = new MySqlCommand("SRM_ArticleSupplier_Delete", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdArticleSupplier", idArticleSupplier);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedBy", IdUser);
                    mySqlCommand.Parameters.AddWithValue("_ModifiedIn", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    mySqlCommand.ExecuteNonQuery();
                    mySqlConnection.Close();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error DeleteArticleSupplier(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return status;
        }

        public ArticleSupplier GetArticleSupplierByIdArticleSupplier(Warehouses warehouse, UInt64 IdArticleSupplier)
        {
            string connectionString = warehouse.Company.ConnectPlantConstr;
            ArticleSupplier articleSupplier = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetArticleSupplierByIdArticleSupplier", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdArticleSupplier", IdArticleSupplier);

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    if (mySqlDataReader.Read())
                    {
                        try
                        {
                            articleSupplier = new ArticleSupplier();

                            articleSupplier.IdArticleSupplier = Convert.ToInt64(IdArticleSupplier);

                            articleSupplier.Code = "SUPL" + articleSupplier.IdArticleSupplier.ToString().PadLeft(6, '0');

                            if (mySqlDataReader["name"] != DBNull.Value)
                                articleSupplier.Name = Convert.ToString(mySqlDataReader["name"]);

                            if (mySqlDataReader["Cif"] != DBNull.Value)
                                articleSupplier.Cif = Convert.ToString(mySqlDataReader["Cif"]);

                            if (mySqlDataReader["serie"] != DBNull.Value)
                                articleSupplier.Serie = Convert.ToChar(mySqlDataReader["serie"]);

                            if (mySqlDataReader["Observations"] != DBNull.Value)
                                articleSupplier.Observations = Convert.ToString(mySqlDataReader["Observations"]);

                            if (mySqlDataReader["lastOrder"] != DBNull.Value)
                                articleSupplier.LastOrderDate = Convert.ToDateTime(mySqlDataReader["lastOrder"].ToString());

                            if (mySqlDataReader["Phone1"] != DBNull.Value)
                                articleSupplier.Phone1 = Convert.ToString(mySqlDataReader["Phone1"]);

                            if (mySqlDataReader["Phone2"] != DBNull.Value)
                                articleSupplier.Phone2 = Convert.ToString(mySqlDataReader["Phone2"]);

                            if (mySqlDataReader["Email"] != DBNull.Value)
                                articleSupplier.Email = Convert.ToString(mySqlDataReader["Email"]);

                            if (mySqlDataReader["fax"] != DBNull.Value)
                                articleSupplier.Fax = Convert.ToString(mySqlDataReader["fax"]);

                            if (mySqlDataReader["Web"] != DBNull.Value)
                                articleSupplier.Web = Convert.ToString(mySqlDataReader["Web"]);

                            if (mySqlDataReader["Address"] != DBNull.Value)
                                articleSupplier.Address = Convert.ToString(mySqlDataReader["Address"]);

                            if (mySqlDataReader["City"] != DBNull.Value)
                                articleSupplier.City = Convert.ToString(mySqlDataReader["City"]);

                            if (mySqlDataReader["PostCode"] != DBNull.Value)
                                articleSupplier.PostCode = Convert.ToString(mySqlDataReader["PostCode"]);

                            if (mySqlDataReader["idcountry"] != DBNull.Value)
                            {
                                articleSupplier.IdCountry = Convert.ToInt64(mySqlDataReader["idcountry"]);

                                articleSupplier.Country = new Country();
                                articleSupplier.Country.IdCountry = Convert.ToByte(mySqlDataReader["idcountry"]);
                                articleSupplier.Country.Name = Convert.ToString(mySqlDataReader["CountryName"]);
                            }

                            if (mySqlDataReader["modifiedIn"] != DBNull.Value)
                                articleSupplier.ModifiedIn = Convert.ToDateTime(mySqlDataReader["modifiedIn"].ToString());

                            if (mySqlDataReader["createdIn"] != DBNull.Value)
                                articleSupplier.CreatedIn = Convert.ToDateTime(mySqlDataReader["createdIn"].ToString());

                            if (mySqlDataReader["State"] != DBNull.Value)
                                articleSupplier.Region = Convert.ToString(mySqlDataReader["State"]);

                            if (mySqlDataReader["Latitude"] != DBNull.Value)
                            {
                                articleSupplier.Latitude = Convert.ToDouble(mySqlDataReader["Latitude"]);
                                articleSupplier.Coordinates = articleSupplier.Latitude.ToString();
                            }

                            if (mySqlDataReader["Longitude"] != DBNull.Value)
                            {
                                articleSupplier.Longitude = Convert.ToDouble(mySqlDataReader["Longitude"]);
                                if (mySqlDataReader["Latitude"] != DBNull.Value)
                                {
                                    articleSupplier.Coordinates = articleSupplier.Latitude.ToString() + ", " + articleSupplier.Longitude.ToString();
                                }
                                else
                                {
                                    articleSupplier.Coordinates = articleSupplier.Longitude.ToString();
                                }
                            }
                            articleSupplier.ArticleList = GetArticlesByIdArticleSupplier(connectionString, IdArticleSupplier);
                            articleSupplier.DocumentList = GetDocumentsByIdArticleSupplier(connectionString, IdArticleSupplier);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticleSupplierByIdArticleSupplier(IdArticleSupplier-{0}), ErrorMessage- {1} ", IdArticleSupplier, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetArticleSupplierByIdArticleSupplier().", category: Category.Info, priority: Priority.Low);
            return articleSupplier;
        }

        public List<ArticleBySupplier> GetArticlesByIdArticleSupplier(string connectionString, UInt64 IdArticleSupplier)
        {
            List<ArticleBySupplier> articleBySupplierList = new List<ArticleBySupplier>();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetArticlesByIdArticleSupplier", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdArticleSupplier", IdArticleSupplier);

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    while (mySqlDataReader.Read())
                    {
                        try
                        {
                            ArticleBySupplier articleBySupplier = new ArticleBySupplier();

                            articleBySupplier.IdArticleSupplier = Convert.ToInt64(IdArticleSupplier);
                            articleBySupplier.IdArticle = Convert.ToInt64(mySqlDataReader["idArticle"]);

                            if (mySqlDataReader["Reference"] != DBNull.Value)
                                articleBySupplier.Reference = Convert.ToString(mySqlDataReader["Reference"]);

                            if (mySqlDataReader["BasePrice"] != DBNull.Value)
                                articleBySupplier.BasePrice = Convert.ToDouble(mySqlDataReader["BasePrice"]);

                            if (mySqlDataReader["Description"] != DBNull.Value)
                                articleBySupplier.Description = Convert.ToString(mySqlDataReader["Description"]);

                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                            {
                                articleBySupplier.IdCurrency = Convert.ToByte(mySqlDataReader["IdCurrency"]);
                                articleBySupplier.Currency = new Currency();
                                articleBySupplier.Currency.IdCurrency = Convert.ToByte(mySqlDataReader["IdCurrency"]);
                                if (mySqlDataReader["Name"] != DBNull.Value)
                                    articleBySupplier.Currency.Name = Convert.ToString(mySqlDataReader["Name"]);
                            }
                            articleBySupplierList.Add(articleBySupplier);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticlesByIdArticleSupplier(IdArticleSupplier-{0}), ErrorMessage- {1} ", IdArticleSupplier, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetArticlesByIdArticleSupplier().", category: Category.Info, priority: Priority.Low);
            return articleBySupplierList;
        }

        public List<ArticleSuppliersDoc> GetDocumentsByIdArticleSupplier(string connectionString, UInt64 IdArticleSupplier)
        {
            List<ArticleSuppliersDoc> articleSuppliersDocList = new List<ArticleSuppliersDoc>();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetDocumentsByIdArticleSupplier", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdArticleSupplier", IdArticleSupplier);

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    while (mySqlDataReader.Read())
                    {
                        try
                        {
                            ArticleSuppliersDoc articleSuppliersDoc = new ArticleSuppliersDoc();

                            articleSuppliersDoc.IdArticleSupplier = IdArticleSupplier;
                            articleSuppliersDoc.IdSupplierDoc = Convert.ToUInt64(mySqlDataReader["IdSupplierDoc"]);

                            if (mySqlDataReader["OriginalFileName"] != DBNull.Value)
                                articleSuppliersDoc.OriginalFileName = Convert.ToString(mySqlDataReader["OriginalFileName"]);

                            if (mySqlDataReader["SavedFileName"] != DBNull.Value)
                                articleSuppliersDoc.SavedFileName = Convert.ToString(mySqlDataReader["SavedFileName"]);

                            if (mySqlDataReader["Description"] != DBNull.Value)
                                articleSuppliersDoc.Description = Convert.ToString(mySqlDataReader["Description"]);

                            if (mySqlDataReader["IdDocType"] != DBNull.Value)
                                articleSuppliersDoc.IdDocType = Convert.ToUInt64(mySqlDataReader["IdDocType"]);

                            if (mySqlDataReader["ExpirationDate"] != DBNull.Value)
                                articleSuppliersDoc.ExpirationDate = Convert.ToDateTime(mySqlDataReader["ExpirationDate"].ToString());

                            articleSuppliersDocList.Add(articleSuppliersDoc);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetDocumentsByIdArticleSupplier(IdArticleSupplier-{0}), ErrorMessage- {1} ", IdArticleSupplier, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetDocumentsByIdArticleSupplier().", category: Category.Info, priority: Priority.Low);
            return articleSuppliersDocList;
        }


        public bool SendSupplierPurchaseOrderRequestMail(WarehousePurchaseOrder warehousePurchaseOrder, string MailServerName, string MailServerPort, string emailTemplatePath, string POAttachmentPath, string EmailFrom)
        {
            try
            {
                Log4NetLogger.Logger.Log(string.Format("[INFO] SendSupplierPurchaseOrderRequestMail......"), category: Category.Info, priority: Priority.Low);
                bool isSend = false;
                if (warehousePurchaseOrder != null && warehousePurchaseOrder.ArticleSupplier != null)
                {
                    string HTMLFileName = null;
                    if (File.Exists(string.Format("{0}{1}SupplierPurchaseOrderRequestMailFormat_{2}-{3}.html", emailTemplatePath, @"\", warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToLower(), warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToUpper())))
                    {
                        HTMLFileName = string.Format("SupplierPurchaseOrderRequestMailFormat_{0}-{1}.html", warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToLower(), warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToUpper());
                    }
                    else if (File.Exists(string.Format("{0}{1}SupplierPurchaseOrderRequestMailFormat.html", emailTemplatePath, @"\")))
                    {
                        HTMLFileName = "SupplierPurchaseOrderRequestMailFormat.html";
                    }
                    if (HTMLFileName != null)
                    {
                        StringBuilder emailbody = new StringBuilder();

                        try
                        {
                            string text = ReadMailTemplate(HTMLFileName, emailTemplatePath);

                            text = text.Replace("[Supplier_Name]", warehousePurchaseOrder.ArticleSupplier.ContactPerson);

                            text = text.Replace("[PO_Code]", warehousePurchaseOrder.Code);

                            emailbody.Append(text);

                            string Subject = "PO " + warehousePurchaseOrder.Code;
                            string POFileName = warehousePurchaseOrder.Code + ".pdf";

                            POAttachmentPath = POAttachmentPath + @"\" + warehousePurchaseOrder.CreatedIn.Year + @"\" + warehousePurchaseOrder.Code + @"\01 Purchase Order\PO_" + warehousePurchaseOrder.Code + ".pdf";
                            if (File.Exists(POAttachmentPath))
                            {
                                List<string> ccAddress = new List<string>();
                               MailControl.SendHtmlMailWithAttachment(Subject, emailbody.ToString(), warehousePurchaseOrder.ArticleSupplier.ContactEmail, ccAddress, EmailFrom, MailServerName, MailServerPort, null, POFileName, POAttachmentPath);
                              //  MailControl.SendHtmlMailWithAttachment(Subject, emailbody.ToString(), "adhatkar@emdep.com", ccAddress, "adhatkar@emdep.com", MailServerName, MailServerPort, null, POFileName, POAttachmentPath);
                                isSend = true;
                                Log4NetLogger.Logger.Log(string.Format("[INFO] Supplier email sent to - {0} email - {1}", warehousePurchaseOrder.ArticleSupplier.Name, warehousePurchaseOrder.ArticleSupplier.ContactEmail), category: Category.Info, priority: Priority.Low);
                            }
                        }
                        catch (System.Net.Mail.SmtpFailedRecipientException ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("[ERROR] SendSupplierPurchaseOrderRequestMail() in MailControl.SendHtmlMail-SmtpFailedRecipient - {0} - {1}", ex.FailedRecipient, ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("[ERROR] SendSupplierPurchaseOrderRequestMail() in MailControl.SendHtmlMail - {0} - {1}", warehousePurchaseOrder.ArticleSupplier.Name, ex.Message), category: Category.Exception, priority: Priority.Low);
                        }
                    }
                }
                return isSend;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error SendSupplierPurchaseOrderRequestMail(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
        }

        public string ReadMailTemplate(string templateName, string EmailTemplatePath)
        {
            string Text = "";
            try
            {
                Text = System.IO.File.ReadAllText(string.Format(@"{0}\{1}", EmailTemplatePath, templateName));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error ReadMailTemplate(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
            }
            return Text;
        }





        #endregion

        #region GET methods

        /// <summary>
        /// Pending Purchase Orders by warehouse 
        /// </summary>
        /// <param name="warehouse">The warehouse for connection string</param>
        /// <returns>List of pending warehouse purchase order details</returns>
        public List<WarehousePurchaseOrder> GetPendingPurchaseOrdersByWarehouse(Warehouses warehouse)
        {
            string connectionString = warehouse.Company.ConnectPlantConstr;
            List<WarehousePurchaseOrder> warehousePurchaseOrders = new List<WarehousePurchaseOrder>();

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetPendingPurchaseOrdersByWarehouse", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdWarehouse", warehouse.IdWarehouse);

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    double totalQuantity = 0;
                    double receivedQuantity = 0;

                    while (mySqlDataReader.Read())
                    {
                        try
                        {
                            totalQuantity = 0;
                            receivedQuantity = 0;

                            WarehousePurchaseOrder warehousePurchaseOrder = new WarehousePurchaseOrder();

                            warehousePurchaseOrder.IdWarehousePurchaseOrder = Convert.ToInt64(mySqlDataReader["idWarehousepurchaseorder"]);

                            if (mySqlDataReader["Code"] != DBNull.Value)
                                warehousePurchaseOrder.Code = Convert.ToString(mySqlDataReader["Code"]);

                            if (mySqlDataReader["IdArticleSupplier"] != DBNull.Value)
                            {
                                ArticleSupplier articleSupplier = new ArticleSupplier();

                                articleSupplier.IdArticleSupplier = Convert.ToInt64(mySqlDataReader["IdArticleSupplier"]);

                                if (mySqlDataReader["Supplier"] != DBNull.Value)
                                    articleSupplier.Name = Convert.ToString(mySqlDataReader["Supplier"]);

                                if (mySqlDataReader["IdCountry"] != DBNull.Value)
                                {
                                    articleSupplier.IdCountry = Convert.ToInt64(mySqlDataReader["IdCountry"]);

                                    articleSupplier.Country = new Country();
                                    articleSupplier.Country.IdCountry = Convert.ToByte(mySqlDataReader["IdCountry"]);
                                    articleSupplier.Country.Name = Convert.ToString(mySqlDataReader["CountryName"]);
                                }

                                warehousePurchaseOrder.ArticleSupplier = articleSupplier;
                            }

                            if (mySqlDataReader["DeliveryDate"] != DBNull.Value && Convert.ToDateTime(mySqlDataReader["deliveryDate"]) != DateTime.MinValue)
                            {
                                warehousePurchaseOrder.DeliveryDate = Convert.ToDateTime(mySqlDataReader["deliveryDate"]);
                                warehousePurchaseOrder.Delay = (int)(warehousePurchaseOrder.DeliveryDate.Value.Date - DateTime.Now.Date).TotalDays;
                            }

                            if (mySqlDataReader["AttachedPO"] != DBNull.Value)
                                warehousePurchaseOrder.AttachedPO = Convert.ToByte(mySqlDataReader["AttachedPO"]);

                            if (mySqlDataReader["Deliveries"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.Deliveries = Convert.ToInt32(mySqlDataReader["Deliveries"]);

                                if (warehousePurchaseOrder.Deliveries > 0)
                                {
                                    warehousePurchaseOrder.IsPartialPending = true;
                                }
                            }

                            if (mySqlDataReader["LatestDeliveryDate"] != DBNull.Value)
                                warehousePurchaseOrder.LatestDeliveryDate = Convert.ToDateTime(mySqlDataReader["LatestDeliveryDate"]);

                            if (mySqlDataReader["IdWarehouse"] != DBNull.Value)
                                warehousePurchaseOrder.IdWarehouse = Convert.ToInt64(mySqlDataReader["IdWarehouse"]);

                            if (mySqlDataReader["OrderQty"] != DBNull.Value)
                                totalQuantity = Convert.ToDouble(mySqlDataReader["OrderQty"]);

                            if (mySqlDataReader["ReceivedQuantity"] != DBNull.Value)
                                receivedQuantity = Convert.ToDouble(mySqlDataReader["ReceivedQuantity"]);

                            if (totalQuantity > 0)
                                warehousePurchaseOrder.Status = (Int16)((receivedQuantity / totalQuantity) * 100);

                            if (mySqlDataReader["TotalAmount"] != DBNull.Value)
                                warehousePurchaseOrder.TotalAmount = Convert.ToDecimal(mySqlDataReader["TotalAmount"]);

                            if (mySqlDataReader["CreatedIn"] != DBNull.Value)
                                warehousePurchaseOrder.CreatedIn = Convert.ToDateTime(mySqlDataReader["CreatedIn"]);

                            if (mySqlDataReader["AttachPdf"] != DBNull.Value)
                                warehousePurchaseOrder.AttachPdf = Convert.ToString(mySqlDataReader["AttachPdf"]);

                            if (mySqlDataReader["idCurrency"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.IdCurrency = Convert.ToByte(mySqlDataReader["idCurrency"]);
                                warehousePurchaseOrder.Currency = new Currency();
                                warehousePurchaseOrder.Currency.IdCurrency= Convert.ToByte(mySqlDataReader["idCurrency"]);
                                warehousePurchaseOrder.Currency.Name = Convert.ToString(mySqlDataReader["CurrencyName"]);
                                warehousePurchaseOrder.Currency.Symbol = Convert.ToString(mySqlDataReader["CurrencySymbol"]);
                            }

                            if (mySqlDataReader["IdWorkflowStatus"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);
                                warehousePurchaseOrder.WorkflowStatus = new WorkflowStatus();
                                warehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);
                                warehousePurchaseOrder.WorkflowStatus.Name = Convert.ToString(mySqlDataReader["Status"]);
                                warehousePurchaseOrder.WorkflowStatus.HtmlColor = Convert.ToString(mySqlDataReader["HtmlColor"]);
                            }

                            warehousePurchaseOrders.Add(warehousePurchaseOrder);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingPurchaseOrdersByWarehouse(). ErrorMessage-" + ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetPendingPurchaseOrdersByWarehouse().", category: Category.Info, priority: Priority.Low);
            return warehousePurchaseOrders;
        }


        /// <summary>
        /// This method is used to get PO pdf.
        /// </summary>
        /// <param name="PurchaseOrderPath">Get Path.</param>
        /// <param name="AttachPDF">Get File Name.</param>
        public byte[] GetPurchaseOrderPdf(string PurchaseOrderPath, string AttachPDF)
        {
            byte[] bytes = null;
            string fileUploadPath = string.Format(@"{0}\{1}", PurchaseOrderPath, AttachPDF);

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
        /// Get Pending PO By IdWarehousePurchaseOrder
        /// </summary>
        /// <param name="connectionString">The connection string. (crm-context)</param>
        /// <param name="idWarehousePurchaseOrder">The idwarehousepurchseorder.</param>
        /// <returns>Get Pending PO By IdWarehousePurchaseOrder</returns>
        public WarehousePurchaseOrder GetPendingPOByIdWarehousePurchaseOrder(string connectionString, Int64 idWarehousePurchaseOrder, string purchaseOrdersPath, string emailTemplatePath)
        {
            WarehousePurchaseOrder warehousePurchaseOrder = null;

            using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
            {
                mySqlConnection.Open();

                MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetPendingPOByIdWarehousePurchaseOrder", mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                mySqlCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrder", idWarehousePurchaseOrder);

                using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                {
                    if (mySqlDataReader.Read())
                    {
                        try
                        {
                            warehousePurchaseOrder = new WarehousePurchaseOrder();

                            warehousePurchaseOrder.IdWarehousePurchaseOrder = idWarehousePurchaseOrder;

                            if (mySqlDataReader["Code"] != DBNull.Value)
                                warehousePurchaseOrder.Code = Convert.ToString(mySqlDataReader["Code"]);

                            if (mySqlDataReader["IdArticleSupplier"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.IdArticleSupplier = Convert.ToInt64(mySqlDataReader["IdArticleSupplier"]);

                                ArticleSupplier articleSupplier = new ArticleSupplier();
                                articleSupplier.IdArticleSupplier = Convert.ToInt64(mySqlDataReader["IdArticleSupplier"]);

                                if (mySqlDataReader["Supplier"] != DBNull.Value)
                                    articleSupplier.Name = Convert.ToString(mySqlDataReader["Supplier"]);

                                if (mySqlDataReader["SupplierContactPerson"] != DBNull.Value)
                                    articleSupplier.ContactPerson = Convert.ToString(mySqlDataReader["SupplierContactPerson"]);

                                if (mySqlDataReader["SupplierContactEmail"] != DBNull.Value)
                                    articleSupplier.ContactEmail = Convert.ToString(mySqlDataReader["SupplierContactEmail"]);

                                if (mySqlDataReader["Serie"] != DBNull.Value)
                                    articleSupplier.Serie = Convert.ToChar(mySqlDataReader["Serie"]);

                                if (mySqlDataReader["IdCountry"] != DBNull.Value)
                                {
                                    articleSupplier.IdCountry = Convert.ToInt64(mySqlDataReader["IdCountry"]);

                                    articleSupplier.Country = new Country();
                                    articleSupplier.Country.IdCountry = (byte)articleSupplier.IdCountry;
                                    articleSupplier.Country.Name = Convert.ToString(mySqlDataReader["CountryName"]);

                                    if (mySqlDataReader["SupplierCountryISO"] != DBNull.Value)
                                        articleSupplier.Country.Iso = Convert.ToString(mySqlDataReader["SupplierCountryISO"]);

                                    if (mySqlDataReader["IdCountryGroup"] != DBNull.Value)
                                        articleSupplier.Country.IdCountryGroup = Convert.ToInt64(mySqlDataReader["IdCountryGroup"]);
                                }

                                if (mySqlDataReader["IdArticleSupplierType"] != DBNull.Value)
                                {
                                    articleSupplier.IdArticleSupplierType = Convert.ToInt32(mySqlDataReader["IdArticleSupplierType"]);

                                    articleSupplier.ArticleSupplierType = new ArticleSupplierType();
                                    articleSupplier.ArticleSupplierType.IdArticleSupplierType = Convert.ToInt32(mySqlDataReader["IdArticleSupplierType"]);
                                    articleSupplier.ArticleSupplierType.Name = Convert.ToString(mySqlDataReader["ArticleSupplierType"]);
                                    articleSupplier.ArticleSupplierType.HtmlColor = Convert.ToString(mySqlDataReader["HtmlColor"]);
                                }

                                warehousePurchaseOrder.ArticleSupplier = articleSupplier;
                            }

                            if (mySqlDataReader["DeliveryDate"] != DBNull.Value && Convert.ToDateTime(mySqlDataReader["deliveryDate"]) != DateTime.MinValue)
                            {
                                warehousePurchaseOrder.DeliveryDate = Convert.ToDateTime(mySqlDataReader["deliveryDate"]);
                            }

                            if (mySqlDataReader["AttachedPO"] != DBNull.Value)
                                warehousePurchaseOrder.AttachedPO = Convert.ToByte(mySqlDataReader["AttachedPO"]);

                            if (mySqlDataReader["IdPaymentType"] != DBNull.Value)
                                warehousePurchaseOrder.IdPaymentType = Convert.ToInt64(mySqlDataReader["IdPaymentType"]);

                            if (mySqlDataReader["ReminderEmailDate"] != DBNull.Value)
                                warehousePurchaseOrder.ReminderEmailDate = Convert.ToDateTime(mySqlDataReader["ReminderEmailDate"]);

                            if (mySqlDataReader["CreatedIn"] != DBNull.Value)
                                warehousePurchaseOrder.CreatedIn = Convert.ToDateTime(mySqlDataReader["CreatedIn"]);

                            if (mySqlDataReader["ModifiedIn"] != DBNull.Value)
                                warehousePurchaseOrder.ModifiedIn = Convert.ToDateTime(mySqlDataReader["ModifiedIn"]);

                            if (mySqlDataReader["Comments"] != DBNull.Value)
                                warehousePurchaseOrder.Comments = Convert.ToString(mySqlDataReader["Comments"]);

                            if (mySqlDataReader["IsClosed"] != DBNull.Value)
                                warehousePurchaseOrder.IsClosed = Convert.ToByte(mySqlDataReader["IsClosed"]);

                            if (mySqlDataReader["CreatedBy"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.CreatedBy = Convert.ToInt64(mySqlDataReader["CreatedBy"]);
                                warehousePurchaseOrder.Creator = new People();
                                warehousePurchaseOrder.Creator.IdPerson = Convert.ToInt32(mySqlDataReader["CreatedBy"].ToString());
                                warehousePurchaseOrder.Creator.Name = mySqlDataReader["CreatorName"].ToString();
                                warehousePurchaseOrder.Creator.Surname = mySqlDataReader["CreatorSurname"].ToString();
                            }

                            if (mySqlDataReader["ModifiedBy"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.ModifiedBy = Convert.ToInt64(mySqlDataReader["ModifiedBy"]);
                                warehousePurchaseOrder.Modifier = new People();
                                warehousePurchaseOrder.Modifier.IdPerson = Convert.ToInt32(mySqlDataReader["ModifiedBy"].ToString());
                                warehousePurchaseOrder.Modifier.Name = mySqlDataReader["ModifierName"].ToString();
                                warehousePurchaseOrder.Modifier.Surname = mySqlDataReader["ModifierSurname"].ToString();
                            }

                            if (mySqlDataReader["ReasonClosed"] != DBNull.Value)
                                warehousePurchaseOrder.ReasonClosed = Convert.ToString(mySqlDataReader["ReasonClosed"]);

                            if (mySqlDataReader["IdWarehouse"] != DBNull.Value)
                                warehousePurchaseOrder.IdWarehouse = Convert.ToInt64(mySqlDataReader["IdWarehouse"]);

                            if (mySqlDataReader["IdCurrency"] != DBNull.Value)
                                warehousePurchaseOrder.IdCurrency = Convert.ToByte(mySqlDataReader["IdCurrency"]);

                            if (mySqlDataReader["IdWorkflowStatus"] != DBNull.Value)
                            {
                                warehousePurchaseOrder.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);
                                warehousePurchaseOrder.WorkflowStatus = new WorkflowStatus();
                                warehousePurchaseOrder.WorkflowStatus.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);
                                warehousePurchaseOrder.WorkflowStatus.Name = Convert.ToString(mySqlDataReader["Status"]);
                                warehousePurchaseOrder.WorkflowStatus.HtmlColor = Convert.ToString(mySqlDataReader["StatusHtmlColor"]);
                            }
                            string HTMLFileName = null;
                            if (File.Exists(string.Format("{0}{1}SupplierPurchaseOrderRequestMailFormat_{2}-{3}.html", emailTemplatePath, @"\", warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToLower(), warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToUpper())))
                            {
                                HTMLFileName = string.Format("SupplierPurchaseOrderRequestMailFormat_{0}-{1}.html", warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToLower(), warehousePurchaseOrder.ArticleSupplier.Country.Iso.ToUpper());
                            }
                            else if (File.Exists(string.Format("{0}{1}SupplierPurchaseOrderRequestMailFormat.html", emailTemplatePath, @"\")))
                            {
                                HTMLFileName = "SupplierPurchaseOrderRequestMailFormat.html";
                            }
                            if (HTMLFileName != null)
                            {
                                StringBuilder emailbody = new StringBuilder();

                                string text = ReadMailTemplate(HTMLFileName, emailTemplatePath);

                                text = text.Replace("[Supplier_Name]", warehousePurchaseOrder.ArticleSupplier.ContactPerson);

                                text = text.Replace("[PO_Code]", warehousePurchaseOrder.Code);

                                emailbody.Append(text);

                                warehousePurchaseOrder.EmailBody = emailbody.ToString();
                            }
                            warehousePurchaseOrder.AttachPdf = warehousePurchaseOrder.CreatedIn.Year + @"\" + warehousePurchaseOrder.Code + @"\01 Purchase Order\PO_" + warehousePurchaseOrder.Code + ".pdf";

                            warehousePurchaseOrder.AttachmentBytes = GetPurchaseOrderPdf(purchaseOrdersPath, warehousePurchaseOrder.AttachPdf);
                            warehousePurchaseOrder.WarehouseDeliveryNotes = GetDeliveryNotesByIdWarehousePurchaseOrder(connectionString, warehousePurchaseOrder, purchaseOrdersPath);

                            warehousePurchaseOrder.WarehousePurchaseOrderItems = GetArticlesByIdWarehousePurchaseOrder(connectionString, warehousePurchaseOrder.IdWarehousePurchaseOrder);

                            warehousePurchaseOrder.WarehousePOComments = GetlogEntriesByWarehousePO(idWarehousePurchaseOrder, 252, connectionString);
                            warehousePurchaseOrder.WarehousePOLogEntries = GetlogEntriesByWarehousePO(idWarehousePurchaseOrder, 253, connectionString);

                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetPendingPOByIdWarehousePurchaseOrder(idWarehousePurchaseOrder-{0}), ErrorMessage- {1} ", idWarehousePurchaseOrder, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetPendingPOByIdWarehousePurchaseOrder().", category: Category.Info, priority: Priority.Low);
            return warehousePurchaseOrder;
        }


        /// <summary>
        /// Get delivery notes by IdWarehousePurchaseOrder.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="idWarehousePurchaseOrder"></param>
        /// <returns>The idwarehousepurchseorder.</returns>
        public List<WarehouseDeliveryNote> GetDeliveryNotesByIdWarehousePurchaseOrder(string connectionString, WarehousePurchaseOrder warehousePurchaseOrder, string purchaseOrdersPath)
        {
            List<WarehouseDeliveryNote> warehouseDeliveryNotes = new List<WarehouseDeliveryNote>();

            using (MySqlConnection connActivities = new MySqlConnection(connectionString))
            {
                connActivities.Open();

                MySqlCommand activitiesCommand = new MySqlCommand("warehouse_GetDeliveryNotesByIdWarehousePurchaseOrder", connActivities);
                activitiesCommand.CommandType = CommandType.StoredProcedure;
                activitiesCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrder", warehousePurchaseOrder.IdWarehousePurchaseOrder);

                using (MySqlDataReader poReader = activitiesCommand.ExecuteReader())
                {
                    while (poReader.Read())
                    {
                        try
                        {
                            WarehouseDeliveryNote warehouseDeliveryNote = new WarehouseDeliveryNote();

                            warehouseDeliveryNote.IdWarehousePurchaseOrder = warehousePurchaseOrder.IdWarehousePurchaseOrder;

                            if (poReader["Idwarehousedeliverynote"] != DBNull.Value)
                                warehouseDeliveryNote.IdWarehouseDeliveryNote = Convert.ToInt64(poReader["Idwarehousedeliverynote"]);

                            if (poReader["Attacheddeliverynotes"] != DBNull.Value)
                                warehouseDeliveryNote.AttachedDeliveryNotes = Convert.ToByte(poReader["Attacheddeliverynotes"]);

                            if (poReader["IdCurrencyImportExpenses"] != DBNull.Value)
                                warehouseDeliveryNote.IdCurrencyImportExpenses = Convert.ToInt64(poReader["IdCurrencyImportExpenses"]);

                            if (poReader["ImportExpenses"] != DBNull.Value)
                                warehouseDeliveryNote.ImportExpenses = Convert.ToDouble(poReader["ImportExpenses"]);

                            if (poReader["Code"] != DBNull.Value)
                                warehouseDeliveryNote.Code = Convert.ToString(poReader["Code"]);

                            if (poReader["DeliveryDate"] != DBNull.Value)
                                warehouseDeliveryNote.DeliveryDate = Convert.ToDateTime(poReader["DeliveryDate"]);

                            if (poReader["CreatedIn"] != DBNull.Value)
                                warehouseDeliveryNote.CreatedIn = Convert.ToDateTime(poReader["CreatedIn"]);

                            if (poReader["ModifiedIn"] != DBNull.Value)
                                warehouseDeliveryNote.ModifiedIn = Convert.ToDateTime(poReader["ModifiedIn"]);

                            if (poReader["SupplierCode"] != DBNull.Value)
                                warehouseDeliveryNote.SupplierCode = Convert.ToString(poReader["SupplierCode"]);

                            if (poReader["Idcurrency"] != DBNull.Value)
                                warehouseDeliveryNote.IdCurrency = Convert.ToByte(poReader["Idcurrency"]);

                            //if (poReader["LocalIdCurrency"] != DBNull.Value)
                            //    warehouseDeliveryNote.LocalIdCurrency = Convert.ToByte(poReader["LocalIdCurrency"]);

                            if (poReader["ExchangeRate"] != DBNull.Value)
                                warehouseDeliveryNote.ExchangeRate = Convert.ToByte(poReader["ExchangeRate"]);

                            if (poReader["ZF01"] != DBNull.Value)
                                warehouseDeliveryNote.ZF01 = Convert.ToString(poReader["ZF01"]);

                            if (poReader["CurrentlyAccessedBy"] != DBNull.Value)
                                warehouseDeliveryNote.CurrentlyAccessedBy = Convert.ToInt32(poReader["CurrentlyAccessedBy"]);

                            warehouseDeliveryNote.PdfFilePath = purchaseOrdersPath + @"\" + warehousePurchaseOrder.CreatedIn.Year + @"\" + warehousePurchaseOrder.Code + @"\02 Delivery Notes\DN_" + warehouseDeliveryNote.Code + ".pdf"; ;

                            warehouseDeliveryNotes.Add(warehouseDeliveryNote);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Executed GetDeliveryNotesByIdWarehousePurchaseOrder(idWarehousePurchaseOrder-{0}). ErrorMessage- {1}", warehousePurchaseOrder.IdWarehousePurchaseOrder, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetDeliveryNotesByIdWarehousePurchaseOrder().", category: Category.Info, priority: Priority.Low);
            return warehouseDeliveryNotes;
        }


        /// <summary>
        /// Get articles by IdWarehousePurchaseOrder
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="idWarehousePurchaseOrder">The idwarehousepurchseorder.</param>
        /// <returns>The List WarehousePurchaseOrderItems</returns>
        public List<WarehousePurchaseOrderItem> GetArticlesByIdWarehousePurchaseOrder(string connectionString, Int64 idWarehousePurchaseOrder)
        {
            List<WarehousePurchaseOrderItem> warehousePurchaseOrderItems = new List<WarehousePurchaseOrderItem>();

            using (MySqlConnection connWarehousePurchaseOrderItem = new MySqlConnection(connectionString))
            {
                connWarehousePurchaseOrderItem.Open();

                MySqlCommand activitiesCommand = new MySqlCommand("SRM_GetArticlesAndPOItemsByIdWarehousePurchaseOrder", connWarehousePurchaseOrderItem);
                activitiesCommand.CommandType = CommandType.StoredProcedure;
                activitiesCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrder", idWarehousePurchaseOrder);

                using (MySqlDataReader poReader = activitiesCommand.ExecuteReader())
                {
                    while (poReader.Read())
                    {
                        try
                        {
                            WarehousePurchaseOrderItem warehousePurchaseOrderItem = new WarehousePurchaseOrderItem();

                            warehousePurchaseOrderItem.IdWarehousePurchaseOrder = idWarehousePurchaseOrder;

                            if (poReader["IdWarehousePurchaseOrderItem"] != DBNull.Value)
                                warehousePurchaseOrderItem.IdWarehousePurchaseOrderItem = Convert.ToInt64(poReader["IdWarehousePurchaseOrderItem"]);

                            if (poReader["Description"] != DBNull.Value)
                                warehousePurchaseOrderItem.Description = Convert.ToString(poReader["Description"]);

                            if (poReader["IdArticle"] != DBNull.Value)
                            {
                                warehousePurchaseOrderItem.IdArticle = Convert.ToInt32(poReader["IdArticle"]);

                                Article article = new Article();

                                if (poReader["Reference"] != DBNull.Value)
                                    article.Reference = Convert.ToString(poReader["Reference"]);

                                if (poReader["IsGeneric"] != DBNull.Value)
                                    article.IsGeneric = Convert.ToSByte(poReader["IsGeneric"]);

                                if (poReader["IdArticleCategory"] != DBNull.Value)
                                    article.IdArticleCategory = Convert.ToInt32(poReader["IdArticleCategory"]);

                                warehousePurchaseOrderItem.Article = article;
                            }

                            if (poReader["Quantity"] != DBNull.Value)
                                warehousePurchaseOrderItem.Quantity = Convert.ToInt32(poReader["Quantity"]);

                            if (poReader["UnitPrice"] != DBNull.Value)
                                warehousePurchaseOrderItem.UnitPrice = Convert.ToDouble(poReader["UnitPrice"]);

                            if (poReader["Discount"] != DBNull.Value)
                                warehousePurchaseOrderItem.Discount = Convert.ToDouble(poReader["Discount"]);

                            if (poReader["Iva"] != DBNull.Value)
                                warehousePurchaseOrderItem.IVA = Convert.ToDouble(poReader["Iva"]);

                            if (poReader["AdditionalComments"] != DBNull.Value)
                                warehousePurchaseOrderItem.AdditionalComments = Convert.ToString(poReader["AdditionalComments"]);

                            if (poReader["Position"] != DBNull.Value)
                                warehousePurchaseOrderItem.Position = Convert.ToInt64(poReader["Position"]);

                            if (poReader["AlbaranQty"] != DBNull.Value)
                                warehousePurchaseOrderItem.ReceivedQuantity = Convert.ToInt32(poReader["AlbaranQty"]);


                            if (poReader["TotalAmount"] != DBNull.Value)
                                warehousePurchaseOrderItem.TotalAmount = Convert.ToDecimal(poReader["TotalAmount"]);

                            if (warehousePurchaseOrderItem.Quantity > 0)
                            {
                                warehousePurchaseOrderItem.Status = (Int32)(warehousePurchaseOrderItem.ReceivedQuantity / warehousePurchaseOrderItem.Quantity * 100);
                            }

                            warehousePurchaseOrderItem.WarehousePurchaseOrderExpectedItems = GetWarehousePOExpectedItems(connectionString, warehousePurchaseOrderItem.IdWarehousePurchaseOrderItem);

                            if (warehousePurchaseOrderItem.WarehousePurchaseOrderExpectedItems.Count > 0)
                            {
                                warehousePurchaseOrderItem.ExpectedDate = warehousePurchaseOrderItem.WarehousePurchaseOrderExpectedItems.First().Date;
                            }

                            warehousePurchaseOrderItem.ReceivedDeliveryNotes = GetDeliveryNotesByIdWarehousePurchaseOrderItem(connectionString, warehousePurchaseOrderItem.IdWarehousePurchaseOrderItem);
                            warehousePurchaseOrderItems.Add(warehousePurchaseOrderItem);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetArticlesByIdWarehousePurchaseOrder(idWarehousePurchaseOrder-{0}). ErrorMessage- {1}", idWarehousePurchaseOrder, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetArticlesByIdWarehousePurchaseOrder().", category: Category.Info, priority: Priority.Low);
            return warehousePurchaseOrderItems;
        }


        /// <summary>
        /// [WMS-M028-08] Changes in PO details section - Show POExpectedItems dates in grid.
        /// </summary>
        /// <param name="connectionString">The Connection string to PO - warehouse</param>
        /// <param name="idWarehousePurchaseOrderItem">The IdWarehousePurchaseOrderItem</param>
        /// <returns>List of warehouse purchase order expected item details</returns>
        public List<WarehousePurchaseOrderExpectedItem> GetWarehousePOExpectedItems(string connectionString, Int64 idWarehousePurchaseOrderItem)
        {
            List<WarehousePurchaseOrderExpectedItem> warehousePurchaseOrderExpectedItems = new List<WarehousePurchaseOrderExpectedItem>();

            using (MySqlConnection connWarehousePurchaseOrderItem = new MySqlConnection(connectionString))
            {
                connWarehousePurchaseOrderItem.Open();

                MySqlCommand activitiesCommand = new MySqlCommand("warehouse_GetWarehousePOExpectedItems", connWarehousePurchaseOrderItem);
                activitiesCommand.CommandType = CommandType.StoredProcedure;
                activitiesCommand.Parameters.AddWithValue("_idWarehousePurchaseOrderItem", idWarehousePurchaseOrderItem);

                using (MySqlDataReader poReader = activitiesCommand.ExecuteReader())
                {
                    while (poReader.Read())
                    {
                        try
                        {
                            WarehousePurchaseOrderExpectedItem warehousePurchaseOrderExpectedItem = new WarehousePurchaseOrderExpectedItem();

                            if (poReader["idWarehousePurchaseOrderItem"] != DBNull.Value)
                                warehousePurchaseOrderExpectedItem.IdWarehousePurchaseOrderItem = Convert.ToInt64(poReader["idWarehousePurchaseOrderItem"]);

                            if (poReader["Quantity"] != DBNull.Value)
                                warehousePurchaseOrderExpectedItem.Quantity = Convert.ToInt64(poReader["Quantity"]);

                            if (poReader["Date"] != DBNull.Value)
                                warehousePurchaseOrderExpectedItem.Date = Convert.ToDateTime(poReader["Date"]);

                            warehousePurchaseOrderExpectedItems.Add(warehousePurchaseOrderExpectedItem);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Error GetWarehousePOExpectedItems(idWarehousePurchaseOrder-{0}). ErrorMessage- {1}", idWarehousePurchaseOrderItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetWarehousePOExpectedItems().", category: Category.Info, priority: Priority.Low);
            return warehousePurchaseOrderExpectedItems;
        }


        /// <summary>
        /// [WMS-M028-08] Changes in PO details section - Show delivery notes to perticuler PO item.
        /// </summary>
        /// <param name="connectionString">The Connection string to PO - warehouse</param>
        /// <param name="IdWarehousePurchaseOrderItem">The IdWarehousePurchaseOrderItem</param>
        /// <returns>List of warehouse delivery note details</returns>
        public List<WarehouseDeliveryNote> GetDeliveryNotesByIdWarehousePurchaseOrderItem(string connectionString, Int64 IdWarehousePurchaseOrderItem)
        {
            List<WarehouseDeliveryNote> warehouseDeliveryNotes = new List<WarehouseDeliveryNote>();

            using (MySqlConnection connActivities = new MySqlConnection(connectionString))
            {
                connActivities.Open();

                MySqlCommand activitiesCommand = new MySqlCommand("warehouse_GetDeliveryNotesByIdWarehousePOItem", connActivities);
                activitiesCommand.CommandType = CommandType.StoredProcedure;
                activitiesCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrderItem", IdWarehousePurchaseOrderItem);

                using (MySqlDataReader poReader = activitiesCommand.ExecuteReader())
                {
                    while (poReader.Read())
                    {
                        try
                        {
                            WarehouseDeliveryNote warehouseDeliveryNote = new WarehouseDeliveryNote();

                            if (poReader["IdWarehousePurchaseOrder"] != DBNull.Value)
                                warehouseDeliveryNote.IdWarehousePurchaseOrder = Convert.ToInt64(poReader["IdWarehousePurchaseOrder"]);

                            if (poReader["IdWarehouseDeliveryNote"] != DBNull.Value)
                                warehouseDeliveryNote.IdWarehouseDeliveryNote = Convert.ToInt64(poReader["IdWarehouseDeliveryNote"]);

                            if (poReader["Code"] != DBNull.Value)
                                warehouseDeliveryNote.Code = Convert.ToString(poReader["Code"]);

                            if (poReader["DeliveryDate"] != DBNull.Value)
                                warehouseDeliveryNote.DeliveryDate = Convert.ToDateTime(poReader["DeliveryDate"]);

                            if (poReader["SupplierCode"] != DBNull.Value)
                                warehouseDeliveryNote.SupplierCode = Convert.ToString(poReader["SupplierCode"]);

                            if (poReader["Quantity"] != DBNull.Value)
                                warehouseDeliveryNote.Quantity = Convert.ToInt64(poReader["Quantity"]);

                            warehouseDeliveryNotes.Add(warehouseDeliveryNote);
                        }
                        catch (Exception ex)
                        {
                            Log4NetLogger.Logger.Log(string.Format("Executed GetDeliveryNotesByIdWarehousePurchaseOrderItem(idWarehousePurchaseOrder-{0}). ErrorMessage- {1}", IdWarehousePurchaseOrderItem, ex.Message), category: Category.Exception, priority: Priority.Low);
                            throw;
                        }
                    }
                }
            }

            Log4NetLogger.Logger.Log("Executed GetDeliveryNotesByIdWarehousePurchaseOrderItem().", category: Category.Info, priority: Priority.Low);
            return warehouseDeliveryNotes;
        }

        /// <summary>
        /// This method is used to get log entries by warehouse PO.
        /// </summary>
        /// <param name="idWarehousePurchaseOrder">Get warehouse PO id.</param>
        /// <param name="idLogEntryType">Get log entry type.</param>
        /// <param name="connectionString">connection string.</param>
        public List<LogEntriesByWarehousePO> GetlogEntriesByWarehousePO(Int64 idWarehousePurchaseOrder, byte idLogEntryType, string connectionString)
        {
            List<LogEntriesByWarehousePO> logEntriesByWarehousePOList = new List<LogEntriesByWarehousePO>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetLogEntriesByWarehousePurchaseOrder", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_IdWarehousePurchaseOrder", idWarehousePurchaseOrder);
                    mySqlCommand.Parameters.AddWithValue("_IdLogEntryType", idLogEntryType);

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            LogEntriesByWarehousePO logEntriesByWarehousePO = new LogEntriesByWarehousePO();

                            logEntriesByWarehousePO.IdLogEntryByPurchaseOrder = Convert.ToInt64(mySqlDataReader["IdLogEntryByPurchaseOrder"]);

                            if (mySqlDataReader["IdUser"] != DBNull.Value)
                                logEntriesByWarehousePO.IdUser = Convert.ToInt32(mySqlDataReader["IdUser"].ToString());

                            if (mySqlDataReader["IdWarehousePurchaseOrder"] != DBNull.Value)
                                logEntriesByWarehousePO.IdWarehousePurchaseOrder = Convert.ToInt64(mySqlDataReader["IdWarehousePurchaseOrder"].ToString());

                            if (mySqlDataReader["IdPerson"] != DBNull.Value)
                            {
                                logEntriesByWarehousePO.People = new People();
                                logEntriesByWarehousePO.People.IdPerson = Convert.ToInt32(mySqlDataReader["IdPerson"].ToString());
                                logEntriesByWarehousePO.People.Name = mySqlDataReader["Name"].ToString();
                                logEntriesByWarehousePO.People.Surname = mySqlDataReader["Surname"].ToString();
                            }

                            if (mySqlDataReader["Datetime"] != DBNull.Value)
                                logEntriesByWarehousePO.Datetime = Convert.ToDateTime(mySqlDataReader["Datetime"].ToString());

                            if (mySqlDataReader["Comments"] != DBNull.Value)
                                logEntriesByWarehousePO.Comments = mySqlDataReader["Comments"].ToString();

                            if (mySqlDataReader["IdLogEntryType"] != DBNull.Value)
                                logEntriesByWarehousePO.IdLogEntryType = Convert.ToByte(mySqlDataReader["IdLogEntryType"]);

                            if (mySqlDataReader["IsRtfText"] != DBNull.Value)
                                logEntriesByWarehousePO.IsRtfText = Convert.ToBoolean(mySqlDataReader["IsRtfText"]);

                            logEntriesByWarehousePOList.Add(logEntriesByWarehousePO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetlogEntriesByWarehousePO(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return logEntriesByWarehousePOList;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowStatus> GetAllWorkflowStatus(string connectionString)
        {

            List<WorkflowStatus> workflowStatusList = new List<WorkflowStatus>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SRM_GetAllWorkflowStatus", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorkflowStatus workflowStatus = new WorkflowStatus();

                            workflowStatus.IdWorkflowStatus = Convert.ToByte(mySqlDataReader["IdWorkflowStatus"]);

                            if (mySqlDataReader["IdWorkflow"] != DBNull.Value)
                                workflowStatus.IdWorkflow = Convert.ToUInt32(mySqlDataReader["IdWorkflow"].ToString());

                            if (mySqlDataReader["Name"] != DBNull.Value)
                                workflowStatus.Name = mySqlDataReader["Name"].ToString();

                            if (mySqlDataReader["HtmlColor"] != DBNull.Value)
                                workflowStatus.HtmlColor = mySqlDataReader["HtmlColor"].ToString();

                            if (mySqlDataReader["IdWorkflowScope"] != DBNull.Value)
                                workflowStatus.IdWorkflowScope = Convert.ToUInt32(mySqlDataReader["IdWorkflowScope"].ToString());

                            workflowStatusList.Add(workflowStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWorkflowStatus(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return workflowStatusList;
        }

        /// <summary>
        /// This method is used to get All workflow status.
        /// </summary>
        /// <param name="connectionString">connection string.</param>
        public List<WorkflowTransition> GetAllWorkflowTransitions(string connectionString)
        {

            List<WorkflowTransition> workflowTransitionList = new List<WorkflowTransition>();
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
                {
                    mySqlConnection.Open();

                    MySqlCommand mySqlCommand = new MySqlCommand("SRM_Get_workflow_transitions", mySqlConnection);
                    mySqlCommand.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        while (mySqlDataReader.Read())
                        {
                            WorkflowTransition workflowTransition = new WorkflowTransition();

                            workflowTransition.IdWorkflowTransition = Convert.ToUInt32(mySqlDataReader["IdWorkflowTransition"]);

                            if (mySqlDataReader["IdWorkflow"] != DBNull.Value)
                                workflowTransition.IdWorkflow = Convert.ToUInt32(mySqlDataReader["IdWorkflow"]);

                            if (mySqlDataReader["Name"] != DBNull.Value)
                                workflowTransition.Name = mySqlDataReader["Name"].ToString();

                            if (mySqlDataReader["IdWorkflowStatusFrom"] != DBNull.Value)
                                workflowTransition.IdWorkflowStatusFrom = Convert.ToByte(mySqlDataReader["IdWorkflowStatusFrom"]);

                            if (mySqlDataReader["IdWorkflowStatusTo"] != DBNull.Value)
                                workflowTransition.IdWorkflowStatusTo = Convert.ToByte(mySqlDataReader["IdWorkflowStatusTo"]);

                            if (mySqlDataReader["IsCommentRequired"] != DBNull.Value)
                                workflowTransition.IsCommentRequired = Convert.ToByte(mySqlDataReader["IsCommentRequired"]);

                            if (mySqlDataReader["IdWorkflowScope"] != DBNull.Value)
                                workflowTransition.IdWorkflowScope = Convert.ToUInt32(mySqlDataReader["IdWorkflowScope"]);

                            workflowTransitionList.Add(workflowTransition);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Logger.Log(string.Format("Error GetAllWorkflowTransitions(). ErrorMessage- {0}", ex.Message), category: Category.Exception, priority: Priority.Low);
                throw;
            }
            return workflowTransitionList;
        }
        #endregion

        #region Other methods
        static internal double CalculatePrice(double basePrice, int quantity, float discount, float iva, float overCost)
        {
            //[001] Round-Off the value for base price and then calculate the total value
            basePrice = Convert.ToDouble(basePrice.ToString("n5"));
            double total = (basePrice * quantity) *((100 + iva) / 100) *((100 - discount) / 100) *((100 + overCost) / 100);
            return total;
        }
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
    }
}