using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Widget
{
    public partial class CSVLoader : Form
    {
        public CSVLoader()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openCSV.ShowDialog() == DialogResult.OK)
            {
                filePath.Text = Path.GetFileName(openCSV.FileName);

            }
        }
    }
}
