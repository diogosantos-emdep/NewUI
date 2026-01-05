using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
//Save this file as Emdep.Geos.Utility.cs
// Compile with: csc Emdep.Geos.Utility.cs /doc:Results.xml 
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// This class is to get filepath in SHA1Util format.
    /// </summary>
    public class SHA1Util 
    {
        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="filePath">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         string SHA1UtilText = SHA1Util.GetSha1Hash("C:\xampp\htdocs\glpi\files\_tmp\a6415b1e-321a4b3a-aaa0f7a05eadf44b.81953648\IMG_22122015_142444 - Copy.png");
        ///         
        ///         OUTPUT :- cfc144d12defe9331883218aab758ceb98c95aee
        ///     }
        /// }
        /// </code>
        /// </example>
       public static string GetSha1Hash(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                SHA1 sha = new SHA1Managed();
                return BitConverter.ToString(sha.ComputeHash(fs)).ToLower().Replace("-","").ToString();
            }
        }

        
    }
}
