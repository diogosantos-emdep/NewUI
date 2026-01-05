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

namespace Emdep.Geos.Modules.Warehouse.Common_Classes
{
    public class PrintLabel
    {
        #region TaskLog
        //[WMS M049-16][Print Label in picking][adadibathina]
        #endregion

        #region Services

        IGeosRepositoryService GeosRepositoryService = new GeosRepositoryServiceController(GeosApplication.Instance.ApplicationSettings["ServicePath"].ToString());

        #endregion

        #region Declaration
        private Dictionary<string, string> printValues = new Dictionary<string, string>();
        // public string ServerIp { get; set; }


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

        /// <summary>
        /// To print a file
        /// </summary>
        /// <param name="filePath"></param>
        [STAThread]
        public bool IsPrinterOnline()
        {
            GeosApplication.Instance.Logger.Log(" PrintLabel Method IsPrinterOnline....", category: Category.Info, priority: Priority.Low);
            // Set management scope
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
                if (GeosApplication.Instance.UserSettings["LabelPrinter"].Contains("\\"))
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

                if ((GeosApplication.Instance.UserSettings["LabelPrinter"] == string.Empty))
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("LabelPrinterNotFound").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }
                if ((GeosApplication.Instance.UserSettings["LabelPrinterModel"] == string.Empty))
                {
                    CustomMessageBox.Show(string.Format(System.Windows.Application.Current.FindResource("PrinterModelNotfond").ToString()), "Red", CustomMessageBox.MessageImagePath.NotOk, MessageBoxButton.OK);
                    return;
                }

                if ((GeosApplication.Instance.UserSettings["ParallelPort"] == string.Empty))
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



        /// <summary>
        /// CreateBatFileFile for shared and local conected printers
        /// </summary>
        /// <returns></returns>
        public string CreateBatFile()
        {
            string batFile = string.Empty;
            string labelPrinterSharedName = GeosApplication.Instance.LabelPrinterSharedName;
            string labelPrinter = GeosApplication.Instance.LabelPrinter;
            GeosApplication.Instance.Logger.Log("PrintLabel Method CreateBatFileFile....", category: Category.Info, priority: Priority.Low);
            try
            {
                batFile = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "Scripts\\Printing.bat");
                // return @"COPY D:\_tmp.prn /B \\EPINRDO36\ZDesigner GX420t";
                //if the printer is shared cmd is net use NET USE LPT9: \\MachineName\PrinterName
                //if shared printer is by ip cmd is net use lpt9 : "\\SystemIp\sharedName"
                if (IsSharedPrinter)
                {
                    labelPrinter = labelPrinter.Replace("\\\\", "");
                    IPAddress SharedPrinterIp;
                    IPAddress.TryParse(labelPrinter.Substring(0, labelPrinter.IndexOf("\\")), out SharedPrinterIp);
                    if (SharedPrinterIp == null)
                    {
                        // string sharedName = GeosApplication.Instance.GetSharedNameOfPrinter(GeosApplication.Instance.UserSettings["LabelPrinter"]);
                        batFile = batFile.Replace("\"", "");
                        batFile = batFile.Replace("@SelectedPrinterIp", labelPrinter.Substring(0, labelPrinter.IndexOf("\\")));
                        batFile = batFile.Replace("@SharedPrinterName", labelPrinterSharedName);
                    }
                    else
                    {
                        //  string sharedName = GeosApplication.Instance.GetSharedNameOfPrinter(GeosApplication.Instance.UserSettings["LabelPrinter"]);
                        batFile = batFile.Replace("@SelectedPrinterIp", SharedPrinterIp.ToString());
                        batFile = batFile.Replace("@SharedPrinterName", labelPrinterSharedName);
                    }

                }
                else
                {  //if the printer is Notshared cmd is net use NET USE LPT9: "\\Localip\PrintersSharedName"
                   //   string sharedName = GeosApplication.Instance.GetSharedNameOfPrinter(GeosApplication.Instance.UserSettings["LabelPrinter"]);
                    batFile = batFile.Replace("@SelectedPrinterIp", GeosApplication.Instance.SystemIp.ToString());
                    batFile = batFile.Replace("@SharedPrinterName", labelPrinterSharedName);
                }

                batFile = batFile.Replace("@LPT", GeosApplication.Instance.UserSettings["ParallelPort"]);
            }
            catch (Exception ex)
            {
                GeosApplication.Instance.Logger.Log("Get an error in PrintLabel Method CreateBatFileFile...." + ex.Message, category: Category.Exception, priority: Priority.Low);
            }

            GeosApplication.Instance.Logger.Log("Method CreateBatFileFile executed successfully", category: Category.Info, priority: Priority.Low);
            return batFile;
        }

        /// <summary>
        /// convert a string to barcode
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string SplitStringForBarcode(string s)
        {
            GeosApplication.Instance.Logger.Log("PrintLabel Method SplitStringForBarcode....", category: Category.Info, priority: Priority.Low);
            string numbers = ">5";
            string letters = ">6";
            string code = string.Empty;
            bool firstNumber = false;

            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsNumber(s[i]) && (!firstNumber))
                {
                    string tempCode = string.Empty;

                    for (int j = i; j < s.Length; j++)
                    {

                        if (char.IsNumber(s[j]))
                            tempCode += s[j];
                        else
                        {
                            int notPair = 0;

                            if (tempCode.Length % 2 != 0)
                                notPair = 1;

                            if (tempCode.Length - notPair != 0)
                            {
                                code += numbers;
                                for (int x = 0; x < tempCode.Length - notPair; x++)
                                    code += tempCode[x];
                            }
                            else
                                firstNumber = true;

                            i += (tempCode.Length - 1) - notPair;
                            break;

                        }

                        if (j == s.Length - 1)
                        {
                            int notPair = 0;

                            if (tempCode.Length % 2 != 0)
                                notPair = 1;

                            code += numbers + tempCode;

                            if (notPair == 1)
                            {
                                code += letters;
                                for (int x = tempCode.Length - 1; x < tempCode.Length; x++)
                                    code += tempCode[x];
                            }
                            i = s.Length - 1;
                        }
                    }
                }
                else
                {
                    firstNumber = false;
                    int charNumbers = 0;
                    string tempCode = string.Empty;

                    for (int j = i; j < s.Length; j++)
                    {
                        tempCode += s[j];

                        if (char.IsNumber(s[j]))
                            charNumbers++;
                        else
                        {
                            if (charNumbers > 3)
                            {
                                if (charNumbers % 2 != 0)
                                    charNumbers -= 1;

                                code += letters;
                                for (int x = 0; x < (tempCode.Length - 1) - charNumbers; x++)
                                    code += tempCode[x];

                                code += numbers;
                                for (int x = (tempCode.Length - 1) - charNumbers; x < tempCode.Length - 1; x++)
                                    code += tempCode[x];

                                i += tempCode.Length - 2;
                                break;

                            }
                            else
                                charNumbers = 0;
                        }

                        if (j == s.Length - 1)
                        {
                            if (charNumbers % 2 != 0)
                                charNumbers -= 1;

                            code += letters;
                            for (int x = 0; x < tempCode.Length - charNumbers; x++)
                                code += tempCode[x];

                            if (charNumbers > 0)
                            {
                                code += numbers;
                                for (int x = tempCode.Length - charNumbers; x < tempCode.Length; x++)
                                    code += tempCode[x];
                            }

                            i = s.Length - 1;
                        }
                    }
                }
            }

            GeosApplication.Instance.Logger.Log("PrintLabel Method SplitStringForBarcode executed successfully", category: Category.Info, priority: Priority.Low);
            return code;
        }

        #endregion

    }
}
