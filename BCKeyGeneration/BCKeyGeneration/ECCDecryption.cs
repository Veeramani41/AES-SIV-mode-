using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;

namespace BCKeyGeneration
{
    public class ECCDecryption
    {
        private string algorithm;

        private ECDomainParameters eCDomainParameters;
        private SecureRandom random;
        AsymmetricCipherKeyPair asymmetricCipherKeyPair;
        public byte[] receiverPublicKey;
        public byte[] sharedSecret;
        public byte[] derivedKey;

        public ECCDecryption(bool isCompressed)
        {
            DefineCurve(isCompressed);
        }

        private void DefineCurve(bool isCompressed)
        {
            string curveName = "P-256";
            X9ECParameters ecP = NistNamedCurves.GetByName(curveName);
            var c = (FpCurve)ecP.Curve;
            eCDomainParameters = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());
            this.random = new SecureRandom();
            algorithm = "ECDH";
            asymmetricCipherKeyPair = GenerateKeyPair();
            //senderPrivate = ((ECPrivateKeyParameters)asymmetricCipherKeyPair.Private).D.ToByteArray();
            receiverPublicKey = ((ECPublicKeyParameters)asymmetricCipherKeyPair.Public).Q.GetEncoded(isCompressed);
            var ddf = ((ECPublicKeyParameters)asymmetricCipherKeyPair.Public).Q.XCoord.ToString();
        }

        private AsymmetricCipherKeyPair GenerateKeyPair()
        {
            BigInteger n = eCDomainParameters.N;
            BigInteger d;
            int minWeight = n.BitLength >> 2;

            for (;;)
            {
                d = new BigInteger(n.BitLength, random);

                if (d.CompareTo(BigInteger.Two) < 0 || d.CompareTo(n) >= 0)
                    continue;

                if (WNafUtilities.GetNafWeight(d) < minWeight)
                    continue;

                break;
            }

            ECPoint q = CreateBasePointMultiplier().Multiply(eCDomainParameters.G, d);

            return new AsymmetricCipherKeyPair(
                new ECPublicKeyParameters(algorithm, q, eCDomainParameters),
                new ECPrivateKeyParameters(algorithm, d, eCDomainParameters));
        }

        protected virtual ECMultiplier CreateBasePointMultiplier()
        {
            return new FixedPointCombMultiplier();
        }

        private byte[] GetSharedSecretValueForSender(byte[] reciverPublicKey)
        {
            ECCurve curve = eCDomainParameters.Curve;
            ECPoint q = curve.DecodePoint(reciverPublicKey);

            ECPublicKeyParameters peerPub = new ECPublicKeyParameters(q, eCDomainParameters);

            ECDHCBasicAgreement acAgreement = new ECDHCBasicAgreement();
            acAgreement.Init(asymmetricCipherKeyPair.Private);
            BigInteger a = acAgreement.CalculateAgreement(peerPub);
            if (a != null)
            {
                return a.ToByteArray();
            }
            return null;
        }

        private byte[] DeriveSymmetricKeyFromSharedSecret(byte[] reciverPublicKey)
        {
           // var key = new byte[32];
            //sharedSecret = GetSharedSecretValueForSender(reciverPublicKey);
            //System.Array.Copy(sharedSecret, 1, key, 0, 32);
            //sharedSecret = GetTag("DC800298EF61F540AA2F971BCBE748F0BF023EE804E2278B48A1EFF9E640B1F0");
            //ECDHKekGenerator egH =
            //        new ECDHKekGenerator(DigestUtilities.GetDigest("SHA256"));

            //egH.Init(new DHKdfParameters(NistObjectIdentifiers.Aes, sharedSecret.Length, sharedSecret));
            //byte[] symmetricKey = new byte[DigestUtilities.GetDigest("SHA256").GetDigestSize()];
            //egH.GenerateBytes(symmetricKey, 0, symmetricKey.Length);

            //return sharedSecret;
            //return testKDF.getSHA512(key);

            var symmetricKey = new byte[32];
            sharedSecret = GetSharedSecretValueForSender(reciverPublicKey);
            object index = sharedSecret.GetValue(0);
            if (Convert.ToInt64(index) == 0)
                Array.Copy(sharedSecret, 1, symmetricKey, 0, 32);
            else
                Array.Copy(sharedSecret, 0, symmetricKey, 0, 32);

            return testKDF.getSHA512(symmetricKey);
        }

        private byte[] GetTag(string strTag)
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
        public byte[] Decrypt(byte[] cipherData, string reciverPublicKey)
        {
            //byte[] publicKey;// = GetTag(reciverPublicKey);
            //try
            //{
            //    publicKey = Convert.FromBase64String(reciverPublicKey);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("E2E Public Key is not a valid base64string," + ex.Message);
            //}

            derivedKey = GetTag("b0b193394ea1814cdccc9d3bcd554f90dbe82a05cc5cb8899608c74490a005ff");//DeriveSymmetricKeyFromSharedSecret(publicKey);//GetTag("b0b193394ea1814cdccc9d3bcd554f90dbe82a05cc5cb8899608c74490a005ff");//
            var cmacKey = new byte[16];
            var aesKey = new byte[16];
            Array.Copy(derivedKey, 0, cmacKey, 0, 16);
            Array.Copy(derivedKey, 16, aesKey, 0, 16);
            try
            {
                return AesSivCipherUtil.sivDecrypt(aesKey, cmacKey, cipherData, new List<byte[]>());
            }
            catch (Exception ex)
            {
                throw new CryptoException("Invalid Data" + ex.Message);
            }
            //byte[] output = null;
            //try
            //{
            //    byte[] iv = new byte[16];

            //    IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
            //    cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", derivedKey), iv));
            //    try
            //    {
            //        output = cipher.DoFinal(cipherData);
            //        return output;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new CryptoException("Invalid Data" + ex.Message);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

    }

}
