using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// ProcessControl class use controlling all running processes in system 
    /// </summary>
    public static class ProcessControl
    {

        /// <summary>
        /// This method use for process is running on system or not 
        /// </summary>
        /// <param name="processname">Process name </param>
        /// <returns>If return true then process is running otherwise </returns>
        public static bool IsProcessStart(string processname)
        {
            Process[] procs = Process.GetProcessesByName(processname);
            if (procs.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// This method is use to start process in system using process path 
        /// </summary>
        /// <param name="filepath">Process path</param>
        /// <param name="argument">Process argument</param>
        public static void ProcessStart(string filepath, string argument, string Verb = "")
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(filepath);
            if (!string.IsNullOrEmpty(Verb))
            {
                startInfo.Verb = Verb;
            }

            startInfo.Arguments = argument;
            startInfo.UseShellExecute = false;
            System.Diagnostics.Process.Start(startInfo);

        }

        /// <summary>
        /// This method is killing single or all running process in system using process name
        /// </summary>
        /// <param name="processname">Process name</param>
        /// <param name="allprocesskill">If use sent true then all processes in system is killed using process name and otherwise only single</param>
        public static void ProcessKill(string processname, bool allprocesskill = false)
        {
            Process[] procs = Process.GetProcessesByName(processname);
            if (allprocesskill == true)
            {
                foreach (Process proc in procs)
                    proc.Kill();
            }
            else
            {
                procs[0].Kill();
            }
            procs = null;
        }

    }
}
