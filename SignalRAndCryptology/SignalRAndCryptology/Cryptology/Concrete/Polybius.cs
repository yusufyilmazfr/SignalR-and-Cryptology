using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public class Polybius : ICryptor
    {
        private char[,] alphabet = new char[5, 5] {
                {'a', 'b', 'c', 'd', 'e'  },
                {'f', 'g', 'h', 'ı', 'j'  },
                {'k', 'l', 'm', 'n', 'o'  },
                {'p', 'r', 's', 't', 'u'  },
                {'v', 'y', 'z', 'ğ', ' '  }
            };

        public Task<string> Decrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                var decryptText = "";

                var indexesArray = messageModel.Message.Split('-');

                for (int i = 0; i < indexesArray.Length; i++)
                {
                    int x = Convert.ToInt16(indexesArray[i].Substring(0, 1));
                    int y = Convert.ToInt16(indexesArray[i].Substring(1, 1));

                    decryptText += alphabet[x, y];
                }

                return decryptText;
            });
        }

        public Task<string> Encrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                var encryptText = "";

                var charText = messageModel.Message.ToArray();

                for (int i = 0; i < charText.Length; i++)
                {
                    for (int x = 0; x < alphabet.GetLength(0); x++)
                    {
                        for (int y = 0; y < alphabet.GetLength(1); y++)
                        {
                            if (alphabet[x, y] == charText[i])
                            {
                                encryptText += $"{x}{y}-";
                            }
                        }
                    }
                }

                return encryptText.TrimEnd('-');
            });
        }
    }
}
