using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public class NonSingularMatrix : ICryptor
    {

        public Task<string> Decrypt(MessageModel messageModel)
        {
            throw new NotImplementedException();
        }

        public Task<string> Encrypt(MessageModel messageModel)
        {
            throw new NotImplementedException();
        }
    }
}
