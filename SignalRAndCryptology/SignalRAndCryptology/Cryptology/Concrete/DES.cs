using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public class DES : ICryptor
    {
        private string _randomKey;

        public DES()
        {
            _randomKey = new Random().Next(11111111, 99999999).ToString();
        }

        public Task<string> Encrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string result = String.Empty;
                string text = messageModel.Message;

                if (String.IsNullOrEmpty(text))
                {
                    throw new ArgumentNullException("Message can not be null or empty!");
                }
                else
                {
                    byte[] aryKey = Byte8(_randomKey);
                    byte[] aryIV = Byte8(_randomKey);

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(aryKey, aryIV), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(text);
                    writer.Flush();
                    cs.FlushFinalBlock();
                    writer.Flush();
                    result = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                    writer.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }
                messageModel.Key = _randomKey;
                return result;
            });
        }

        public Task<string> Decrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string result = "";
                string encText = messageModel.Message;

                if (String.IsNullOrEmpty(encText))
                {
                    throw new ArgumentNullException("Message can not be null or empty!");
                }
                else
                {
                    byte[] aryKey = Byte8(messageModel.Key);
                    byte[] aryIV = Byte8(messageModel.Key);

                    DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(encText));
                    CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(aryKey, aryIV), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cs);
                    result = reader.ReadToEnd();
                    reader.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }
                return result;
            });
        }

        //private byte[] ChangeStringToByte(string text)
        //{
        //    UnicodeEncoding ByteConverter = new UnicodeEncoding();
        //    return ByteConverter.GetBytes(text);
        //}

        public byte[] Byte8(string text)
        {
            char[] arrayChar = text.ToCharArray();
            byte[] arrayByte = new byte[arrayChar.Length];
            for (int i = 0; i < arrayByte.Length; i++)
            {
                arrayByte[i] = Convert.ToByte(arrayChar[i]);
            }
            return arrayByte;
        }
    }
}
