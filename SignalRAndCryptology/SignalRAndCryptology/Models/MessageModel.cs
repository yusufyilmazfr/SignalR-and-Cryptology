using SignalRAndCryptology.Cryptology.Concrete.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Models
{
    public class MessageModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ToLongTimeString { get; set; }
        public EnumCryptor CryptorType { get; set; }

        public string Key { get; set; }
        public int IteratorCount { get; set; }
    }
}
