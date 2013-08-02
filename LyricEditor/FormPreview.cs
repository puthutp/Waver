using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LyricEditor
{
    public partial class FormPreview : Form
    {
        public FormPreview(List<string> input)
        {
            InitializeComponent();
            for (int i = 0; i < input.Count; i++)
                textBox1.AppendText(input[i] + "\n");
        }
    }
}
