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
        public string[] pList;          //피험자 ID
        public double[] pScore;            //피험자 응시율 누적 점수

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

            pList = new string[openPanel.FileNames.Length];
            pScore = new double[openPanel.FileNames.Length];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Equals("None"))
            {
                MessageBox.Show("데이터 파일을 선택해 주세요.", "오류", MessageBoxButtons.OK);
                return;
            }

            encode = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

            // 파일 읽어오기
            for (int f = 0; f < fFullList.Length; f++)
            {
                reader = new StreamReader(fFullList[f], encode);

                string line = reader.ReadLine();
                int tNum = 2;       // 앞 번호를 기억하기 위한 임시 번호, 2번부터 시작해서 2로 할당
                int pNum = 0;       // 문항 번호
                int rTime = 0;      // 반응시간
                int gTime = 0;      // 개별 응시시간 누적 시간
                string[] w;
                pScore[f] = 0.0;

                while ((line = reader.ReadLine()) != null)
                {
                    w = line.Split('\t');
                    pList[f] = w[0];
                    pNum = Convert.ToInt32(w[1]);
                    if (tNum == pNum)
                    {
                        rTime = Convert.ToInt32(w[3]);
                        gTime = gTime + Convert.ToInt32(w[6]);
                        tNum = pNum;
                    }
                    else
                    {
                        pScore[f] = pScore[f] + ((double)gTime / (double)rTime);
                        gTime = 0;

                        rTime = Convert.ToInt32(w[3]);
                        gTime = gTime + Convert.ToInt32(w[6]);
                        tNum = pNum;
                    }
                }
                pScore[f] = pScore[f] + (gTime / rTime);

                reader.Close();
            }
        }
    }
}
