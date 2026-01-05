using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Emdep.Geos.UI.Common;
using Emdep.Geos.UI.ServiceProcess;
using Emdep.Geos.Services.Contracts;
using System.ServiceModel;
using DevExpress.Xpf.Core;
using Emdep.Geos.UI.CustomControls;
using Prism.Logging;
using System.Windows;
using System.Runtime.InteropServices;
using System.Management;
using System.Net;

namespace Emdep.Geos.Modules.SAM.CommonClasses
{
	//[nsatpute][04-09-2024][GEOS2-5415]
    public class PrintLabel
    {
        #region Declaration

        private Dictionary<string, string> printValues = new Dictionary<string, string>();
        byte[] PrintFile;
        public bool IsSharedPrinter { get; set; }

        #endregion

        #region Constructor

        public PrintLabel(Dictionary<string, string> printValues, byte[] printFile)
        {
            this.printValues = printValues;
            this.PrintFile = printFile;
        }

        #endregion

        #region Methods

        [STAThread]
        public bool IsPrinterOnline()
        {
            GeosApplication.Instance.Logger.Log(" PrintLabel Method IsPrinterOnline....", category: Category.Info, priority: Priority.Low);
            // Set mancopagement scope
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            string printerName = "";
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(GeosApplication.Instance.UserSettings["LabelPrinter"].ToLower()))
                {
                    // Console.WriteLine("Printer = " + printer["Name"]);
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        GeosApplication.Instance.Logger.Log(" PrintLabel Method IsPrinterOnline() printer is not working", category: Category.Info, priority: Priority.Low);
                        return false;
                    }
                    else
                    {
                        // printer is not offline
                        GeosApplication.Instance.Logger.Log(" PrintLabel Method IsPrinterOnline() printer is working", category: Category.Info, priority: Priority.Low);
                        return true;
                    }
                }
            }
            GeosApplication.Instance.Logger.Log(" PrintLabel Method IsPrinterOnline() executed successfully", category: Category.Info, priority: Priority.Low);
            return true;
        }

        public void Print()
        {
            string fileContent;
            string user = string.Empty;
            string batFile = string.Empty;

            try
            {
                GeosApplication.Instance.Logger.Log("PrintLabel Method Print....", category: Category.Info, priority: Priority.Low);
                //eliminem tots els fitxers temporals que hagin quedat sense borrar d'altres ocasions
                if (GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"].Contains("\\"))
                    IsSharedPrinter = true;


                string tempDirectory = Path.Combine(Path.GetTempPath(), "Emdep");
                Directory.CreateDirectory(tempDirectory);

                string[] tmpFiles = System.IO.Directory.GetFiles(tempDirectory + "\\", "_*");
                for (int i = 0; i < tmpFiles.Length; i++)
                {
                    System.IO.File.Delete(tmpFiles[i]);
                }

                Encoding encode = Encoding.GetEncoding("windows-1252");

                //if (!IsPrinterOnline())
                //{
                //    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LabelPrinterNotFound").ToString()), Application.Current.Resources["PopUpNotifyColor"].ToString(), CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                //    return;
                //}

                if ((GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"] == string.Empty))
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LabelPrinterNotFound").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if ((GeosApplication.Instance.UserSettings["SAM_LabelPrinterModel"] == string.Empty))
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PrinterModelNotfond").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                if ((GeosApplication.Instance.UserSettings["SAM_ParallelPort"] == string.Empty))
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("ParallelPortNotfond").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if (PrintFile == null)
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PrintFileNotFound").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                fileContent = encode.GetString(PrintFile);
                foreach (var printValue in printValues)
                {
                    fileContent = fileContent.Replace(printValue.Key.ToString(), printValue.Value);
                }
                batFile = CreateBatFile();
                System.IO.File.WriteAllText(tempDirectory + "\\_tmp.prn", fileContent, encode);
                System.IO.File.WriteAllText(tempDirectory + "\\_TempPrinting.bat", batFile);
                Process myProcess = new Process();
                myProcess.StartInfo.WorkingDirectory = tempDirectory.ToString();
                myProcess.StartInfo.FileName = tempDirectory + "\\_TempPrinting.bat";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.StartInfo.Arguments = "_tmp.prn";
                myProcess.Start();
                myProcess.WaitForExit();
                System.IO.File.Delete(tempDirectory + "\\_tmp.prn");
                System.IO.File.Delete(tempDirectory + "\\_TempPrinting.bat");

                GeosApplication.Instance.Logger.Log("PrintLabel Method Print() executed successfully", category: Category.Info, priority: Priority.Low);
            }
            catch (FaultException<ServiceException> ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DevExpress.Xpf.Core.DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Print() Method " + ex.Detail.ErrorMessage, category: Category.Exception, priority: Priority.Low);
                CustomMessageBox.Show(GeosApplication.Instance.ExceptionHandlingOperationString(ex.Detail, null), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
            }
            catch (ServiceUnexceptedException ex)
            {
                if (DXSplashScreen.IsActive && !GeosApplication.Instance.IsLoadOneTime) { DXSplashScreen.Close(); }
                GeosApplication.Instance.Logger.Log("Get an error in Print() Method - ServiceUnexceptedException " + ex.Message, category: Category.Exception, priority: Priority.Low);
                GeosApplication.Instance.ExceptionHandlingOperation(ex.ExceptionType, GeosApplication.Instance.ApplicationSettings["ServiceProviderIP"].ToString(), null);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method Print...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }
            GeosApplication.Instance.Logger.Log("Method Print executed successfully", category: Category.Info, priority: Priority.Low);
        }

        public string CreateBatFile()
        {
            string batFile = string.Empty;
            GeosApplication.Instance.Logger.Log("PrintLabel Method CreateBatFileFile....", category: Category.Info, priority: Priority.Low);
            try
            {
                string labelPrinterSharedName = GetSharedNameOfPrinter(GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"]);
                string labelPrinter = GeosApplication.Instance.UserSettings["SAM_DefaultLabelPrinter"];

                batFile = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\Printing.bat");
                if (IsSharedPrinter)
                {
                    labelPrinter = labelPrinter.Replace("\\\\", "");
                    IPAddress SharedPrinterIp;
                    IPAddress.TryParse(labelPrinter.Substring(0, labelPrinter.IndexOf("\\")), out SharedPrinterIp);
                    if (SharedPrinterIp == null)
                    {
                        batFile = batFile.Replace("\"", "");
                        batFile = batFile.Replace("@SelectedPrinterIp", labelPrinter.Substring(0, labelPrinter.IndexOf("\\")));
                        batFile = batFile.Replace("@SharedPrinterName", labelPrinterSharedName);
                    }
                    else
                    {
                        batFile = batFile.Replace("@SelectedPrinterIp", SharedPrinterIp.ToString());
                        batFile = batFile.Replace("@SharedPrinterName", labelPrinterSharedName);
                    }

                }
                else
                { 
                    batFile = batFile.Replace("@SelectedPrinterIp", GeosApplication.Instance.SystemIp.ToString());
                    batFile = batFile.Replace("@SharedPrinterName", labelPrinterSharedName);
                }

                batFile = batFile.Replace("@LPT", GeosApplication.Instance.UserSettings["SAM_ParallelPort"]);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintLabel Method CreateBatFileFile...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method CreateBatFileFile executed successfully", category: Category.Info, priority: Priority.Low);
            return batFile;
        }

        private string GetSharedNameOfPrinter(string printerName)
        {
            string sharedName = string.Empty;
            GeosApplication.Instance.Logger.Log(" GeosApplication Method GetSharedNameOfPrinter....", category: Category.Info, priority: Priority.Low);
            try
            {
                var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                foreach (var printer in printerQuery.Get())
                {
                    var name = printer.GetPropertyValue("Name");
                    if (name.ToString().Equals(printerName))
                    {
                        sharedName = printer.GetPropertyValue("ShareName").ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in Method GetSharedNameOfPrinter...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log(" GeosApplication Method GetSharedNameOfPrinter executed successfully", category: Category.Info, priority: Priority.Low);
            return sharedName;
        }
        #endregion


    }
}
