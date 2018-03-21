using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Anssi;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCKeyGeneration
{
    public class ECCKeyPairGenetation
    {
        private string algorithm;

        private ECDomainParameters eCDomainParameters;
        private DerObjectIdentifier publicKeyParamSet;
        private SecureRandom random;
        AsymmetricCipherKeyPair asymmetricCipherKeyPair;
        AsymmetricCipherKeyPair asymmetricCipherKeyPairA;
        string senderPublic;
        string reciverPublic;

        public void DefineCurve()
        {
            string curveName = "P-256";
            //var ecP1 = AnssiNamedCurves.GetByName("FRP256v1");
            //var ecP21 = TeleTrusTNamedCurves.GetByName("brainpoolp512t1");
            X9ECParameters ecP = NistNamedCurves.GetByName(curveName);
            var c = (FpCurve)ecP.Curve;
            eCDomainParameters = new ECDomainParameters(ecP.Curve, ecP.G, ecP.N, ecP.H, ecP.GetSeed());
            this.random = new SecureRandom();
            algorithm = "ECDH";
            asymmetricCipherKeyPair = GenerateKeyPair();
            this.random = new SecureRandom();
            asymmetricCipherKeyPairA = GenerateKeyPair();
            //senderPrivate = ((ECPrivateKeyParameters)asymmetricCipherKeyPair.Private).D.ToByteArray();
            var senderPublicb = ((ECPublicKeyParameters)asymmetricCipherKeyPairA.Public).Q.GetEncoded(true);
            var reciverPublicb = ((ECPublicKeyParameters)asymmetricCipherKeyPair.Public).Q.GetEncoded(true);

            senderPublic = Convert.ToBase64String(senderPublicb);
            reciverPublic = Convert.ToBase64String(reciverPublicb);
        }

        public byte[] GetSharedSecretValueForSender()
        {
            ECCurve curve = eCDomainParameters.Curve;
            ECPoint q = curve.DecodePoint(Convert.FromBase64String(reciverPublic));

            ECPublicKeyParameters peerPub = new ECPublicKeyParameters(q, eCDomainParameters);

            //ECDHCBasicAgreement eLacAgreement = new ECDHCBasicAgreement();
            //eLacAgreement.Init(asymmetricCipherKeyPair.Private);
            ECDHCBasicAgreement acAgreement = new ECDHCBasicAgreement();
            acAgreement.Init(asymmetricCipherKeyPairA.Private);
            //BigInteger eLA = eLacAgreement.CalculateAgreement(asymmetricCipherKeyPairA.Public);
            BigInteger a = acAgreement.CalculateAgreement(peerPub);
            //if (eLA.Equals(a) && !isEncrypt)
            //{
            //    return eLA.ToByteArray();
            //}
            if (a != null)
            {
                return a.ToByteArray();
            }
            return null;
        }

        public byte[] GetSharedSecretValueForReceiver()
        {
            ECCurve curve = eCDomainParameters.Curve;
            ECPoint q = curve.DecodePoint(Convert.FromBase64String(senderPublic));

            ECPublicKeyParameters peerPub = new ECPublicKeyParameters(q, eCDomainParameters);


            ECDHCBasicAgreement eLacAgreement = new ECDHCBasicAgreement();
            eLacAgreement.Init(asymmetricCipherKeyPair.Private);
            //ECDHCBasicAgreement acAgreement = new ECDHCBasicAgreement();
            //acAgreement.Init(asymmetricCipherKeyPairA.Private);
            BigInteger eLA = eLacAgreement.CalculateAgreement(peerPub);
            //BigInteger a = acAgreement.CalculateAgreement(asymmetricCipherKeyPair.Public);
            if (eLA != null)
            {
                return eLA.ToByteArray();
            }
            //if (eLA.Equals(a) && isEncrypt)
            //{
            //    return a.ToByteArray();
            //}
            return null;
        }

        public byte[] DeriveSymmetricKeyFromSharedSecret(byte[] sharedSecret)
        {

            var df = DigestUtilities.GetDigest("SHA256");
            var dg = DigestUtilities.GetDigest("SHA256").GetDigestSize();

            ECDHKekGenerator egH =
                    new ECDHKekGenerator(DigestUtilities.GetDigest("SHA256"));

            egH.Init(new DHKdfParameters(NistObjectIdentifiers.Aes, sharedSecret.Length, sharedSecret));
            byte[] symmetricKey = new byte[DigestUtilities.GetDigest("SHA256").GetDigestSize()];
            egH.GenerateBytes(symmetricKey, 0, symmetricKey.Length);

            return symmetricKey;
            //return sharedSecret;
        }

        public byte[] Encrypt(byte[] data, byte[] derivedKey)
        {

            byte[] output = null;
            try
            {
                
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
                cipher.Init(true, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", derivedKey), new byte[16]));
                try
                {
                    output = cipher.DoFinal(data);
                    return output;
                }
                catch (System.Exception ex)
                {
                    throw new CryptoException("Invalid Data");
                }
            }
            catch (Exception ex)
            {

            }

            return output;
        }

        public AsymmetricCipherKeyPair GenerateKeyPair()
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

            if (publicKeyParamSet != null)
            {
                return new AsymmetricCipherKeyPair(
                    new ECPublicKeyParameters(algorithm, q, publicKeyParamSet),
                    new ECPrivateKeyParameters(algorithm, d, publicKeyParamSet));
            }

            return new AsymmetricCipherKeyPair(
                new ECPublicKeyParameters(algorithm, q, eCDomainParameters),
                new ECPrivateKeyParameters(algorithm, d, eCDomainParameters));
        }

        protected virtual ECMultiplier CreateBasePointMultiplier()
        {
            return new FixedPointCombMultiplier();
        }

        public byte[] Decrypt(byte[] cipherData, byte[] derivedKey)
        {
            byte[] output = null;
            try
            {
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/NoPadding");
                cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", derivedKey), new byte[16]));
                try
                {
                    output = cipher.DoFinal(cipherData);

                }
                catch (System.Exception ex)
                {
                    throw new CryptoException("Invalid Data");
                }
            }
            catch (Exception ex)
            {
            }

            return output;
        }
    }
}
