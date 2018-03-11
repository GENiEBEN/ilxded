using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.AllowDrop = true;
            textBox1.DragEnter += new DragEventHandler(textBox1_DragEnter);
            textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)){
                e.Effect = DragDropEffects.All;
            }
            
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);


            string s = "";

            foreach (string File in FileList)
                s = s + " " + File;
            label2.Text = s;
            textBox1.Text = Decrypt(File.ReadAllText(s), "ILX:EXU A MO JUBA");
        }

        public static string Encrypt(string strToEncrypt, string strKey)
        {
            string base64String;
            try
            {
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                string str = strKey;
                byte[] numArray = mD5CryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(str));
                mD5CryptoServiceProvider = null;
                tripleDESCryptoServiceProvider.Key = numArray;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                byte[] bytes = Encoding.ASCII.GetBytes(strToEncrypt);
                base64String = Convert.ToBase64String(tripleDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bytes, 0, (int)bytes.Length));
            }
            catch (Exception exception)
            {
                base64String = string.Concat("Wrong Input. ", exception.Message);
            }
            return base64String;
        }

        public static string Decrypt(string strEncrypted, string strKey)
        {
            string str;
            try
            {
                TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                string str1 = strKey;
                byte[] numArray = mD5CryptoServiceProvider.ComputeHash(Encoding.ASCII.GetBytes(str1));
                mD5CryptoServiceProvider = null;
                tripleDESCryptoServiceProvider.Key = numArray;
                tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
                byte[] numArray1 = Convert.FromBase64String(strEncrypted);
                string str2 = Encoding.ASCII.GetString(tripleDESCryptoServiceProvider.CreateDecryptor().TransformFinalBlock(numArray1, 0, (int)numArray1.Length));
                tripleDESCryptoServiceProvider = null;
                str = str2;
            }
            catch (Exception exception)
            {
                str = string.Concat("Wrong Input. ", exception.Message);
            }
            return str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText(label2.Text, Encrypt(textBox1.Text, "ILX:EXU A MO JUBA"));
        }
    }
}
