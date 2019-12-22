using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public class Caesar : ICryptor
    {
        private string alphabet = "abcçdefgğhıijklmnoöprsştuüvyz";
        private static int ITERATOR_COUNT = 2;

        public Task<string> Encrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
               {
                   string text = messageModel.Message;

                   string cryptText = "";

                   for (int i = 0; i < text.Length; i++)
                   {
                       if (!alphabet.Contains(text[i]))
                       {
                           cryptText += text[i];
                           continue;
                       }

                       for (int j = 0; j < alphabet.Length; j++)
                       {
                           if (text[i] == alphabet[j])
                           {
                               int temp = (j + ITERATOR_COUNT > 28) ? (j + ITERATOR_COUNT) - 29 : (j + ITERATOR_COUNT);
                               cryptText += alphabet[temp];

                               break;
                           }

                       }
                   }

                   messageModel.IteratorCount = ITERATOR_COUNT;
                   return cryptText;

               });
        }

        public Task<string> Decrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string text = messageModel.Message;
                int iteratorCount = messageModel.IteratorCount;

                string decryptText = "";

                for (int i = 0; i < text.Length; i++)
                {
                    if (!alphabet.Contains(text[i]))
                    {
                        decryptText += text[i];
                        continue;
                    }

                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (text[i] == alphabet[j])
                        {
                            int temp = (j - iteratorCount < 0) ? (j - iteratorCount) + 29 : (j - iteratorCount);
                            decryptText += alphabet[temp];

                            break;
                        }

                    }
                }

                return decryptText;
            });
        }
    }
}
