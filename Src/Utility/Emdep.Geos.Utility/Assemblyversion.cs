using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{

    /// <summary>
    /// AssemblyVersion class use for getting information of Assembly Version
    /// </summary>
    public static class AssemblyVersion
    {
        /// <summary>
        /// This method use for get assembly version information from assembly using path
        /// </summary>
        /// <param name="path">Assembly path</param>
        /// <returns>Return Version class</returns>
        public static Version GetAssemblyVersion(string path)
        {
            Version v = AssemblyName.GetAssemblyName(path).Version;
            return v;
        }

        /// <summary>
        /// This method use for get assembly version information from assembly version format  
        /// </summary>
        /// <param name="assemblVersion"></param>
        /// <returns>Return version class</returns>
        public static Version GetAssemblyVersionbyString(string assemblVersion)
        {
            return new Version(assemblVersion);
        }

        /// <summary>
        /// This method use for compare two assembly versions 
        /// </summary>
        /// <param name="firstversion">First assembly version</param>
        /// <param name="secondversion">First assembly version</param>
        /// <returns>Less than zero= The current Version object is a version before value., Zero= The current Version object is the same version as value.,Greater than zero=The current Version object is a version subsequent to value.  OR value is null.</returns>
        public static int CompareAssemblyVersions(Version firstversion, Version secondversion)
        {
            return firstversion.CompareTo(secondversion);
        }
    }
}
