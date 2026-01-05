using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
// Save this file as Emdep.Geos.Utility.cs
// Compile with: csc Emdep.Geos.Utility.cs /doc:Results.xml
/// <summary>
/// Emdep.Geos.Utility namespace is use for getting Utility related information
/// </summary>
namespace Emdep.Geos.Utility.Text
{
    /// <summary>
    /// This class is to get datetime in datetimeformat.
    /// </summary>
    public class TextUtility
    {
       
        /// <summary>
        /// This method is to get date time in text format(Eg:-DateTime-10 hours ago/18 hours ago)
        /// </summary>
        /// <param name="dt">Get Date in DateTime format</param>
        /// <returns>Date Time in Text Format</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         DateTime dt = System.DateTime.Now;
        ///         string DateTimeText = TextUtility.GetDateTimeTextFormat(dt);
        ///         
        ///         OUTPUT:- just now
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetDateTimeTextFormat(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        /// <summary>
        /// This method is to check password strength
        /// </summary>
        /// <param name="password">Get password to check</param>
        /// <returns>Get password score i.e blank,veryweak,weak,medium,strong,very strong</returns>
        /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         
        ///         string checkstrength = TextUtility.CheckPasswordStrength("123456789@").ToString();
        ///         if(checkstrength == PasswordScore.Blank.ToString())
        ///         {
        ///               Console.WriteLine("Blank");
        ///         }
        ///         else if (checkstrength == PasswordScore.VeryWeak.ToString())
        ///         {
        ///            Console.WriteLine("VeryWeak");
        ///         }
        ///         else if (checkstrength == PasswordScore.Weak.ToString())
        ///         {
        ///             Console.WriteLine("Weak");
        ///         }
        ///         else if (checkstrength == PasswordScore.Medium.ToString())
        ///         {
        ///            Console.WriteLine("Medium");
        ///         }
        ///         else if (checkstrength == PasswordScore.Strong.ToString())
        ///         {
        ///            Console.WriteLine("Strong");
        ///         }
        ///         else if (checkstrength == PasswordScore.VeryStrong.ToString())
        ///         {
        ///            Console.WriteLine("VeryStrong");
        ///         }
        ///         Console.ReadLine();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static PasswordScore CheckPasswordStrength(string password)
        {
            int score = 0;
            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))   //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript)) //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;
            return (PasswordScore)score;
        }


        /// <summary>
        /// Method for check required character in Password
        /// </summary>
        /// <param name="newPassword">Get password</param>
        /// <param name="dictLanguage">Get dictLanguage as per lanugage resource for key</param>
        /// <returns>Get list of message required character in password </returns>
       /// <example>
        /// This sample shows how to call the method
        /// <code>
        /// class TestClass 
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         Dictionary<string,string> dictLanguage;
        ///         List<string> requiredCharacterInPassword = TextUtility.CheckRequiredCharacterInPassword(NewPassword ,dictLanguage);
        ///         
        ///         Console.ReadLine();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static List<string> CheckRequiredCharacterInPassword(string newPassword,Dictionary<string,string> dictLanguage)
        {
            //You can set these from your custom service methods
            int minLen = 8;
            int minDigit = 1;
            int minSpChar = 1;
            int minLower = 1;
            int minUpper = 1;
            List<string> requiredCharacterInPassword = new List<string>();
           
            //Check for password length
            if (newPassword.Length < minLen)
            {
                requiredCharacterInPassword.Add(dictLanguage.FirstOrDefault(x => x.Key == "minLen").Value);
            }

            //Check for Digits and Special Characters
            int digitCount = 0;
            int splCharCount = 0;
            int lowerCaseCharCount = 0;
            int UpperCaseCharCount = 0;
            foreach (char passwordChar in newPassword)
            {
                if (char.IsDigit(passwordChar)) digitCount++;
                if (char.IsLower(passwordChar)) lowerCaseCharCount++;
                if (char.IsUpper(passwordChar)) UpperCaseCharCount++;
                if (Regex.IsMatch(passwordChar.ToString(), @"[!#$%&'()*+,-.:;<=>?@[\\\]{}^_`|~]")) splCharCount++;

            }

            if (digitCount < minDigit)
            {
                requiredCharacterInPassword.Add(dictLanguage.FirstOrDefault(x => x.Key == "minDigit").Value);
            }

            if (lowerCaseCharCount < minLower)
            {
                requiredCharacterInPassword.Add(dictLanguage.FirstOrDefault(x => x.Key == "minLower").Value);
            }

            if (UpperCaseCharCount < minUpper)
            {
                requiredCharacterInPassword.Add(dictLanguage.FirstOrDefault(x => x.Key == "minUpper").Value);
            }

            if (splCharCount < minSpChar)
            {
                requiredCharacterInPassword.Add(dictLanguage.FirstOrDefault(x => x.Key == "minSpChar").Value);
            }

            return requiredCharacterInPassword;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }
}
