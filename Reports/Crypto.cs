using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Reports
{
    public class Crypto
    {
        private static bool Initialized = false;
        private static byte[] Key;
        private static byte[] IV;
        private static Random r = new Random();
        private bool UseSalt;

        public Crypto(bool useSalt)
        {
            if (!Initialized)
            {
                Initialized = true;
                string strKeyPhrase = @"Apple*Crisp7";
                byte[] byteKeyPhrase = Encoding.ASCII.GetBytes(strKeyPhrase);
                SHA256Managed sha256 = new SHA256Managed();
                sha256.ComputeHash(byteKeyPhrase);
                Key = new byte[24];
                for (int i = 0; i < 24; i++)
                    Key[i] = sha256.Hash[i];

                string strIVPhrase = @"Peach%Pie5";
                byte[] byteIVPhrase = Encoding.ASCII.GetBytes(strIVPhrase);
                sha256.ComputeHash(byteIVPhrase);
                IV = new byte[16];
                for (int i = 0; i < 16; i++)
                    IV[i] = sha256.Hash[i];
                sha256.Clear();
            }
            UseSalt = useSalt;
        }

        public string Encrypt(string strDecryptedText)
        {
            if (strDecryptedText == null || strDecryptedText.Equals(""))
                return strDecryptedText;
            string strSalt = "";
            if (UseSalt)
            {
                for (int i = 0; i < 8; i++)
                    strSalt += r.Next(10);
            }
            strDecryptedText = strSalt + strDecryptedText;
            string strEncrpytedText = null;
            RijndaelManaged rijndael = new RijndaelManaged();
            ICryptoTransform encryptor = rijndael.CreateEncryptor(Key, IV);
            MemoryStream memStream = new MemoryStream();
            CryptoStream encryptStream = null;
            try
            {
                encryptStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
                byte[] byteDecryptedText = Encoding.UTF8.GetBytes(strDecryptedText);
                encryptStream.Write(byteDecryptedText, 0, byteDecryptedText.Length);
                encryptStream.FlushFinalBlock();
                strEncrpytedText = Convert.ToBase64String(memStream.ToArray());
            }
            finally
            {
                if (rijndael != null) rijndael.Clear();
                if (encryptor != null) encryptor.Dispose();
                if (memStream != null) memStream.Close();
                if (encryptStream != null) encryptStream.Close();
            }
            return strEncrpytedText;
        }

        public string Decrypt(string strEncryptedText)
        {
            if (strEncryptedText == null || strEncryptedText.Equals(""))
                return strEncryptedText;
            string strDecryptedText = null;
            RijndaelManaged rijndael = new RijndaelManaged();
            ICryptoTransform decryptor = rijndael.CreateDecryptor(Key, IV);
            byte[] byteEncryptedText = Convert.FromBase64String(strEncryptedText);
            MemoryStream memStream = new MemoryStream(byteEncryptedText);
            CryptoStream decryptStream = null;
            try
            {
                decryptStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
                byte[] byteDecryptedText = new byte[byteEncryptedText.Length];
                int decryptedByteCount = decryptStream.Read(byteDecryptedText, 0, byteDecryptedText.Length);
                strDecryptedText = Encoding.UTF8.GetString(byteDecryptedText, 0, decryptedByteCount);
            }
            finally
            {
                if (rijndael != null) rijndael.Clear();
                if (decryptor != null) decryptor.Dispose();
                if (memStream != null) memStream.Close();
                if (decryptStream != null) decryptStream.Close();
            }
            if (UseSalt)
                strDecryptedText = strDecryptedText.Substring(8);
            return strDecryptedText;
        }
    }
}
