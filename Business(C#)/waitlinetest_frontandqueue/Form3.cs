using System;
using System.Drawing;
using System.Windows.Forms;

namespace waitlinetest_frontandqueue
{
    public partial class Form3 : Form
    {
        public Form1 form1 = null;
        public Form3()
        {
            InitializeComponent();
            setForm();
        }

        public void setForm()
        {
            this.Height = SystemInformation.WorkingArea.Height;
            this.Width = SystemInformation.WorkingArea.Width;
            this.Visible = true;
            this.BackgroundImage = new Bitmap(@"..\..\UI_resource\B_AD_background.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.label1.BackColor = Color.Transparent;
            this.label2.BackColor = Color.Transparent;
            this.label3.BackColor = Color.Transparent;
            this.label4.BackColor = Color.Transparent;
            this.label5.BackColor = Color.Transparent;
            this.label6.BackColor = Color.Transparent;
            this.label7.BackColor = Color.Transparent;
            InitVedio();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            this.player.BeginInit();
            player.settings.setMode("loop", true);
        }

        private void InitVedio()
        {
            player.CreateControl();
            //player.URL = @"D:\source\repos\waitlinetest_frontandqueue\waitlinetest_frontandqueue\UI_resource\AD.mp4";
            string path = Application.StartupPath;
            string url = path.Substring(0, path.Length - 9) + "UI_resource\\AD.mp4";
            player.URL = url;
            player.Enabled = true;
            player.Location = new Point(50, 150);
            player.Name = "player";
            player.Size = new Size(820, 700);
            player.TabIndex = 2;
            player.uiMode = "none";
            player.settings.volume = 0;
            Controls.Add(this.player);
        }


    }
}
