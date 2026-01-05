using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// ApplicationOperation class use for getting information of application
    /// </summary>
    public static class ApplicationOperation
    {
        /// <summary>
        /// This method use for create new shortcut on Desktop
        /// </summary>
        /// <param name="linkName">Link name</param>
        /// <param name="appPath">Application path</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         ApplicationOperation.ApplicationShortcutToDesktop(linkName,appPath);
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void ApplicationShortcutToDesktop(string linkName, string appPath)
        {
             string _path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string pathexe = appPath.Replace(@"\GeosWorkbench.exe", @"\geosSoftwareIcon.ico");
            if(!System.IO.File.Exists(pathexe))
            {
                pathexe = null;
            }
            //string _path = @"C:\Users\Public\Desktop";
            if (pathexe != null)
                CreateShortcut(linkName, appPath, _path, pathexe);
            else
                CreateShortcut(linkName, appPath, _path, pathexe);

        }

        /// <summary>
        /// This method use for create new shortcut on start menu
        /// </summary>
        /// <param name="linkName">Link name</param>
        /// <param name="subfolder">Sub folder of shortcut</param>
        /// <param name="appPath">Application path</param>
        public static void ApplicationShortcutToStartMenu(string linkName, string subfolder, string appPath)
        {
            string _path = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            if (!Directory.Exists(_path + "\\" + subfolder))
            {
                Directory.CreateDirectory(_path + "\\" + subfolder);
            }
            CreateShortcut(linkName, appPath, _path + "\\" + subfolder);
        }

        /// <summary>
        /// This method use for create new shortcut in location
        /// </summary>
        /// <param name="linkName">Link name</param>
        /// <param name="appPath">Application path</param>
        /// <param name="shortcutLocation">Shortcut location</param>
        private static void CreateShortcut(string linkName, string appPath, string shortcutLocation, string iconPath = null)
        {
            try
            {
                var wsh = new IWshShell_Class();
                IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(shortcutLocation + "\\" + linkName + ".lnk") as IWshRuntimeLibrary.IWshShortcut;
                if (iconPath != null)
                    shortcut.IconLocation = iconPath;
                shortcut.TargetPath = appPath;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// This method is to get current computer name
        /// </summary>
        /// <returns>current computer name</returns>
        public static string GetCurrentComputerName()
        {
            return System.Environment.MachineName;
        }

        /// <summary>
        /// This method is to get xml file path
        /// </summary>
        /// <returns>xml file path</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///        string filePath = ApplicationOperation.GetUserSetting();
        ///        
        ///         //OUTPUT:- C:\Users\cpatil\AppData\Local\Geos\UserSetting.xml
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetUserSetting()
        {
            string _path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return _path + @"\Geos\UserSetting.xml";
        }

        /// <summary>
        /// This method is to create and write dictionary in xml file
        /// </summary>
        /// <param name="tuples">Get tuples</param>
        /// <param name="filePath">Get xml filepath</param>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          Tuple&lt;string, string&gt; tuple = new  Tuple&lt;string, string&gt;("Address", "localhost");
        ///         Tuple&lt;string, string&gt; tuple1 = new  Tuple&lt;string, string&gt;("Port", "8890");
        ///         List &lt;tuple&lt;string, string&gt;&gt; tuples = new List &lt;tuple&lt;string, string&gt;&gt;();
        ///         tuples.Add(tuple);
        ///         tuples.Add(tuple1);
        ///         ApplicationOperation.CreateNewSetting(tuples, filePath);
        ///        
        ///     }
        /// }
        /// </code>
        /// </example>

        public static void CreateNewSetting(List<Tuple<string, string>> tuples, string filePath, string fileName)
        {
            string _path = System.IO.Path.GetDirectoryName(filePath);

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(fileName);
                writer.WriteString("\n");
                foreach (Tuple<string, string> tuple in tuples)
                {
                    writer.WriteStartElement("Setting");
                    writer.WriteAttributeString("Name", tuple.Item1);
                    writer.WriteString(tuple.Item2);
                    writer.WriteEndElement();
                    writer.WriteString("\n");
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }


        }




        public static void CreateNewSetting(Dictionary<string,string> Dictionary, string filePath, string fileName)
        {
            string _path = System.IO.Path.GetDirectoryName(filePath);

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            using (XmlWriter writer = XmlWriter.Create(filePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(fileName);
                writer.WriteString("\n");
                foreach (var item in Dictionary)
                {
                    writer.WriteStartElement("Setting");
                    writer.WriteAttributeString("Name", item.Key);
                    writer.WriteString(item.Value);
                    writer.WriteEndElement();
                    writer.WriteString("\n");
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }


        }

        /// <summary>
        /// This method is to get user settings from- xml file
        /// </summary>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          
        ///           List &lt;Dictionary&lt;string, string&gt;&gt; dict = ApplicationOperation.GetSetting(filePath);
        ///        
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="filePath">Get xml file path</param>
        /// <returns>Dictionary of settings</returns>
        public static Dictionary<string, string> GetSetting(string filePath)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    XElement xelement = XElement.Load(filePath);
                    foreach (XElement el in xelement.Elements())
                        dict.Add(el.LastAttribute.Value, el.Value);
                }
            }
            catch (Exception)
            {
            }

            return dict;
        }

        /// <summary>
        /// This method is to get ip address of machine
        /// </summary>
        /// <param name="ipAddressStartFrom">Get ip adddress start from</param>
        /// <returns>Machine ip address</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///          string machineIPAddress = ApplicationOperation.GetEmdepGroupIP("10.");
        ///        
        ///         //OUTPUT:- 10.0.9.53
        ///        
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetEmdepGroupIP(string ipAddressStartFrom)
        {
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.ToString().StartsWith(ipAddressStartFrom))
                {
                    return addr.ToString();
                }
            }
            return null;
        }


        /// <summary>
        /// This method is to get access permission to file or not
        /// </summary>
        /// <param name="filePath">Get file path</param>
        /// <returns>Access permission for file or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         bool checkAccess = ApplicationOperation.HasWriteAccessToFile(@"C:\Users\username\AppData\Local\Temp");
        ///        
        ///         //OUTPUT:- true
        ///        
        ///     }
        /// }
        /// </code>
        /// </example>
        public static bool HasWriteAccessToDirectory(string filePath)
        {
            string getAccess = null;
            bool chkAccess = false;
            try
            {

                DirectoryInfo myDirectoryInfo = new DirectoryInfo(filePath);
                chkAccess = CurrentUserSecurity.HasAccess(myDirectoryInfo, FileSystemRights.FullControl);

                return chkAccess;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        /// <summary>
        /// This method is to get space available in drive or not 
        /// </summary>
        /// <param name="filePath">Get file path</param>
        /// <param name="fileSize">Get file Size</param>
        /// <returns>Space available or not</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         bool checkAccess = ApplicationOperation.HasSpaceAvaiableInDrive(@"C:\Users\username\AppData\Local\Temp");
        ///        
        ///         //OUTPUT:- true
        ///        
        ///     }
        /// }
        /// </code>
        /// </example>
        public static bool HasSpaceAvaiableInDrive(string filePath, long? fileSize)
        {
            try
            {
                DriveInfo driveTemp = new DriveInfo(Path.GetPathRoot(filePath));

                if (!(driveTemp.TotalFreeSpace - fileSize > 0))
                {
                    return false;
                }
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (DriveNotFoundException)
            {
                return false;
            }
        }
    }
}
