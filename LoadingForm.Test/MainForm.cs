using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LoadingForm.Test
{
    public partial class MainForm : Form
    {
        Thread thread = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //开始做复杂操作
            LoadingFormHelper.ClearAllEvent();
            LoadingFormHelper.WaitCompleted += (s, ex) =>
            {
                MessageBox.Show("下载完成");
            };
            LoadingFormHelper.Show("正在下载...", 100, ProgressRunMode.Once);
            thread?.Abort();
            thread = new Thread(new ThreadStart(DownloadData));
            thread.Start();
        }

        private void DownloadData()
        {
            //此处模拟费时操作，并更新进度值
            for (int i = 0; i < 120; i++)
            {
                LoadingFormHelper.UpdateProgress(i);
                Thread.Sleep(100);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            thread?.Abort();           
            LoadingFormHelper.Close();
        }
    }
}
