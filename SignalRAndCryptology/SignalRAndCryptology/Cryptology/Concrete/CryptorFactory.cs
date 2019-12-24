using SignalRAndCryptology.Cryptology.Abstract;
using SignalRAndCryptology.Cryptology.Concrete.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRAndCryptology.Cryptology.Concrete
{
    public static class CryptorFactory
    {
        public static ICryptor CreateInstance(EnumCryptor enumCryptor)
        {
            switch (enumCryptor)
            {
                case EnumCryptor.CAESAR:
                    return new Caesar();
                case EnumCryptor.POLYBIUS:
                    return new Polybius();
                case EnumCryptor.PICKETFENCE:
                    return new PicketFence();
                case EnumCryptor.COLUMNAR:
                    return new Columnar();
                case EnumCryptor.DES:
                    return new DES();
                case EnumCryptor.AES:
                    return new AES();
                default:
                    throw new Exception("Unknown ICryptor instance");
            }
        }
    }
}
