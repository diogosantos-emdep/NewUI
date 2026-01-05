using Emdep.Geos.UI.Adapters.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ActivityReminder.Classes
{



    public static class clsLogFile
    {


        //[pramod.misal][GEOS2-2364][9.06.2023]
        #region Properties
        public static Prism.Logging.ILoggerFacade Logger { get; set; }
        #endregion


        //[pramod.misal][GEOS2-2364][14.06.2023]
        #region Static constructor
        static clsLogFile( )
        {
            try
           {
                string logfilepath = AppDomain.CurrentDomain.BaseDirectory + @"\" + "log4net.config";

                if (!File.Exists(logfilepath))
                {
                    if (!Directory.Exists(Directory.GetParent(logfilepath).FullName))
                        Directory.CreateDirectory(Directory.GetParent(logfilepath).FullName);

                    string fullPath = AppDomain.CurrentDomain.BaseDirectory;
                    string fileName = Path.GetFileName(logfilepath);

                    File.Copy(Path.Combine(fullPath, fileName), logfilepath,true);
                }

                FileInfo file = new FileInfo(logfilepath);
                Logger = LoggerFactory.CreateLogger(LogType.Log4Net, file);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// [001][Ganaraj Chavan][27-04-2020][GSYNC-13] Do not update the summary of the issue in Jira once has been created
        /// </summary>
        /// <param name="message"></param>
        public static void WriteErrorLog(string message)
        {
            StreamWriter sw = null;

            try
            {
                RenameFileIfFull(AppDomain.CurrentDomain.BaseDirectory + "\\ActivityReminderLogFile.txt");
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ActivityReminderLogFile.txt", true);                
                sw.WriteLine(string.Format("[{0}] : {1}", DateTime.Now, message));  //  "[ " + DateTime.Now.ToString() + " ]" + " : " + " " + message);
                //sw.WriteLine("[ " + DateTime.Now.ToString() + " ]" + " : " + " " + message);
            }
            catch (IOException ex)
            {
                //clsLogFile.WriteErrorLog(string.Format("[ERROR] WriteErrorLog - Exception - {0}", ex.Message));
            }
            catch (Exception ex)
            {
                clsLogFile.WriteErrorLog(string.Format("[ERROR] WriteErrorLog - Exception - {0}", ex.Message));
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// One back temp file and delete old temp log file.
        /// <para>[000][Ganaraj Chavan][27-05-2020][GEOS2-2361] GeosWokbenchActivityReminderService service stopped then never start again.</para>
        /// </summary>
        /// <param name="fileFullName"></param>
        private static void RenameFileIfFull(string fileFullName)
        {
            if (File.Exists(fileFullName))
            {
                long length = new System.IO.FileInfo(fileFullName).Length;
                string tempFileName = fileFullName.Replace(".txt", "_tmp.txt");
                if (length > 1000000 * 10)
                {
                    if (File.Exists(tempFileName))
                    {
                        File.Delete(tempFileName);
                    }
                    File.Move(fileFullName, tempFileName);
                }
            }
        }
    }
}
