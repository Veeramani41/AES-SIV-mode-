using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCKeyGeneration
{
    class testKDF
    {
        public static byte[] getSHA512(byte[] key)
        {
            Sha256Digest digester = new Sha256Digest();
            byte[] retValue = new byte[digester.GetDigestSize()];
            foreach (var item in intToFourBytes(1))
            {
                digester.Update(item);
            }
            foreach (var item in key)
            {
                digester.Update(item);
            }
            digester.DoFinal(retValue, 0);
            return retValue;
        }


        public static byte[] intToFourBytes(int i)
        {
            byte[] res = new byte[4];
            res[0] = (byte)(i >> 24);
            res[1] = (byte)((i >> 16) & 0xFF);
            res[2] = (byte)((i >> 8) & 0xFF);
            res[3] = (byte)(i & 0xFF);
            return res;
        }
    }
}
