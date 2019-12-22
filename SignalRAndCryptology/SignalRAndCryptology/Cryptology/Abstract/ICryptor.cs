using SignalRAndCryptology.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Abstract
{
    public interface ICryptor
    {
        Task<string> Encrypt(MessageModel messageModel);
        Task<string> Decrypt(MessageModel messageModel);
    }
}
