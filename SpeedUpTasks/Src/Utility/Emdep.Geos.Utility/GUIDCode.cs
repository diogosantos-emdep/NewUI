using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// GUIDCode class use for getting GUIDCode 
    /// </summary>
    public class GUIDCode
    {
        /// <summary>
        /// This method is to get GUID string in 32 digit
        /// </summary>
        /// <returns>GUID string in 32 digit</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///        string GUIDCodeString = GUIDCode.GUIDCodeString();
        ///        
        ///        OUTPUT :- 9e2f0352-32434836-9107c0599f48b13b.76927308
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GUIDCodeString()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            String guid = Guid.NewGuid().ToString() + "." + myRandomNo.ToString();

            string sha2 = System.Guid.NewGuid().ToString().Replace("-", "");

            string sha3;
            string sha4 = "";
            int a = 0;
            for (int i = 0; i < 2; i++)
            {
                sha3 = sha2.Substring(a, 8) + "-";
                sha4 = sha4 + sha3;
                a = a + 8;
                if (a >= 16)
                {
                    sha3 = sha2.Substring(a, 16);
                    sha4 = sha4 + sha3 + "." + myRandomNo.ToString();
                }
            }
            return sha4;
        }
    }
}
