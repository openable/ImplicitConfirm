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

namespace ImplicitConfirm
{
    public partial class Main : Form
    {
        public string path;
        public string file;
        public string[] fFullList;
        public string[] fList;
        public string fullPath;

        public Encoding encode;
        public StreamReader reader;

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openPanel = new OpenFileDialog();
            openPanel.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openPanel.Filter = "CSV Data (*csv.txt)|*.csv|All files (*.*)|*.*";
            openPanel.Multiselect = true;

            if (openPanel.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "0 / " + openPanel.FileNames.Length;
                file = openPanel.SafeFileName;
                path = openPanel.FileName.Substring(0, (openPanel.FileName.Length - openPanel.SafeFileName.Length));
                fList = openPanel.SafeFileNames;
                fFullList = openPanel.FileNames;
            }
            else
            {
                textBox1.Text = "None";
                textBox1.Tag = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Equals("None"))
            {
                MessageBox.Show("데이터 파일을 선택해 주세요.", "오류", MessageBoxButtons.OK);
                return;
            }

            encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");
        }
    }
}
