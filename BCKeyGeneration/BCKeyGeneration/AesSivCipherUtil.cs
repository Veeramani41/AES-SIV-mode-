using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCKeyGeneration
{
    public class AesSivCipherUtil
    {

        private static byte[] BYTES_ZERO = new byte[16];
        private static byte DOUBLING_CONST = (byte)0x87;

        public static byte[] aesKeyBytes { get; set; }
        public static byte[] macKeyBytes { get; set; }

        public static byte[] sivEncrypt(byte[] plaintext, List<byte[]> additionalData)
        {
            //var keyGen = GeneratorUtilities.GetKeyGenerator("AES128");
            //var key = keyGen.GenerateKey();

            //var keyGen1 = GeneratorUtilities.GetKeyPairGenerator("ECDH");
            //var key1 = keyGen1.GenerateKeyPair();


            aesKeyBytes = GetTag("7AF8DFA60ADAB39FB3C066F0A64476EF");
            macKeyBytes = GetTag("F1300AC549F683934E91AA0D7E38057A");
            plaintext = GetTag("800419890312");
            if (aesKeyBytes == null || macKeyBytes == null)
            {
                throw new IllegalArgumentException("Can't get bytes of given key.");
            }
            try
            {
                return sivEncrypt(aesKeyBytes, macKeyBytes, plaintext, additionalData);
            }
            catch (InvalidKeyException ex)
            {
                throw new IllegalArgumentException();
            }
        }

        public static byte[] sivEncrypt(byte[] aesKey, byte[] macKey, byte[] plaintext, List<byte[]> additionalData)
        {
            if (aesKey.Length != 16 && aesKey.Length != 24 && aesKey.Length != 32)
            {
                throw new InvalidKeyException("Invalid aesKey length " + aesKey.Length);
            }

            byte[] iv = s2v(macKey, plaintext, additionalData);

            var str = BitConverter.ToString(iv).Replace("-", "");

            int numBlocks = (plaintext.Length + 15) / 16;

            // clear out the 31st and 63rd (rightmost) bit: 
            byte[] ctr = Arrays.CopyOf(iv, 16);
            ctr[8] = (byte)(ctr[8] & 0x7F);
            ctr[12] = (byte)(ctr[12] & 0x7F);
            var dffg = BitConverter.ToString(ctr).Replace("-", "");

            byte[] gh = new byte[8];
            Array.Copy(ctr, 8, gh, 0, 8);
            var bb = BitConverter.ToString(gh).Replace("-", "");
            long part2 = Convert.ToInt64(bb, 16);
            //ByteBuffer ctrBuf = ByteBuffer.wrap(ctr);

            long initialCtrVal = part2;//BitConverter.ToInt64(ctr, 8);//3213076589569161342;

            //long initialCtrVal = ctrBuf.getLong(8);

            byte[] x = new byte[numBlocks * 16];
            //IBlockCipher aes = new SicBlockCipher(new AesFastEngine());
            //IBlockCipher aes = new SicBlockCipher(new AesFastEngine());
            IBlockCipher aes = new AesFastEngine();

            //aes.Init(true, new ParametersWithIV(new KeyParameter(aesKey), new byte[16]));
            aes.Init(true, new KeyParameter(aesKey));
            //aes.Init(true, new ParametersWithIV(new KeyParameter(aesKey), new byte[16]));

            for (int i = 0; i < numBlocks; i++)
            {
                long ctrVal = initialCtrVal + i;
                CultureInfo ci = new CultureInfo("en-us");
                var df = ctrVal.ToString("X", ci);
                //df = "0" + df;
                if (df.Length < 16)
                {
                    var dss = Convert.ToUInt32(16 - df.Length);
                    
                    for (int ii = 0; ii < dss; ii++)
                    {
                        df = "0" + df;
                    }
                }
                if (df.Length % 2 == 1)
                {
                    df = "0" + df;
                }
                var njk = GetTag(df);
                
                //var njk = BitConverter.GetBytes(ctrVal);//long.TryParse(hexNumber, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out decNum);
                int k = 8;
                foreach (var item in njk)
                {
                    Buffer.SetByte(ctr, k, item);
                    k++;
                }
                var ddd = BitConverter.ToString(ctr).Replace("-", "");
                //ctrBuf.putLong(8, ctrVal);
                aes.ProcessBlock(ctr, 0, x, i * 16);
                aes.Reset();
            }

            byte[] ciphertext = xor(plaintext, x);
            //  concat IV + ciphertext:
            byte[] result = new byte[(iv.Length + ciphertext.Length)];
            System.Array.Copy(iv, 0, result, 0, iv.Length);
            System.Array.Copy(ciphertext, 0, result, iv.Length, ciphertext.Length);
            return result;
        }

        public static byte[] sivDecrypt(byte[] plaintext, List<byte[]> additionalData)
        {
            // byte[] aesKeyBytes = aesKey.getEncoded();
            // byte[] macKeyBytes = macKey.getEncoded();
            //if (aesKeyBytes == null || macKeyBytes == null)
            //{
            //    throw new IllegalArgumentException("Can't get bytes of given key.");
            //}
            try
            {
                //            sbyte[] aesKey = {(sbyte) -16, (sbyte) -15, (sbyte) -14, (sbyte) -13, //
                //(sbyte) -12, (sbyte) -11, (sbyte) -10, (sbyte) -9, //
                //(sbyte) -8, (sbyte) -7, (sbyte) -6, (sbyte) -5, //
                //(sbyte) -4, (sbyte) -3, (sbyte) -2, (sbyte) -1};

                //            sbyte[] macKey = {(sbyte) -1, (sbyte) -2, (sbyte) -3, (sbyte) -4, //
                //(sbyte) -5, (sbyte) -6, (sbyte) -7, (sbyte) -8, //
                //(sbyte) -9, (sbyte) -10, (sbyte) -11, (sbyte) -12, //
                //(sbyte) -13, (sbyte) -14, (sbyte) -15, (sbyte) -16};

                //aesKeyBytes = Enumerable.Range(0, "df5ebf2a3ecfafdbabd5234df5b0c050".Length)
                //      .Where(x => x % 2 == 0)
                //      .Select(x => Convert.ToByte("df5ebf2a3ecfafdbabd5234df5b0c050".Substring(x, 2), 16))
                //      .ToArray();

                //macKeyBytes = Enumerable.Range(0, "62ddb3438c9daf282402a0b8396999fe".Length)
                //     .Where(x => x % 2 == 0)
                //     .Select(x => Convert.ToByte("62ddb3438c9daf282402a0b8396999fe".Substring(x, 2), 16))
                //     .ToArray();

                //plaintext = Enumerable.Range(0, "15ec8853b4f1c79770d45a8aa1511ce605".Length)
                //     .Where(x => x % 2 == 0)
                //     .Select(x => Convert.ToByte("15ec8853b4f1c79770d45a8aa1511ce605".Substring(x, 2), 16))
                //     .ToArray();
                //var encryptedBytes = Convert.FromBase64String(textBox3.Text);

                aesKeyBytes = GetTag("7AF8DFA60ADAB39FB3C066F0A64476EF");//Convert.FromBase64String("GtFP7gJbrZyb/iwuOZfw7A==");

                macKeyBytes = GetTag("F1300AC549F683934E91AA0D7E38057A"); //Convert.FromBase64String("m66v0egqHRx0THsH2yZEzw==");
                plaintext = GetTag("AB71D6633F83E69F0061550135D45019B8A01B9693AE");
                //var ad = Convert.FromBase64String("EBESExQVFhcYGRobHB0eHyAhIiMkJSYn");
                //additionalData.Add(ad);
                return sivDecrypt(aesKeyBytes, macKeyBytes, plaintext, new List<byte[]>());
            }
            catch (InvalidKeyException ex)
            {
                throw new IllegalArgumentException();
            }
        }

        private static byte[] GetTag(string strTag)
        {
            string s = strTag.Replace("0x", "").Replace("0X", "");
            byte[] tag = new byte[0];
            if (s.Length > 0 && s.Length % 2 == 0)
            {
                tag = new byte[s.Length / 2];
                int j = 0;
                for (int i = 0; i < s.Length / 2; i++)
                {
                    string tempTag = s.Substring(j, 2);
                    tag[i] = Convert.ToByte(tempTag, 16);
                    j = j + 2;
                }
            }

            return tag;
        }

        public static byte[] sivDecrypt(byte[] aesKey, byte[] macKey, byte[] ciphertext, List<byte[]> additionalData)
        {
            if (aesKey.Length != 16 && aesKey.Length != 24 && aesKey.Length != 32)
            {
                throw new InvalidKeyException("Invalid aesKey length " + aesKey.Length);
            }

            byte[] iv = Arrays.CopyOf(ciphertext, 16);

            byte[] actualCiphertext = Arrays.CopyOfRange(ciphertext, 16, ciphertext.Length);
            int numBlocks = (actualCiphertext.Length + 15) / 16;

            // clear out the 31st and 63rd (rightmost) bit: 
            byte[] ctr = Arrays.CopyOf(iv, 16);
            ctr[8] = (byte)(ctr[8] & 0x7F);
            ctr[12] = (byte)(ctr[12] & 0x7F);
            //ByteBuffer ctrBuf = ByteBuffer.wrap(ctr);

            byte[] gh = new byte[8];
            Array.Copy(ctr, 8, gh, 0, 8);
            var bb = BitConverter.ToString(gh).Replace("-", "");
            long part2 = Convert.ToInt64(bb, 16);

            long initialCtrVal = part2;

            byte[] x = new byte[numBlocks * 16];
            //IBlockCipher aes = new SicBlockCipher(new AesFastEngine());
            IBlockCipher aes = new AesFastEngine();

            //aes.Init(true, new ParametersWithIV(new KeyParameter(aesKey), new byte[16]));
            aes.Init(true, new KeyParameter(aesKey));
            for (int i = 0; i < numBlocks; i++)
            {
                long ctrVal = initialCtrVal + i;
                CultureInfo ci = new CultureInfo("en-us");
                var df = ctrVal.ToString("X", ci);
                if (df.Length < 16)
                {
                    var dss = Convert.ToUInt32(16 - df.Length);

                    for (int ii = 0; ii < dss; ii++)
                    {
                        df = "0" + df;
                    }
                }
                if (df.Length > 0 && df.Length % 2 == 1)
                {
                    df = "0" + df;
                }
                var njk = GetTag(df);
                //var njk = BitConverter.GetBytes(ctrVal);
                int k = 8;
                foreach (var item in njk)
                {
                    Buffer.SetByte(ctr, k, item);
                    k++;
                }

                //ctrBuf.ToArray().putLong(8, ctrVal);
                var dfs = BitConverter.ToString(ctr).Replace("-", "");
                aes.ProcessBlock(ctr.ToArray(), 0, x, i * 16);
                aes.Reset();
            }

            var xi = BitConverter.ToString(x).Replace("-", "");
            byte[] plaintext = xor(actualCiphertext, x);

            byte[] control = s2v(macKey, plaintext, additionalData);

            var co = BitConverter.ToString(control).Replace("-", "");

            var ivs = BitConverter.ToString(iv).Replace("-", "");

            var pl = BitConverter.ToString(plaintext).Replace("-", "");
            if (Arrays.AreEqual(control, iv))
            {
                return plaintext;
            }
            else
            {
                throw new Exception("Authentication failed");
            }
        }

        static byte[] s2v(byte[] macKey, byte[] plaintext, List<byte[]> additionalData)
        {
            ICipherParameters param = new KeyParameter(macKey);
            IBlockCipher aes = new AesFastEngine();
            CMac mac = new CMac(aes);
            mac.Init(param);

            byte[] d = macs(mac, BYTES_ZERO);
            var str = BitConverter.ToString(d).Replace("-", "");
            foreach (byte[] s in additionalData)
            {
                d = xor(dbl(d), macs(mac, s));
            }
            var str1 = BitConverter.ToString(d).Replace("-", "");

            byte[] t;
            if (plaintext.Length >= 16)
            {
                t = xorend(plaintext, d);
            }
            else
            {
                t = xor(dbl(d), pad(plaintext));
            }

            var str2 = BitConverter.ToString(t).Replace("-", "");

            return macs(mac, t);
        }

        private static byte[] macs(IMac mac, byte[] ins)
        {
            byte[] result = new byte[mac.GetMacSize()];
            mac.BlockUpdate(ins, 0, ins.Length);
            mac.DoFinal(result, 0);
            return result;
        }

        /**  * First bit 1, following bits 0. 
         */
        private static byte[] pad(byte[] ins)
        {
            byte[] result = Arrays.CopyOf(ins, 16);
            new ISO7816d4Padding().AddPadding(result, ins.Length);
            return result;
        }

        /**  * Code taken from {@link org.bouncycastle.crypto.macs.CMac} 
         */
        private static int shiftLeft(byte[] block, byte[] output)
        {
            int i = block.Length;
            int bit = 0;
            while (--i >= 0)
            {
                int b = block[i] & 0xff;
                output[i] = (byte)((b << 1) | bit);
                bit = (b >> 7) & 1;
            }
            return bit;
        }

        /**  * Code taken from {@link org.bouncycastle.crypto.macs.CMac} 
         */
        private static byte[] dbl(byte[] ins)
        {
            byte[] ret = new byte[ins.Length];
            int carry = shiftLeft(ins, ret);
            int xor = 0xff & DOUBLING_CONST;

            /*   * NOTE: This construction is an attempt at a constant-time implementation. 
             */
            ret[ins.Length - 1] ^= (byte)(xor >> ((1 - carry) << 3));

            return ret;
        }

        private static byte[] xor(byte[] in1, byte[] in2)
        {
            if (in1 == null || in2 == null || in1.Length > in2.Length)
            {
                throw new IllegalArgumentException("Length of first input must be <= length of second input.");
            }

            byte[] result = new byte[in1.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(in1[i] ^ in2[i]);
            }
            return result;
        }

        private static byte[] xorend(byte[] in1, byte[] in2)
        {
            if (in1 == null || in2 == null || in1.Length < in2.Length)
            {
                throw new IllegalArgumentException("Length of first input must be >= length of second input.");
            }

            byte[] result = Arrays.CopyOf(in1, in1.Length);
            int diff = in1.Length - in2.Length;
            for (int i = 0; i < in2.Length; i++)
            {
                result[i + diff] = (byte)(result[i + diff] ^ in2[i]);
            }
            return result;
        }

    }
}
