using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace LyricEditor
{
    public partial class FormLibrary : Form
    {
        public FormLibrary()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int del = DeleteSong();

            if (del == -2)
            {
                MessageBox.Show("Artist or Title is empty", "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (del == -1)
            {
                MessageBox.Show("Song not found", "Delete failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (del == 0 || del == 1)
            {
                MessageBox.Show("Song deleted");
            }
        }


        private int DeleteSong()
        {
            string artist = textBoxArtist.Text.Trim();
            string title = textBoxTitle.Text.Trim();

            if (artist == "" || title == "")
            {
                return -2;
            }

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://" + Program.DBServer + "/index.php/editor/deleteSong2/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = Encoding.ASCII.GetBytes("artist=" + artist + "&title=" + title);

            request.ContentLength = byteArray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Decoder d;
            int charLen;
            String szData;

            int length = (int)response.ContentLength;

            byte[] bytes = new byte[length];
            char[] chars = new char[length];

            response.GetResponseStream().Read(bytes, 0, length);

            d = Encoding.UTF8.GetDecoder();
            charLen = d.GetChars(bytes, 0, length, chars, 0);
            szData = new String(chars);

            label1.Text = szData;

            int retval;
            try
            {
                retval = int.Parse(szData);
            }
            catch
            {
                retval = -1;
            }

            return retval;
        }
    }
}
