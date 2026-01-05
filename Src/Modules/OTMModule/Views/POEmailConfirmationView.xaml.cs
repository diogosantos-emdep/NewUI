using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Ribbon;
using Emdep.Geos.Modules.OTM.ViewModels;
using DevExpress.XtraPrinting;
using System.Text.RegularExpressions;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.DataProcessing;
using DevExpress.Xpf.RichEdit;
using DevExpress.Mvvm.Native;
using static System.Net.WebRequestMethods;
using Emdep.Geos.Modules.OTM.CommonClass;

namespace Emdep.Geos.Modules.OTM.Views
{
    /// <summary>
    /// Interaction logic for POEmailConfirmationView.xaml
    /// </summary>
    public partial class POEmailConfirmationView : WinUIDialogWindow
    {
        public POEmailConfirmationView()
        {
            InitializeComponent();
           

        }

        private void richEdit_Loaded(object sender, RoutedEventArgs e)
        {
            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "0")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("This is an automatically generated email, so please DO NOT reply") && !paragraphText.Contains("Before printing this") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                                && !paragraphText.Contains("© 2006 GEOS. All rights reserved.") && !paragraphText.Contains("Before printing this email, consider your contribution to the conservation of the Environment by reducing paper consumption"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }
           
        }

        private void richEdit_DocumentLoaded(object sender, EventArgs e)
        {

            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "0")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("This is an automatically generated email, so please DO NOT reply") && !paragraphText.Contains("Before printing this") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                                && !paragraphText.Contains("© 2006 GEOS. All rights reserved.") && !paragraphText.Contains("Before printing this email, consider your contribution to the conservation of the Environment by reducing paper consumption"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }
            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex =="1")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("Este es un correo electrónico generado automáticamente, así que NO responda a este mensaje. Esto es sólo para su información.") && !paragraphText.Contains("Antes de imprimir este correo electrónico, considere su contribución a la conservación del medio ambiente reduciendo el consumo de papel.") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                                && !paragraphText.Contains("Copiar; 2006 GEOS. Reservados todos los derechos.") && !paragraphText.Contains("Antes de imprimir este correo electrónico, considere su contribución a la conservación del medio ambiente reduciendo el consumo de papel.")
                                && !paragraphText.Contains("Reservados todos los derechos"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }

            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "2")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("Il s'agit d'un e-mail généré automatiquement, veuillez donc NE PAS répondre à ce message. Ceci est uniquement pour votre information.") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                            && !paragraphText.Contains("Tous droits réservés") && !paragraphText.Contains("Avant d'imprimer cet e-mail, réfléchissez à votre contribution à la conservation de l'environnement en réduisant la consommation de papier."))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }

            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "3")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("Este é um e-mail gerado automaticamente, por isso NÃO responda a esta mensagem. Isto é apenas para sua informação.") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                            && !paragraphText.Contains("Todos os direitos reservados.") && !paragraphText.Contains("Antes de imprimir este e-mail, considere o seu contributo para a conservação do ambiente, reduzindo o consumo de papel"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }

            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "4")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("Acesta este un e-mail generat automat, așa că vă rugăm să NU răspundeți la acest mesaj. Acest lucru este doar pentru informarea dvs") && !paragraphText.Contains("Înainte de a imprima acest e-mail, luați în considerare contribuția dvs. la conservarea mediului prin reducerea consumului de hârtie.") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                                && !paragraphText.Contains("2006 GEOS. Toate drepturile rezervate.") && !paragraphText.Contains("Înainte de a imprima acest e-mail, luați în considerare contribuția dvs. la conservarea mediului prin reducerea consumului de hârtie."))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }

            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "5")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("Это электронное письмо создано автоматически, поэтому, пожалуйста, НЕ отвечайте на это сообщение. Это только для вашей информации.") && !paragraphText.Contains("Прежде чем распечатать это письмо, подумайте о своем вкладе в сохранение окружающей среды за счет сокращения потребления бумаги.") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                                && !paragraphText.Contains("2006 ГЕОС. Все права защищены.") && !paragraphText.Contains("Прежде чем распечатать это письмо, подумайте о своем вкладе в сохранение окружающей среды за счет сокращения потребления бумаги."))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }

            if (OTMCommon.Instance.SettingWindowLanguageSelectedIndex == "6")
            {
                var richEditServer = sender as RichEditControl;
                // Begin updating the document
                richEditServer.Document.BeginUpdate();
                // Check if the document is already protected
                if (!richEditServer.Document.IsDocumentProtected)
                {
                    DevExpress.XtraRichEdit.API.Native.RangePermissionCollection rangePermissions = richEditServer.Document.BeginUpdateRangePermissions();
                    DevExpress.XtraRichEdit.API.Native.RangePermission rpFullDocument = rangePermissions.CreateRangePermission(richEditServer.Document.Range);
                    rpFullDocument.Group = ""; // Default group (all users)
                    rangePermissions.Add(rpFullDocument);
                    // Find the "Objective" paragraph by its ID
                    foreach (DevExpress.XtraRichEdit.API.Native.Section section in richEditServer.Document.Sections)
                    {
                        //foreach (DevExpress.XtraRichEdit.API.Native.lin paragraph in section.LineNumbering)
                        //{

                        //}
                        foreach (DevExpress.XtraRichEdit.API.Native.Paragraph paragraph in section.Paragraphs)
                        {// © 2006 GEOS. All rights reserved.
                            string paragraphText = richEditServer.Document.GetText(paragraph.Range).Trim();

                            if (!((DevExpress.XtraRichEdit.API.Native.Implementation.NativeDocumentRange)paragraph.Range).End.PieceTable.TextBuffer.ToString().Contains("www"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                            if (paragraphText.Trim() != string.Empty && !paragraphText.Contains("這是一封自動產生的電子郵件，因此請不要回覆此訊息。這僅供您參考。") && !paragraphText.Contains("在列印此電子郵件之前，請考慮您透過減少紙張消耗對保護環境所做的貢獻。") && !paragraphText.Contains("https://api.emdep.com/images/logo-geos.png") && !paragraphText.Contains("https://ecos.emdep.com/images/logo-emdep.png")
                                && !paragraphText.Contains("2006 年地球觀測。版權所有。") && !paragraphText.Contains("在列印此電子郵件之前，請考慮您透過減少紙張消耗對保護環境所做的貢獻。"))
                            {
                                // Create a range permission to make this paragraph read-only
                                DevExpress.XtraRichEdit.API.Native.RangePermission rpParagraph = rangePermissions.CreateRangePermission(paragraph.Range);
                                rpParagraph.Group = "Everyone"; // Allow everyone to read this section, but not edit
                                rpParagraph.UserName = ""; // User-independent permissions
                                richEditServer.Document.DefaultCharacterProperties.BackColor = System.Drawing.Color.Transparent;
                                paragraph.Style.HighlightColor = (System.Drawing.Color)System.Drawing.Color.Transparent;
                                // End updating range permissions
                                richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                                // Add the range permission to the collection
                                rangePermissions.Add(rpParagraph);
                            }
                        }
                    }
                    // End updating range permissions
                    richEditServer.Document.EndUpdateRangePermissions(rangePermissions);
                }
                // Apply document protection (read-only mode for the whole document)
                richEditServer.Document.Protect("YourPassword", DevExpress.XtraRichEdit.API.Native.DocumentProtectionType.ReadOnly);
                richEditServer.ClearUndo();
                // End document update
                richEditServer.Document.EndUpdate();

            }


        }
    }
}
