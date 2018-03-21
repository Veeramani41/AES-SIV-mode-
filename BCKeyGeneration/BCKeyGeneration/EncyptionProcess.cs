using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCKeyGeneration
{
    public partial class EncyptionProcess : Form
    {
        ECCEncryption eCCEncryption;
        ECCDecryption eCCDecryption;
        public EncyptionProcess()
        {
            InitializeComponent();
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

        private void GenerateReceiver_Click(object sender, EventArgs e)
        {
            eCCDecryption = new ECCDecryption(IsReceiverComp.Checked);

            txtReceiverPub.Text = Convert.ToBase64String(eCCDecryption.receiverPublicKey);
        }

        private void GenerateSender_Click(object sender, EventArgs e)
        {
            eCCEncryption = new ECCEncryption(IsSenderComp.Checked);

            txtSenderPublic.Text = Convert.ToBase64String(eCCEncryption.senderPublicKey);
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            var plainTextBytes = Convert.FromBase64String(txtPlainData.Text);// Encoding.GetEncoding("iso-8859-1").GetBytes(txtPlainData.Text);
            var encryptedValue = eCCEncryption.Encrypt(plainTextBytes, txtReceiverPublic.Text);
            txtSenderSecret.Text = BitConverter.ToString(eCCEncryption.sharedSecret).Replace("-", "");
            txtSenderDerived.Text = BitConverter.ToString(eCCEncryption.derivedKey).Replace("-", "");
            txtChiperData.Text = BitConverter.ToString(encryptedValue).Replace("-", "");
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            var encryptedBytes = GetTag(txtChiperData.Text);
            var decryptedValue = eCCDecryption.Decrypt(encryptedBytes, txtSenderPub.Text);
            //txtReceiverSecret.Text = BitConverter.ToString(eCCDecryption.sharedSecret).Replace("-", ""); //Encoding.GetEncoding("iso-8859-1").GetString(decryptedValue);
            //txtReceiverDerived.Text = BitConverter.ToString(eCCDecryption.derivedKey).Replace("-", "");//File.WriteAllBytes(@"C:\test\test.jp2", decryptedValue);
            //var df = File.ReadAllBytes(@"C:\test\test1.jp2");
            //var dfe = BitConverter.ToString(df).Replace("-", "");
            txtDecryptedData.Text = BitConverter.ToString(decryptedValue).Replace("-", "");//Encoding.GetEncoding("iso-8859-1").GetString(decryptedValue);;
        }
    }
}
