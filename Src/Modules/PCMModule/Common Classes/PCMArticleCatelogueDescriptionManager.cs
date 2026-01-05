using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Export;
using DevExpress.XtraRichEdit.Export.Html;
using Emdep.Geos.Data.Common;
using Emdep.Geos.Data.Common.PCM;
using Emdep.Geos.Modules.PCM.ViewModels;
using Emdep.Geos.UI.Common;
using Prism.Logging;

namespace Emdep.Geos.Modules.PCM
{
    public static class PcmArticleCatalogueDescriptionManager
    {
        private static EditPCMArticleViewModel EditPCMArticleViewModelInstance;
        
        private static Dictionary<string, RichEditControl> dictionaryLanguagewiseRichEditControls =
            new Dictionary<string, RichEditControl>
            {
                {"EN", new RichEditControl()},
                {"ES", new RichEditControl()},
                {"FR", new RichEditControl()},
                {"PT", new RichEditControl()},
                {"RO", new RichEditControl()},
                {"RU", new RichEditControl()},
                {"ZH", new RichEditControl()}
            };
        
        public static Language LanguageSelectedInCatelogueDescription
        {
            get { return EditPCMArticleViewModelInstance.LanguageSelectedInCatelogueDescription; }
        }
        private static bool IsCheckedCopyCatelogueDescription
        {
            get { return EditPCMArticleViewModelInstance.IsCheckedCopyCatelogueDescription; }
        }

        private static bool IsRtf
        {
            get { return EditPCMArticleViewModelInstance.IsRtf; }
        }
        private static string PCMDescription
        {
            get { return EditPCMArticleViewModelInstance.PCMDescription; }
        }
        private static string PCMDescription_Richtext
        {
            get { return EditPCMArticleViewModelInstance.PCMDescription_Richtext; }
        }

        public static string GetHTMLPageBodyOnlyForSavingInDatabase(
            RichEditControl richEditControlObj)
        {
            const string methodNameWithBrackets = nameof(GetHTMLPageBodyOnlyForSavingInDatabase) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                if (richEditControlObj == null || String.IsNullOrWhiteSpace(richEditControlObj.Text))
                {
                    return string.Empty;
                }
                else
                {
                    using (var RichEditDocumentServerObj = new RichEditDocumentServer { HtmlText = richEditControlObj.HtmlText.Trim(' ', '\r') })
                    {
                        var htmlDocumentExporterOptions = new HtmlDocumentExporterOptions
                        {
                            ExportRootTag = ExportRootTag.Body,
                            CssPropertiesExportType = CssPropertiesExportType.Inline
                        };
                        var exporter = new HtmlExporter(RichEditDocumentServerObj.Model,
                            htmlDocumentExporterOptions);
                        var htmlDocumentExported = exporter.Export();
                        var pFrom = htmlDocumentExported.IndexOf("<body>", StringComparison.InvariantCultureIgnoreCase) + "<body>".Length;
                        var pTo = htmlDocumentExported.LastIndexOf("</body>", StringComparison.InvariantCultureIgnoreCase);
                        GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}....executed successfully", category: Category.Info, priority: Priority.Low);
                        return htmlDocumentExported.Substring(pFrom, pTo - pFrom);
                    }
                }                
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return string.Empty;
        }

        public static void GetTextForCurrentOnelanguage(
            ref string PCMDescription_Richtext, ref string PCMDescription)
        {
            const string methodNameWithBrackets = nameof(GetTextForCurrentOnelanguage) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                if (IsRtf)
                {
                    PCMDescription_Richtext = dictionaryLanguagewiseRichEditControls[
                        LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].HtmlText;
                }
                else
                {
                    PCMDescription = dictionaryLanguagewiseRichEditControls[
                        LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].Text;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void SetEditPCMArticleViewModelInstance(EditPCMArticleViewModel editPCMArticleViewModelInstance)
        {
            const string methodNameWithBrackets = nameof(SetEditPCMArticleViewModelInstance) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                EditPCMArticleViewModelInstance = editPCMArticleViewModelInstance;
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static void SetNormalAndRichTextForAllLanguagesFromDatabaseData(
                    Articles Article1,
                    ref bool IsRtf1, ref bool IsNormal1,
                    ref bool IsCheckedCopyCatelogueDescription1,
                    ref string PCMDescription_Richtext1,
                    ref string PCMDescription1,
                    ref Visibility Richtextboxrtf1, ref Visibility Textboxnormal1,
                    EditPCMArticleViewModel editPCMArticleViewModelInstance)
        {
            const string methodNameWithBrackets = nameof(SetNormalAndRichTextForAllLanguagesFromDatabaseData) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                EditPCMArticleViewModelInstance = editPCMArticleViewModelInstance;
                dictionaryLanguagewiseRichEditControls["EN"].HtmlText = Article1.PCMDescription;
                dictionaryLanguagewiseRichEditControls["ES"].HtmlText = Article1.PCMDescription_es;
                dictionaryLanguagewiseRichEditControls["FR"].HtmlText = Article1.PCMDescription_fr;
                dictionaryLanguagewiseRichEditControls["PT"].HtmlText = Article1.PCMDescription_pt;
                dictionaryLanguagewiseRichEditControls["RO"].HtmlText = Article1.PCMDescription_ro;
                dictionaryLanguagewiseRichEditControls["RU"].HtmlText = Article1.PCMDescription_ru;
                dictionaryLanguagewiseRichEditControls["ZH"].HtmlText = Article1.PCMDescription_zh;

                if (Article1.IsRtfText)
                {
                    IsRtf1 = true;
                    IsNormal1 = false;
                    Richtextboxrtf1 = Visibility.Visible;
                    Textboxnormal1 = Visibility.Collapsed;
                    PCMDescription_Richtext1 = dictionaryLanguagewiseRichEditControls["EN"].HtmlText;
                    PCMDescription1 = string.Empty;
                }
                else
                {
                    dictionaryLanguagewiseRichEditControls["EN"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["EN"].Text;
                    dictionaryLanguagewiseRichEditControls["ES"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["ES"].Text;
                    dictionaryLanguagewiseRichEditControls["FR"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["FR"].Text;
                    dictionaryLanguagewiseRichEditControls["PT"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["PT"].Text;
                    dictionaryLanguagewiseRichEditControls["RO"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["RO"].Text;
                    dictionaryLanguagewiseRichEditControls["RU"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["RU"].Text;
                    dictionaryLanguagewiseRichEditControls["ZH"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["ZH"].Text;

                    IsRtf1 = false;
                    IsNormal1 = true;
                    Richtextboxrtf1 = Visibility.Collapsed;
                    Textboxnormal1 = Visibility.Visible;
                    PCMDescription1 = dictionaryLanguagewiseRichEditControls["EN"].Text;
                    PCMDescription_Richtext1 = string.Empty;
                }

                IsCheckedCopyCatelogueDescription1 = false;
                if (!string.IsNullOrEmpty(Article1.PCMDescription) &&
                        Article1.PCMDescription == Article1.PCMDescription_es &&
                        Article1.PCMDescription == Article1.PCMDescription_fr &&
                        Article1.PCMDescription == Article1.PCMDescription_pt &&
                        Article1.PCMDescription == Article1.PCMDescription_ro &&
                        Article1.PCMDescription == Article1.PCMDescription_ru &&
                        Article1.PCMDescription == Article1.PCMDescription_zh)
                {
                    IsCheckedCopyCatelogueDescription1 = true;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        
        public static void CopyCatelogueDescription()
        {
            const string methodNameWithBrackets = nameof(CopyCatelogueDescription) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                if (IsCheckedCopyCatelogueDescription)
                {
                    if (IsRtf)
                    {
                        dictionaryLanguagewiseRichEditControls["EN"].HtmlText =
                            dictionaryLanguagewiseRichEditControls[
                                LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].HtmlText;

                        dictionaryLanguagewiseRichEditControls["ES"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["FR"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["PT"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["RO"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["RU"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["ZH"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["EN"].HtmlText;
                    }
                    else
                    {
                        dictionaryLanguagewiseRichEditControls["EN"].Text =
                                dictionaryLanguagewiseRichEditControls[
                                    LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].Text;

                        dictionaryLanguagewiseRichEditControls["ES"].Text =
                        dictionaryLanguagewiseRichEditControls["FR"].Text =
                        dictionaryLanguagewiseRichEditControls["PT"].Text =
                        dictionaryLanguagewiseRichEditControls["RO"].Text =
                        dictionaryLanguagewiseRichEditControls["RU"].Text =
                        dictionaryLanguagewiseRichEditControls["ZH"].Text =
                        dictionaryLanguagewiseRichEditControls["EN"].Text;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }

        public static Articles UpdateArticleNormalOrRtfPCMDescriptionFromLocalData(Articles updatedArticle)
        {
            const string methodNameWithBrackets = nameof(UpdateArticleNormalOrRtfPCMDescriptionFromLocalData) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);
                
                updatedArticle.IsRtfText = IsRtf;
                if (IsCheckedCopyCatelogueDescription)
                {
                    if (IsRtf)
                    {
                        updatedArticle.PCMDescription = GetHTMLPageBodyOnlyForSavingInDatabase(
                            dictionaryLanguagewiseRichEditControls[
                            LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage]);
                    }
                    else
                    {
                        updatedArticle.PCMDescription =
                            dictionaryLanguagewiseRichEditControls[
                    LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].Text;
                    }

                    updatedArticle.PCMDescription_es =
                    updatedArticle.PCMDescription_fr =
                    updatedArticle.PCMDescription_pt =
                    updatedArticle.PCMDescription_ro =
                    updatedArticle.PCMDescription_ru =
                    updatedArticle.PCMDescription_zh = updatedArticle.PCMDescription;
                }
                else if (IsRtf)
                {
                    updatedArticle.PCMDescription =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["EN"]);
                    updatedArticle.PCMDescription_es =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["ES"]);
                    updatedArticle.PCMDescription_fr =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["FR"]);
                    updatedArticle.PCMDescription_pt =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["PT"]);
                    updatedArticle.PCMDescription_ro =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["RO"]);
                    updatedArticle.PCMDescription_ru =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["RU"]);
                    updatedArticle.PCMDescription_zh =
                        GetHTMLPageBodyOnlyForSavingInDatabase(dictionaryLanguagewiseRichEditControls["ZH"]);
                }
                else if (!IsRtf)
                {
                    updatedArticle.PCMDescription = dictionaryLanguagewiseRichEditControls["EN"].Text;
                    updatedArticle.PCMDescription_es = dictionaryLanguagewiseRichEditControls["ES"].Text;
                    updatedArticle.PCMDescription_fr = dictionaryLanguagewiseRichEditControls["FR"].Text;
                    updatedArticle.PCMDescription_pt = dictionaryLanguagewiseRichEditControls["PT"].Text;
                    updatedArticle.PCMDescription_ro = dictionaryLanguagewiseRichEditControls["RO"].Text;
                    updatedArticle.PCMDescription_ru = dictionaryLanguagewiseRichEditControls["RU"].Text;
                    updatedArticle.PCMDescription_zh = dictionaryLanguagewiseRichEditControls["ZH"].Text;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
            return updatedArticle;
        }
        
        public static void SwitchTheTextModeBetweenRichAndNormal()
        {
            const string methodNameWithBrackets = nameof(SwitchTheTextModeBetweenRichAndNormal) + "()";
            try
            {
                GeosApplication.Instance.Logger.Log($"Method {methodNameWithBrackets}...", category: Category.Info, priority: Priority.Low);

                if (!IsRtf)
                {
                    //copy the all languages rich text to normal text
                    dictionaryLanguagewiseRichEditControls["EN"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["EN"].Text;
                    dictionaryLanguagewiseRichEditControls["ES"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["ES"].Text;
                    dictionaryLanguagewiseRichEditControls["FR"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["FR"].Text;
                    dictionaryLanguagewiseRichEditControls["PT"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["PT"].Text;
                    dictionaryLanguagewiseRichEditControls["RO"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["RO"].Text;
                    dictionaryLanguagewiseRichEditControls["RU"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["RU"].Text;
                    dictionaryLanguagewiseRichEditControls["ZH"].HtmlText =
                        dictionaryLanguagewiseRichEditControls["ZH"].Text;
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log($"Get an error in Method {methodNameWithBrackets}...." + ex.Message + GeosApplication.createExceptionDetailsMsg(ex), category: Category.Exception, priority: Priority.Low);
            }
        }
        
        public static void UpdateEnteredNormalTextForCurrentOnelanguage(
            TextEdit ObjTextEdit)
        {
            // Not added log because the method is executed continuously while typing
            dictionaryLanguagewiseRichEditControls[
                LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].Text=
                ObjTextEdit.Text;            
        }

        public static void UpdateEnteredRichTextForCurrentOnelanguage(
            RichEditControl ObjRichEditControl)
        {
            // Not added log because the method is executed continuously while typing
            dictionaryLanguagewiseRichEditControls[
                LanguageSelectedInCatelogueDescription.TwoLetterISOLanguage].HtmlText =
                ObjRichEditControl.HtmlText;
        }
    }
}
