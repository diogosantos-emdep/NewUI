using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
// Save this file as Emdep.Geos.Utility.cs
// Compile with: csc Emdep.Geos.Utility.cs /doc:Results.xml 
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility
{
    /// <summary>
    /// This class is to encrypt param.
    /// </summary>
    public class Encrypt
    {
        /// <summary>
        /// This method is to get encrypted password
        /// </summary>
        ///<example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///        string EncryptedText = Encrypt.Encryption("12");
        ///        
        ///        OUTPUT :- c20ad4d76fe97759aa27a0c99bff6710
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="password">string to encrypt</param>
        /// <returns>encrypted string</returns>
        
        public static string Encryption(string password)
        {
            string hash = null;
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                hash = sBuilder.ToString();
            }
            return hash;
        }

    }
}
