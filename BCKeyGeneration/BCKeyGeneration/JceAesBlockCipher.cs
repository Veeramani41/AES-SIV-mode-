using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


namespace BCKeyGeneration
{
    public class JceAesBlockCipher : IBlockCipher
    {

        private static String ALG_NAME = "AES";
        private static String KEY_DESIGNATION = "AES";
        private static String JCE_CIPHER_NAME = "AES/ECB/NoPadding";

        private IBufferedCipher cipher;
        private Key key;
        private int opmode;

        public JceAesBlockCipher()
        {
            try
            {
                //IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");

                //this.cipher = Cipher.getInstance(JCE_CIPHER_NAME); // defaults to SunJCE but allows to configure different providers
                this.cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            }
            catch (NoSuchAlgorithmException  e) {
                throw new IllegalStateException("Every implementation of the Java platform is required to support AES/ECB/NoPadding.");
            }
            }
        


    public void Init(Boolean forEncryption, ICipherParameters param)
        {
            if (param == typeof(KeyParameter))
            {
                Init(forEncryption, (KeyParameter)param);
            }
            else
            {
                throw new IllegalArgumentException("Invalid or missing parameter of type KeyParameter.");
            }
        }

        private void Init(Boolean forEncryption, KeyParameter keyParam)
        {
            this.key = new SecretKeySpec(keyParam.getKey(), KEY_DESIGNATION);
            this.opmode = forEncryption ? Cipher.ENCRYPT_MODE : Cipher.DECRYPT_MODE;
            try
            {
                cipher.Init(opmode, key);
            }
            catch (InvalidKeyException e)
            {
                throw new IllegalArgumentException("Invalid key.", e);
            }
        }

        public String getAlgorithmName()
        {
            return ALG_NAME;
        }

        public int GetBlockSize()
        {
            return cipher.GetBlockSize();
        }

        public int ProcessBlock(byte[] ins, int inOff, byte[] outs, int outOff)
        {
            if (ins.Length - inOff < GetBlockSize())
            {
                throw new DataLengthException("Insufficient data in 'in'.");
            }
            try
            {
                return cipher.Update(ins, inOff, GetBlockSize(), outs, outOff);
            }
            catch (Exception e)
            {
                throw new DataLengthException("Insufficient space in 'out'.");
            }
        }

        public void Reset()
        {
            if (key == null)
            {
                return; // no-op if init has not been called yet.
            }
            try
            {
                cipher.Init(opmode, key);
            }
            catch (InvalidKeyException e)
            {
                throw new IllegalStateException("cipher.init(...) already invoked successfully earlier with same parameters.");
            }
        }

    }
}
