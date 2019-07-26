using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace LoadingForm
{
    public partial class WaitingForm : Form
    {
        /// <summary>
        /// 等待完成
        /// </summary>
        public event EventHandler WaitingCompleted;

        public WaitingForm()
        {
            InitializeComponent();
        }
        #region 内部变量
        private int _diameter = 100;//半径
        private int _bordWidth = 10;//边缘宽度
        #endregion

        private Timer t = new Timer();
        /// <summary>
        /// 当前进度值
        /// </summary>
        private int progress = 0;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
            }
        }

        private int _foreverValue = 0;//循环时的值

        /// <summary>
        /// 最大值
        /// </summary>
        private int maxValue=100;
        public int MaxValue
        {
            set
            {
                maxValue = value;
            }
        }
        /// <summary>
        /// 提示语
        /// </summary>
        private string tips = "请稍后...";
        public string Tips
        {
            set
            {
                tips = value;
            }
        }

        /// <summary>
        /// 进度条运行方式
        /// </summary>
        public ProgressRunMode Mode { get; set; } = ProgressRunMode.Once;

        /// <summary>
        /// 画进度
        /// </summary>
        /// <param name="x"></param>
        private void DrawProgress(int x)
        {
            if(Mode==ProgressRunMode.Once)
            {
                float angle = (x / (float)maxValue) * 360;//算出角度
                Bitmap map = new Bitmap(_diameter, _diameter);
                Graphics g = Graphics.FromImage(map);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawArc(new Pen(Color.Blue, _bordWidth), new Rectangle(_bordWidth/2, _bordWidth/2, (int)(_diameter - 1.5*_bordWidth), (int)(_diameter - 1.5 * _bordWidth)), -90f, angle);
                g.DrawArc(new Pen(Color.LightGray, _bordWidth), new Rectangle(_bordWidth / 2, _bordWidth / 2, (int)(_diameter - 1.5 * _bordWidth), (int)(_diameter - 1.5 * _bordWidth)), (-90 + angle), 360f - angle);
                Font stringFont = new Font(new FontFamily("宋体"), 10f);
                Brush sb = new SolidBrush(Color.Black);
                SizeF ss = g.MeasureString(tips, stringFont);
                if (ss.Width > _diameter)
                {
                    tips = tips.Substring(0, 4) + "...";
                }
                g.DrawString(tips, stringFont, sb, (_diameter - ss.Width) / 2, (_diameter - ss.Height) / 2);
                pictureBox1.Image = (Image)map;
                return;
            }
            if(Mode==ProgressRunMode.Forever)
            {
                Bitmap map = new Bitmap(_diameter, _diameter);
                Graphics g = Graphics.FromImage(map);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawArc(new Pen(Color.Blue, _bordWidth), new Rectangle(_bordWidth/2, _bordWidth / 2, (int)(_diameter - 1.5 * _bordWidth), (int)(_diameter - 1.5 * _bordWidth)), x, 45f);
                g.DrawArc(new Pen(Color.LightGray, _bordWidth), new Rectangle(_bordWidth / 2, _bordWidth / 2, (int)(_diameter - 1.5 * _bordWidth), (int)(_diameter - 1.5 * _bordWidth)), (x+45), 315f);
                Font stringFont = new Font(new FontFamily("宋体"), 10f);
                Brush sb = new SolidBrush(Color.Black);
                SizeF ss = g.MeasureString(tips, stringFont);
                if (ss.Width > _diameter)
                {
                    tips = tips.Substring(0, 4) + "...";
                }
                g.DrawString(tips, stringFont, sb, (_diameter - ss.Width) / 2, (_diameter - ss.Height) / 2);
                pictureBox1.Image = (Image)map;
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.Control;
            this.TransparencyKey = this.BackColor;
            this.ShowInTaskbar = false;
            //增加角度
            t.Interval = 50;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if(Mode==ProgressRunMode.Once)
            {
                DrawProgress(progress);
                if(progress==maxValue)
                {
                    t.Stop();
                    t.Dispose();
                    this.Close();
                    this.Dispose();
                    WaitingCompleted?.Invoke(this, null);
                }
                return;
            }

            if(Mode==ProgressRunMode.Forever)
            {
                _foreverValue += 20;
                DrawProgress(_foreverValue);
                if (_foreverValue >= 360)
                {
                    _foreverValue = 0;
                }
            }
        }
    }

    public enum ProgressRunMode
    {
        Forever,
        Once
    }
}
