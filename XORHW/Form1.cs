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

namespace XORHW
{
    public partial class Form1 : Form
    {
        private byte password;
        OpenFileDialog openFile = new OpenFileDialog();
        public Form1()
        {
            InitializeComponent();
            selectFileButton.Click += FileSelectButtonClick;
            startButton.Click += StartButtonClick;
        }

        void FileSelectButtonClick(object sender, EventArgs e)
        {
            openFile.ShowDialog();
            nameFileTextBox.Text = openFile.FileName;
        }


        void StartButtonClick(object sender, EventArgs e)
        {
            if (encryptRadioButton.Checked && passwordTextBox.Text!=String.Empty)
            {
                Byte.TryParse(passwordTextBox.Text, out password);
                using (var encryptFile = File.Open(openFile.FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    while (encryptFile.Position < encryptFile.Length)
                    {
                        progressBar.Minimum = 0;
                        progressBar.Maximum = 10;
                        byte[] array = new byte[encryptFile.Length];
                        encryptFile.Read(array, 0, array.Length);
                        for (int i = 0; i < array.Length; i++)
                        {
                            array[i] ^= password;
                            encryptFile.Seek(0, SeekOrigin.Begin);
                            encryptFile.Write(array, 0, array.Length);
                            progressBar.Increment((int)encryptFile.Length);
                        }
                        MessageBox.Show("Файл зашифрован");
                        progressBar.Value = 0;
                    }
                }
            }

            if (decipherRadioButton.Checked)
            {
                byte secondPassword;
                Byte.TryParse(passwordTextBox.Text, out secondPassword);
                if (password == secondPassword)
                {
                    using (var decipherFile = File.Open(openFile.FileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        while (decipherFile.Position < decipherFile.Length)
                        {
                            progressBar.Minimum = 0;
                            progressBar.Maximum = 10;
                            byte[] array = new byte[decipherFile.Length];
                            decipherFile.Read(array, 0, array.Length);
                            for (int i = 0; i < array.Length; i++)
                            {
                                array[i] ^= password;
                                decipherFile.Seek(0, SeekOrigin.Begin);
                                decipherFile.Write(array, 0, array.Length);
                                progressBar.Increment((int) decipherFile.Length);
                            }

                            MessageBox.Show("Файл расшифрован");
                            progressBar.Value = 0;
                        }
                    }
                }
                else MessageBox.Show("Пароли не совпадают");
            }
            else if (passwordTextBox.Text==String.Empty)
                MessageBox.Show("Пароль не введен либо не выбрана функция!");
        }
    }
}
