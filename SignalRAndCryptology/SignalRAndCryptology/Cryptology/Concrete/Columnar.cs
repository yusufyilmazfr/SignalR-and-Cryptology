using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public class Columnar : ICryptor
    {
        private string alphabet = "abcçdefgğhıijklmnoöprsştuüvyz";
        private const int COLUMN_COUNT = 5;

        public Task<string> Decrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string text = messageModel.Message;
                string encryptText = String.Empty;

                string[] messageWordArray = messageModel.Message.Split(' ');

                int columnCount = messageWordArray.Length;
                int rowCount = messageWordArray[0].Length;

                char[,] characterTable = new char[rowCount, columnCount];


                for (int column = 0; column < columnCount; column++)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        characterTable[row, column] = messageWordArray[column][row];
                    }
                }

                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        encryptText += $"{characterTable[row, column]}";
                    }
                }

                return encryptText;
            });
        }

        public Task<string> Encrypt(MessageModel messageModel)
        {
            return Task.Run(() =>
            {
                string encryptText = String.Empty;

                string text = messageModel.Message;

                text = text.Replace(" ", string.Empty);


                int rowCount = (int)Math.Ceiling(text.Length / ((double)COLUMN_COUNT));

                char[,] characterTable = new char[rowCount, COLUMN_COUNT];

                int indexOfTextItem = 0;

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < COLUMN_COUNT; j++)
                    {
                        if (indexOfTextItem < text.Length)
                        {
                            characterTable[i, j] = text[indexOfTextItem];
                            indexOfTextItem++;
                        }
                        else
                        {
                            characterTable[i, j] = 'x';
                            //characterTable[i, j] = alphabet[5];
                            //characterTable[i, j] = ' ';
                        }
                    }
                }

                for (int column = 0; column < COLUMN_COUNT; column++)
                {
                    for (int row = 0; row < rowCount; row++)
                    {
                        encryptText += $"{characterTable[row, column]}";
                    }
                    encryptText += " ";
                }


                return encryptText.TrimEnd();
            });
        }
    }
}
