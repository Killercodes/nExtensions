using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace nExtensions
{
    public static class StringExtended
    {

        public static void Speak(this string value)
        { 
            System.Speech.Synthesis.SpeechSynthesizer ssy = new System.Speech.Synthesis.SpeechSynthesizer();
            ssy.Speak(value);
        }
        /// <summary>
        /// Uppercase the first letter in the string.
        /// </summary>
        public static string UppercaseFirstLetter (this string value)
        { 
            if (value.Length > 0)
            {
                char[] array = value.ToCharArray();
                array[0] = char.ToUpper(array[0]);
                return new string(array);
            }
            return value;
        }

        /// <summary>
        /// Convert string in title case
        /// </summary>
        public static string TitleCase (this string value)
        {
            string result = null;
            if (value.Length > 0)
            {
                string[] words = value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    char[] array = word.ToCharArray();
                    array[0] = char.ToUpper(array[0]);
                    result += new string(array);
                }
            }
            return result;
        }

        public static bool IsAllCaps (this string inputString)
        {
            return Regex.IsMatch(inputString, @"^[A-Z]+$");
        }
        
        public static void WriteToConsole (this string value)
        {
            Console.WriteLine(value);
        }

        public static string AppendLine (this string value, string stringToAppend)
        {
            return value + stringToAppend;
        }

        public static string ToHex (this string value)
        {
            string output = null;
            char[] values = value.ToCharArray();
            foreach (char letter in values)
            {
                int ivalue = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:X}", ivalue);
                output += hexOutput;
            }
            return (output);
        }

        public static string FromHex (this string hexValues)
        {
            string output = null;
            string[] hexValuesSplit = hexValues.Split(' ');
            foreach (String hex in hexValuesSplit)
            {
                // Convert the number expressed in base-16 to an integer.
                int value = Convert.ToInt32(hex, 16);
                // Get the character corresponding to the integral value.
                string stringValue = Char.ConvertFromUtf32(value);
                char charValue = (char)value;
                output += stringValue;
            }

            return output;
        }
        public static void WriteToFile (this string value, string Filename)
        {
            File.WriteAllText(Filename, value);
        }

        public static void ReadFromFile (this string value, string Filename)
        {
            File.ReadAllText(value);
        }

        public static string Base64Encode (this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode (string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string HtmlEncode (this string value)
        {
            return (System.Net.WebUtility.HtmlEncode(value));
        }

        public static string HtmlDecode (this string value)
        {
            return (System.Net.WebUtility.HtmlDecode(value));
        }

        static string sha256 (string password)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string Decrypt (string cipherText, string Password)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
                 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));

                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return null;
            }
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        public static byte[] Decrypt (byte[] cipherData, byte[] Key, byte[] IV)
        {
            try
            {
                MemoryStream ms = new MemoryStream();

                // Create a symmetric algorithm. 
                // We are going to use Rijndael because it is strong and
                // available on all platforms.  
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                CryptoStream cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);

                // Write the data and make it do the decryption 
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
                byte[] decryptedData = ms.ToArray();

                return decryptedData;
            }
            catch
            {
                return null;
            }
        }

        public static byte[] Encrypt (byte[] clearData, byte[] Key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
              alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            cs.Close();
            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }


        // Encrypt a string into a string using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 

        public static string Encrypt (string clearText, string Password)
        {
            // First we need to turn the input string into a byte array. 
            byte[] clearBytes =
              System.Text.Encoding.Unicode.GetBytes(clearText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
              new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            byte[] encryptedData = Encrypt(clearBytes,
              pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);

        }

        public static string MD5Encrypt (string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            MD5CryptoServiceProvider md5hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = md5hasher.ComputeHash(encoder.GetBytes(phrase));

            return byteArrayToString(hashedDataBytes);
        }

        private static string byteArrayToString (byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");

            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }

            return output.ToString();
        }

        public static string GetSHA1Hash (this string filePath)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(filePath, sha1);
        }

        private static string GetHash (string filePath, HashAlgorithm hasher)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                return GetHash(fs, hasher);
        }

        public static string GetMD5Hash (Stream s)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(s, md5);
        }
        public static string GetMD5Hash (string filePath)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(filePath, md5);
        }


        public static string GetSHA1Hash (Stream s)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(s, sha1);
        }

        private static string GetHash (Stream s, HashAlgorithm hasher)
        {
            var hash = hasher.ComputeHash(s);
            var hashStr = Convert.ToBase64String(hash);
            return hashStr.TrimEnd('=');
        }
    }
}
