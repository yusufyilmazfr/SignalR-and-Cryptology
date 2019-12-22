using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public class PicketFence : ICryptor
    {
        public Task<string> Encrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string evenEncryptText = "";
                string oddEncryptText = "";

                string text = messageModel.Message;

                for (int i = 0; i < text.Length; i++)
                {
                    if (i % 2 == 0)
                        evenEncryptText += text[i];
                    else
                        oddEncryptText += text[i];
                }

                int evenCharacterCount = evenEncryptText.Length;
                int oddCharacterCount = oddEncryptText.Length;

                int sumCharacterCount = evenCharacterCount + oddCharacterCount;

                if (sumCharacterCount % 2 != 0)
                {
                    return evenCharacterCount > oddCharacterCount ?
                    evenEncryptText + oddEncryptText : oddEncryptText + evenEncryptText;
                }

                Debug.WriteLine($"evenEncryptText {evenEncryptText}");
                Debug.WriteLine($"oddEncryptText {oddEncryptText}");

                return evenEncryptText + oddEncryptText;
            });
        }

        public Task<string> Decrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string encryptText = messageModel.Message;

                string evenEncryptText = "";
                string oddEncryptText = "";

                string decryptText = "";

                if (encryptText.Length % 2 != 0)
                {
                    evenEncryptText = encryptText.Substring(0, encryptText.Length / 2 + 1);
                    oddEncryptText = encryptText.Substring(encryptText.Length / 2 + 1);
                }
                else
                {
                    evenEncryptText = encryptText.Substring(0, encryptText.Length / 2);
                    oddEncryptText = encryptText.Substring(encryptText.Length / 2);
                }

                for (int i = 0; i < oddEncryptText.Length; i++)
                {
                    decryptText += $"{evenEncryptText[i]}{oddEncryptText[i]}";
                }

                if (encryptText.Length % 2 != 0)
                    decryptText += evenEncryptText[evenEncryptText.Length - 1];


                Debug.WriteLine($"evenEncryptText {evenEncryptText}");
                Debug.WriteLine($"oddEncryptText {oddEncryptText}");

                return decryptText;
            });
        }
    }
}
