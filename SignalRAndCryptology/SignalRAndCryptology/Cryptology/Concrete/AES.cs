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
    public class AES : ICryptor
    {
        private string _randomKey;

        public AES()
        {
            Random random = new Random();

            for (int i = 0; i < 2; i++)
            {
                _randomKey += random.Next(11111111, 99999999);
            }
        }

        public Task<string> Decrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string result = String.Empty;
                string encText = messageModel.Message;

                Aes encryptor = Aes.Create();
                encryptor.Mode = CipherMode.CBC;
                encryptor.KeySize = 256;
                encryptor.BlockSize = 128;
                encryptor.Padding = PaddingMode.Zeros;

                // Set key and IV
                encryptor.Key = Byte8(messageModel.Key);
                encryptor.IV = Byte8(messageModel.Key);

                MemoryStream memoryStream = new MemoryStream();
                ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

                CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

                string plainText = String.Empty;

                try
                {
                    byte[] cipherBytes = Convert.FromBase64String(encText);
                    cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] plainBytes = memoryStream.ToArray();
                    plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
                }
                finally
                {
                    memoryStream.Close();
                    cryptoStream.Close();
                }

                return plainText;

            });
        }

        public Task<string> Encrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string result = "";
                string text = messageModel.Message;

                Aes encryptor = Aes.Create();
                encryptor.Mode = CipherMode.CBC;

                encryptor.KeySize = 256;
                encryptor.BlockSize = 128;
                encryptor.Padding = PaddingMode.Zeros;

                encryptor.Key = Byte8(_randomKey);
                encryptor.IV = Byte8(_randomKey);

                MemoryStream memoryStream = new MemoryStream();
                ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

                CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
                byte[] plainBytes = Encoding.ASCII.GetBytes(text);
                cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();
                result = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

                messageModel.Key = _randomKey;

                return result;
            });
        }

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

        private byte[] ChangeStringToByte(string text)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetBytes(text);
        }
    }
}
