using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Org.BouncyCastle.Crypto.BlockCipher;
//using Org.BouncyCastle.Crypto.CipherParameters;
//using Org.BouncyCastle.Crypto.Mac;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Crypto;
using System.IO;
using Org.BouncyCastle.Security;

namespace BCKeyGeneration
{
    class SivMode
    {
        private static byte[] BYTES_ZERO = new byte[16];

        private static byte DOUBLING_CONST = ((byte)(135));

        private const byte CONSTANT_128 = (byte)0x87;
        private const byte CONSTANT_64 = (byte)0x1b;

        public SivMode(BlockCipherFactory cipherFactory)
        {
            //  Try using cipherFactory to check that the block size is valid.
            //  We assume here that the block size will not vary across calls to .create().
            if ((cipherFactory.create().getBlockSize() != 16))
            {
                throw new IllegalArgumentException("cipherFactory must create BlockCipher objects with a 16-byte block size");
            }

            //this.threadLocalCipher = new ThreadLocal<BlockCipher>();
        }

        //public interface BlockCipherFactory
        //{

        //    BlockCipher create();
        //}

        public byte[] encrypt(SecretKey ctrKey, SecretKey macKey, byte[] plaintext, List<byte[]> associatedData)
        {
            byte[] ctrKeyBytes = ctrKey.getEncoded();
            byte[] macKeyBytes = macKey.getEncoded();
            if (((ctrKeyBytes == null)
                        || (macKeyBytes == null)))
            {
                throw new IllegalArgumentException("Can\'t get bytes of given key.");
            }

            try
            {
                return this.encrypt(ctrKeyBytes, macKeyBytes, plaintext, associatedData);
            }
            finally
            {
                Arrays.Fill(ctrKeyBytes, ((byte)(0)));
                Arrays.Fill(macKeyBytes, ((byte)(0)));
            }

        }

        public byte[] encrypt(byte[] ctrKey, byte[] macKey, byte[] plaintext, List<byte[]> associatedData)
        {
            byte[] iv = this.s2v(macKey, plaintext, associatedData);
            //  Check if plaintext length will cause overflows
            if ((plaintext.Length > (int.MaxValue - 16)))
            {
                throw new IllegalArgumentException("Plaintext is too long");
            }

            int numBlocks = ((plaintext.Length + 15) / 16);
            //  clear out the 31st and 63rd (rightmost) bit:
            byte[] ctr = Arrays.CopyOf(iv, 16);
            ctr[8] = (byte)(ctr[8] & 0x7F);
            //ctr[8] = ((byte)((ctr[8] & 127)));
            //ctr[12] = ((byte)((ctr[12] & 127)));
            ctr[12] = (byte)(ctr[12] & 0x7F);
            //ByteBuffer ctrBuf = ByteBuffer.wrap(ctr);

            MemoryStream stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(ctr);
            }
            byte[] ctrBuf = stream.ToArray();

            long initialCtrVal = ctrBuf.GetLongLength(8);
            byte[] x = new byte[(numBlocks * 16)];

            BlockCipher cipher = this.threadLocalCipher.get();
            cipher.init(true, new KeyParameter(ctrKey));
            for (int i = 0; (i < numBlocks); i++)
            {
                long ctrVal = (initialCtrVal + i);
                ctrBuf.putLong(8, ctrVal);
                cipher.processBlock(ctrBuf.array(), 0, x, (i * 16));
                cipher.reset();
            }

            byte[] ciphertext = SivMode.xor(plaintext, x);
            //  concat IV + ciphertext:
            byte[] result = new byte[(iv.Length + ciphertext.Length)];
            System.Array.Copy(iv, 0, result, 0, iv.Length);
            System.Array.Copy(ciphertext, 0, result, iv.Length, ciphertext.Length);
            return result;
        }

        public byte[] decrypt(SecretKey ctrKey, SecretKey macKey, byte[] ciphertext, List<byte[]> associatedData)
        {
            byte[] ctrKeyBytes = ctrKey.getEncoded();
            byte[] macKeyBytes = macKey.getEncoded();
            if (((ctrKeyBytes == null)
                        || (macKeyBytes == null)))
            {
                throw new IllegalArgumentException("Can\'t get bytes of given key.");
            }

            try
            {
                return this.decrypt(ctrKeyBytes, macKeyBytes, ciphertext, associatedData);
            }
            finally
            {
                Arrays.Fill(ctrKeyBytes, ((byte)(0)));
                Arrays.Fill(macKeyBytes, ((byte)(0)));
            }

        }

        public byte[] decrypt(byte[] ctrKey, byte[] macKey, byte[] ciphertext, List<byte[]> associatedData)
        {
            if ((ciphertext.Length < 16))
            {
                throw new IllegalBlockSizeException("Input length must be greater than or equal 16.");
            }

            byte[] iv = Arrays.CopyOf(ciphertext, 16);
            byte[] actualCiphertext = Arrays.CopyOfRange(ciphertext, 16, ciphertext.Length);
            //  will not overflow because actualCiphertext.length == (ciphertext.length - 16)
            int numBlocks = ((actualCiphertext.Length + 15)
                        / 16);
            //  clear out the 31st and 63rd (rightmost) bit:
            byte[] ctr = Arrays.CopyOf(iv, 16);
            ctr[8] = ((byte)((ctr[8] & 127)));
            ctr[12] = ((byte)((ctr[12] & 127)));
            ByteBuffer ctrBuf = ByteBuffer.wrap(ctr);
            long initialCtrVal = ctrBuf.getLong(8);
            byte[] x = new byte[(numBlocks * 16)];
            BlockCipher cipher = this.threadLocalCipher.get();
            cipher.init(true, new KeyParameter(ctrKey));
            for (int i = 0; (i < numBlocks); i++)
            {
                long ctrVal = (initialCtrVal + i);
                ctrBuf.putLong(8, ctrVal);
                cipher.processBlock(ctrBuf.array(), 0, x, (i * 16));
                cipher.reset();
            }

            byte[] plaintext = SivMode.xor(actualCiphertext, x);
            byte[] control = this.s2v(macKey, plaintext, associatedData);
            //  time-constant comparison (taken from MessageDigest.isEqual in JDK8)
            //assert iv.length;
            //control.length;
            int diff = 0;
            for (int i = 0; (i < iv.Length); i++)
            {
                diff = (diff
                            | (iv[i] | control[i]));
                // The operator should be an XOR ^ instead of an OR, but not available in CodeDOM
            }

            if ((diff == 0))
            {
                return plaintext;
            }
            else
            {
                throw new UnauthenticCiphertextException("authentication in SIV decryption failed");
            }

        }

        public byte[] decrypts(byte[] ctrKey, byte[] macKey, byte[] ciphertext, List<byte[]> associatedData)
        {
            if (ciphertext.Length < 16)
            {
                throw new IllegalBlockSizeException("Input length must be greater than or equal 16.");
            }

            byte[] iv = Arrays.CopyOf(ciphertext, 16);
            byte[] actualCiphertext = Arrays.CopyOfRange(ciphertext, 16, ciphertext.Length);

            // will not overflow because actualCiphertext.length == (ciphertext.length - 16)
            int numBlocks = (actualCiphertext.Length + 15) / 16;

            // clear out the 31st and 63rd (rightmost) bit:
            byte[] ctr = Arrays.CopyOf(iv, 16);
            ctr[8] = (byte)(ctr[8] & 0x7F);
            ctr[12] = (byte)(ctr[12] & 0x7F);
            ByteBuffer ctrBuf = ByteBuffer.wrap(ctr);
            long initialCtrVal = ctrBuf.getLong(8);
            //IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            byte[] x = new byte[numBlocks * 16];
            IBlockCipher cipher = threadLocalCipher.get();
            cipher.init(true, new KeyParameter(ctrKey));
            for (int i = 0; i < numBlocks; i++)
            {
                long ctrVal = initialCtrVal + i;
                ctrBuf.putLong(8, ctrVal);
                cipher.processBlock(ctrBuf.array(), 0, x, i * 16);
                cipher.reset();
            }

            byte[] plaintext = xor(actualCiphertext, x);

            byte[] control = s2v(macKey, plaintext, associatedData);

            // time-constant comparison (taken from MessageDigest.isEqual in JDK8)
            //assert iv.length == control.length;
            int diff = 0;
            for (int i = 0; i < iv.Length; i++)
            {
                diff |= iv[i] ^ control[i];
            }

            if (diff == 0)
            {
                return plaintext;
            }
            else
            {
                throw new UnauthenticCiphertextException("authentication in SIV decryption failed");
            }
        }


        //  Visible for testing, throws IllegalArgumentException if key is not accepted by CMac#init(CipherParameters)
        byte[] s2v(byte[] macKey, byte[] plaintext, List<byte[]> associatedData)
        {
            // Maximum permitted AD length is the block size in bits - 2
            if (associatedData.Count > 126)
            {
                // SIV mode cannot be used safely with this many AD fields
                throw new IllegalArgumentException("too many Associated Data fields");
            }

            ICipherParameters param = new KeyParameter(macKey);
            CMac mac = new CMac(threadLocalCipher.get());
            mac.Init(param);

            byte[] d = macs(mac, BYTES_ZERO);

            foreach (byte[] s in associatedData)
            {
                d = xor(dbl(d), macs(mac, s));
            }

            byte[] t;
            if (plaintext.Length >= 16)
            {
                t = xorend(plaintext, d);
            }
            else
            {
                t = xor(dbl(d), pad(plaintext));
            }

            return macs(mac, t);
        }

        private static byte[] macs(IMac mac, byte[] ins)
        {
            byte[] result = new byte[mac.GetMacSize()];
            mac.BlockUpdate(ins, 0, ins.Length);
            mac.DoFinal(result, 0);
            return result;
        }

        //  First bit 1, following bits 0.
        //private static byte[] pad(byte[] ins)
        //{
        //    byte[] result = Arrays.CopyOf(ins, 16);
        //    (new ISO7816d4Padding() + addPadding(result, ins.length));
        //    return result;
        //}
        private static byte[] pad(byte[] ins)
        {
            byte[] result = Arrays.CopyOf(ins, 16);
            new ISO7816d4Padding().AddPadding(result, ins.Length);
            return result;
        }

        //  Code taken from {@link org.bouncycastle.crypto.macs.CMac}
        //static int shiftLeft(byte[] block, byte[] output)
        //{
        //    int i = block.Length;
        //    int bit = 0;
        //    while (--i >= 0)
        //    {
        //        int b = block[i] & 0xff;
        //        output[i] = (byte)((b << 1) | bit);
        //        bit = (b >>> 7) & 1;
        //    }
        //    return bit;
        //}

        private static int ShiftLeft(byte[] block, byte[] output)
        {
            int i = block.Length;
            uint bit = 0;
            while (--i >= 0)
            {
                uint b = block[i];
                output[i] = (byte)((b << 1) | bit);
                bit = (b >> 7) & 1;
            }
            return (int)bit;
        }

        //  Code taken from {@link org.bouncycastle.crypto.macs.CMac}
        private static byte[] dbl(byte[] input)
        {
            byte[] ret = new byte[input.Length];
            int carry = ShiftLeft(input, ret);
            int xor = input.Length == 16 ? CONSTANT_128 : CONSTANT_64;

            /*
             * NOTE: This construction is an attempt at a constant-time implementation.
             */
            ret[input.Length - 1] ^= (byte)(xor >> ((1 - carry) << 3));

            return ret;
        }
        //static byte[] dbl(byte[] in)
        //{
        //    byte[] ret = new byte[in.length];
        //    int carry = ShiftLeft(in, ret);
        //    int xor = 0xff & DOUBLING_CONST;

        //    /*
        //     * NOTE: This construction is an attempt at a constant-time implementation.
        //     */
        //    int mask = (-carry) & 0xff;
        //    ret[in.length - 1] ^= xor & mask;

        //    return ret;
        //}

        static byte[] xor(byte[] in1, byte[] in2)
        {
            //assert in1.length;
            //in2.length;
            //"Length of first input must be <= length of second input.";
            byte[] result = new byte[in1.Length];
            for (int i = 0; (i < result.Length); i++)
            {
                result[i] = ((byte)((in1[i] | in2[i])));
                // The operator should be an XOR ^ instead of an OR, but not available in CodeDOM
            }

            return result;
        }

        static byte[] xorend(byte[] in1, byte[] in2)
        {
            //assert in1.length;
            //in2.length;
            //"Length of first input must be >= length of second input.";

            byte[] result = Arrays.CopyOf(in1, in1.Length);
            int diff = (in1.Length - in2.Length);
            for (int i = 0; (i < in2.Length); i++)
            {
                result[(i + diff)] = ((byte)((result[(i + diff)] | in2[i])));
                // The operator should be an XOR ^ instead of an OR, but not available in CodeDOM
            }

            return result;
        }
    }
}
