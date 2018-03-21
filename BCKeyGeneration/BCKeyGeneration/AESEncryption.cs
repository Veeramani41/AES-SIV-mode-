using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCKeyGeneration
{
    public partial class AESEncryption : Form
    {
        public AESEncryption()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var plainTextBytes = Encoding.GetEncoding("iso-8859-1").GetBytes(textBox2.Text);
            //var plainTextBytes = Convert.FromBase64String(textBox2.Text);
            
            var encryptedValue = AesSivCipherUtil.sivEncrypt(plainTextBytes, new List<byte[]>());
            var dfg = BitConverter.ToString(encryptedValue).Replace("-", "");
            textBox3.Text = Convert.ToBase64String(encryptedValue);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var encryptedBytes = Convert.FromBase64String(textBox3.Text);
            
            var decryptedValue = AesSivCipherUtil.sivDecrypt(encryptedBytes, new List<byte[]> { new byte[0]});
            var dfg = BitConverter.ToString(decryptedValue).Replace("-", "");
            textBox5.Text = Encoding.GetEncoding("iso-8859-1").GetString(decryptedValue);

        }
    }
}
