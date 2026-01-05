using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class Helper
    {
        public static string EncodeBase64(string value)
        {
            return System.Text.Encoding.UTF8.EncodeBase64(value);
        }
        public static string DecodeBase64(string value)
        {
            return System.Text.Encoding.UTF8.DecodeBase64(value);
        }
    }
    public static class EncodingForBase64
    {
        public static string EncodeBase64(this System.Text.Encoding encoding, string text)
        {
            if (text == null)
            {
                return null;
            }

            byte[] textAsBytes = encoding.GetBytes(text);
            return System.Convert.ToBase64String(textAsBytes);
        }

        public static string DecodeBase64(this System.Text.Encoding encoding, string encodedText)
        {
            if (encodedText == null)
            {
                return null;
            }

            byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
            return encoding.GetString(textAsBytes);
        }
    }
}
