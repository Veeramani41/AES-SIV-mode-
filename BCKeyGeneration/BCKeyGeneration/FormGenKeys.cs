using Org.BouncyCastle.Asn1.Anssi;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Agreement.Kdf;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCKeyGeneration
{
    public partial class FormGenKeys : Form
    {
        public ECPrivateKeyParameters senderPrivateKeyParameters;
        public byte[] senderPublic;
        public byte[] senderPrivate;

        public ECPrivateKeyParameters receiverPrivateKeyParameters;
        public byte[] receiverPublic;
        public byte[] receiverPrivate;

        public FormGenKeys()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (P128.Checked)
                GeneratePKeys(128);
            else if (P256.Checked)
                GeneratePKeys(256);

        }




        /// <summary>
        /// This method creates 256 bit keys and creates the public/private key pair (if they are not yet created only)
        /// </summary>
        private void GeneratePKeys(int intSize)
        {
            //Generating p-128 keys 128 specifies strength
            var senderkeyPair = GenerateKeys(intSize);
            senderPrivateKeyParameters = ((ECPrivateKeyParameters)senderkeyPair.Private);
            senderPrivate = senderPrivateKeyParameters.D.ToByteArray();
            senderPublic = ((ECPublicKeyParameters)senderkeyPair.Public).Q.GetEncoded();

            var dfg = Convert.ToBase64String(senderPublic);

            var receiverkeyPair = GenerateKeys(intSize);
            receiverPrivateKeyParameters = ((ECPrivateKeyParameters)receiverkeyPair.Private);
            receiverPrivate = receiverPrivateKeyParameters.D.ToByteArray();
            receiverPublic = ((ECPublicKeyParameters)receiverkeyPair.Public).Q.GetEncoded();


            TextWriter textWriter = new StringWriter();
            Org.BouncyCastle.OpenSsl.PemWriter pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(senderkeyPair.Private);
            pemWriter.Writer.Flush();
            string privateKey = textWriter.ToString();
            var privateBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(privateKey);

            //txtPrivateKey.Text = Convert.ToBase64String(privateBytes);
            txtPrivateKey.Text = privateKey;
            ECPrivateKeyParameters privateKeyParam = (ECPrivateKeyParameters)senderkeyPair.Private;
            txtD.Text = privateKeyParam.D.ToString();

            textWriter = new StringWriter();
            pemWriter = new Org.BouncyCastle.OpenSsl.PemWriter(textWriter);
            pemWriter.WriteObject(senderkeyPair.Public);
            pemWriter.Writer.Flush();

            ECPublicKeyParameters publicKeyParam = (ECPublicKeyParameters)senderkeyPair.Public;

            string publickey = textWriter.ToString();
            //var plainTextBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(publickey);

            txtPublicKey.Text = publickey;// Convert.ToBase64String(plainTextBytes);

            txtX.Text = publicKeyParam.Q.X.ToBigInteger().ToString();
            txtY.Text = publicKeyParam.Q.Y.ToBigInteger().ToString();

        }

        public AsymmetricCipherKeyPair GenerateKeys(int keySize)
        {
            //using ECDSA algorithm for the key generation
            var gen = new CEBA.Org.BouncyCastle.Crypto.Generators.ECKeyPairGenerator("ECDH");

            //Creating Random
            var secureRandom = new SecureRandom();

            //Parameters creation using the random and keysize
            var keyGenParam = new KeyGenerationParameters(secureRandom, keySize);

            //Initializing generation algorithm with the Parameters
            gen.Init(keyGenParam);

            //Generation of Key Pair
            return gen.GenerateKeyPair();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtX.Clear();
            txtY.Clear();
            txtD.Clear();
            txtPublicKey.Clear();
            txtPrivateKey.Clear();
        }

        private void P256_CheckedChanged(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtX.Text))
            {
                MessageBox.Show("X is Empty");
                return;
            }
            if (string.IsNullOrEmpty(txtY.Text))
            {
                MessageBox.Show("Y is Empty");
                return;
            }
            if (string.IsNullOrEmpty(txtD.Text))
            {
                MessageBox.Show("D is Empty");
                return;
            }

            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "AllFiles|*.*";
            saveFileDialog1.Title = "Save X Y D Parameters";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        var byteArrX = System.Text.Encoding.UTF8.GetBytes("X: " + txtX.Text);
                        fs.Write(byteArrX, 0, byteArrX.Length);

                        var byteArrY = System.Text.Encoding.UTF8.GetBytes("\nY: " + txtY.Text);
                        fs.Write(byteArrY, 0, byteArrY.Length);

                        var byteArrD = System.Text.Encoding.UTF8.GetBytes("\nD: " + txtD.Text);
                        fs.Write(byteArrD, 0, byteArrD.Length);

                        break;
                    default: break;

                }

                fs.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(txtPublicKey.Text))
            {
                MessageBox.Show("Public key is Empty");
                return;
            }
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "AllFiles|*.*";
            saveFileDialog1.Title = "Save Public Key";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        var bytePub = System.Text.Encoding.UTF8.GetBytes(txtPublicKey.Text);
                        fs.Write(bytePub, 0, bytePub.Length);
                        break;
                    default: break;

                }

                fs.Close();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtPrivateKey.Text))
            {
                MessageBox.Show("Private key is Empty");
                return;
            }
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "AllFiles|*.*";
            saveFileDialog1.Title = "Save Private Key";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        var bytePrivate = System.Text.Encoding.UTF8.GetBytes(txtPrivateKey.Text);
                        fs.Write(bytePrivate, 0, bytePrivate.Length);
                        break;
                    default: break;

                }

                fs.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            var plainTextBytes = Encoding.GetEncoding("iso-8859-1").GetBytes(textBox1.Text);
            //var publicKey = Convert.FromBase64String(txtPublicKey.Text);
            var publicKey = System.Text.Encoding.UTF8.GetBytes(txtPublicKey.Text);
            var encryptedValue = Encrypt(plainTextBytes, receiverPublic);

            textBox2.Text = Convert.ToBase64String(encryptedValue);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var encryptedBytes = Convert.FromBase64String(textBox2.Text);
            var privateKey = Encoding.UTF8.GetBytes(txtPrivateKey.Text);
            var decryptedValue = Decrypt(encryptedBytes, senderPrivate);

            textBox2.Text = Encoding.GetEncoding("iso-8859-1").GetString(decryptedValue);
        }

        public byte[] Encrypt(byte[] data, byte[] pubKey)
        {
            byte[] output = null;
            try
            {
                var xx = PublicKeyFactory.CreateKey(pubKey);

                var pub = ((ECPublicKeyParameters)xx);
                var derivedKey = GetSharedSecretValueForSender(pub);

                KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("AES", derivedKey, 0, derivedKey.Length);
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/ISO7816_4PADDING");
                cipher.Init(true, keyparam);
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

        public byte[] GetSharedSecretValueForSender(ECPublicKeyParameters pub)
        {
            //ECDHCBasicAgreement eLacAgreement = new ECDHCBasicAgreement();
            //eLacAgreement.Init(asymmetricCipherKeyPair.Private);
            ECDHCBasicAgreement acAgreement = new ECDHCBasicAgreement();
            acAgreement.Init(senderPrivateKeyParameters);
            //BigInteger eLA = eLacAgreement.CalculateAgreement(asymmetricCipherKeyPairA.Public);
            BigInteger a = acAgreement.CalculateAgreement(pub);
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

        public byte[] GetSharedSecretValueForReceiver(ECPublicKeyParameters pub)
        {
            ECDHCBasicAgreement eLacAgreement = new ECDHCBasicAgreement();
            eLacAgreement.Init(receiverPrivateKeyParameters);
            //ECDHCBasicAgreement acAgreement = new ECDHCBasicAgreement();
            //acAgreement.Init(asymmetricCipherKeyPairA.Private);
            BigInteger eLA = eLacAgreement.CalculateAgreement(pub);
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

        public byte[] Decrypt(byte[] cipherData, byte[] pubKey)
        {
            

            byte[] output = null;
            try
            {
                var xx = PublicKeyFactory.CreateKey(pubKey);

                var pub = ((ECPublicKeyParameters)xx);
                var derivedKey = GetSharedSecretValueForReceiver(pub);

                KeyParameter keyparam = ParameterUtilities.CreateKeyParameter("AES", derivedKey);
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/ISO7816_4PADDING");
                cipher.Init(false, keyparam);
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
