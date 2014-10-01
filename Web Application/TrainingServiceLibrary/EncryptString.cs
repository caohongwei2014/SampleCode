using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrainingServiceLibrary
{
    public class EncryptString
    {

        public static String Encrypt(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //AesCryptoServiceProvider aesCSP = new AesCryptoServiceProvider();
            //aesCSP.GenerateKey();
            //aesCSP.GenerateIV();
            byte[] Key = { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8 };
            ICryptoTransform encryptor = des.CreateEncryptor(Key, IV);

            //ICryptoTransform encryptor = aesCSP.CreateEncryptor();
            try
            {
                byte[] IDToBytes = UnicodeEncoding.Unicode.GetBytes(source);
                //byte[] IDToBytes = ASCIIEncoding.ASCII.GetBytes(source);
                byte[] encryptedID = encryptor.TransformFinalBlock(IDToBytes, 0, IDToBytes.Length);
                return Convert.ToBase64String(encryptedID);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Decrypt(string encrypted)
        {
            byte[] Key = { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8 };
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform decryptor = des.CreateDecryptor(Key, IV);
            try
            {
                byte[] encryptedIDToBytes = Convert.FromBase64String(encrypted);
                byte[] IDToBytes = decryptor.TransformFinalBlock(encryptedIDToBytes, 0, encryptedIDToBytes.Length);
                //return ASCIIEncoding.ASCII.GetString(IDToBytes);
                return UnicodeEncoding.Unicode.GetString(IDToBytes);
            }
            catch (FormatException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        } 
    }
}
